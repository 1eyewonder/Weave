module Weave.Tests.Unit.ValidationTests

open Expecto
open Weave

[<Tests>]
let validationTests =
  testList "Validation" [
    testCase "comparisons distinguish inclusive and exclusive boundaries"
    <| fun () ->
      let cases = [
        Validate.atLeast 0 "Low", [ false; true; true ]
        Validate.atMost 0 "High", [ true; true; false ]
        Validate.greaterThan 0 "Low", [ false; false; true ]
        Validate.lessThan 0 "High", [ true; false; false ]
        Validate.equalTo 0 "Different", [ false; true; false ]
        Validate.notEqualTo 0 "Same", [ true; false; true ]
      ]

      for rule, expected in cases do
        Expect.equal
          ([ -1; 0; 1 ] |> List.map (rule >> List.isEmpty))
          expected
          "Below, at, and above the boundary"

    testCase "ranges include both endpoints and reject inverted configuration"
    <| fun () ->
      let rule = Validate.between -2 2 "Out of range"

      for value in [ -2; 0; 2 ] do
        Expect.isEmpty (rule value) "Inclusive endpoints and interior"

      for value in [ -3; 3 ] do
        Expect.equal (rule value) [ "Out of range" ] "Outside either endpoint"

      Expect.isEmpty (Validate.between 2 2 "Exactly two" 2) "A single-value range is valid"

      Expect.throwsT<System.ArgumentException>
        (fun () -> Validate.between 2 1 "Bad range" |> ignore)
        "Inverted bounds are configuration errors"

    testCase "comparison rules work with floats and decimals"
    <| fun () ->
      Expect.isEmpty (Validate.between 0.5 1.5 "Float range" 1.5) "Float upper boundary"
      Expect.equal (Validate.greaterThan 0.5 "Too small" 0.5) [ "Too small" ] "Strict float comparison"
      Expect.isEmpty (Validate.between 0.1M 0.3M "Decimal range" (0.1M + 0.2M)) "Exact decimal arithmetic"
      Expect.equal (Validate.atMost 10M "Budget" 10.01M) [ "Budget" ] "Decimal above maximum"

    testCase "ordered float comparisons reject NaN"
    <| fun () ->
      for rule in
        [
          Validate.atLeast 0.0 "Invalid"
          Validate.atMost 1.0 "Invalid"
          Validate.greaterThan 0.0 "Invalid"
          Validate.lessThan 1.0 "Invalid"
          Validate.between 0.0 1.0 "Invalid"
        ] do
        Expect.equal (rule System.Double.NaN) [ "Invalid" ] "NaN cannot satisfy an ordered bound"

      Expect.equal
        (Validate.atLeast 0.0f "Invalid" System.Single.NaN)
        [ "Invalid" ]
        "Float32 NaN is also rejected"

      Expect.equal
        (Validate.atLeast System.Double.NaN "Invalid" 1.0)
        [ "Invalid" ]
        "NaN is not a valid lower bound"

      Expect.equal
        (Validate.atMost System.Double.NaN "Invalid" 1.0)
        [ "Invalid" ]
        "NaN is not a valid upper bound"

      Expect.throwsT<System.ArgumentException>
        (fun () -> Validate.between 0.0 System.Double.NaN "Invalid" |> ignore)
        "NaN range bounds are configuration errors"

    testCase "length projections validate lists arrays and sets at their boundaries"
    <| fun () ->
      let listRule = Validate.minLengthBy List.length 2 "Choose two"
      Expect.equal (listRule [ 1 ]) [ "Choose two" ] "List below minimum"
      Expect.isEmpty (listRule [ 1; 2 ]) "List at minimum"
      let arrayRule = Validate.maxLengthBy Array.length 2 "Choose at most two"
      Expect.isEmpty (arrayRule [| 1; 2 |]) "Array at maximum"
      Expect.equal (arrayRule [| 1; 2; 3 |]) [ "Choose at most two" ] "Array above maximum"
      let setRule = Validate.minLengthBy Set.count 2 "Two distinct choices"

      Expect.equal
        (setRule (Set.ofList [ 1; 1 ]))
        [ "Two distinct choices" ]
        "Set cardinality counts distinct choices"

      Expect.isEmpty (setRule (Set.ofList [ 1; 2 ])) "Set at minimum"
      Expect.isEmpty (Validate.maxLengthBy List.length 0 "Empty only" []) "Empty collection at zero maximum"

    testCase "projection composes arithmetic and collection bounds"
    <| fun () ->
      let totalRule =
        Validate.on (fun (price, quantity) -> price * decimal quantity) (Validate.atMost 100M "Over budget")

      Expect.isEmpty (totalRule (25M, 4)) "Total at budget"
      Expect.equal (totalRule (25M, 5)) [ "Over budget" ] "Total exceeds budget"
      let lengthRule = Validate.on String.length (Validate.between 2 4 "Length")
      Expect.isEmpty (lengthRule "abcd") "Projected upper endpoint"
      Expect.equal (lengthRule "a") [ "Length" ] "Projected value below range"
      let even = Validate.custom (fun value -> value % 2 = 0) "Must be even"
      Expect.isEmpty (even 4) "Arithmetic predicates remain supported"
      Expect.equal (even 3) [ "Must be even" ] "Odd value fails"

    testCase "required rejects null, empty, and whitespace but preserves user text"
    <| fun () ->
      let rule = Validate.required "Required"

      for value in [ null; ""; " \t\r\n" ] do
        Expect.equal (rule value) [ "Required" ] "Blank input is invalid"

      Expect.isEmpty (rule "  Ada  ") "Nonblank input is valid"

    testCase "length boundaries are inclusive and null is safe"
    <| fun () ->
      let rule =
        Validate.all [ Validate.minLength 2 "Too short"; Validate.maxLength 4 "Too long" ]

      Expect.equal (rule null) [ "Too short" ] "Null is below the minimum"
      Expect.equal (rule "a") [ "Too short" ] "Below minimum"

      for value in [ "ab"; "abcd" ] do
        Expect.isEmpty (rule value) "Inclusive boundaries"

      Expect.equal (rule "abcde") [ "Too long" ] "Above maximum"

    testCase "all accumulates messages in declaration order and runs every rule"
    <| fun () ->
      let mutable calls = 0

      let second value =
        calls <- calls + 1
        Validate.minLength 3 "Short" value

      Expect.equal
        (Validate.all [ Validate.required "Required"; second ] "")
        [ "Required"; "Short" ]
        "Both errors retained"

      Expect.equal calls 1 "Second rule ran despite the first failure"
      Expect.isEmpty (Validate.all [] "anything") "No constraints accepts any input"

    testCase "andThen guards a dependent rule"
    <| fun () ->
      let mutable calls = 0

      let next value =
        calls <- calls + 1
        Validate.minLength 3 "Short" value

      let rule = Validate.required "Required" |> Validate.andThen next
      Expect.equal (rule null) [ "Required" ] "Guard failed"
      Expect.equal calls 0 "Dependent rule was not run"
      Expect.equal (rule "a") [ "Short" ] "Dependent rule fails"
      Expect.isEmpty (rule "Ada") "Both rules pass"
      Expect.equal calls 2 "Dependent rule ran for each nonblank input"

    testCase "optional validates present values and skips absent values"
    <| fun () ->
      let rule = Validate.optional (Validate.custom (fun age -> age >= 18) "Adults only")
      Expect.isEmpty (rule None) "Absent optional value"
      Expect.equal (rule (Some 17)) [ "Adults only" ] "Present invalid value"
      Expect.isEmpty (rule (Some 18)) "Present valid value"
      Expect.equal (Validate.isSome "Choose" None) [ "Choose" ] "Required option"
      Expect.isEmpty (Validate.isSome "Choose" (Some "")) "Presence is independent of content"

    testCase "required consent does not accept false"
    <| fun () ->
      Expect.equal (Validate.isTrue "Consent required" false) [ "Consent required" ] "Unchecked consent"
      Expect.isEmpty (Validate.isTrue "Consent required" true) "Checked consent"
  ]
