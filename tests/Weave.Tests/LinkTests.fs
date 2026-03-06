module Weave.Tests.LinkTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let linkTests =
  testList "Link" [

    testList "Underline.toClass" [
      testTheory "each underline variant maps to the correct class" [
        Link.Underline.OnHover, "weave-link--underline-hover"
        Link.Underline.Always, "weave-link--underline-always"
        Link.Underline.None, "weave-link--underline-none"
      ]
      <| fun (underline, expected) -> Expect.equal (Link.Underline.toClass underline) expected ""

      testCase "all underline variants produce distinct classes"
      <| fun () ->
        let classes =
          [ Link.Underline.OnHover; Link.Underline.Always; Link.Underline.None ]
          |> List.map Link.Underline.toClass

        Expect.equal
          (List.distinct classes).Length
          classes.Length
          "each underline variant maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-link--primary"
        BrandColor.Secondary, "weave-link--secondary"
        BrandColor.Tertiary, "weave-link--tertiary"
        BrandColor.Error, "weave-link--error"
        BrandColor.Warning, "weave-link--warning"
        BrandColor.Success, "weave-link--success"
        BrandColor.Info, "weave-link--info"
      ]
      <| fun (color, expected) -> Expect.equal (Link.Color.toClass color) expected ""

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
          |> List.map Link.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
