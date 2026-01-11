namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.JavaScript
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

  [<Inline "window.highlightCodeElement($0)">]
  let highlightCodeElement (el: Dom.Element) = X<unit>

  let codeSampleSection title description (content: Doc) (linesOfCode: string) =
    div [ Margin.toClasses Margin.Bottom.small |> cls ] [
      div [ Margin.toClasses Margin.Bottom.extraSmall |> cls ] [ H4.Div(View.Const title) ]

      div [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ] [ description ]

      div [
        SurfaceColor.toBackgroundColor SurfaceColor.Surface

        cls [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          yield! Padding.toClasses Padding.All.small
          BorderRadius.toClass BorderRadius.All.small
        ]

      ] [
        content

        let codeIsExpanded = Var.Create false

        let header =
          ExpansionPanelHeader.CreateWithDefaultIcons(
            content = Subtitle2.Div("Show Code"),
            expanded = codeIsExpanded,
            attrs = [ cls [ ExpansionPanel.Color.toColor BrandColor.Primary ] ]
          )

        let codeContent =
          pre [] [
            code [
              SurfaceColor.toBackgroundColor SurfaceColor.Background
              Attr.Class "language-fsharp"
              on.afterRender highlightCodeElement
            ] [ text linesOfCode ]
          ]

        ExpansionPanelContainer.Create(
          [
            ExpansionPanel.Create(
              header = header,
              content = ExpansionPanelContent.Create(codeContent, gutters = View.Const false),
              expanded = codeIsExpanded
            )
          ]
        )
      ]
    ]
