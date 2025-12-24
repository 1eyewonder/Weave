namespace Weave.Docs

open WebSharper
open WebSharper.UI
open WebSharper.AspNetCore
open WebSharper.UI.Client
open Weave.Theming
open Weave.Docs.Examples

[<JavaScript>]
module Client =

  [<SPAEntryPoint>]
  let Main () =
    // Initialize with dark mode
    setMode Dark

    // Render the examples router
    ExamplesRouter.render () |> Doc.RunById "main"
