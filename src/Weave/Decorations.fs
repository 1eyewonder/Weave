namespace Weave.CssHelpers

open WebSharper
open Weave.CssHelpers.Core

[<AutoOpen; JavaScript>]
module Decorations =

  module BorderRadius =

    let pill = cl Css.``weave-rounded-pill``
    let circle = cl Css.``weave-rounded-circle``

    module All =

      let none = cl Css.``weave-rounded-none``
      let small = cl Css.``weave-rounded-sm``
      let medium = cl Css.``weave-rounded``
      let large = cl Css.``weave-rounded-lg``

    module Top =

      let none = cl Css.``weave-rounded-t-none``
      let small = cl Css.``weave-rounded-t-sm``
      let medium = cl Css.``weave-rounded-t``
      let large = cl Css.``weave-rounded-t-lg``

    module Bottom =

      let none = cl Css.``weave-rounded-b-none``
      let small = cl Css.``weave-rounded-b-sm``
      let medium = cl Css.``weave-rounded-b``
      let large = cl Css.``weave-rounded-b-lg``

    module Left =

      let none = cl Css.``weave-rounded-l-none``
      let small = cl Css.``weave-rounded-l-sm``
      let medium = cl Css.``weave-rounded-l``
      let large = cl Css.``weave-rounded-l-lg``

    module Right =

      let none = cl Css.``weave-rounded-r-none``
      let small = cl Css.``weave-rounded-r-sm``
      let medium = cl Css.``weave-rounded-r``
      let large = cl Css.``weave-rounded-r-lg``

    module TopLeft =

      let none = cl Css.``weave-rounded-tl-none``
      let small = cl Css.``weave-rounded-tl-sm``
      let medium = cl Css.``weave-rounded-tl``
      let large = cl Css.``weave-rounded-tl-lg``

    module TopRight =

      let none = cl Css.``weave-rounded-tr-none``
      let small = cl Css.``weave-rounded-tr-sm``
      let medium = cl Css.``weave-rounded-tr``
      let large = cl Css.``weave-rounded-tr-lg``

    module BottomLeft =

      let none = cl Css.``weave-rounded-bl-none``
      let small = cl Css.``weave-rounded-bl-sm``
      let medium = cl Css.``weave-rounded-bl``
      let large = cl Css.``weave-rounded-bl-lg``

    module BottomRight =

      let none = cl Css.``weave-rounded-br-none``
      let small = cl Css.``weave-rounded-br-sm``
      let medium = cl Css.``weave-rounded-br``
      let large = cl Css.``weave-rounded-br-lg``

  module BorderWidth =

    module All =

      let zero = cl Css.``weave-border-0``
      let one = cl Css.``weave-border``
      let two = cl Css.``weave-border-2``
      let four = cl Css.``weave-border-4``
      let eight = cl Css.``weave-border-8``

    module Top =

      let zero = cl Css.``weave-border-t-0``
      let one = cl Css.``weave-border-t``
      let two = cl Css.``weave-border-t-2``
      let four = cl Css.``weave-border-t-4``
      let eight = cl Css.``weave-border-t-8``

    module Right =

      let zero = cl Css.``weave-border-r-0``
      let one = cl Css.``weave-border-r``
      let two = cl Css.``weave-border-r-2``
      let four = cl Css.``weave-border-r-4``
      let eight = cl Css.``weave-border-r-8``

    module Bottom =

      let zero = cl Css.``weave-border-b-0``
      let one = cl Css.``weave-border-b``
      let two = cl Css.``weave-border-b-2``
      let four = cl Css.``weave-border-b-4``
      let eight = cl Css.``weave-border-b-8``

    module Left =

      let zero = cl Css.``weave-border-l-0``
      let one = cl Css.``weave-border-l``
      let two = cl Css.``weave-border-l-2``
      let four = cl Css.``weave-border-l-4``
      let eight = cl Css.``weave-border-l-8``

    module Horizontal =

      let zero = cl Css.``weave-border-x-0``
      let one = cl Css.``weave-border-x``
      let two = cl Css.``weave-border-x-2``
      let four = cl Css.``weave-border-x-4``
      let eight = cl Css.``weave-border-x-8``

    module Vertical =

      let zero = cl Css.``weave-border-y-0``
      let one = cl Css.``weave-border-y``
      let two = cl Css.``weave-border-y-2``
      let four = cl Css.``weave-border-y-4``
      let eight = cl Css.``weave-border-y-8``

  module BorderColor =

    let primary = cl Css.``weave-border-primary``
    let secondary = cl Css.``weave-border-secondary``
    let tertiary = cl Css.``weave-border-tertiary``
    let info = cl Css.``weave-border-info``
    let success = cl Css.``weave-border-success``
    let warning = cl Css.``weave-border-warning``
    let error = cl Css.``weave-border-error``
    let linesDefault = cl Css.``weave-border-lines-default``

  module Cursor =

    let auto = cl Css.``weave-cursor-auto``
    let default' = cl Css.``weave-cursor-default``
    let pointer = cl Css.``weave-cursor-pointer``
    let wait = cl Css.``weave-cursor-wait``
    let notAllowed = cl Css.``weave-cursor-not-allowed``
    let grab = cl Css.``weave-cursor-grab``
    let grabbing = cl Css.``weave-cursor-grabbing``

  module Visibility =

    let visible = cl Css.``weave-visible``
    let invisible = cl Css.``weave-invisible``
    let srOnly = cl Css.``weave-sr-only``

  module ZIndex =

    let z0 = cl Css.``weave-z-0``
    let z10 = cl Css.``weave-z-10``
    let z20 = cl Css.``weave-z-20``
    let z30 = cl Css.``weave-z-30``
    let z40 = cl Css.``weave-z-40``
    let z50 = cl Css.``weave-z-50``
    let z60 = cl Css.``weave-z-60``
    let z70 = cl Css.``weave-z-70``
    let z80 = cl Css.``weave-z-80``
    let z90 = cl Css.``weave-z-90``
    let z100 = cl Css.``weave-z-100``
    let auto = cl Css.``weave-z-auto``

  module Elevation =

    let e0 = cl Css.``weave-elevation-0``
    let e1 = cl Css.``weave-elevation-1``
    let e2 = cl Css.``weave-elevation-2``
    let e3 = cl Css.``weave-elevation-3``
    let e4 = cl Css.``weave-elevation-4``
    let e5 = cl Css.``weave-elevation-5``
    let e6 = cl Css.``weave-elevation-6``
    let e7 = cl Css.``weave-elevation-7``
    let e8 = cl Css.``weave-elevation-8``
    let e9 = cl Css.``weave-elevation-9``
    let e10 = cl Css.``weave-elevation-10``
    let e11 = cl Css.``weave-elevation-11``
    let e12 = cl Css.``weave-elevation-12``
    let e13 = cl Css.``weave-elevation-13``
    let e14 = cl Css.``weave-elevation-14``
    let e15 = cl Css.``weave-elevation-15``
    let e16 = cl Css.``weave-elevation-16``
    let e17 = cl Css.``weave-elevation-17``
    let e18 = cl Css.``weave-elevation-18``
    let e19 = cl Css.``weave-elevation-19``
    let e20 = cl Css.``weave-elevation-20``
    let e21 = cl Css.``weave-elevation-21``
    let e22 = cl Css.``weave-elevation-22``
    let e23 = cl Css.``weave-elevation-23``
    let e24 = cl Css.``weave-elevation-24``
    let e25 = cl Css.``weave-elevation-25``
