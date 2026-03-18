namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Button =

  module Variant =

    let filled = cl Css.``weave-button--filled``
    let outlined = cl Css.``weave-button--outlined``
    let text = cl Css.``weave-button--text``

  module Width =

    let full = cl Css.``weave-button--full-width``

  module Color =

    let primary = cl Css.``weave-button--primary``
    let secondary = cl Css.``weave-button--secondary``
    let tertiary = cl Css.``weave-button--tertiary``
    let error = cl Css.``weave-button--error``
    let warning = cl Css.``weave-button--warning``
    let success = cl Css.``weave-button--success``
    let info = cl Css.``weave-button--info``

    let toAttr color =
      match color with
      | BrandColor.Primary -> primary
      | BrandColor.Secondary -> secondary
      | BrandColor.Tertiary -> tertiary
      | BrandColor.Error -> error
      | BrandColor.Warning -> warning
      | BrandColor.Success -> success
      | BrandColor.Info -> info

open Button

[<JavaScript>]
type Button =

  static member Create(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    let content =
      Typography.ButtonText.Div(
        innerContents,
        textWrap = View.Const false,
        attrs = [ cls [ Css.``weave-button__label``; Flex.Inline.allSizes ] ]
      )

    button [
      attr.``type`` "button"
      cl Css.``weave-button``

      yield! attrs

      Disabled.disabledClass Css.``weave-button--disabled`` enabled
      Disabled.nativeAttr enabled
      on.clickTapKeyViewGuarded enabled onClick
    ] [ content ]

  static member CreateIcon(icon: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    button [
      attr.``type`` "button"
      cls [ Css.``weave-button``; Css.``weave-button--icon`` ]

      yield! attrs

      Disabled.disabledClass Css.``weave-button--disabled`` enabled
      Disabled.nativeAttr enabled
      on.clickTapKeyViewGuarded enabled onClick
    ] [ icon ]
