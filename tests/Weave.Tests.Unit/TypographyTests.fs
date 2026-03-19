module Weave.Tests.Unit.TypographyTests

open Expecto

// Typography variant, align, and color use flat Attr let-bindings validated at
// compile time by TypedCssClasses — no runtime mapping tests needed.
// The internal Typo DU (used by Typography.Text.create) is not public API.
[<Tests>]
let typographyTests = testList "Typography" []
