namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Icons.MaterialSymbols

[<JavaScript; RequireQualifiedAccess>]
module Pagination =

  module Controls =
    /// Show first and last page buttons, which are hidden by default.
    let showFirstLast = cl Css.``weave-pagination--show-first-last``
    /// Hide previous and next page buttons, including from keyboard navigation.
    let hidePreviousNext = cl Css.``weave-pagination--hide-previous-next``

  module Variant =
    let text = cl Css.``weave-pagination--text``
    let filled = cl Css.``weave-pagination--filled``
    let outlined = cl Css.``weave-pagination--outlined``

  module Color =
    let primary = cl Css.``weave-pagination--primary``
    let secondary = cl Css.``weave-pagination--secondary``
    let tertiary = cl Css.``weave-pagination--tertiary``
    let error = cl Css.``weave-pagination--error``
    let warning = cl Css.``weave-pagination--warning``
    let success = cl Css.``weave-pagination--success``
    let info = cl Css.``weave-pagination--info``

[<JavaScript>]
module private PaginationInternal =

  // Only enumerate visible ranges, even when the page count is very large.
  // Int64 arithmetic keeps window calculations safe at Int32.MaxValue.
  // Negative keys distinguish the two non-interactive ellipses.
  let pages count selected boundaryCount siblingCount =
    let n = int64 (max 0 count)
    let b = min n (int64 (max 0 boundaryCount))
    let s = min n (int64 (max 0 siblingCount))
    let current = int64 selected
    let first = max (b + 2L) (min (current - s) (n - b - 2L * s - 1L))
    let last = min (n - b - 1L) (max (current + s) (b + 2L * s + 2L))

    [
      for page in 1L .. b do
        int page
      if first > b + 2L then
        -1
      elif b + 1L < n - b then
        int (b + 1L)
      for page in first..last do
        int page
      if last < n - b - 1L then
        -2
      elif n - b > b then
        int (n - b)
      for page in max (n - b + 1L) (b + 1L) .. n do
        int page
    ]

[<JavaScript; RequireQualifiedAccess>]
type Pagination =

  /// Creates page navigation with one-based selection. Count is normalized to zero
  /// or greater; out-of-range selection is clamped for display without writing to
  /// the caller's Var. With no pages, no page is current and all controls disable.
  /// Native buttons support Tab, Enter, and Space. Use Pagination.Controls attrs
  /// for control visibility and an aria-label attr to override "Pagination".
  static member create
    (
      count: View<int>,
      ?selectedPage: Var<int>,
      ?boundaryCount: int,
      ?siblingCount: int,
      ?enabled: View<bool>,
      ?attrs: Attr list
    ) =
    let selectedPage = defaultArg selectedPage (Var.Create 1)
    let boundaryCount = max 0 (defaultArg boundaryCount 1)
    let siblingCount = max 0 (defaultArg siblingCount 1)
    let enabled = defaultArg enabled (View.Const true)
    let count = count |> View.MapCached(max 0)

    let selected =
      View.Map2 (fun n page -> if n = 0 then 0 else max 1 (min n page)) count selectedPage.View

    let state = View.zip count selected

    let select target =
      View.zip enabled state
      |> View.Get(fun (canSelect, (n, current)) ->
        if canSelect && n > 0 then
          let page = max 1 (min n (target n current))

          if page <> current then
            selectedPage.Set page)

    let control controlClass label icon canMove target =
      IconButton.create (
        Icon.create (
          Icon.UiActions icon,
          style = View.Const Style.Outlined,
          attrs = [ cl Css.``weave-pagination__icon``; Attr.Create "aria-hidden" "true" ]
        ),
        (fun () -> select target),
        enabled = View.Map2 (fun canSelect state -> canSelect && canMove state) enabled state,
        attrs = [
          cl Css.``weave-pagination__control``
          cl controlClass
          Attr.Create "aria-label" label
        ]
      )

    let boundary = control Css.``weave-pagination__boundary``
    let step = control Css.``weave-pagination__step``

    let pageButton page =
      if page < 0 then
        span [ cl Css.``weave-pagination__ellipsis``; Attr.Create "aria-hidden" "true" ] [ text "\u2026" ]
      else
        Button.create (
          text (string page),
          (fun () -> select (fun _ _ -> page)),
          enabled = enabled,
          attrs = [
            cl Css.``weave-pagination__page``
            Attr.Create "aria-label" (sprintf "Go to page %d" page)
            selected
            |> View.Map(fun current -> if current = page then "page" else "false")
            |> Attr.Dynamic "aria-current"
          ]
        )

    nav [
      cl Css.``weave-pagination``
      Attr.Create "aria-label" "Pagination"
      yield! defaultArg attrs []
    ] [
      boundary "First page" UiActions.FirstPage (fun (_, page) -> page > 1) (fun _ _ -> 1)
      step "Previous page" UiActions.ChevronLeft (fun (_, page) -> page > 1) (fun _ page -> page - 1)
      state
      |> View.MapCached(fun (n, page) ->
        PaginationInternal.pages n page boundaryCount siblingCount :> seq<int>)
      |> Doc.BindSeqCachedBy id pageButton
      step "Next page" UiActions.ChevronRight (fun (n, page) -> page < n) (fun _ page -> page + 1)
      boundary "Last page" UiActions.LastPage (fun (n, page) -> page < n) (fun n _ -> n)
    ]
