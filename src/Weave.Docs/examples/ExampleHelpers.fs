namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open Weave
open Weave.CssHelpers

[<JavaScript>]
module Helpers =

  let bodyText (text: string) = Body1.Div(text)

  let divider () =
    Divider.Create(attrs = [ Margin.toClasses Margin.Vertical.small |> cls ])

  let section title description content =
    div [ Margin.toClasses Margin.Bottom.small |> cls ] [
      div [ Margin.toClasses Margin.Bottom.extraSmall |> cls ] [ H4.Div(View.Const title) ]

      div [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ] [ description ]

      div [
        Padding.toClasses Padding.All.small |> cls
        SurfaceColor.toBackgroundColor SurfaceColor.Surface
        BorderRadius.toClass BorderRadius.All.small |> cl
      ] [ content ]
    ]
