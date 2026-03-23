namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module Typography =

  let h1 = cls [ Css.``weave-typography``; Css.``weave-typography--h1`` ]
  let h2 = cls [ Css.``weave-typography``; Css.``weave-typography--h2`` ]
  let h3 = cls [ Css.``weave-typography``; Css.``weave-typography--h3`` ]
  let h4 = cls [ Css.``weave-typography``; Css.``weave-typography--h4`` ]
  let h5 = cls [ Css.``weave-typography``; Css.``weave-typography--h5`` ]
  let h6 = cls [ Css.``weave-typography``; Css.``weave-typography--h6`` ]

  let subtitle1 =
    cls [ Css.``weave-typography``; Css.``weave-typography--subtitle1`` ]

  let subtitle2 =
    cls [ Css.``weave-typography``; Css.``weave-typography--subtitle2`` ]

  let body1 = cls [ Css.``weave-typography``; Css.``weave-typography--body1`` ]
  let body2 = cls [ Css.``weave-typography``; Css.``weave-typography--body2`` ]
  let button = cls [ Css.``weave-typography``; Css.``weave-typography--button`` ]
  let caption = cls [ Css.``weave-typography``; Css.``weave-typography--caption`` ]
  let overline = cls [ Css.``weave-typography``; Css.``weave-typography--overline`` ]

  let srOnly = cl Css.``weave-typography--sr-only``
  let paragraph = cl Css.``weave-typography--paragraph``
  let noWrap = cl Css.``weave-typography--nowrap``

  module Family =

    let display = cl Css.``weave-typography--family-display``
    let body = cl Css.``weave-typography--family-body``
    let mono = cl Css.``weave-typography--family-mono``

  module Color =

    let primary = cl Css.``weave-typography--primary``
    let secondary = cl Css.``weave-typography--secondary``
    let tertiary = cl Css.``weave-typography--tertiary``
    let error = cl Css.``weave-typography--error``
    let warning = cl Css.``weave-typography--warning``
    let success = cl Css.``weave-typography--success``
    let info = cl Css.``weave-typography--info``
    let textPrimary = cl Css.``weave-typography--text-primary``
    let textSecondary = cl Css.``weave-typography--text-secondary``
    let textDisabled = cl Css.``weave-typography--text-disabled``

  module Weight =

    let light = cl Css.``weave-typography--weight-light``
    let regular = cl Css.``weave-typography--weight-regular``
    let medium = cl Css.``weave-typography--weight-medium``
    let semiBold = cl Css.``weave-typography--weight-semibold``
    let bold = cl Css.``weave-typography--weight-bold``

  module Align =

    let ``inherit`` = cl Css.``weave-typography--align-inherit``
    let justify = cl Css.``weave-typography--align-justify``
    let center = cl Css.``weave-typography--align-center``
    let left = cl Css.``weave-typography--align-left``
    let right = cl Css.``weave-typography--align-right``
    let start = cl Css.``weave-typography--align-start``
    let ``end`` = cl Css.``weave-typography--align-end``
