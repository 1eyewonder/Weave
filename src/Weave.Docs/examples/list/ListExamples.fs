namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

open Weave.Icons
open Weave.Icons.MaterialSymbols
open WeaveList

[<JavaScript>]
module ListExamples =

  let private comparisonGrid columns items =
    let cells =
      items
      |> List.map (fun item ->
        GridItem.create (
          item,
          attrs = [
            GridItem.Span.twelve
            GridItem.Span.Small.six
            (if columns = 4 then GridItem.Span.Medium.three
             elif columns = 3 then GridItem.Span.Medium.four
             else GridItem.Span.Medium.six)
          ]
        ))

    Grid.create (cells, attrs = [ Grid.Spacing.large ])

  let private iconLabel (icon: Doc) (label: string) =
    div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [ icon; text label ]

  let private simpleListExample () =
    let description =
      Helpers.bodyText
        "WeaveList displays a collection of items. Icons, avatars, or other content can be included directly in the item's content. Use ListChild.Content to include arbitrary elements like dividers."

    let content =
      let iconLabel (icon: Doc) (label: string) =
        div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [ icon; text label ]

      let inboxIcon = Icon.create (Icon.Communicate Communicate.Inbox)

      let sendIcon = Icon.create (Icon.Communicate Communicate.Send)

      let draftsIcon = Icon.create (Icon.Communicate Communicate.Drafts)

      WeaveList.create (
        [
          ListItem.create (iconLabel inboxIcon "Inbox")
          ListItem.create (iconLabel sendIcon "Sent")
          ListItem.create (iconLabel draftsIcon "Drafts")
          ListChild.Content(Divider.create ())
          ListItem.create (text "Trash", secondaryContent = text "Removed e-mails")
          ListItem.create (text "Spam", secondaryContent = text "E-mails from common providers")
        ]
      )

    let code =
      """open WeaveList
open Weave.Icons
open Weave.Icons.MaterialSymbols

let iconLabel (icon: Doc) (label: string) =
  div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [
    icon
    text label
  ]

let inboxIcon = Icon.create (Icon.Communicate Communicate.Inbox)

let sendIcon = Icon.create (Icon.Communicate Communicate.Send)

let draftsIcon = Icon.create (Icon.Communicate Communicate.Drafts)

WeaveList.create (
  [
    ListItem.create (iconLabel inboxIcon "Inbox")
    ListItem.create (iconLabel sendIcon "Sent")
    ListItem.create (iconLabel draftsIcon "Drafts")
    ListChild.Content(Divider.create ())
    ListItem.create (text "Trash", secondaryContent = text "Removed e-mails")
    ListItem.create (
      text "Spam",
      secondaryContent = text "E-mails from common providers"
    )
  ]
)"""

    Helpers.codeSampleSection "Simple List" description content code

  let private nestedListExample () =
    let description =
      Helpers.bodyText
        "To create a nested list use the nestedChildren parameter of ListItem. The expansion state can be controlled via the expanded parameter. You can bind it externally to control sub-list expansion."

    let content =
      let iconLabel (icon: Doc) (label: string) =
        div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [ icon; text label ]

      let sentMailExpanded = Var.Create false

      WeaveList.create (
        [
          ListSubheader.create (text "Nested List Items")

          ListItem.create (
            iconLabel (Icon.create (Icon.Communicate Communicate.Inbox)) "Inbox",
            nestedChildren = [
              ListItem.create (iconLabel (Icon.create (Icon.UiActions UiActions.Star)) "Starred")
              ListItem.create (iconLabel (Icon.create (Icon.Action Action.Schedule)) "Snoozed")
            ],
            expanded = Var.Create true
          )

          ListItem.create (
            iconLabel (Icon.create (Icon.Communicate Communicate.Send)) "Sent mail",
            nestedChildren = [
              ListItem.create (text "Re: Meeting tomorrow")
              ListItem.create (text "Fwd: JavaScript memes xD")
            ],
            expanded = sentMailExpanded // see here
          )

          ListItem.create (iconLabel (Icon.create (Icon.Communicate Communicate.Drafts)) "Drafts")

          ListChild.Content(
            Switch.create (
              sentMailExpanded,
              div [ Typography.body1 ] [ text "\"Sent mail\" Expansion" ],
              attrs = [ Switch.Color.secondary; Margin.All.extraSmall ]
            )
          )
        ]
      )

    let code =
      """open WeaveList
open Weave.Icons
open Weave.Icons.MaterialSymbols

let iconLabel (icon: Doc) (label: string) =
  div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [
    icon
    text label
  ]

let sentMailExpanded = Var.Create false

WeaveList.create (
  [
    ListSubheader.create (text "Nested List Items")

    ListItem.create (
      iconLabel (Icon.create (Icon.Communicate Communicate.Inbox)) "Inbox",
      nestedChildren = [
        ListItem.create (
          iconLabel (Icon.create (Icon.UiActions UiActions.Star)) "Starred"
        )
        ListItem.create (
          iconLabel (Icon.create (Icon.Action Action.Schedule)) "Snoozed"
        )
      ],
      expanded = Var.Create true
    )

    ListItem.create (
      iconLabel (Icon.create (Icon.Communicate Communicate.Send)) "Sent mail",
      nestedChildren = [
        ListItem.create (text "Re: Meeting tomorrow")
        ListItem.create (text "Fwd: JavaScript memes xD")
      ],
      expanded = sentMailExpanded // see here
    )

    ListItem.create (
      iconLabel (Icon.create (Icon.Communicate Communicate.Drafts)) "Drafts"
    )

    ListChild.Content(
      Switch.create (
        sentMailExpanded,
        div [ Typography.body1 ] [ text "\"Sent mail\" Expansion" ],
        attrs = [ Switch.Color.secondary; Margin.All.extraSmall ]
      )
    )
  ]
)"""

    Helpers.codeSampleSection "Nested List" description content code

  let private singleSelectionExample () =
    let description =
      Helpers.bodyText
        "Pass selectedValue and selectionMode to WeaveList.create for single/toggle selection across all items and nested lists. ToggleSelection allows deselecting by clicking the selected item again."

    let content =
      let selectedDrink = Var.Create<string option> None

      let selectionMode = Var.Create WeaveList.SelectionMode.SingleSelection

      let readOnly = Var.Create false

      let drinks = [ "Milk"; "Sparkling Water" ]

      let teaGroup = [ "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]

      let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

      comparisonGrid 2 [
        selectionMode.View
        |> Doc.BindView(fun mode ->
          WeaveList.create (
            [
              yield! drinks |> List.map (fun d -> ListItem.create (text d, value = d))

              ListItem.create (
                text "Teas",
                nestedChildren = (teaGroup |> List.map (fun t -> ListItem.create (text t, value = t)))
              )

              ListItem.create (
                text "Coffees",
                nestedChildren = (coffeeGroup |> List.map (fun c -> ListItem.create (text c, value = c)))
              )
            ],
            selectedValue = selectedDrink, // see here
            selectionMode = mode,
            readOnly = readOnly.View
          ))

        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
          fieldset [ Attr.Style "border" "0"; Attr.Style "padding" "0"; Attr.Style "margin" "0" ] [
            legend [ Typography.subtitle2 ] [ text "Selection mode" ]
            Radio.create (
              selectionMode,
              WeaveList.SelectionMode.SingleSelection,
              displayText = View.Const "SingleSelection",
              inputAttrs = [ attr.name "list-selection-mode" ]
            )

            Radio.create (
              selectionMode,
              WeaveList.SelectionMode.ToggleSelection,
              displayText = View.Const "ToggleSelection",
              inputAttrs = [ attr.name "list-selection-mode" ]
            )
          ]

          Switch.create (
            readOnly,
            div [ Typography.body1 ] [ text "ReadOnly" ],
            attrs = [ Switch.Color.secondary ]
          )
        ]
      ]

    let code =
      """let selectedDrink = Var.Create<string option> None

let selectionMode = Var.Create WeaveList.SelectionMode.SingleSelection

let readOnly = Var.Create false

let drinks = [ "Milk"; "Sparkling Water" ]

let teaGroup = [ "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]

let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

Doc.Concat [
  selectionMode.View
  |> Doc.BindView(fun mode ->
    WeaveList.create (
      [
        yield!
          drinks |> List.map (fun d -> ListItem.create (text d, value = d))

        ListItem.create (
          text "Teas",
          nestedChildren =
            (teaGroup
             |> List.map (fun t -> ListItem.create (text t, value = t)))
        )

        ListItem.create (
          text "Coffees",
          nestedChildren =
            (coffeeGroup
             |> List.map (fun c -> ListItem.create (text c, value = c)))
        )
      ],
      selectedValue = selectedDrink, // see here
      selectionMode = mode,
      readOnly = readOnly.View
    ))

  fieldset [] [
    legend [] [ text "Selection mode" ]
    Radio.create (
      selectionMode,
      WeaveList.SelectionMode.SingleSelection,
      displayText = View.Const "SingleSelection",
      inputAttrs = [ attr.name "list-selection-mode" ]
    )

    Radio.create (
      selectionMode,
      WeaveList.SelectionMode.ToggleSelection,
      displayText = View.Const "ToggleSelection",
      inputAttrs = [ attr.name "list-selection-mode" ]
    )
  ]

  Switch.create (
    readOnly,
    div [ Typography.body1 ] [ text "ReadOnly" ],
    attrs = [ Switch.Color.secondary ]
  )
]"""

    Helpers.codeSampleSection "Single-Selection" description content code

  let private multiSelectionExample () =
    let description =
      Helpers.bodyText
        "Pass selectedValues to WeaveList.create for multi-selection. A checkbox indicator is displayed on each item automatically."

    let content =
      let selectedDrinks = Var.Create<Set<string>>(Set.ofList [ "Milk" ])

      let readOnly = Var.Create false

      let drinks = [ "Milk"; "Sparkling Water" ]

      let teaGroup = [ "Carbonated H\u00B2O"; "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]

      let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

      comparisonGrid 2 [
        WeaveList.create (
          [
            ListSubheader.create (text "Select your favourite drinks:")

            yield! drinks |> List.map (fun d -> ListItem.create (text d, value = d))

            ListItem.create (
              text "Teas",
              nestedChildren = (teaGroup |> List.map (fun t -> ListItem.create (text t, value = t)))
            )

            ListItem.create (
              text "Coffees",
              nestedChildren = (coffeeGroup |> List.map (fun c -> ListItem.create (text c, value = c)))
            )
          ],
          selectedValues = selectedDrinks, // see here
          readOnly = readOnly.View
        )

        div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
          Switch.create (
            readOnly,
            div [ Typography.body1 ] [ text "ReadOnly" ],
            attrs = [ Switch.Color.secondary ]
          )
        ]
      ]

    let code =
      """let selectedDrinks = Var.Create<Set<string>>(Set.ofList [ "Milk" ])

let readOnly = Var.Create false

let drinks = [ "Milk"; "Sparkling Water" ]

let teaGroup = [
  "Carbonated H\u00B2O"
  "Earl Grey"
  "Gunpowder Tea"
  "Bubble Tea"
]

let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

Doc.Concat [
  WeaveList.create (
    [
      ListSubheader.create (text "Select your favourite drinks:")

      yield! drinks |> List.map (fun d -> ListItem.create (text d, value = d))

      ListItem.create (
        text "Teas",
        nestedChildren =
          (teaGroup |> List.map (fun t -> ListItem.create (text t, value = t)))
      )

      ListItem.create (
        text "Coffees",
        nestedChildren =
          (coffeeGroup
           |> List.map (fun c -> ListItem.create (text c, value = c)))
      )
    ],
    selectedValues = selectedDrinks, // see here
    readOnly = readOnly.View
  )

  Switch.create (
    readOnly,
    div [ Typography.body1 ] [ text "ReadOnly" ],
    attrs = [ Switch.Color.secondary ]
  )
]"""

    Helpers.codeSampleSection "Multiselection" description content code

  let private interactiveExample () =
    let description =
      Helpers.bodyText
        "Per-item color can be customized via attrs using the CSS color modifier classes (e.g. weave-list-item--secondary). The default selection color is Primary. Disabled items cannot be clicked."

    let content =
      let selectedValue = Var.Create<string option> None

      WeaveList.create (
        [
          ListItem.create (text "Primary (default)", value = "primary")
          ListItem.create (
            text "Secondary",
            value = "secondary",
            attrs = [ WeaveList.Color.secondary ] // see here
          )
          ListItem.create (text "Tertiary", value = "tertiary", attrs = [ WeaveList.Color.tertiary ])
          ListItem.create (text "Success", value = "success", attrs = [ WeaveList.Color.success ])
          ListItem.create (text "Disabled item", value = "disabled", disabled = View.Const true)
        ],
        selectedValue = selectedValue,
        selectionMode = WeaveList.SelectionMode.ToggleSelection
      )

    let code =
      """let selectedValue = Var.Create<string option> None

WeaveList.create (
  [
    ListItem.create (text "Primary (default)", value = "primary")
    ListItem.create (
      text "Secondary",
      value = "secondary",
      attrs = [ WeaveList.Color.secondary ] // see here
    )
    ListItem.create (
      text "Tertiary",
      value = "tertiary",
      attrs = [ WeaveList.Color.tertiary ]
    )
    ListItem.create (
      text "Success",
      value = "success",
      attrs = [ WeaveList.Color.success ]
    )
    ListItem.create (
      text "Disabled item",
      value = "disabled",
      disabled = View.Const true
    )
  ],
  selectedValue = selectedValue,
  selectionMode = WeaveList.SelectionMode.ToggleSelection
)"""

    Helpers.codeSampleSection "Interactive" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for WeaveList, ListItem, and ListSubheader.") [
      Helpers.apiTable "WeaveList.create" [
        Helpers.apiParam
          "children"
          "ListChild list"
          ""
          "Child elements — use ListItem.create or ListChild.Content for arbitrary Docs"
        Helpers.apiParam
          "?selectedValue"
          "Var<string option>"
          ""
          "Two-way binding for single/toggle selection"
        Helpers.apiParam
          "?selectedValues"
          "Var<Set<string>>"
          ""
          "Two-way binding for multi-selection (shows checkboxes)"
        Helpers.apiParam
          "?selectionMode"
          "SelectionMode"
          "SingleSelection"
          "How items respond to clicks — SingleSelection, ToggleSelection, or MultiSelection"
        Helpers.apiParam
          "?readOnly"
          "View<bool>"
          "View.Const false"
          "Items display state but do not respond to clicks"
        Helpers.apiParam "?bordered" "View<bool>" "View.Const true" "Whether to draw a border around the list"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
      ]

      Helpers.returnTypeNote
        "ListItem.create returns a ListChild union case, not a Doc. Build items with ListItem.create and pass them as a list to WeaveList.create."

      Helpers.apiTable "ListItem.create" [
        Helpers.apiParam "content" "Doc" "" "Primary content (text, icons, avatars, etc.)"
        Helpers.apiParam
          "?value"
          "string"
          ""
          "Selectable value — required when using selection on the parent list"
        Helpers.apiParam "?secondaryContent" "Doc" "" "Secondary line rendered below the primary content"
        Helpers.apiParam
          "?nestedChildren"
          "ListChild list"
          "[]"
          "Child items rendered as an expandable nested list"
        Helpers.apiParam
          "?expanded"
          "Var<bool>"
          "Var.Create false"
          "Reactive variable controlling nested list expansion"
        Helpers.apiParam
          "?disabled"
          "View<bool>"
          "View.Const false"
          "Whether this item is non-interactive and visually dimmed"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the item row"
      ]

      Helpers.apiTable "ListSubheader.create" [
        Helpers.apiParam "content" "Doc" "" "The subheader content"
        Helpers.apiParam
          "?inset"
          "bool"
          "false"
          "When true, aligns with items that have icons (adds left padding)"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the subheader element"
      ]

      Helpers.styleModuleTable "WeaveList.SelectionMode (DU)" [
        ("SingleSelection", "Clicking an item selects it; clicking another deselects the previous one")
        ("ToggleSelection", "Like SingleSelection, but clicking the selected item deselects it")
        ("MultiSelection", "Multiple items can be selected simultaneously; shows checkboxes")
      ]

      Helpers.styleModuleTable "WeaveList.Color" [
        ("primary", "Primary brand color for selected item highlight")
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
        Helpers.pageTitle "List"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "A scrollable list for displaying text, avatars, icons, and interactive items. Use lists to help users find a specific item and act on it."
        ]

        Helpers.divider ()
        simpleListExample ()
        Helpers.divider ()
        ListAppearance.render ()
        Helpers.divider ()
        nestedListExample ()
        Helpers.divider ()
        singleSelectionExample ()
        Helpers.divider ()
        multiSelectionExample ()
        Helpers.divider ()
        interactiveExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
