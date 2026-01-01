namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper
open WebSharper.UI
open WebSharper.Sitelets
open Zanaptak.TypedCssClasses
open WebSharper.UI.Client
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

  type Text =

    static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
      let textWrap = defaultArg textWrap (View.Const true)
      let attrs = defaultArg attrs List.empty

      div [
        cl Css.``weave-typography``
        yield! attrs

        textWrap
        |> View.MapCached not
        |> Attr.DynamicClassPred Css.``weave-typography--nowrap``
      ] [ displayText ]

    static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
      Text.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

    static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
      Text.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

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

    static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
      let attr = Typo.toClass Typo.Button |> cl
      let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
      Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

    static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
      Button.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

    static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
      Button.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

open Typography

[<JavaScript>]
type H1 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H1.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H2 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H2.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H3 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H3 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H3.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H4 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H4 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H4.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H5 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H5 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H5.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type H6 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H6 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    H6.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Subtitle1 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Subtitle1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle1.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Subtitle2 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Subtitle2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Subtitle2.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Body1 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Body1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body1.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Body2 =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Body2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Body2.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Caption =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Caption |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Caption.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)

[<JavaScript>]
type Overline =

  static member Create(displayText: Doc, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Overline |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.Create(text displayText, ?textWrap = textWrap, ?attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    Overline.Create(textView displayText, ?textWrap = textWrap, ?attrs = attrs)
