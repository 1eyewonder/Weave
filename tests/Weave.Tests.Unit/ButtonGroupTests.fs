module Weave.Tests.Unit.ButtonGroupTests

open Expecto

// ButtonGroup variant, orientation, density, and color use flat Attr let-bindings
// validated at compile time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let buttonGroupTests = testList "ButtonGroup" []
