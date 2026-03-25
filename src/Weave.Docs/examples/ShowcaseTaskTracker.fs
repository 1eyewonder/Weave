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

    let filteredItems =
      (items.View, showCompleted.View)
      ||> View.Map2(fun list show ->
        if show then
          list
        else
          list |> List.filter (fun t -> not t.IsCompleted.Value))

    let totalCount = items.View |> View.Map List.length

    let completedCount =
      items.View
      |> View.Map(List.filter (fun t -> t.IsCompleted.Value) >> List.length)

    let description =
      Helpers.bodyText
        "The complete Task Tracker app. Add tasks with a priority, check them off, filter, and track your progress — all fully reactive."

    let content =
      div [] [
        // Add task form
        Grid.create (
          [
            GridItem.create (
              Field.create (
                newText,
                variant = Field.Variant.Outlined,
                labelText = View.Const "New task",
                placeholder = View.Const "What needs to be done?",
                attrs = [ Field.Color.primary; Field.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.five ]
            )

            GridItem.create (
              Select.create (
                priorityItems,
                newPriority,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Priority",
                attrs = [ Select.Color.primary; Select.Width.full ]
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
                  Attr.Style "height" "100%"
                  Attr.Style "min-height" "40px"
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
        filteredItems
        |> Doc.BindView(fun list ->
          if List.isEmpty list then
            Alert.create (
              div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
                Icon.create (Icon.Text Text.Checklist)
                text "No tasks yet — add one above!"
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
                  SurfaceColor.toBackgroundColor SurfaceColor.Surface
                  BorderRadius.All.small
                  Margin.Bottom.extraSmall
                  Elevation.e1
                ] [
                  Checkbox.create (
                    item.IsCompleted,
                    View.Const "",
                    attrs = [ Checkbox.Color.primary; Density.compact ]
                  )

                  div [
                    FlexItem.Flex.allSizes
                    Typography.body1
                    Attr.DynamicStyle
                      "text-decoration"
                      (item.IsCompleted.View |> View.Map(fun c -> if c then "line-through" else "none"))
                    Attr.DynamicStyle
                      "opacity"
                      (item.IsCompleted.View |> View.Map(fun c -> if c then "0.5" else "1"))
                  ] [ text item.Text ]

                  Chip.create (
                    text (priorityToString item.Priority),
                    attrs = [ priorityToColor item.Priority; Chip.Variant.filled ]
                  )

                  IconButton.create (
                    Icon.create (Icon.UiActions UiActions.Delete, attrs = [ Attr.Style "font-size" "18px" ]),
                    onClick = (fun () -> removeTask item.Id),
                    attrs = [ Density.compact ]
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
    | High -> Chip.Color.error"""

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

// The form
Grid.create ([
    GridItem.create (
        Field.create(
            newText,
            variant = Field.Variant.Outlined,
            labelText = View.Const "New task",
            placeholder = View.Const "What needs to be done?",
            attrs = [ Field.Color.primary; Field.Width.full ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.five ]
    )
    GridItem.create (
        Select.create(
            priorityItems,
            newPriority,
            variant = Select.Variant.Outlined,
            labelText = View.Const "Priority",
            attrs = [ Select.Color.primary; Select.Width.full ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
    )
    GridItem.create (
        Button.create(
            text "Add",
            onClick = addTask,
            attrs = [ Button.Color.primary; Button.Variant.filled ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.three ]
    )
])"""

    Helpers.codeSampleSection "Adding Tasks" description content code

  let private taskListSection () =
    let description =
      Helpers.bodyText
        "Each task renders a Checkbox for completion, the task text with a reactive strikethrough, a priority Chip, and a delete IconButton. The list is bound reactively with Doc.BindView — when items change, the UI updates automatically."

    let content = Doc.Empty

    let code =
      """// Reactive task list rendering
filteredItems
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
                    // Two-way bound checkbox
                    Checkbox.create(
                        item.IsCompleted,
                        View.Const "",
                        attrs = [ Checkbox.Color.primary ]
                    )

                    // Reactive strikethrough when completed
                    div [
                        FlexItem.Flex.allSizes
                        Attr.DynamicStyle "text-decoration"
                            (item.IsCompleted.View
                             |> View.Map(fun c ->
                                 if c then "line-through" else "none"))
                    ] [ text item.Text ]

                    // Priority chip — color from pattern match
                    Chip.create(
                        text (priorityToString item.Priority),
                        attrs = [ priorityToColor item.Priority; Chip.Variant.filled ]
                    )

                    // Delete button
                    IconButton.create(
                        Icon.create(Icon.UiActions UiActions.Delete),
                        onClick = (fun () -> removeTask item.Id)
                    )
                ]
        ])"""

    Helpers.codeSampleSection "Task List" description content code

  let private filteringSection () =
    let description =
      Helpers.bodyText
        "A Switch toggles the 'show completed' filter. The completed/total count is a computed View — no imperative counters needed. View.Map2 combines the list and filter state into a single derived view."

    let content = Doc.Empty

    let code =
      """let showCompleted = Var.Create true

// Derived filtered list — recomputes whenever items or filter changes
let filteredItems =
    (items.View, showCompleted.View)
    ||> View.Map2(fun list show ->
        if show then list
        else list |> List.filter (fun t -> not t.IsCompleted.Value))

// Computed stats — no manual counter, just View.Map
let totalCount = items.View |> View.Map List.length
let completedCount =
    items.View
    |> View.Map (List.filter (fun t -> t.IsCompleted.Value) >> List.length)

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
