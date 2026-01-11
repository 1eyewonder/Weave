namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open WebSharper.JavaScript

[<JavaScript>]
module TooltipExamples =

  let grid content = Grid.Create(items = content) //, attrs = [ Padding.toClasses Padding.Vertical.large |> cls ])

  let private directionExamples () =
    let tooltipBtn displayText direction =
      GridItem.Create(
        content =
          Tooltip.Create(
            Button.Create(
              text displayText,
              onClick = (fun () -> ()),
              attrs = [
                cls [
                  Button.Variant.Outlined |> Button.Variant.toClass
                  Button.Color.toClass BrandColor.Primary
                  Button.Width.toClass Button.Width.Full |> Option.defaultValue ""
                ]
              ]
            ),
            Body1.Div(sprintf "Tooltip on %A" direction),
            direction = direction
          ),
        xs = Grid.Width.create 3
      )

    [
      tooltipBtn "Left" Tooltip.Direction.Left
      tooltipBtn "Top" Tooltip.Direction.Top
      tooltipBtn "Bottom" Tooltip.Direction.Bottom
      tooltipBtn "Right" Tooltip.Direction.Right
    ]
    |> grid
    |> Helpers.section
      "Directions"
      (Helpers.bodyText "Tooltips can be positioned in four directions relative to the target element")

  let private colorExamples () =
    [
      let tooltipButton displayText color =
        GridItem.Create(
          Tooltip.Create(
            Button.Create(
              text displayText,
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                Button.Color.toClass color |> cl
                Button.Width.toClass Button.Width.Full |> Option.mapOrDefault Attr.Empty cl
              ]
            ),
            Body1.Div(sprintf "%s tooltip" displayText),
            tooltipAttrs = [ Tooltip.Color.toClass color |> cl ]
          ),
          xs = Grid.Width.create 6,
          md = Grid.Width.create 1
        )

      tooltipButton "Primary" BrandColor.Primary
      tooltipButton "Secondary" BrandColor.Secondary
      tooltipButton "Tertiary" BrandColor.Tertiary
      tooltipButton "Success" BrandColor.Success
      tooltipButton "Error" BrandColor.Error
      tooltipButton "Warning" BrandColor.Warning
      tooltipButton "Info" BrandColor.Info
    ]
    |> grid
    |> Helpers.section
      "Colors"
      (Helpers.bodyText "Tooltips support all theme colors to match your design system")

  let private activationExamples () =
    [
      let gridItem content =
        GridItem.Create(
          div
            [
              cls [
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.toClass AlignItems.Center
              ]
            ]
            content
        )

      [
        Tooltip.Create(
          Button.Create(
            text "Hover Me",
            onClick = (fun () -> ()),
            attrs = [
              Button.Variant.Outlined |> Button.Variant.toClass |> cl
              Button.Color.toClass BrandColor.Primary |> cl
            ]
          ),
          Body1.Div("Appears on hover"),
          activationEvents = [ Tooltip.Activation.Hover ]
        )
        Caption.Div("Hover activation", attrs = [ Margin.toClasses Margin.Top.small |> cls ])
      ]
      |> gridItem

      [
        Tooltip.Create(
          Button.Create(
            text "Click Me",
            onClick = (fun () -> ()),
            attrs = [
              Button.Variant.Outlined |> Button.Variant.toClass |> cl
              Button.Color.toClass BrandColor.Secondary |> cl
            ]
          ),
          Body1.Div("Appears on click"),
          activationEvents = [ Tooltip.Activation.Click ]
        )
        Caption.Div("Click activation", attrs = [ Margin.toClasses Margin.Top.small |> cls ])
      ]
      |> gridItem

    // [
    //   Tooltip..Create(
    //     Button.Create(
    //       text "Hover or Focus",
    //       onClick = (fun () -> ()),
    //       attrs = [
    //         Button.Variant.Outlined |> Button.Variant.toClass |> cl
    //         Button.Color.toClass BrandColor.Tertiary |> cl
    //       ]
    //     ),
    //     text "Hover or tab to focus",
    //     activationEvents = [ Tooltip.Activation.Hover; Tooltip.Activation.Focus ]
    //   )
    //   Caption.Create("Hover or focus (default)", attrs = [ Margin.toClasses Margin.Top.small |> cls ])
    // ]
    // |> gridItem
    ]
    |> grid
    |> Helpers.section
      "Activation Events"
      (Helpers.bodyText "Tooltips can be triggered by different user interactions")

  let private arrowExamples () =
    div [
      Attr.Style "display" "flex"
      Attr.Style "justify-content" "center"
      Attr.Style "gap" "32px"
      Attr.Style "flex-wrap" "wrap"
      Attr.Style "padding" "32px"
    ] [
      div [ Attr.Style "text-align" "center" ] [
        Tooltip.Create(
          Button.Create(
            text "With Arrow",
            onClick = (fun () -> ()),
            attrs = [
              Button.Variant.Filled |> Button.Variant.toClass |> cl
              Button.Color.toClass BrandColor.Primary |> cl
            ]
          ),
          Body1.Div("I have an arrow pointing"),
          showArrow = true
        )
        Caption.Div("showArrow = true (default)", attrs = [ Margin.toClasses Margin.Top.small |> cls ])
      ]

      div [ Attr.Style "text-align" "center" ] [
        Tooltip.Create(
          Button.Create(
            text "Without Arrow",
            onClick = (fun () -> ()),
            attrs = [
              Button.Variant.Filled |> Button.Variant.toClass |> cl
              Button.Color.toClass BrandColor.Secondary |> cl
            ]
          ),
          Body1.Div("No arrow here"),
          showArrow = false
        )
        Caption.Div("showArrow = false", attrs = [ Margin.toClasses Margin.Top.small |> cls ])
      ]
    ]
    |> Helpers.section
      "Arrow Visibility"
      (Helpers.bodyText "Tooltips can optionally display an arrow pointing to the target")

  let private customContentExample () =
    div [
      Attr.Style "display" "flex"
      Attr.Style "justify-content" "center"
      Attr.Style "padding" "32px"
    ] [
      Tooltip.Create(
        Button.Create(
          text "Rich Content Tooltip",
          onClick = (fun () -> ()),
          attrs = [
            Button.Variant.Filled |> Button.Variant.toClass |> cl
            Button.Color.toClass BrandColor.Info |> cl
          ]
        ),
        div [ Attr.Style "padding" "4px" ] [
          Body2.Div(
            "Custom Tooltip",
            attrs = [
              Attr.Style "font-weight" "bold"
              Margin.toClasses Margin.Bottom.extraSmall |> cls
            ]
          )
          Caption.Div("You can use Doc elements for rich content")
        ]
      )
    ]
    |> Helpers.section
      "Custom Content"
      (Helpers.bodyText "Tooltips can display rich content using Doc instead of plain text")

  let private textTooltipExample () =
    div [ Padding.toClasses Padding.All.medium |> cls ] [
      Body1.Div("Hover over ", attrs = [ Attr.Style "display" "inline" ])

      Tooltip.Create(
        span [
          Attr.Style "color" "var(--palette-primary)"
          Attr.Style "text-decoration" "underline"
          Attr.Style "cursor" "help"
        ] [ text "this text" ],
        text "This is a tooltip on inline text"
      )

      Body1.Div(" to see a tooltip. You can also hover over ", attrs = [ Attr.Style "display" "inline" ])

      Tooltip.Create(
        span [
          Attr.Style "color" "var(--palette-secondary)"
          Attr.Style "font-weight" "bold"
          Attr.Style "cursor" "help"
        ] [ text "this other text" ],
        text "Another tooltip example",
        direction = Tooltip.Direction.Bottom,
        tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Secondary |> cl ]
      )

      Body1.Div(" for more information.", attrs = [ Attr.Style "display" "inline" ])
    ]
    |> Helpers.section
      "Text Tooltips"
      (Helpers.bodyText "Tooltips can be applied to inline text elements, not just buttons")

  let private iconTooltipExample () =
    div [
      Attr.Style "display" "flex"
      Attr.Style "justify-content" "center"
      Attr.Style "gap" "16px"
      Attr.Style "padding" "24px"
    ] [
      Tooltip.Create(
        div [
          Attr.Style "display" "inline-flex"
          Attr.Style "align-items" "center"
          Attr.Style "justify-content" "center"
          Attr.Style "width" "40px"
          Attr.Style "height" "40px"
          Attr.Style "border-radius" "50%"
          BrandColor.toBackgroundColor BrandColor.Primary
          Attr.Style "color" "white"
          Attr.Style "cursor" "pointer"
          Attr.Style "font-size" "20px"
        ] [ text "ℹ️" ],
        text "Information icon",
        tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Info |> cl ]
      )

      Tooltip.Create(
        div [
          Attr.Style "display" "inline-flex"
          Attr.Style "align-items" "center"
          Attr.Style "justify-content" "center"
          Attr.Style "width" "40px"
          Attr.Style "height" "40px"
          Attr.Style "border-radius" "50%"
          BrandColor.toBackgroundColor BrandColor.Success
          Attr.Style "color" "white"
          Attr.Style "cursor" "pointer"
          Attr.Style "font-size" "20px"
        ] [ text "✓" ],
        text "Success!",
        tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Success |> cl ]
      )

      Tooltip.Create(
        div [
          Attr.Style "display" "inline-flex"
          Attr.Style "align-items" "center"
          Attr.Style "justify-content" "center"
          Attr.Style "width" "40px"
          Attr.Style "height" "40px"
          Attr.Style "border-radius" "50%"
          BrandColor.toBackgroundColor BrandColor.Error
          Attr.Style "color" "white"
          Attr.Style "cursor" "pointer"
          Attr.Style "font-size" "20px"
        ] [ text "⚠" ],
        text "Warning: Be careful!",
        tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Error |> cl ]
      )
    ]
    |> Helpers.section "Icon Tooltips" (Helpers.bodyText "Tooltips work great with icon buttons and badges")

  let render () =
    Container.Create(
      div [] [
        H1.Div("Tooltip Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "Tooltips display informative text when users hover over, focus on, or tap an element.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        directionExamples ()
        Helpers.divider ()
        colorExamples ()
        Helpers.divider ()
        activationExamples ()
        Helpers.divider ()
        arrowExamples ()
        Helpers.divider ()
        customContentExample ()
        Helpers.divider ()
        textTooltipExample ()
        Helpers.divider ()
        iconTooltipExample ()
      ]
    )
