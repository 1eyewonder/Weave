namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ChipSetExamples =

  let private singleSelectionExample () =
    let description =
      Helpers.bodyText "Single selection behaves like a radio group. One chip can be active at a time."

    let selected = Var.Create<string option> None

    let chips = [
      ChipItem.create (text "Morning", "morning", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
      ChipItem.create (text "Afternoon", "afternoon", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
      ChipItem.create (text "Evening", "evening", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          match s with
          | Some v -> sprintf "Selected: %s" v
          | None -> "No selection")
        |> View.printfn

        ChipSet.create (chips, selectedValue = selected, selectionMode = ChipSet.SelectionMode.Single)
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let chips = [
    ChipItem.create (text "Morning", "morning", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
    ChipItem.create (text "Afternoon", "afternoon", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
    ChipItem.create (text "Evening", "evening", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
]

ChipSet.create(
    chips,
    selectedValue = selected,
    selectionMode = ChipSet.SelectionMode.Single
)"""

    Helpers.codeSampleSection "Single Selection" description content code

  let private multiSelectionExample () =
    let description =
      Helpers.bodyText "Multi selection behaves like checkboxes. Any number of chips can be active at once."

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

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          if Set.isEmpty s then
            "No selection"
          else
            s |> Set.toList |> String.concat ", " |> sprintf "Selected: %s")
        |> View.printfn

        ChipSet.create (chips, selectedValues = selected, selectionMode = ChipSet.SelectionMode.Multi)
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<Set<string>> Set.empty

let chips = [
    ChipItem.create (text "TypeScript", "typescript", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
    ChipItem.create (text "F#", "fsharp", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
    ChipItem.create (text "Rust", "rust", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
    ChipItem.create (text "Go", "go", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
    ChipItem.create (text "Python", "python", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
]

ChipSet.create(
    chips,
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Multi Selection" description content code

  let private toggleSelectionExample () =
    let description =
      Helpers.bodyText
        "Toggle selection lets you select and deselect a single chip. Clicking the active chip clears the selection."

    let selected = Var.Create<string option> None

    let chips = [
      ChipItem.create (text "Draft", "draft", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ChipItem.create (text "Published", "published", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ChipItem.create (text "Archived", "archived", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          match s with
          | Some v -> sprintf "Selected: %s" v
          | None -> "No selection")
        |> View.printfn

        ChipSet.create (chips, selectedValue = selected, selectionMode = ChipSet.SelectionMode.Toggle)
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let chips = [
    ChipItem.create (text "Draft", "draft", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Published", "published", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Archived", "archived", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
]

ChipSet.create(
    chips,
    selectedValue = selected,
    selectionMode = ChipSet.SelectionMode.Toggle
)"""

    Helpers.codeSampleSection "Toggle Selection" description content code

  let private defaultSelectedExample () =
    let description =
      Helpers.bodyText "Set the initial value of the Var to pre-select chips on render."

    let selected = Var.Create<Set<string>>(Set.ofList [ "fsharp"; "rust" ])

    let chips = [
      ChipItem.create (text "TypeScript", "typescript", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ChipItem.create (text "F#", "fsharp", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ChipItem.create (text "Rust", "rust", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ChipItem.create (text "Go", "go", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          if Set.isEmpty s then
            "No selection"
          else
            s |> Set.toList |> String.concat ", " |> sprintf "Selected: %s")
        |> View.printfn

        ChipSet.create (chips, selectedValues = selected, selectionMode = ChipSet.SelectionMode.Multi)
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<Set<string>> (Set.ofList ["fsharp"; "rust"])

let chips = [
    ChipItem.create (text "TypeScript", "typescript", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "F#", "fsharp", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Rust", "rust", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Go", "go", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
]

ChipSet.create(
    chips,
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Default Selected" description content code

  let private variantsExample () =
    let description =
      Helpers.bodyText
        "Apply variant and color classes to each chip via the attrs parameter. Each variant renders differently."

    let filledSel = Var.Create<string option> None
    let outlinedSel = Var.Create<string option> None
    let textSel = Var.Create<string option> None

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Filled" ]
              ChipSet.create (
                [
                  ChipItem.create (text "Option A", "a", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
                  ChipItem.create (text "Option B", "b", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
                  ChipItem.create (text "Option C", "c", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
                ],
                selectedValue = filledSel,
                selectionMode = ChipSet.SelectionMode.Toggle
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Outlined" ]
              ChipSet.create (
                [
                  ChipItem.create (
                    text "Option A",
                    "a",
                    attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
                  )
                  ChipItem.create (
                    text "Option B",
                    "b",
                    attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
                  )
                  ChipItem.create (
                    text "Option C",
                    "c",
                    attrs = [ Chip.Variant.outlined; Chip.Color.secondary ]
                  )
                ],
                selectedValue = outlinedSel,
                selectionMode = ChipSet.SelectionMode.Toggle
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Text" ]
              ChipSet.create (
                [
                  ChipItem.create (text "Option A", "a", attrs = [ Chip.Variant.text; Chip.Color.tertiary ])
                  ChipItem.create (text "Option B", "b", attrs = [ Chip.Variant.text; Chip.Color.tertiary ])
                  ChipItem.create (text "Option C", "c", attrs = [ Chip.Variant.text; Chip.Color.tertiary ])
                ],
                selectedValue = textSel,
                selectionMode = ChipSet.SelectionMode.Toggle
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
        ],
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let chips = [
    ChipItem.create (text "Option A", "a", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Option B", "b", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Option C", "c", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
]

ChipSet.create(
    chips,
    selectedValue = selected,
    selectionMode = ChipSet.SelectionMode.Toggle
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private iconsAndAvatarsExample () =
    let description =
      Helpers.bodyText "Use the content parameter to add leading visuals to individual chips in a set."

    let selected = Var.Create<Set<string>> Set.empty

    let chips = [
      ChipItem.create (
        text "Saved",
        "saved",
        content = Icon.create (Icon.UiActions UiActions.CheckCircle),
        attrs = [ Chip.Variant.filled; Chip.Color.primary; Density.compact ]
      )
      ChipItem.create (
        text "Warning",
        "warning",
        content = Icon.create (Icon.Action Action.Warning),
        attrs = [ Chip.Variant.filled; Chip.Color.primary; Density.compact ]
      )
      ChipItem.create (
        text "Alice",
        "alice",
        content = img [ attr.src "https://i.pravatar.cc/40?u=bob"; attr.alt "Alice" ] [],
        attrs = [ Chip.Variant.filled; Chip.Color.primary; Density.compact ]
      )
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          if Set.isEmpty s then
            "No selection"
          else
            s |> Set.toList |> String.concat ", " |> sprintf "Selected: %s")
        |> View.printfn

        ChipSet.create (chips, selectedValues = selected, selectionMode = ChipSet.SelectionMode.Multi)
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols
open WebSharper.UI
open WebSharper.UI.Html

let chips = [
    ChipItem.create (
        text "Saved", "saved",
        content = Icon.create(Icon.UiActions UiActions.CheckCircle),
        attrs = [ Chip.Variant.filled; Chip.Color.primary ]
    )
    ChipItem.create (
        text "Warning", "warning",
        content = Icon.create(Icon.Action Action.Warning),
        attrs = [ Chip.Variant.filled; Chip.Color.primary ]
    )
    ChipItem.create (
        text "Alice", "alice",
        content = img [
            attr.src "https://i.pravatar.cc/40?u=alice"
            attr.alt "Alice"
            Attr.Style "width" "100%"
            Attr.Style "height" "100%"
            Attr.Style "object-fit" "cover"
            Attr.Style "border-radius" "50%"
        ] [],
        attrs = [ Chip.Variant.filled; Chip.Color.primary ]
    )
]

ChipSet.create(
    chips,
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Icons and Avatars" description content code

  let private closableExample () =
    let description =
      Helpers.bodyText
        "Pass an onClose callback to ChipSet.create and mark chips as closable with the closable parameter."

    let items =
      Var.Create [
        ChipItem.create (
          text "React",
          "react",
          closable = true,
          attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
        )
        ChipItem.create (
          text "Angular",
          "angular",
          closable = true,
          attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
        )
        ChipItem.create (
          text "Vue",
          "vue",
          closable = true,
          attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
        )
        ChipItem.create (
          text "Svelte",
          "svelte",
          closable = true,
          attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
        )
        ChipItem.create (
          text "WebSharper",
          "websharper",
          closable = true,
          attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
        )
      ]

    let content =
      div [] [
        items.View
        |> Doc.BindView(fun currentItems ->
          if List.isEmpty currentItems then
            Button.primary (
              text "Reset",
              onClick =
                (fun () ->
                  Var.Set items [
                    ChipItem.create (
                      text "React",
                      "react",
                      closable = true,
                      attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
                    )
                    ChipItem.create (
                      text "Angular",
                      "angular",
                      closable = true,
                      attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
                    )
                    ChipItem.create (
                      text "Vue",
                      "vue",
                      closable = true,
                      attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
                    )
                    ChipItem.create (
                      text "Svelte",
                      "svelte",
                      closable = true,
                      attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
                    )
                    ChipItem.create (
                      text "WebSharper",
                      "websharper",
                      closable = true,
                      attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
                    )
                  ]),
              attrs = [ Button.Variant.outlined ]
            )
          else
            ChipSet.create (
              currentItems,
              onClose =
                (fun value -> items.Value |> List.filter (fun c -> c.Value <> value) |> Var.Set items)
            ))
      ]

    let code =
      """open Weave
open WebSharper.UI

let items = Var.Create [
    ChipItem.create (text "React", "react", closable = true, attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
    ChipItem.create (text "Angular", "angular", closable = true, attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
    ChipItem.create (text "Vue", "vue", closable = true, attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
]

ChipSet.create(
    items.Value,
    onClose = fun value ->
        items.Value
        |> List.filter (fun c -> c.Value <> value)
        |> Var.Set items
)"""

    Helpers.codeSampleSection "Closable" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText
        "Use the disabled parameter to disable individual chips within a set. Disabled chips cannot be selected."

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

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          if Set.isEmpty s then
            "No selection"
          else
            s |> Set.toList |> String.concat ", " |> sprintf "Selected: %s")
        |> View.printfn

        ChipSet.create (chips, selectedValues = selected, selectionMode = ChipSet.SelectionMode.Multi)
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<Set<string>> Set.empty

let chips = [
    ChipItem.create (text "Available", "available", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Sold Out", "soldout", disabled = View.Const true, attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Preorder", "preorder", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Discontinued", "discontinued", disabled = View.Const true, attrs = [ Chip.Variant.filled; Chip.Color.primary ])
]

ChipSet.create(
    chips,
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Disabled" description content code

  let private globalDisabledExample () =
    let description =
      Helpers.bodyText
        "Pass an enabled parameter to ChipSet.create to disable the entire set reactively. Toggle the switch to see the effect."

    let enabled = Var.Create true
    let selected = Var.Create<Set<string>> Set.empty

    let chips = [
      ChipItem.create (text "Option A", "a", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ChipItem.create (text "Option B", "b", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
      ChipItem.create (text "Option C", "c", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ]

    let content =
      div [] [
        div [ Flex.Flex.allSizes; AlignItems.center; Margin.Bottom.extraSmall ] [
          Switch.create (enabled, attrs = [ Margin.Right.extraSmall ])
          div [ Typography.body2 ] [
            textView (enabled.View |> View.MapCached(fun e -> if e then "Enabled" else "Disabled"))
          ]
        ]

        ChipSet.create (
          chips,
          selectedValues = selected,
          selectionMode = ChipSet.SelectionMode.Multi,
          enabled = enabled.View
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let enabled = Var.Create true
let selected = Var.Create<Set<string>> Set.empty

let chips = [
    ChipItem.create (text "Option A", "a", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Option B", "b", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
    ChipItem.create (text "Option C", "c", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
]

ChipSet.create(
    chips,
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi,
    enabled = enabled.View
)"""

    Helpers.codeSampleSection "Global Disabled" description content code

  let private customSelectedIconExample () =
    let description =
      Helpers.bodyText
        "Pass a selectedIcon factory to replace the default checkmark with a custom icon on selected chips."

    let selected = Var.Create<Set<string>> Set.empty

    let chips = [
      ChipItem.create (text "Favorite", "favorite", attrs = [ Chip.Variant.filled; Chip.Color.error ])
      ChipItem.create (text "Bookmark", "bookmark", attrs = [ Chip.Variant.filled; Chip.Color.error ])
      ChipItem.create (text "Pinned", "pinned", attrs = [ Chip.Variant.filled; Chip.Color.error ])
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          if Set.isEmpty s then
            "No selection"
          else
            s |> Set.toList |> String.concat ", " |> sprintf "Selected: %s")
        |> View.printfn

        ChipSet.create (
          chips,
          selectedValues = selected,
          selectionMode = ChipSet.SelectionMode.Multi,
          selectedIcon = (fun () -> Icon.create (Icon.UiActions UiActions.Favorite))
        )
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols
open WebSharper.UI

let selected = Var.Create<Set<string>> Set.empty

let chips = [
    ChipItem.create (text "Favorite", "favorite", attrs = [ Chip.Variant.filled; Chip.Color.error ])
    ChipItem.create (text "Bookmark", "bookmark", attrs = [ Chip.Variant.filled; Chip.Color.error ])
    ChipItem.create (text "Pinned", "pinned", attrs = [ Chip.Variant.filled; Chip.Color.error ])
]

ChipSet.create(
    chips,
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi,
    selectedIcon = (fun () -> Icon.create(Icon.UiActions UiActions.Favorite))
)"""

    Helpers.codeSampleSection "Custom Selected Icon" description content code

  let private noSelectedIconExample () =
    let description =
      Helpers.bodyText "Pass selectedIcon = None to disable the default checkmark icon on selected chips."

    let selected = Var.Create<string option> None

    let chips = [
      ChipItem.create (text "Small", "small", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
      ChipItem.create (text "Medium", "medium", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
      ChipItem.create (text "Large", "large", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          match s with
          | Some v -> sprintf "Selected: %s" v
          | None -> "No selection")
        |> View.printfn

        ChipSet.create (
          chips,
          selectedValue = selected,
          selectionMode = ChipSet.SelectionMode.Toggle,
          showSelectedIcon = false
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let chips = [
    ChipItem.create (text "Small", "small", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
    ChipItem.create (text "Medium", "medium", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
    ChipItem.create (text "Large", "large", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
]

ChipSet.create(
    chips,
    selectedValue = selected,
    selectionMode = ChipSet.SelectionMode.Toggle,
    showSelectedIcon = false
)"""

    Helpers.codeSampleSection "No Selected Icon" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Chip Set"
        Helpers.bodyText
          "ChipSet groups multiple chips into a selectable set with single or multi selection modes."
        Helpers.divider ()
        singleSelectionExample ()
        Helpers.divider ()
        multiSelectionExample ()
        Helpers.divider ()
        toggleSelectionExample ()
        Helpers.divider ()
        defaultSelectedExample ()
        Helpers.divider ()
        variantsExample ()
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
        noSelectedIconExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
