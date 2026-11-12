namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module FieldAppearance =

  type Configuration = {
    Variant: string
    Color: string
    FullWidth: bool
    LabelAbove: bool
  }

  let variants = [ "standard", "Standard"; "filled", "Filled"; "outlined", "Outlined" ]

  let colors = [
    "default", "Default"
    "primary", "Primary"
    "secondary", "Secondary"
    "tertiary", "Tertiary"
    "error", "Error"
    "warning", "Warning"
    "success", "Success"
    "info", "Info"
  ]

  let widths = [ false, "Auto"; true, "Full" ]

  let defaults = {
    Variant = "standard"
    Color = "default"
    FullWidth = false
    LabelAbove = false
  }

  let normalise current =
    if current.Variant = "outlined" then
      current
    else
      { current with LabelAbove = false }

  let attributes config =
    let current = normalise config

    [
      if current.Variant <> "standard" then
        yield "Field.Variant." + current.Variant
      if current.Color <> "default" then
        yield "Field.Color." + current.Color
      if current.FullWidth then
        yield "Field.Width.full"
      if current.LabelAbove then
        yield "Field.Label.above"
    ]

  let render current value =
    let attrs = attributes current

    [
      yield! SourceString.varBinding "value" value
      "let isFocused = Var.Create false"
      "let hasValue = value.View |> View.MapCached (System.String.IsNullOrEmpty >> not)"
      "let shouldFloat = isFocused.View <||> hasValue"
      "let inputId = \"custom-field\""
      ""
      "let inputElement ="
      "  Doc.InputType.Text"
      "    ["
      "      Attr.Class \"weave-field__input\""
      "      attr.id inputId"
      "      on.focus (fun _ _ -> isFocused.Value <- true)"
      "      on.blur (fun _ _ -> isFocused.Value <- false)"
      "    ]"
      "    value"
      ""
      "Field.create ("
      "  inputElement,"
      "  isFocused.View,"
      "  shouldFloat,"
      "  labelText = View.Const \"Name\","
      "  inputId = inputId" + (if List.isEmpty attrs then "" else ",")
      match attrs with
      | [] -> ()
      | [ attr ] -> "  attrs = [ " + attr + " ]"
      | _ ->
        "  attrs = ["

        for attr in attrs do
          "    " + attr

        "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant, _ in variants do
      for color, _ in colors do
        for width, _ in widths do
          for label in [ false; true ] do
            if not label || variant = "outlined" then
              let current = {
                Variant = variant
                Color = color
                FullWidth = width
                LabelAbove = label
              }

              sprintf "%s %s %b %b" variant color width label, render current ""
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
      sprintf "Field value %d" (index + 1), render defaults value
  ]
