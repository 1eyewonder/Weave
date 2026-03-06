module Weave.Tests.DropdownTests

open Expecto
open Weave

let private allAnchorOrigins = [
  Dropdown.AnchorOrigin.TopLeft
  Dropdown.AnchorOrigin.TopCenter
  Dropdown.AnchorOrigin.TopRight
  Dropdown.AnchorOrigin.CenterLeft
  Dropdown.AnchorOrigin.CenterCenter
  Dropdown.AnchorOrigin.CenterRight
  Dropdown.AnchorOrigin.BottomLeft
  Dropdown.AnchorOrigin.BottomCenter
  Dropdown.AnchorOrigin.BottomRight
]

let private allTransformOrigins = [
  Dropdown.TransformOrigin.TopLeft
  Dropdown.TransformOrigin.TopCenter
  Dropdown.TransformOrigin.TopRight
  Dropdown.TransformOrigin.CenterLeft
  Dropdown.TransformOrigin.CenterCenter
  Dropdown.TransformOrigin.CenterRight
  Dropdown.TransformOrigin.BottomLeft
  Dropdown.TransformOrigin.BottomCenter
  Dropdown.TransformOrigin.BottomRight
]

[<Tests>]
let dropdownTests =
  testList "Dropdown" [

    testList "AnchorOrigin.toString" [
      testTheory "each anchor origin maps to the correct string" [
        Dropdown.AnchorOrigin.TopLeft, "Top Left"
        Dropdown.AnchorOrigin.TopCenter, "Top Center"
        Dropdown.AnchorOrigin.TopRight, "Top Right"
        Dropdown.AnchorOrigin.CenterLeft, "Center Left"
        Dropdown.AnchorOrigin.CenterCenter, "Center Center"
        Dropdown.AnchorOrigin.CenterRight, "Center Right"
        Dropdown.AnchorOrigin.BottomLeft, "Bottom Left"
        Dropdown.AnchorOrigin.BottomCenter, "Bottom Center"
        Dropdown.AnchorOrigin.BottomRight, "Bottom Right"
      ]
      <| fun (origin, expected) -> Expect.equal (Dropdown.AnchorOrigin.toString origin) expected ""

      testCase "all anchor origins produce distinct strings"
      <| fun () ->
        let strs = allAnchorOrigins |> List.map Dropdown.AnchorOrigin.toString
        Expect.equal (List.distinct strs).Length strs.Length "each anchor origin maps to a unique string"
    ]

    testList "TransformOrigin.toString" [
      testTheory "each transform origin maps to the correct string" [
        Dropdown.TransformOrigin.TopLeft, "Top Left"
        Dropdown.TransformOrigin.TopCenter, "Top Center"
        Dropdown.TransformOrigin.TopRight, "Top Right"
        Dropdown.TransformOrigin.CenterLeft, "Center Left"
        Dropdown.TransformOrigin.CenterCenter, "Center Center"
        Dropdown.TransformOrigin.CenterRight, "Center Right"
        Dropdown.TransformOrigin.BottomLeft, "Bottom Left"
        Dropdown.TransformOrigin.BottomCenter, "Bottom Center"
        Dropdown.TransformOrigin.BottomRight, "Bottom Right"
      ]
      <| fun (origin, expected) -> Expect.equal (Dropdown.TransformOrigin.toString origin) expected ""

      testCase "all transform origins produce distinct strings"
      <| fun () ->
        let strs = allTransformOrigins |> List.map Dropdown.TransformOrigin.toString
        Expect.equal (List.distinct strs).Length strs.Length "each transform origin maps to a unique string"
    ]

    testList "AnchorOrigin and TransformOrigin string consistency" [
      testCase "matching cases produce the same string across both types"
      <| fun () ->
        let anchorStrs = allAnchorOrigins |> List.map Dropdown.AnchorOrigin.toString

        let transformStrs =
          allTransformOrigins |> List.map Dropdown.TransformOrigin.toString

        Expect.equal
          anchorStrs
          transformStrs
          "AnchorOrigin and TransformOrigin should use identical string representations"
    ]
  ]
