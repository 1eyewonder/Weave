namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ButtonExamples =

  let private variantExamples () =
    let description =
      Helpers.bodyText "Buttons come in three variants: Filled, Outlined, and Text"

    let content =
      Grid.create (
        [
          GridItem.create (
            Button.primary (text "Filled", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
          )
          GridItem.create (
            Button.primary (text "Outlined", onClick = (fun () -> ()), attrs = [ Button.Variant.outlined ])
          )
          GridItem.create (
            Button.primary (text "Text", onClick = (fun () -> ()), attrs = [ Button.Variant.text ])
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


let doNothing () = ()

Button.primary(
    text "Filled",
    onClick = doNothing,
    attrs = [
        Button.Variant.filled // see here
    ]
)

Button.primary(
    text "Outlined",
    onClick = doNothing,
    attrs = [
        Button.Variant.outlined // see here
    ]
)

Button.primary(
    text "Text",
    onClick = doNothing,
    attrs = [
        Button.Variant.text // see here
    ]
)
    """

    Helpers.codeSampleSection "Variants" description content code

  let private colorExamples () =
    let colors = [
      "Primary", Button.Color.primary
      "Secondary", Button.Color.secondary
      "Tertiary", Button.Color.tertiary
      "Error", Button.Color.error
      "Warning", Button.Color.warning
      "Success", Button.Color.success
      "Info", Button.Color.info
    ]

    let description = Helpers.bodyText "Buttons support all theme colors"

    let code =
      """open Weave


let onClick name = printfn "%s clicked" name

let colors = [
    "Primary", Button.Color.primary
    "Secondary", Button.Color.secondary
    "Tertiary", Button.Color.tertiary
    "Error", Button.Color.error
    "Warning", Button.Color.warning
    "Success", Button.Color.success
    "Info", Button.Color.info
]

colors
|> List.map (fun (colorName, colorAttr) ->

    Button.create(
        text colorName,
        onClick = onClick colorName,
        attrs = [
            Button.Variant.filled
            colorAttr // see here
            Button.Width.full
        ]
    )
)
"""

    let content =
      Grid.create (
        colors
        |> List.collect (fun (colorName, colorAttr) -> [
          GridItem.create (
            Button.create (
              text colorName,
              onClick = (fun () -> printfn "%s clicked" colorName),
              attrs = [ Button.Variant.filled; colorAttr; Button.Width.full ]
            )
          )
        ])
      )

    Helpers.codeSampleSection "Colors" description content code

  let private densityExamples () =
    let description =
      Helpers.bodyText
        "Density controls button height and padding. Pass the density class in attrs to set it per-instance. See the Density section on the Weave Styling page for container-level usage."

    let content =
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          div [] [
            Button.primary (text "Filled", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
          ]
          div [ Margin.Top.extraSmall ] [
            Button.primary (text "Outlined", onClick = (fun () -> ()), attrs = [ Button.Variant.outlined ])
          ]
          div [ Margin.Top.extraSmall ] [
            Button.primary (text "Text", onClick = (fun () -> ()), attrs = [ Button.Variant.text ])
          ]
        ]

      Grid.create (
        [
          GridItem.create (col "Compact" Density.compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.create (
            col "Standard" Density.standard,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave


Button.primary(
    text "Compact",
    onClick = (fun () -> ()),
    attrs = [
        Density.compact // see here
        Button.Variant.filled
    ]
)

Button.primary(
    text "Standard",
    onClick = (fun () -> ()),
    attrs = [
        Density.standard // see here
        Button.Variant.filled
    ]
)

Button.primary(
    text "Spacious",
    onClick = (fun () -> ()),
    attrs = [
        Density.spacious // see here
        Button.Variant.filled
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let private disabledExamples () =
    let description =
      Helpers.bodyText "Buttons can be disabled using the enabled parameter"

    let content =
      Grid.create (
        [
          GridItem.create (
            Button.primary (
              text "Enabled",
              onClick = (fun () -> printfn "Enabled clicked"),
              enabled = View.Const true,
              attrs = [ Button.Variant.filled ]
            )
          )
          GridItem.create (
            Button.create (
              text "Disabled",
              onClick = (fun () -> printfn "This won't fire"),
              enabled = View.Const false,
              attrs = [ Button.Variant.filled ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


Button.primary(
    text "Enabled",
    onClick = (fun () -> printfn "Enabled clicked"),
    enabled = View.Const true, // see here
    attrs = [
        Button.Variant.filled
    ]
)

Button.create(
    text "Disabled",
    onClick = (fun () -> printfn "This won't fire"),
    enabled = View.Const false, // see here
    attrs = [
        Button.Variant.filled
    ]
)
"""

    Helpers.codeSampleSection "Disabled State" description content code

  let private fullWidthExample () =
    let description =
      Helpers.bodyText "Buttons can span the full width of their container"

    let content =
      Button.primary (
        text "Full Width Button",
        onClick = (fun () -> printfn "Full width clicked"),
        attrs = [ Button.Variant.filled; Button.Width.full ]
      )

    let code =
      """open Weave

open WebSharper.UI

Button.primary(
    text "Full Width Button",
    onClick = (fun () -> printfn "Full width clicked"),
    attrs = [
        Button.Variant.filled
        Button.Width.full // see here
    ]
)
"""

    Helpers.codeSampleSection "Full Width" description content code

  let private borderRadiusExamples () =
    let description = Helpers.bodyText "Buttons can have different border radius styles"

    let content =
      Grid.create (
        [
          let btn label radiusAttr =
            GridItem.create (
              Button.primary (
                text label,
                onClick = (fun () -> ()),
                attrs = [ Button.Variant.filled; radiusAttr ]
              )
            )

          btn "None" BorderRadius.All.none
          btn "Small" BorderRadius.All.small
          btn "Medium" BorderRadius.All.medium
          btn "Large" BorderRadius.All.large
          btn "Pill" BorderRadius.pill
          btn "Circle" BorderRadius.circle
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


Button.primary(text "None",   onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.All.none   ]) // see here
Button.primary(text "Small",  onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.All.small  ])
Button.primary(text "Medium", onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.All.medium ])
Button.primary(text "Large",  onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.All.large  ])
Button.primary(text "Pill",   onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.pill       ])
Button.primary(text "Circle", onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.circle     ])
"""

    Helpers.codeSampleSection "Border Radius" description content code

  let private iconButtonExamples () =
    let description =
      Helpers.bodyText
        "Icon buttons render a single icon without a text label. Use IconButton.create to create them. They reuse the same variant, color, and size classes as regular buttons."

    let content =
      Grid.create (
        [
          GridItem.create (
            IconButton.error (
              Icon.create (Icon.UiActions UiActions.Delete),
              onClick = (fun () -> printfn "delete clicked"),
              attrs = [
                Attr.Create "aria-label" "delete"
                Button.Variant.filled
                BorderRadius.circle
              ]
            )
          )
          GridItem.create (
            IconButton.secondary (
              Icon.create (Icon.UiActions UiActions.Favorite),
              onClick = (fun () -> printfn "favorite clicked"),
              attrs = [
                Attr.Create "aria-label" "favorite"
                Button.Variant.filled
                BorderRadius.circle
              ]
            )
          )
          GridItem.create (
            IconButton.primary (
              Icon.create (Icon.UiActions UiActions.Home),
              onClick = (fun () -> printfn "home clicked"),
              attrs = [ Attr.Create "aria-label" "home"; Button.Variant.filled; BorderRadius.circle ]
            )
          )
          GridItem.create (
            IconButton.info (
              Icon.create (Icon.UiActions UiActions.Search),
              onClick = (fun () -> printfn "search clicked"),
              attrs = [
                Attr.Create "aria-label" "search"
                Button.Variant.filled
                BorderRadius.circle
              ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


IconButton.error(
    Icon.create(Icon.UiActions UiActions.Delete),
    onClick = (fun () -> printfn "delete clicked"),
    attrs = [
        Attr.Create "aria-label" "delete" // see here
        Button.Variant.filled
        BorderRadius.circle
    ]
)

IconButton.secondary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> printfn "favorite clicked"),
    attrs = [
        Attr.Create "aria-label" "favorite"
        Button.Variant.filled
        BorderRadius.circle
    ]
)

IconButton.primary(
    Icon.create(Icon.UiActions UiActions.Home),
    onClick = (fun () -> printfn "home clicked"),
    attrs = [
        Attr.Create "aria-label" "home"
        Button.Variant.filled
        BorderRadius.circle
    ]
)

IconButton.info(
    Icon.create(Icon.UiActions UiActions.Search),
    onClick = (fun () -> printfn "search clicked"),
    attrs = [
        Attr.Create "aria-label" "search"
        Button.Variant.filled
        BorderRadius.circle
    ]
)
"""

    Helpers.codeSampleSection "Icon Buttons" description content code

  let private iconButtonDensityExamples () =
    let description =
      Helpers.bodyText
        "Icon buttons respond to density the same way as text buttons. Pass the density class in attrs to set it per-instance. See the Density section on the Weave Styling page for container-level usage."

    let content =
      let row (label: string) densityAttr =
        div [ densityAttr; Margin.Bottom.small ] [
          Grid.create (
            [
              GridItem.create (
                div [ Typography.subtitle2 ] [ text label ],
                xs = Grid.Width.create 12,
                sm = Grid.Width.create 3
              )
              GridItem.create (
                IconButton.secondary (
                  Icon.create (Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "favorite"; Button.Variant.filled ]
                ),
                xs = Grid.Width.create 4,
                sm = Grid.Width.create 3
              )
              GridItem.create (
                IconButton.error (
                  Icon.create (Icon.UiActions UiActions.Delete),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "delete"; Button.Variant.outlined ]
                ),
                xs = Grid.Width.create 4,
                sm = Grid.Width.create 3
              )
              GridItem.create (
                IconButton.primary (
                  Icon.create (Icon.UiActions UiActions.Search),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "search"; Button.Variant.text ]
                ),
                xs = Grid.Width.create 4,
                sm = Grid.Width.create 3
              )
            ],
            spacing = Grid.GutterSpacing.create 2,
            justify = JustifyContent.flexStart,
            attrs = [ AlignItems.center ]
          )
        ]

      div [] [
        row "Compact" Density.compact
        row "Standard" Density.standard
        row "Spacious" Density.spacious
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


IconButton.primary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        Density.compact // see here
        Button.Variant.filled
    ]
)

IconButton.primary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        Density.standard // see here
        Button.Variant.filled
    ]
)

IconButton.primary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        Density.spacious // see here
        Button.Variant.filled
    ]
)
"""

    Helpers.codeSampleSection "Icon Button Density" description content code

  let private iconButtonDisabledExamples () =
    let description =
      Helpers.bodyText "Icon buttons can be disabled the same way as regular buttons."

    let content =
      Grid.create (
        [
          GridItem.create (
            IconButton.secondary (
              Icon.create (Icon.UiActions UiActions.Favorite),
              onClick = (fun () -> printfn "favorite clicked"),
              enabled = View.Const true,
              attrs = [ Attr.Create "aria-label" "favorite"; Button.Variant.filled ]
            )
          )
          GridItem.create (
            IconButton.create (
              Icon.create (Icon.UiActions UiActions.Favorite),
              onClick = (fun () -> printfn "This won't fire"),
              enabled = View.Const false,
              attrs = [ Attr.Create "aria-label" "favorite"; Button.Variant.filled ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


IconButton.secondary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> printfn "favorite clicked"),
    enabled = View.Const true, // see here
    attrs = [
        Attr.Create "aria-label" "favorite"
        Button.Variant.filled
    ]
)

IconButton.create(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> printfn "This won't fire"),
    enabled = View.Const false, // see here
    attrs = [
        Attr.Create "aria-label" "favorite"
        Button.Variant.filled
    ]
)
"""

    Helpers.codeSampleSection "Disabled Icon Buttons" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Button"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Buttons allow users to take actions and make choices with a single tap."
        ]

        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        colorExamples ()
        Helpers.divider ()
        densityExamples ()
        Helpers.divider ()
        disabledExamples ()
        Helpers.divider ()
        fullWidthExample ()
        Helpers.divider ()
        borderRadiusExamples ()
        Helpers.divider ()
        iconButtonExamples ()
        Helpers.divider ()
        iconButtonDensityExamples ()
        Helpers.divider ()
        iconButtonDisabledExamples ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
