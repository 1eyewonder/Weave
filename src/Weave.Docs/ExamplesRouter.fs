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
    | AppBarExamples
    | SpacerExamples
    | ButtonExamples
    | ButtonGroupExamples
    | ButtonMenuExamples
    | TypographyExamples
    | TooltipExamples
    | GridExamples
    | CheckboxExamples
    | RadioButtonExamples
    | SwitchExamples
    | ContainerExamples
    | FieldExamples
    | NumericFieldExamples
    | DropdownExamples
    | ExpansionPanelExamples
    | DialogExamples
    | DrawerExamples
    | IconsExamples
    | TabsExamples
    | ListExamples
    | LinkExamples

  let private pageToString page =
    match page with
    | Home -> "Home"
    | AppBarExamples -> "App Bar"
    | SpacerExamples -> "Spacer"
    | ButtonExamples -> "Button"
    | ButtonGroupExamples -> "Button Group"
    | ButtonMenuExamples -> "Button Menu"
    | TypographyExamples -> "Typography"
    | TooltipExamples -> "Tooltip"
    | GridExamples -> "Grid"
    | CheckboxExamples -> "Checkbox"
    | RadioButtonExamples -> "Radio Button"
    | SwitchExamples -> "Switch"
    | ContainerExamples -> "Container"
    | FieldExamples -> "Field"
    | NumericFieldExamples -> "Numeric Field"
    | DropdownExamples -> "Dropdown"
    | ExpansionPanelExamples -> "Expansion Panel"
    | DialogExamples -> "Dialog"
    | DrawerExamples -> "Drawer"
    | IconsExamples -> "Icons"
    | TabsExamples -> "Tabs"
    | ListExamples -> "List"
    | LinkExamples -> "Link"

  let private renderPage page =
    match page with
    | Home ->
      Container.Create(
        div [] [
          H1.Div("Component Examples", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
          Body1.Div(
            "Welcome to the component documentation. Select a component from the navigation to view examples.",
            attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
          )
        ],
        maxWidth = Container.MaxWidth.Large
      )
    | AppBarExamples -> AppBarExamples.render ()
    | SpacerExamples -> SpacerExamples.render ()
    | ButtonExamples -> ButtonExamples.render ()
    | ButtonGroupExamples -> ButtonGroupExamples.render ()
    | ButtonMenuExamples -> ButtonMenuExamples.render ()
    | TypographyExamples -> TypographyExamples.render ()
    | TooltipExamples -> TooltipExamples.render ()
    | GridExamples -> GridExamples.render ()
    | CheckboxExamples -> CheckboxExamples.render ()
    | RadioButtonExamples -> RadioButtonExamples.render ()
    | SwitchExamples -> SwitchExamples.render ()
    | ContainerExamples -> ContainerExamples.render ()
    | FieldExamples -> FieldExamples.render ()
    | NumericFieldExamples -> NumericFieldExamples.render ()
    | DropdownExamples -> DropdownExamples.render ()
    | ExpansionPanelExamples -> ExpansionPanelExamples.render ()
    | DialogExamples -> DialogExamples.render ()
    | DrawerExamples -> DrawerExamples.render ()
    | IconsExamples -> IconsExamples.render ()
    | TabsExamples -> TabsExamples.render ()
    | ListExamples -> ListExamples.render ()
    | LinkExamples -> LinkExamples.render ()

  let private logo =
    div [
      cls [
        Flex.Inline.allSizes
        JustifyContent.toClass JustifyContent.Center
        AlignItems.toClass AlignItems.Center
      ]
    ] [
      img [
        attr.src "assets/weave-logo.png"
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
        H3.Div("Weave", attrs = [ cls [ "weave-logo-title" ] ])
        Body1.Div(
          "Threading Logic. Fabricating UI.",
          attrs = [ Attr.Style "font-style" "italic"; Attr.Style "white-space" "nowrap" ]
        )
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

    div [
      cls [
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
        AlignItems.toClass AlignItems.Start
        AlignContent.toClass AlignContent.Start
        yield! Margin.toClasses Margin.Bottom.extraLarge
      ]

      Attr.Style "min-height" "100vh"
      SurfaceColor.toBackgroundColor SurfaceColor.Background
    ] [
      let navButton target =
        currentPage.View
        |> Doc.BindView(fun page -> navButton page target (fun p -> Var.Set currentPage p))

      Grid.Create(
        items = [
          let item target =
            GridItem.Create(
              navButton target,
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              md = Grid.Width.create 4,
              lg = Grid.Width.create 3,
              xl = Grid.Width.create 1
            )

          GridItem.Create(logo)
          FlexBreak.Create()

          yield!
            [
              Home
              AppBarExamples
              SpacerExamples
              ButtonExamples
              ButtonGroupExamples
              ButtonMenuExamples
              TypographyExamples
              TooltipExamples
              GridExamples
              CheckboxExamples
              RadioButtonExamples
              SwitchExamples
              ContainerExamples
              FieldExamples
              NumericFieldExamples
              DropdownExamples
              ExpansionPanelExamples
              DialogExamples
              DrawerExamples
              IconsExamples
              TabsExamples
              ListExamples
              LinkExamples
            ]
            |> List.map item

          GridItem.Create(
            Button.Create(
              Theme.current.View
              |> View.Map(fun mode ->
                match mode with
                | Theming.Light -> text "Dark Mode"
                | Theming.Dark -> text "Light Mode")
              |> Doc.EmbedView,
              onClick =
                (fun () ->
                  let newMode = Theming.toggleMode ()
                  Var.Set Theme.current newMode),
              attrs = [
                Button.Width.toClass Button.Width.Full |> Attr.bindOption cl
                Button.Variant.Filled |> Button.Variant.toClass |> cl
                Button.Color.toClass BrandColor.Secondary |> cl
              ]
            ),
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 6,
            md = Grid.Width.create 4,
            lg = Grid.Width.create 3,
            xl = Grid.Width.create 1
          )

        ],
        justify = JustifyContent.SpaceEvenly,
        attrs = [
          cls [ AlignItems.toClass AlignItems.Center ]

          Breakpoint.browser
          |> View.Map(fun bp ->
            match bp with
            | Breakpoint.ExtraSmall
            | Breakpoint.Small -> "static"
            | _ -> "sticky")
          |> Attr.DynamicStyle "position"

          Attr.Style "top" "0"
          Attr.Style "z-index" "10"
          SurfaceColor.toBackgroundColor SurfaceColor.Background
          Padding.toClasses Padding.Bottom.medium |> cls
          Padding.toClasses Padding.Horizontal.small |> cls
        ]
      )

      Container.Create(
        div [
          cls [
            yield! Padding.toClasses Padding.Horizontal.large
            yield! Padding.toClasses Padding.Bottom.large
            FlexItem.Grow.allSizes
            FlexDirection.Column.allSizes
            AlignItems.toClass AlignItems.Start
            AlignContent.toClass AlignContent.Start
          ]
        ] [ currentPage.View |> Doc.BindView renderPage ],
        gutters = false
      )
    ]
