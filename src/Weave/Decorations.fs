namespace Weave.CssHelpers

open WebSharper
open Weave.CssHelpers.Core

[<AutoOpen; JavaScript>]
module Decorations =

  [<RequireQualifiedAccess; Struct>]
  type Outline =
    | Solid
    | Dashed
    | Dotted
    | Double
    | Hidden

  module Outline =

    let toClass outline =
      match outline with
      | Outline.Solid -> Css.``outline-solid``
      | Outline.Dashed -> Css.``outline-dashed``
      | Outline.Dotted -> Css.``outline-dotted``
      | Outline.Double -> Css.``outline-double``
      | Outline.Hidden -> Css.``outline-hidden``

  [<RequireQualifiedAccess; Struct>]
  type BorderRadiusStrength =
    | None
    | Small
    | Medium
    | Large

  [<RequireQualifiedAccess; Struct>]
  type BorderRadius =
    | All of BorderRadiusStrength
    | Top of BorderRadiusStrength
    | Bottom of BorderRadiusStrength
    | Left of BorderRadiusStrength
    | Right of BorderRadiusStrength
    | TopLeft of BorderRadiusStrength
    | TopRight of BorderRadiusStrength
    | BottomLeft of BorderRadiusStrength
    | BottomRight of BorderRadiusStrength
    | Pill
    | Circle

  module BorderRadius =

    module All =

      let none = BorderRadius.All BorderRadiusStrength.None
      let small = BorderRadius.All BorderRadiusStrength.Small
      let medium = BorderRadius.All BorderRadiusStrength.Medium
      let large = BorderRadius.All BorderRadiusStrength.Large

    module Top =

      let none = BorderRadius.Top BorderRadiusStrength.None
      let small = BorderRadius.Top BorderRadiusStrength.Small
      let medium = BorderRadius.Top BorderRadiusStrength.Medium
      let large = BorderRadius.Top BorderRadiusStrength.Large

    module Bottom =

      let none = BorderRadius.Bottom BorderRadiusStrength.None
      let small = BorderRadius.Bottom BorderRadiusStrength.Small
      let medium = BorderRadius.Bottom BorderRadiusStrength.Medium
      let large = BorderRadius.Bottom BorderRadiusStrength.Large

    module Left =

      let none = BorderRadius.Left BorderRadiusStrength.None
      let small = BorderRadius.Left BorderRadiusStrength.Small
      let medium = BorderRadius.Left BorderRadiusStrength.Medium
      let large = BorderRadius.Left BorderRadiusStrength.Large

    module Right =

      let none = BorderRadius.Right BorderRadiusStrength.None
      let small = BorderRadius.Right BorderRadiusStrength.Small
      let medium = BorderRadius.Right BorderRadiusStrength.Medium
      let large = BorderRadius.Right BorderRadiusStrength.Large

    module TopLeft =

      let none = BorderRadius.TopLeft BorderRadiusStrength.None
      let small = BorderRadius.TopLeft BorderRadiusStrength.Small
      let medium = BorderRadius.TopLeft BorderRadiusStrength.Medium
      let large = BorderRadius.TopLeft BorderRadiusStrength.Large

    module TopRight =

      let none = BorderRadius.TopRight BorderRadiusStrength.None
      let small = BorderRadius.TopRight BorderRadiusStrength.Small
      let medium = BorderRadius.TopRight BorderRadiusStrength.Medium
      let large = BorderRadius.TopRight BorderRadiusStrength.Large

    module BottomLeft =

      let none = BorderRadius.BottomLeft BorderRadiusStrength.None
      let small = BorderRadius.BottomLeft BorderRadiusStrength.Small
      let medium = BorderRadius.BottomLeft BorderRadiusStrength.Medium
      let large = BorderRadius.BottomLeft BorderRadiusStrength.Large

    module BottomRight =

      let none = BorderRadius.BottomRight BorderRadiusStrength.None
      let small = BorderRadius.BottomRight BorderRadiusStrength.Small
      let medium = BorderRadius.BottomRight BorderRadiusStrength.Medium
      let large = BorderRadius.BottomRight BorderRadiusStrength.Large

    let toClass borderRadius =
      match borderRadius with
      | BorderRadius.All strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-none``
        | BorderRadiusStrength.Small -> Css.``rounded-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded``
        | BorderRadiusStrength.Large -> Css.``rounded-lg``
      | BorderRadius.Top strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-t-none``
        | BorderRadiusStrength.Small -> Css.``rounded-t-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded-t``
        | BorderRadiusStrength.Large -> Css.``rounded-t-lg``
      | BorderRadius.Bottom strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-b-none``
        | BorderRadiusStrength.Small -> Css.``rounded-b-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded-b``
        | BorderRadiusStrength.Large -> Css.``rounded-b-lg``
      | BorderRadius.Left strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-l-none``
        | BorderRadiusStrength.Small -> Css.``rounded-l-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded-l``
        | BorderRadiusStrength.Large -> Css.``rounded-l-lg``
      | BorderRadius.Right strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-r-none``
        | BorderRadiusStrength.Small -> Css.``rounded-r-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded-r``
        | BorderRadiusStrength.Large -> Css.``rounded-r-lg``
      | BorderRadius.TopLeft strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-tl-none``
        | BorderRadiusStrength.Small -> Css.``rounded-tl-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded-tl``
        | BorderRadiusStrength.Large -> Css.``rounded-tl-lg``
      | BorderRadius.TopRight strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-tr-none``
        | BorderRadiusStrength.Small -> Css.``rounded-tr-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded-tr``
        | BorderRadiusStrength.Large -> Css.``rounded-tr-lg``
      | BorderRadius.BottomLeft strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-bl-none``
        | BorderRadiusStrength.Small -> Css.``rounded-bl-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded-bl``
        | BorderRadiusStrength.Large -> Css.``rounded-bl-lg``
      | BorderRadius.BottomRight strength ->
        match strength with
        | BorderRadiusStrength.None -> Css.``rounded-br-none``
        | BorderRadiusStrength.Small -> Css.``rounded-br-sm``
        | BorderRadiusStrength.Medium -> Css.``rounded-br``
        | BorderRadiusStrength.Large -> Css.``rounded-br-lg``
      | BorderRadius.Pill -> Css.``rounded-pill``
      | BorderRadius.Circle -> Css.``rounded-circle``
