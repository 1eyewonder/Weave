namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.Theming
open WebSharper.JavaScript

/// <summary>
/// Examples of how to use the Theming system
/// </summary>
[<JavaScript>]
module ThemingExamples =

  // =============================================================================
  // EXAMPLE 1: Simple Light/Dark Toggle
  // =============================================================================

  let simpleToggle () =
    let currentMode = Var.Create Light

    let toggle () =
      let newMode = toggleMode ()
      Var.Set currentMode newMode

    button [ on.click (fun _ _ -> toggle ()) ] [
      currentMode.View.Map(fun mode ->
        match mode with
        | Light -> "🌙 Switch to Dark"
        | Dark -> "☀️ Switch to Light")
      |> Doc.TextView
    ]

  // =============================================================================
  // EXAMPLE 2: Using Default Theme
  // =============================================================================

  let initializeDefaultTheme () =
    // Just set the mode - CSS defaults will be used
    setMode Light

  // =============================================================================
  // EXAMPLE 3: Custom Brand Colors
  // =============================================================================

  let initializeCustomBrandColors () =
    // Create custom theme with your brand colors
    let customTheme =
      ThemeBuilder.weaveDesign
      |> ThemeBuilder.withPrimary
        (PaletteColor.create "#6200ea" "#ffffff" "#7c4dff" "#4a00b8" "rgb(98, 0, 234)")
        (PaletteColor.create "#b388ff" "rgba(0,0,0,0.87)" "#d1b3ff" "#8e5fff" "rgb(179, 136, 255)")

    // Initialize with light mode
    initialize customTheme Light

  // =============================================================================
  // EXAMPLE 4: Full Custom Theme
  // =============================================================================

  let initializeFullCustomTheme () =
    let customTheme = {
      LightPalette = {
        ThemePalette.empty with
            Background = Some "#fafafa"
            BackgroundPaper = Some "#ffffff"
            BackgroundDarken = Some "#f5f5f5"
            TextPrimary = Some "rgba(0, 0, 0, 0.87)"
            TextSecondary = Some "rgba(0, 0, 0, 0.6)"
            Primary = Some(PaletteColor.create "#6200ea" "#ffffff" "#7c4dff" "#4a00b8" "rgb(98, 0, 234)")
            Secondary =
              Some(PaletteColor.create "#00bfa5" "rgba(0,0,0,0.87)" "#33ccb8" "#008e76" "rgb(0, 191, 165)")
            Success = Some(PaletteColor.create "#4caf50" "#ffffff" "#6fbf73" "#388e3c" "rgb(76, 175, 80)")
            Error = Some(PaletteColor.create "#f44336" "#ffffff" "#f6685e" "#d32f2f" "rgb(244, 67, 54)")
            Warning = Some(PaletteColor.create "#ff9800" "#ffffff" "#ffac33" "#c77700" "rgb(255, 152, 0)")
            Info = Some(PaletteColor.create "#2196f3" "#ffffff" "#4dabf5" "#1976d2" "rgb(33, 150, 243)")
      }
      DarkPalette = {
        ThemePalette.empty with
            Background = Some "#121212"
            BackgroundPaper = Some "#1e1e1e"
            BackgroundDarken = Some "#181818"
            TextPrimary = Some "rgba(255, 255, 255, 0.87)"
            TextSecondary = Some "rgba(255, 255, 255, 0.6)"
            Primary =
              Some(PaletteColor.create "#b388ff" "rgba(0,0,0,0.87)" "#d1b3ff" "#8e5fff" "rgb(179, 136, 255)")
            Secondary =
              Some(PaletteColor.create "#64ffda" "rgba(0,0,0,0.87)" "#8cffe5" "#3dccab" "rgb(100, 255, 218)")
      }
      Typography = {
        FontFamily = Some "'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif"
        FontSize = Some "16px"
        FontWeight = Some "400"
        LineHeight = Some "1.5"
        LetterSpacing = Some "0.00938em"
      }
    }

    initialize customTheme Light

  // =============================================================================
  // EXAMPLE 5: Theme Builder Pattern
  // =============================================================================

  let initializeWithBuilder () =
    let myTheme =
      ThemeBuilder.empty
      |> ThemeBuilder.withLightBackground "#ffffff" "#f5f5f5" "#fafafa"
      |> ThemeBuilder.withDarkBackground "#121212" "#1e1e1e" "#181818"
      |> ThemeBuilder.withPrimary
        (PaletteColor.create "#1976d2" "#ffffff" "#42a5f5" "#1565c0" "rgb(25, 118, 210)")
        (PaletteColor.create "#90caf9" "rgba(0,0,0,0.87)" "#b3d9fb" "#5fa8e6" "rgb(144, 202, 249)")
      |> ThemeBuilder.withTypography {
        FontFamily = Some "'Roboto', sans-serif"
        FontSize = Some "14px"
        FontWeight = Some "400"
        LineHeight = Some "1.43"
        LetterSpacing = Some "0.01071em"
      }

    initialize myTheme Light

  // =============================================================================
  // EXAMPLE 6: System Preference Detection
  // =============================================================================

  let initializeWithSystemPreference () =
    let theme = ThemeConfig.weaveDesign
    let detectedMode = initializeWithSystemPreference theme

    printfn "Detected system preference: %A" detectedMode

  // =============================================================================
  // EXAMPLE 7: Theme Switcher Component
  // =============================================================================

  let themeSwitcher (config: ThemeConfig) =
    let currentMode = Var.Create(getMode ())

    let switchTheme newMode =
      applyTheme config newMode
      currentMode.Value <- newMode

    div [] [
      button [ on.click (fun _ _ -> switchTheme Light) ] [ text "Light" ]

      button [ on.click (fun _ _ -> switchTheme Dark) ] [ text "Dark" ]

      div [] [
        text "Current mode: "
        currentMode.View.Map(fun mode ->
          match mode with
          | Light -> "Light"
          | Dark -> "Dark")
        |> Doc.TextView
      ]
    ]

  // =============================================================================
  // EXAMPLE 8: Persistent Theme (with localStorage)
  // =============================================================================

  let initializeWithPersistence () =
    let theme = ThemeConfig.weaveDesign

    // Try to load saved preference
    let savedMode = JS.Window.LocalStorage.GetItem("theme-mode")
    let initialMode = if savedMode = "dark" then Dark else Light

    initialize theme initialMode

    // Save preference when changed
    let switchAndSave newMode =
      applyTheme theme newMode

      let modeStr =
        match newMode with
        | Light -> "light"
        | Dark -> "dark"

      JS.Window.LocalStorage.SetItem("theme-mode", modeStr)

    switchAndSave

  // =============================================================================
  // EXAMPLE 9: Multiple Theme Variants
  // =============================================================================

  module ThemeVariants =

    let oceanBlue =
      ThemeBuilder.weaveDesign
      |> ThemeBuilder.withPrimary
        (PaletteColor.create "#0277bd" "#ffffff" "#0291d6" "#01598a" "rgb(2, 119, 189)")
        (PaletteColor.create "#4fc3f7" "rgba(0,0,0,0.87)" "#7ad3f9" "#29a9d6" "rgb(79, 195, 247)")

    let forestGreen =
      ThemeBuilder.weaveDesign
      |> ThemeBuilder.withPrimary
        (PaletteColor.create "#388e3c" "#ffffff" "#5fa463" "#2c6b2f" "rgb(56, 142, 60)")
        (PaletteColor.create "#81c784" "rgba(0,0,0,0.87)" "#a0d6a3" "#5da961" "rgb(129, 199, 132)")

    let sunsetOrange =
      ThemeBuilder.weaveDesign
      |> ThemeBuilder.withPrimary
        (PaletteColor.create "#f57c00" "#ffffff" "#ff9100" "#c46200" "rgb(245, 124, 0)")
        (PaletteColor.create "#ffb74d" "rgba(0,0,0,0.87)" "#ffc670" "#d99528" "rgb(255, 183, 77)")

    let applyVariant variantName mode =
      let theme =
        match variantName with
        | "ocean" -> oceanBlue
        | "forest" -> forestGreen
        | "sunset" -> sunsetOrange
        | _ -> ThemeConfig.weaveDesign

      applyTheme theme mode

  // =============================================================================
  // EXAMPLE 10: Real-world Integration in SPAEntryPoint
  // =============================================================================

  //[<SPAEntryPoint>]
  let MainWithTheming () =
    // Initialize theme on app startup
    let customTheme =
      ThemeBuilder.weaveDesign
      |> ThemeBuilder.withPrimary
        (PaletteColor.create "#1976d2" "#ffffff" "#42a5f5" "#1565c0" "rgb(25, 118, 210)")
        (PaletteColor.create "#90caf9" "rgba(0,0,0,0.87)" "#b3d9fb" "#5fa8e6" "rgb(144, 202, 249)")

    let savedMode = JS.Window.LocalStorage.GetItem("theme-mode")

    let initialMode =
      if savedMode = "dark" then Dark
      else if savedMode = "light" then Light
      else detectSystemPreference ()

    initialize customTheme initialMode

    let themeToggle = simpleToggle ()

    div [] [ themeToggle; text "Your app content here..." ] |> Doc.RunById "main"
