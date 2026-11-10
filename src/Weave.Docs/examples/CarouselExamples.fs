namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module CarouselExamples =

  let private caption value =
    span [ Typography.overline ] [ text value ]

  let private miniCard title detail =
    Card.create (
      CardContent.create (
        div [] [
          div [ Typography.subtitle2 ] [ text title ]
          div [ Typography.caption; Attr.Class "docs-carousel__muted" ] [ text detail ]
        ]
      )
    )

  let private boardArt () =
    div [ Attr.Class "docs-carousel__board" ] [
      for title, cards in
        [
          "Planned", [ "A fresh start"; "Explore ideas" ]
          "In progress", [ "Make it yours" ]
          "Ready", [ "Ship something good" ]
        ] do
        div [ Attr.Class "docs-carousel__column" ] [
          caption title
          for name in cards do
            miniCard name "Workspace / Today"
        ]
    ]

  let private paletteArt () =
    div [ Attr.Class "docs-carousel__palette" ] [
      for name, color in
        [
          "Primary", "primary"
          "Secondary", "secondary"
          "Tertiary", "tertiary"
          "Success", "success"
        ] do
        div [ Attr.Class "docs-carousel__swatch" ] [
          div [ Attr.Style "background" (sprintf "var(--palette-%s)" color) ] []
          span [ Typography.caption ] [ text name ]
        ]
      div [ Attr.Class "docs-carousel__palette-note" ] [
        Chip.create (text "Aa", attrs = [ Chip.Color.primary; Chip.Variant.outlined ])
        div [] [
          div [ Typography.subtitle2 ] [ text "One palette. Every component." ]
          div [ Typography.caption; Attr.Class "docs-carousel__muted" ] [
            text "Light and dark, already connected."
          ]
        ]
      ]
    ]

  let private activityArt () =
    div [ Attr.Class "docs-carousel__activity" ] [
      div [ Attr.Class "docs-carousel__art-heading" ] [
        caption "This week"
        Chip.create (text "+24%", attrs = [ Chip.Color.success; Chip.Variant.outlined ])
      ]
      div [ Attr.Class "docs-carousel__bars" ] [
        for height in [ 35; 55; 42; 72; 62; 88; 100 ] do
          div [ Attr.Style "height" (sprintf "%d%%" height) ] []
      ]
      div [ Attr.Class "docs-carousel__art-heading"; Typography.caption ] [ text "Monday"; text "Sunday" ]
    ]

  let private featureSlide number category title description accent art =
    CarouselItem.create (
      div [
        Attr.Class "docs-carousel__feature"
        Attr.Style "--demo-accent" (sprintf "var(--palette-%s-rgb)" accent)
      ] [
        div [ Attr.Class "docs-carousel__copy" ] [
          div [ Attr.Class "docs-carousel__eyebrow" ] [
            span [ Attr.Class "docs-carousel__number"; Typography.caption ] [ text number ]
            caption category
          ]
          h3 [ Typography.h4 ] [ text title ]
          p [ Typography.body1; Attr.Class "docs-carousel__muted" ] [ text description ]
          div [ Attr.Class "docs-carousel__tags" ] [
            Chip.create (text "Built with Weave", attrs = [ Chip.Variant.outlined ])
            span [ Typography.caption; Attr.Class "docs-carousel__muted" ] [
              text "A few components. Plenty of possibilities."
            ]
          ]
        ]
        div [ Attr.Class "docs-carousel__art"; Attr.Create "aria-hidden" "true" ] [ art ]
      ],
      label = category
    )

  let private slides () = [
    featureSlide
      "01"
      "Your workspace"
      "Less clutter. More flow."
      "Turn a handful of cards into a space where your next idea can take shape."
      "primary"
      (boardArt ())
    featureSlide
      "02"
      "Your palette"
      "A little color goes a long way."
      "Give every surface a shared language, from the smallest chip to the whole workspace."
      "tertiary"
      (paletteArt ())
    featureSlide
      "03"
      "Your momentum"
      "See the progress you're making."
      "Bring the useful numbers into focus and leave room for what comes next."
      "info"
      (activityArt ())
  ]

  let private basicExample () =
    Helpers.codeSampleSection
      "Basic Carousel"
      (Helpers.bodyText
        "A small feature showcase, composed from cards, chips, and typography. Browse with the arrows or indicators, or focus the slides and use Left/Right.")
      (Carousel.create (View.Const(slides ()), label = "Weave highlights"))
      """let slides = [
    CarouselItem.create(CardContent.create(text "First slide"), label = "Welcome")
    CarouselItem.create(CardContent.create(text "Second slide"), label = "Explore")
]

Carousel.create(View.Const slides, label = "Highlights")"""

  let private controlledExample () =
    let selected = Var.Create 0

    let stages = [
      "Design",
      "Find the right shape.",
      "Explore the layout, settle on a palette, and make the important things easy to find.",
      "12",
      "Components",
      "4",
      "Color roles",
      "primary"
      "Build",
      "Make the pieces work together.",
      "Connect your state, compose the interactions, and watch the workspace come to life.",
      "8",
      "Interactions",
      "3",
      "Shared views",
      "info"
      "Ship",
      "Ready for the next chapter.",
      "Check the details, share what you've made, and leave a little room for the next idea.",
      "24",
      "Checks passed",
      "0",
      "Loose ends",
      "success"
    ]

    let stageSlides =
      stages
      |> List.mapi (fun i (name, title, body, first, firstLabel, second, secondLabel, accent) ->
        CarouselItem.create (
          div [
            Attr.Class "docs-carousel__snapshot"
            Attr.Style "--demo-accent" (sprintf "var(--palette-%s-rgb)" accent)
          ] [
            div [ Attr.Class "docs-carousel__snapshot-heading" ] [
              caption (sprintf "Project notebook / %s" name)
              span [ Typography.caption ] [ text (sprintf "0%d / 03" (i + 1)) ]
            ]
            h3 [ Typography.h4 ] [ text title ]
            p [ Typography.body1; Attr.Class "docs-carousel__muted" ] [ text body ]
            div [ Attr.Class "docs-carousel__stats" ] [
              for value, label in [ first, firstLabel; second, secondLabel ] do
                Card.create (
                  CardContent.create (
                    div [] [
                      div [ Typography.h4 ] [ text value ]
                      div [ Typography.caption; Attr.Class "docs-carousel__muted" ] [ text label ]
                    ]
                  )
                )
            ]
          ],
          label = name
        ))

    Helpers.codeSampleSection
      "Controlled Selection"
      (Helpers.bodyText
        "One project, three stages. Share selection with your own buttons and hide the built-in controls using Carousel.Controls attributes. Keyboard navigation remains available.")
      (div [] [
        div [
          Attr.Class "docs-carousel__stage-picker"
          Attr.Create "role" "group"
          Attr.Create "aria-label" "Project stage"
        ] [
          for i, name in [ 0, "Design"; 1, "Build"; 2, "Ship" ] do
            Button.create (
              text name,
              (fun () -> selected.Set i),
              attrs = [
                Button.Color.primary
                Button.Variant.outlined
                selected.View
                |> View.Map((=) i)
                |> Attr.DynamicClassPred "docs-carousel__stage--selected"
                selected.View
                |> View.Map(fun s -> string<bool>(s = i))
                |> Attr.Dynamic "aria-pressed"
              ]
            )
        ]
        Carousel.create (
          View.Const stageSlides,
          activeIndex = selected,
          label = "Project notebook",
          attrs = [ Carousel.Controls.hideArrows; Carousel.Controls.hideIndicators ]
        )
      ])
      """let selected = Var.Create 0
Carousel.create(
    View.Const slides,
    activeIndex = selected,
    label = "Project notebook",
    attrs = [ Carousel.Controls.hideArrows; Carousel.Controls.hideIndicators ]
)
Button.create(text "Design", fun () -> selected.Set 0)
Button.create(text "Build", fun () -> selected.Set 1)
Button.create(text "Ship", fun () -> selected.Set 2)"""

  let private autoCycleExample () =
    let notes = [
      "01",
      "Small details, better days.",
      "A clearer focus ring. A quieter surface. The little refinements that make a workspace feel right.",
      "Design notes",
      "primary"
      "02",
      "A rhythm you can settle into.",
      "Consistent spacing and thoughtful defaults keep the interface out of the way of your work.",
      "From the studio",
      "tertiary"
      "03",
      "Make room for what's next.",
      "Start with the essentials, add your own ideas, and let the next iteration surprise you.",
      "Looking ahead",
      "info"
    ]

    let updates =
      notes
      |> List.map (fun (number, title, body, category, accent) ->
        CarouselItem.create (
          div [
            Attr.Class "docs-carousel__editorial"
            Attr.Style "--demo-accent" (sprintf "var(--palette-%s-rgb)" accent)
          ] [
            div [
              Attr.Class "docs-carousel__editorial-number"
              Attr.Create "aria-hidden" "true"
            ] [ text number ]
            div [ Attr.Class "docs-carousel__editorial-copy" ] [
              Chip.create (text category, attrs = [ Chip.Variant.outlined ])
              h3 [ Typography.h4 ] [ text title ]
              p [ Typography.body1; Attr.Class "docs-carousel__muted" ] [ text body ]
              span [ Typography.caption ] [ text "The Weave notebook · A moment of inspiration" ]
            ]
          ],
          label = category
        ))

    Helpers.codeSampleSection
      "Automatic Cycling"
      (Helpers.bodyText
        "Autoplay is opt-in. It pauses while hovered or the browser tab is hidden, and stops when slides or navigation receive focus. Use Play slides to resume. Reduced-motion preferences start it paused and disable the fade. Removing the carousel releases its timer.")
      (Carousel.create (View.Const updates, autoCycleInterval = 5000, label = "The Weave notebook"))
      """Carousel.create(
    View.Const slides,
    autoCycleInterval = 5000,
    label = "Highlights"
)"""

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
        controlledExample ()
        Helpers.divider ()
        autoCycleExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
