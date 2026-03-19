module Weave.Tests.Unit.TabsTests

open Expecto

// Tabs modifier classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let tabsTests = testList "Tabs" []
