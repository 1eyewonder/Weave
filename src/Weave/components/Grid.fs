namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Grid =

  module Spacing =

    let none = cl Css.``weave-grid--spacing-0``
    let extraSmall = cl Css.``weave-grid--spacing-2``
    let small = cl Css.``weave-grid--spacing-4``
    let medium = cl Css.``weave-grid--spacing-8``
    let large = cl Css.``weave-grid--spacing-12``
    let extraLarge = cl Css.``weave-grid--spacing-20``

[<JavaScript>]
module GridItem =

  module Span =

    let one = cl Css.``weave-grid__item--xs-1``
    let two = cl Css.``weave-grid__item--xs-2``
    let three = cl Css.``weave-grid__item--xs-3``
    let four = cl Css.``weave-grid__item--xs-4``
    let five = cl Css.``weave-grid__item--xs-5``
    let six = cl Css.``weave-grid__item--xs-6``
    let seven = cl Css.``weave-grid__item--xs-7``
    let eight = cl Css.``weave-grid__item--xs-8``
    let nine = cl Css.``weave-grid__item--xs-9``
    let ten = cl Css.``weave-grid__item--xs-10``
    let eleven = cl Css.``weave-grid__item--xs-11``
    let twelve = cl Css.``weave-grid__item--xs-12``

    module Small =
      let one = cl Css.``weave-grid__item--sm-1``
      let two = cl Css.``weave-grid__item--sm-2``
      let three = cl Css.``weave-grid__item--sm-3``
      let four = cl Css.``weave-grid__item--sm-4``
      let five = cl Css.``weave-grid__item--sm-5``
      let six = cl Css.``weave-grid__item--sm-6``
      let seven = cl Css.``weave-grid__item--sm-7``
      let eight = cl Css.``weave-grid__item--sm-8``
      let nine = cl Css.``weave-grid__item--sm-9``
      let ten = cl Css.``weave-grid__item--sm-10``
      let eleven = cl Css.``weave-grid__item--sm-11``
      let twelve = cl Css.``weave-grid__item--sm-12``

    module Medium =
      let one = cl Css.``weave-grid__item--md-1``
      let two = cl Css.``weave-grid__item--md-2``
      let three = cl Css.``weave-grid__item--md-3``
      let four = cl Css.``weave-grid__item--md-4``
      let five = cl Css.``weave-grid__item--md-5``
      let six = cl Css.``weave-grid__item--md-6``
      let seven = cl Css.``weave-grid__item--md-7``
      let eight = cl Css.``weave-grid__item--md-8``
      let nine = cl Css.``weave-grid__item--md-9``
      let ten = cl Css.``weave-grid__item--md-10``
      let eleven = cl Css.``weave-grid__item--md-11``
      let twelve = cl Css.``weave-grid__item--md-12``

    module Large =
      let one = cl Css.``weave-grid__item--lg-1``
      let two = cl Css.``weave-grid__item--lg-2``
      let three = cl Css.``weave-grid__item--lg-3``
      let four = cl Css.``weave-grid__item--lg-4``
      let five = cl Css.``weave-grid__item--lg-5``
      let six = cl Css.``weave-grid__item--lg-6``
      let seven = cl Css.``weave-grid__item--lg-7``
      let eight = cl Css.``weave-grid__item--lg-8``
      let nine = cl Css.``weave-grid__item--lg-9``
      let ten = cl Css.``weave-grid__item--lg-10``
      let eleven = cl Css.``weave-grid__item--lg-11``
      let twelve = cl Css.``weave-grid__item--lg-12``

    module ExtraLarge =
      let one = cl Css.``weave-grid__item--xl-1``
      let two = cl Css.``weave-grid__item--xl-2``
      let three = cl Css.``weave-grid__item--xl-3``
      let four = cl Css.``weave-grid__item--xl-4``
      let five = cl Css.``weave-grid__item--xl-5``
      let six = cl Css.``weave-grid__item--xl-6``
      let seven = cl Css.``weave-grid__item--xl-7``
      let eight = cl Css.``weave-grid__item--xl-8``
      let nine = cl Css.``weave-grid__item--xl-9``
      let ten = cl Css.``weave-grid__item--xl-10``
      let eleven = cl Css.``weave-grid__item--xl-11``
      let twelve = cl Css.``weave-grid__item--xl-12``

[<JavaScript>]
type Grid =

  static member create(items: Doc list, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty

    div [ cl Css.``weave-grid``; yield! attrs ] items

[<JavaScript>]
type GridItem =

  static member create(content: Doc, ?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty

    div [ cl Css.``weave-grid__item``; yield! attrs ] [ content ]

[<JavaScript>]
type FlexBreak =

  static member create() =
    div [ cl Css.``weave-flex-break``; Attr.Create "aria-hidden" "true" ] []
