module Weave.Tests.Unit.ExpansionPanelTests

open Expecto

// ExpansionPanel modifier classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let expansionPanelTests = testList "ExpansionPanel" []
