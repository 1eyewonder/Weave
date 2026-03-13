module Weave.Tests.Unit.ChipSetTests

open Expecto
open Weave
open WebSharper.UI

[<Tests>]
let chipSetTests =
  testList "ChipSet" [

    testList "ChipDef.create" [
      testCase "creates a chip def with the given value"
      <| fun () ->
        let def = ChipSet.ChipDef.create Doc.Empty "test-value"
        Expect.equal def.Value "test-value" "Value should match the provided string"

      testCase "defaults Content to None"
      <| fun () ->
        let def = ChipSet.ChipDef.create Doc.Empty "v"
        Expect.isNone def.Content "Content should default to None"

      testCase "defaults Closable to false"
      <| fun () ->
        let def = ChipSet.ChipDef.create Doc.Empty "v"
        Expect.isFalse def.Closable "Closable should default to false"

      testCase "defaults Attrs to empty list"
      <| fun () ->
        let def = ChipSet.ChipDef.create Doc.Empty "v"
        Expect.isEmpty def.Attrs "Attrs should default to empty list"
    ]

    testList "ChipDef builder functions" [
      testCase "withClosable sets Closable to true"
      <| fun () ->
        let def = ChipSet.ChipDef.create Doc.Empty "v" |> ChipSet.ChipDef.withClosable

        Expect.isTrue def.Closable "Closable should be true after withClosable"

      testCase "withContent sets Content to Some"
      <| fun () ->
        let def =
          ChipSet.ChipDef.create Doc.Empty "v" |> ChipSet.ChipDef.withContent Doc.Empty

        Expect.isSome def.Content "Content should be Some after withContent"

      testCase "withDisabled sets Disabled to the provided view"
      <| fun () ->
        let disabledView = View.Const true

        let def =
          ChipSet.ChipDef.create Doc.Empty "v"
          |> ChipSet.ChipDef.withDisabled disabledView

        Expect.isTrue
          (obj.ReferenceEquals(def.Disabled, disabledView))
          "Disabled should be set to the provided view"

      testCase "withAttrs sets Attrs to the provided list"
      <| fun () ->
        let customAttrs = [ Attr.Create "data-test" "chip" ]

        let def =
          ChipSet.ChipDef.create Doc.Empty "v" |> ChipSet.ChipDef.withAttrs customAttrs

        Expect.equal def.Attrs.Length 1 "Attrs should contain the provided attributes"
    ]
  ]
