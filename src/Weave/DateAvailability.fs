namespace Weave

open System
open WebSharper

/// Calendar-date availability, composed from allowed intervals and additional restrictions.
[<JavaScript; RequireQualifiedAccess; NoEquality; NoComparison>]
type DateAvailability = private DateAvailability of Availability<DateTime>

[<JavaScript; RequireQualifiedAccess>]
module DateAvailability =

  // Availability is expressed in whole dates; endpoint markers retain their meaning.
  let private toCalendarDateIntervals intervals =
    intervals
    |> List.choose (Interval.mapBounds (fun (date: DateTime) -> date.Date))

  /// Allow the union of these intervals. An empty list allows no dates; overlaps need no merging.
  /// Endpoint times are removed without changing inclusive/exclusive markers.
  /// Intervals whose projected bounds become contradictory contribute no dates.
  let ofIntervals intervals =
    intervals
    |> toCalendarDateIntervals
    |> Availability.ofIntervals
    |> DateAvailability.DateAvailability

  /// Allow every calendar date.
  let all = ofIntervals [ Interval.Unbounded ]

  /// Allow no calendar dates.
  let none = ofIntervals []

  /// Keep only dates satisfying the predicate. Predicates receive dates at midnight.
  /// Repeated filters combine with AND; predicates run only after earlier restrictions accept the date.
  let filter predicate (DateAvailability.DateAvailability availability) =
    availability
    |> Availability.filter predicate
    |> DateAvailability.DateAvailability

  /// Exclude the union of these intervals. Repeated calls accumulate exclusions.
  let excluding intervals (DateAvailability.DateAvailability availability) =
    availability
    |> Availability.excluding (toCalendarDateIntervals intervals)
    |> DateAvailability.DateAvailability

  /// Test a calendar date, ignoring its time portion.
  let allows (DateAvailability.DateAvailability availability) (date: DateTime) =
    date.Date |> Availability.allows availability

  // The calendar derives navigation limits before exclusions and predicates.
  let internal sourceIntervals (DateAvailability.DateAvailability availability) =
    availability |> Availability.sourceIntervals
