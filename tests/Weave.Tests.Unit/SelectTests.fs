module Weave.Tests.Unit.SelectTests

open Expecto

// Select color and width use flat Attr let-bindings validated at compile time
// by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let selectTests = testList "Select" []
