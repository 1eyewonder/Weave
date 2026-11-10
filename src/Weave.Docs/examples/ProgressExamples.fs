namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module ProgressExamples =

  let private paragraphs children =
    div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] children

  let private determinate () =
    let value = Var.Create 60
    let progress = value.View |> View.Map float

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        Slider.create (value, labelText = View.Const "Completion")
        Progress.Linear.create (
          value = progress,
          attrs = [ Attr.Create "aria-label" "Upload"; Progress.Linear.Variant.rounded ]
        )
        Progress.Circular.create (
          value = progress,
          content = textView (value.View |> View.Map(fun v -> string v + "%")),
          attrs = [ Attr.Create "aria-label" "Upload"; Progress.Circular.Size.large ]
        )
      ]

    let description =
      paragraphs [
        Helpers.bodyText
          "Determinate progress represents a known amount of completed work. For example, uploading 60 of 100 files fills 60% of the bar or ring. It does not estimate time remaining."
        div [ Typography.body1 ] [
          text "Pass a "
          Helpers.inlineCode "View<float>"
          text " to "
          Helpers.inlineCode "value"
          text
            ". The component reads this view and updates its fill and accessible value when it changes. It does not modify your state or advance automatically. Use "
          Helpers.inlineCode "View.Const"
          text " for a fixed value, or a variable's view for live progress."
        ]
        Helpers.bodyText
          "Move the slider to update both indicators. Circular center content is supplied separately, so the caller controls its text and formatting."
      ]

    Helpers.codeSampleSection
      "Determinate"
      description
      content
      """open WebSharper.UI
open WebSharper.UI.Html
open Weave

let completedFiles = Var.Create 60
let progress = completedFiles.View |> View.Map float

// Updating completedFiles updates both indicators.
div [] [
    Progress.Linear.create(
        value = progress,
        attrs = [
            Attr.Create "aria-label" "Files uploaded"
        ]
    )
    Progress.Circular.create(
        value = progress,
        content = textView (
            completedFiles.View |> View.Map(fun count -> string count + "%")
        ),
        attrs = [
            Attr.Create "aria-label" "Files uploaded"
            Progress.Circular.Size.large
        ]
    )
]"""

  let private indeterminate () =
    let busy = Var.Create true

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        Switch.create (busy, content = text "Unknown completion")
        Progress.Linear.create (
          value = View.Const 65.0,
          indeterminate = busy.View,
          attrs = [ Attr.Create "aria-label" "Processing" ]
        )
        div [] [ Progress.Circular.create (attrs = [ Attr.Create "aria-label" "Loading" ]) ]
      ]

    let description =
      paragraphs [
        Helpers.bodyText
          "Indeterminate progress means an operation is running but its completion cannot be measured. The moving segment is an activity indicator. Its position and length do not represent a percentage."
        div [ Typography.body1 ] [
          text "Omitting "
          Helpers.inlineCode "value"
          text
            " selects indeterminate mode by default. To change modes without rebuilding the component, pass a "
          Helpers.inlineCode "View<bool>"
          text " to "
          Helpers.inlineCode "indeterminate"
          text ". When true, it hides the measured fill and removes "
          Helpers.inlineCode "aria-valuenow"
          text
            ". When false, it displays the latest value. With no value supplied, false displays the range minimum."
        ]
        Helpers.bodyText
          "Toggle the switch to change the linear indicator between unknown completion and 65%. The circular indicator stays indeterminate. Both animations stop when the user requests reduced motion."
      ]

    Helpers.codeSampleSection
      "Indeterminate"
      description
      content
      """open WebSharper.UI
open WebSharper.UI.Html
open Weave

let unknownCompletion = Var.Create true

// Change unknownCompletion.Value to false once progress can be measured.
div [] [
    Progress.Linear.create(
        value = View.Const 65.0,
        indeterminate = unknownCompletion.View,
        attrs = [
            Attr.Create "aria-label" "Processing"
        ]
    )
    Progress.Circular.create(
        attrs = [
            Attr.Create "aria-label" "Loading"
        ]
    )
]"""

  let private colorsAndSizes () =
    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        for name, color in
          [
            "Primary", Progress.Color.primary
            "Secondary", Progress.Color.secondary
            "Tertiary", Progress.Color.tertiary
            "Success", Progress.Color.success
            "Error", Progress.Color.error
            "Warning", Progress.Color.warning
            "Info", Progress.Color.info
          ] do
          div [] [
            div [ Typography.caption; Margin.Bottom.extraSmall ] [ text name ]
            div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g4 ] [
              Progress.Linear.create (value = View.Const 60.0, attrs = [ color; Progress.Linear.Size.large ])
              Progress.Circular.create (value = View.Const 60.0, attrs = [ color ])
            ]
          ]
        div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g4 ] [
          for name, size in
            [
              "Small", Progress.Circular.Size.small
              "Medium", Progress.Circular.Size.medium
              "Large", Progress.Circular.Size.large
            ] do
            div [] [
              div [ Typography.caption; Margin.Bottom.extraSmall ] [ text name ]
              Progress.Circular.create (value = View.Const 75.0, attrs = [ size ])
            ]
        ]
      ]

    let description =
      paragraphs [
        div [ Typography.body1 ] [
          text "Apply "
          Helpers.inlineCode "Progress.Color"
          text
            " attributes to either shape. They use Weave's palette tokens and follow the current light or dark theme. The default color is primary."
        ]
        Helpers.bodyText
          "Linear indicators fill their container's width. Their small, medium, and large sizes set the track height to 2px, 4px, and 8px. Circular sizes set the outer width and height to 24px, 40px, and 56px. Medium is the default for both shapes."
        div [ Typography.body1 ] [
          Helpers.inlineCode "Progress.Linear.Variant.rounded"
          text " applies the theme border radius. "
          Helpers.inlineCode "Progress.Linear.Variant.striped"
          text
            " adds static diagonal stripes to the completed fill. The gaps are transparent, revealing the track or buffered fill blended over the surrounding surface. Neither variant changes the value or mode."
        ]
      ]

    Helpers.codeSampleSection
      "Colors and Sizes"
      description
      content
      """open WebSharper.UI
open WebSharper.UI.Html
open Weave

div [] [
    Progress.Linear.create(
        value = View.Const 60.0,
        attrs = [
            Progress.Color.success
            Progress.Linear.Size.large
            Progress.Linear.Variant.rounded
        ]
    )
    Progress.Circular.create(
        value = View.Const 60.0,
        attrs = [
            Progress.Color.secondary
            Progress.Circular.Size.small
        ]
    )
]"""

  let private buffering () =
    let downloaded = Var.Create 75

    let content =
      div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g4 ] [
        Slider.create (
          downloaded,
          min = 40,
          labelText = (downloaded.View |> View.Map(fun value -> "Downloaded: " + string value + "%"))
        )
        Helpers.bodyText
          "Playback position: 40%. The solid bar shows playback. The lighter bar shows downloaded video."
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
      paragraphs [
        Helpers.bodyText
          "Buffering represents work prepared ahead of the work already completed. In a video player, playback might be at 40% while 75% of the video has downloaded. The solid fill ends at 40%, the lighter buffered fill extends to 75%, and the remaining track represents video that has not downloaded."
        div [ Typography.body1 ] [
          text "Set "
          Helpers.inlineCode "bufferValue"
          text
            " to the total amount prepared, measured from the range minimum. It is not an additional amount beyond "
          Helpers.inlineCode "value"
          text
            ". In this example, pass 75, not 35. Both parameters accept independent views and use the same range. Weave only displays these values. It does not download data, reserve memory, or manage a buffer."
        ]
        Helpers.bodyText
          "Use buffering when your operation has separate preparation and completion stages. For an ordinary upload or task with only one progress measurement, omit the buffered value."
        Helpers.bodyText
          "The buffered fill is clamped to the range and never ends before the completed fill. It is hidden in indeterminate mode. It is visual context only: assistive technology receives the completed value, not a second buffered value."
      ]

    Helpers.codeSampleSection
      "Buffering"
      description
      content
      """open WebSharper.UI
open Weave

let downloadedPercent = Var.Create 75.0

Progress.Linear.create(
    value = View.Const 40.0,
    bufferValue = downloadedPercent.View,
    attrs = [
        Attr.Create "aria-label" "Video playback"
        Progress.Linear.Size.large
        Progress.Linear.Variant.rounded
    ]
)"""

  let private ranges () =
    let content =
      Progress.Linear.create (
        value = View.Const 150.0,
        min = 100.0,
        max = 200.0,
        attrs = [
          Attr.Create "aria-label" "Batch processing"
          Progress.Color.info
          Progress.Linear.Size.large
          Progress.Linear.Variant.striped
        ]
      )

    let description =
      paragraphs [
        Helpers.bodyText
          "The default range is 0 to 100. Custom ranges use the same units as your input data. For a stage that starts at item 100 and ends at item 200, a current value of 150 fills half the indicator."
        div [ Typography.body1 ] [
          text "The fill fraction is "
          Helpers.inlineCode "(value - min) / (max - min)"
          text
            ". The accessible value remains in the original units, so this example reports a current value of 150 with a minimum of 100 and maximum of 200."
        ]
        Helpers.bodyText
          "Values below the minimum display as empty. Values above the maximum display as full. NaN is treated as the minimum, and positive or negative infinity is clamped to the corresponding bound. The same rules apply to buffered values."
        Helpers.bodyText
          "The range must have finite bounds, a maximum greater than the minimum, and a finite difference between them. Otherwise, both bounds fall back to 0 and 100. Range bounds are fixed when the component is created."
      ]

    Helpers.codeSampleSection
      "Value Ranges"
      description
      content
      """open WebSharper.UI
open Weave

Progress.Linear.create(
    value = View.Const 150.0,
    min = 100.0,
    max = 200.0,
    attrs = [
        Attr.Create "aria-label" "Batch processing"
        Progress.Color.info
        Progress.Linear.Size.large
        Progress.Linear.Variant.striped
    ]
)"""

  let private accessibility () =
    Helpers.textSection "Accessibility" [
      div [ Typography.body1 ] [
        text "Both shapes render a non-interactive "
        Helpers.inlineCode "progressbar"
        text " role. Supply a descriptive "
        Helpers.inlineCode "aria-label"
        text
          " through attrs to identify the operation, or use aria-labelledby to reference visible text. These native attributes set the accessible name. They do not render visible text. The default name is Progress."
      ]
      Helpers.bodyText
        "Determinate indicators expose the clamped current value and range bounds. Indeterminate indicators omit the current value. Circular center content is presentation inside the progressbar, so keep it short and non-interactive. It does not replace the accessible name."
      div [ Typography.body1 ] [
        text "For a region waiting on the operation, set "
        Helpers.inlineCode "aria-busy"
        text " on that region and use "
        Helpers.inlineCode "aria-describedby"
        text
          " to reference the progressbar's ID. Weave does not mark other regions busy or announce each value change through a live region."
      ]
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
        indeterminate ()
        Helpers.divider ()
        colorsAndSizes ()
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
