namespace Weave.Docs.Examples

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module TextFieldAppearance =

  [<RequireQualifiedAccess>]
  type Variant =
    | Standard
    | Filled
    | Outlined

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
  type Width =
    | Auto
    | Full

  [<RequireQualifiedAccess>]
  type LabelPosition =
    | Floating
    | Above

  [<RequireQualifiedAccess>]
  type Adornment =
    | None
    | Start
    | End
    | Both

  type Configuration = {
    Variant: Variant
    Color: Color
    Width: Width
    LabelPosition: LabelPosition
    Adornment: Adornment
    Placeholder: bool
  }

  [<RequireQualifiedAccess>]
  type Setting =
    | Filled
    | Outlined
    | Accent of Color
    | FullWidth
    | LabelAbove

  let variants = [
    Variant.Standard, "Standard"
    Variant.Filled, "Filled"
    Variant.Outlined, "Outlined"
  ]

  let colors = [
    Color.Primary, "Primary"
    Color.Secondary, "Secondary"
    Color.Tertiary, "Tertiary"
    Color.Error, "Error"
    Color.Warning, "Warning"
    Color.Success, "Success"
    Color.Info, "Info"
  ]

  let widths = [ Width.Auto, "Auto"; Width.Full, "Full" ]
  let labelPositions = [ LabelPosition.Floating, "Floating"; LabelPosition.Above, "Above (outlined)" ]

  let adornments = [
    Adornment.None, "None"
    Adornment.Start, "Start"
    Adornment.End, "End"
    Adornment.Both, "Both"
  ]

  let defaults = {
    Variant = Variant.Standard
    Color = Color.Primary
    Width = Width.Auto
    LabelPosition = LabelPosition.Floating
    Adornment = Adornment.None
    Placeholder = false
  }

  let normalise configuration =
    match configuration.Variant with
    | Variant.Outlined -> configuration
    | Variant.Standard
    | Variant.Filled -> { configuration with LabelPosition = LabelPosition.Floating }

  let settings configuration =
    let configuration = normalise configuration

    [
      match configuration.Variant with
      | Variant.Standard -> ()
      | Variant.Filled -> Setting.Filled
      | Variant.Outlined -> Setting.Outlined

      match configuration.Color with
      | Color.Primary -> ()
      | Color.Secondary
      | Color.Tertiary
      | Color.Error
      | Color.Warning
      | Color.Success
      | Color.Info -> Setting.Accent configuration.Color

      match configuration.Width with
      | Width.Auto -> ()
      | Width.Full -> Setting.FullWidth

      match configuration.LabelPosition with
      | LabelPosition.Floating -> ()
      | LabelPosition.Above -> Setting.LabelAbove
    ]

  let private colorName color =
    match color with
    | Color.Primary -> "primary"
    | Color.Secondary -> "secondary"
    | Color.Tertiary -> "tertiary"
    | Color.Error -> "error"
    | Color.Warning -> "warning"
    | Color.Success -> "success"
    | Color.Info -> "info"

  let private settingSource setting =
    match setting with
    | Setting.Filled -> "TextField.Variant.filled"
    | Setting.Outlined -> "TextField.Variant.outlined"
    | Setting.Accent color -> "TextField.Color." + colorName color
    | Setting.FullWidth -> "TextField.Width.full"
    | Setting.LabelAbove -> "TextField.Label.above"

  let source configuration value =
    let attributes = configuration |> settings |> List.map settingSource

    let hasStart =
      configuration.Adornment = Adornment.Start
      || configuration.Adornment = Adornment.Both

    let hasEnd =
      configuration.Adornment = Adornment.End
      || configuration.Adornment = Adornment.Both

    let args = [
      [ "name" ]
      [ "labelText = View.Const \"Name\"" ]
      if configuration.Placeholder then
        [ "placeholder = View.Const \"Ada Lovelace\"" ]
      if hasStart then
        [ "startAdornment = Icon.create (Icon.UiActions UiActions.Search)" ]
      if hasEnd then
        [ "endAdornment = Icon.create (Icon.UiActions UiActions.CheckCircle)" ]
      match attributes with
      | [] -> ()
      | [ attr ] -> [ "attrs = [ " + attr + " ]" ]
      | _ -> [ "attrs = ["; yield! attributes |> List.map (fun attr -> "  " + attr); "]" ]
    ]

    [
      if hasStart || hasEnd then
        "open Weave.Icons"
        "open Weave.Icons.MaterialSymbols"
        ""
      yield! Weave.Docs.SourceString.varBinding "name" value
      ""
      if args.Length = 2 then
        "TextField.singleLine (name, labelText = View.Const \"Name\")"
      else
        "TextField.singleLine ("

        for index, lines in args |> List.indexed do
          for lineIndex, line in lines |> List.indexed do
            let comma =
              if index < args.Length - 1 && lineIndex = lines.Length - 1 then
                ","
              else
                ""

            "  " + line + comma

        ")"
    ]
    |> String.concat "\n"

  let configurations =
    [
      for variant, _ in variants do
        for color, _ in colors do
          for width, _ in widths do
            for labelPosition, _ in labelPositions do
              for adornment, _ in adornments do
                for placeholder in [ false; true ] do
                  {
                    Variant = variant
                    Color = color
                    Width = width
                    LabelPosition = labelPosition
                    Adornment = adornment
                    Placeholder = placeholder
                  }
                  |> normalise
    ]
    |> List.distinct

  let sourceCases () = [
    yield!
      configurations
      |> List.mapi (fun index configuration ->
        sprintf "Text Field appearance %d" (index + 1), source configuration "")
    for index, value in
      [
        "Ada \"Q\""
        "C:\\docs"
        "one\ntwo\tthree"
        "café 👍"
        String.replicate 100 "a"
        "\u0001"
      ]
      |> List.indexed do
      sprintf "Text Field value %d" (index + 1), source defaults value
  ]
