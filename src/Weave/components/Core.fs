namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.Sitelets
open Zanaptak.TypedCssClasses
open Weave.Theming

[<JavaScript>]
module CssHelpers =

  type internal Css = CssClasses<"styles.css">

  type WebSharperElement = Attr list -> Doc list -> Doc

  let div = Html.div
  let cl = Attr.Class
  let cls = List.map Attr.Class >> Attr.Concat

  let text = Html.text
  let textView = Html.textView

  module Theme =

    let current = Var.Create(Theming.getMode ())

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

  module Density =

    let toClass density =
      match density with
      | Density.Compact -> Css.``weave-density--compact``
      | Density.Standard -> Css.``weave-density--standard``
      | Density.Spacious -> Css.``weave-density--spacious``

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

  /// <summary>
  /// Represents a margin spacing value with an optional responsive breakpoint.
  /// </summary>
  [<RequireQualifiedAccess>]
  type Margin =
    | Top of Size option * Breakpoint option
    | Bottom of Size option * Breakpoint option
    | Left of Size option * Breakpoint option
    | Right of Size option * Breakpoint option
    | Vertical of Size option * Breakpoint option
    | Horizontal of Size option * Breakpoint option
    | All of Size option * Breakpoint option

  module private SpacingHelpers =

    let sizeToNum size =
      match size with
      | None -> "0"
      | Some Size.ExtraSmall -> "4"
      | Some Size.Small -> "8"
      | Some Size.Medium -> "12"
      | Some Size.Large -> "16"
      | Some Size.ExtraLarge -> "20"

    let breakpointPrefix bp =
      match bp with
      | None -> ""
      | Some Breakpoint.ExtraSmall -> ""
      | Some Breakpoint.Small -> "sm-"
      | Some Breakpoint.Medium -> "md-"
      | Some Breakpoint.Large -> "lg-"
      | Some Breakpoint.ExtraLarge -> "xl-"
      | Some Breakpoint.ExtraExtraLarge -> "xxl-"

  /// <summary>
  /// Provides preset margin values and a function to convert them to CSS class names.
  /// </summary>
  module Margin =

    /// <summary>
    /// Preset top margin values. Use nested breakpoint modules (e.g., <c>Top.Small</c>) for responsive variants.
    /// </summary>
    module Top =

      /// <summary>
      /// Removes top margin (0px).
      /// </summary>
      let none = Margin.Top(None, None)

      /// <summary>
      /// Applies extra-small top margin (4px).
      /// </summary>
      let extraSmall = Margin.Top(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small top margin (8px).
      /// </summary>
      let small = Margin.Top(Some Size.Small, None)

      /// <summary>
      /// Applies medium top margin (12px).
      /// </summary>
      let medium = Margin.Top(Some Size.Medium, None)

      /// <summary>
      /// Applies large top margin (16px).
      /// </summary>
      let large = Margin.Top(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large top margin (20px).
      /// </summary>
      let extraLarge = Margin.Top(Some Size.ExtraLarge, None)

      /// <summary>
      /// Top margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes top margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Margin.Top(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Top(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small top margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Margin.Top(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium top margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Margin.Top(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large top margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Margin.Top(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Top(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Top margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes top margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = Margin.Top(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small top margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Top(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small top margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = Margin.Top(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium top margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Margin.Top(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large top margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = Margin.Top(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large top margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Top(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Top margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes top margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Margin.Top(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small top margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Top(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small top margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Margin.Top(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium top margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Margin.Top(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large top margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Margin.Top(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large top margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Top(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Top margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes top margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = Margin.Top(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small top margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Top(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small top margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = Margin.Top(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium top margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Margin.Top(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large top margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = Margin.Top(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large top margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Top(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Top margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes top margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Top(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Top(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small top margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Top(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium top margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Top(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large top margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Top(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Top(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Top margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes top margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Top(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Top(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small top margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Top(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium top margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Top(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large top margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Top(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Top(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset bottom margin values. Use nested breakpoint modules (e.g., <c>Bottom.Small</c>) for responsive variants.
    /// </summary>
    module Bottom =

      /// <summary>
      /// Removes bottom margin (0px).
      /// </summary>
      let none = Margin.Bottom(None, None)

      /// <summary>
      /// Applies extra-small bottom margin (4px).
      /// </summary>
      let extraSmall = Margin.Bottom(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small bottom margin (8px).
      /// </summary>
      let small = Margin.Bottom(Some Size.Small, None)

      /// <summary>
      /// Applies medium bottom margin (12px).
      /// </summary>
      let medium = Margin.Bottom(Some Size.Medium, None)

      /// <summary>
      /// Applies large bottom margin (16px).
      /// </summary>
      let large = Margin.Bottom(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large bottom margin (20px).
      /// </summary>
      let extraLarge = Margin.Bottom(Some Size.ExtraLarge, None)

      /// <summary>
      /// Bottom margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Margin.Bottom(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Bottom(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Margin.Bottom(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Margin.Bottom(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Margin.Bottom(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Bottom(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Bottom margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes bottom margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = Margin.Bottom(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Bottom(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small bottom margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = Margin.Bottom(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium bottom margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Margin.Bottom(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large bottom margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = Margin.Bottom(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Bottom(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Bottom margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes bottom margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Margin.Bottom(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Bottom(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small bottom margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Margin.Bottom(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium bottom margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Margin.Bottom(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large bottom margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Margin.Bottom(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Bottom(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Bottom margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes bottom margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = Margin.Bottom(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Bottom(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small bottom margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = Margin.Bottom(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium bottom margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Margin.Bottom(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large bottom margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = Margin.Bottom(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Bottom(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Bottom margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Bottom(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Bottom(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Bottom(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Bottom(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Bottom(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Bottom(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Bottom margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Bottom(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall =
          Margin.Bottom(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Bottom(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Bottom(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Bottom(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge =
          Margin.Bottom(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset left margin values. Use nested breakpoint modules (e.g., <c>Left.Small</c>) for responsive variants.
    /// </summary>
    module Left =

      /// <summary>
      /// Removes left margin (0px).
      /// </summary>
      let none = Margin.Left(None, None)

      /// <summary>
      /// Applies extra-small left margin (4px).
      /// </summary>
      let extraSmall = Margin.Left(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small left margin (8px).
      /// </summary>
      let small = Margin.Left(Some Size.Small, None)

      /// <summary>
      /// Applies medium left margin (12px).
      /// </summary>
      let medium = Margin.Left(Some Size.Medium, None)

      /// <summary>
      /// Applies large left margin (16px).
      /// </summary>
      let large = Margin.Left(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large left margin (20px).
      /// </summary>
      let extraLarge = Margin.Left(Some Size.ExtraLarge, None)

      /// <summary>
      /// Left margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes left margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Margin.Left(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Left(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small left margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Margin.Left(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium left margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Margin.Left(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large left margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Margin.Left(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Left(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Left margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes left margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = Margin.Left(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small left margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Left(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small left margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = Margin.Left(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium left margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Margin.Left(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large left margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = Margin.Left(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large left margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Left(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Left margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes left margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Margin.Left(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small left margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Left(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small left margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Margin.Left(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium left margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Margin.Left(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large left margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Margin.Left(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large left margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Left(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Left margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes left margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = Margin.Left(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small left margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Left(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small left margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = Margin.Left(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium left margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Margin.Left(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large left margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = Margin.Left(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large left margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Left(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Left margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes left margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Left(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Left(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small left margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Left(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium left margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Left(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large left margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Left(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Left(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Left margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes left margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Left(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Left(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small left margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Left(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium left margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Left(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large left margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Left(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Left(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset right margin values. Use nested breakpoint modules (e.g., <c>Right.Small</c>) for responsive variants.
    /// </summary>
    module Right =

      /// <summary>
      /// Removes right margin (0px).
      /// </summary>
      let none = Margin.Right(None, None)

      /// <summary>
      /// Applies extra-small right margin (4px).
      /// </summary>
      let extraSmall = Margin.Right(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small right margin (8px).
      /// </summary>
      let small = Margin.Right(Some Size.Small, None)

      /// <summary>
      /// Applies medium right margin (12px).
      /// </summary>
      let medium = Margin.Right(Some Size.Medium, None)

      /// <summary>
      /// Applies large right margin (16px).
      /// </summary>
      let large = Margin.Right(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large right margin (20px).
      /// </summary>
      let extraLarge = Margin.Right(Some Size.ExtraLarge, None)

      /// <summary>
      /// Right margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes right margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Margin.Right(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Right(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small right margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Margin.Right(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium right margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Margin.Right(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large right margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Margin.Right(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Right(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Right margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes right margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = Margin.Right(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small right margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Right(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small right margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = Margin.Right(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium right margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Margin.Right(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large right margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = Margin.Right(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large right margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Right(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Right margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes right margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Margin.Right(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small right margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Right(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small right margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Margin.Right(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium right margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Margin.Right(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large right margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Margin.Right(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large right margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Right(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Right margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes right margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = Margin.Right(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small right margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Right(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small right margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = Margin.Right(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium right margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Margin.Right(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large right margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = Margin.Right(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large right margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Right(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Right margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes right margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Right(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Right(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small right margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Right(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium right margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Right(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large right margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Right(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Right(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Right margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes right margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Right(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Right(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small right margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Right(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium right margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Right(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large right margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Right(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Right(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset vertical margin (top and bottom) values. Use nested breakpoint modules (e.g., <c>Vertical.Small</c>) for responsive variants.
    /// </summary>
    module Vertical =

      /// <summary>
      /// Removes vertical margin (top and bottom) (0px).
      /// </summary>
      let none = Margin.Vertical(None, None)

      /// <summary>
      /// Applies extra-small vertical margin (top and bottom) (4px).
      /// </summary>
      let extraSmall = Margin.Vertical(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small vertical margin (top and bottom) (8px).
      /// </summary>
      let small = Margin.Vertical(Some Size.Small, None)

      /// <summary>
      /// Applies medium vertical margin (top and bottom) (12px).
      /// </summary>
      let medium = Margin.Vertical(Some Size.Medium, None)

      /// <summary>
      /// Applies large vertical margin (top and bottom) (16px).
      /// </summary>
      let large = Margin.Vertical(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large vertical margin (top and bottom) (20px).
      /// </summary>
      let extraLarge = Margin.Vertical(Some Size.ExtraLarge, None)

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Margin.Vertical(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Vertical(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Margin.Vertical(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Margin.Vertical(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Margin.Vertical(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Vertical(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the small breakpoint and above.
        /// </summary>
        let none = Margin.Vertical(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Vertical(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the small breakpoint and above.
        /// </summary>
        let small = Margin.Vertical(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Margin.Vertical(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the small breakpoint and above.
        /// </summary>
        let large = Margin.Vertical(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Vertical(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Margin.Vertical(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Vertical(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Margin.Vertical(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Margin.Vertical(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Margin.Vertical(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Vertical(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the large breakpoint and above.
        /// </summary>
        let none = Margin.Vertical(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Vertical(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the large breakpoint and above.
        /// </summary>
        let small = Margin.Vertical(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Margin.Vertical(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the large breakpoint and above.
        /// </summary>
        let large = Margin.Vertical(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Vertical(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Vertical(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Vertical(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Vertical(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Vertical(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Vertical(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Vertical(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Vertical(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall =
          Margin.Vertical(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Vertical(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Vertical(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Vertical(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge =
          Margin.Vertical(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset horizontal margin (left and right) values. Use nested breakpoint modules (e.g., <c>Horizontal.Small</c>) for responsive variants.
    /// </summary>
    module Horizontal =

      /// <summary>
      /// Removes horizontal margin (left and right) (0px).
      /// </summary>
      let none = Margin.Horizontal(None, None)

      /// <summary>
      /// Applies extra-small horizontal margin (left and right) (4px).
      /// </summary>
      let extraSmall = Margin.Horizontal(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small horizontal margin (left and right) (8px).
      /// </summary>
      let small = Margin.Horizontal(Some Size.Small, None)

      /// <summary>
      /// Applies medium horizontal margin (left and right) (12px).
      /// </summary>
      let medium = Margin.Horizontal(Some Size.Medium, None)

      /// <summary>
      /// Applies large horizontal margin (left and right) (16px).
      /// </summary>
      let large = Margin.Horizontal(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large horizontal margin (left and right) (20px).
      /// </summary>
      let extraLarge = Margin.Horizontal(Some Size.ExtraLarge, None)

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Margin.Horizontal(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Horizontal(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Margin.Horizontal(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Margin.Horizontal(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Margin.Horizontal(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Horizontal(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Horizontal margin (left and right) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the small breakpoint and above.
        /// </summary>
        let none = Margin.Horizontal(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Horizontal(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the small breakpoint and above.
        /// </summary>
        let small = Margin.Horizontal(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Margin.Horizontal(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the small breakpoint and above.
        /// </summary>
        let large = Margin.Horizontal(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Horizontal(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Horizontal margin (left and right) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Margin.Horizontal(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Horizontal(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Margin.Horizontal(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Margin.Horizontal(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Margin.Horizontal(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Horizontal(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Horizontal margin (left and right) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the large breakpoint and above.
        /// </summary>
        let none = Margin.Horizontal(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Horizontal(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the large breakpoint and above.
        /// </summary>
        let small = Margin.Horizontal(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Margin.Horizontal(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the large breakpoint and above.
        /// </summary>
        let large = Margin.Horizontal(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Horizontal(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Horizontal(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.Horizontal(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Horizontal(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Horizontal(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Horizontal(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.Horizontal(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Margin.Horizontal(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall =
          Margin.Horizontal(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Margin.Horizontal(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.Horizontal(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Margin.Horizontal(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge =
          Margin.Horizontal(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset margin on all sides values. Use nested breakpoint modules (e.g., <c>All.Small</c>) for responsive variants.
    /// </summary>
    module All =

      /// <summary>
      /// Removes margin on all sides (0px).
      /// </summary>
      let none = Margin.All(None, None)

      /// <summary>
      /// Applies extra-small margin on all sides (4px).
      /// </summary>
      let extraSmall = Margin.All(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small margin on all sides (8px).
      /// </summary>
      let small = Margin.All(Some Size.Small, None)

      /// <summary>
      /// Applies medium margin on all sides (12px).
      /// </summary>
      let medium = Margin.All(Some Size.Medium, None)

      /// <summary>
      /// Applies large margin on all sides (16px).
      /// </summary>
      let large = Margin.All(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large margin on all sides (20px).
      /// </summary>
      let extraLarge = Margin.All(Some Size.ExtraLarge, None)

      /// <summary>
      /// Margin on all sides values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Margin.All(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.All(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Margin.All(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Margin.All(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Margin.All(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.All(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Margin on all sides values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes margin on all sides (0px) at the small breakpoint and above.
        /// </summary>
        let none = Margin.All(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Margin.All(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small margin on all sides (8px) at the small breakpoint and above.
        /// </summary>
        let small = Margin.All(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium margin on all sides (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Margin.All(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large margin on all sides (16px) at the small breakpoint and above.
        /// </summary>
        let large = Margin.All(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Margin.All(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Margin on all sides values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes margin on all sides (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Margin.All(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Margin.All(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small margin on all sides (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Margin.All(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium margin on all sides (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Margin.All(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large margin on all sides (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Margin.All(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Margin.All(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Margin on all sides values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes margin on all sides (0px) at the large breakpoint and above.
        /// </summary>
        let none = Margin.All(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.All(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small margin on all sides (8px) at the large breakpoint and above.
        /// </summary>
        let small = Margin.All(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium margin on all sides (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Margin.All(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large margin on all sides (16px) at the large breakpoint and above.
        /// </summary>
        let large = Margin.All(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.All(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Margin on all sides values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Margin.All(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.All(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Margin.All(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.All(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Margin.All(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.All(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Margin on all sides values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Margin.All(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Margin.All(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Margin.All(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Margin.All(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Margin.All(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Margin.All(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Converts a Margin value to its corresponding CSS utility class names.
    /// </summary>
    let toClasses margin =
      let bp = SpacingHelpers.breakpointPrefix
      let sz = SpacingHelpers.sizeToNum

      match margin with
      | Margin.Top(size, b) -> [ $"mt-{bp b}{sz size}" ]
      | Margin.Bottom(size, b) -> [ $"mb-{bp b}{sz size}" ]
      | Margin.Left(size, b) -> [ $"ml-{bp b}{sz size}" ]
      | Margin.Right(size, b) -> [ $"mr-{bp b}{sz size}" ]
      | Margin.Vertical(size, b) -> [ $"mt-{bp b}{sz size}"; $"mb-{bp b}{sz size}" ]
      | Margin.Horizontal(size, b) -> [ $"ml-{bp b}{sz size}"; $"mr-{bp b}{sz size}" ]
      | Margin.All(size, b) -> [ $"ma-{bp b}{sz size}" ]

  /// <summary>
  /// Represents a padding spacing value with an optional responsive breakpoint.
  /// </summary>
  [<RequireQualifiedAccess>]
  type Padding =
    | Top of Size option * Breakpoint option
    | Bottom of Size option * Breakpoint option
    | Left of Size option * Breakpoint option
    | Right of Size option * Breakpoint option
    | Vertical of Size option * Breakpoint option
    | Horizontal of Size option * Breakpoint option
    | All of Size option * Breakpoint option

  /// <summary>
  /// Provides preset padding values and a function to convert them to CSS class names.
  /// </summary>
  module Padding =

    /// <summary>
    /// Preset top padding values. Use nested breakpoint modules (e.g., <c>Top.Small</c>) for responsive variants.
    /// </summary>
    module Top =

      /// <summary>
      /// Removes top padding (0px).
      /// </summary>
      let none = Padding.Top(None, None)

      /// <summary>
      /// Applies extra-small top padding (4px).
      /// </summary>
      let extraSmall = Padding.Top(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small top padding (8px).
      /// </summary>
      let small = Padding.Top(Some Size.Small, None)

      /// <summary>
      /// Applies medium top padding (12px).
      /// </summary>
      let medium = Padding.Top(Some Size.Medium, None)

      /// <summary>
      /// Applies large top padding (16px).
      /// </summary>
      let large = Padding.Top(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large top padding (20px).
      /// </summary>
      let extraLarge = Padding.Top(Some Size.ExtraLarge, None)

      /// <summary>
      /// Top padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes top padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Padding.Top(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Top(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small top padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Padding.Top(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium top padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Padding.Top(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large top padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Padding.Top(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Top(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Top padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes top padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = Padding.Top(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small top padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Top(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small top padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = Padding.Top(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium top padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Padding.Top(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large top padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = Padding.Top(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large top padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Top(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Top padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes top padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Padding.Top(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small top padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Top(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small top padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Padding.Top(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium top padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Padding.Top(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large top padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Padding.Top(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large top padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Top(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Top padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes top padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = Padding.Top(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small top padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Top(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small top padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = Padding.Top(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium top padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Padding.Top(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large top padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = Padding.Top(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large top padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Top(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Top padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes top padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Top(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Top(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small top padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Top(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium top padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Top(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large top padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Top(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Top(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Top padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes top padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Top(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Top(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small top padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Top(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium top padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Top(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large top padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Top(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Top(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset bottom padding values. Use nested breakpoint modules (e.g., <c>Bottom.Small</c>) for responsive variants.
    /// </summary>
    module Bottom =

      /// <summary>
      /// Removes bottom padding (0px).
      /// </summary>
      let none = Padding.Bottom(None, None)

      /// <summary>
      /// Applies extra-small bottom padding (4px).
      /// </summary>
      let extraSmall = Padding.Bottom(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small bottom padding (8px).
      /// </summary>
      let small = Padding.Bottom(Some Size.Small, None)

      /// <summary>
      /// Applies medium bottom padding (12px).
      /// </summary>
      let medium = Padding.Bottom(Some Size.Medium, None)

      /// <summary>
      /// Applies large bottom padding (16px).
      /// </summary>
      let large = Padding.Bottom(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large bottom padding (20px).
      /// </summary>
      let extraLarge = Padding.Bottom(Some Size.ExtraLarge, None)

      /// <summary>
      /// Bottom padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Padding.Bottom(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Bottom(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Padding.Bottom(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Padding.Bottom(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Padding.Bottom(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Bottom(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Bottom padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes bottom padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = Padding.Bottom(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Bottom(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small bottom padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = Padding.Bottom(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium bottom padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Padding.Bottom(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large bottom padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = Padding.Bottom(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Bottom(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Bottom padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes bottom padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Padding.Bottom(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Bottom(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small bottom padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Padding.Bottom(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium bottom padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Padding.Bottom(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large bottom padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Padding.Bottom(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Bottom(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Bottom padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes bottom padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = Padding.Bottom(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Bottom(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small bottom padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = Padding.Bottom(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium bottom padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Padding.Bottom(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large bottom padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = Padding.Bottom(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Bottom(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Bottom padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Bottom(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Bottom(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Bottom(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Bottom(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Bottom(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Bottom(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Bottom padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Bottom(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall =
          Padding.Bottom(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Bottom(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Bottom(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Bottom(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge =
          Padding.Bottom(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset left padding values. Use nested breakpoint modules (e.g., <c>Left.Small</c>) for responsive variants.
    /// </summary>
    module Left =

      /// <summary>
      /// Removes left padding (0px).
      /// </summary>
      let none = Padding.Left(None, None)

      /// <summary>
      /// Applies extra-small left padding (4px).
      /// </summary>
      let extraSmall = Padding.Left(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small left padding (8px).
      /// </summary>
      let small = Padding.Left(Some Size.Small, None)

      /// <summary>
      /// Applies medium left padding (12px).
      /// </summary>
      let medium = Padding.Left(Some Size.Medium, None)

      /// <summary>
      /// Applies large left padding (16px).
      /// </summary>
      let large = Padding.Left(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large left padding (20px).
      /// </summary>
      let extraLarge = Padding.Left(Some Size.ExtraLarge, None)

      /// <summary>
      /// Left padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes left padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Padding.Left(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Left(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small left padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Padding.Left(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium left padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Padding.Left(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large left padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Padding.Left(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Left(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Left padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes left padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = Padding.Left(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small left padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Left(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small left padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = Padding.Left(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium left padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Padding.Left(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large left padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = Padding.Left(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large left padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Left(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Left padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes left padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Padding.Left(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small left padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Left(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small left padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Padding.Left(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium left padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Padding.Left(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large left padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Padding.Left(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large left padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Left(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Left padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes left padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = Padding.Left(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small left padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Left(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small left padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = Padding.Left(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium left padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Padding.Left(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large left padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = Padding.Left(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large left padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Left(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Left padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes left padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Left(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Left(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small left padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Left(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium left padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Left(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large left padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Left(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Left(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Left padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes left padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Left(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Left(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small left padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Left(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium left padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Left(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large left padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Left(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Left(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset right padding values. Use nested breakpoint modules (e.g., <c>Right.Small</c>) for responsive variants.
    /// </summary>
    module Right =

      /// <summary>
      /// Removes right padding (0px).
      /// </summary>
      let none = Padding.Right(None, None)

      /// <summary>
      /// Applies extra-small right padding (4px).
      /// </summary>
      let extraSmall = Padding.Right(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small right padding (8px).
      /// </summary>
      let small = Padding.Right(Some Size.Small, None)

      /// <summary>
      /// Applies medium right padding (12px).
      /// </summary>
      let medium = Padding.Right(Some Size.Medium, None)

      /// <summary>
      /// Applies large right padding (16px).
      /// </summary>
      let large = Padding.Right(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large right padding (20px).
      /// </summary>
      let extraLarge = Padding.Right(Some Size.ExtraLarge, None)

      /// <summary>
      /// Right padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes right padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Padding.Right(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Right(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small right padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Padding.Right(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium right padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Padding.Right(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large right padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Padding.Right(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Right(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Right padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes right padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = Padding.Right(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small right padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Right(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small right padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = Padding.Right(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium right padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Padding.Right(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large right padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = Padding.Right(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large right padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Right(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Right padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes right padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Padding.Right(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small right padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Right(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small right padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Padding.Right(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium right padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Padding.Right(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large right padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Padding.Right(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large right padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Right(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Right padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes right padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = Padding.Right(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small right padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Right(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small right padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = Padding.Right(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium right padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Padding.Right(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large right padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = Padding.Right(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large right padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Right(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Right padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes right padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Right(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Right(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small right padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Right(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium right padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Right(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large right padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Right(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Right(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Right padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes right padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Right(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall =
          Padding.Right(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small right padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Right(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium right padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Right(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large right padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Right(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge =
          Padding.Right(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset vertical padding (top and bottom) values. Use nested breakpoint modules (e.g., <c>Vertical.Small</c>) for responsive variants.
    /// </summary>
    module Vertical =

      /// <summary>
      /// Removes vertical padding (top and bottom) (0px).
      /// </summary>
      let none = Padding.Vertical(None, None)

      /// <summary>
      /// Applies extra-small vertical padding (top and bottom) (4px).
      /// </summary>
      let extraSmall = Padding.Vertical(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small vertical padding (top and bottom) (8px).
      /// </summary>
      let small = Padding.Vertical(Some Size.Small, None)

      /// <summary>
      /// Applies medium vertical padding (top and bottom) (12px).
      /// </summary>
      let medium = Padding.Vertical(Some Size.Medium, None)

      /// <summary>
      /// Applies large vertical padding (top and bottom) (16px).
      /// </summary>
      let large = Padding.Vertical(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large vertical padding (top and bottom) (20px).
      /// </summary>
      let extraLarge = Padding.Vertical(Some Size.ExtraLarge, None)

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Padding.Vertical(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Vertical(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Padding.Vertical(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Padding.Vertical(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Padding.Vertical(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Vertical(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the small breakpoint and above.
        /// </summary>
        let none = Padding.Vertical(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Vertical(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the small breakpoint and above.
        /// </summary>
        let small = Padding.Vertical(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Padding.Vertical(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the small breakpoint and above.
        /// </summary>
        let large = Padding.Vertical(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Vertical(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Padding.Vertical(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Vertical(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Padding.Vertical(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Padding.Vertical(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Padding.Vertical(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Vertical(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the large breakpoint and above.
        /// </summary>
        let none = Padding.Vertical(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Vertical(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the large breakpoint and above.
        /// </summary>
        let small = Padding.Vertical(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Padding.Vertical(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the large breakpoint and above.
        /// </summary>
        let large = Padding.Vertical(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Vertical(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Vertical(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Vertical(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Vertical(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Vertical(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Vertical(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Vertical(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Vertical(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall =
          Padding.Vertical(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Vertical(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Vertical(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Vertical(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge =
          Padding.Vertical(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset horizontal padding (left and right) values. Use nested breakpoint modules (e.g., <c>Horizontal.Small</c>) for responsive variants.
    /// </summary>
    module Horizontal =

      /// <summary>
      /// Removes horizontal padding (left and right) (0px).
      /// </summary>
      let none = Padding.Horizontal(None, None)

      /// <summary>
      /// Applies extra-small horizontal padding (left and right) (4px).
      /// </summary>
      let extraSmall = Padding.Horizontal(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small horizontal padding (left and right) (8px).
      /// </summary>
      let small = Padding.Horizontal(Some Size.Small, None)

      /// <summary>
      /// Applies medium horizontal padding (left and right) (12px).
      /// </summary>
      let medium = Padding.Horizontal(Some Size.Medium, None)

      /// <summary>
      /// Applies large horizontal padding (left and right) (16px).
      /// </summary>
      let large = Padding.Horizontal(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large horizontal padding (left and right) (20px).
      /// </summary>
      let extraLarge = Padding.Horizontal(Some Size.ExtraLarge, None)

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Padding.Horizontal(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall =
          Padding.Horizontal(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Padding.Horizontal(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Padding.Horizontal(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Padding.Horizontal(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge =
          Padding.Horizontal(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Horizontal padding (left and right) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the small breakpoint and above.
        /// </summary>
        let none = Padding.Horizontal(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Horizontal(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the small breakpoint and above.
        /// </summary>
        let small = Padding.Horizontal(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Padding.Horizontal(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the small breakpoint and above.
        /// </summary>
        let large = Padding.Horizontal(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Horizontal(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Horizontal padding (left and right) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Padding.Horizontal(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Horizontal(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Padding.Horizontal(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Padding.Horizontal(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Padding.Horizontal(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Horizontal(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Horizontal padding (left and right) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the large breakpoint and above.
        /// </summary>
        let none = Padding.Horizontal(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.Horizontal(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the large breakpoint and above.
        /// </summary>
        let small = Padding.Horizontal(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Padding.Horizontal(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the large breakpoint and above.
        /// </summary>
        let large = Padding.Horizontal(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.Horizontal(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Horizontal(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall =
          Padding.Horizontal(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Horizontal(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Horizontal(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Horizontal(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge =
          Padding.Horizontal(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Padding.Horizontal(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall =
          Padding.Horizontal(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Padding.Horizontal(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.Horizontal(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Padding.Horizontal(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge =
          Padding.Horizontal(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Preset padding on all sides values. Use nested breakpoint modules (e.g., <c>All.Small</c>) for responsive variants.
    /// </summary>
    module All =

      /// <summary>
      /// Removes padding on all sides (0px).
      /// </summary>
      let none = Padding.All(None, None)

      /// <summary>
      /// Applies extra-small padding on all sides (4px).
      /// </summary>
      let extraSmall = Padding.All(Some Size.ExtraSmall, None)

      /// <summary>
      /// Applies small padding on all sides (8px).
      /// </summary>
      let small = Padding.All(Some Size.Small, None)

      /// <summary>
      /// Applies medium padding on all sides (12px).
      /// </summary>
      let medium = Padding.All(Some Size.Medium, None)

      /// <summary>
      /// Applies large padding on all sides (16px).
      /// </summary>
      let large = Padding.All(Some Size.Large, None)

      /// <summary>
      /// Applies extra-large padding on all sides (20px).
      /// </summary>
      let extraLarge = Padding.All(Some Size.ExtraLarge, None)

      /// <summary>
      /// Padding on all sides values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = Padding.All(None, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.All(Some Size.ExtraSmall, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = Padding.All(Some Size.Small, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = Padding.All(Some Size.Medium, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = Padding.All(Some Size.Large, Some Breakpoint.ExtraSmall)

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.All(Some Size.ExtraLarge, Some Breakpoint.ExtraSmall)

      /// <summary>
      /// Padding on all sides values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes padding on all sides (0px) at the small breakpoint and above.
        /// </summary>
        let none = Padding.All(None, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = Padding.All(Some Size.ExtraSmall, Some Breakpoint.Small)

        /// <summary>
        /// Applies small padding on all sides (8px) at the small breakpoint and above.
        /// </summary>
        let small = Padding.All(Some Size.Small, Some Breakpoint.Small)

        /// <summary>
        /// Applies medium padding on all sides (12px) at the small breakpoint and above.
        /// </summary>
        let medium = Padding.All(Some Size.Medium, Some Breakpoint.Small)

        /// <summary>
        /// Applies large padding on all sides (16px) at the small breakpoint and above.
        /// </summary>
        let large = Padding.All(Some Size.Large, Some Breakpoint.Small)

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = Padding.All(Some Size.ExtraLarge, Some Breakpoint.Small)

      /// <summary>
      /// Padding on all sides values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes padding on all sides (0px) at the medium breakpoint and above.
        /// </summary>
        let none = Padding.All(None, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = Padding.All(Some Size.ExtraSmall, Some Breakpoint.Medium)

        /// <summary>
        /// Applies small padding on all sides (8px) at the medium breakpoint and above.
        /// </summary>
        let small = Padding.All(Some Size.Small, Some Breakpoint.Medium)

        /// <summary>
        /// Applies medium padding on all sides (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = Padding.All(Some Size.Medium, Some Breakpoint.Medium)

        /// <summary>
        /// Applies large padding on all sides (16px) at the medium breakpoint and above.
        /// </summary>
        let large = Padding.All(Some Size.Large, Some Breakpoint.Medium)

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = Padding.All(Some Size.ExtraLarge, Some Breakpoint.Medium)

      /// <summary>
      /// Padding on all sides values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes padding on all sides (0px) at the large breakpoint and above.
        /// </summary>
        let none = Padding.All(None, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.All(Some Size.ExtraSmall, Some Breakpoint.Large)

        /// <summary>
        /// Applies small padding on all sides (8px) at the large breakpoint and above.
        /// </summary>
        let small = Padding.All(Some Size.Small, Some Breakpoint.Large)

        /// <summary>
        /// Applies medium padding on all sides (12px) at the large breakpoint and above.
        /// </summary>
        let medium = Padding.All(Some Size.Medium, Some Breakpoint.Large)

        /// <summary>
        /// Applies large padding on all sides (16px) at the large breakpoint and above.
        /// </summary>
        let large = Padding.All(Some Size.Large, Some Breakpoint.Large)

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.All(Some Size.ExtraLarge, Some Breakpoint.Large)

      /// <summary>
      /// Padding on all sides values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = Padding.All(None, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.All(Some Size.ExtraSmall, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = Padding.All(Some Size.Small, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.All(Some Size.Medium, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = Padding.All(Some Size.Large, Some Breakpoint.ExtraLarge)

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.All(Some Size.ExtraLarge, Some Breakpoint.ExtraLarge)

      /// <summary>
      /// Padding on all sides values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = Padding.All(None, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = Padding.All(Some Size.ExtraSmall, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = Padding.All(Some Size.Small, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = Padding.All(Some Size.Medium, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = Padding.All(Some Size.Large, Some Breakpoint.ExtraExtraLarge)

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = Padding.All(Some Size.ExtraLarge, Some Breakpoint.ExtraExtraLarge)

    /// <summary>
    /// Converts a Padding value to its corresponding CSS utility class names.
    /// </summary>
    let toClasses padding =
      let bp = SpacingHelpers.breakpointPrefix
      let sz = SpacingHelpers.sizeToNum

      match padding with
      | Padding.Top(size, b) -> [ $"pt-{bp b}{sz size}" ]
      | Padding.Bottom(size, b) -> [ $"pb-{bp b}{sz size}" ]
      | Padding.Left(size, b) -> [ $"pl-{bp b}{sz size}" ]
      | Padding.Right(size, b) -> [ $"pr-{bp b}{sz size}" ]
      | Padding.Vertical(size, b) -> [ $"pt-{bp b}{sz size}"; $"pb-{bp b}{sz size}" ]
      | Padding.Horizontal(size, b) -> [ $"pl-{bp b}{sz size}"; $"pr-{bp b}{sz size}" ]
      | Padding.All(size, b) -> [ $"pa-{bp b}{sz size}" ]

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

  module BrowserUtils =

    let windowWidth: Var<int> = Var.Create JS.Window.InnerWidth

    do JS.Window.AddEventListener("resize", fun (e: Dom.Event) -> windowWidth.Value <- JS.Window.InnerWidth)

  module Breakpoint =

    let browser =
      BrowserUtils.windowWidth.View
      |> View.MapCached(fun width ->
        if width < 600 then Breakpoint.ExtraSmall
        elif width < 960 then Breakpoint.Small
        elif width < 1280 then Breakpoint.Medium
        elif width < 1920 then Breakpoint.Large
        elif width < 2560 then Breakpoint.ExtraLarge
        else Breakpoint.ExtraExtraLarge)

    let browserAsText =
      browser
      |> View.MapCached(fun bp ->
        match bp with
        | Breakpoint.ExtraSmall -> "XS"
        | Breakpoint.Small -> "SM"
        | Breakpoint.Medium -> "MD"
        | Breakpoint.Large -> "LG"
        | Breakpoint.ExtraLarge -> "XL"
        | Breakpoint.ExtraExtraLarge -> "XXL")

  [<RequireQualifiedAccess; Struct>]
  type Orientation =
    | Portrait
    | Landscape

  module Orientation =

    let browser =
      BrowserUtils.windowWidth.View
      |> View.MapCached(fun width ->
        if JS.Window.InnerHeight >= width then
          Orientation.Portrait
        else
          Orientation.Landscape)

  [<RequireQualifiedAccess; Struct>]
  type Flex =
    | Flex
    | Inline
    | InlineBlock

  /// <summary>
  /// Controls if a container is a flex box
  /// </summary>
  module Flex =

    let toClass breakpoint flex =
      match breakpoint, flex with
      | None, Flex.Flex -> Css.``d-flex``
      | None, Flex.Inline -> Css.``d-inline-flex``
      | None, Flex.InlineBlock -> Css.``d-inline-block``
      | Some Breakpoint.ExtraSmall, Flex.Flex -> Css.``d-xs-flex``
      | Some Breakpoint.ExtraSmall, Flex.Inline -> Css.``d-xs-inline-flex``
      | Some Breakpoint.ExtraSmall, Flex.InlineBlock -> Css.``d-xs-inline-block``
      | Some Breakpoint.Small, Flex.Flex -> Css.``d-sm-flex``
      | Some Breakpoint.Small, Flex.Inline -> Css.``d-sm-inline-flex``
      | Some Breakpoint.Small, Flex.InlineBlock -> Css.``d-sm-inline-block``
      | Some Breakpoint.Medium, Flex.Flex -> Css.``d-md-flex``
      | Some Breakpoint.Medium, Flex.Inline -> Css.``d-md-inline-flex``
      | Some Breakpoint.Medium, Flex.InlineBlock -> Css.``d-md-inline-block``
      | Some Breakpoint.Large, Flex.Flex -> Css.``d-lg-flex``
      | Some Breakpoint.Large, Flex.Inline -> Css.``d-lg-inline-flex``
      | Some Breakpoint.Large, Flex.InlineBlock -> Css.``d-lg-inline-block``
      | Some Breakpoint.ExtraLarge, Flex.Flex -> Css.``d-xl-flex``
      | Some Breakpoint.ExtraLarge, Flex.Inline -> Css.``d-xl-inline-flex``
      | Some Breakpoint.ExtraLarge, Flex.InlineBlock -> Css.``d-xl-inline-block``
      | Some Breakpoint.ExtraExtraLarge, Flex.Flex -> Css.``d-xxl-flex``
      | Some Breakpoint.ExtraExtraLarge, Flex.Inline -> Css.``d-xxl-inline-flex``
      | Some Breakpoint.ExtraExtraLarge, Flex.InlineBlock -> Css.``d-xxl-inline-block``

    module Flex =

      let allSizes = toClass None Flex.Flex
      let xs = toClass (Some Breakpoint.ExtraSmall) Flex.Flex
      let sm = toClass (Some Breakpoint.Small) Flex.Flex
      let md = toClass (Some Breakpoint.Medium) Flex.Flex
      let lg = toClass (Some Breakpoint.Large) Flex.Flex
      let xl = toClass (Some Breakpoint.ExtraLarge) Flex.Flex
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) Flex.Flex

    module Inline =

      let allSizes = toClass None Flex.Inline
      let xs = toClass (Some Breakpoint.ExtraSmall) Flex.Inline
      let sm = toClass (Some Breakpoint.Small) Flex.Inline
      let md = toClass (Some Breakpoint.Medium) Flex.Inline
      let lg = toClass (Some Breakpoint.Large) Flex.Inline
      let xl = toClass (Some Breakpoint.ExtraLarge) Flex.Inline
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) Flex.Inline

    module InlineBlock =

      let allSizes = toClass None Flex.InlineBlock
      let xs = toClass (Some Breakpoint.ExtraSmall) Flex.InlineBlock
      let sm = toClass (Some Breakpoint.Small) Flex.InlineBlock
      let md = toClass (Some Breakpoint.Medium) Flex.InlineBlock
      let lg = toClass (Some Breakpoint.Large) Flex.InlineBlock
      let xl = toClass (Some Breakpoint.ExtraLarge) Flex.InlineBlock
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) Flex.InlineBlock

  [<RequireQualifiedAccess; Struct>]
  type FlexWrap =
    | NoWrap
    | Wrap
    | WrapReverse

  /// <summary>
  /// Controls the wrap of flex items. Place on a flex container.
  /// </summary>
  module FlexWrap =

    let toClass breakpoint wrap =
      match breakpoint, wrap with
      | None, FlexWrap.NoWrap -> Css.``flex-nowrap``
      | None, FlexWrap.Wrap -> Css.``flex-wrap``
      | None, FlexWrap.WrapReverse -> Css.``flex-wrap-reverse``
      | Some Breakpoint.ExtraSmall, FlexWrap.NoWrap -> Css.``flex-xs-nowrap``
      | Some Breakpoint.ExtraSmall, FlexWrap.Wrap -> Css.``flex-xs-wrap``
      | Some Breakpoint.ExtraSmall, FlexWrap.WrapReverse -> Css.``flex-xs-wrap-reverse``
      | Some Breakpoint.Small, FlexWrap.NoWrap -> Css.``flex-sm-nowrap``
      | Some Breakpoint.Small, FlexWrap.Wrap -> Css.``flex-sm-wrap``
      | Some Breakpoint.Small, FlexWrap.WrapReverse -> Css.``flex-sm-wrap-reverse``
      | Some Breakpoint.Medium, FlexWrap.NoWrap -> Css.``flex-md-nowrap``
      | Some Breakpoint.Medium, FlexWrap.Wrap -> Css.``flex-md-wrap``
      | Some Breakpoint.Medium, FlexWrap.WrapReverse -> Css.``flex-md-wrap-reverse``
      | Some Breakpoint.Large, FlexWrap.NoWrap -> Css.``flex-lg-nowrap``
      | Some Breakpoint.Large, FlexWrap.Wrap -> Css.``flex-lg-wrap``
      | Some Breakpoint.Large, FlexWrap.WrapReverse -> Css.``flex-lg-wrap-reverse``
      | Some Breakpoint.ExtraLarge, FlexWrap.NoWrap -> Css.``flex-xl-nowrap``
      | Some Breakpoint.ExtraLarge, FlexWrap.Wrap -> Css.``flex-xl-wrap``
      | Some Breakpoint.ExtraLarge, FlexWrap.WrapReverse -> Css.``flex-xl-wrap-reverse``
      | Some Breakpoint.ExtraExtraLarge, FlexWrap.NoWrap -> Css.``flex-xxl-nowrap``
      | Some Breakpoint.ExtraExtraLarge, FlexWrap.Wrap -> Css.``flex-xxl-wrap``
      | Some Breakpoint.ExtraExtraLarge, FlexWrap.WrapReverse -> Css.``flex-xxl-wrap-reverse``

    module NoWrap =

      let allSizes = toClass None FlexWrap.NoWrap
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexWrap.NoWrap
      let sm = toClass (Some Breakpoint.Small) FlexWrap.NoWrap
      let md = toClass (Some Breakpoint.Medium) FlexWrap.NoWrap
      let lg = toClass (Some Breakpoint.Large) FlexWrap.NoWrap
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexWrap.NoWrap
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexWrap.NoWrap

    module Wrap =

      let allSizes = toClass None FlexWrap.Wrap
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexWrap.Wrap
      let sm = toClass (Some Breakpoint.Small) FlexWrap.Wrap
      let md = toClass (Some Breakpoint.Medium) FlexWrap.Wrap
      let lg = toClass (Some Breakpoint.Large) FlexWrap.Wrap
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexWrap.Wrap
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexWrap.Wrap

    module WrapReverse =

      let allSizes = toClass None FlexWrap.WrapReverse
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexWrap.WrapReverse
      let sm = toClass (Some Breakpoint.Small) FlexWrap.WrapReverse
      let md = toClass (Some Breakpoint.Medium) FlexWrap.WrapReverse
      let lg = toClass (Some Breakpoint.Large) FlexWrap.WrapReverse
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexWrap.WrapReverse
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexWrap.WrapReverse

  [<RequireQualifiedAccess; Struct>]
  type FlexDirection =
    | Row
    | RowReverse
    | Column
    | ColumnReverse

  /// <summary>
  /// Controls the direction of flex items. Place on a flex container.
  /// </summary>
  module FlexDirection =

    let toClass breakpoint direction =
      match breakpoint, direction with
      | None, FlexDirection.Row -> Css.``flex-row``
      | None, FlexDirection.RowReverse -> Css.``flex-row-reverse``
      | None, FlexDirection.Column -> Css.``flex-column``
      | None, FlexDirection.ColumnReverse -> Css.``flex-column-reverse``
      | Some Breakpoint.ExtraSmall, FlexDirection.Row -> Css.``flex-xs-row``
      | Some Breakpoint.ExtraSmall, FlexDirection.RowReverse -> Css.``flex-xs-row-reverse``
      | Some Breakpoint.ExtraSmall, FlexDirection.Column -> Css.``flex-xs-column``
      | Some Breakpoint.ExtraSmall, FlexDirection.ColumnReverse -> Css.``flex-xs-column-reverse``
      | Some Breakpoint.Small, FlexDirection.Row -> Css.``flex-sm-row``
      | Some Breakpoint.Small, FlexDirection.RowReverse -> Css.``flex-sm-row-reverse``
      | Some Breakpoint.Small, FlexDirection.Column -> Css.``flex-sm-column``
      | Some Breakpoint.Small, FlexDirection.ColumnReverse -> Css.``flex-sm-column-reverse``
      | Some Breakpoint.Medium, FlexDirection.Row -> Css.``flex-md-row``
      | Some Breakpoint.Medium, FlexDirection.RowReverse -> Css.``flex-md-row-reverse``
      | Some Breakpoint.Medium, FlexDirection.Column -> Css.``flex-md-column``
      | Some Breakpoint.Medium, FlexDirection.ColumnReverse -> Css.``flex-md-column-reverse``
      | Some Breakpoint.Large, FlexDirection.Row -> Css.``flex-lg-row``
      | Some Breakpoint.Large, FlexDirection.RowReverse -> Css.``flex-lg-row-reverse``
      | Some Breakpoint.Large, FlexDirection.Column -> Css.``flex-lg-column``
      | Some Breakpoint.Large, FlexDirection.ColumnReverse -> Css.``flex-lg-column-reverse``
      | Some Breakpoint.ExtraLarge, FlexDirection.Row -> Css.``flex-xl-row``
      | Some Breakpoint.ExtraLarge, FlexDirection.RowReverse -> Css.``flex-xl-row-reverse``
      | Some Breakpoint.ExtraLarge, FlexDirection.Column -> Css.``flex-xl-column``
      | Some Breakpoint.ExtraLarge, FlexDirection.ColumnReverse -> Css.``flex-xl-column-reverse``
      | Some Breakpoint.ExtraExtraLarge, FlexDirection.Row -> Css.``flex-xxl-row``
      | Some Breakpoint.ExtraExtraLarge, FlexDirection.RowReverse -> Css.``flex-xxl-row-reverse``
      | Some Breakpoint.ExtraExtraLarge, FlexDirection.Column -> Css.``flex-xxl-column``
      | Some Breakpoint.ExtraExtraLarge, FlexDirection.ColumnReverse -> Css.``flex-xxl-column-reverse``

    module Row =

      let allSizes = toClass None FlexDirection.Row
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexDirection.Row
      let sm = toClass (Some Breakpoint.Small) FlexDirection.Row
      let md = toClass (Some Breakpoint.Medium) FlexDirection.Row
      let lg = toClass (Some Breakpoint.Large) FlexDirection.Row
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexDirection.Row
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexDirection.Row

    module RowReverse =

      let allSizes = toClass None FlexDirection.RowReverse
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexDirection.RowReverse
      let sm = toClass (Some Breakpoint.Small) FlexDirection.RowReverse
      let md = toClass (Some Breakpoint.Medium) FlexDirection.RowReverse
      let lg = toClass (Some Breakpoint.Large) FlexDirection.RowReverse
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexDirection.RowReverse
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexDirection.RowReverse

    module Column =

      let allSizes = toClass None FlexDirection.Column
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexDirection.Column
      let sm = toClass (Some Breakpoint.Small) FlexDirection.Column
      let md = toClass (Some Breakpoint.Medium) FlexDirection.Column
      let lg = toClass (Some Breakpoint.Large) FlexDirection.Column
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexDirection.Column
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexDirection.Column

    module ColumnReverse =

      let allSizes = toClass None FlexDirection.ColumnReverse
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexDirection.ColumnReverse
      let sm = toClass (Some Breakpoint.Small) FlexDirection.ColumnReverse
      let md = toClass (Some Breakpoint.Medium) FlexDirection.ColumnReverse
      let lg = toClass (Some Breakpoint.Large) FlexDirection.ColumnReverse
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexDirection.ColumnReverse
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexDirection.ColumnReverse

  [<RequireQualifiedAccess; Struct>]
  type FlexItem =
    | Flex
    | Auto
    | Initial
    | Grow
    | NoGrow
    | Shrink
    | NoShrink
    | None

  /// <summary>
  /// Utilities for controlling how flex items both grow and shrink. Place on items within a flex container.
  /// </summary>
  module FlexItem =

    let toClass breakpoint flex =
      match breakpoint, flex with
      | None, FlexItem.Flex -> Css.``flex-1``
      | None, FlexItem.Auto -> Css.``flex-auto``
      | None, FlexItem.Initial -> Css.``flex-initial``
      | None, FlexItem.Grow -> Css.``flex-grow-1``
      | None, FlexItem.NoGrow -> Css.``flex-grow-0``
      | None, FlexItem.Shrink -> Css.``flex-shrink-1``
      | None, FlexItem.NoShrink -> Css.``flex-shrink-0``
      | None, FlexItem.None -> Css.``flex-none``
      | Some Breakpoint.ExtraSmall, FlexItem.Flex -> Css.``flex-xs-1``
      | Some Breakpoint.ExtraSmall, FlexItem.Auto -> Css.``flex-xs-auto``
      | Some Breakpoint.ExtraSmall, FlexItem.Initial -> Css.``flex-xs-initial``
      | Some Breakpoint.ExtraSmall, FlexItem.Grow -> Css.``flex-xs-grow-1``
      | Some Breakpoint.ExtraSmall, FlexItem.NoGrow -> Css.``flex-xs-grow-0``
      | Some Breakpoint.ExtraSmall, FlexItem.Shrink -> Css.``flex-xs-shrink-1``
      | Some Breakpoint.ExtraSmall, FlexItem.NoShrink -> Css.``flex-xs-shrink-0``
      | Some Breakpoint.ExtraSmall, FlexItem.None -> Css.``flex-xs-none``
      | Some Breakpoint.Small, FlexItem.Flex -> Css.``flex-sm-1``
      | Some Breakpoint.Small, FlexItem.Auto -> Css.``flex-sm-auto``
      | Some Breakpoint.Small, FlexItem.Initial -> Css.``flex-sm-initial``
      | Some Breakpoint.Small, FlexItem.Grow -> Css.``flex-sm-grow-1``
      | Some Breakpoint.Small, FlexItem.NoGrow -> Css.``flex-sm-grow-0``
      | Some Breakpoint.Small, FlexItem.Shrink -> Css.``flex-sm-shrink-1``
      | Some Breakpoint.Small, FlexItem.NoShrink -> Css.``flex-sm-shrink-0``
      | Some Breakpoint.Small, FlexItem.None -> Css.``flex-sm-none``
      | Some Breakpoint.Medium, FlexItem.Flex -> Css.``flex-md-1``
      | Some Breakpoint.Medium, FlexItem.Auto -> Css.``flex-md-auto``
      | Some Breakpoint.Medium, FlexItem.Initial -> Css.``flex-md-initial``
      | Some Breakpoint.Medium, FlexItem.Grow -> Css.``flex-md-grow-1``
      | Some Breakpoint.Medium, FlexItem.NoGrow -> Css.``flex-md-grow-0``
      | Some Breakpoint.Medium, FlexItem.Shrink -> Css.``flex-md-shrink-1``
      | Some Breakpoint.Medium, FlexItem.NoShrink -> Css.``flex-md-shrink-0``
      | Some Breakpoint.Medium, FlexItem.None -> Css.``flex-md-none``
      | Some Breakpoint.Large, FlexItem.Flex -> Css.``flex-lg-1``
      | Some Breakpoint.Large, FlexItem.Auto -> Css.``flex-lg-auto``
      | Some Breakpoint.Large, FlexItem.Initial -> Css.``flex-lg-initial``
      | Some Breakpoint.Large, FlexItem.Grow -> Css.``flex-lg-grow-1``
      | Some Breakpoint.Large, FlexItem.NoGrow -> Css.``flex-lg-grow-0``
      | Some Breakpoint.Large, FlexItem.Shrink -> Css.``flex-lg-shrink-1``
      | Some Breakpoint.Large, FlexItem.NoShrink -> Css.``flex-lg-shrink-0``
      | Some Breakpoint.Large, FlexItem.None -> Css.``flex-lg-none``
      | Some Breakpoint.ExtraLarge, FlexItem.Flex -> Css.``flex-xl-1``
      | Some Breakpoint.ExtraLarge, FlexItem.Auto -> Css.``flex-xl-auto``
      | Some Breakpoint.ExtraLarge, FlexItem.Initial -> Css.``flex-xl-initial``
      | Some Breakpoint.ExtraLarge, FlexItem.Grow -> Css.``flex-xl-grow-1``
      | Some Breakpoint.ExtraLarge, FlexItem.NoGrow -> Css.``flex-xl-grow-0``
      | Some Breakpoint.ExtraLarge, FlexItem.Shrink -> Css.``flex-xl-shrink-1``
      | Some Breakpoint.ExtraLarge, FlexItem.NoShrink -> Css.``flex-xl-shrink-0``
      | Some Breakpoint.ExtraLarge, FlexItem.None -> Css.``flex-xl-none``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Flex -> Css.``flex-xxl-1``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Auto -> Css.``flex-xxl-auto``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Initial -> Css.``flex-xxl-initial``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Grow -> Css.``flex-xxl-grow-1``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.NoGrow -> Css.``flex-xxl-grow-0``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Shrink -> Css.``flex-xxl-shrink-1``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.NoShrink -> Css.``flex-xxl-shrink-0``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.None -> Css.``flex-xxl-none``

    module Flex =

      let allSizes = toClass None FlexItem.Flex
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Flex
      let sm = toClass (Some Breakpoint.Small) FlexItem.Flex
      let md = toClass (Some Breakpoint.Medium) FlexItem.Flex
      let lg = toClass (Some Breakpoint.Large) FlexItem.Flex
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Flex
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Flex

    module Auto =

      let allSizes = toClass None FlexItem.Auto
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Auto
      let sm = toClass (Some Breakpoint.Small) FlexItem.Auto
      let md = toClass (Some Breakpoint.Medium) FlexItem.Auto
      let lg = toClass (Some Breakpoint.Large) FlexItem.Auto
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Auto
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Auto

    module Initial =

      let allSizes = toClass None FlexItem.Initial
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Initial
      let sm = toClass (Some Breakpoint.Small) FlexItem.Initial
      let md = toClass (Some Breakpoint.Medium) FlexItem.Initial
      let lg = toClass (Some Breakpoint.Large) FlexItem.Initial
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Initial
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Initial

    module Grow =

      let allSizes = toClass None FlexItem.Grow
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Grow
      let sm = toClass (Some Breakpoint.Small) FlexItem.Grow
      let md = toClass (Some Breakpoint.Medium) FlexItem.Grow
      let lg = toClass (Some Breakpoint.Large) FlexItem.Grow
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Grow
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Grow

    module NoGrow =

      let allSizes = toClass None FlexItem.NoGrow
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.NoGrow
      let sm = toClass (Some Breakpoint.Small) FlexItem.NoGrow
      let md = toClass (Some Breakpoint.Medium) FlexItem.NoGrow
      let lg = toClass (Some Breakpoint.Large) FlexItem.NoGrow
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.NoGrow
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.NoGrow

    module Shrink =

      let allSizes = toClass None FlexItem.Shrink
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Shrink
      let sm = toClass (Some Breakpoint.Small) FlexItem.Shrink
      let md = toClass (Some Breakpoint.Medium) FlexItem.Shrink
      let lg = toClass (Some Breakpoint.Large) FlexItem.Shrink
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Shrink
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Shrink

    module NoShrink =

      let allSizes = toClass None FlexItem.NoShrink
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.NoShrink
      let sm = toClass (Some Breakpoint.Small) FlexItem.NoShrink
      let md = toClass (Some Breakpoint.Medium) FlexItem.NoShrink
      let lg = toClass (Some Breakpoint.Large) FlexItem.NoShrink
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.NoShrink
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.NoShrink

    module None =

      let allSizes = toClass None FlexItem.None
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.None
      let sm = toClass (Some Breakpoint.Small) FlexItem.None
      let md = toClass (Some Breakpoint.Medium) FlexItem.None
      let lg = toClass (Some Breakpoint.Large) FlexItem.None
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.None
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.None

  [<RequireQualifiedAccess; Struct>]
  type JustifyContent =
    | FlexStart
    | Center
    | FlexEnd
    | SpaceBetween
    | SpaceAround
    | SpaceEvenly

  /// <summary>
  /// Controls how flex and grid items are positioned along a container's main axis. Place on a flex or grid container.
  /// </summary>
  module JustifyContent =

    let toClass justify =
      match justify with
      | JustifyContent.FlexStart -> Css.``justify-start``
      | JustifyContent.Center -> Css.``justify-center``
      | JustifyContent.FlexEnd -> Css.``justify-end``
      | JustifyContent.SpaceBetween -> Css.``justify-space-between``
      | JustifyContent.SpaceAround -> Css.``justify-space-around``
      | JustifyContent.SpaceEvenly -> Css.``justify-space-evenly``

  [<RequireQualifiedAccess; Struct>]
  type AlignContent =
    | Center
    | Start
    | End
    | SpaceBetween
    | SpaceAround
    | Stretch

  /// <summary>
  /// Controls how rows are positioned in a flex or grid container. Place on a flex or grid container.
  /// </summary>
  module AlignContent =

    let toClass align =
      match align with
      | AlignContent.Center -> Css.``align-content-center``
      | AlignContent.Start -> Css.``align-content-start``
      | AlignContent.End -> Css.``align-content-end``
      | AlignContent.SpaceBetween -> Css.``align-content-space-between``
      | AlignContent.SpaceAround -> Css.``align-content-space-around``
      | AlignContent.Stretch -> Css.``align-content-stretch``

  [<RequireQualifiedAccess; Struct>]
  type AlignItems =
    | Baseline
    | Center
    | Start
    | End
    | Stretch

  /// <summary>
  /// Controls how flex and grid items are positioned along a container's cross axis. Place on a flex or grid container.
  /// </summary>
  module AlignItems =

    let toClass align =
      match align with
      | AlignItems.Baseline -> Css.``align-baseline``
      | AlignItems.Center -> Css.``align-center``
      | AlignItems.Start -> Css.``align-start``
      | AlignItems.End -> Css.``align-end``
      | AlignItems.Stretch -> Css.``align-stretch``

  [<RequireQualifiedAccess; Struct>]
  type AlignSelf =
    | Auto
    | Center
    | Start
    | End
    | Stretch

  /// <summary>
  /// Controls how an individual flex or grid item is positioned along its container's cross axis. Place on a flex or grid item.
  /// </summary>
  module AlignSelf =

    let toClass align =
      match align with
      | AlignSelf.Auto -> Css.``align-self-auto``
      | AlignSelf.Center -> Css.``align-self-center``
      | AlignSelf.Start -> Css.``align-self-start``
      | AlignSelf.End -> Css.``align-self-end``
      | AlignSelf.Stretch -> Css.``align-self-stretch``

  [<RequireQualifiedAccess; Struct>]
  type Visibility =
    | AlwaysVisible
    | AlwaysHidden
    | HideOnlyOn of Breakpoint
    | VisibleOnlyOn of Breakpoint

  [<RequireQualifiedAccess; Struct>]
  type Display =
    | Block of Visibility
    | Flex of Visibility
    | Inline of Visibility
    | InlineBlock of Visibility
    | InlineFlex of Visibility

  module Display =

    let toClasses display =
      match display with
      | Display.Block visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-block`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-block`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-block``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-block``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-block``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-block``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-block``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-none``; Css.``d-xxl-block`` ]
        | Visibility.AlwaysVisible -> [ Css.``d-block`` ]
      | Display.Flex visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-flex``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-flex``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-flex``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-flex``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-flex``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-none``; Css.``d-xxl-flex`` ]
        | Visibility.AlwaysVisible -> [ Css.``d-flex`` ]
      | Display.Inline visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-inline``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-inline``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-inline``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-inline``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-inline``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-none``; Css.``d-xxl-inline`` ]
        | Visibility.AlwaysVisible -> [ Css.``d-inline`` ]
      | Display.InlineBlock visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-inline-block``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-inline-block``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-inline-block``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-inline-block``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-inline-block``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [
            Css.``d-none``
            Css.``d-xxl-inline-block``
          ]
        | Visibility.AlwaysVisible -> [ Css.``d-inline-block`` ]
      | Display.InlineFlex visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-inline-flex``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-inline-flex``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-inline-flex``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-inline-flex``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-inline-flex``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-none``; Css.``d-xxl-inline-flex`` ]
        | Visibility.AlwaysVisible -> [ Css.``d-inline-flex`` ]

  [<RequireQualifiedAccess; Struct>]
  type Overflow =
    | Auto
    | Hidden
    | Visible
    | Scroll
    | XAuto
    | XHidden
    | YAuto
    | YHidden

  module Overflow =

    let toClass overflow =
      match overflow with
      | Overflow.Auto -> Css.``overflow-auto``
      | Overflow.Hidden -> Css.``overflow-hidden``
      | Overflow.Visible -> Css.``overflow-visible``
      | Overflow.Scroll -> Css.``overflow-scroll``
      | Overflow.XAuto -> Css.``overflow-x-auto``
      | Overflow.XHidden -> Css.``overflow-x-hidden``
      | Overflow.YAuto -> Css.``overflow-y-auto``
      | Overflow.YHidden -> Css.``overflow-y-hidden``

  [<RequireQualifiedAccess; Struct>]
  type ObjectFit =
    | Fill
    | Contain
    | Cover
    | None
    | ScaleDown

  /// <summary>
  /// Specifies how a replaced element's content should be resized
  /// </summary>
  module ObjectFit =

    let toClass objectFit =
      match objectFit with
      | ObjectFit.Fill -> Css.``object-fill``
      | ObjectFit.Contain -> Css.``object-contain``
      | ObjectFit.Cover -> Css.``object-cover``
      | ObjectFit.None -> Css.``object-none``
      | ObjectFit.ScaleDown -> Css.``object-scale-down``

  [<RequireQualifiedAccess; Struct>]
  type ObjectPosition =
    | Bottom
    | Center
    | Left
    | LeftBottom
    | LeftTop
    | Right
    | RightBottom
    | RightTop

  /// <summary>
  /// Specifies how a replaced element's content should be positioned within its container
  /// </summary>
  module ObjectPosition =

    let toClass objectPosition =
      match objectPosition with
      | ObjectPosition.Bottom -> Css.``object-bottom``
      | ObjectPosition.Center -> Css.``object-center``
      | ObjectPosition.Left -> Css.``object-left``
      | ObjectPosition.LeftBottom -> Css.``object-left-bottom``
      | ObjectPosition.LeftTop -> Css.``object-left-top``
      | ObjectPosition.Right -> Css.``object-right``
      | ObjectPosition.RightBottom -> Css.``object-right-bottom``
      | ObjectPosition.RightTop -> Css.``object-right-top``

  [<RequireQualifiedAccess; Struct>]
  type Position =
    | Static
    | Relative
    | Absolute
    | Fixed
    | Sticky

  /// <summary>
  /// Utilities for controlling how an element is positioned in the DOM
  /// </summary>
  module Position =

    let toClass position =
      match position with
      | Position.Static -> Css.``static``
      | Position.Relative -> Css.``relative``
      | Position.Absolute -> Css.``absolute``
      | Position.Fixed -> Css.``fixed``
      | Position.Sticky -> Css.``sticky``
