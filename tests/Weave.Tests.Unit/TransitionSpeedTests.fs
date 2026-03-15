module Weave.Tests.Unit.TransitionSpeedTests

open Expecto
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<Tests>]
let transitionSpeedTests =
  testList "TransitionSpeed" [

    testTheory "each speed maps to the correct class" [
      TransitionSpeed.None, "weave-transition--none"
      TransitionSpeed.Fast, "weave-transition--fast"
      TransitionSpeed.Standard, "weave-transition--standard"
      TransitionSpeed.Slow, "weave-transition--slow"
    ]
    <| fun (speed, expected) -> Expect.equal (TransitionSpeed.toClass speed) expected ""

  ]
