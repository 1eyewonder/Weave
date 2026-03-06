module Weave.Tests.ButtonTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let buttonTests =
  testList "Button" [

    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        Button.Variant.Filled, "weave-button--filled"
        Button.Variant.Outlined, "weave-button--outlined"
        Button.Variant.Text, "weave-button--text"
      ]
      <| fun (variant, expected) -> Expect.equal (Button.Variant.toClass variant) expected ""

      testCase "all variants produce distinct classes"
      <| fun () ->
        let classes =
          [ Button.Variant.Filled; Button.Variant.Outlined; Button.Variant.Text ]
          |> List.map Button.Variant.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each variant maps to a unique class"
    ]

    testList "Width.toClass" [
      testCase "Full returns Some weave-button--full-width"
      <| fun () -> Expect.equal (Button.Width.toClass Button.Width.Full) (Some "weave-button--full-width") ""
      testCase "Auto returns None"
      <| fun () -> Expect.isNone (Button.Width.toClass Button.Width.Auto) "Auto should return None"
    ]

    testList "Size.toClass" [
      testTheory "each size maps to the correct class" [
        Button.Size.Small, "weave-button--small"
        Button.Size.Medium, "weave-button--medium"
        Button.Size.Large, "weave-button--large"
      ]
      <| fun (size, expected) -> Expect.equal (Button.Size.toClass size) expected ""

      testCase "all sizes produce distinct classes"
      <| fun () ->
        let classes =
          [ Button.Size.Small; Button.Size.Medium; Button.Size.Large ]
          |> List.map Button.Size.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each size maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-button--primary"
        BrandColor.Secondary, "weave-button--secondary"
        BrandColor.Tertiary, "weave-button--tertiary"
        BrandColor.Error, "weave-button--error"
        BrandColor.Warning, "weave-button--warning"
        BrandColor.Success, "weave-button--success"
        BrandColor.Info, "weave-button--info"
      ]
      <| fun (color, expected) -> Expect.equal (Button.Color.toClass color) expected ""

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
          |> List.map Button.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
