namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module AppBar =

  /// <summary>
  /// Controls how the AppBar is positioned in the document.
  /// </summary>
  [<RequireQualifiedAccess; Struct>]
  type Position =
    /// <summary>
    /// Fixes the bar to the top of the viewport; scrolls with nothing.
    /// </summary>
    | Fixed
    /// <summary>
    /// Sticks to the top once the page scrolls to it; stays in flow until then.
    /// </summary>
    | Sticky
    /// <summary>
    /// Stays in the normal document flow (default); no scroll behavior.
    /// </summary>
    | Static
    /// <summary>
    /// Fixes the bar to the bottom of the viewport.
    /// </summary>
    | Bottom

  module Position =

    let toClass position =
      match position with
      | Position.Fixed -> Some Css.``weave-appbar--fixed-top``
      | Position.Bottom -> Some Css.``weave-appbar--fixed-bottom``
      | Position.Sticky -> Some Css.``weave-appbar--sticky``
      | Position.Static -> None

open AppBar

[<JavaScript>]
type AppBar =

  /// <summary>
  /// Creates an AppBar — a full-width horizontal bar typically used for
  /// top-level branding, navigation actions, and global controls.
  /// </summary>
  /// <param name="content">The content to render inside the bar.</param>
  /// <param name="position">
  /// How the bar is positioned in the document.
  /// Defaults to <c>Position.Fixed</c> (fixed to the top of the viewport).
  /// </param>
  /// <param name="attrs">Additional attributes applied to the root element.</param>
  static member Create(content: Doc, ?position: Position, ?attrs: Attr list) =

    let position = defaultArg position Position.Fixed
    let attrs = defaultArg attrs List.empty

    header [
      cl Css.``weave-appbar``
      Position.toClass position |> Attr.bindOption cl
      yield! attrs
    ] [ content ]
