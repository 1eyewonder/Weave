namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module WeaveId =

  let mutable private counter = 0L

  let create prefix =
    counter <- counter + 1L
    sprintf "%s-%d" prefix counter

[<JavaScript>]
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
module Bounded =

  /// Clamp a value to [lo, hi]. Works with any comparable ordered type (int, float, etc.).
  [<Inline>]
  let inline clamp lo hi v = max lo (min hi v)

  /// Increment by step, clamped to [lo, hi].
  [<Inline>]
  let inline stepUp lo hi step v = clamp lo hi (v + step)

  /// Decrement by step, clamped to [lo, hi].
  [<Inline>]
  let inline stepDown lo hi step v = clamp lo hi (v - step)

  /// Value as a percentage within [lo, hi], result clamped to [0, 100].
  let percentOf (lo: float) (hi: float) (value: float) : float =
    if hi <= lo then
      0.0
    else
      clamp 0.0 100.0 (((value - lo) / (hi - lo)) * 100.0)

  /// Snap a value to the nearest step grid point within [lo, hi].
  let snapToStep (lo: float) (hi: float) (step: float) (v: float) : float =
    let clamped = clamp lo hi v

    if step > 0.0 then
      let steps = round ((clamped - lo) / step)
      clamp lo hi (lo + steps * step)
    else
      clamped

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
    map2Cached (fun v1 v2 -> v1, v2) view1 view2

  [<Inline>]
  let inline zip3 view1 view2 view3 =
    View.Map3 (fun v1 v2 v3 -> v1, v2, v3) view1 view2 view3

  [<Inline>]
  let inline zip3Cached view1 view2 view3 =
    map3Cached (fun v1 v2 v3 -> v1, v2, v3) view1 view2 view3

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

[<JavaScript>]
module Operators =

  let (<||>) a b = (a, b) ||> View.Map2(fun a b -> a || b)

  let (<&&>) a b = (a, b) ||> View.Map2(fun a b -> a && b)

[<JavaScript>]
module Attr =

  let enabled (isEnabled: View<bool>) : Attr =
    Attr.DynamicBool "disabled" (View.not isEnabled)

  [<Inline>]
  let inline bindOption ([<InlineIfLambda>] f: 'T -> Attr) (x: 'T option) = Option.mapOrDefault Attr.Empty f x

  /// <summary>
  /// Dynamically applies classes based on the value of a View and its corresponding map of values to class names. Meant to selectively apply one class at a time.
  /// </summary>
  [<Inline>]
  let inline classSelection<'T when 'T: comparison> (view: View<'T>) (map: Map<'T, string>) =
    Map.values map
    |> Seq.map (fun className ->
      view
      |> View.MapCached(fun v -> Map.tryFind v map = Some className)
      |> Attr.DynamicClassPred className)
    |> Attr.Concat

  let ariaInvalid (isInvalid: View<bool>) : Attr =
    isInvalid
    |> View.Map(sprintf "%b")
    |> Attr.DynamicCustom(fun el v -> el.SetAttribute("aria-invalid", v))

  module List =

    [<Inline>]
    let inline bindOption ([<InlineIfLambda>] f: 'T -> Attr) (x: 'T list option) =
      Option.mapOrDefault [] (List.map f) x

  module Tab =

    let natural = Attr.Create "tabindex" "-1"

    /// <summary>
    /// Will still be focusable
    /// </summary>
    let skip = Attr.Create "tabindex" "0"

/// <summary>
/// Active patterns for keyboard event handling. Provides a consistent,
/// type-safe vocabulary for matching key presses across all components.
/// </summary>
[<JavaScript; RequireQualifiedAccess>]
module internal Key =

  /// Matches Enter or Space — the standard ARIA "activate" keys for buttons and interactive elements.
  [<return: Struct>]
  let (|Activate|_|) (ev: Dom.KeyboardEvent) =
    match ev.Key with
    | "Enter"
    | " " -> ValueSome()
    | _ -> ValueNone

  [<return: Struct>]
  let (|Enter|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "Enter" then ValueSome() else ValueNone

  [<return: Struct>]
  let (|Space|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = " " then ValueSome() else ValueNone

  [<return: Struct>]
  let (|Escape|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "Escape" then ValueSome() else ValueNone

  [<return: Struct>]
  let (|Tab|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "Tab" then ValueSome() else ValueNone

  [<return: Struct>]
  let (|ArrowUp|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "ArrowUp" then ValueSome() else ValueNone

  [<return: Struct>]
  let (|ArrowDown|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "ArrowDown" then ValueSome() else ValueNone

  [<return: Struct>]
  let (|ArrowLeft|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "ArrowLeft" then ValueSome() else ValueNone

  [<return: Struct>]
  let (|ArrowRight|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "ArrowRight" then ValueSome() else ValueNone

  [<return: Struct>]
  let (|Home|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "Home" then ValueSome() else ValueNone

  [<return: Struct>]
  let (|End|_|) (ev: Dom.KeyboardEvent) =
    if ev.Key = "End" then ValueSome() else ValueNone

  /// Matches when the pressed key equals the given string.
  /// Useful for parameterized navigation where next/prev keys vary by orientation.
  [<return: Struct>]
  let (|NavKey|_|) (key: string) (ev: Dom.KeyboardEvent) =
    if ev.Key = key then ValueSome() else ValueNone
