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
      ChipSet.ChipDef.create (text "Morning") "morning"
      ChipSet.ChipDef.create (text "Afternoon") "afternoon"
      ChipSet.ChipDef.create (text "Evening") "evening"
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          match s with
          | Some v -> sprintf "Selected: %s" v
          | None -> "No selection")
        |> View.printfn

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Outlined
                Chip.Color.toClass BrandColor.Primary
              ]
            ]
          ),
          selectedValue = selected,
          selectionMode = ChipSet.SelectionMode.Single
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let chips = [
    ChipSet.ChipDef.create (text "Morning") "morning"
    ChipSet.ChipDef.create (text "Afternoon") "afternoon"
    ChipSet.ChipDef.create (text "Evening") "evening"
]

let styledChips =
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Outlined
            Chip.Color.toClass BrandColor.Primary
        ]
    ])

ChipSet.Create(
    styledChips, // see here
    selectedValue = selected,
    selectionMode = ChipSet.SelectionMode.Single
)"""

    Helpers.codeSampleSection "Single Selection" description content code

  let private multiSelectionExample () =
    let description =
      Helpers.bodyText "Multi selection behaves like checkboxes. Any number of chips can be active at once."

    let selected = Var.Create<Set<string>> Set.empty

    let chips = [
      ChipSet.ChipDef.create (text "TypeScript") "typescript"
      ChipSet.ChipDef.create (text "F#") "fsharp"
      ChipSet.ChipDef.create (text "Rust") "rust"
      ChipSet.ChipDef.create (text "Go") "go"
      ChipSet.ChipDef.create (text "Python") "python"
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

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Outlined
                Chip.Color.toClass BrandColor.Secondary
              ]
            ]
          ),
          selectedValues = selected,
          selectionMode = ChipSet.SelectionMode.Multi
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<Set<string>> Set.empty

let chips = [
    ChipSet.ChipDef.create (text "TypeScript") "typescript"
    ChipSet.ChipDef.create (text "F#") "fsharp"
    ChipSet.ChipDef.create (text "Rust") "rust"
    ChipSet.ChipDef.create (text "Go") "go"
    ChipSet.ChipDef.create (text "Python") "python"
]

let styledChips =
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Outlined
            Chip.Color.toClass BrandColor.Secondary
        ]
    ])

ChipSet.Create(
    styledChips, // see here
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
      ChipSet.ChipDef.create (text "Draft") "draft"
      ChipSet.ChipDef.create (text "Published") "published"
      ChipSet.ChipDef.create (text "Archived") "archived"
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          match s with
          | Some v -> sprintf "Selected: %s" v
          | None -> "No selection")
        |> View.printfn

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Filled
                Chip.Color.toClass BrandColor.Primary
              ]
            ]
          ),
          selectedValue = selected,
          selectionMode = ChipSet.SelectionMode.Toggle
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let chips = [
    ChipSet.ChipDef.create (text "Draft") "draft"
    ChipSet.ChipDef.create (text "Published") "published"
    ChipSet.ChipDef.create (text "Archived") "archived"
]

let styledChips =
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Primary
        ]
    ])

ChipSet.Create(
    styledChips,
    selectedValue = selected,
    selectionMode = ChipSet.SelectionMode.Toggle // see here
)"""

    Helpers.codeSampleSection "Toggle Selection" description content code

  let private defaultSelectedExample () =
    let description =
      Helpers.bodyText "Set the initial value of the Var to pre-select chips on render."

    let selected = Var.Create<Set<string>>(Set.ofList [ "fsharp"; "rust" ])

    let chips = [
      ChipSet.ChipDef.create (text "TypeScript") "typescript"
      ChipSet.ChipDef.create (text "F#") "fsharp"
      ChipSet.ChipDef.create (text "Rust") "rust"
      ChipSet.ChipDef.create (text "Go") "go"
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

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Filled
                Chip.Color.toClass BrandColor.Primary
              ]
            ]
          ),
          selectedValues = selected,
          selectionMode = ChipSet.SelectionMode.Multi
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<Set<string>> (Set.ofList ["fsharp"; "rust"]) // see here

let chips = [
    ChipSet.ChipDef.create (text "TypeScript") "typescript"
    ChipSet.ChipDef.create (text "F#") "fsharp"
    ChipSet.ChipDef.create (text "Rust") "rust"
    ChipSet.ChipDef.create (text "Go") "go"
]

let styledChips =
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Primary
        ]
    ])

ChipSet.Create(
    styledChips,
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Default Selected" description content code

  let private variantsExample () =
    let description =
      Helpers.bodyText
        "Apply variant and color classes to each chip via ChipSet.ChipDef.withAttrs. Each variant renders differently."

    let filledSel = Var.Create<string option> None
    let outlinedSel = Var.Create<string option> None
    let textSel = Var.Create<string option> None

    let chips () = [
      ChipSet.ChipDef.create (text "Option A") "a"
      ChipSet.ChipDef.create (text "Option B") "b"
      ChipSet.ChipDef.create (text "Option C") "c"
    ]

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [] [
              Subtitle2.Div("Filled", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              ChipSet.Create(
                chips ()
                |> List.map (
                  ChipSet.ChipDef.withAttrs [
                    cls [
                      Chip.Variant.toClass Chip.Variant.Filled
                      Chip.Color.toClass BrandColor.Primary
                    ]
                  ]
                ),
                selectedValue = filledSel,
                selectionMode = ChipSet.SelectionMode.Toggle
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
          GridItem.Create(
            div [] [
              Subtitle2.Div("Outlined", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              ChipSet.Create(
                chips ()
                |> List.map (
                  ChipSet.ChipDef.withAttrs [
                    cls [
                      Chip.Variant.toClass Chip.Variant.Outlined
                      Chip.Color.toClass BrandColor.Secondary
                    ]
                  ]
                ),
                selectedValue = outlinedSel,
                selectionMode = ChipSet.SelectionMode.Toggle
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
          GridItem.Create(
            div [] [
              Subtitle2.Div("Text", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              ChipSet.Create(
                chips ()
                |> List.map (
                  ChipSet.ChipDef.withAttrs [
                    cls [
                      Chip.Variant.toClass Chip.Variant.Text
                      Chip.Color.toClass BrandColor.Tertiary
                    ]
                  ]
                ),
                selectedValue = textSel,
                selectionMode = ChipSet.SelectionMode.Toggle
              )
            ],
            xs = Grid.Width.create 12,
            md = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.toClass AlignItems.Start |> cl ]
      )

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<string option> None

let chips = [
    ChipSet.ChipDef.create (text "Option A") "a"
    ChipSet.ChipDef.create (text "Option B") "b"
    ChipSet.ChipDef.create (text "Option C") "c"
]

let styledChips =
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled // see here
            Chip.Color.toClass BrandColor.Primary
        ]
    ])

ChipSet.Create(
    styledChips,
    selectedValue = selected,
    selectionMode = ChipSet.SelectionMode.Toggle
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private iconsAndAvatarsExample () =
    let description =
      Helpers.bodyText "Use ChipDef.withContent to add leading visuals to individual chips in a set."

    let selected = Var.Create<Set<string>> Set.empty

    let chips = [
      ChipSet.ChipDef.create (text "Saved") "saved"
      |> ChipSet.ChipDef.withContent (Icon.Create(Icon.UiActions UiActions.CheckCircle))
      ChipSet.ChipDef.create (text "Warning") "warning"
      |> ChipSet.ChipDef.withContent (Icon.Create(Icon.Action Action.Warning))
      ChipSet.ChipDef.create (text "Alice") "alice"
      |> ChipSet.ChipDef.withContent (img [ attr.src "https://i.pravatar.cc/40?u=bob"; attr.alt "Alice" ] [])
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

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Filled
                Chip.Color.toClass BrandColor.Primary
                Density.toClass Density.Compact
              ]
            ]
          ),
          selectedValues = selected,
          selectionMode = ChipSet.SelectionMode.Multi
        )
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols
open WebSharper.UI
open WebSharper.UI.Html

let chips = [
    ChipSet.ChipDef.create (text "Saved") "saved"
    |> ChipSet.ChipDef.withContent (Icon.Create(Icon.UiActions UiActions.CheckCircle)) // see here

    ChipSet.ChipDef.create (text "Warning") "warning"
    |> ChipSet.ChipDef.withContent (Icon.Create(Icon.Action Action.Warning))

    ChipSet.ChipDef.create (text "Alice") "alice"
    |> ChipSet.ChipDef.withContent ( // see here
        img [
            attr.src "https://i.pravatar.cc/40?u=alice"
            attr.alt "Alice"
            Attr.Style "width" "100%"
            Attr.Style "height" "100%"
            Attr.Style "object-fit" "cover"
            Attr.Style "border-radius" "50%"
        ] []
    )
]

ChipSet.Create(
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Primary
        ]
    ]),
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Icons and Avatars" description content code

  let private closableExample () =
    let description =
      Helpers.bodyText
        "Pass an onClose callback to ChipSet.Create and mark chips as closable with ChipSet.ChipDef.withClosable."

    let items =
      Var.Create [
        ChipSet.ChipDef.create (text "React") "react" |> ChipSet.ChipDef.withClosable
        ChipSet.ChipDef.create (text "Angular") "angular"
        |> ChipSet.ChipDef.withClosable
        ChipSet.ChipDef.create (text "Vue") "vue" |> ChipSet.ChipDef.withClosable
        ChipSet.ChipDef.create (text "Svelte") "svelte" |> ChipSet.ChipDef.withClosable
        ChipSet.ChipDef.create (text "WebSharper") "websharper"
        |> ChipSet.ChipDef.withClosable
      ]

    let content =
      div [] [
        items.View
        |> Doc.BindView(fun currentItems ->
          if List.isEmpty currentItems then
            Button.Create(
              text "Reset",
              onClick =
                (fun () ->
                  Var.Set items [
                    ChipSet.ChipDef.create (text "React") "react" |> ChipSet.ChipDef.withClosable
                    ChipSet.ChipDef.create (text "Angular") "angular"
                    |> ChipSet.ChipDef.withClosable
                    ChipSet.ChipDef.create (text "Vue") "vue" |> ChipSet.ChipDef.withClosable
                    ChipSet.ChipDef.create (text "Svelte") "svelte" |> ChipSet.ChipDef.withClosable
                    ChipSet.ChipDef.create (text "WebSharper") "websharper"
                    |> ChipSet.ChipDef.withClosable
                  ]),
              attrs = [
                Button.Variant.Outlined |> Button.Variant.toClass |> cl
                Button.Color.toClass BrandColor.Primary |> cl
              ]
            )
          else
            ChipSet.Create(
              currentItems
              |> List.map (
                ChipSet.ChipDef.withAttrs [
                  cls [
                    Chip.Variant.toClass Chip.Variant.Outlined
                    Chip.Color.toClass BrandColor.Primary
                  ]
                ]
              ),
              onClose =
                (fun value -> items.Value |> List.filter (fun c -> c.Value <> value) |> Var.Set items)
            ))
      ]

    let code =
      """open Weave
open WebSharper.UI

let items = Var.Create [
    ChipSet.ChipDef.create (text "React") "react" |> ChipSet.ChipDef.withClosable
    ChipSet.ChipDef.create (text "Angular") "angular" |> ChipSet.ChipDef.withClosable
    ChipSet.ChipDef.create (text "Vue") "vue" |> ChipSet.ChipDef.withClosable
]

ChipSet.Create(
    items.Value
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Outlined
            Chip.Color.toClass BrandColor.Primary
        ]
    ]),
    onClose = fun value -> // see here
        items.Value
        |> List.filter (fun c -> c.Value <> value)
        |> Var.Set items
)"""

    Helpers.codeSampleSection "Closable" description content code

  let private disabledExample () =
    let description =
      Helpers.bodyText
        "Use ChipSet.ChipDef.withDisabled to disable individual chips within a set. Disabled chips cannot be selected."

    let selected = Var.Create<Set<string>> Set.empty

    let chips = [
      ChipSet.ChipDef.create (text "Available") "available"
      ChipSet.ChipDef.create (text "Sold Out") "soldout"
      |> ChipSet.ChipDef.withDisabled (View.Const true)
      ChipSet.ChipDef.create (text "Preorder") "preorder"
      ChipSet.ChipDef.create (text "Discontinued") "discontinued"
      |> ChipSet.ChipDef.withDisabled (View.Const true)
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

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Filled
                Chip.Color.toClass BrandColor.Primary
              ]
            ]
          ),
          selectedValues = selected,
          selectionMode = ChipSet.SelectionMode.Multi
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let selected = Var.Create<Set<string>> Set.empty

let chips = [
    ChipSet.ChipDef.create (text "Available") "available"
    ChipSet.ChipDef.create (text "Sold Out") "soldout"
    |> ChipSet.ChipDef.withDisabled (View.Const true) // see here
    ChipSet.ChipDef.create (text "Preorder") "preorder"
    ChipSet.ChipDef.create (text "Discontinued") "discontinued"
    |> ChipSet.ChipDef.withDisabled (View.Const true)
]

ChipSet.Create(
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Primary
        ]
    ]),
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi
)"""

    Helpers.codeSampleSection "Disabled" description content code

  let private globalDisabledExample () =
    let description =
      Helpers.bodyText
        "Pass an enabled parameter to ChipSet.Create to disable the entire set reactively. Toggle the switch to see the effect."

    let enabled = Var.Create true
    let selected = Var.Create<Set<string>> Set.empty

    let chips = [
      ChipSet.ChipDef.create (text "Option A") "a"
      ChipSet.ChipDef.create (text "Option B") "b"
      ChipSet.ChipDef.create (text "Option C") "c"
    ]

    let content =
      div [] [
        div [
          cls [ Flex.toClass None Flex.Flex; AlignItems.toClass AlignItems.Center ]
          Margin.toClasses Margin.Bottom.extraSmall |> cls
        ] [
          Switch.Create(enabled, attrs = [ Margin.toClasses Margin.Right.extraSmall |> cls ])
          Body2.Div(enabled.View |> View.MapCached(fun e -> if e then "Enabled" else "Disabled"))
        ]

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Filled
                Chip.Color.toClass BrandColor.Primary
              ]
            ]
          ),
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
    ChipSet.ChipDef.create (text "Option A") "a"
    ChipSet.ChipDef.create (text "Option B") "b"
    ChipSet.ChipDef.create (text "Option C") "c"
]

ChipSet.Create(
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Primary
        ]
    ]),
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi,
    enabled = enabled.View // see here
)"""

    Helpers.codeSampleSection "Global Disabled" description content code

  let private customSelectedIconExample () =
    let description =
      Helpers.bodyText
        "Pass a selectedIcon factory to replace the default checkmark with a custom icon on selected chips."

    let selected = Var.Create<Set<string>> Set.empty

    let chips = [
      ChipSet.ChipDef.create (text "Favorite") "favorite"
      ChipSet.ChipDef.create (text "Bookmark") "bookmark"
      ChipSet.ChipDef.create (text "Pinned") "pinned"
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

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Filled
                Chip.Color.toClass BrandColor.Error
              ]
            ]
          ),
          selectedValues = selected,
          selectionMode = ChipSet.SelectionMode.Multi,
          selectedIcon = (fun () -> Icon.Create(Icon.UiActions UiActions.Favorite))
        )
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols
open WebSharper.UI

let selected = Var.Create<Set<string>> Set.empty

let chips = [
    ChipSet.ChipDef.create (text "Favorite") "favorite"
    ChipSet.ChipDef.create (text "Bookmark") "bookmark"
    ChipSet.ChipDef.create (text "Pinned") "pinned"
]

ChipSet.Create(
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Error
        ]
    ]),
    selectedValues = selected,
    selectionMode = ChipSet.SelectionMode.Multi,
    selectedIcon = Some (fun () -> Icon.Create(Icon.UiActions UiActions.Favorite)) // see here
)"""

    Helpers.codeSampleSection "Custom Selected Icon" description content code

  let private noSelectedIconExample () =
    let description =
      Helpers.bodyText "Pass selectedIcon = None to disable the default checkmark icon on selected chips."

    let selected = Var.Create<string option> None

    let chips = [
      ChipSet.ChipDef.create (text "Small") "small"
      ChipSet.ChipDef.create (text "Medium") "medium"
      ChipSet.ChipDef.create (text "Large") "large"
    ]

    let content =
      div [] [
        selected.View
        |> View.MapCached(fun s ->
          match s with
          | Some v -> sprintf "Selected: %s" v
          | None -> "No selection")
        |> View.printfn

        ChipSet.Create(
          chips
          |> List.map (
            ChipSet.ChipDef.withAttrs [
              cls [
                Chip.Variant.toClass Chip.Variant.Outlined
                Chip.Color.toClass BrandColor.Primary
              ]
            ]
          ),
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
    ChipSet.ChipDef.create (text "Small") "small"
    ChipSet.ChipDef.create (text "Medium") "medium"
    ChipSet.ChipDef.create (text "Large") "large"
]

ChipSet.Create(
    chips
    |> List.map (ChipSet.ChipDef.withAttrs [
        cls [
            Chip.Variant.toClass Chip.Variant.Outlined
            Chip.Color.toClass BrandColor.Primary
        ]
    ]),
    selectedValue = selected,
    selectionMode = ChipSet.SelectionMode.Toggle,
    showSelectedIcon = false // see here
)"""

    Helpers.codeSampleSection "No Selected Icon" description content code

  let render () =
    Container.Create(
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
      maxWidth = Container.MaxWidth.Large
    )
