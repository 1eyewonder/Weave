module Weave.Tests.Unit.TooltipTests

open Expecto

// Tooltip modifier classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let tooltipTests = testList "Tooltip" []
