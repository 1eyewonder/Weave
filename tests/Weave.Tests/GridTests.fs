module Weave.Tests.GridTests

open Expecto
open FsCheck
open Weave

[<Tests>]
let gridTests =
  testList "Grid" [

    testList "Breakpoint.toString" [
      testTheory "each breakpoint maps to the correct string" [
        Grid.Breakpoint.ExtraSmall, "xs"
        Grid.Breakpoint.Small, "sm"
        Grid.Breakpoint.Medium, "md"
        Grid.Breakpoint.Large, "lg"
        Grid.Breakpoint.ExtraLarge, "xl"
      ]
      <| fun (bp, expected) -> Expect.equal (Grid.Breakpoint.toString bp) expected ""

      testCase "all breakpoints produce distinct strings"
      <| fun () ->
        let strs =
          [
            Grid.Breakpoint.ExtraSmall
            Grid.Breakpoint.Small
            Grid.Breakpoint.Medium
            Grid.Breakpoint.Large
            Grid.Breakpoint.ExtraLarge
          ]
          |> List.map Grid.Breakpoint.toString

        Expect.equal (List.distinct strs).Length strs.Length "each breakpoint maps to a unique string"
    ]

    testList "GutterSpacing.create clamping" [
      testTheory "values in range [0, 20] are preserved" [ 0..20 ]
      <| fun n -> Expect.equal (Grid.GutterSpacing.create n |> Grid.GutterSpacing.value) n ""

      testTheory "values below minimum are clamped to 0" [ -1; -100; -999 ]
      <| fun n -> Expect.equal (Grid.GutterSpacing.create n |> Grid.GutterSpacing.value) 0 ""

      testTheory "values above maximum are clamped to 20" [ 21; 100; 999 ]
      <| fun n -> Expect.equal (Grid.GutterSpacing.create n |> Grid.GutterSpacing.value) 20 ""

      testProperty "arbitrary int clamps to [0, 20]"
      <| fun (n: int) ->
        let v = Grid.GutterSpacing.create n |> Grid.GutterSpacing.value
        v >= 0 && v <= 20
    ]

    testList "GutterSpacing boundary values" [
      yield!
        testParam (Grid.GutterSpacing.create 0) [
          "spacing 0: value is preserved", fun s () -> Expect.equal (Grid.GutterSpacing.value s) 0 ""
          "spacing 0: class is weave-grid--spacing-0",
          fun s () -> Expect.equal (Grid.GutterSpacing.toClass s) "weave-grid--spacing-0" ""
        ]
      yield!
        testParam (Grid.GutterSpacing.create 5) [
          "spacing 5: value is preserved", fun s () -> Expect.equal (Grid.GutterSpacing.value s) 5 ""
          "spacing 5: class is weave-grid--spacing-5",
          fun s () -> Expect.equal (Grid.GutterSpacing.toClass s) "weave-grid--spacing-5" ""
        ]
      yield!
        testParam (Grid.GutterSpacing.create 20) [
          "spacing 20: value is preserved", fun s () -> Expect.equal (Grid.GutterSpacing.value s) 20 ""
          "spacing 20: class is weave-grid--spacing-20",
          fun s () -> Expect.equal (Grid.GutterSpacing.toClass s) "weave-grid--spacing-20" ""
        ]
    ]

    testList "Width.create clamping" [
      testTheory "values in range [1, 12] are preserved" [ 1..12 ]
      <| fun n -> Expect.equal (Grid.Width.create n |> Grid.Width.value) n ""

      testTheory "values below minimum are clamped to 1" [ 0; -5; -100 ]
      <| fun n -> Expect.equal (Grid.Width.create n |> Grid.Width.value) 1 ""

      testTheory "values above maximum are clamped to 12" [ 13; 100; 999 ]
      <| fun n -> Expect.equal (Grid.Width.create n |> Grid.Width.value) 12 ""

      testProperty "arbitrary int clamps to [1, 12]"
      <| fun (n: int) ->
        let v = Grid.Width.create n |> Grid.Width.value
        v >= 1 && v <= 12
    ]

    testList "Width.toClass" [
      testTheory "each breakpoint and width produce the correct class" [
        Grid.Breakpoint.ExtraSmall, 6, "weave-grid__item--xs-6"
        Grid.Breakpoint.Medium, 12, "weave-grid__item--md-12"
        Grid.Breakpoint.Large, 4, "weave-grid__item--lg-4"
      ]
      <| fun (bp, w, expected) -> Expect.equal (Grid.Width.toClass bp (Grid.Width.create w)) expected ""
    ]
  ]
