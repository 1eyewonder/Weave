module Weave.Tests.Unit.Generators

open System
open Expecto
open FsCheck
open Weave

let config = { FsCheckConfig.defaultConfig with maxTest = 500 }

// Full-width values plus frequent boundaries; use the standard numeric/tuple shrinkers.
let integerGenerator =
  Gen.frequency [
    // FsCheck 2's choose overflows for the full Int32 span; split it into safe halves.
    3, Gen.oneof [ Gen.choose (Int32.MinValue + 1, -1); Gen.choose (0, Int32.MaxValue - 1) ]
    2, Gen.elements [ Int32.MinValue; -1; 0; 1; Int32.MaxValue ]
  ]

let integers = Arb.fromGenShrink (integerGenerator, Arb.shrink)

let endpoints =
  let generator = gen {
    let! first = integerGenerator
    let! last = Gen.frequency [ 1, Gen.constant first; 3, integerGenerator ]
    return first, last
  }

  let shrink (first, last) = seq {
    // Shrink equal endpoints together so singleton/rejected-bound failures can reach (0, 0).
    if first = last then
      for value in Arb.shrink first do
        yield value, value

    yield! Arb.shrink (first, last)
  }

  Arb.fromGenShrink (generator, shrink)

let classifyOrder first last property =
  property
  |> Prop.classify (first < last) "ascending"
  |> Prop.classify (first = last) "equal"
  |> Prop.classify (first > last) "reversed"
  |> Prop.classify
    (first = Int32.MinValue
     || last = Int32.MinValue
     || first = Int32.MaxValue
     || last = Int32.MaxValue)
    "integer boundary"

let boundPairs first last = [
  for lower in
    [
      LowerBounds.Inclusive first
      LowerBounds.Exclusive first
      LowerBounds.Unbounded
    ] do
    for upper in
      [
        UpperBounds.Inclusive last
        UpperBounds.Exclusive last
        UpperBounds.Unbounded
      ] do
      lower, upper
]

let isFinitePair =
  function
  | LowerBounds.Unbounded, _
  | _, UpperBounds.Unbounded -> false
  | (LowerBounds.Inclusive _ | LowerBounds.Exclusive _), (UpperBounds.Inclusive _ | UpperBounds.Exclusive _) ->
    true

let hasExclusiveBound =
  function
  | LowerBounds.Exclusive _, _
  | _, UpperBounds.Exclusive _ -> true
  | _ -> false

let requireRange lower upper =
  match Range.create lower upper with
  | Some range -> range
  | None -> failtest "Valid fixture endpoints must construct a range."

let ranges first last =
  boundPairs (min first last) (max first last)
  |> List.choose (fun (lower, upper) ->
    let bounds = lower, upper

    if first <> last || not (isFinitePair bounds && hasExclusiveBound bounds) then
      Some(requireRange lower upper)
    else
      None)

let probes first last value = [
  yield first
  yield last
  yield value
  yield Int32.MinValue
  yield Int32.MaxValue
  if first > Int32.MinValue then
    yield first - 1
  if first < Int32.MaxValue then
    yield first + 1
  if last > Int32.MinValue then
    yield last - 1
  if last < Int32.MaxValue then
    yield last + 1
]

let intervals first last = [
  yield! ranges first last |> List.map Interval.Bounded
  Interval.Degenerate first
  Interval.Unbounded
]

type CalendarWindows = {
  Start: int
  Left: int
  Overlap: int
  Right: int
}

let calendarWindows =
  let generator = gen {
    let! start = Gen.choose (-36500, 36500)
    let! left = Gen.choose (0, 14)
    let! overlap = Gen.choose (0, 14)
    let! right = Gen.choose (0, 14)

    return {
      Start = start
      Left = left
      Overlap = overlap
      Right = right
    }
  }

  let shrinkLength value =
    Arb.shrink value |> Seq.filter (fun value -> value >= 0 && value <= 14)

  let shrink input = seq {
    for value in Arb.shrink input.Start do
      yield { input with Start = value }

    for value in shrinkLength input.Left do
      yield { input with Left = value }

    for value in shrinkLength input.Overlap do
      yield { input with Overlap = value }

    for value in shrinkLength input.Right do
      yield { input with Right = value }
  }

  Arb.fromGenShrink (generator, shrink)

type ClockWindows = {
  StartMinute: int
  LeftMinutes: int
  OverlapMinutes: int
  RightMinutes: int
}

let clockWindows =
  let generator = gen {
    let! start =
      Gen.frequency [ 3, Gen.choose (0, 1400); 1, Gen.elements [ 0; 540; 720; 1400 ] ]

    let! left = Gen.choose (0, 10)
    let! overlap = Gen.choose (0, 10)
    let! right = Gen.choose (0, 10)

    return {
      StartMinute = start
      LeftMinutes = left
      OverlapMinutes = overlap
      RightMinutes = right
    }
  }

  let shrink input = seq {
    for start in
      Arb.shrink input.StartMinute
      |> Seq.filter (fun value -> value >= 0 && value <= 1400) do
      yield { input with StartMinute = start }

    for value in
      Arb.shrink input.LeftMinutes
      |> Seq.filter (fun value -> value >= 0 && value <= 10) do
      yield { input with LeftMinutes = value }

    for value in
      Arb.shrink input.OverlapMinutes
      |> Seq.filter (fun value -> value >= 0 && value <= 10) do
      yield { input with OverlapMinutes = value }

    for value in
      Arb.shrink input.RightMinutes
      |> Seq.filter (fun value -> value >= 0 && value <= 10) do
      yield { input with RightMinutes = value }
  }

  Arb.fromGenShrink (generator, shrink)

let clockTimes =
  let generator = gen {
    let! seconds = Gen.choose (-1, 86400)
    let! ticks = Gen.choose (0, 9999999)
    return TimeSpan.FromTicks(int64 seconds * TimeSpan.TicksPerSecond + int64 ticks)
  }

  let boundaries = [
    TimeSpan.MinValue
    TimeSpan.FromTicks -1L
    TimeSpan.Zero
    TimeSpan.FromTicks(TimeSpan.TicksPerDay - 1L)
    TimeSpan.FromDays 1.0
    TimeSpan.MaxValue
  ]

  let shrink (time: TimeSpan) =
    Arb.shrink time.Ticks |> Seq.map TimeSpan.FromTicks

  Arb.fromGenShrink (Gen.frequency [ 3, generator; 1, Gen.elements boundaries ], shrink)

// Include equal endpoints explicitly, and preserve them while shrinking.
let clockEndpoints =
  let generator = gen {
    let! first = Arb.toGen clockTimes
    let! last = Gen.frequency [ 1, Gen.constant first; 3, Arb.toGen clockTimes ]
    return first, last
  }

  let shrink (first: TimeSpan, last: TimeSpan) = seq {
    if first = last then
      for ticks in Arb.shrink first.Ticks do
        yield TimeSpan.FromTicks ticks, TimeSpan.FromTicks ticks

    for firstTicks, lastTicks in Arb.shrink (first.Ticks, last.Ticks) do
      yield TimeSpan.FromTicks firstTicks, TimeSpan.FromTicks lastTicks
  }

  Arb.fromGenShrink (generator, shrink)
