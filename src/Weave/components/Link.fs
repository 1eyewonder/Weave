namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript; RequireQualifiedAccess>]
module Link =

  module Underline =

    let onHover = cl Css.``weave-link--underline-hover``
    let always = cl Css.``weave-link--underline-always``
    let none = cl Css.``weave-link--underline-none``

  module Color =

    let primary = cl Css.``weave-link--primary``
    let secondary = cl Css.``weave-link--secondary``
    let tertiary = cl Css.``weave-link--tertiary``
    let error = cl Css.``weave-link--error``
    let warning = cl Css.``weave-link--warning``
    let success = cl Css.``weave-link--success``
    let info = cl Css.``weave-link--info``

  let internal render href onClick enabled attrs children =
    let disabled = View.not enabled
    let state = Disabled.disabledClass Css.``weave-link--disabled`` enabled

    let isAction (el: WebSharper.JavaScript.Dom.Element) =
      Option.isNone href && Option.isSome onClick && el.GetAttribute("href") = "#"

    let mutable previousTabIndex: string option = None
    let mutable tabIndexSuppressed = false

    a
      [
        attr.href (defaultArg href "#")
        Attr.DynamicPred "aria-disabled" disabled (View.Const "true")
        on.clickView enabled (fun el ev canNavigate ->
          if not canNavigate || isAction el then
            ev.PreventDefault()

          if canNavigate then
            onClick |> Option.iter (fun activate -> activate ()))
        state
        yield! attrs

        enabled
        |> Attr.DynamicCustom(fun el canNavigate ->
          if canNavigate && tabIndexSuppressed then
            match previousTabIndex with
            | Some value -> el.SetAttribute("tabindex", value)
            | None -> el.RemoveAttribute "tabindex"

            tabIndexSuppressed <- false
          elif not canNavigate && not tabIndexSuppressed then
            previousTabIndex <- Option.ofObj (el.GetAttribute "tabindex")
            el.SetAttribute("tabindex", "-1")
            tabIndexSuppressed <- true)

      ]
      children

[<JavaScript; RequireQualifiedAccess>]
type Link =

  /// <summary>
  /// Creates a link with optional text label and start/end icon adornments.
  /// A callback without a destination does not navigate away from the current page.
  /// The underline decoration is applied only to the text portion, never to icons.
  /// Pass <c>Link.Underline.*</c> via <c>?attrs</c> to control decoration.
  /// </summary>
  static member create
    (
      innerContents: Doc,
      ?href: string,
      ?enabled: View<bool>,
      ?onClick: unit -> unit,
      ?startIcon: Doc,
      ?endIcon: Doc,
      ?attrs: Attr list
    ) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    let startIconDoc =
      match startIcon with
      | Some icon -> Html.span [ cl Css.``weave-link__start-icon`` ] [ icon ]
      | None -> Doc.Empty

    let endIconDoc =
      match endIcon with
      | Some icon -> Html.span [ cl Css.``weave-link__end-icon`` ] [ icon ]
      | None -> Doc.Empty

    let textLabel = Html.span [ cl Css.``weave-link__text`` ] [ innerContents ]

    Link.render href onClick enabled (cl Css.``weave-link`` :: attrs) [ startIconDoc; textLabel; endIconDoc ]

[<JavaScript; RequireQualifiedAccess>]
type IconLink =

  /// <summary>
  /// Creates a link containing only an icon — no text label and no underline.
  /// A callback without a destination does not navigate away from the current page.
  /// </summary>
  static member create
    (icon: Doc, ?href: string, ?enabled: View<bool>, ?onClick: unit -> unit, ?attrs: Attr list)
    =

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    Link.render href onClick enabled (cls [ Css.``weave-link``; Css.``weave-link--icon`` ] :: attrs) [ icon ]
