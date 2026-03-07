module Weave.Tests.Unit.RadioButtonTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let radioButtonTests =
  testList "Radio" [

    testList "Size.toClass" [
      testTheory "each size maps to the correct class" [
        Radio.Size.Small, "weave-radio--small"
        Radio.Size.Medium, "weave-radio--medium"
        Radio.Size.Large, "weave-radio--large"
      ]
      <| fun (size, expected) -> Expect.equal (Radio.Size.toClass size) expected ""

      testCase "all sizes produce distinct classes"
      <| fun () ->
        let classes =
          [ Radio.Size.Small; Radio.Size.Medium; Radio.Size.Large ]
          |> List.map Radio.Size.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each size maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-radio--primary"
        BrandColor.Secondary, "weave-radio--secondary"
        BrandColor.Tertiary, "weave-radio--tertiary"
        BrandColor.Error, "weave-radio--error"
        BrandColor.Warning, "weave-radio--warning"
        BrandColor.Success, "weave-radio--success"
        BrandColor.Info, "weave-radio--info"
      ]
      <| fun (color, expected) -> Expect.equal (Radio.Color.toClass color) expected ""

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
          |> List.map Radio.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
