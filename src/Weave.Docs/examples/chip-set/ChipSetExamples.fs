namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ChipSetExamples =

  let private center content =
    div [ Flex.Flex.allSizes; JustifyContent.center ] [ content ]

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

  let private singleSelectionExample () =
    let description =
      Helpers.bodyText "Single selection behaves like a radio group. One chip can be active at a time."

    let content =
      let selected = Var.Create<string option> None

      let chips = [
        ChipItem.create (text "Morning", "morning", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
        ChipItem.create (text "Afternoon", "afternoon", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
        ChipItem.create (text "Evening", "evening", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
      ]

      ChipSet.create (chips, selectedValue = selected, selectionMode = ChipSet.SelectionMode.Single)

    let code =
      """let selected = Var.Create<string option> None

let chips = [
  ChipItem.create (
    text "Morning",
    "morning",
    attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Afternoon",
    "afternoon",
    attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Evening",
    "evening",
    attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
  )
]

ChipSet.create (
  chips,
  selectedValue = selected,
  selectionMode = ChipSet.SelectionMode.Single
)"""

    Helpers.codeSampleSection "Single Selection" description (center content) code

  let private multiSelectionExample () =
    let description =
      Helpers.bodyText "Multi selection behaves like checkboxes. Any number of chips can be active at once."

    let content =
      let selected = Var.Create<Set<string>> Set.empty

      let chips = [
        ChipItem.create (
          text "TypeScript",
          "typescript",
          attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
        )
        ChipItem.create (text "F#", "fsharp", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
        ChipItem.create (text "Rust", "rust", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
        ChipItem.create (text "Go", "go", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
        ChipItem.create (text "Python", "python", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
      ]

      ChipSet.create (chips, selectedValues = selected, selectionMode = ChipSet.SelectionMode.Multi)

    let code =
      """let selected = Var.Create<Set<string>> Set.empty

let chips = [
  ChipItem.create (
    text "TypeScript",
    "typescript",
    attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
  )
  ChipItem.create (
    text "F#",
    "fsharp",
    attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
  )
  ChipItem.create (
    text "Rust",
    "rust",
    attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
  )
  ChipItem.create (
    text "Go",
    "go",
    attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
  )
  ChipItem.create (
    text "Python",
    "python",
    attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
  )
]

ChipSet.create (
  chips,
  selectedValues = selected,
  selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Multi Selection" description (center content) code

  let private defaultSelectedExample () =
    let description =
      Helpers.bodyText "Set the initial value of the Var to pre-select chips on render."

    let content =
      let selected = Var.Create<Set<string>>(Set.ofList [ "fsharp"; "rust" ])

      let chips = [
        ChipItem.create (text "TypeScript", "typescript", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
        ChipItem.create (text "F#", "fsharp", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
        ChipItem.create (text "Rust", "rust", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
        ChipItem.create (text "Go", "go", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ]

      ChipSet.create (chips, selectedValues = selected, selectionMode = ChipSet.SelectionMode.Multi)

    let code =
      """let selected = Var.Create<Set<string>>(Set.ofList [ "fsharp"; "rust" ])

let chips = [
  ChipItem.create (
    text "TypeScript",
    "typescript",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "F#",
    "fsharp",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Rust",
    "rust",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Go",
    "go",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
]

ChipSet.create (
  chips,
  selectedValues = selected,
  selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Default Selected" description (center content) code

  let private iconsAndAvatarsExample () =
    let description =
      Helpers.bodyText
        "ChipItem content accepts any Doc. These items use an icon or initials beside their visible label."

    let content =
      let selected = Var.Create<Set<string>> Set.empty

      let chips = [
        ChipItem.create (
          text "Saved",
          "saved",
          content = Icon.create (Icon.UiActions UiActions.CheckCircle),
          attrs = [ Chip.Variant.filled; Chip.Color.primary ]
        )
        ChipItem.create (
          text "Warning",
          "warning",
          content = Icon.create (Icon.Action Action.Warning),
          attrs = [ Chip.Variant.filled; Chip.Color.primary ]
        )
        ChipItem.create (
          text "Alice",
          "alice",
          content = text "AL",
          attrs = [ Chip.Variant.filled; Chip.Color.primary ]
        )
      ]

      ChipSet.create (chips, selectedValues = selected, selectionMode = ChipSet.SelectionMode.Multi)

    let code =
      """open Weave.Icons.MaterialSymbols

let selected = Var.Create<Set<string>> Set.empty

let chips = [
  ChipItem.create (
    text "Saved",
    "saved",
    content = Icon.create (Icon.UiActions UiActions.CheckCircle),
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Warning",
    "warning",
    content = Icon.create (Icon.Action Action.Warning),
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Alice",
    "alice",
    content = text "AL",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
]

ChipSet.create (
  chips,
  selectedValues = selected,
  selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Icons and Avatars" description (center content) code

  let private closableExample () =
    let description =
      Helpers.bodyText
        "Handle onClose by removing the item from your state. Bind the item list so the set rerenders after removal; restore the list when it is empty."

    let content =
      let initialItems =
        [ "React"; "Angular"; "Vue" ]
        |> List.map (fun value ->
          ChipItem.create (
            text value,
            value,
            closable = true,
            attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
          ))

      let items = Var.Create initialItems

      let removeItem value =
        Var.Update items (List.filter (fun item -> item.Value <> value))

      items.View
      |> Doc.BindView(fun current ->
        if List.isEmpty current then
          Button.create (text "Restore chips", onClick = (fun () -> Var.Set items initialItems))
        else
          ChipSet.create (current, onClose = removeItem))

    let code =
      """let initialItems =
  [ "React"; "Angular"; "Vue" ]
  |> List.map (fun value ->
    ChipItem.create (
      text value,
      value,
      closable = true,
      attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
    ))

let items = Var.Create initialItems

let removeItem value =
  Var.Update items (List.filter (fun item -> item.Value <> value))

items.View
|> Doc.BindView(fun current ->
  if List.isEmpty current then
    Button.create (
      text "Restore chips",
      onClick = (fun () -> Var.Set items initialItems)
    )
  else
    ChipSet.create (current, onClose = removeItem))"""

    Helpers.codeSampleSection "Closable" description (center content) code

  let private disabledExample () =
    let description =
      Helpers.bodyText
        "Use the disabled parameter to disable individual chips within a set. Disabled chips cannot be selected."

    let content =
      let selected = Var.Create<Set<string>> Set.empty

      let chips = [
        ChipItem.create (text "Available", "available", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
        ChipItem.create (
          text "Sold Out",
          "soldout",
          disabled = View.Const true,
          attrs = [ Chip.Variant.filled; Chip.Color.primary ]
        )
        ChipItem.create (text "Preorder", "preorder", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
        ChipItem.create (
          text "Discontinued",
          "discontinued",
          disabled = View.Const true,
          attrs = [ Chip.Variant.filled; Chip.Color.primary ]
        )
      ]

      ChipSet.create (chips, selectedValues = selected, selectionMode = ChipSet.SelectionMode.Multi)

    let code =
      """let selected = Var.Create<Set<string>> Set.empty

let chips = [
  ChipItem.create (
    text "Available",
    "available",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Sold Out",
    "soldout",
    disabled = View.Const true,
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Preorder",
    "preorder",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Discontinued",
    "discontinued",
    disabled = View.Const true,
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
]

ChipSet.create (
  chips,
  selectedValues = selected,
  selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Disabled" description (center content) code

  let private globalDisabledExample () =
    let description =
      Helpers.bodyText
        "Pass enabled to disable the entire set reactively. Use the switch to compare enabled and disabled states."

    let content =
      let enabled = Var.Create true

      let selected = Var.Create<Set<string>> Set.empty

      let chips = [
        ChipItem.create (text "Option A", "a", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
        ChipItem.create (text "Option B", "b", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
        ChipItem.create (text "Option C", "c", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ]

      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
      ] [
        Switch.create (enabled, content = text "Enable chip set")

        ChipSet.create (
          chips,
          selectedValues = selected,
          selectionMode = ChipSet.SelectionMode.Multi,
          enabled = enabled.View
        )
      ]

    let code =
      """let enabled = Var.Create true

let selected = Var.Create<Set<string>> Set.empty

let chips = [
  ChipItem.create (
    text "Option A",
    "a",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Option B",
    "b",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
  ChipItem.create (
    text "Option C",
    "c",
    attrs = [ Chip.Variant.filled; Chip.Color.primary ]
  )
]

Doc.Concat [
  Switch.create (enabled, content = text "Enable chip set")

  ChipSet.create (
    chips,
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi,
    enabled = enabled.View
  )
]"""

    Helpers.codeSampleSection "Global Disabled" description (center content) code

  let private customSelectedIconExample () =
    let description =
      Helpers.bodyText
        "Pass a selectedIcon factory to replace the default checkmark with a custom icon on selected chips."

    let content =
      let selected = Var.Create<Set<string>>(Set.singleton "favorite")

      let chips = [
        ChipItem.create (text "Favorite", "favorite", attrs = [ Chip.Variant.filled; Chip.Color.error ])
        ChipItem.create (text "Bookmark", "bookmark", attrs = [ Chip.Variant.filled; Chip.Color.error ])
        ChipItem.create (text "Pinned", "pinned", attrs = [ Chip.Variant.filled; Chip.Color.error ])
      ]

      ChipSet.create (
        chips,
        selectedValues = selected,
        selectionMode = ChipSet.SelectionMode.Multi,
        selectedIcon = (fun () -> Icon.create (Icon.UiActions UiActions.Favorite))
      )

    let code =
      """open Weave.Icons.MaterialSymbols

let selected = Var.Create<Set<string>>(Set.singleton "favorite")

let chips = [
  ChipItem.create (
    text "Favorite",
    "favorite",
    attrs = [ Chip.Variant.filled; Chip.Color.error ]
  )
  ChipItem.create (
    text "Bookmark",
    "bookmark",
    attrs = [ Chip.Variant.filled; Chip.Color.error ]
  )
  ChipItem.create (
    text "Pinned",
    "pinned",
    attrs = [ Chip.Variant.filled; Chip.Color.error ]
  )
]

ChipSet.create (
  chips,
  selectedValues = selected,
  selectionMode = ChipSet.SelectionMode.Multi,
  selectedIcon = (fun () -> Icon.create (Icon.UiActions UiActions.Favorite))
)"""

    Helpers.codeSampleSection "Custom Selected Icon" description (center content) code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for ChipSet and ChipItem.") [
      Helpers.apiTable "ChipSet.create" [
        Helpers.apiParam "chips" "ChipDef list" "" "List of chip definitions built with ChipItem.create"
        Helpers.apiParam
          "?selectedValue"
          "Var<string option>"
          ""
          "Two-way binding for single/toggle selection (Some value or None)"
        Helpers.apiParam "?selectedValues" "Var<Set<string>>" "" "Two-way binding for multi-selection"
        Helpers.apiParam
          "?selectionMode"
          "SelectionMode"
          "Single"
          "How chips respond to clicks — Single, Toggle, or Multi"
        Helpers.apiParam "?selectedIcon" "(unit -> Doc)" "" "Custom icon factory shown on selected chips"
        Helpers.apiParam
          "?showSelectedIcon"
          "bool"
          "true"
          "Whether to display the selected icon on chosen chips"
        Helpers.apiParam
          "?onClose"
          "string -> unit"
          ""
          "Callback when a closable chip's close button is clicked; receives the chip value"
        Helpers.apiParam
          "?enabled"
          "View<bool>"
          "View.Const true"
          "Whether the entire chip set is interactive"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
      ]

      Helpers.returnTypeNote
        "ChipItem.create returns a ChipSet.ChipDef record, not a Doc. Build items with ChipItem.create and pass them as a list to ChipSet.create."

      Helpers.apiTable "ChipItem.create" [
        Helpers.apiParam "label" "Doc" "" "Visual content displayed as the chip label"
        Helpers.apiParam "value" "string" "" "Unique identifier for this chip used in selection"
        Helpers.apiParam
          "?content"
          "Doc"
          ""
          "Optional content slot (e.g. avatar or icon); replaced by selected icon when chosen"
        Helpers.apiParam "?closable" "bool" "false" "Whether this chip shows a close button"
        Helpers.apiParam
          "?disabled"
          "View<bool>"
          "View.Const false"
          "Whether this individual chip is non-interactive"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to this chip"
      ]

      Helpers.styleModuleTable "ChipSet.SelectionMode (DU)" [
        ("Single", "Clicking a chip selects it; clicking another deselects the previous one")
        ("Toggle", "Like Single, but clicking the already-selected chip deselects it")
        ("Multi", "Multiple chips can be selected simultaneously")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Chip Set"
        Helpers.bodyText
          "ChipSet groups multiple chips into a selectable set with single or multi selection modes."
        Helpers.divider ()
        span [ attr.id "when-to-use" ] []
        singleSelectionExample ()
        Helpers.divider ()
        multiSelectionExample ()
        Helpers.divider ()
        defaultSelectedExample ()
        Helpers.divider ()
        ChipSetConfigurator.render ()
        Helpers.divider ()
        iconsAndAvatarsExample ()
        Helpers.divider ()
        closableExample ()
        Helpers.divider ()
        disabledExample ()
        Helpers.divider ()
        globalDisabledExample ()
        Helpers.divider ()
        customSelectedIconExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
