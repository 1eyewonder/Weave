namespace Weave

open WebSharper

[<JavaScript; Struct>]
type internal HsvColor = {
  Hue: float
  Saturation: float
  Brightness: float
}

/// An sRGB color with byte channels, including alpha (0 = transparent, 255 = opaque).
[<JavaScript; Struct>]
type PickerColor = private {
  R: int
  G: int
  B: int
  A: int
} with

  member this.Red = this.R
  member this.Green = this.G
  member this.Blue = this.B
  member this.Alpha = this.A

[<JavaScript; RequireQualifiedAccess>]
module PickerColor =

  [<Struct>]
  type private RgbChannels = {
    Red: float
    Green: float
    Blue: float
  }

  /// Creates a color, clamping each channel to 0–255.
  let rgba red green blue alpha = {
    R = Bounded.clamp 0 255 red
    G = Bounded.clamp 0 255 green
    B = Bounded.clamp 0 255 blue
    A = Bounded.clamp 0 255 alpha
  }

  let rgb red green blue = rgba red green blue 255

  let withAlpha alpha color = rgba color.R color.G color.B alpha

  let internal withRed red color = rgba red color.G color.B color.A

  let internal withGreen green color = rgba color.R green color.B color.A

  let internal withBlue blue color = rgba color.R color.G blue color.A

  let private normalizeHex (value: string) =
    let trimmed = value.Trim().ToUpper()

    if trimmed.Length > 0 && trimmed.[0] = '#' then
      trimmed.Substring(1)
    else
      trimmed

  let private hexDigit (digit: char) = "0123456789ABCDEF".IndexOf digit

  let private isValidHex (hex: string) =
    let validLength = [ 3; 4; 6; 8 ] |> List.contains hex.Length
    validLength && (hex |> Seq.forall (fun digit -> hexDigit digit >= 0))

  let private decodeHex (hex: string) =
    let channel index =
      if hex.Length <= 4 then
        hexDigit hex.[index] * 17
      else
        hexDigit hex.[index * 2] * 16 + hexDigit hex.[index * 2 + 1]

    let alpha = if hex.Length = 4 || hex.Length = 8 then channel 3 else 255
    rgba (channel 0) (channel 1) (channel 2) alpha

  let private tryDecodeHex hex =
    if isValidHex hex then Some(decodeHex hex) else None

  /// Accepts #RGB, #RGBA, #RRGGBB and #RRGGBBAA (the # is optional).
  let tryParseHex value =
    value |> Option.ofObj |> Option.map normalizeHex |> Option.bind tryDecodeHex

  /// Parses a hexadecimal color; raises an argument error for malformed input.
  let ofHex value =
    match tryParseHex value with
    | Some color -> color
    | None -> invalidArg "value" "Expected #RGB, #RGBA, #RRGGBB or #RRGGBBAA."

  let private hexByte n =
    let digits = "0123456789ABCDEF"
    string<char> digits.[n / 16] + string<char> digits.[n % 16]

  /// Formats #RRGGBB for opaque colors and #RRGGBBAA otherwise.
  let toHex color =
    "#"
    + hexByte color.R
    + hexByte color.G
    + hexByte color.B
    + (if color.A = 255 then "" else hexByte color.A)

  let toCss color = toHex color

  let internal toHsv color =
    let channels = {
      Red = float color.R / 255.
      Green = float color.G / 255.
      Blue = float color.B / 255.
    }

    let maximum = max channels.Green channels.Blue |> max channels.Red
    let minimum = min channels.Green channels.Blue |> min channels.Red
    let delta = maximum - minimum

    let hue =
      if delta = 0. then
        0.
      elif maximum = channels.Red then
        60. * (((channels.Green - channels.Blue) / delta + 6.) % 6.)
      elif maximum = channels.Green then
        60. * ((channels.Blue - channels.Red) / delta + 2.)
      else
        60. * ((channels.Red - channels.Green) / delta + 4.)

    {
      Hue = hue
      Saturation = if maximum = 0. then 0. else delta / maximum
      Brightness = maximum
    }

  let internal ofHsv alpha (color: HsvColor) =
    let hueSector = ((color.Hue % 360.) + 360.) % 360. / 60.
    let saturation = color.Saturation |> Bounded.clamp 0. 1.
    let brightness = color.Brightness |> Bounded.clamp 0. 1.
    let chroma = brightness * saturation
    let secondary = chroma * (1. - abs (hueSector % 2. - 1.))

    let channels =
      if hueSector < 1. then
        {
          Red = chroma
          Green = secondary
          Blue = 0.
        }
      elif hueSector < 2. then
        {
          Red = secondary
          Green = chroma
          Blue = 0.
        }
      elif hueSector < 3. then
        {
          Red = 0.
          Green = chroma
          Blue = secondary
        }
      elif hueSector < 4. then
        {
          Red = 0.
          Green = secondary
          Blue = chroma
        }
      elif hueSector < 5. then
        {
          Red = secondary
          Green = 0.
          Blue = chroma
        }
      else
        {
          Red = chroma
          Green = 0.
          Blue = secondary
        }

    // A tiny tolerance keeps half-byte rounding stable after floating-point subtraction.
    let channel value =
      int (floor ((value + brightness - chroma) * 255. + 0.5 + 1e-9))

    rgba (channel channels.Red) (channel channels.Green) (channel channels.Blue) alpha
