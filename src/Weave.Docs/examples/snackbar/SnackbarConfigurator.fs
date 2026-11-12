namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module SnackbarConfigurator =

  let render (snackbars: SnackbarController) (selectedPosition: Var<string>) =
    let appearance = Var.Create SnackbarAppearance.defaults

    let select label choices get set =
      let choice =
        Var.Lens appearance (get >> Some) (fun current value ->
          match value with
          | Some value -> set value current
          | None -> current)

      let items =
        choices
        |> List.map (fun (value, name) -> SelectItem.create (text name, name, value))
        |> View.Const

      Select.create (
        items,
        choice,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let setPosition (current: SnackbarAppearance.Configuration) value =
      let label =
        SnackbarAppearance.positions |> List.find (fun (key, _) -> key = value) |> snd

      snackbars.clear ()
      Var.Set selectedPosition label
      { current with Position = value }

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        select "Variant" SnackbarAppearance.variants (fun x -> x.Variant) (fun value x -> {
          x with
              Variant = value
        })
        select "Color" SnackbarAppearance.colors (fun x -> x.Color) (fun value x -> { x with Color = value })
        select "Position" SnackbarAppearance.positions (fun x -> x.Position) (fun value x ->
          setPosition x value)
      ]

    let attrs =
      Map.ofList [
        "Snackbar.Variant.standard", Snackbar.Variant.standard
        "Snackbar.Variant.outlined", Snackbar.Variant.outlined
        "Snackbar.Variant.filled", Snackbar.Variant.filled
        "Snackbar.Color.primary", Snackbar.Color.primary
        "Snackbar.Color.secondary", Snackbar.Color.secondary
        "Snackbar.Color.tertiary", Snackbar.Color.tertiary
        "Snackbar.Color.error", Snackbar.Color.error
        "Snackbar.Color.warning", Snackbar.Color.warning
        "Snackbar.Color.success", Snackbar.Color.success
        "Snackbar.Color.info", Snackbar.Color.info
      ]

    let show () =
      let current = appearance.Value

      snackbars.show (
        "Your changes have been saved.",
        dismissal = Snackbar.Dismissal.After 4000,
        attrs = [
          Map.find ("Snackbar.Variant." + current.Variant) attrs
          Map.find ("Snackbar.Color." + current.Color) attrs
        ]
      )
      |> ignore

    let preview =
      Button.primary (text "Show notification", onClick = show, attrs = [ Button.Variant.filled ])

    let reset () =
      snackbars.clear ()
      Var.Set selectedPosition "Bottom right"
      Var.Set appearance SnackbarAppearance.defaults

    let description =
      Helpers.bodyText
        "Choose a style, color, and host position, then show a notification. Color does not change announcement priority; include a clear text description. Changing position clears the demo stack."

    Helpers.appearanceSection
      [ "colors-and-variants"; "position" ]
      description
      controls
      None
      preview
      reset
      (appearance.View |> View.Map SnackbarAppearance.render)
