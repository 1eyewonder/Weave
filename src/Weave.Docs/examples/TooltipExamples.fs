namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module TooltipExamples =

  let grid content = Grid.create (items = content)

  let private directionExamples () =
    let description =
      Helpers.bodyText "Tooltips can be positioned in four directions relative to the target element"

    let tooltipBtn direction =
      let displayText = sprintf "%A" direction

      GridItem.create (
        content =
          Tooltip.create (
            innerContent =
              Button.primary (
                text displayText,
                onClick = (fun () -> ()),
                attrs = [ Button.Variant.outlined; Button.Width.full ]
              ),
            tooltipContent = Body1.div (sprintf "Tooltip on %A" direction),
            direction = direction
          ),
        xs = Grid.Width.create 12,
        sm = Grid.Width.create 6,
        md = Grid.Width.create 3
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


let tooltipBtn direction =
    let displayText = sprintf "%A" direction

    Tooltip.create(
        innerContent =
          Button.primary(
              text displayText,
              onClick = (fun () -> ()),
              attrs = [
                  Button.Variant.outlined
                  Button.Width.full
              ]
          ),
        tooltipContent = Body1.div(sprintf "Tooltip on %A" direction),
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
        Flex.Flex.allSizes
        FlexDirection.Row.allSizes
        FlexWrap.Wrap.allSizes
        JustifyContent.center
      ] [
        let colors = [
          "Primary", Button.Color.primary, Tooltip.Color.primary
          "Secondary", Button.Color.secondary, Tooltip.Color.secondary
          "Tertiary", Button.Color.tertiary, Tooltip.Color.tertiary
          "Success", Button.Color.success, Tooltip.Color.success
          "Error", Button.Color.error, Tooltip.Color.error
          "Warning", Button.Color.warning, Tooltip.Color.warning
          "Info", Button.Color.info, Tooltip.Color.info
        ]

        for (displayText, btnColor, tipColor) in colors do
          Tooltip.create (
            Button.create (
              text displayText,
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.filled; btnColor; Button.Width.full ]
            ),
            Body1.div (sprintf "%s tooltip" displayText),
            tooltipAttrs = [ tipColor ],
            wrapperAttrs = [ Margin.All.extraSmall ]
          )
      ]

    let code =
      """open Weave


let colors = [
    "Primary", Button.Color.primary, Tooltip.Color.primary
    "Secondary", Button.Color.secondary, Tooltip.Color.secondary
    "Tertiary", Button.Color.tertiary, Tooltip.Color.tertiary
    "Success", Button.Color.success, Tooltip.Color.success
    "Error", Button.Color.error, Tooltip.Color.error
    "Warning", Button.Color.warning, Tooltip.Color.warning
    "Info", Button.Color.info, Tooltip.Color.info
]

for (displayText, btnColor, tipColor) in colors do
    Tooltip.create(
        Button.create(
            text displayText,
            onClick = (fun () -> ()),
            attrs = [
                Button.Variant.filled
                btnColor // see here
                Button.Width.full
            ]
        ),
        Body1.div(sprintf "%s tooltip" displayText),
        tooltipAttrs = [ tipColor ] // see here
    )
    """

    Helpers.codeSampleSection "Colors" description content code

  let private activationExamples () =
    let description =
      Helpers.bodyText "Tooltips can be triggered by different user interactions"

    let content =
      [
        let gridItem content =
          GridItem.create (
            div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] content
          )

        [
          Tooltip.create (
            Button.primary (text "Hover Me", onClick = (fun () -> ()), attrs = [ Button.Variant.outlined ]),
            Body1.div ("Appears on hover"),
            activationEvents = [ Tooltip.Activation.Hover ]
          )
          Caption.div ("Hover activation", attrs = [ Margin.Top.small ])
        ]
        |> gridItem

        [
          Tooltip.create (
            Button.secondary (text "Click Me", onClick = (fun () -> ()), attrs = [ Button.Variant.outlined ]),
            Body1.div ("Appears on click"),
            activationEvents = [ Tooltip.Activation.Click ]
          )
          Caption.div ("Click activation", attrs = [ Margin.Top.small ])
        ]
        |> gridItem
      ]
      |> grid

    let code =
      """open Weave


Tooltip.create(
    Button.primary(
        text "Hover Me",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.outlined
        ]
    ),
    Body1.div("Appears on hover"),
    activationEvents = [ Tooltip.Activation.Hover ] // see here
)

Tooltip.create(
    Button.secondary(
        text "Click Me",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.outlined
        ]
    ),
    Body1.div("Appears on click"),
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
          Tooltip.create (
            Button.primary (text "With Arrow", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ]),
            Body1.div ("I have an arrow pointing"),
            showArrow = true
          )
          Caption.div ("showArrow = true (default)", attrs = [ Margin.Top.small ])
        ]

        div [ Attr.Style "text-align" "center" ] [
          Tooltip.create (
            Button.secondary (
              text "Without Arrow",
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.filled ]
            ),
            Body1.div ("No arrow here"),
            showArrow = false
          )
          Caption.div ("showArrow = false", attrs = [ Margin.Top.small ])
        ]
      ]

    let code =
      """open Weave


Tooltip.create(
    Button.primary(
        text "With Arrow",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.filled
        ]
    ),
    Body1.div("I have an arrow pointing"),
    showArrow = true // see here (default)
)

Tooltip.create(
    Button.secondary(
        text "Without Arrow",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.filled
        ]
    ),
    Body1.div("No arrow here"),
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
        Tooltip.create (
          Button.info (
            text "Rich Content Tooltip",
            onClick = (fun () -> ()),
            attrs = [ Button.Variant.filled ]
          ),
          div [ Attr.Style "padding" "4px" ] [
            Body2.div (
              "Custom Tooltip",
              attrs = [ Attr.Style "font-weight" "bold"; Margin.Bottom.extraSmall ]
            )
            Caption.div ("You can use Doc elements for rich content")
          ]
        )
      ]

    let code =
      """open Weave


Tooltip.create(
    Button.info(
        text "Rich Content Tooltip",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.filled
        ]
    ),
    div [ Attr.Style "padding" "4px" ] [ // see here - custom Doc content
        Body2.div(
            "Custom Tooltip",
            attrs = [
                Attr.Style "font-weight" "bold"
                Margin.Bottom.extraSmall
            ]
        )
        Caption.div("You can use Doc elements for rich content")
    ]
)
    """

    Helpers.codeSampleSection "Custom Content" description content code

  let private textTooltipExample () =
    let description =
      Helpers.bodyText "Tooltips can be applied to inline text elements, not just buttons"

    let content =
      div [] [
        Body1.span ("Hover over ")

        Tooltip.create (
          Body1.span (
            text "this text",
            attrs = [ Typography.Color.primary; Attr.Style "text-decoration" "underline" ]
          ),
          Body1.span (text "This is a tooltip on inline text")
        )

        Body1.span (" to see a tooltip. You can also hover over ")

        Tooltip.create (
          Body1.span (
            text "this other text",
            attrs = [ Typography.Color.secondary; Attr.Style "text-decoration" "underline" ]
          ),
          Body1.span (text "Another tooltip example"),
          direction = Tooltip.Direction.Bottom,
          tooltipAttrs = [ Tooltip.Color.secondary ]
        )

        Body1.span (" for more information.")
      ]

    let code =
      """open Weave

open WebSharper.UI

Body1.span("Hover over ")

// see here
Tooltip.create(
    Body1.span(
        text "this text",
        attrs = [
            Typography.Color.primary
            Attr.Style "text-decoration" "underline"
        ]
    ),
    Body1.span(text "This is a tooltip on inline text")
)

Body1.span(" to see a tooltip. You can also hover over ")

// see here
Tooltip.create(
    Body1.span(
        text "this other text",
        attrs = [
            Typography.Color.secondary
            Attr.Style "text-decoration" "underline"
        ]
    ),
    Body1.span(text "Another tooltip example"),
    direction = Tooltip.Direction.Bottom,
    tooltipAttrs = [ Tooltip.Color.secondary ]
)

Body1.span(" for more information.")"""

    Helpers.codeSampleSection "Text Tooltips" description content code

  let private iconTooltipExample () =
    let description =
      Helpers.bodyText "Tooltips work great with icon buttons and badges"

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Row.allSizes; JustifyContent.center ] [
        Tooltip.create (
          Icon.create (
            Icon.UiActions UiActions.CheckCircle,
            attrs = [
              BrandColor.toColor BrandColor.Success
              Attr.Style "font-size" "32px"
              Margin.All.extraSmall
            ]
          ),
          Body1.span (text "Success!"),
          tooltipAttrs = [ Tooltip.Color.success ]
        )

        Tooltip.create (
          Icon.create (
            Icon.Action Action.Warning,
            attrs = [
              BrandColor.toColor BrandColor.Warning
              Attr.Style "font-size" "32px"
              Margin.All.extraSmall
            ]
          ),
          Body1.span (text "Warning: Be careful!"),
          tooltipAttrs = [ Tooltip.Color.warning ]
        )

        Tooltip.create (
          Icon.create (
            Icon.Action Action.Error,
            attrs = [
              BrandColor.toColor BrandColor.Error
              Attr.Style "font-size" "32px"
              Margin.All.extraSmall
            ]
          ),
          Body1.span (text "Error!!!!"),
          tooltipAttrs = [ Tooltip.Color.error ]
        )
      ]

    let code =
      """open Weave

open WebSharper.UI

Tooltip.create(
    Icon.create(
        Icon.UiActions UiActions.CheckCircle,
        attrs = [
            BrandColor.toColor BrandColor.Success
            Attr.Style "font-size" "32px"
            Margin.All.extraSmall
        ]
    ),
    Body1.span(text "Success!"),
    tooltipAttrs = [ Tooltip.Color.success ]
)

Tooltip.create(
    Icon.create(
        Icon.Action Action.Warning,
        attrs = [
            BrandColor.toColor BrandColor.Warning
            Attr.Style "font-size" "32px"
            Margin.All.extraSmall
        ]
    ),
    Body1.span(text "Warning: Be careful!"),
    tooltipAttrs = [ Tooltip.Color.warning ]
)

Tooltip.create(
    Icon.create(
        Icon.Action Action.Error,
        attrs = [
            BrandColor.toColor BrandColor.Error
            Attr.Style "font-size" "32px"
            Margin.All.extraSmall
        ]
    ),
    Body1.span(text "Error!!!!"),
    tooltipAttrs = [ Tooltip.Color.error ]
)"""

    Helpers.codeSampleSection "Icon Tooltips" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Tooltip"
        Body1.div (
          "Tooltips display informative text when users hover over, focus on, or tap an element.",
          attrs = [ Margin.Bottom.extraSmall ]
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
      ],
      attrs = [ Container.MaxWidth.large ]
    )
