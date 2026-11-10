namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module SnackbarExamples =

  let private apiMember name returnType parameters =
    div [ Margin.Bottom.small ] [
      Helpers.apiTable name parameters
      p [ Typography.body2; Typography.Color.textSecondary ] [
        text "Returns "
        Helpers.inlineCode returnType
      ]
    ]

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

  let private buttonRow buttons =
    div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; AlignItems.center; Gap.All.g2 ] buttons

  let private decorativeIcon symbol color =
    Icon.create (symbol, attrs = [ color; Attr.Create "aria-hidden" "true" ])

  let private buttonLabel symbol label =
    span [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
      decorativeIcon symbol Attr.Empty
      text label
    ]

  let private saveExample (snackbars: SnackbarController) =
    let saveCount = Var.Create 0

    let saveDraft () =
      Var.Set saveCount (saveCount.Value + 1)

      snackbars.show ("Release notes saved.", attrs = [ Snackbar.Color.success ])
      |> ignore

    let content =
      Card.create (
        Doc.Concat [
          CardHeader.create (
            text "Release notes",
            subtitle = text "Workspace / Product updates",
            leading = decorativeIcon (Icon.UiActions UiActions.CheckCircle) Typography.Color.primary,
            actions = Chip.create (text "Draft", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
          )
          CardContent.create (
            div [] [
              div [ Typography.subtitle1; Margin.Bottom.extraSmall ] [
                text "A little progress, worth saving."
              ]
              div [
                Typography.body2
                Typography.Color.textSecondary
                Attr.Create "role" "status"
                Attr.Create "data-testid" "draft-save-status"
              ] [
                Doc.TextView(
                  saveCount.View
                  |> View.Map(fun count ->
                    if count = 0 then
                      "Your draft is ready. Give it a save."
                    else
                      sprintf "Saved in this demo %d time%s." count (if count = 1 then "" else "s"))
                )
              ]
            ]
          )
          CardActions.create (
            Button.primary (text "Save draft", onClick = saveDraft, attrs = [ Button.Variant.filled ])
          )
        ]
      )

    Helpers.codeSampleSection
      "Save Feedback"
      (Helpers.bodyText
        "A quiet confirmation after a familiar action. Save twice while the message is visible: the card updates, but the notification is not duplicated. The examples below share this controller and one host.")
      content
      """open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

let snackbars = SnackbarController.create()

let saveDraft () =
    snackbars.show(
        "Release notes saved.",
        attrs = [Snackbar.Color.success]
    ) |> ignore

div [] [
    Button.primary(text "Save draft", onClick = saveDraft,
        attrs = [Button.Variant.filled])

    // Mount once near your app root.
    Snackbar.create(snackbars)
]"""

  let private toneExamples (snackbars: SnackbarController) =
    let scenarios = [
      "All saved",
      "Your workspace is up to date.",
      "Filled",
      Icon.UiActions UiActions.CheckCircle,
      Typography.Color.success,
      Snackbar.Color.success,
      Snackbar.Variant.filled,
      Button.Color.success
      "Upload interrupted",
      "Connection lost. Your draft is safe.",
      "Filled",
      Icon.Action Action.Error,
      Typography.Color.error,
      Snackbar.Color.error,
      Snackbar.Variant.filled,
      Button.Color.error
      "Storage nearly full",
      "You have 200 MB of storage remaining.",
      "Outlined",
      Icon.Action Action.Warning,
      Typography.Color.warning,
      Snackbar.Color.warning,
      Snackbar.Variant.outlined,
      Button.Color.warning
      "Someone joined",
      "Alex joined the design review.",
      "Standard",
      Icon.Action Action.Info,
      Typography.Color.info,
      Snackbar.Color.info,
      Snackbar.Variant.standard,
      Button.Color.info
    ]

    let content =
      Grid.create (
        [
          for title, message, variantName, symbol, foreground, color, variant, buttonColor in scenarios do
            GridItem.create (
              Card.create (
                Doc.Concat [
                  CardHeader.create (text title, leading = decorativeIcon symbol foreground)
                  CardContent.create (
                    div [] [
                      Chip.create (text variantName, attrs = [ Chip.Variant.outlined ])
                      p [ Typography.body2; Typography.Color.textSecondary; Margin.Top.extraSmall ] [
                        text message
                      ]
                    ]
                  )
                  CardActions.create (
                    Button.create (
                      text "Show notification",
                      onClick =
                        (fun () ->
                          snackbars.show (
                            message,
                            icon = decorativeIcon symbol Attr.Empty,
                            attrs = [ color; variant ]
                          )
                          |> ignore),
                      attrs = [
                        Button.Variant.outlined
                        buttonColor
                        Attr.Create "aria-label" ("Show notification: " + title)
                      ]
                    )
                  )
                ],
                attrs = [ FlexItem.Flex.allSizes ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six ]
            )
        ]
      )

    Helpers.codeSampleSection
      "Colors and Variants"
      (Helpers.bodyText
        "Give each message an appropriate tone. These examples pair a semantic color with an icon and a filled, outlined, or standard surface. Color does not change announcement priority.")
      content
      """// Reuse snackbars from Save Feedback.
snackbars.show(
    "Your workspace is up to date.",
    attrs = [Snackbar.Color.success; Snackbar.Variant.filled]
) |> ignore

snackbars.show(
    "You have 200 MB of storage remaining.",
    attrs = [Snackbar.Color.warning; Snackbar.Variant.outlined]
) |> ignore

snackbars.show(
    "Alex joined the design review.",
    attrs = [Snackbar.Color.info; Snackbar.Variant.standard]
) |> ignore"""

  let private undoExample (snackbars: SnackbarController) =
    let reportInTrash = Var.Create false
    let restoreReport () = Var.Set reportInTrash false

    let deleteReport () =
      Var.Set reportInTrash true

      snackbars.show (
        "Quarterly report moved to trash.",
        action = SnackbarAction.create ("Undo", restoreReport)
      )
      |> ignore

    let content =
      Card.create (
        Doc.Concat [
          CardHeader.create (
            text "Quarterly report.pdf",
            subtitle = text "Documents / Reports",
            leading = decorativeIcon (Icon.UiActions UiActions.Delete) Typography.Color.secondary,
            actions = Chip.create (text "PDF", attrs = [ Chip.Variant.outlined ])
          )
          CardContent.create (
            div [
              Typography.body2
              Attr.Create "role" "status"
              Attr.Create "data-testid" "report-state"
            ] [
              Doc.TextView(
                reportInTrash.View
                |> View.Map(fun deleted ->
                  if deleted then
                    "In trash. Use Undo in the notification to restore this report."
                  else
                    "In your workspace. Try deleting the report, then bring it back.")
              )
            ]
          )
          CardActions.create (
            Button.error (
              text "Delete report",
              onClick = deleteReport,
              enabled = (reportInTrash.View |> View.Map not),
              attrs = [ Button.Variant.outlined ]
            )
          )
        ]
      )

    Helpers.codeSampleSection
      "Undo a Delete"
      (Helpers.bodyText
        "The file card changes with the action. Notifications with an action remain visible by default, so Undo is available until you act or dismiss the message.")
      content
      """let reportInTrash = Var.Create false

let restoreReport () =
    Var.Set reportInTrash false

let deleteReport () =
    Var.Set reportInTrash true
    snackbars.show(
        "Quarterly report moved to trash.",
        action = SnackbarAction.create("Undo", restoreReport)
    ) |> ignore

Button.error(
    text "Delete report",
    onClick = deleteReport,
    enabled = (reportInTrash.View |> View.Map not),
    attrs = [Button.Variant.outlined]
)"""

  let private uploadExample (snackbars: SnackbarController) =
    let uploadComplete = Var.Create false

    let showUploadSummary () =
      Var.Set uploadComplete true

      snackbars.add (
        div [] [
          strong [] [ text "Report bundle uploaded" ]
          div [ Typography.caption ] [ text "3 files are ready to share with your team." ]
        ],
        key = "report-upload",
        icon = decorativeIcon (Icon.UiActions UiActions.CheckCircle) Attr.Empty,
        attrs = [ Snackbar.Color.success; Snackbar.Variant.outlined ]
      )
      |> ignore

    let content =
      Card.create (
        Doc.Concat [
          CardHeader.create (
            text "Report bundle",
            subtitle = text "Quarterly report, charts, and notes",
            leading = decorativeIcon (Icon.UiActions UiActions.Upload) Typography.Color.tertiary
          )
          CardContent.create (
            div [] [
              buttonRow [
                Chip.create (text "3 files", attrs = [ Chip.Variant.outlined ])
                Chip.create (text "4.8 MB", attrs = [ Chip.Variant.outlined ])
              ]
              div [
                Typography.body2
                Typography.Color.textSecondary
                Margin.Top.extraSmall
                Attr.Create "role" "status"
              ] [
                Doc.TextView(
                  uploadComplete.View
                  |> View.Map(fun complete ->
                    if complete then
                      "Uploaded in this demo. Ready to share."
                    else
                      "Ready for a demo upload.")
                )
              ]
            ]
          )
          CardActions.create (
            Button.tertiary (
              buttonLabel (Icon.UiActions UiActions.Upload) "Upload reports",
              onClick = showUploadSummary,
              attrs = [ Button.Variant.filled ]
            )
          )
        ]
      )

    Helpers.codeSampleSection
      "Custom Content"
      (Helpers.bodyText
        "Use add for a richer message: a title, a second line, and a leading icon. A stable key keeps repeated uploads from stacking the same summary.")
      content
      """let showUploadSummary () =
    let summary = div [] [
        strong [] [text "Report bundle uploaded"]
        div [Typography.caption] [
            text "3 files are ready to share with your team."
        ]
    ]

    snackbars.add(
        summary,
        key = "report-upload",
        attrs = [Snackbar.Color.success; Snackbar.Variant.outlined]
    ) |> ignore

Button.tertiary(text "Upload reports", onClick = showUploadSummary,
    attrs = [Button.Variant.filled])"""

  let private queueExample (snackbars: SnackbarController) =
    let runQueueDemo () =
      snackbars.clear ()

      for message in
        [
          "Design review ready"
          "Build finished"
          "Preview published"
          "Release tagged"
        ] do
        snackbars.show (
          message,
          dismissal = Snackbar.Dismissal.Manual,
          attrs = [ Snackbar.Color.info; Snackbar.Variant.outlined ]
        )
        |> ignore

    let showTimedMessage () =
      snackbars.clear ()

      snackbars.show ("Preview link copied.", dismissal = Snackbar.Dismissal.After 3000)
      |> ignore

    let content =
      div [] [
        buttonRow [
          Chip.create (text "4 events", attrs = [ Chip.Variant.outlined; Chip.Color.info ])
          Chip.create (text "3 visible", attrs = [ Chip.Variant.outlined ])
          Chip.create (text "1 waiting", attrs = [ Chip.Variant.outlined ])
        ]
        div [ Typography.body2; Typography.Color.textSecondary; Margin.Vertical.extraSmall ] [
          text "Dismiss Design review ready to let Release tagged take its place."
        ]
        buttonRow [
          Button.info (text "Run queue demo", onClick = runQueueDemo, attrs = [ Button.Variant.filled ])
          Button.create (
            text "Show timed message",
            onClick = showTimedMessage,
            attrs = [ Button.Variant.outlined ]
          )
          Button.create (text "Clear all", onClick = snackbars.clear, attrs = [ Button.Variant.outlined ])
        ]
      ]

    Helpers.codeSampleSection
      "Queue and Dismissal"
      (Helpers.bodyText
        "These demos clear earlier messages first. Run queue demo sends four updates: three appear and the fourth waits. Manual keeps them in place for inspection. Show timed message switches to a single notification that expires after three seconds.")
      content
      """let runQueueDemo () =
    snackbars.clear()

    for message in [
        "Design review ready"
        "Build finished"
        "Preview published"
        "Release tagged"
    ] do
        snackbars.show(
            message,
            dismissal = Snackbar.Dismissal.Manual
        ) |> ignore

let showTimedMessage () =
    snackbars.clear()
    snackbars.show(
        "Preview link copied.",
        dismissal = Snackbar.Dismissal.After 3000
    ) |> ignore

div [] [
    Button.info(text "Run queue demo", onClick = runQueueDemo)
    Button.create(text "Show timed message", onClick = showTimedMessage)
]"""

  let private positions = [
    "Top left", "top-left"
    "Top center", "top-center"
    "Top right", "top-right"
    "Bottom left", "bottom-left"
    "Bottom center", "bottom-center"
    "Bottom right", "bottom-right"
  ]

  let private hostPositionAttrs (selectedPosition: Var<string>) =
    positions
    |> List.map (fun (_, position) ->
      Attr.DynamicClassPred
        ("weave-snackbar-host--" + position)
        (selectedPosition.View |> View.Map((=) position)))

  let private positionExample (snackbars: SnackbarController) (selectedPosition: Var<string>) =
    let previewPosition label position =
      Var.Set selectedPosition position
      snackbars.clear ()

      snackbars.show (
        label + " notification",
        dismissal = Snackbar.Dismissal.After 4000,
        attrs = [ Snackbar.Color.info; Snackbar.Variant.outlined ]
      )
      |> ignore

    let positionButton (label: string, position: string) =
      let isSelected = selectedPosition.View |> View.Map((=) position)

      GridItem.create (
        Button.primary (
          span [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; AlignItems.center ] [
            for word in label.Split(' ') do
              span [] [ text word ]
          ],
          onClick = (fun () -> previewPosition label position),
          attrs = [
            Button.Width.full
            Attr.Create "aria-label" label
            Attr.Dynamic
              "aria-pressed"
              (isSelected |> View.Map(fun value -> if value then "true" else "false"))
            Attr.DynamicClassPred "weave-button--filled" isSelected
            Attr.DynamicClassPred "weave-button--outlined" (isSelected |> View.Map not)
          ]
        ),
        attrs = [ GridItem.Span.four ]
      )

    let content =
      Card.create (
        Doc.Concat [
          CardHeader.create (
            text "Choose a position",
            subtitle = text "Select any position to send a fresh preview",
            leading = decorativeIcon (Icon.Action Action.Info) Typography.Color.info
          )
          CardContent.create (
            div [
              SurfaceColor.BackgroundColor.background
              BorderRadius.All.small
              Padding.All.extraSmall
            ] [
              Grid.create (
                [
                  yield! positions |> List.take 3 |> List.map positionButton
                  GridItem.create (
                    span [ Typography.caption; Typography.Color.textSecondary; Padding.Vertical.small ] [
                      text "Your app viewport"
                    ],
                    attrs = [ GridItem.Span.twelve ]
                  )
                  yield! positions |> List.skip 3 |> List.map positionButton
                ],
                attrs = [ Grid.Spacing.small ]
              )
            ]
          )
        ]
      )

    Helpers.codeSampleSection
      "Position"
      (Helpers.bodyText
        "See all six positions at a glance. Each choice moves the host and replaces earlier demo messages with a new preview. Position attributes belong on the host, so the whole notification stack moves together.")
      content
      """let selectedPosition = Var.Create "bottom-right"
let positions = [
    "Top left", "top-left"
    "Top center", "top-center"
    "Top right", "top-right"
    "Bottom left", "bottom-left"
    "Bottom center", "bottom-center"
    "Bottom right", "bottom-right"
]

let previewPosition label position =
    Var.Set selectedPosition position
    snackbars.clear()
    snackbars.show(label + " notification") |> ignore

let positionButtons =
    positions |> List.map (fun (label, position) ->
        Button.primary(text label,
            onClick = (fun () -> previewPosition label position),
            attrs = [Button.Variant.outlined]))

let hostPositionAttrs =
    positions |> List.map (fun (_, position) ->
        Attr.DynamicClassPred ("weave-snackbar-host--" + position)
            (selectedPosition.View |> View.Map ((=) position)))

// Use this in place of the host from Save Feedback.
div [] [
    div [] positionButtons
    Snackbar.create(snackbars, attrs = hostPositionAttrs)
]"""

  let render () =
    let snackbars = SnackbarController.create ()
    let selectedPosition = Var.Create "bottom-right"

    Container.create (
      div [] [
        div [ Margin.Bottom.small; Attr.Create "data-testid" "snackbar-introduction" ] [
          Helpers.pageTitle "Snackbar"
          Helpers.bodyText
            "Small updates that keep you in the flow: save a draft, undo a delete, or share an upload. These demo actions update the cards locally. Messages pause while hovered or focused; every message has a dismiss button."
        ]
        saveExample snackbars
        toneExamples snackbars
        undoExample snackbars
        uploadExample snackbars
        queueExample snackbars
        positionExample snackbars selectedPosition
        apiReference ()
        Snackbar.create (snackbars, attrs = hostPositionAttrs selectedPosition)
      ],
      attrs = [ Container.MaxWidth.large ]
    )
