module Weave.Tests.Unit.FieldTests

open Expecto

// Field modifier classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let fieldTests = testList "Field" []
