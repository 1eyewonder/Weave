namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ElevationExamples =

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

  // ---------------------------------------------------------------------------
  // 1. How it works
  // ---------------------------------------------------------------------------

  let private howItWorksSection () =
    let description =
      Helpers.bodyText
        "Elevation adds depth through box-shadow. Each level (0-25) maps to a progressively larger shadow via CSS custom properties. Apply with Elevation.e0 through Elevation.e25."

    let content =
      div [ Attr.Style "overflow-x" "auto" ] [
        table [ Attr.Style "width" "100%"; Attr.Style "border-collapse" "collapse" ] [
          thead [] [
            tr [] [
              tableHeaderCell "Level"
              tableHeaderCell "F# value"
              tableHeaderCell "Use case"
            ]
          ]
          tbody [] [
            tr [] [
              tableCell [ text "0" ]
              tableCell [ inlineCode "Elevation.e0" ]
              tableCell [ text "Flat surface, no shadow" ]
            ]
            tr [] [
              tableCell [ text "1-2" ]
              tableCell [ inlineCode "Elevation.e1 / e2" ]
              tableCell [ text "Cards, subtle lift" ]
            ]
            tr [] [
              tableCell [ text "3-4" ]
              tableCell [ inlineCode "Elevation.e3 / e4" ]
              tableCell [ text "Raised buttons, hover states" ]
            ]
            tr [] [
              tableCell [ text "6-8" ]
              tableCell [ inlineCode "Elevation.e6 / e8" ]
              tableCell [ text "FABs, bottom sheets, menus" ]
            ]
            tr [] [
              tableCell [ text "12-16" ]
              tableCell [ inlineCode "Elevation.e12 / e16" ]
              tableCell [ text "Dialogs, navigation drawers" ]
            ]
            tr [] [
              tableCell [ text "20-24" ]
              tableCell [ inlineCode "Elevation.e20 / e24" ]
              tableCell [ text "Modals, pickers, high-emphasis overlays" ]
            ]
          ]
        ]
      ]

    let code =
      """open Weave

// Apply elevation as an Attr on any element
div [
    Elevation.e4
    Padding.All.small
    BorderRadius.All.medium
    SurfaceColor.BackgroundColor.surface
] [
    text "Elevated card at level 4"
]

// Elevation is just box-shadow — combine with any other attrs
div [ Elevation.e8; BorderRadius.All.large ] [
    text "Higher elevation surface"
]"""

    Helpers.codeSampleSection "How it works" description content code

  // ---------------------------------------------------------------------------
  // 2. Visual scale
  // ---------------------------------------------------------------------------

  let private visualScaleSection () =
    let description =
      Helpers.bodyText
        "A representative sample of the 0-24 elevation scale. Each card shows its shadow depth."

    let elevationDemo (level: string) (elevAttr: Attr) =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
        div [
          elevAttr
          SurfaceColor.BackgroundColor.surface
          BorderRadius.All.medium
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.center
          Attr.Style "width" "100%"
          Attr.Style "min-height" "80px"
        ] [ div [ Typography.h6 ] [ text level ] ]
        div [ Typography.body2; Margin.Top.extraSmall; Attr.Style "opacity" "0.6" ] [
          text (sprintf "e%s" level)
        ]
      ]

    let content =
      Grid.create (
        [
          GridItem.create (
            elevationDemo "0" Elevation.e0,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            elevationDemo "2" Elevation.e2,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            elevationDemo "4" Elevation.e4,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            elevationDemo "6" Elevation.e6,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            elevationDemo "8" Elevation.e8,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            elevationDemo "12" Elevation.e12,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            elevationDemo "16" Elevation.e16,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            elevationDemo "20" Elevation.e20,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            elevationDemo "24" Elevation.e24,
            attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.three ]
          )
        ]
      )

    let code =
      """open Weave

// Each elevation level is a direct Attr
div [ Elevation.e0 ] [ text "Flat" ]
div [ Elevation.e4 ] [ text "Raised" ]
div [ Elevation.e8 ] [ text "Floating" ]
div [ Elevation.e16 ] [ text "Dialog-level" ]
div [ Elevation.e24 ] [ text "Modal" ]"""

    Helpers.codeSampleSection "Visual scale" description content code

  // ---------------------------------------------------------------------------
  // 3. Playground
  // ---------------------------------------------------------------------------

  let private playgroundSection () =
    let description =
      Helpers.bodyText "Pick an elevation level from the dropdown to preview the shadow on a live card."

    let allElevations = [
      "0", Elevation.e0
      "1", Elevation.e1
      "2", Elevation.e2
      "3", Elevation.e3
      "4", Elevation.e4
      "5", Elevation.e5
      "6", Elevation.e6
      "7", Elevation.e7
      "8", Elevation.e8
      "9", Elevation.e9
      "10", Elevation.e10
      "11", Elevation.e11
      "12", Elevation.e12
      "13", Elevation.e13
      "14", Elevation.e14
      "15", Elevation.e15
      "16", Elevation.e16
      "17", Elevation.e17
      "18", Elevation.e18
      "19", Elevation.e19
      "20", Elevation.e20
      "21", Elevation.e21
      "22", Elevation.e22
      "23", Elevation.e23
      "24", Elevation.e24
      "25", Elevation.e25
    ]

    let selectedLevel = Var.Create<string option>(Some "4")

    let levelItems =
      allElevations
      |> List.map (fun (label, _) -> SelectItem.create (text (sprintf "Level %s" label), label, label))
      |> View.Const

    let content =
      div [] [
        Grid.create (
          [
            GridItem.create (
              Select.create (
                levelItems,
                selectedLevel,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Elevation Level",
                placeholder = View.Const "Choose...",
                attrs = [ Select.Color.primary ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )
          ]
        )

        selectedLevel.View
        |> Doc.BindView(fun sel ->
          let elevAttr =
            sel
            |> Option.bind (fun label -> allElevations |> List.tryFind (fun (l, _) -> l = label))
            |> Option.map snd
            |> Option.defaultValue Attr.Empty

          let levelLabel = sel |> Option.defaultValue "0"

          div [ Margin.Top.small ] [
            div [
              elevAttr
              SurfaceColor.BackgroundColor.surface
              BorderRadius.All.medium
              Flex.Flex.allSizes
              AlignItems.center
              JustifyContent.center
              Attr.Style "min-height" "160px"
            ] [
              div [ Typography.h4; Attr.Style "opacity" "0.8" ] [ text (sprintf "Elevation.e%s" levelLabel) ]
            ]
          ])
      ]

    let code =
      """open Weave

// Apply any elevation level — the shadow scales smoothly from 0 to 25
div [
    Elevation.e4  // see here
    SurfaceColor.BackgroundColor.surface
    BorderRadius.All.medium
    Padding.All.medium
] [
    text "Preview card"
]"""

    Helpers.codeSampleSection "Playground" description content code

  // ---------------------------------------------------------------------------
  // 4. Common patterns
  // ---------------------------------------------------------------------------

  let private commonPatternsSection () =
    let description =
      Helpers.bodyText
        "Elevation on surfaces gives visual hierarchy. Here are typical levels for common UI patterns."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Flat card (e0)" ]
              div [
                Elevation.e0
                SurfaceColor.BackgroundColor.surface
                BorderWidth.All.one
                Attr.Style "border-style" "solid"
                BorderColor.linesDefault
                BorderRadius.All.medium
                Padding.All.small
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Flat card" ]
                div [ Typography.body2; Attr.Style "opacity" "0.7" ] [
                  text "Uses a border instead of shadow for separation."
                ]
              ]
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Raised card (e2)" ]
              div [
                Elevation.e2
                SurfaceColor.BackgroundColor.surface
                BorderRadius.All.medium
                Padding.All.small
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Raised card" ]
                div [ Typography.body2; Attr.Style "opacity" "0.7" ] [
                  text "Default card elevation. Subtle lift off the background."
                ]
              ]
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Dialog surface (e8)" ]
              div [
                Elevation.e8
                SurfaceColor.BackgroundColor.surface
                BorderRadius.All.medium
                Padding.All.small
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Confirm action" ]
                div [ Typography.body2; Attr.Style "opacity" "0.7"; Margin.Bottom.extraSmall ] [
                  text "Are you sure you want to proceed?"
                ]
                div [ Flex.Flex.allSizes; JustifyContent.flexEnd; Attr.Style "gap" "8px" ] [
                  Button.create (text "Cancel", onClick = ignore, attrs = [ Button.Variant.text ])
                  Button.primary (text "Confirm", onClick = ignore, attrs = [ Button.Variant.filled ])
                ]
              ]
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "FAB (e6)" ]
              div [
                Flex.Flex.allSizes
                AlignItems.center
                JustifyContent.center
                Attr.Style "min-height" "100px"
              ] [
                div [
                  Elevation.e6
                  BrandColor.BackgroundColor.primary
                  BorderRadius.circle
                  Flex.Flex.allSizes
                  AlignItems.center
                  JustifyContent.center
                  Attr.Style "width" "56px"
                  Attr.Style "height" "56px"
                ] [
                  Icon.create (
                    Icon.UiActions UiActions.Add,
                    attrs = [ Attr.Style "color" "white"; Attr.Style "font-size" "24px" ]
                  )
                ]
              ]
            ],
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
          )
        ]
      )

    let code =
      """open Weave

// Flat card with border instead of shadow
div [
    Elevation.e0
    BorderWidth.All.one
    Attr.Style "border-style" "solid"
    BorderColor.linesDefault
    BorderRadius.All.medium
    Padding.All.small
] [ text "Flat card" ]

// Raised card — default card level
div [
    Elevation.e2  // see here
    SurfaceColor.BackgroundColor.surface
    BorderRadius.All.medium
    Padding.All.small
] [ text "Raised card" ]

// Dialog-level surface
div [ Elevation.e8; BorderRadius.All.medium; Padding.All.small ] [
    text "Dialog content"
]

// FAB — floating action button level
div [
    Elevation.e6  // see here
    BrandColor.BackgroundColor.primary
    BorderRadius.circle
] [ text "+" ]"""

    Helpers.codeSampleSection "Common patterns" description content code

  // ---------------------------------------------------------------------------
  // Render
  // ---------------------------------------------------------------------------

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Elevation"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Add depth to surfaces with box-shadow utility classes. 26 levels (0-25) cover everything from flat cards to high-emphasis modals."
        ]

        Helpers.divider ()
        howItWorksSection ()
        Helpers.divider ()
        visualScaleSection ()
        Helpers.divider ()
        playgroundSection ()
        Helpers.divider ()
        commonPatternsSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
