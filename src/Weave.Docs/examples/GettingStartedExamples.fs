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

  let private templatePreview name description slug command =
    let image mode =
      let path = sprintf "assets/templates/%s-%s.jpg" slug mode
      let label = sprintf "%s template in %s mode" name mode

      a [
        Attr.Class("docs-template-preview__image docs-template-preview__image--" + mode)
        attr.href path
        Attr.Create "aria-label" (label + "; open full-size screenshot")
      ] [
        img [
          attr.src path
          attr.alt label
          Attr.Create "loading" "lazy"
          Attr.Create "width" "1440"
          Attr.Create "height" "900"
        ] []
      ]

    figure [ Attr.Class "docs-template-preview" ] [
      image "light"
      image "dark"
      figcaption [ Attr.Class "docs-template-preview__caption" ] [
        strong [] [ text name ]
        span [] [ text description ]
        Helpers.inlineCode command
      ]
    ]

  let private templateGallery () =
    div [ Attr.Class "docs-template-gallery" ] [
      templatePreview "Minimal app" "A small starting point for a custom app." "app" "weave-app"
      templatePreview
        "Project workspace"
        "A project tracker with list and board views."
        "projects"
        "weave-projects"
      templatePreview
        "Repository workspace"
        "Browse sample code, branches, and issues."
        "repository"
        "weave-repository"
      templatePreview
        "Business dashboard"
        "Explore metrics and a transactions table."
        "dashboard"
        "weave-dashboard"
    ]

  let private starterTemplates () =
    Helpers.textSection "4. Start from a template (optional)" [
      Helpers.bodyText
        "Start with an editable F# application and customize it for your project. The NuGet workflow below is planned for the template release; the template and Weave packages are not yet published."
      Helpers.bodyText
        "Choose weave-app for a minimal app, weave-projects for a project tracker with list and board views, weave-repository for a repository workspace, or weave-dashboard for a business dashboard. The workspace data is fictional, in memory, and resets on refresh."
      templateGallery ()
      Helpers.bodyText "Once the packages are available, install the template package with the .NET 10 SDK:"
      codeBlock "bash" "dotnet new install Weave.Templates"
      Helpers.bodyText
        "Choose one command below. Each creates a separate application containing only that starter; generate another project if you want to try a different workspace."
      codeBlock
        "bash"
        """dotnet new weave-app -n MyApp
dotnet new weave-projects -n MyProjects
dotnet new weave-repository -n MyRepository
dotnet new weave-dashboard -n MyDashboard"""
      Helpers.bodyText "For example, after creating MyProjects, run it with:"
      codeBlock
        "bash"
        """cd MyProjects
dotnet run --urls http://localhost:5000"""
      Helpers.bodyText
        "Open http://localhost:5000. Build and deploy the generated project you chose using your preferred hosting setup. Deployment configuration is not included, and the sample data is not persisted. The planned NuGet packages will supply Weave and its styles without a local repository checkout."
      Helpers.bodyText
        "In the generated project, start with README.md; the workspace starters separate sample data (Domain.fs), state (State.fs), page components (Views.fs), and CSS. Use the light/dark toggle and edit the stylesheet's body theme variables to make the starter yours. To uninstall the templates, run dotnet new uninstall Weave.Templates."
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
          stepLink "4.-start-from-a-template-optional" "04" "Try a template" "Start with a complete workspace"
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
        starterTemplates ()
        Helpers.divider ()
        nextSteps ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
