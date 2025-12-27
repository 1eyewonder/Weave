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
      | Orientation.Vertical -> [ Css.``weave-divider--vertical`` ]

  [<RequireQualifiedAccess; Struct>]
  type Variant =
    | FullWidth
    | Inset
    | Middle

  module Variant =

    let toClass variant =
      match variant with
      | Variant.FullWidth -> Css.``weave-divider--fullwidth``
      | Variant.Inset -> Css.``weave-divider--inset``
      | Variant.Middle -> Css.``weave-divider--middle``

open Divider

[<JavaScript>]
type Divider =

  static member Create(?orientation: Orientation, ?attrs: Attr list) =
    let attrs = defaultArg attrs []
    let orientation = defaultArg orientation Orientation.Horizontal

    div [
      cl Css.``weave-divider``
      Orientation.toClasses orientation |> cls
      yield! attrs
    ] []
