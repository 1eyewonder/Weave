module Weave.Tests.Unit.CrossComponentTests

open Expecto
open Weave
open Weave.Tests.Unit.Generators
open Weave

let colorMappings: (string * (BrandColor -> string)) list = [
  "ButtonGroup", ButtonGroup.Color.toClass
  "Checkbox", Checkbox.Color.toClass
  "Chip", Chip.Color.toClass
  "ExpansionPanel", ExpansionPanel.Color.toClass
  "Field", Field.Color.toClass
  "Link", Link.Color.toClass
  "Radio", Radio.Color.toClass
  "Select", Select.Color.toClass
  "Switch", Switch.Color.toClass
  "Tabs", Tabs.Color.toClass
  "Tooltip", Tooltip.Color.toClass
  "Typography", Typography.Color.toClass
  "WeaveList", WeaveList.Color.toClass
]

[<Tests>]
let crossComponentTests =
  testList "Cross-Component" [

    testCase "color classes are unique across components"
    <| fun () ->
      let allClasses =
        colorMappings
        |> List.collect (fun (_, toClass) -> allBrandColors |> List.map toClass)

      Expect.equal
        (List.distinct allClasses).Length
        allClasses.Length
        "Color classes should be unique across all components (no accidental reuse)"
  ]
