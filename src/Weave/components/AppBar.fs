namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module AppBar =

  module Position =

    /// <summary>Fixes the bar to the top of the viewport; scrolls with nothing.</summary>
    let fixedTop = cl Css.``weave-appbar--fixed-top``
    /// <summary>Fixes the bar to the bottom of the viewport.</summary>
    let fixedBottom = cl Css.``weave-appbar--fixed-bottom``
    /// <summary>Sticks to the top once the page scrolls to it; stays in flow until then.</summary>
    let sticky = cl Css.``weave-appbar--sticky``

[<JavaScript; RequireQualifiedAccess>]
type AppBar =

  /// <summary>
  /// Creates an AppBar — a full-width horizontal bar typically used for
  /// top-level branding, navigation actions, and global controls.
  /// </summary>
  /// <param name="content">The content to render inside the bar.</param>
  /// <param name="attrs">
  /// Additional attributes applied to the root element.
  /// Use <c>AppBar.Position.*</c> to control positioning.
  /// </param>
  static member create(content: Doc, ?attrs: Attr list) =

    let attrs = defaultArg attrs List.empty

    header [ cl Css.``weave-appbar``; yield! attrs ] [ content ]
