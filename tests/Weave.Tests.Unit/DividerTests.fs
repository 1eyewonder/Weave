module Weave.Tests.Unit.DividerTests

open Expecto

// Divider orientation and variant use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let dividerTests = testList "Divider" []
