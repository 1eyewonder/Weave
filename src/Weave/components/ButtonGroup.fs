namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module ButtonGroup =

  module Variant =

    let filled = cl Css.``weave-button-group--filled``
    let outlined = cl Css.``weave-button-group--outlined``
    let text = cl Css.``weave-button-group--text``

  module Orientation =

    let vertical = cl Css.``weave-button-group--vertical``

  module Density =

    let compact = cl Css.``weave-button-group--compact``
    let standard = cl Css.``weave-button-group--standard``
    let spacious = cl Css.``weave-button-group--spacious``

  module Color =

    let primary = cl Css.``weave-button-group--primary``
    let secondary = cl Css.``weave-button-group--secondary``
    let tertiary = cl Css.``weave-button-group--tertiary``
    let error = cl Css.``weave-button-group--error``
    let warning = cl Css.``weave-button-group--warning``
    let success = cl Css.``weave-button-group--success``
    let info = cl Css.``weave-button-group--info``

[<JavaScript; RequireQualifiedAccess>]
type ButtonGroup =

  /// <summary>
  /// Creates a button group that visually groups child elements (buttons, icon buttons, button menus, dropdowns).
  /// Pass variant, orientation, density, and color classes via attrs.
  /// </summary>
  static member create(items: Doc list, ?attrs: Attr list) =

    let attrs = defaultArg attrs []

    let itemDocs =
      items
      |> List.map (fun item -> div [ cl Css.``weave-button-group__item`` ] [ item ])

    div [ cl Css.``weave-button-group``; yield! attrs ] itemDocs
