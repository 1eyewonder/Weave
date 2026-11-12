namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module GridSource =

  let justifyNames = [
    "flexStart"
    "center"
    "flexEnd"
    "spaceBetween"
    "spaceAround"
    "spaceEvenly"
  ]

  let spacingNames = [ "none"; "extraSmall"; "small"; "medium"; "large"; "extraLarge" ]

  let private spanName span =
    match span with
    | 1 -> "one"
    | 2 -> "two"
    | 3 -> "three"
    | 4 -> "four"
    | 5 -> "five"
    | 6 -> "six"
    | 7 -> "seven"
    | 8 -> "eight"
    | 9 -> "nine"
    | 10 -> "ten"
    | 11 -> "eleven"
    | 12 -> "twelve"
    | _ -> invalidArg "span" "A grid span must be between 1 and 12."

  /// Generates the selected layout, including its visible public cell styling.
  let renderWithSpacing justifyName spacingName spans =
    if not (List.contains justifyName justifyNames) then
      invalidArg "justifyName" "Unknown grid justification."

    if not (List.contains spacingName spacingNames) then
      invalidArg "spacingName" "Unknown grid spacing."

    let items =
      spans
      |> List.mapi (fun index span ->
        sprintf "    item GridItem.Span.%s \"Item %d\"" (spanName span) (index + 1))

    [
      "let item span label ="
      "  let content ="
      "    Container.create ("
      "      Doc.Empty,"
      "      attrs = ["
      "        Attr.Create \"role\" \"img\""
      "        Attr.Create \"aria-label\" label"
      "        BrandColor.Fill.primary"
      "        BorderWidth.All.one"
      "        Attr.Style \"border-style\" \"solid\""
      "        BorderColor.linesDefault"
      "        BorderRadius.All.small"
      "        Attr.Style \"inline-size\" \"100%\""
      "        Attr.Style \"min-inline-size\" \"0\""
      "        Attr.Style \"box-sizing\" \"border-box\""
      "        Attr.Style \"block-size\" \"32px\""
      "      ]"
      "    )"
      ""
      "  GridItem.create (content, attrs = [ span ])"
      ""
      "Grid.create ("
      "  ["
      yield! items
      "  ],"
      sprintf "  attrs = [ JustifyContent.%s; Grid.Spacing.%s ]" justifyName spacingName
      ")"
    ]
    |> String.concat "\n"

  let render justifyName spans =
    renderWithSpacing justifyName "medium" spans

  let spacingCases () = [
    for spacing in spacingNames do
      for justify in justifyNames do
        sprintf "%s %s" justify spacing, renderWithSpacing justify spacing [ 3; 3 ]
  ]
