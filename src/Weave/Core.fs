namespace Weave.CssHelpers

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.Sitelets
open Zanaptak.TypedCssClasses
open Weave.Theming

[<JavaScript>]
module Core =

  type internal Css = CssClasses<"styles.css">

  type WebSharperElement = Attr list -> Doc list -> Doc

  let div = Html.div

  let cl name =
    try
      Attr.Class name
    with _ ->
      Unchecked.defaultof<Attr>

  let cls names =
    try
      List.map Attr.Class names |> Attr.Concat
    with _ ->
      Unchecked.defaultof<Attr>

  let text = Html.text
  let textView = Html.textView

  module Theme =

    let current =
      let mode =
        try
          getMode ()
        with _ ->
          ThemeMode.Light

      Var.Create mode

  module Style =

    [<Literal>]
    let backgroundColor = "background-color"

    [<Literal>]
    let color = "color"

  module Attr =

    let toggleStyleOrDefault style value defaultValue enabled =
      enabled
      |> View.MapCached(fun e -> if e then value else defaultValue)
      |> Attr.DynamicStyle style

    let toggleBackgroundColorOrDefault value defaultValue enabled =
      toggleStyleOrDefault Style.backgroundColor value defaultValue enabled

    let toggleBackgroundColor value enabled =
      toggleStyleOrDefault Style.backgroundColor value "" enabled

    let toggleColorOrDefault value defaultValue enabled =
      toggleStyleOrDefault Style.color value defaultValue enabled

    let toggleColor value enabled =
      toggleStyleOrDefault Style.color value "" enabled

  /// <summary>
  /// CSS variables for Weave's theme palette
  /// </summary>
  module Palette =

    [<Literal>]
    let primary = "var(--palette-primary)"

    [<Literal>]
    let secondary = "var(--palette-secondary)"

    [<Literal>]
    let tertiary = "var(--palette-tertiary)"

    [<Literal>]
    let error = "var(--palette-error)"

    [<Literal>]
    let warning = "var(--palette-warning)"

    [<Literal>]
    let success = "var(--palette-success)"

    [<Literal>]
    let info = "var(--palette-info)"

    [<Literal>]
    let background = "var(--palette-background)"

    [<Literal>]
    let backgroundDarken = "var(--palette-background-darken)"

    [<Literal>]
    let backgroundPaper = "var(--palette-background-paper)"

    [<Literal>]
    let surface = "var(--palette-surface)"

    [<Literal>]
    let textDisabled = "var(--palette-text-disabled)"

    [<Literal>]
    let actionDisabled = "var(--palette-action-disabled)"

    [<Literal>]
    let backgroundDisabled = "var(--palette-action-disabled-background)"

  [<RequireQualifiedAccess; Struct>]
  type BrandColor =
    | Primary
    | Secondary
    | Tertiary
    | Error
    | Warning
    | Success
    | Info

  module BrandColor =

    module BackgroundColor =

      let primary = Attr.Style Style.backgroundColor Palette.primary
      let secondary = Attr.Style Style.backgroundColor Palette.secondary
      let tertiary = Attr.Style Style.backgroundColor Palette.tertiary
      let error = Attr.Style Style.backgroundColor Palette.error
      let warning = Attr.Style Style.backgroundColor Palette.warning
      let success = Attr.Style Style.backgroundColor Palette.success
      let info = Attr.Style Style.backgroundColor Palette.info

    module TextColor =

      let primary = Attr.Style Style.color Palette.primary
      let secondary = Attr.Style Style.color Palette.secondary
      let tertiary = Attr.Style Style.color Palette.tertiary
      let error = Attr.Style Style.color Palette.error
      let warning = Attr.Style Style.color Palette.warning
      let success = Attr.Style Style.color Palette.success
      let info = Attr.Style Style.color Palette.info

  module TransitionSpeed =

    let none = cl Css.``weave-transition--none``
    let fast = cl Css.``weave-transition--fast``
    let standard = cl Css.``weave-transition--standard``
    let slow = cl Css.``weave-transition--slow``

  module Opacity =

    let zero = cl Css.``weave-opacity-0``
    let ten = cl Css.``weave-opacity-10``
    let twenty = cl Css.``weave-opacity-20``
    let thirty = cl Css.``weave-opacity-30``
    let forty = cl Css.``weave-opacity-40``
    let fifty = cl Css.``weave-opacity-50``
    let sixty = cl Css.``weave-opacity-60``
    let seventy = cl Css.``weave-opacity-70``
    let eighty = cl Css.``weave-opacity-80``
    let ninety = cl Css.``weave-opacity-90``
    let hundred = cl Css.``weave-opacity-100``

  module SurfaceColor =

    module BackgroundColor =

      let background = Attr.Style Style.backgroundColor Palette.background
      let backgroundDarker = Attr.Style Style.backgroundColor Palette.backgroundDarken
      let paper = Attr.Style Style.backgroundColor Palette.backgroundPaper
      let surface = Attr.Style Style.backgroundColor Palette.surface

    module TextColor =

      let background = Attr.Style Style.color Palette.background
      let backgroundDarker = Attr.Style Style.color Palette.backgroundDarken
      let paper = Attr.Style Style.color Palette.backgroundPaper
      let surface = Attr.Style Style.color Palette.surface

  module DisabledColor =

    module BackgroundColor =

      let background = Attr.Style Style.backgroundColor Palette.backgroundDisabled
      let text = Attr.Style Style.backgroundColor Palette.textDisabled
      let action = Attr.Style Style.backgroundColor Palette.actionDisabled

    module TextColor =

      let background = Attr.Style Style.color Palette.backgroundDisabled
      let text = Attr.Style Style.color Palette.textDisabled
      let action = Attr.Style Style.color Palette.actionDisabled

  [<RequireQualifiedAccess; Struct>]
  type Breakpoint =
    | ExtraSmall
    | Small
    | Medium
    | Large
    | ExtraLarge
    | ExtraExtraLarge
