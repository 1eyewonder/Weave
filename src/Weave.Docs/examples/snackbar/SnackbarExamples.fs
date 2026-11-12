namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module SnackbarExamples =

  let private comparison components =
    Grid.create (
      components |> List.map (fun content -> GridItem.create content),
      attrs = [ AlignItems.center ]
    )

  let private apiMember name returnType parameters =
    Helpers.apiMember name returnType parameters

  let private messageOptions keyDefault = [
    Helpers.apiParam
      "?dismissal"
      "Snackbar.Dismissal"
      "Manual / After"
      "Defaults to Manual when an action is supplied; otherwise After the controller duration. After sets visible milliseconds (minimum 1). Hover and focus pause the countdown; queued time does not count."
    Helpers.apiParam
      "?action"
      "SnackbarAction"
      ""
      "A labeled button that dismisses the message before calling its handler"
    Helpers.apiParam
      "?key"
      "string"
      keyDefault
      "A matching visible or queued key returns the existing message ID"
    Helpers.apiParam "?icon" "Doc" "" "Decorative leading icon"
    Helpers.apiParam "?attrs" "Attr list" "[]" "Message colors, variants, and native attributes"
  ]

  let private apiReference () =
    Helpers.apiSection
      (Helpers.bodyText
        "Create a controller once and mount one host for it. show and add return a message ID that you can pass to remove. Place the host near your app root, outside transformed or clipped containers.")
      [
        apiMember "SnackbarController.create" "SnackbarController" [
          Helpers.apiParam
            "?duration"
            "int"
            "5000"
            "Default visible milliseconds for messages without an action; minimum 1"
          Helpers.apiParam
            "?maxVisible"
            "int"
            "3"
            "Maximum displayed messages; additional messages wait in FIFO order; minimum 1"
        ]
        apiMember "Snackbar.create" "Doc" [
          Helpers.apiParam
            "controller"
            "SnackbarController"
            ""
            "Queue to render; mount one host per controller"
          Helpers.apiParam
            "?attrs"
            "Attr list"
            "[]"
            "Host position and native attributes; default bottom right"
        ]
        apiMember "SnackbarController.show" "string" [
          Helpers.apiParam
            "message"
            "string"
            ""
            "Plain text, safely encoded; identical messages are deduplicated by default"
          yield! messageOptions "message text"
        ]
        apiMember "SnackbarController.add" "string" [
          Helpers.apiParam
            "content"
            "Doc"
            ""
            "Custom content; supply a key when duplicate suppression is needed"
          yield! messageOptions "none"
        ]
        apiMember "SnackbarAction.create" "SnackbarAction" [
          Helpers.apiParam "label" "string" "" "Accessible action button label"
          Helpers.apiParam "onClick" "unit -> unit" "" "Called after the message is dismissed"
        ]
        Helpers.styleModuleTable "Queue management" [
          "snackbars.remove(messageId)", "Remove a visible or queued message by ID. Returns unit."
          "snackbars.clear()", "Remove every visible and queued message. Returns unit."
        ]
        Helpers.styleModuleTable "Snackbar.Dismissal" [
          "After milliseconds", "Dismiss after the given visible time; hover and focus pause the countdown"
          "Manual", "Keep visible until closed, removed, cleared, or its action is activated"
        ]
        Helpers.styleModuleTable "Snackbar.Position (host attrs)" [
          "topLeft / topCenter / topRight", "Top viewport positions"
          "bottomLeft / bottomCenter / bottomRight", "Bottom viewport positions"
        ]
        Helpers.styleModuleTable "Snackbar.Variant (message attrs)" [
          "standard", "Surface background and semantic foreground"
          "outlined", "Bordered paper background"
          "filled", "Solid semantic background and matching foreground"
        ]
        Helpers.styleModuleTable "Snackbar.Color (message attrs)" [
          "primary / secondary / tertiary", "Brand colors"
          "success / error / warning / info", "Semantic colors; announcements remain polite"
        ]
      ]

  let private saveExample (snackbars: SnackbarController) =
    let content =
      let saved = Var.Create 0

      let saveDraft () =
        Var.Update saved ((+) 1)

        snackbars.show ("Release notes saved.", attrs = [ Snackbar.Color.success ])
        |> ignore

      div [ Attr.Style "text-align" "center" ] [
        Button.primary (text "Save draft", onClick = saveDraft, attrs = [ Button.Variant.filled ])
        p [ Attr.Create "role" "status" ] [
          textView (saved.View |> View.Map(sprintf "Saved in this demo %d times."))
        ]
      ]

    Helpers.codeSampleSection
      "Save Feedback"
      (Helpers.bodyText
        "Create a controller and mount one host for it. Save twice while the message is visible: the count changes, and the matching notification is deduplicated. This demo saves no data.")
      content
      """let snackbars = SnackbarController.create ()
let saved = Var.Create 0

let saveDraft () =
  Var.Update saved ((+) 1)

  snackbars.show ("Release notes saved.", attrs = [ Snackbar.Color.success ])
  |> ignore

Doc.Concat [
  Button.primary (
    text "Save draft",
    onClick = saveDraft,
    attrs = [ Button.Variant.filled ]
  )
  p [ Attr.Create "role" "status" ] [
    textView (saved.View |> View.Map(sprintf "Saved in this demo %d times."))
  ]
  Snackbar.create snackbars
]"""

  let private undoExample (snackbars: SnackbarController) =
    let content =
      let reportInTrash = Var.Create false

      let deleteReport () =
        Var.Set reportInTrash true
        let restore () = Var.Set reportInTrash false

        snackbars.show ("Quarterly report moved to trash.", action = SnackbarAction.create ("Undo", restore))
        |> ignore

      div [ Attr.Style "text-align" "center" ] [
        Button.error (
          text "Delete report",
          onClick = deleteReport,
          enabled = (reportInTrash.View |> View.Map not),
          attrs = [ Button.Variant.outlined ]
        )
        p [ Attr.Create "role" "status" ] [
          textView (
            reportInTrash.View
            |> View.Map(fun deleted ->
              if deleted then
                "Report in trash."
              else
                "Report in workspace.")
          )
        ]
      ]

    Helpers.codeSampleSection
      "Undo a Delete"
      (Helpers.bodyText
        "Delete the sample report, then use Undo in the notification to restore its state. An action keeps its message visible until you act or dismiss it.")
      content
      """let snackbars = SnackbarController.create ()
let reportInTrash = Var.Create false

let deleteReport () =
  Var.Set reportInTrash true
  let restore () = Var.Set reportInTrash false

  snackbars.show (
    "Quarterly report moved to trash.",
    action = SnackbarAction.create ("Undo", restore)
  )
  |> ignore

Doc.Concat [
  Button.error (
    text "Delete report",
    onClick = deleteReport,
    enabled = (reportInTrash.View |> View.Map not),
    attrs = [ Button.Variant.outlined ]
  )
  p [ Attr.Create "role" "status" ] [
    textView (
      reportInTrash.View
      |> View.Map(fun deleted ->
        if deleted then
          "Report in trash."
        else
          "Report in workspace.")
    )
  ]
  Snackbar.create snackbars
]"""

  let private uploadExample (snackbars: SnackbarController) =
    let content =

      let showUploadSummary () =
        let summary =
          div [] [
            strong [] [ text "Report bundle uploaded" ]
            div [ Typography.caption ] [ text "3 files are ready to share." ]
          ]

        snackbars.add (
          summary,
          key = "report-upload",
          attrs = [ Snackbar.Color.success; Snackbar.Variant.outlined ]
        )
        |> ignore

      comparison [
        Button.tertiary (
          text "Upload reports",
          onClick = showUploadSummary,
          attrs = [ Button.Variant.filled ]
        )
      ]

    Helpers.codeSampleSection
      "Custom Content"
      (Helpers.bodyText
        "Use add for a title and a second line. A stable key prevents repeated actions from stacking identical messages. This upload is illustrative.")
      content
      """let snackbars = SnackbarController.create ()

let showUploadSummary () =
  let summary =
    div [] [
      strong [] [ text "Report bundle uploaded" ]
      div [ Typography.caption ] [ text "3 files are ready to share." ]
    ]

  snackbars.add (
    summary,
    key = "report-upload",
    attrs = [ Snackbar.Color.success; Snackbar.Variant.outlined ]
  )
  |> ignore

Doc.Concat [
  Button.tertiary (
    text "Upload reports",
    onClick = showUploadSummary,
    attrs = [ Button.Variant.filled ]
  )
  Snackbar.create snackbars
]"""

  let private queueExample (snackbars: SnackbarController) =
    let content =

      let runQueueDemo () =
        snackbars.clear ()

        for message in [ "Design ready"; "Build finished"; "Preview ready"; "Tagged" ] do
          snackbars.show (message, dismissal = Snackbar.Dismissal.Manual) |> ignore

      let showTimedMessage () =
        snackbars.clear ()

        snackbars.show ("Preview link copied.", dismissal = Snackbar.Dismissal.After 3000)
        |> ignore

      comparison [
        Button.info (text "Run queue demo", onClick = runQueueDemo)
        Button.create (text "Show timed message", onClick = showTimedMessage)
        Button.create (text "Clear all", onClick = snackbars.clear)
      ]

    Helpers.codeSampleSection
      "Queue and Dismissal"
      (Helpers.bodyText
        "Run queue demo sends four messages: three are visible and one waits. Dismiss one to see the next. The timed example expires after three visible seconds; hover or focus pauses its countdown.")
      content
      """let snackbars = SnackbarController.create (maxVisible = 3)

let runQueueDemo () =
  snackbars.clear ()

  for message in
    [ "Design ready"; "Build finished"; "Preview ready"; "Tagged" ] do
    snackbars.show (message, dismissal = Snackbar.Dismissal.Manual) |> ignore

let showTimedMessage () =
  snackbars.clear ()

  snackbars.show (
    "Preview link copied.",
    dismissal = Snackbar.Dismissal.After 3000
  )
  |> ignore

Doc.Concat [
  Button.info (text "Run queue demo", onClick = runQueueDemo)
  Button.create (text "Show timed message", onClick = showTimedMessage)
  Button.create (text "Clear all", onClick = snackbars.clear)
  Snackbar.create snackbars
]"""

  let render () =
    let snackbars = SnackbarController.create ()
    let selectedPosition = Var.Create "Bottom right"

    let host =
      selectedPosition.View
      |> Doc.BindView(fun selected ->
        let position =
          match selected with
          | "Top left" -> Snackbar.Position.topLeft
          | "Top center" -> Snackbar.Position.topCenter
          | "Top right" -> Snackbar.Position.topRight
          | "Bottom left" -> Snackbar.Position.bottomLeft
          | "Bottom center" -> Snackbar.Position.bottomCenter
          | _ -> Snackbar.Position.bottomRight

        Snackbar.create (snackbars, attrs = [ position ]))

    Container.create (
      div [] [
        Helpers.pageTitle "Snackbar"
        Helpers.bodyText
          "Give brief feedback without interrupting a task. The live examples share one controller and host; each source sample includes its own setup so it can run independently. Messages pause on hover or focus and include a dismiss button."
        saveExample snackbars
        SnackbarConfigurator.render snackbars selectedPosition
        undoExample snackbars
        uploadExample snackbars
        queueExample snackbars
        apiReference ()
        host
      ],
      attrs = [ Container.MaxWidth.large ]
    )
