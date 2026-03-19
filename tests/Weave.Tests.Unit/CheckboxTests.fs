module Weave.Tests.Unit.CheckboxTests

open Expecto

// Checkbox size and color use flat Attr let-bindings validated at compile time
// by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let checkboxTests = testList "Checkbox" []
