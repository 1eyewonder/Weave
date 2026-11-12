namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module SpacerAppearance =

  type Configuration = {
    Direction: string
    Count: int
  }

  let directions = [ "row", "Row"; "column", "Column" ]
  let counts = [ 1, "One"; 2, "Two" ]

  let defaults = {
    Direction = "row"
    Count = 2
  }

  let render current =
    [
      "let action label ="
      "  Button.primary ("
      "    text label,"
      "    onClick = (fun () -> ()),"
      "    attrs = [ Button.Variant.filled ]"
      "  )"
      ""
      "div ["
      "  Flex.Flex.allSizes"
      if current.Direction = "column" then
        "  FlexDirection.Column.allSizes"
        "  Attr.Style \"height\" \"180px\""
      else
        "  AlignItems.center"
        "  Attr.Style \"width\" \"100%\""
      "] ["
      "  action \"One\""
      "  Spacer.create ()"
      "  action \"Two\""
      if current.Count = 2 then
        "  Spacer.create ()"
        "  action \"Three\""
      "]"
    ]
    |> String.concat "\n"

  let cases () = [
    for direction, _ in directions do
      for count, _ in counts do
        let current = {
          Direction = direction
          Count = count
        }

        sprintf "%s %d" direction count, render current
  ]
