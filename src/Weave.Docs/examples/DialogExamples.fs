namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module DialogExamples =

  let cancel dialog =
    Button.error (
      text "Cancel",
      onClick = (fun () -> Var.Set dialog false),
      attrs = [ Button.Variant.filled ]
    )

  let confirm dialog =
    Button.primary (
      text "Confirm",
      onClick = (fun () -> Var.Set dialog false),
      attrs = [ Button.Variant.filled; Margin.Right.extraSmall ]
    )

  let private basicDialogExample () =
    let dialogVisible = Var.Create false

    let dialogContent dialog =
      div [] [
        div [ Typography.body1 ] [ text "This is a basic dialog with title and content." ]
        div [ Margin.Top.small ] [ confirm dialog; cancel dialog ]
      ]

    let description =
      Helpers.bodyText
        "A basic dialog controlled by a Var<bool>. Interaction defaults to Force, so the user must act on the dialog to dismiss it."

    let content =
      div [] [
        Button.primary (
          text "Open Dialog",
          onClick = (fun () -> Var.Set dialogVisible true),
          attrs = [ Button.Variant.filled ]
        )
        dialogVisible.View
        |> Doc.BindView(fun isOpen ->
          if isOpen then
            Dialog.create (
              DialogTitle.create (div [ Typography.h6 ] [ text "Dialog Title" ]),
              DialogContent.create (dialogContent dialogVisible)
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
        Dialog.create(
            DialogTitle.create(div [ Typography.h6 ] [ text "Dialog Title" ]),
            DialogContent.create(
                div [] [
                    div [ Typography.body1 ] [ text "This is a basic dialog with title and content." ]
                    Button.create(
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
        div [ Typography.body1 ] [ text "This dialog can be dismissed by clicking outside." ]
        div [ Margin.Top.small ] [ confirm dialog; cancel dialog ]
      ]

    let description =
      Helpers.bodyText
        "Pass Dialog.Interaction.Optional to allow dismissal by clicking outside the dialog window."

    let content =
      div [] [
        Button.secondary (
          text "Open Optional Dialog",
          onClick = (fun () -> Var.Set dialogVisible true),
          attrs = [ Button.Variant.filled ]
        )
        dialogVisible.View
        |> Doc.BindView(fun isOpen ->
          if isOpen then
            Dialog.create (
              DialogTitle.create (div [ Typography.h6 ] [ text "Optional Dialog" ]),
              DialogContent.create (dialogContent dialogVisible),
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

Dialog.create(
    DialogTitle.create(div [ Typography.h6 ] [ text "Optional Dialog" ]),
    DialogContent.create(
        div [] [ div [ Typography.body1 ] [ text "Click outside to dismiss." ] ]
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
        div [ Typography.body1 ] [ text "This dialog demonstrates different positions." ]
        div [ Margin.Top.small ] [
          Button.primary (
            text "Close",
            onClick = (fun () -> Var.Set dialog None),
            attrs = [ Button.Variant.filled ]
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
        Button.secondary (
          text label,
          onClick = (fun () -> Var.Set dialogVisible (Some pos)),
          attrs = [ Button.Variant.filled ]
        ))

    let description =
      Helpers.bodyText "Pass a DialogPosition value to control where the dialog appears on screen."

    let content =
      Grid.create (
        [
          dialogVisible.View
          |> Doc.BindView (function
            | Some pos ->
              Dialog.create (
                DialogTitle.create (div [ Typography.h6 ] [ text (sprintf "%A Dialog" pos) ]),
                DialogContent.create (dialogContent dialogVisible),
                dialogPosition = View.Const pos
              )
            | None -> Doc.Empty)
          yield! positionButtons |> List.map (fun btn -> GridItem.create (btn))
        ]
      )

    let code =
      """open Weave
open Weave.Dialog
open WebSharper.UI

Dialog.create(
    DialogTitle.create(div [ Typography.h6 ] [ text "Top Center Dialog" ]),
    DialogContent.create(
        div [] [ div [ Typography.body1 ] [ text "Anchored to the top center." ] ]
    ),
    dialogPosition = View.Const DialogPosition.TopCenter // see here
)

Dialog.create(
    DialogTitle.create(div [ Typography.h6 ] [ text "Bottom Center Dialog" ]),
    DialogContent.create(
        div [] [ div [ Typography.body1 ] [ text "Anchored to the bottom center." ] ]
    ),
    dialogPosition = View.Const DialogPosition.BottomCenter // see here
)
"""

    Helpers.codeSampleSection "Dialog Positions" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Dialog"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Dialogs are used to display important information or request user input in a modal window."
        ]
        Helpers.divider ()
        basicDialogExample ()
        Helpers.divider ()
        optionalDialogExample ()
        Helpers.divider ()
        positionDialogExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
