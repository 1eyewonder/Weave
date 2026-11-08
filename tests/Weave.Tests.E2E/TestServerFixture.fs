namespace Weave.Tests.E2E

open System
open System.Diagnostics
open System.IO
open System.Threading.Tasks
open Xunit
open Microsoft.Playwright

module private ServerHelpers =

  let buildConfiguration =
    match Environment.GetEnvironmentVariable "BUILD_CONFIGURATION" with
    | cfg when String.IsNullOrEmpty cfg -> "Debug"
    | cfg -> cfg

/// Shared fixture for the E2E test assembly.
/// Hosts the test server process and a single browser instance (Chromium by default).
/// Each test creates a fresh BrowserContext + Page for isolation.
type TestFixture() =
  let mutable baseUrl = ""
  let mutable proc: Process option = None
  let mutable playwright: IPlaywright = null
  let mutable browser: IBrowser = null

  do
    let rootDir =
      let assemblyDir =
        Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().Location)

      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "..", ".."))

    let siteProject =
      Path.Combine(rootDir, "tests", "Weave.Tests.E2E.Site", "Weave.Tests.E2E.Site.fsproj")

    let configuration = ServerHelpers.buildConfiguration

    let psi =
      ProcessStartInfo(
        "dotnet",
        $"run --no-build --configuration %s{configuration} --project \"%s{siteProject}\" --urls http://127.0.0.1:0",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
      )

    psi.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] <- "Development"

    // Kestrel binds an ephemeral port itself, avoiding a reserve-and-release race.
    let listening =
      TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously)

    let p = new Process(StartInfo = psi, EnableRaisingEvents = true)

    p.OutputDataReceived.Add(fun args ->
      if not (isNull args.Data) then
        let marker = "Now listening on: "
        let index = args.Data.IndexOf(marker, StringComparison.Ordinal)

        if index >= 0 then
          listening.TrySetResult(args.Data.Substring(index + marker.Length).Trim())
          |> ignore)

    p.Exited.Add(fun _ ->
      listening.TrySetException(Exception("Test server exited before listening"))
      |> ignore)

    if not (p.Start()) then
      failwith "Could not start test server"

    proc <- Some p
    p.BeginOutputReadLine()
    p.StandardError.ReadToEndAsync() |> ignore

    try
      baseUrl <- listening.Task.WaitAsync(TimeSpan.FromSeconds(60.0)).GetAwaiter().GetResult()
    with _ ->
      if not p.HasExited then
        p.Kill(entireProcessTree = true)

      p.Dispose()
      reraise ()

  member _.BaseUrl = baseUrl

  member _.NewPageAsync() = task {
    let! context = browser.NewContextAsync()
    let! page = context.NewPageAsync()
    return context, page
  }

  member _.NewTouchContextAsync() =
    browser.NewContextAsync(
      BrowserNewContextOptions(
        HasTouch = true,
        IsMobile = true,
        ViewportSize = ViewportSize(Width = 390, Height = 844)
      )
    )

  interface IAsyncLifetime with
    member _.InitializeAsync() =
      ValueTask(
        task {
          let! pw = Playwright.CreateAsync()
          playwright <- pw

          let browserType =
            match Environment.GetEnvironmentVariable("WEAVE_E2E_BROWSER") with
            | null
            | ""
            | "chromium" -> pw.Chromium
            | "firefox" -> pw.Firefox
            | "webkit" -> pw.Webkit
            | value -> invalidArg "WEAVE_E2E_BROWSER" $"Unknown browser: %s{value}"

          let! b = browserType.LaunchAsync(BrowserTypeLaunchOptions(Headless = true))
          browser <- b
        }
        :> Task
      )

    member _.DisposeAsync() =
      ValueTask(
        task {
          if not (isNull browser) then
            do! browser.CloseAsync()

          if not (isNull playwright) then
            playwright.Dispose()

          proc
          |> Option.iter (fun p ->
            if not p.HasExited then
              p.Kill(entireProcessTree = true)

            p.Dispose())
        }
        :> Task

      )
