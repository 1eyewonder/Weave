namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ButtonSource =

  [<RequireQualifiedAccess>]
  type Variant =
    | Filled
    | Outlined
    | Text

  [<RequireQualifiedAccess>]
  type Color =
    | Primary
    | Secondary
    | Tertiary
    | Error
    | Warning
    | Success
    | Info

  [<RequireQualifiedAccess>]
  type Density =
    | Compact
    | Standard
    | Spacious

  [<RequireQualifiedAccess>]
  type Corners =
    | Default
    | None
    | Small
    | Medium
    | Large
    | Pill

  type Appearance = {
    Variant: Variant
    Color: Color
    Density: Density
    Corners: Corners
    FullWidth: bool
    Disabled: bool
  }

  let variants = [ Variant.Filled, "Filled"; Variant.Outlined, "Outlined"; Variant.Text, "Text" ]

  let colors = [
    Color.Primary, "Primary"
    Color.Secondary, "Secondary"
    Color.Tertiary, "Tertiary"
    Color.Error, "Error"
    Color.Warning, "Warning"
    Color.Success, "Success"
    Color.Info, "Info"
  ]

  let densities = [
    Density.Compact, "Compact"
    Density.Standard, "Standard"
    Density.Spacious, "Spacious"
  ]

  let corners = [
    Corners.Default, "Default"
    Corners.None, "None"
    Corners.Small, "Small"
    Corners.Medium, "Medium"
    Corners.Large, "Large"
    Corners.Pill, "Pill"
  ]

  let defaults = {
    Variant = Variant.Filled
    Color = Color.Primary
    Density = Density.Standard
    Corners = Corners.Default
    FullWidth = false
    Disabled = false
  }

  let private colorName color =
    match color with
    | Color.Primary -> "primary"
    | Color.Secondary -> "secondary"
    | Color.Tertiary -> "tertiary"
    | Color.Error -> "error"
    | Color.Warning -> "warning"
    | Color.Success -> "success"
    | Color.Info -> "info"

  /// The source contains the configured button, without the preview controls.
  let render appearance =
    let variant =
      match appearance.Variant with
      | Variant.Filled -> "Button.Variant.filled"
      | Variant.Outlined -> "Button.Variant.outlined"
      | Variant.Text -> "Button.Variant.text"

    let attributes = [
      variant

      match appearance.Density with
      | Density.Compact -> "Density.compact"
      | Density.Standard -> ()
      | Density.Spacious -> "Density.spacious"

      match appearance.Corners with
      | Corners.Default -> ()
      | Corners.None -> "BorderRadius.All.none"
      | Corners.Small -> "BorderRadius.All.small"
      | Corners.Medium -> "BorderRadius.All.medium"
      | Corners.Large -> "BorderRadius.All.large"
      | Corners.Pill -> "BorderRadius.pill"

      if appearance.FullWidth then
        "Button.Width.full"
    ]

    let attributeLines =
      if List.length attributes = 1 then
        [ sprintf "  attrs = [ %s ]" variant ]
      else
        [ "  attrs = ["; yield! attributes |> List.map (sprintf "    %s"); "  ]" ]

    [
      sprintf "Button.%s (" (colorName appearance.Color)
      "  text \"Save changes\","
      "  onClick = (fun () -> ()),"
      if appearance.Disabled then
        "  enabled = View.Const false,"
      yield! attributeLines
      ")"
    ]
    |> String.concat "\n"

  /// Every combination exposed by the Appearance controls is compiled in docs checks.
  let validationCases () = [
    for variant, variantLabel in variants do
      for color, colorLabel in colors do
        for density, densityLabel in densities do
          for corner, cornerLabel in corners do
            for fullWidth in [ false; true ] do
              for disabled in [ false; true ] do
                let appearance = {
                  Variant = variant
                  Color = color
                  Density = density
                  Corners = corner
                  FullWidth = fullWidth
                  Disabled = disabled
                }

                let title =
                  sprintf
                    "%s / %s / %s / %s / %s / %s"
                    variantLabel
                    colorLabel
                    densityLabel
                    cornerLabel
                    (if fullWidth then "Full width" else "Natural width")
                    (if disabled then "Disabled" else "Enabled")

                title, render appearance
  ]

  /// Source for the icon button preview, with its required accessible name.
  let renderIcon (appearance: Appearance) circle =
    let variant =
      match appearance.Variant with
      | Variant.Filled -> "Button.Variant.filled"
      | Variant.Outlined -> "Button.Variant.outlined"
      | Variant.Text -> "Button.Variant.text"

    let attributes = [
      "Attr.Create \"aria-label\" \"Favorite\""
      variant
      match appearance.Density with
      | Density.Compact -> "Density.compact"
      | Density.Standard -> ()
      | Density.Spacious -> "Density.spacious"
      if circle then
        "BorderRadius.circle"
    ]

    [
      "open Weave.Icons.MaterialSymbols"
      ""
      sprintf "IconButton.%s (" (colorName appearance.Color)
      "  Icon.create (Icon.UiActions UiActions.Favorite),"
      "  onClick = (fun () -> ()),"
      if appearance.Disabled then
        "  enabled = View.Const false,"
      "  attrs = ["
      yield! attributes |> List.map (sprintf "    %s")
      "  ]"
      ")"
    ]
    |> String.concat "\n"

  let iconValidationCases () = [
    for variant, variantLabel in variants do
      for color, colorLabel in colors do
        for density, densityLabel in densities do
          for circle in [ false; true ] do
            for disabled in [ false; true ] do
              let appearance = {
                defaults with
                    Variant = variant
                    Color = color
                    Density = density
                    Disabled = disabled
              }

              sprintf
                "%s / %s / %s / circle=%b / disabled=%b"
                variantLabel
                colorLabel
                densityLabel
                circle
                disabled,
              renderIcon appearance circle
  ]
