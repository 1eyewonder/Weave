namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module DialogExamples =

  let cancel dialog =
    Button.Create(
      text "Cancel",
      onClick = (fun () -> Var.Set dialog false),
      attrs = [ Button.Color.error; Button.Variant.filled ]
    )

  let confirm dialog =
    Button.Create(
      text "Confirm",
      onClick = (fun () -> Var.Set dialog false),
      attrs = [
        Button.Color.primary
        Button.Variant.filled
        Margin.toClasses Margin.Right.extraSmall |> cls
      ]
    )

  let private basicDialogExample () =
    let dialogVisible = Var.Create false

    let dialogContent dialog =
      div [] [
        Body1.Div("This is a basic dialog with title and content.")
        div [ Margin.toClasses Margin.Top.small |> cls ] [ confirm dialog; cancel dialog ]
      ]

    let description =
      Helpers.bodyText
        "A basic dialog controlled by a Var<bool>. Interaction defaults to Force, so the user must act on the dialog to dismiss it."

    let content =
      div [] [
        Button.Create(
          text "Open Dialog",
          onClick = (fun () -> Var.Set dialogVisible true),
          attrs = [ Button.Color.primary; Button.Variant.filled ]
        )
        dialogVisible.View
        |> Doc.BindView(fun isOpen ->
          if isOpen then
            Dialog.Create(
              DialogTitle.Create(H6.Div("Dialog Title")),
              DialogContent.Create(dialogContent dialogVisible)
            )
          else
            Doc.Empty)
      ]

    let code =
      """open Weave
open WebSharper.UI

let dialogVisible = Var.Create false // see here

dialogVisible.View
|> Doc.BindView(fun isOpen ->
    if isOpen then
        Dialog.Create(
            DialogTitle.Create(H6.Div("Dialog Title")),
            DialogContent.Create(
                div [] [
                    Body1.Div("This is a basic dialog with title and content.")
                    Button.Create(
                        text "Close",
                        onClick = (fun () -> Var.Set dialogVisible false)
                    )
                ]
            )
        )
    else
        Doc.Empty)
"""

    Helpers.codeSampleSection "Basic Dialog" description content code

  let private optionalDialogExample () =
    let dialogVisible = Var.Create false

    let dialogContent dialog =
      div [] [
        Body1.Div("This dialog can be dismissed by clicking outside.")
        div [ Margin.toClasses Margin.Top.small |> cls ] [ confirm dialog; cancel dialog ]
      ]

    let description =
      Helpers.bodyText
        "Pass Dialog.Interaction.Optional to allow dismissal by clicking outside the dialog window."

    let content =
      div [] [
        Button.Create(
          text "Open Optional Dialog",
          onClick = (fun () -> Var.Set dialogVisible true),
          attrs = [ Button.Color.secondary; Button.Variant.filled ]
        )
        dialogVisible.View
        |> Doc.BindView(fun isOpen ->
          if isOpen then
            Dialog.Create(
              DialogTitle.Create(H6.Div("Optional Dialog")),
              DialogContent.Create(dialogContent dialogVisible),
              dialogInteraction =
                View.Const(Dialog.Interaction.Optional(fun () -> Var.Set dialogVisible false))
            )
          else
            Doc.Empty)
      ]

    let code =
      """open Weave
open Weave.Dialog
open WebSharper.UI

let dialogVisible = Var.Create false

Dialog.Create(
    DialogTitle.Create(H6.Div("Optional Dialog")),
    DialogContent.Create(
        div [] [ Body1.Div("Click outside to dismiss.") ]
    ),
    dialogInteraction = // see here
        View.Const(
            Interaction.Optional(fun () -> Var.Set dialogVisible false)
        )
)
"""

    Helpers.codeSampleSection "Optional Dialog" description content code

  let private positionDialogExample () =
    let dialogVisible = Var.Create None

    let dialogContent dialog =
      div [] [
        Body1.Div("This dialog demonstrates different positions.")
        div [ Margin.toClasses Margin.Top.small |> cls ] [
          Button.Create(
            text "Close",
            onClick = (fun () -> Var.Set dialog None),
            attrs = [ Button.Color.primary; Button.Variant.filled ]
          )
        ]
      ]

    let positionButtons =
      [
        Dialog.DialogPosition.Center, "Center"
        Dialog.DialogPosition.TopCenter, "Top Center"
        Dialog.DialogPosition.BottomCenter, "Bottom Center"
        Dialog.DialogPosition.CenterRight, "Center Right"
        Dialog.DialogPosition.CenterLeft, "Center Left"
      ]
      |> List.map (fun (pos, label) ->
        Button.Create(
          text label,
          onClick = (fun () -> Var.Set dialogVisible (Some pos)),
          attrs = [ Button.Color.secondary; Button.Variant.filled ]
        ))

    let description =
      Helpers.bodyText "Pass a DialogPosition value to control where the dialog appears on screen."

    let content =
      Grid.Create(
        [
          dialogVisible.View
          |> Doc.BindView (function
            | Some pos ->
              Dialog.Create(
                DialogTitle.Create(H6.Div(sprintf "%A Dialog" pos)),
                DialogContent.Create(dialogContent dialogVisible),
                dialogPosition = View.Const pos
              )
            | None -> Doc.Empty)
          yield! positionButtons |> List.map (fun btn -> GridItem.Create(btn))
        ]
      )

    let code =
      """open Weave
open Weave.Dialog
open WebSharper.UI

Dialog.Create(
    DialogTitle.Create(H6.Div("Top Center Dialog")),
    DialogContent.Create(
        div [] [ Body1.Div("Anchored to the top center.") ]
    ),
    dialogPosition = View.Const DialogPosition.TopCenter // see here
)

Dialog.Create(
    DialogTitle.Create(H6.Div("Bottom Center Dialog")),
    DialogContent.Create(
        div [] [ Body1.Div("Anchored to the bottom center.") ]
    ),
    dialogPosition = View.Const DialogPosition.BottomCenter // see here
)
"""

    Helpers.codeSampleSection "Dialog Positions" description content code

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Dialog"
        Body1.Div(
          "Dialogs are used to display important information or request user input in a modal window.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Helpers.divider ()
        basicDialogExample ()
        Helpers.divider ()
        optionalDialogExample ()
        Helpers.divider ()
        positionDialogExample ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
