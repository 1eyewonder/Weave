#r "FSharp.Compiler.Service.dll"
#load "../../src/Weave.Docs/examples/grid/GridSource.fs"
#load "../../src/Weave.Docs/examples/button/ButtonSource.fs"
#load "../../src/Weave.Docs/examples/shared/SourceString.fs"
#load "../../src/Weave.Docs/examples/text-field/TextFieldAppearance.fs"
#load "../../src/Weave.Docs/examples/tabs/TabsAppearanceSource.fs"
#load "../../src/Weave.Docs/examples/progress/ProgressAppearance.fs"
#load "../../src/Weave.Docs/examples/alert/AlertAppearance.fs"
#load "../../src/Weave.Docs/examples/shared/GroupBAppearance.fs"
#load "../../src/Weave.Docs/examples/shared/ToggleAppearance.fs"
#load "../../src/Weave.Docs/examples/radio-button/RadioAppearance.fs"
#load "../../src/Weave.Docs/examples/numeric-field/NumericFieldAppearance.fs"
#load "../../src/Weave.Docs/examples/expansion-panel/ExpansionAppearance.fs"
#load "../../src/Weave.Docs/examples/button-menu/ButtonMenuAppearance.fs"
#load "../../src/Weave.Docs/examples/chip/ChipAppearance.fs"
#load "../../src/Weave.Docs/examples/chip-set/ChipSetAppearance.fs"
#load "../../src/Weave.Docs/examples/slider/SliderAppearance.fs"
#load "../../src/Weave.Docs/examples/pagination/PaginationAppearance.fs"
#load "../../src/Weave.Docs/examples/app-bar/AppBarAppearance.fs"
#load "../../src/Weave.Docs/examples/dialog/DialogAppearance.fs"
#load "../../src/Weave.Docs/examples/drawer/DrawerAppearance.fs"
#load "../../src/Weave.Docs/examples/carousel/CarouselAppearance.fs"
#load "../../src/Weave.Docs/examples/select/SelectAppearance.fs"
#load "../../src/Weave.Docs/examples/dropdown-menu/DropdownAppearance.fs"
#load "../../src/Weave.Docs/examples/container/ContainerAppearance.fs"
#load "../../src/Weave.Docs/examples/field/FieldAppearance.fs"
#load "../../src/Weave.Docs/examples/snackbar/SnackbarAppearance.fs"
#load "../../src/Weave.Docs/examples/scroll-to-top/ScrollTopAppearance.fs"
#load "../../src/Weave.Docs/examples/spacer/SpacerAppearance.fs"
#load "../../src/Weave.Docs/examples/color-picker/ColorPickerAppearance.fs"
#load "../../src/Weave.Docs/examples/shared/ReferenceAppearanceSource.fs"
#load "../../src/Weave.Docs/examples/typography/TypographyAppearanceSource.fs"
#load "../../src/Weave.Docs/examples/shared/DataAppearanceSource.fs"
#load "../../src/Weave.Docs/examples/list/ListAppearanceSource.fs"
#load "../../src/Weave.Docs/examples/icons/IconPlaygroundSource.fs"

open System
open System.IO
open System.Text.RegularExpressions
open FSharp.Compiler.CodeAnalysis
open FSharp.Compiler.Diagnostics
open FSharp.Compiler.Text
open FSharp.Compiler.Syntax
open Microsoft.FSharp.Reflection

// FCS ships with the SDK used by `dotnet fsi`; no extra package is needed.
let root = Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, "..", ".."))

let imports =
  """open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
"""

type Snippet = {
  File: string
  Line: int
  Column: int
  Number: int
  Code: string
  Language: string
  Title: string
  Context: string
}

type Failure = {
  File: string
  Line: int
  Snippet: int
  Message: string
}

type CheckOptions = {
  Configuration: string
  Filters: string list
  SelfTest: bool
  CompositionAudit: bool
}

let parseArguments arguments =
  let rec parse options remaining =
    match remaining with
    | [] -> { options with Filters = List.rev options.Filters }
    | "--configuration" :: value :: rest ->
      let configuration =
        match value.ToLowerInvariant() with
        | "debug" -> "Debug"
        | "release" -> "Release"
        | _ -> failwith $"Unknown configuration '{value}'. Use Debug or Release."

      parse { options with Configuration = configuration } rest
    | [ "--configuration" ] -> failwith "--configuration requires Debug or Release."
    | "--self-test" :: rest -> parse { options with SelfTest = true } rest
    | "--composition-audit" :: rest -> parse { options with CompositionAudit = true } rest
    | argument :: _ when argument.StartsWith("--", StringComparison.Ordinal) ->
      failwith
        $"Unknown option '{argument}'. Use --configuration Debug|Release, --self-test, or --composition-audit."
    | filter :: rest -> parse { options with Filters = filter :: options.Filters } rest

  parse
    {
      Configuration = "Debug"
      Filters = []
      SelfTest = false
      CompositionAudit = false
    }
    arguments

let commandLine =
  fsi.CommandLineArgs |> Array.skip 1 |> Array.toList |> parseArguments

let output =
  Path.Combine(root, "src", "Weave.Docs", "bin", commandLine.Configuration, "net10.0")

let selected (name: string) =
  commandLine.Filters.IsEmpty
  || commandLine.Filters
     |> List.exists (fun filter -> name.Contains(filter, StringComparison.OrdinalIgnoreCase))

let examplesRoot = Path.Combine(root, "src", "Weave.Docs", "examples")

let exampleSources =
  Directory.GetFiles(examplesRoot, "*.fs", SearchOption.AllDirectories)

let isDisplayedExample (path: string) =
  let name = Path.GetFileName path

  name.EndsWith("Examples.fs", StringComparison.Ordinal)
  || name.EndsWith("Configurator.fs", StringComparison.Ordinal)
  || name.StartsWith("Showcase", StringComparison.Ordinal)
  || List.contains name [
    "DataAppearance.fs"
    "ReferenceAppearance.fs"
    "ListAppearance.fs"
    "TabsAppearance.fs"
    "TypographyAppearance.fs"
  ]

let files = [
  yield! exampleSources |> Array.filter isDisplayedExample |> Array.sort
  yield Path.Combine(root, "src", "Weave.Docs", "DocsHome.fs")
]

let exampleSourceByName =
  exampleSources
  |> Array.groupBy Path.GetFileName
  |> Array.map (fun (name, paths) ->
    if paths.Length <> 1 then
      failwith $"Ambiguous example source: {name}"

    name, paths.[0])
  |> Map.ofArray

let referenceNames = [
  "Weave.dll"
  "WebSharper.Core.dll"
  "WebSharper.Core.JavaScript.dll"
  "WebSharper.JavaScript.dll"
  "WebSharper.StdLib.dll"
  "WebSharper.UI.dll"
  "WebSharper.UI.Templating.Common.dll"
  "WebSharper.Web.dll"
  "WebSharper.Sitelets.dll"
  "FsToolkit.ErrorHandling.dll"
]

let references = referenceNames |> List.map (fun name -> Path.Combine(output, name))

for reference in references do
  if not (File.Exists reference) then
    failwith
      $"Missing {reference}. Run dotnet build src/Weave.Docs/Weave.Docs.fsproj -c {commandLine.Configuration} first."

let checker = FSharpChecker.Create()

let flags = [|
  "--targetprofile:netcore"
  yield! references |> List.map (fun path -> "-r:" + path)
|]

let templatePath = Path.Combine(Path.GetTempPath(), "WeaveDocsSnippet.fsx")

let options, optionDiagnostics =
  checker.GetProjectOptionsFromScript(
    templatePath,
    SourceText.ofString "",
    otherFlags = flags,
    useSdkRefs = true
  )
  |> Async.RunSynchronously

for diagnostic in optionDiagnostics do
  if diagnostic.Severity = FSharpDiagnosticSeverity.Error then
    failwith diagnostic.Message

// Read the language from actual helper applications, including strings bound
// to a local `code` value. Syntax-tree ranges distinguish source from comments.
let syntaxFields (value: obj) =
  let kind = value.GetType()

  if kind = typeof<string> then
    [||]
  elif FSharpType.IsUnion kind then
    FSharpValue.GetUnionFields(value, kind) |> snd
  elif FSharpType.IsRecord kind then
    FSharpValue.GetRecordFields value
  elif kind.IsArray then
    (value :?> System.Collections.IEnumerable) |> Seq.cast<obj> |> Seq.toArray
  else
    [||]

let collectSyntax (tree: ParsedInput) =
  let expressions = ResizeArray<SynExpr>()
  let bindings = ResizeArray<SynBinding * range option>()
  let parameters = ResizeArray<string * range>()

  let rec parameterNames (value: obj) = [
    if not (isNull value) then
      match value with
      | :? SynPat as pattern ->
        match pattern with
        | SynPat.Named(SynIdent(identifier, _), _, _, _) -> yield identifier.idText
        | _ -> ()
      | :? SynSimplePat as pattern ->
        match pattern with
        | SynSimplePat.Id(identifier, _, _, _, _, _) -> yield identifier.idText
        | _ -> ()
      | _ -> ()

      for field in syntaxFields value do
        yield! parameterNames field
  ]

  let rec visit (scope: range option) (value: obj) =
    if not (isNull value) then
      let nextScope =
        match value with
        | :? SynExpr as expression ->
          expressions.Add expression

          match expression with
          | SynExpr.LetOrUse _ -> Some expression.Range
          | SynExpr.Lambda(_, _, arguments, body, _, _, _) ->
            for name in parameterNames arguments do
              parameters.Add(name, body.Range)

            scope
          | _ -> scope
        | :? SynBinding as binding ->
          bindings.Add(binding, scope)
          let (SynBinding(_, _, _, _, _, _, _, pattern, _, body, _, _, _)) = binding

          match pattern with
          | SynPat.LongIdent(_, _, _, arguments, _, _) ->
            for name in parameterNames arguments do
              parameters.Add(name, body.Range)
          | _ -> ()

          scope
        | :? SynModuleDecl as declaration ->
          match declaration with
          | SynModuleDecl.NestedModule _ -> Some declaration.Range
          | _ -> scope
        | _ -> scope

      syntaxFields value |> Array.iter (visit nextScope)

  visit None tree
  expressions, bindings, parameters

let containsRange (outer: range) (inner: range) =
  (outer.StartLine, outer.StartColumn) <= (inner.StartLine, inner.StartColumn)
  && (outer.EndLine, outer.EndColumn) >= (inner.EndLine, inner.EndColumn)

let rec stringValue expression =
  match expression with
  | SynExpr.Const(SynConst.String(value, _, _), range) -> Some(value, range)
  | SynExpr.Paren(inner, _, _, _) -> stringValue inner
  | _ -> None

let bindingValue binding =
  let (SynBinding(_, _, _, _, _, _, _, pattern, _, expression, _, _, _)) = binding

  match pattern with
  | SynPat.Named(SynIdent(identifier, _), _, _, _) -> Some(identifier.idText, expression, expression.Range)
  | _ -> None

let rec application expression arguments =
  match expression with
  | SynExpr.App(_, _, functionExpression, argument, _) ->
    application functionExpression (argument :: arguments)
  | SynExpr.Paren(inner, _, _, _) -> application inner arguments
  | SynExpr.Ident identifier -> Some(identifier.idText, arguments)
  | SynExpr.LongIdent(_, identifier, _, _) ->
    Some(identifier.LongIdent |> List.map _.idText |> String.concat ".", arguments)
  | _ -> None

let inventorySource file (source: string) =

  let parsed =
    checker.ParseFile(
      file,
      SourceText.ofString source,
      { FSharpParsingOptions.Default with SourceFiles = [| file |] }
    )
    |> Async.RunSynchronously

  for diagnostic in parsed.Diagnostics do
    if diagnostic.Severity = FSharpDiagnosticSeverity.Error then
      failwith $"Cannot inventory invalid F# source {file}:{diagnostic.StartLine}: {diagnostic.Message}"

  let expressions, bindings, parameters = collectSyntax parsed.ParseTree

  let strings =
    bindings
    |> Seq.choose (fun (binding, scope) ->
      bindingValue binding
      |> Option.map (fun (name, expression, range) -> name, expression, range, scope))
    |> Seq.toArray

  let findBinding (callRange: range) name =
    let candidate =
      strings
      |> Array.filter (fun (candidate, _, range, scope) ->
        candidate = name
        && (range.EndLine, range.EndColumn) < (callRange.StartLine, callRange.StartColumn)
        && (scope |> Option.forall (fun scope -> containsRange scope callRange)))
      |> Array.sortByDescending (fun (_, _, range, _) -> range.StartLine, range.StartColumn)
      |> Array.tryHead

    candidate
    |> Option.filter (fun (_, _, boundRange, _) ->
      parameters
      |> Seq.exists (fun (parameter, scope) ->
        parameter = name
        && containsRange scope callRange
        && (scope.StartLine, scope.StartColumn) > (boundRange.StartLine, boundRange.StartColumn))
      |> not)

  let rec listExpressions expression =
    match expression with
    | SynExpr.Sequential(_, _, left, right, _, _) -> listExpressions left @ listExpressions right
    | _ -> [ expression ]

  let rec resolve (callRange: range) expression =
    match stringValue expression with
    | Some value -> Some value
    | None ->
      match expression with
      | SynExpr.Ident identifier ->
        findBinding callRange identifier.idText
        |> Option.bind (fun (_, bound, _, _) -> resolve bound.Range bound)
      | SynExpr.Paren(inner, _, _, _) -> resolve callRange inner
      | _ ->
        match application expression [] with
        | Some("String.concat", [ separator; parts ]) ->
          match resolve expression.Range separator, resolveList expression.Range parts with
          | Some(separator, _), Some parts ->
            Some(parts |> List.map fst |> String.concat separator, expression.Range)
          | _ -> None
        | Some("op_Addition", [ left; right ]) ->
          match resolve expression.Range left, resolve expression.Range right with
          | Some(left, _), Some(right, _) -> Some(left + right, expression.Range)
          | _ -> None
        | _ -> None

  and resolveList (callRange: range) expression =
    match expression with
    | SynExpr.Ident identifier ->
      findBinding callRange identifier.idText
      |> Option.bind (fun (_, bound, _, _) -> resolveList bound.Range bound)
    | SynExpr.ArrayOrList(_, parts, _) -> resolveParts parts
    | SynExpr.ArrayOrListComputed(_, parts, _) -> resolveParts (listExpressions parts)
    | SynExpr.Paren(inner, _, _, _) -> resolveList callRange inner
    | _ -> None

  and resolveParts expressions =
    let values = expressions |> List.map (fun part -> resolve part.Range part)

    if values |> List.exists Option.isNone then
      None
    else
      Some(values |> List.choose id)

  let declared = System.Collections.Generic.Dictionary<int * int, Snippet>()
  let displayed = System.Collections.Generic.HashSet<int * int>()

  for expression in expressions do
    match application expression [] with
    | Some(name, arguments) ->
      let signature =
        match name, arguments.Length with
        | "Helpers.codeSection", 4 -> Some(0, 1)
        | "Helpers.codeSampleSectionFor", 5 -> Some(0, 1)
        | "codeBlock", 2 -> Some(0, -1)
        | "Helpers.codeSampleSection", 4 -> Some(-1, 0)
        | _ -> None

      match signature with
      | Some(languageIndex, titleIndex) ->
        let language =
          if languageIndex = -1 then
            "fsharp"
          else
            resolve expression.Range arguments[languageIndex]
            |> Option.map fst
            |> Option.defaultWith (fun () ->
              failwith $"Cannot resolve source language in {file}:{expression.Range.StartLine}.")

        let title =
          if titleIndex = -1 then
            "codeBlock"
          else
            resolve expression.Range arguments[titleIndex]
            |> Option.map fst
            |> Option.defaultValue name

        // Dynamic installation commands are shell source. Inventory the literal
        // format while keeping F# snippets strict about resolving their source.
        let sourceExpression = List.last arguments

        let resolved =
          match resolve expression.Range sourceExpression with
          | Some value -> Some value
          | None when language <> "fsharp" ->
            match application sourceExpression [] with
            | Some("sprintf", format :: _) -> resolve expression.Range format
            | _ -> None
          | None -> None

        match resolved with
        | Some(code, range) ->
          displayed.Add((range.StartLine, range.StartColumn)) |> ignore

          declared[(range.StartLine, range.StartColumn)] <- {
            File = file
            Line = range.StartLine
            Column = range.StartColumn
            Number = 0
            Code = code
            Language = language
            Title = title
            Context = ""
          }
        | None -> failwith $"Cannot resolve displayed source in {file}:{expression.Range.StartLine}."
      | None -> ()
    | None -> ()

  // Triple-quoted source outside the helpers (for example DocsHome) remains
  // covered and defaults to F#. New source never becomes a silent exclusion.
  for block in Regex.Matches(source, "\"\"\"([\\s\\S]*?)\"\"\"") do
    let prefix = source[.. block.Index - 1]
    let line = prefix |> Seq.filter ((=) '\n') |> Seq.length |> (+) 1
    let lastNewline = prefix.LastIndexOf '\n'
    let column = block.Index - lastNewline - 1

    if not (declared.ContainsKey((line, column))) then
      declared[(line, column)] <- {
        File = file
        Line = line
        Column = column
        Number = 0
        Code = block.Groups[1].Value
        Language = "fsharp"
        Title = "source block"
        Context = ""
      }

  // A sourceParts binding is an explicit cumulative sequence of the displayed
  // blocks. Each block sees only its displayed predecessors, never live helpers.
  for name, expression, _, _ in strings do
    if name = "sourceParts" then
      let parts =
        resolveList expression.Range expression
        |> Option.defaultWith (fun () -> failwith $"Cannot resolve sourceParts in {file}.")

      let mutable context = ""

      for code, range in parts do
        let key = range.StartLine, range.StartColumn

        if not (displayed.Contains key) then
          failwith $"A sourceParts entry is not displayed in {file}:{range.StartLine}."

        declared[key] <- { declared[key] with Context = context }
        context <- context + code + "\n\n"

  declared.Values
  |> Seq.sortBy (fun snippet -> snippet.Line, snippet.Column)
  |> Seq.mapi (fun index snippet -> { snippet with Number = index + 1 })
  |> Seq.toList

let readSnippets file =
  inventorySource file (File.ReadAllText file)

let runInventorySelfTests () =
  let file = Path.Combine(root, "SnippetInventoryFixture.fs")
  let mutable count = 0

  let accepts name source verify =
    let snippets = inventorySource file source

    if not (verify snippets) then
      failwith $"Inventory self-test failed: {name}."

    count <- count + 1

  let rejects name source =
    let mutable rejected = false

    try
      inventorySource file source |> ignore
    with _ ->
      rejected <- true

    if not rejected then
      failwith $"Inventory self-test did not reject {name}."

    count <- count + 1

  accepts
    "inline language"
    """module Fixture
let show () = Helpers.codeSection "html" "Markup" Doc.Empty "<p>Hello</p>"
"""
    (fun snippets -> snippets.Length = 1 && snippets.Head.Language = "html")

  accepts
    "local source"
    """module Fixture
let show () =
  let code = "Doc.Empty"
  Helpers.codeSection "fsharp" "Example" Doc.Empty code
"""
    (fun snippets -> snippets.Length = 1 && snippets.Head.Code = "Doc.Empty")

  rejects
    "a local binding from another function"
    """module Fixture
let first () =
  let code = "Doc.Empty"
  code
let second code = Helpers.codeSection "fsharp" "Example" Doc.Empty code
"""

  rejects
    "a source parameter shadowing a constant"
    """module Fixture
let code = "Doc.Empty"
let show code = Helpers.codeSection "fsharp" "Example" Doc.Empty code
"""

  rejects
    "a lambda parameter shadowing a constant"
    """module Fixture
let code = "Doc.Empty"
let show = fun code -> Helpers.codeSection "fsharp" "Example" Doc.Empty code
"""

  rejects
    "a hidden cumulative predecessor"
    "module Fixture\nlet hidden = \"\"\"let ghost = 1\"\"\"\nlet shown = \"\"\"text (string ghost)\"\"\"\nlet sourceParts = [ hidden; shown ]\nlet show () = Helpers.codeSection \"fsharp\" \"Example\" Doc.Empty shown\n"

  accepts
    "displayed cumulative predecessors"
    """module Fixture
let first = "let value = 1"
let second = "text (string value)"
let sourceParts = [ first; second ]
let combined = String.concat "\n\n" sourceParts
let showFirst () = Helpers.codeSection "fsharp" "First" Doc.Empty first
let showSecond () = Helpers.codeSection "fsharp" "Second" Doc.Empty second
let showCombined () = Helpers.codeSection "fsharp" "Combined" Doc.Empty combined
"""
    (fun snippets ->
      snippets.Length = 3
      && (snippets |> List.exists (fun snippet -> snippet.Context = "let value = 1\n\n"))
      && (snippets
          |> List.exists (fun snippet -> snippet.Code = "let value = 1\n\ntext (string value)")))

  rejects "invalid source during inventory" "module Fixture\nlet value ="
  printfn "Passed %d snippet inventory self-tests." count

let blocksSnippet (diagnostic: FSharpDiagnostic) =
  diagnostic.Severity = FSharpDiagnosticSeverity.Error
  || diagnostic.ErrorNumber = 20

let mutable sourceVersion = 0

let checkSource path source isCompleteFile =
  sourceVersion <- sourceVersion + 1

  let project = {
    options with
        ProjectFileName = path + ".fsproj"
        SourceFiles = [| path |]
        UseScriptResolutionRules = not isCompleteFile
  }

  let parsed, answer =
    checker.ParseAndCheckFileInProject(path, sourceVersion, source, project)
    |> Async.RunSynchronously

  match answer with
  | FSharpCheckFileAnswer.Succeeded result -> Array.append parsed.Diagnostics result.Diagnostics
  | FSharpCheckFileAnswer.Aborted -> failwith $"F# checking aborted for {path}."

let namesIn (expr: SynExpr) =
  let ids = System.Collections.Generic.HashSet<string>()
  let qualified = System.Collections.Generic.HashSet<string>()

  let rec visit (value: obj) =
    if not (isNull value) then
      match value with
      | :? SynExpr as e ->
        match e with
        | SynExpr.Ident id -> ids.Add id.idText |> ignore
        | SynExpr.LongIdent(_, longId, _, _) ->
          let path = longId.LongIdent |> List.map _.idText

          match path with
          | head :: _ ->
            ids.Add head |> ignore
            qualified.Add(String.concat "." path) |> ignore
          | [] -> ()
        | _ -> ()
      | _ -> ()

      syntaxFields value |> Array.iter visit

  visit expr
  Set.ofSeq ids, Set.ofSeq qualified

let bindingParts binding =
  let (SynBinding(_, _, _, _, _, _, _, pattern, _, body, _, _, _)) = binding

  match pattern with
  | SynPat.Named(SynIdent(id, _), _, _, _) -> Some(id.idText, body)
  | SynPat.LongIdent(longId, _, _, _, _, _) ->
    longId.LongIdent |> List.tryLast |> Option.map (fun id -> id.idText, body)
  | _ -> None

let linkedExprs (code: string) =
  let parsed =
    checker.ParseFile(
      "prototype.fsx",
      SourceText.ofString code,
      {
        FSharpParsingOptions.Default with
            SourceFiles = [| "prototype.fsx" |]
            IsInteractive = true
      }
    )
    |> Async.RunSynchronously

  let declarations = ResizeArray<SynModuleDecl>()

  let rec visit (value: obj) =
    if not (isNull value) then
      match value with
      | :? SynModuleOrNamespace as (SynModuleOrNamespace(_, _, _, decls, _, _, _, _, _)) ->
        decls |> List.iter declarations.Add
      | _ -> syntaxFields value |> Array.iter visit

  visit parsed.ParseTree

  let bindings =
    declarations
    |> Seq.collect (function
      | SynModuleDecl.Let(_, binds, _, _) -> binds |> Seq.choose bindingParts
      | _ -> Seq.empty)
    |> Seq.toArray

  let deps =
    bindings
    |> Array.map (fun (name, expr) -> let ids, q = namesIn expr in name, (ids, q))
    |> Map.ofArray

  let roots =
    declarations
    |> Seq.collect (function
      | SynModuleDecl.Let(_, binds, _, _) ->
        binds
        |> Seq.choose (fun binding ->
          let (SynBinding(_, _, _, _, _, _, _, pattern, _, body, _, _, _)) = binding

          match pattern with
          | SynPat.Named(SynIdent(id, _), _, _, _) ->
            let _, q = namesIn body

            if
              q
              |> Set.exists (fun path -> path = "Var.Create" || path.EndsWith("Controller.create"))
            then
              Some id.idText
            else
              None
          | _ -> None)
      | _ -> Seq.empty)
    |> Set.ofSeq

  let rec expand seen queue =
    match queue with
    | [] -> seen
    | name :: rest when Set.contains name seen -> expand seen rest
    | name :: rest ->
      let children =
        deps
        |> Map.tryFind name
        |> Option.map fst
        |> Option.defaultValue Set.empty
        |> Set.toList

      expand (Set.add name seen) (children @ rest)

  let exprs =
    declarations
    |> Seq.choose (function
      | SynModuleDecl.Expr(expr, _) ->
        let ids, _ = namesIn expr
        Some(Set.intersect roots (expand Set.empty (Set.toList ids)))
      | _ -> None)
    |> Seq.toArray

  [
    for i in 0 .. exprs.Length - 1 do
      for j in i + 1 .. exprs.Length - 1 do
        let shared = Set.intersect exprs[i] exprs[j]

        if not shared.IsEmpty then
          yield i, j, shared
  ]

let topLevelRanges (code: string) =
  let parsed =
    checker.ParseFile(
      "composition-probe.fsx",
      SourceText.ofString code,
      {
        FSharpParsingOptions.Default with
            SourceFiles = [| "composition-probe.fsx" |]
            IsInteractive = true
      }
    )
    |> Async.RunSynchronously

  let declarations = ResizeArray<SynModuleDecl>()

  let rec visit (value: obj) =
    if not (isNull value) then
      match value with
      | :? SynModuleOrNamespace as (SynModuleOrNamespace(_, _, _, decls, _, _, _, _, _)) ->
        decls |> List.iter declarations.Add
      | _ -> syntaxFields value |> Array.iter visit

  visit parsed.ParseTree

  declarations
  |> Seq.choose (function
    | SynModuleDecl.Expr(expr, _) -> Some expr.Range
    | _ -> None)
  |> Seq.toArray

let isDocExpression (prefix: string) (code: string) (ranges: range array) target =
  let lines = code.Split '\n'
  let offsets = Array.zeroCreate lines.Length

  for i in 1 .. lines.Length - 1 do
    offsets[i] <- offsets[i - 1] + lines[i - 1].Length + 1

  let mutable modified = code

  for index in ranges.Length - 1 .. -1 .. 0 do
    let range = ranges[index]
    let start = offsets[range.StartLine - 1] + range.StartColumn
    let finish = offsets[range.EndLine - 1] + range.EndColumn
    let original = modified.Substring(start, finish - start)

    let replacement =
      if index = target then
        let body =
          original.Split '\n' |> Array.map (fun line -> "  " + line) |> String.concat "\n"

        "let __weave_doc_probe : WebSharper.UI.Doc = (\n" + body + "\n)"
      else
        "()"

    modified <- modified.Remove(start, finish - start).Insert(start, replacement)

  let path = Path.Combine(Path.GetTempPath(), "WeaveDocsCompositionProbe.fsx")

  checkSource path (SourceText.ofString (prefix + modified)) false
  |> Array.exists blocksSnippet
  |> not

// Shared reactive state implies that separate Docs usually form one working
// example. Probe their types so unit-valued mutations are not mistaken for Docs.
// Authors can explicitly mark reviewed independent alternatives with the line
// `// Independent alternatives`; comparisons are then intentionally exempt.
let linkedDocPairs (prefix: string) (source: string) =
  if Regex.IsMatch(source, @"(?m)^\s*// Independent alternatives\s*$") then
    []
  else
    let code = source.Replace("\r\n", "\n")
    let linked = linkedExprs code

    if linked.IsEmpty then
      []
    else
      let ranges = topLevelRanges code

      let cache = System.Collections.Generic.Dictionary<int, bool>()

      let isDoc i =
        match cache.TryGetValue i with
        | true, value -> value
        | _ ->
          let value = isDocExpression prefix code ranges i
          cache[i] <- value
          value

      linked
      |> List.choose (fun (first, second, shared) ->
        if isDoc first && isDoc second then
          Some(ranges[first].StartLine, ranges[second].StartLine, shared)
        else
          None)

let runCompositionSelfTests () =
  let diagnostics name code =
    let path = Path.Combine(Path.GetTempPath(), $"WeaveDocsComposition-{name}.fsx")
    checkSource path (SourceText.ofString (imports + "\n" + code)) false

  let discarded =
    diagnostics "discarded" "let render () =\n  text \"First\"\n  text \"Second\""

  if
    not (
      discarded
      |> Array.exists (fun diagnostic -> diagnostic.ErrorNumber = 20 && blocksSnippet diagnostic)
    )
  then
    failwith "Composition self-test failed: a discarded Doc must fail with FS0020."

  let composed =
    diagnostics "composed" "let render () =\n  Doc.Concat [ text \"First\"; text \"Second\" ]"

  if composed |> Array.exists blocksSnippet then
    failwith "Composition self-test failed: a returned Doc composition must pass."

  let prefix = imports + "\n"

  let linked =
    "let state = Var.Create false\nSwitch.create (state, text \"Toggle\")\ntextView (state.View |> View.Map string)"

  let independent =
    "let first = Var.Create false\nlet second = Var.Create false\nSwitch.create (first, text \"First\")\nSwitch.create (second, text \"Second\")"

  let mutation =
    "let state = Var.Create false\nVar.Set state true\ntextView (state.View |> View.Map string)"

  let factory =
    "let make () = let state = Var.Create false in Switch.create (state, text \"Toggle\")\nmake ()\nmake ()"

  let controller =
    "let controller = SnackbarController.create ()\nlet notify () = controller.show \"Saved\" |> ignore\nButton.primary (text \"Save\", onClick = notify)\nSnackbar.create controller"

  let composedTop =
    "let state = Var.Create false\nDoc.Concat [ Switch.create (state, text \"Toggle\"); textView (state.View |> View.Map string) ]"

  if (linkedDocPairs prefix linked).IsEmpty then
    failwith "Composition self-test: linked Docs must be flagged."

  if (linkedDocPairs prefix controller).IsEmpty then
    failwith "Composition self-test: callbacks must retain controller dependencies."

  if not (linkedDocPairs prefix independent).IsEmpty then
    failwith "Composition self-test: distinct states are independent."

  if not (linkedDocPairs prefix mutation).IsEmpty then
    failwith "Composition self-test: unit effects are not Docs."

  if not (linkedDocPairs prefix factory).IsEmpty then
    failwith "Composition self-test: factory-local state is independent."

  if not (linkedDocPairs prefix composedTop).IsEmpty then
    failwith "Composition self-test: composed Docs must pass."

  if not (linkedDocPairs prefix ("// Independent alternatives\n" + linked)).IsEmpty then
    failwith "Composition self-test: explicit alternatives must pass."

  printfn "Passed 9 snippet composition self-tests."

let runProjectCacheSelfTest () =
  let path = Path.Combine(Path.GetTempPath(), "WeaveDocsCacheSelfTest.fsx")

  let diagnostics code =
    checkSource path (SourceText.ofString (imports + "\n" + code)) false

  if diagnostics "let localOnly = 1" |> Array.exists blocksSnippet then
    failwith "Project cache self-test: the initial snippet must pass."

  if diagnostics "text (string localOnly)" |> Array.exists blocksSnippet |> not then
    failwith "Project cache self-test: a later snippet must not borrow prior source."

  if diagnostics "text \"Valid\"" |> Array.exists blocksSnippet then
    failwith "Project cache self-test: a valid snippet must pass after a failure."

  printfn "Passed 1 snippet project-cache self-test."

let runArgumentSelfTests () =
  let release = parseArguments [ "--configuration"; "Release"; "ButtonExamples" ]

  if
    release.Configuration <> "Release"
    || release.Filters <> [ "ButtonExamples" ]
    || release.SelfTest
  then
    failwith "Argument self-test failed: Release must select assemblies without becoming a filename filter."

  let defaults = parseArguments [ "ButtonExamples"; "--self-test" ]

  if
    defaults.Configuration <> "Debug"
    || defaults.Filters <> [ "ButtonExamples" ]
    || not defaults.SelfTest
  then
    failwith "Argument self-test failed: filename filters and self-test must preserve the Debug default."

  let audit =
    parseArguments [ "--composition-audit"; "--configuration"; "Release"; "ButtonExamples" ]

  if
    not audit.CompositionAudit
    || audit.Configuration <> "Release"
    || audit.Filters <> [ "ButtonExamples" ]
  then
    failwith "Argument self-test failed: composition audit must preserve configuration and filters."

  let mutable rejected = false

  try
    parseArguments [ "--configuration"; "Shipping" ] |> ignore
  with _ ->
    rejected <- true

  if not rejected then
    failwith "Argument self-test failed: unknown configurations must fail."

  printfn "Passed 4 snippet argument self-tests."

if commandLine.SelfTest then
  runArgumentSelfTests ()
  runInventorySelfTests ()
  runCompositionSelfTests ()
  runProjectCacheSelfTest ()
  exit 0

if commandLine.CompositionAudit then
  let mutable count = 0
  let mutable failures = 0

  for file in files do
    if selected (Path.GetFileName file) then
      for snippet in readSnippets file do
        if
          snippet.Language = "fsharp"
          && not (
            snippet.Code.TrimStart().StartsWith("namespace ")
            || snippet.Code.TrimStart().StartsWith("module ")
          )
        then
          count <- count + 1
          let pairs = linkedDocPairs (imports + "\n" + snippet.Context) snippet.Code

          if not pairs.IsEmpty then
            failures <- failures + 1

            printfn
              "FAIL %s:%d [%s] linked top-level Docs: %A"
              (Path.GetFileName file)
              snippet.Line
              snippet.Title
              pairs

  printfn "Audited %d static F# snippets for linked Docs; %d failures." count failures

  if count = 0 then
    eprintfn "No static F# snippets matched the supplied filenames."
    exit 1
  elif failures > 0 then
    exit 1
  else
    exit 0

let generatedSnippets = [
  if selected "GridExamples.fs" || selected "GridSource.fs" then
    let cases = [
      // Every public span name with every justification.
      for justify in Weave.Docs.GridSource.justifyNames do
        for span in 1..12 do
          yield $"span {span}, {justify}", justify, [ span ]

      // Every supported item count, including mixed spans and the longest list.
      for justify in Weave.Docs.GridSource.justifyNames do
        for count in 1..12 do
          yield $"count {count}, {justify}", justify, [ 1..count ]
    ]

    if cases.Length <> 144 then
      failwith "Grid source coverage requires 12 spans, 12 counts and 6 justifications."

    for number, (title, justify, spans) in List.indexed cases do
      let code = Weave.Docs.GridSource.render justify spans

      if code.Split('\n') |> Array.exists (fun line -> line.Length > 78) then
        failwith $"Generated grid source exceeds 78 columns: {title}."

      yield {
        File = Path.Combine(root, "src", "Weave.Docs", "GridSource.fs")
        Line = 1
        Column = 0
        Number = number + 1
        Code = code
        Language = "fsharp"
        Title = title
        Context = ""
      }
]

let appearanceSnippets = [
  for page, sourceFile, cases in
    [
      "GridExamples.fs", "GridSource.fs", Weave.Docs.GridSource.spacingCases ()
      "ButtonExamples.fs",
      "ButtonSource.fs",
      Weave.Docs.ButtonSource.validationCases ()
      @ Weave.Docs.ButtonSource.iconValidationCases ()
      "TextFieldExamples.fs", "TextFieldAppearance.fs", Weave.Docs.Examples.TextFieldAppearance.sourceCases ()
      "TabsExamples.fs", "TabsAppearanceSource.fs", Weave.Docs.TabsAppearanceSource.cases ()
      "ProgressExamples.fs", "ProgressAppearance.fs", Weave.Docs.ProgressAppearance.cases ()
      "AlertExamples.fs", "AlertAppearance.fs", Weave.Docs.AlertAppearance.cases ()
      "CardExamples.fs", "GroupBAppearance.fs", Weave.Docs.GroupBAppearance.cardCases ()
      "ButtonGroupExamples.fs", "GroupBAppearance.fs", Weave.Docs.GroupBAppearance.groupCases ()
      "CheckboxExamples.fs", "ToggleAppearance.fs", Weave.Docs.ToggleAppearance.cases "Checkbox" ()
      "SwitchExamples.fs", "ToggleAppearance.fs", Weave.Docs.ToggleAppearance.cases "Switch" ()
      "ExpansionPanelExamples.fs", "ExpansionAppearance.fs", Weave.Docs.ExpansionAppearance.cases ()
      "ButtonMenuExamples.fs", "ButtonMenuAppearance.fs", Weave.Docs.ButtonMenuAppearance.cases ()
      "ChipExamples.fs", "ChipAppearance.fs", Weave.Docs.ChipAppearance.cases ()
      "SliderExamples.fs", "SliderAppearance.fs", Weave.Docs.SliderAppearance.cases ()
      "PaginationExamples.fs", "PaginationAppearance.fs", Weave.Docs.PaginationAppearance.cases ()
      "AppBarExamples.fs", "AppBarAppearance.fs", Weave.Docs.AppBarAppearance.cases ()
      "DialogExamples.fs", "DialogAppearance.fs", Weave.Docs.DialogAppearance.cases ()
      "DrawerExamples.fs", "DrawerAppearance.fs", Weave.Docs.DrawerAppearance.cases ()
      "CarouselExamples.fs", "CarouselAppearance.fs", Weave.Docs.CarouselAppearance.cases ()
      "SelectExamples.fs", "SelectAppearance.fs", Weave.Docs.SelectAppearance.cases ()
      "DropdownMenuExamples.fs", "DropdownAppearance.fs", Weave.Docs.DropdownAppearance.cases ()
      "ContainerExamples.fs", "ContainerAppearance.fs", Weave.Docs.ContainerAppearance.cases ()
      "FieldExamples.fs", "FieldAppearance.fs", Weave.Docs.FieldAppearance.cases ()
      "SnackbarExamples.fs", "SnackbarAppearance.fs", Weave.Docs.SnackbarAppearance.cases ()
      "ScrollToTopExamples.fs", "ScrollTopAppearance.fs", Weave.Docs.ScrollTopAppearance.cases ()
      "SpacerExamples.fs", "SpacerAppearance.fs", Weave.Docs.SpacerAppearance.cases ()
      "ColorPickerExamples.fs", "ColorPickerAppearance.fs", Weave.Docs.ColorPickerAppearance.cases ()
      "RadioButtonExamples.fs", "RadioAppearance.fs", Weave.Docs.RadioAppearance.cases ()
      "NumericFieldExamples.fs", "NumericFieldAppearance.fs", Weave.Docs.NumericFieldAppearance.cases ()
      "ChipSetExamples.fs", "ChipSetAppearance.fs", Weave.Docs.ChipSetAppearance.cases ()
      "LinkExamples.fs", "ReferenceAppearanceSource.fs", Weave.Docs.ReferenceAppearanceSource.Link.cases ()
      "TooltipExamples.fs",
      "ReferenceAppearanceSource.fs",
      Weave.Docs.ReferenceAppearanceSource.Tooltip.cases ()
      "DividerExamples.fs",
      "ReferenceAppearanceSource.fs",
      Weave.Docs.ReferenceAppearanceSource.Divider.cases ()
      "TypographyExamples.fs", "TypographyAppearanceSource.fs", Weave.Docs.TypographyAppearanceSource.cases ()
      "HighlighterExamples.fs",
      "DataAppearanceSource.fs",
      Weave.Docs.DataAppearanceSource.Highlighter.cases ()
      "TableExamples.fs", "DataAppearanceSource.fs", Weave.Docs.DataAppearanceSource.Table.cases ()
      "ListExamples.fs", "ListAppearanceSource.fs", Weave.Docs.ListAppearanceSource.cases ()
      "IconsExamples.fs", "IconPlaygroundSource.fs", Weave.Docs.IconPlaygroundSource.cases ()
    ] do
    if selected page || selected sourceFile then
      for number, (title, code) in List.indexed cases do
        if code.Split('\n') |> Array.exists (fun line -> line.Length > 80) then
          failwith $"Generated appearance source exceeds 80 columns: {title}."

        yield {
          File = exampleSourceByName.[sourceFile]
          Line = 1
          Column = 0
          Number = number + 1
          Code = code
          Language = "fsharp"
          Title = title
          Context = ""
        }
]

let snippets = [
  for file in files do
    if selected (Path.GetFileName file) then
      yield! readSnippets file

  yield! generatedSnippets
  yield! appearanceSnippets
]

let failures = ResizeArray<Failure>()
let mutable checkedCount = 0
let mutable excludedCount = 0
let mutable cumulativeCount = 0

for snippet in snippets do
  let name = Path.GetFileName snippet.File

  if not (List.contains snippet.Language [ "fsharp"; "css"; "html"; "bash"; "powershell" ]) then
    failwith $"Unknown source language '{snippet.Language}' in {name}:{snippet.Line}."

  if snippet.Language <> "fsharp" then
    excludedCount <- excludedCount + 1
    printfn "EXCLUDED %s:%d [%s] %s" name snippet.Line snippet.Language snippet.Title
  else
    checkedCount <- checkedCount + 1

    if checkedCount % 1000 = 0 then
      printfn "Checked %d F# snippets so far." checkedCount

    if snippet.Context <> "" then
      cumulativeCount <- cumulativeCount + 1

    let isCompleteFile =
      (snippet.Code.TrimStart().StartsWith("namespace ", StringComparison.Ordinal)
       || snippet.Code.TrimStart().StartsWith("module ", StringComparison.Ordinal))

    let extension = if isCompleteFile then ".fs" else ".fsx"

    // Reuse the project identity so FCS can reuse its project-wide cache.
    // The source version changes for each independent snippet check.
    let path = Path.Combine(Path.GetTempPath(), "WeaveDocsCheckedSnippet" + extension)

    let prefix =
      if isCompleteFile then
        ""
      else
        imports + "\n" + snippet.Context

    let prefixLines = prefix |> Seq.filter ((=) '\n') |> Seq.length
    let source = SourceText.ofString (prefix + snippet.Code)

    let diagnostics = checkSource path source isCompleteFile

    let staticExample =
      (snippet.File.StartsWith(
        examplesRoot + string Path.DirectorySeparatorChar,
        StringComparison.OrdinalIgnoreCase
       )
       && isDisplayedExample snippet.File)
      || String.Equals(
        snippet.File,
        Path.Combine(root, "src", "Weave.Docs", "DocsHome.fs"),
        StringComparison.OrdinalIgnoreCase
      )

    if
      staticExample
      && not isCompleteFile
      && not (diagnostics |> Array.exists blocksSnippet)
    then
      let pairs = linkedDocPairs prefix snippet.Code

      if not pairs.IsEmpty then
        let message =
          "Linked top-level Docs share reactive state; compose them or mark reviewed independent alternatives."

        printfn "FAIL %s:%d [snippet %d] %s %A" name snippet.Line snippet.Number message pairs

        failures.Add {
          File = name
          Line = snippet.Line
          Snippet = snippet.Number
          Message = message
        }

    for diagnostic in
      diagnostics
      |> Array.distinctBy (fun item -> item.StartLine, item.StartColumn, item.ErrorNumber, item.Message) do
      if
        blocksSnippet diagnostic
        && (diagnostic.StartLine > prefixLines || diagnostic.StartLine <= 0)
      then
        let line = snippet.Line
        let codeLine = max 1 (diagnostic.StartLine - prefixLines)
        let message = diagnostic.Message.Replace("\r", " ").Replace("\n", " ")

        printfn
          "FAIL %s:%d [snippet %d, code line %d, FS%04d] %s"
          name
          line
          snippet.Number
          codeLine
          diagnostic.ErrorNumber
          message

        failures.Add {
          File = name
          Line = line
          Snippet = snippet.Number
          Message = message
        }

printfn
  "Checked %d isolated, %d cumulative and %d generated F# snippets; explicitly excluded %d non-F# blocks; %d errors."
  (checkedCount
   - cumulativeCount
   - generatedSnippets.Length
   - appearanceSnippets.Length)
  cumulativeCount
  (generatedSnippets.Length + appearanceSnippets.Length)
  excludedCount
  failures.Count

if snippets.IsEmpty then
  eprintfn "No snippets matched the supplied filenames."
  exit 1

if failures.Count > 0 then
  exit 1
