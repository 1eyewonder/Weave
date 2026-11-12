namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ButtonMenuAppearance =

  let directions = [ "Top"; "Bottom"; "Left"; "Right" ]
  let variants = [ "Filled"; "Outlined"; "Text" ]
  let colors = [ "Primary"; "Secondary"; "Tertiary"; "Error"; "Warning"; "Success"; "Info" ]

  let source direction (variant: string) (color: string) swapIcon =
    [
      "open Weave.Icons.MaterialSymbols"
      ""
      "IconButtonMenu.create ("
      "  closedIcon = Icon.create (Icon.UiActions UiActions.Add),"
      if swapIcon then
        "  openIcon = Icon.create (Icon.UiActions UiActions.Close),"
      "  items = ["
      "    IconButton.info ("
      "      Icon.create (Icon.Action Action.Alarm),"
      "      onClick = ignore,"
      "      attrs = [ Attr.Create \"aria-label\" \"Alarm\" ]"
      "    )"
      "    IconButton.warning ("
      "      Icon.create (Icon.UiActions UiActions.Star),"
      "      onClick = ignore,"
      "      attrs = [ Attr.Create \"aria-label\" \"Star\" ]"
      "    )"
      "  ],"
      if direction <> "Top" then
        "  direction = ButtonMenu.Direction." + direction + ","
      "  triggerAttrs = ["
      "    Button.Variant." + variant.ToLower()
      "    Button.Color." + color.ToLower()
      "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for direction in directions do
      for variant in variants do
        for color in colors do
          for swap in [ false; true ] do
            sprintf "%s %s %s %b" direction variant color swap, source direction variant color swap
  ]
