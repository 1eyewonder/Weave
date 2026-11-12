namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module TabsAppearanceSource =

  [<RequireQualifiedAccess>]
  type Variant =
    | Text
    | Outlined
    | Filled

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
  type Position =
    | Top
    | Bottom
    | Left
    | Right
    | Start
    | End

  [<RequireQualifiedAccess>]
  type Tray =
    | Plain
    | Matching

  [<RequireQualifiedAccess>]
  type Alignment =
    | Start
    | Center

  [<RequireQualifiedAccess>]
  type Icons =
    | None
    | Start
    | End

  type Appearance = {
    Variant: Variant
    Color: Color
    Density: Density
    Position: Position
    Tray: Tray
    Alignment: Alignment
    Icons: Icons
  }

  let defaults = {
    Variant = Variant.Text
    Color = Color.Primary
    Density = Density.Standard
    Position = Position.Top
    Tray = Tray.Plain
    Alignment = Alignment.Start
    Icons = Icons.None
  }

  let variants = [ Variant.Text, "Text"; Variant.Outlined, "Outlined"; Variant.Filled, "Filled" ]

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

  let positions = [
    Position.Top, "Top"
    Position.Bottom, "Bottom"
    Position.Left, "Left"
    Position.Right, "Right"
    Position.Start, "Start (logical)"
    Position.End, "End (logical)"
  ]

  let alignments = [ Alignment.Start, "Start"; Alignment.Center, "Center" ]

  let icons = [ Icons.None, "None"; Icons.Start, "Start"; Icons.End, "End" ]

  let tabs = [
    "Overview", "Overview content", "UiActions UiActions.Home"
    "Activity", "Activity content", "Action Action.Info"
    "Settings", "Settings content", "UiActions UiActions.Settings"
  ]

  let positionName position =
    match position with
    | Position.Top -> "Top"
    | Position.Bottom -> "Bottom"
    | Position.Left -> "Left"
    | Position.Right -> "Right"
    | Position.Start -> "Start"
    | Position.End -> "End"

  let private colorName color =
    match color with
    | Color.Primary -> "primary"
    | Color.Secondary -> "secondary"
    | Color.Tertiary -> "tertiary"
    | Color.Error -> "error"
    | Color.Warning -> "warning"
    | Color.Success -> "success"
    | Color.Info -> "info"

  let attributeNames appearance = [
    match appearance.Variant with
    | Variant.Text -> ()
    | Variant.Outlined -> "Tabs.Variant.outlined"
    | Variant.Filled -> "Tabs.Variant.filled"

    "Tabs.Color." + colorName appearance.Color

    match appearance.Density with
    | Density.Standard -> ()
    | Density.Compact -> "Density.compact"
    | Density.Spacious -> "Density.spacious"

    if appearance.Alignment = Alignment.Center then
      "Tabs.Alignment.center"

    match appearance.Variant, appearance.Tray with
    | Variant.Filled, Tray.Matching -> "Tabs.HeaderBackground." + colorName appearance.Color
    | Variant.Text, _
    | Variant.Outlined, _
    | Variant.Filled, Tray.Plain -> ()
  ]

  let render appearance activeIndex =
    if activeIndex < 0 || activeIndex >= tabs.Length then
      invalidArg "activeIndex" "Choose one of the three preview panels."

    let attributes = attributeNames appearance

    [
      if appearance.Icons <> Icons.None then
        "open Weave.Icons"
        "open Weave.Icons.MaterialSymbols"
        ""

      if activeIndex <> 0 then
        sprintf "let selectedTab = Var.Create %d" activeIndex
        ""

      "Tabs.create ("
      "  View.Const ["
      for label, panel, icon in tabs do
        match appearance.Icons with
        | Icons.None -> sprintf "    TabItem.create (\"%s\", text \"%s\")" label panel
        | iconPosition ->
          let parameter =
            if iconPosition = Icons.Start then
              "startIcon"
            else
              "endIcon"

          "    TabItem.create ("
          sprintf "      \"%s\"," label
          sprintf "      text \"%s\"," panel
          sprintf "      %s = Icon.create (Icon.%s)" parameter icon
          "    )"
      "  ],"

      if activeIndex <> 0 then
        "  activeIndex = selectedTab,"

      if appearance.Position <> Position.Top then
        "  position = Tabs.Position." + positionName appearance.Position + ","

      match attributes with
      | [ attribute ] -> "  attrs = [ " + attribute + " ]"
      | _ ->
        "  attrs = ["

        for attribute in attributes do
          "    " + attribute

        "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant, _ in variants do
      let trays =
        if variant = Variant.Filled then
          [ Tray.Plain; Tray.Matching ]
        else
          [ Tray.Plain ]

      for color, _ in colors do
        for density, _ in densities do
          for position, _ in positions do
            for tray in trays do
              let appearance = {
                Variant = variant
                Color = color
                Density = density
                Position = position
                Tray = tray
                Alignment = Alignment.Start
                Icons = Icons.None
              }

              for activeIndex in 0..2 do
                let title =
                  sprintf "%A %A %A %A %A panel %d" variant color density position tray activeIndex

                title, render appearance activeIndex

    // Cross the new options with each position and variant, including the filled tray.
    for variant, _ in variants do
      for position, _ in positions do
        for alignment, _ in alignments do
          for icons, _ in icons do
            let appearance = {
              defaults with
                  Variant = variant
                  Position = position
                  Alignment = alignment
                  Icons = icons
                  Tray =
                    if variant = Variant.Filled then
                      Tray.Matching
                    else
                      Tray.Plain
            }

            let title = sprintf "%A %A %A %A" variant position alignment icons
            title, render appearance 1
  ]
