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
  let cl = Attr.Class
  let cls = List.map Attr.Class >> Attr.Concat

  let text = Html.text
  let textView = Html.textView

  module Theme =

    let current = Var.Create(getMode ())

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
  type Size =
    | ExtraSmall
    | Small
    | Medium
    | Large
    | ExtraLarge

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

    let internal toStyle style color =
      match color with
      | BrandColor.Primary -> Attr.Style style Palette.primary
      | BrandColor.Secondary -> Attr.Style style Palette.secondary
      | BrandColor.Tertiary -> Attr.Style style Palette.tertiary
      | BrandColor.Error -> Attr.Style style Palette.error
      | BrandColor.Warning -> Attr.Style style Palette.warning
      | BrandColor.Success -> Attr.Style style Palette.success
      | BrandColor.Info -> Attr.Style style Palette.info

    let palette color =
      match color with
      | BrandColor.Primary -> Palette.primary
      | BrandColor.Secondary -> Palette.secondary
      | BrandColor.Tertiary -> Palette.tertiary
      | BrandColor.Error -> Palette.error
      | BrandColor.Warning -> Palette.warning
      | BrandColor.Success -> Palette.success
      | BrandColor.Info -> Palette.info

    let toBackgroundColor color = toStyle Style.backgroundColor color
    let toColor color = toStyle Style.color color

  [<RequireQualifiedAccess; Struct>]
  type Density =
    | Compact
    | Standard
    | Spacious

  [<RequireQualifiedAccess; Struct>]
  type TransitionSpeed =
    | None
    | Fast
    | Standard
    | Slow

  module TransitionSpeed =

    let toClass speed =
      match speed with
      | TransitionSpeed.None -> Css.``weave-transition--none``
      | TransitionSpeed.Fast -> Css.``weave-transition--fast``
      | TransitionSpeed.Standard -> Css.``weave-transition--standard``
      | TransitionSpeed.Slow -> Css.``weave-transition--slow``

  [<Struct>]
  type Opacity = private Opacity of int

  module Opacity =

    let create percent =
      match percent with
      | p when p < 0 -> Opacity 0
      | p when p > 100 -> Opacity 100
      | p -> Opacity p

    let toClass (Opacity percent) = $"weave-opacity-{percent}"

  [<RequireQualifiedAccess; Struct>]
  type SurfaceColor =
    | Background
    | BackgroundDarker
    | Paper
    | Surface

  module SurfaceColor =

    let internal toStyle style color =
      match color with
      | SurfaceColor.Background -> Attr.Style style Palette.background
      | SurfaceColor.BackgroundDarker -> Attr.Style style Palette.backgroundDarken
      | SurfaceColor.Paper -> Attr.Style style Palette.backgroundPaper
      | SurfaceColor.Surface -> Attr.Style style Palette.surface

    let palette color =
      match color with
      | SurfaceColor.Background -> Palette.background
      | SurfaceColor.BackgroundDarker -> Palette.backgroundDarken
      | SurfaceColor.Paper -> Palette.backgroundPaper
      | SurfaceColor.Surface -> Palette.surface

    let toBackgroundColor color = toStyle Style.backgroundColor color
    let toColor color = toStyle Style.color color

  [<RequireQualifiedAccess; Struct>]
  type DisabledColor =
    | Background
    | Text
    | Action

  module DisabledColor =

    let internal toStyle style color =
      match color with
      | DisabledColor.Background -> Attr.Style style Palette.backgroundDisabled
      | DisabledColor.Text -> Attr.Style style Palette.textDisabled
      | DisabledColor.Action -> Attr.Style style Palette.actionDisabled

    let palette color =
      match color with
      | DisabledColor.Background -> Palette.backgroundDisabled
      | DisabledColor.Text -> Palette.textDisabled
      | DisabledColor.Action -> Palette.actionDisabled

    let toBackgroundColor color = toStyle Style.backgroundColor color
    let toColor color = toStyle Style.color color

  [<RequireQualifiedAccess; Struct>]
  type Color =
    | Brand of brand: BrandColor
    | Surface of surface: SurfaceColor
    | Disabled of disabled: DisabledColor

  module Color =

    let palette color =
      match color with
      | Color.Brand brand -> BrandColor.palette brand
      | Color.Surface surface -> SurfaceColor.palette surface
      | Color.Disabled disabled -> DisabledColor.palette disabled

    let toBackgroundColor color =
      match color with
      | Color.Brand brand -> BrandColor.toBackgroundColor brand
      | Color.Surface surface -> SurfaceColor.toBackgroundColor surface
      | Color.Disabled disabled -> DisabledColor.toBackgroundColor disabled

    let toColor color =
      match color with
      | Color.Brand brand -> BrandColor.toColor brand
      | Color.Surface surface -> SurfaceColor.toColor surface
      | Color.Disabled disabled -> DisabledColor.toColor disabled

  [<RequireQualifiedAccess; Struct>]
  type Breakpoint =
    | ExtraSmall
    | Small
    | Medium
    | Large
    | ExtraLarge
    | ExtraExtraLarge
