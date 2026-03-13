module Weave.Tests.Unit.Generators

open FsCheck
open Weave
open Weave.CssHelpers
open Weave.CssHelpers.Core

let allSizes = [ Size.ExtraSmall; Size.Small; Size.Medium; Size.Large; Size.ExtraLarge ]

let allBreakpoints = [
  Breakpoint.ExtraSmall
  Breakpoint.Small
  Breakpoint.Medium
  Breakpoint.Large
  Breakpoint.ExtraLarge
  Breakpoint.ExtraExtraLarge
]

let allBrandColors = [
  BrandColor.Primary
  BrandColor.Secondary
  BrandColor.Tertiary
  BrandColor.Error
  BrandColor.Warning
  BrandColor.Success
  BrandColor.Info
]

let sizeOptionGen = Gen.elements (None :: (allSizes |> List.map Some))
let breakpointOptionGen = Gen.elements (None :: (allBreakpoints |> List.map Some))

let marginBuilders: (Size option * Breakpoint option -> Margin) list = [
  Margin.Top
  Margin.Bottom
  Margin.Left
  Margin.Right
  Margin.Vertical
  Margin.Horizontal
  Margin.All
]

let singleMarginBuilders: (Size option * Breakpoint option -> Margin) list = [
  Margin.Top
  Margin.Bottom
  Margin.Left
  Margin.Right
  Margin.All
]

let paddingBuilders: (Size option * Breakpoint option -> Padding) list = [
  Padding.Top
  Padding.Bottom
  Padding.Left
  Padding.Right
  Padding.Vertical
  Padding.Horizontal
  Padding.All
]

let marginGen = gen {
  let! build = Gen.elements marginBuilders
  let! size = sizeOptionGen
  let! bp = breakpointOptionGen
  return build (size, bp)
}

let paddingGen = gen {
  let! build = Gen.elements paddingBuilders
  let! size = sizeOptionGen
  let! bp = breakpointOptionGen
  return build (size, bp)
}
