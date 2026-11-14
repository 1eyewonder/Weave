namespace Weave

open WebSharper

[<JavaScript; NoEquality; NoComparison>]
type internal Availability<'T when 'T: comparison> = private {
  Intervals: Interval<'T> list
  Accepts: 'T -> bool
}

[<JavaScript; RequireQualifiedAccess>]
module internal Availability =

  let ofIntervals intervals = {
    Intervals = intervals
    Accepts = fun value -> intervals |> List.exists (fun interval -> value |> Interval.inInterval interval)
  }

  let filter predicate availability = {
    availability with
        Accepts = fun value -> availability.Accepts value && predicate value
  }

  let excluding intervals availability =
    availability
    |> filter (fun value ->
      intervals
      |> List.exists (fun interval -> value |> Interval.inInterval interval)
      |> not)

  let allows availability value = availability.Accepts value

  // Navigation uses the original union; arbitrary predicates cannot provide finite limits.
  let sourceIntervals availability = availability.Intervals
