module Weave.Tests.Unit.AppBarTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let appBarTests =
  testList "AppBar" [

    testList "Position.toClass" [
      testTheory "each position maps to the correct class" [
        AppBar.Position.Fixed, Some "weave-appbar--fixed-top"
        AppBar.Position.Bottom, Some "weave-appbar--fixed-bottom"
        AppBar.Position.Sticky, Some "weave-appbar--sticky"
        AppBar.Position.Static, None
      ]
      <| fun (position, expected) -> Expect.equal (AppBar.Position.toClass position) expected ""

      testCase "Static returns None"
      <| fun () -> Expect.isNone (AppBar.Position.toClass AppBar.Position.Static) "Static should return None"
    ]
  ]
