namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module TabsAppearance =

  module Source = TabsAppearanceSource

  let private appearanceClasses = [
    "Tabs.Variant.outlined", "weave-tabs--outlined"
    "Tabs.Variant.filled", "weave-tabs--filled"
    "Tabs.Color.primary", "weave-tabs--primary"
    "Tabs.Color.secondary", "weave-tabs--secondary"
    "Tabs.Color.tertiary", "weave-tabs--tertiary"
    "Tabs.Color.error", "weave-tabs--error"
    "Tabs.Color.warning", "weave-tabs--warning"
    "Tabs.Color.success", "weave-tabs--success"
    "Tabs.Color.info", "weave-tabs--info"
    "Tabs.Alignment.center", "weave-tabs--centered"
    "Density.compact", "weave-density--compact"
    "Density.spacious", "weave-density--spacious"
  ]

  let private position (appearance: Source.Appearance) =
    match appearance.Position with
    | Source.Position.Top -> Tabs.Position.Top
    | Source.Position.Bottom -> Tabs.Position.Bottom
    | Source.Position.Left -> Tabs.Position.Left
    | Source.Position.Right -> Tabs.Position.Right
    | Source.Position.Start -> Tabs.Position.Start
    | Source.Position.End -> Tabs.Position.End

  let private choice
    (label: string)
    (options: ('Choice * string) list)
    (getter: Source.Appearance -> 'Choice)
    (setter: Source.Appearance -> 'Choice -> Source.Appearance)
    (appearance: Var<Source.Appearance>)
    =
    let updateSelection current selection =
      match selection with
      | Some value -> setter current value
      | None -> current

    let selected = Var.Lens appearance (getter >> Some) updateSelection

    let items =
      options
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

  let render () =
    let appearance = Var.Create Source.defaults
    let activeIndex = Var.Create 0
    let structure = Var.Create(Source.defaults.Position, Source.defaults.Icons)

    let setVariant (current: Source.Appearance) variant = {
      current with
          Variant = variant
          Tray =
            if variant = Source.Variant.Filled then
              current.Tray
            else
              Source.Tray.Plain
    }

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice "Variant" Source.variants (fun current -> current.Variant) setVariant appearance
        choice
          "Color"
          Source.colors
          (fun current -> current.Color)
          (fun current value -> { current with Color = value })
          appearance
        choice
          "Density"
          Source.densities
          (fun current -> current.Density)
          (fun current value -> { current with Density = value })
          appearance
        choice
          "Position"
          Source.positions
          (fun current -> current.Position)
          (fun current value ->
            Var.Set structure (value, current.Icons)
            { current with Position = value })
          appearance
        choice
          "Alignment"
          Source.alignments
          (fun current -> current.Alignment)
          (fun current value -> { current with Alignment = value })
          appearance
        choice
          "Icons"
          Source.icons
          (fun current -> current.Icons)
          (fun current value ->
            Var.Set structure (current.Position, value)
            { current with Icons = value })
          appearance
      ]

    let tray =
      Var.Lens appearance (fun current -> current.Tray = Source.Tray.Matching) (fun current enabled -> {
        current with
            Tray = if enabled then Source.Tray.Matching else Source.Tray.Plain
      })

    let accessory =
      Some(
        Switch.create (
          tray,
          content = text "Colored tray",
          enabled =
            (appearance.View
             |> View.Map(fun current -> current.Variant = Source.Variant.Filled))
        )
      )

    let selectedAttributes = appearance.View |> View.Map Source.attributeNames

    let previewAttributes = [
      for name, cssClass in appearanceClasses do
        Attr.DynamicClassPred cssClass (selectedAttributes |> View.Map(List.contains name))

      let trayColor =
        selectedAttributes
        |> View.Map(fun names ->
          names
          |> List.tryPick (fun name ->
            let prefix = "Tabs.HeaderBackground."

            if name.StartsWith prefix then
              Some(name.Substring prefix.Length)
            else
              None))

      Attr.DynamicStyle
        "--tabs-header-background"
        (trayColor
         |> View.Map(function
           | Some color -> "var(--palette-" + color + ")"
           | None -> ""))

      Attr.DynamicStyle
        "--tabs-header-foreground"
        (trayColor
         |> View.Map(function
           | Some color -> "var(--palette-" + color + "-text)"
           | None -> ""))

      Attr.Class "docs-appearance__bounds"
    ]

    let preview =
      structure.View
      |> Doc.BindView(fun (selectedPosition, selectedIcons) ->
        let items =
          Source.tabs
          |> List.map (fun (label, panel, iconName) ->
            let icon () =
              match iconName with
              | "UiActions UiActions.Home" -> Icon.create (Icon.UiActions UiActions.Home)
              | "Action Action.Info" -> Icon.create (Icon.Action Action.Info)
              | _ -> Icon.create (Icon.UiActions UiActions.Settings)

            match selectedIcons with
            | Source.Icons.None -> TabItem.create (label, text panel)
            | Source.Icons.Start -> TabItem.create (label, text panel, startIcon = icon ())
            | Source.Icons.End -> TabItem.create (label, text panel, endIcon = icon ()))
          |> View.Const

        let current = { Source.defaults with Position = selectedPosition }

        Tabs.create (
          items,
          activeIndex = activeIndex,
          position = position current,
          attrs = previewAttributes
        ))

    let codeView = (appearance.View, activeIndex.View) ||> View.Map2 Source.render

    let reset () =
      Var.Set appearance Source.defaults
      Var.Set structure (Source.defaults.Position, Source.defaults.Icons)
      Var.Set activeIndex 0

    let description =
      Helpers.bodyText
        "Text is the default style. Outlined adds a border to the active tab; Filled adds a pill. The colored tray matches the selected color. Start and End positions follow the reading direction. Centering applies when the tabs fit. Icons use Material Symbols; see the Icons page for font setup. Choose a panel or adjust the options to update the code. The outline marks the component's size."

    Helpers.appearanceSection
      [
        "variants"
        "colors"
        "tab-positions"
        "tabs-with-icons"
        "centered-tabs"
        "density"
      ]
      description
      controls
      accessory
      preview
      reset
      codeView
