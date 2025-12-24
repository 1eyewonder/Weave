namespace Weave

open WebSharper
open WebSharper.UI
open Weave.CssHelpers

[<JavaScript>]
module Container =

  [<RequireQualifiedAccess; Struct>]
  type MaxWidth =
    | ExtraSmall
    | Small
    | Medium
    | Large
    | ExtraLarge
    | ExtraExtraLarge

  module MaxWidth =

    let toClass maxWidth =
      match maxWidth with
      | MaxWidth.ExtraSmall -> Css.``container--maxwidth-xs``
      | MaxWidth.Small -> Css.``container--maxwidth-sm``
      | MaxWidth.Medium -> Css.``container--maxwidth-md``
      | MaxWidth.Large -> Css.``container--maxwidth-lg``
      | MaxWidth.ExtraLarge -> Css.``container--maxwidth-xl``
      | MaxWidth.ExtraExtraLarge -> Css.``container--maxwidth-xxl``

open Container

[<JavaScript>]
type Container =

  static member Create
    (content: Doc, ?fixedWidth: bool, ?maxWidth: MaxWidth, ?gutters: bool, ?attrs: Attr list)
    =
    let fixedWidth = defaultArg fixedWidth false
    let maxWidth = defaultArg maxWidth MaxWidth.ExtraExtraLarge
    let gutters = defaultArg gutters true
    let attrs = defaultArg attrs List.empty

    let containerClasses = [
      Css.container
      Css.``container--fill-height``

      if fixedWidth then
        Css.``container--fixed``
      else
        MaxWidth.toClass maxWidth

      if gutters then
        Css.``container--gutters``
    ]

    div [ yield! containerClasses |> List.map cl; yield! attrs ] [ content ]
