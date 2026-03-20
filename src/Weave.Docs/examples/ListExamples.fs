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

  let private iconLabel (icon: Doc) (label: string) =
    div [ Flex.Flex.allSizes; AlignItems.center; Attr.Style "gap" "8px" ] [ icon; text label ]

  let private simpleListExample () =
    let inboxIcon = Icon.create (Icon.Communicate Communicate.Inbox)
    let sendIcon = Icon.create (Icon.Communicate Communicate.Send)
    let draftsIcon = Icon.create (Icon.Communicate Communicate.Drafts)

    let content =
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

    Helpers.codeSampleSection
      "Simple List"
      (Helpers.bodyText
        "WeaveList displays a collection of items. Icons, avatars, or other content can be included directly in the item's content. Use ListChild.Content to include arbitrary elements like dividers.")
      content
      """WeaveList.create(
  [
    ListItem.create(iconLabel inboxIcon "Inbox")
    ListItem.create(iconLabel sendIcon "Sent")
    ListItem.create(iconLabel draftsIcon "Drafts")
    ListChild.Content(Divider.create())
    ListItem.create(
      text "Trash",
      secondaryContent = text "Removed e-mails"
    )
    ListItem.create(
      text "Spam",
      secondaryContent = text "E-mails from common providers"
    )
  ]
)"""

  let private nestedListExample () =
    let sentMailExpanded = Var.Create false

    let content =
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
            expanded = sentMailExpanded
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

    Helpers.codeSampleSection
      "Nested List"
      (Helpers.bodyText
        "To create a nested list use the nestedChildren parameter of ListItem. The expansion state can be controlled via the expanded parameter. You can bind it externally to control sub-list expansion.")
      content
      """let sentMailExpanded = Var.Create false

WeaveList.create(
  [
    ListSubheader.create(text "Nested List Items")

    ListItem.create(
      iconLabel inboxIcon "Inbox",
      nestedChildren = [
        ListItem.create(iconLabel starIcon "Starred")
        ListItem.create(iconLabel scheduleIcon "Snoozed")
      ],
      expanded = Var.Create true
    )

    ListItem.create(
      iconLabel sendIcon "Sent mail",
      nestedChildren = [
        ListItem.create(text "Re: Meeting tomorrow")
        ListItem.create(text "Fwd: JavaScript memes xD")
      ],
      expanded = sentMailExpanded
    )

    ListItem.create(iconLabel draftsIcon "Drafts")

    ListChild.Content(
      Switch.create(
        sentMailExpanded,
        div [ Typography.body1 ] [ text "\"Sent mail\" Expansion" ],
        attrs = [
          Switch.Color.secondary
          Margin.All.extraSmall
        ]
      )
    )
  ]
)"""

  let private singleSelectionExample () =
    let selectionMode = Var.Create WeaveList.SelectionMode.SingleSelection
    let readOnly = Var.Create false
    let selectedDrink = Var.Create<string option> None

    let drinks = [ "Milk"; "Sparkling Water" ]

    let teaGroup = [ "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]

    let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

    let content =
      div [] [
        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          AlignItems.center
          Attr.Style "gap" "8px"
          Margin.Bottom.extraSmall
        ] [
          span [ Typography.body1 ] [ text "Your drink:" ]

          selectedDrink.View
          |> Doc.BindView(fun sel ->
            match sel with
            | Some v ->
              span [
                Attr.Style "background" "var(--palette-primary)"
                Attr.Style "color" "var(--palette-primary-text)"
                Attr.Style "padding" "4px 12px"
                Attr.Style "border-radius" "16px"
                Attr.Style "font-size" "0.875rem"
              ] [ text v ]
            | None -> Doc.Empty)
        ]

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
            selectedValue = selectedDrink,
            selectionMode = mode,
            readOnly = readOnly.View
          ))

        div [
          Flex.Flex.allSizes
          AlignItems.center
          FlexDirection.Column.xs
          FlexDirection.Row.sm
          JustifyContent.spaceAround
          Attr.Style "gap" "16px"
          Margin.Top.extraSmall
        ] [
          Radio.create (
            selectionMode,
            WeaveList.SelectionMode.SingleSelection,
            displayText = View.Const "SingleSelection"
          )

          Radio.create (
            selectionMode,
            WeaveList.SelectionMode.ToggleSelection,
            displayText = View.Const "ToggleSelection"
          )
        ]

        div [ Margin.Top.extraSmall ] [
          Switch.create (
            readOnly,
            div [ Typography.body1 ] [ text "ReadOnly" ],
            attrs = [ Switch.Color.secondary ]
          )
        ]

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          Attr.Style "gap" "8px"
          Margin.Top.extraSmall
        ] [
          let allValues = drinks @ teaGroup @ coffeeGroup

          yield!
            allValues
            |> List.map (fun d ->
              selectedDrink.View
              |> Doc.BindView(fun sel ->
                if sel = Some d then
                  span [
                    Attr.Style "background" "var(--palette-primary)"
                    Attr.Style "color" "var(--palette-primary-text)"
                    Attr.Style "padding" "4px 12px"
                    Attr.Style "border-radius" "16px"
                    Attr.Style "font-size" "0.875rem"
                  ] [ text d ]
                else
                  Doc.Empty))
        ]
      ]

    Helpers.codeSampleSection
      "Single-Selection"
      (Helpers.bodyText
        "Pass selectedValue and selectionMode to WeaveList.create for single/toggle selection across all items and nested lists. ToggleSelection allows deselecting by clicking the selected item again.")
      content
      """let selectedDrink = Var.Create<string option> None
let selectionMode = Var.Create WeaveList.SelectionMode.SingleSelection
let readOnly = Var.Create false

let drinks = [ "Milk"; "Sparkling Water" ]
let teaGroup = [ "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]
let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

selectionMode.View
|> Doc.BindView(fun mode ->
  WeaveList.create(
    [
      yield! drinks |> List.map (fun d -> ListItem.create(text d, value = d))

      ListItem.create(
        text "Teas",
        nestedChildren =
          teaGroup |> List.map (fun t -> ListItem.create(text t, value = t))
      )

      ListItem.create(
        text "Coffees",
        nestedChildren =
          coffeeGroup |> List.map (fun c -> ListItem.create(text c, value = c))
      )
    ],
    selectedValue = selectedDrink,
    selectionMode = mode,
    readOnly = readOnly.View
  ))

Radio.create(
  selectionMode,
  WeaveList.SelectionMode.SingleSelection,
  displayText = View.Const "SingleSelection"
)

Radio.create(
  selectionMode,
  WeaveList.SelectionMode.ToggleSelection,
  displayText = View.Const "ToggleSelection"
)

Switch.create(readOnly, displayText = View.Const "ReadOnly", attrs = [ Switch.Color.secondary ])"""

  let private multiSelectionExample () =
    let readOnly = Var.Create false
    let selectedDrinks = Var.Create<Set<string>>(Set.ofList [ "Milk" ])

    let drinks = [ "Milk"; "Sparkling Water" ]

    let teaGroup = [ "Carbonated H\u00B2O"; "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]

    let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

    let content =
      div [] [
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
          selectedValues = selectedDrinks,
          readOnly = readOnly.View
        )

        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          Attr.Style "gap" "8px"
          Margin.Top.extraSmall
        ] [
          let allValues = drinks @ teaGroup @ coffeeGroup

          yield!
            allValues
            |> List.map (fun d ->
              selectedDrinks.View
              |> Doc.BindView(fun sel ->
                if Set.contains d sel then
                  span [
                    Attr.Style "background" "var(--palette-primary)"
                    Attr.Style "color" "var(--palette-primary-text)"
                    Attr.Style "padding" "4px 12px"
                    Attr.Style "border-radius" "16px"
                    Attr.Style "font-size" "0.875rem"
                  ] [ text d ]
                else
                  Doc.Empty))
        ]

        div [ Margin.Top.extraSmall ] [
          Switch.create (
            readOnly,
            div [ Typography.body1 ] [ text "ReadOnly" ],
            attrs = [ Switch.Color.secondary ]
          )
        ]
      ]

    Helpers.codeSampleSection
      "Multiselection"
      (Helpers.bodyText
        "Pass selectedValues to WeaveList.create for multi-selection. A checkbox indicator is displayed on each item automatically.")
      content
      """let selectedDrinks = Var.Create<Set<string>> (Set.ofList [ "Milk" ])
let readOnly = Var.Create false

let drinks = [ "Milk"; "Sparkling Water" ]
let teaGroup = [ "Carbonated H\u00B2O"; "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]
let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

WeaveList.create(
  [
    ListSubheader.create(text "Select your favourite drinks:")

    yield! drinks |> List.map (fun d -> ListItem.create(text d, value = d))

    ListItem.create(
      text "Teas",
      nestedChildren =
        teaGroup |> List.map (fun t -> ListItem.create(text t, value = t))
    )

    ListItem.create(
      text "Coffees",
      nestedChildren =
        coffeeGroup |> List.map (fun c -> ListItem.create(text c, value = c))
    )
  ],
  selectedValues = selectedDrinks,
  readOnly = readOnly.View
)

Switch.create(readOnly, displayText = View.Const "ReadOnly", attrs = [ Switch.Color.secondary ])"""

  let private interactiveExample () =
    let selectedValue = Var.Create<string option> None

    let content =
      WeaveList.create (
        [
          ListItem.create (text "Primary (default)", value = "primary")
          ListItem.create (text "Secondary", value = "secondary", attrs = [ WeaveList.Color.secondary ])
          ListItem.create (text "Tertiary", value = "tertiary", attrs = [ WeaveList.Color.tertiary ])
          ListItem.create (text "Success", value = "success", attrs = [ WeaveList.Color.success ])
          ListItem.create (text "Disabled item", value = "disabled", disabled = View.Const true)
        ],
        selectedValue = selectedValue,
        selectionMode = WeaveList.SelectionMode.ToggleSelection
      )

    Helpers.codeSampleSection
      "Interactive"
      (Helpers.bodyText
        "Per-item color can be customized via attrs using the CSS color modifier classes (e.g. weave-list-item--secondary). The default selection color is Primary. Disabled items cannot be clicked.")
      content
      """let selectedValue = Var.Create<string option> None

WeaveList.create(
  [
    ListItem.create(
      text "Primary (default)",
      value = "primary"
    )
    ListItem.create(
      text "Secondary",
      value = "secondary",
      attrs = [ WeaveList.Color.secondary ]
    )
    ListItem.create(
      text "Tertiary",
      value = "tertiary",
      attrs = [ WeaveList.Color.tertiary ]
    )
    ListItem.create(
      text "Success",
      value = "success",
      attrs = [ WeaveList.Color.success ]
    )
    ListItem.create(
      text "Disabled item",
      value = "disabled",
      disabled = View.Const true
    )
  ],
  selectedValue = selectedValue,
  selectionMode = WeaveList.SelectionMode.ToggleSelection
)"""

  let private densityExample () =
    let content =
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          WeaveList.create (
            [
              ListItem.create (iconLabel (Icon.create (Icon.Communicate Communicate.Inbox)) "Item 1")
              ListItem.create (iconLabel (Icon.create (Icon.Communicate Communicate.Send)) "Item 2")
              ListItem.create (iconLabel (Icon.create (Icon.Communicate Communicate.Drafts)) "Item 3")
            ]
          )
        ]

      Grid.create (
        [
          GridItem.create (col "Compact" Density.compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.create (
            col "Standard" Density.standard,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.start ]
      )

    Helpers.codeSampleSection
      "Density"
      (Helpers.bodyText
        "Density controls list item padding and height. Pass the density class in attrs to set it per-instance.")
      content
      """open Weave


WeaveList.create(
  [
    ListItem.create(iconLabel inboxIcon "Item 1")
    ListItem.create(iconLabel sendIcon "Item 2")
    ListItem.create(iconLabel draftsIcon "Item 3")
  ],
  attrs = [ Density.compact ]
)
"""

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
        nestedListExample ()
        Helpers.divider ()
        singleSelectionExample ()
        Helpers.divider ()
        multiSelectionExample ()
        Helpers.divider ()
        interactiveExample ()
        Helpers.divider ()
        densityExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
