namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave

[<JavaScript>]
module ExamplesRouter =

  [<Struct>]
  type Page =
    | Home
    | ButtonExamples
    | TypographyExamples
    | TooltipExamples
    | GridExamples
    | CheckboxExamples
    | RadioButtonExamples
    | SwitchExamples
    | ContainerExamples
    | NumericFieldExamples

  let private pageToString page =
    match page with
    | Home -> "Home"
    | ButtonExamples -> "Button"
    | TypographyExamples -> "Typography"
    | TooltipExamples -> "Tooltip"
    | GridExamples -> "Grid"
    | CheckboxExamples -> "Checkbox"
    | RadioButtonExamples -> "Radio Button"
    | SwitchExamples -> "Switch"
    | ContainerExamples -> "Container"
    | NumericFieldExamples -> "Numeric Field"

  let private renderPage page =
    match page with
    | Home ->
      Container.Create(
        div [] [
          H1.Create("Component Examples", attrs = [ Margin.toClasses Margin.Bottom.extraLarge |> cls ])
          Body1.Create(
            "Welcome to the component documentation. Select a component from the navigation to view examples.",
            attrs = [ Margin.toClasses Margin.Bottom.extraLarge |> cls ]
          )
        ],
        maxWidth = Container.MaxWidth.Large
      )
    | ButtonExamples -> ButtonExamples.render ()
    | TypographyExamples -> TypographyExamples.render ()
    | TooltipExamples -> TooltipExamples.render ()
    | GridExamples -> GridExamples.render ()
    | CheckboxExamples -> CheckboxExamples.render ()
    | RadioButtonExamples -> RadioButtonExamples.render ()
    | SwitchExamples -> SwitchExamples.render ()
    | ContainerExamples -> div [] [ text "Container examples coming soon..." ]
    | NumericFieldExamples -> div [] [ text "Numeric Field examples coming soon..." ]

  let private logo =
    div [
      cls [
        Flex.Inline.allSizes
        JustifyContent.toClass JustifyContent.Center
        AlignItems.toClass AlignItems.Center
      ]
    ] [
      img [
        attr.src "/assets/weave-logo.png"
        attr.alt "Weave Component Library Logo"
        Attr.Style "height" "100px"
        Attr.Style "display" "block"
        Attr.Style "object-fit" "contain"
      ] []
      div [
        cls [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          AlignItems.toClass AlignItems.Start
        ]
      ] [
        H3.Create("Weave", attrs = [ cls [ "weave-logo-title" ] ])
        Body1.Create("Threading Logic. Fabricating UI.", attrs = [ Attr.Style "font-style" "italic" ])
      ]
    ]

  let private navButton currentPage targetPage (setPage: Page -> unit) =
    let isActive = currentPage = targetPage

    Button.Create(
      text (pageToString targetPage),
      onClick = (fun () -> setPage targetPage),
      attrs = [
        (if isActive then
           Button.Variant.Filled
         else
           Button.Variant.Text)
        |> Button.Variant.toClass
        |> cl
        Margin.toClasses Margin.All.small |> cls
      ]
    )

  let render () =
    let currentPage = Var.Create Home
    let modeVar = Var.Create(Theming.getMode ())

    div [
      cls [
        yield! Margin.toClasses Margin.Bottom.large
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
      ]

      SurfaceColor.toAttr SurfaceColor.BackgroundDarker
    ] [
      currentPage.View
      |> Doc.BindView(fun page ->
        Grid.Create(
          items = [
            let item target =
              GridItem.Create(navButton page target (fun p -> Var.Set currentPage p))

            GridItem.Create(logo)
            FlexBreak.Create()

            yield!
              [
                Home
                ButtonExamples
                TypographyExamples
                TooltipExamples
                GridExamples
                CheckboxExamples
                RadioButtonExamples
                SwitchExamples
                ContainerExamples
                NumericFieldExamples
              ]
              |> List.map item

            GridItem.Create(
              Button.Create(
                modeVar.View
                |> View.Map(fun mode ->
                  match mode with
                  | Theming.Light -> text "🌙 Dark Mode"
                  | Theming.Dark -> text "☀️ Light Mode")
                |> Doc.EmbedView,
                onClick =
                  (fun () ->
                    let newMode = Theming.toggleMode ()
                    Var.Set modeVar newMode),
                attrs = [
                  Button.Variant.Text |> Button.Variant.toClass |> cl
                  JustifyContent.toClass JustifyContent.FlexEnd |> cl
                ]
              )
            )

          ],
          attrs = [ AlignItems.toClass AlignItems.Center |> cl ]
        ))

      div [
        Padding.toClasses Padding.All.large |> cls
        FlexItem.Grow.allSizes |> cl
        FlexDirection.Column.allSizes |> cl
      ] [ currentPage.View |> Doc.BindView renderPage ]
    ]
