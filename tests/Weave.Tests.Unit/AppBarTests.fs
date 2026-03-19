module Weave.Tests.Unit.AppBarTests

open Expecto

// AppBar modifier classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let appBarTests = testList "AppBar" []
