namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module GettingStartedExamples =

  let private codeBlock lang content =
    pre [] [
      code [
        SurfaceColor.toBackgroundColor SurfaceColor.Background
        Attr.Class(sprintf "language-%s" lang)
        on.afterRender Helpers.highlightCodeElement
      ] [ text content ]
    ]

  let private whatIsWeave () =
    Helpers.textSection "What is Weave" [
      Helpers.bodyText
        "Weave is an F# component library for building web UIs with WebSharper. It provides reactive UI components, layout primitives, and a light/dark theming system. Weave targets netstandard2.0, so it works anywhere WebSharper does."
    ]

  let private installation () =
    Helpers.textSection "Installation" [
      Body1.Div(
        "Weave is not yet published to NuGet. To use it, add a project reference to your WebSharper project."
      )

      div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
        codeBlock "xml" """<ProjectReference Include="../path/to/Weave/Weave.fsproj" />"""
      ]
    ]

  let private basicSetup () =
    Helpers.textSection "Basic Setup" [
      Body1.Div(
        "Open the Weave namespaces and render a component. Here is a minimal example that displays a button."
      )

      div [ Margin.toClasses Margin.Top.extraSmall |> cls ] [
        codeBlock
          "fsharp"
          """open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open Weave


[<JavaScript>]
let page () =
    Button.primary(
        text "Click me",
        onClick = (fun () -> JavaScript.JS.Alert "Hello from Weave!"),
        attrs = [
            Button.Variant.filled
        ]
    )"""
      ]
    ]

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
      ],
      maxWidth = Container.MaxWidth.Large
    )
