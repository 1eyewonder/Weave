namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module ProgressExamples =

  let private determinate () =
    let content =
      let value = Var.Create 60
      let progress = value.View |> View.Map float
      let percentage = value.View |> View.Map(sprintf "%d%%")

      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
      ] [
        Slider.create (value, labelText = View.Const "Completion")
        Progress.Linear.create (
          value = progress,
          attrs = [ Attr.Create "aria-label" "Upload"; Progress.Linear.Variant.rounded ]
        )
        Progress.Circular.create (
          value = progress,
          content = textView percentage,
          attrs = [ Attr.Create "aria-label" "Upload"; Progress.Circular.Size.large ]
        )
      ]

    let description =
      Helpers.bodyText
        "Pass a value view to show measurable progress. Move the slider to update both indicators; circular center content is supplied separately."

    Helpers.codeSampleSection
      "Determinate"
      description
      content
      """let value = Var.Create 60
let progress = value.View |> View.Map float
let percentage = value.View |> View.Map(sprintf "%d%%")

Doc.Concat [
  Slider.create (value, labelText = View.Const "Completion")

  Progress.Linear.create (
    value = progress,
    attrs = [
      Attr.Create "aria-label" "Upload"
      Progress.Linear.Variant.rounded
    ]
  )

  Progress.Circular.create (
    value = progress,
    content = textView percentage,
    attrs = [ Attr.Create "aria-label" "Upload"; Progress.Circular.Size.large ]
  )
]
"""

  let private indeterminate () =
    let content =
      let busy = Var.Create true

      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
      ] [
        Switch.create (busy, content = text "Unknown completion")
        Progress.Linear.create (
          value = View.Const 65.0,
          indeterminate = busy.View,
          attrs = [ Attr.Create "aria-label" "Processing" ]
        )
        Progress.Circular.create (attrs = [ Attr.Create "aria-label" "Loading" ])
      ]

    let description =
      Helpers.bodyText
        "Omit value when completion is unknown, or pass an indeterminate view to switch modes. Toggle the switch to see the linear indicator change between activity and 65%. Animations respect reduced motion."

    Helpers.codeSampleSection
      "Indeterminate"
      description
      content
      """let busy = Var.Create true

Doc.Concat [
  Switch.create (busy, content = text "Unknown completion")

  Progress.Linear.create (
    value = View.Const 65.0,
    indeterminate = busy.View,
    attrs = [ Attr.Create "aria-label" "Processing" ]
  )

  Progress.Circular.create (attrs = [ Attr.Create "aria-label" "Loading" ])
]
"""

  let appearance () =
    let state = Var.Create ProgressAppearance.defaults

    let select label choices get set enabled =
      let selected =
        Var.Lens state (get >> Some) (fun current value ->
          match value with
          | Some value -> set current value
          | None -> current)

      let items =
        choices
        |> List.map (fun value -> SelectItem.create (text value, value, value))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        enabled = enabled,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        select
          "Shape"
          ProgressAppearance.shapes
          (fun c -> c.Shape)
          (fun c value -> {
            c with
                Shape = value
                Style = "Plain"
          })
          (View.Const true)
        select
          "Color"
          ProgressAppearance.colors
          (fun c -> c.Color)
          (fun c value -> { c with Color = value })
          (View.Const true)
        select
          "Size"
          ProgressAppearance.sizes
          (fun c -> c.Size)
          (fun c value -> { c with Size = value })
          (View.Const true)
        select
          "Linear style"
          ProgressAppearance.styles
          (fun c -> c.Style)
          (fun c value -> { c with Style = value })
          (state.View |> View.Map(fun c -> c.Shape = "Linear"))
      ]

    let attributes =
      Map.ofList [
        "Progress.Color.secondary", Progress.Color.secondary
        "Progress.Color.tertiary", Progress.Color.tertiary
        "Progress.Color.success", Progress.Color.success
        "Progress.Color.error", Progress.Color.error
        "Progress.Color.warning", Progress.Color.warning
        "Progress.Color.info", Progress.Color.info
        "Progress.Linear.Size.small", Progress.Linear.Size.small
        "Progress.Linear.Size.large", Progress.Linear.Size.large
        "Progress.Circular.Size.small", Progress.Circular.Size.small
        "Progress.Circular.Size.large", Progress.Circular.Size.large
        "Progress.Linear.Variant.rounded", Progress.Linear.Variant.rounded
        "Progress.Linear.Variant.striped", Progress.Linear.Variant.striped
      ]

    let preview =
      state.View
      |> Doc.BindView(fun current ->
        let attrs = [
          Attr.Create "aria-label" "Upload"
          yield!
            ProgressAppearance.attributes current
            |> List.map (fun name -> Map.find name attributes)
        ]

        if current.Shape = "Linear" then
          Progress.Linear.create (value = View.Const 60.0, attrs = attrs)
        else
          Progress.Circular.create (value = View.Const 60.0, attrs = attrs))

    let description =
      Helpers.bodyText
        "Choose a shape, color, and size. Rounded and striped styles apply to linear indicators."

    Helpers.appearanceSection
      [ "colors-and-sizes" ]
      description
      controls
      None
      preview
      (fun () -> Var.Set state ProgressAppearance.defaults)
      (state.View |> View.Map ProgressAppearance.source)

  let private buffering () =
    let content =
      let downloaded = Var.Create 75
      let downloadLabel = downloaded.View |> View.Map(sprintf "Downloaded: %d%%")

      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        Slider.create (downloaded, min = 40, labelText = downloadLabel)
        p [] [ text "Playback: 40%. Downloaded video appears ahead of playback." ]
        Progress.Linear.create (
          value = View.Const 40.0,
          bufferValue = (downloaded.View |> View.Map float),
          attrs = [
            Attr.Create "aria-label" "Video playback"
            Progress.Linear.Size.large
            Progress.Linear.Variant.rounded
          ]
        )
      ]

    let description =
      Helpers.bodyText
        "A buffer shows work prepared ahead of completion. Here playback is at 40%; move the slider to change the downloaded amount. bufferValue is the total prepared amount, not an increment, and is hidden in indeterminate mode."

    Helpers.codeSampleSection
      "Buffering"
      description
      content
      """let downloaded = Var.Create 75
let downloadLabel = downloaded.View |> View.Map(sprintf "Downloaded: %d%%")

Doc.Concat [
  Slider.create (downloaded, min = 40, labelText = downloadLabel)

  Progress.Linear.create (
    value = View.Const 40.0,
    bufferValue = (downloaded.View |> View.Map float),
    attrs = [
      Attr.Create "aria-label" "Video playback"
      Progress.Linear.Size.large
      Progress.Linear.Variant.rounded
    ]
  )
]
"""

  let private ranges () =
    Helpers.textSection "Value Ranges" [
      Helpers.bodyText
        "The default range is 0–100. Set min and max for another range: a value of 150 between 100 and 200 fills half the indicator. Values are clamped to the range; invalid or non-finite ranges fall back to 0–100. Bounds are fixed at creation."
    ]

  let private accessibility () =
    Helpers.textSection "Accessibility" [
      Helpers.bodyText
        "Give each indicator an accessible name with aria-label or aria-labelledby; the default name is Progress. Determinate indicators expose their current value and range, while indeterminate indicators omit the current value. Keep circular center content short and non-interactive."
      Helpers.bodyText
        "For a region waiting on an operation, set aria-busy on the region and use aria-describedby to reference the indicator's ID."
    ]

  let private api () =
    let common = [
      Helpers.apiParam
        "?value"
        "View<float>"
        "Not supplied"
        "Completed work in the range's units. Defaults to the minimum. Omitting this selects indeterminate mode unless explicitly overridden."
      Helpers.apiParam
        "?indeterminate"
        "View<bool>"
        "True when value is omitted"
        "True shows an activity indicator. False displays the current value. Can change reactively."
      Helpers.apiParam
        "?min"
        "float"
        "0"
        "Lower range bound. Fixed at creation. Invalid ranges fall back to 0–100."
      Helpers.apiParam "?max" "float" "100" "Upper range bound. Must exceed the minimum. Fixed at creation."
      Helpers.apiParam
        "?attrs"
        "Attr list"
        "[]"
        "Style and native attributes, including aria-label or aria-labelledby for the accessible name. Applied last."
    ]

    Helpers.apiSection
      (Helpers.bodyText "Both constructors return Doc. All optional parameters are named arguments.")
      [
        Helpers.apiTable
          "Progress.Linear.create"
          (common
           @ [
             Helpers.apiParam
               "?bufferValue"
               "View<float>"
               "Not supplied"
               "Total prepared work from the range minimum, not an increment. Drawn behind the completed fill. Hidden in indeterminate mode."
           ])
        Helpers.apiTable
          "Progress.Circular.create"
          (common
           @ [
             Helpers.apiParam
               "?content"
               "Doc"
               "Not supplied"
               "Optional center content. The caller controls its text and updates."
           ])
        Helpers.styleModuleTable "Progress.Color" [
          "primary", "Primary palette color (default)"
          "secondary", "Secondary palette color"
          "tertiary", "Tertiary palette color"
          "success", "Success palette color"
          "error", "Error palette color"
          "warning", "Warning palette color"
          "info", "Info palette color"
        ]
        Helpers.styleModuleTable "Progress.Linear.Size" [
          "small", "2px track height"
          "medium", "4px track height (default)"
          "large", "8px track height"
        ]
        Helpers.styleModuleTable "Progress.Circular.Size" [
          "small", "24px outer width and height"
          "medium", "40px outer width and height (default)"
          "large", "56px outer width and height"
        ]
        Helpers.styleModuleTable "Progress.Linear.Variant" [
          "rounded", "Uses the theme border radius"
          "striped", "Adds static diagonal stripes with transparent gaps to the completed fill"
        ]
        Helpers.styleModuleTable "CSS Custom Properties" [
          "--progress-color", "Indicator color. Defaults to the primary palette token."
          "--progress-thickness",
          "Linear track height or circular SVG stroke width. Defaults to 4px. Circular strokes scale with the SVG."
          "--progress-size", "Circular outer width and height. Defaults to 40px."
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Progress"
        Helpers.bodyText
          "Linear and circular indicators display the state of an operation. Use measured values when the amount of work is known, or an activity animation when it is unknown."
        Helpers.divider ()
        determinate ()
        Helpers.divider ()
        appearance ()
        Helpers.divider ()
        indeterminate ()
        Helpers.divider ()
        buffering ()
        Helpers.divider ()
        ranges ()
        Helpers.divider ()
        accessibility ()
        Helpers.divider ()
        api ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
