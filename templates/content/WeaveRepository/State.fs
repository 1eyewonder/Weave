namespace WeaveStarter

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Theming

open Domain

[<JavaScript>]
module State =
  type Model = {
    Page: Var<string>
    Branch: Var<string option>
    Folder: Var<string>
    SelectedFile: Var<Entry option>
    StateFilter: Var<string option>
    Query: Var<string>
    SelectedIssue: Var<Issue option>
  }

  let navigateFolder (state: Model) path =
    Var.Set state.Folder path
    Var.Set state.SelectedFile None

  let switchPage (state: Model) value =
    Var.Set state.Page value
    Var.Set state.SelectedIssue None

  let create () =
    let state = {
      Page = Var.Create "Code"
      Branch = Var.Create(Some "main")
      Folder = Var.Create ""
      SelectedFile = Var.Create None
      StateFilter = Var.Create(Some "Open")
      Query = Var.Create ""
      SelectedIssue = Var.Create None
    }
    // A branch change returns to its root, so files always belong to that branch.
    state.Branch.View
    |> View.Sink(fun _ ->
      if state.Folder.Value <> "" || state.SelectedFile.Value.IsSome then
        navigateFolder state "")

    state
