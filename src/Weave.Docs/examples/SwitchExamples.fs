namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave

[<JavaScript>]
module SwitchExamples =

  let private basicSwitchExample () =
    let description =
      Helpers.bodyText "A simple switch with a label and reactive state output."

    let content =
      let basicIsChecked = Var.Create false

      div [] [
        basicIsChecked.View
        |> View.MapCached(sprintf "Basic is Checked: %b")
        |> View.printfn
        Switch.create (basicIsChecked, div [ Typography.body1 ] [ text "Default Switch" ])
      ]

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

Switch.create(isChecked, div [ Typography.body1 ] [ text "Default Switch" ])"""

    Helpers.codeSampleSection "Basic Switch" description content code

  let private disabledSwitchExample () =
    let description =
      Helpers.bodyText "A switch that is disabled and cannot be toggled."

    let content =
      let isChecked = Var.Create true

      Switch.create (
        isChecked,
        div [ Typography.body1 ] [ text "Disabled Switch" ],
        enabled = View.Const false
      )

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create true

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Disabled Switch" ],
    enabled = View.Const false
)"""

    Helpers.codeSampleSection "Disabled Switch" description content code

  let private switchWithDynamicLabel () =
    let description =
      Helpers.bodyText "A switch with a label that updates based on its state."

    let content =
      let isChecked = Var.Create false

      let label =
        isChecked.View
        |> View.Map(fun v -> if v then "I am on!" else "Turn me on!")
        |> Doc.BindView(fun t -> div [ Typography.body1 ] [ text t ])

      Switch.create (isChecked, label)

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

let label =
    isChecked.View
    |> View.Map(fun v -> if v then "I am on!" else "Turn me on!")
    |> Doc.BindView(fun t -> div [ Typography.body1 ] [ text t ])

Switch.create(isChecked, label)"""

    Helpers.codeSampleSection "Dynamic Label" description content code

  let private switchSizesExample () =
    let description =
      Helpers.bodyText "Switches come in three sizes: Small, Medium, and Large."

    let content =
      let sizes = [
        Switch.Size.small, "Small", Var.Create false
        Switch.Size.medium, "Medium", Var.Create true
        Switch.Size.large, "Large", Var.Create false
      ]

      Grid.create (
        sizes
        |> List.map (fun (size, label, v) ->
          GridItem.create (
            Switch.create (v, div [ Typography.body1 ] [ text label ], attrs = [ size ]),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Small" ],
    attrs = [ Switch.Size.small ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Medium" ],
    attrs = [ Switch.Size.medium ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Large" ],
    attrs = [ Switch.Size.large ]
)"""

    Helpers.codeSampleSection "Sizes" description content code

  let private switchColorsExample () =
    let description =
      Helpers.bodyText "Apply a brand color to the switch track and thumb using the Switch.Color module."

    let content =
      let switches =
        [
          Switch.Color.primary, "Primary"
          Switch.Color.secondary, "Secondary"
          Switch.Color.tertiary, "Tertiary"
          Switch.Color.error, "Error"
          Switch.Color.warning, "Warning"
          Switch.Color.success, "Success"
          Switch.Color.info, "Info"
        ]
        |> List.map (fun (colorAttr, label) -> Var.Create false, colorAttr, label)

      Grid.create (
        switches
        |> List.map (fun (v, colorAttr, label) ->
          GridItem.create (
            Switch.create (v, div [ Typography.body1 ] [ text label ], attrs = [ colorAttr ]),
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Primary" ],
    attrs = [ Switch.Color.primary ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Secondary" ],
    attrs = [ Switch.Color.secondary ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Tertiary" ],
    attrs = [ Switch.Color.tertiary ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Error" ],
    attrs = [ Switch.Color.error ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Warning" ],
    attrs = [ Switch.Color.warning ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Success" ],
    attrs = [ Switch.Color.success ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Info" ],
    attrs = [ Switch.Color.info ]
)"""

    Helpers.codeSampleSection "Colors" description content code

  let private contentPlacementExample () =
    let description =
      Helpers.bodyText
        "Change the label position using the ContentPlacement module. Available placements are right (default), left, top, and bottom."

    let content =
      let placements = [
        Switch.ContentPlacement.left, "Left"
        Switch.ContentPlacement.right, "Right"
        Switch.ContentPlacement.top, "Top"
        Switch.ContentPlacement.bottom, "Bottom"
      ]

      Grid.create (
        placements
        |> List.map (fun (placementAttr, label) ->
          GridItem.create (
            Switch.create (
              Var.Create false,
              div [ Typography.body1 ] [ text label ],
              attrs = [ Switch.Size.large; Switch.Color.primary; placementAttr ]
            ),
            attrs = [ GridItem.Span.six; GridItem.Span.Medium.three ]
          ))
      )

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Left" ],
    attrs = [ Switch.ContentPlacement.left; Switch.Color.primary ] // see here
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Right" ],
    attrs = [ Switch.ContentPlacement.right; Switch.Color.primary ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Top" ],
    attrs = [ Switch.ContentPlacement.top; Switch.Color.primary ]
)

Switch.create(
    isChecked,
    div [ Typography.body1 ] [ text "Bottom" ],
    attrs = [ Switch.ContentPlacement.bottom; Switch.Color.primary ]
)"""

    Helpers.codeSampleSection "Content Placement" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls the touch-target padding on a three-step scale: Compact, Standard, and Spacious. Pass the density class in attrs to override a single instance."

    let content =
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          Switch.create (
            Var.Create false,
            div [ Typography.body1 ] [ text "Off" ],
            attrs = [ Switch.Color.primary ]
          )
          Switch.create (
            Var.Create true,
            div [ Typography.body1 ] [ text "On" ],
            attrs = [ Switch.Color.primary ]
          )
        ]

      Grid.create (
        [
          GridItem.create (
            col "Compact" Density.compact,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Standard" Density.standard,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
        ],
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave
open WebSharper.UI

let isChecked = Var.Create false

// Compact density
div [ Density.compact ] [
    Switch.create(
        isChecked,
        div [ Typography.body1 ] [ text "Compact" ],
        attrs = [ Switch.Color.primary ]
    )
]

// Standard density (the default)
div [ Density.standard ] [
    Switch.create(
        isChecked,
        div [ Typography.body1 ] [ text "Standard" ],
        attrs = [ Switch.Color.primary ]
    )
]

// Spacious density
div [ Density.spacious ] [
    Switch.create(
        isChecked,
        div [ Typography.body1 ] [ text "Spacious" ],
        attrs = [ Switch.Color.primary ]
    )
]"""

    Helpers.codeSampleSection "Density" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Switch.") [
      Helpers.apiTable "Switch.create" [
        Helpers.apiParam "isChecked" "Var<bool>" "" "Two-way binding for the on/off state"
        Helpers.apiParam "?content" "Doc" "" "Optional label content displayed next to the switch"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the switch is interactive"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (size, color, content placement, etc.)"
      ]

      Helpers.styleModuleTable "Switch.Size" [
        ("small", "Small switch")
        ("medium", "Medium switch (default)")
        ("large", "Large switch")
      ]

      Helpers.styleModuleTable "Switch.Color" [
        ("primary", "Primary brand color when on")
        ("secondary", "Secondary brand color when on")
        ("tertiary", "Tertiary brand color when on")
        ("error", "Error/red color when on")
        ("warning", "Warning/orange color when on")
        ("success", "Success/green color when on")
        ("info", "Info/blue color when on")
      ]

      Helpers.styleModuleTable "Switch.ContentPlacement" [
        ("right", "Label to the right of the switch (default)")
        ("left", "Label to the left")
        ("top", "Label above the switch")
        ("bottom", "Label below the switch")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Switch"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "The Switch component allows users to toggle between two states. It supports different sizes, colors, and can be disabled."
        ]

        Helpers.divider ()
        basicSwitchExample ()
        Helpers.divider ()
        disabledSwitchExample ()
        Helpers.divider ()
        switchWithDynamicLabel ()
        Helpers.divider ()
        switchSizesExample ()
        Helpers.divider ()
        switchColorsExample ()
        Helpers.divider ()
        contentPlacementExample ()
        Helpers.divider ()
        densityExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
