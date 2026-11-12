namespace Weave.Docs
#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ListAppearanceSource =
  type Configuration = {
    Mode: string
    Density: string
    Bordered: bool
    ReadOnly: bool
  }

  let defaults = {
    Mode = "Single"
    Density = "Standard"
    Bordered = true
    ReadOnly = false
  }

  let modes = [ "Single"; "Toggle"; "Multiple" ]
  let densities = [ "Compact"; "Standard"; "Spacious" ]

  let source config selected (selectedMany: string list) =
    let selection =
      if config.Mode = "Multiple" then
        let values = selectedMany |> List.map (sprintf "\"%s\"")

        let singleLine =
          "let selected = Var.Create<Set<string>> (Set.ofList [ "
          + String.concat "; " values
          + " ])"

        if singleLine.Length <= 80 then
          [ singleLine ]
        else
          [
            "let selected ="
            "  Var.Create<Set<string>> (Set.ofList ["
            yield! values |> List.map (fun value -> "    " + value)
            "  ])"
          ]
      else
        [
          "let selected = Var.Create<string option> "
          + (match selected with
             | Some value -> "(Some \"" + value + "\")"
             | None -> "None")
        ]

    let args = [
      if config.Mode = "Multiple" then
        "selectedValues = selected"
      else
        "selectedValue = selected"
      if config.Mode = "Toggle" then
        "selectionMode = WeaveList.SelectionMode.ToggleSelection"
      if not config.Bordered then
        "bordered = View.Const false"
      if config.ReadOnly then
        "readOnly = View.Const true"
      if config.Density = "Compact" then
        "attrs = [ Density.compact ]"
      if config.Density = "Spacious" then
        "attrs = [ Density.spacious ]"
    ]

    [
      "open WeaveList"
      ""
      yield! selection
      ""
      "WeaveList.create ("
      "  ["
      "    ListItem.create (text \"Inbox\", value = \"Inbox\")"
      "    ListItem.create (text \"Sent\", value = \"Sent\")"
      "    ListItem.create (text \"Drafts\", value = \"Drafts\")"
      "  ],"
      for i, arg in args |> List.indexed do
        "  " + arg + (if i < args.Length - 1 then "," else "")
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    let items = [ "Drafts"; "Inbox"; "Sent" ]

    for mode in modes do
      for density in densities do
        for bordered in [ false; true ] do
          for readOnly in [ false; true ] do
            let c = {
              Mode = mode
              Density = density
              Bordered = bordered
              ReadOnly = readOnly
            }

            if mode = "Multiple" then
              for mask in 0..7 do
                let selected =
                  items
                  |> List.indexed
                  |> List.choose (fun (index, value) ->
                    if mask &&& (1 <<< index) <> 0 then Some value else None)

                sprintf "List %s %s %b %b subset %d" mode density bordered readOnly mask,
                source c None selected
            else
              for selected in None :: (items |> List.map Some) do
                sprintf "List %s %s %b %b %A" mode density bordered readOnly selected, source c selected []
  ]
