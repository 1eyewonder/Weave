#load "../../src/Weave.Docs/examples/typography/TypographyAppearanceSource.fs"
open Weave.Docs
let cases = TypographyAppearanceSource.cases ()

for name, source in cases do
  for line in source.Split('\n') do
    if line.Length > 80 then
      failwithf "%s: line too long: %s" name line

printfn "Validated width of %d typography appearance cases" cases.Length
