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
      Grid.Create(
        [
          GridItem.Create(
            Button.Create(
              text "Filled",
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.filled; Button.Color.primary ]
            )
          )
          GridItem.Create(
            Button.Create(
              text "Outlined",
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.outlined; Button.Color.primary ]
            )
          )
          GridItem.Create(
            Button.Create(
              text "Text",
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.text; Button.Color.primary ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


let doNothing () = ()

Button.Create(
    text "Filled",
    onClick = doNothing,
    attrs = [
        Button.Variant.filled // see here
        Button.Color.primary
    ]
)

Button.Create(
    text "Outlined",
    onClick = doNothing,
    attrs = [
        Button.Variant.outlined // see here
        Button.Color.primary
    ]
)

Button.Create(
    text "Text",
    onClick = doNothing,
    attrs = [
        Button.Variant.text // see here
        Button.Color.primary
    ]
)
    """

    Helpers.codeSampleSection "Variants" description content code

  let private colorExamples () =
    let colors = [
      BrandColor.Primary
      BrandColor.Secondary
      BrandColor.Tertiary
      BrandColor.Error
      BrandColor.Warning
      BrandColor.Success
      BrandColor.Info
    ]

    let description = Helpers.bodyText "Buttons support all theme colors"

    let code =
      """open Weave

open WebSharper.UI

let onClick name = printfn "%s clicked" name

let colors : BrandColor list = [
    BrandColor.Primary
    BrandColor.Secondary
    BrandColor.Tertiary
    BrandColor.Error
    BrandColor.Warning
    BrandColor.Success
    BrandColor.Info
]

colors
|> List.map (fun color ->
    let colorName = sprintf "%A" color

    Button.Create(
        text colorName,
        onClick = onClick colorName,
        attrs = [
            Button.Variant.filled
            Button.Color.toAttr color // see here
            Button.Width.full
        ]
    )
)
"""

    let content =
      Grid.Create(
        colors
        |> List.collect (fun color -> [
          let colorName = sprintf "%A" color

          GridItem.Create(
            Button.Create(
              text colorName,
              onClick = (fun () -> printfn "%s clicked" colorName),
              attrs = [ Button.Variant.filled; Button.Color.toAttr color; Button.Width.full ]
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
      let col density =
        let label = sprintf "%A" density

        div [ cl (Density.toClass density) ] [
          Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          div [] [
            Button.Create(
              text "Filled",
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.filled; Button.Color.primary ]
            )
          ]
          div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
            Button.Create(
              text "Outlined",
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.outlined; Button.Color.primary ]
            )
          ]
          div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
            Button.Create(
              text "Text",
              onClick = (fun () -> ()),
              attrs = [ Button.Variant.text; Button.Color.primary ]
            )
          ]
        ]

      Grid.Create(
        [
          GridItem.Create(col Density.Compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Standard, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.Create(col Density.Spacious, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.toClass AlignItems.Start |> cl ]
      )

    let code =
      """open Weave


Button.Create(
    text "Compact",
    onClick = (fun () -> ()),
    attrs = [
        cl (Density.toClass Density.Compact) // see here
        Button.Variant.filled
        Button.Color.primary
    ]
)

Button.Create(
    text "Standard",
    onClick = (fun () -> ()),
    attrs = [
        cl (Density.toClass Density.Standard) // see here
        Button.Variant.filled
        Button.Color.primary
    ]
)

Button.Create(
    text "Spacious",
    onClick = (fun () -> ()),
    attrs = [
        cl (Density.toClass Density.Spacious) // see here
        Button.Variant.filled
        Button.Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let private disabledExamples () =
    let description =
      Helpers.bodyText "Buttons can be disabled using the enabled parameter"

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Button.Create(
              text "Enabled",
              onClick = (fun () -> printfn "Enabled clicked"),
              enabled = View.Const true,
              attrs = [ Button.Variant.filled; Button.Color.primary ]
            )
          )
          GridItem.Create(
            Button.Create(
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


Button.Create(
    text "Enabled",
    onClick = (fun () -> printfn "Enabled clicked"),
    enabled = View.Const true, // see here
    attrs = [
        Button.Variant.filled
        Button.Color.primary
    ]
)

Button.Create(
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
      Button.Create(
        text "Full Width Button",
        onClick = (fun () -> printfn "Full width clicked"),
        attrs = [ Button.Color.primary; Button.Variant.filled; Button.Width.full ]
      )

    let code =
      """open Weave

open WebSharper.UI

Button.Create(
    text "Full Width Button",
    onClick = (fun () -> printfn "Full width clicked"),
    attrs = [
        Button.Color.primary
        Button.Variant.filled
        Button.Width.full // see here
    ]
)
"""

    Helpers.codeSampleSection "Full Width" description content code

  let private borderRadiusExamples () =
    let description = Helpers.bodyText "Buttons can have different border radius styles"

    let content =
      Grid.Create(
        [
          let btn radius =
            let displayText = sprintf "%A" radius

            GridItem.Create(
              Button.Create(
                text displayText,
                onClick = (fun () -> ()),
                attrs = [
                  Button.Variant.filled
                  BorderRadius.toClass radius |> cl
                  Button.Color.primary
                ]
              )
            )

          btn BorderRadius.All.none
          btn BorderRadius.All.small
          btn BorderRadius.All.medium
          btn BorderRadius.All.large
          btn BorderRadius.Pill
          btn BorderRadius.Circle
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


let btn (radius: BorderRadius) =
    let displayText = sprintf "%A" radius

    Button.Create(
        text displayText,
        onClick = (fun () -> ()),
        attrs = [
            Button.Variant.filled
            BorderRadius.toClass radius |> cl // see here
            Button.Color.primary
        ]
    )

btn BorderRadius.All.none
btn BorderRadius.All.small
btn BorderRadius.All.medium
btn BorderRadius.All.large
btn BorderRadius.Pill
btn BorderRadius.Circle
"""

    Helpers.codeSampleSection "Border Radius" description content code

  let private iconButtonExamples () =
    let description =
      Helpers.bodyText
        "Icon buttons render a single icon without a text label. Use Button.CreateIcon to create them. They reuse the same variant, color, and size classes as regular buttons."

    let content =
      Grid.Create(
        [
          let btn icon ariaLabel (color: Attr) =
            GridItem.Create(
              Button.CreateIcon(
                Icon.Create(icon),
                onClick = (fun () -> printfn "%s clicked" ariaLabel),
                attrs = [
                  Attr.Create "aria-label" ariaLabel
                  color
                  Button.Variant.filled
                  BorderRadius.toClass BorderRadius.Circle |> cl
                ]
              )
            )

          btn (Icon.UiActions UiActions.Delete) "delete" Button.Color.error
          btn (Icon.UiActions UiActions.Favorite) "favorite" Button.Color.secondary
          btn (Icon.UiActions UiActions.Home) "home" Button.Color.primary
          btn (Icon.UiActions UiActions.Search) "search" Button.Color.info
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


let btn icon ariaLabel color =
    Button.CreateIcon(
        Icon.Create(icon),
        onClick = (fun () -> printfn "%s clicked" ariaLabel),
        attrs = [
            Attr.Create "aria-label" ariaLabel // see here
            Button.Color.toAttr color
            Button.Variant.filled
            BorderRadius.toClass BorderRadius.Circle |> cl
        ]
    )

btn (Icon.UiActions UiActions.Delete) "delete" BrandColor.Error
btn (Icon.UiActions UiActions.Favorite) "favorite" BrandColor.Secondary
btn (Icon.UiActions UiActions.Home) "home" BrandColor.Primary
btn (Icon.UiActions UiActions.Search) "search" BrandColor.Info
"""

    Helpers.codeSampleSection "Icon Buttons" description content code

  let private iconButtonDensityExamples () =
    let description =
      Helpers.bodyText
        "Icon buttons respond to density the same way as text buttons. Pass the density class in attrs to set it per-instance. See the Density section on the Weave Styling page for container-level usage."

    let content =
      let row density =
        let label = sprintf "%A" density

        div [ cl (Density.toClass density); Margin.toClasses Margin.Bottom.small |> cls ] [
          Grid.Create(
            [
              GridItem.Create(Subtitle2.Div(label), xs = Grid.Width.create 12, sm = Grid.Width.create 3)
              GridItem.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> ()),
                  attrs = [
                    Attr.Create "aria-label" "favorite"
                    Button.Variant.filled
                    Button.Color.secondary
                  ]
                ),
                xs = Grid.Width.create 4,
                sm = Grid.Width.create 3
              )
              GridItem.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Delete),
                  onClick = (fun () -> ()),
                  attrs = [
                    Attr.Create "aria-label" "delete"
                    Button.Variant.outlined
                    Button.Color.error
                  ]
                ),
                xs = Grid.Width.create 4,
                sm = Grid.Width.create 3
              )
              GridItem.Create(
                Button.CreateIcon(
                  Icon.Create(Icon.UiActions UiActions.Search),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "search"; Button.Variant.text; Button.Color.primary ]
                ),
                xs = Grid.Width.create 4,
                sm = Grid.Width.create 3
              )
            ],
            spacing = Grid.GutterSpacing.create 2,
            justify = JustifyContent.FlexStart,
            attrs = [ AlignItems.toClass AlignItems.Center |> cl ]
          )
        ]

      div [] [ row Density.Compact; row Density.Standard; row Density.Spacious ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


Button.CreateIcon(
    Icon.Create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        cl (Density.toClass Density.Compact) // see here
        Button.Variant.filled
        Button.Color.primary
    ]
)

Button.CreateIcon(
    Icon.Create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        cl (Density.toClass Density.Standard) // see here
        Button.Variant.filled
        Button.Color.primary
    ]
)

Button.CreateIcon(
    Icon.Create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        cl (Density.toClass Density.Spacious) // see here
        Button.Variant.filled
        Button.Color.primary
    ]
)
"""

    Helpers.codeSampleSection "Icon Button Density" description content code

  let private iconButtonDisabledExamples () =
    let description =
      Helpers.bodyText "Icon buttons can be disabled the same way as regular buttons."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Button.CreateIcon(
              Icon.Create(Icon.UiActions UiActions.Favorite),
              onClick = (fun () -> printfn "favorite clicked"),
              enabled = View.Const true,
              attrs = [
                Attr.Create "aria-label" "favorite"
                Button.Variant.filled
                Button.Color.secondary
              ]
            )
          )
          GridItem.Create(
            Button.CreateIcon(
              Icon.Create(Icon.UiActions UiActions.Favorite),
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


Button.CreateIcon(
    Icon.Create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> printfn "favorite clicked"),
    enabled = View.Const true, // see here
    attrs = [
        Attr.Create "aria-label" "favorite"
        Button.Variant.filled
        Button.Color.secondary
    ]
)

Button.CreateIcon(
    Icon.Create(Icon.UiActions UiActions.Favorite),
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
    Container.Create(
      div [] [
        Helpers.pageTitle "Button"
        Body1.Div(
          "Buttons allow users to take actions and make choices with a single tap.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

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
      maxWidth = Container.MaxWidth.Large
    )
