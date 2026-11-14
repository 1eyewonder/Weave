module Weave.Tests.Unit.TimeAvailabilityTests

open System
open Expecto
open FsCheck
open Weave
open Weave.Tests.Unit.Generators

let private time hour minute = TimeSpan(hour, minute, 0)
let private minute value = TimeSpan.FromMinutes(float value)

let private requireInterval interval =
  match interval with
  | Some interval -> interval
  | None -> failtest "The test fixture requires an ordered interval."

let private closed first last =
  Interval.closed first last |> requireInterval

[<Tests>]
let timeAvailabilityTests =
  testList "TimeAvailability" [
    testTheory "Given all availability When checking a clock time Then its exact precision is allowed" [
      TimeSpan.Zero
      time 9 30
      TimeSpan.FromTicks(TimeSpan.TicksPerDay - 1L)
    ]
    <| fun value -> Expect.isTrue (TimeAvailability.allows TimeAvailability.all value) "Clock-day member"

    testTheory "Given unbounded intervals When checking outside the clock day Then the time is unavailable" [
      TimeSpan.MinValue
      TimeSpan.FromTicks -1L
      TimeSpan.FromDays 1.0
      TimeSpan.MaxValue
    ]
    <| fun value ->
      Expect.isFalse
        (TimeAvailability.allows (TimeAvailability.ofIntervals [ Interval.Unbounded ]) value)
        "Clock bounds cannot be widened"

    testCase "Given no intervals When checking a valid time Then none are allowed"
    <| fun () -> Expect.isFalse (TimeAvailability.allows TimeAvailability.none (time 9 0)) "Empty union"

    testCase "Given overlapping windows When checking their union Then shared times are allowed once"
    <| fun () ->
      let rule =
        TimeAvailability.ofIntervals [ closed (time 9 0) (time 12 0); closed (time 11 0) (time 14 0) ]

      Expect.sequenceEqual
        ([ 8..15 ]
         |> List.filter (fun hour -> TimeAvailability.allows rule (time hour 0)))
        [ 9..14 ]
        "Union and outside times"

    testCase "Given exclusive endpoints When checking their boundaries Then they remain excluded"
    <| fun () ->
      let interval =
        Interval.create (LowerBounds.Exclusive(time 9 0)) (UpperBounds.Exclusive(time 10 0))
        |> requireInterval

      let rule = TimeAvailability.ofIntervals [ interval ]

      Expect.sequenceEqual
        ([ time 9 0; time 9 30; time 10 0 ] |> List.map (TimeAvailability.allows rule))
        [ false; true; false ]
        "Endpoint rules"

    testCase "Given a singleton When checking a nearby tick Then bounds are not rounded"
    <| fun () ->
      let exact = (time 9 30).Add(TimeSpan.FromTicks 7L)
      let rule = TimeAvailability.ofIntervals [ Interval.singleton exact ]
      Expect.isTrue (TimeAvailability.allows rule exact) "Exact singleton"
      Expect.isFalse (TimeAvailability.allows rule (exact.Add(TimeSpan.FromTicks 1L))) "Adjacent tick"

    testCase "Given overlapping exclusions When checking their union Then exclusions take precedence"
    <| fun () ->
      let rule =
        TimeAvailability.ofIntervals [ closed (time 9 0) (time 17 0) ]
        |> TimeAvailability.excluding [ closed (time 11 0) (time 12 0) ]
        |> TimeAvailability.excluding [ closed (time 11 30) (time 13 0) ]

      Expect.sequenceEqual
        ([ 9..17 ]
         |> List.filter (fun hour -> TimeAvailability.allows rule (time hour 0)))
        [ 9; 10; 14; 15; 16; 17 ]
        "Accumulated exclusions"

    testCase "Given an overnight schedule When using two intervals Then both sides of midnight are allowed"
    <| fun () ->
      let before =
        Interval.create LowerBounds.Unbounded (UpperBounds.Exclusive(time 2 0))
        |> requireInterval

      let after =
        Interval.create (LowerBounds.Inclusive(time 22 0)) UpperBounds.Unbounded
        |> requireInterval

      let rule = TimeAvailability.ofIntervals [ before; after ]

      Expect.sequenceEqual
        ([ time 1 0; time 2 0; time 12 0; time 22 0; time 23 0 ]
         |> List.map (TimeAvailability.allows rule))
        [ true; false; false; true; true ]
        "No implicit endpoint wrapping"

    testCase
      "Given an interval extending beyond midnight When checking times Then bounds are clipped without wrapping"
    <| fun () ->
      let rule =
        TimeAvailability.ofIntervals [ closed (time 22 0) (TimeSpan.FromHours 26.0) ]

      Expect.isFalse (TimeAvailability.allows rule (time 1 0)) "The next-day endpoint does not wrap"
      Expect.isTrue (TimeAvailability.allows rule (time 23 0)) "Clock-day intersection"
      Expect.isFalse (TimeAvailability.allows rule (TimeSpan.FromDays 1.0)) "Exclusive day limit"

    testCase "Given a predicate When checking outside the clock day Then the predicate is not called"
    <| fun () ->
      let rule =
        TimeAvailability.all
        |> TimeAvailability.filter (fun _ -> failtest "Invalid clock time reached predicate")

      Expect.isFalse (TimeAvailability.allows rule (TimeSpan.FromDays 1.0)) "Domain check runs first"

    testCase "Given an unbounded exclusion When filtering Then no time remains"
    <| fun () ->
      let rule = TimeAvailability.all |> TimeAvailability.excluding [ Interval.Unbounded ]
      Expect.isFalse (TimeAvailability.allows rule (time 9 0)) "Unbounded exclusion"
  ]

// A tick-based oracle avoids calling the production membership helpers.
let private containsTicks (interval: Interval<TimeSpan>) (value: TimeSpan) =
  let lower =
    function
    | LowerBounds.Inclusive(bound: TimeSpan) -> value.Ticks >= bound.Ticks
    | LowerBounds.Exclusive bound -> value.Ticks > bound.Ticks
    | LowerBounds.Unbounded -> true

  let upper =
    function
    | UpperBounds.Inclusive(bound: TimeSpan) -> value.Ticks <= bound.Ticks
    | UpperBounds.Exclusive bound -> value.Ticks < bound.Ticks
    | UpperBounds.Unbounded -> true

  match interval with
  | Interval.Unbounded -> true
  | Interval.Degenerate bound -> value.Ticks = bound.Ticks
  | Interval.Bounded range -> lower range.Lower && upper range.Upper

let private bounds (input: ClockWindows) =
  let shared = input.StartMinute + input.LeftMinutes + 1
  let finish = shared + input.OverlapMinutes
  (input.StartMinute, finish), (shared, finish + input.RightMinutes + 1)

let private windows input =
  let (first, leftEnd), (rightStart, last) = bounds input

  [
    closed (minute first) (minute leftEnd)
    closed (minute rightStart) (minute last)
  ]

let private observed input rule =
  let _, (_, last) = bounds input

  [ input.StartMinute - 2 .. last + 2 ]
  |> List.filter (fun value -> TimeAvailability.allows rule (minute value))
  |> Set.ofList

let private classify (input: ClockWindows) property =
  property
  |> Prop.classify (input.OverlapMinutes = 0) "touching"
  |> Prop.classify (input.OverlapMinutes > 0) "overlapping"

[<Tests>]
let timeAvailabilityProperties =
  testList "TimeAvailability.Properties" [
    testPropertyWithConfig config "All allows exactly one clock day without rounding or wrapping"
    <| Prop.forAll clockTimes (fun value ->
      Expect.equal
        (TimeAvailability.allows TimeAvailability.all value)
        (value.Ticks >= 0L && value.Ticks < TimeSpan.TicksPerDay)
        "Exact clock-day membership"

      true
      |> Prop.ofTestable
      |> Prop.classify (value.Ticks % TimeSpan.TicksPerMinute <> 0L) "sub-minute")

    testPropertyWithConfig config "Overlapping intervals form a union independent of order and duplication"
    <| Prop.forAll clockWindows (fun input ->
      let _, (_, last) = bounds input
      let expected = Set.ofList [ input.StartMinute .. last ]

      for source in [ windows input; List.rev (windows input); windows input @ windows input ] do
        Expect.equal (TimeAvailability.ofIntervals source |> observed input) expected "Union membership"

      true |> Prop.ofTestable |> classify input)

    testPropertyWithConfig config "Overlapping exclusions subtract their union and accumulate"
    <| Prop.forAll clockWindows (fun input ->
      let (_, leftEnd), (rightStart, last) = bounds input
      let offered = TimeAvailability.ofIntervals (windows input)
      let left = closed (minute rightStart) (minute leftEnd)
      let right = closed (minute leftEnd) (minute last)
      let expected = Set.ofList [ input.StartMinute .. rightStart - 1 ]
      let combined = offered |> TimeAvailability.excluding [ left; right ]

      let composed =
        offered
        |> TimeAvailability.excluding [ left ]
        |> TimeAvailability.excluding [ right ]

      Expect.equal (observed input combined) expected "Exclusion union"
      Expect.equal (observed input composed) expected "Repeated exclusions"
      true |> Prop.ofTestable |> classify input)

    testPropertyWithConfig config "Composed filters intersect and commute with interval exclusions"
    <| Prop.forAll clockWindows (fun input ->
      let (_, leftEnd), (rightStart, last) = bounds input
      let offered = TimeAvailability.ofIntervals (windows input)
      let reserved = [ closed (minute rightStart) (minute leftEnd) ]
      let even (value: TimeSpan) = int value.TotalMinutes % 2 = 0
      let third (value: TimeSpan) = int value.TotalMinutes % 3 <> 0

      let expected =
        [ input.StartMinute .. last ]
        |> List.filter (fun value ->
          (value < rightStart || value > leftEnd) && value % 2 = 0 && value % 3 <> 0)
        |> Set.ofList

      let left =
        offered
        |> TimeAvailability.filter even
        |> TimeAvailability.excluding reserved
        |> TimeAvailability.filter third

      let right =
        offered
        |> TimeAvailability.filter third
        |> TimeAvailability.filter even
        |> TimeAvailability.excluding reserved

      Expect.equal (observed input left) expected "Composed intersection"
      Expect.equal (observed input right) expected "Order independence"
      true |> Prop.ofTestable |> classify input)

    testPropertyWithConfig config "Singleton intervals preserve their exact payload only within the clock day"
    <| Prop.forAll clockTimes (fun value ->
      let rule = TimeAvailability.ofIntervals [ Interval.singleton value ]
      let valid = value.Ticks >= 0L && value.Ticks < TimeSpan.TicksPerDay
      Expect.equal (TimeAvailability.allows rule value) valid "No time normalization"
      true)

    testPropertyWithConfig config "Inclusive and exclusive endpoints retain tick precision"
    <| Prop.forAll clockTimes (fun value ->
      let first = max 0L (min (TimeSpan.TicksPerDay - 3L) value.Ticks)
      let lower, upper = TimeSpan.FromTicks first, TimeSpan.FromTicks(first + 2L)

      let interval =
        Interval.create (LowerBounds.Inclusive lower) (UpperBounds.Exclusive upper)
        |> requireInterval

      let rule = TimeAvailability.ofIntervals [ interval ]
      Expect.isTrue (TimeAvailability.allows rule lower) "Inclusive first tick"
      Expect.isTrue (TimeAvailability.allows rule (TimeSpan.FromTicks(first + 1L))) "Interior tick"
      Expect.isFalse (TimeAvailability.allows rule upper) "Exclusive last tick"
      true)

    testPropertyWithConfig config "Arbitrary bounds and exclusions intersect the clock day without wrapping"
    <| Prop.forAll clockEndpoints (fun (first, last) ->
      Prop.forAll clockEndpoints (fun (excludedFirst, excludedLast) ->
        let probes = [
          first
          last
          excludedFirst
          excludedLast
          TimeSpan.Zero
          TimeSpan.FromTicks -1L
          TimeSpan.FromTicks(TimeSpan.TicksPerDay - 1L)
          TimeSpan.FromDays 1.0
        ]

        for offered in intervals first last do
          for excluded in intervals excludedFirst excludedLast do
            let rule =
              TimeAvailability.ofIntervals [ offered ]
              |> TimeAvailability.excluding [ excluded ]

            for value in probes do
              let expected =
                value.Ticks >= 0L
                && value.Ticks < TimeSpan.TicksPerDay
                && containsTicks offered value
                && not (containsTicks excluded value)

              Expect.equal
                (TimeAvailability.allows rule value)
                expected
                "Clock clipping and endpoint membership"

        true
        |> Prop.ofTestable
        |> Prop.classify (first = last) "equal offered endpoints"
        |> Prop.classify (first < TimeSpan.Zero || last >= TimeSpan.FromDays 1.0) "outside clock bounds"))

    testPropertyWithConfig config "Restrictions short circuit later predicates and preserve their source"
    <| Prop.forAll clockWindows (fun input ->
      let (_, leftEnd), (rightStart, _) = bounds input
      let original = TimeAvailability.ofIntervals (windows input)
      let reservations = [ closed (minute rightStart) (minute leftEnd) ]

      let mustNotRun (_: TimeSpan) =
        failtest "A rejected time reached a later predicate."

      let excluded =
        original
        |> TimeAvailability.excluding reservations
        |> TimeAvailability.filter mustNotRun

      let rejected =
        original
        |> TimeAvailability.filter (fun _ -> false)
        |> TimeAvailability.filter mustNotRun

      let outside = original |> TimeAvailability.filter mustNotRun

      Expect.isFalse (TimeAvailability.allows excluded (minute rightStart)) "Exclusions short circuit."

      Expect.isFalse
        (TimeAvailability.allows rejected (minute input.StartMinute))
        "Earlier filters short circuit."

      Expect.isFalse
        (TimeAvailability.allows outside (minute (input.StartMinute - 1)))
        "Outside windows short circuit."

      Expect.isTrue
        (TimeAvailability.allows original (minute rightStart))
        "Deriving restrictions preserves the original."

      true)
  ]
