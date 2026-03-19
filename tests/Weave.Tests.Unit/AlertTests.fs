module Weave.Tests.Unit.AlertTests

open Expecto

// Alert.Variant and Alert.Color use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime toClass mapping tests needed.
[<Tests>]
let alertTests = testList "Alert" []
