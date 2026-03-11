module Weave.Tests.Unit.TabsTests

open Expecto
open Weave

[<Tests>]
let tabsTests =
  testList "Tabs" [

    testList "Position.toClass" [
      testTheory "each position maps to the correct class" [
        Tabs.Position.Top, "weave-tabs--top"
        Tabs.Position.Bottom, "weave-tabs--bottom"
        Tabs.Position.Left, "weave-tabs--left"
        Tabs.Position.Right, "weave-tabs--right"
        Tabs.Position.Start, "weave-tabs--start"
        Tabs.Position.End, "weave-tabs--end"
      ]
      <| fun (position, expected) -> Expect.equal (Tabs.Position.toClass position) expected ""

      testCase "all positions produce distinct classes"
      <| fun () ->
        let classes =
          [
            Tabs.Position.Top
            Tabs.Position.Bottom
            Tabs.Position.Left
            Tabs.Position.Right
            Tabs.Position.Start
            Tabs.Position.End
          ]
          |> List.map Tabs.Position.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each position maps to a unique class"
    ]

    testList "Position.isHorizontal" [
      testTheory "Top and Bottom are horizontal" [ Tabs.Position.Top; Tabs.Position.Bottom ]
      <| fun position -> Expect.isTrue (Tabs.Position.isHorizontal position) ""

      testTheory "Left, Right, Start, and End are not horizontal" [
        Tabs.Position.Left
        Tabs.Position.Right
        Tabs.Position.Start
        Tabs.Position.End
      ]
      <| fun position -> Expect.isFalse (Tabs.Position.isHorizontal position) ""
    ]

    testList "Alignment.toClass" [
      testCase "Start returns None (default, no modifier class)"
      <| fun () -> Expect.isNone (Tabs.Alignment.toClass Tabs.Alignment.Start) ""
      testCase "Center returns Some weave-tabs--centered"
      <| fun () ->
        Expect.equal (Tabs.Alignment.toClass Tabs.Alignment.Center) (Some "weave-tabs--centered") ""
    ]

    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        Tabs.Variant.Text, "weave-tabs--text"
        Tabs.Variant.Outlined, "weave-tabs--outlined"
        Tabs.Variant.Filled, "weave-tabs--filled"
      ]
      <| fun (variant, expected) -> Expect.equal (Tabs.Variant.toClass variant) expected ""

      testCase "all variants produce distinct classes"
      <| fun () ->
        let classes =
          [ Tabs.Variant.Text; Tabs.Variant.Outlined; Tabs.Variant.Filled ]
          |> List.map Tabs.Variant.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each variant maps to a unique class"
    ]

    testList "Color.toClass" [
      testTheory "each color maps to the correct class" [
        BrandColor.Primary, "weave-tabs--primary"
        BrandColor.Secondary, "weave-tabs--secondary"
        BrandColor.Tertiary, "weave-tabs--tertiary"
        BrandColor.Error, "weave-tabs--error"
        BrandColor.Warning, "weave-tabs--warning"
        BrandColor.Success, "weave-tabs--success"
        BrandColor.Info, "weave-tabs--info"
      ]
      <| fun (color, expected) -> Expect.equal (Tabs.Color.toClass color) expected ""

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
          |> List.map Tabs.Color.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each color maps to a unique class"
    ]
  ]
