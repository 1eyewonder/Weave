namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Button =

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | Filled
    | Outlined
    | Text

  module Variant =

    let toClass variant =
      match variant with
      | Variant.Filled -> Css.``weave-button--filled``
      | Variant.Outlined -> Css.``weave-button--outlined``
      | Variant.Text -> Css.``weave-button--text``

  [<RequireQualifiedAccess; Struct>]
  type Width =
    | Full
    | Auto

  module Width =

    let toClass variant =
      match variant with
      | Width.Full -> Some Css.``weave-button--full-width``
      | Width.Auto -> None

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-button--primary``
      | BrandColor.Secondary -> Css.``weave-button--secondary``
      | BrandColor.Tertiary -> Css.``weave-button--tertiary``
      | BrandColor.Error -> Css.``weave-button--error``
      | BrandColor.Warning -> Css.``weave-button--warning``
      | BrandColor.Success -> Css.``weave-button--success``
      | BrandColor.Info -> Css.``weave-button--info``

open Button

[<JavaScript>]
type Button =

  static member Create(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    let content =
      Typography.Button.Div(
        innerContents,
        textWrap = View.Const false,
        attrs = [ cls [ Css.``weave-button__label``; Flex.Inline.allSizes ] ]
      )

    button [
      attr.``type`` "button"
      cl Css.``weave-button``

      Width.toClass Width.Auto |> Option.map cl |> Option.defaultValue Attr.Empty

      yield! attrs

      Disabled.disabledClass Css.``weave-button--disabled`` enabled
      on.clickTapViewGuarded enabled onClick
    ] [ content ]

  static member CreateIcon(icon: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    button [
      attr.``type`` "button"
      cl Css.``weave-button``
      cl Css.``weave-button--icon``

      yield! attrs

      Disabled.disabledClass Css.``weave-button--disabled`` enabled
      on.clickTapViewGuarded enabled onClick
    ] [ icon ]
