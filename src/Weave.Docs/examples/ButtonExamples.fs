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
    Helpers.section
      "Variants"
      (Helpers.bodyText "Buttons come in three variants: Filled, Outlined, and Text")
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
      |> List.collect (fun (name, color) -> [
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
          xs = Grid.Width.create 12,
          md = Grid.Width.create 3
        )
      ])
    )
    |> Helpers.section "Colors" (Helpers.bodyText "Buttons support all theme colors")

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
    |> Helpers.section
      "Sizes"
      (Helpers.bodyText "Buttons come in three sizes: Small, Medium (default), and Large")

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
    |> Helpers.section
      "Disabled State"
      (Helpers.bodyText "Buttons can be disabled using the enabled parameter")

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
    |> Helpers.section "Full Width" (Helpers.bodyText "Buttons can span the full width of their container")

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
    |> Helpers.section "Border Radius" (Helpers.bodyText "Buttons can have different border radius styles")

  let render () =
    Container.Create(
      div [] [
        H1.Create("Button Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Create(
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
      ]
    )
