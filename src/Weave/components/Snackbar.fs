namespace Weave

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core
open Weave.Icons.MaterialSymbols

[<JavaScript; RequireQualifiedAccess>]
module Snackbar =

  [<RequireQualifiedAccess>]
  type Dismissal =
    | After of milliseconds: int
    | Manual

  module Position =
    let topLeft = cl Css.``weave-snackbar-host--top-left``
    let topCenter = cl Css.``weave-snackbar-host--top-center``
    let topRight = cl Css.``weave-snackbar-host--top-right``
    let bottomLeft = cl Css.``weave-snackbar-host--bottom-left``
    let bottomCenter = cl Css.``weave-snackbar-host--bottom-center``
    let bottomRight = cl Css.``weave-snackbar-host--bottom-right``

  module Variant =
    let standard = cl Css.``weave-snackbar--standard``
    let outlined = cl Css.``weave-snackbar--outlined``
    let filled = cl Css.``weave-snackbar--filled``

  module Color =
    let primary = cl Css.``weave-snackbar--primary``
    let secondary = cl Css.``weave-snackbar--secondary``
    let tertiary = cl Css.``weave-snackbar--tertiary``
    let success = cl Css.``weave-snackbar--success``
    let error = cl Css.``weave-snackbar--error``
    let warning = cl Css.``weave-snackbar--warning``
    let info = cl Css.``weave-snackbar--info``

[<JavaScript>]
type SnackbarAction = private {
  Label: string
  Callback: unit -> unit
} with

  static member create(label: string, onClick: unit -> unit) = {
    Label = label
    Callback = onClick
  }

  member internal this.label = this.Label
  member internal this.invoke() = this.Callback()

[<JavaScript>]
type internal SnackbarEntry = {
  Id: string
  Key: string option
  Content: Doc
  Icon: Doc option
  Action: SnackbarAction option
  Dismissal: Snackbar.Dismissal
  Attrs: Attr list
}

/// Owns a queue of messages. Mount one Snackbar.create per controller.
[<JavaScript>]
type SnackbarController private (duration: int, maxVisible: int) =
  let defaultDuration = duration
  let entries = ListModel.Create (fun entry -> entry.Id) Seq.empty<SnackbarEntry>

  static member create(?duration: int, ?maxVisible: int) =
    SnackbarController(max 1 (defaultArg duration 5000), max 1 (defaultArg maxVisible 3))

  member internal _.visible = entries.View |> View.Map(Seq.truncate maxVisible)

  /// Removes a visible or queued message by its returned ID.
  member _.remove(id: string) = entries.RemoveByKey id

  member _.clear() = entries.Clear()

  /// A matching key returns the existing message ID without enqueueing another message.
  member _.add
    (
      content: Doc,
      ?dismissal: Snackbar.Dismissal,
      ?action: SnackbarAction,
      ?key: string,
      ?icon: Doc,
      ?attrs: Attr list
    ) =
    let existing =
      key
      |> Option.bind (fun k -> entries.Value |> Seq.tryFind (fun entry -> entry.Key = Some k))

    match existing with
    | Some entry -> entry.Id
    | None ->
      let id = WeaveId.create "weave-snackbar"

      let dismissal =
        defaultArg
          dismissal
          (if action.IsSome then
             Snackbar.Dismissal.Manual
           else
             Snackbar.Dismissal.After defaultDuration)
        |> function
          | Snackbar.Dismissal.After milliseconds -> Snackbar.Dismissal.After(max 1 milliseconds)
          | Snackbar.Dismissal.Manual -> Snackbar.Dismissal.Manual

      entries.Add {
        Id = id
        Key = key
        Content = content
        Icon = icon
        Action = action
        Dismissal = dismissal
        Attrs = defaultArg attrs []
      }

      id

  /// Text messages are safely encoded and deduplicated by message unless a key is supplied.
  member this.show
    (
      message: string,
      ?dismissal: Snackbar.Dismissal,
      ?action: SnackbarAction,
      ?key: string,
      ?icon: Doc,
      ?attrs: Attr list
    ) =
    this.add (
      text message,
      ?dismissal = dismissal,
      ?action = action,
      key = defaultArg key message,
      ?icon = icon,
      ?attrs = attrs
    )

[<JavaScript>]
module private SnackbarInternal =

  let render (controller: SnackbarController) (entry: SnackbarEntry) =
    let duration =
      match entry.Dismissal with
      | Snackbar.Dismissal.After milliseconds -> Some milliseconds
      | Snackbar.Dismissal.Manual -> None

    let timer = Var.Create<JS.Handle option> None
    let remaining = Var.Create(float (defaultArg duration 0))
    let started = Var.Create 0.0
    let hovered = Var.Create false
    let focused = Var.Create false
    let owner = Var.Create<Dom.Element option> None
    let removal = DomRemovalListener.create ()

    let paused = View.Map2 (||) hovered.View focused.View

    let timerOwner =
      View.Map2
        (fun element isPaused -> if duration.IsSome && not isPaused then element else None)
        owner.View
        paused

    let stop () =
      match timer.Value with
      | Some handle ->
        JS.ClearTimeout handle
        Var.Set timer None
        Var.Set remaining (max 0.0 (remaining.Value - (float (Date.Now()) - started.Value)))
      | None -> ()

    // The sink belongs to this message's Doc, so it follows its DOM lifetime.
    let watchTimer =
      timerOwner
      |> Doc.sinkCached (fun element ->
        stop ()

        match element with
        | Some el when As<bool>(el?isConnected) ->
          Var.Set started (float (Date.Now()))

          Var.Set
            timer
            (Some(
              JS.SetTimeout
                (fun () ->
                  stop ()
                  // A pause may arrive before the next DOM synchronization frame.
                  paused
                  |> View.Get(fun isPaused ->
                    if not isPaused && As<bool>(el?isConnected) then
                      controller.remove entry.Id))
                (int remaining.Value)
            ))
        | _ -> ())

    let dismiss () =
      stop ()
      controller.remove entry.Id

    div [
      cl Css.``weave-snackbar``
      Typography.body2
      Attr.Create "data-snackbar-id" entry.Id
      on.afterRender (fun el ->
        Var.Set owner (Some el)

        removal.Watch(
          el,
          fun () ->
            Var.Set owner None
            stop ()
        ))
      on.mouseEnter (fun _ _ -> Var.Set hovered true)
      on.mouseLeave (fun _ _ -> Var.Set hovered false)
      on.focusIn (fun _ _ -> Var.Set focused true)
      on.focusOut (fun el ev ->
        let next = ev?relatedTarget
        Var.Set focused (not (isNull next) && As<bool>(el?contains (next))))
      yield! entry.Attrs
    ] [
      watchTimer
      match entry.Icon with
      | Some icon -> div [ cl Css.``weave-snackbar__icon``; Attr.Create "aria-hidden" "true" ] [ icon ]
      | None -> Doc.Empty
      // Keep each message atomic within the host live region.
      div [ cl Css.``weave-snackbar__content``; Attr.Create "aria-atomic" "true" ] [ entry.Content ]
      match entry.Action with
      | Some action ->
        Button.create (
          text action.label,
          onClick =
            (fun () ->
              dismiss ()
              action.invoke ()),
          attrs = [ Button.Variant.text; cl Css.``weave-snackbar__action`` ]
        )
      | None -> Doc.Empty
      IconButton.create (
        Icon.create (Icon.UiActions UiActions.Close, attrs = [ Attr.Create "aria-hidden" "true" ]),
        onClick = dismiss,
        attrs = [
          Attr.Create "aria-label" "Dismiss notification"
          cl Css.``weave-snackbar__close``
        ]
      )
    ]

[<JavaScript>]
type Snackbar =

  /// Place near the application root, outside clipped or transformed containers.
  /// Timers run only for displayed messages and stop when their DOM is removed.
  static member create(controller: SnackbarController, ?attrs: Attr list) =
    div [
      cl Css.``weave-snackbar-host``
      Attr.Create "role" "region"
      Attr.Create "aria-label" "Notifications"
      Attr.Create "aria-live" "polite"
      Attr.Create "aria-relevant" "additions"
      yield! defaultArg attrs []
    ] [
      controller.visible
      |> Doc.BindSeqCachedBy (fun entry -> entry.Id) (SnackbarInternal.render controller)
    ]
