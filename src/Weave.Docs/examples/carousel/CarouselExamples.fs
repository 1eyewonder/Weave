namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module CarouselExamples =

  let private slide title detail symbol fill =
    let content =
      Doc.Concat [
        Icon.create (
          symbol,
          opticalSize = View.Const OpticalSize.``48``,
          attrs = [ Attr.Style "font-size" "3rem"; Attr.Create "aria-hidden" "true" ]
        )
        h3 [ Typography.h4; Attr.Style "max-width" "24ch" ] [ text title ]
        p [ Typography.body1; Attr.Style "max-width" "40ch" ] [ text detail ]
      ]

    CarouselItem.create (
      content,
      label = title,
      attrs = [
        fill
        Padding.All.small
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        JustifyContent.center
        Gap.All.g3
        Typography.Align.center
        Attr.Style "min-height" "240px"
        Attr.Style "box-sizing" "border-box"
      ]
    )

  let private basicExample () =
    let items = [
      CarouselItem.create (
        Doc.Concat [
          h3 [ Typography.h5 ] [ text "Plan" ]
          p [] [ text "Give your idea room to grow." ]
        ],
        label = "Plan",
        attrs = [ BrandColor.Fill.primary; Padding.All.medium; Attr.Style "min-height" "180px" ]
      )
      CarouselItem.create (
        Doc.Concat [
          h3 [ Typography.h5 ] [ text "Build" ]
          p [] [ text "Bring your components together." ]
        ],
        label = "Build",
        attrs = [
          BrandColor.Fill.tertiary
          Padding.All.medium
          Attr.Style "min-height" "180px"
        ]
      )
    ]

    Helpers.codeSampleSection
      "Basic Carousel"
      (Helpers.bodyText
        "Slides can contain any Doc. Use the arrows or indicators, or focus a slide and press Left or Right.")
      (Carousel.create (View.Const items, label = "Project stages"))
      """let items = [
  CarouselItem.create (
    Doc.Concat [
      h3 [ Typography.h5 ] [ text "Plan" ]
      p [] [ text "Give your idea room to grow." ]
    ],
    label = "Plan",
    attrs = [
      BrandColor.Fill.primary
      Padding.All.medium
      Attr.Style "min-height" "180px"
    ]
  )
  CarouselItem.create (
    Doc.Concat [
      h3 [ Typography.h5 ] [ text "Build" ]
      p [] [ text "Bring your components together." ]
    ],
    label = "Build",
    attrs = [
      BrandColor.Fill.tertiary
      Padding.All.medium
      Attr.Style "min-height" "180px"
    ]
  )
]

Carousel.create (View.Const items, label = "Project stages")"""

  let appearanceExample () =
    let arrows = Var.Create true
    let indicators = Var.Create true
    let transition = Var.Create true
    let autoplay = Var.Create false
    let selected = Var.Create 0

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        Switch.create (arrows, text "Show arrows")
        Switch.create (indicators, text "Show indicators")
        Switch.create (transition, text "Crossfade")
        Switch.create (autoplay, text "Automatic cycling")
      ]

    let config =
      let navigation = (arrows.View, indicators.View) ||> View.Map2(fun a b -> a, b)
      let motion = (transition.View, autoplay.View) ||> View.Map2(fun a b -> a, b)
      (navigation, motion) ||> View.Map2(fun (a, b) (c, d) -> a, b, c, d)

    let preview =
      config
      |> Doc.BindView(fun (arrows, indicators, transition, autoplay) ->
        let items = [
          slide
            "Less clutter. More flow."
            "Give your next idea room to grow."
            (Icon.Text Text.Dashboard)
            BrandColor.Fill.primary
          slide
            "A little color goes a long way."
            "Bring your components together with a shared palette."
            (Icon.Images Images.Palette)
            BrandColor.Fill.tertiary
          slide
            "Make something yours."
            "Start with a few components and see what takes shape."
            (Icon.Action Action.Build)
            BrandColor.Fill.secondary
        ]

        let attrs = [
          if not arrows then
            Carousel.Controls.hideArrows
          if not indicators then
            Carousel.Controls.hideIndicators
          if not transition then
            Carousel.Transition.none
        ]

        let interval = if autoplay then Some 5000 else None

        div [ Attr.Style "width" "100%" ] [
          Carousel.create (
            View.Const items,
            activeIndex = selected,
            ?autoCycleInterval = interval,
            label = "Weave highlights",
            attrs = attrs
          )
        ])

    let source =
      (config, selected.View)
      ||> View.Map2(fun (arrows, indicators, transition, autoplay) selected ->
        CarouselAppearance.source arrows indicators transition autoplay selected)

    let reset () =
      Var.Set arrows true
      Var.Set indicators true
      Var.Set transition true
      Var.Set autoplay false
      Var.Set selected 0

    Helpers.appearanceSection
      [ "automatic-cycling" ]
      (Helpers.bodyText
        "Choose navigation and motion options. Automatic cycling pauses on hover or when the tab is hidden, and stops when a slide or control receives focus. Use Play slides to resume; reduced motion starts it paused.")
      controls
      None
      preview
      reset
      source

  let private controlledExample () =
    let content =
      let selected = Var.Create 0
      let stages = [ "Design"; "Build"; "Ship" ]

      let items = [
        slide
          "Find the right shape."
          "Settle on a layout and a palette."
          (Icon.Images Images.Palette)
          BrandColor.Fill.secondary
        slide
          "Make the pieces work."
          "Connect your state and interactions."
          (Icon.Action Action.Build)
          BrandColor.Fill.primary
        slide
          "Ready for the next chapter."
          "Review the details, then share your work."
          (Icon.Social Social.RocketLaunch)
          BrandColor.Fill.success
      ]

      let navigation =
        stages
        |> List.mapi (fun index stage ->
          let isSelected = selected.View |> View.Map((=) index)

          Button.primary (
            text stage,
            onClick = (fun () -> Var.Set selected index),
            attrs = [
              Attr.DynamicClassPred "weave-button--filled" isSelected
              Attr.DynamicClassPred "weave-button--outlined" (isSelected |> View.Map not)
              Attr.Dynamic "aria-pressed" (isSelected |> View.Map string)
            ]
          ))

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
        div
          [
            Flex.Flex.allSizes
            FlexWrap.Wrap.allSizes
            JustifyContent.center
            Gap.All.g2
            Attr.Create "role" "group"
            Attr.Create "aria-label" "Project stage"
          ]
          navigation
        Carousel.create (
          View.Const items,
          activeIndex = selected,
          label = "Project stages",
          attrs = [ Carousel.Controls.hideArrows; Carousel.Controls.hideIndicators ]
        )
      ]

    Helpers.codeSampleSection
      "Controlled Selection"
      (Helpers.bodyText
        "Share one zero-based Var with custom navigation. The selected button is filled and exposes aria-pressed; the slides retain keyboard navigation.")
      content
      """open Weave.Icons
open Weave.Icons.MaterialSymbols

let slide title detail symbol fill =
  let content =
    Doc.Concat [
      Icon.create (
        symbol,
        opticalSize = View.Const OpticalSize.``48``,
        attrs = [
          Attr.Style "font-size" "3rem"
          Attr.Create "aria-hidden" "true"
        ]
      )
      h3 [ Typography.h4; Attr.Style "max-width" "24ch" ] [ text title ]
      p [ Typography.body1; Attr.Style "max-width" "40ch" ] [ text detail ]
    ]

  CarouselItem.create (
    content,
    label = title,
    attrs = [
      fill
      Padding.All.small
      Flex.Flex.allSizes
      FlexDirection.Column.allSizes
      AlignItems.center
      JustifyContent.center
      Gap.All.g3
      Typography.Align.center
      Attr.Style "min-height" "240px"
      Attr.Style "box-sizing" "border-box"
    ]
  )

let selected = Var.Create 0
let stages = [ "Design"; "Build"; "Ship" ]

let items = [
  slide
    "Find the right shape."
    "Settle on a layout and a palette."
    (Icon.Images Images.Palette)
    BrandColor.Fill.secondary
  slide
    "Make the pieces work."
    "Connect your state and interactions."
    (Icon.Action Action.Build)
    BrandColor.Fill.primary
  slide
    "Ready for the next chapter."
    "Review the details, then share your work."
    (Icon.Social Social.RocketLaunch)
    BrandColor.Fill.success
]

let navigation =
  stages
  |> List.mapi (fun index stage ->
    let isSelected = selected.View |> View.Map ((=) index)

    Button.primary (
      text stage,
      onClick = (fun () -> Var.Set selected index),
      attrs = [
        Attr.DynamicClassPred "weave-button--filled" isSelected
        Attr.DynamicClassPred
          "weave-button--outlined"
          (isSelected |> View.Map not)
        Attr.Dynamic "aria-pressed" (isSelected |> View.Map string)
      ]
    ))

div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
  div [
    Flex.Flex.allSizes
    FlexWrap.Wrap.allSizes
    JustifyContent.center
    Gap.All.g2
    Attr.Create "role" "group"
    Attr.Create "aria-label" "Project stage"
  ] navigation
  Carousel.create (
    View.Const items,
    activeIndex = selected,
    label = "Project stages",
    attrs = [ Carousel.Controls.hideArrows; Carousel.Controls.hideIndicators ]
  )
]
"""

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText
        "Carousel supports reactive lists. Empty and single-slide lists hide navigation. Selection is clamped for display without rewriting the caller's Var; navigation wraps at either end.")
      [
        Helpers.apiTable "Carousel.create" [
          Helpers.apiParam "items" "View<Carousel.Item list>" "" "Reactive slide collection"
          Helpers.apiParam "?activeIndex" "Var<int>" "Var.Create 0" "Two-way, zero-based selection"
          Helpers.apiParam
            "?autoCycleInterval"
            "int"
            "omitted"
            "Opt into autoplay with this delay in milliseconds (minimum 1000); includes a play/pause control"
          Helpers.apiParam "?label" "string" "Slides" "Accessible name; use a distinct name for each carousel"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Root attributes and styling"
        ]
        Helpers.apiTable "CarouselItem.create" [
          Helpers.apiParam "content" "Doc" "" "Any Weave or WebSharper content"
          Helpers.apiParam
            "?label"
            "string"
            "\"\""
            "Descriptive accessible slide name, in addition to its position"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Slide wrapper attributes"
        ]
        Helpers.returnTypeNote "Carousel.Item"
        Helpers.styleModuleTable "Carousel.Controls" [
          ("hideArrows", "Hide the previous and next buttons")
          ("hideIndicators", "Hide the slide indicators; combine with hideArrows for custom navigation")
        ]
        Helpers.styleModuleTable "Carousel.Transition" [
          ("none", "Disable the default crossfade and indicator motion")
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Carousel"
        Helpers.bodyText "Cycle through content using accessible controls and reactive selection."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        controlledExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
