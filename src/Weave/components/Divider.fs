namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module Divider =

  module Orientation =

    /// <summary>Renders the divider as a vertical bar.</summary>
    let vertical = cl Css.``weave-divider--vertical``

  module Variant =

    let fullWidth = cl Css.``weave-divider--fullwidth``
    let inset = cl Css.``weave-divider--inset``
    let middle = cl Css.``weave-divider--middle``

[<JavaScript; RequireQualifiedAccess>]
type Divider =

  static member create(?attrs: Attr list) =
    let attrs = defaultArg attrs []
    div [ cl Css.``weave-divider``; yield! attrs ] []
