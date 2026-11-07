namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.JavaScript
open Weave

[<JavaScript>]
module GettingStartedExamples =

  [<Inline "document.getElementById($id).scrollIntoView({block: 'start', behavior: 'smooth'})">]
  let private scrollTo (id: string) = X<unit>

  let private stepLink id number title subtitle =
    a [
      attr.href ("#getting-started/" + id)
      on.click (fun _ ev ->
        ev.PreventDefault()
        scrollTo id)
    ] [
      span [] [ text number ]
      div [] [ strong [] [ text title ]; small [] [ text subtitle ] ]
    ]

  let private codeBlock lang content =
    div [ Attr.Class "docs-getting-started__code-wrap" ] [
      Helpers.copyButton content
      pre [ Attr.Class "docs-getting-started__code" ] [
        code [
          SurfaceColor.BackgroundColor.background
          Attr.Class(sprintf "language-%s" lang)
          on.afterRender Helpers.highlightCodeElement
        ] [ text content ]
      ]
    ]

  let private introduction () =
    div [ Attr.Class "docs-getting-started__orientation" ] [
      Helpers.sectionHeader "Get ready to build"
      div [ Attr.Class "docs-getting-started__orientation-body" ] [
        p [ Attr.Class "docs-getting-started__orientation-copy" ] [
          text
            "In this guide, you’ll run the included WebSharper app, build a reactive component, and see how to add Weave to your own project. Basic familiarity with F# will help."
        ]
        div [ Attr.Class "docs-getting-started__requirements" ] [
          div [ Attr.Class "docs-getting-started__requirements-label" ] [ text "What you’ll need" ]
          ul [ Attr.Class "docs-getting-started__requirements-list" ] [
            li [] [ strong [] [ text "Git" ]; span [] [ text "Source control" ] ]
            li [] [ strong [] [ text ".NET SDK" ]; span [] [ text "Version 10" ] ]
            li [] [ strong [] [ text "Node.js" ]; span [] [ text "22.12 or later" ] ]
            li [] [ strong [] [ text "Yarn" ]; span [] [ text "Version 4.5.3" ] ]
          ]
        ]
      ]
    ]

  let private runDocs () =
    Helpers.textSection "1. Get the source and run the site" [
      Helpers.bodyText
        "Clone the repository, initialize its dependencies, and start the docs from the repository root. Use build.cmd on Windows or build.sh on macOS/Linux."
      codeBlock
        "bash"
        """git clone https://github.com/1eyewonder/Weave.git
cd Weave"""
      div [ Attr.Class "docs-getting-started__platforms" ] [
        div [] [
          div [ Attr.Class "docs-getting-started__label" ] [ text "macOS / Linux" ]
          codeBlock
            "bash"
            """./build.sh init
yarn dev:docs"""
        ]
        div [] [
          div [ Attr.Class "docs-getting-started__label" ] [ text "Windows PowerShell" ]
          codeBlock
            "powershell"
            """.\build.cmd init
yarn dev:docs"""
        ]
      ]
      Helpers.bodyText
        "Open the URL printed by Vite (normally http://localhost:5173). You should see the Weave docs. Leave the command running while editing; F# changes rebuild the client and may reset page state. Stop it with Ctrl+C."
    ]

  let private firstPage () =
    Helpers.textSection "2. Render your first component" [
      Helpers.bodyText
        "Keep these instructions open elsewhere: the next step temporarily replaces the docs UI. Replace src/Weave.Docs/Client.fs with the code below, save it, and return to the Vite URL. The button should display a count and increment each time it is clicked. Restore the file afterward to return to the docs."
      codeBlock
        "fsharp"
        """namespace Weave.Docs

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module Client =
    [<SPAEntryPoint>]
    let Main () =
        let count = Var.Create 0
        div [] [
            textView (count.View |> View.Map (sprintf "Clicks: %d"))
            Button.primary(
                text "Click me",
                onClick = (fun () -> Var.Update count ((+) 1)),
                attrs = [ Button.Variant.filled ]
            )
        ] |> Doc.RunById "main"
"""
      Helpers.bodyText
        "Var holds changing state; its View lets the text update when that state changes. Doc is rendered UI, and attrs customize the component using WebSharper attributes. The existing index.html supplies the main element and loads the compiled client and stylesheet."
    ]

  let private existingProject () =
    Helpers.textSection "3. Use Weave in an existing WebSharper app" [
      Helpers.bodyText
        "Add a project reference to your checkout of src/Weave/Weave.fsproj. Adjust the path relative to your own .fsproj. The referenced project uses the repository's Paket and Yarn build setup, so keep the checkout and its initialized dependencies together."
      codeBlock
        "xml"
        """<ItemGroup>
  <ProjectReference Include="../Weave/src/Weave/Weave.fsproj" />
</ItemGroup>"""
      Helpers.bodyText
        "Weave's styles are separate from the F# reference. Run yarn build:css from the Weave repository root, copy src/Weave/styles.css into your app's static files, and link it from your HTML. Copy it again after rebuilding styles. The docs project demonstrates this with its CopyWeaveStyles build target."
      codeBlock "html" """<link rel="stylesheet" href="styles.css" />"""
      Helpers.bodyText
        "Use a URL matching where your app serves the copied file. Your existing WebSharper SPA entry point should render a Doc into an element in your page, as in the example above. WebSharper's own setup determines your script and host configuration."
    ]

  let private nextSteps () =
    Helpers.textSection "Where to go next" [
      Helpers.bodyText "Continue exploring with live examples, styling guides, and a complete application."
      div [ Attr.Class "docs-getting-started__next" ] [
        a [ attr.href "#button" ] [ text "Button" ]
        a [ attr.href "#text-field" ] [ text "Text Field" ]
        a [ attr.href "#reactive-helpers" ] [ text "Reactive Helpers" ]
        a [ attr.href "#theming" ] [ text "Theming" ]
        a [ attr.href "#showcase-task-tracker" ] [ text "Task Tracker" ]
      ]
    ]

  let render () =
    Container.create (
      div [ Attr.Class "docs-getting-started" ] [
        Helpers.pageTitle "Getting Started"
        div [ Attr.Class "docs-getting-started__lead" ] [
          text "From a local checkout to your first interactive component."
        ]
        div [ Attr.Class "docs-getting-started__path" ] [
          stepLink "1.-get-the-source-and-run-the-site" "01" "Run the docs" "Set up your workspace"
          stepLink "2.-render-your-first-component" "02" "Try a component" "See reactivity in action"
          stepLink
            "3.-use-weave-in-an-existing-websharper-app"
            "03"
            "Use in your app"
            "Connect an existing project"
        ]
        Helpers.divider ()
        introduction ()
        Helpers.divider ()
        runDocs ()
        Helpers.divider ()
        firstPage ()
        Helpers.divider ()
        existingProject ()
        Helpers.divider ()
        nextSteps ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
