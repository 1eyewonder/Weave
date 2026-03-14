namespace Weave.Tests.E2E

open System
open System.Diagnostics
open System.IO
open System.Net.Http
open System.Net.Sockets
open System.Threading
open Xunit

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

type TestServerFixture() =
  let port = ServerHelpers.findFreePort ()
  let baseUrl = $"http://localhost:%d{port}"
  let mutable proc: Process option = None

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

  interface IDisposable with
    member _.Dispose() =
      proc
      |> Option.iter (fun p ->
        if not p.HasExited then
          p.Kill(entireProcessTree = true)

        p.Dispose())

[<CollectionDefinition("E2E")>]
type E2ECollection() =
  interface ICollectionFixture<TestServerFixture>
