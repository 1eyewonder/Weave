namespace Weave.Docs

open WebSharper
open WebSharper.UI
open WebSharper.AspNetCore
open WebSharper.UI.Client
open Weave.Docs.Examples

[<JavaScript>]
module Client =

  [<SPAEntryPoint>]
  let Main () =
    DocsTheme.initialize ()

    // Render the examples router
    ExamplesRouter.render () |> Doc.RunById "main"
