module Weave.Tests.Unit.RangeTests

open System
open Expecto
open FsCheck
open Weave.Tests.Unit.Generators
open Weave

let private contains value range =
  range |> Option.map (fun bounds -> value |> Range.inRange bounds)

[<Tests>]
let rangeTests =
  testList "Range" [
    testCase "Given ordered bounds When creating a range Then the named endpoints are retained"
    <| fun () ->
      let actual =
        Range.create (LowerBounds.Exclusive 1) (UpperBounds.Inclusive 5)
        |> Option.map (fun range -> range.Lower, range.Upper)

      Expect.equal actual (Some(LowerBounds.Exclusive 1, UpperBounds.Inclusive 5)) "Validated endpoints"

    testTheory "Given reversed finite bounds When creating a range Then None is returned" [
      LowerBounds.Inclusive 5, UpperBounds.Inclusive 1
      LowerBounds.Inclusive 5, UpperBounds.Exclusive 1
      LowerBounds.Exclusive 5, UpperBounds.Inclusive 1
      LowerBounds.Exclusive 5, UpperBounds.Exclusive 1
    ]
    <| fun (lower, upper) -> Expect.isNone (Range.create lower upper) "Invalid ordering is not representable"

    testCase "Given reversed dates When creating a closed range Then None is returned"
    <| fun () ->
      Expect.isNone
        (Range.closed (DateTime(2026, 11, 22)) (DateTime(2026, 11, 21)))
        "Closed construction validates ordering"

    testCase "Given equal inclusive endpoints When creating a range Then a single value is permitted"
    <| fun () -> Expect.equal (Range.closed 7 7 |> contains 7) (Some true) "Inclusive singleton"

    testTheory "Given equal endpoints with exclusion When creating a range Then None is returned" [
      LowerBounds.Exclusive 7, UpperBounds.Inclusive 7
      LowerBounds.Inclusive 7, UpperBounds.Exclusive 7
      LowerBounds.Exclusive 7, UpperBounds.Exclusive 7
    ]
    <| fun (lower, upper) -> Expect.isNone (Range.create lower upper) "Equal endpoints must both be inclusive"

    testTheory "Given an unbounded endpoint When creating a range Then the other endpoint is retained" [
      LowerBounds.Unbounded, UpperBounds.Inclusive 5
      LowerBounds.Exclusive 1, UpperBounds.Unbounded
      LowerBounds.Unbounded, UpperBounds.Unbounded
    ]
    <| fun (lower, upper) ->
      let actual =
        Range.create lower upper |> Option.map (fun range -> range.Lower, range.Upper)

      Expect.equal actual (Some(lower, upper)) "Unbounded ends do not introduce invalid ordering"

    testTheory "Given a range bound member When inspecting its public API Then it cannot be assigned" [
      "Lower"
      "Upper"
    ]
    <| fun name -> Expect.isFalse (typeof<Range<int>>.GetProperty(name).CanWrite) "Read-only members"

    testCase "Given equal validated ranges When comparing them Then structural equality is preserved"
    <| fun () ->
      Expect.equal
        (Range.closed 1 5)
        (Range.create (LowerBounds.Inclusive 1) (UpperBounds.Inclusive 5))
        "Equal bounds"

    testCase "Given validated ranges When ordering them Then lower bound comparison is preserved"
    <| fun () -> Expect.isLessThan (Range.closed 1 5) (Range.closed 2 5) "Lower bound ordering"

    testCase "Given a closed range When reading its endpoints Then both values are returned"
    <| fun () ->
      Expect.equal (Range.closed 1 5 |> Option.bind Range.tryGetClosed) (Some(1, 5)) "Closed endpoints"

    testCase "Given a singleton When reading its endpoints Then both values are equal"
    <| fun () ->
      Expect.equal (Range.singleton 7 |> Range.tryGetClosed) (Some(7, 7)) "Total singleton constructor"

    testTheory
      "Given non-inclusive or unbounded endpoints When reading closed endpoints Then none are returned"
      [
        LowerBounds.Exclusive 1, UpperBounds.Inclusive 5
        LowerBounds.Inclusive 1, UpperBounds.Exclusive 5
        LowerBounds.Unbounded, UpperBounds.Inclusive 5
        LowerBounds.Inclusive 1, UpperBounds.Unbounded
      ]
    <| fun (lower, upper) ->
      Expect.isNone (Range.create lower upper |> Option.bind Range.tryGetClosed) "Closed endpoint extraction"

    testCase "Given date-time endpoints When reading a closed range Then time precision is retained"
    <| fun () ->
      let first = DateTime(2026, 11, 15, 9, 30, 0)
      let last = DateTime(2026, 11, 15, 17, 45, 0)

      Expect.equal
        (Range.closed first last |> Option.bind Range.tryGetClosed)
        (Some(first, last))
        "Exact instants"

    testTheory "Given a half-open range When checking an included value Then it is a member" [ 1; 3; 4 ]
    <| fun value ->
      Expect.equal
        (Range.create (LowerBounds.Inclusive 1) (UpperBounds.Exclusive 5)
         |> contains value)
        (Some true)
        "Included value"

    testTheory "Given a half-open range When checking an excluded value Then it is not a member" [ 0; 5; 6 ]
    <| fun value ->
      Expect.equal
        (Range.create (LowerBounds.Inclusive 1) (UpperBounds.Exclusive 5)
         |> contains value)
        (Some false)
        "Excluded value"

    testCase "Given two unbounded ends When converting Then the interval is Unbounded"
    <| fun () ->
      let actual =
        Range.create (LowerBounds.Unbounded: LowerBounds<int>) UpperBounds.Unbounded
        |> Option.map Range.toInterval

      Expect.equal actual (Some Interval.Unbounded) "Explicit unbounded case"

    testCase "Given a singleton When converting Then the interval is Degenerate"
    <| fun () ->
      Expect.equal (Range.singleton 7 |> Range.toInterval) (Interval.Degenerate 7) "Explicit degenerate case"

    testCase "Given equal inclusive endpoints When converting a created range Then the interval is Degenerate"
    <| fun () ->
      Expect.equal
        (Range.closed 7 7 |> Option.map Range.toInterval)
        (Some(Interval.Degenerate 7))
        "Equal inclusive endpoints retain the single value"

    testTheory "Given bound markers When mapping values Then the markers are retained" [
      (LowerBounds.Inclusive 1, UpperBounds.Exclusive 3), (LowerBounds.Inclusive 11, UpperBounds.Exclusive 13)
      (LowerBounds.Exclusive 2, UpperBounds.Inclusive 5), (LowerBounds.Exclusive 12, UpperBounds.Inclusive 15)
      (LowerBounds.Unbounded, UpperBounds.Exclusive 3), (LowerBounds.Unbounded, UpperBounds.Exclusive 13)
      (LowerBounds.Inclusive 3, UpperBounds.Unbounded), (LowerBounds.Inclusive 13, UpperBounds.Unbounded)
    ]
    <| fun ((lower, upper), expected) ->
      let actual =
        Range.create lower upper
        |> Option.bind (Range.mapBounds ((+) 10))
        |> Option.map (fun range -> range.Lower, range.Upper)

      Expect.equal actual (Some expected) "Mapped bound kinds"

    testCase "Given unbounded ends When mapping Then the projection is not evaluated"
    <| fun () ->
      let projection (_: int) : string = failwith "No endpoint to project"

      let actual =
        Range.create LowerBounds.Unbounded UpperBounds.Unbounded
        |> Option.bind (Range.mapBounds projection)
        |> Option.map (fun range -> range.Lower, range.Upper)

      Expect.equal actual (Some(LowerBounds.Unbounded, UpperBounds.Unbounded)) "Unbounded payloads"

    testCase "Given date-time bounds When projecting times Then the result retains precision and markers"
    <| fun () ->
      let source =
        Range.create
          (LowerBounds.Inclusive(DateTime(2026, 11, 15, 9, 30, 0)))
          (UpperBounds.Exclusive(DateTime(2026, 11, 15, 17, 45, 0)))

      let actual =
        source
        |> Option.bind (Range.mapBounds (fun date -> date.TimeOfDay))
        |> Option.map (fun range -> range.Lower, range.Upper)

      Expect.equal
        actual
        (Some(LowerBounds.Inclusive(TimeSpan(9, 30, 0)), UpperBounds.Exclusive(TimeSpan(17, 45, 0))))
        "Type-changing projection"

    testCase "Given a projection that reverses endpoints When mapping Then None is returned"
    <| fun () ->
      Expect.isNone
        (Range.closed 1 5 |> Option.bind (Range.mapBounds (~-)))
        "Mapping cannot bypass creation validation"

    testTheory
      "Given an excluded finite endpoint When mapping both endpoints to one value Then None is returned"
      [
        LowerBounds.Exclusive 1, UpperBounds.Inclusive 5
        LowerBounds.Inclusive 1, UpperBounds.Exclusive 5
        LowerBounds.Exclusive 1, UpperBounds.Exclusive 5
      ]
    <| fun (lower, upper) ->
      Expect.isNone
        (Range.create lower upper |> Option.bind (Range.mapBounds (fun _ -> 0)))
        "Projection must not construct contradictory bounds"

  ]

[<Tests>]
let rangeProperties =
  testList "Range.Properties" [
    testPropertyWithConfig
      config
      "Construction rejects reversed finite endpoints and equal excluded endpoints while retaining accepted bounds"
    <| Prop.forAll endpoints (fun (first, last) ->
      for lower, upper in boundPairs first last do
        let actual =
          Range.create lower upper |> Option.map (fun range -> range.Lower, range.Upper)

        let expected =
          if
            isFinitePair (lower, upper)
            && (first > last || (first = last && hasExclusiveBound (lower, upper)))
          then
            None
          else
            Some(lower, upper)

        Expect.equal actual expected "Construction must preserve endpoint order and bound markers."

      true |> Prop.ofTestable |> classifyOrder first last)

    testPropertyWithConfig
      config
      "Finite membership includes only values between the endpoints with the selected inclusivity"
    <| Prop.forAll endpoints (fun (first, last) ->
      Prop.forAll integers (fun value ->
        let lower, upper = min first last, max first last

        for candidate in probes lower upper value do
          let inClosed = lower <= candidate && candidate <= upper

          let cases = [
            LowerBounds.Inclusive lower, UpperBounds.Inclusive upper, inClosed
            LowerBounds.Exclusive lower, UpperBounds.Inclusive upper, inClosed && candidate <> lower
            LowerBounds.Inclusive lower, UpperBounds.Exclusive upper, inClosed && candidate <> upper
            LowerBounds.Exclusive lower,
            UpperBounds.Exclusive upper,
            inClosed && candidate <> lower && candidate <> upper
          ]

          for first, last, expected in cases do
            let actual =
              Range.create first last
              |> Option.exists (fun range -> Range.inRange range candidate)

            Expect.equal actual expected "Endpoint exclusion must remove only that endpoint."

        true |> Prop.ofTestable |> classifyOrder first last))

    testPropertyWithConfig
      config
      "Conversion to Interval preserves membership and uses the appropriate explicit case"
    <| Prop.forAll endpoints (fun (first, last) ->
      Prop.forAll integers (fun value ->
        for range in ranges first last do
          let interval = Range.toInterval range

          let expectedCase =
            match range.Lower, range.Upper with
            | LowerBounds.Unbounded, UpperBounds.Unbounded -> "unbounded"
            | LowerBounds.Inclusive lower, UpperBounds.Inclusive upper when lower = upper -> "degenerate"
            | _ -> "bounded"

          let actualCase =
            match interval with
            | Interval.Bounded _ -> "bounded"
            | Interval.Degenerate _ -> "degenerate"
            | Interval.Unbounded -> "unbounded"

          Expect.equal actualCase expectedCase "Canonical interval case."

          for candidate in probes first last value do
            Expect.equal
              (Interval.inInterval interval candidate)
              (Range.inRange range candidate)
              "Conversion must not change membership."

        true |> Prop.ofTestable |> classifyOrder first last))

    testPropertyWithConfig
      config
      "An increasing type-changing projection preserves membership across all bound combinations"
    <| Prop.forAll endpoints (fun (first, last) ->
      Prop.forAll integers (fun offset ->
        let project value = int64 value + int64 offset

        for range in ranges first last do
          let mapped =
            match Range.mapBounds project range with
            | Some mapped -> mapped
            | None -> failtest "An increasing projection cannot reverse ordered endpoints."

          for candidate in probes first last offset do
            Expect.equal
              (Range.inRange mapped (project candidate))
              (Range.inRange range candidate)
              "Translation must retain inclusive exclusive and unbounded membership."

        true |> Prop.ofTestable |> classifyOrder first last))

    testPropertyWithConfig
      config
      "A decreasing projection rejects distinct finite endpoints without reordering them"
    <| Prop.forAll endpoints (fun (first, last) ->
      for range in ranges first last do
        let mapped = Range.mapBounds (fun value -> -(int64 value)) range
        let reverses = first <> last && isFinitePair (range.Lower, range.Upper)
        Expect.equal mapped.IsNone reverses "Only a pair of distinct finite endpoints becomes invalid."

      true |> Prop.ofTestable |> classifyOrder first last)

    testPropertyWithConfig
      config
      "Projecting DateTime bounds to TimeOfDay validates the projected order and retains tick precision"
    <| Prop.forAll endpoints (fun (first, last) ->
      let instant value =
        DateTime(2000, 1, 1).AddTicks(int64 value * 10007L)

      let start, finish = instant (min first last), instant (max first last)

      let source =
        requireRange (LowerBounds.Inclusive start) (UpperBounds.Inclusive finish)

      let actual =
        source
        |> Range.mapBounds (fun value -> value.TimeOfDay)
        |> Option.bind Range.tryGetClosed

      let expected =
        if start.TimeOfDay <= finish.TimeOfDay then
          Some(start.TimeOfDay, finish.TimeOfDay)
        else
          None

      Expect.equal actual expected "Projection maps endpoints; it does not sort or round them."

      true
      |> Prop.ofTestable
      |> Prop.classify (start.TimeOfDay > finish.TimeOfDay) "time-of-day order reversed"
      |> classifyOrder first last)
  ]
