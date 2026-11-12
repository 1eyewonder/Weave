namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module FormExamples =

  let private personExample () =
    let content =
      let name = FormField.create ("", rules = [ Validate.required "Enter your name." ])

      let age =
        FormField.create (18, rules = [ Validate.atLeast 18 "You must be at least 18." ])

      let person = Form.model {
        let! name = name
        and! age = age
        return name.Trim(), age
      }

      let submitted = Var.Create ""

      Form.create (
        person,
        (fun value -> Var.Set submitted (sprintf "Submitted profile: %s, age %d." (fst value) (snd value))),
        [
          TextField.singleLine (name, labelText = View.Const "Name")
          NumericField.create (age, labelText = View.Const "Age")

          div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
            Button.primary (
              text "Save profile",
              onClick = ignore,
              attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
            )
            Button.create (
              text "Reset",
              onClick = ignore,
              attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
            )
          ]
          p [ Attr.Create "role" "status" ] [ textView submitted.View ]
        ],
        attrs = [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          Gap.All.g3
          on.reset (fun _ _ -> Var.Set submitted "")
        ]
      )

    Helpers.codeSampleSection
      "A simple form"
      (Helpers.bodyText
        "Declare fields and their rules, combine their values into a model, and pass the fields directly to controls. Submit an empty name to reveal its error. Reset restores the initial values. These actions only update this demo.")
      (div [ Attr.Style "max-inline-size" "32rem"; Attr.Style "margin-inline" "auto" ] [ content ])
      """let name =
  FormField.create ("", rules = [ Validate.required "Enter your name." ])

let age =
  FormField.create (
    18,
    rules = [ Validate.atLeast 18 "You must be at least 18." ]
  )

let person =
  Form.model {
    let! name = name
    and! age = age
    return name.Trim(), age
  }

let submitted = Var.Create ""

Form.create (
  person,
  (fun value ->
    Var.Set
      submitted
      (sprintf "Submitted profile: %s, age %d." (fst value) (snd value))),
  [
    TextField.singleLine (name, labelText = View.Const "Name")
    NumericField.create (age, labelText = View.Const "Age")

    div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
      Button.primary (
        text "Save profile",
        onClick = ignore,
        attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
      )
      Button.create (
        text "Reset",
        onClick = ignore,
        attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
      )
    ]
    p [ Attr.Create "role" "status" ] [ textView submitted.View ]
  ],
  attrs = [
    Flex.Flex.allSizes
    FlexDirection.Column.allSizes
    Gap.All.g3
    on.reset (fun _ _ -> Var.Set submitted "")
  ]
)"""

  let private rulesExample () =
    let content =
      let password =
        FormField.create (
          "",
          rules = [
            Validate.required "Enter a password."
            |> Validate.andThen (Validate.minLength 8 "Use at least eight characters.")
          ]
        )

      let confirmationRules =
        password.Value.View
        |> View.Map(fun expected -> [
          Validate.required "Confirm your password."
          |> Validate.andThen (Validate.equalTo expected "Passwords must match.")
        ])

      let confirmation = FormField.create ("", rulesView = confirmationRules)

      let account = Form.model {
        let! password = password
        and! _ = confirmation
        return password
      }

      let submitted = Var.Create ""

      Form.create (
        account,
        (fun _ -> Var.Set submitted "Passwords matched at submission."),
        [
          TextField.singleLine (
            password,
            labelText = View.Const "Password",
            inputAttrs = [ attr.``type`` "password"; attr.autocomplete "new-password" ]
          )
          TextField.singleLine (
            confirmation,
            labelText = View.Const "Confirm password",
            inputAttrs = [ attr.``type`` "password"; attr.autocomplete "new-password" ]
          )

          div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
            Button.primary (
              text "Check passwords",
              onClick = ignore,
              attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
            )
            Button.create (
              text "Reset",
              onClick = ignore,
              attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
            )
          ]
          p [ Attr.Create "role" "status" ] [ textView submitted.View ]
        ],
        attrs = [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          Gap.All.g3
          on.reset (fun _ _ -> Var.Set submitted "")
        ]
      )

    Helpers.codeSampleSection
      "Cross-field validation"
      (Helpers.bodyText
        "Enter matching passwords, then change the original. The confirmation revalidates through rulesView. Submission remains local to this demo.")
      (div [ Attr.Style "max-inline-size" "32rem"; Attr.Style "margin-inline" "auto" ] [ content ])
      """let password =
  FormField.create (
    "",
    rules = [
      Validate.required "Enter a password."
      |> Validate.andThen (
        Validate.minLength 8 "Use at least eight characters."
      )
    ]
  )

let confirmationRules =
  password.Value.View
  |> View.Map(fun expected -> [
    Validate.required "Confirm your password."
    |> Validate.andThen (Validate.equalTo expected "Passwords must match.")
  ])

let confirmation = FormField.create ("", rulesView = confirmationRules)

let account =
  Form.model {
    let! password = password
    and! _ = confirmation
    return password
  }

let submitted = Var.Create ""

Form.create (
  account,
  (fun _ -> Var.Set submitted "Passwords matched at submission."),
  [
    TextField.singleLine (
      password,
      labelText = View.Const "Password",
      inputAttrs = [
        attr.``type`` "password"
        attr.autocomplete "new-password"
      ]
    )
    TextField.singleLine (
      confirmation,
      labelText = View.Const "Confirm password",
      inputAttrs = [
        attr.``type`` "password"
        attr.autocomplete "new-password"
      ]
    )

    div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
      Button.primary (
        text "Check passwords",
        onClick = ignore,
        attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
      )
      Button.create (
        text "Reset",
        onClick = ignore,
        attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
      )
    ]
    p [ Attr.Create "role" "status" ] [ textView submitted.View ]
  ],
  attrs = [
    Flex.Flex.allSizes
    FlexDirection.Column.allSizes
    Gap.All.g3
    on.reset (fun _ _ -> Var.Set submitted "")
  ]
)"""

  let private numericExample () =
    let content =
      let age =
        FormField.create (18, rules = [ Validate.between 18 120 "Age must be between 18 and 120." ])

      let quantity =
        FormField.create (
          2,
          rules = [
            Validate.greaterThan 0 "Quantity must be positive."
            |> Validate.andThen (Validate.custom (fun value -> value % 2 = 0) "Order tickets in pairs.")
          ]
        )

      let booking = Form.model {
        let! age = age
        and! quantity = quantity
        return age, quantity
      }

      let submitted = Var.Create ""

      Form.create (
        booking,
        (fun value ->
          Var.Set submitted (sprintf "Reserved %d demo tickets for a guest aged %d." (snd value) (fst value))),
        [
          NumericField.create (age, labelText = View.Const "Guest age")
          NumericField.create (quantity, labelText = View.Const "Tickets")

          div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
            Button.primary (
              text "Reserve tickets",
              onClick = ignore,
              attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
            )
            Button.create (
              text "Reset",
              onClick = ignore,
              attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
            )
          ]
          p [ Attr.Create "role" "status" ] [ textView submitted.View ]
        ],
        attrs = [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          Gap.All.g3
          on.reset (fun _ _ -> Var.Set submitted "")
        ]
      )

    Helpers.codeSampleSection
      "Numeric comparisons"
      (Helpers.bodyText
        "between includes its boundaries; greaterThan excludes its boundary. A custom predicate requires an even ticket count. Submit to test the rules.")
      (div [ Attr.Style "max-inline-size" "32rem"; Attr.Style "margin-inline" "auto" ] [ content ])
      """let age =
  FormField.create (
    18,
    rules = [ Validate.between 18 120 "Age must be between 18 and 120." ]
  )

let quantity =
  FormField.create (
    2,
    rules = [
      Validate.greaterThan 0 "Quantity must be positive."
      |> Validate.andThen (
        Validate.custom (fun value -> value % 2 = 0) "Order tickets in pairs."
      )
    ]
  )

let booking =
  Form.model {
    let! age = age
    and! quantity = quantity
    return age, quantity
  }

let submitted = Var.Create ""

Form.create (
  booking,
  (fun value ->
    Var.Set
      submitted
      (sprintf
        "Reserved %d demo tickets for a guest aged %d."
        (snd value)
        (fst value))),
  [
    NumericField.create (age, labelText = View.Const "Guest age")
    NumericField.create (quantity, labelText = View.Const "Tickets")

    div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
      Button.primary (
        text "Reserve tickets",
        onClick = ignore,
        attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
      )
      Button.create (
        text "Reset",
        onClick = ignore,
        attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
      )
    ]
    p [ Attr.Create "role" "status" ] [ textView submitted.View ]
  ],
  attrs = [
    Flex.Flex.allSizes
    FlexDirection.Column.allSizes
    Gap.All.g3
    on.reset (fun _ _ -> Var.Set submitted "")
  ]
)"""

  let private collectionExample () =
    let content =
      let interests =
        FormField.create (
          Set.empty<string>,
          rules = [
            Validate.minLengthBy Set.count 1 "Choose at least one interest."
            Validate.maxLengthBy Set.count 3 "Choose at most three interests."
          ]
        )

      let model = Form.model {
        let! interests = interests
        return interests
      }

      let items =
        [ "Design"; "Engineering"; "Music"; "Photography"; "Travel" ]
        |> List.map (fun name -> SelectItem.create (text name, name, name))
        |> View.Const

      let submitted = Var.Create ""

      Form.create (
        model,
        (fun value -> Var.Set submitted ("Submitted interests: " + String.concat ", " (Set.toList value))),
        [
          MultiSelect.create (
            items,
            interests,
            labelText = View.Const "Interests",
            placeholder = View.Const "Choose your interests",
            clearable = View.Const true,
            attrs = [ Select.Width.full ]
          )

          div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
            Button.primary (
              text "Save interests",
              onClick = ignore,
              attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
            )
            Button.create (
              text "Reset",
              onClick = ignore,
              attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
            )
          ]
          p [ Attr.Create "role" "status" ] [ textView submitted.View ]
        ],
        attrs = [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          Gap.All.g3
          on.reset (fun _ _ -> Var.Set submitted "")
        ]
      )

    Helpers.codeSampleSection
      "Collection lengths"
      (Helpers.bodyText
        "Choose one to three interests; a fourth reveals the maximum-length error. minLengthBy and maxLengthBy accept the measurement function for your collection type.")
      (div [ Attr.Style "max-inline-size" "32rem"; Attr.Style "margin-inline" "auto" ] [ content ])
      """let interests =
  FormField.create (
    Set.empty<string>,
    rules = [
      Validate.minLengthBy Set.count 1 "Choose at least one interest."
      Validate.maxLengthBy Set.count 3 "Choose at most three interests."
    ]
  )

let model =
  Form.model {
    let! interests = interests
    return interests
  }

let items =
  [ "Design"; "Engineering"; "Music"; "Photography"; "Travel" ]
  |> List.map (fun name -> SelectItem.create (text name, name, name))
  |> View.Const

let submitted = Var.Create ""

Form.create (
  model,
  (fun value ->
    Var.Set
      submitted
      ("Submitted interests: " + String.concat ", " (Set.toList value))),
  [
    MultiSelect.create (
      items,
      interests,
      labelText = View.Const "Interests",
      placeholder = View.Const "Choose your interests",
      clearable = View.Const true,
      attrs = [ Select.Width.full ]
    )

    div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
      Button.primary (
        text "Save interests",
        onClick = ignore,
        attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
      )
      Button.create (
        text "Reset",
        onClick = ignore,
        attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
      )
    ]
    p [ Attr.Create "role" "status" ] [ textView submitted.View ]
  ],
  attrs = [
    Flex.Flex.allSizes
    FlexDirection.Column.allSizes
    Gap.All.g3
    on.reset (fun _ _ -> Var.Set submitted "")
  ]
)"""

  let private arithmeticExample () =
    let content =
      let price =
        FormField.create (25.0, rules = [ Validate.greaterThan 0.0 "Price must be positive." ])

      let quantityRules =
        price.Value.View
        |> View.Map(fun price -> [
          Validate.atLeast 1 "Quantity must be positive."
          Validate.on
            (fun quantity -> price * float quantity)
            (Validate.atMost 100.0 "Total must not exceed 100.")
        ])

      let quantity = FormField.create (4, rulesView = quantityRules)

      let order = Form.model {
        let! price = price
        and! quantity = quantity
        return price, quantity
      }

      let total =
        View.Map2 (fun price count -> price * float count) price.Value.View quantity.Value.View

      let submitted = Var.Create ""

      Form.create (
        order,
        (fun value ->
          Var.Set
            submitted
            (sprintf "Submitted %d items for $%.2f." (snd value) (fst value * float (snd value)))),
        [
          NumericField.create (price, labelText = View.Const "Unit price ($)")
          NumericField.create (quantity, labelText = View.Const "Quantity")
          p [] [ textView (total |> View.Map(sprintf "Total: $%.2f (budget: $100.00)")) ]

          div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
            Button.primary (
              text "Save order",
              onClick = ignore,
              attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
            )
            Button.create (
              text "Reset",
              onClick = ignore,
              attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
            )
          ]
          p [ Attr.Create "role" "status" ] [ textView submitted.View ]
        ],
        attrs = [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          Gap.All.g3
          on.reset (fun _ _ -> Var.Set submitted "")
        ]
      )

    Helpers.codeSampleSection
      "Arithmetic across fields"
      (Helpers.bodyText
        "The budget is $100. Increase quantity to 5, then lower the unit price to 20. Validate.on checks the derived total while the field retains its original quantity.")
      (div [ Attr.Style "max-inline-size" "32rem"; Attr.Style "margin-inline" "auto" ] [ content ])
      """let price =
  FormField.create (
    25.0,
    rules = [ Validate.greaterThan 0.0 "Price must be positive." ]
  )

let quantityRules =
  price.Value.View
  |> View.Map(fun price -> [
    Validate.atLeast 1 "Quantity must be positive."
    Validate.on
      (fun quantity -> price * float quantity)
      (Validate.atMost 100.0 "Total must not exceed 100.")
  ])

let quantity = FormField.create (4, rulesView = quantityRules)

let order =
  Form.model {
    let! price = price
    and! quantity = quantity
    return price, quantity
  }

let total =
  View.Map2
    (fun price count -> price * float count)
    price.Value.View
    quantity.Value.View

let submitted = Var.Create ""

Form.create (
  order,
  (fun value ->
    Var.Set
      submitted
      (sprintf
        "Submitted %d items for $%.2f."
        (snd value)
        (fst value * float (snd value)))),
  [
    NumericField.create (price, labelText = View.Const "Unit price ($)")
    NumericField.create (quantity, labelText = View.Const "Quantity")
    p [] [
      textView (total |> View.Map(sprintf "Total: $%.2f (budget: $100.00)"))
    ]

    div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
      Button.primary (
        text "Save order",
        onClick = ignore,
        attrs = [ attr.``type`` "submit"; Button.Variant.filled ]
      )
      Button.create (
        text "Reset",
        onClick = ignore,
        attrs = [ attr.``type`` "reset"; Button.Variant.outlined ]
      )
    ]
    p [ Attr.Create "role" "status" ] [ textView submitted.View ]
  ],
  attrs = [
    Flex.Flex.allSizes
    FlexDirection.Column.allSizes
    Gap.All.g3
    on.reset (fun _ _ -> Var.Set submitted "")
  ]
)"""

  let private apiReference () =
    Helpers.apiSection
      (Helpers.bodyText
        "Client validation is synchronous. Keep server validation authoritative; the submit callback owns saving, pending state, and server error handling. Disabled controls remain part of the model; express conditional requirements through rulesView. NumericField validates its typed, clamped value; use a string field and custom rules when you must validate raw numeric input.")
      [
        Helpers.apiTable "FormField.create" [
          Helpers.apiParam
            "initialValue"
            "'T (equality)"
            ""
            "Initial and reset value; exposed through Value: Var<'T>"
          Helpers.apiParam "?rules" "Validator<'T> list" "[]" "Rules evaluated in order on the current value"
          Helpers.apiParam
            "?rulesView"
            "View<Validator<'T> list>"
            ""
            "Reactive rules; replaces rules when supplied"
        ]
        Helpers.apiTable "Form.model" [
          Helpers.apiParam
            "let! / and! / return"
            "computation expression"
            ""
            "Read independent fields or nested models and construct the typed submit value; accumulates all errors"
        ]
        Helpers.apiTable "Form.create" [
          Helpers.apiParam "model" "FormModel<'T>" "" "Result of a form expression"
          Helpers.apiParam
            "onSubmit"
            "'T -> unit"
            ""
            "Called only with a valid result; native navigation is prevented"
          Helpers.apiParam "content" "Doc list" "" "Form controls; use native submit and reset buttons"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Attributes on the form element"
        ]
        Helpers.apiTable "Field state" [
          Helpers.apiParam
            "Value"
            "Var<'T>"
            ""
            "Raw binding for custom controls; standard fields accept FormField directly"
          Helpers.apiParam "Errors" "View<string list>" "" "All current errors, including before touch"
          Helpers.apiParam
            "IsTouched / IsDirty"
            "View<bool>"
            ""
            "Blur/submit state and difference from initial value"
          Helpers.apiParam
            "Validation"
            "FieldValidation"
            ""
            "Low-level presentation state for custom controls; standard fields wire it automatically"
          Helpers.apiParam
            "Touch() / Reset()"
            "unit"
            ""
            "Reveal errors / restore initial value and untouched state"
          Helpers.apiParam "Model" "FormModel<'T>" "" "Field as a composable model"
        ]
        Helpers.apiTable "FormModel<'T>" [
          Helpers.apiParam
            "Result"
            "View<Result<'T, FormError list>>"
            ""
            "Live typed value or all errors, each with FieldId and Message"
          Helpers.apiParam "IsValid" "View<bool>" "" "Independent of error visibility"
          Helpers.apiParam "Touch() / Reset()" "unit" "" "Apply to every field in the model"
          Helpers.apiParam
            "Submit(onValid)"
            "('T -> unit) -> unit"
            ""
            "Reveal errors and sample the current result once"
          Helpers.apiParam
            "FormModel.map / zip"
            "functions"
            ""
            "Transform a valid result / combine models and accumulate errors"
        ]
        Helpers.apiTable "Validate" [
          Helpers.apiParam
            "custom predicate message"
            "Validator<'T>"
            ""
            "Fail when the predicate returns false"
          Helpers.apiParam "required message" "Validator<string>" "" "Reject null, empty, and whitespace"
          Helpers.apiParam
            "minLength / maxLength"
            "int -> string -> Validator<string>"
            ""
            "Inclusive UTF-16 string length bounds"
          Helpers.apiParam
            "minLengthBy / maxLengthBy"
            "('T -> int) -> int -> string -> Validator<'T>"
            ""
            "Inclusive length bounds using List.length, Array.length, Set.count, or a custom measurement"
          Helpers.apiParam
            "atLeast / atMost"
            "'T -> string -> Validator<'T>"
            ""
            "Inclusive lower / upper bound; requires comparison"
          Helpers.apiParam
            "greaterThan / lessThan"
            "'T -> string -> Validator<'T>"
            ""
            "Exclusive lower / upper bound; requires comparison"
          Helpers.apiParam
            "between min max message"
            "Validator<'T>"
            ""
            "Inclusive range; inverted or NaN bounds throw ArgumentException"
          Helpers.apiParam
            "equalTo / notEqualTo"
            "'T -> string -> Validator<'T>"
            ""
            "Equality / inequality; requires equality"
          Helpers.apiParam
            "on project rule"
            "('T -> 'U) -> Validator<'U> -> Validator<'T>"
            ""
            "Apply a rule to a derived value without changing the bound field value"
          Helpers.apiParam "isSome / isTrue" "rules" "" "Require an option value / checked consent"
          Helpers.apiParam "optional rule" "Validator<'T option>" "" "Validate Some; accept None"
          Helpers.apiParam "all rules" "Validator<'T>" "" "Accumulate every rule's messages in order"
          Helpers.apiParam "first |> andThen next" "Validator<'T>" "" "Run next only if first succeeds"
        ]
        Helpers.apiTable "Custom controls" [
          Helpers.apiParam
            "ValidationInput.attrs state"
            "Attr list"
            ""
            "Apply to the actual input: blur, aria-invalid, aria-describedby. When supplying your own aria-describedby, include state.ErrorId alongside existing help IDs."
          Helpers.apiParam
            "Field.create(?validation)"
            "FieldValidation"
            ""
            "Renders error messages and styling; validation errors take precedence over normal help text"
        ]
      ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Forms & Validation"
        personExample ()
        Helpers.divider ()
        rulesExample ()
        Helpers.divider ()
        numericExample ()
        Helpers.divider ()
        collectionExample ()
        Helpers.divider ()
        arithmeticExample ()
        Helpers.divider ()
        apiReference ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
