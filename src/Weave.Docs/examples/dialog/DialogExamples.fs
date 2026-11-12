namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module DialogExamples =

  let private basicDialogExample () =
    let description =
      Helpers.bodyText
        "Mount the dialog while a Var<bool> is true. Force is the default interaction: close it through an action inside the dialog."

    let content =
      let isOpen = Var.Create false
      let close () = Var.Set isOpen false

      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
      ] [
        Button.primary (
          text "Open dialog",
          onClick = (fun () -> Var.Set isOpen true),
          attrs = [ Button.Variant.filled ]
        )

        isOpen.View
        |> Doc.BindView(fun openNow ->
          if openNow then
            let actions =
              DialogActions.create (
                Doc.Concat [
                  Button.create (text "Cancel", onClick = close, attrs = [ Button.Variant.outlined ])
                  Button.primary (text "Confirm", onClick = close, attrs = [ Button.Variant.filled ])
                ]
              )

            Dialog.create (
              DialogTitle.create (text "Basic dialog"),
              Doc.Concat [
                DialogContent.create (text "Choose an action to close this dialog.")
                actions
              ]
            )
          else
            Doc.Empty)
      ]

    let code =
      """let isOpen = Var.Create false
let close () = Var.Set isOpen false

Doc.Concat [
  Button.primary (
    text "Open dialog",
    onClick = (fun () -> Var.Set isOpen true),
    attrs = [ Button.Variant.filled ]
  )

  isOpen.View
  |> Doc.BindView(fun openNow ->
    if openNow then
      let actions =
        DialogActions.create (
          Doc.Concat [
            Button.create (
              text "Cancel",
              onClick = close,
              attrs = [ Button.Variant.outlined ]
            )
            Button.primary (
              text "Confirm",
              onClick = close,
              attrs = [ Button.Variant.filled ]
            )
          ]
        )

      Dialog.create (
        DialogTitle.create (text "Basic dialog"),
        Doc.Concat [
          DialogContent.create (
            text "Choose an action to close this dialog."
          )
          actions
        ]
      )
    else
      Doc.Empty)
]"""

    Helpers.codeSampleSection "Basic Dialog" description content code

  let appearanceExample () =
    let position = Var.Create "Center"
    let interaction = Var.Create "Force"
    let blur = Var.Create true
    let isOpen = Var.Create false
    let close () = Var.Set isOpen false

    let choice label values (value: Var<string>) =
      let selected =
        Var.Lens value Some (fun current next -> Option.defaultValue current next)

      let items =
        values
        |> List.map (fun name -> SelectItem.create (text name, name, name))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice "Position" DialogAppearance.positions position
        choice "Interaction" DialogAppearance.interactions interaction
      ]

    let config =
      let layout = (position.View, interaction.View) ||> View.Map2(fun a b -> a, b)
      (layout, blur.View) ||> View.Map2(fun (a, b) c -> a, b, c)

    let preview =
      config
      |> Doc.BindView(fun (position, interaction, blur) ->
        let dialogPosition =
          match position with
          | "Top center" -> Dialog.DialogPosition.TopCenter
          | "Bottom center" -> Dialog.DialogPosition.BottomCenter
          | "Left" -> Dialog.DialogPosition.CenterLeft
          | "Right" -> Dialog.DialogPosition.CenterRight
          | _ -> Dialog.DialogPosition.Center

        let dialogInteraction =
          if interaction = "Optional" then
            Dialog.Interaction.Optional close
          else
            Dialog.Interaction.Force

        Doc.Concat [
          Button.primary (
            text "Open dialog",
            onClick = (fun () -> Var.Set isOpen true),
            attrs = [ Button.Variant.filled ]
          )
          isOpen.View
          |> Doc.BindView(fun openNow ->
            if openNow then
              Dialog.create (
                DialogTitle.create (text "Dialog"),
                Doc.Concat [
                  DialogContent.create (text "Review the details.")
                  DialogActions.create (Button.primary (text "Close", onClick = close))
                ],
                dialogPosition = View.Const dialogPosition,
                dialogInteraction = View.Const dialogInteraction,
                blurBackground = View.Const blur
              )
            else
              Doc.Empty)
        ])

    let source =
      config
      |> View.Map(fun (position, interaction, blur) -> DialogAppearance.source position interaction blur)

    let reset () =
      Var.Set isOpen false
      Var.Set position "Center"
      Var.Set interaction "Force"
      Var.Set blur true

    Helpers.appearanceSection
      [ "optional-dialog"; "dialog-positions" ]
      (Helpers.bodyText
        "Choose a position and interaction, then open the dialog. Force closes through its action; Optional also closes with Escape or the backdrop. Blur controls the backdrop treatment.")
      controls
      (Some(Switch.create (blur, text "Blur background")))
      preview
      reset
      source

  let private apiReferenceSection () =
    Helpers.apiSection
      (Helpers.bodyText "Complete API reference for Dialog, DialogTitle, DialogContent, and DialogActions.")
      [
        Helpers.apiTable "Dialog.create" [
          Helpers.apiParam "title" "Doc" "" "Title content — typically built with DialogTitle.create"
          Helpers.apiParam "content" "Doc" "" "Body content — typically built with DialogContent.create"
          Helpers.apiParam
            "?blurBackground"
            "View<bool>"
            "View.Const true"
            "Whether the backdrop uses a blur filter"
          Helpers.apiParam
            "?dialogPosition"
            "View<DialogPosition>"
            "View.Const Center"
            "Where the dialog is positioned on screen"
          Helpers.apiParam
            "?dialogInteraction"
            "View<Interaction>"
            "View.Const Force"
            "Whether the dialog can be dismissed by clicking the backdrop or pressing Escape"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the root element"
        ]

        Helpers.apiTable "DialogTitle.create" [
          Helpers.apiParam "content" "Doc" "" "Title content rendered inside the dialog header"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the title element"
        ]

        Helpers.apiTable "DialogContent.create" [
          Helpers.apiParam "content" "Doc" "" "Body content rendered inside the dialog"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the content element"
        ]

        Helpers.apiTable "DialogActions.create" [
          Helpers.apiParam
            "content"
            "Doc"
            ""
            "Actions aligned at the bottom of the dialog; use Doc.Concat for multiple actions"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes applied to the actions element"
        ]

        Helpers.styleModuleTable "Dialog.DialogPosition (DU)" [
          ("Center", "Centered on screen (default)")
          ("TopCenter", "Centered horizontally, near the top")
          ("BottomCenter", "Centered horizontally, near the bottom")
          ("CenterRight", "Centered vertically, on the right")
          ("CenterLeft", "Centered vertically, on the left")
        ]

        Helpers.styleModuleTable "Dialog.Interaction (DU)" [
          ("Force", "User cannot dismiss the dialog — must interact with its content (default)")
          ("Optional of closeDialog",
           "User can dismiss via backdrop click or Escape; provide a close callback")
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Dialog"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Dialogs are used to display important information or request user input in a modal window."
        ]
        p [] [
          text "Use a dialog for a focused decision. For persistent navigation or tools, see "
          a [ attr.href "#drawer" ] [ text "Drawer" ]
          text "."
        ]
        Helpers.divider ()
        basicDialogExample ()
        Helpers.divider ()
        appearanceExample ()
        Helpers.divider ()
        span [ attr.id "when-to-use" ] []
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
