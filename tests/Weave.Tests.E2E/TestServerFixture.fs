namespace Weave.Tests.E2E

open System
open System.Diagnostics
open System.IO
open System.Net.Http
open System.Net.Sockets
open System.Threading
open System.Threading.Tasks
open Xunit
open Microsoft.Playwright

module private ServerHelpers =

  let findFreePort () =
    let listener = new TcpListener(Net.IPAddress.Loopback, 0)
    listener.Start()
    let port = (listener.LocalEndpoint :?> Net.IPEndPoint).Port
    listener.Stop()
    port

  let buildConfiguration =
    match Environment.GetEnvironmentVariable "BUILD_CONFIGURATION" with
    | cfg when String.IsNullOrEmpty cfg -> "Debug"
    | cfg -> cfg

/// Shared fixture for the E2E test collection.
/// Hosts the test server process and a single Chromium browser instance.
/// Each test creates a fresh BrowserContext + Page for isolation.
type TestFixture() =
  let port = ServerHelpers.findFreePort ()
  let baseUrl = $"http://localhost:%d{port}"
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
        $"run --no-build --configuration %s{configuration} --project \"%s{siteProject}\" --urls %s{baseUrl}",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
      )

    psi.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] <- "Development"

    let p = Process.Start(psi)
    proc <- Some p

    // Drain stdout/stderr to prevent pipe buffer deadlock.
    // Without this, the 64 KB OS pipe buffer fills after ~190 requests,
    // causing ASP.NET Core's console logger to block on write and
    // deadlocking Kestrel's request pipeline.
    p.StandardOutput.ReadToEndAsync() |> ignore
    p.StandardError.ReadToEndAsync() |> ignore

    // Wait for the server to be ready
    use client = new HttpClient()
    let mutable ready = false
    let stopwatch = Stopwatch.StartNew()
    let timeout = TimeSpan.FromSeconds(60.0)

    while not ready && stopwatch.Elapsed < timeout do
      try
        let response = client.GetAsync(baseUrl).Result

        if int response.StatusCode < 500 then
          ready <- true
      with _ ->
        Thread.Sleep(500)

    if not ready then
      failwith $"Test server failed to start within %d{int timeout.TotalSeconds}s on %s{baseUrl}"

  member _.BaseUrl = baseUrl

  member _.NewPageAsync() = task {
    let! context = browser.NewContextAsync()
    let! page = context.NewPageAsync()
    return context, page
  }

  interface IAsyncLifetime with
    member _.InitializeAsync() =
      task {
        let! pw = Playwright.CreateAsync()
        playwright <- pw
        let! b = pw.Chromium.LaunchAsync(BrowserTypeLaunchOptions(Headless = true))
        browser <- b
      }
      :> Task

    member _.DisposeAsync() =
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

[<CollectionDefinition("E2E")>]
type E2ECollection() =
  interface ICollectionFixture<TestFixture>
