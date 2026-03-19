module Weave.Tests.Unit.SwitchTests

open Expecto

// Switch size and color use flat Attr let-bindings validated at compile time
// by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let switchTests = testList "Switch" []
