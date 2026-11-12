namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module AlertExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText "Use Alert.create with a content doc to display a simple inline message."

    let content = Alert.create (text "This is an alert.")

    let code = """Alert.create (text "This is an alert.")"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private appearanceExample () =
    let appearance = Var.Create AlertAppearance.defaults

    let select label choices get set =
      let selected =
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
        selected,
        labelText = View.Const label,
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
        select "Variant" AlertAppearance.variants (fun x -> x.Variant) (fun value x -> {
          x with
              Variant = value
        })
        select "Color" AlertAppearance.colors (fun x -> x.Color) (fun value x -> { x with Color = value })
        select "Density" AlertAppearance.densities (fun x -> x.Density) (fun value x -> {
          x with
              Density = value
        })
      ]

    let hasIcon =
      Var.Lens appearance (fun current -> current.Icon) (fun current value -> { current with Icon = value })

    let attrs =
      Map.ofList [
        "Alert.Variant.standard", Alert.Variant.standard
        "Alert.Variant.outlined", Alert.Variant.outlined
        "Alert.Variant.filled", Alert.Variant.filled
        "Alert.Color.primary", Alert.Color.primary
        "Alert.Color.secondary", Alert.Color.secondary
        "Alert.Color.tertiary", Alert.Color.tertiary
        "Alert.Color.error", Alert.Color.error
        "Alert.Color.warning", Alert.Color.warning
        "Alert.Color.success", Alert.Color.success
        "Alert.Color.info", Alert.Color.info
        "Density.compact", Density.compact
        "Density.spacious", Density.spacious
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        let icon =
          if current.Icon then
            Some(Icon.create (Icon.UiActions UiActions.CheckCircle))
          else
            None

        Alert.create (
          text "Your changes have been saved.",
          ?icon = icon,
          attrs =
            (current
             |> AlertAppearance.attributeNames
             |> List.map (fun key -> Map.find key attrs))
        ))

    let description =
      Helpers.bodyText
        "Combine variants, colors, density, and an optional leading icon. The default color uses the surface palette. Icons use Material Symbols; see the Icons page for font setup."

    Helpers.appearanceSection
      [ "variants"; "colors"; "icons"; "density" ]
      description
      controls
      (Some(Switch.create (hasIcon, text "Leading icon")))
      preview
      (fun () -> Var.Set appearance AlertAppearance.defaults)
      (appearance.View |> View.Map AlertAppearance.render)

  let private closeExamples () =
    let description =
      Helpers.bodyText
        "Pass an onClose callback to render a close button. The callback is responsible for hiding or removing the alert."

    let visible = Var.Create true

    let content =
      div [] [
        visible.View
        |> Doc.BindView(fun show ->
          if show then
            Alert.create (
              text "Click the close button to dismiss this alert.",
              onClose = (fun () -> Var.Set visible false),
              attrs = [ Alert.Variant.outlined; Alert.Color.info ]
            )
          else
            Button.info (
              text "Reset",
              onClick = (fun () -> Var.Set visible true),
              attrs = [ Button.Variant.outlined ]
            ))
      ]

    let code =
      """let visible = Var.Create true

visible.View
|> Doc.BindView(fun show ->
  if show then
    Alert.create (
      text "Click the close button to dismiss this alert.",
      onClose = (fun () -> Var.Set visible false),
      attrs = [ Alert.Variant.outlined; Alert.Color.info ]
    )
  else
    Button.info (
      text "Reset",
      onClick = (fun () -> Var.Set visible true),
      attrs = [ Button.Variant.outlined ]
    ))"""

    Helpers.codeSampleSection "Close Button" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Alert.") [
      Helpers.apiTable "Alert.create" [
        Helpers.apiParam "content" "Doc" "" "The alert message content"
        Helpers.apiParam "?icon" "Doc" "" "Optional icon displayed at the start of the alert"
        Helpers.apiParam
          "?onClose"
          "unit -> unit"
          ""
          "Callback invoked when the close button is clicked; shows close button when provided"
        Helpers.apiParam "?closeIcon" "Doc" "" "Custom close icon (defaults to × character)"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (variant, color, etc.)"
      ]

      Helpers.styleModuleTable "Alert.Variant" [
        ("standard", "Default style with colored left border")
        ("outlined", "Bordered alert with transparent background")
        ("filled", "Solid background alert with a contrasting foreground")
      ]

      Helpers.styleModuleTable "Alert.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Alert"
        Helpers.bodyText "Displays an important message inline within the page content."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        closeExamples ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
