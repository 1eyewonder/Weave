namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Operators
open Weave.Docs.Examples.DocsRouting

[<JavaScript>]
module DocsHome =

  let catalog = [
    "Components",
    [
      AlertExamples
      AppBarExamples
      ButtonExamples
      ButtonGroupExamples
      ButtonMenuExamples
      CardExamples
      CarouselExamples
      CheckboxExamples
      ChipExamples
      ChipSetExamples
      ColorPickerExamples
      ContainerExamples
      DialogExamples
      DividerExamples
      DrawerExamples
      DropdownMenuExamples
      ExpansionPanelExamples
      FieldExamples
      FormExamples
      GridExamples
      HighlighterExamples
      IconsExamples
      LinkExamples
      ListExamples
      NumericFieldExamples
      PaginationExamples
      ProgressExamples
      RadioButtonExamples
      ScrollToTopExamples
      SelectExamples
      SliderExamples
      SnackbarExamples
      SpacerExamples
      SwitchExamples
      TableExamples
      TabsExamples
      TextFieldExamples
      TooltipExamples
      TypographyExamples
    ]
    "Styling",
    [
      SpacingExamples
      OpacityExamples
      BorderExamples
      DisplayExamples
      ElevationExamples
      FlexboxExamples
      TransitionExamples
      AnimationExamples
      ThemingExamples
    ]
    "Utilities", [ ReactiveHelpersExamples; EventHandlersExamples ]
    "Showcases",
    [
      ShowcaseTaskTracker
      ShowcasePomodoroTimer
      ShowcaseExpenseTracker
      ShowcaseCharacterSheet
    ]
  ]

  type private DemoTask = {
    Name: string
    Done: Var<bool>
  }

  let private navigationLink href label = Link.create (text label, href = href)

  let private workbench () =
    let tasks =
      Var.Create [
        {
          Name = "Explore the component library"
          Done = Var.Create true
        }
        {
          Name = "Make the workspace yours"
          Done = Var.Create false
        }
        {
          Name = "Ship a first idea"
          Done = Var.Create false
        }
      ]

    let draft = Var.Create ""

    let completed =
      tasks.View
      |> View.Bind(fun rows ->
        (rows, View.Const 0)
        ||> List.foldBack (fun row rest ->
          View.Map2 (fun doneValue count -> count + (if doneValue then 1 else 0)) row.Done.View rest))

    let total = tasks.View |> View.Map List.length

    let progress =
      View.Map2
        (fun count countAll ->
          if countAll = 0 then
            0.0
          else
            100.0 * float count / float countAll)
        completed
        total

    let addTask () =
      let name = draft.Value.Trim()

      if name <> "" then
        Var.Update tasks (fun rows ->
          rows
          @ [
            {
              Name = name
              Done = Var.Create false
            }
          ])

        Var.Set draft ""

    let sample =
      """let completed = Var.Create false
let label = View.Const "Make the workspace yours"

Checkbox.create (completed, label)"""

    Card.create (
      Doc.Concat [
        div [ Attr.Class "docs-home__specimen" ] [
          h2 [ Typography.h5 ] [ text "A little project, taking shape." ]
          p [ Attr.Class "docs-home__source-note" ] [
            text "Sample tasks. Changes stay in this demo until you leave or refresh."
          ]
          div [ Attr.Class "docs-home__task-list" ] [
            tasks.View
            |> Doc.BindView(fun rows ->
              div [] [
                for row in rows do
                  div [ Attr.Class "docs-home__task" ] [
                    Checkbox.create (row.Done, displayText = View.Const row.Name)
                    row.Done.View
                    |> Doc.BindView(fun doneValue ->
                      Chip.create (
                        text (if doneValue then "Done" else "In progress"),
                        attrs = [
                          Chip.Variant.outlined
                          if doneValue then Chip.Color.success else Chip.Color.primary
                        ]
                      ))
                  ]
              ])
          ]
          div [ Attr.Class "docs-home__add" ] [
            TextField.singleLine (
              draft,
              labelText = View.Const "Add a task",
              placeholder = View.Const "Your next idea",
              inputAttrs = [
                on.keyDown (fun _ ev ->
                  if ev.Key = "Enter" then
                    ev.PreventDefault()
                    addTask ())
              ],
              attrs = [ TextField.Variant.outlined; TextField.Label.above; TextField.Width.full ]
            )
            Button.primary (
              text "Add task",
              onClick = addTask,
              enabled = (draft.View |> View.Map(fun value -> value.Trim() <> "")),
              attrs = [ Button.Variant.filled ]
            )
          ]
          div [ Attr.Class "docs-home__progress" ] [
            p [ Attr.Create "role" "status" ] [
              textView (
                View.Map2
                  (fun count countAll -> sprintf "%d of %d tasks completed" count countAll)
                  completed
                  total
              )
            ]
            Progress.Linear.create (
              value = progress,
              attrs = [ Attr.Create "aria-label" "Project task progress" ]
            )
          ]
          div [ Attr.Class "docs-home__component-trail" ] [
            span [] [ text "Made with" ]
            for page in
              [
                CardExamples
                ButtonExamples
                CheckboxExamples
                TextFieldExamples
                ChipExamples
                ProgressExamples
              ] do
              Link.create (
                text (pageToString page),
                href = pageToHash page,
                attrs = [ Link.Color.primary; Link.Underline.always ]
              )
          ]
        ]
        div [ Attr.Class "docs-home__source" ] [
          div [ Attr.Class "docs-home__source-heading" ] [
            h3 [ Typography.h6 ] [ text "One task, connected." ]
            Helpers.copyButton sample
          ]
          p [ Attr.Class "docs-home__source-note" ] [
            text
              "This is the binding behind one checkbox in the project. A Var holds its state; Checkbox updates it as you interact."
          ]
          pre [ attr.tabindex "0"; Attr.Create "aria-label" "Reactive task F# source code" ] [
            code [ Attr.Class "language-fsharp"; on.afterRender Helpers.highlightCodeElement ] [ text sample ]
          ]
          Link.create (
            text "Open the complete Task Tracker example",
            href = pageToHash ShowcaseTaskTracker,
            attrs = [ Link.Color.primary; Link.Underline.always ]
          )
        ]
      ],
      attrs = [ Attr.Class "docs-home__workbench" ]
    )

  let private starter name description slug =
    figure [ Attr.Class "docs-home__starter" ] [
      a [ attr.href "#getting-started/4.-explore-the-workspace-templates" ] [
        for mode in [ "light"; "dark" ] do
          img [
            attr.src (sprintf "assets/templates/%s-%s.jpg" slug mode)
            attr.alt (sprintf "%s starter in %s mode" name mode)
            Attr.Class("docs-home__starter-image docs-home__starter-image--" + mode)
            Attr.Create "width" "1440"
            Attr.Create "height" "900"
            Attr.Create "loading" "lazy"
          ] []
        figcaption [] [ strong [] [ text name ]; span [] [ text description ] ]
      ]
    ]

  let render () =
    div [ Attr.Class "docs-home" ] [
      div [ Attr.Class "docs-home__intro" ] [
        h1 [ Typography.h2 ] [ text "From idea to interface, in F#." ]
        p [] [
          text
            "Reactive components, expressive layouts, and a shared theme. Compose your next interface with Weave for WebSharper."
        ]
        div [ Attr.Class "docs-home__actions" ] [
          navigationLink (pageToHash GettingStartedExamples) "Get started"
          navigationLink "#home/component-directory" "Browse components"
        ]
      ]
      workbench ()
      section [ Attr.Class "docs-home__section" ] [
        h2 [ Typography.h5 ] [ text "Take the next step." ]
        div [ Attr.Class "docs-home__paths" ] [
          for title, description, href in
            [
              "Build your first component",
              "Start an app and connect your first reactive view.",
              pageToHash GettingStartedExamples
              "Put a form together",
              "Combine fields, validation, and useful feedback.",
              pageToHash FormExamples
              "Make the theme yours",
              "Give every component a shared color language.",
              pageToHash ThemingExamples
            ] do
            article [ Attr.Class "docs-home__path" ] [
              Link.create (
                strong [] [ text title ],
                href = href,
                attrs = [ Link.Color.primary; Link.Underline.always ]
              )
              p [] [ text description ]
            ]
        ]
      ]
      section [ Attr.Class "docs-home__section" ] [
        h2 [ Typography.h5 ] [ text "Start with a complete workspace." ]
        p [] [
          text
            "Real starter applications, built from the same components. Explore the layouts, then make one your own."
        ]
        div [ Attr.Class "docs-home__starters" ] [
          starter "Project workspace" "A project tracker with list and board views." "projects"
          starter "Business dashboard" "Metrics, filters, and a transactions table." "dashboard"
        ]
      ]
      section [ Attr.Class "docs-home__section docs-home__directory" ] [
        Helpers.sectionHeader "Component directory"
        p [] [
          text "Find a building block, inspect the live example, and take the F# with you."
        ]
        for title, pages in catalog do
          details [
            if title = "Components" then
              Attr.Create "open" ""
          ] [
            summary [] [ text (sprintf "%s (%d)" title pages.Length) ]
            div [ Attr.Class "docs-home__directory-links" ] [
              for page in pages do
                Link.create (
                  text (pageToString page),
                  href = pageToHash page,
                  attrs = [ Link.Color.primary; Link.Underline.always ]
                )
            ]
          ]
      ]
    ]
