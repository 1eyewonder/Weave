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
      ]
    )
