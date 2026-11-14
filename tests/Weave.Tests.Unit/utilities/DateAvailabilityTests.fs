module Weave.Tests.Unit.DateAvailabilityTests

open System
open Expecto
open FsCheck
open Weave.Tests.Unit.Generators
open Weave

let private date day = DateTime(2026, 11, day)

let private requireInterval interval =
  match interval with
  | Some value -> value
  | None -> failtest "The test fixture requires an ordered interval."

let private closed first last =
  Interval.closed (date first) (date last) |> requireInterval

let private windows =
  DateAvailability.ofIntervals [ closed 1 7; closed 5 7; closed 15 21 ]

[<Tests>]
let dateAvailabilityTests =
  testList "DateAvailability" [
    testTheory "Given overlapping windows When checking either window Then the date is allowed" [
      1
      5
      7
      15
      21
    ]
    <| fun day -> Expect.isTrue (date day |> DateAvailability.allows windows) "Union membership"

    testTheory "Given separate windows When checking outside their union Then the date is unavailable" [
      8
      14
      22
    ]
    <| fun day -> Expect.isFalse (date day |> DateAvailability.allows windows) "Gaps and outside dates"

    testCase "Given no allowed intervals When checking a date Then it is unavailable"
    <| fun () ->
      Expect.isFalse (date 1 |> DateAvailability.allows (DateAvailability.ofIntervals [])) "Empty union"

    testCase "Given no availability When checking a date Then it is unavailable"
    <| fun () -> Expect.isFalse (date 1 |> DateAvailability.allows DateAvailability.none) "Explicit none"

    testTheory "Given unrestricted availability When checking a date Then it is allowed" [
      DateTime.MinValue
      DateTime.MaxValue
    ]
    <| fun value -> Expect.isTrue (value |> DateAvailability.allows DateAvailability.all) "Unrestricted dates"

    testTheory "Given overlapping exclusions When checking either interval Then the date is unavailable" [
      3
      5
      7
      16
    ]
    <| fun day ->
      let availability =
        windows
        |> DateAvailability.excluding [ closed 3 5; closed 5 7; Interval.Degenerate(date 16) ]

      Expect.isFalse (date day |> DateAvailability.allows availability) "Exclusion union takes precedence"

    testCase "Given exclusions When checking an allowed date outside them Then it is allowed"
    <| fun () ->
      let availability =
        windows
        |> DateAvailability.excluding [ closed 3 5; Interval.Degenerate(date 16) ]

      Expect.isTrue (date 6 |> DateAvailability.allows availability) "Keep allowed windows"

    testCase "Given an exclusion outside the windows When checking that date Then it remains unavailable"
    <| fun () ->
      let availability =
        windows |> DateAvailability.excluding [ Interval.Degenerate(date 10) ]

      Expect.isFalse (date 10 |> DateAvailability.allows availability) "Exclusion never grants access"

    testCase "Given no exclusions When checking an allowed date Then it remains allowed"
    <| fun () ->
      Expect.isTrue
        (date 7 |> DateAvailability.allows (windows |> DateAvailability.excluding []))
        "Empty exclusion union"

    testTheory "Given repeated exclusions When checking either excluded date Then it is unavailable" [ 3; 16 ]
    <| fun day ->
      let availability =
        windows
        |> DateAvailability.excluding [ Interval.Degenerate(date 3) ]
        |> DateAvailability.excluding [ Interval.Degenerate(date 16) ]

      Expect.isFalse (date day |> DateAvailability.allows availability) "Accumulating exclusions"

    testCase "Given an unbounded exclusion When checking an allowed date Then it is unavailable"
    <| fun () ->
      Expect.isFalse
        (date 7
         |> DateAvailability.allows (windows |> DateAvailability.excluding [ Interval.Unbounded ]))
        "Exclude everything"

    testTheory "Given an exclusive exclusion When checking its endpoints Then they remain allowed" [ 3; 5 ]
    <| fun day ->
      let excluded =
        Interval.create (LowerBounds.Exclusive(date 3)) (UpperBounds.Exclusive(date 5))
        |> requireInterval

      Expect.isTrue
        (date day
         |> DateAvailability.allows (windows |> DateAvailability.excluding [ excluded ]))
        "Exclusive endpoints"

    testCase "Given an exclusive exclusion When checking its interior Then it is unavailable"
    <| fun () ->
      let excluded =
        Interval.create (LowerBounds.Exclusive(date 3)) (UpperBounds.Exclusive(date 5))
        |> requireInterval

      Expect.isFalse
        (date 4
         |> DateAvailability.allows (windows |> DateAvailability.excluding [ excluded ]))
        "Exclusive interior"

    testCase
      "Given an intraday exclusion When normalization makes its bounds contradictory Then that date remains allowed"
    <| fun () ->
      let excluded =
        Interval.create
          (LowerBounds.Inclusive((date 5).AddHours 9.0))
          (UpperBounds.Exclusive((date 5).AddHours 17.0))
        |> requireInterval

      Expect.isTrue
        (date 5
         |> DateAvailability.allows (windows |> DateAvailability.excluding [ excluded ]))
        "An exclusion with no selectable calendar dates is ignored"

    testCase "Given a singleton instant When checking another instant on that date Then it is allowed"
    <| fun () ->
      let availability =
        DateAvailability.ofIntervals [ Interval.Degenerate((date 7).AddHours 9.0) ]

      Expect.isTrue ((date 7).AddHours 18.0 |> DateAvailability.allows availability) "Whole-date singleton"

    testCase "Given an excluded instant When checking another instant on that date Then it is unavailable"
    <| fun () ->
      let availability =
        windows
        |> DateAvailability.excluding [ Interval.Degenerate((date 7).AddHours 9.0) ]

      Expect.isFalse ((date 7).AddHours 18.0 |> DateAvailability.allows availability) "Whole-date exclusion"

    testCase "Given exclusive timed bounds When checking their endpoint date Then it is unavailable"
    <| fun () ->
      let interval =
        Interval.create (LowerBounds.Exclusive((date 3).AddHours 9.0)) (UpperBounds.Inclusive(date 7))
        |> requireInterval

      Expect.isFalse
        ((date 3).AddHours 18.0
         |> DateAvailability.allows (DateAvailability.ofIntervals [ interval ]))
        "Exclusive calendar date"

    testCase "Given an open lower end When checking an earlier date Then it is allowed"
    <| fun () ->
      let interval =
        Interval.create LowerBounds.Unbounded (UpperBounds.Inclusive(date 7))
        |> requireInterval

      Expect.isTrue
        (DateTime.MinValue
         |> DateAvailability.allows (DateAvailability.ofIntervals [ interval ]))
        "Open lower end"

    testCase "Given an open upper end When checking a later date Then it is allowed"
    <| fun () ->
      let interval =
        Interval.create (LowerBounds.Inclusive(date 7)) UpperBounds.Unbounded
        |> requireInterval

      Expect.isTrue
        (DateTime.MaxValue
         |> DateAvailability.allows (DateAvailability.ofIntervals [ interval ]))
        "Open upper end"

    testCase "Given a positive filter When its predicate fails Then the date is unavailable"
    <| fun () ->
      let availability =
        windows
        |> DateAvailability.filter (fun date -> date.DayOfWeek <> DayOfWeek.Sunday)

      Expect.isFalse (date 1 |> DateAvailability.allows availability) "Positive filtering"

    testCase "Given multiple filters When one fails Then the date is unavailable"
    <| fun () ->
      let availability =
        windows
        |> DateAvailability.filter (fun date -> date.Day > 1)
        |> DateAvailability.filter (fun date -> date.Day < 7)

      Expect.isFalse (date 7 |> DateAvailability.allows availability) "Filters combine with AND"

    testCase "Given a filter When checking a date-time Then its predicate receives midnight"
    <| fun () ->
      let availability =
        windows |> DateAvailability.filter (fun date -> date.TimeOfDay = TimeSpan.Zero)

      Expect.isTrue
        ((date 7).AddHours 18.0 |> DateAvailability.allows availability)
        "Normalized predicate input"

    testCase "Given an unavailable date When filtering Then the predicate is not evaluated"
    <| fun () ->
      let failIfCalled (_: DateTime) =
        failwith "Unavailable dates must short-circuit"

      Expect.isFalse
        (date 8
         |> DateAvailability.allows (windows |> DateAvailability.filter failIfCalled))
        "Short-circuit filtering"

    testCase "Given an existing rule When deriving exclusions Then the original is preserved"
    <| fun () ->
      windows |> DateAvailability.excluding [ Interval.Unbounded ] |> ignore
      Expect.isTrue (date 7 |> DateAvailability.allows windows) "Immutable composition"
  ]

// Use a finite day set as the oracle, independently of Range/Interval membership.
let private calendarDate offset =
  DateTime(2000, 1, 1).AddDays(float offset)

let private overlappingBounds input =
  let sharedStart = input.Start + input.Left + 1
  let sharedEnd = sharedStart + input.Overlap
  (input.Start, sharedEnd), (sharedStart, sharedEnd + input.Right + 1)

let private calendarInterval first last =
  Interval.closed (calendarDate first) (calendarDate last) |> requireInterval

let private offeredWindows input =
  let (first, sharedEnd), (sharedStart, last) = overlappingBounds input
  [ calendarInterval first sharedEnd; calendarInterval sharedStart last ]

let private offeredDays input =
  let _, (_, last) = overlappingBounds input
  Set.ofList [ input.Start .. last ]

let private observedDays input availability =
  let _, (_, last) = overlappingBounds input

  [ input.Start - 4 .. last + 4 ]
  |> List.filter (fun day ->
    // Probe away from midnight so normalization is exercised in every case.
    (calendarDate day).AddHours(18.0).AddTicks(31L)
    |> DateAvailability.allows availability)
  |> Set.ofList

let private closures input =
  let (_, sharedEnd), (sharedStart, last) = overlappingBounds input

  [
    calendarInterval sharedStart sharedEnd
    calendarInterval sharedEnd (last - 1)
    calendarInterval (input.Start - 3) (input.Start - 1)
  ]

let private reservedDays input =
  let _, (sharedStart, last) = overlappingBounds input
  Set.ofList [ sharedStart .. last - 1 ]

let private classifyWindows input property =
  property
  |> Prop.classify (input.Overlap = 0) "touching windows"
  |> Prop.classify (input.Overlap > 0) "overlapping windows"
  |> Prop.classify (input.Start < 0) "before epoch"
  |> Prop.classify (input.Start >= 0) "on or after epoch"

[<Tests>]
let dateAvailabilityProperties =
  testList "DateAvailability.Properties" [
    testPropertyWithConfig
      config
      "Overlapping offered windows form a union regardless of order or duplication"
    <| Prop.forAll calendarWindows (fun input ->
      let windows = offeredWindows input

      for intervals in [ windows; List.rev windows; windows @ windows ] do
        let actual = DateAvailability.ofIntervals intervals |> observedDays input

        Expect.equal
          actual
          (offeredDays input)
          "The shared dates count once and neither unique part is lost."

      true |> Prop.ofTestable |> classifyWindows input)

    testPropertyWithConfig
      config
      "Exclusions subtract their union and never grant dates outside offered windows"
    <| Prop.forAll calendarWindows (fun input ->
      let expected = Set.difference (offeredDays input) (reservedDays input)

      let actual =
        DateAvailability.ofIntervals (offeredWindows input)
        |> DateAvailability.excluding (closures input)
        |> observedDays input

      Expect.equal
        actual
        expected
        "Overlapping reservations win; reservations outside the windows do not grant access."

      true |> Prop.ofTestable |> classifyWindows input)

    testPropertyWithConfig config "Repeated exclusions accumulate commute and remain idempotent"
    <| Prop.forAll calendarWindows (fun input ->
      let availability = DateAvailability.ofIntervals (offeredWindows input)
      let reservations = closures input
      let expected = Set.difference (offeredDays input) (reservedDays input)

      let forward =
        reservations
        |> List.fold
          (fun current interval -> current |> DateAvailability.excluding [ interval ])
          availability

      let reverse =
        List.rev reservations
        |> List.fold
          (fun current interval -> current |> DateAvailability.excluding [ interval ])
          availability

      let repeated = forward |> DateAvailability.excluding reservations

      for rule in [ forward; reverse; repeated ] do
        Expect.equal
          (observedDays input rule)
          expected
          "Earlier exclusions must survive later pipeline operations."

      true |> Prop.ofTestable |> classifyWindows input)

    testPropertyWithConfig
      config
      "Pure filters compose as intersection regardless of order repetition or fusion"
    <| Prop.forAll calendarWindows (fun input ->
      let (_, sharedEnd), (sharedStart, _) = overlappingBounds input
      let availability = DateAvailability.ofIntervals (offeredWindows input)

      let afterStart (date: DateTime) =
        date.TimeOfDay = TimeSpan.Zero && date >= calendarDate sharedStart

      let beforeEnd (date: DateTime) = date <= calendarDate sharedEnd
      let expected = Set.ofList [ sharedStart..sharedEnd ]
      // Commutativity applies to these pure predicates, not arbitrary side effects.
      let rules = [
        availability
        |> DateAvailability.filter afterStart
        |> DateAvailability.filter beforeEnd
        availability
        |> DateAvailability.filter beforeEnd
        |> DateAvailability.filter afterStart
        availability
        |> DateAvailability.filter (fun date -> afterStart date && beforeEnd date)
        availability
        |> DateAvailability.filter afterStart
        |> DateAvailability.filter afterStart
        |> DateAvailability.filter beforeEnd
      ]

      for rule in rules do
        Expect.equal
          (observedDays input rule)
          expected
          "Filters must narrow the original rule and receive whole dates."

      true |> Prop.ofTestable |> classifyWindows input)

    testPropertyWithConfig config "Pure filters and exclusions commute without restoring removed dates"
    <| Prop.forAll calendarWindows (fun input ->
      let availability = DateAvailability.ofIntervals (offeredWindows input)

      let isWeekday (date: DateTime) =
        date.DayOfWeek <> DayOfWeek.Saturday && date.DayOfWeek <> DayOfWeek.Sunday

      let expected =
        Set.difference (offeredDays input) (reservedDays input)
        |> Set.filter (calendarDate >> isWeekday)

      let rules = [
        availability
        |> DateAvailability.excluding (closures input)
        |> DateAvailability.filter isWeekday
        availability
        |> DateAvailability.filter isWeekday
        |> DateAvailability.excluding (closures input)
      ]

      for rule in rules do
        Expect.equal
          (observedDays input rule)
          expected
          "The combined policy is the same intersection of restrictions."

      true |> Prop.ofTestable |> classifyWindows input)

    testPropertyWithConfig
      config
      "Calendar normalization preserves date membership and drops contradictory projected bounds"
    <| Prop.forAll calendarWindows (fun input ->
      let _, (_, last) = overlappingBounds input
      let universe = Set.ofList [ input.Start - 4 .. last + 4 ]

      let cases = [
        for excludeFirst in [ false; true ] do
          for excludeLast in [ false; true ] do
            let firstInstant = (calendarDate input.Start).AddHours(9.0).AddTicks(7L)
            let lastInstant = (calendarDate last).AddHours(17.0).AddTicks(19L)

            let lower =
              if excludeFirst then
                LowerBounds.Exclusive firstInstant
              else
                LowerBounds.Inclusive firstInstant

            let upper =
              if excludeLast then
                UpperBounds.Exclusive lastInstant
              else
                UpperBounds.Inclusive lastInstant

            let interval = Interval.create lower upper |> requireInterval
            let firstDay = input.Start + (if excludeFirst then 1 else 0)
            let lastDay = last - (if excludeLast then 1 else 0)
            yield interval, Set.ofList [ firstDay..lastDay ]
        yield Interval.Degenerate((calendarDate input.Start).AddHours 12.0), Set.singleton input.Start
        yield Interval.Unbounded, universe
        let intraday =
          Interval.create
            (LowerBounds.Inclusive((calendarDate input.Start).AddHours 1.0))
            (UpperBounds.Exclusive((calendarDate input.Start).AddHours 2.0))
          |> requireInterval

        yield intraday, Set.empty
      ]

      for interval, expected in cases do
        let allowed = DateAvailability.ofIntervals [ interval ] |> observedDays input

        let excluded =
          DateAvailability.all
          |> DateAvailability.excluding [ interval ]
          |> observedDays input

        Expect.equal allowed expected "Endpoint times are removed without changing endpoint markers."

        Expect.equal
          excluded
          (Set.difference universe expected)
          "Exclusions normalize by the same calendar-date rules."

      true |> Prop.ofTestable |> classifyWindows input)

    testPropertyWithConfig config "Empty lists and unbounded exclusions obey their identities"
    <| Prop.forAll calendarWindows (fun input ->
      let availability = DateAvailability.ofIntervals (offeredWindows input)

      Expect.equal
        (availability |> DateAvailability.excluding [] |> observedDays input)
        (offeredDays input)
        "Empty exclusions change nothing."

      Expect.isEmpty
        (DateAvailability.ofIntervals [] |> observedDays input)
        "An empty allowed union allows no dates."

      Expect.isEmpty
        (availability
         |> DateAvailability.excluding [ Interval.Unbounded ]
         |> observedDays input)
        "An unbounded exclusion removes every date."

      true |> Prop.ofTestable |> classifyWindows input)

    testPropertyWithConfig
      config
      "Filters short circuit unavailable dates and predicates rejected earlier in the pipeline"
    <| Prop.forAll calendarWindows (fun input ->
      let availability = DateAvailability.ofIntervals (offeredWindows input)
      let _, (sharedStart, _) = overlappingBounds input

      let mustNotRun (_: DateTime) =
        failtest "A predicate was evaluated after the policy had already rejected the date."

      let excluded =
        availability
        |> DateAvailability.excluding (closures input)
        |> DateAvailability.filter mustNotRun

      let rejected =
        availability
        |> DateAvailability.filter (fun _ -> false)
        |> DateAvailability.filter mustNotRun

      let outside = availability |> DateAvailability.filter mustNotRun

      Expect.isFalse
        (DateAvailability.allows excluded (calendarDate sharedStart))
        "An exclusion must short circuit later predicates."

      Expect.isFalse
        (DateAvailability.allows rejected (calendarDate input.Start))
        "An earlier filter must short circuit later predicates."

      Expect.isFalse
        (DateAvailability.allows outside (calendarDate (input.Start - 4)))
        "Outside windows must short circuit predicates."

      true |> Prop.ofTestable |> classifyWindows input)

    testPropertyWithConfig config "Deriving restrictions leaves the source availability unchanged"
    <| Prop.forAll calendarWindows (fun input ->
      let original = DateAvailability.ofIntervals (offeredWindows input)

      let restricted =
        original
        |> DateAvailability.excluding [ Interval.Unbounded ]
        |> DateAvailability.filter (fun _ -> false)

      Expect.isEmpty (observedDays input restricted) "The derived rule has no available dates."
      Expect.equal (observedDays input original) (offeredDays input) "The original policy is immutable."
      true |> Prop.ofTestable |> classifyWindows input)
  ]
