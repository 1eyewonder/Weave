namespace Weave.CssHelpers

open WebSharper
open Weave.CssHelpers.Core

[<AutoOpen; JavaScript>]
module Decorations =

  module Outline =

    let solid = cl Css.``outline-solid``
    let dashed = cl Css.``outline-dashed``
    let dotted = cl Css.``outline-dotted``
    let double = cl Css.``outline-double``
    let hidden = cl Css.``outline-hidden``

  module BorderRadius =

    let pill = cl Css.``rounded-pill``
    let circle = cl Css.``rounded-circle``

    module All =

      let none = cl Css.``rounded-none``
      let small = cl Css.``rounded-sm``
      let medium = cl Css.``rounded``
      let large = cl Css.``rounded-lg``

    module Top =

      let none = cl Css.``rounded-t-none``
      let small = cl Css.``rounded-t-sm``
      let medium = cl Css.``rounded-t``
      let large = cl Css.``rounded-t-lg``

    module Bottom =

      let none = cl Css.``rounded-b-none``
      let small = cl Css.``rounded-b-sm``
      let medium = cl Css.``rounded-b``
      let large = cl Css.``rounded-b-lg``

    module Left =

      let none = cl Css.``rounded-l-none``
      let small = cl Css.``rounded-l-sm``
      let medium = cl Css.``rounded-l``
      let large = cl Css.``rounded-l-lg``

    module Right =

      let none = cl Css.``rounded-r-none``
      let small = cl Css.``rounded-r-sm``
      let medium = cl Css.``rounded-r``
      let large = cl Css.``rounded-r-lg``

    module TopLeft =

      let none = cl Css.``rounded-tl-none``
      let small = cl Css.``rounded-tl-sm``
      let medium = cl Css.``rounded-tl``
      let large = cl Css.``rounded-tl-lg``

    module TopRight =

      let none = cl Css.``rounded-tr-none``
      let small = cl Css.``rounded-tr-sm``
      let medium = cl Css.``rounded-tr``
      let large = cl Css.``rounded-tr-lg``

    module BottomLeft =

      let none = cl Css.``rounded-bl-none``
      let small = cl Css.``rounded-bl-sm``
      let medium = cl Css.``rounded-bl``
      let large = cl Css.``rounded-bl-lg``

    module BottomRight =

      let none = cl Css.``rounded-br-none``
      let small = cl Css.``rounded-br-sm``
      let medium = cl Css.``rounded-br``
      let large = cl Css.``rounded-br-lg``
