module Weave.Tests.Unit.DividerTests

open Expecto
open Weave

[<Tests>]
let dividerTests =
  testList "Divider" [

    testList "Orientation.toClasses" [
      testTheory "each orientation maps to the correct class list" [
        Divider.Orientation.Horizontal, ([]: string list)
        Divider.Orientation.Vertical, [ "weave-divider--vertical" ]
      ]
      <| fun (orientation, expected) -> Expect.equal (Divider.Orientation.toClasses orientation) expected ""
    ]

    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        Divider.Variant.FullWidth, "weave-divider--fullwidth"
        Divider.Variant.Inset, "weave-divider--inset"
        Divider.Variant.Middle, "weave-divider--middle"
      ]
      <| fun (variant, expected) -> Expect.equal (Divider.Variant.toClass variant) expected ""

      testCase "all variants produce distinct classes"
      <| fun () ->
        let classes =
          [ Divider.Variant.FullWidth; Divider.Variant.Inset; Divider.Variant.Middle ]
          |> List.map Divider.Variant.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each variant maps to a unique class"
    ]
  ]
