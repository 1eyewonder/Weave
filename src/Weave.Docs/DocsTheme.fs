namespace Weave.Docs

open WebSharper
open WebSharper.JavaScript
open Weave.Theming

/// Documentation preference; the library itself only selects a CSS mode.
[<JavaScript>]
module DocsTheme =

  [<Literal>]
  let private preferenceKey = "weave-docs-theme"

  let initialize () =
    let preferred =
      try
        match JS.Window.LocalStorage.GetItem preferenceKey with
        | "light" -> Light
        | "dark" -> Dark
        | _ -> detectSystemPreference ()
      with _ ->
        detectSystemPreference ()

    setMode preferred

  let toggle () =
    let selected = toggleMode ()

    try
      JS.Window.LocalStorage.SetItem(preferenceKey, if selected = Light then "light" else "dark")
    with _ ->
      ()
