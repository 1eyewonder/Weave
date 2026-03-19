module Weave.Tests.Unit.ListTests

open Expecto

// WeaveList modifier classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let listTests = testList "List" []
