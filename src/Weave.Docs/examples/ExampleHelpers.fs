namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open Weave
open Weave.CssHelpers

[<JavaScript>]
module Helpers =

  let divider () =
    Divider.Create(attrs = [ Margin.toClasses Margin.Vertical.medium |> cls ])

  let section title description content =
    div [ Margin.toClasses Margin.Bottom.small |> cls ] [
      H3.Create(View.Const title, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

      div [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ] [ description ]

      div [
        Padding.toClasses Padding.All.small |> cls
        SurfaceColor.toAttr SurfaceColor.Paper
        BorderRadius.toClass BorderRadius.All.small |> cl
      ] [ content ]
    ]
