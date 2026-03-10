module Weave.Tests.Unit.ListTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let listTests =
  testList "WeaveList" [
    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-list-item--primary"
        BrandColor.Secondary, "weave-list-item--secondary"
        BrandColor.Tertiary, "weave-list-item--tertiary"
        BrandColor.Error, "weave-list-item--error"
        BrandColor.Warning, "weave-list-item--warning"
        BrandColor.Success, "weave-list-item--success"
        BrandColor.Info, "weave-list-item--info"
      ]
      <| fun (color, expected) -> Expect.equal (WeaveList.Color.toClass color) expected ""

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
          |> List.map WeaveList.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
