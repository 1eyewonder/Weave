module Weave.Tests.Unit.ChipSetTests

open Expecto
open Weave
open WebSharper.UI

[<Tests>]
let chipSetTests =
  testList "ChipSet" [

    testList "ChipItem.create" [
      testCase "creates a chip def with the given value"
      <| fun () ->
        let def = ChipItem.create (Doc.Empty, "test-value")
        Expect.equal def.Value "test-value" "Value should match the provided string"

      testCase "defaults Content to None"
      <| fun () ->
        let def = ChipItem.create (Doc.Empty, "v")
        Expect.isNone def.Content "Content should default to None"

      testCase "defaults Closable to false"
      <| fun () ->
        let def = ChipItem.create (Doc.Empty, "v")
        Expect.isFalse def.Closable "Closable should default to false"

      testCase "defaults Attrs to empty list"
      <| fun () ->
        let def = ChipItem.create (Doc.Empty, "v")
        Expect.isEmpty def.Attrs "Attrs should default to empty list"

      testCase "closable sets Closable to true"
      <| fun () ->
        let def = ChipItem.create (Doc.Empty, "v", closable = true)

        Expect.isTrue def.Closable "Closable should be true after closable = true"

      testCase "content sets Content to Some"
      <| fun () ->
        let def = ChipItem.create (Doc.Empty, "v", content = Doc.Empty)

        Expect.isSome def.Content "Content should be Some after content = Doc.Empty"

      testCase "disabled sets Disabled to the provided view"
      <| fun () ->
        let disabledView = View.Const true

        let def = ChipItem.create (Doc.Empty, "v", disabled = disabledView)

        Expect.isTrue
          (obj.ReferenceEquals(def.Disabled, disabledView))
          "Disabled should be set to the provided view"

      testCase "attrs sets Attrs to the provided list"
      <| fun () ->
        let customAttrs = [ Attr.Create "data-test" "chip" ]

        let def = ChipItem.create (Doc.Empty, "v", attrs = customAttrs)

        Expect.equal def.Attrs.Length 1 "Attrs should contain the provided attributes"
    ]
  ]
