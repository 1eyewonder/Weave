module Weave.Tests.Unit.NumericFieldTests

open System
open Expecto
open Weave

[<Tests>]
let numericFieldTests =
  testList "NumericField" [

    testList "clamp with int" [
      testTheory "value below min is clamped to min" [ (0, 10, -5, 0); (5, 100, 3, 5); (-10, 10, -20, -10) ]
      <| fun (lo, hi, v, expected) -> Expect.equal (NumericField.clamp lo hi v) expected ""

      testTheory "value above max is clamped to max" [
        (0, 10, 15, 10)
        (5, 100, 200, 100)
        (-10, 10, 20, 10)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (NumericField.clamp lo hi v) expected ""

      testTheory "value in range is preserved" [
        (0, 10, 5, 5)
        (0, 10, 0, 0)
        (0, 10, 10, 10)
        (-100, 100, 0, 0)
        (-50, -10, -30, -30)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (NumericField.clamp lo hi v) expected ""

      testCase "min equals max returns that value"
      <| fun () ->
        Expect.equal (NumericField.clamp 7 7 0) 7 ""
        Expect.equal (NumericField.clamp 7 7 7) 7 ""
        Expect.equal (NumericField.clamp 7 7 100) 7 ""

      testTheory "negative ranges clamp correctly" [
        (-100, -10, -50, -50)
        (-100, -10, -200, -100)
        (-100, -10, 0, -10)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (NumericField.clamp lo hi v) expected ""

      testTheory "Int32 boundary values" [
        (Int32.MinValue, Int32.MaxValue, 0, 0)
        (Int32.MinValue, Int32.MaxValue, Int32.MinValue, Int32.MinValue)
        (Int32.MinValue, Int32.MaxValue, Int32.MaxValue, Int32.MaxValue)
        (0, Int32.MaxValue, -1, 0)
        (Int32.MinValue, 0, 1, 0)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (NumericField.clamp lo hi v) expected ""
    ]

    testList "clamp with float" [
      testTheory "value below min is clamped to min" [ (0.0, 10.0, -5.0, 0.0); (1.5, 9.5, 0.0, 1.5) ]
      <| fun (lo, hi, v, expected) -> Expect.equal (NumericField.clamp lo hi v) expected ""

      testTheory "value above max is clamped to max" [ (0.0, 10.0, 15.0, 10.0); (1.5, 9.5, 100.0, 9.5) ]
      <| fun (lo, hi, v, expected) -> Expect.equal (NumericField.clamp lo hi v) expected ""

      testTheory "value in range is preserved" [
        (0.0, 10.0, 5.5, 5.5)
        (0.0, 10.0, 0.0, 0.0)
        (0.0, 10.0, 10.0, 10.0)
        (-1.0, 1.0, 0.5, 0.5)
      ]
      <| fun (lo, hi, v, expected) -> Expect.equal (NumericField.clamp lo hi v) expected ""

      testCase "infinity boundaries preserve in-range values"
      <| fun () ->
        Expect.equal (NumericField.clamp -infinity infinity 42.0) 42.0 ""
        Expect.equal (NumericField.clamp -infinity infinity 0.0) 0.0 ""

      testCase "infinity as max does not cap upward"
      <| fun () -> Expect.equal (NumericField.clamp 0.0 infinity 1e18) 1e18 ""

      testCase "negative infinity as min does not cap downward"
      <| fun () -> Expect.equal (NumericField.clamp -infinity 0.0 -1e18) -1e18 ""
    ]
  ]
