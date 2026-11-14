module Weave.Tests.Unit.DateTimeGuideTests

open System
open System.Diagnostics
open System.IO
open Expecto
open Weave.Docs

type private Evaluation = {
  ExitCode: int
  Output: string
  Errors: string
}

// Execute the copyable source, not a second implementation of the scheduling rule.
let private evaluate expression source =
  let references =
    [
      "WebSharper.Core.dll"
      "WebSharper.JavaScript.dll"
      "WebSharper.StdLib.dll"
      "WebSharper.UI.dll"
      "Weave.dll"
    ]
    |> List.map (fun name -> sprintf "#r @\"%s\"" (Path.Combine(AppContext.BaseDirectory, name)))
    |> String.concat "\n"

  let fileName = Guid.NewGuid().ToString("N") |> sprintf "weave-guide-%s.fsx"
  let path = Path.Combine(Path.GetTempPath(), fileName)
  let program = references + "\n" + source + "\nprintf \"%A\" (" + expression + ")\n"
  File.WriteAllText(path, program)

  try
    let start =
      ProcessStartInfo(
        "dotnet",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
      )

    for argument in [ "fsi"; "--exec"; "--quiet"; "--warnaserror:20"; path ] do
      start.ArgumentList.Add argument

    use evaluator = Process.Start start
    let stdout = evaluator.StandardOutput.ReadToEndAsync()
    let stderr = evaluator.StandardError.ReadToEndAsync()

    if not (evaluator.WaitForExit 30000) then
      evaluator.Kill(entireProcessTree = true)
      evaluator.WaitForExit()
      failtest "The copied guide example did not finish within 30 seconds."

    {
      ExitCode = evaluator.ExitCode
      Output = stdout.GetAwaiter().GetResult().Trim()
      Errors = stderr.GetAwaiter().GetResult()
    }
  finally
    File.Delete path

let private expectOutput expected actual =
  Expect.equal actual.ExitCode 0 actual.Errors

  Expect.equal
    actual.Output
    (sprintf "%A" expected)
    "The copied example should produce the documented result."

let private availableDays = [ 1; 2; 3; 4; 7; 15; 16; 17; 19; 20; 21 ]

let private allowedDays =
  "[1..22] |> List.filter (fun day -> DateAvailability.allows availability (date day))"

[<Tests>]
let dateTimeGuideTests =
  testList "DateTimeGuide" [
    testCase "Given the copied booking policy When checking dates Then reservations are excluded"
    <| fun () ->
      DateTimeGuideSource.bookingRule ()
      |> evaluate allowedDays
      |> expectOutput availableDays

    testCase
      "Given the copied manual scheduling rule When checking dates Then it matches the documented schedule"
    <| fun () ->
      DateTimeGuideSource.bookingRule ()
      |> evaluate "[1..22] |> List.filter (fun day -> isAvailableManually (date day))"
      |> expectOutput availableDays

    testCase
      "Given the copied booking policy When its exclusion is removed Then the documented result changes"
    <| fun () ->
      let source = DateTimeGuideSource.bookingRule ()
      let changed = source.Replace("|> DateAvailability.excluding reservations", "")
      Expect.notEqual changed source "The negative control must remove an actual exclusion."
      let actual = changed |> evaluate allowedDays
      expectOutput [ 1; 2; 3; 4; 5; 6; 7; 15; 16; 17; 18; 19; 20; 21 ] actual

      Expect.notEqual
        actual.Output
        (sprintf "%A" availableDays)
        "The original expectation must reject this change."

    testCase
      "Given the copied calendar projection When filtering requests Then time portions do not exclude the first day"
    <| fun () ->
      DateTimeGuideSource.calendarRange ()
      |> evaluate "acceptedDates |> List.map (fun day -> day.Day)"
      |> expectOutput [ 1; 4; 7 ]

    testCase
      "Given the copied rescheduling pipeline When filtering requests Then dates use the shifted window"
    <| fun () ->
      DateTimeGuideSource.rescheduledInterval ()
      |> evaluate "acceptedDates |> Option.map (List.map (fun day -> day.Day))"
      |> expectOutput (Some [ 8; 10; 13 ])

    testCase
      "Given the copied rescheduling pipeline When checking endpoints Then the exclusive end stays excluded"
    <| fun () ->
      DateTimeGuideSource.rescheduledInterval ()
      |> evaluate
        "shiftedWindow |> Option.map (fun interval -> Interval.inInterval interval (date 8), Interval.inInterval interval (date 14))"
      |> expectOutput (Some(true, false))

    testCase
      "Given the copied rescheduling pipeline When reading bounds Then both positions and markers are preserved"
    <| fun () ->
      DateTimeGuideSource.rescheduledInterval ()
      |> evaluate
        "shiftedBounds |> Option.map (fun range -> range.Lower = LowerBounds.Inclusive (date 8) && range.Upper = UpperBounds.Exclusive (date 14))"
      |> expectOutput (Some true)

    testCase
      "Given the copied time policy When checking bounds Then the picker and analysis use the same schedule"
    <| fun () ->
      DateTimeGuideSource.timeRule ()
      |> evaluate
        "[TimeSpan(8,59,0); TimeSpan(9,0,0); TimeSpan(16,59,0); TimeSpan(17,0,0)] |> List.map (TimeAvailability.allows availability)"
      |> expectOutput [ false; true; true; false ]

    testCase
      "Given the copied overnight policy When checking both sides of midnight Then noon remains unavailable"
    <| fun () ->
      DateTimeGuideSource.overnightTimeRule ()
      |> evaluate "acceptsLate, acceptsEarly, acceptsNoon"
      |> expectOutput (true, true, false)

    testCase
      "Given the copied interval edge cases When checking outcomes Then contradictory bounds are rejected and single values remain distinct from unbounded"
    <| fun () ->
      DateTimeGuideSource.edgeCases ()
      |> evaluate "acceptsEvent, acceptsSameDay, acceptsUnbounded, reversed.IsNone, contradictory.IsNone"
      |> expectOutput (true, true, true, true, true)

    testTheory
      "Given a copied interval rule When checking dates Then membership agrees with the manual predicate"
      [ "window"; "handover"; "event"; "unbounded" ]
    <| fun name ->
      DateTimeGuideSource.ruleDefinition name
      |> evaluate "[1..22] |> List.forall (fun day -> allowsManually (date day) = allowsDate (date day))"
      |> expectOutput true
  ]
