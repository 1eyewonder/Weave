namespace Weave

open WebSharper
open WebSharper.JavaScript

/// <summary>
/// Theme system for managing light/dark mode and custom color configurations.
/// Inspired by MudBlazor's theming approach but adapted for WebSharper.
/// </summary>
[<JavaScript>]
module Theming =

  [<Struct>]
  type ThemeMode =
    | Light
    | Dark

  type PaletteColor = {
    Main: string
    ContrastText: string
    Lighten: string
    Darken: string
    Rgb: string
  }

  module PaletteColor =

    let create main contrastText lighten darken rgb = {
      Main = main
      ContrastText = contrastText
      Lighten = lighten
      Darken = darken
      Rgb = rgb
    }

  type TypographyConfig = {
    FontFamily: string option
    FontSize: string option
    FontWeight: string option
    LineHeight: string option
    LetterSpacing: string option
  }

  module TypographyConfig =

    let empty = {
      FontFamily = None
      FontSize = None
      FontWeight = None
      LineHeight = None
      LetterSpacing = None
    }

  type ThemePalette = {
    Background: string option
    BackgroundPaper: string option
    BackgroundDarken: string option
    TextPrimary: string option
    TextSecondary: string option
    TextDisabled: string option
    Divider: string option
    Primary: PaletteColor option
    Secondary: PaletteColor option
    Tertiary: PaletteColor option
    Success: PaletteColor option
    Error: PaletteColor option
    Warning: PaletteColor option
    Info: PaletteColor option
    Dark: PaletteColor option
    AppBarBackground: string option
    AppBarText: string option
    DrawerBackground: string option
    DrawerText: string option
  }

  module ThemePalette =

    /// Empty palette (will use CSS defaults)
    let empty = {
      Background = None
      BackgroundPaper = None
      BackgroundDarken = None
      TextPrimary = None
      TextSecondary = None
      TextDisabled = None
      Divider = None
      Primary = None
      Secondary = None
      Tertiary = None
      Success = None
      Error = None
      Warning = None
      Info = None
      Dark = None
      AppBarBackground = None
      AppBarText = None
      DrawerBackground = None
      DrawerText = None
    }

  type ThemeConfig = {
    LightPalette: ThemePalette
    DarkPalette: ThemePalette
    Typography: TypographyConfig
  }

  module ThemeConfig =

    let empty = {
      LightPalette = ThemePalette.empty
      DarkPalette = ThemePalette.empty
      Typography = TypographyConfig.empty
    }

    let weaveDesign = {
      LightPalette = {
        ThemePalette.empty with
            Primary = Some(PaletteColor.create "#0164fc" "#ffffff" "#0187f1" "#002fc6" "1, 70, 176")
            Secondary = Some(PaletteColor.create "#594ae2" "#ffffff" "#7d6ff0" "#3d2eb4" "89, 74, 226")
            Tertiary = Some(PaletteColor.create "#ff4081" "#ffffff" "#ff6699" "#d91d63" "255, 64, 129")
            Info = Some(PaletteColor.create "#1ec8a5" "#ffffff" "#4ed9ba" "#149977" "30, 200, 165")
            Success = Some(PaletteColor.create "#00C853" "#ffffff" "#33d672" "#009638" "0, 200, 83")
            Error = Some(PaletteColor.create "#F44336" "#ffffff" "#f6685e" "#d32f2f" "244, 67, 54")
            Warning = Some(PaletteColor.create "#FF9800" "#ffffff" "#ffac33" "#c77700" "255, 152, 0")
            Dark = Some(PaletteColor.create "#424242" "#ffffff" "#616161" "#212121" "66, 66, 66")
            Background = Some "#ffffff"
            BackgroundPaper = Some "#ffffff"
            BackgroundDarken = Some "#f5f5f5"
            TextPrimary = Some "rgba(0, 0, 0, 0.87)"
            TextSecondary = Some "rgba(0, 0, 0, 0.54)"
            TextDisabled = Some "rgba(0, 0, 0, 0.38)"
            Divider = Some "#e0e0e0"
      }
      DarkPalette = {
        ThemePalette.empty with
            Primary = Some(PaletteColor.create "#088ffd" "#ffffff" "#35d7ff" "#0164fc" "1, 70, 176")
            Secondary = Some(PaletteColor.create "#594ae2" "#ffffff" "#9b92ed" "#4d40c2" "119, 107, 231")
            Tertiary = Some(PaletteColor.create "#ff4081" "#ffffff" "#ff6699" "#d91d63" "255, 64, 129")
            Info = Some(PaletteColor.create "#1ec8a5" "#ffffff" "#4ed9ba" "#149977" "30, 200, 165")
            Success = Some(PaletteColor.create "#0bba83" "#ffffff" "#3dc99a" "#089a6b" "11, 186, 131")
            Error = Some(PaletteColor.create "#f64e62" "#ffffff" "#f77381" "#b53746" "246, 78, 98")
            Warning = Some(PaletteColor.create "#ffa800" "#ffffff" "#ffb933" "#cc8600" "255, 168, 0")
            Dark = Some(PaletteColor.create "#6e6e6e" "#ffffff" "#9e9e9e" "#424242" "110, 110, 110")
            Background = Some "#32333d"
            BackgroundPaper = Some "#373740"
            BackgroundDarken = Some "#27272f"
            TextPrimary = Some "rgba(255, 255, 255, 0.87)"
            TextSecondary = Some "rgba(255, 255, 255, 0.50)"
            TextDisabled = Some "rgba(255, 255, 255, 0.2)"
            Divider = Some "rgba(255, 255, 255, 0.12)"
      }
      Typography = TypographyConfig.empty
    }

  let private setCssVar (varName: string) (value: string) =
    JS.Document.DocumentElement?style?setProperty(varName, value)

  let private applyPaletteColor (prefix: string) (color: PaletteColor) =
    setCssVar $"--palette-%s{prefix}" color.Main
    setCssVar $"--palette-%s{prefix}-text" color.ContrastText
    setCssVar $"--palette-%s{prefix}-lighten" color.Lighten
    setCssVar $"--palette-%s{prefix}-darken" color.Darken
    setCssVar $"--palette-%s{prefix}-rgb" color.Rgb

  let private applyPalette (palette: ThemePalette) =
    palette.Background |> Option.iter (setCssVar "--palette-background")
    palette.BackgroundPaper |> Option.iter (setCssVar "--palette-background-paper")

    palette.BackgroundDarken
    |> Option.iter (setCssVar "--palette-background-darken")

    palette.TextPrimary |> Option.iter (setCssVar "--palette-text-primary")
    palette.TextSecondary |> Option.iter (setCssVar "--palette-text-secondary")
    palette.TextDisabled |> Option.iter (setCssVar "--palette-text-disabled")
    palette.Divider |> Option.iter (setCssVar "--palette-divider")
    palette.Primary |> Option.iter (applyPaletteColor "primary")
    palette.Secondary |> Option.iter (applyPaletteColor "secondary")
    palette.Tertiary |> Option.iter (applyPaletteColor "tertiary")
    palette.Success |> Option.iter (applyPaletteColor "success")
    palette.Error |> Option.iter (applyPaletteColor "error")
    palette.Warning |> Option.iter (applyPaletteColor "warning")
    palette.Info |> Option.iter (applyPaletteColor "info")
    palette.Dark |> Option.iter (applyPaletteColor "dark")

    palette.AppBarBackground
    |> Option.iter (setCssVar "--palette-appbar-background")

    palette.AppBarText |> Option.iter (setCssVar "--palette-appbar-text")

    palette.DrawerBackground
    |> Option.iter (setCssVar "--palette-drawer-background")

    palette.DrawerText |> Option.iter (setCssVar "--palette-drawer-text")
    palette.Background |> Option.iter (setCssVar "--input-label-background")

  let private applyTypography (typography: TypographyConfig) =
    typography.FontFamily |> Option.iter (setCssVar "--typography-default-family")
    typography.FontSize |> Option.iter (setCssVar "--typography-default-size")
    typography.FontWeight |> Option.iter (setCssVar "--typography-default-weight")

    typography.LineHeight
    |> Option.iter (setCssVar "--typography-default-lineheight")

    typography.LetterSpacing
    |> Option.iter (setCssVar "--typography-default-letterspacing")

  let setMode (mode: ThemeMode) =
    let modeStr =
      match mode with
      | Light -> "light"
      | Dark -> "dark"

    JS.Document.Body.SetAttribute("data-theme", modeStr)

  let getMode () =
    let currentMode = JS.Document.Body.GetAttribute("data-theme")
    if currentMode = "dark" then Dark else Light

  let toggleMode () =
    let newMode =
      match getMode () with
      | Light -> Dark
      | Dark -> Light

    setMode newMode
    newMode

  let applyTheme (config: ThemeConfig) (mode: ThemeMode) =
    setMode mode

    let palette =
      match mode with
      | Light -> config.LightPalette
      | Dark -> config.DarkPalette

    applyPalette palette
    applyTypography config.Typography

  let applyThemePreserveMode (config: ThemeConfig) =
    let currentMode = getMode ()
    applyTheme config currentMode

  let initialize (config: ThemeConfig) (initialMode: ThemeMode) = applyTheme config initialMode

  let detectSystemPreference () =
    if JS.Window.MatchMedia("(prefers-color-scheme: dark)").Matches then
      Dark
    else
      Light

  let initializeWithSystemPreference (config: ThemeConfig) =
    let mode = detectSystemPreference ()
    initialize config mode
    mode

  module ThemeBuilder =

    let empty = ThemeConfig.empty
    let weaveDesign = ThemeConfig.weaveDesign

    let withLightPalette palette (config: ThemeConfig) = { config with LightPalette = palette }

    let withDarkPalette palette (config: ThemeConfig) = { config with DarkPalette = palette }

    let withTypography typography (config: ThemeConfig) = { config with Typography = typography }

    let withLightPrimary color (config: ThemeConfig) = {
      config with
          LightPalette = { config.LightPalette with Primary = Some color }
    }

    let withDarkPrimary color (config: ThemeConfig) = {
      config with
          DarkPalette = { config.DarkPalette with Primary = Some color }
    }

    let withPrimary lightColor darkColor config =
      config |> withLightPrimary lightColor |> withDarkPrimary darkColor

    let withLightBackground bg bgPaper bgDefault (config: ThemeConfig) = {
      config with
          LightPalette = {
            config.LightPalette with
                Background = Some bg
                BackgroundPaper = Some bgPaper
                BackgroundDarken = Some bgDefault
          }
    }

    let withDarkBackground bg bgPaper bgDefault (config: ThemeConfig) = {
      config with
          DarkPalette = {
            config.DarkPalette with
                Background = Some bg
                BackgroundPaper = Some bgPaper
                BackgroundDarken = Some bgDefault
          }
    }
