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

  let private comparisonGrid columns items =
    let cells =
      items
      |> List.map (fun item ->
        GridItem.create (
          div [ Flex.Flex.allSizes; JustifyContent.center ] [ item ],
          attrs = [
            GridItem.Span.twelve
            GridItem.Span.Small.six
            (if columns = 4 then GridItem.Span.Medium.three
             elif columns = 3 then GridItem.Span.Medium.four
             else GridItem.Span.Medium.six)
          ]
        ))

    Grid.create (cells, attrs = [ Grid.Spacing.large ])

  let grid content = Grid.create (items = content)

  let private activationExamples () =
    let description =
      Helpers.bodyText
        "The default combines Hover and Focus. For a hover interaction, retain Focus so keyboard users can reveal the same information. Click uses the native button activation, including Enter and Space."

    let content =
      let example label activationEvents color =
        Tooltip.create (
          Button.create (text label, onClick = ignore, attrs = [ Button.Variant.outlined; color ]),
          text ("Opened by " + label),
          activationEvents = activationEvents
        )

      comparisonGrid 3 [
        example "Hover or focus" [ Tooltip.Activation.Hover; Tooltip.Activation.Focus ] Button.Color.primary
        example "Click or press" [ Tooltip.Activation.Click ] Button.Color.secondary
        example "Focus" [ Tooltip.Activation.Focus ] Button.Color.tertiary
      ]

    let code =
      """let example label activationEvents color =
  Tooltip.create (
    Button.create (
      text label,
      onClick = ignore,
      attrs = [ Button.Variant.outlined; color ]
    ),
    text ("Opened by " + label),
    activationEvents = activationEvents
  )

example
  "Hover or focus"
  [ Tooltip.Activation.Hover; Tooltip.Activation.Focus ]
  Button.Color.primary

example "Click or press" [ Tooltip.Activation.Click ] Button.Color.secondary
example "Focus" [ Tooltip.Activation.Focus ] Button.Color.tertiary"""

    Helpers.codeSampleSection "Activation Events" description content code

  let private customContentExample () =
    let description =
      Helpers.bodyText
        "Pass any Doc as tooltipContent for rich layouts with headings, icons, or multiple lines."

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
      """Tooltip.create (
  Button.info (
    text "Rich Content Tooltip",
    onClick = (fun () -> ()),
    attrs = [ Button.Variant.filled ]
  ),
  div [ Attr.Style "padding" "4px" ] [
    div [
      Typography.body2
      Attr.Style "font-weight" "bold"
      Margin.Bottom.extraSmall
    ] [ text "Custom Tooltip" ]
    div [ Typography.caption ] [
      text "You can use Doc elements for rich content"
    ]
  ]
)"""

    Helpers.codeSampleSection "Custom Content" description content code

  let private textTooltipExample () =
    let description =
      Helpers.bodyText "Wrap any inline element in a tooltip, not just buttons."

    let content =
      div [] [
        span [ Typography.body1 ] [ text "Hover over " ]

        Tooltip.create (
          span [
            Typography.body1
            Typography.Color.primary
            Attr.Style "text-decoration" "underline"
          ] [ text "this text" ],
          text "Inline text tooltip",
          wrapperAttrs = [ Attr.Create "tabindex" "0" ]
        )

        span [ Typography.body1 ] [ text " to see a tooltip. You can also hover over " ]

        Tooltip.create (
          span [
            Typography.body1
            Typography.Color.secondary
            Attr.Style "text-decoration" "underline"
          ] [ text "this other text" ],
          text "More information",
          direction = Tooltip.Direction.Bottom,
          tooltipAttrs = [ Tooltip.Color.secondary ],
          wrapperAttrs = [ Attr.Create "tabindex" "0" ]
        )

        span [ Typography.body1 ] [ text " for more information." ]
      ]

    let code =
      """div [] [
  span [ Typography.body1 ] [ text "Hover over " ]

  Tooltip.create (
    span [
      Typography.body1
      Typography.Color.primary
      Attr.Style "text-decoration" "underline"
    ] [ text "this text" ],
    text "Inline text tooltip",
    wrapperAttrs = [ Attr.Create "tabindex" "0" ]
  )

  span [ Typography.body1 ] [
    text " to see a tooltip. You can also hover over "
  ]

  Tooltip.create (
    span [
      Typography.body1
      Typography.Color.secondary
      Attr.Style "text-decoration" "underline"
    ] [ text "this other text" ],
    text "More information",
    direction = Tooltip.Direction.Bottom,
    tooltipAttrs = [ Tooltip.Color.secondary ],
    wrapperAttrs = [ Attr.Create "tabindex" "0" ]
  )

  span [ Typography.body1 ] [ text " for more information." ]
]"""

    Helpers.codeSampleSection "Text Tooltips" description content code

  let private iconTooltipExample () =
    let description =
      Helpers.bodyText
        "Wrap icons in a tooltip to provide context on hover or keyboard focus. Make static triggers focusable with wrapperAttrs."

    let content =
      div [
        Flex.Flex.allSizes
        FlexDirection.Row.allSizes
        JustifyContent.center
        Gap.All.g4
      ] [
        Tooltip.create (
          Icon.create (Icon.UiActions UiActions.CheckCircle, attrs = [ BrandColor.TextColor.success ]),
          text "Saved successfully.",
          tooltipAttrs = [ Tooltip.Color.success ],
          wrapperAttrs = [
            Attr.Create "tabindex" "0"
            Attr.Create "role" "img"
            Attr.Create "aria-label" "Success"
          ]
        )

        Tooltip.create (
          Icon.create (Icon.Action Action.Warning, attrs = [ BrandColor.TextColor.warning ]),
          text "Review this setting.",
          tooltipAttrs = [ Tooltip.Color.warning ],
          wrapperAttrs = [
            Attr.Create "tabindex" "0"
            Attr.Create "role" "img"
            Attr.Create "aria-label" "Warning"
          ]
        )

        Tooltip.create (
          Icon.create (Icon.Action Action.Error, attrs = [ BrandColor.TextColor.error ]),
          text "Action could not be completed.",
          tooltipAttrs = [ Tooltip.Color.error ],
          wrapperAttrs = [
            Attr.Create "tabindex" "0"
            Attr.Create "role" "img"
            Attr.Create "aria-label" "Error"
          ]
        )
      ]

    let code =
      """open Weave.Icons.MaterialSymbols

Tooltip.create (
  Icon.create (
    Icon.UiActions UiActions.CheckCircle,
    attrs = [ BrandColor.TextColor.success ]
  ),
  text "Saved successfully.",
  tooltipAttrs = [ Tooltip.Color.success ],
  wrapperAttrs = [
    Attr.Create "tabindex" "0"
    Attr.Create "role" "img"
    Attr.Create "aria-label" "Success"
  ]
)

Tooltip.create (
  Icon.create (
    Icon.Action Action.Warning,
    attrs = [ BrandColor.TextColor.warning ]
  ),
  text "Review this setting.",
  tooltipAttrs = [ Tooltip.Color.warning ],
  wrapperAttrs = [
    Attr.Create "tabindex" "0"
    Attr.Create "role" "img"
    Attr.Create "aria-label" "Warning"
  ]
)

Tooltip.create (
  Icon.create (
    Icon.Action Action.Error,
    attrs = [ BrandColor.TextColor.error ]
  ),
  text "Action could not be completed.",
  tooltipAttrs = [ Tooltip.Color.error ],
  wrapperAttrs = [
    Attr.Create "tabindex" "0"
    Attr.Create "role" "img"
    Attr.Create "aria-label" "Error"
  ]
)"""

    Helpers.codeSampleSection "Icon Tooltips" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Tooltip.") [
      Helpers.apiTable "Tooltip.create" [
        Helpers.apiParam "innerContent" "Doc" "" "The element that triggers the tooltip on interaction"
        Helpers.apiParam "tooltipContent" "Doc" "" "Content displayed inside the tooltip popup"
        Helpers.apiParam
          "?activationEvents"
          "Activation list"
          "[Hover; Focus]"
          "Which interactions show the tooltip"
        Helpers.apiParam "?direction" "Direction" "Top" "Preferred side; flips and shifts to fit the viewport"
        Helpers.apiParam "?showArrow" "bool" "true" "Whether to display a directional arrow on the tooltip"
        Helpers.apiParam
          "?tooltipAttrs"
          "Attr list"
          "[]"
          "Additional attributes applied to the tooltip popup element"
        Helpers.apiParam
          "?wrapperAttrs"
          "Attr list"
          "[]"
          "Additional attributes applied to the root wrapper element"
      ]

      Helpers.styleModuleTable "Tooltip.Direction (DU)" [
        ("Top", "Tooltip appears above the trigger (default)")
        ("Bottom", "Tooltip appears below the trigger")
        ("Left", "Tooltip appears to the left of the trigger")
        ("Right", "Tooltip appears to the right of the trigger")
      ]

      Helpers.styleModuleTable "Tooltip.Activation (DU)" [
        ("Hover", "Show on mouse enter, hide on mouse leave (default)")
        ("Focus", "Show on focus, hide on blur (default)")
        ("Click", "Toggle visibility on click/tap")
      ]

      Helpers.styleModuleTable "Tooltip.Color" [
        ("primary", "Primary brand color tooltip background")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Tooltip"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Tooltips display informative text when users hover over, focus on, or tap an element."
        ]

        Helpers.divider ()
        ReferenceAppearance.tooltip ()
        Helpers.divider ()
        activationExamples ()
        Helpers.divider ()
        customContentExample ()
        Helpers.divider ()
        textTooltipExample ()
        Helpers.divider ()
        iconTooltipExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
