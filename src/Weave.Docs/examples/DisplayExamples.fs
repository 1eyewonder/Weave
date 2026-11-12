namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module DisplayExamples =

  let private inlineCode = Helpers.inlineCode

  let private tableCell (children: Doc list) =
    td [ Attr.Style "padding" "8px 12px"; Attr.Style "overflow-wrap" "anywhere" ] children

  let private tableHeaderCell (label: string) =
    th [
      Attr.Style "text-align" "start"
      Attr.Create "scope" "col"
      Attr.Style "padding" "8px 12px"
      Attr.Style "overflow-wrap" "anywhere"
      Attr.Style "border-bottom" "1px solid var(--palette-divider)"
    ] [ text label ]

  let private colorBox (label: string) (color: Attr) (extraAttrs: Attr list) =
    div [
      color
      Typography.subtitle2
      Attr.Style "padding" "8px 12px"
      Attr.Style "border-radius" "4px"
      Attr.Style "text-align" "center"
      yield! extraAttrs
    ] [ text label ]

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "The Display module controls CSS display types with responsive breakpoint support. Each display type (Block, Flex, Inline, etc.) has always, hidden, HideOn, and ShowOnly sub-modules for responsive visibility."

    let content =
      div [
        Attr.Class "docs-table-scroll"
        Attr.Style "overflow-x" "auto"
        attr.tabindex "0"
        Attr.Create "aria-label" "Utility reference"
      ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [
              tableHeaderCell "Display type"
              tableHeaderCell "Always visible"
              tableHeaderCell "Hidden"
              tableHeaderCell "F# module"
            ]
          ]
          tbody [] [
            tr [] [
              tableCell [ text "Block" ]
              tableCell [ inlineCode "Display.Block.always" ]
              tableCell [ inlineCode "Display.Block.hidden" ]
              tableCell [ inlineCode "Display.Block" ]
            ]
            tr [] [
              tableCell [ text "Flex" ]
              tableCell [ inlineCode "Display.Flex.always" ]
              tableCell [ inlineCode "Display.Flex.hidden" ]
              tableCell [ inlineCode "Display.Flex" ]
            ]
            tr [] [
              tableCell [ text "Inline" ]
              tableCell [ inlineCode "Display.Inline.always" ]
              tableCell [ inlineCode "Display.Inline.hidden" ]
              tableCell [ inlineCode "Display.Inline" ]
            ]
            tr [] [
              tableCell [ text "Inline Block" ]
              tableCell [ inlineCode "Display.InlineBlock.always" ]
              tableCell [ inlineCode "Display.InlineBlock.hidden" ]
              tableCell [ inlineCode "Display.InlineBlock" ]
            ]
            tr [] [
              tableCell [ text "Inline Flex" ]
              tableCell [ inlineCode "Display.InlineFlex.always" ]
              tableCell [ inlineCode "Display.InlineFlex.hidden" ]
              tableCell [ inlineCode "Display.InlineFlex" ]
            ]
            tr [] [
              tableCell [ text "Table" ]
              tableCell [ inlineCode "Display.Table.always" ]
              tableCell [ inlineCode "Display.Table.hidden" ]
              tableCell [ inlineCode "Display.Table" ]
            ]
            tr [] [
              tableCell [ text "Table Row" ]
              tableCell [ inlineCode "Display.TableRow.always" ]
              tableCell [ inlineCode "Display.TableRow.hidden" ]
              tableCell [ inlineCode "Display.TableRow" ]
            ]
            tr [] [
              tableCell [ text "Table Cell" ]
              tableCell [ inlineCode "Display.TableCell.always" ]
              tableCell [ inlineCode "Display.TableCell.hidden" ]
              tableCell [ inlineCode "Display.TableCell" ]
            ]
            tr [] [
              tableCell [ text "Contents" ]
              tableCell [ inlineCode "Display.Contents.always" ]
              tableCell [ inlineCode "Display.Contents.hidden" ]
              tableCell [ inlineCode "Display.Contents" ]
            ]
          ]
        ]
      ]

    Helpers.section "How it works" description content

  let private basicDisplaySection () =
    let description =
      Helpers.bodyText "Elements in different display modes side by side."

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Block" ]
        div [ Margin.Bottom.small ] [
          div [
            Display.Block.always
            BrandColor.Fill.primary
            Padding.All.extraSmall
            BorderRadius.All.small
            Margin.Bottom.extraSmall
          ] [ text "Block A — takes full width" ]
          div [
            Display.Block.always
            BrandColor.Fill.secondary
            Padding.All.extraSmall
            BorderRadius.All.small
          ] [ text "Block B — stacks below" ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Inline" ]
        div [ Margin.Bottom.small ] [
          span [
            Display.Inline.always
            BrandColor.Fill.info
            Padding.Horizontal.extraSmall
            BorderRadius.All.small
          ] [ text "Inline A" ]
          text " "
          span [
            Display.Inline.always
            BrandColor.Fill.success
            Padding.Horizontal.extraSmall
            BorderRadius.All.small
          ] [ text "Inline B" ]
          text " "
          span [
            Display.Inline.always
            BrandColor.Fill.warning
            Padding.Horizontal.extraSmall
            BorderRadius.All.small
          ] [ text "Inline C" ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Inline Block" ]
        div [ Margin.Bottom.small ] [
          div [
            Display.InlineBlock.always
            BrandColor.Fill.error
            Padding.All.extraSmall
            BorderRadius.All.small
            Attr.Style "width" "120px"
          ] [ text "Inline Block" ]
          text " "
          div [
            Display.InlineBlock.always
            BrandColor.Fill.tertiary
            Padding.All.extraSmall
            BorderRadius.All.small
            Attr.Style "width" "120px"
          ] [ text "Inline Block" ]
        ]
      ]

    let code =
      """div [
  Display.Block.always
  BrandColor.Fill.primary
  Padding.All.extraSmall
  BorderRadius.All.small
  Margin.Bottom.extraSmall
] [ text "Block A — takes full width" ]

div [
  Display.Block.always
  BrandColor.Fill.secondary
  Padding.All.extraSmall
  BorderRadius.All.small
] [ text "Block B — stacks below" ]

span [
  Display.Inline.always
  BrandColor.Fill.info
  Padding.Horizontal.extraSmall
  BorderRadius.All.small
] [ text "Inline A" ]

span [
  Display.Inline.always
  BrandColor.Fill.success
  Padding.Horizontal.extraSmall
  BorderRadius.All.small
] [ text "Inline B" ]

span [
  Display.Inline.always
  BrandColor.Fill.warning
  Padding.Horizontal.extraSmall
  BorderRadius.All.small
] [ text "Inline C" ]

div [
  Display.InlineBlock.always
  BrandColor.Fill.error
  Padding.All.extraSmall
  BorderRadius.All.small
  Attr.Style "width" "120px"
] [ text "Inline Block" ]

div [
  Display.InlineBlock.always
  BrandColor.Fill.tertiary
  Padding.All.extraSmall
  BorderRadius.All.small
  Attr.Style "width" "120px"
] [ text "Inline Block" ]"""

    Helpers.codeSampleSection "Basic display" description content code

  let private responsiveVisibilitySection () =
    let description =
      Helpers.bodyText
        "HideOn hides an element at a specific breakpoint and below. ShowOnly shows an element only at exactly that breakpoint. Resize your browser to see boxes appear and disappear."

    let content =
      div [] [
        div [
          Flex.Flex.allSizes
          AlignItems.center
          Attr.Style "gap" "8px"
          Margin.Bottom.small
        ] [
          div [ Typography.subtitle2 ] [ text "Current breakpoint:" ]
          Chip.create (textView Breakpoint.browserAsText, attrs = [ Chip.Variant.filled; Chip.Color.primary ])
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [
          text "HideOn — hides at breakpoint and below"
        ]

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          JustifyContent.spaceEvenly
          Attr.Style "gap" "8px"
          Margin.Bottom.small
        ] [
          div [ Display.Block.HideOn.xs ] [ colorBox "Hidden on XS" BrandColor.Fill.primary [] ]
          div [ Display.Block.HideOn.sm ] [ colorBox "Hidden on SM" BrandColor.Fill.secondary [] ]
          div [ Display.Block.HideOn.md ] [ colorBox "Hidden on MD" BrandColor.Fill.info [] ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [
          text "ShowOnly — visible at exactly one breakpoint"
        ]

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          JustifyContent.spaceEvenly
          Attr.Style "gap" "8px"
          Margin.Bottom.small
        ] [
          div [ Display.Block.ShowOnly.xs ] [ colorBox "XS only" BrandColor.Fill.error [] ]
          div [ Display.Block.ShowOnly.sm ] [ colorBox "SM only" BrandColor.Fill.warning [] ]
          div [ Display.Block.ShowOnly.md ] [ colorBox "MD only" BrandColor.Fill.success [] ]
          div [ Display.Block.ShowOnly.lg ] [ colorBox "LG only" BrandColor.Fill.info [] ]
          div [ Display.Block.ShowOnly.xl ] [ colorBox "XL only" BrandColor.Fill.primary [] ]
          div [ Display.Block.ShowOnly.xxl ] [ colorBox "XXL only" BrandColor.Fill.tertiary [] ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Responsive flex container" ]

        div [ Attr.Style "gap" "8px" ] [
          div [
            Display.Flex.HideOn.xs // see here
            Attr.Style "gap" "8px"
          ] [
            colorBox "Flex A" BrandColor.Fill.primary [ Attr.Style "flex" "1" ]
            colorBox "Flex B" BrandColor.Fill.secondary [ Attr.Style "flex" "1" ]
            colorBox "Flex C" BrandColor.Fill.info [ Attr.Style "flex" "1" ]
          ]
          div [ Display.Block.ShowOnly.xs ] [
            colorBox "Stacked on XS (flex hidden)" BrandColor.Fill.primary []
          ]
        ]
      ]

    let code =
      """Chip.create (
  textView Breakpoint.browserAsText,
  attrs = [ Chip.Variant.filled; Chip.Color.primary ]
)

div [ Display.Block.HideOn.xs; BrandColor.Fill.primary ] [
  text "Hidden on XS"
]

div [ Display.Block.HideOn.sm; BrandColor.Fill.secondary ] [
  text "Hidden on SM"
]

div [ Display.Block.HideOn.md; BrandColor.Fill.info ] [ text "Hidden on MD" ]

div [ Display.Block.ShowOnly.xs; BrandColor.Fill.error ] [ text "XS only" ]

div [ Display.Block.ShowOnly.sm; BrandColor.Fill.warning ] [ text "SM only" ]

div [ Display.Block.ShowOnly.md; BrandColor.Fill.success ] [ text "MD only" ]

div [ Display.Block.ShowOnly.lg; BrandColor.Fill.info ] [ text "LG only" ]

div [ Display.Block.ShowOnly.xl; BrandColor.Fill.primary ] [ text "XL only" ]

div [ Display.Block.ShowOnly.xxl; BrandColor.Fill.tertiary ] [
  text "XXL only"
]

// This parent changes the arrangement of its children above XS.
div [ Display.Flex.HideOn.xs; Gap.All.g2 ] [
  div [ BrandColor.Fill.primary; FlexItem.Grow.allSizes ] [ text "Flex A" ]
  div [ BrandColor.Fill.secondary; FlexItem.Grow.allSizes ] [ text "Flex B" ]
  div [ BrandColor.Fill.info; FlexItem.Grow.allSizes ] [ text "Flex C" ]
]

div [ Display.Block.ShowOnly.xs; BrandColor.Fill.primary ] [
  text "Stacked on XS (flex hidden)"
]"""

    Helpers.codeSampleSection "Responsive visibility" description content code

  let private visibilitySection () =
    let description =
      Helpers.bodyText
        "Visibility controls element visibility without changing layout. Use srOnly for screen-reader-only content that remains accessible but invisible."

    let content =
      div [
        Attr.Class "docs-table-scroll"
        Attr.Style "overflow-x" "auto"
        attr.tabindex "0"
        Attr.Create "aria-label" "Utility reference"
      ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [
              tableHeaderCell "Utility"
              tableHeaderCell "F# value"
              tableHeaderCell "Effect"
            ]
          ]
          tbody [] [
            tr [] [
              tableCell [ text "Visible" ]
              tableCell [ inlineCode "Visibility.visible" ]
              tableCell [ text "Element is visible (default)" ]
            ]
            tr [] [
              tableCell [ text "Invisible" ]
              tableCell [ inlineCode "Visibility.invisible" ]
              tableCell [ text "Element is hidden but still occupies space in layout" ]
            ]
            tr [] [
              tableCell [ text "Screen Reader Only" ]
              tableCell [ inlineCode "Visibility.srOnly" ]
              tableCell [ text "Visually hidden, accessible to screen readers" ]
            ]
          ]
        ]

        div [ Margin.Top.small ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Invisible vs Hidden" ]

          div [ Flex.Flex.allSizes; Attr.Style "gap" "8px"; AlignItems.center ] [
            colorBox "Visible" BrandColor.Fill.primary []
            div [ Visibility.invisible ] [ colorBox "Invisible" BrandColor.Fill.secondary [] ]
            colorBox "Still here" BrandColor.Fill.info []
          ]

          div [ Typography.body2; Margin.Top.extraSmall; Typography.Color.textSecondary ] [
            text
              "The middle box is invisible but still takes up space. Notice the gap between 'Visible' and 'Still here'."
          ]
        ]
      ]

    Helpers.section "Visibility utilities" description content

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Display"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Control CSS display types and responsive visibility with breakpoint-aware utility classes. Hide and show elements at specific screen sizes without writing media queries."
        ]

        Helpers.divider ()
        basicDisplaySection ()
        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        responsiveVisibilitySection ()
        Helpers.divider ()
        visibilitySection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
