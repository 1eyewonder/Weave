namespace Weave

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave.CssHelpers.Core

[<JavaScript; RequireQualifiedAccess; Struct>]
type PickerMode =
  | Editable
  | ReadOnly
  | Disabled

[<JavaScript; RequireQualifiedAccess; Struct>]
type ClockFormat =
  | TwelveHour
  | TwentyFourHour

/// Shared field styles for date, date range, and time pickers.
[<JavaScript; RequireQualifiedAccess>]
module Picker =
  module Variant =
    let standard = Field.Variant.standard
    let filled = Field.Variant.filled
    let outlined = Field.Variant.outlined

  module Label =
    let above = Field.Label.above

  module Width =
    let full = cl Css.``weave-picker--full-width``

  module Controls =
    let clearable = cl Css.``weave-picker--clearable``

    let clearableWhen visible =
      Attr.DynamicClassPred Css.``weave-picker--clearable`` visible

  module Color =
    let primary = Attr.Concat [ cl Css.``weave-picker--primary``; Field.Color.primary ]

    let secondary =
      Attr.Concat [ cl Css.``weave-picker--secondary``; Field.Color.secondary ]

    let tertiary =
      Attr.Concat [ cl Css.``weave-picker--tertiary``; Field.Color.tertiary ]

    let error = Attr.Concat [ cl Css.``weave-picker--error``; Field.Color.error ]
    let warning = Attr.Concat [ cl Css.``weave-picker--warning``; Field.Color.warning ]
    let success = Attr.Concat [ cl Css.``weave-picker--success``; Field.Color.success ]
    let info = Attr.Concat [ cl Css.``weave-picker--info``; Field.Color.info ]

[<JavaScript>]
module internal PickerRender =
  let contributeErrors (field: FormField<'T>) errors =
    let owner = WeaveId.create "weave-picker-validation"
    let removal = DomRemovalListener.create ()

    on.afterRender (fun element ->
      field.ContributeErrors(owner, errors)
      removal.Watch(element, fun () -> field.RemoveErrors owner))

  // Like Carousel's controls, these small SVGs work without an external icon font.
  let icon path =
    Doc.SvgElement "svg" [
      cl Css.``weave-picker__icon``
      Attr.Create "viewBox" "0 0 24 24"
      Attr.Create "aria-hidden" "true"
      Attr.Create "focusable" "false"
    ] [
      Doc.SvgElement "path" [
        Attr.Create "d" path
        Attr.Create "fill" "none"
        Attr.Create "stroke" "currentColor"
        Attr.Create "stroke-width" "1.75"
        Attr.Create "stroke-linecap" "round"
        Attr.Create "stroke-linejoin" "round"
      ] []
    ]

  let boolAttr name view =
    Attr.DynamicCustom
      (fun el value -> el.SetAttribute(name, value))
      (view |> View.Map(fun b -> if b then "true" else "false"))

  let canInteract mode =
    mode |> View.Map((=) PickerMode.Editable)

  let isEnabled mode =
    mode |> View.Map((<>) PickerMode.Disabled)

  let isReadOnly mode =
    mode |> View.Map((=) PickerMode.ReadOnly)

  let iconAction label content enabled callback =
    IconButton.create (
      content,
      callback,
      enabled = enabled,
      attrs = [
        Button.Variant.text
        cl Css.``weave-picker__button``
        Attr.Create "aria-label" label
      ]
    )

  type Shell = {
    Name: string
    Icon: Doc
    Display: View<string>
    HasValue: View<bool>
    Clear: unit -> unit
    Opened: Var<bool>
    Mode: View<PickerMode>
    Label: View<string>
    Placeholder: View<string>
    Help: Doc option
    Validation: FieldValidation option
    ValidationMessages: View<string list> option
    Attrs: Attr list
  }

  [<Struct>]
  type private CloseFocus =
    | Restore
    | Preserve

  let shell (settings: Shell) content =
    let {
          Name = name
          Icon = icon
          Display = display
          HasValue = hasValue
          Clear = clear
          Opened = opened
          Mode = mode
          Label = label
          Placeholder = placeholder
          Help = help
          Validation = validation
          ValidationMessages = validationMessages
          Attrs = attrs
        } =
      settings

    let active = canInteract mode
    let enabled = isEnabled mode
    let readOnly = isReadOnly mode
    let visible = (opened.View, active) ||> View.Map2 (&&)
    let root = ref (Unchecked.defaultof<Dom.Element>)
    let trigger = ref (Unchecked.defaultof<Dom.Element>)
    let focused = Var.Create false
    let id = WeaveId.create "weave-picker"

    let close restore =
      Var.Set opened false

      if restore = Restore then
        JS.SetTimeout
          (fun () ->
            if not (isNull trigger.Value) && As<bool>(trigger.Value?isConnected) then
              trigger.Value?focus())
          0
        |> ignore

    let isFocused = (focused.View, visible) ||> View.Map2 (||)

    let hasPlaceholder =
      placeholder |> View.Map(fun value -> not (String.IsNullOrEmpty value))

    let floatLabel =
      (hasValue, isFocused, hasPlaceholder)
      |||> View.Map3(fun value focus hint -> value || focus || hint)

    let input =
      button [
        attr.``type`` "button"
        cl Css.``weave-field__input``
        cl Css.``weave-picker__trigger``
        Attr.Create "id" id
        Attr.Create "aria-haspopup" "dialog"
        Attr.Create "aria-controls" (id + "-popup")
        Attr.DynamicCustom
          (fun el v -> el.SetAttribute("aria-label", v))
          (label
           |> View.Map(fun value -> if String.IsNullOrWhiteSpace value then name else value))
        boolAttr "aria-expanded" visible
        Attr.enabled enabled
        boolAttr "aria-disabled" readOnly
        on.afterRender (fun el -> trigger.Value <- el)
        on.focus (fun _ _ -> Var.Set focused true)
        on.blur (fun _ _ -> Var.Set focused false)
        on.clickView active (fun _ _ can ->
          if can then
            Var.Set opened (not opened.Value))
        on.keyDownView active (fun _ ev can ->
          match ev with
          | Key.ArrowDown
          | Key.ArrowUp when can ->
            ev.PreventDefault()
            Var.Set opened true
          | Key.Escape -> close Preserve
          | _ -> ())
        match validation with
        | Some state -> yield! ValidationInput.attrs state
        | None -> ()
      ] [
        span [] [
          Doc.TextView(
            (hasValue, display, placeholder)
            |||> View.Map3(fun has value hint -> if has then value else hint)
          )
        ]
        icon
      ]

    let popup =
      visible
      |> Doc.BindView(fun show ->
        if not show then
          Doc.Empty
        else
          div [
            cl Css.``weave-picker__popup``
            // Field's label-click handler must not steal focus from popup controls.
            on.click (fun _ ev -> ev?stopPropagation ())
            Attr.Create "id" (id + "-popup")
            Attr.Create "role" "dialog"
            // Keep focus inside the picker when its non-interactive background is clicked.
            Attr.Create "tabindex" "-1"
            Attr.DynamicCustom
              (fun el v -> el.SetAttribute("aria-label", v))
              (label
               |> View.Map(fun value -> if String.IsNullOrWhiteSpace value then name else value))
            on.afterRender (fun el ->
              let bounds = el.GetBoundingClientRect()
              let viewport = float JS.Window.InnerWidth
              let shift = min 0.0 (viewport - 16.0 - bounds.Right)

              if shift < 0.0 then
                el?style?left <- sprintf "%fpx" shift

              let selected =
                el.QuerySelector("[data-picker-focus], [data-date][tabindex='0']:not([disabled])")

              let target =
                if isNull selected then
                  el.QuerySelector("button:not([disabled]), select:not([disabled])")
                else
                  selected

              if not (isNull target) then
                target?focus ())
          ] [
            content ()
            div [ cl Css.``weave-picker__actions`` ] [
              Button.create (
                text "Clear",
                (fun () ->
                  clear ()
                  close Restore),
                enabled = active,
                attrs = [
                  Button.Variant.outlined
                  cl Css.``weave-picker__button``
                  cl Css.``weave-picker__clear``
                  Attr.Create "aria-label" "Clear selection"
                  Attr.DynamicClassPred Css.``weave-picker__clear--has-value`` hasValue
                ]
              )

              Button.create (
                text "Done",
                (fun () -> close Restore),
                attrs = [
                  Button.Variant.filled
                  cl Css.``weave-picker__button``
                  Attr.Create "aria-label" "Close picker"
                ]
              )
            ]
          ])

    let rootAttrs = [
      cl Css.``weave-picker``
      on.afterRender (fun el -> root.Value <- el)
      on.keyDown (fun _ ev ->
        match ev with
        | Key.Escape when opened.Value ->
          ev.PreventDefault()
          close Restore
        | _ -> ())
      on.focusOut (fun _ ev ->
        let next = As<Dom.Element>(ev?relatedTarget)
        // A removed calendar cell has no next target; its replacement restores focus.
        if not (isNull next) && not (root.Value.Contains next) then
          close Preserve)
      yield! attrs
    ]

    // Popup editors can present messages beside their inputs without duplicating
    // the outer help text. The trigger retains the full validation attributes.
    let presentation =
      validation
      |> Option.map (fun state ->
        match validationMessages with
        | Some messages -> { state with VisibleErrors = messages }
        | None -> state)

    Field.create (
      Doc.Concat [
        input
        popup
        DocumentEventListener.onMouseDown [ root ] (fun () -> close Preserve) visible
        active
        |> Doc.sinkCached (fun can ->
          if not can then
            close Preserve)
      ],
      isFocused,
      floatLabel,
      labelText = label,
      ?helpText = help,
      showHelpText = View.Const help.IsSome,
      ?validation = presentation,
      enabled = enabled,
      inputId = id,
      attrs = rootAttrs
    )
