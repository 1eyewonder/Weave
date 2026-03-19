namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess>]
module Button =

  module Variant =

    let filled = cl Css.``weave-button--filled``
    let outlined = cl Css.``weave-button--outlined``
    let text = cl Css.``weave-button--text``

  module Width =

    let full = cl Css.``weave-button--full-width``

  module Color =

    let primary = cl Css.``weave-button--primary``
    let secondary = cl Css.``weave-button--secondary``
    let tertiary = cl Css.``weave-button--tertiary``
    let error = cl Css.``weave-button--error``
    let warning = cl Css.``weave-button--warning``
    let success = cl Css.``weave-button--success``
    let info = cl Css.``weave-button--info``

    let toAttr color =
      match color with
      | BrandColor.Primary -> primary
      | BrandColor.Secondary -> secondary
      | BrandColor.Tertiary -> tertiary
      | BrandColor.Error -> error
      | BrandColor.Warning -> warning
      | BrandColor.Success -> success
      | BrandColor.Info -> info

[<JavaScript; RequireQualifiedAccess>]
type Button =

  /// <summary>Creates a button with the given content and click handler.</summary>
  /// <param name="innerContents">The Doc to render inside the button label.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes (variant, color, width, etc.) merged onto the root element.</param>
  static member create(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    let content =
      Typography.ButtonText.Div(
        innerContents,
        textWrap = View.Const false,
        attrs = [ cls [ Css.``weave-button__label``; Flex.Inline.allSizes ] ]
      )

    button [
      attr.``type`` "button"
      cl Css.``weave-button``

      yield! attrs

      Disabled.disabledClass Css.``weave-button--disabled`` enabled
      Disabled.nativeAttr enabled
      on.clickTapKeyViewGuarded enabled onClick
    ] [ content ]

  /// <summary>Creates a primary-colored button. Shorthand for <c>Button.create</c> with <c>Button.Color.primary</c>.</summary>
  /// <param name="innerContents">The Doc to render inside the button label.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member primary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.primary :: defaultArg attrs []
    )

  /// <summary>Creates a secondary-colored button. Shorthand for <c>Button.create</c> with <c>Button.Color.secondary</c>.</summary>
  /// <param name="innerContents">The Doc to render inside the button label.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member secondary
    (innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list)
    =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.secondary :: defaultArg attrs []
    )

  /// <summary>Creates a tertiary-colored button. Shorthand for <c>Button.create</c> with <c>Button.Color.tertiary</c>.</summary>
  /// <param name="innerContents">The Doc to render inside the button label.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member tertiary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.tertiary :: defaultArg attrs []
    )

  /// <summary>Creates an error-colored button. Shorthand for <c>Button.create</c> with <c>Button.Color.error</c>.</summary>
  /// <param name="innerContents">The Doc to render inside the button label.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member error(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.error :: defaultArg attrs []
    )

  /// <summary>Creates a warning-colored button. Shorthand for <c>Button.create</c> with <c>Button.Color.warning</c>.</summary>
  /// <param name="innerContents">The Doc to render inside the button label.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member warning(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.warning :: defaultArg attrs []
    )

  /// <summary>Creates a success-colored button. Shorthand for <c>Button.create</c> with <c>Button.Color.success</c>.</summary>
  /// <param name="innerContents">The Doc to render inside the button label.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member success(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.success :: defaultArg attrs []
    )

  /// <summary>Creates an info-colored button. Shorthand for <c>Button.create</c> with <c>Button.Color.info</c>.</summary>
  /// <param name="innerContents">The Doc to render inside the button label.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member info(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    Button.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.info :: defaultArg attrs []
    )

[<JavaScript; RequireQualifiedAccess>]
type IconButton =

  /// <summary>Creates an icon-only button with the given icon and click handler.</summary>
  /// <param name="icon">The Doc to render as the button's icon content.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes (variant, color, etc.) merged onto the root element.</param>
  static member create(icon: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =

    let enabled = defaultArg enabled (View.Const true)
    let attrs = defaultArg attrs List.empty

    button [
      attr.``type`` "button"
      cls [ Css.``weave-button``; Css.``weave-button--icon`` ]

      yield! attrs

      Disabled.disabledClass Css.``weave-button--disabled`` enabled
      Disabled.nativeAttr enabled
      on.clickTapKeyViewGuarded enabled onClick
    ] [ icon ]

  /// <summary>Creates a primary-colored icon button. Shorthand for <c>IconButton.create</c> with <c>Button.Color.primary</c>.</summary>
  /// <param name="innerContents">The Doc to render as the button's icon content.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member primary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.primary :: defaultArg attrs []
    )

  /// <summary>Creates a secondary-colored icon button. Shorthand for <c>IconButton.create</c> with <c>Button.Color.secondary</c>.</summary>
  /// <param name="innerContents">The Doc to render as the button's icon content.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member secondary
    (innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list)
    =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.secondary :: defaultArg attrs []
    )

  /// <summary>Creates a tertiary-colored icon button. Shorthand for <c>IconButton.create</c> with <c>Button.Color.tertiary</c>.</summary>
  /// <param name="innerContents">The Doc to render as the button's icon content.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member tertiary(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.tertiary :: defaultArg attrs []
    )

  /// <summary>Creates an error-colored icon button. Shorthand for <c>IconButton.create</c> with <c>Button.Color.error</c>.</summary>
  /// <param name="innerContents">The Doc to render as the button's icon content.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member error(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.error :: defaultArg attrs []
    )

  /// <summary>Creates a warning-colored icon button. Shorthand for <c>IconButton.create</c> with <c>Button.Color.warning</c>.</summary>
  /// <param name="innerContents">The Doc to render as the button's icon content.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member warning(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.warning :: defaultArg attrs []
    )

  /// <summary>Creates a success-colored icon button. Shorthand for <c>IconButton.create</c> with <c>Button.Color.success</c>.</summary>
  /// <param name="innerContents">The Doc to render as the button's icon content.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member success(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.success :: defaultArg attrs []
    )

  /// <summary>Creates an info-colored icon button. Shorthand for <c>IconButton.create</c> with <c>Button.Color.info</c>.</summary>
  /// <param name="innerContents">The Doc to render as the button's icon content.</param>
  /// <param name="onClick">Callback invoked when the button is clicked, tapped, or activated via keyboard.</param>
  /// <param name="enabled">Reactive view controlling the enabled/disabled state. Defaults to <c>true</c>.</param>
  /// <param name="attrs">Additional attributes merged onto the root element.</param>
  static member info(innerContents: Doc, onClick: unit -> unit, ?enabled: View<bool>, ?attrs: Attr list) =
    IconButton.create (
      innerContents,
      onClick,
      ?enabled = enabled,
      attrs = Button.Color.info :: defaultArg attrs []
    )
