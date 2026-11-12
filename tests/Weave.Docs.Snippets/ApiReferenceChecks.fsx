#r "../../src/Weave.Docs/bin/Release/net10.0/WebSharper.Core.dll"
#load "../../src/Weave.Docs/examples/shared/ApiReference.fs"

open Weave.Docs.Examples.ApiReference

let mutable checks = 0

let equal label expected actual =
  checks <- checks + 1

  if expected <> actual then
    failwithf "%s: expected %A, got %A" label expected actual

let parameter name typ defaultValue description : Parameter = {
  Name = name
  Type = typ
  Default = defaultValue
  Description = description
}

let definitions = [
  Note "Leading guidance."
  Note "Second leading note."
  Parameters(
    "Widget.create",
    [
      parameter "value" "Var<int>" "1" "Primary value"
      parameter "enabled" "View<bool>" "View.Const true" "Allows activation"
      parameter "attrs" "Attr list" "[]" "Styling attributes"
    ],
    Some "Doc"
  )
  Note "Keyboard handles Enter."
  Note "Escape closes the menu."
  Options("Widget.Color", [ "primary", "Brand palette"; "error", "Danger palette" ])
  Note "Choose a contrast-safe role."
  Parameters("Widget.reset", [], Some "unit")
  Note "Clears previous state."
]

let groups = group definitions
let create = groups.[0]
let colors = groups.[1]
let reset = groups.[2]
let filtered query item = filterRows (normalise query) item
let names rows = rows |> List.map (fun row -> row.Name)
let keys rows = rows |> List.map (fun row -> row.Key)

equal
  "group declaration order"
  [ "Widget.create"; "Widget.Color"; "Widget.reset" ]
  (groups |> List.map _.Title)

equal "leading note order" [ "Leading guidance."; "Second leading note." ] create.IntroNotes
equal "following note order" [ "Keyboard handles Enter."; "Escape closes the menu." ] create.Notes
equal "leading notes do not attach to later groups" [] colors.IntroNotes
equal "following note stays with preceding options" [ "Choose a contrast-safe role." ] colors.Notes
equal "later member note stays separate" [ "Clears previous state." ] reset.Notes
equal "parameter group kind" true create.IsParameterTable
equal "options group kind" false colors.IsParameterTable
equal "return type retained" (Some "Doc") create.ReturnType
equal "options have no return type" None colors.ReturnType

equal
  "leading note context is searchable"
  [ "value"; "enabled"; "attrs" ]
  (filtered "leading" create |> names)

equal "empty query" [] (normalise "")
equal "whitespace and case" [ "view<bool>"; "allows" ] (normalise " \t ViEW<bool>\r\n Allows \n")
equal "null query is empty" [] (normalise null)
equal "name search" [ "enabled" ] (filtered "ENABLED" create |> names)
equal "type search" [ "value" ] (filtered "Var<int>" create |> names)
equal "default search" [ "enabled" ] (filtered "view.const TRUE" create |> names)
equal "description search" [ "attrs" ] (filtered "styling" create |> names)
equal "AND across fields" [ "enabled" ] (filtered "view<bool> activation" create |> names)
equal "AND does not combine separate rows" [] (filtered "value styling" create |> names)
equal "literal punctuation" [ "attrs" ] (filtered "[]" create |> names)
equal "punctuation is not a regex" [] (filtered ".*" create |> names)

equal
  "title match includes all member rows"
  [ "value"; "enabled"; "attrs" ]
  (filtered "widget.create" create |> names)

equal
  "return type match includes all member rows"
  [ "value"; "enabled"; "attrs" ]
  (filtered "Doc" create |> names)

equal
  "note match includes all member rows"
  [ "value"; "enabled"; "attrs" ]
  (filtered "keyboard" create |> names)

equal "note search does not leak to next group" false (matches (normalise "keyboard") colors)
equal "style value search" [ "error" ] (filtered "error" colors |> names)
equal "style description search" [ "primary" ] (filtered "brand" colors |> names)
equal "zero rows" [] (filtered "no-such-reference" create)
equal "zero group result" false (matches (normalise "no-such-reference") create)

equal "source ordinal keys" [ "0"; "1"; "2" ] (keys create.Rows)
equal "filter preserves original key" [ "1" ] (filtered "enabled" create |> keys)
equal "clear restores declaration order" [ "value"; "enabled"; "attrs" ] (filtered "" create |> names)
equal "filter leaves source unchanged" [ "value"; "enabled"; "attrs" ] (names create.Rows)
equal "parameter total count" 3 (totalCount create)
equal "visible matching row count" 1 (visibleCount (normalise "enabled") create)
equal "visible contextual count" 3 (visibleCount (normalise "Doc") create)
equal "visible zero count" 0 (visibleCount (normalise "no-such-reference") create)
equal "no-argument member matches title" true (matches (normalise "reset") reset)
equal "no-argument member matches return type" true (matches (normalise "unit") reset)
equal "no-argument member matches note" true (matches (normalise "previous") reset)
equal "no-argument member does not invent rows" [] (filtered "reset" reset)
equal "no-argument total count" 1 (totalCount reset)
equal "no-argument visible count" 1 (visibleCount (normalise "reset") reset)
equal "no-argument hidden count" 0 (visibleCount (normalise "no-such-reference") reset)

let duplicateNames =
  group [
    Parameters(
      "Overloads",
      [
        parameter "value" "int" "0" "First overload"
        parameter "value" "string" "" "Second overload"
      ],
      None
    )
  ]
  |> List.head

equal "duplicate names retain both rows" [ "value"; "value" ] (names duplicateNames.Rows)
equal "duplicate names have independent keys" [ "0"; "1" ] (keys duplicateNames.Rows)
equal "duplicate-name filtering keeps source key" [ "1" ] (filtered "second" duplicateNames |> keys)

let notesOnly = group [ Note "First note."; Note "Second note." ] |> List.head

equal "notes-only content preserved" [ "First note."; "Second note." ] notesOnly.IntroNotes
equal "notes-only fallback has no trailing notes" [] notesOnly.Notes
equal "notes-only heading is empty" "" notesOnly.Title
equal "notes-only group matches content" true (matches (normalise "second") notesOnly)
equal "notes-only group can be excluded" false (matches (normalise "no-such-reference") notesOnly)
equal "notes-only group is not a counted member" 0 (totalCount notesOnly)
equal "notes-only group has zero visible entries" 0 (visibleCount [] notesOnly)
equal "empty definition sequence" [] (group [])

printfn "API reference model: %d contract checks passed." checks
