#load "../src/Weave.Docs/PackageVersions.fs"

open System
open System.IO
open Weave.Docs

let verifyVersion packageName expected actual =
  if actual <> expected then
    failwith $"{packageName} release tag {actual} does not match documented version {expected}"

match fsi.CommandLineArgs |> Array.skip 1 with
| [||] ->
  let output = Environment.GetEnvironmentVariable "GITHUB_OUTPUT"

  if String.IsNullOrWhiteSpace output then
    failwith "GITHUB_OUTPUT must be set by GitHub Actions"

  File.AppendAllLines(
    output,
    [
      $"library={PackageVersions.Library}"
      $"templates={PackageVersions.Templates}"
    ]
  )
| [| "--verify-library"; tag |] -> verifyVersion "Library" $"v{PackageVersions.Library}" tag
| [| "--verify-templates"; tag |] -> verifyVersion "Templates" $"templates-v{PackageVersions.Templates}" tag
| _ -> failwith "Expected --verify-library TAG, --verify-templates TAG, or no arguments"
