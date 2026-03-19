module Weave.Tests.Unit.AnimationTests

open Expecto
open Weave.CssHelpers

// AnimationEntrance/Exit/Emphasis/Duration/Easing/Iteration/On/Kind use flat
// Attr let-bindings. TypedCssClasses enforces correctness at compile time.
// AnimationEntrance/Exit DU types are kept for AnimationPair typed fields.
//
// AnimationDelay.stagger returns Attr directly. WebSharper.UI.Attr does not
// support structural equality, so stagger values cannot be compared at test
// time. The clamping logic (max 1 (min 10 step)) is trivial; the TypeProvider
// ensures the generated class name exists in the compiled CSS at build time.

[<Tests>]
let animationTests =
  testList "Animation" [

    testList "AnimationPair" [
      testTheory "predefined pairs have correct typed enter and exit" [
        AnimationPair.fadeInOut, AnimationEntrance.FadeIn, AnimationExit.FadeOut
        AnimationPair.scaleInOut, AnimationEntrance.ScaleIn, AnimationExit.ScaleOut
        AnimationPair.scaleYInOut, AnimationEntrance.ScaleYIn, AnimationExit.ScaleYOut
        AnimationPair.slideUpInOut, AnimationEntrance.SlideUpIn, AnimationExit.SlideDownOut
        AnimationPair.slideDownInOut, AnimationEntrance.SlideDownIn, AnimationExit.SlideUpOut
        AnimationPair.slideLeftInOut, AnimationEntrance.SlideLeftIn, AnimationExit.SlideRightOut
        AnimationPair.slideRightInOut, AnimationEntrance.SlideRightIn, AnimationExit.SlideLeftOut
      ]
      <| fun (pair, expectedEnter, expectedExit) ->
        Expect.equal pair.Enter expectedEnter "enter type"
        Expect.equal pair.Exit expectedExit "exit type"

      testCase "AnimationPair.create constructs correctly"
      <| fun () ->
        let pair = AnimationPair.create AnimationEntrance.ScaleIn AnimationExit.FadeOut
        Expect.equal pair.Enter AnimationEntrance.ScaleIn "enter"
        Expect.equal pair.Exit AnimationExit.FadeOut "exit"
    ]

  ]
