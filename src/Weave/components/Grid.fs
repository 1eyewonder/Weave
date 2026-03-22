namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Grid =

  [<RequireQualifiedAccess; Struct>]
  type Breakpoint =
    | ExtraSmall
    | Small
    | Medium
    | Large
    | ExtraLarge

  module Breakpoint =

    let toString breakpoint =
      match breakpoint with
      | Breakpoint.ExtraSmall -> "xs"
      | Breakpoint.Small -> "sm"
      | Breakpoint.Medium -> "md"
      | Breakpoint.Large -> "lg"
      | Breakpoint.ExtraLarge -> "xl"

  module Spacing =

    let none = cl Css.``weave-grid--spacing-0``
    let extraSmall = cl Css.``weave-grid--spacing-2``
    let small = cl Css.``weave-grid--spacing-4``
    let medium = cl Css.``weave-grid--spacing-8``
    let large = cl Css.``weave-grid--spacing-12``
    let extraLarge = cl Css.``weave-grid--spacing-20``

  [<Struct>]
  type Width = private Width of int

  module Width =

    let create spacing =
      if spacing < 1 then Width 1
      elif spacing > 12 then Width 12
      else Width spacing

    let value (Width s) = s

    let toClass breakpoint (Width width) =
      sprintf "weave-grid__item--%s-%i" (Breakpoint.toString breakpoint) width

open Grid

[<JavaScript>]
type Grid =

  static member create(items: Doc list, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty

    div [ cl Css.``weave-grid``; yield! attrs ] items

[<JavaScript>]
type GridItem =

  static member create
    (content: Doc, ?xs: Width, ?sm: Width, ?md: Width, ?lg: Width, ?xl: Width, ?attrs: Attr list)
    =
    let attrs = defaultArg attrs List.empty

    let itemClasses = [
      cl Css.``weave-grid__item``
      xs |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.ExtraSmall >> cl)
      sm |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.Small >> cl)
      md |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.Medium >> cl)
      lg |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.Large >> cl)
      xl |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.ExtraLarge >> cl)
    ]

    div [ yield! itemClasses; yield! attrs ] [ content ]

[<JavaScript>]
type FlexBreak =

  static member create() =
    div [ cl Css.``weave-flex-break``; Attr.Create "aria-hidden" "true" ] []
