module Weave.Tests.Unit.AnimationTests

open Expecto
open Weave.CssHelpers

[<Tests>]
let animationTests =
  testList "Animation" [

    testList "AnimationEntrance.toClass" [
      testTheory "each entrance maps to the correct class" [
        AnimationEntrance.FadeIn, "weave-animation--fade-in"
        AnimationEntrance.ScaleIn, "weave-animation--scale-in"
        AnimationEntrance.ScaleYIn, "weave-animation--scale-y-in"
        AnimationEntrance.SlideUpIn, "weave-animation--slide-up-in"
        AnimationEntrance.SlideDownIn, "weave-animation--slide-down-in"
        AnimationEntrance.SlideLeftIn, "weave-animation--slide-left-in"
        AnimationEntrance.SlideRightIn, "weave-animation--slide-right-in"
      ]
      <| fun (entrance, expected) -> Expect.equal (AnimationEntrance.toClass entrance) expected ""

      testCase "all entrances produce distinct classes"
      <| fun () ->
        let classes =
          [
            AnimationEntrance.FadeIn
            AnimationEntrance.ScaleIn
            AnimationEntrance.ScaleYIn
            AnimationEntrance.SlideUpIn
            AnimationEntrance.SlideDownIn
            AnimationEntrance.SlideLeftIn
            AnimationEntrance.SlideRightIn
          ]
          |> List.map AnimationEntrance.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each entrance maps to a unique class"
    ]

    testList "AnimationExit.toClass" [
      testTheory "each exit maps to the correct class" [
        AnimationExit.FadeOut, "weave-animation--fade-out"
        AnimationExit.ScaleOut, "weave-animation--scale-out"
        AnimationExit.ScaleYOut, "weave-animation--scale-y-out"
        AnimationExit.SlideUpOut, "weave-animation--slide-up-out"
        AnimationExit.SlideDownOut, "weave-animation--slide-down-out"
        AnimationExit.SlideLeftOut, "weave-animation--slide-left-out"
        AnimationExit.SlideRightOut, "weave-animation--slide-right-out"
      ]
      <| fun (exit, expected) -> Expect.equal (AnimationExit.toClass exit) expected ""

      testCase "all exits produce distinct classes"
      <| fun () ->
        let classes =
          [
            AnimationExit.FadeOut
            AnimationExit.ScaleOut
            AnimationExit.ScaleYOut
            AnimationExit.SlideUpOut
            AnimationExit.SlideDownOut
            AnimationExit.SlideLeftOut
            AnimationExit.SlideRightOut
          ]
          |> List.map AnimationExit.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each exit maps to a unique class"
    ]

    testList "AnimationEmphasis.toClass" [
      testTheory "each emphasis maps to the correct class" [
        AnimationEmphasis.Pulse, "weave-animation--pulse"
        AnimationEmphasis.Shake, "weave-animation--shake"
        AnimationEmphasis.Bounce, "weave-animation--bounce"
      ]
      <| fun (emphasis, expected) -> Expect.equal (AnimationEmphasis.toClass emphasis) expected ""

      testCase "all emphasis produce distinct classes"
      <| fun () ->
        let classes =
          [ AnimationEmphasis.Pulse; AnimationEmphasis.Shake; AnimationEmphasis.Bounce ]
          |> List.map AnimationEmphasis.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each emphasis maps to a unique class"
    ]

    testList "AnimationKind.toClass" [
      testTheory "delegates to the correct sub-DU toClass" [
        AnimationKind.Entrance AnimationEntrance.FadeIn, "weave-animation--fade-in"
        AnimationKind.Entrance AnimationEntrance.ScaleIn, "weave-animation--scale-in"
        AnimationKind.Exit AnimationExit.FadeOut, "weave-animation--fade-out"
        AnimationKind.Exit AnimationExit.ScaleOut, "weave-animation--scale-out"
        AnimationKind.Emphasis AnimationEmphasis.Pulse, "weave-animation--pulse"
        AnimationKind.Emphasis AnimationEmphasis.Shake, "weave-animation--shake"
        AnimationKind.Suppress, "weave-animation--none"
      ]
      <| fun (kind, expected) -> Expect.equal (AnimationKind.toClass kind) expected ""
    ]

    testList "AnimationDuration.toClass" [
      testTheory "each duration maps to the correct class" [
        AnimationDuration.Shortest, "weave-animation-duration--shortest"
        AnimationDuration.Shorter, "weave-animation-duration--shorter"
        AnimationDuration.Short, "weave-animation-duration--short"
        AnimationDuration.Standard, "weave-animation-duration--standard"
        AnimationDuration.Medium, "weave-animation-duration--medium"
        AnimationDuration.Long, "weave-animation-duration--long"
        AnimationDuration.Longer, "weave-animation-duration--longer"
        AnimationDuration.Longest, "weave-animation-duration--longest"
      ]
      <| fun (duration, expected) -> Expect.equal (AnimationDuration.toClass duration) expected ""

      testCase "all durations produce distinct classes"
      <| fun () ->
        let classes =
          [
            AnimationDuration.Shortest
            AnimationDuration.Shorter
            AnimationDuration.Short
            AnimationDuration.Standard
            AnimationDuration.Medium
            AnimationDuration.Long
            AnimationDuration.Longer
            AnimationDuration.Longest
          ]
          |> List.map AnimationDuration.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each duration maps to a unique class"
    ]

    testList "AnimationEasing.toClass" [
      testTheory "each easing maps to the correct class" [
        AnimationEasing.Standard, "weave-animation-easing--standard"
        AnimationEasing.Decelerate, "weave-animation-easing--decelerate"
        AnimationEasing.Accelerate, "weave-animation-easing--accelerate"
        AnimationEasing.Bounce, "weave-animation-easing--bounce"
      ]
      <| fun (easing, expected) -> Expect.equal (AnimationEasing.toClass easing) expected ""

      testCase "all easings produce distinct classes"
      <| fun () ->
        let classes =
          [
            AnimationEasing.Standard
            AnimationEasing.Decelerate
            AnimationEasing.Accelerate
            AnimationEasing.Bounce
          ]
          |> List.map AnimationEasing.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each easing maps to a unique class"
    ]

    testList "AnimationIteration.toClass" [
      testCase "Infinite maps to the correct class"
      <| fun () ->
        Expect.equal
          (AnimationIteration.toClass AnimationIteration.Infinite)
          "weave-animation-iteration--infinite"
          ""
    ]

    testList "AnimationDelay.stagger" [
      testTheory "each step maps to the correct class" [
        1, "weave-animation-delay--1"
        5, "weave-animation-delay--5"
        10, "weave-animation-delay--10"
      ]
      <| fun (step, expected) -> Expect.equal (AnimationDelay.stagger step) expected ""

      testCase "steps below 1 are clamped to 1"
      <| fun () ->
        Expect.equal (AnimationDelay.stagger 0) "weave-animation-delay--1" ""
        Expect.equal (AnimationDelay.stagger -5) "weave-animation-delay--1" ""

      testCase "steps above 10 are clamped to 10"
      <| fun () ->
        Expect.equal (AnimationDelay.stagger 11) "weave-animation-delay--10" ""
        Expect.equal (AnimationDelay.stagger 100) "weave-animation-delay--10" ""
    ]

    testList "AnimationOn.toClass" [
      testTheory "each trigger maps to the correct class" [
        AnimationOn.Hover, "weave-animation-on--hover"
        AnimationOn.Focus, "weave-animation-on--focus"
        AnimationOn.HoverFocus, "weave-animation-on--hover-focus"
      ]
      <| fun (trigger, expected) -> Expect.equal (AnimationOn.toClass trigger) expected ""

      testCase "all triggers produce distinct classes"
      <| fun () ->
        let classes =
          [ AnimationOn.Hover; AnimationOn.Focus; AnimationOn.HoverFocus ]
          |> List.map AnimationOn.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each trigger maps to a unique class"
    ]

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
