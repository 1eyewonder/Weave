namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers

[<JavaScript>]
module GettingStartedExamples =

  let private whatIsWeave () =
    let description =
      Helpers.bodyText
        "Weave is an F# component library for building web UIs with WebSharper. It provides reactive UI components, layout primitives, and a light/dark theming system. Weave targets netstandard2.0, so it works anywhere WebSharper does."

    let content = Doc.Empty

    Helpers.section "What is Weave" description content

  let private installation () =
    let description =
      div [] [
        Body1.Div(
          "Weave is not yet published to NuGet. To use it, add a project reference to your WebSharper project."
        )
      ]

    let content =
      pre [] [
        code [
          SurfaceColor.toBackgroundColor SurfaceColor.Background
          Attr.Class "language-xml"
          on.afterRender Helpers.highlightCodeElement
        ] [ text """<ProjectReference Include="../path/to/Weave/Weave.fsproj" />""" ]
      ]

    Helpers.section "Installation" description content

  let private basicSetup () =
    let description =
      Body1.Div(
        "Open the Weave namespaces and render a component. Here is a minimal example that displays a button."
      )

    let content =
      pre [] [
        code [
          SurfaceColor.toBackgroundColor SurfaceColor.Background
          Attr.Class "language-fsharp"
          on.afterRender Helpers.highlightCodeElement
        ] [
          text
            """open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers

[<JavaScript>]
let page () =
    Button.Create(
        text "Click me",
        onClick = (fun () -> JavaScript.JS.Alert "Hello from Weave!"),
        attrs = [
            Button.Variant.Filled |> Button.Variant.toClass |> cl
            Button.Color.toClass BrandColor.Primary |> cl
        ]
    )"""
        ]
      ]

    Helpers.section "Basic Setup" description content

  let private coreConcepts () =
    let description =
      div [] [
        Body1.Div("Weave components share a small set of conventions that apply across the entire library.")

        div [ Margin.toClasses Margin.Top.small |> cls ] [
          H6.Div("CSS Helpers: cl and cls", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

          Body1.Div(
            "Use cl to apply a single CSS class and cls to apply several at once. These are the primary way to style Weave components."
          )

          pre [] [
            code [
              SurfaceColor.toBackgroundColor SurfaceColor.Background
              Attr.Class "language-fsharp"
              on.afterRender Helpers.highlightCodeElement
            ] [
              text
                """// Single class
cl Css.``weave-button--filled``

// Multiple classes
cls [
    Button.Variant.toClass Button.Variant.Filled
    Button.Color.toClass BrandColor.Primary
]"""
            ]
          ]
        ]

        div [ Margin.toClasses Margin.Top.small |> cls ] [
          H6.Div("BrandColor", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

          Body1.Div(
            "Components that support color accept a BrandColor value. The palette includes Primary, Secondary, Tertiary, Error, Warning, Success, and Info."
          )
        ]

        div [ Margin.toClasses Margin.Top.small |> cls ] [
          H6.Div("Reactive State: Var and View", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

          Body1.Div(
            "Var<'T> holds mutable state that you own. View<'T> is a read-only projection of that state. Components that accept a View update automatically when the underlying Var changes. Components that accept a Var can both read and write the value."
          )

          pre [] [
            code [
              SurfaceColor.toBackgroundColor SurfaceColor.Background
              Attr.Class "language-fsharp"
              on.afterRender Helpers.highlightCodeElement
            ] [
              text
                """let isChecked = Var.Create false

// Checkbox reads and writes the Var directly
Checkbox.Create(isChecked)

// A label that reacts to changes
isChecked.View
|> View.Map (fun v -> if v then "Checked" else "Unchecked")
|> Doc.TextView"""
            ]
          ]
        ]

        div [ Margin.toClasses Margin.Top.small |> cls ] [
          H6.Div("The attrs Pattern", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

          Body1.Div(
            "Every component accepts an optional attrs parameter as its last argument. Pass variant, color, size, and any custom attributes through this list. Weave never exposes styling as named parameters on Create."
          )

          pre [] [
            code [
              SurfaceColor.toBackgroundColor SurfaceColor.Background
              Attr.Class "language-fsharp"
              on.afterRender Helpers.highlightCodeElement
            ] [
              text
                """Button.Create(
    text "Save",
    onClick = (fun () -> ()),
    attrs = [
        cls [
            Button.Variant.toClass Button.Variant.Filled
            Button.Color.toClass BrandColor.Primary
        ]
        Attr.Style "min-width" "120px"
    ]
)"""
            ]
          ]
        ]
      ]

    let content = Doc.Empty

    Helpers.section "Core Concepts" description content

  let private nextSteps () =
    let description =
      div [] [
        Body1.Div("Explore the component pages in the sidebar to see live examples and code samples.")

        div [ Margin.toClasses Margin.Top.small |> cls ] [
          Grid.Create(
            [
              GridItem.Create(
                div [
                  SurfaceColor.toBackgroundColor SurfaceColor.Surface
                  BorderRadius.toClass BorderRadius.All.small |> cl
                  cls [ yield! Padding.toClasses Padding.All.small ]
                ] [
                  H6.Div("Layout", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

                  Body2.Div(
                    "Grid, Container, AppBar, Drawer, Divider, and Spacer provide the structural foundation for your pages."
                  )
                ],
                xs = Grid.Width.create 12,
                md = Grid.Width.create 4
              )
              GridItem.Create(
                div [
                  SurfaceColor.toBackgroundColor SurfaceColor.Surface
                  BorderRadius.toClass BorderRadius.All.small |> cl
                  cls [ yield! Padding.toClasses Padding.All.small ]
                ] [
                  H6.Div("Inputs", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

                  Body2.Div(
                    "Button, Field, Checkbox, Switch, Dropdown, and RadioButton handle user input with reactive two-way binding."
                  )
                ],
                xs = Grid.Width.create 12,
                md = Grid.Width.create 4
              )
              GridItem.Create(
                div [
                  SurfaceColor.toBackgroundColor SurfaceColor.Surface
                  BorderRadius.toClass BorderRadius.All.small |> cl
                  cls [ yield! Padding.toClasses Padding.All.small ]
                ] [
                  H6.Div("Feedback", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

                  Body2.Div(
                    "Alert, Dialog, and ExpansionPanel communicate status and reveal content progressively."
                  )
                ],
                xs = Grid.Width.create 12,
                md = Grid.Width.create 4
              )
            ],
            spacing = Grid.GutterSpacing.create 2
          )
        ]
      ]

    let content = Doc.Empty

    Helpers.section "Next Steps" description content

  let render () =
    Container.Create(
      div [] [
        Helpers.pageTitle "Getting Started"

        Helpers.bodyText "Everything you need to start building UIs with Weave and WebSharper."

        Helpers.divider ()
        whatIsWeave ()
        Helpers.divider ()
        installation ()
        Helpers.divider ()
        basicSetup ()
        Helpers.divider ()
        coreConcepts ()
        Helpers.divider ()
        nextSteps ()
      ],
      maxWidth = Container.MaxWidth.Large
    )
