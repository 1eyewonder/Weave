module Weave.Tests.TypographyTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let typographyTests =
  testList "Typography" [

    testList "Typo.toClass" [
      testTheory "each typo variant maps to the correct class" [
        Typography.Typo.H1, "weave-typography--h1"
        Typography.Typo.H2, "weave-typography--h2"
        Typography.Typo.H3, "weave-typography--h3"
        Typography.Typo.H4, "weave-typography--h4"
        Typography.Typo.H5, "weave-typography--h5"
        Typography.Typo.H6, "weave-typography--h6"
        Typography.Typo.Subtitle1, "weave-typography--subtitle1"
        Typography.Typo.Subtitle2, "weave-typography--subtitle2"
        Typography.Typo.Body1, "weave-typography--body1"
        Typography.Typo.Body2, "weave-typography--body2"
        Typography.Typo.Button, "weave-typography--button"
        Typography.Typo.Caption, "weave-typography--caption"
        Typography.Typo.Overline, "weave-typography--overline"
      ]
      <| fun (typo, expected) -> Expect.equal (Typography.Typo.toClass typo) expected ""

      testCase "all typo variants produce distinct classes"
      <| fun () ->
        let allTypos = [
          Typography.Typo.H1
          Typography.Typo.H2
          Typography.Typo.H3
          Typography.Typo.H4
          Typography.Typo.H5
          Typography.Typo.H6
          Typography.Typo.Subtitle1
          Typography.Typo.Subtitle2
          Typography.Typo.Body1
          Typography.Typo.Body2
          Typography.Typo.Button
          Typography.Typo.Caption
          Typography.Typo.Overline
        ]

        let classes = allTypos |> List.map Typography.Typo.toClass
        Expect.equal (List.distinct classes).Length classes.Length "each typo variant maps to a unique class"
    ]

    testList "Align.toClass" [
      testTheory "each alignment maps to the correct class" [
        Typography.Align.Inherit, "weave-typography--align-inherit"
        Typography.Align.Justify, "weave-typography--align-justify"
        Typography.Align.Center, "weave-typography--align-center"
        Typography.Align.Left, "weave-typography--align-left"
        Typography.Align.Right, "weave-typography--align-right"
        Typography.Align.Start, "weave-typography--align-start"
        Typography.Align.End, "weave-typography--align-end"
      ]
      <| fun (align, expected) -> Expect.equal (Typography.Align.toClass align) expected ""

      testCase "all alignments produce distinct classes"
      <| fun () ->
        let allAligns = [
          Typography.Align.Inherit
          Typography.Align.Justify
          Typography.Align.Center
          Typography.Align.Left
          Typography.Align.Right
          Typography.Align.Start
          Typography.Align.End
        ]

        let classes = allAligns |> List.map Typography.Align.toClass
        Expect.equal (List.distinct classes).Length classes.Length "each alignment maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-typography--primary"
        BrandColor.Secondary, "weave-typography--secondary"
        BrandColor.Tertiary, "weave-typography--tertiary"
        BrandColor.Error, "weave-typography--error"
        BrandColor.Warning, "weave-typography--warning"
        BrandColor.Success, "weave-typography--success"
        BrandColor.Info, "weave-typography--info"
      ]
      <| fun (color, expected) -> Expect.equal (Typography.Color.toClass color) expected ""

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
          |> List.map Typography.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
