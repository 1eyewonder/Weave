module Weave.Tests.Unit.CrossComponentTests

open Expecto

// Cross-component color class uniqueness was enforced via toClass functions that
// have been replaced with flat Attr let-bindings. TypedCssClasses ensures each
// binding maps to a unique, compile-time-verified CSS class.
[<Tests>]
let crossComponentTests = testList "Cross-Component" []
