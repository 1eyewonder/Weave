---
name: Scaffold Weave Component
description: This prompt provides a step-by-step guide for scaffolding a new Weave component to help developers get started on new concepts.
---

# Weave Component Scaffolding

When asked to create a new Weave component. The component name used throughout this guide is the PascalCase name (e.g. `Chip`); the lowercase kebab form is used in SCSS and CSS class names (e.g. `chip`). When asked to create new components, please ask relevant questions about the design to ensure you and the engineer are aligned on the requirements and scope of the component.

---

## 1. F# Component File — `src/Weave/components/{Name}.fs`

Use this structure as an example, but not a silver bullet. Per-component styling types now use plain `let` bindings returning `Attr` directly — only include the style modules that are semantically meaningful for the component.

**Rules:**

- Styling props (variant, color, size, etc.) must not be individual optional parameters on `create` — pass them via `?attrs: Attr list` using the component's style modules (e.g., `MyComponent.Variant.filled`). See the **create Function Parameters** section of `weave-component-conventions.skill.md` for the full rule and examples.
- Per-component style modules (`Variant`, `Size`, etc.) contain plain `let` bindings that return `Attr` directly via the `cl` helper and type-provided CSS classes. DU types with `[<RequireQualifiedAccess; Struct>]` are no longer needed for these — the type provider enforces correctness at compile time.
- `Color` using `BrandColor` is conventional for any interactive or themeable component since `BrandColor` is a global type while the `Color` module is unique to the component. The `Color` module exposes both direct `let` bindings (e.g. `Color.primary`) and a `toAttr` function that maps `BrandColor` to the corresponding `Attr`.
- Per-component modules contain plain `let` bindings returning `Attr` directly (e.g. `module Variant` with `let filled = cl Css.weave-{name}--filled`). Both the module and type use `[<RequireQualifiedAccess>]` — there is no `open {Name}` line needed.
- Optional parameters (`?enabled`, `?attrs`, `?size`, etc.) always have a `defaultArg` binding at the top of `create`.
- Reactive state uses `View<'T>` for read-only props and `Var<'T>` for two-way bindings (e.g. `isChecked: Var<bool>`).
- Never use bare `attr.class`; always use `cl` (single class) or `cls [...]` (multiple) from `Weave.CssHelpers`. If you find yourself needing to use WebSharper's `Attr.Style "name" "value"`, consider whether you need to use a supported CSS helper or even create a new one in `Core.fs`.
- `Attr.DynamicClassPred` gates a modifier class on a `View<bool>`.
- `Attr.bindOption` unwraps an `Option<Attr>` (used when a `toClass` returns `Option`).
- `Size` (Small/Medium/Large) is conventional for inputs and controls but not always necessary.
- Make sure to check existing components for patterns and conventions that you can follow. Sometimes when creating new components, using existing components nested inside can help maintain visual consistency and reduce the amount of new CSS you need to write. For example, a `Card` component might use `Container` for its layout, and a `Chip` might use `Typography` for its text.
- When a component has styling combinations callers will reach for constantly, add **shorthand static members** on the type (e.g. `{Name}.primary(...)`, `{Name}.secondary(...)`) that prepend the relevant attr(s) and delegate to `create`. This is optional — add them where the ergonomic gain is clear, not exhaustively for every combination.

```fsharp
namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module {Name} =

  module Variant =

    let filled = cl Css.``weave-{name}--filled``
    let outlined = cl Css.``weave-{name}--outlined``

  module Size =

    let small = cl Css.``weave-{name}--small``
    let medium = cl Css.``weave-{name}--medium``
    let large = cl Css.``weave-{name}--large``

  module Color =

    let primary = cl Css.``weave-{name}--primary``
    let secondary = cl Css.``weave-{name}--secondary``
    let tertiary = cl Css.``weave-{name}--tertiary``
    let error = cl Css.``weave-{name}--error``
    let warning = cl Css.``weave-{name}--warning``
    let success = cl Css.``weave-{name}--success``
    let info = cl Css.``weave-{name}--info``

    let toAttr color =
      match color with
      | BrandColor.Primary -> primary
      | BrandColor.Secondary -> secondary
      | BrandColor.Tertiary -> tertiary
      | BrandColor.Error -> error
      | BrandColor.Warning -> warning
      | BrandColor.Success -> success
      | BrandColor.Info -> info

[<JavaScript; RequireQualifiedAccess>]
type {Name} =

  static member create
    (
      innerContents: Doc,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs   = defaultArg attrs   List.empty

    div [
      cl Css.``weave-{name}``
      View.not enabled |> Attr.DynamicClassPred Css.``weave-{name}--disabled``
      yield! attrs
    ] [
      innerContents
    ]

  // Optional shorthand members for the most common color/styling combinations.
  // Each prepends the relevant attr and delegates to create.
  static member primary(innerContents: Doc, ?enabled: View<bool>, ?attrs: Attr list) =
    {Name}.create(
      innerContents,
      ?enabled = enabled,
      attrs = {Name}.Color.primary :: defaultArg attrs []
    )

  static member secondary(innerContents: Doc, ?enabled: View<bool>, ?attrs: Attr list) =
    {Name}.create(
      innerContents,
      ?enabled = enabled,
      attrs = {Name}.Color.secondary :: defaultArg attrs []
    )
```

---

## 2. SCSS File — `src/Weave/scss/components/_{name}.scss`

Follow BEM naming and use CSS custom properties from the theme system. Never hard-code colors — always reference `var(--palette-*)` or `var(--typography-*)` variables.

```scss
@import "../abstracts/variables";
@import "../core/themes";

.weave-{name} {
  display: inline-flex;
  align-items: center;
  position: relative;
  box-sizing: border-box;

  color: var(--palette-text-primary);
  background-color: transparent;
  border-radius: var(--default-button-borderradius);

  transition: background-color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms,
              box-shadow 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms;

  &.weave-{name}--disabled {
    color: var(--palette-action-disabled);
    cursor: default;
    pointer-events: none;
  }

  @media (hover: hover) and (pointer: fine) {
    &:hover {
      background-color: var(--palette-action-default-hover);
    }
  }
}

.weave-{name}--filled {
  // ...
}

.weave-{name}--outlined {
  border: 1px solid currentColor;
  // ...
}

.weave-{name}--small  { /* ... */ }
.weave-{name}--medium { /* ... */ }
.weave-{name}--large  { /* ... */ }

.weave-{name}--filled {
  @each $color in $palette-colors {
    &.weave-{name}--#{$color} {
      background-color: var(--palette-#{$color});
      color: var(--palette-#{$color}-contrast);
      --ripple-color: var(--palette-#{$color}-contrast);

      @media (hover: hover) and (pointer: fine) {
        &:hover {
          background-color: rgba(var(--palette-#{$color}-rgb), 0.85);
        }
      }
    }
  }
}

.weave-{name}__label {
  // ...
}
```

**Rules:**

- File name is always `_{name}.scss` (underscore prefix, all lowercase)
- Color modifier classes must use the `@each $color in $palette-colors` SCSS loop — never hard-code individual color rules.
- Disabled state, hover, focus-visible, and active states are mandatory for interactive components.
- Use `var(--palette-{color})`, `var(--palette-{color}-contrast)`, and `rgba(var(--palette-{color}-rgb), alpha)` for color values.
- Prefer `var(--default-*)` or `var(--typography-*)` custom properties over magic numbers.

---

## 3. Register SCSS in `src/Weave/scss/main.scss`

Add the import **inside the `// Components` block**, in alphabetical order relative to the other component imports:

```scss
// Components
@import "components/button";
// ... other components in alphabetical order ...
@import "components/{name}";     // <-- add here, alphabetically
// ...
```

---

## 4. Register the F# file in `src/Weave/Weave.fsproj`

Add a `<Compile>` entry inside the existing `<ItemGroup>`. F# compilation order matters — place the new component after any types it depends on (typically after `Core.fs`, `Typography.fs`, and any other components it composes). Most new components belong near the end:

```xml
<ItemGroup>
  <Compile Include="Helpers.fs" />
  <Compile Include="Theming.fs" />
  <Compile Include="components/Core.fs" />
  <Compile Include="components/Icons.fs" />
  <Compile Include="components/Typography.fs" />
  <Compile Include="components/Container.fs" />
  <!-- ... existing components ... -->
  <Compile Include="components/{Name}.fs" />   <!-- add last, or after any dependency -->
</ItemGroup>
```

---

## 5. Create the Example File — `src/Weave.Docs/examples/{Name}Examples.fs`

Each example function demonstrates one prop or concept. Use `Helpers.codeSampleSection` when you want to show the F# snippet that produces the example, and `Helpers.section` when the example needs no code sample.

```fsharp
namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers

[<JavaScript>]
module {Name}Examples =

  // One private function per concept/prop group
  let private variantExamples () =
    let description =
      Helpers.bodyText "Short description of what this example demonstrates."

    let content =
      Grid.create (
        [
          // Render each variant/case inside a GridItem
          GridItem.create (
            {Name}.create (
              text "Example",
              attrs = [
                {Name}.Variant.filled
                {Name}.Color.primary
              ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """{Name}.create(
    text "Example",
    attrs = [
        {Name}.Variant.filled
        {Name}.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Variants" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "{Name}"
        Body1.div (
          "Brief one-line description of when to use this component.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Helpers.divider ()
        variantExamples ()
        // Add Helpers.divider () between each example section
      ],
      attrs = [ Container.MaxWidth.large ]
    )
```

**Rules:**

- All example functions are `private`. Only `render ()` is public.
- Always wrap the top-level output in `Container.create(..., attrs = [ Container.MaxWidth.large ])` — `maxWidth` is **not** a named parameter; pass `Container.MaxWidth.*` via `?attrs`.
- Separate example sections with `Helpers.divider ()`.
- Use `Helpers.codeSampleSection` (renders a collapsible code block) for all prop demonstrations.
- Use `Helpers.section` (no code block) only for purely visual/layout demonstrations.
- The first section should demonstrate the most basic usage with no optional props.
- All Weave static member calls use **camelCase** (`Grid.create`, `GridItem.create`, `Button.create`, etc.) — never PascalCase `Create`.

---

## 6. Register the Example and Update the Router

### 6a. Add to `src/Weave.Docs/Weave.Docs.fsproj`

Add the new example file **before** `ExamplesRouter.fs` in the compile order:

```xml
<Compile Include="examples/{Name}Examples.fs" />
<Compile Include="ExamplesRouter.fs" />    <!-- this line already exists, add above it -->
```

### 6b. Update `src/Weave.Docs/ExamplesRouter.fs` in four places

**1 — Add to the `Page` DU:**

```fsharp
[<Struct>]
type Page =
  | Home
  // ... existing cases ...
  | {Name}Examples    // <-- add here
```

**2 — Add to `pageToString`:**

```fsharp
let private pageToString page =
  match page with
  // ... existing cases ...
  | {Name}Examples -> "{Name}"
```

**3 — Add to `renderPage`:**

```fsharp
let private renderPage page =
  match page with
  // ... existing cases ...
  | {Name}Examples -> {Name}Examples.render ()
```

**4 — Add to the nav item list inside `render ()`:**

```fsharp
yield!
  [
    Home
    ButtonExamples
    // ... existing entries ...
    {Name}Examples    // <-- add here
  ]
  |> List.map item
```

---

---

## 7. Unit Tests — `tests/Weave.Tests.Unit/{Name}Tests.fs`

Unit tests validate pure mapping functions like `Color.toAttr`. Components using plain `let` bindings (returning `Attr` directly from type-provided CSS classes) don't need unit tests -- the compiler enforces correctness. Use **Expecto** with `testList`/`testTheory`/`testCase`.

**Rules:**

- Module name is `Weave.Tests.Unit.{Name}Tests` (no `namespace`, just a top-level `module`).
- The root binding must be decorated with `[<Tests>]` so Expecto discovers it automatically — no manual registration needed.
- Only write unit tests when a mapping function like `Color.toAttr` exists. Plain `let` style bindings (returning `Attr` directly) need no tests — the type provider enforces correctness at compile time.
- Use `testTheory` with a list of `(BrandColor, expectedAttr)` tuples where the expected value is the corresponding module-level binding (e.g. `{Name}.Color.primary`).
- Never test `create` or DOM rendering in unit tests — that belongs in the rendering tests.

```fsharp
module Weave.Tests.Unit.{Name}Tests

open Expecto
open Weave

[<Tests>]
let {name}Tests =
  testList "{Name}" [

    testList "Color.toAttr" [
      testTheory "each BrandColor maps to the expected attr"
        [
          BrandColor.Primary,   {Name}.Color.primary
          BrandColor.Secondary, {Name}.Color.secondary
          BrandColor.Tertiary,  {Name}.Color.tertiary
          BrandColor.Error,     {Name}.Color.error
          BrandColor.Warning,   {Name}.Color.warning
          BrandColor.Success,   {Name}.Color.success
          BrandColor.Info,      {Name}.Color.info
        ]
      <| fun (color, expected) ->
        Expect.equal ({Name}.Color.toAttr color) expected ""
    ]
  ]
```

### 7a. Register in `tests/Weave.Tests.Unit/Weave.Tests.Unit.fsproj`

Add a `<Compile>` entry in alphabetical order relative to the other test files:

```xml
<ItemGroup>
  <!-- ... existing entries ... -->
  <Compile Include="{Name}Tests.fs" />
</ItemGroup>
```

---

## 8. Rendering Tests — `tests/Weave.Tests.Rendering/{Name}LayoutTests.fs`

Rendering tests validate visual layout and CSS behaviour using **Playwright** (xUnit). They load a static HTML fixture in a headless browser and assert on bounding boxes, computed styles, and DOM state.

**Rules:**

- Class name is `{Name}LayoutTests` inheriting `PageTest()` from `Microsoft.Playwright.Xunit`.
- Always call `this.LoadFixture()` first in every `[<Fact>]` test.
- Give every meaningful element in the fixture an `id` attribute so tests can locate them with `this.Page.Locator("#id")`.
- Use `BoundingBoxAsync()` to assert positional relationships (left/right, above/below, width/height).
- Use `Page.EvaluateAsync<string>` to check computed styles (e.g. `display`, `visibility`) when bounding boxes are unavailable (e.g. hidden elements return `null`).
- Allow ±1px tolerance in floating-point comparisons (`abs (a - b) <= 1.0f`).
- Tests should be coarse-grained and layout-focused — avoid pixel-perfect assertions.

```fsharp
module Weave.Tests.Rendering.{Name}LayoutTests

open Microsoft.Playwright.Xunit
open Xunit
open System.IO
open System.Reflection

type {Name}LayoutTests() =
  inherit PageTest()

  member private _.FixturePath =
    let assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

    let fixtureDir =
      Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "fixtures"))

    Path.Combine(fixtureDir, "{name}.html")

  member this.LoadFixture() = task {
    do! this.Page.SetViewportSizeAsync(1280, 800)
    let! _ = this.Page.GotoAsync($"file://%s{this.FixturePath}")
    ()
  }

  [<Fact>]
  member this.``describe the layout expectation``() = task {
    do! this.LoadFixture()
    let! elementA = this.Page.Locator("#element-a").BoundingBoxAsync()
    let! elementB = this.Page.Locator("#element-b").BoundingBoxAsync()

    Assert.True(elementA.X < elementB.X, $"Element A (x={elementA.X}) should be left of B (x={elementB.X})")
  }

  [<Fact>]
  member this.``hidden element is not displayed``() = task {
    do! this.LoadFixture()

    let! display =
      this.Page.EvaluateAsync<string>("() => getComputedStyle(document.querySelector('#hidden-el')).display")

    Assert.Equal("none", display)
  }
```

### 8a. Create the HTML fixture — `tests/Weave.Tests.Rendering/fixtures/{name}.html`

The fixture is a hand-written static HTML file using the component's BEM classes directly. It does **not** run WebSharper — it simply exercises the compiled CSS.

**Rules:**

- Link to the compiled stylesheet with `<link rel="stylesheet" href="../../../src/Weave/styles.css" />`.
- Apply `margin: 0; padding: 16px; box-sizing: border-box;` on `body` to give a predictable layout origin.
- Assign `id` attributes to every element that a test will locate.
- Use explicit `style="display: block;"` / `style="display: none;"` on panels/content that are statically active or hidden.
- Cover enough states to make at least one meaningful layout assertion per important structural relationship (e.g. tab order, panel visibility, alignment).

```html
<!doctype html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>{Name} Fixture</title>
    <link rel="stylesheet" href="../../../src/Weave/styles.css" />
    <style>
      body {
        margin: 0;
        padding: 16px;
        box-sizing: border-box;
      }
    </style>
  </head>
  <body>
    <!-- Example: render the component with BEM classes and id attributes -->
    <div id="{name}-root" class="weave-{name} weave-{name}--filled weave-{name}--primary">
      <div id="{name}-content">Content</div>
    </div>
  </body>
</html>
```

### 8b. Register in `tests/Weave.Tests.Rendering/Weave.Tests.Rendering.fsproj`

```xml
<ItemGroup>
  <!-- ... existing entries ... -->
  <Compile Include="{Name}LayoutTests.fs" />
</ItemGroup>
```

---

## 9. E2E Accessibility Tests — `tests/Weave.Tests.E2E/accessibility/{Name}Tests.fs`

E2E tests validate keyboard navigation and accessibility in a live WebSharper-rendered page using **Playwright** (xUnit) and **axe-core**.

**Rules:**

- Inherit from `E2ETestBase(server)` (in `TestBase.fs`), which provides `NavigateTo`, `RunAxeScan`, and `Expect` helpers.
- Every component must have at least a `passes axe-core accessibility scan` test.
- **Use Playwright's typed Locator assertions instead of raw `EvaluateAsync` JavaScript strings wherever possible.** This gives compile-time safety on the F# side and catches selector/assertion errors earlier. Only fall back to `EvaluateAsync` when there is no typed API (e.g. DOM containment checks, injecting custom event listeners).
- Use `this.Expect(locator)` (defined on `E2ETestBase`) to access Playwright assertions: `ToBeFocusedAsync()`, `ToBeCheckedAsync()`, `ToHaveAttributeAsync()`, `ToHaveClassAsync(Regex)`, `ToHaveCountAsync()`, `ToBeVisibleAsync()`, `ToBeHiddenAsync()`.
- Playwright assertions auto-retry until the condition is met (or timeout), so they replace both `WaitForFunctionAsync` + `EvaluateAsync` in most cases.
- When WebSharper's reactive DOM update needs time to propagate, use auto-retrying assertions (e.g. `Expect(panel).ToHaveClassAsync(Regex("expanded"))`) rather than `WaitForFunctionAsync` with JS strings.
- Mark tests for known unimplemented features with `[<Fact(Skip = "Known gap: ...")>]` so they serve as a roadmap.

```fsharp
namespace Weave.Tests.E2E

open System.Text.RegularExpressions
open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type {Name}Tests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("{name}")

  [<Fact>]
  member this.``element is focusable``() = task {
    do! this.NavigateTo("{name}")
    let element = this.Page.Locator(".weave-{name}").First
    do! element.FocusAsync()
    // Typed assertion — no raw JS needed
    do! this.Expect(element).ToBeFocusedAsync()
  }

  [<Fact>]
  member this.``keyboard interaction works``() = task {
    do! this.NavigateTo("{name}")
    let element = this.Page.Locator(".weave-{name}").First
    do! element.FocusAsync()
    do! element.PressAsync("Enter")
    // Auto-retrying assertion — handles async reactive DOM updates
    do! this.Expect(element).ToHaveClassAsync(Regex("weave-{name}--active"))
  }
```

### 9a. Register the E2E test page in `tests/Weave.Tests.E2E.Site/Pages.fs`

`Pages.fs` is a **separate project** (`Weave.Tests.E2E.Site`) that the E2E test runner hosts as a live WebSharper SPA. It must stay in sync with the core library API — **always build it explicitly** after making any changes here:

```bash
dotnet build tests/Weave.Tests.E2E.Site/Weave.Tests.E2E.Site.fsproj
```

Add a page function and register it in `renderPage`. Use the same camelCase API as everywhere else — **never PascalCase `Create`** in this file:

```fsharp
let private {name}Page () =
  div [] [
    {Name}.create (...)
  ]

// In renderPage:
| "{name}" -> {name}Page ()
```

**Common API pitfalls in Pages.fs** (these caused 47 build errors when missed after an API migration):
- `Var.Create` / `Var.Create<T>` stays PascalCase — it is from WebSharper, not Weave
- Styling attrs go via `?attrs`, never as named parameters (e.g. `position`, `underline`, `maxWidth` are removed; use `attrs = [ AppBar.Position.sticky ]`, `attrs = [ Link.Underline.always ]`, etc.)
- `Body1.div`, `Body2.div`, `H6.div` etc. are camelCase in Weave's Typography module
- `Color.toAttr BrandColor.X` replaces the old `AlertColor.toClass (AlertColor.BrandColor X) |> Attr.bindOption cl` pattern

### 9b. Register in `tests/Weave.Tests.E2E/Weave.Tests.E2E.fsproj`

```xml
<ItemGroup>
  <!-- ... existing entries ... -->
  <Compile Include="accessibility/{Name}Tests.fs" />
</ItemGroup>
```

---

## Checklist

Before marking the component as done, verify:

- [ ] `src/Weave/components/{Name}.fs` created with correct namespace, opens, style modules with `let` bindings returning `Attr`, and `create` static method (camelCase)
- [ ] `src/Weave/scss/components/_{name}.scss` created with BEM classes, theme variables, `$palette-colors` loop, disabled/hover/focus states
- [ ] `src/Weave/scss/main.scss` has `@import "components/{name}";` in alphabetical order
- [ ] `src/Weave/Weave.fsproj` has `<Compile Include="components/{Name}.fs" />`
- [ ] `src/Weave.Docs/examples/{Name}Examples.fs` created with at least one example section and a `render ()` function
- [ ] `src/Weave.Docs/Weave.Docs.fsproj` has `<Compile Include="examples/{Name}Examples.fs" />` before `ExamplesRouter.fs`
- [ ] `ExamplesRouter.fs` updated in all four locations: `Page` DU, `pageToString`, `renderPage`, nav list
- [ ] `tests/Weave.Tests.Unit/{Name}Tests.fs` created **only if** a mapping function like `Color.toAttr` is present — plain `let` style bindings need no unit tests since the type provider guarantees correctness
- [ ] `tests/Weave.Tests.Unit/Weave.Tests.Unit.fsproj` has `<Compile Include="{Name}Tests.fs" />`
- [ ] `tests/Weave.Tests.Rendering/{Name}LayoutTests.fs` created with at least one layout assertion per key structural relationship
- [ ] `tests/Weave.Tests.Rendering/fixtures/{name}.html` created with BEM markup, `id` attributes, and stylesheet link
- [ ] `tests/Weave.Tests.Rendering/Weave.Tests.Rendering.fsproj` has `<Compile Include="{Name}LayoutTests.fs" />`
- [ ] `tests/Weave.Tests.E2E/accessibility/{Name}Tests.fs` created with axe-core scan and keyboard/focus tests using typed Playwright assertions
- [ ] `tests/Weave.Tests.E2E.Site/Pages.fs` has page function and `renderPage` entry for the component (camelCase API only — no PascalCase `Create`)
- [ ] `tests/Weave.Tests.E2E/Weave.Tests.E2E.fsproj` has `<Compile Include="accessibility/{Name}Tests.fs" />`
- [ ] `dotnet build tests/Weave.Tests.E2E.Site/Weave.Tests.E2E.Site.fsproj` passes with 0 errors (this project is compiled separately — CI won't catch it otherwise)
- [ ] `dotnet build src/Weave.Docs/Weave.Docs.fsproj` passes with 0 errors
- [ ] Run `dotnet fantomas .` in the root of the repo to format all files
