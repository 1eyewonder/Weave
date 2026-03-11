namespace Weave

/// Module exists to AutoOpen all relevant Weave modules for ease of use when opening the main Weave namespace.
[<AutoOpen>]
module AutoOpens =

  [<assembly: AutoOpen("Weave")>]
  [<assembly: AutoOpen("Weave.Generic")>]
  [<assembly: AutoOpen("Weave.Operators")>]
  [<assembly: AutoOpen("Weave.CssHelpers.Core")>]
  [<assembly: AutoOpen("Weave.CssHelpers.Spacing")>]
  [<assembly: AutoOpen("Weave.CssHelpers.Layout")>]
  [<assembly: AutoOpen("Weave.CssHelpers.Decorations")>]
  do ()
