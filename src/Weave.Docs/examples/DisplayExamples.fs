namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module DisplayExamples =

  let private inlineCode (value: string) =
    span [ Typography.caption; Typography.Color.primary ] [ text value ]

  let private tableCell (children: Doc list) =
    td [ Attr.Style "padding" "8px 12px"; Attr.Style "white-space" "nowrap" ] children

  let private tableHeaderCell (label: string) =
    th [
      Attr.Style "text-align" "left"
      Attr.Style "padding" "8px 12px"
      Attr.Style "white-space" "nowrap"
      Attr.Style "border-bottom" "1px solid var(--palette-divider)"
    ] [ text label ]

  let private colorBox (label: string) (color: string) (extraAttrs: Attr list) =
    div [
      Attr.Style "background" color
      Attr.Style "color" "white"
      Attr.Style "padding" "8px 12px"
      Attr.Style "font-size" "13px"
      Attr.Style "font-weight" "600"
      Attr.Style "border-radius" "4px"
      Attr.Style "text-align" "center"
      yield! extraAttrs
    ] [ text label ]

  // ---------------------------------------------------------------------------
  // 1. How it works
  // ---------------------------------------------------------------------------

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "The Display module controls CSS display types with responsive breakpoint support. Each display type (Block, Flex, Inline, etc.) has always, hidden, HideOn, and ShowOnly sub-modules for responsive visibility."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
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

    let code =
      """open Weave

// Always display as block
div [ Display.Block.always ] [ text "Block element" ]

// Always display as flex
div [ Display.Flex.always ] [ text "Flex container" ]

// Completely hidden
div [ Display.Block.hidden ] [ text "You cannot see me" ]

// Hide on small screens, show as block on medium and up
div [ Display.Block.HideOn.xs ] [ text "Hidden on XS" ]

// Show only on large screens as a flex container
div [ Display.Flex.ShowOnly.lg ] [ text "LG only" ]"""

    Helpers.codeSampleSection "How it works" description content code

  // ---------------------------------------------------------------------------
  // 2. Basic display
  // ---------------------------------------------------------------------------

  let private basicDisplaySection () =
    let description =
      Helpers.bodyText "Elements in different display modes side by side."

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Block" ]
        div [ Margin.Bottom.small ] [
          div [
            Display.Block.always
            BrandColor.toBackgroundColor BrandColor.Primary
            Padding.All.extraSmall
            BorderRadius.All.small
            Margin.Bottom.extraSmall
          ] [ div [ Typography.body2 ] [ text "Block A — takes full width" ] ]
          div [
            Display.Block.always
            BrandColor.toBackgroundColor BrandColor.Secondary
            Padding.All.extraSmall
            BorderRadius.All.small
          ] [ div [ Typography.body2 ] [ text "Block B — stacks below" ] ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Inline" ]
        div [ Margin.Bottom.small ] [
          span [
            Display.Inline.always
            BrandColor.toBackgroundColor BrandColor.Info
            Padding.All.extraSmall
            BorderRadius.All.small
          ] [ text "Inline A" ]
          text " "
          span [
            Display.Inline.always
            BrandColor.toBackgroundColor BrandColor.Success
            Padding.All.extraSmall
            BorderRadius.All.small
          ] [ text "Inline B" ]
          text " "
          span [
            Display.Inline.always
            BrandColor.toBackgroundColor BrandColor.Warning
            Padding.All.extraSmall
            BorderRadius.All.small
          ] [ text "Inline C" ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Inline Block" ]
        div [ Margin.Bottom.small ] [
          div [
            Display.InlineBlock.always
            BrandColor.toBackgroundColor BrandColor.Error
            Padding.All.extraSmall
            BorderRadius.All.small
            Attr.Style "width" "120px"
          ] [ div [ Typography.body2 ] [ text "Inline Block" ] ]
          text " "
          div [
            Display.InlineBlock.always
            BrandColor.toBackgroundColor BrandColor.Tertiary
            Padding.All.extraSmall
            BorderRadius.All.small
            Attr.Style "width" "120px"
          ] [ div [ Typography.body2 ] [ text "Inline Block" ] ]
        ]
      ]

    let code =
      """open Weave

// Block — stacks vertically, takes full width
div [ Display.Block.always ] [ text "Block A" ]
div [ Display.Block.always ] [ text "Block B" ]

// Inline — flows horizontally with text
span [ Display.Inline.always ] [ text "Inline A" ]
span [ Display.Inline.always ] [ text "Inline B" ]

// Inline Block — flows inline but respects width/height
div [ Display.InlineBlock.always; Attr.Style "width" "120px" ] [
    text "Inline Block"
]"""

    Helpers.codeSampleSection "Basic display" description content code

  // ---------------------------------------------------------------------------
  // 3. Responsive visibility
  // ---------------------------------------------------------------------------

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
          Attr.Style "gap" "8px"
          Margin.Bottom.small
        ] [
          div [ Display.Block.HideOn.xs ] [ colorBox "Hidden on XS" "var(--palette-primary)" [] ]
          div [ Display.Block.HideOn.sm ] [ colorBox "Hidden on SM" "var(--palette-secondary)" [] ]
          div [ Display.Block.HideOn.md ] [ colorBox "Hidden on MD" "var(--palette-info)" [] ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [
          text "ShowOnly — visible at exactly one breakpoint"
        ]

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          Attr.Style "gap" "8px"
          Margin.Bottom.small
        ] [
          div [ Display.Block.ShowOnly.xs ] [ colorBox "XS only" "var(--palette-error)" [] ]
          div [ Display.Block.ShowOnly.sm ] [ colorBox "SM only" "var(--palette-warning)" [] ]
          div [ Display.Block.ShowOnly.md ] [ colorBox "MD only" "var(--palette-success)" [] ]
          div [ Display.Block.ShowOnly.lg ] [ colorBox "LG only" "var(--palette-info)" [] ]
          div [ Display.Block.ShowOnly.xl ] [ colorBox "XL only" "var(--palette-primary)" [] ]
          div [ Display.Block.ShowOnly.xxl ] [ colorBox "XXL only" "var(--palette-tertiary)" [] ]
        ]

        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Responsive flex container" ]

        div [ Attr.Style "gap" "8px" ] [
          div [
            Display.Flex.HideOn.xs // see here
            Attr.Style "gap" "8px"
          ] [
            colorBox "Flex A" "var(--palette-primary)" [ Attr.Style "flex" "1" ]
            colorBox "Flex B" "var(--palette-secondary)" [ Attr.Style "flex" "1" ]
            colorBox "Flex C" "var(--palette-info)" [ Attr.Style "flex" "1" ]
          ]
          div [ Display.Block.ShowOnly.xs ] [
            colorBox "Stacked on XS (flex hidden)" "var(--palette-primary)" []
          ]
        ]
      ]

    let code =
      """open Weave

// HideOn — hides at a breakpoint and below, shows above
div [ Display.Block.HideOn.xs ] [  // see here
    text "Hidden on extra-small screens"
]

// ShowOnly — visible at exactly one breakpoint
div [ Display.Block.ShowOnly.md ] [  // see here
    text "Visible only on medium screens"
]

// Responsive flex — hide the flex layout on XS
div [ Display.Flex.HideOn.xs ] [
    div [] [ text "A" ]
    div [] [ text "B" ]
    div [] [ text "C" ]
]

// Use Breakpoint.browserAsText to show current breakpoint
Chip.create(
    textView Breakpoint.browserAsText,
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
)"""

    Helpers.codeSampleSection "Responsive visibility" description content code

  // ---------------------------------------------------------------------------
  // 4. Visibility utilities
  // ---------------------------------------------------------------------------

  let private visibilitySection () =
    let description =
      Helpers.bodyText
        "Visibility controls element visibility without changing layout. Use srOnly for screen-reader-only content that remains accessible but invisible."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
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
            colorBox "Visible" "var(--palette-primary)" []
            div [ Visibility.invisible ] [ colorBox "Invisible" "var(--palette-secondary)" [] ]
            colorBox "Still here" "var(--palette-info)" []
          ]

          div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
            text
              "The middle box is invisible but still takes up space. Notice the gap between 'Visible' and 'Still here'."
          ]
        ]
      ]

    let code =
      """open Weave

// Invisible — hidden but keeps its layout space
div [ Visibility.invisible ] [ text "Takes space but invisible" ]

// Screen reader only — accessible but not visible
span [ Visibility.srOnly ] [ text "Opens navigation menu" ]

// Useful for accessible labels on icon buttons
IconButton.create(
    Icon.create(Icon.UiActions UiActions.Menu),
    onClick = (fun () -> ()),
    attrs = []
)
// Add an srOnly label nearby for screen readers
span [ Visibility.srOnly ] [ text "Toggle menu" ]"""

    Helpers.codeSampleSection "Visibility utilities" description content code

  // ---------------------------------------------------------------------------
  // Render
  // ---------------------------------------------------------------------------

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Display"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Control CSS display types and responsive visibility with breakpoint-aware utility classes. Hide and show elements at specific screen sizes without writing media queries."
        ]

        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        basicDisplaySection ()
        Helpers.divider ()
        responsiveVisibilitySection ()
        Helpers.divider ()
        visibilitySection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
