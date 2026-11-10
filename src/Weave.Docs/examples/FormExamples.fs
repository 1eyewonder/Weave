namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module FormExamples =

  type Person = {
    Name: string
    Age: int
  }

  let private fieldAttrs color = [ Field.Variant.outlined; Field.Label.above; Field.Width.full; color ]

  let private actionButton (color: Attr) kind label =
    button [
      attr.``type`` kind
      Attr.Class "weave-button"
      if kind = "submit" then
        color
        Button.Variant.filled
      else
        Button.Variant.outlined
    ] [ span [ Attr.Class "weave-button__label" ] [ text label ] ]

  let private demo id color (model: FormModel<'T>) onSubmit submitLabel (fields: Doc list) extra =
    let submitted = Var.Create ""

    let save value = submitted.Value <- onSubmit value

    let columnAttrs = [
      GridItem.Span.twelve
      if fields.Length > 1 then
        GridItem.Span.Small.six
    ]

    Form.create (
      model,
      save,
      [
        model.Result |> Doc.sink (fun _ -> submitted.Value <- "")

        Grid.create (
          fields |> List.map (fun field -> GridItem.create (field, attrs = columnAttrs)),
          attrs = [ Grid.Spacing.extraLarge; AlignItems.start ]
        )

        extra

        div [ Margin.Top.small; Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g3 ] [
          actionButton color "submit" submitLabel
          actionButton color "reset" "Reset"
        ]

        div [ Typography.body2; Margin.Top.extraSmall; Attr.Create "role" "status" ] [
          Doc.TextView submitted.View
        ]
      ],
      attrs = [ Attr.Create "data-demo" id; on.reset (fun _ _ -> submitted.Value <- "") ]
    )

  let private personExample () =
    let name = FormField.create ("", rules = [ Validate.required "Enter your name." ])

    let age =
      FormField.create (18, rules = [ Validate.atLeast 18 "You must be at least 18." ])

    let person = Form.model {
      let! name = name
      and! age = age

      return {
        Name = name.Trim()
        Age = age
      }
    }

    let content =
      demo
        "profile"
        Button.Color.primary
        person
        (fun value -> $"Profile saved for {value.Name}, age {value.Age}.")
        "Save profile"
        [
          TextField.singleLine (
            name,
            labelText = View.Const "Name",
            placeholder = View.Const "Ada Lovelace",
            attrs = fieldAttrs Field.Color.primary
          )
          NumericField.create (age, labelText = View.Const "Age", attrs = fieldAttrs Field.Color.primary)
        ]
        Doc.Empty

    Helpers.codeSampleSection
      "A simple form"
      (Helpers.bodyText
        "Declare fields and their rules, combine their values into a Person, and pass the fields directly to controls. Errors appear after blur or submit; reset restores the initial values. Top-align rows with AlignItems.start to keep controls anchored. Messages take space only while visible.")
      content
      """let name = FormField.create ("", rules = [ Validate.required "Enter your name." ])

let age =
  FormField.create (18, rules = [ Validate.atLeast 18 "You must be at least 18." ])

let person = Form.model {
  let! name = name
  and! age = age

  return {
    Name = name.Trim()
    Age = age
  }
}

Form.create (
  person,
  savePerson,
  [
    TextField.singleLine (
      name,
      labelText = View.Const "Name",
      attrs = [ Field.Variant.outlined; Field.Label.above ]
    )

    NumericField.create (
      age,
      labelText = View.Const "Age",
      attrs = [ Field.Variant.outlined; Field.Label.above ]
    )

    // Native buttons preserve Enter-to-submit and reset behavior.
    button [
      attr.``type`` "submit"
      Attr.Class "weave-button"
      Button.Color.primary
      Button.Variant.filled
    ] [ span [ Attr.Class "weave-button__label" ] [ text "Save profile" ] ]
  ]
)"""

  let private rulesExample () =
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

    let content =
      demo
        "passwords"
        Button.Color.secondary
        account
        (fun _ -> "Passwords match. Your demo account is ready.")
        "Check passwords"
        [
          TextField.singleLine (
            password,
            labelText = View.Const "Password",
            inputAttrs = [ attr.``type`` "password"; Attr.Create "autocomplete" "new-password" ],
            attrs = fieldAttrs Field.Color.secondary
          )
          TextField.singleLine (
            confirmation,
            labelText = View.Const "Confirm password",
            inputAttrs = [ attr.``type`` "password"; Attr.Create "autocomplete" "new-password" ],
            attrs = fieldAttrs Field.Color.secondary
          )
        ]
        Doc.Empty

    Helpers.codeSampleSection
      "Cross-field validation"
      (Helpers.bodyText
        "Enter matching passwords, then change the original to see the confirmation revalidate. The rulesView dependency keeps both fields in sync.")
      content
      """let password =
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

TextField.singleLine (
  confirmation,
  labelText = View.Const "Confirm password",
  attrs = [ TextField.Variant.outlined; TextField.Color.secondary ]
)"""

  let private numericExample () =
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

    let content =
      demo
        "numbers"
        Button.Color.tertiary
        booking
        (fun (age, quantity) -> $"Reserved {quantity} demo tickets for a guest aged {age}.")
        "Reserve tickets"
        [
          NumericField.create (
            age,
            labelText = View.Const "Guest age",
            attrs = fieldAttrs Field.Color.tertiary
          )
          NumericField.create (
            quantity,
            labelText = View.Const "Tickets",
            attrs = fieldAttrs Field.Color.tertiary
          )
        ]
        Doc.Empty

    Helpers.codeSampleSection
      "Numeric comparisons"
      (Helpers.bodyText
        "atLeast, atMost, and between include their boundaries; greaterThan and lessThan exclude them. Arithmetic predicates use ordinary F#. Browser forms use int or float; the installed WebSharper version does not transpile decimal arithmetic.")
      content
      """let age =
  FormField.create (18, rules = [ Validate.between 18 120 "Age must be between 18 and 120." ])

let quantity =
  FormField.create (
    2,
    rules = [
      Validate.greaterThan 0 "Quantity must be positive."
      |> Validate.andThen (Validate.custom (fun value -> value % 2 = 0) "Order tickets in pairs.")
    ]
  )"""

  let private collectionExample () =
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
      |> List.map (fun interest -> SelectItem.create (text interest, interest, interest))
      |> View.Const

    let selection =
      div [ Typography.body2; Margin.Top.extraSmall ] [
        Doc.TextView(
          interests.Value.View
          |> View.Map(fun values ->
            let names =
              if Set.isEmpty values then
                "None"
              else
                String.concat ", " (Set.toList values)

            $"Selected ({values.Count}/3): {names}")
        )
      ]

    let content =
      demo
        "collections"
        Button.Color.secondary
        model
        (fun values -> "Interests saved: " + String.concat ", " (Set.toList values) + ".")
        "Save interests"
        [
          MultiSelect.create (
            items,
            interests,
            labelText = View.Const "Interests",
            placeholder = View.Const "Choose your interests",
            clearable = View.Const true,
            attrs = fieldAttrs Field.Color.secondary @ [ Select.Color.secondary; Select.Width.full ]
          )
        ]
        selection

    Helpers.codeSampleSection
      "Collection lengths"
      (Helpers.bodyText
        "Choose one to three interests; selecting a fourth shows the maximum-length error. Supply List.length, Array.length, Set.count, or a custom measurement to minLengthBy and maxLengthBy.")
      content
      """let interests =
  FormField.create (
    Set.empty<string>,
    rules = [
      Validate.minLengthBy Set.count 1 "Choose at least one interest."
      Validate.maxLengthBy Set.count 3 "Choose at most three interests."
    ]
  )

MultiSelect.create (
  items,
  interests,
  labelText = View.Const "Interests",
  attrs = [ Select.Variant.outlined; Select.Color.secondary ]
)

// You can apply other rules to a measured length, too.
let tagsRule = Validate.on List.length (Validate.between 1 5 "Choose 1 to 5 tags.")"""

  let private arithmeticExample () =
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
      View.Map2 (fun price quantity -> price * float quantity) price.Value.View quantity.Value.View

    let summary =
      div [ Typography.body2; Margin.Top.extraSmall ] [
        Doc.TextView(total |> View.Map(fun value -> sprintf "Total: $%.2f (budget: $100.00)" value))
      ]

    let content =
      demo
        "arithmetic"
        Button.Color.tertiary
        order
        (fun (price, quantity) ->
          sprintf "Demo order saved: %d items, total $%.2f." quantity (price * float quantity))
        "Save order"
        [
          NumericField.create (
            price,
            labelText = View.Const "Unit price ($)",
            attrs = fieldAttrs Field.Color.tertiary
          )
          NumericField.create (
            quantity,
            labelText = View.Const "Quantity",
            attrs = fieldAttrs Field.Color.tertiary
          )
        ]
        summary

    Helpers.codeSampleSection
      "Arithmetic across fields"
      (Helpers.bodyText
        "The budget is $100. Increase quantity to 5, then lower the unit price to 20. Either field rechecks the total through Validate.on, while quantity keeps its original value.")
      content
      """let price =
  FormField.create (25.0, rules = [ Validate.greaterThan 0.0 "Price must be positive." ])

let quantityRules =
  price.Value.View
  |> View.Map(fun price -> [
    Validate.atLeast 1 "Quantity must be positive."
    Validate.on (fun quantity -> price * float quantity) (Validate.atMost 100.0 "Total must not exceed 100.")
  ])

let quantity = FormField.create (4, rulesView = quantityRules)"""

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
