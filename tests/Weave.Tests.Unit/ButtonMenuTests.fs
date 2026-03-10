module Weave.Tests.Unit.ButtonMenuTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let buttonMenuTests =
  testList "ButtonMenu" [

    testList "Direction.toClass" [
      testTheory "each direction maps to the correct class" [
        ButtonMenu.Direction.Top, "weave-button-menu--top"
        ButtonMenu.Direction.Bottom, "weave-button-menu--bottom"
        ButtonMenu.Direction.Left, "weave-button-menu--left"
        ButtonMenu.Direction.Right, "weave-button-menu--right"
      ]
      <| fun (direction, expected) -> Expect.equal (ButtonMenu.Direction.toClass direction) expected ""

      testCase "all directions produce distinct classes"
      <| fun () ->
        let classes =
          [
            ButtonMenu.Direction.Top
            ButtonMenu.Direction.Bottom
            ButtonMenu.Direction.Left
            ButtonMenu.Direction.Right
          ]
          |> List.map ButtonMenu.Direction.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each direction maps to a unique class"
    ]
  ]
