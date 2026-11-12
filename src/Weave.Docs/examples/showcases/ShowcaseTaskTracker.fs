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
    let items: Var<TodoItem list> =
      Var.Create [
        {
          Id = 0
          Text = "Choose the workspace layout"
          Priority = High
          IsCompleted = Var.Create true
        }
        {
          Id = 1
          Text = "Connect the task form"
          Priority = Medium
          IsCompleted = Var.Create false
        }
        {
          Id = 2
          Text = "Try the dark theme"
          Priority = Low
          IsCompleted = Var.Create false
        }
      ]

    let nextId = Var.Create 3
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
        p [ Typography.body2; Typography.Color.textSecondary; Margin.Bottom.small ] [
          text "Fictional sample tasks. Changes stay in this demo and reset when you leave or refresh."
        ]
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
                  FlexWrap.Wrap.allSizes
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
                    Attr.Style "min-inline-size" "0"
                    Attr.Style "flex-basis" "16rem"
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

                  div [
                    Flex.Flex.allSizes
                    AlignItems.center
                    Gap.All.g2
                    FlexItem.NoShrink.allSizes
                    Attr.Style "margin-inline-start" "auto"
                  ] [
                    Chip.create (
                      text (priorityToString item.Priority),
                      attrs = [
                        priorityToColor item.Priority
                        Chip.Variant.filled
                        FlexItem.NoShrink.allSizes
                      ]
                    )

                    IconButton.create (
                      Icon.create (
                        Icon.UiActions UiActions.Delete,
                        attrs = [ Attr.Style "font-size" "18px" ]
                      ),
                      onClick = (fun () -> removeTask item.Id),
                      attrs = [ Density.compact; Attr.Create "aria-label" ("Delete " + item.Text) ]
                    )
                  ]
                ]
            ])
      ]

    Helpers.section "The Full App" description content

  let private domainSource =
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

  let private addingSource =
    """// Start empty in your app. The live demo seeds three fictional sample tasks.
let items: Var<TodoItem list> = Var.Create []
let nextId = Var.Create 0
let newText = Var.Create ""
let newPriority = Var.Create<string option>(Some "Medium")

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
  |> List.map (fun label -> SelectItem.create (text label, label, label))
  |> View.Const

// The form
let taskForm =
  Grid.create (
    [
      GridItem.create (
        TextField.singleLine (
          newText,
          labelText = View.Const "New task",
          placeholder = View.Const "What needs to be done?",
          attrs = [
            TextField.Variant.outlined
            TextField.Color.primary
            TextField.Width.full
          ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.five ]
      )
      GridItem.create (
        Select.create (
          priorityItems,
          newPriority,
          labelText = View.Const "Priority",
          attrs = [
            Select.Variant.outlined
            Select.Color.primary
            Select.Width.full
          ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
      )
      GridItem.create (
        Button.create (
          text "Add",
          onClick = addTask,
          attrs = [
            Button.Color.primary
            Button.Variant.filled
            // Align with the inputs below their labels.
            Attr.Style "margin-top" "var(--weave-field-label-gutter)"
          ]
        ),
        attrs = [
          GridItem.Span.twelve
          GridItem.Span.Medium.three
          Flex.Flex.allSizes
        ]
      )
    ],
    attrs = [ AlignItems.stretch ]
  )"""

  let private listSource =
    """open Weave.Icons.MaterialSymbols

let removeTask (id: int) =
  Var.Update items (List.filter (fun t -> t.Id <> id))

let taskList (source: View<TodoItem list>) =
  source
  |> Doc.BindView(fun list ->
    if List.isEmpty list then
      Alert.create (
        text "No tasks yet — add one above!",
        attrs = [ Alert.Color.info; Alert.Variant.outlined ]
      )
    else
      div [] [
        for item in list do
          div [
            Flex.Flex.allSizes
            FlexWrap.Wrap.allSizes
            AlignItems.center
            Gap.All.g2
          ] [
            // Label the checkbox with the task text and style it reactively
            div [
              FlexItem.Flex.allSizes
              Attr.Style "min-inline-size" "0"
              Attr.Style "flex-basis" "16rem"
              Attr.DynamicStyle
                "text-decoration"
                (item.IsCompleted.View
                 |> View.Map(fun c -> if c then "line-through" else "none"))
            ] [
              Checkbox.create (
                item.IsCompleted,
                View.Const item.Text,
                attrs = [ Checkbox.Color.primary ]
              )
            ]

            div [
              Flex.Flex.allSizes
              AlignItems.center
              Gap.All.g2
              FlexItem.NoShrink.allSizes
              Attr.Style "margin-inline-start" "auto"
            ] [
              // Priority chip — color from pattern match
              Chip.create (
                text (priorityToString item.Priority),
                attrs = [
                  priorityToColor item.Priority
                  Chip.Variant.filled
                  FlexItem.NoShrink.allSizes
                ]
              )

              // Delete button
              IconButton.create (
                Icon.create (Icon.UiActions UiActions.Delete),
                onClick = (fun () -> removeTask item.Id),
                attrs = [ Attr.Create "aria-label" ("Delete " + item.Text) ]
              )
            ]
          ]
      ])"""

  let private filterSource =
    """let showCompleted = Var.Create true

// Subscribe to the current list and to every task's completion state.
// View.Bind rebuilds the combined view when tasks are added or removed.
let itemStates =
  items.View
  |> View.Bind(fun list ->
    (list, View.Const [])
    ||> List.foldBack (fun item acc ->
      View.Map2
        (fun completed rest -> (item, completed) :: rest)
        item.IsCompleted.View
        acc))

// When showing all tasks, listen to the list directly so toggling a checkbox
// does not rebuild its row and remove keyboard focus.
let filteredItems =
  showCompleted.View
  |> View.Bind(fun show ->
    if show then
      items.View
    else
      itemStates
      |> View.Map(fun states ->
        states
        |> List.choose (fun (item, completed) ->
          if completed then None else Some item)))

let totalCount = items.View |> View.MapCached List.length
let completedCount = itemStates |> View.Map(List.filter snd >> List.length)

let filterBar =
  div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g3 ] [
    Switch.create (showCompleted, content = text "Show completed")
    textView (
      (completedCount, totalCount)
      ||> View.Map2(fun completed total ->
        sprintf "%d / %d completed" completed total)
    )
  ]

div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
  taskForm
  filterBar
  taskList filteredItems
]"""

  // These are the exact displayed blocks, in their learning order.
  let private sourceParts = [ domainSource; addingSource; listSource; filterSource ]

  let private combinedSource = String.concat "\n\n" sourceParts

  let private sourceSections () =
    div [] [

      Helpers.codeSection
        "fsharp"
        "Domain Model"
        (Helpers.bodyText "Start here. Define the task model and its priority mappings.")
        domainSource

      Helpers.codeSection
        "fsharp"
        "Adding Tasks"
        (Helpers.bodyText
          "Continues Domain Model. Define the shared list, input state, and taskForm. The small example starts empty; the full demo uses fictional sample tasks.")
        addingSource

      Helpers.codeSection
        "fsharp"
        "Task List"
        (Helpers.bodyText
          "Continues Domain Model and Adding Tasks. Define taskList so the final composition can supply a filtered View.")
        listSource

      Helpers.codeSection
        "fsharp"
        "Filtering & Stats"
        (Helpers.bodyText
          "Continues all previous sections. Derive the filter and counts, then compose the form, filter bar, and task list.")
        filterSource

      Helpers.codeSection
        "fsharp"
        "Combined example"
        (Helpers.bodyText
          "Copy all the sections in order. Use the application setup in Getting Started and paste this source inside its JavaScript module; its final expression returns the composed Doc. The smaller example focuses on the mechanisms above, while the full demo adds presentation and sample data.")
        combinedSource
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Task Tracker"

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "A task list with priorities, completion state, filtering, and a progress indicator. Follow the source sections to combine a typed task model with Var/View bindings."
        ]

        Helpers.divider ()
        fullAppSection ()
        Helpers.divider ()
        Helpers.bodyText
          "Build the smaller example from top to bottom. Each source section continues the preceding sections; Combined example copies the same sequence in one block."
        sourceSections ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
