module Weave.Tests.Unit.DrawerTests

open Expecto

// Drawer modifier classes use flat Attr let-bindings validated at compile
// time by TypedCssClasses — no runtime mapping tests needed.
[<Tests>]
let drawerTests = testList "Drawer" []
