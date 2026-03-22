module Weave.Tests.Unit.GridTests

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
