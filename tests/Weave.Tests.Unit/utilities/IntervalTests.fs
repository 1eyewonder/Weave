module Weave.Tests.Unit.IntervalTests

open System
open Expecto
open FsCheck
open Weave.Tests.Unit.Generators
open Weave

let private contains value interval =
  interval |> Option.map (fun bounds -> value |> Interval.inInterval bounds)

[<Tests>]
let intervalTests =
  testList "Interval" [
    testTheory "Given inclusive bounds When checking an endpoint Then it is included" [ 1; 5 ]
    <| fun value -> Expect.equal (Interval.closed 1 5 |> contains value) (Some true) "Inclusive endpoints"

    testTheory "Given exclusive bounds When checking an endpoint Then it is excluded" [ 1; 5 ]
    <| fun value ->
      Expect.equal
        (Interval.create (LowerBounds.Exclusive 1) (UpperBounds.Exclusive 5)
         |> contains value)
        (Some false)
        "Exclusive endpoints"

    testCase "Given exclusive bounds When checking an interior value Then it is included"
    <| fun () ->
      Expect.equal
        (Interval.create (LowerBounds.Exclusive 1) (UpperBounds.Exclusive 5)
         |> contains 3)
        (Some true)
        "Interior value"

    testTheory "Given an unbounded interval When checking a value Then it is included" [
      Int32.MinValue
      0
      Int32.MaxValue
    ]
    <| fun value -> Expect.isTrue (value |> Interval.inInterval Interval.Unbounded) "Unbounded interval"

    testTheory "Given an open lower end When checking a value below the upper end Then it is included" [
      -100
      0
      5
    ]
    <| fun value ->
      Expect.equal
        (Interval.create LowerBounds.Unbounded (UpperBounds.Inclusive 5)
         |> contains value)
        (Some true)
        "Open lower end"

    testTheory "Given an open upper end When checking a value above the lower end Then it is included" [
      5
      6
      Int32.MaxValue
    ]
    <| fun value ->
      Expect.equal
        (Interval.create (LowerBounds.Inclusive 5) UpperBounds.Unbounded
         |> contains value)
        (Some true)
        "Open upper end"

    testCase "Given a degenerate interval When checking its value Then it is included"
    <| fun () -> Expect.isTrue (7 |> Interval.inInterval (Interval.Degenerate 7)) "Single value"

    testTheory "Given a degenerate interval When checking another value Then it is excluded" [ 6; 8 ]
    <| fun value ->
      Expect.isFalse (value |> Interval.inInterval (Interval.Degenerate 7)) "Only the single value"

    testCase "Given a degenerate interval When reading bounds Then inclusive equal endpoints are returned"
    <| fun () ->
      Expect.equal
        (Interval.tryGetBounds (Interval.Degenerate 7))
        (Some(Range.singleton 7))
        "Singleton bounds"

    testCase "Given an unbounded interval When reading bounds Then no range is returned"
    <| fun () -> Expect.isNone (Interval.tryGetBounds (Interval.Unbounded: Interval<int>)) "No bounds"

    testCase "Given the singleton constructor When creating an interval Then its case is Degenerate"
    <| fun () -> Expect.equal (Interval.singleton 7) (Interval.Degenerate 7) "Explicit case"

    testCase "Given reversed endpoints When creating a closed interval Then None is returned"
    <| fun () -> Expect.isNone (Interval.closed 5 1) "Invalid ordering"

    testCase "Given reversed explicit bounds When creating an interval Then None is returned"
    <| fun () ->
      Expect.isNone
        (Interval.create (LowerBounds.Exclusive 5) (UpperBounds.Inclusive 1))
        "Validation is shared with Range.create"

    testTheory "Given equal endpoints with exclusion When creating an interval Then None is returned" [
      LowerBounds.Exclusive 5, UpperBounds.Inclusive 5
      LowerBounds.Inclusive 5, UpperBounds.Exclusive 5
      LowerBounds.Exclusive 5, UpperBounds.Exclusive 5
    ]
    <| fun (lower, upper) ->
      Expect.isNone (Interval.create lower upper) "Contradictory bounds are not unbounded"

    testTheory
      "Given a finite excluded endpoint When projection collapses both endpoints Then None is returned"
      [
        LowerBounds.Exclusive 1, UpperBounds.Inclusive 5
        LowerBounds.Inclusive 1, UpperBounds.Exclusive 5
        LowerBounds.Exclusive 1, UpperBounds.Exclusive 5
      ]
    <| fun (lower, upper) ->
      Expect.isNone
        (Interval.create lower upper |> Option.bind (Interval.mapBounds (fun _ -> 0)))
        "Projection shares constructor validation"

    testCase "Given a degenerate instant When checking a different instant on that date Then it is excluded"
    <| fun () ->
      let instant = DateTime(2026, 11, 15, 13, 30, 0)

      Expect.isFalse
        (instant.AddMinutes 1.0 |> Interval.inInterval (Interval.Degenerate instant))
        "Time precision"

    testCase "Given a time interval When checking its exclusive upper endpoint Then it is excluded"
    <| fun () ->
      let interval =
        Interval.create
          (LowerBounds.Inclusive(TimeSpan.FromHours 9.0))
          (UpperBounds.Exclusive(TimeSpan.FromHours 17.0))

      Expect.equal (interval |> contains (TimeSpan.FromHours 17.0)) (Some false) "Half-open time interval"

    testCase "Given a degenerate date-time interval When projecting its year Then the result stays Degenerate"
    <| fun () ->
      let actual =
        Interval.Degenerate(DateTime(2026, 11, 15, 13, 30, 0))
        |> Interval.mapBounds (fun date -> date.Year)

      Expect.equal actual (Some(Interval.Degenerate 2026)) "Type-changing singleton projection"

    testCase "Given an unbounded interval When mapping Then the projection is not evaluated"
    <| fun () ->
      let projection (_: int) : string = failwith "No endpoint to project"

      Expect.equal
        ((Interval.Unbounded: Interval<int>) |> Interval.mapBounds projection)
        (Some Interval.Unbounded)
        "Unbounded case"

    testCase "Given a bounded interval When mapped endpoints coincide Then its case remains Bounded"
    <| fun () ->
      let actual = Interval.closed 1 5 |> Option.bind (Interval.mapBounds (fun _ -> 7))
      Expect.equal actual (Some(Interval.Bounded(Range.singleton 7))) "No canonicalization"

    testCase "Given a bounded interval with unbounded ends When mapping Then its case remains Bounded"
    <| fun () ->
      let actual =
        Interval.create LowerBounds.Unbounded UpperBounds.Unbounded
        |> Option.bind (Interval.mapBounds string<int>)

      let expected: Interval<string> option =
        Interval.create LowerBounds.Unbounded UpperBounds.Unbounded

      Expect.equal actual expected "Explicit bounded case"

    testCase "Given mixed markers When projecting another value type Then the markers are retained"
    <| fun () ->
      let actual =
        Interval.create (LowerBounds.Exclusive 1) (UpperBounds.Inclusive 5)
        |> Option.bind (Interval.mapBounds float)
        |> Option.bind Interval.tryGetBounds
        |> Option.map (fun range -> range.Lower, range.Upper)

      Expect.equal actual (Some(LowerBounds.Exclusive 1.0, UpperBounds.Inclusive 5.0)) "Mapped bound kinds"

    testCase "Given a projection that reverses bounds When mapping an interval Then None is returned"
    <| fun () ->
      Expect.isNone
        (Interval.closed 1 5 |> Option.bind (Interval.mapBounds (~-)))
        "No invalid bounded payload"

    testCase "Given an interval When matching an included value Then WithinInterval matches"
    <| fun () ->
      let bounds = Interval.Bounded(Range.singleton 7)

      let (|Within|Outside|) value =
        Interval.(|WithinInterval|OutsideInterval|) bounds value

      let outcome =
        match 7 with
        | Within -> true
        | Outside -> false

      Expect.isTrue outcome "Membership active pattern"
  ]

[<Tests>]
let intervalProperties =
  testList "Interval.Properties" [
    testPropertyWithConfig
      config
      "Extracting bounds preserves membership for every bounded and singleton interval"
    <| Prop.forAll endpoints (fun (first, last) ->
      Prop.forAll integers (fun value ->
        for interval in intervals first last do
          match interval, Interval.tryGetBounds interval with
          | Interval.Unbounded, None -> ()
          | Interval.Unbounded, Some _ -> failtest "Unbounded has no explicit range."
          | (Interval.Bounded _ | Interval.Degenerate _), None ->
            failtest "Bounded and singleton intervals must expose bounds."
          | (Interval.Bounded _ | Interval.Degenerate _), Some bounds ->
            for candidate in probes first last value do
              Expect.equal
                (Range.inRange bounds candidate)
                (Interval.inInterval interval candidate)
                "Extracted bounds must describe the same members."

        true |> Prop.ofTestable |> classifyOrder first last))

    testPropertyWithConfig
      config
      "Mapping to a constant rejects contradictory bounds and preserves accepted interval cases"
    <| Prop.forAll endpoints (fun (first, last) ->
      Prop.forAll integers (fun value ->
        for interval in intervals first last do
          let mapped = Interval.mapBounds (fun _ -> value) interval

          let rejects =
            match interval with
            | Interval.Bounded bounds ->
              isFinitePair (bounds.Lower, bounds.Upper)
              && hasExclusiveBound (bounds.Lower, bounds.Upper)
            | Interval.Unbounded
            | Interval.Degenerate _ -> false

          Expect.equal mapped.IsNone rejects "Only excluded finite endpoints become contradictory."

          match interval, mapped with
          | Interval.Unbounded, Some Interval.Unbounded -> ()
          | Interval.Degenerate _, Some(Interval.Degenerate actual) ->
            Expect.equal actual value "The singleton payload must be projected."
          | Interval.Bounded _, Some(Interval.Bounded bounds) ->
            Expect.equal
              (Range.inRange bounds value)
              (not (hasExclusiveBound (bounds.Lower, bounds.Upper)))
              "Preserved markers determine membership at the projected endpoint."
          | Interval.Bounded _, None -> () // Rejection is checked independently above.
          | _ -> failtest "Accepted mappings must preserve Bounded Degenerate and Unbounded cases."

        true |> Prop.ofTestable |> classifyOrder first last))

    testPropertyWithConfig
      config
      "Increasing projections preserve interval membership including singleton payloads"
    <| Prop.forAll endpoints (fun (first, last) ->
      Prop.forAll integers (fun offset ->
        let project value = int64 value + int64 offset

        for interval in intervals first last do
          let mapped =
            match Interval.mapBounds project interval with
            | Some mapped -> mapped
            | None -> failtest "An increasing projection must accept each interval case."

          for candidate in probes first last offset do
            Expect.equal
              (Interval.inInterval mapped (project candidate))
              (Interval.inInterval interval candidate)
              "Projected singleton and bound values must track the same member."

        true |> Prop.ofTestable |> classifyOrder first last))
  ]
