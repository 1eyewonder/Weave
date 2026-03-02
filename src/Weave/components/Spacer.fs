namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
type Spacer =

  /// <summary>
  /// Renders an invisible element that expands to fill all remaining space
  /// in a flex row or column (<c>flex: 1 1 auto</c>), pushing sibling
  /// elements outward to the edges.
  /// </summary>
  /// <param name="attrs">Additional attributes applied to the root element.</param>
  static member Create(?attrs: Attr list) =
    let attrs = defaultArg attrs List.empty
    div [ cl Css.``weave-spacer``; yield! attrs ] []
