namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<AutoOpen; JavaScript>]
module Generic =

  [<Inline>]
  let inline curry f a b = f (a, b)

  [<Inline>]
  let inline uncurry f (a, b) = f a b

[<JavaScript>]
module Option =

  [<Inline>]
  let inline mapOrDefault defaultValue ([<InlineIfLambda>] f: 'T -> 'U) opt =
    match opt with
    | Some v -> f v
    | None -> defaultValue

[<JavaScript>]
module Doc =

  [<Inline>]
  let inline sink ([<InlineIfLambda>] f: 'T -> unit) (view: View<'T>) =
    view
    |> Doc.BindView(fun v ->
      f v
      Doc.Empty)

  [<Inline>]
  let inline sinkCached ([<InlineIfLambda>] f: 'T -> unit) (view: View<'T>) =
    let mutable cache = None

    view
    |> sink (fun x ->
      match cache with
      | Some v when v = x -> ()
      | _ ->
        cache <- Some x
        f x)

  [<Inline>]
  let inline bindViewOption ([<InlineIfLambda>] f: 'T -> Doc) (view: View<'T option>) =
    view
    |> Doc.BindView(fun opt ->
      match opt with
      | Some v -> f v
      | None -> Doc.Empty)

  [<Inline>]
  let inline bindViewOptionOrDefault defaultValue ([<InlineIfLambda>] f: 'T -> Doc) (view: View<'T option>) =
    view
    |> Doc.BindView(fun opt ->
      match opt with
      | Some v -> f v
      | None -> defaultValue)

[<JavaScript>]
module View =

  [<Inline>]
  let inline bindCached ([<InlineIfLambda>] f: 'T -> View<'U>) view = View.MapCached f view |> View.Bind id

  [<Inline>]
  let inline map2Cached ([<InlineIfLambda>] f: 'T1 -> 'T2 -> 'U) view1 view2 =
    View.Map2 f view1 view2 |> View.MapCached id

  [<Inline>]
  let inline map3Cached ([<InlineIfLambda>] f: 'T1 -> 'T2 -> 'T3 -> 'U) view1 view2 view3 =
    View.Map3 f view1 view2 view3 |> View.MapCached id

  [<Inline>]
  let inline zip view1 view2 =
    View.Map2 (fun v1 v2 -> v1, v2) view1 view2

  [<Inline>]
  let inline zipCached view1 view2 =
    View.Map2 (fun v1 v2 -> v1, v2) view1 view2

  [<Inline>]
  let inline zip3 view1 view2 view3 =
    View.Map3 (fun v1 v2 v3 -> v1, v2, v3) view1 view2 view3

  [<Inline>]
  let inline zip3Cached view1 view2 view3 =
    View.Map3 (fun v1 v2 v3 -> v1, v2, v3) view1 view2 view3

  [<Inline>]
  let inline unzip view =
    let view1 = view |> View.Map fst
    let view2 = view |> View.Map snd
    view1, view2

  [<Inline>]
  let inline unzipCached view =
    let view1 = view |> View.MapCached fst
    let view2 = view |> View.MapCached snd
    view1, view2

  [<Inline>]
  let inline unzip3 view =
    let view1 = view |> View.Map(fun (v1, _, _) -> v1)
    let view2 = view |> View.Map(fun (_, v2, _) -> v2)
    let view3 = view |> View.Map(fun (_, _, v3) -> v3)
    view1, view2, view3

  [<Inline>]
  let inline unzip3Cached view =
    let view1 = view |> View.MapCached(fun (v1, _, _) -> v1)
    let view2 = view |> View.MapCached(fun (_, v2, _) -> v2)
    let view3 = view |> View.MapCached(fun (_, _, v3) -> v3)
    view1, view2, view3

  [<Inline>]
  let inline not (view: View<bool>) : View<bool> = view |> View.Map not

  [<Inline>]
  let inline printfn (view: View<'T>) =
    view |> View.MapCached(printfn "%A") |> Doc.sinkCached id

[<JavaScript>]
module ViewOption =

  [<Inline>]
  let inline map ([<InlineIfLambda>] f: 'T -> 'U) view =
    match view with
    | Some v -> Some(v |> View.Map f)
    | None -> None

  [<Inline>]
  let inline mapCached ([<InlineIfLambda>] f: 'T -> 'U) view =
    match view with
    | Some v -> Some(v |> View.MapCached f)
    | None -> None

  [<Inline>]
  let inline mapCachedOrDefault defaultValue ([<InlineIfLambda>] f: 'T -> 'U) view =
    view
    |> View.MapCached (function
      | Some v -> f v
      | None -> defaultValue)

  [<Inline>]
  let inline bind ([<InlineIfLambda>] f: 'T -> View<'U>) view =
    match view with
    | Some v -> Some(v |> View.Bind f)
    | None -> None

  [<Inline>]
  let inline bindCached ([<InlineIfLambda>] f: 'T -> View<'U>) view =
    match view with
    | Some v -> Some(v |> View.Bind f)
    | None -> None

  [<Inline>]
  let inline sequence (view: View<'T> option) : View<'T option> =
    match view with
    | Some v -> v |> View.Map Some
    | None -> View.Const None

[<JavaScript; AutoOpen>]
module Operators =

  let (<||>) a b = (a, b) ||> View.Map2(fun a b -> a || b)

  let (<&&>) a b = (a, b) ||> View.Map2(fun a b -> a && b)

[<JavaScript>]
module Attr =

  let enabled (isEnabled: View<bool>) : Attr =
    Attr.DynamicBool "disabled" (View.not isEnabled)

  [<Inline>]
  let inline bindOption ([<InlineIfLambda>] f: 'T -> Attr) (x: 'T option) = Option.mapOrDefault Attr.Empty f x

  /// Dynamically applies classes based on the value of a View and its corresponding map of values to class names. Meant to selectively apply one class at a time.
  [<Inline>]
  let inline classSelection<'T when 'T: comparison> (view: View<'T>) (map: Map<'T, string>) =
    Map.values map
    |> Seq.map (fun className ->
      view
      |> View.MapCached(fun v -> Map.tryFind v map = Some className)
      |> Attr.DynamicClassPred className)
    |> Attr.Concat

  module List =

    [<Inline>]
    let inline bindOption ([<InlineIfLambda>] f: 'T -> Attr) (x: 'T list option) =
      Option.mapOrDefault [] (List.map f) x

  module Tab =

    let natural = Attr.Create "tabindex" "-1"

    /// Will still be focusable
    let skip = Attr.Create "tabindex" "0"
