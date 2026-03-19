module Weave.Tests.Unit.LinkTests

open Expecto

// Link modifier classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let linkTests = testList "Link" []
