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
            "Start a WebSharper application with Weave, render an interactive button, and explore the component library."
        ]
        div [ Attr.Class "docs-getting-started__requirements" ] [
          div [ Attr.Class "docs-getting-started__requirements-label" ] [ text "What you’ll need" ]
          ul [ Attr.Class "docs-getting-started__requirements-list" ] [
            li [] [
              strong [] [ text ".NET SDK" ]
              span [] [ text "10.0.403 or newer .NET 10 feature band" ]
            ]
            li [] [
              strong [] [ text "Package feed" ]
              span [] [ text "Weave release candidate packages" ]
            ]
            li [] [ strong [] [ text "F# and WebSharper" ]; span [] [ text "Basic familiarity" ] ]
          ]
        ]
      ]
    ]

  let private installPackage () =
    Helpers.textSection "1. Create a starter application" [
      Helpers.bodyText
        "The published release candidate packages are available on NuGet. The starter template references the matching Weave library version."
      Helpers.bodyText "Install the template, create the minimal app, and run it:"
      codeBlock
        "bash"
        """dotnet new install Weave.WebSharper.Templates@0.1.0-rc.1
dotnet new weave-app -n MyApp
cd MyApp
dotnet run"""
      Helpers.bodyText
        "Open the address printed by the app. The starter already references Weave and includes its styles. Its README explains the project layout and theme toggle."
    ]

  let private firstPage () =
    Helpers.textSection "2. Build an interactive component" [
      Helpers.bodyText
        "In the generated app, replace the contents of Client.fs with the example below. Run the app again and click the button to update the count. This changes your new starter, not the documentation site."
      codeBlock
        "fsharp"
        """namespace WeaveStarter

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
        "Var holds changing state and its View updates the displayed text. The starter's index.html supplies the main element and loads the app and stylesheet."
    ]

  let private existingProject () =
    Helpers.textSection "3. Add Weave to an existing app" [
      Helpers.bodyText "Add the published release candidate to an existing F# WebSharper project:"
      codeBlock "bash" "dotnet add package Weave.WebSharper --version 0.1.0-rc.1"
      Helpers.bodyText
        "The package includes styles.css as a WebSharper resource and a NuGet content file. Link the extracted stylesheet, or copy the content file into your app's static files and link it at the URL your host serves. Your existing WebSharper setup determines the script and host configuration."
      codeBlock "html" """<link rel="stylesheet" href="Scripts/Weave/styles.css" />"""
      Helpers.bodyText
        "For the tested WebSharper baseline and a local source checkout workflow, see the repository README."
    ]

  let private contributing () =
    Helpers.textSection "Work on the documentation" [
      Helpers.bodyText
        "To contribute to Weave itself, clone the repository and initialize its dependencies from the repository root:"
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
        "Open the URL printed by Vite. F# edits rebuild the client; stop the development processes with Ctrl+C."
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
    Helpers.textSection "4. Explore the workspace templates" [
      Helpers.bodyText
        "Start with an editable F# application and customize it for your project. The template and Weave packages are available as release candidates on NuGet."
      Helpers.bodyText
        "Choose weave-app for a minimal app, weave-projects for a project tracker with list and board views, weave-repository for a repository workspace, or weave-dashboard for a business dashboard. The workspace data is fictional, in memory, and resets on refresh."
      templateGallery ()
      Helpers.bodyText "Install the release-candidate template package with the .NET 10 SDK:"
      codeBlock "bash" "dotnet new install Weave.WebSharper.Templates@0.1.0-rc.1"
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
        "Open http://localhost:5000. Build and deploy the generated project you chose using your preferred hosting setup. Deployment configuration is not included, and the sample data is not persisted. The NuGet packages supply Weave and its styles without a local repository checkout."
      Helpers.bodyText
        "In the generated project, start with README.md; the workspace starters separate sample data (Domain.fs), state (State.fs), page components (Views.fs), and CSS. Use the light/dark toggle and edit the stylesheet's body theme variables to make the starter yours. To uninstall the templates, run dotnet new uninstall Weave.WebSharper.Templates."
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
          text "From a starter app to your first interactive component."
        ]
        div [ Attr.Class "docs-getting-started__path" ] [
          stepLink "1.-create-a-starter-application" "01" "Create an app" "Install the starter"
          stepLink "2.-build-an-interactive-component" "02" "Try a component" "See reactivity in action"
          stepLink "3.-add-weave-to-an-existing-app" "03" "Use in your app" "Connect an existing project"
          stepLink "4.-explore-the-workspace-templates" "04" "Explore starters" "See complete workspaces"
        ]
        Helpers.divider ()
        introduction ()
        Helpers.divider ()
        installPackage ()
        Helpers.divider ()
        firstPage ()
        Helpers.divider ()
        existingProject ()
        Helpers.divider ()
        starterTemplates ()
        Helpers.divider ()
        contributing ()
        Helpers.divider ()
        nextSteps ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
