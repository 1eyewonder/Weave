namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

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

  [<Struct>]
  type GutterSpacing = private Spacing of int

  module GutterSpacing =
    let create spacing =
      if spacing < 0 then Spacing 0
      elif spacing > 20 then Spacing 20
      else Spacing spacing

    let value (Spacing s) = s

    let toClass (Spacing size) = sprintf "grid--spacing-%i" size

  [<Struct>]
  type Width = private Width of int

  module Width =

    let create spacing =
      if spacing < 1 then Width 1
      elif spacing > 12 then Width 12
      else Width spacing

    let value (Width s) = s

    let toClass breakpoint (Width width) =
      sprintf "grid__item--%s-%i" (Breakpoint.toString breakpoint) width

open Grid

[<JavaScript>]
type Grid =

  static member Create
    (items: Doc list, ?spacing: GutterSpacing, ?justify: JustifyContent, ?attrs: Attr list)
    =
    let spacing = defaultArg spacing (GutterSpacing.create 5)
    let attrs = defaultArg attrs List.empty
    let justify = defaultArg justify JustifyContent.SpaceAround

    let gridClasses = [ Css.grid; GutterSpacing.toClass spacing; JustifyContent.toClass justify ]

    div [ yield! gridClasses |> List.map cl; yield! attrs ] items

[<JavaScript>]
type GridItem =

  static member Create
    (content: Doc, ?xs: Width, ?sm: Width, ?md: Width, ?lg: Width, ?xl: Width, ?attrs: Attr list)
    =
    let attrs = defaultArg attrs List.empty

    let itemClasses = [
      cl Css.grid__item
      xs |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.ExtraSmall >> cl)
      sm |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.Small >> cl)
      md |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.Medium >> cl)
      lg |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.Large >> cl)
      xl |> Option.mapOrDefault Attr.Empty (Width.toClass Breakpoint.ExtraLarge >> cl)
    ]

    div [ yield! itemClasses; yield! attrs ] [ content ]

[<JavaScript>]
type FlexBreak =

  static member Create() =
    div [ cl Css.``flex-break``; Attr.Style "aria-hidden" "true" ] []
