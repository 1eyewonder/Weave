module Weave.Tests.Unit.ExpansionPanelTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let expansionPanelTests =
  testList "ExpansionPanel" [
    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-expansion-panel__header--primary"
        BrandColor.Secondary, "weave-expansion-panel__header--secondary"
        BrandColor.Tertiary, "weave-expansion-panel__header--tertiary"
        BrandColor.Error, "weave-expansion-panel__header--error"
        BrandColor.Warning, "weave-expansion-panel__header--warning"
        BrandColor.Success, "weave-expansion-panel__header--success"
        BrandColor.Info, "weave-expansion-panel__header--info"
      ]
      <| fun (color, expected) -> Expect.equal (ExpansionPanel.Color.toClass color) expected ""

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
          |> List.map ExpansionPanel.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]

    testList "FocusColor.toClass" [
      testTheory "each color maps to the correct focus class" [
        BrandColor.Primary, "weave-expansion-panel__header--focus-primary"
        BrandColor.Secondary, "weave-expansion-panel__header--focus-secondary"
        BrandColor.Tertiary, "weave-expansion-panel__header--focus-tertiary"
        BrandColor.Error, "weave-expansion-panel__header--focus-error"
        BrandColor.Warning, "weave-expansion-panel__header--focus-warning"
        BrandColor.Success, "weave-expansion-panel__header--focus-success"
        BrandColor.Info, "weave-expansion-panel__header--focus-info"
      ]
      <| fun (color, expected) -> Expect.equal (ExpansionPanel.FocusColor.toClass color) expected ""

      testCase "all focus colors produce distinct classes"
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
          |> List.map ExpansionPanel.FocusColor.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each focus color maps to a unique class"
    ]
  ]
