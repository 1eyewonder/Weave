module Weave.Tests.Unit.SpacingTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let spacingTests =
  testList "Spacing" [

    testList "Margin.toClasses" [

      testList "basic directions (no breakpoint)" [
        testTheory "each direction produces the correct class(es)" [
          Margin.Top(Some Size.Medium, None), [ "mt-12" ]
          Margin.Bottom(Some Size.Medium, None), [ "mb-12" ]
          Margin.Left(Some Size.Medium, None), [ "ml-12" ]
          Margin.Right(Some Size.Medium, None), [ "mr-12" ]
          Margin.Vertical(Some Size.Medium, None), [ "mt-12"; "mb-12" ]
          Margin.Horizontal(Some Size.Medium, None), [ "ml-12"; "mr-12" ]
          Margin.All(Some Size.Medium, None), [ "ma-12" ]
        ]
        <| fun (margin, expected) -> Expect.equal (Margin.toClasses margin) expected ""
      ]

      testList "all sizes for Top" [
        testTheory "each size maps to the correct class" [
          Margin.Top(None, None), [ "mt-0" ]
          Margin.Top(Some Size.ExtraSmall, None), [ "mt-4" ]
          Margin.Top(Some Size.Small, None), [ "mt-8" ]
          Margin.Top(Some Size.Medium, None), [ "mt-12" ]
          Margin.Top(Some Size.Large, None), [ "mt-16" ]
          Margin.Top(Some Size.ExtraLarge, None), [ "mt-20" ]
        ]
        <| fun (margin, expected) -> Expect.equal (Margin.toClasses margin) expected ""
      ]

      testList "responsive breakpoints for Top" [
        testTheory "each breakpoint inserts the correct prefix" [
          Margin.Top(Some Size.Small, None), [ "mt-8" ]
          Margin.Top(Some Size.Small, Some Breakpoint.ExtraSmall), [ "mt-8" ]
          Margin.Top(Some Size.Small, Some Breakpoint.Small), [ "mt-sm-8" ]
          Margin.Top(Some Size.Small, Some Breakpoint.Medium), [ "mt-md-8" ]
          Margin.Top(Some Size.Small, Some Breakpoint.Large), [ "mt-lg-8" ]
          Margin.Top(Some Size.Small, Some Breakpoint.ExtraLarge), [ "mt-xl-8" ]
          Margin.Top(Some Size.Small, Some Breakpoint.ExtraExtraLarge), [ "mt-xxl-8" ]
        ]
        <| fun (margin, expected) -> Expect.equal (Margin.toClasses margin) expected ""
      ]

      testCase "responsive Vertical expands to two classes with breakpoint prefix"
      <| fun () ->
        let result =
          Margin.toClasses (Margin.Vertical(Some Size.Large, Some Breakpoint.Medium))

        Expect.equal result [ "mt-md-16"; "mb-md-16" ] ""

      testCase "responsive Horizontal expands to two classes with breakpoint prefix"
      <| fun () ->
        let result =
          Margin.toClasses (Margin.Horizontal(Some Size.ExtraSmall, Some Breakpoint.Large))

        Expect.equal result [ "ml-lg-4"; "mr-lg-4" ] ""

      testCase "None size with breakpoint produces responsive zero-spacing class"
      <| fun () ->
        let result = Margin.toClasses (Margin.Top(None, Some Breakpoint.Large))
        Expect.equal result [ "mt-lg-0" ] ""

      testCase "Vertical returns exactly two classes in top-then-bottom order"
      <| fun () ->
        let result = Margin.toClasses (Margin.Vertical(Some Size.Small, None))
        Expect.equal result.Length 2 "should return exactly 2 classes"
        Expect.equal result.[0] "mt-8" "first class should be top"
        Expect.equal result.[1] "mb-8" "second class should be bottom"

      testCase "Horizontal returns exactly two classes in left-then-right order"
      <| fun () ->
        let result = Margin.toClasses (Margin.Horizontal(Some Size.Small, None))
        Expect.equal result.Length 2 "should return exactly 2 classes"
        Expect.equal result.[0] "ml-8" "first class should be left"
        Expect.equal result.[1] "mr-8" "second class should be right"

      testCase "All returns exactly one class"
      <| fun () ->
        let result = Margin.toClasses (Margin.All(Some Size.Medium, None))
        Expect.equal result.Length 1 "should return exactly 1 class"
    ]

    testList "Padding.toClasses" [

      testList "basic directions (no breakpoint)" [
        testTheory "each direction produces the correct class(es)" [
          Padding.Top(Some Size.Medium, None), [ "pt-12" ]
          Padding.Bottom(Some Size.Medium, None), [ "pb-12" ]
          Padding.Left(Some Size.Medium, None), [ "pl-12" ]
          Padding.Right(Some Size.Medium, None), [ "pr-12" ]
          Padding.Vertical(Some Size.Medium, None), [ "pt-12"; "pb-12" ]
          Padding.Horizontal(Some Size.Medium, None), [ "pl-12"; "pr-12" ]
          Padding.All(Some Size.Medium, None), [ "pa-12" ]
        ]
        <| fun (padding, expected) -> Expect.equal (Padding.toClasses padding) expected ""
      ]

      testList "responsive breakpoints" [
        testTheory "breakpoint prefix works for Padding" [
          Padding.Bottom(Some Size.Large, Some Breakpoint.Small), [ "pb-sm-16" ]
          Padding.Bottom(Some Size.Large, Some Breakpoint.ExtraLarge), [ "pb-xl-16" ]
          Padding.Top(Some Size.Small, Some Breakpoint.Medium), [ "pt-md-8" ]
          Padding.All(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge), [ "pa-xxl-4" ]
        ]
        <| fun (padding, expected) -> Expect.equal (Padding.toClasses padding) expected ""
      ]

      testCase "responsive Vertical expands with breakpoint"
      <| fun () ->
        let result =
          Padding.toClasses (Padding.Vertical(Some Size.Small, Some Breakpoint.Medium))

        Expect.equal result [ "pt-md-8"; "pb-md-8" ] ""

      testCase "responsive Horizontal expands with breakpoint"
      <| fun () ->
        let result =
          Padding.toClasses (Padding.Horizontal(Some Size.Large, Some Breakpoint.Small))

        Expect.equal result [ "pl-sm-16"; "pr-sm-16" ] ""
    ]

    testList "distinctness" [

      testCase "all single-direction Margin classes are distinct for a given size"
      <| fun () ->
        let classes =
          [
            Margin.Top(Some Size.Medium, None)
            Margin.Bottom(Some Size.Medium, None)
            Margin.Left(Some Size.Medium, None)
            Margin.Right(Some Size.Medium, None)
            Margin.All(Some Size.Medium, None)
          ]
          |> List.collect Margin.toClasses

        Expect.equal
          (List.distinct classes).Length
          classes.Length
          "each direction should produce a unique class"

      testCase "all single-direction Padding classes are distinct for a given size"
      <| fun () ->
        let classes =
          [
            Padding.Top(Some Size.Medium, None)
            Padding.Bottom(Some Size.Medium, None)
            Padding.Left(Some Size.Medium, None)
            Padding.Right(Some Size.Medium, None)
            Padding.All(Some Size.Medium, None)
          ]
          |> List.collect Padding.toClasses

        Expect.equal
          (List.distinct classes).Length
          classes.Length
          "each direction should produce a unique class"

      testCase "Margin and Padding classes are disjoint for the same direction and size"
      <| fun () ->
        let marginClasses =
          [
            Margin.Top(Some Size.Medium, None)
            Margin.Bottom(Some Size.Medium, None)
            Margin.Left(Some Size.Medium, None)
            Margin.Right(Some Size.Medium, None)
            Margin.All(Some Size.Medium, None)
          ]
          |> List.collect Margin.toClasses
          |> Set.ofList

        let paddingClasses =
          [
            Padding.Top(Some Size.Medium, None)
            Padding.Bottom(Some Size.Medium, None)
            Padding.Left(Some Size.Medium, None)
            Padding.Right(Some Size.Medium, None)
            Padding.All(Some Size.Medium, None)
          ]
          |> List.collect Padding.toClasses
          |> Set.ofList

        Expect.equal
          (Set.intersect marginClasses paddingClasses)
          Set.empty
          "margin and padding classes should not overlap"
    ]
  ]
