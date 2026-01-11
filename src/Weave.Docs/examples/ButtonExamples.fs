namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open WebSharper.JavaScript

[<JavaScript>]
module ButtonExamples =

  let private variantExamples () =
    let description =
      Helpers.bodyText "Buttons come in three variants: Filled, Outlined, and Text"

    let content =
      Grid.Create(
        [
          let btn displayText variant =
            GridItem.Create(
              Button.Create(
                text displayText,
                onClick = (fun () -> ()),
                attrs = [
                  Button.Variant.toClass variant |> cl
                  BrandColor.Primary |> Button.Color.toClass |> cl
                ]
              )
            )

          btn "Filled" Button.Variant.Filled
          btn "Outlined" Button.Variant.Outlined
          btn "Text" Button.Variant.Text
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

let doNothing () = ()

Button.Create(
    text "Filled",
    onClick = doNothing,
    attrs = [
        cls [
            Button.Variant.toClass Button.Variant.Filled // see here
            BrandColor.Primary |> Button.Color.toClass
        ]
    ]
)

Button.Create(
    text "Outlined",
    onClick = doNothing,
    attrs = [
        cls [
            Button.Variant.toClass Button.Variant.Outlined // see here
            BrandColor.Primary |> Button.Color.toClass
        ]
    ]
)

Button.Create(
    text "Text",
    onClick = doNothing,
    attrs = [
        cls [
            Button.Variant.toClass Button.Variant.Text // see here
            BrandColor.Primary |> Button.Color.toClass
        ]
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
open Weave.CssHelpers
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
            cls [
                Button.Variant.toClass Button.Variant.Filled
                Button.Color.toClass color // see here

                match Button.Width.toClass Button.Width.Full with
                | Some c -> c
                | None -> Attr.Empty
            ]
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
              attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                Button.Color.toClass color |> cl
                Button.Width.toClass Button.Width.Full |> Option.mapOrDefault Attr.Empty cl
              ]
            )
          )
        ])
      )

    Helpers.codeSampleSection "Colors" description content code

  let private sizeExamples () =
    let description =
      Helpers.bodyText
        "Buttons come in three sizes: Small, Medium (default), and Large. These sizes effect the button's minimum height and padding."

    let content =
      Grid.Create(
        [
          let btn displayText size variant =
            GridItem.Create(
              Button.Create(
                text displayText,
                onClick = (fun () -> printfn "%s clicked" displayText),
                attrs = [
                  variant |> Button.Variant.toClass |> cl
                  Button.Size.toClass size |> cl
                  Button.Color.toClass BrandColor.Primary |> cl
                ]
              ),
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 4
            )

          btn "Small" Button.Size.Small Button.Variant.Filled
          btn "Medium" Button.Size.Medium Button.Variant.Filled
          btn "Large" Button.Size.Large Button.Variant.Filled
          btn "Small" Button.Size.Small Button.Variant.Outlined
          btn "Medium" Button.Size.Medium Button.Variant.Outlined
          btn "Large" Button.Size.Large Button.Variant.Outlined
          btn "Small" Button.Size.Small Button.Variant.Text
          btn "Medium" Button.Size.Medium Button.Variant.Text
          btn "Large" Button.Size.Large Button.Variant.Text
        ],
        justify = JustifyContent.Center,
        attrs = [ AlignItems.toClass AlignItems.Center |> cl ]
      )

    let code =
      """open Weave
open Weave.CssHelpers

let onClick name = printfn "%s clicked" name

let btn (size: Size) variant =
    let displayText = sprintf "%A" size

    Button.Create(
        text displayText,
        onClick = (fun () -> onClick displayText),
        attrs = [
            cls [
                Button.Variant.toClass variant
                Button.Size.toClass size // see here
                Button.Color.toClass BrandColor.Primary
            ]
        ]
    )

btn Button.Size.Small Button.Variant.Filled
btn Button.Size.Medium Button.Variant.Filled
btn Button.Size.Large Button.Variant.Filled
btn Button.Size.Small Button.Variant.Outlined
btn Button.Size.Medium Button.Variant.Outlined
btn Button.Size.Large Button.Variant.Outlined
btn Button.Size.Small Button.Variant.Text
btn Button.Size.Medium Button.Variant.Text
btn Button.Size.Large Button.Variant.Text
"""

    Helpers.codeSampleSection "Sizes" description content code

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
              attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                Button.Color.toClass BrandColor.Primary |> cl
              ]
            )
          )
          GridItem.Create(
            Button.Create(
              text "Disabled",
              onClick = (fun () -> printfn "This won't fire"),
              enabled = View.Const false,
              attrs = [ Button.Variant.Filled |> Button.Variant.toClass |> cl ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

Button.Create(
    text "Enabled",
    onClick = (fun () -> printfn "Enabled clicked"),
    enabled = View.Const true, // see here
    attrs = [
        Button.Variant.Filled |> Button.Variant.toClass
        Button.Color.toClass BrandColor.Primary
    ]
)

Button.Create(
    text "Disabled",
    onClick = (fun () -> printfn "This won't fire"),
    enabled = View.Const false, // see here
    attrs = [
        Button.Variant.Filled |> Button.Variant.toClass
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
        attrs = [
          Button.Color.toClass BrandColor.Primary |> cl
          Button.Variant.Filled |> Button.Variant.toClass |> cl
          match Button.Width.toClass Button.Width.Full with
          | Some c -> c |> cl
          | None -> Attr.Empty
        ]
      )

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

Button.Create(
    text "Full Width Button",
    onClick = (fun () -> printfn "Full width clicked"),
    attrs = [
        Button.Color.toClass BrandColor.Primary
        Button.Variant.Filled |> Button.Variant.toClass
        Button.Width.toClass Button.Width.Full |> Option.mapOrDefault Attr.Empty // see here
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
                  Button.Variant.Filled |> Button.Variant.toClass |> cl
                  BorderRadius.toClass radius |> cl
                  BrandColor.Primary |> Button.Color.toClass |> cl
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
open Weave.CssHelpers

let btn (radius: BorderRadius) =
    let displayText = sprintf "%A" radius

    Button.Create(
        text displayText,
        onClick = (fun () -> ()),
        attrs = [
            cls [
                Button.Variant.toClass Button.Variant.Filled
                BorderRadius.toClass radius // see here
                BrandColor.Primary |> Button.Color.toClass
            ]
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

  let render () =
    Container.Create(
      div [] [
        H1.Div("Button Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "Buttons allow users to take actions and make choices with a single tap.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        colorExamples ()
        Helpers.divider ()
        sizeExamples ()
        Helpers.divider ()
        disabledExamples ()
        Helpers.divider ()
        fullWidthExample ()
        Helpers.divider ()
        borderRadiusExamples ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
