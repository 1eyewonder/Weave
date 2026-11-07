namespace WeaveStarter

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Theming

[<JavaScript>]
module Domain =
  type Issue = {
    Id: int
    Title: string
    Status: string
    Assignee: string
    Description: string
  }

  let statuses = [ "Backlog"; "In progress"; "Done" ]
  let people = [ "Alex"; "Jordan"; "Sam"; "Unassigned" ]

  let sampleIssues = [
    {
      Id = 101
      Title = "Map the onboarding journey"
      Status = "Done"
      Assignee = "Alex"
      Description = "Document the steps from signup to the first successful project."
    }
    {
      Id = 102
      Title = "Design the workspace navigation"
      Status = "In progress"
      Assignee = "Jordan"
      Description = "Explore navigation that stays clear on desktop and mobile."
    }
    {
      Id = 103
      Title = "Add project activity feed"
      Status = "In progress"
      Assignee = "Sam"
      Description = "Give the team a concise view of recent project updates."
    }
    {
      Id = 104
      Title = "Interview new customers"
      Status = "Backlog"
      Assignee = "Alex"
      Description = "Collect feedback on the first-run experience."
    }
    {
      Id = 105
      Title = "Write launch checklist"
      Status = "Backlog"
      Assignee = "Unassigned"
      Description = "Capture the remaining steps before the release."
    }
  ]
