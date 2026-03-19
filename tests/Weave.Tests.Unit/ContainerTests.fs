module Weave.Tests.Unit.ContainerTests

open Expecto

// Container maxWidth classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let containerTests = testList "Container" []
