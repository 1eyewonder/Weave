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
    | DropdownExamples

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
    | DropdownExamples -> "Dropdown"

  let private renderPage page =
    match page with
    | Home ->
      Container.Create(
        div [] [
          H1.Create("Component Examples", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          Body1.Create(
            "Welcome to the component documentation. Select a component from the navigation to view examples.",
            attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
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
    | DropdownExamples -> DropdownExamples.render ()

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
           Button.Variant.Outlined)
        |> Button.Variant.toClass
        |> cl

        Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
        Button.Color.toClass BrandColor.Primary |> cl
      ]
    )

  let render () =
    let currentPage = Var.Create Home
    let modeVar = Var.Create(Theming.getMode ())

    div [
      cls [
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
        AlignItems.toClass AlignItems.Start
        AlignContent.toClass AlignContent.Start
        yield! Margin.toClasses Margin.Bottom.extraLarge
      ]

      Attr.Style "min-height" "100vh"
      SurfaceColor.toAttr SurfaceColor.BackgroundDarker
    ] [
      let navButton target =
        currentPage.View
        |> Doc.BindView(fun page -> navButton page target (fun p -> Var.Set currentPage p))

      Grid.Create(
        items = [
          let item target =
            GridItem.Create(navButton target, xs = Grid.Width.create 3, xl = Grid.Width.create 1)

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
              DropdownExamples
            ]
            |> List.map item

          GridItem.Create(
            Button.Create(
              modeVar.View
              |> View.Map(fun mode ->
                match mode with
                | Theming.Light -> text "Dark Mode"
                | Theming.Dark -> text "Light Mode")
              |> Doc.EmbedView,
              onClick =
                (fun () ->
                  let newMode = Theming.toggleMode ()
                  Var.Set modeVar newMode),
              attrs = [
                Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                Button.Color.toClass BrandColor.Secondary |> cl
              ]
            ),
            xs = Grid.Width.create 3,
            xl = Grid.Width.create 1
          )

        ],
        justify = JustifyContent.SpaceEvenly,
        attrs = [
          cls [ AlignItems.toClass AlignItems.Center ]
          Attr.Style "position" "sticky"
          Attr.Style "top" "0"
          Attr.Style "z-index" "1000"
          SurfaceColor.toAttr SurfaceColor.BackgroundDarker
          Padding.toClasses Padding.Bottom.medium |> cls
          Padding.toClasses Padding.Horizontal.small |> cls
        ]
      )

      div [
        cls [
          yield! Padding.toClasses Padding.Horizontal.large
          yield! Padding.toClasses Padding.Bottom.large
          FlexItem.Grow.allSizes
          FlexDirection.Column.allSizes
          AlignItems.toClass AlignItems.Start
          AlignContent.toClass AlignContent.Start
        ]
      ] [ currentPage.View |> Doc.BindView renderPage ]
    ]
