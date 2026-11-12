namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module CarouselAppearance =

  let private slidesSource =
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

"""

  let source arrows indicators transition autoplay selected =
    [
      slidesSource
      if selected <> 0 then
        sprintf "let selected = Var.Create %d" selected
        ""
      "Carousel.create ("
      "  View.Const items,"
      if selected <> 0 then
        "  activeIndex = selected,"
      if autoplay then
        "  autoCycleInterval = 5000,"
      "  label = \"Weave highlights\""
      + (if not arrows || not indicators || not transition then
           ","
         else
           "")
      if not arrows || not indicators || not transition then
        "  attrs = ["

        if not arrows then
          "    Carousel.Controls.hideArrows"

        if not indicators then
          "    Carousel.Controls.hideIndicators"

        if not transition then
          "    Carousel.Transition.none"

        "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for arrows in [ false; true ] do
      for indicators in [ false; true ] do
        for transition in [ false; true ] do
          for autoplay in [ false; true ] do
            for selected in 0..2 do
              sprintf "%b %b %b %b slide %d" arrows indicators transition autoplay selected,
              source arrows indicators transition autoplay selected
  ]
