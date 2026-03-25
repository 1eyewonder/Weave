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

**Run E2E accessibility tests:**

```bash
dotnet test tests/Weave.Tests.E2E/Weave.Tests.E2E.fsproj
```

**Build the E2E site** (must be verified separately — the E2E test project compiles independently of the site it hosts):

```bash
dotnet build tests/Weave.Tests.E2E.Site/Weave.Tests.E2E.Site.fsproj
```

**Check/apply code formatting** (Fantomas):

```bash
dotnet fantomas .              # format all files
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
    DocsRouting.fs        # Page DU, URL hash routing helpers
    ComponentPreviews.fs  # Thumbnail previews for the home-page grid
    ExamplesRouter.fs     # SPA shell: renderPage, navigation, scroll/TOC
tests/
  Weave.Tests.Unit/       # Expecto unit tests
  Weave.Tests.Rendering/  # Playwright layout tests + HTML fixtures
  Weave.Tests.E2E/        # Playwright E2E + axe-core accessibility tests
    accessibility/        # One test file per component
    TestServerFixture.fs  # Shared xUnit fixture that hosts the E2E site
  Weave.Tests.E2E.Site/   # WebSharper SPA served to E2E tests — compiled separately!
    Pages.fs              # One page function per component (uses same camelCase Weave API)
build/
  Build.fs                # FAKE build targets
```

> **Important:** `Weave.Tests.E2E.Site` is **not** a dependency of `Weave.Tests.E2E` at compile time — the test runner launches it as a subprocess at runtime. This means `dotnet build tests/Weave.Tests.E2E/...` will succeed even when `Pages.fs` has errors. **Always build the site project explicitly** after any API changes:
>
> ```bash
> dotnet build tests/Weave.Tests.E2E.Site/Weave.Tests.E2E.Site.fsproj
> ```

**Compilation order matters** in F# — files in `.fsproj` must be ordered so dependencies come first. New components go after `Core.fs` and any components they compose.

## Component Conventions

See `weave-component-conventions.skill.md` for full patterns and code templates. The essentials:

- `[<JavaScript>]` on every top level module and type (required for WebSharper transpilation). Child modules/types inherit from parents, so only the root module and root types need the attribute.
- **Static member functions use camelCase** (e.g., `Button.create`, `IconButton.create`), not PascalCase
- **No styling parameters** on `create` — callers pass them via `?attrs` using the component's style modules
- `?attrs: Attr list` is always the last optional parameter; defaults to `[]`; `yield! attrs` comes last
- **Reactive parameters:** `Var<'T>` for two-way bindings, `View<'T>` for read-only; avoid plain values for anything that can change
- **Collection items use `*Item` types** with `static member create` and optional parameters — never pipeline/builder `with*` modules

## SCSS Conventions

- Files: `src/Weave/scss/components/_{name}.scss` (underscore prefix, lowercase)
- BEM naming: `.weave-{name}`, `.weave-{name}--modifier`, `.weave-{name}__element`
- **Never hard-code colors** — always use `var(--palette-*)`, `var(--typography-*)`, `var(--default-*)`
- Color modifiers use the SCSS loop: `@each $color in $palette-colors { ... }`
- Register new component SCSS in `src/Weave/scss/main.scss` (alphabetical order within the `// Components` block)
- After SCSS changes, run `yarn build:css` to regenerate `styles.css`

## Adding a New Component

Use the `/scaffold-component` skill — it provides the full step-by-step checklist covering the F# component, SCSS, docs example, router registration, unit tests, rendering tests, E2E accessibility tests, and verification builds.

## Testing Patterns

Detailed patterns and code templates live in the agent/skill definitions. Quick reference:

- **Unit tests** (Expecto) — test pure mapping functions like `toAttr`, never DOM. Components using plain `let` bindings don't need unit tests — the compiler enforces correctness. See `unit-tester` agent.
- **Rendering tests** (Playwright/xUnit) — load a static HTML fixture, assert on layout with `BoundingBoxAsync()`. See `/scaffold-component` skill §8.
- **E2E accessibility tests** (Playwright/xUnit + axe-core) — every component needs at least `this.RunAxeScan("{name}")`. Prefer typed Playwright assertions over `EvaluateAsync` JS strings. See `playwright-pro` agent and `/scaffold-component` skill §9.

## Key Utilities

- `Helpers.fs`: `View` extensions (`mapCached`, `zip`, `not`), `ViewOption` monad, `Attr.bindOption`, `Attr.classSelection`
- `CssHelpers` (in `Core.fs`): `cl`, `cls`, `Css` (TypedCssClasses), `Palette` module (CSS var refs), `Attr.toggleStyleOrDefault`
- `Utilities.fs`: `DocumentEventListener` (click-outside detection), `ResizeListener` (ResizeObserver), `Disabled` class helper
- `Theming.fs`: `ThemeMode` (Light/Dark), `ThemePalette`, `PaletteColor`
