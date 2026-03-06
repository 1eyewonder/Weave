module Weave.Tests.ButtonGroupTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let buttonGroupTests =
  testList "ButtonGroup" [

    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        ButtonGroup.Variant.Filled, "weave-button-group--filled"
        ButtonGroup.Variant.Outlined, "weave-button-group--outlined"
        ButtonGroup.Variant.Text, "weave-button-group--text"
      ]
      <| fun (variant, expected) -> Expect.equal (ButtonGroup.Variant.toClass variant) expected ""

      testCase "all variants produce distinct classes"
      <| fun () ->
        let classes =
          [
            ButtonGroup.Variant.Filled
            ButtonGroup.Variant.Outlined
            ButtonGroup.Variant.Text
          ]
          |> List.map ButtonGroup.Variant.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each variant maps to a unique class"
    ]

    testList "Orientation.toClass" [
      testCase "Horizontal returns None"
      <| fun () ->
        Expect.isNone
          (ButtonGroup.Orientation.toClass ButtonGroup.Orientation.Horizontal)
          "Horizontal is the default, no class needed"
      testCase "Vertical returns Some weave-button-group--vertical"
      <| fun () ->
        Expect.equal
          (ButtonGroup.Orientation.toClass ButtonGroup.Orientation.Vertical)
          (Some "weave-button-group--vertical")
          ""
    ]

    testList "Size.toClass" [
      testTheory "each size maps to the correct class" [
        ButtonGroup.Size.Small, "weave-button-group--small"
        ButtonGroup.Size.Medium, "weave-button-group--medium"
        ButtonGroup.Size.Large, "weave-button-group--large"
      ]
      <| fun (size, expected) -> Expect.equal (ButtonGroup.Size.toClass size) expected ""

      testCase "all sizes produce distinct classes"
      <| fun () ->
        let classes =
          [ ButtonGroup.Size.Small; ButtonGroup.Size.Medium; ButtonGroup.Size.Large ]
          |> List.map ButtonGroup.Size.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each size maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-button-group--primary"
        BrandColor.Secondary, "weave-button-group--secondary"
        BrandColor.Tertiary, "weave-button-group--tertiary"
        BrandColor.Error, "weave-button-group--error"
        BrandColor.Warning, "weave-button-group--warning"
        BrandColor.Success, "weave-button-group--success"
        BrandColor.Info, "weave-button-group--info"
      ]
      <| fun (color, expected) -> Expect.equal (ButtonGroup.Color.toClass color) expected ""

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
          |> List.map ButtonGroup.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
