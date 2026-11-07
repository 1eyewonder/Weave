namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ShowcaseTaskTracker =

  [<JavaScript; Struct>]
  type Priority =
    | Low
    | Medium
    | High

  [<JavaScript>]
  type TodoItem = {
    Id: int
    Text: string
    Priority: Priority
    IsCompleted: Var<bool>
  }

  let private priorityToColor (p: Priority) =
    match p with
    | Low -> Chip.Color.success
    | Medium -> Chip.Color.warning
    | High -> Chip.Color.error

  let private priorityToString (p: Priority) =
    match p with
    | Low -> "Low"
    | Medium -> "Medium"
    | High -> "High"

  let private stringToPriority (s: string) =
    match s with
    | "Low" -> Some Low
    | "Medium" -> Some Medium
    | "High" -> Some High
    | _ -> None

  let private fullAppSection () =
    let items: Var<TodoItem list> = Var.Create []
    let nextId = Var.Create 0
    let newText = Var.Create ""
    let newPriority = Var.Create<string option>(Some "Medium")
    let showCompleted = Var.Create true

    let addTask () =
      let txt = newText.Value.Trim()

      if txt.Length > 0 then
        let priority =
          newPriority.Value |> Option.bind stringToPriority |> Option.defaultValue Medium

        let item = {
          Id = nextId.Value
          Text = txt
          Priority = priority
          IsCompleted = Var.Create false
        }

        Var.Update items (fun list -> list @ [ item ])
        Var.Set nextId (nextId.Value + 1)
        Var.Set newText ""

    let removeTask (id: int) =
      Var.Update items (List.filter (fun t -> t.Id <> id))

    let priorityItems =
      [ "Low"; "Medium"; "High" ]
      |> List.map (fun label -> SelectItem.create (text label, label, label))
      |> View.Const

    // Subscribe to each nested completion Var as well as changes to the list.
    let itemStates =
      items.View
      |> View.Bind(fun list ->
        (list, View.Const [])
        ||> List.foldBack (fun item acc ->
          View.Map2 (fun completed rest -> (item, completed) :: rest) item.IsCompleted.View acc))

    let filteredItems =
      showCompleted.View
      |> View.Bind(fun show ->
        if show then
          items.View
        else
          itemStates
          |> View.Map(fun states ->
            states
            |> List.choose (fun (item, completed) -> if completed then None else Some item)))

    let totalCount = items.View |> View.MapCached List.length
    let completedCount = itemStates |> View.Map(List.filter snd >> List.length)

    let description =
      Helpers.bodyText
        "The complete Task Tracker app. Add tasks with a priority, check them off, filter, and track your progress — all fully reactive."

    let content =
      div [] [
        // Add task form
        Grid.create (
          [
            GridItem.create (
              TextField.singleLine (
                newText,
                labelText = View.Const "New task",
                placeholder = View.Const "What needs to be done?",
                attrs = [ TextField.Variant.outlined; TextField.Color.primary; TextField.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.five ]
            )

            GridItem.create (
              Select.create (
                priorityItems,
                newPriority,
                labelText = View.Const "Priority",
                attrs = [ Select.Variant.outlined; Select.Color.primary; Select.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )

            GridItem.create (
              Button.create (
                div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                  Icon.create (Icon.UiActions UiActions.Add)
                  text "Add"
                ],
                onClick = addTask,
                attrs = [
                  Button.Color.primary
                  Button.Variant.filled
                  Button.Width.full
                  Attr.Style "margin-top" "var(--weave-field-label-gutter)"
                ]
              ),
              attrs = [
                GridItem.Span.twelve
                GridItem.Span.Small.six
                GridItem.Span.Medium.three
                Flex.Flex.allSizes
              ]
            )
          ],
          attrs = [ AlignItems.stretch ]
        )

        Divider.create (attrs = [ Margin.Vertical.small ])

        // Filter and stats bar
        div [
          Flex.Flex.allSizes
          AlignItems.center
          JustifyContent.spaceBetween
          Margin.Bottom.extraSmall
          FlexWrap.Wrap.allSizes
          Gap.All.g2
        ] [
          Switch.create (
            showCompleted,
            div [ Typography.body2 ] [ text "Show completed" ],
            attrs = [ Switch.Color.primary ]
          )

          div [ Flex.Flex.allSizes; Gap.All.g4; AlignItems.center ] [
            div [ Typography.body2; Typography.Color.textSecondary ] [
              textView (
                (completedCount, totalCount)
                ||> View.Map2(fun c t -> sprintf "%d / %d completed" c t)
              )
            ]
          ]
        ]

        // Task list
        (filteredItems, totalCount)
        ||> View.Map2(fun list total -> list, total)
        |> Doc.BindView(fun (list, total) ->
          if List.isEmpty list then
            Alert.create (
              div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
                Icon.create (Icon.Text Text.Checklist)
                text (
                  if total = 0 then
                    "No tasks yet — add one above!"
                  else
                    "No active tasks — show completed tasks to see them."
                )
              ],
              attrs = [ Alert.Color.info; Alert.Variant.outlined; Margin.Top.extraSmall ]
            )
          else
            div [ Margin.Top.extraSmall ] [
              for item in list do
                div [
                  Flex.Flex.allSizes
                  AlignItems.center
                  Gap.All.g2
                  Padding.Vertical.extraSmall
                  Padding.Horizontal.extraSmall
                  SurfaceColor.BackgroundColor.surface
                  BorderRadius.All.small
                  Margin.Bottom.extraSmall
                  Elevation.e1
                ] [
                  div [
                    FlexItem.Flex.allSizes
                    Typography.body1
                    Attr.DynamicStyle
                      "text-decoration"
                      (item.IsCompleted.View |> View.Map(fun c -> if c then "line-through" else "none"))
                    Attr.DynamicStyle
                      "opacity"
                      (item.IsCompleted.View |> View.Map(fun c -> if c then "0.5" else "1"))
                  ] [
                    Checkbox.create (
                      item.IsCompleted,
                      View.Const item.Text,
                      attrs = [ Checkbox.Color.primary; Density.compact ]
                    )
                  ]

                  Chip.create (
                    text (priorityToString item.Priority),
                    attrs = [ priorityToColor item.Priority; Chip.Variant.filled ]
                  )

                  IconButton.create (
                    Icon.create (Icon.UiActions UiActions.Delete, attrs = [ Attr.Style "font-size" "18px" ]),
                    onClick = (fun () -> removeTask item.Id),
                    attrs = [ Density.compact; Attr.Create "aria-label" ("Delete " + item.Text) ]
                  )
                ]
            ])
      ]

    Helpers.section "The Full App" description content

  let private domainModelSection () =
    let description =
      Helpers.bodyText
        "The domain is modeled with F# discriminated unions — Priority for importance levels and a TodoItem record for each task. The IsCompleted field is a Var<bool> for two-way reactive binding to checkboxes."

    let content = Doc.Empty

    let code =
      """[<Struct>]
type Priority =
    | Low
    | Medium
    | High

type TodoItem = {
    Id: int
    Text: string
    Priority: Priority
    IsCompleted: Var<bool>
}

let priorityToColor (p: Priority) =
    match p with
    | Low -> Chip.Color.success
    | Medium -> Chip.Color.warning
    | High -> Chip.Color.error

let priorityToString (p: Priority) =
    match p with
    | Low -> "Low"
    | Medium -> "Medium"
    | High -> "High"

let stringToPriority (s: string) =
    match s with
    | "Low" -> Some Low
    | "Medium" -> Some Medium
    | "High" -> Some High
    | _ -> None"""

    Helpers.codeSampleSection "Domain Model" description content code

  let private addingTasksSection () =
    let description =
      Helpers.bodyText
        "Adding a task composes Field, Select, and Button. The form state lives in Vars — when 'Add' is clicked, we create a new TodoItem and append it to the reactive list. Notice how the Select maps display strings back to the Priority DU."

    let content = Doc.Empty

    let code =
      """open Weave
open WebSharper.UI

let items: Var<TodoItem list> = Var.Create []
let nextId = Var.Create 0
let newText = Var.Create ""
let newPriority = Var.Create<string option> (Some "Medium")

let addTask () =
    let txt = newText.Value.Trim()
    if txt.Length > 0 then
        let priority =
            newPriority.Value
            |> Option.bind stringToPriority
            |> Option.defaultValue Medium
        let item = {
            Id = nextId.Value
            Text = txt
            Priority = priority
            IsCompleted = Var.Create false
        }
        Var.Update items (fun list -> list @ [ item ])
        Var.Set nextId (nextId.Value + 1)
        Var.Set newText ""

let priorityItems =
    [ "Low"; "Medium"; "High" ]
    |> List.map (fun label -> SelectItem.create(text label, label, label))
    |> View.Const

// The form
Grid.create ([
    GridItem.create (
        TextField.singleLine(
            newText,
            labelText = View.Const "New task",
            placeholder = View.Const "What needs to be done?",
            attrs = [ TextField.Variant.outlined; TextField.Color.primary; TextField.Width.full ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.five ]
    )
    GridItem.create (
        Select.create(
            priorityItems,
            newPriority,
            labelText = View.Const "Priority",
            attrs = [ Select.Variant.outlined; Select.Color.primary; Select.Width.full ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
    )
    GridItem.create (
        Button.create(
            text "Add",
            onClick = addTask,
            attrs = [
                Button.Color.primary
                Button.Variant.filled
                // Starts the button where the fields' input boxes start, below their labels
                Attr.Style "margin-top" "var(--weave-field-label-gutter)"
            ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.three; Flex.Flex.allSizes ]
    )
], attrs = [ AlignItems.stretch ])"""

    Helpers.codeSampleSection "Adding Tasks" description content code

  let private taskListSection () =
    let description =
      Helpers.bodyText
        "Each task renders a Checkbox for completion, the task text with a reactive strikethrough, a priority Chip, and a delete IconButton. The list is bound reactively with Doc.BindView — when items change, the UI updates automatically."

    let content = Doc.Empty

    let code =
      """open Weave.Icons.MaterialSymbols

let removeTask (id: int) =
    Var.Update items (List.filter (fun t -> t.Id <> id))

// Start with all tasks; the Filtering & Stats section derives filteredItems.
items.View
|> Doc.BindView(fun list ->
    if List.isEmpty list then
        Alert.create(
            text "No tasks yet — add one above!",
            attrs = [ Alert.Color.info; Alert.Variant.outlined ]
        )
    else
        div [] [
            for item in list do
                div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
                    // Label the checkbox with the task text and style it reactively
                    div [
                        FlexItem.Flex.allSizes
                        Attr.DynamicStyle "text-decoration"
                            (item.IsCompleted.View
                             |> View.Map(fun c ->
                                 if c then "line-through" else "none"))
                    ] [
                        Checkbox.create(
                            item.IsCompleted,
                            View.Const item.Text,
                            attrs = [ Checkbox.Color.primary ]
                        )
                    ]

                    // Priority chip — color from pattern match
                    Chip.create(
                        text (priorityToString item.Priority),
                        attrs = [ priorityToColor item.Priority; Chip.Variant.filled ]
                    )

                    // Delete button
                    IconButton.create(
                        Icon.create(Icon.UiActions UiActions.Delete),
                        onClick = (fun () -> removeTask item.Id),
                        attrs = [ Attr.Create "aria-label" ("Delete " + item.Text) ]
                    )
                ]
        ])"""

    Helpers.codeSampleSection "Task List" description content code

  let private filteringSection () =
    let description =
      Helpers.bodyText
        "A Switch toggles the 'show completed' filter. Each task has its own completion Var, so View.Bind and View.Map2 combine those nested views with the changing task list. Both the filter and count then update when a checkbox changes."

    let content = Doc.Empty

    let code =
      """let showCompleted = Var.Create true

// Subscribe to the current list and to every task's completion state.
// View.Bind rebuilds the combined view when tasks are added or removed.
let itemStates =
    items.View
    |> View.Bind(fun list ->
        (list, View.Const [])
        ||> List.foldBack (fun item acc ->
            View.Map2 (fun completed rest -> (item, completed) :: rest)
                item.IsCompleted.View acc))

// When showing all tasks, listen to the list directly so toggling a checkbox
// does not rebuild its row and remove keyboard focus.
let filteredItems =
    showCompleted.View
    |> View.Bind(fun show ->
        if show then items.View
        else
            itemStates
            |> View.Map(fun states ->
                states |> List.choose (fun (item, completed) ->
                    if completed then None else Some item)))

let totalCount = items.View |> View.MapCached List.length
let completedCount = itemStates |> View.Map (List.filter snd >> List.length)

// Bind the task list to filteredItems. When it is empty, use totalCount
// to distinguish no tasks from tasks hidden by the filter.

// The filter bar
Switch.create(
    showCompleted,
    div [ Typography.body2 ] [ text "Show completed" ],
    attrs = [ Switch.Color.primary ]
)

div [ Typography.body2 ] [
    textView (
        (completedCount, totalCount)
        ||> View.Map2(fun c t -> sprintf "%d / %d completed" c t)
    )
]"""

    Helpers.codeSampleSection "Filtering & Stats" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Task Tracker"

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "A reactive to-do list with priorities and filtering — built with Weave. This showcase demonstrates F# discriminated unions for domain modeling, reactive state with Var/View, and how little boilerplate Weave needs for a fully functional app."
        ]

        Helpers.divider ()
        fullAppSection ()
        Helpers.divider ()
        domainModelSection ()
        Helpers.divider ()
        addingTasksSection ()
        Helpers.divider ()
        taskListSection ()
        Helpers.divider ()
        filteringSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
