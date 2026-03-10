module Weave.Tests.Unit.FieldTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let fieldTests =
  testList "Field" [

    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        Field.Variant.Standard, "weave-field--standard"
        Field.Variant.Filled, "weave-field--filled"
        Field.Variant.Outlined, "weave-field--outlined"
      ]
      <| fun (variant, expected) -> Expect.equal (Field.Variant.toClass variant) expected ""

      testCase "all variants produce distinct classes"
      <| fun () ->
        let classes =
          [ Field.Variant.Standard; Field.Variant.Filled; Field.Variant.Outlined ]
          |> List.map Field.Variant.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each variant maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-field--primary"
        BrandColor.Secondary, "weave-field--secondary"
        BrandColor.Tertiary, "weave-field--tertiary"
        BrandColor.Error, "weave-field--error"
        BrandColor.Warning, "weave-field--warning"
        BrandColor.Success, "weave-field--success"
        BrandColor.Info, "weave-field--info"
      ]
      <| fun (color, expected) -> Expect.equal (Field.Color.toClass color) expected ""

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
          |> List.map Field.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]

    testList "Width.toClass" [
      testCase "Full returns Some weave-field--full-width"
      <| fun () -> Expect.equal (Field.Width.toClass Field.Width.Full) (Some "weave-field--full-width") ""

      testCase "Auto returns None"
      <| fun () -> Expect.isNone (Field.Width.toClass Field.Width.Auto) "Auto should return None"
    ]

    testList "HelpTextColor.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-field__help-text--primary"
        BrandColor.Secondary, "weave-field__help-text--secondary"
        BrandColor.Tertiary, "weave-field__help-text--tertiary"
        BrandColor.Error, "weave-field__help-text--error"
        BrandColor.Warning, "weave-field__help-text--warning"
        BrandColor.Success, "weave-field__help-text--success"
        BrandColor.Info, "weave-field__help-text--info"
      ]
      <| fun (color, expected) -> Expect.equal (Field.HelpTextColor.toClass color) expected ""
    ]
  ]
