module Weave.Tests.Unit.ChipTests

open Expecto
open Weave

[<Tests>]
let chipTests =
  testList "Chip" [

    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        Chip.Variant.Filled, "weave-chip--filled"
        Chip.Variant.Text, "weave-chip--text"
        Chip.Variant.Outlined, "weave-chip--outlined"
      ]
      <| fun (variant, expected) -> Expect.equal (Chip.Variant.toClass variant) expected ""

      testCase "all variants produce distinct classes"
      <| fun () ->
        let classes =
          [ Chip.Variant.Filled; Chip.Variant.Text; Chip.Variant.Outlined ]
          |> List.map Chip.Variant.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each variant maps to a unique class"
    ]

    testList "Density.toClass" [
      testTheory "each density maps to the correct class" [
        Density.Compact, "weave-chip--compact"
        Density.Standard, "weave-chip--standard"
        Density.Spacious, "weave-chip--spacious"
      ]
      <| fun (density, expected) -> Expect.equal (Chip.Density.toClass density) expected ""

      testCase "all densities produce distinct classes"
      <| fun () ->
        let classes =
          [ Density.Compact; Density.Standard; Density.Spacious ]
          |> List.map Chip.Density.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each density maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-chip--primary"
        BrandColor.Secondary, "weave-chip--secondary"
        BrandColor.Tertiary, "weave-chip--tertiary"
        BrandColor.Error, "weave-chip--error"
        BrandColor.Warning, "weave-chip--warning"
        BrandColor.Success, "weave-chip--success"
        BrandColor.Info, "weave-chip--info"
      ]
      <| fun (color, expected) -> Expect.equal (Chip.Color.toClass color) expected ""

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
          |> List.map Chip.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
