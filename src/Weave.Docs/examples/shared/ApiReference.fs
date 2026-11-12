namespace Weave.Docs.Examples

open WebSharper

[<JavaScript>]
module ApiReference =

  type Parameter = {
    Name: string
    Type: string
    Default: string
    Description: string
  }

  type Row = {
    Key: string
    Name: string
    Type: string
    Default: string
    Description: string
  }

  type Definition =
    | Parameters of title: string * rows: Parameter list * returns: string option
    | Options of title: string * values: (string * string) list
    | Note of string

  type Group = {
    Title: string
    IsParameterTable: bool
    Rows: Row list
    ReturnType: string option
    IntroNotes: string list
    Notes: string list
  }

  let private parameterRows (rows: Parameter list) : Row list =
    rows
    |> List.mapi (fun index row -> {
      Key = string index
      Name = row.Name
      Type = row.Type
      Default = row.Default
      Description = row.Description
    })

  let private optionRows (values: (string * string) list) : Row list =
    values
    |> List.mapi (fun index (name, description) -> {
      Key = string index
      Name = name
      Type = ""
      Default = ""
      Description = description
    })

  /// Notes before the first table belong to it; later notes belong to the preceding table.
  let group (definitions: Definition list) : Group list =
    let rec collect leading completed remaining =
      match remaining with
      | [] ->
        match completed with
        | [] when not (List.isEmpty leading) -> [
            {
              Title = ""
              IsParameterTable = false
              Rows = []
              ReturnType = None
              IntroNotes = List.rev leading
              Notes = []
            }
          ]
        | _ ->
          completed
          |> List.rev
          |> List.map (fun item -> { item with Notes = List.rev item.Notes })
      | Note note :: rest ->
        match completed with
        | current :: previous ->
          collect leading ({ current with Notes = note :: current.Notes } :: previous) rest
        | [] -> collect (note :: leading) [] rest
      | Parameters(title, rows, returns) :: rest ->
        let next = {
          Title = title
          IsParameterTable = true
          Rows = parameterRows rows
          ReturnType = returns
          IntroNotes = List.rev leading
          Notes = []
        }

        collect [] (next :: completed) rest
      | Options(title, values) :: rest ->
        let next = {
          Title = title
          IsParameterTable = false
          Rows = optionRows values
          ReturnType = None
          IntroNotes = List.rev leading
          Notes = []
        }

        collect [] (next :: completed) rest

    collect [] [] definitions

  let normalise (query: string) : string list =
    if isNull query then
      []
    else
      query.Trim().ToLower().Split([| ' '; '\t'; '\r'; '\n' |], System.StringSplitOptions.RemoveEmptyEntries)
      |> Array.toList

  let private containsAll (terms: string list) (value: string) =
    let searchable = value.ToLower()
    terms |> List.forall (fun term -> searchable.Contains term)

  let private context (item: Group) =
    String.concat " " [
      item.Title
      defaultArg item.ReturnType ""
      yield! item.IntroNotes
      yield! item.Notes
    ]

  /// Literal AND terms may match different fields of the same row and its group context.
  let filterRows (terms: string list) (item: Group) : Row list =
    let shared = context item

    item.Rows
    |> List.filter (fun row ->
      String.concat " " [ shared; row.Name; row.Type; row.Default; row.Description ]
      |> containsAll terms)

  let matches (terms: string list) (item: Group) =
    if List.isEmpty item.Rows then
      containsAll terms (context item)
    else
      not (List.isEmpty (filterRows terms item))

  /// A no-argument member is one reference entry; a notes-only group is not a member.
  let totalCount (item: Group) =
    if System.String.IsNullOrEmpty item.Title && List.isEmpty item.Rows then
      0
    else
      max 1 item.Rows.Length

  let visibleCount (terms: string list) (item: Group) =
    if totalCount item = 0 then
      0
    elif List.isEmpty item.Rows then
      if matches terms item then 1 else 0
    else
      (filterRows terms item).Length
