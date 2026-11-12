namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module ScrollTopConfigurator =

  let render () =
    let appearance = Var.Create ScrollTopAppearance.defaults
    let id = WeaveId.create "scroll-demo"

    let select label choices get set =
      let choice =
        Var.Lens appearance (get >> Some) (fun current value ->
          match value with
          | Some value -> set value current
          | None -> current)

      let items =
        choices
        |> List.map (fun (value, name) -> SelectItem.create (text name, name, value))
        |> View.Const

      Select.create (
        items,
        choice,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        select "Reveal after" ScrollTopAppearance.offsets (fun x -> x.Offset) (fun value x -> {
          x with
              Offset = value
        })
        select "Color" ScrollTopAppearance.colors (fun x -> x.Color) (fun value x -> { x with Color = value })
      ]

    let instant =
      Var.Lens appearance (fun current -> current.Instant) (fun current value -> {
        current with
            Instant = value
      })

    let colors =
      Map.ofList [
        "primary", Button.Color.primary
        "secondary", Button.Color.secondary
        "tertiary", Button.Color.tertiary
        "error", Button.Color.error
        "warning", Button.Color.warning
        "success", Button.Color.success
        "info", Button.Color.info
      ]

    let preview =
      appearance.View
      |> Doc.BindView(fun current ->
        div [ Attr.Style "position" "relative" ] [
          div [
            attr.id id
            attr.tabindex "0"
            Attr.Create "role" "region"
            Attr.Create "aria-label" "Scrollable chapters"
            Attr.Style "height" "260px"
            Overflow.yAuto
            Padding.All.small
            SurfaceColor.BackgroundColor.paper
          ] [
            for chapter in 1..12 do
              p [ Typography.body1; Margin.Bottom.small ] [
                text (sprintf "Chapter %d. Keep scrolling." chapter)
              ]
          ]
          ScrollToTop.create (
            selector = "#" + id,
            topOffset = current.Offset,
            attrs = [
              Map.find current.Color colors
              Attr.Style "position" "absolute"
              Attr.Create "aria-label" "Back to panel top"
              if current.Instant then
                ScrollToTop.Behavior.instant
            ]
          )
        ])

    let description =
      Helpers.bodyText
        "Focus the panel and scroll past the selected threshold to reveal Back to panel top. Smooth scrolling respects reduced-motion settings. The example includes a unique target ID and a positioned wrapper."

    Helpers.appearanceSection
      [ "scrollable-container" ]
      description
      controls
      (Some(Switch.create (instant, text "Instant scroll")))
      preview
      (fun () -> Var.Set appearance ScrollTopAppearance.defaults)
      (appearance.View |> View.Map ScrollTopAppearance.render)
