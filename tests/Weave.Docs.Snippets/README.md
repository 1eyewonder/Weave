# Documentation snippet checks

The checker compiles displayed F# against the built Weave and WebSharper assemblies. Build the docs project first, then run the checker from the repository root:

```sh
dotnet build src/Weave.Docs/Weave.Docs.fsproj
dotnet fsi tests/Weave.Docs.Snippets/check.fsx
```

Pass example filenames to check only the pages you edited:

```sh
dotnet fsi tests/Weave.Docs.Snippets/check.fsx ButtonExamples TabsExamples
```

The checker uses Debug by default. After a Release build, pass `--configuration Release`. The `BuildDocs`, `RunTests`, and `RunNonBrowserTests` targets run the source checks as part of their own workflows. Run `--self-test` to check the checker's inventory and composition rules.

## What it checks

- F# syntax and types in static examples, cumulative showcases, and registered configurator output. The checker uses FSharp.Compiler.Service from the selected .NET SDK.
- Unknown code languages and unresolved dynamic source fail the check. Declared non-F# blocks are reported as exclusions.
- FS0020 warnings fail when a component expression is discarded. Other warning codes do not fail the gate.
- Static examples with separate top-level `Doc` expressions sharing reactive state are flagged. If a reviewed comparison intentionally shows independent alternatives, put `// Independent alternatives` on its own line in that source block.

Ordinary component snippets are checked independently. They can use these standard imports, but cannot borrow private helpers or state from the live preview:

```fsharp
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
```

Put any additional imports, model types, and local helpers in the displayed source. Complete files beginning with `namespace` or `module` are checked without the standard imports. Keep shared project setup in Getting Started.

Showcases can list displayed blocks in `sourceParts`. The checker compiles each block with only its displayed predecessors and checks the combined copyable source too. See [ShowcaseTaskTracker.fs](../../src/Weave.Docs/examples/showcases/ShowcaseTaskTracker.fs). Comparison blocks may contain independent component calls; do not add preview-only layout just to make them one expression.

The [Grid Builder source](../../src/Weave.Docs/examples/grid/GridSource.fs) and configurator generators are registered for generated-case checks. Add new runtime-generated source to `appearanceSnippets` in [check.fsx](check.fsx). The report shows separate isolated, cumulative, and generated counts.

The checker does not run browser code or prove that source matches the preview, translates through WebSharper, or works at every viewport. Review rendered behavior and copied code separately. For the static composition audit alone, run:

```sh
dotnet fsi tests/Weave.Docs.Snippets/check.fsx --configuration Release --composition-audit
```
