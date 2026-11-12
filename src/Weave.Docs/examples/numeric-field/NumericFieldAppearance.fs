namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module NumericFieldAppearance =

  type Configuration = {
    Variant: string
    Color: string
    FullWidth: bool
    SpinButtons: bool
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
    SpinButtons = true
  }

  let attributeNames current = [
    if current.Variant <> "standard" then
      yield "NumericField.Variant." + current.Variant
    if current.Color <> "default" then
      yield "NumericField.Color." + current.Color
    if current.FullWidth then
      yield "NumericField.Width.full"
  ]

  let render current value =
    let attrs = attributeNames current

    [
      sprintf "let quantity = Var.Create %s" (if value < 0 then sprintf "(%d)" value else string value)
      ""
      if current.SpinButtons && List.isEmpty attrs then
        "NumericField.create (quantity, labelText = View.Const \"Quantity\")"
      else
        "NumericField.create ("
        "  quantity,"

        "  labelText = View.Const \"Quantity\""
        + (if not current.SpinButtons || not (List.isEmpty attrs) then
             ","
           else
             "")

        if not current.SpinButtons then
          "  showSpinButtons = View.Const false"
          + (if List.isEmpty attrs then "" else ",")

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
          for spin in [ false; true ] do
            let current = {
              Variant = variant
              Color = color
              FullWidth = width
              SpinButtons = spin
            }

            for value in [ -2; 1 ] do
              sprintf "%s %s %b %b %d" variant color width spin value, render current value
  ]
