namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ChipExamples =

  let private centered content =
    div [
      Flex.Flex.allSizes
      FlexDirection.Column.allSizes
      AlignItems.center
      Gap.All.g3
    ] [ content ]

  let private comparisonGrid columns items =
    let span =
      match columns with
      | 2 -> GridItem.Span.Medium.six
      | 4 -> GridItem.Span.Medium.three
      | _ -> GridItem.Span.Medium.four

    Grid.create (
      items
      |> List.map (fun item ->
        GridItem.create (item, attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; span ])),
      attrs = [ AlignItems.start ]
    )

  let private appearanceExample () =
    let appearance = Var.Create ChipAppearance.defaults

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
        select "Variant" ChipAppearance.variants (fun x -> x.Variant) (fun value x -> {
          x with
              Variant = value
        })
        select "Color" ChipAppearance.colors (fun x -> x.Color) (fun value x -> { x with Color = value })
        select "Density" ChipAppearance.densities (fun x -> x.Density) (fun value x -> {
          x with
              Density = value
        })
        select "Corners" ChipAppearance.corners (fun x -> x.Corners) (fun value x -> {
          x with
              Corners = value
        })
      ]

    let isSelected =
      Var.Lens appearance (fun current -> current.Selected) (fun current value -> {
        current with
            Selected = value
      })

    let disabled =
      Var.Lens appearance (fun current -> current.Disabled) (fun current value -> {
        current with
            Disabled = value
      })

    let attrs =
      Map.ofList [
        "Chip.Variant.filled", Chip.Variant.filled
        "Chip.Variant.outlined", Chip.Variant.outlined
        "Chip.Variant.text", Chip.Variant.text
        "Chip.Color.primary", Chip.Color.primary
        "Chip.Color.secondary", Chip.Color.secondary
        "Chip.Color.tertiary", Chip.Color.tertiary
        "Chip.Color.error", Chip.Color.error
        "Chip.Color.warning", Chip.Color.warning
        "Chip.Color.success", Chip.Color.success
        "Chip.Color.info", Chip.Color.info
        "Chip.Density.compact", Chip.Density.compact
        "Chip.Density.spacious", Chip.Density.spacious
        "Chip.Shape.label", Chip.Shape.label
        "BorderRadius.All.none", BorderRadius.All.none
        "BorderRadius.All.small", BorderRadius.All.small
        "BorderRadius.All.medium", BorderRadius.All.medium
        "BorderRadius.All.large", BorderRadius.All.large
      ]

    let preview =
      appearance.View
      |> View.MapCached(fun current -> { current with Selected = false })
      |> Doc.BindView(fun current ->
        let selectedAttrs =
          current
          |> ChipAppearance.attributeNames
          |> List.map (fun key -> Map.find key attrs)

        Chip.create (
          text "Ready",
          onClick = (fun () -> Var.Update isSelected not),
          selected = isSelected.View,
          enabled = View.Const(not current.Disabled),
          attrs = [
            Attr.Dynamic "aria-pressed" (isSelected.View |> View.Map string<bool>)
            yield! selectedAttrs
          ]
        ))

    let description =
      Helpers.bodyText
        "Combine variants, theme colors, density, corners, and states. Chips are pill-shaped by default; Label uses the theme's rounded rectangle. Activate the chip to toggle selection; disabled chips cannot be activated."

    Helpers.appearanceSection
      [
        "density"
        "filled-chips"
        "text-chips"
        "outlined-chips"
        "shape"
        "border-radius"
        "disabled"
      ]
      description
      controls
      (Some(
        div [ Attr.Class "docs-appearance__actions" ] [
          Switch.create (isSelected, text "Selected")
          Switch.create (disabled, text "Disabled")
        ]
      ))
      preview
      (fun () -> Var.Set appearance ChipAppearance.defaults)
      (appearance.View |> View.Map ChipAppearance.render)

  let private closableExample () =
    let description =
      Helpers.bodyText
        "Pass an onClose callback to render a close button. The callback handles removing the chip from the UI."

    let content =
      let items = Var.Create [ "React"; "Angular"; "Vue"; "Svelte"; "WebSharper" ]

      items.View
      |> Doc.BindView(fun currentItems ->
        currentItems
        |> List.map (fun item ->
          Chip.create (
            text item,
            onClose = (fun () -> items.Value |> List.filter (fun i -> i <> item) |> Var.Set items),
            attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
          ))
        |> fun chips ->
            div
              [
                Flex.Flex.allSizes
                FlexWrap.Wrap.allSizes
                JustifyContent.center
                Gap.All.g3
              ]
              chips)

    let code =
      """let items = Var.Create [ "React"; "Angular"; "Vue"; "Svelte"; "WebSharper" ]

items.View
|> Doc.BindView(fun currentItems ->
  currentItems
  |> List.map (fun item ->
    Chip.create (
      text item,
      onClose =
        (fun () ->
          items.Value |> List.filter (fun i -> i <> item) |> Var.Set items
        ),
      attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
    )
  )
  |> Doc.Concat
)
"""

    Helpers.codeSampleSection "Closable" description content code

  let private customCloseIconExample () =
    let description =
      Helpers.bodyText
        "Replace the close icon with closeIcon. The callback owns removal; restore the chip to try the interaction again."

    let content =
      let visible = Var.Create true

      visible.View
      |> Doc.BindView(fun show ->
        if show then
          Chip.create (
            text "Draft",
            onClose = (fun () -> Var.Set visible false),
            closeIcon = Icon.create (Icon.UiActions UiActions.Close),
            attrs = [ Chip.Variant.filled; Chip.Color.secondary ]
          )
        else
          Button.create (text "Restore draft", onClick = (fun () -> Var.Set visible true)))

    let code =
      """open Weave.Icons.MaterialSymbols

let visible = Var.Create true

visible.View
|> Doc.BindView(fun show ->
  if show then
    Chip.create (
      text "Draft",
      onClose = (fun () -> Var.Set visible false),
      closeIcon = Icon.create (Icon.UiActions UiActions.Close),
      attrs = [ Chip.Variant.filled; Chip.Color.secondary ]
    )
  else
    Button.create (
      text "Restore draft",
      onClick = (fun () -> Var.Set visible true)
    )
)
"""

    Helpers.codeSampleSection "Custom Close Icon" description (centered content) code

  let private clickableExample () =
    let description =
      Helpers.bodyText "Pass onClick to make a chip interactive. This example increments the visible count."

    let content =
      let count = Var.Create 0
      let clickText = count.View |> View.Map(sprintf "Clicked %d times")

      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
      ] [
        textView clickText

        Chip.create (
          text "Click me",
          onClick = (fun () -> Var.Update count ((+) 1)),
          attrs = [ Chip.Variant.filled; Chip.Color.primary ]
        )
      ]

    let code =
      """let count = Var.Create 0
let clickText = count.View |> View.Map(sprintf "Clicked %d times")

Doc.Concat [
  textView clickText

  Chip.create (
    text "Click me",
    onClick = (fun () -> Var.Update count ((+) 1)),
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
]
"""

    Helpers.codeSampleSection "Clickable" description content code

  let private iconExample () =
    let description =
      Helpers.bodyText "Pass content to display a leading element inside the chip, such as an icon or avatar."

    let content =
      Chip.create (
        text "Saved",
        content = Icon.create (Icon.UiActions UiActions.CheckCircle),
        attrs = [ Chip.Variant.filled; Chip.Color.success ]
      )

    let code =
      """open Weave.Icons.MaterialSymbols

Chip.create (
  text "Saved",
  content = Icon.create (Icon.UiActions UiActions.CheckCircle),
  attrs = [ Chip.Variant.filled; Chip.Color.success ]
)
"""

    Helpers.codeSampleSection "Content" description (centered content) code

  let private linkChipExample () =
    let description =
      Helpers.bodyText
        "Pass an href to render the chip as an anchor element. The chip navigates to the given URL on click."

    let content =
      Chip.create (
        text "WebSharper",
        href = "https://websharper.com/",
        content = Icon.create (Icon.UiActions UiActions.OpenInNew),
        attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
      )

    let code =
      """open Weave.Icons.MaterialSymbols

Chip.create (
  text "WebSharper",
  href = "https://websharper.com/",
  content = Icon.create (Icon.UiActions UiActions.OpenInNew),
  attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
)
"""

    Helpers.codeSampleSection "Link Chip" description (centered content) code

  let private selectedExample () =
    let description =
      Helpers.bodyText
        "Pass a reactive View<bool> to the selected parameter to control the chip's selected state. Click the button to toggle."

    let content =
      let isSelected = Var.Create false

      Chip.create (
        text "Toggle me",
        onClick = (fun () -> Var.Set isSelected (not isSelected.Value)),
        selected = isSelected.View,
        attrs = [
          Attr.Dynamic "aria-pressed" (isSelected.View |> View.Map string<bool>)
          Chip.Variant.filled
          Chip.Color.primary
        ]
      )

    let code =
      """let isSelected = Var.Create false

Chip.create (
  text "Toggle me",
  onClick = (fun () -> Var.Set isSelected (not isSelected.Value)),
  selected = isSelected.View,
  attrs = [
    Attr.Dynamic "aria-pressed" (isSelected.View |> View.Map string<bool>)
    Chip.Variant.filled
    Chip.Color.primary
  ]
)
"""

    Helpers.codeSampleSection "Selected" description (centered content) code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Chip.") [
      Helpers.apiTable "Chip.create" [
        Helpers.apiParam "label" "Doc" "" "Visual content displayed as the chip label"
        Helpers.apiParam
          "?onClick"
          "unit -> unit"
          ""
          "Click handler; makes the chip clickable with keyboard support"
        Helpers.apiParam "?onClose" "unit -> unit" "" "Close handler; shows a close button when provided"
        Helpers.apiParam "?closeIcon" "Doc" "" "Custom close icon (defaults to × character)"
        Helpers.apiParam "?content" "Doc" "" "Leading content slot (e.g. avatar, icon)"
        Helpers.apiParam "?href" "string" "" "URL to navigate to; renders the chip as an anchor element"
        Helpers.apiParam
          "?selected"
          "View<bool>"
          "View.Const false"
          "Reactive selected state for visual highlighting"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Whether the chip is interactive"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (variant, density, shape, color, etc.)"
      ]

      Helpers.styleModuleTable "Chip.Variant" [
        ("filled", "Solid background chip (default)")
        ("outlined", "Bordered chip with transparent background")
        ("text", "Text-only chip with no background or border")
      ]

      Helpers.styleModuleTable "Chip.Density" [
        ("compact", "Compact chip height")
        ("standard", "Standard chip height (default)")
        ("spacious", "Spacious chip height")
      ]

      Helpers.styleModuleTable "Chip.Shape" [
        ("label", "Rounded rectangle using the theme's default border radius")
      ]

      Helpers.styleModuleTable "Chip.Color" [
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
        Helpers.pageTitle "Chip"
        Helpers.bodyText "Chips represent compact elements such as tags, filters, or choices."

        Alert.create (
          div [ Typography.body1 ] [
            text "Need a group of selectable chips? See "
            Helpers.inlineCode "ChipSet"
            text
              " for built-in single/multi/toggle selection. Use standalone Chip when you need per-chip colors, conditional variant switching, or heterogeneous behavior. The ChipSet docs include a detailed comparison."
          ],
          attrs = [
            Alert.Color.info
            Alert.Variant.standard
            Margin.Bottom.extraSmall
            Margin.Top.extraSmall
          ]
        )

        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        selectedExample ()
        Helpers.divider ()
        closableExample ()
        Helpers.divider ()
        customCloseIconExample ()
        Helpers.divider ()
        clickableExample ()
        Helpers.divider ()
        iconExample ()
        Helpers.divider ()
        linkChipExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
