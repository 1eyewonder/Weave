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
            tooltipContent = div [ Typography.body1 ] [ text (sprintf "Tooltip on %A" direction) ],
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
        tooltipContent = div [ Typography.body1 ] [ text (sprintf "Tooltip on %A" direction) ],
        direction = direction
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
            div [ Typography.body1 ] [ text (sprintf "%s tooltip" displayText) ],
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
                btnColor
                Button.Width.full
            ]
        ),
        div [ Typography.body1 ] [ text (sprintf "%s tooltip" displayText) ],
        tooltipAttrs = [ tipColor ]
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
            div [ Typography.body1 ] [ text "Appears on hover" ],
            activationEvents = [ Tooltip.Activation.Hover ]
          )
          div [ Typography.caption; Margin.Top.small ] [ text "Hover activation" ]
        ]
        |> gridItem

        [
          Tooltip.create (
            Button.secondary (text "Click Me", onClick = (fun () -> ()), attrs = [ Button.Variant.outlined ]),
            div [ Typography.body1 ] [ text "Appears on click" ],
            activationEvents = [ Tooltip.Activation.Click ]
          )
          div [ Typography.caption; Margin.Top.small ] [ text "Click activation" ]
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
    div [ Typography.body1 ] [ text "Appears on hover" ],
    activationEvents = [ Tooltip.Activation.Hover ]
)

Tooltip.create(
    Button.secondary(
        text "Click Me",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.outlined
        ]
    ),
    div [ Typography.body1 ] [ text "Appears on click" ],
    activationEvents = [ Tooltip.Activation.Click ]
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
            div [ Typography.body1 ] [ text "I have an arrow pointing" ],
            showArrow = true
          )
          div [ Typography.caption; Margin.Top.small ] [ text "showArrow = true (default)" ]
        ]

        div [ Attr.Style "text-align" "center" ] [
          Tooltip.create (
            Button.secondary (
              text "Without Arrow",
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.filled ]
            ),
            div [ Typography.body1 ] [ text "No arrow here" ],
            showArrow = false
          )
          div [ Typography.caption; Margin.Top.small ] [ text "showArrow = false" ]
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
    div [ Typography.body1 ] [ text "I have an arrow pointing" ],
    showArrow = true (default)
)

Tooltip.create(
    Button.secondary(
        text "Without Arrow",
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.filled
        ]
    ),
    div [ Typography.body1 ] [ text "No arrow here" ],
    showArrow = false
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
            div [ Typography.body2; Attr.Style "font-weight" "bold"; Margin.Bottom.extraSmall ] [
              text "Custom Tooltip"
            ]
            div [ Typography.caption ] [ text "You can use Doc elements for rich content" ]
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
    div [ Attr.Style "padding" "4px" ] [ - custom Doc content
        div [ Typography.body2
              Attr.Style "font-weight" "bold"
              Margin.Bottom.extraSmall ] [
            text "Custom Tooltip"
        ]
        div [ Typography.caption ] [ text "You can use Doc elements for rich content" ]
    ]
)
    """

    Helpers.codeSampleSection "Custom Content" description content code

  let private textTooltipExample () =
    let description =
      Helpers.bodyText "Tooltips can be applied to inline text elements, not just buttons"

    let content =
      div [] [
        span [ Typography.body1 ] [ text "Hover over " ]

        Tooltip.create (
          span [
            Typography.body1
            Typography.Color.primary
            Attr.Style "text-decoration" "underline"
          ] [ text "this text" ],
          span [ Typography.body1 ] [ text "This is a tooltip on inline text" ]
        )

        span [ Typography.body1 ] [ text " to see a tooltip. You can also hover over " ]

        Tooltip.create (
          span [
            Typography.body1
            Typography.Color.secondary
            Attr.Style "text-decoration" "underline"
          ] [ text "this other text" ],
          span [ Typography.body1 ] [ text "Another tooltip example" ],
          direction = Tooltip.Direction.Bottom,
          tooltipAttrs = [ Tooltip.Color.secondary ]
        )

        span [ Typography.body1 ] [ text " for more information." ]
      ]

    let code =
      """open Weave

open WebSharper.UI

span [ Typography.body1 ] [ text "Hover over " ]

Tooltip.create(
    span [
        Typography.body1
        Typography.Color.primary
        Attr.Style "text-decoration" "underline"
    ] [ text "this text" ],
    span [ Typography.body1 ] [ text "This is a tooltip on inline text" ]
)

span [ Typography.body1 ] [ text " to see a tooltip. You can also hover over " ]

Tooltip.create(
    span [
        Typography.body1
        Typography.Color.secondary
        Attr.Style "text-decoration" "underline"
    ] [ text "this other text" ],
    span [ Typography.body1 ] [ text "Another tooltip example" ],
    direction = Tooltip.Direction.Bottom,
    tooltipAttrs = [ Tooltip.Color.secondary ]
)

span [ Typography.body1 ] [ text " for more information." ]"""

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
          span [ Typography.body1 ] [ text "Success!" ],
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
          span [ Typography.body1 ] [ text "Warning: Be careful!" ],
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
          span [ Typography.body1 ] [ text "Error!!!!" ],
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
    span [ Typography.body1 ] [ text "Success!" ],
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
    span [ Typography.body1 ] [ text "Warning: Be careful!" ],
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
    span [ Typography.body1 ] [ text "Error!!!!" ],
    tooltipAttrs = [ Tooltip.Color.error ]
)"""

    Helpers.codeSampleSection "Icon Tooltips" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Tooltip"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Tooltips display informative text when users hover over, focus on, or tap an element."
        ]

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
