module Weave.Tests.Unit.AlertTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let alertTests =
  testList "Alert" [

    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        Alert.Variant.Standard, "weave-alert--standard"
        Alert.Variant.Outlined, "weave-alert--outlined"
        Alert.Variant.Filled, "weave-alert--filled"
      ]
      <| fun (variant, expected) -> Expect.equal (Alert.Variant.toClass variant) expected ""

      testCase "all variants produce distinct classes"
      <| fun () ->
        let classes =
          [ Alert.Variant.Standard; Alert.Variant.Outlined; Alert.Variant.Filled ]
          |> List.map Alert.Variant.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each variant maps to a unique class"
    ]

    testList "AlertColor.toClass" [
      testCase "Default returns None"
      <| fun () -> Expect.isNone (Alert.AlertColor.toClass Alert.AlertColor.Default) ""

      testTheory "each BrandColor maps to the correct class" [
        Alert.AlertColor.BrandColor BrandColor.Primary, Some "weave-alert--primary"
        Alert.AlertColor.BrandColor BrandColor.Secondary, Some "weave-alert--secondary"
        Alert.AlertColor.BrandColor BrandColor.Tertiary, Some "weave-alert--tertiary"
        Alert.AlertColor.BrandColor BrandColor.Error, Some "weave-alert--error"
        Alert.AlertColor.BrandColor BrandColor.Warning, Some "weave-alert--warning"
        Alert.AlertColor.BrandColor BrandColor.Success, Some "weave-alert--success"
        Alert.AlertColor.BrandColor BrandColor.Info, Some "weave-alert--info"
      ]
      <| fun (color, expected) -> Expect.equal (Alert.AlertColor.toClass color) expected ""

    ]
  ]
