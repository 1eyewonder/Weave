namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Operators

[<JavaScript>]
module Typography =

  module Color =

    let primary = cl Css.``weave-typography--primary``
    let secondary = cl Css.``weave-typography--secondary``
    let tertiary = cl Css.``weave-typography--tertiary``
    let error = cl Css.``weave-typography--error``
    let warning = cl Css.``weave-typography--warning``
    let success = cl Css.``weave-typography--success``
    let info = cl Css.``weave-typography--info``

  module internal Text =

    let create htmlElement content textWrap attrs =
      let textWrap = defaultArg textWrap (View.Const true)
      let attrs = defaultArg attrs List.empty

      htmlElement [
        cl Css.``weave-typography``
        yield! attrs

        textWrap
        |> View.MapCached not
        |> Attr.DynamicClassPred Css.``weave-typography--nowrap``
      ] [ content ]

  /// <summary>Typography variant classes. Pass these via <c>?attrs</c>.</summary>
  module Variant =

    let h1 = cl Css.``weave-typography--h1``
    let h2 = cl Css.``weave-typography--h2``
    let h3 = cl Css.``weave-typography--h3``
    let h4 = cl Css.``weave-typography--h4``
    let h5 = cl Css.``weave-typography--h5``
    let h6 = cl Css.``weave-typography--h6``
    let subtitle1 = cl Css.``weave-typography--subtitle1``
    let subtitle2 = cl Css.``weave-typography--subtitle2``
    let body1 = cl Css.``weave-typography--body1``
    let body2 = cl Css.``weave-typography--body2``
    let button = cl Css.``weave-typography--button``
    let caption = cl Css.``weave-typography--caption``
    let overline = cl Css.``weave-typography--overline``

  /// <summary>Text alignment modifier classes. Pass these via <c>?attrs</c>.</summary>
  module Align =

    let ``inherit`` = cl Css.``weave-typography--align-inherit``
    let justify = cl Css.``weave-typography--align-justify``
    let center = cl Css.``weave-typography--align-center``
    let left = cl Css.``weave-typography--align-left``
    let right = cl Css.``weave-typography--align-right``
    let start = cl Css.``weave-typography--align-start``
    let end' = cl Css.``weave-typography--align-end``

  type ButtonText =

    static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
      let attr = cl Css.``weave-typography--button``

      attrs
      |> Option.mapOrDefault [ attr ] (List.append [ attr ])
      |> Some
      |> Text.create element content textWrap

    static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
      ButtonText.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

    static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
      ButtonText.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

    static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
      ButtonText.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

    static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
      ButtonText.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

    static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
      ButtonText.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

    static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
      ButtonText.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

open Typography

[<JavaScript>]
type H1 =

  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--h1``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member h1(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (h1, content, ?textWrap = textWrap, ?attrs = attrs)

  static member h1(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (h1, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member h1(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.create (h1, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H2 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--h2``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member h2(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (h2, content, ?textWrap = textWrap, ?attrs = attrs)

  static member h2(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (h2, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member h2(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.create (h2, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H3 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--h3``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member h3(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (h3, content, ?textWrap = textWrap, ?attrs = attrs)

  static member h3(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (h3, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member h3(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.create (h3, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H4 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--h4``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member h4(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (h4, content, ?textWrap = textWrap, ?attrs = attrs)

  static member h4(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (h4, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member h4(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.create (h4, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H5 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--h5``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member h5(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (h5, content, ?textWrap = textWrap, ?attrs = attrs)

  static member h5(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (h5, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member h5(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.create (h5, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H6 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--h6``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member h6(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (h6, content, ?textWrap = textWrap, ?attrs = attrs)

  static member h6(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (h6, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member h6(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.create (h6, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Subtitle1 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--subtitle1``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Subtitle2 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--subtitle2``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Body1 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--body1``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Body2 =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--body2``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Caption =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--caption``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Overline =
  static member create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = cl Css.``weave-typography--overline``

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.create (div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.create (div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.create (div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.create (span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.create (span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.create (span, text content, ?textWrap = textWrap, ?attrs = attrs)
