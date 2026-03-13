module Weave.Tests.Unit.SelectTests

open Expecto
open Weave

[<Tests>]
let selectTests =
  testList "Select" [

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-select--primary"
        BrandColor.Secondary, "weave-select--secondary"
        BrandColor.Tertiary, "weave-select--tertiary"
        BrandColor.Error, "weave-select--error"
        BrandColor.Warning, "weave-select--warning"
        BrandColor.Success, "weave-select--success"
        BrandColor.Info, "weave-select--info"
      ]
      <| fun (color, expected) -> Expect.equal (Select.Color.toClass color) expected ""

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
          |> List.map Select.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]

    testList "Width.toClass" [
      testCase "Full returns Some weave-select--full-width"
      <| fun () -> Expect.equal (Select.Width.toClass Select.Width.Full) (Some "weave-select--full-width") ""

      testCase "FitContent returns Some weave-select--fit-content"
      <| fun () ->
        Expect.equal (Select.Width.toClass Select.Width.FitContent) (Some "weave-select--fit-content") ""

      testCase "Auto returns None"
      <| fun () -> Expect.isNone (Select.Width.toClass Select.Width.Auto) "Auto should return None"

      testCase "all Some widths produce distinct classes"
      <| fun () ->
        let classes =
          [ Select.Width.Full; Select.Width.FitContent; Select.Width.Auto ]
          |> List.map Select.Width.toClass
          |> List.choose id

        Expect.equal (List.distinct classes).Length classes.Length "each Some width maps to a unique class"
    ]
  ]
