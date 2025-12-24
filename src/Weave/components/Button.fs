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
      | Variant.Filled -> Css.``button--filled``
      | Variant.Outlined -> Css.``button--outlined``
      | Variant.Text -> Css.``button--text``

  [<RequireQualifiedAccess; Struct>]
  type Width =
    | Full
    | Auto

  module Width =

    let toClass variant =
      match variant with
      | Width.Full -> Some Css.``button--full-width``
      | Width.Auto -> None

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``button--primary``
      | BrandColor.Secondary -> Css.``button--secondary``
      | BrandColor.Tertiary -> Css.``button--tertiary``
      | BrandColor.Error -> Css.``button--error``
      | BrandColor.Warning -> Css.``button--warning``
      | BrandColor.Success -> Css.``button--success``
      | BrandColor.Info -> Css.``button--info``

  [<RequireQualifiedAccess; Struct>]
  type Size =
    | Small
    | Medium
    | Large

  module Size =

    let toClass size =
      match size with
      | Size.Small -> Css.``button--small``
      | Size.Medium -> ""
      | Size.Large -> Css.``button--large``

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
          div [ cls [ Css.``button__icon--start`` ] ] [ iconDoc ]
          div [ cl Css.``button__label`` ] [ innerContents ]
        ]
        |> Doc.Concat
      | Some iconDoc, IconPosition.End ->
        [
          div [ cl Css.``button__label`` ] [ innerContents ]
          div [ cls [ Css.``button__icon--end`` ] ] [ iconDoc ]
        ]
        |> Doc.Concat
      | None, _ -> div [ cl Css.``button__label`` ] [ innerContents ]

    button [
      attr.``type`` "button"
      cl Css.button

      Width.toClass Width.Auto |> Option.map cl |> Option.defaultValue Attr.Empty

      yield! attrs

      View.not enabled |> Attr.DynamicClassPred Css.``button--disabled``

      on.clickView enabled (fun _ _ enabled ->
        if enabled then
          onClick ())
    ] [ content ]
