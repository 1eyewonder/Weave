namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open Weave.Icons
open Weave.Icons.MaterialSymbols
open WeaveList

[<JavaScript>]
module ListExamples =

  /// Renders an icon and a text label side-by-side with vertical centering.
  let private iconLabel (icon: Doc) (label: string) =
    div [
      cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ]
      Attr.Style "gap" "8px"
    ] [ icon; text label ]

  let private simpleListExample () =
    let inboxIcon = Icon.Create(Icon.Communicate Communicate.Inbox)
    let sendIcon = Icon.Create(Icon.Communicate Communicate.Send)
    let draftsIcon = Icon.Create(Icon.Communicate Communicate.Drafts)

    let content =
      WeaveList.Create(
        [
          ListItem.Create(iconLabel inboxIcon "Inbox")
          ListItem.Create(iconLabel sendIcon "Sent")
          ListItem.Create(iconLabel draftsIcon "Drafts")
          ListChild.Content(Divider.Create())
          ListItem.Create(text "Trash", secondaryContent = text "Removed e-mails")
          ListItem.Create(text "Spam", secondaryContent = text "E-mails from common providers")
        ]
      )

    Helpers.codeSampleSection
      "Simple List"
      (Helpers.bodyText
        "WeaveList displays a collection of items. Icons, avatars, or other content can be included directly in the item's content. Use ListChild.Content to include arbitrary elements like dividers.")
      content
      """WeaveList.Create(
  [
    ListItem.Create(iconLabel inboxIcon "Inbox")
    ListItem.Create(iconLabel sendIcon "Sent")
    ListItem.Create(iconLabel draftsIcon "Drafts")
    ListChild.Content(Divider.Create())
    ListItem.Create(
      text "Trash",
      secondaryContent = text "Removed e-mails"
    )
    ListItem.Create(
      text "Spam",
      secondaryContent = text "E-mails from common providers"
    )
  ]
)"""

  let private nestedListExample () =
    let sentMailExpanded = Var.Create false

    let content =
      WeaveList.Create(
        [
          ListSubheader.Create(text "Nested List Items")

          ListItem.Create(
            iconLabel (Icon.Create(Icon.Communicate Communicate.Inbox)) "Inbox",
            nestedChildren = [
              ListItem.Create(iconLabel (Icon.Create(Icon.UiActions UiActions.Star)) "Starred")
              ListItem.Create(iconLabel (Icon.Create(Icon.Action Action.Schedule)) "Snoozed")
            ],
            expanded = Var.Create true
          )

          ListItem.Create(
            iconLabel (Icon.Create(Icon.Communicate Communicate.Send)) "Sent mail",
            nestedChildren = [
              ListItem.Create(text "Re: Meeting tomorrow")
              ListItem.Create(text "Fwd: JavaScript memes xD")
            ],
            expanded = sentMailExpanded
          )

          ListItem.Create(iconLabel (Icon.Create(Icon.Communicate Communicate.Drafts)) "Drafts")

          ListChild.Content(
            Switch.Create(
              sentMailExpanded,
              displayText = View.Const "\"Sent mail\" Expansion",
              attrs = [
                cls [
                  Switch.Color.toClass BrandColor.Secondary
                  yield! Margin.toClasses Margin.All.extraSmall
                ]
              ]
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

WeaveList.Create(
  [
    ListSubheader.Create(text "Nested List Items")

    ListItem.Create(
      iconLabel inboxIcon "Inbox",
      nestedChildren = [
        ListItem.Create(iconLabel starIcon "Starred")
        ListItem.Create(iconLabel scheduleIcon "Snoozed")
      ],
      expanded = Var.Create true
    )

    ListItem.Create(
      iconLabel sendIcon "Sent mail",
      nestedChildren = [
        ListItem.Create(text "Re: Meeting tomorrow")
        ListItem.Create(text "Fwd: JavaScript memes xD")
      ],
      expanded = sentMailExpanded
    )

    ListItem.Create(iconLabel draftsIcon "Drafts")

    ListChild.Content(
      Switch.Create(
        sentMailExpanded,
        displayText = View.Const "\"Sent mail\" Expansion",
        attrs = [
          cls [
            Switch.Color.toClass BrandColor.Secondary
            yield! Margin.toClasses Margin.All.extraSmall
          ]
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
        // Display selected value as a chip
        div [
          cls [
            Flex.Flex.allSizes
            FlexWrap.Wrap.allSizes
            AlignItems.toClass AlignItems.Center
          ]
          Attr.Style "gap" "8px"
          Margin.toClasses Margin.Bottom.extraSmall |> cls
        ] [
          Body1.Span("Your drink:")

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

        // List re-renders when selection mode changes
        selectionMode.View
        |> Doc.BindView(fun mode ->
          WeaveList.Create(
            [
              yield! drinks |> List.map (fun d -> ListItem.Create(text d, value = d))

              // Tea group
              ListItem.Create(
                text "Teas",
                nestedChildren = (teaGroup |> List.map (fun t -> ListItem.Create(text t, value = t)))
              )

              // Coffee group
              ListItem.Create(
                text "Coffees",
                nestedChildren = (coffeeGroup |> List.map (fun c -> ListItem.Create(text c, value = c)))
              )
            ],
            selectedValue = selectedDrink,
            selectionMode = mode,
            readOnly = readOnly.View
          ))

        // Controls
        div [
          cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ]
          Attr.Style "gap" "16px"
          Margin.toClasses Margin.Top.extraSmall |> cls
        ] [
          Radio.Create(
            selectionMode,
            WeaveList.SelectionMode.SingleSelection,
            displayText = View.Const "SingleSelection"
          )

          Radio.Create(
            selectionMode,
            WeaveList.SelectionMode.ToggleSelection,
            displayText = View.Const "ToggleSelection"
          )
        ]

        div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
          Switch.Create(
            readOnly,
            displayText = View.Const "ReadOnly",
            attrs = [ Switch.Color.toClass BrandColor.Secondary |> cl ]
          )
        ]

        // Show all selected values as chips
        div [
          cls [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes ]
          Attr.Style "gap" "8px"
          Margin.toClasses Margin.Top.extraSmall |> cls
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
        "Pass selectedValue and selectionMode to WeaveList.Create for single/toggle selection across all items and nested lists. ToggleSelection allows deselecting by clicking the selected item again.")
      content
      """let selectedDrink = Var.Create<string option> None
let selectionMode = Var.Create WeaveList.SelectionMode.SingleSelection
let readOnly = Var.Create false

let drinks = [ "Milk"; "Sparkling Water" ]
let teaGroup = [ "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]
let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

selectionMode.View
|> Doc.BindView(fun mode ->
  WeaveList.Create(
    [
      yield! drinks |> List.map (fun d -> ListItem.Create(text d, value = d))

      ListItem.Create(
        text "Teas",
        nestedChildren =
          teaGroup |> List.map (fun t -> ListItem.Create(text t, value = t))
      )

      ListItem.Create(
        text "Coffees",
        nestedChildren =
          coffeeGroup |> List.map (fun c -> ListItem.Create(text c, value = c))
      )
    ],
    selectedValue = selectedDrink,
    selectionMode = mode,
    readOnly = readOnly.View
  ))

Radio.Create(
  selectionMode,
  WeaveList.SelectionMode.SingleSelection,
  displayText = View.Const "SingleSelection"
)

Radio.Create(
  selectionMode,
  WeaveList.SelectionMode.ToggleSelection,
  displayText = View.Const "ToggleSelection"
)

Switch.Create(readOnly, displayText = View.Const "ReadOnly", attrs = [ Switch.Color.toClass BrandColor.Secondary |> cl ])"""

  let private multiSelectionExample () =
    let readOnly = Var.Create false
    let selectedDrinks = Var.Create<Set<string>>(Set.ofList [ "Milk" ])

    let drinks = [ "Milk"; "Sparkling Water" ]

    let teaGroup = [ "Carbonated H\u00B2O"; "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]

    let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

    let content =
      div [] [
        WeaveList.Create(
          [
            ListSubheader.Create(text "Select your favourite drinks:")

            yield! drinks |> List.map (fun d -> ListItem.Create(text d, value = d))

            ListItem.Create(
              text "Teas",
              nestedChildren = (teaGroup |> List.map (fun t -> ListItem.Create(text t, value = t)))
            )

            ListItem.Create(
              text "Coffees",
              nestedChildren = (coffeeGroup |> List.map (fun c -> ListItem.Create(text c, value = c)))
            )
          ],
          selectedValues = selectedDrinks,
          readOnly = readOnly.View
        )

        // Show selected values as chips
        div [
          cls [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes ]
          Attr.Style "gap" "8px"
          Margin.toClasses Margin.Top.extraSmall |> cls
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

        div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
          Switch.Create(
            readOnly,
            displayText = View.Const "ReadOnly",
            attrs = [ Switch.Color.toClass BrandColor.Secondary |> cl ]
          )
        ]
      ]

    Helpers.codeSampleSection
      "Multiselection"
      (Helpers.bodyText
        "Pass selectedValues to WeaveList.Create for multi-selection. A checkbox indicator is displayed on each item automatically.")
      content
      """let selectedDrinks = Var.Create<Set<string>> (Set.ofList [ "Milk" ])
let readOnly = Var.Create false

let drinks = [ "Milk"; "Sparkling Water" ]
let teaGroup = [ "Carbonated H\u00B2O"; "Earl Grey"; "Gunpowder Tea"; "Bubble Tea" ]
let coffeeGroup = [ "Irish Coffee"; "Double Espresso"; "Cafe Latte" ]

WeaveList.Create(
  [
    ListSubheader.Create(text "Select your favourite drinks:")

    yield! drinks |> List.map (fun d -> ListItem.Create(text d, value = d))

    ListItem.Create(
      text "Teas",
      nestedChildren =
        teaGroup |> List.map (fun t -> ListItem.Create(text t, value = t))
    )

    ListItem.Create(
      text "Coffees",
      nestedChildren =
        coffeeGroup |> List.map (fun c -> ListItem.Create(text c, value = c))
    )
  ],
  selectedValues = selectedDrinks,
  readOnly = readOnly.View
)

Switch.Create(readOnly, displayText = View.Const "ReadOnly", attrs = [ Switch.Color.toClass BrandColor.Secondary |> cl ])"""

  let private interactiveExample () =
    let selectedValue = Var.Create<string option> None

    let content =
      WeaveList.Create(
        [
          ListItem.Create(text "Primary (default)", value = "primary")
          ListItem.Create(
            text "Secondary",
            value = "secondary",
            attrs = [ WeaveList.Color.toClass BrandColor.Secondary |> cl ]
          )
          ListItem.Create(
            text "Tertiary",
            value = "tertiary",
            attrs = [ WeaveList.Color.toClass BrandColor.Tertiary |> cl ]
          )
          ListItem.Create(
            text "Success",
            value = "success",
            attrs = [ WeaveList.Color.toClass BrandColor.Success |> cl ]
          )
          ListItem.Create(text "Disabled item", value = "disabled", disabled = View.Const true)
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

WeaveList.Create(
  [
    ListItem.Create(
      text "Primary (default)",
      value = "primary"
    )
    ListItem.Create(
      text "Secondary",
      value = "secondary",
      attrs = [ cl (WeaveList.Color.toClass BrandColor.Secondary) ]
    )
    ListItem.Create(
      text "Tertiary",
      value = "tertiary",
      attrs = [ cl (WeaveList.Color.toClass BrandColor.Tertiary) ]
    )
    ListItem.Create(
      text "Success",
      value = "success",
      attrs = [ cl (WeaveList.Color.toClass BrandColor.Success) ]
    )
    ListItem.Create(
      text "Disabled item",
      value = "disabled",
      disabled = View.Const true
    )
  ],
  selectedValue = selectedValue,
  selectionMode = WeaveList.SelectionMode.ToggleSelection
)"""

  let private denseExample () =
    let dense = Var.Create true

    let content =
      div [] [
        WeaveList.Create(
          [
            ListItem.Create(iconLabel (Icon.Create(Icon.Communicate Communicate.Inbox)) "Item 1")
            ListItem.Create(iconLabel (Icon.Create(Icon.Communicate Communicate.Send)) "Item 2")
            ListItem.Create(iconLabel (Icon.Create(Icon.Communicate Communicate.Drafts)) "Item 3")
          ],
          dense = dense.View
        )

        div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
          Switch.Create(
            dense,
            displayText = View.Const "Dense",
            attrs = [ Switch.Color.toClass BrandColor.Secondary |> cl ]
          )
        ]
      ]

    Helpers.codeSampleSection
      "Dense"
      (Helpers.bodyText
        "Set dense to true to reduce vertical padding on each item, producing a more compact layout.")
      content
      """let dense = Var.Create true

WeaveList.Create(
  [
    ListItem.Create(iconLabel inboxIcon "Item 1")
    ListItem.Create(iconLabel sendIcon "Item 2")
    ListItem.Create(iconLabel draftsIcon "Item 3")
  ],
  dense = dense.View
)

Switch.Create(dense, displayText = View.Const "Dense", attrs = [ Switch.Color.toClass BrandColor.Secondary |> cl ])"""

  let render () =
    Container.Create(
      div [] [
        H1.Div("List Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Div(
          "A scrollable list for displaying text, avatars, icons, and interactive items. Use lists to help users find a specific item and act on it.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

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
        denseExample ()
      ]
    )
