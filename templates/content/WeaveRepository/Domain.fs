namespace WeaveStarter

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Theming

[<JavaScript>]
module Domain =
  type Entry = {
    Path: string
    Name: string
    Folder: bool
    Message: string
    Updated: string
    Contents: string
  }

  type Issue = {
    Id: int
    Title: string
    State: string
    Label: string
    Author: string
    Description: string
  }

  let mainFiles = [
    {
      Path = ""
      Name = "docs"
      Folder = true
      Message = "Add the contributor guide"
      Updated = "2 days ago"
      Contents = ""
    }
    {
      Path = ""
      Name = "src"
      Folder = true
      Message = "Organize the starter modules"
      Updated = "yesterday"
      Contents = ""
    }
    {
      Path = ""
      Name = ".gitignore"
      Folder = false
      Message = "Ignore build output"
      Updated = "last week"
      Contents = "bin/\nobj/\n.env"
    }
    {
      Path = ""
      Name = "LICENSE"
      Folder = false
      Message = "Add project license"
      Updated = "last week"
      Contents = "Sample project license\n\nReplace with your project's chosen license."
    }
    {
      Path = ""
      Name = "README.md"
      Folder = false
      Message = "Welcome contributors"
      Updated = "2 days ago"
      Contents =
        "# Northstar\n\nA thoughtful starting point for your next project.\n\nGet oriented, explore the source, and make something useful."
    }
    {
      Path = "src"
      Name = "App.fs"
      Folder = false
      Message = "Organize the starter modules"
      Updated = "yesterday"
      Contents = "module Northstar.App\n\nlet greeting = \"Hello, world!\""
    }
    {
      Path = "src"
      Name = "Domain.fs"
      Folder = false
      Message = "Define project types"
      Updated = "2 days ago"
      Contents = "module Northstar.Domain\n\ntype Project = { Name: string }"
    }
    {
      Path = "docs"
      Name = "CONTRIBUTING.md"
      Folder = false
      Message = "Add the contributor guide"
      Updated = "2 days ago"
      Contents = "# Contributing\n\nThanks for helping make Northstar better."
    }
  ]

  let previewFiles = [
    {
      Path = ""
      Name = "preview"
      Folder = true
      Message = "Explore the next release"
      Updated = "today"
      Contents = ""
    }
    {
      Path = ""
      Name = "README.md"
      Folder = false
      Message = "Describe the preview branch"
      Updated = "today"
      Contents = "# Northstar preview\n\nAn early look at the next iteration."
    }
    {
      Path = "preview"
      Name = "Ideas.md"
      Folder = false
      Message = "Collect ideas"
      Updated = "today"
      Contents = "# Ideas\n\n- Faster onboarding\n- Simpler collaboration"
    }
  ]

  let issues = [
    {
      Id = 24
      Title = "Make the quick start easier to follow"
      State = "Open"
      Label = "documentation"
      Author = "Maya"
      Description = "A clearer first-run guide will help new contributors get started with confidence."
    }
    {
      Id = 21
      Title = "Add a compact project overview"
      State = "Open"
      Label = "enhancement"
      Author = "Noah"
      Description = "Explore a short overview that explains the project's structure and goals."
    }
    {
      Id = 18
      Title = "Improve keyboard focus styles"
      State = "Open"
      Label = "accessibility"
      Author = "Avery"
      Description = "Make focus visible throughout the application for keyboard users."
    }
    {
      Id = 12
      Title = "Document the release process"
      State = "Closed"
      Label = "documentation"
      Author = "Maya"
      Description = "Write down the steps for creating and publishing a release."
    }
  ]
