namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open Weave
open Weave.CssHelpers

[<JavaScript>]
module Helpers =

  let bodyText (text: string) = Body1.Create(text)

  let divider () =
    Divider.Create(attrs = [ Margin.toClasses Margin.Vertical.small |> cls ])

  let section title description content =
    div [ Margin.toClasses Margin.Bottom.small |> cls ] [
      H4.Create(View.Const title, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

      div [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ] [ description ]

      div [
        Padding.toClasses Padding.All.small |> cls
        SurfaceColor.toBackgroundColor SurfaceColor.Paper
        BorderRadius.toClass BorderRadius.All.small |> cl
      ] [ content ]
    ]
