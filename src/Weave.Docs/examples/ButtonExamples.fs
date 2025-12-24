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

  let private section title description content =
    div [ Margin.toClasses Margin.Bottom.extraLarge |> cls ] [
      H3.Create(View.Const title, attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
      Body1.Create(View.Const description, attrs = [ Margin.toClasses Margin.Bottom.medium |> cls ])
      div [
        Padding.toClasses Padding.All.medium |> cls
        SurfaceColor.toAttr SurfaceColor.Paper
        BorderRadius.toClass BorderRadius.All.small |> cl
      ] [ content ]
    ]

  let private variantExamples () =
    section
      "Variants"
      "Buttons come in three variants: Filled, Outlined, and Text"
      (Grid.Create(
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
              ),
              xs = Grid.Width.create 4
            )

          btn "Filled" Button.Variant.Filled
          btn "Outlined" Button.Variant.Outlined
          btn "Text" Button.Variant.Text
        ],
        spacing = Grid.GutterSpacing.create 2
      ))

  let private colorExamples () =
    let colors = [
      "Primary", BrandColor.Primary
      "Secondary", BrandColor.Secondary
      "Tertiary", BrandColor.Tertiary
      "Error", BrandColor.Error
      "Warning", BrandColor.Warning
      "Success", BrandColor.Success
      "Info", BrandColor.Info
    ]

    Grid.Create(
      colors
      |> List.indexed
      |> List.collect (fun (i, (name, color)) -> [
        GridItem.Create(
          Button.Create(
            text name,
            onClick = (fun () -> printfn "%s clicked" name),
            attrs = [
              Button.Variant.Filled |> Button.Variant.toClass |> cl
              Button.Color.toClass color |> cl

              Button.Width.toClass Button.Width.Full |> Option.mapOrDefault Attr.Empty cl
            ]
          ),
          xs = Grid.Width.create 6,
          md = Grid.Width.create 2
        )

        if i % 3 = 1 then
          FlexBreak.Create()
      ]),
      spacing = Grid.GutterSpacing.create 2
    )
    |> section "Colors" "Buttons support all theme colors"

  let private sizeExamples () =
    Grid.Create(
      [
        let btn displayText size =
          GridItem.Create(
            Button.Create(
              text displayText,
              onClick = (fun () -> printfn "%s clicked" displayText),
              attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                Button.Size.toClass size |> cl
                Button.Color.toClass BrandColor.Primary |> cl
              ]
            ),
            xs = Grid.Width.create 4
          )

        btn "Small" Button.Size.Small
        btn "Medium" Button.Size.Medium
        btn "Large" Button.Size.Large
      ],
      spacing = Grid.GutterSpacing.create 2
    )
    |> section "Sizes" "Buttons come in three sizes: Small, Medium (default), and Large"

  let private disabledExamples () =
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
          ),
          xs = Grid.Width.create 6
        )
        GridItem.Create(
          Button.Create(
            text "Disabled",
            onClick = (fun () -> printfn "This won't fire"),
            enabled = View.Const false,
            attrs = [ Button.Variant.Filled |> Button.Variant.toClass |> cl ]
          ),
          xs = Grid.Width.create 6
        )
      ],
      spacing = Grid.GutterSpacing.create 2
    )
    |> section "Disabled State" "Buttons can be disabled using the enabled parameter"

  let private fullWidthExample () =
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
    |> section "Full Width" "Buttons can span the full width of their container"

  let private borderRadiusExamples () =
    Grid.Create(
      [
        let btn displayText radius =
          GridItem.Create(
            Button.Create(
              text displayText,
              onClick = (fun () -> ()),
              attrs = [
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                BorderRadius.toClass radius |> cl
                BrandColor.Primary |> Button.Color.toClass |> cl
              ]
            ),
            xs = Grid.Width.create 4
          )

        btn "None" BorderRadius.All.none
        btn "Small" BorderRadius.All.small
        btn "Medium" BorderRadius.All.medium
        btn "Large" BorderRadius.All.large
        btn "Pill" BorderRadius.Pill
        btn "Circle" BorderRadius.Circle
      ],
      spacing = Grid.GutterSpacing.create 2
    )
    |> section "Border Radius" "Buttons can have different border radius styles"

  let render () =
    Container.Create(
      div [] [
        H1.Create("Button Component", attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
        Body1.Create(
          "Buttons allow users to take actions and make choices with a single tap.",
          attrs = [ Margin.toClasses Margin.Bottom.extraLarge |> cls ]
        )

        variantExamples ()
        colorExamples ()
        sizeExamples ()
        disabledExamples ()
        fullWidthExample ()
        borderRadiusExamples ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
