namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Theming

[<JavaScript>]
module ThemingExamples =

  let private toggleExample () =
    let currentMode = Var.Create(getMode ())

    button [ on.click (fun _ _ -> currentMode.Value <- toggleMode ()) ] [
      currentMode.View.Map(fun mode ->
        match mode with
        | Light -> "Switch to dark"
        | Dark -> "Switch to light")
      |> Doc.TextView
    ]

  let private modeSection () =
    let description =
      Helpers.bodyText "F# selects the mode on the document body; CSS provides its colors and typography."

    let code =
      """open Weave.Theming

let currentMode = Var.Create (getMode ())
let toggle () = currentMode.Value <- toggleMode ()

// Or choose a mode explicitly:
setMode Dark"""

    Helpers.codeSampleSection "Light / Dark Mode" description (toggleExample ()) code

  let private cssSection () =
    let description =
      Helpers.bodyText
        "Load your CSS after Weave. Target the body that carries data-theme so the mode rules do not shadow your overrides. Set matching RGB channels for colors used with transparency. Set shared font roles on :root; override individual typography variant tokens on the mode selector if needed."

    let code =
      """/* app.css, loaded after Weave */
:root { --weave-font-family-body: Arial, sans-serif; }

body:not([data-theme]),
body[data-theme="light"] {
  --palette-primary: #6200ea;
  --palette-primary-text: #fff;
  --palette-primary-lighten: #7c4dff;
  --palette-primary-darken: #4a00b8;
  --palette-primary-rgb: 98, 0, 234;
  --palette-surface: #fff;
  --palette-text-primary: #21243a;
  --palette-text-primary-rgb: 33, 36, 58;
}

body[data-theme="dark"] {
  --palette-primary: #b388ff;
  --palette-primary-text: #201b2e;
  --palette-primary-lighten: #d1b3ff;
  --palette-primary-darken: #8e5fff;
  --palette-primary-rgb: 179, 136, 255;
  --palette-surface: #202333;
  --palette-text-primary: #e6ebf5;
  --palette-text-primary-rgb: 230, 235, 245;
}"""

    Helpers.codeSampleSection "Customize with CSS" description Doc.Empty code

  // Reference values mirror the default CSS; keep these tables in sync with the stylesheets.
  let private referenceTable headers (rows: string list list) =
    div [ Attr.Style "overflow-x" "auto"; Margin.Vertical.extraSmall ] [
      table [ Attr.Class "docs-api-table" ] [
        thead [] [
          tr [] [ for header in headers -> th [ Attr.Create "scope" "col" ] [ text header ] ]
        ]
        tbody [] [ for row in rows -> tr [] [ for cell in row -> td [] [ text cell ] ] ]
      ]
    ]

  let private colorReferenceSection () =
    div [ Margin.Bottom.small ] [
      Helpers.sectionHeader "Color variables"
      Helpers.bodyText
        "Override these shared tokens on body[data-theme=light] and body[data-theme=dark]. Unspecified tokens keep the Weave defaults shown below. The tables describe the defaults, not calculated colors."
      div [ Margin.Bottom.small ] [
        h3 [ Typography.h6 ] [ text "Brand and status colors" ]
        Helpers.bodyText
          "Each family has a main color, foreground (-text), lighter and darker variants, and comma-separated RGB channels used for alpha. Change the companions together when replacing a color."
        referenceTable [ "Variable"; "Light default"; "Dark default" ] [
          [ "--palette-primary"; "#0164fc"; "#088ffd" ]
          [ "--palette-primary-text"; "#ffffff"; "#ffffff" ]
          [ "--palette-primary-lighten"; "#0187f1"; "#35d7ff" ]
          [ "--palette-primary-darken"; "#002fc6"; "#0054d1" ]
          [ "--palette-primary-rgb"; "1, 100, 252"; "8, 143, 253" ]
          [ "--palette-secondary"; "#594ae2"; "#594ae2" ]
          [ "--palette-secondary-text"; "#ffffff"; "#ffffff" ]
          [ "--palette-secondary-lighten"; "#7d6ff0"; "#7569e0" ]
          [ "--palette-secondary-darken"; "#3d2eb4"; "#4236ad" ]
          [ "--palette-secondary-rgb"; "89, 74, 226"; "89, 74, 226" ]
          [ "--palette-tertiary"; "#ff4081"; "#ff4081" ]
          [ "--palette-tertiary-text"; "#ffffff"; "#ffffff" ]
          [ "--palette-tertiary-lighten"; "#ff6699"; "#ff6699" ]
          [ "--palette-tertiary-darken"; "#d91d63"; "#c71959" ]
          [ "--palette-tertiary-rgb"; "255, 64, 129"; "255, 64, 129" ]
          [ "--palette-success"; "#00aa47"; "#0bba83" ]
          [ "--palette-success-text"; "#ffffff"; "#ffffff" ]
          [ "--palette-success-lighten"; "#21ca62"; "#3dc99a" ]
          [ "--palette-success-darken"; "#009638"; "#089a6b" ]
          [ "--palette-success-rgb"; "0, 170, 71"; "11, 186, 131" ]
          [ "--palette-error"; "#f44336"; "#f64e62" ]
          [ "--palette-error-text"; "#ffffff"; "#ffffff" ]
          [ "--palette-error-lighten"; "#f6685e"; "#f77381" ]
          [ "--palette-error-darken"; "#d32f2f"; "#b53746" ]
          [ "--palette-error-rgb"; "244, 67, 54"; "246, 78, 98" ]
          [ "--palette-warning"; "#f19306"; "#ffa800" ]
          [ "--palette-warning-text"; "#ffffff"; "#ffffff" ]
          [ "--palette-warning-lighten"; "#ff9800"; "#ffb933" ]
          [ "--palette-warning-darken"; "#d4840b"; "#cc8600" ]
          [ "--palette-warning-rgb"; "241, 147, 6"; "255, 168, 0" ]
          [ "--palette-info"; "#15b9be"; "#17cdd3" ]
          [ "--palette-info-text"; "#ffffff"; "#ffffff" ]
          [ "--palette-info-lighten"; "#12dde4"; "#63f8fd" ]
          [ "--palette-info-darken"; "#119397"; "#09b2b8" ]
          [ "--palette-info-rgb"; "21, 185, 190"; "23, 205, 211" ]
        ]
      ]
      div [ Margin.Bottom.small ] [
        h3 [ Typography.h6 ] [ text "Surfaces, text, borders and actions" ]
        Helpers.bodyText
          "Surface tokens fill backgrounds; text and action tokens cover the default, hover, active and disabled states. Primary text RGB also controls translucent component borders."
        referenceTable [ "Variable"; "Light default"; "Dark default" ] [
          [ "--palette-background"; "#ebebeb"; "#1b1c25" ]
          [ "--palette-background-paper"; "#d5d5d5"; "#43454f" ]
          [ "--palette-background-darken"; "#bfbfbf"; "#13141b" ]
          [ "--palette-surface"; "#ffffff"; "#32343e" ]
          [ "--palette-text-primary"; "rgba(66, 66, 66, 1)"; "rgba(255, 255, 255)" ]
          [ "--palette-text-primary-rgb"; "66, 66, 66"; "255, 255, 255" ]
          [
            "--palette-text-secondary"
            "rgba(0, 0, 0, 0.64)"
            "rgba(255, 255, 255, 0.5)"
          ]
          [ "--palette-text-disabled"; "rgba(0, 0, 0, 0.38)"; "rgba(255, 255, 255, 0.2)" ]
          [ "--palette-divider"; "rgba(0, 0, 0, 0.24)"; "rgba(255, 255, 255, 0.24)" ]
          [
            "--palette-divider-light"
            "rgba(0, 0, 0, 0.10)"
            "rgba(255, 255, 255, 0.12)"
          ]
          [
            "--palette-lines-default"
            "rgba(0, 0, 0, 0.20)"
            "rgba(255, 255, 255, 0.12)"
          ]
          [ "--palette-lines-inputs"; "#9e9e9e"; "rgba(255, 255, 255, 0.3)" ]
          [ "--palette-action-default"; "rgba(0, 0, 0, 0.64)"; "#adadb1" ]
          [
            "--palette-action-default-hover"
            "rgba(0, 0, 0, 0.10)"
            "rgba(173, 173, 177, 0.3)"
          ]
          [
            "--palette-action-default-active"
            "rgba(0, 0, 0, 0.08)"
            "rgba(173, 173, 177, 0.45)"
          ]
          [
            "--palette-action-disabled"
            "rgba(0, 0, 0, 0.26)"
            "rgba(255, 255, 255, 0.26)"
          ]
          [
            "--palette-action-disabled-background"
            "rgba(0, 0, 0, 0.12)"
            "rgba(255, 255, 255, 0.12)"
          ]
        ]
      ]
      div [ Margin.Bottom.small ] [
        h3 [ Typography.h6 ] [ text "Component colors" ]
        Helpers.bodyText
          "These affect app bars, drawers, switches and fields. A themed component can define a more local value, which takes precedence over an inherited body value."
        referenceTable [ "Variable"; "Light default"; "Dark default" ] [
          [ "--palette-appbar-background"; "#594ae2"; "#27272f" ]
          [ "--palette-appbar-text"; "#ffffff"; "#ffffff" ]
          [ "--palette-drawer-background"; "#f5f5f5"; "#27272f" ]
          [ "--palette-drawer-text"; "#424242"; "rgba(255, 255, 255, 0.5)" ]
          [ "--palette-drawer-icon"; "#616161"; "rgba(255, 255, 255, 0.5)" ]
          [
            "--switch-thumb-idle"
            "var(--palette-action-default)"
            "var(--palette-action-default)"
          ]
          [ "--input-border-color"; "rgba(0, 0, 0, 0.42)"; "rgba(255, 255, 255, 0.42)" ]
          [
            "--input-outlined-border"
            "rgba(0, 0, 0, 0.42)"
            "rgba(255, 255, 255, 0.36)"
          ]
          [
            "--input-filled-background"
            "rgba(0, 0, 0, 0.04)"
            "rgba(255, 255, 255, 0.09)"
          ]
          [ "--input-label-color"; "rgba(0, 0, 0, 0.6)"; "rgba(255, 255, 255, 0.7)" ]
          [ "--input-help-text-color"; "rgba(0, 0, 0, 0.6)"; "rgba(255, 255, 255, 0.7)" ]
          [
            "--input-label-background"
            "var(--palette-background)"
            "var(--palette-background)"
          ]
        ]
      ]
      div [ Margin.Bottom.small ] [
        h3 [ Typography.h6 ] [ text "Neutral and overlay colors" ]
        Helpers.bodyText
          "Gray shades, contrast text, opacity values and the scrim provide additional color controls."
        referenceTable [ "Variable"; "Light default"; "Dark default" ] [
          [ "--palette-gray-default"; "#9e9e9e"; "#9e9e9e" ]
          [ "--palette-gray-light"; "#bdbdbd"; "#bdbdbd" ]
          [ "--palette-gray-lighter"; "#e0e0e0"; "#e0e0e0" ]
          [ "--palette-gray-dark"; "#757575"; "#757575" ]
          [ "--palette-gray-darker"; "#212121"; "#212121" ]
          [ "--palette-dark-text"; "#ffffff"; "rgba(0, 0, 0, 0.87)" ]
          [ "--palette-hover-opacity"; "0.06"; "0.06" ]
          [ "--palette-border-opacity"; "0.8"; "0.8" ]
          [ "--weave-scrim-background"; "rgba(0, 0, 0, 0.2)"; "rgba(0, 0, 0, 0.4)" ]
        ]
      ]
    ]

  let private typographyReferenceSection () =
    let example =
      """/* app.css, after Weave */
:root {
  --weave-font-family-display: Georgia, serif;
  --weave-font-family-body: Arial, sans-serif;
  --weave-weight-bold: 600;
  --typography-h1-size: clamp(2rem, 4vw, 3rem);
  --typography-button-text-transform: none;
}

/* Override a resolved variant when it needs a mode-specific value. */
body[data-theme="dark"] { --typography-body1-family: Georgia, serif; }"""

    div [ Margin.Bottom.small ] [
      Helpers.sectionHeader "Typography variables"
      Helpers.bodyText
        "Set shared roles on :root after Weave. Variant variables are resolved there, so changing a role only on body does not recalculate inherited variants; override an individual --typography-* token on the mode selector for mode-specific typography. Load any custom fonts separately."
      h3 [ Typography.h6 ] [ text "Font and weight roles" ]
      referenceTable [ "Variable"; "Default"; "Purpose" ] [
        [ "--weave-font-family-display"; "Lato, sans-serif"; "Headings h1–h6" ]
        [
          "--weave-font-family-body"
          "Fira Sans, sans-serif"
          "Body, subtitles and UI text"
        ]
        [
          "--weave-font-family-mono"
          "Fira Mono, Consolas, monospace"
          "Monospace text"
        ]
        [ "--weave-weight-light"; "300"; "Light weight role" ]
        [ "--weave-weight-regular"; "400"; "Regular weight role" ]
        [ "--weave-weight-bold"; "700"; "Bold weight role" ]
      ]
      h3 [ Typography.h6 ] [ text "Typography variants" ]
      Helpers.bodyText
        "Every row supports --typography-{variant}-{property}, with property family, size, weight, lineheight, letterspacing or text-transform. For example, --typography-h1-size and --typography-button-text-transform. Defaults below show role references where applicable."
      referenceTable [
        "Variant"
        "family"
        "size"
        "weight"
        "lineheight"
        "letterspacing"
        "text-transform"
      ] [
        [
          "h1"
          "var(--weave-font-family-display)"
          "clamp(2.25rem, calc(1.8rem + 2.15vw), 3.75rem)"
          "var(--weave-weight-light)"
          "1.15"
          "-0.015em"
          "none"
        ]
        [
          "h2"
          "var(--weave-font-family-display)"
          "clamp(1.875rem, calc(1.5rem + 1.6vw), 3rem)"
          "var(--weave-weight-light)"
          "1.2"
          "-0.005em"
          "none"
        ]
        [
          "h3"
          "var(--weave-font-family-display)"
          "clamp(1.625rem, calc(1.35rem + 1.25vw), 2.5rem)"
          "var(--weave-weight-regular)"
          "1.25"
          "0em"
          "none"
        ]
        [
          "h4"
          "var(--weave-font-family-display)"
          "clamp(1.375rem, calc(1.15rem + 0.9vw), 2rem)"
          "var(--weave-weight-regular)"
          "1.3"
          "0.007em"
          "none"
        ]
        [
          "h5"
          "var(--weave-font-family-display)"
          "clamp(1.125rem, calc(1rem + 0.55vw), 1.5rem)"
          "var(--weave-weight-regular)"
          "1.35"
          "0em"
          "none"
        ]
        [
          "h6"
          "var(--weave-font-family-display)"
          "clamp(1.0625rem, calc(0.9rem + 0.35vw), 1.25rem)"
          "var(--weave-weight-bold)"
          "1.4"
          "0.008em"
          "none"
        ]
        [
          "subtitle1"
          "var(--weave-font-family-body)"
          "clamp(1rem, calc(0.95rem + 0.18vw), 1.125rem)"
          "var(--weave-weight-regular)"
          "1.5"
          "0.009em"
          "none"
        ]
        [
          "subtitle2"
          "var(--weave-font-family-body)"
          "0.875rem"
          "var(--weave-weight-bold)"
          "1.5"
          "0.007em"
          "none"
        ]
        [
          "body1"
          "var(--weave-font-family-body)"
          "1rem"
          "var(--weave-weight-regular)"
          "1.6"
          "0.01em"
          "none"
        ]
        [
          "body2"
          "var(--weave-font-family-body)"
          "0.875rem"
          "var(--weave-weight-regular)"
          "1.5"
          "0.011em"
          "none"
        ]
        [
          "button"
          "var(--weave-font-family-body)"
          "0.875rem"
          "var(--weave-weight-bold)"
          "1.75"
          "0.03em"
          "uppercase"
        ]
        [
          "caption"
          "var(--weave-font-family-body)"
          "0.75rem"
          "var(--weave-weight-regular)"
          "1.66"
          "0.03em"
          "none"
        ]
        [
          "overline"
          "var(--weave-font-family-body)"
          "0.75rem"
          "var(--weave-weight-bold)"
          "1.75"
          "0.1em"
          "uppercase"
        ]
        [
          "default"
          "var(--weave-font-family-body)"
          "1rem"
          "var(--weave-weight-regular)"
          "1.6"
          "0.01em"
          "none"
        ]
      ]
      Helpers.codeSampleSection
        "Typography override example"
        (Helpers.bodyText "Change the shared roles or one variant at a time.")
        Doc.Empty
        example
    ]

  let private opacityReferenceSection () =
    div [ Margin.Bottom.small ] [
      Helpers.sectionHeader "Opacity and alpha variables"
      Helpers.bodyText
        "These :root values are unitless. Opacity tokens set element opacity; alpha tokens are used in translucent colors. Components may choose their own tokens for individual states."
      referenceTable [ "Variable"; "Default" ] [
        [ "--weave-opacity-disabled"; "0.38" ]
        [ "--weave-opacity-disabled-container"; "0.6" ]
        [ "--weave-opacity-placeholder"; "0.42" ]
        [ "--weave-opacity-secondary"; "0.5" ]
        [ "--weave-opacity-secondary-hover"; "0.7" ]
        [ "--weave-opacity-muted"; "0.8" ]
        [ "--weave-alpha-subtle"; "0.04" ]
        [ "--weave-alpha-selected"; "0.08" ]
        [ "--weave-alpha-border"; "0.12" ]
        [ "--weave-alpha-selected-hover"; "0.16" ]
        [ "--weave-alpha-hover"; "0.2" ]
        [ "--weave-alpha-focus"; "0.25" ]
        [ "--weave-alpha-medium"; "0.3" ]
        [ "--weave-alpha-overlay"; "0.4" ]
        [ "--weave-alpha-scrim"; "0.5" ]
      ]
      h3 [ Typography.h6 ] [ text "Component-scoped color variables" ]
      Helpers.bodyText
        "Components set these on their own elements or variants, so body-level declarations can be shadowed. Override the relevant element with sufficient specificity; prefer shared palette tokens for application-wide colors. RGB values use comma-separated channels."
      referenceTable [ "Variable"; "Use or default" ] [
        [
          "--checkbox-brand-color"
          "Checkbox color; defaults to action color or selected palette"
        ]
        [
          "--radio-brand-color"
          "Radio color; defaults to action color or selected palette"
        ]
        [
          "--switch-brand-color"
          "Switch color; defaults to action color or selected palette"
        ]
        [
          "--slider-brand-color"
          "Slider color; defaults to primary or selected palette"
        ]
        [
          "--select-brand-color / --select-brand-rgb"
          "Select color and RGB channels; defaults to primary or selected palette"
        ]
        [
          "--list-item-selected-color / --list-item-selected-rgb"
          "Selected list item color and RGB channels; defaults to primary or selected palette"
        ]
        [
          "--expansion-panel-focus-color"
          "Expansion panel focus outline; defaults to action color or selected palette"
        ]
        [
          "--table-hover-color"
          "Table row hover; falls back to --palette-action-default-hover"
        ]
        [
          "--ripple-color / --ripple-opacity / --ripple-opacity-secondary"
          "Ripple color and opacity; defaults are currentColor, 0.3 and 0.15; some components set their own values"
        ]
      ]
      h3 [ Typography.h6 ] [ text "Optional colors without built-in defaults" ]
      Helpers.bodyText
        "These names are referenced by components but are not defined in the default palettes. Define them on the theme selector when using the relevant styles."
      referenceTable [ "Variable"; "Use" ] [
        [ "--palette-action-hover"; "Select and dropdown hover background" ]
        [ "--palette-background-gray"; "Gray background utility" ]
      ]
    ]

  let private systemSection () =
    let description =
      Helpers.bodyText
        "Read the current system preference at startup. The helper does not subscribe to later OS changes."

    let code =
      """open Weave.Theming

setMode (detectSystemPreference ())"""

    Helpers.codeSampleSection "System Preference" description Doc.Empty code

  let private persistenceSection () =
    let description =
      Helpers.bodyText "Save an explicit choice and use the system preference when no valid choice exists."

    let code =
      """open Weave.Theming
open WebSharper.JavaScript

let initialMode =
    match JS.Window.LocalStorage.GetItem("theme-mode") with
    | "dark" -> Dark
    | "light" -> Light
    | _ -> detectSystemPreference ()

setMode initialMode

let switchAndSave mode =
    setMode mode
    let value = if mode = Dark then "dark" else "light"
    JS.Window.LocalStorage.SetItem("theme-mode", value)"""

    Helpers.codeSampleSection "Persist a Choice" description Doc.Empty code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Theming"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Select a mode in F# and customize Weave with CSS custom properties."
        ]
        Helpers.divider ()
        modeSection ()
        Helpers.divider ()
        cssSection ()
        Helpers.divider ()
        colorReferenceSection ()
        Helpers.divider ()
        typographyReferenceSection ()
        Helpers.divider ()
        opacityReferenceSection ()
        Helpers.divider ()
        systemSection ()
        Helpers.divider ()
        persistenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
