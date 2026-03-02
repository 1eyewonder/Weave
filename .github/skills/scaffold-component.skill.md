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
- Class constructors are used to simplify the API for users so the library doesn't have difficulty with naming and discoverability for the end users. In order to simplify these constructors, it is suggested styling props are passed in as `?attrs: Attr list` instead of individual optional parameters (e.g. `?variant`, `?color`, etc.). This also gives users more flexibility to combine multiple styles (e.g. `Variant.Filled` + `Size.Small` + `BrandColor.Primary`) without the library needing to define every possible combination as an explicit parameter.
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
- File name is always `_{name}.scss` (underscore prefix, all lowercase).
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

### 6b. Update `src/Weave.Docs/ExamplesRouter.fs` in four places:

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

## Checklist

Before marking the component as done, verify:

- [ ] `src/Weave/components/{Name}.fs` created with correct namespace, opens, DU types, `toClass` functions, and `Create` static method
- [ ] `src/Weave/scss/components/_{name}.scss` created with BEM classes, theme variables, `$palette-colors` loop, disabled/hover/focus states
- [ ] `src/Weave/scss/main.scss` has `@import "components/{name}";` in alphabetical order
- [ ] `src/Weave/Weave.fsproj` has `<Compile Include="components/{Name}.fs" />`
- [ ] `src/Weave.Docs/examples/{Name}Examples.fs` created with at least one example section and a `render ()` function
- [ ] `src/Weave.Docs/Weave.Docs.fsproj` has `<Compile Include="examples/{Name}Examples.fs" />` before `ExamplesRouter.fs`
- [ ] `ExamplesRouter.fs` updated in all four locations: `Page` DU, `pageToString`, `renderPage`, nav list
