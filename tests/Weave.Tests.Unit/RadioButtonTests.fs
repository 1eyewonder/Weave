module Weave.Tests.Unit.RadioButtonTests

open Expecto

// Radio size and color use flat Attr let-bindings validated at compile time
// by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let radioButtonTests = testList "Radio" []
