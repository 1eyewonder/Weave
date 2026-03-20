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

  module BorderStyle =

    let solid = cl Css.``border-solid``
    let dashed = cl Css.``border-dashed``
    let dotted = cl Css.``border-dotted``
    let double = cl Css.``border-double``
    let hidden = cl Css.``border-hidden``
    let none = cl Css.``border-none``

  module BorderWidth =

    module All =

      let zero = cl Css.``border-0``
      let one = cl Css.border
      let two = cl Css.``border-2``
      let four = cl Css.``border-4``
      let eight = cl Css.``border-8``

    module Top =

      let zero = cl Css.``border-t-0``
      let one = cl Css.``border-t``
      let two = cl Css.``border-t-2``
      let four = cl Css.``border-t-4``
      let eight = cl Css.``border-t-8``

    module Right =

      let zero = cl Css.``border-r-0``
      let one = cl Css.``border-r``
      let two = cl Css.``border-r-2``
      let four = cl Css.``border-r-4``
      let eight = cl Css.``border-r-8``

    module Bottom =

      let zero = cl Css.``border-b-0``
      let one = cl Css.``border-b``
      let two = cl Css.``border-b-2``
      let four = cl Css.``border-b-4``
      let eight = cl Css.``border-b-8``

    module Left =

      let zero = cl Css.``border-l-0``
      let one = cl Css.``border-l``
      let two = cl Css.``border-l-2``
      let four = cl Css.``border-l-4``
      let eight = cl Css.``border-l-8``

    module Horizontal =

      let zero = cl Css.``border-x-0``
      let one = cl Css.``border-x``
      let two = cl Css.``border-x-2``
      let four = cl Css.``border-x-4``
      let eight = cl Css.``border-x-8``

    module Vertical =

      let zero = cl Css.``border-y-0``
      let one = cl Css.``border-y``
      let two = cl Css.``border-y-2``
      let four = cl Css.``border-y-4``
      let eight = cl Css.``border-y-8``

  module BorderColor =

    let primary = cl Css.``border-primary``
    let secondary = cl Css.``border-secondary``
    let tertiary = cl Css.``border-tertiary``
    let info = cl Css.``border-info``
    let success = cl Css.``border-success``
    let warning = cl Css.``border-warning``
    let error = cl Css.``border-error``
    let linesDefault = cl Css.``border-lines-default``
