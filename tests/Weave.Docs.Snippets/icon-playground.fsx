#load "../../src/Weave.Docs/examples/icons/IconPlaygroundSource.fs"
open Weave.Docs
let cases = IconPlaygroundSource.cases ()

for title, source in cases do
  for line in source.Split('\n') do
    if line.Length > 80 then
      failwithf "%s: line too long: %s" title line

printfn "Validated %d icon playground cases" cases.Length
