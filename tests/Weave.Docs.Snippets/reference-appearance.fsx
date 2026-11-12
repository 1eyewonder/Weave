#load "../../src/Weave.Docs/examples/shared/ReferenceAppearanceSource.fs"
open Weave.Docs

let cases =
  ReferenceAppearanceSource.Link.cases ()
  @ ReferenceAppearanceSource.Tooltip.cases ()
  @ ReferenceAppearanceSource.Divider.cases ()

for name, source in cases do
  for line in source.Split('\n') do
    if line.Length > 80 then
      failwithf "%s: line too long: %s" name line

printfn "Validated width of %d reference appearance cases" cases.Length
