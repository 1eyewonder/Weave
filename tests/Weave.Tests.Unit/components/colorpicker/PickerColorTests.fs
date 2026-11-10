module Weave.Tests.Unit.PickerColorTests

open Expecto
open Weave

type private Channels = {
  Red: int
  Green: int
  Blue: int
  Alpha: int
}

let private readChannels (color: PickerColor) = {
  Red = color.Red
  Green = color.Green
  Blue = color.Blue
  Alpha = color.Alpha
}

[<Tests>]
let tests =
  testList "PickerColor" [
    testTheory "Given valid hex when parsed then byte channels match" [
      "#f09",
      {
        Red = 255
        Green = 0
        Blue = 153
        Alpha = 255
      }

      "#f098",
      {
        Red = 255
        Green = 0
        Blue = 153
        Alpha = 136
      }

      "  12aBef  ",
      {
        Red = 18
        Green = 171
        Blue = 239
        Alpha = 255
      }

      "#12ABEF00",
      {
        Red = 18
        Green = 171
        Blue = 239
        Alpha = 0
      }

      "000000",
      {
        Red = 0
        Green = 0
        Blue = 0
        Alpha = 255
      }

      "#FFFFFFFF",
      {
        Red = 255
        Green = 255
        Blue = 255
        Alpha = 255
      }
    ]
    <| fun (input, expected) ->
      let color = PickerColor.ofHex input
      Expect.equal (readChannels color) expected "Decoded channels"

    testTheory "Given malformed hex when parsed then no color is returned" [
      "null", null
      "empty", ""
      "hash only", "#"
      "two hashes", "##123"
      "two digits", "#12"
      "five digits", "#12345"
      "seven digits", "#1234567"
      "nine digits", "#123456789"
      "invalid digits", "#12GG34"
      "named color", "red"
      "embedded space", "#12 456"
    ]
    <| fun (_, input) -> Expect.isNone (PickerColor.tryParseHex input) "Malformed input"

    testCase "Given out of range channels when constructed then each is clamped"
    <| fun () ->
      let color = PickerColor.rgba -1 256 128 999

      let expected = {
        Red = 0
        Green = 255
        Blue = 128
        Alpha = 255
      }

      Expect.equal (readChannels color) expected "Clamped bytes"

    testCase "Given an RGB color when alpha changes then RGB is preserved"
    <| fun () ->
      let color = PickerColor.rgb 18 171 239 |> PickerColor.withAlpha 64
      Expect.equal (PickerColor.toHex color) "#12ABEF40" "Alpha is last"

    testCase "Given a transparent color when formatted for CSS then alpha is included"
    <| fun () ->
      let color = PickerColor.rgba 18 171 239 64
      Expect.equal (PickerColor.toCss color) "#12ABEF40" "CSS hex output"

    testTheory "Given byte channels when formatted and parsed then the color is preserved" [ 0..255 ]
    <| fun channel ->
      let color = PickerColor.rgba channel (255 - channel) 127 channel
      let parsed = color |> PickerColor.toHex |> PickerColor.tryParseHex
      Expect.equal parsed (Some color) "Round trip without channel loss"

    testCase "Given an opaque color when formatted then alpha is omitted"
    <| fun () ->
      let color = PickerColor.rgb 0 16 255
      Expect.equal (PickerColor.toHex color) "#0010FF" "Six-digit output"

    testCase "Given malformed hex when using ofHex then an argument error is raised"
    <| fun () -> Expect.throws (fun () -> PickerColor.ofHex "invalid" |> ignore) "Malformed input"
  ]
