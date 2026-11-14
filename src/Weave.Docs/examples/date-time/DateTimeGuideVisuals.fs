namespace Weave.Docs.Examples

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module internal DateTimeGuideVisuals =

  type private Timeline = {
    Labels: string list
    Lower: LowerBounds<int>
    Upper: UpperBounds<int>
    Interval: Interval<int> option
  }

  let private svg name attrs children = Doc.SvgElement name attrs children
  let private coordinate value = sprintf "%.1f" value

  let private renderTimeline model =
    let lower =
      match model.Lower with
      | LowerBounds.Inclusive index -> Some index
      | LowerBounds.Exclusive index -> Some index
      | LowerBounds.Unbounded -> None

    let upper =
      match model.Upper with
      | UpperBounds.Inclusive index -> Some index
      | UpperBounds.Exclusive index -> Some index
      | UpperBounds.Unbounded -> None

    let x index =
      30.0 + float index * 360.0 / float (model.Labels.Length - 1)

    let includes index =
      model.Interval
      |> Option.exists (fun interval -> index |> Interval.inInterval interval)

    let includedLabels =
      model.Labels
      |> List.mapi (fun index label -> index, label)
      |> List.choose (fun (index, label) -> if includes index then Some label else None)

    let accessible =
      match model.Interval, includedLabels with
      | None, _ -> "Invalid bounds. No range was created."
      | Some _, [] -> "No shown dates fall within the interval."
      | Some _, labels -> "Included days: " + String.concat ", " labels

    figure [
      Attr.Class "docs-date-guide__timeline"
      Attr.Create "role" "img"
      Attr.Create "aria-label" accessible
    ] [
      div [ Attr.Class "docs-date-guide__labels"; Attr.Create "aria-hidden" "true" ] [
        for index in 0 .. model.Labels.Length - 1 do
          let label =
            if lower = Some index && upper = Some index then
              "start + end"
            elif lower = Some index then
              "start"
            elif upper = Some index then
              "end"
            else
              ""

          span [ Typography.body2; Attr.Class "docs-date-guide__endpoint-label" ] [ text label ]
      ]
      svg "svg" [ Attr.Create "viewBox" "0 20 420 36"; Attr.Create "aria-hidden" "true" ] [
        svg "line" [
          Attr.Create "x1" "12"
          Attr.Create "x2" "408"
          Attr.Create "y1" "38"
          Attr.Create "y2" "38"
          Attr.Class "docs-date-guide__axis"
        ] []
        if model.Interval.IsSome then
          let start = lower |> Option.map x |> Option.defaultValue 12.0
          let finish = upper |> Option.map x |> Option.defaultValue 408.0

          svg "line" [
            Attr.Create "x1" (coordinate start)
            Attr.Create "x2" (coordinate finish)
            Attr.Create "y1" "38"
            Attr.Create "y2" "38"
            Attr.Class "docs-date-guide__band"
          ] []

          if lower.IsNone then
            svg "path" [ Attr.Create "d" "M20 30 12 38 20 46"; Attr.Class "docs-date-guide__arrow" ] []

          if upper.IsNone then
            svg "path" [ Attr.Create "d" "M400 30 408 38 400 46"; Attr.Class "docs-date-guide__arrow" ] []
        for index, label in model.Labels |> List.indexed do
          let endpoint = lower = Some index || upper = Some index

          let state =
            if model.Interval.IsNone && endpoint then "invalid"
            elif includes index then "included"
            elif endpoint then "excluded"
            else "outside"

          svg "circle" [
            Attr.Create "cx" (coordinate (x index))
            Attr.Create "cy" "38"
            Attr.Create "r" (if endpoint then "9" else "4")
            Attr.Class("docs-date-guide__point docs-date-guide__point--" + state)
          ] []

      ]
      div [ Attr.Class "docs-date-guide__labels"; Attr.Create "aria-hidden" "true" ] [
        for label in model.Labels do
          span [ Typography.body1; Attr.Class "docs-date-guide__tick" ] [ text label ]
      ]
    ]

  let dateLine lower upper (interval: Interval<DateTime> option) =
    let lowerIndex =
      match lower with
      | LowerBounds.Inclusive(date: DateTime) -> LowerBounds.Inclusive(date.Day - 1)
      | LowerBounds.Exclusive date -> LowerBounds.Exclusive(date.Day - 1)
      | LowerBounds.Unbounded -> LowerBounds.Unbounded

    let upperIndex =
      match upper with
      | UpperBounds.Inclusive(date: DateTime) -> UpperBounds.Inclusive(date.Day - 1)
      | UpperBounds.Exclusive date -> UpperBounds.Exclusive(date.Day - 1)
      | UpperBounds.Unbounded -> UpperBounds.Unbounded

    renderTimeline {
      Labels = [ for day in 1..7 -> string<int> day ]
      Lower = lowerIndex
      Upper = upperIndex
      Interval = interval |> Option.bind (Interval.mapBounds (fun date -> date.Day - 1))
    }

  let endpointLegend () =
    div [ Typography.body2; Attr.Class "docs-date-guide__legend" ] [
      span [] [
        span [
          Attr.Class "docs-date-guide__legend-dot docs-date-guide__legend-dot--filled"
          Attr.Create "aria-hidden" "true"
        ] []
        text "Included endpoint"
      ]
      span [] [
        span [ Attr.Class "docs-date-guide__legend-dot"; Attr.Create "aria-hidden" "true" ] []
        text "Excluded endpoint"
      ]
    ]

  let availabilityGrid (month: DateTime) availability closures =
    div [
      Attr.Class "docs-date-guide__calendar"
      Attr.Create "role" "group"
      Attr.Create "aria-label" "Studio booking availability"
    ] [
      for day in 1..21 do
        let date = DateTime(month.Year, month.Month, day)
        let available = date |> DateAvailability.allows availability

        let closed =
          closures |> List.exists (fun interval -> date |> Interval.inInterval interval)

        let state, symbol, description =
          if closed then "closed", "×", "Already booked"
          elif available then "available", "✓", "Available"
          else "outside", "–", "Not offered"

        div [
          Attr.Class("docs-date-guide__date docs-date-guide__date--" + state)
          Attr.Create "role" "img"
          Attr.Create "aria-label" (sprintf "Day %d: %s" day description)
          Attr.Create "data-guide-day" (string<int> day)
        ] [
          span [] [ text (string<int> day) ]
          small [ Attr.Create "aria-hidden" "true" ] [ text symbol ]
        ]
    ]
