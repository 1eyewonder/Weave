namespace Weave

open WebSharper
open WebSharper.JavaScript

/// Helpers for selecting the CSS light or dark theme.
[<JavaScript>]
module Theming =

  [<Struct>]
  type ThemeMode =
    | Light
    | Dark

  /// Selects the theme on the document body. Palette and typography values come from CSS.
  let setMode (mode: ThemeMode) =
    let modeStr =
      match mode with
      | Light -> "light"
      | Dark -> "dark"

    JS.Document.Body.SetAttribute("data-theme", modeStr)

  /// Returns Dark when the body selects dark; otherwise returns Light.
  let getMode () =
    let currentMode = JS.Document.Body.GetAttribute("data-theme")
    if currentMode = "dark" then Dark else Light

  /// Switches the selected CSS mode and returns the new mode.
  let toggleMode () =
    let newMode =
      match getMode () with
      | Light -> Dark
      | Dark -> Light

    setMode newMode
    newMode

  /// Reads the current OS color scheme. It does not subscribe to future changes.
  let detectSystemPreference () =
    if JS.Window.MatchMedia("(prefers-color-scheme: dark)").Matches then
      Dark
    else
      Light
