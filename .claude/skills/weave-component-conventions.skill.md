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

## Style Modules

Per-component style sub-modules (`Variant`, `Size`, `Color`, etc.) live inside the component's `module` and expose **plain `let` bindings that return `Attr` directly** via `cl`. There are no DU types for these — the TypedCssClasses type provider enforces correctness at compile time.

```fsharp
module Variant =
  let filled  = cl Css.``weave-mycomponent--filled``
  let outlined = cl Css.``weave-mycomponent--outlined``

module Size =
  let small  = cl Css.``weave-mycomponent--small``
  let medium = cl Css.``weave-mycomponent--medium``
  let large  = cl Css.``weave-mycomponent--large``
```

### Color module

`Color` is conventional for any interactive or themeable component. It exposes:

1. **Direct `let` bindings** for each brand color — for callers who know the color statically.
2. **`toAttr : BrandColor -> Attr`** — for callers who hold a `BrandColor` value at runtime (e.g., driven by a `Var` or config). Whether to include `toAttr` is situational; add it when programmatic color selection is likely.

```fsharp
module Color =
  let primary   = cl Css.``weave-mycomponent--primary``
  let secondary = cl Css.``weave-mycomponent--secondary``
  // ... other colors ...

  let toAttr color =
    match color with
    | BrandColor.Primary   -> primary
    | BrandColor.Secondary -> secondary
    // ...
```

## Shorthand Constructors

When a component has styling combinations callers will reach for constantly, add **shorthand static members** on the type. Each prepends the relevant attr(s) and delegates to `create`, keeping call sites concise:

```fsharp
static member primary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
  MyComponent.create(
    innerContents,
    onClick,
    ?enabled = enabled,
    attrs = MyComponent.Color.primary :: defaultArg attrs []
  )
```

Shorthand members are optional — add them where the ergonomic gain is clear (typically the most common color variants), not exhaustively for every possible combination.

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

  module Variant =
    let filled  = cl Css.``weave-mycomponent--filled``
    let outlined = cl Css.``weave-mycomponent--outlined``

  module Color =
    let primary   = cl Css.``weave-mycomponent--primary``
    let secondary = cl Css.``weave-mycomponent--secondary``
    // ...

    // Optional: include when callers may hold a BrandColor value at runtime
    let toAttr color =
      match color with
      | BrandColor.Primary   -> primary
      | BrandColor.Secondary -> secondary
      // ...

[<JavaScript; RequireQualifiedAccess>]
type MyComponent =

  // Internal helpers go here as plain module-level functions (not static members)
  // so partial application and functional composition remain available.

  static member create(..., ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    ...
    div [
      cl Css.``weave-my-component``
      yield! attrs
    ] [ ... ]

  // Optional shorthand for the most common styling combinations:
  static member primary(..., ?attrs: Attr list) =
    MyComponent.create(..., attrs = MyComponent.Color.primary :: defaultArg attrs [])
```

Key points:

- `[<JavaScript; RequireQualifiedAccess>]` on both the module and the type — no `open MyComponent` needed at call sites
- Static member functions use **camelCase** (e.g., `create`, `primary`), not PascalCase
- Style modules use plain `let` bindings returning `Attr` — no DU types, no `toClass` functions; the type provider guarantees correctness
- `open Weave.CssHelpers` to bring `cl`, `cls`, and HTML aliases into scope
- `?attrs: Attr list` is always the last optional parameter and defaults to `[]`
- `yield! attrs` is placed after the component's own structural attrs so callers can override or extend
- When structurally different HTML is needed (e.g., icon-only vs text), use a separate type (e.g., `IconButton`) rather than a variant factory on the same type

## Collection Item Pattern (`*Item` Types)

When a container component takes a list of configurable items (e.g., `ChipSet`, `Tabs`, `Select`, `WeaveList`), follow this pattern:

1. **Internal `*Def` record** inside the container's module — holds all per-item configuration fields
2. **Public `*Item` type** outside the module (after `open {Module}`, before the container type) — provides `static member create` with optional parameters, returns the `*Def` record
3. **No pipeline/builder modules** — never use `module *Def` with `with*` functions (e.g., `withAttrs`, `withDisabled`, `withContent`). All item configuration goes through optional parameters on `*Item.create`

When an item type has multiple structurally different variants (e.g., text tabs vs icon-only tabs vs custom-header tabs), use multiple `create*` static members on the same `*Item` type rather than separate types.

```fsharp
// Internal record
[<JavaScript>]
module ChipSet =
  type ChipDef = {
    Label: Doc; Value: string; Content: Doc option
    Closable: bool; Disabled: View<bool>; Attrs: Attr list
  }

open ChipSet

// Public item constructor
[<JavaScript>]
type ChipItem =
  static member create
    (label: Doc, value: string, ?content: Doc, ?closable: bool, ?disabled: View<bool>, ?attrs: Attr list)
    : ChipSet.ChipDef =
    {
      Label = label; Value = value; Content = content
      Closable = defaultArg closable false
      Disabled = defaultArg disabled (View.Const false)
      Attrs = defaultArg attrs []
    }

// Container still takes the internal record type
[<JavaScript>]
type ChipSet =
  static member create(chips: ChipDef list, ...) = ...
```

Canonical examples: `ListItem.create`, `ChipItem.create`, `TabItem.create` (with `createIconOnly`, `createCustom`), `SelectItem.create`
