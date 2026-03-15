module Weave.Tests.Unit.OpacityTests

open Expecto
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<Tests>]
let opacityTests =
  testList "Opacity" [

    testTheory "each percentage maps to the correct class" [
      for i in 0..100 do
        yield i, $"weave-opacity-{i}"
    ]
    <| fun (percent, expected) -> Expect.equal (Opacity.create percent |> Opacity.toClass) expected ""

  ]
