# Weave

<div align="center">
    <img src="./src/Weave/resources/weave-logo.png" alt="Weave Logo" width="400"/>
</div>
<div align="center">
    <i>Threading Logic. Fabricating UI.</i>
</div>
<br/>

[Weave](https://1eyewonder.github.io/Weave) is a component library for building web applications in F# with [WebSharper](https://websharper.com/). It provides a growing set of reusable UI components, layout primitives, and styling utilities designed to reduce boilerplate and improve developer experience.

Weave is opinionated where it matters — consistent theming, ergonomic APIs, and structured styling — while remaining flexible enough to work seamlessly with native WebSharper constructs.

> **Note:** Weave is in active development. The first stable release is available on NuGet.

## Releases and WebSharper compatibility

[`Weave.WebSharper` 0.2.0](https://www.nuget.org/packages/Weave.WebSharper/0.2.0) and [`Weave.WebSharper.Templates` 0.2.1](https://www.nuget.org/packages/Weave.WebSharper.Templates/0.2.1) are available on NuGet. See [starter templates](templates/README.md) for install commands. The currently tested baseline is WebSharper and WebSharper.FSharp **10.1.9.686** with WebSharper.UI **10.1.7.686**, as locked in `paket.lock`. NuGet dependencies express minimum versions; other versions are not yet verified. See the [release playbook](RELEASING.md) for publication and post-release documentation steps.

## Why Weave?

WebSharper provides a powerful foundation for full-stack F# web development, but it lacks a cohesive, modern component ecosystem comparable to what exists in other frameworks. While working in WebSharper, I found myself reaching for the kind of ergonomics and completeness that component libraries in other ecosystems take for granted — clean APIs, minimal boilerplate, rich theming, and strong developer experience. Weave is an attempt to bring those qualities to F# and WebSharper.

Weave aims to fill that gap by providing:

- **Type-safe styling** — Discriminated unions and typed helpers guide you to valid styles, without locking you out of WebSharper's raw functionality when you need it
- **A functional-first API** — Clean camelCase members, optional parameters, and composable attrs, designed for F# from the ground up
- **Built-in theming** — Light and dark mode via CSS custom properties with a one-line toggle
- **Seamless WebSharper integration** — Reactive `Var`/`View` bindings, native `Attr`/`Doc` types, and full interop with existing WebSharper code
- **Less boilerplate** — Sensible defaults out of the box; customize only when you need to
- **Structured styling** — BEM naming conventions and CSS custom properties, themed by default and overridable by design

This is not a port of patterns from another ecosystem — it is a reinterpretation with an F# mindset.

## Component Design Philosophy

### Functional Language, Practical API

F# encourages partial application and functional composition. However, UI components often require many optional parameters — events, styles, variants, and configuration options.

To avoid excessive function overloads and deeply nested parameter patterns, Weave components use classes with optional parameters:

```fsharp
Button.create(
    text "Hello World!",
    onClick = (fun () -> ())
)
```

This provides:

- Clear discoverability via IntelliSense
- Predictable component construction
- A manageable API surface
- Extensibility as components grow in complexity

### Forms and validation

Use `Form.model` to compose validated fields into typed values and bind them directly to Weave controls. See [Forms & Validation](https://1eyewonder.github.io/Weave/#forms) for live examples and the API reference.

### Strongly-Typed Styling

Styling in Weave is designed to be discoverable, constrained, composable, and type-safe where practical. Rather than relying on raw strings for CSS classes, Weave provides modules and helpers that map directly to supported styling variants:

```fsharp
Button.primary(
    text "Hello World!",
    onClick = (fun () -> printfn "Clicked!"),
    attrs = [
        Button.Variant.outlined
    ]
)
```

This approach improves IntelliSense discoverability, reduces invalid style combinations, and keeps styling aligned with component intent. Developers still retain full access to WebSharper's native styling tools (`Attr.Style`, raw classes, etc.) when flexibility is required.

## Theming

Weave's CSS custom properties define its light and dark palettes. Load application CSS after Weave and override tokens on the body that holds `data-theme`:

```css
:root { --weave-font-family-body: Arial, sans-serif; }

body:not([data-theme]), body[data-theme="light"] {
  --palette-primary: #6200ea;
  --palette-primary-text: #fff;
  --palette-primary-lighten: #7c4dff;
  --palette-primary-darken: #4a00b8;
  --palette-primary-rgb: 98, 0, 234;
  --palette-primary-fill: var(--palette-primary);
  --palette-primary-fill-text: var(--palette-primary-text);
  --palette-primary-fill-hover: var(--palette-primary-darken);
  --palette-primary-fill-border: transparent;
  --palette-surface: #fff;
  --palette-text-primary: #21243a;
  --palette-text-primary-rgb: 33, 36, 58;
}

body[data-theme="dark"] {
  --palette-primary: #b388ff;
  --palette-primary-text: #201b2e;
  --palette-primary-lighten: #d1b3ff;
  --palette-primary-darken: #9970ff;
  --palette-primary-rgb: 179, 136, 255;
  --palette-primary-fill: var(--palette-primary);
  --palette-primary-fill-text: var(--palette-primary-text);
  --palette-primary-fill-hover: var(--palette-primary-darken);
  --palette-primary-fill-border: transparent;
  --palette-surface: #202333;
  --palette-text-primary: #e6ebf5;
  --palette-text-primary-rgb: 230, 235, 245;
}
```

Common customization tokens include `--palette-background`, `--palette-background-paper`, `--palette-surface`, `--palette-text-primary`, `--palette-text-secondary`, `--palette-divider`, `--palette-divider-light`, `--palette-lines-default`, `--palette-lines-inputs`, and `--weave-font-family-display`, `--weave-font-family-body`, and `--weave-font-family-mono`. Primary, secondary, tertiary, success, error, warning, and info colors each have a main token and `-text`, `-lighten`, `-darken`, and `-rgb` companions. RGB companions contain comma-separated channels, not an `rgb()` function. Filled components also use `-fill`, `-fill-text`, `-fill-hover`, and `-fill-border` companions; override these on each theme selector when you customize a color, as shown above. Override matching companions together where components use them for contrast, hover, or transparency. Set shared font-role tokens on `:root`, where typography variants resolve them. To change a specific variant by mode, set a token such as `--typography-body1-family` on the mode selector. The complete defaults are in [the theme stylesheet](src/Weave/scss/core/_themes.scss) and [the typography variables](src/Weave/scss/core/_variables.scss).

The documentation site’s **Theming** page includes a grouped color and typography variable reference, light/dark defaults, and examples.

Use F# to select the CSS mode:

```fsharp
open Weave.Theming

setMode (detectSystemPreference ()) // reads the current OS preference once
let newMode = toggleMode ()         // returns the selected mode
```

`setMode` writes `data-theme` to `body`; `getMode` returns `Light` when it is absent or not `dark`. The helpers do not persist preferences or watch for OS changes. Set the initial mode as early as practical to minimize a flash of the wrong mode.

**Migration:** The F# palette/configuration APIs (`PaletteColor`, `ThemePalette`, `TypographyConfig`, `ThemeConfig`, `ThemeBuilder`, `ThemeController`, `applyTheme`, and initialization helpers taking a configuration) have been removed. Move their colors and typography to CSS rules like those above, replace initialization with `setMode` (or `setMode (detectSystemPreference ())`), and use `toggleMode` for switching. This is a breaking API change.

### Dark Theme

<img src="./resources/dark-theme.png"/>

### Light Theme

<img src="./resources/light-theme.png"/>

## Getting Started

### Development Setup

1. Open `weave.code-workspace`
2. Install the recommended extensions
3. Run the initialization script:

    ```bash
    ./build.sh init     # Linux/macOS
    ./build.cmd init    # Windows
    ```

4. Start the docs development server from the repository root:

    ```bash
    yarn dev:docs
    ```

    Open the URL printed by Vite (normally `http://localhost:5173`). The command builds the site, then starts the Sass and .NET watchers and Vite together. Sass changes update the styles; F# changes rebuild WebSharper output and reload the page, which may reset page state. Press Ctrl+C to stop them all. This workflow serves the client-side docs; use the ASP.NET host below if server-side functionality is needed.

    Alternatively, build the styles and start the documentation site with ASP.NET:

    ```bash
    yarn build:css
    dotnet run --project src/Weave.Docs/Weave.Docs.fsproj
    ```

    Navigate to the URL printed by ASP.NET to view the documentation and interactive examples.

## Testing

### Without Docker

**Dependencies:** .NET SDK 10.0.403 or a newer .NET 10 feature band, Node.js 22.12+, and PowerShell (`pwsh`) for the Playwright installer. After running the initialization step above, build the E2E project and install Chromium:

```bash
dotnet build tests/Weave.Tests.E2E/Weave.Tests.E2E.fsproj
pwsh tests/Weave.Tests.E2E/bin/Debug/net10.0/playwright.ps1 install chromium
```

Then run the tests:

```bash
./build.sh RunTests    # Linux/macOS
./build.cmd RunTests   # Windows
```

### With Docker

**Dependencies:** Docker

```bash
docker compose build
docker compose run --rm playwright-tests
```

The Docker path runs the normal test target, including unit, rendering, and Chromium E2E tests, in a pre-configured browser environment. CI uses the same image but runs non-browser tests separately from its Chromium, Firefox, and WebKit E2E matrix.

To run the full browser interaction suite across Chromium, Firefox and WebKit locally, install the remaining Playwright browsers using the installer generated above, then run the dedicated build target:

```powershell
pwsh tests/Weave.Tests.E2E/bin/Debug/net10.0/playwright.ps1 install chromium firefox webkit
./build.sh RunBrowserTests    # Linux/macOS
./build.cmd RunBrowserTests   # Windows
```

The normal `RunTests` target still uses Chromium by default. The all-browser target runs mobile-emulation tests in Chromium and WebKit; Firefox runs the other interactions because Playwright does not document Firefox mobile emulation as supported. The CI browser matrix runs these same suites in parallel.


## Contributing

Community interest will shape the future of Weave.

- Open issues to report bugs or suggest features
- Submit pull requests
- Share design ideas
- Discuss on the F# Discord

Even early feedback is valuable at this stage.

## License

Weave is licensed under the [Mozilla Public License 2.0](LICENSE).
