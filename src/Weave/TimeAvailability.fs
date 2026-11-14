namespace Weave

open System
open WebSharper

/// Availability within one clock day. Bounds retain their precision; times never wrap across midnight.
[<JavaScript; RequireQualifiedAccess; NoEquality; NoComparison>]
type TimeAvailability = private TimeAvailability of Availability<TimeSpan>

[<JavaScript; RequireQualifiedAccess>]
module TimeAvailability =

  /// Allow the union, restricted to 00:00 inclusive through 24:00 exclusive.
  /// An empty list allows no times. Overlaps need no merging.
  /// Use two intervals for overnight windows; endpoints never wrap across midnight.
  let ofIntervals intervals =
    intervals
    |> Availability.ofIntervals
    |> Availability.filter (fun time -> time >= TimeSpan.Zero && time < TimeSpan.FromDays 1.0)
    |> TimeAvailability.TimeAvailability

  /// Allow every time of day, retaining sub-minute precision.
  let all = ofIntervals [ Interval.Unbounded ]

  /// Allow no times.
  let none = ofIntervals []

  /// Keep only times satisfying the predicate. Repeated filters combine with AND.
  /// Predicates receive exact valid times only after earlier restrictions accept them.
  let filter predicate (TimeAvailability.TimeAvailability availability) =
    availability
    |> Availability.filter predicate
    |> TimeAvailability.TimeAvailability

  /// Exclude the union of these intervals. Repeated calls accumulate exclusions.
  let excluding intervals (TimeAvailability.TimeAvailability availability) =
    availability
    |> Availability.excluding intervals
    |> TimeAvailability.TimeAvailability

  /// Test an exact time of day. Negative times and times at or after 24:00 are never allowed.
  let allows (TimeAvailability.TimeAvailability availability) time =
    time |> Availability.allows availability
