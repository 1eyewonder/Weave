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
    State.create () |> Views.render |> Doc.RunById "main"
