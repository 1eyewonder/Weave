---
name: Scaffold Weave Component
description: This prompt provides a step-by-step guide for scaffolding a new Weave component to help developers get started on new concepts.
---

# Weave Component Scaffolding

When asked to create a new Weave component. The component name used throughout this guide is the PascalCase name (e.g. `Chip`); the lowercase kebab form is used in SCSS and CSS class names (e.g. `chip`). When asked to create new components, please ask relevant questions about the design to ensure you and the engineer are aligned on the requirements and scope of the component.

---

## 1. F# Component File — `src/Weave/components/{Name}.fs`

Use this structure as an example, but not a silver bullet. Only include the DU sub-modules that are semantically meaningful for the component.

**Rules:**

- Styling props (variant, color, size, etc.) must not be individual optional parameters on `Create` — pass them via `?attrs: Attr list` using the component's `toClass` helpers. See the **Create Function Parameters** section of `weave-component-conventions.skill.md` for the full rule and examples.
- DU types carrying variants/sizes/etc. should have `[<RequireQualifiedAccess; Struct>]` due to the fact their union cases likely share names with other components. This also prevents users from having to deal with name collisions and having to open modules in various orders to get the right cases in scope.
- `Color` using `BrandColor` is conventional for any interactive or themeable component since `BrandColor` is a global type while the `Color` module is unique to the component.
- All `toClass` functions live inside a companion sub-module of the same name (e.g. `module Variant`, `module Size`, `module Color`).
- The `open {Name}` after the module exposes the DU cases into scope for the `type {Name}` static class below it.
- Optional parameters (`?enabled`, `?attrs`, `?size`, etc.) always have a `defaultArg` binding at the top of `Create`.
- Reactive state uses `View<'T>` for read-only props and `Var<'T>` for two-way bindings (e.g. `isChecked: Var<bool>`).
- Never use bare `attr.class`; always use `cl` (single class) or `cls [...]` (multiple) from `Weave.CssHelpers`. If you find yourself needing to use WebSharper's `Attr.Style "name" "value"`, consider whether you need to use a supported CSS helper or even create a new one in `Core.fs`.
- `Attr.DynamicClassPred` gates a modifier class on a `View<bool>`.
- `Attr.bindOption` unwraps an `Option<Attr>` (used when a `toClass` returns `Option`).
- `Size` (Small/Medium/Large) is conventional for inputs and controls but not always necessary.
- Make sure to check existing components for patterns and conventions that you can follow. Sometimes when creating new components, using existing components nested inside can help maintain visual consistency and reduce the amount of new CSS you need to write. For example, a `Card` component might use `Container` for its layout, and a `Chip` might use `Typography` for its text.

```fsharp
namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module {Name} =

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | Filled
    | Outlined

  module Variant =

    let toClass variant =
      match variant with
      | Variant.Filled   -> Css.``weave-{name}--filled``
      | Variant.Outlined -> Css.``weave-{name}--outlined``

  [<RequireQualifiedAccess; Struct>]
  type Size =
    | Small
    | Medium
    | Large

  module Size =

    let toClass size =
      match size with
      | Size.Small  -> Css.``weave-{name}--small``
      | Size.Medium -> Css.``weave-{name}--medium``
      | Size.Large  -> Css.``weave-{name}--large``

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary   -> Css.``weave-{name}--primary``
      | BrandColor.Secondary -> Css.``weave-{name}--secondary``
      | BrandColor.Tertiary  -> Css.``weave-{name}--tertiary``
      | BrandColor.Error     -> Css.``weave-{name}--error``
      | BrandColor.Warning   -> Css.``weave-{name}--warning``
      | BrandColor.Success   -> Css.``weave-{name}--success``
      | BrandColor.Info      -> Css.``weave-{name}--info``

open {Name}

[<JavaScript>]
type {Name} =

  static member Create
    (
      // required parameters come first, then optional ones prefixed with ?
      innerContents: Doc,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs   = defaultArg attrs   List.empty

    // Build your element here using WebSharper.UI.Html primitives.
    // Always apply the root BEM class as the first class, then spread ?attrs last
    // so callers can override or extend with Variant/Color/Size toClass results.
    div [
      cl Css.``weave-{name}``
      View.not enabled |> Attr.DynamicClassPred Css.``weave-{name}--disabled``
      yield! attrs
    ] [
      innerContents
    ]
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
      Grid.Create(
        [
          // Render each variant/case inside a GridItem
          GridItem.Create(
            {Name}.Create(
              text "Example",
              attrs = [
                {Name}.Variant.toClass {Name}.Variant.Filled |> cl
                BrandColor.Primary |> {Name}.Color.toClass |> cl
              ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """{Name}.Create(
    text "Example",
    attrs = [
        {Name}.Variant.toClass {Name}.Variant.Filled |> cl
        BrandColor.Primary |> {Name}.Color.toClass |> cl
    ]
)"""

    Helpers.codeSampleSection "Variants" description content code

  let render () =
    Container.Create(
      div [] [
        H1.Div("{Name}", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Helpers.bodyText "Brief one-line description of when to use this component."
        Helpers.divider ()
        variantExamples ()
        // Add Helpers.divider () between each example section
      ],
      maxWidth = Container.MaxWidth.Large
    )
```

**Rules:**

- All example functions are `private`. Only `render ()` is public.
- Always wrap the top-level output in `Container.Create(..., maxWidth = Container.MaxWidth.Large)`.
- Separate example sections with `Helpers.divider ()`.
- Use `Helpers.codeSampleSection` (renders a collapsible code block) for all prop demonstrations.
- Use `Helpers.section` (no code block) only for purely visual/layout demonstrations.
- The first section should demonstrate the most basic usage with no optional props.

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

Unit tests validate that every `toClass` function maps each DU case to the correct BEM class string and that all cases produce distinct classes. Use **Expecto** with `testList`/`testTheory`/`testCase`.

**Rules:**

- Module name is `Weave.Tests.Unit.{Name}Tests` (no `namespace`, just a top-level `module`).
- The root binding must be decorated with `[<Tests>]` so Expecto discovers it automatically — no manual registration needed.
- Write one `testList` per `toClass` (or other pure mapping) function.
- For exhaustive DU cases: use `testTheory` with a list of `(input, expectedClass)` tuples, then a separate `testCase` that checks all cases produce distinct strings via `List.distinct`.
- For `toClass` functions that return `Option<string>` (e.g. the default/no-modifier case returns `None`): use `Expect.isNone` and `Expect.equal ... (Some "...")` in individual `testCase` entries.
- Never test `Create` or DOM rendering in unit tests — that belongs in the rendering tests.

```fsharp
module Weave.Tests.Unit.{Name}Tests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let {name}Tests =
  testList "{Name}" [

    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        {Name}.Variant.Filled,   "weave-{name}--filled"
        {Name}.Variant.Outlined, "weave-{name}--outlined"
      ]
      <| fun (variant, expected) -> Expect.equal ({Name}.Variant.toClass variant) expected ""

      testCase "all variants produce distinct classes"
      <| fun () ->
        let classes =
          [ {Name}.Variant.Filled; {Name}.Variant.Outlined ]
          |> List.map {Name}.Variant.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each variant maps to a unique class"
    ]

    // Repeat the same pattern for Size.toClass, Color.toClass, etc.

    // For toClass functions that return Option<string>:
    testList "SomeProp.toClass" [
      testCase "Default returns None (no modifier class)"
      <| fun () -> Expect.isNone ({Name}.SomeProp.toClass {Name}.SomeProp.Default) ""
      testCase "Other returns Some weave-{name}--other"
      <| fun () ->
        Expect.equal ({Name}.SomeProp.toClass {Name}.SomeProp.Other) (Some "weave-{name}--other") ""
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

## Checklist

Before marking the component as done, verify:

- [ ] `src/Weave/components/{Name}.fs` created with correct namespace, opens, DU types, `toClass` functions, and `Create` static method
- [ ] `src/Weave/scss/components/_{name}.scss` created with BEM classes, theme variables, `$palette-colors` loop, disabled/hover/focus states
- [ ] `src/Weave/scss/main.scss` has `@import "components/{name}";` in alphabetical order
- [ ] `src/Weave/Weave.fsproj` has `<Compile Include="components/{Name}.fs" />`
- [ ] `src/Weave.Docs/examples/{Name}Examples.fs` created with at least one example section and a `render ()` function
- [ ] `src/Weave.Docs/Weave.Docs.fsproj` has `<Compile Include="examples/{Name}Examples.fs" />` before `ExamplesRouter.fs`
- [ ] `ExamplesRouter.fs` updated in all four locations: `Page` DU, `pageToString`, `renderPage`, nav list
- [ ] `tests/Weave.Tests.Unit/{Name}Tests.fs` created with `testTheory` coverage for every `toClass` function and distinct-class assertions
- [ ] `tests/Weave.Tests.Unit/Weave.Tests.Unit.fsproj` has `<Compile Include="{Name}Tests.fs" />`
- [ ] `tests/Weave.Tests.Rendering/{Name}LayoutTests.fs` created with at least one layout assertion per key structural relationship
- [ ] `tests/Weave.Tests.Rendering/fixtures/{name}.html` created with BEM markup, `id` attributes, and stylesheet link
- [ ] `tests/Weave.Tests.Rendering/Weave.Tests.Rendering.fsproj` has `<Compile Include="{Name}LayoutTests.fs" />`
- [ ] Run `dotnet fantomas .` in the root of the repo to format all files
