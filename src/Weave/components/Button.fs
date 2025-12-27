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

  [<RequireQualifiedAccess; Struct>]
  type Size =
    | Small
    | Medium
    | Large

  module Size =

    let toClass size =
      match size with
      | Size.Small -> Css.``weave-button--small``
      | Size.Medium -> ""
      | Size.Large -> Css.``weave-button--large``

  [<RequireQualifiedAccess; Struct>]
  type IconPosition =
    | Start
    | End

open Button

[<JavaScript>]
type Button =

  static member Create
    (
      innerContents: Doc,
      onClick: unit -> unit,
      ?enabled: View<bool>,
      ?icon: Doc,
      ?iconPosition: IconPosition,
      ?attrs: Attr list
    ) =

    let enabled = defaultArg enabled (View.Const true)
    let iconPosition = defaultArg iconPosition IconPosition.Start
    let attrs = defaultArg attrs List.empty

    let content =
      match icon, iconPosition with
      | Some iconDoc, IconPosition.Start ->
        [
          div [ cls [ Css.``weave-button__icon--start`` ] ] [ iconDoc ]
          div [ cl Css.``weave-button__label`` ] [ innerContents ]
        ]
        |> Doc.Concat
      | Some iconDoc, IconPosition.End ->
        [
          div [ cl Css.``weave-button__label`` ] [ innerContents ]
          div [ cls [ Css.``weave-button__icon--end`` ] ] [ iconDoc ]
        ]
        |> Doc.Concat
      | None, _ -> div [ cl Css.``weave-button__label`` ] [ innerContents ]

    button [
      attr.``type`` "button"
      cl Css.``weave-button``

      Width.toClass Width.Auto |> Option.map cl |> Option.defaultValue Attr.Empty

      yield! attrs

      View.not enabled |> Attr.DynamicClassPred Css.``weave-button--disabled``

      on.clickView enabled (fun _ _ enabled ->
        if enabled then
          onClick ())
    ] [ content ]
