#load "../../src/Weave.Docs/examples/shared/DataAppearanceSource.fs"
open Weave.Docs

let cases =
  DataAppearanceSource.Highlighter.cases () @ DataAppearanceSource.Table.cases ()

for title, source in cases do
  for line in source.Split('\n') do
    if line.Length > 80 then
      failwithf "%s: line too long: %s" title line

printfn "Validated %d generated data appearance cases" cases.Length
