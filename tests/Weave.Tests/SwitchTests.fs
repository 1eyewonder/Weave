module Weave.Tests.SwitchTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let switchTests =
  testList "Switch" [

    testList "Size.toClass" [
      testTheory "each size maps to the correct class" [
        Switch.Size.Small, "weave-switch__base--small"
        Switch.Size.Medium, "weave-switch__base--medium"
        Switch.Size.Large, "weave-switch__base--large"
      ]
      <| fun (size, expected) -> Expect.equal (Switch.Size.toClass size) expected ""

      testCase "all sizes produce distinct classes"
      <| fun () ->
        let classes =
          [ Switch.Size.Small; Switch.Size.Medium; Switch.Size.Large ]
          |> List.map Switch.Size.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each size maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-switch--primary"
        BrandColor.Secondary, "weave-switch--secondary"
        BrandColor.Tertiary, "weave-switch--tertiary"
        BrandColor.Error, "weave-switch--error"
        BrandColor.Warning, "weave-switch--warning"
        BrandColor.Success, "weave-switch--success"
        BrandColor.Info, "weave-switch--info"
      ]
      <| fun (color, expected) -> Expect.equal (Switch.Color.toClass color) expected ""

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
          |> List.map Switch.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
