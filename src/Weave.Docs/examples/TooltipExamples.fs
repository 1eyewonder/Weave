namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module TooltipExamples =

  let grid content = Grid.Create(items = content)

  let private directionExamples () =
    let description =
      Helpers.bodyText "Tooltips can be positioned in four directions relative to the target element"

    let tooltipBtn direction =
      let displayText = sprintf "%A" direction

      GridItem.Create(
        content =
          Tooltip.Create(
            innerContent =
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
            tooltipContent = Body1.Div(sprintf "Tooltip on %A" direction),
            direction = direction
          ),
        xs = Grid.Width.create 3
      )

    let content =
      [
        tooltipBtn Tooltip.Direction.Left
        tooltipBtn Tooltip.Direction.Top
        tooltipBtn Tooltip.Direction.Bottom
        tooltipBtn Tooltip.Direction.Right
      ]
      |> grid

    let code =
      """open Weave
open Weave.CssHelpers

let tooltipBtn direction =
    let displayText = sprintf "%A" direction

    Tooltip.Create(
        innerContent =
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
        tooltipContent = Body1.Div(sprintf "Tooltip on %A" direction),
        direction = direction // see here
    )

tooltipBtn Tooltip.Direction.Left
tooltipBtn Tooltip.Direction.Top
tooltipBtn Tooltip.Direction.Bottom
tooltipBtn Tooltip.Direction.Right
    """

    Helpers.codeSampleSection "Directions" description content code

  let private colorExamples () =
    let description =
      Helpers.bodyText "Tooltips support all theme colors to match your design system"

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          FlexDirection.Row.allSizes
          FlexWrap.Wrap.allSizes
          JustifyContent.toClass JustifyContent.Center
        ]
      ] [
        let tooltipButton color =
          let displayText = sprintf "%A" color

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
            tooltipAttrs = [ Tooltip.Color.toClass color |> cl ],
            wrapperAttrs = [ cls [ yield! Margin.toClasses Margin.All.extraSmall ] ]
          )

        tooltipButton BrandColor.Primary
        tooltipButton BrandColor.Secondary
        tooltipButton BrandColor.Tertiary
        tooltipButton BrandColor.Success
        tooltipButton BrandColor.Error
        tooltipButton BrandColor.Warning
        tooltipButton BrandColor.Info
      ]

    let code =
      """open Weave
open Weave.CssHelpers

let tooltipButton color =
    let displayText = sprintf "%A" color

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
        tooltipAttrs = [ Tooltip.Color.toClass color |> cl ] // see here
    )

tooltipButton BrandColor.Primary
tooltipButton BrandColor.Secondary
tooltipButton BrandColor.Tertiary
tooltipButton BrandColor.Success
tooltipButton BrandColor.Error
tooltipButton BrandColor.Warning
tooltipButton BrandColor.Info
    """

    Helpers.codeSampleSection "Colors" description content code

  let private activationExamples () =
    let description =
      Helpers.bodyText "Tooltips can be triggered by different user interactions"

    let content =
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
      ]
      |> grid

    let code =
      """open Weave
open Weave.CssHelpers

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
    activationEvents = [ Tooltip.Activation.Hover ] // see here
)

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
    activationEvents = [ Tooltip.Activation.Click ] // see here
)
    """

    Helpers.codeSampleSection "Activation Events" description content code

  let private arrowExamples () =
    let description =
      Helpers.bodyText "Tooltips can optionally display an arrow pointing to the target"

    let content =
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

    let code =
      """open Weave
open Weave.CssHelpers

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
    showArrow = true // see here (default)
)

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
    showArrow = false // see here
)
    """

    Helpers.codeSampleSection "Arrow Visibility" description content code

  let private customContentExample () =
    let description =
      Helpers.bodyText "Tooltips can display rich content using Doc instead of plain text"

    let content =
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

    let code =
      """open Weave
open Weave.CssHelpers

Tooltip.Create(
    Button.Create(
        text "Rich Content Tooltip",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.Filled |> Button.Variant.toClass |> cl
            Button.Color.toClass BrandColor.Info |> cl
        ]
    ),
    div [ Attr.Style "padding" "4px" ] [ // see here - custom Doc content
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
    """

    Helpers.codeSampleSection "Custom Content" description content code

  let private textTooltipExample () =
    let description =
      Helpers.bodyText "Tooltips can be applied to inline text elements, not just buttons"

    let content =
      div [] [
        Body1.Span("Hover over ")

        Tooltip.Create(
          Body1.Span(
            text "this text",
            attrs = [
              Typography.Color.toClass BrandColor.Primary |> cl
              Attr.Style "text-decoration" "underline"
            ]
          ),
          Body1.Span(text "This is a tooltip on inline text")
        )

        Body1.Span(" to see a tooltip. You can also hover over ")

        Tooltip.Create(
          Body1.Span(
            text "this other text",
            attrs = [
              Typography.Color.toClass BrandColor.Secondary |> cl
              Attr.Style "text-decoration" "underline"
            ]
          ),
          Body1.Span(text "Another tooltip example"),
          direction = Tooltip.Direction.Bottom,
          tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Secondary |> cl ]
        )

        Body1.Span(" for more information.")
      ]

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

Body1.Span("Hover over ")

// see here
Tooltip.Create(
    Body1.Span(
        text "this text",
        attrs = [
            Typography.Color.toClass BrandColor.Primary |> cl
            Attr.Style "text-decoration" "underline"
        ]
    ),
    Body1.Span(text "This is a tooltip on inline text")
)

Body1.Span(" to see a tooltip. You can also hover over ")

// see here
Tooltip.Create(
    Body1.Span(
        text "this other text",
        attrs = [
            Typography.Color.toClass BrandColor.Secondary |> cl
            Attr.Style "text-decoration" "underline"
        ]
    ),
    Body1.Span(text "Another tooltip example"),
    direction = Tooltip.Direction.Bottom,
    tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Secondary |> cl ]
)

Body1.Span(" for more information.")"""

    Helpers.codeSampleSection "Text Tooltips" description content code

  let private iconTooltipExample () =
    let description =
      Helpers.bodyText "Tooltips work great with icon buttons and badges"

    let content =
      div [
        cls [
          Flex.Flex.allSizes
          FlexDirection.Row.allSizes
          JustifyContent.toClass JustifyContent.Center
        ]
      ] [
        Tooltip.Create(
          Icon.Create(
            Icon.UiActions UiActions.CheckCircle,
            attrs = [
              BrandColor.toColor BrandColor.Success
              Attr.Style "font-size" "32px"
              cls [ yield! Margin.toClasses Margin.All.extraSmall ]
            ]
          ),
          Body1.Span(text "Success!"),
          tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Success |> cl ]
        )

        Tooltip.Create(
          Icon.Create(
            Icon.Action Action.Warning,
            attrs = [
              BrandColor.toColor BrandColor.Warning
              Attr.Style "font-size" "32px"
              cls [ yield! Margin.toClasses Margin.All.extraSmall ]
            ]
          ),
          Body1.Span(text "Warning: Be careful!"),
          tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Warning |> cl ]
        )

        Tooltip.Create(
          Icon.Create(
            Icon.Action Action.Error,
            attrs = [
              BrandColor.toColor BrandColor.Error
              Attr.Style "font-size" "32px"
              cls [ yield! Margin.toClasses Margin.All.extraSmall ]
            ]
          ),
          Body1.Span(text "Error!!!!"),
          tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Error |> cl ]
        )
      ]

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

Tooltip.Create(
    Icon.Create(
        Icon.UiActions UiActions.CheckCircle,
        attrs = [
            BrandColor.toColor BrandColor.Success
            Attr.Style "font-size" "32px"
            cls [ yield! Margin.toClasses Margin.All.extraSmall ]
        ]
    ),
    Body1.Span(text "Success!"),
    tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Success |> cl ]
)

Tooltip.Create(
    Icon.Create(
        Icon.Action Action.Warning,
        attrs = [
            BrandColor.toColor BrandColor.Warning
            Attr.Style "font-size" "32px"
            cls [ yield! Margin.toClasses Margin.All.extraSmall ]
        ]
    ),
    Body1.Span(text "Warning: Be careful!"),
    tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Warning |> cl ]
)

Tooltip.Create(
    Icon.Create(
        Icon.Action Action.Error,
        attrs = [
            BrandColor.toColor BrandColor.Error
            Attr.Style "font-size" "32px"
            cls [ yield! Margin.toClasses Margin.All.extraSmall ]
        ]
    ),
    Body1.Span(text "Error!!!!"),
    tooltipAttrs = [ Tooltip.Color.toClass BrandColor.Error |> cl ]
)"""

    Helpers.codeSampleSection "Icon Tooltips" description content code

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
