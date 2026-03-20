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
      let none = cl Css.``weave-mt-0``

      /// <summary>
      /// Applies extra-small top margin (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-mt-4``

      /// <summary>
      /// Applies small top margin (8px).
      /// </summary>
      let small = cl Css.``weave-mt-8``

      /// <summary>
      /// Applies medium top margin (12px).
      /// </summary>
      let medium = cl Css.``weave-mt-12``

      /// <summary>
      /// Applies large top margin (16px).
      /// </summary>
      let large = cl Css.``weave-mt-16``

      /// <summary>
      /// Applies extra-large top margin (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-mt-20``

      /// <summary>
      /// Top margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes top margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mt-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mt-4``

        /// <summary>
        /// Applies small top margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mt-8``

        /// <summary>
        /// Applies medium top margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mt-12``

        /// <summary>
        /// Applies large top margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mt-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mt-20``

      /// <summary>
      /// Top margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes top margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mt-sm-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mt-sm-4``

        /// <summary>
        /// Applies small top margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mt-sm-8``

        /// <summary>
        /// Applies medium top margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mt-sm-12``

        /// <summary>
        /// Applies large top margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mt-sm-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mt-sm-20``

      /// <summary>
      /// Top margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes top margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mt-md-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mt-md-4``

        /// <summary>
        /// Applies small top margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mt-md-8``

        /// <summary>
        /// Applies medium top margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mt-md-12``

        /// <summary>
        /// Applies large top margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mt-md-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mt-md-20``

      /// <summary>
      /// Top margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes top margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mt-lg-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mt-lg-4``

        /// <summary>
        /// Applies small top margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mt-lg-8``

        /// <summary>
        /// Applies medium top margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mt-lg-12``

        /// <summary>
        /// Applies large top margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mt-lg-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mt-lg-20``

      /// <summary>
      /// Top margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes top margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mt-xl-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mt-xl-4``

        /// <summary>
        /// Applies small top margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mt-xl-8``

        /// <summary>
        /// Applies medium top margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mt-xl-12``

        /// <summary>
        /// Applies large top margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mt-xl-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mt-xl-20``

      /// <summary>
      /// Top margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes top margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mt-xxl-0``

        /// <summary>
        /// Applies extra-small top margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mt-xxl-4``

        /// <summary>
        /// Applies small top margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mt-xxl-8``

        /// <summary>
        /// Applies medium top margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mt-xxl-12``

        /// <summary>
        /// Applies large top margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mt-xxl-16``

        /// <summary>
        /// Applies extra-large top margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mt-xxl-20``

    /// <summary>
    /// Preset bottom margin values. Use nested breakpoint modules (e.g., <c>Bottom.Small</c>) for responsive variants.
    /// </summary>
    module Bottom =

      /// <summary>
      /// Removes bottom margin (0px).
      /// </summary>
      let none = cl Css.``weave-mb-0``

      /// <summary>
      /// Applies extra-small bottom margin (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-mb-4``

      /// <summary>
      /// Applies small bottom margin (8px).
      /// </summary>
      let small = cl Css.``weave-mb-8``

      /// <summary>
      /// Applies medium bottom margin (12px).
      /// </summary>
      let medium = cl Css.``weave-mb-12``

      /// <summary>
      /// Applies large bottom margin (16px).
      /// </summary>
      let large = cl Css.``weave-mb-16``

      /// <summary>
      /// Applies extra-large bottom margin (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-mb-20``

      /// <summary>
      /// Bottom margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mb-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mb-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mb-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mb-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mb-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mb-20``

      /// <summary>
      /// Bottom margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes bottom margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mb-sm-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mb-sm-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mb-sm-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mb-sm-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mb-sm-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mb-sm-20``

      /// <summary>
      /// Bottom margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes bottom margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mb-md-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mb-md-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mb-md-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mb-md-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mb-md-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mb-md-20``

      /// <summary>
      /// Bottom margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes bottom margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mb-lg-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mb-lg-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mb-lg-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mb-lg-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mb-lg-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mb-lg-20``

      /// <summary>
      /// Bottom margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mb-xl-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mb-xl-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mb-xl-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mb-xl-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mb-xl-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mb-xl-20``

      /// <summary>
      /// Bottom margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes bottom margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mb-xxl-0``

        /// <summary>
        /// Applies extra-small bottom margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mb-xxl-4``

        /// <summary>
        /// Applies small bottom margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mb-xxl-8``

        /// <summary>
        /// Applies medium bottom margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mb-xxl-12``

        /// <summary>
        /// Applies large bottom margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mb-xxl-16``

        /// <summary>
        /// Applies extra-large bottom margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mb-xxl-20``

    /// <summary>
    /// Preset left margin values. Use nested breakpoint modules (e.g., <c>Left.Small</c>) for responsive variants.
    /// </summary>
    module Left =

      /// <summary>
      /// Removes left margin (0px).
      /// </summary>
      let none = cl Css.``weave-ml-0``

      /// <summary>
      /// Applies extra-small left margin (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-ml-4``

      /// <summary>
      /// Applies small left margin (8px).
      /// </summary>
      let small = cl Css.``weave-ml-8``

      /// <summary>
      /// Applies medium left margin (12px).
      /// </summary>
      let medium = cl Css.``weave-ml-12``

      /// <summary>
      /// Applies large left margin (16px).
      /// </summary>
      let large = cl Css.``weave-ml-16``

      /// <summary>
      /// Applies extra-large left margin (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-ml-20``

      /// <summary>
      /// Left margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes left margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ml-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ml-4``

        /// <summary>
        /// Applies small left margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ml-8``

        /// <summary>
        /// Applies medium left margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ml-12``

        /// <summary>
        /// Applies large left margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ml-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ml-20``

      /// <summary>
      /// Left margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes left margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ml-sm-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ml-sm-4``

        /// <summary>
        /// Applies small left margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ml-sm-8``

        /// <summary>
        /// Applies medium left margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ml-sm-12``

        /// <summary>
        /// Applies large left margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ml-sm-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ml-sm-20``

      /// <summary>
      /// Left margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes left margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ml-md-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ml-md-4``

        /// <summary>
        /// Applies small left margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ml-md-8``

        /// <summary>
        /// Applies medium left margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ml-md-12``

        /// <summary>
        /// Applies large left margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ml-md-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ml-md-20``

      /// <summary>
      /// Left margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes left margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ml-lg-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ml-lg-4``

        /// <summary>
        /// Applies small left margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ml-lg-8``

        /// <summary>
        /// Applies medium left margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ml-lg-12``

        /// <summary>
        /// Applies large left margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ml-lg-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ml-lg-20``

      /// <summary>
      /// Left margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes left margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ml-xl-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ml-xl-4``

        /// <summary>
        /// Applies small left margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ml-xl-8``

        /// <summary>
        /// Applies medium left margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ml-xl-12``

        /// <summary>
        /// Applies large left margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ml-xl-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ml-xl-20``

      /// <summary>
      /// Left margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes left margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ml-xxl-0``

        /// <summary>
        /// Applies extra-small left margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ml-xxl-4``

        /// <summary>
        /// Applies small left margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ml-xxl-8``

        /// <summary>
        /// Applies medium left margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ml-xxl-12``

        /// <summary>
        /// Applies large left margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ml-xxl-16``

        /// <summary>
        /// Applies extra-large left margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ml-xxl-20``

    /// <summary>
    /// Preset right margin values. Use nested breakpoint modules (e.g., <c>Right.Small</c>) for responsive variants.
    /// </summary>
    module Right =

      /// <summary>
      /// Removes right margin (0px).
      /// </summary>
      let none = cl Css.``weave-mr-0``

      /// <summary>
      /// Applies extra-small right margin (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-mr-4``

      /// <summary>
      /// Applies small right margin (8px).
      /// </summary>
      let small = cl Css.``weave-mr-8``

      /// <summary>
      /// Applies medium right margin (12px).
      /// </summary>
      let medium = cl Css.``weave-mr-12``

      /// <summary>
      /// Applies large right margin (16px).
      /// </summary>
      let large = cl Css.``weave-mr-16``

      /// <summary>
      /// Applies extra-large right margin (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-mr-20``

      /// <summary>
      /// Right margin values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes right margin (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mr-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mr-4``

        /// <summary>
        /// Applies small right margin (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mr-8``

        /// <summary>
        /// Applies medium right margin (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mr-12``

        /// <summary>
        /// Applies large right margin (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mr-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mr-20``

      /// <summary>
      /// Right margin values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes right margin (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mr-sm-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mr-sm-4``

        /// <summary>
        /// Applies small right margin (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mr-sm-8``

        /// <summary>
        /// Applies medium right margin (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mr-sm-12``

        /// <summary>
        /// Applies large right margin (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mr-sm-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mr-sm-20``

      /// <summary>
      /// Right margin values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes right margin (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mr-md-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mr-md-4``

        /// <summary>
        /// Applies small right margin (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mr-md-8``

        /// <summary>
        /// Applies medium right margin (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mr-md-12``

        /// <summary>
        /// Applies large right margin (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mr-md-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mr-md-20``

      /// <summary>
      /// Right margin values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes right margin (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mr-lg-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mr-lg-4``

        /// <summary>
        /// Applies small right margin (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mr-lg-8``

        /// <summary>
        /// Applies medium right margin (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mr-lg-12``

        /// <summary>
        /// Applies large right margin (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mr-lg-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mr-lg-20``

      /// <summary>
      /// Right margin values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes right margin (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mr-xl-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mr-xl-4``

        /// <summary>
        /// Applies small right margin (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mr-xl-8``

        /// <summary>
        /// Applies medium right margin (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mr-xl-12``

        /// <summary>
        /// Applies large right margin (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mr-xl-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mr-xl-20``

      /// <summary>
      /// Right margin values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes right margin (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-mr-xxl-0``

        /// <summary>
        /// Applies extra-small right margin (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-mr-xxl-4``

        /// <summary>
        /// Applies small right margin (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-mr-xxl-8``

        /// <summary>
        /// Applies medium right margin (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-mr-xxl-12``

        /// <summary>
        /// Applies large right margin (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-mr-xxl-16``

        /// <summary>
        /// Applies extra-large right margin (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-mr-xxl-20``

    /// <summary>
    /// Preset vertical margin (top and bottom) values. Use nested breakpoint modules (e.g., <c>Vertical.Small</c>) for responsive variants.
    /// </summary>
    module Vertical =

      /// <summary>
      /// Removes vertical margin (top and bottom) (0px).
      /// </summary>
      let none = cls [ Css.``weave-mt-0``; Css.``weave-mb-0`` ]

      /// <summary>
      /// Applies extra-small vertical margin (top and bottom) (4px).
      /// </summary>
      let extraSmall = cls [ Css.``weave-mt-4``; Css.``weave-mb-4`` ]

      /// <summary>
      /// Applies small vertical margin (top and bottom) (8px).
      /// </summary>
      let small = cls [ Css.``weave-mt-8``; Css.``weave-mb-8`` ]

      /// <summary>
      /// Applies medium vertical margin (top and bottom) (12px).
      /// </summary>
      let medium = cls [ Css.``weave-mt-12``; Css.``weave-mb-12`` ]

      /// <summary>
      /// Applies large vertical margin (top and bottom) (16px).
      /// </summary>
      let large = cls [ Css.``weave-mt-16``; Css.``weave-mb-16`` ]

      /// <summary>
      /// Applies extra-large vertical margin (top and bottom) (20px).
      /// </summary>
      let extraLarge = cls [ Css.``weave-mt-20``; Css.``weave-mb-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-mt-0``; Css.``weave-mb-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-mt-4``; Css.``weave-mb-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-mt-8``; Css.``weave-mb-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-mt-12``; Css.``weave-mb-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-mt-16``; Css.``weave-mb-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-mt-20``; Css.``weave-mb-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-mt-sm-0``; Css.``weave-mb-sm-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-mt-sm-4``; Css.``weave-mb-sm-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-mt-sm-8``; Css.``weave-mb-sm-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-mt-sm-12``; Css.``weave-mb-sm-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-mt-sm-16``; Css.``weave-mb-sm-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-mt-sm-20``; Css.``weave-mb-sm-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-mt-md-0``; Css.``weave-mb-md-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-mt-md-4``; Css.``weave-mb-md-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-mt-md-8``; Css.``weave-mb-md-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-mt-md-12``; Css.``weave-mb-md-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-mt-md-16``; Css.``weave-mb-md-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-mt-md-20``; Css.``weave-mb-md-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-mt-lg-0``; Css.``weave-mb-lg-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-mt-lg-4``; Css.``weave-mb-lg-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-mt-lg-8``; Css.``weave-mb-lg-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-mt-lg-12``; Css.``weave-mb-lg-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-mt-lg-16``; Css.``weave-mb-lg-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-mt-lg-20``; Css.``weave-mb-lg-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-mt-xl-0``; Css.``weave-mb-xl-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-mt-xl-4``; Css.``weave-mb-xl-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-mt-xl-8``; Css.``weave-mb-xl-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-mt-xl-12``; Css.``weave-mb-xl-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-mt-xl-16``; Css.``weave-mb-xl-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-mt-xl-20``; Css.``weave-mb-xl-20`` ]

      /// <summary>
      /// Vertical margin (top and bottom) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes vertical margin (top and bottom) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-mt-xxl-0``; Css.``weave-mb-xxl-0`` ]

        /// <summary>
        /// Applies extra-small vertical margin (top and bottom) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-mt-xxl-4``; Css.``weave-mb-xxl-4`` ]

        /// <summary>
        /// Applies small vertical margin (top and bottom) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-mt-xxl-8``; Css.``weave-mb-xxl-8`` ]

        /// <summary>
        /// Applies medium vertical margin (top and bottom) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-mt-xxl-12``; Css.``weave-mb-xxl-12`` ]

        /// <summary>
        /// Applies large vertical margin (top and bottom) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-mt-xxl-16``; Css.``weave-mb-xxl-16`` ]

        /// <summary>
        /// Applies extra-large vertical margin (top and bottom) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-mt-xxl-20``; Css.``weave-mb-xxl-20`` ]

    /// <summary>
    /// Preset horizontal margin (left and right) values. Use nested breakpoint modules (e.g., <c>Horizontal.Small</c>) for responsive variants.
    /// </summary>
    module Horizontal =

      /// <summary>
      /// Removes horizontal margin (left and right) (0px).
      /// </summary>
      let none = cls [ Css.``weave-ml-0``; Css.``weave-mr-0`` ]

      /// <summary>
      /// Applies extra-small horizontal margin (left and right) (4px).
      /// </summary>
      let extraSmall = cls [ Css.``weave-ml-4``; Css.``weave-mr-4`` ]

      /// <summary>
      /// Applies small horizontal margin (left and right) (8px).
      /// </summary>
      let small = cls [ Css.``weave-ml-8``; Css.``weave-mr-8`` ]

      /// <summary>
      /// Applies medium horizontal margin (left and right) (12px).
      /// </summary>
      let medium = cls [ Css.``weave-ml-12``; Css.``weave-mr-12`` ]

      /// <summary>
      /// Applies large horizontal margin (left and right) (16px).
      /// </summary>
      let large = cls [ Css.``weave-ml-16``; Css.``weave-mr-16`` ]

      /// <summary>
      /// Applies extra-large horizontal margin (left and right) (20px).
      /// </summary>
      let extraLarge = cls [ Css.``weave-ml-20``; Css.``weave-mr-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-ml-0``; Css.``weave-mr-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-ml-4``; Css.``weave-mr-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-ml-8``; Css.``weave-mr-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-ml-12``; Css.``weave-mr-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-ml-16``; Css.``weave-mr-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-ml-20``; Css.``weave-mr-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-ml-sm-0``; Css.``weave-mr-sm-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-ml-sm-4``; Css.``weave-mr-sm-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-ml-sm-8``; Css.``weave-mr-sm-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-ml-sm-12``; Css.``weave-mr-sm-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-ml-sm-16``; Css.``weave-mr-sm-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-ml-sm-20``; Css.``weave-mr-sm-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-ml-md-0``; Css.``weave-mr-md-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-ml-md-4``; Css.``weave-mr-md-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-ml-md-8``; Css.``weave-mr-md-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-ml-md-12``; Css.``weave-mr-md-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-ml-md-16``; Css.``weave-mr-md-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-ml-md-20``; Css.``weave-mr-md-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-ml-lg-0``; Css.``weave-mr-lg-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-ml-lg-4``; Css.``weave-mr-lg-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-ml-lg-8``; Css.``weave-mr-lg-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-ml-lg-12``; Css.``weave-mr-lg-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-ml-lg-16``; Css.``weave-mr-lg-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-ml-lg-20``; Css.``weave-mr-lg-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-ml-xl-0``; Css.``weave-mr-xl-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-ml-xl-4``; Css.``weave-mr-xl-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-ml-xl-8``; Css.``weave-mr-xl-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-ml-xl-12``; Css.``weave-mr-xl-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-ml-xl-16``; Css.``weave-mr-xl-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-ml-xl-20``; Css.``weave-mr-xl-20`` ]

      /// <summary>
      /// Horizontal margin (left and right) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes horizontal margin (left and right) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-ml-xxl-0``; Css.``weave-mr-xxl-0`` ]

        /// <summary>
        /// Applies extra-small horizontal margin (left and right) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-ml-xxl-4``; Css.``weave-mr-xxl-4`` ]

        /// <summary>
        /// Applies small horizontal margin (left and right) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-ml-xxl-8``; Css.``weave-mr-xxl-8`` ]

        /// <summary>
        /// Applies medium horizontal margin (left and right) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-ml-xxl-12``; Css.``weave-mr-xxl-12`` ]

        /// <summary>
        /// Applies large horizontal margin (left and right) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-ml-xxl-16``; Css.``weave-mr-xxl-16`` ]

        /// <summary>
        /// Applies extra-large horizontal margin (left and right) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-ml-xxl-20``; Css.``weave-mr-xxl-20`` ]

    /// <summary>
    /// Preset margin on all sides values. Use nested breakpoint modules (e.g., <c>All.Small</c>) for responsive variants.
    /// </summary>
    module All =

      /// <summary>
      /// Removes margin on all sides (0px).
      /// </summary>
      let none = cl Css.``weave-ma-0``

      /// <summary>
      /// Applies extra-small margin on all sides (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-ma-4``

      /// <summary>
      /// Applies small margin on all sides (8px).
      /// </summary>
      let small = cl Css.``weave-ma-8``

      /// <summary>
      /// Applies medium margin on all sides (12px).
      /// </summary>
      let medium = cl Css.``weave-ma-12``

      /// <summary>
      /// Applies large margin on all sides (16px).
      /// </summary>
      let large = cl Css.``weave-ma-16``

      /// <summary>
      /// Applies extra-large margin on all sides (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-ma-20``

      /// <summary>
      /// Margin on all sides values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ma-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ma-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ma-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ma-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ma-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ma-20``

      /// <summary>
      /// Margin on all sides values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes margin on all sides (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ma-sm-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ma-sm-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ma-sm-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ma-sm-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ma-sm-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ma-sm-20``

      /// <summary>
      /// Margin on all sides values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes margin on all sides (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ma-md-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ma-md-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ma-md-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ma-md-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ma-md-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ma-md-20``

      /// <summary>
      /// Margin on all sides values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes margin on all sides (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ma-lg-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ma-lg-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ma-lg-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ma-lg-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ma-lg-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ma-lg-20``

      /// <summary>
      /// Margin on all sides values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ma-xl-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ma-xl-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ma-xl-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ma-xl-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ma-xl-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ma-xl-20``

      /// <summary>
      /// Margin on all sides values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes margin on all sides (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-ma-xxl-0``

        /// <summary>
        /// Applies extra-small margin on all sides (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-ma-xxl-4``

        /// <summary>
        /// Applies small margin on all sides (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-ma-xxl-8``

        /// <summary>
        /// Applies medium margin on all sides (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-ma-xxl-12``

        /// <summary>
        /// Applies large margin on all sides (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-ma-xxl-16``

        /// <summary>
        /// Applies extra-large margin on all sides (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-ma-xxl-20``

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
      let none = cl Css.``weave-pt-0``

      /// <summary>
      /// Applies extra-small top padding (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-pt-4``

      /// <summary>
      /// Applies small top padding (8px).
      /// </summary>
      let small = cl Css.``weave-pt-8``

      /// <summary>
      /// Applies medium top padding (12px).
      /// </summary>
      let medium = cl Css.``weave-pt-12``

      /// <summary>
      /// Applies large top padding (16px).
      /// </summary>
      let large = cl Css.``weave-pt-16``

      /// <summary>
      /// Applies extra-large top padding (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-pt-20``

      /// <summary>
      /// Top padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes top padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pt-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pt-4``

        /// <summary>
        /// Applies small top padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pt-8``

        /// <summary>
        /// Applies medium top padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pt-12``

        /// <summary>
        /// Applies large top padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pt-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pt-20``

      /// <summary>
      /// Top padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes top padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pt-sm-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pt-sm-4``

        /// <summary>
        /// Applies small top padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pt-sm-8``

        /// <summary>
        /// Applies medium top padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pt-sm-12``

        /// <summary>
        /// Applies large top padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pt-sm-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pt-sm-20``

      /// <summary>
      /// Top padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes top padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pt-md-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pt-md-4``

        /// <summary>
        /// Applies small top padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pt-md-8``

        /// <summary>
        /// Applies medium top padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pt-md-12``

        /// <summary>
        /// Applies large top padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pt-md-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pt-md-20``

      /// <summary>
      /// Top padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes top padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pt-lg-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pt-lg-4``

        /// <summary>
        /// Applies small top padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pt-lg-8``

        /// <summary>
        /// Applies medium top padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pt-lg-12``

        /// <summary>
        /// Applies large top padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pt-lg-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pt-lg-20``

      /// <summary>
      /// Top padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes top padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pt-xl-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pt-xl-4``

        /// <summary>
        /// Applies small top padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pt-xl-8``

        /// <summary>
        /// Applies medium top padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pt-xl-12``

        /// <summary>
        /// Applies large top padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pt-xl-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pt-xl-20``

      /// <summary>
      /// Top padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes top padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pt-xxl-0``

        /// <summary>
        /// Applies extra-small top padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pt-xxl-4``

        /// <summary>
        /// Applies small top padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pt-xxl-8``

        /// <summary>
        /// Applies medium top padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pt-xxl-12``

        /// <summary>
        /// Applies large top padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pt-xxl-16``

        /// <summary>
        /// Applies extra-large top padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pt-xxl-20``

    /// <summary>
    /// Preset bottom padding values. Use nested breakpoint modules (e.g., <c>Bottom.Small</c>) for responsive variants.
    /// </summary>
    module Bottom =

      /// <summary>
      /// Removes bottom padding (0px).
      /// </summary>
      let none = cl Css.``weave-pb-0``

      /// <summary>
      /// Applies extra-small bottom padding (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-pb-4``

      /// <summary>
      /// Applies small bottom padding (8px).
      /// </summary>
      let small = cl Css.``weave-pb-8``

      /// <summary>
      /// Applies medium bottom padding (12px).
      /// </summary>
      let medium = cl Css.``weave-pb-12``

      /// <summary>
      /// Applies large bottom padding (16px).
      /// </summary>
      let large = cl Css.``weave-pb-16``

      /// <summary>
      /// Applies extra-large bottom padding (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-pb-20``

      /// <summary>
      /// Bottom padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pb-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pb-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pb-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pb-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pb-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pb-20``

      /// <summary>
      /// Bottom padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes bottom padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pb-sm-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pb-sm-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pb-sm-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pb-sm-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pb-sm-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pb-sm-20``

      /// <summary>
      /// Bottom padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes bottom padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pb-md-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pb-md-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pb-md-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pb-md-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pb-md-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pb-md-20``

      /// <summary>
      /// Bottom padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes bottom padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pb-lg-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pb-lg-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pb-lg-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pb-lg-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pb-lg-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pb-lg-20``

      /// <summary>
      /// Bottom padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pb-xl-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pb-xl-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pb-xl-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pb-xl-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pb-xl-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pb-xl-20``

      /// <summary>
      /// Bottom padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes bottom padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pb-xxl-0``

        /// <summary>
        /// Applies extra-small bottom padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pb-xxl-4``

        /// <summary>
        /// Applies small bottom padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pb-xxl-8``

        /// <summary>
        /// Applies medium bottom padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pb-xxl-12``

        /// <summary>
        /// Applies large bottom padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pb-xxl-16``

        /// <summary>
        /// Applies extra-large bottom padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pb-xxl-20``

    /// <summary>
    /// Preset left padding values. Use nested breakpoint modules (e.g., <c>Left.Small</c>) for responsive variants.
    /// </summary>
    module Left =

      /// <summary>
      /// Removes left padding (0px).
      /// </summary>
      let none = cl Css.``weave-pl-0``

      /// <summary>
      /// Applies extra-small left padding (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-pl-4``

      /// <summary>
      /// Applies small left padding (8px).
      /// </summary>
      let small = cl Css.``weave-pl-8``

      /// <summary>
      /// Applies medium left padding (12px).
      /// </summary>
      let medium = cl Css.``weave-pl-12``

      /// <summary>
      /// Applies large left padding (16px).
      /// </summary>
      let large = cl Css.``weave-pl-16``

      /// <summary>
      /// Applies extra-large left padding (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-pl-20``

      /// <summary>
      /// Left padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes left padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pl-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pl-4``

        /// <summary>
        /// Applies small left padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pl-8``

        /// <summary>
        /// Applies medium left padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pl-12``

        /// <summary>
        /// Applies large left padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pl-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pl-20``

      /// <summary>
      /// Left padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes left padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pl-sm-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pl-sm-4``

        /// <summary>
        /// Applies small left padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pl-sm-8``

        /// <summary>
        /// Applies medium left padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pl-sm-12``

        /// <summary>
        /// Applies large left padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pl-sm-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pl-sm-20``

      /// <summary>
      /// Left padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes left padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pl-md-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pl-md-4``

        /// <summary>
        /// Applies small left padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pl-md-8``

        /// <summary>
        /// Applies medium left padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pl-md-12``

        /// <summary>
        /// Applies large left padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pl-md-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pl-md-20``

      /// <summary>
      /// Left padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes left padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pl-lg-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pl-lg-4``

        /// <summary>
        /// Applies small left padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pl-lg-8``

        /// <summary>
        /// Applies medium left padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pl-lg-12``

        /// <summary>
        /// Applies large left padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pl-lg-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pl-lg-20``

      /// <summary>
      /// Left padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes left padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pl-xl-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pl-xl-4``

        /// <summary>
        /// Applies small left padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pl-xl-8``

        /// <summary>
        /// Applies medium left padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pl-xl-12``

        /// <summary>
        /// Applies large left padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pl-xl-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pl-xl-20``

      /// <summary>
      /// Left padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes left padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pl-xxl-0``

        /// <summary>
        /// Applies extra-small left padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pl-xxl-4``

        /// <summary>
        /// Applies small left padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pl-xxl-8``

        /// <summary>
        /// Applies medium left padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pl-xxl-12``

        /// <summary>
        /// Applies large left padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pl-xxl-16``

        /// <summary>
        /// Applies extra-large left padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pl-xxl-20``

    /// <summary>
    /// Preset right padding values. Use nested breakpoint modules (e.g., <c>Right.Small</c>) for responsive variants.
    /// </summary>
    module Right =

      /// <summary>
      /// Removes right padding (0px).
      /// </summary>
      let none = cl Css.``weave-pr-0``

      /// <summary>
      /// Applies extra-small right padding (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-pr-4``

      /// <summary>
      /// Applies small right padding (8px).
      /// </summary>
      let small = cl Css.``weave-pr-8``

      /// <summary>
      /// Applies medium right padding (12px).
      /// </summary>
      let medium = cl Css.``weave-pr-12``

      /// <summary>
      /// Applies large right padding (16px).
      /// </summary>
      let large = cl Css.``weave-pr-16``

      /// <summary>
      /// Applies extra-large right padding (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-pr-20``

      /// <summary>
      /// Right padding values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes right padding (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pr-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pr-4``

        /// <summary>
        /// Applies small right padding (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pr-8``

        /// <summary>
        /// Applies medium right padding (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pr-12``

        /// <summary>
        /// Applies large right padding (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pr-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pr-20``

      /// <summary>
      /// Right padding values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes right padding (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pr-sm-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pr-sm-4``

        /// <summary>
        /// Applies small right padding (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pr-sm-8``

        /// <summary>
        /// Applies medium right padding (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pr-sm-12``

        /// <summary>
        /// Applies large right padding (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pr-sm-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pr-sm-20``

      /// <summary>
      /// Right padding values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes right padding (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pr-md-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pr-md-4``

        /// <summary>
        /// Applies small right padding (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pr-md-8``

        /// <summary>
        /// Applies medium right padding (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pr-md-12``

        /// <summary>
        /// Applies large right padding (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pr-md-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pr-md-20``

      /// <summary>
      /// Right padding values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes right padding (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pr-lg-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pr-lg-4``

        /// <summary>
        /// Applies small right padding (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pr-lg-8``

        /// <summary>
        /// Applies medium right padding (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pr-lg-12``

        /// <summary>
        /// Applies large right padding (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pr-lg-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pr-lg-20``

      /// <summary>
      /// Right padding values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes right padding (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pr-xl-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pr-xl-4``

        /// <summary>
        /// Applies small right padding (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pr-xl-8``

        /// <summary>
        /// Applies medium right padding (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pr-xl-12``

        /// <summary>
        /// Applies large right padding (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pr-xl-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pr-xl-20``

      /// <summary>
      /// Right padding values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes right padding (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pr-xxl-0``

        /// <summary>
        /// Applies extra-small right padding (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pr-xxl-4``

        /// <summary>
        /// Applies small right padding (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pr-xxl-8``

        /// <summary>
        /// Applies medium right padding (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pr-xxl-12``

        /// <summary>
        /// Applies large right padding (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pr-xxl-16``

        /// <summary>
        /// Applies extra-large right padding (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pr-xxl-20``

    /// <summary>
    /// Preset vertical padding (top and bottom) values. Use nested breakpoint modules (e.g., <c>Vertical.Small</c>) for responsive variants.
    /// </summary>
    module Vertical =

      /// <summary>
      /// Removes vertical padding (top and bottom) (0px).
      /// </summary>
      let none = cls [ Css.``weave-pt-0``; Css.``weave-pb-0`` ]

      /// <summary>
      /// Applies extra-small vertical padding (top and bottom) (4px).
      /// </summary>
      let extraSmall = cls [ Css.``weave-pt-4``; Css.``weave-pb-4`` ]

      /// <summary>
      /// Applies small vertical padding (top and bottom) (8px).
      /// </summary>
      let small = cls [ Css.``weave-pt-8``; Css.``weave-pb-8`` ]

      /// <summary>
      /// Applies medium vertical padding (top and bottom) (12px).
      /// </summary>
      let medium = cls [ Css.``weave-pt-12``; Css.``weave-pb-12`` ]

      /// <summary>
      /// Applies large vertical padding (top and bottom) (16px).
      /// </summary>
      let large = cls [ Css.``weave-pt-16``; Css.``weave-pb-16`` ]

      /// <summary>
      /// Applies extra-large vertical padding (top and bottom) (20px).
      /// </summary>
      let extraLarge = cls [ Css.``weave-pt-20``; Css.``weave-pb-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pt-0``; Css.``weave-pb-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pt-4``; Css.``weave-pb-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pt-8``; Css.``weave-pb-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pt-12``; Css.``weave-pb-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pt-16``; Css.``weave-pb-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pt-20``; Css.``weave-pb-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pt-sm-0``; Css.``weave-pb-sm-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pt-sm-4``; Css.``weave-pb-sm-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pt-sm-8``; Css.``weave-pb-sm-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pt-sm-12``; Css.``weave-pb-sm-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pt-sm-16``; Css.``weave-pb-sm-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pt-sm-20``; Css.``weave-pb-sm-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pt-md-0``; Css.``weave-pb-md-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pt-md-4``; Css.``weave-pb-md-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pt-md-8``; Css.``weave-pb-md-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pt-md-12``; Css.``weave-pb-md-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pt-md-16``; Css.``weave-pb-md-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pt-md-20``; Css.``weave-pb-md-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pt-lg-0``; Css.``weave-pb-lg-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pt-lg-4``; Css.``weave-pb-lg-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pt-lg-8``; Css.``weave-pb-lg-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pt-lg-12``; Css.``weave-pb-lg-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pt-lg-16``; Css.``weave-pb-lg-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pt-lg-20``; Css.``weave-pb-lg-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pt-xl-0``; Css.``weave-pb-xl-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pt-xl-4``; Css.``weave-pb-xl-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pt-xl-8``; Css.``weave-pb-xl-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pt-xl-12``; Css.``weave-pb-xl-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pt-xl-16``; Css.``weave-pb-xl-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pt-xl-20``; Css.``weave-pb-xl-20`` ]

      /// <summary>
      /// Vertical padding (top and bottom) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes vertical padding (top and bottom) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pt-xxl-0``; Css.``weave-pb-xxl-0`` ]

        /// <summary>
        /// Applies extra-small vertical padding (top and bottom) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pt-xxl-4``; Css.``weave-pb-xxl-4`` ]

        /// <summary>
        /// Applies small vertical padding (top and bottom) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pt-xxl-8``; Css.``weave-pb-xxl-8`` ]

        /// <summary>
        /// Applies medium vertical padding (top and bottom) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pt-xxl-12``; Css.``weave-pb-xxl-12`` ]

        /// <summary>
        /// Applies large vertical padding (top and bottom) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pt-xxl-16``; Css.``weave-pb-xxl-16`` ]

        /// <summary>
        /// Applies extra-large vertical padding (top and bottom) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pt-xxl-20``; Css.``weave-pb-xxl-20`` ]

    /// <summary>
    /// Preset horizontal padding (left and right) values. Use nested breakpoint modules (e.g., <c>Horizontal.Small</c>) for responsive variants.
    /// </summary>
    module Horizontal =

      /// <summary>
      /// Removes horizontal padding (left and right) (0px).
      /// </summary>
      let none = cls [ Css.``weave-pl-0``; Css.``weave-pr-0`` ]

      /// <summary>
      /// Applies extra-small horizontal padding (left and right) (4px).
      /// </summary>
      let extraSmall = cls [ Css.``weave-pl-4``; Css.``weave-pr-4`` ]

      /// <summary>
      /// Applies small horizontal padding (left and right) (8px).
      /// </summary>
      let small = cls [ Css.``weave-pl-8``; Css.``weave-pr-8`` ]

      /// <summary>
      /// Applies medium horizontal padding (left and right) (12px).
      /// </summary>
      let medium = cls [ Css.``weave-pl-12``; Css.``weave-pr-12`` ]

      /// <summary>
      /// Applies large horizontal padding (left and right) (16px).
      /// </summary>
      let large = cls [ Css.``weave-pl-16``; Css.``weave-pr-16`` ]

      /// <summary>
      /// Applies extra-large horizontal padding (left and right) (20px).
      /// </summary>
      let extraLarge = cls [ Css.``weave-pl-20``; Css.``weave-pr-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pl-0``; Css.``weave-pr-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pl-4``; Css.``weave-pr-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pl-8``; Css.``weave-pr-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pl-12``; Css.``weave-pr-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pl-16``; Css.``weave-pr-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pl-20``; Css.``weave-pr-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the small breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pl-sm-0``; Css.``weave-pr-sm-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pl-sm-4``; Css.``weave-pr-sm-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the small breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pl-sm-8``; Css.``weave-pr-sm-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pl-sm-12``; Css.``weave-pr-sm-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the small breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pl-sm-16``; Css.``weave-pr-sm-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pl-sm-20``; Css.``weave-pr-sm-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pl-md-0``; Css.``weave-pr-md-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pl-md-4``; Css.``weave-pr-md-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pl-md-8``; Css.``weave-pr-md-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pl-md-12``; Css.``weave-pr-md-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pl-md-16``; Css.``weave-pr-md-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pl-md-20``; Css.``weave-pr-md-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pl-lg-0``; Css.``weave-pr-lg-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pl-lg-4``; Css.``weave-pr-lg-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pl-lg-8``; Css.``weave-pr-lg-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pl-lg-12``; Css.``weave-pr-lg-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pl-lg-16``; Css.``weave-pr-lg-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pl-lg-20``; Css.``weave-pr-lg-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pl-xl-0``; Css.``weave-pr-xl-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pl-xl-4``; Css.``weave-pr-xl-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pl-xl-8``; Css.``weave-pr-xl-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pl-xl-12``; Css.``weave-pr-xl-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pl-xl-16``; Css.``weave-pr-xl-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pl-xl-20``; Css.``weave-pr-xl-20`` ]

      /// <summary>
      /// Horizontal padding (left and right) values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes horizontal padding (left and right) (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cls [ Css.``weave-pl-xxl-0``; Css.``weave-pr-xxl-0`` ]

        /// <summary>
        /// Applies extra-small horizontal padding (left and right) (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cls [ Css.``weave-pl-xxl-4``; Css.``weave-pr-xxl-4`` ]

        /// <summary>
        /// Applies small horizontal padding (left and right) (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cls [ Css.``weave-pl-xxl-8``; Css.``weave-pr-xxl-8`` ]

        /// <summary>
        /// Applies medium horizontal padding (left and right) (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cls [ Css.``weave-pl-xxl-12``; Css.``weave-pr-xxl-12`` ]

        /// <summary>
        /// Applies large horizontal padding (left and right) (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cls [ Css.``weave-pl-xxl-16``; Css.``weave-pr-xxl-16`` ]

        /// <summary>
        /// Applies extra-large horizontal padding (left and right) (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cls [ Css.``weave-pl-xxl-20``; Css.``weave-pr-xxl-20`` ]

    /// <summary>
    /// Preset padding on all sides values. Use nested breakpoint modules (e.g., <c>All.Small</c>) for responsive variants.
    /// </summary>
    module All =

      /// <summary>
      /// Removes padding on all sides (0px).
      /// </summary>
      let none = cl Css.``weave-pa-0``

      /// <summary>
      /// Applies extra-small padding on all sides (4px).
      /// </summary>
      let extraSmall = cl Css.``weave-pa-4``

      /// <summary>
      /// Applies small padding on all sides (8px).
      /// </summary>
      let small = cl Css.``weave-pa-8``

      /// <summary>
      /// Applies medium padding on all sides (12px).
      /// </summary>
      let medium = cl Css.``weave-pa-12``

      /// <summary>
      /// Applies large padding on all sides (16px).
      /// </summary>
      let large = cl Css.``weave-pa-16``

      /// <summary>
      /// Applies extra-large padding on all sides (20px).
      /// </summary>
      let extraLarge = cl Css.``weave-pa-20``

      /// <summary>
      /// Padding on all sides values applied at the extra-small breakpoint and above.
      /// </summary>
      module ExtraSmall =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pa-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pa-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pa-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pa-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pa-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pa-20``

      /// <summary>
      /// Padding on all sides values applied at the small breakpoint and above.
      /// </summary>
      module Small =

        /// <summary>
        /// Removes padding on all sides (0px) at the small breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pa-sm-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the small breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pa-sm-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the small breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pa-sm-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the small breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pa-sm-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the small breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pa-sm-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the small breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pa-sm-20``

      /// <summary>
      /// Padding on all sides values applied at the medium breakpoint and above.
      /// </summary>
      module Medium =

        /// <summary>
        /// Removes padding on all sides (0px) at the medium breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pa-md-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the medium breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pa-md-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the medium breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pa-md-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the medium breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pa-md-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the medium breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pa-md-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the medium breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pa-md-20``

      /// <summary>
      /// Padding on all sides values applied at the large breakpoint and above.
      /// </summary>
      module Large =

        /// <summary>
        /// Removes padding on all sides (0px) at the large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pa-lg-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pa-lg-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pa-lg-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pa-lg-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pa-lg-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pa-lg-20``

      /// <summary>
      /// Padding on all sides values applied at the extra-large breakpoint and above.
      /// </summary>
      module ExtraLarge =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pa-xl-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pa-xl-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pa-xl-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pa-xl-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pa-xl-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pa-xl-20``

      /// <summary>
      /// Padding on all sides values applied at the extra-extra-large breakpoint and above.
      /// </summary>
      module ExtraExtraLarge =

        /// <summary>
        /// Removes padding on all sides (0px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let none = cl Css.``weave-pa-xxl-0``

        /// <summary>
        /// Applies extra-small padding on all sides (4px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraSmall = cl Css.``weave-pa-xxl-4``

        /// <summary>
        /// Applies small padding on all sides (8px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let small = cl Css.``weave-pa-xxl-8``

        /// <summary>
        /// Applies medium padding on all sides (12px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let medium = cl Css.``weave-pa-xxl-12``

        /// <summary>
        /// Applies large padding on all sides (16px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let large = cl Css.``weave-pa-xxl-16``

        /// <summary>
        /// Applies extra-large padding on all sides (20px) at the extra-extra-large breakpoint and above.
        /// </summary>
        let extraLarge = cl Css.``weave-pa-xxl-20``
