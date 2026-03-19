module Weave.Tests.Unit.ChipTests

open Expecto

// Chip variant, density, and color use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let chipTests = testList "Chip" []
