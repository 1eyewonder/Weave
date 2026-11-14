namespace Weave.Docs.Examples

open System
open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module PickerExampleHelpers =

  let guideLink () =
    div [ Margin.Bottom.small ] [
      Link.create (text "Date & Time Guide: Range, Interval, and availability", href = "#date-time-guide")
    ]

  let dateText (date: DateTime) = date.ToString("yyyy-MM-dd")

  let timeText (time: TimeSpan) =
    sprintf "%02d:%02d" time.Hours time.Minutes

  let stack content =
    div
      [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.start
        Gap.All.g4
      ]
      content

  let previews content =
    div [ Attr.Class "docs-picker-previews" ] content

  let columns preview settings =
    div [ Attr.Class "docs-appearance docs-picker-columns" ] [
      div [ Attr.Class "docs-appearance__preview" ] [ preview ]
      div [ Attr.Class "docs-appearance__settings" ] [ settings ]
    ]

  let formAttrs = [
    Flex.Flex.allSizes
    FlexDirection.Column.allSizes
    AlignItems.start
    Gap.All.g4
    Attr.Class "docs-picker-form"
  ]

  let actions content =
    div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] content

  let summary category title detail =
    Card.create (
      CardContent.create (
        stack [
          div [] [
            Chip.create (
              text category,
              attrs = [ Chip.Color.primary; Chip.Variant.outlined; Chip.Density.compact ]
            )
          ]
          div [ Typography.h6; Attr.Create "role" "status" ] [ textView title ]
          div [ Typography.body2; Attr.Class "docs-picker-summary__detail" ] [ text detail ]
        ],
        attrs = [ Padding.All.extraSmall ]
      ),
      attrs = [ Card.Variant.filled; Attr.Class "docs-picker-summary" ]
    )

  let modeControl (mode: Var<PickerMode>) =
    let selected =
      Var.Lens mode Some (fun current value -> value |> Option.defaultValue current)

    let items =
      [
        "Editable", PickerMode.Editable
        "Read Only", PickerMode.ReadOnly
        "Disabled", PickerMode.Disabled
      ]
      |> List.map (fun (label, value) -> SelectItem.create (text label, label, value))
      |> View.Const

    Select.create (
      items,
      selected,
      labelText = View.Const "Interaction",
      attrs = [
        Select.Variant.outlined
        Select.Label.above
        Select.Width.full
        Select.Color.primary
      ]
    )

  let cultureButtons () =
    div [ Attr.Class "docs-picker-culture-controls" ] [
      stack [
        actions [
          Button.primary (
            text "US",
            onClick = (fun () -> Globalization.setCulture "en-US"),
            attrs = [ Button.Variant.outlined ]
          )
          Button.primary (
            text "UK",
            onClick = (fun () -> Globalization.setCulture "en-GB"),
            attrs = [ Button.Variant.outlined ]
          )
          Button.primary (
            text "French",
            onClick = (fun () -> Globalization.setCulture "fr-FR"),
            attrs = [ Button.Variant.outlined ]
          )
        ]
        div [ Typography.caption ] [ text "Application culture: "; textView Globalization.culture ]
      ]

    ]

  // Match FormExamples: native submit behavior with the shared button styling.
  let submitButton label =
    button [
      attr.``type`` "submit"
      Attr.Class "weave-button"
      Button.Color.primary
      Button.Variant.filled
    ] [ span [ Attr.Class "weave-button__label" ] [ text label ] ]

  let boundsApi () =
    Helpers.apiTable "LowerBounds and UpperBounds" [
      Helpers.apiParam
        "LowerBounds.Inclusive value / UpperBounds.Inclusive value"
        "LowerBounds<'T> / UpperBounds<'T>"
        ""
        "Include the endpoint in membership checks."
      Helpers.apiParam
        "LowerBounds.Exclusive value / UpperBounds.Exclusive value"
        "LowerBounds<'T> / UpperBounds<'T>"
        ""
        "Exclude the endpoint in membership checks."
      Helpers.apiParam
        "LowerBounds.Unbounded / UpperBounds.Unbounded"
        "LowerBounds<'T> / UpperBounds<'T>"
        ""
        "Remove the limit on that side. Both bounds use the same comparable value type."
    ]

  let availabilityApi () =
    Helpers.apiTable "DateAvailability" [
      Helpers.apiParam "DateAvailability.all / none" "DateAvailability" "" "Allow every date or no dates."
      Helpers.apiParam
        "DateAvailability.ofIntervals intervals"
        "Interval<DateTime> list -> DateAvailability"
        ""
        "Allow dates in any interval. An empty list permits no dates. Overlaps are handled automatically. Endpoint times are interpreted as whole calendar dates, preserving inclusive/exclusive markers. Intervals whose projected bounds become contradictory contribute no dates: for example, [09:00, 17:00) on one date projects to [date, date), allowing no dates."
      Helpers.apiParam
        "DateAvailability.excluding intervals availability"
        "Interval<DateTime> list -> DateAvailability -> DateAvailability"
        ""
        "Exclude dates in any supplied interval, including singleton and unbounded intervals. Exclusions take precedence; repeated calls accumulate. An empty list changes nothing."
      Helpers.apiParam
        "DateAvailability.filter predicate availability"
        "(DateTime -> bool) -> DateAvailability -> DateAvailability"
        ""
        "Keep only dates satisfying the predicate, for example weekdays. Predicates receive dates at midnight; repeated filters are combined with AND and run only after earlier restrictions accept the date."
      Helpers.apiParam
        "DateAvailability.allows availability date"
        "DateAvailability -> DateTime -> bool"
        ""
        "Check the composed rule for a date, ignoring its time portion. Calendar navigation uses the envelope of the allowed intervals, before exclusions and predicates."
    ]

  let timeAvailabilityApi () =
    Helpers.apiTable "TimeAvailability" [
      Helpers.apiParam
        "TimeAvailability.all / none"
        "TimeAvailability"
        ""
        "Allow every valid time of day or no times."
      Helpers.apiParam
        "TimeAvailability.ofIntervals intervals"
        "Interval<TimeSpan> list -> TimeAvailability"
        ""
        "Allow the union, restricted to 00:00 inclusive through 24:00 exclusive. An empty list permits no times; overlaps need no merging. Preserve endpoint precision; no rounding or wrapping. Use two intervals for an overnight window."
      Helpers.apiParam
        "TimeAvailability.excluding intervals availability"
        "Interval<TimeSpan> list -> TimeAvailability -> TimeAvailability"
        ""
        "Remove matching times, including singleton or unbounded exclusions. Repeated calls accumulate; exclusions win over allowed windows. An empty list changes nothing."
      Helpers.apiParam
        "TimeAvailability.filter predicate availability"
        "(TimeSpan -> bool) -> TimeAvailability -> TimeAvailability"
        ""
        "Keep only times satisfying the predicate. Predicates receive exact valid times of day; repeated filters combine with AND and run only after earlier restrictions accept the time."
      Helpers.apiParam
        "TimeAvailability.allows availability time"
        "TimeAvailability -> TimeSpan -> bool"
        ""
        "Test exact membership. Negative times and times at or after 24:00 are always unavailable. The picker separately enforces whole minutes and minuteStep."
    ]

  let rangeApi () =
    Helpers.apiTable "Shared range model" [
      Helpers.apiParam
        "Range<'T>"
        "private single-case DU"
        ""
        "Opaque representation. Construct values through the Range module; Lower and Upper are read-only members."
      Helpers.apiParam
        "Range.Lower / Range.Upper"
        "LowerBounds<'T> / UpperBounds<'T>"
        ""
        "Read-only access to the inclusive, exclusive, or unbounded endpoints."
      Helpers.apiParam
        "Range.create lower upper"
        "Range<'T> option"
        ""
        "Returns None for reversed finite endpoints or equal endpoints with either end excluded. Equal inclusive endpoints permit one value. Unbounded ends are valid."
      Helpers.apiParam
        "Range.closed lower upper"
        "Range<'T> option"
        ""
        "Create inclusive endpoints. Returns None if lower is greater than upper; endpoints are never reordered."
      Helpers.apiParam
        "Range.singleton value"
        "Range<'T>"
        ""
        "A closed range with two equal endpoints; supported by the range picker."
      Helpers.apiParam
        "Range.tryGetClosed range"
        "('T * 'T) option"
        ""
        "Some for ordered, finite, inclusive endpoints; None otherwise. Values retain their original precision."
      Helpers.apiParam
        "Range.mapBounds projection range"
        "Range<'U> option"
        ""
        "Project endpoint values from 'T to 'U, preserving inclusive/exclusive markers, unbounded ends, and endpoint positions. Both types must support comparison. Returns None when projection reverses finite endpoints or makes them equal with either end excluded. No sorting or membership-preserving conversion is implied."
      Helpers.apiParam
        "Range.inRange range value"
        "bool"
        ""
        "Ordered-value membership using the named bounds."
      Helpers.apiParam
        "Range.toInterval range"
        "Interval<'T>"
        ""
        "Unbounded for two unbounded ends; Degenerate for equal inclusive ends; otherwise Bounded range."
    ]

  let intervalApi () =
    Helpers.apiTable "Shared interval model" [
      Helpers.apiParam
        "Interval.Bounded range"
        "Interval<'T>"
        ""
        "Wrap a Range<'T> containing named Lower and Upper bounds."
      Helpers.apiParam "Interval.Unbounded" "Interval<'T>" "" "Every value is permitted."
      Helpers.apiParam "Interval.Degenerate value" "Interval<'T>" "" "Exactly one value is permitted."
      Helpers.apiParam
        "Interval.create lower upper"
        "Interval<'T> option"
        ""
        "Construct a validated Bounded range. Returns None for reversed finite endpoints or equal endpoints with either end excluded."
      Helpers.apiParam
        "Interval.closed lower upper"
        "Interval<'T> option"
        ""
        "Construct a validated inclusive Bounded range. Returns None for reversed endpoints."
      Helpers.apiParam "Interval.singleton value" "Interval<'T>" "" "Construct a Degenerate interval."
      Helpers.apiParam
        "Interval.mapBounds projection interval"
        "Interval<'U> option"
        ""
        "Return None for reversed projected endpoints or equal finite endpoints with either end excluded; otherwise preserve Bounded, Unbounded, and Degenerate cases. Equal inclusive projected endpoints remain Bounded when the input was Bounded. Maps endpoints only; projections may reverse or collapse bounds. For example: let toCalendarDateBounds = Interval.mapBounds (fun (date: DateTime) -> date.Date)."
      Helpers.apiParam
        "Interval.tryGetBounds interval"
        "Range<'T> option"
        ""
        "None for Unbounded; an inclusive singleton range for Degenerate; the original range for Bounded."
      Helpers.apiParam
        "Interval.inInterval interval value"
        "bool"
        ""
        "Ordered-value membership for dates, times, and other comparable values."
      Helpers.apiParam
        "Interval.WithinInterval / OutsideInterval"
        "active pattern"
        ""
        "Bind this multi-case active pattern to an interval locally before matching (as required by F#)."
    ]
