namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module PaginationAppearance =
  type Configuration = {
    Variant: string
    Color: string
    FirstLast: bool
    Disabled: bool
  }

  let defaults = {
    Variant = "text"
    Color = "primary"
    FirstLast = false
    Disabled = false
  }

  let variants = [ "text"; "filled"; "outlined" ]
  let colors = [ "primary"; "secondary"; "tertiary"; "error"; "warning"; "success"; "info" ]

  let attributes config = [
    if config.Variant <> "text" then
      "Pagination.Variant." + config.Variant
    if config.Color <> "primary" then
      "Pagination.Color." + config.Color
    if config.FirstLast then
      "Pagination.Controls.showFirstLast"
  ]

  let source config page =
    [
      sprintf "let page = Var.Create %d" page
      ""
      "Pagination.create ("
      "  View.Const 3,"
      "  selectedPage = page,"
      if config.Disabled then
        "  enabled = View.Const false,"
      "  attrs = ["
      "    Attr.Create \"aria-label\" \"Collection pages\""
      yield! attributes config |> List.map (fun attr -> "    " + attr)
      "  ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for variant in variants do
      for color in colors do
        for firstLast in [ false; true ] do
          for disabled in [ false; true ] do
            for page in 1..3 do
              let config = {
                Variant = variant
                Color = color
                FirstLast = firstLast
                Disabled = disabled
              }

              yield sprintf "%s %s %b %b %d" variant color firstLast disabled page, source config page
  ]
