namespace WeaveStarter

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Theming

[<JavaScript>]
module Client =
  [<SPAEntryPoint>]
  let Main () =
    setMode Light
    let mode = Var.Create Light
    let snackbars = SnackbarController.create ()

    let sayHello () =
      snackbars.show "Hello from Weave!" |> ignore

    Container.create (
      div [] [
        button [
          Attr.Class "theme-toggle"
          Attr.Create "type" "button"
          on.click (fun _ _ -> Var.Set mode (toggleMode ()))
        ] [
          mode.View
          |> View.Map(fun current -> if current = Light then "Dark mode" else "Light mode")
          |> Doc.TextView
        ]
        h1 [] [ text "Welcome to Weave" ]
        p [] [ text "Your WebSharper application is ready to customize." ]
        Button.primary (text "Say hello", onClick = sayHello)
        Snackbar.create snackbars
      ],
      attrs = [ Container.MaxWidth.medium ]
    )
    |> Doc.RunById "main"
