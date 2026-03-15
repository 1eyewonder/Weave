# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What is Weave

Weave is a component library for building web UIs in F# using [WebSharper](https://websharper.com/). It provides reactive UI components, layout primitives, and a theming system (light/dark). Status: experimental, no NuGet packages yet.

## Build & Development Commands

**Initial setup:**

```bash
./build.sh init        # Linux/macOS
./build.cmd init       # Windows
```

**Start the docs site** (view components interactively at `http://localhost:5000`):

```bash
dotnet run --project src/Weave.Docs/Weave.Docs.fsproj
```

**Compile CSS** (required after SCSS changes):

```bash
yarn build:css         # one-shot
yarn watch:css         # watch mode
```

**Run all tests:**

```bash
./build.sh RunTests    # Linux/macOS (requires .NET 10, Node 22)
./build.cmd RunTests   # Windows

# Or via Docker (matches CI exactly):
docker compose build && docker compose run --rm playwright-tests
```

**Run a single unit test project:**

```bash
dotnet test tests/Weave.Tests.Unit/Weave.Tests.Unit.fsproj
```

**Run a single rendering test project:**

```bash
dotnet test tests/Weave.Tests.Rendering/Weave.Tests.Rendering.fsproj
```

**Check/apply code formatting** (Fantomas):

```bash
dotnet fantomas .              # format all files
dotnet fantomas --check .      # verify formatting (what CI runs)
```

**Other FAKE build targets:** `Clean`, `Restore`, `Build`, `BuildDocs`, `Analyze`, `CheckFormat`

## Architecture Overview

```text
src/
  Weave/                  # Core library (netstandard2.0)
    Helpers.fs            # View/Doc/Attr combinators
    Theming.fs            # Light/Dark theme system, PaletteColor
    components/Core.fs    # TypedCssClasses (Css), CssHelpers (cl, cls), HTML aliases
    components/Utilities.fs  # DocumentEventListener, ResizeListener, Disabled
    components/*.fs       # ~30 UI components
    scss/                 # SCSS source (compiled to styles.css)
  Weave.Docs/             # Interactive docs site (net10.0, ASP.NET Core + WebSharper)
    examples/             # One example file per component
    ExamplesRouter.fs     # Client-side SPA routing
tests/
  Weave.Tests.Unit/       # Expecto unit tests (verify toClass mappings)
  Weave.Tests.Rendering/  # Playwright layout tests + HTML fixtures
build/
  Build.fs                # FAKE build targets
```

**Compilation order matters** in F# — files in `.fsproj` must be ordered so dependencies come first. New components go after `Core.fs` and any components they compose.

## Component Conventions

Every component follows this pattern:

```fsharp
namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module MyComponent =

  [<RequireQualifiedAccess; Struct>]
  type Variant = | Filled | Outlined

  module Variant =
    let toClass variant =
      match variant with
      | Variant.Filled   -> Css.``weave-mycomponent--filled``
      | Variant.Outlined -> Css.``weave-mycomponent--outlined``

  // Color module uses global BrandColor DU (no RequireQualifiedAccess needed on Color)
  module Color =
    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-mycomponent--primary``
      // ...

open MyComponent

[<JavaScript>]
type MyComponent =

  static member Create(innerContents: Doc, ?enabled: View<bool>, ?attrs: Attr list) =
    let enabled = defaultArg enabled (View.Const true)
    let attrs   = defaultArg attrs   []
    div [
      cl Css.``weave-mycomponent``
      View.not enabled |> Attr.DynamicClassPred Css.``weave-mycomponent--disabled``
      yield! attrs
    ] [ innerContents ]
```

**Key rules:**

- `[<JavaScript>]` on every module and type (required for WebSharper transpilation)
- **No styling parameters** (`variant`, `color`, `size`) on `Create` — callers pass them via `?attrs` using `toClass` helpers
- `?attrs: Attr list` is always the last optional parameter; defaults to `[]`; `yield! attrs` comes last so callers can extend
- **Reactive parameters:** use `Var<'T>` for two-way bindings, `View<'T>` for read-only reactive inputs; avoid plain values for anything that can change
- **CSS helpers:** `cl` for a single class, `cls [...]` for multiple — never chain `cl` calls where `cls` applies
- DU types for variants/sizes use `[<RequireQualifiedAccess; Struct>]` to prevent name collisions; `Color` modules use global `BrandColor` DU
- `Attr.DynamicClassPred` gates a modifier class on a `View<bool>`

## SCSS Conventions

- Files: `src/Weave/scss/components/_{name}.scss` (underscore prefix, lowercase)
- BEM naming: `.weave-{name}`, `.weave-{name}--modifier`, `.weave-{name}__element`
- **Never hard-code colors** — always use `var(--palette-*)`, `var(--typography-*)`, `var(--default-*)`
- Color modifiers use the SCSS loop: `@each $color in $palette-colors { ... }`
- Register new component SCSS in `src/Weave/scss/main.scss` (alphabetical order within the `// Components` block)
- After SCSS changes, run `yarn build:css` to regenerate `styles.css`

## Adding a New Component

When asked to scaffold a new component, use the `/scaffold-component` skill — it provides the full step-by-step checklist. The high-level steps are:

1. `src/Weave/components/{Name}.fs` — component module + type
2. `src/Weave/scss/components/_{name}.scss` — BEM styles
3. Register SCSS import in `src/Weave/scss/main.scss`
4. Register F# file in `src/Weave/Weave.fsproj`
5. `src/Weave.Docs/examples/{Name}Examples.fs` — interactive docs examples
6. Register example in `src/Weave.Docs/Weave.Docs.fsproj` and `ExamplesRouter.fs` (4 places: `Page` DU, `pageToString`, `renderPage`, nav list)
7. `tests/Weave.Tests.Unit/{Name}Tests.fs` — Expecto tests for `toClass` mappings
8. `tests/Weave.Tests.Rendering/{Name}LayoutTests.fs` — Playwright layout tests
9. `tests/Weave.Tests.Rendering/fixtures/{name}.html` — static HTML fixture using compiled CSS
10. `tests/Weave.Tests.E2E/accessibility/{Name}Tests.fs` — axe-core scan + keyboard/focus tests
11. Register E2E page in `tests/Weave.Tests.E2E.Site/Pages.fs`
12. Run `dotnet fantomas .` to format

## Testing Patterns

**Unit tests** (Expecto) — test pure `toClass` mappings only, never DOM:

```fsharp
module Weave.Tests.Unit.{Name}Tests
open Expecto
open Weave

[<Tests>]
let {name}Tests =
  testList "{Name}" [
    testTheory "each variant maps to the correct class" [
      MyComponent.Variant.Filled, "weave-mycomponent--filled"
    ]
    <| fun (variant, expected) -> Expect.equal (MyComponent.Variant.toClass variant) expected ""
  ]
```

**Rendering tests** (Playwright/xUnit) — load a static HTML fixture, assert on layout:

- Inherit `PageTest()`, call `this.LoadFixture()` at the start of every `[<Fact>]`
- Use `BoundingBoxAsync()` for positional assertions; allow ±1px tolerance
- Use `Page.EvaluateAsync<string>` for computed styles on hidden elements
- Fixtures link to `../../../src/Weave/styles.css` and use BEM classes directly

**E2E accessibility tests** (Playwright/xUnit + axe-core) — test against live WebSharper-rendered pages:

- Inherit `E2ETestBase(server)` (provides `NavigateTo`, `RunAxeScan`, `Expect`)
- Every component needs at least `this.RunAxeScan("{name}")` for automated a11y scanning
- **Prefer typed Playwright assertions over `EvaluateAsync` JS strings:** use `this.Expect(locator).ToBeFocusedAsync()`, `.ToBeCheckedAsync()`, `.ToHaveAttributeAsync()`, `.ToHaveClassAsync(Regex(...))`, `.ToHaveCountAsync()`, `.ToBeVisibleAsync()`, `.ToBeHiddenAsync()`
- Playwright assertions auto-retry, so they replace `WaitForFunctionAsync` + `EvaluateAsync` in most cases
- Only use `EvaluateAsync` when no typed API exists (e.g. DOM containment, injecting event listeners)
- Mark unimplemented keyboard features with `[<Fact(Skip = "Known gap: ...")>]`

## Key Utilities

- `Helpers.fs`: `View` extensions (`mapCached`, `zip`, `not`), `ViewOption` monad, `Attr.bindOption`, `Attr.classSelection`
- `CssHelpers` (in `Core.fs`): `cl`, `cls`, `Css` (TypedCssClasses), `Palette` module (CSS var refs), `Attr.toggleStyleOrDefault`
- `Utilities.fs`: `DocumentEventListener` (click-outside detection), `ResizeListener` (ResizeObserver), `Disabled` class helper
- `Theming.fs`: `ThemeMode` (Light/Dark), `ThemePalette`, `PaletteColor`
