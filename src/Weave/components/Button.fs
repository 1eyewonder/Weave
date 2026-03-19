namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
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

[<JavaScript; RequireQualifiedAccess>]
type Button =

  static member create(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =

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

  static member primary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.primary :: defaultArg attrs []
    )

  static member secondary
    (innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list)
    =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.secondary :: defaultArg attrs []
    )

  static member tertiary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.tertiary :: defaultArg attrs []
    )

  static member error(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.error :: defaultArg attrs []
    )

  static member warning(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.warning :: defaultArg attrs []
    )

  static member success(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.success :: defaultArg attrs []
    )

  static member info(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.info :: defaultArg attrs []
    )

[<JavaScript; RequireQualifiedAccess>]
type IconButton =

  static member create(icon: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =

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

  static member primary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.primary :: defaultArg attrs []
    )

  static member secondary
    (innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list)
    =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.secondary :: defaultArg attrs []
    )

  static member tertiary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.tertiary :: defaultArg attrs []
    )

  static member error(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.error :: defaultArg attrs []
    )

  static member warning(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.warning :: defaultArg attrs []
    )

  static member success(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.success :: defaultArg attrs []
    )

  static member info(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.info :: defaultArg attrs []
    )
