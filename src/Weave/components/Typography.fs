namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

[<JavaScript>]
module Typography =

  module Color =

    let toClass color =
      match color with
      | BrandColor.Primary -> Css.``weave-typography--primary``
      | BrandColor.Secondary -> Css.``weave-typography--secondary``
      | BrandColor.Tertiary -> Css.``weave-typography--tertiary``
      | BrandColor.Error -> Css.``weave-typography--error``
      | BrandColor.Warning -> Css.``weave-typography--warning``
      | BrandColor.Success -> Css.``weave-typography--success``
      | BrandColor.Info -> Css.``weave-typography--info``

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

  [<RequireQualifiedAccess; Struct>]
  type Typo =
    | H1
    | H2
    | H3
    | H4
    | H5
    | H6
    | Subtitle1
    | Subtitle2
    | Body1
    | Body2
    | Button
    | Caption
    | Overline

  module Typo =

    let toClass typo =
      match typo with
      | Typo.H1 -> Css.``weave-typography--h1``
      | Typo.H2 -> Css.``weave-typography--h2``
      | Typo.H3 -> Css.``weave-typography--h3``
      | Typo.H4 -> Css.``weave-typography--h4``
      | Typo.H5 -> Css.``weave-typography--h5``
      | Typo.H6 -> Css.``weave-typography--h6``
      | Typo.Subtitle1 -> Css.``weave-typography--subtitle1``
      | Typo.Subtitle2 -> Css.``weave-typography--subtitle2``
      | Typo.Body1 -> Css.``weave-typography--body1``
      | Typo.Body2 -> Css.``weave-typography--body2``
      | Typo.Button -> Css.``weave-typography--button``
      | Typo.Caption -> Css.``weave-typography--caption``
      | Typo.Overline -> Css.``weave-typography--overline``

  [<RequireQualifiedAccess; Struct>]
  type Align =
    | Inherit
    | Justify
    | Center
    | Left
    | Right
    | Start
    | End

  module Align =

    let toClass align =
      match align with
      | Align.Inherit -> Css.``weave-typography--align-inherit``
      | Align.Justify -> Css.``weave-typography--align-justify``
      | Align.Center -> Css.``weave-typography--align-center``
      | Align.Left -> Css.``weave-typography--align-left``
      | Align.Right -> Css.``weave-typography--align-right``
      | Align.Start -> Css.``weave-typography--align-start``
      | Align.End -> Css.``weave-typography--align-end``

  type Button =

    static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
      let attr = Typo.toClass Typo.Button |> cl

      attrs
      |> Option.mapOrDefault [ attr ] (List.append [ attr ])
      |> Some
      |> Text.create element content textWrap

    static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
      Button.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

    static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
      Button.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

    static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
      Button.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

    static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
      Button.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

    static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
      Button.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

    static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
      Button.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

open Typography

[<JavaScript>]
type H1 =

  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H1 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member H1(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(h1, content, ?textWrap = textWrap, ?attrs = attrs)

  static member H1(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(h1, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member H1(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(h1, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H2 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H2 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member H2(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(h2, content, ?textWrap = textWrap, ?attrs = attrs)

  static member H2(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(h2, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member H2(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(h2, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H3 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H3 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member H3(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(h3, content, ?textWrap = textWrap, ?attrs = attrs)

  static member H3(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(h3, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member H3(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(h3, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H4 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H4 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member H4(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(h4, content, ?textWrap = textWrap, ?attrs = attrs)

  static member H4(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(h4, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member H4(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(h4, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H5 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H5 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member H5(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(h5, content, ?textWrap = textWrap, ?attrs = attrs)

  static member H5(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(h5, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member H5(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(h5, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H6 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H6 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member H6(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(h6, content, ?textWrap = textWrap, ?attrs = attrs)

  static member H6(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(h6, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member H6(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(h6, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Subtitle1 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Subtitle1 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Subtitle2 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Subtitle2 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Body1 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Body1 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Body2 =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Body2 |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Caption =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Caption |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Overline =
  static member Create(element: WebSharperElement, content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Overline |> cl

    attrs
    |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    |> Some
    |> Text.create element content textWrap

  static member Div(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.Create(div, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.Create(div, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Div(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.Create(div, text content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.Create(span, content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.Create(span, textView content, ?textWrap = textWrap, ?attrs = attrs)

  static member Span(content: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.Create(span, text content, ?textWrap = textWrap, ?attrs = attrs)
