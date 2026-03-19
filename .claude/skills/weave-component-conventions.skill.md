# Weave F# Component Conventions

When creating or modifying F# components in this Weave project, follow these conventions unless otherwise justified. These guidelines help maintain consistency, readability, and ease of use across our component library.

## Comments

Comments should be used when necessary to clarify non-obvious implementation details, but should not be used to explain what the code is doing at a high level. The code itself should be as self-explanatory as possible through clear naming and structure. We should not be using comments to create "sections" within the code.

## create Function Parameters

**Static member functions use camelCase** (e.g., `create`, `primary`, `secondary`), not PascalCase.

**Minimize explicit styling parameters.** Do not add parameters for things like `variant`, `color`, `size`, `width`, `alignment`, or other visual/styling concerns. These should be passed by the caller via the `attrs` parameter instead.

**Good** — styling comes through attrs:

```fsharp
static member create(value: Var<bool>, ?enabled: View<bool>, ?attrs: Attr list) =
```

**Bad** — explicit styling params pollute the signature:

```fsharp
static member create(value: Var<bool>, ?size: Size, ?color: BrandColor, ?enabled: View<bool>, ?attrs: Attr list) =
```

The exception is structural/behavioral parameters which don't get placed into a single `attr` list but rather other internal components. If we start seeing a use to place multiple attrs into an inner component, we can consider adding a second `?innerAttrs` parameter (but better named) for that purpose.

## Reactive Parameters

**Most parameters should be reactive** — prefer `Var<'T>` or `View<'T>` over plain values so components can update without full re-renders.

- Use `Var<'T>` when the caller owns and mutates the value (e.g., `isChecked: Var<bool>`, `activeIndex: Var<int>`)
- Use `View<'T>` for read-only reactive inputs (e.g., `enabled: View<bool>`, `displayText: View<string>`)
- Use plain values only for things that are truly static and structural

## CSS Class Helpers

Use `cl` for a single class. Use `cls` with a list of strings for two or more classes. **Never chain multiple `cl` calls** where `cls` would work.

**Good:**

```fsharp
cl Css.``weave-button``                               // single class

cls [
  Css.``weave-checkbox``
  Flex.Inline.allSizes
  AlignItems.toClass AlignItems.Center
]                                                      // multiple classes
```

**Bad:**

```fsharp
cl Css.``weave-checkbox``
cl Flex.Inline.allSizes
cl (AlignItems.toClass AlignItems.Center)              // should be cls + list
```

Both `cl` and `cls` are defined in `Weave.CssHelpers`:

- `cl = Attr.Class` — wraps a single string into a class `Attr`
- `cls = List.map Attr.Class >> Attr.Concat` — wraps a list of strings into a combined class `Attr`

## Standard Boilerplate

Every component file follows this structure:

```fsharp
namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module MyComponent =
  // Style modules with let bindings returning Attr (Variant, Color, etc.)
  ...

[<JavaScript; RequireQualifiedAccess>]
type MyComponent =


  // If there are any internal/private functions, should should instead use standard F# module functions here rather than static members on the type. This way we can utilize partial application and other functional patterns

  static member create(..., ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    ...
    div [
      cl Css.``weave-my-component``
      yield! attrs
    ] [ ... ]
```

Key points:

- `[<JavaScript>]` on every module and type
- `[<RequireQualifiedAccess>]` on both module and type — no `open MyComponent` needed
- Static member functions use **camelCase** (e.g., `create`, `primary`), not PascalCase
- `open Weave.CssHelpers` to bring `cl`, `cls`, `text`, `textView`, `div` etc. into scope
- `?attrs: Attr list` is always the last optional parameter and defaults to `[]`
- `yield! attrs` is placed after the component's own structural attrs so callers can override or extend
- When structurally different HTML is needed (e.g., icon-only vs text), use a separate type (e.g., `IconButton`) rather than a variant factory on the same type
