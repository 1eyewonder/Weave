#load "../../src/Weave.Docs/examples/list/ListAppearanceSource.fs"
open Weave.Docs
let cases = ListAppearanceSource.cases ()

for title, source in cases do
  for line in source.Split('\n') do
    if line.Length > 80 then
      failwithf "%s: line too long: %s" title line

printfn "Validated %d list cases" cases.Length
