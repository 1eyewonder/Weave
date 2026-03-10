module Weave.Tests.Unit.TooltipTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let tooltipTests =
  testList "Tooltip" [
    testList "Direction.toClass" [
      testTheory "each direction maps to the correct class" [
        Tooltip.Direction.Top, "weave-tooltip--top-center"
        Tooltip.Direction.Bottom, "weave-tooltip--bottom-center"
        Tooltip.Direction.Left, "weave-tooltip--center-left"
        Tooltip.Direction.Right, "weave-tooltip--center-right"
      ]
      <| fun (direction, expected) -> Expect.equal (Tooltip.Direction.toClass direction) expected ""

      testCase "all directions produce distinct classes"
      <| fun () ->
        let classes =
          [
            Tooltip.Direction.Top
            Tooltip.Direction.Bottom
            Tooltip.Direction.Left
            Tooltip.Direction.Right
          ]
          |> List.map Tooltip.Direction.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each direction maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-tooltip--primary"
        BrandColor.Secondary, "weave-tooltip--secondary"
        BrandColor.Tertiary, "weave-tooltip--tertiary"
        BrandColor.Error, "weave-tooltip--error"
        BrandColor.Warning, "weave-tooltip--warning"
        BrandColor.Success, "weave-tooltip--success"
        BrandColor.Info, "weave-tooltip--info"
      ]
      <| fun (color, expected) -> Expect.equal (Tooltip.Color.toClass color) expected ""

      testCase "all colors produce distinct classes"
      <| fun () ->
        let classes =
          [
            BrandColor.Primary
            BrandColor.Secondary
            BrandColor.Tertiary
            BrandColor.Error
            BrandColor.Warning
            BrandColor.Success
            BrandColor.Info
          ]
          |> List.map Tooltip.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
