namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module DataAppearanceSource =
  module Highlighter =
    type Configuration = {
      Term: string
      Color: string
      Sensitive: bool
    }

    let defaults = {
      Term = "item"
      Color = "Warning"
      Sensitive = false
    }

    let terms = [ "item"; "Item"; "other" ]
    let colors = [ "Warning"; "Primary"; "Secondary"; "Tertiary"; "Success"; "Error"; "Info" ]

    let source config =
      [
        "Highlighter.create ("
        "  View.Const \"This Item is an item.\","
        "  View.Const \""
        + config.Term
        + "\""
        + (if config.Sensitive || config.Color <> "Warning" then
             ","
           else
             "")
        if config.Sensitive then
          "  caseSensitive = View.Const true"
          + (if config.Color <> "Warning" then "," else "")
        if config.Color <> "Warning" then
          "  attrs = [ Highlighter.Color." + config.Color.ToLower() + " ]"
        ")"
      ]
      |> String.concat "\n"

    let cases () = [
      for term in terms do
        for color in colors do
          for sensitive in [ false; true ] do
            let config = {
              Term = term
              Color = color
              Sensitive = sensitive
            }

            sprintf "Highlighter %s %s %b" term color sensitive, source config
    ]

  module Table =
    type Configuration = {
      Density: string
      Striped: bool
      Hoverable: bool
    }

    let defaults = {
      Density = "Standard"
      Striped = false
      Hoverable = false
    }

    let densities = [ "Compact"; "Standard"; "Spacious" ]

    let source config =
      let attrs = [
        if config.Density = "Compact" then
          "Table.Density.compact"
        if config.Density = "Spacious" then
          "Table.Density.spacious"
        if config.Striped then
          "Table.striped"
        if config.Hoverable then
          "Table.hoverable"
      ]

      [
        "let rows = [ \"Apple\", 42; \"Banana\", 17; \"Cherry\", 120 ]"
        ""
        "let columns = ["
        "  TableColumn.create (\"fruit\", text \"Fruit\", (fun (name, _) -> text name))"
        "  TableColumn.create ("
        "    \"stock\", text \"Stock\", (fun (_, stock) -> text (string<int> stock))"
        "  )"
        "]"
        ""
        if attrs.IsEmpty then
          "Table.create (columns, View.Const rows, fst, caption = text \"Fruit stock\")"
        else
          "Table.create ("
          "  columns, View.Const rows, fst,"
          "  caption = text \"Fruit stock\","

          match attrs with
          | [ attr ] -> "  attrs = [ " + attr + " ]"
          | _ ->
            "  attrs = ["
            yield! attrs |> List.map (fun attr -> "    " + attr)
            "  ]"

          ")"
      ]
      |> String.concat "\n"

    let cases () = [
      for density in densities do
        for striped in [ false; true ] do
          for hoverable in [ false; true ] do
            let config = {
              Density = density
              Striped = striped
              Hoverable = hoverable
            }

            sprintf "Table %s %b %b" density striped hoverable, source config
    ]
