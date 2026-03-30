module Weave.Tests.Unit.BoundedTests

open System
open Expecto
open Weave

[<Tests>]
let numericFieldTests =
  testList "Bounded" [

    testList "clamp with int" [
      testTheory "value below min is clamped to min" [ (0, 10, -5, 0); (5, 100, 3, 5); (-10, 10, -20, -10) ]
      <| fun (lo, hi, v, expected) -> Expect.equal (Bounded.clamp lo hi v) expected ""

      testTheory "value above max is clamped to max" [
        (0, 10, 15, 10)
        (5, 100, 200, 100)
        (-10, 10, 20, 10)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (Bounded.clamp lo hi v) expected ""

      testTheory "value in range is preserved" [
        (0, 10, 5, 5)
        (0, 10, 0, 0)
        (0, 10, 10, 10)
        (-100, 100, 0, 0)
        (-50, -10, -30, -30)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (Bounded.clamp lo hi v) expected ""

      testCase "min equals max returns that value"
      <| fun () ->
        Expect.equal (Bounded.clamp 7 7 0) 7 ""
        Expect.equal (Bounded.clamp 7 7 7) 7 ""
        Expect.equal (Bounded.clamp 7 7 100) 7 ""

      testTheory "negative ranges clamp correctly" [
        (-100, -10, -50, -50)
        (-100, -10, -200, -100)
        (-100, -10, 0, -10)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (Bounded.clamp lo hi v) expected ""

      testTheory "Int32 boundary values" [
        (Int32.MinValue, Int32.MaxValue, 0, 0)
        (Int32.MinValue, Int32.MaxValue, Int32.MinValue, Int32.MinValue)
        (Int32.MinValue, Int32.MaxValue, Int32.MaxValue, Int32.MaxValue)
        (0, Int32.MaxValue, -1, 0)
        (Int32.MinValue, 0, 1, 0)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (Bounded.clamp lo hi v) expected ""
    ]

    testList "clamp with float" [
      testTheory "value below min is clamped to min" [ (0.0, 10.0, -5.0, 0.0); (1.5, 9.5, 0.0, 1.5) ]
      <| fun (lo, hi, v, expected) -> Expect.equal (Bounded.clamp lo hi v) expected ""

      testTheory "value above max is clamped to max" [ (0.0, 10.0, 15.0, 10.0); (1.5, 9.5, 100.0, 9.5) ]
      <| fun (lo, hi, v, expected) -> Expect.equal (Bounded.clamp lo hi v) expected ""

      testTheory "value in range is preserved" [
        (0.0, 10.0, 5.5, 5.5)
        (0.0, 10.0, 0.0, 0.0)
        (0.0, 10.0, 10.0, 10.0)
        (-1.0, 1.0, 0.5, 0.5)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (Bounded.clamp lo hi v) expected ""

      testCase "infinity boundaries preserve in-range values"
      <| fun () ->
        Expect.equal (Bounded.clamp -infinity infinity 42.0) 42.0 ""
        Expect.equal (Bounded.clamp -infinity infinity 0.0) 0.0 ""

      testCase "infinity as max does not cap upward"
      <| fun () -> Expect.equal (Bounded.clamp 0.0 infinity 1e18) 1e18 ""

      testCase "negative infinity as min does not cap downward"
      <| fun () -> Expect.equal (Bounded.clamp -infinity 0.0 -1e18) -1e18 ""
    ]

    testList "stepUp with int" [
      testCase "increments within range"
      <| fun () -> Expect.equal (Bounded.stepUp 0 10 1 5) 6 ""

      testCase "clamps at max"
      <| fun () -> Expect.equal (Bounded.stepUp 0 10 5 8) 10 ""

      testCase "stays at max when already at max"
      <| fun () -> Expect.equal (Bounded.stepUp 0 10 1 10) 10 ""
    ]

    testList "stepDown with int" [
      testCase "decrements within range"
      <| fun () -> Expect.equal (Bounded.stepDown 0 10 1 5) 4 ""

      testCase "clamps at min"
      <| fun () -> Expect.equal (Bounded.stepDown 0 10 5 3) 0 ""

      testCase "stays at min when already at min"
      <| fun () -> Expect.equal (Bounded.stepDown 0 10 1 0) 0 ""
    ]

    testList "percentOf" [
      testCase "midpoint is 50%"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.percentOf 0.0 100.0 50.0) 50.0 ""

      testCase "min is 0%"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.percentOf 0.0 100.0 0.0) 0.0 ""

      testCase "max is 100%"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.percentOf 0.0 100.0 100.0) 100.0 ""

      testCase "below min clamps to 0%"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.percentOf 0.0 100.0 -10.0) 0.0 ""

      testCase "above max clamps to 100%"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.percentOf 0.0 100.0 200.0) 100.0 ""

      testCase "equal min and max returns 0%"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.percentOf 50.0 50.0 50.0) 0.0 ""

      testCase "custom range"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.percentOf 10.0 20.0 15.0) 50.0 ""
    ]

    testList "snapToStep" [
      testCase "snaps to nearest grid point"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.snapToStep 0.0 100.0 10.0 23.0) 20.0 ""

      testCase "snaps up when closer to upper grid point"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.snapToStep 0.0 100.0 10.0 27.0) 30.0 ""

      testCase "clamps to max when above"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.snapToStep 0.0 100.0 10.0 105.0) 100.0 ""

      testCase "clamps to min when below"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.snapToStep 0.0 100.0 10.0 -5.0) 0.0 ""

      testCase "zero step returns clamped value without snapping"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.snapToStep 0.0 100.0 0.0 42.7) 42.7 ""

      testCase "step 0.5 snaps to half"
      <| fun () -> Expect.floatClose Accuracy.high (Bounded.snapToStep 0.0 5.0 0.5 2.3) 2.5 ""
    ]
  ]
