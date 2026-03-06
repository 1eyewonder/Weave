module Weave.Tests.CheckboxTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let checkboxTests =
  testList "Checkbox" [

    testList "Size.toClass" [
      testTheory "each size maps to the correct class" [
        Checkbox.Size.Small, "weave-checkbox--small"
        Checkbox.Size.Medium, "weave-checkbox--medium"
        Checkbox.Size.Large, "weave-checkbox--large"
      ]
      <| fun (size, expected) -> Expect.equal (Checkbox.Size.toClass size) expected ""

      testCase "all sizes produce distinct classes"
      <| fun () ->
        let classes =
          [ Checkbox.Size.Small; Checkbox.Size.Medium; Checkbox.Size.Large ]
          |> List.map Checkbox.Size.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each size maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-checkbox--primary"
        BrandColor.Secondary, "weave-checkbox--secondary"
        BrandColor.Tertiary, "weave-checkbox--tertiary"
        BrandColor.Error, "weave-checkbox--error"
        BrandColor.Warning, "weave-checkbox--warning"
        BrandColor.Success, "weave-checkbox--success"
        BrandColor.Info, "weave-checkbox--info"
      ]
      <| fun (color, expected) -> Expect.equal (Checkbox.Color.toClass color) expected ""

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
          |> List.map Checkbox.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
