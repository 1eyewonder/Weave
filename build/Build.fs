open Fake.Api
open Fake.Core
open Fake.IO
open Fake.DotNet
open Fake.JavaScript
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators
open Fake.IO.Globbing.Operators
open Fake.Tools
open System.IO
open Fake.BuildServer
open Fake.Core

let environVarAsBoolOrDefault varName defaultValue =
  let truthyConsts = [ "1"; "Y"; "YES"; "T"; "TRUE" ]

  try
    let envvar = (Environment.environVar varName).ToUpper()

    truthyConsts |> List.exists ((=) envvar)
  with _ ->
    defaultValue

let isCI = lazy (environVarAsBoolOrDefault "CI" false)

let project = "Weave"
let summary = "A component library for front ends using F# and WebSharper"
let configuration = Environment.environVarOrDefault "BUILD_CONFIGURATION" "Debug"
let solutionFile = "Weave.sln"

let rootDir = __SOURCE_DIRECTORY__ </> ".."

let srcGlob = rootDir </> "src/**/*.??proj"

let testsGlob = rootDir </> "tests/**/*.??proj"

let srcAndTest = !!srcGlob ++ testsGlob

let srcCodeGlob =
  !!(rootDir </> "src/**/*.fs") ++ (rootDir </> "src/**/*.fsx")
  -- (rootDir </> "src/**/obj/**/*.fs")

let testsCodeGlob =
  !!(rootDir </> "tests/**/*.fs") ++ (rootDir </> "tests/**/*.fsx")
  -- (rootDir </> "tests/**/obj/**/*.fs")
  -- (rootDir </> "tests/Weave.Tests.E2E.Site/**")

let gitOwner = "1eyewonder"

let distDir = rootDir @@ "bin"

let distGlob = distDir @@ "*.nupkg"

let githubToken = Environment.environVarOrNone "GITHUB_TOKEN"

let nugetToken = Environment.environVarOrNone "NUGET_TOKEN"

let failOnBadExitAndPrint (p: ProcessResult) =
  if p.ExitCode <> 0 then
    p.Errors |> Seq.iter Trace.traceError

    failwithf "failed with exitcode %d" p.ExitCode

module dotnet =
  let watch cmdParam program args =
    DotNet.exec cmdParam (sprintf "watch %s" program) args

  let run cmdParam args = DotNet.exec cmdParam "run" args

  let tool optionConfig command args =
    DotNet.exec optionConfig (sprintf "%s" command) args |> failOnBadExitAndPrint

  let fantomas args = DotNet.exec id "fantomas" args

  let analyzers args = DotNet.exec id "fsharp-analyzers" args

let formatCode _ =
  let result = dotnet.fantomas "."

  if not result.OK then
    Trace.traceErrorfn "Errors while formatting all files: %A" result.Messages

let checkFormatCode _ =
  let result = dotnet.fantomas "--check ."

  if result.ExitCode = 0 then
    Trace.log "No files need formatting"
  elif result.ExitCode = 99 then
    failwith "Some files need formatting, check output for more info"
  else
    Trace.logf "Errors while formatting: %A" result.Errors

let analyze _ =
  let analyzerPaths = !!"packages/analyzers/**/analyzers/dotnet/fs"

  let createArgsForProject (project: string) analyzerPaths =
    let projectName = Path.GetFileNameWithoutExtension project

    [
      yield "--project"
      yield project
      yield "--analyzers-path"
      yield! analyzerPaths
      if isCI.Value then
        yield "--report"
        yield $"analysisreports/%s{projectName}-analysis.sarif"
    ]
    |> String.concat " "

  seq {
    yield! !!"src/**/*.fsproj"
    yield! !!"build/**/*.fsproj"
  }
  |> Seq.iter (fun fsproj ->
    let result = createArgsForProject fsproj analyzerPaths |> dotnet.analyzers

    result.Errors |> Seq.iter Trace.traceError)

let clean _ =
  !!"bin" ++ "src/**/bin" ++ "tests/**/bin" ++ "src/**/obj" ++ "tests/**/obj"
  |> Shell.cleanDirs

  [ "paket-files/paket.restore.cached" ] |> Seq.iter Shell.rm

let build _ =
  let setParams (defaults: DotNet.BuildOptions) = {
    defaults with
        NoRestore = true
        Configuration = DotNet.BuildConfiguration.fromString configuration
  }

  DotNet.build setParams solutionFile

let restore _ =
  Fake.DotNet.Paket.restore (fun p -> { p with ToolType = ToolType.CreateLocalTool() })
  DotNet.restore id solutionFile

let yarnInstall _ = Yarn.install id

let hardDependency x y = x ==> y |> ignore
let (==>!) = hardDependency

let softDependency x y = x ?=> y |> ignore
let (?=>!) = softDependency

let runTests _ =
  let runsettings = rootDir </> "tests" </> "playwright.runsettings"

  let setParams (defaults: DotNet.TestOptions) = {
    defaults with
        NoBuild = true
        NoRestore = true
        Configuration = DotNet.BuildConfiguration.fromString configuration
        Settings = Some runsettings
  }

  !!testsGlob |> Seq.iter (DotNet.test setParams)

let buildDocs _ =
  Yarn.exec "build:css" id

  let setParams (defaults: DotNet.BuildOptions) = {
    defaults with
        NoRestore = true
        Configuration = DotNet.BuildConfiguration.Release
  }

  DotNet.build setParams (rootDir </> "src" </> "Weave.Docs" </> "Weave.Docs.fsproj")

let initTargets () =

  BuildServer.install [ GitHubActions.Installer ]

  Option.iter (TraceSecrets.register "<GITHUB_TOKEN>") githubToken
  Option.iter (TraceSecrets.register "<NUGET_TOKEN>") nugetToken

  Target.create "Clean" clean
  Target.create "Build" build
  Target.create "Restore" restore
  Target.create "YarnInstall" yarnInstall
  Target.create "CheckFormat" checkFormatCode
  Target.create "Analyze" analyze
  Target.create "RunTests" runTests
  Target.create "BuildDocs" buildDocs
  Target.create "Init" ignore

  "Clean" ?=>! "Restore"
  "Restore" ?=>! "YarnInstall"

  "Restore" ==>! "Build"
  "YarnInstall" ==>! "Build"
  "Build" ==>! "RunTests"

  "RunTests" ?=>! "BuildDocs"
  "Restore" ==>! "BuildDocs"
  "YarnInstall" ==>! "BuildDocs"

  "Restore" ==>! "Analyze"
  "Restore" ==>! "CheckFormat"

  // local dev convenience
  "Clean" ?=>! "Init"
  "Restore" ==>! "Init"
  "YarnInstall" ==>! "Init"
  "Build" ==>! "Init"

[<EntryPoint>]
let main argv =
  argv
  |> Array.toList
  |> Context.FakeExecutionContext.Create false "build.fsx"
  |> Context.RuntimeContext.Fake
  |> Context.setExecutionContext

  initTargets () |> ignore

  Target.runOrDefaultWithArguments "Init"

  0 // return an integer exit code
