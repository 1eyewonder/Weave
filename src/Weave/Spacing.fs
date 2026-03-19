namespace Weave.CssHelpers

open WebSharper
open WebSharper.UI
open Weave.CssHelpers.Core

[<AutoOpen; JavaScript>]
module Spacing =
  module Density =

    let compact = cl Css.``weave-density--compact``
    let standard = cl Css.``weave-density--standard``
    let spacious = cl Css.``weave-density--spacious``

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
      let none = cl Css.``mt-0``

      /// <summary>
      /// Applies extra-small top margin (4px).
      /// </summary>
      let extraSmall = cl Css.``mt-4``

      /// <summary>
      /// Applies small top margin (8px).
      /// </summary>
      let small = cl Css.``mt-8``

      /// <summary>
      /// Applies medium top margin (12px).
      /// </summary>
      let medium = cl Css.``mt-12``

      /// <summary>
      /// Applies large top margin (16px).
      /// </summary>
      let large = cl Css.``mt-16``

      /// <summary>
      /// Applies extra-large top margin (20px).
      /// </summary>
      let extraLarge = cl Css.``mt-20``

      /// <summary>
      /// Top margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes top margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``mt-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mt-4``

        /// <summary>
        /// Applies small top margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``mt-8``

        /// <summary>
        /// Applies medium top margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``mt-12``

        /// <summary>
        /// Applies large top margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``mt-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mt-20``

      /// <summary>
      /// Top margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes top margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``mt-sm-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mt-sm-4``

        /// <summary>
        /// Applies small top margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``mt-sm-8``

        /// <summary>
        /// Applies medium top margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``mt-sm-12``

        /// <summary>
        /// Applies large top margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``mt-sm-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mt-sm-20``

      /// <summary>
      /// Top margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes top margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``mt-md-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mt-md-4``

        /// <summary>
        /// Applies small top margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``mt-md-8``

        /// <summary>
        /// Applies medium top margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``mt-md-12``

        /// <summary>
        /// Applies large top margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``mt-md-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mt-md-20``

      /// <summary>
      /// Top margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes top margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``mt-lg-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mt-lg-4``

        /// <summary>
        /// Applies small top margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``mt-lg-8``

        /// <summary>
        /// Applies medium top margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mt-lg-12``

        /// <summary>
        /// Applies large top margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``mt-lg-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mt-lg-20``

      /// <summary>
      /// Top margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes top margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``mt-xl-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mt-xl-4``

        /// <summary>
        /// Applies small top margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``mt-xl-8``

        /// <summary>
        /// Applies medium top margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mt-xl-12``

        /// <summary>
        /// Applies large top margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``mt-xl-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mt-xl-20``

      /// <summary>
      /// Top margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes top margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``mt-xxl-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mt-xxl-4``

        /// <summary>
        /// Applies small top margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``mt-xxl-8``

        /// <summary>
        /// Applies medium top margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mt-xxl-12``

        /// <summary>
        /// Applies large top margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``mt-xxl-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mt-xxl-20``

    /// <summary>
    /// Preset bottom margin values. Use nested breakpoint modules (e.g., <c>Bottom.Small</c>) for responsive variants.
    /// </summary>
    module Bottom =

      /// <summary>
      /// Removes bottom margin (0px).
      /// </summary>
      let none = cl Css.``mb-0``

      /// <summary>
      /// Applies extra-small bottom margin (4px).
      /// </summary>
      let extraSmall = cl Css.``mb-4``

      /// <summary>
      /// Applies small bottom margin (8px).
      /// </summary>
      let small = cl Css.``mb-8``

      /// <summary>
      /// Applies medium bottom margin (12px).
      /// </summary>
      let medium = cl Css.``mb-12``

      /// <summary>
      /// Applies large bottom margin (16px).
      /// </summary>
      let large = cl Css.``mb-16``

      /// <summary>
      /// Applies extra-large bottom margin (20px).
      /// </summary>
      let extraLarge = cl Css.``mb-20``

      /// <summary>
      /// Bottom margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``mb-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mb-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``mb-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``mb-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``mb-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mb-20``

      /// <summary>
      /// Bottom margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes bottom margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``mb-sm-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mb-sm-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``mb-sm-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``mb-sm-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``mb-sm-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mb-sm-20``

      /// <summary>
      /// Bottom margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes bottom margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``mb-md-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mb-md-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``mb-md-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``mb-md-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``mb-md-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mb-md-20``

      /// <summary>
      /// Bottom margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes bottom margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``mb-lg-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mb-lg-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``mb-lg-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mb-lg-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``mb-lg-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mb-lg-20``

      /// <summary>
      /// Bottom margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``mb-xl-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mb-xl-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``mb-xl-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mb-xl-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``mb-xl-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mb-xl-20``

      /// <summary>
      /// Bottom margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``mb-xxl-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mb-xxl-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``mb-xxl-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mb-xxl-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``mb-xxl-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mb-xxl-20``

    /// <summary>
    /// Preset left margin values. Use nested breakpoint modules (e.g., <c>Left.Small</c>) for responsive variants.
    /// </summary>
    module Left =

      /// <summary>
      /// Removes left margin (0px).
      /// </summary>
      let none = cl Css.``ml-0``

      /// <summary>
      /// Applies extra-small left margin (4px).
      /// </summary>
      let extraSmall = cl Css.``ml-4``

      /// <summary>
      /// Applies small left margin (8px).
      /// </summary>
      let small = cl Css.``ml-8``

      /// <summary>
      /// Applies medium left margin (12px).
      /// </summary>
      let medium = cl Css.``ml-12``

      /// <summary>
      /// Applies large left margin (16px).
      /// </summary>
      let large = cl Css.``ml-16``

      /// <summary>
      /// Applies extra-large left margin (20px).
      /// </summary>
      let extraLarge = cl Css.``ml-20``

      /// <summary>
      /// Left margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes left margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``ml-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ml-4``

        /// <summary>
        /// Applies small left margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``ml-8``

        /// <summary>
        /// Applies medium left margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``ml-12``

        /// <summary>
        /// Applies large left margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``ml-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ml-20``

      /// <summary>
      /// Left margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes left margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``ml-sm-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ml-sm-4``

        /// <summary>
        /// Applies small left margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``ml-sm-8``

        /// <summary>
        /// Applies medium left margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``ml-sm-12``

        /// <summary>
        /// Applies large left margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``ml-sm-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ml-sm-20``

      /// <summary>
      /// Left margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes left margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``ml-md-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ml-md-4``

        /// <summary>
        /// Applies small left margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``ml-md-8``

        /// <summary>
        /// Applies medium left margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``ml-md-12``

        /// <summary>
        /// Applies large left margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``ml-md-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ml-md-20``

      /// <summary>
      /// Left margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes left margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``ml-lg-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ml-lg-4``

        /// <summary>
        /// Applies small left margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``ml-lg-8``

        /// <summary>
        /// Applies medium left margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``ml-lg-12``

        /// <summary>
        /// Applies large left margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``ml-lg-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ml-lg-20``

      /// <summary>
      /// Left margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes left margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``ml-xl-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ml-xl-4``

        /// <summary>
        /// Applies small left margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``ml-xl-8``

        /// <summary>
        /// Applies medium left margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``ml-xl-12``

        /// <summary>
        /// Applies large left margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``ml-xl-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ml-xl-20``

      /// <summary>
      /// Left margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes left margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``ml-xxl-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ml-xxl-4``

        /// <summary>
        /// Applies small left margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``ml-xxl-8``

        /// <summary>
        /// Applies medium left margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``ml-xxl-12``

        /// <summary>
        /// Applies large left margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``ml-xxl-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ml-xxl-20``

    /// <summary>
    /// Preset right margin values. Use nested breakpoint modules (e.g., <c>Right.Small</c>) for responsive variants.
    /// </summary>
    module Right =

      /// <summary>
      /// Removes right margin (0px).
      /// </summary>
      let none = cl Css.``mr-0``

      /// <summary>
      /// Applies extra-small right margin (4px).
      /// </summary>
      let extraSmall = cl Css.``mr-4``

      /// <summary>
      /// Applies small right margin (8px).
      /// </summary>
      let small = cl Css.``mr-8``

      /// <summary>
      /// Applies medium right margin (12px).
      /// </summary>
      let medium = cl Css.``mr-12``

      /// <summary>
      /// Applies large right margin (16px).
      /// </summary>
      let large = cl Css.``mr-16``

      /// <summary>
      /// Applies extra-large right margin (20px).
      /// </summary>
      let extraLarge = cl Css.``mr-20``

      /// <summary>
      /// Right margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes right margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``mr-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mr-4``

        /// <summary>
        /// Applies small right margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``mr-8``

        /// <summary>
        /// Applies medium right margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``mr-12``

        /// <summary>
        /// Applies large right margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``mr-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mr-20``

      /// <summary>
      /// Right margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes right margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``mr-sm-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mr-sm-4``

        /// <summary>
        /// Applies small right margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``mr-sm-8``

        /// <summary>
        /// Applies medium right margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``mr-sm-12``

        /// <summary>
        /// Applies large right margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``mr-sm-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mr-sm-20``

      /// <summary>
      /// Right margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes right margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``mr-md-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mr-md-4``

        /// <summary>
        /// Applies small right margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``mr-md-8``

        /// <summary>
        /// Applies medium right margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``mr-md-12``

        /// <summary>
        /// Applies large right margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``mr-md-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mr-md-20``

      /// <summary>
      /// Right margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes right margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``mr-lg-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mr-lg-4``

        /// <summary>
        /// Applies small right margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``mr-lg-8``

        /// <summary>
        /// Applies medium right margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mr-lg-12``

        /// <summary>
        /// Applies large right margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``mr-lg-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mr-lg-20``

      /// <summary>
      /// Right margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes right margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``mr-xl-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mr-xl-4``

        /// <summary>
        /// Applies small right margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``mr-xl-8``

        /// <summary>
        /// Applies medium right margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mr-xl-12``

        /// <summary>
        /// Applies large right margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``mr-xl-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mr-xl-20``

      /// <summary>
      /// Right margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes right margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``mr-xxl-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``mr-xxl-4``

        /// <summary>
        /// Applies small right margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``mr-xxl-8``

        /// <summary>
        /// Applies medium right margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``mr-xxl-12``

        /// <summary>
        /// Applies large right margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``mr-xxl-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``mr-xxl-20``

    /// <summary>
    /// Preset vertical margin (top and bottom) values. Use nested breakpoint modules (e.g., <c>Vertical.Small</c>) for responsive variants.
    /// </summary>
    module Vertical =

      /// <summary>
      /// Removes vertical margin (top and bottom) (0px).
      /// </summary>
      let none = cls [ Css.``mt-0``; Css.``mb-0`` ]

      /// <summary>
      /// Applies extra-small vertical margin (top and bottom) (4px).
      /// </summary>
      let extraSmall = cls [ Css.``mt-4``; Css.``mb-4`` ]

      /// <summary>
      /// Applies small vertical margin (top and bottom) (8px).
      /// </summary>
      let small = cls [ Css.``mt-8``; Css.``mb-8`` ]

      /// <summary>
      /// Applies medium vertical margin (top and bottom) (12px).
      /// </summary>
      let medium = cls [ Css.``mt-12``; Css.``mb-12`` ]

      /// <summary>
      /// Applies large vertical margin (top and bottom) (16px).
      /// </summary>
      let large = cls [ Css.``mt-16``; Css.``mb-16`` ]

      /// <summary>
      /// Applies extra-large vertical margin (top and bottom) (20px).
      /// </summary>
      let extraLarge = cls [ Css.``mt-20``; Css.``mb-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``mt-0``; Css.``mb-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``mt-4``; Css.``mb-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``mt-8``; Css.``mb-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``mt-12``; Css.``mb-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``mt-16``; Css.``mb-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``mt-20``; Css.``mb-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``mt-sm-0``; Css.``mb-sm-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``mt-sm-4``; Css.``mb-sm-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``mt-sm-8``; Css.``mb-sm-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``mt-sm-12``; Css.``mb-sm-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``mt-sm-16``; Css.``mb-sm-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``mt-sm-20``; Css.``mb-sm-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cls [ Css.``mt-md-0``; Css.``mb-md-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``mt-md-4``; Css.``mb-md-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cls [ Css.``mt-md-8``; Css.``mb-md-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``mt-md-12``; Css.``mb-md-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cls [ Css.``mt-md-16``; Css.``mb-md-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``mt-md-20``; Css.``mb-md-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``mt-lg-0``; Css.``mb-lg-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``mt-lg-4``; Css.``mb-lg-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``mt-lg-8``; Css.``mb-lg-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``mt-lg-12``; Css.``mb-lg-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``mt-lg-16``; Css.``mb-lg-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``mt-lg-20``; Css.``mb-lg-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``mt-xl-0``; Css.``mb-xl-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``mt-xl-4``; Css.``mb-xl-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``mt-xl-8``; Css.``mb-xl-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``mt-xl-12``; Css.``mb-xl-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``mt-xl-16``; Css.``mb-xl-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``mt-xl-20``; Css.``mb-xl-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``mt-xxl-0``; Css.``mb-xxl-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``mt-xxl-4``; Css.``mb-xxl-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``mt-xxl-8``; Css.``mb-xxl-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``mt-xxl-12``; Css.``mb-xxl-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``mt-xxl-16``; Css.``mb-xxl-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``mt-xxl-20``; Css.``mb-xxl-20`` ]

    /// <summary>
    /// Preset horizontal margin (left and right) values. Use nested breakpoint modules (e.g., <c>Horizontal.Small</c>) for responsive variants.
    /// </summary>
    module Horizontal =

      /// <summary>
      /// Removes horizontal margin (left and right) (0px).
      /// </summary>
      let none = cls [ Css.``ml-0``; Css.``mr-0`` ]

      /// <summary>
      /// Applies extra-small horizontal margin (left and right) (4px).
      /// </summary>
      let extraSmall = cls [ Css.``ml-4``; Css.``mr-4`` ]

      /// <summary>
      /// Applies small horizontal margin (left and right) (8px).
      /// </summary>
      let small = cls [ Css.``ml-8``; Css.``mr-8`` ]

      /// <summary>
      /// Applies medium horizontal margin (left and right) (12px).
      /// </summary>
      let medium = cls [ Css.``ml-12``; Css.``mr-12`` ]

      /// <summary>
      /// Applies large horizontal margin (left and right) (16px).
      /// </summary>
      let large = cls [ Css.``ml-16``; Css.``mr-16`` ]

      /// <summary>
      /// Applies extra-large horizontal margin (left and right) (20px).
      /// </summary>
      let extraLarge = cls [ Css.``ml-20``; Css.``mr-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``ml-0``; Css.``mr-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``ml-4``; Css.``mr-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``ml-8``; Css.``mr-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``ml-12``; Css.``mr-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``ml-16``; Css.``mr-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``ml-20``; Css.``mr-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``ml-sm-0``; Css.``mr-sm-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``ml-sm-4``; Css.``mr-sm-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``ml-sm-8``; Css.``mr-sm-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``ml-sm-12``; Css.``mr-sm-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``ml-sm-16``; Css.``mr-sm-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``ml-sm-20``; Css.``mr-sm-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cls [ Css.``ml-md-0``; Css.``mr-md-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``ml-md-4``; Css.``mr-md-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cls [ Css.``ml-md-8``; Css.``mr-md-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``ml-md-12``; Css.``mr-md-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cls [ Css.``ml-md-16``; Css.``mr-md-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``ml-md-20``; Css.``mr-md-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``ml-lg-0``; Css.``mr-lg-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``ml-lg-4``; Css.``mr-lg-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``ml-lg-8``; Css.``mr-lg-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``ml-lg-12``; Css.``mr-lg-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``ml-lg-16``; Css.``mr-lg-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``ml-lg-20``; Css.``mr-lg-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``ml-xl-0``; Css.``mr-xl-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``ml-xl-4``; Css.``mr-xl-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``ml-xl-8``; Css.``mr-xl-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``ml-xl-12``; Css.``mr-xl-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``ml-xl-16``; Css.``mr-xl-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``ml-xl-20``; Css.``mr-xl-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``ml-xxl-0``; Css.``mr-xxl-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``ml-xxl-4``; Css.``mr-xxl-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``ml-xxl-8``; Css.``mr-xxl-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``ml-xxl-12``; Css.``mr-xxl-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``ml-xxl-16``; Css.``mr-xxl-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``ml-xxl-20``; Css.``mr-xxl-20`` ]

    /// <summary>
    /// Preset margin on all sides values. Use nested breakpoint modules (e.g., <c>All.Small</c>) for responsive variants.
    /// </summary>
    module All =

      /// <summary>
      /// Removes margin on all sides (0px).
      /// </summary>
      let none = cl Css.``ma-0``

      /// <summary>
      /// Applies extra-small margin on all sides (4px).
      /// </summary>
      let extraSmall = cl Css.``ma-4``

      /// <summary>
      /// Applies small margin on all sides (8px).
      /// </summary>
      let small = cl Css.``ma-8``

      /// <summary>
      /// Applies medium margin on all sides (12px).
      /// </summary>
      let medium = cl Css.``ma-12``

      /// <summary>
      /// Applies large margin on all sides (16px).
      /// </summary>
      let large = cl Css.``ma-16``

      /// <summary>
      /// Applies extra-large margin on all sides (20px).
      /// </summary>
      let extraLarge = cl Css.``ma-20``

      /// <summary>
      /// Margin on all sides values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``ma-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ma-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``ma-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``ma-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``ma-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ma-20``

      /// <summary>
      /// Margin on all sides values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes margin on all sides (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``ma-sm-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ma-sm-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``ma-sm-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``ma-sm-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``ma-sm-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ma-sm-20``

      /// <summary>
      /// Margin on all sides values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes margin on all sides (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``ma-md-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ma-md-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``ma-md-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``ma-md-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``ma-md-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ma-md-20``

      /// <summary>
      /// Margin on all sides values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes margin on all sides (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``ma-lg-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ma-lg-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``ma-lg-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``ma-lg-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``ma-lg-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ma-lg-20``

      /// <summary>
      /// Margin on all sides values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``ma-xl-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ma-xl-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``ma-xl-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``ma-xl-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``ma-xl-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ma-xl-20``

      /// <summary>
      /// Margin on all sides values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``ma-xxl-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``ma-xxl-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``ma-xxl-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``ma-xxl-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``ma-xxl-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``ma-xxl-20``

    /// <summary>
    /// Converts a Margin value to its corresponding CSS utility class names.
    /// </summary>
    let private toClasses margin =
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
    /// Converts a Margin value to an <c>Attr</c> applying the corresponding CSS utility classes.
    /// </summary>
    let toAttr (margin: Margin) : Attr = cls (toClasses margin)

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
      let none = cl Css.``pt-0``

      /// <summary>
      /// Applies extra-small top padding (4px).
      /// </summary>
      let extraSmall = cl Css.``pt-4``

      /// <summary>
      /// Applies small top padding (8px).
      /// </summary>
      let small = cl Css.``pt-8``

      /// <summary>
      /// Applies medium top padding (12px).
      /// </summary>
      let medium = cl Css.``pt-12``

      /// <summary>
      /// Applies large top padding (16px).
      /// </summary>
      let large = cl Css.``pt-16``

      /// <summary>
      /// Applies extra-large top padding (20px).
      /// </summary>
      let extraLarge = cl Css.``pt-20``

      /// <summary>
      /// Top padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes top padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``pt-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pt-4``

        /// <summary>
        /// Applies small top padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``pt-8``

        /// <summary>
        /// Applies medium top padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pt-12``

        /// <summary>
        /// Applies large top padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``pt-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pt-20``

      /// <summary>
      /// Top padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes top padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``pt-sm-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pt-sm-4``

        /// <summary>
        /// Applies small top padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``pt-sm-8``

        /// <summary>
        /// Applies medium top padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pt-sm-12``

        /// <summary>
        /// Applies large top padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``pt-sm-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pt-sm-20``

      /// <summary>
      /// Top padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes top padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``pt-md-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pt-md-4``

        /// <summary>
        /// Applies small top padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``pt-md-8``

        /// <summary>
        /// Applies medium top padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``pt-md-12``

        /// <summary>
        /// Applies large top padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``pt-md-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pt-md-20``

      /// <summary>
      /// Top padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes top padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``pt-lg-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pt-lg-4``

        /// <summary>
        /// Applies small top padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``pt-lg-8``

        /// <summary>
        /// Applies medium top padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pt-lg-12``

        /// <summary>
        /// Applies large top padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``pt-lg-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pt-lg-20``

      /// <summary>
      /// Top padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes top padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pt-xl-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pt-xl-4``

        /// <summary>
        /// Applies small top padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pt-xl-8``

        /// <summary>
        /// Applies medium top padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pt-xl-12``

        /// <summary>
        /// Applies large top padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pt-xl-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pt-xl-20``

      /// <summary>
      /// Top padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes top padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pt-xxl-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pt-xxl-4``

        /// <summary>
        /// Applies small top padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pt-xxl-8``

        /// <summary>
        /// Applies medium top padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pt-xxl-12``

        /// <summary>
        /// Applies large top padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pt-xxl-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pt-xxl-20``

    /// <summary>
    /// Preset bottom padding values. Use nested breakpoint modules (e.g., <c>Bottom.Small</c>) for responsive variants.
    /// </summary>
    module Bottom =

      /// <summary>
      /// Removes bottom padding (0px).
      /// </summary>
      let none = cl Css.``pb-0``

      /// <summary>
      /// Applies extra-small bottom padding (4px).
      /// </summary>
      let extraSmall = cl Css.``pb-4``

      /// <summary>
      /// Applies small bottom padding (8px).
      /// </summary>
      let small = cl Css.``pb-8``

      /// <summary>
      /// Applies medium bottom padding (12px).
      /// </summary>
      let medium = cl Css.``pb-12``

      /// <summary>
      /// Applies large bottom padding (16px).
      /// </summary>
      let large = cl Css.``pb-16``

      /// <summary>
      /// Applies extra-large bottom padding (20px).
      /// </summary>
      let extraLarge = cl Css.``pb-20``

      /// <summary>
      /// Bottom padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``pb-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pb-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``pb-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pb-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``pb-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pb-20``

      /// <summary>
      /// Bottom padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes bottom padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``pb-sm-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pb-sm-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``pb-sm-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pb-sm-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``pb-sm-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pb-sm-20``

      /// <summary>
      /// Bottom padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes bottom padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``pb-md-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pb-md-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``pb-md-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``pb-md-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``pb-md-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pb-md-20``

      /// <summary>
      /// Bottom padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes bottom padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``pb-lg-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pb-lg-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``pb-lg-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pb-lg-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``pb-lg-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pb-lg-20``

      /// <summary>
      /// Bottom padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pb-xl-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pb-xl-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pb-xl-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pb-xl-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pb-xl-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pb-xl-20``

      /// <summary>
      /// Bottom padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pb-xxl-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pb-xxl-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pb-xxl-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pb-xxl-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pb-xxl-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pb-xxl-20``

    /// <summary>
    /// Preset left padding values. Use nested breakpoint modules (e.g., <c>Left.Small</c>) for responsive variants.
    /// </summary>
    module Left =

      /// <summary>
      /// Removes left padding (0px).
      /// </summary>
      let none = cl Css.``pl-0``

      /// <summary>
      /// Applies extra-small left padding (4px).
      /// </summary>
      let extraSmall = cl Css.``pl-4``

      /// <summary>
      /// Applies small left padding (8px).
      /// </summary>
      let small = cl Css.``pl-8``

      /// <summary>
      /// Applies medium left padding (12px).
      /// </summary>
      let medium = cl Css.``pl-12``

      /// <summary>
      /// Applies large left padding (16px).
      /// </summary>
      let large = cl Css.``pl-16``

      /// <summary>
      /// Applies extra-large left padding (20px).
      /// </summary>
      let extraLarge = cl Css.``pl-20``

      /// <summary>
      /// Left padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes left padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``pl-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pl-4``

        /// <summary>
        /// Applies small left padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``pl-8``

        /// <summary>
        /// Applies medium left padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pl-12``

        /// <summary>
        /// Applies large left padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``pl-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pl-20``

      /// <summary>
      /// Left padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes left padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``pl-sm-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pl-sm-4``

        /// <summary>
        /// Applies small left padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``pl-sm-8``

        /// <summary>
        /// Applies medium left padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pl-sm-12``

        /// <summary>
        /// Applies large left padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``pl-sm-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pl-sm-20``

      /// <summary>
      /// Left padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes left padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``pl-md-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pl-md-4``

        /// <summary>
        /// Applies small left padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``pl-md-8``

        /// <summary>
        /// Applies medium left padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``pl-md-12``

        /// <summary>
        /// Applies large left padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``pl-md-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pl-md-20``

      /// <summary>
      /// Left padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes left padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``pl-lg-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pl-lg-4``

        /// <summary>
        /// Applies small left padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``pl-lg-8``

        /// <summary>
        /// Applies medium left padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pl-lg-12``

        /// <summary>
        /// Applies large left padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``pl-lg-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pl-lg-20``

      /// <summary>
      /// Left padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes left padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pl-xl-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pl-xl-4``

        /// <summary>
        /// Applies small left padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pl-xl-8``

        /// <summary>
        /// Applies medium left padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pl-xl-12``

        /// <summary>
        /// Applies large left padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pl-xl-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pl-xl-20``

      /// <summary>
      /// Left padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes left padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pl-xxl-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pl-xxl-4``

        /// <summary>
        /// Applies small left padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pl-xxl-8``

        /// <summary>
        /// Applies medium left padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pl-xxl-12``

        /// <summary>
        /// Applies large left padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pl-xxl-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pl-xxl-20``

    /// <summary>
    /// Preset right padding values. Use nested breakpoint modules (e.g., <c>Right.Small</c>) for responsive variants.
    /// </summary>
    module Right =

      /// <summary>
      /// Removes right padding (0px).
      /// </summary>
      let none = cl Css.``pr-0``

      /// <summary>
      /// Applies extra-small right padding (4px).
      /// </summary>
      let extraSmall = cl Css.``pr-4``

      /// <summary>
      /// Applies small right padding (8px).
      /// </summary>
      let small = cl Css.``pr-8``

      /// <summary>
      /// Applies medium right padding (12px).
      /// </summary>
      let medium = cl Css.``pr-12``

      /// <summary>
      /// Applies large right padding (16px).
      /// </summary>
      let large = cl Css.``pr-16``

      /// <summary>
      /// Applies extra-large right padding (20px).
      /// </summary>
      let extraLarge = cl Css.``pr-20``

      /// <summary>
      /// Right padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes right padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``pr-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pr-4``

        /// <summary>
        /// Applies small right padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``pr-8``

        /// <summary>
        /// Applies medium right padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pr-12``

        /// <summary>
        /// Applies large right padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``pr-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pr-20``

      /// <summary>
      /// Right padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes right padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``pr-sm-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pr-sm-4``

        /// <summary>
        /// Applies small right padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``pr-sm-8``

        /// <summary>
        /// Applies medium right padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pr-sm-12``

        /// <summary>
        /// Applies large right padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``pr-sm-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pr-sm-20``

      /// <summary>
      /// Right padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes right padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``pr-md-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pr-md-4``

        /// <summary>
        /// Applies small right padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``pr-md-8``

        /// <summary>
        /// Applies medium right padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``pr-md-12``

        /// <summary>
        /// Applies large right padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``pr-md-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pr-md-20``

      /// <summary>
      /// Right padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes right padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``pr-lg-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pr-lg-4``

        /// <summary>
        /// Applies small right padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``pr-lg-8``

        /// <summary>
        /// Applies medium right padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pr-lg-12``

        /// <summary>
        /// Applies large right padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``pr-lg-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pr-lg-20``

      /// <summary>
      /// Right padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes right padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pr-xl-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pr-xl-4``

        /// <summary>
        /// Applies small right padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pr-xl-8``

        /// <summary>
        /// Applies medium right padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pr-xl-12``

        /// <summary>
        /// Applies large right padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pr-xl-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pr-xl-20``

      /// <summary>
      /// Right padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes right padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pr-xxl-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pr-xxl-4``

        /// <summary>
        /// Applies small right padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pr-xxl-8``

        /// <summary>
        /// Applies medium right padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pr-xxl-12``

        /// <summary>
        /// Applies large right padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pr-xxl-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pr-xxl-20``

    /// <summary>
    /// Preset vertical padding (top and bottom) values. Use nested breakpoint modules (e.g., <c>Vertical.Small</c>) for responsive variants.
    /// </summary>
    module Vertical =

      /// <summary>
      /// Removes vertical padding (top and bottom) (0px).
      /// </summary>
      let none = cls [ Css.``pt-0``; Css.``pb-0`` ]

      /// <summary>
      /// Applies extra-small vertical padding (top and bottom) (4px).
      /// </summary>
      let extraSmall = cls [ Css.``pt-4``; Css.``pb-4`` ]

      /// <summary>
      /// Applies small vertical padding (top and bottom) (8px).
      /// </summary>
      let small = cls [ Css.``pt-8``; Css.``pb-8`` ]

      /// <summary>
      /// Applies medium vertical padding (top and bottom) (12px).
      /// </summary>
      let medium = cls [ Css.``pt-12``; Css.``pb-12`` ]

      /// <summary>
      /// Applies large vertical padding (top and bottom) (16px).
      /// </summary>
      let large = cls [ Css.``pt-16``; Css.``pb-16`` ]

      /// <summary>
      /// Applies extra-large vertical padding (top and bottom) (20px).
      /// </summary>
      let extraLarge = cls [ Css.``pt-20``; Css.``pb-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pt-0``; Css.``pb-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pt-4``; Css.``pb-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pt-8``; Css.``pb-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pt-12``; Css.``pb-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pt-16``; Css.``pb-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pt-20``; Css.``pb-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pt-sm-0``; Css.``pb-sm-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pt-sm-4``; Css.``pb-sm-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pt-sm-8``; Css.``pb-sm-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pt-sm-12``; Css.``pb-sm-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pt-sm-16``; Css.``pb-sm-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pt-sm-20``; Css.``pb-sm-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pt-md-0``; Css.``pb-md-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pt-md-4``; Css.``pb-md-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pt-md-8``; Css.``pb-md-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pt-md-12``; Css.``pb-md-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pt-md-16``; Css.``pb-md-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pt-md-20``; Css.``pb-md-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pt-lg-0``; Css.``pb-lg-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pt-lg-4``; Css.``pb-lg-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pt-lg-8``; Css.``pb-lg-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pt-lg-12``; Css.``pb-lg-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pt-lg-16``; Css.``pb-lg-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pt-lg-20``; Css.``pb-lg-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pt-xl-0``; Css.``pb-xl-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pt-xl-4``; Css.``pb-xl-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pt-xl-8``; Css.``pb-xl-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pt-xl-12``; Css.``pb-xl-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pt-xl-16``; Css.``pb-xl-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pt-xl-20``; Css.``pb-xl-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pt-xxl-0``; Css.``pb-xxl-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pt-xxl-4``; Css.``pb-xxl-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pt-xxl-8``; Css.``pb-xxl-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pt-xxl-12``; Css.``pb-xxl-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pt-xxl-16``; Css.``pb-xxl-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pt-xxl-20``; Css.``pb-xxl-20`` ]

    /// <summary>
    /// Preset horizontal padding (left and right) values. Use nested breakpoint modules (e.g., <c>Horizontal.Small</c>) for responsive variants.
    /// </summary>
    module Horizontal =

      /// <summary>
      /// Removes horizontal padding (left and right) (0px).
      /// </summary>
      let none = cls [ Css.``pl-0``; Css.``pr-0`` ]

      /// <summary>
      /// Applies extra-small horizontal padding (left and right) (4px).
      /// </summary>
      let extraSmall = cls [ Css.``pl-4``; Css.``pr-4`` ]

      /// <summary>
      /// Applies small horizontal padding (left and right) (8px).
      /// </summary>
      let small = cls [ Css.``pl-8``; Css.``pr-8`` ]

      /// <summary>
      /// Applies medium horizontal padding (left and right) (12px).
      /// </summary>
      let medium = cls [ Css.``pl-12``; Css.``pr-12`` ]

      /// <summary>
      /// Applies large horizontal padding (left and right) (16px).
      /// </summary>
      let large = cls [ Css.``pl-16``; Css.``pr-16`` ]

      /// <summary>
      /// Applies extra-large horizontal padding (left and right) (20px).
      /// </summary>
      let extraLarge = cls [ Css.``pl-20``; Css.``pr-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pl-0``; Css.``pr-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pl-4``; Css.``pr-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pl-8``; Css.``pr-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pl-12``; Css.``pr-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pl-16``; Css.``pr-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pl-20``; Css.``pr-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pl-sm-0``; Css.``pr-sm-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pl-sm-4``; Css.``pr-sm-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pl-sm-8``; Css.``pr-sm-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pl-sm-12``; Css.``pr-sm-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pl-sm-16``; Css.``pr-sm-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pl-sm-20``; Css.``pr-sm-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pl-md-0``; Css.``pr-md-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pl-md-4``; Css.``pr-md-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pl-md-8``; Css.``pr-md-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pl-md-12``; Css.``pr-md-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pl-md-16``; Css.``pr-md-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pl-md-20``; Css.``pr-md-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pl-lg-0``; Css.``pr-lg-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pl-lg-4``; Css.``pr-lg-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pl-lg-8``; Css.``pr-lg-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pl-lg-12``; Css.``pr-lg-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pl-lg-16``; Css.``pr-lg-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pl-lg-20``; Css.``pr-lg-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pl-xl-0``; Css.``pr-xl-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pl-xl-4``; Css.``pr-xl-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pl-xl-8``; Css.``pr-xl-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pl-xl-12``; Css.``pr-xl-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pl-xl-16``; Css.``pr-xl-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pl-xl-20``; Css.``pr-xl-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``pl-xxl-0``; Css.``pr-xxl-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``pl-xxl-4``; Css.``pr-xxl-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``pl-xxl-8``; Css.``pr-xxl-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``pl-xxl-12``; Css.``pr-xxl-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``pl-xxl-16``; Css.``pr-xxl-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``pl-xxl-20``; Css.``pr-xxl-20`` ]

    /// <summary>
    /// Preset padding on all sides values. Use nested breakpoint modules (e.g., <c>All.Small</c>) for responsive variants.
    /// </summary>
    module All =

      /// <summary>
      /// Removes padding on all sides (0px).
      /// </summary>
      let none = cl Css.``pa-0``

      /// <summary>
      /// Applies extra-small padding on all sides (4px).
      /// </summary>
      let extraSmall = cl Css.``pa-4``

      /// <summary>
      /// Applies small padding on all sides (8px).
      /// </summary>
      let small = cl Css.``pa-8``

      /// <summary>
      /// Applies medium padding on all sides (12px).
      /// </summary>
      let medium = cl Css.``pa-12``

      /// <summary>
      /// Applies large padding on all sides (16px).
      /// </summary>
      let large = cl Css.``pa-16``

      /// <summary>
      /// Applies extra-large padding on all sides (20px).
      /// </summary>
      let extraLarge = cl Css.``pa-20``

      /// <summary>
      /// Padding on all sides values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``pa-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pa-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``pa-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pa-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``pa-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pa-20``

      /// <summary>
      /// Padding on all sides values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes padding on all sides (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``pa-sm-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pa-sm-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``pa-sm-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``pa-sm-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``pa-sm-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pa-sm-20``

      /// <summary>
      /// Padding on all sides values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes padding on all sides (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``pa-md-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pa-md-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``pa-md-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``pa-md-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``pa-md-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pa-md-20``

      /// <summary>
      /// Padding on all sides values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes padding on all sides (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``pa-lg-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pa-lg-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``pa-lg-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pa-lg-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``pa-lg-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pa-lg-20``

      /// <summary>
      /// Padding on all sides values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pa-xl-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pa-xl-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pa-xl-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pa-xl-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pa-xl-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pa-xl-20``

      /// <summary>
      /// Padding on all sides values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``pa-xxl-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``pa-xxl-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``pa-xxl-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``pa-xxl-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``pa-xxl-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``pa-xxl-20``
