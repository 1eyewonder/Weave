namespace Weave.CssHelpers

open WebSharper
open Weave.CssHelpers.Core

[<AutoOpen; JavaScript>]
module Spacing =
  module Density =

    let toClass density =
      match density with
      | Density.Compact -> Css.``weave-density--compact``
      | Density.Standard -> Css.``weave-density--standard``
      | Density.Spacious -> Css.``weave-density--spacious``

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
