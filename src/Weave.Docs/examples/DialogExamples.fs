namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers

[<JavaScript>]
module DialogExamples =

  let cancel dialog =
    Button.Create(
      text "Cancel",
      onClick = (fun () -> Var.Set dialog false),
      attrs = [
        cls [
          Button.Color.toClass BrandColor.Error
          Button.Variant.toClass Button.Variant.Filled
        ]
      ]
    )

  let confirm dialog =
    Button.Create(
      text "Confirm",
      onClick = (fun () -> Var.Set dialog false),
      attrs = [
        cls [
          Button.Color.toClass BrandColor.Primary
          Button.Variant.toClass Button.Variant.Filled
          yield! Margin.toClasses Margin.Right.extraSmall
        ]
      ]
    )

  let private basicDialogExample () =
    let dialogVisible = Var.Create false

    let content dialog =
      div [] [
        Body1.Create("This is a basic dialog with title and content.")
        div [ Margin.toClasses Margin.Top.small |> cls ] [ confirm dialog; cancel dialog ]
      ]

    div [] [
      Button.Create(
        text "Open Dialog",
        onClick = (fun () -> Var.Set dialogVisible true),
        attrs = [
          cls [
            Button.Color.toClass BrandColor.Primary
            Button.Variant.toClass Button.Variant.Filled
          ]
        ]
      )
      dialogVisible.View
      |> Doc.BindView(fun isOpen ->
        if isOpen then
          Dialog.Create(
            DialogTitle.Create(H6.Create("Dialog Title")),
            DialogContent.Create(content dialogVisible)
          )
        else
          Doc.Empty)
    ]
    |> Helpers.section
      "Basic Dialog"
      (Helpers.bodyText
        "A simple dialog that can be opened and closed. Interaction will be forced by default.")

  let private optionalDialogExample () =
    let dialogVisible = Var.Create false

    let content dialog =
      div [] [
        Body1.Create("This dialog can be dismissed by clicking outside.")
        div [ Margin.toClasses Margin.Top.small |> cls ] [ confirm dialog; cancel dialog ]
      ]

    div [] [
      Button.Create(
        text "Open Optional Dialog",
        onClick = (fun () -> Var.Set dialogVisible true),
        attrs = [
          cls [
            Button.Color.toClass BrandColor.Secondary
            Button.Variant.toClass Button.Variant.Filled
          ]
        ]
      )
      dialogVisible.View
      |> Doc.BindView(fun isOpen ->
        if isOpen then
          Dialog.Create(
            DialogTitle.Create(H6.Create("Optional Dialog")),
            DialogContent.Create(content dialogVisible),
            dialogInteraction =
              View.Const(Dialog.Interaction.Optional(fun () -> Var.Set dialogVisible false))
          )
        else
          Doc.Empty)
    ]
    |> Helpers.section
      "Optional Dialog"
      (Helpers.bodyText
        "A dialog that can be dismissed by clicking outside or pressing Escape. Useful for less critical prompts.")

  let private positionDialogExample () =
    let dialogVisible = Var.Create None

    let content dialog =
      div [] [
        Body1.Create("This dialog demonstrates different positions.")
        div [ Margin.toClasses Margin.Top.small |> cls ] [
          Button.Create(
            text "Close",
            onClick = (fun () -> Var.Set dialog None),
            attrs = [
              cls [
                Button.Color.toClass BrandColor.Primary
                Button.Variant.toClass Button.Variant.Filled
              ]
            ]
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
          attrs = [
            cls [
              Button.Color.toClass BrandColor.Secondary
              Button.Variant.toClass Button.Variant.Filled
            ]
          ]
        ))

    Grid.Create(
      [
        dialogVisible.View
        |> Doc.BindView (function
          | Some pos ->
            Dialog.Create(
              DialogTitle.Create(H6.Create(sprintf "%A Dialog" pos)),
              DialogContent.Create(content dialogVisible),
              dialogPosition = View.Const pos
            )
          | None -> Doc.Empty)
        yield! positionButtons |> List.map (fun btn -> GridItem.Create(btn))
      ]
    )
    |> Helpers.section
      "Dialog Positions"
      (Helpers.bodyText
        "Dialogs can appear in different positions. Use the buttons to open dialogs at each position.")

  let render () =
    Container.Create(
      div [] [
        H1.Create("Dialog Component", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Create(
          "Dialogs are used to display important information or request user input in a modal window.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )
        Helpers.divider ()
        basicDialogExample ()
        Helpers.divider ()
        optionalDialogExample ()
        Helpers.divider ()
        positionDialogExample ()
      ]
    )
