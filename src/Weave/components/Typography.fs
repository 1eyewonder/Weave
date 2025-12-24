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
      | BrandColor.Primary -> Css.``typography--primary``
      | BrandColor.Secondary -> Css.``typography--secondary``
      | BrandColor.Tertiary -> Css.``typography--tertiary``
      | BrandColor.Error -> Css.``typography--error``
      | BrandColor.Warning -> Css.``typography--warning``
      | BrandColor.Success -> Css.``typography--success``
      | BrandColor.Info -> Css.``typography--info``

  type Text =

    static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
      let textWrap = defaultArg textWrap (View.Const true)
      let attrs = defaultArg attrs List.empty

      div [
        cl Css.typography
        yield! attrs

        textWrap
        |> View.MapCached not
        |> Attr.DynamicClassPred Css.``typography--nowrap``
      ] [ text displayText ]

    static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
      let textWrap = defaultArg textWrap (View.Const true)
      let attrs = defaultArg attrs List.empty

      div [
        cl Css.typography
        yield! attrs

        textWrap
        |> View.MapCached not
        |> Attr.DynamicClassPred Css.``typography--nowrap``
      ] [ textView displayText ]

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
      | Typo.H1 -> Css.``typography--h1``
      | Typo.H2 -> Css.``typography--h2``
      | Typo.H3 -> Css.``typography--h3``
      | Typo.H4 -> Css.``typography--h4``
      | Typo.H5 -> Css.``typography--h5``
      | Typo.H6 -> Css.``typography--h6``
      | Typo.Subtitle1 -> Css.``typography--subtitle1``
      | Typo.Subtitle2 -> Css.``typography--subtitle2``
      | Typo.Body1 -> Css.``typography--body1``
      | Typo.Body2 -> Css.``typography--body2``
      | Typo.Button -> Css.``typography--button``
      | Typo.Caption -> Css.``typography--caption``
      | Typo.Overline -> Css.``typography--overline``

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
      | Align.Inherit -> Css.``typography--align-inherit``
      | Align.Justify -> Css.``typography--align-justify``
      | Align.Center -> Css.``typography--align-center``
      | Align.Left -> Css.``typography--align-left``
      | Align.Right -> Css.``typography--align-right``
      | Align.Start -> Css.``typography--align-start``
      | Align.End -> Css.``typography--align-end``

  type Button =

    static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
      let attr = Typo.toClass Typo.Button |> cl
      let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
      Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

    static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
      let attr = Typo.toClass Typo.Button |> cl
      let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
      Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

open Typography

[<JavaScript>]
type H1 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type H2 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type H3 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H3 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H3 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type H4 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H4 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H4 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type H5 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H5 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H5 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type H6 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H6 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.H6 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type Subtitle1 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Subtitle1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Subtitle1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type Subtitle2 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Subtitle2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Subtitle2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type Body1 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Body1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Body1 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type Body2 =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Body2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Body2 |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type Caption =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Caption |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Caption |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

[<JavaScript>]
type Overline =

  static member Create(displayText: string, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Overline |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)

  static member Create(displayText: View<string>, ?textWrap: View<bool>, ?attrs: Attr list) =
    let attr = Typo.toClass Typo.Overline |> cl
    let attrs = attrs |> Option.mapOrDefault [ attr ] (List.append [ attr ])
    Text.Create(displayText, ?textWrap = textWrap, attrs = attrs)
