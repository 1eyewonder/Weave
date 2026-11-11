open System
open System.Diagnostics
open System.Net
open System.Net.Http
open System.Text.Json
open System.Text.RegularExpressions
open System.Threading.Tasks

type Package = {
  Id: string
  Version: string
}

let positiveInt name =
  match Int32.TryParse(Environment.GetEnvironmentVariable name) with
  | true, value when value > 0 -> value
  | _ -> failwith $"{name} must be a positive integer"

let parsePackage (spec: string) =
  let spec = spec.Trim()

  let matchResult =
    Regex.Match(spec, @"^([A-Za-z0-9][A-Za-z0-9_.-]*)@([0-9][0-9A-Za-z.+-]*)$")

  if not matchResult.Success then
    failwith $"Invalid NuGet package specification: {spec}"

  {
    Id = matchResult.Groups.[1].Value
    Version = matchResult.Groups.[2].Value
  }

let packages =
  Environment.GetEnvironmentVariable "PACKAGE_SPECS"
  |> Option.ofObj
  |> Option.defaultValue ""
  |> fun value -> value.Split([| '\r'; '\n' |], StringSplitOptions.RemoveEmptyEntries)
  |> Array.map parsePackage

if packages.Length = 0 then
  failwith "At least one NuGet package is required"

let timeout = TimeSpan.FromMinutes(float (positiveInt "WAIT_MINUTES"))
let pollInterval = TimeSpan.FromSeconds(float (positiveInt "POLL_SECONDS"))

let isAvailable (client: HttpClient) (package: Package) = task {
  let id = package.Id.ToLowerInvariant()
  let url = $"https://api.nuget.org/v3-flatcontainer/{id}/index.json"

  try
    use! response = client.GetAsync(url)

    match response.StatusCode with
    | HttpStatusCode.NotFound -> return false
    | HttpStatusCode.OK ->
      use! stream = response.Content.ReadAsStreamAsync()
      use! versions = JsonDocument.ParseAsync(stream)

      return
        versions.RootElement.GetProperty("versions").EnumerateArray()
        |> Seq.exists (fun version ->
          String.Equals(version.GetString(), package.Version, StringComparison.OrdinalIgnoreCase))
    | HttpStatusCode.TooManyRequests ->
      eprintfn "NuGet rate-limited %s; retrying" package.Id
      return false
    | status when int status >= 500 ->
      eprintfn "NuGet returned HTTP %d for %s; retrying" (int status) package.Id
      return false
    | status -> return failwith $"NuGet returned unexpected HTTP {int status} for {package.Id}"
  with
  | :? HttpRequestException as error ->
    eprintfn "NuGet request failed for %s: %s" package.Id error.Message
    return false
  | :? OperationCanceledException ->
    eprintfn "NuGet request timed out for %s; retrying" package.Id
    return false
  | :? JsonException as error ->
    eprintfn "NuGet returned invalid JSON for %s: %s" package.Id error.Message
    return false
}

let rec wait (client: HttpClient) (elapsed: Stopwatch) = task {
  let! availability = packages |> Array.map (isAvailable client) |> Task.WhenAll

  let missing =
    Array.zip packages availability
    |> Array.choose (fun (package, available) -> if available then None else Some package)

  if missing.Length = 0 then
    printfn "All requested NuGet package versions are available"
  else
    let names =
      missing
      |> Array.map (fun package -> $"{package.Id}@{package.Version}")
      |> String.concat ", "

    let remaining = timeout - elapsed.Elapsed

    if remaining <= TimeSpan.Zero then
      failwith $"Timed out waiting for NuGet packages: {names}"

    printfn "Waiting for NuGet packages: %s (%d seconds remaining)" names (int remaining.TotalSeconds)
    do! Task.Delay(min remaining pollInterval)
    return! wait client elapsed
}

let run () = task {
  use client = new HttpClient(Timeout = TimeSpan.FromSeconds 10.0)
  let elapsed = Stopwatch.StartNew()
  do! wait client elapsed
}

if fsi.CommandLineArgs |> Array.contains "--check" then
  printfn "NuGet wait script compiled and inputs validated"
else
  run().GetAwaiter().GetResult()
