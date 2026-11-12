namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module ContainerConfigurator =

  let render () =
    let appearance = Var.Create ContainerAppearance.defaults

    let selected =
      Var.Lens appearance (fun current -> Some current.MaxWidth) (fun current value ->
        match value with
        | Some width -> { current with MaxWidth = width }
        | None -> current)

    let items =
      ContainerAppearance.widths
      |> List.map (fun (value, name) -> SelectItem.create (text name, name, value))
      |> View.Const

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        Select.create (
          items,
          selected,
          labelText = View.Const "Max width",
          attrs = [
            Select.Variant.outlined
            Select.Label.above
            Select.Width.full
            Select.PopoverWidth.unbound
            Select.Color.primary
          ]
        )
      ]

    let fixedWidth =
      Var.Lens appearance (fun current -> current.Fixed) (fun current value -> { current with Fixed = value })

    let gutters =
      Var.Lens appearance (fun current -> current.Gutters) (fun current value -> {
        current with
            Gutters = value
      })

    let attrs =
      Map.ofList [
        "Container.MaxWidth.extraSmall", Container.MaxWidth.extraSmall
        "Container.MaxWidth.small", Container.MaxWidth.small
        "Container.MaxWidth.medium", Container.MaxWidth.medium
        "Container.MaxWidth.large", Container.MaxWidth.large
        "Container.MaxWidth.extraLarge", Container.MaxWidth.extraLarge
        "Container.MaxWidth.extraExtraLarge", Container.MaxWidth.extraExtraLarge
        "SurfaceColor.BackgroundColor.paper", SurfaceColor.BackgroundColor.paper
        "Container.fixedWidth", Container.fixedWidth
        "Container.gutters", Container.gutters
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let selectedAttrs =
          current
          |> ContainerAppearance.attributes
          |> List.map (fun key -> Map.find key attrs)

        Container.create (
          div [ BrandColor.Fill.primary; Padding.All.small ] [ text "Content" ],
          attrs = selectedAttrs
        ))

    let accessory =
      div [ Attr.Class "docs-appearance__actions" ] [
        Switch.create (fixedWidth, text "Fixed width")
        Switch.create (gutters, text "Gutters")
      ]

    let description =
      Helpers.bodyText
        "Combine a maximum width with fixed sizing and gutters. Resize the window to see responsive sizing; limits larger than the preview may look alike. The contrasting surfaces show the container and its content."

    Helpers.appearanceSection
      [ "maxwidth-variants" ]
      description
      controls
      (Some accessory)
      preview
      (fun () -> Var.Set appearance ContainerAppearance.defaults)
      (appearance.View |> View.Map ContainerAppearance.render)
