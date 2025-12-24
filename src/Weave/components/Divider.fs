namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Divider =

  [<RequireQualifiedAccess; Struct>]
  type Orientation =
    | Horizontal
    | Vertical

  module Orientation =

    let toClasses orientation =
      match orientation with
      | Orientation.Horizontal -> []
      | Orientation.Vertical -> [ Css.``divider--vertical`` ]

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | FullWidth
    | Inset
    | Middle

  module Variant =

    let toClass variant =
      match variant with
      | Variant.FullWidth -> Css.``divider--fullwidth``
      | Variant.Inset -> Css.``divider--inset``
      | Variant.Middle -> Css.``divider--middle``

open Divider

[<JavaScript>]
type Divider =

  static member Create(?orientation: Orientation, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    let orientation = defaultArg orientation Orientation.Horizontal

    div [ cl Css.``divider``; Orientation.toClasses orientation |> cls; yield! attrs ] []
