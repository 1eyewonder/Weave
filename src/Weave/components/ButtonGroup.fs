namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module ButtonGroup =

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | Filled
    | Outlined
    | Text

  module Variant =

    let toClass variant =
      match variant with
      | Variant.Filled -> Css.``weave-button-group--filled``
      | Variant.Outlined -> Css.``weave-button-group--outlined``
      | Variant.Text -> Css.``weave-button-group--text``

  [<RequireQualifiedAccess; Struct>]
  type Orientation =
    | Horizontal
    | Vertical

  module Orientation =

    let toClass orientation =
      match orientation with
      | Orientation.Horizontal -> None
      | Orientation.Vertical -> Some Css.``weave-button-group--vertical``

  [<RequireQualifiedAccess; Struct>]
  type Size =
    | Small
    | Medium
    | Large

  module Size =

    let toClass size =
      match size with
      | Size.Small -> Css.``weave-button-group--small``
      | Size.Medium -> Css.``weave-button-group--medium``
      | Size.Large -> Css.``weave-button-group--large``

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-button-group--primary``
      | BrandColor.Secondary -> Css.``weave-button-group--secondary``
      | BrandColor.Tertiary -> Css.``weave-button-group--tertiary``
      | BrandColor.Error -> Css.``weave-button-group--error``
      | BrandColor.Warning -> Css.``weave-button-group--warning``
      | BrandColor.Success -> Css.``weave-button-group--success``
      | BrandColor.Info -> Css.``weave-button-group--info``

open ButtonGroup

[<JavaScript>]
type ButtonGroup =

  /// <summary>
  /// Creates a button group that visually groups child elements (buttons, icon buttons, button menus, dropdowns).
  /// Pass variant, orientation, size, and color classes via attrs.
  /// </summary>
  static member Create(items: Doc list, ?attrs: Attr list) =

    let attrs = defaultArg attrs []

    let itemDocs =
      items
      |> List.map (fun item -> div [ cl Css.``weave-button-group__item`` ] [ item ])

    div [ cl Css.``weave-button-group``; yield! attrs ] itemDocs
