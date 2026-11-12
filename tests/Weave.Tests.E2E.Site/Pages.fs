namespace Weave.Tests.E2E.Site

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.CssHelpers
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module Pages =

  let private formArithmeticPage () =
    let price =
      FormField.create (25.0, rules = [ Validate.greaterThan 0.0 "Price must be positive." ])

    let quantity =
      FormField.create (
        4,
        rulesView =
          (price.Value.View
           |> View.Map(fun unitPrice -> [
             Validate.between 1 10 "Choose between 1 and 10."
             Validate.on (fun count -> unitPrice * float count) (Validate.atMost 100.0 "Over budget.")
           ]))
      )

    let order = Form.model {
      let! price = price
      and! quantity = quantity
      return price, quantity
    }

    let saved = Var.Create ""

    Form.create (
      order,
      (fun (price, quantity) -> saved.Value <- sprintf "%s x %d" (string price) quantity),
      [
        NumericField.create (price, labelText = View.Const "Price")
        NumericField.create (quantity, labelText = View.Const "Quantity")
        p [ attr.id "nan-bounds" ] [
          text (
            [
              Validate.atLeast 0.0 "Invalid"
              Validate.atMost 1.0 "Invalid"
              Validate.greaterThan 0.0 "Invalid"
              Validate.lessThan 1.0 "Invalid"
              Validate.between 0.0 1.0 "Invalid"
            ]
            |> List.forall (fun rule -> rule System.Double.NaN = [ "Invalid" ])
            |> fun valid -> if valid then "true" else "false"
          )
        ]
        button [ attr.``type`` "submit" ] [ text "Save" ]
        p [ attr.id "saved" ] [ Doc.TextView saved.View ]
      ]
    )

  let private formSizingPage () =
    div [] [
      for variant, attrs in
        [
          "standard", []
          "filled", [ Field.Variant.filled ]
          "outlined", [ Field.Variant.outlined ]
        ] do
        let name = FormField.create ""
        let age = FormField.create 18

        let person = Form.model {
          let! name = name
          and! age = age
          return name, age
        }

        Form.create (
          person,
          ignore,
          [
            TextField.singleLine (name, labelText = View.Const "Name", attrs = attrs)
            NumericField.create (age, labelText = View.Const "Age", attrs = attrs)
          ],
          attrs = [ Attr.Create "data-variant" variant ]
        )
    ]

  let private formGridValidationPage () =
    div [] [
      for variant, variantAttrs in
        [
          "standard", []
          "filled", [ Field.Variant.filled ]
          "outlined", [ Field.Variant.outlined; Field.Label.above ]
        ] do
        let name = FormField.create "Ada"

        let quantity =
          FormField.create (2, rules = [ Validate.custom (fun value -> value % 2 = 0) "Order in pairs." ])

        let model = Form.model {
          let! name = name
          and! quantity = quantity
          return name, quantity
        }

        let attrs = Field.Width.full :: variantAttrs

        Form.create (
          model,
          ignore,
          [
            Grid.create (
              [
                GridItem.create (
                  TextField.singleLine (name, labelText = View.Const "Name", attrs = attrs),
                  attrs = [ GridItem.Span.six ]
                )
                GridItem.create (
                  NumericField.create (quantity, labelText = View.Const "Quantity", attrs = attrs),
                  attrs = [ GridItem.Span.six ]
                )
              ],
              attrs = [ Grid.Spacing.large; AlignItems.start ]
            )
            button [ attr.``type`` "submit" ] [ text "Save" ]
          ],
          attrs = [ Attr.Create "data-variant" variant ]
        )
    ]

  let private formPage () =
    let name =
      FormField.create (
        "",
        rules = [ Validate.required "Name required."; Validate.minLength 3 "Name too short." ]
      )

    let password =
      FormField.create ("", rules = [ Validate.minLength 8 "Password too short." ])

    let confirmation =
      FormField.create (
        "",
        rulesView =
          (password.Value.View
           |> View.Map(fun expected -> [
             Validate.required "Confirm password."
             |> Validate.andThen (Validate.equalTo expected "Passwords must match.")
           ]))
      )

    let age = FormField.create (17, rules = [ Validate.atLeast 18 "Adults only." ])

    let country =
      FormField.create (None, rules = [ Validate.isSome "Choose a country." ])

    let interests =
      FormField.create (Set.empty<string>, rules = [ Validate.minLengthBy Set.count 1 "Choose an interest." ])

    let credentials = Form.model {
      let! password = password
      and! _ = confirmation
      return password
    }

    let person = Form.model {
      let! name = name
      and! password = credentials
      and! age = age
      and! country = country
      and! interests = interests
      return name, password, age, country, interests
    }

    let submitted = Var.Create 0
    let result = Var.Create ""

    let save (name, _, age, country, interests) =
      submitted.Value <- submitted.Value + 1

      result.Value <-
        sprintf
          "%s|%d|%s|%s"
          name
          age
          (Option.defaultValue "" country)
          (String.concat "," (Set.toList interests))

    Form.create (
      person,
      save,
      [
        TextField.singleLine (
          name,
          labelText = View.Const "Name",
          showHelpText = View.Const true,
          helpText = FieldHelpText.create (text "Your full name")
        )
        TextField.singleLine (
          password,
          labelText = View.Const "Password",
          inputAttrs = [ attr.``type`` "password" ]
        )
        TextField.multiLine (
          confirmation.Value,
          labelText = View.Const "Confirm password",
          validation = confirmation.Validation
        )
        NumericField.create (age, labelText = View.Const "Age")
        Select.create (
          View.Const [ SelectItem.create (text "Canada", "Canada", "Canada") ],
          country,
          labelText = View.Const "Country"
        )
        MultiSelect.create (
          View.Const [ SelectItem.create (text "Music", "Music", "Music") ],
          interests,
          labelText = View.Const "Interests"
        )
        button [ attr.``type`` "submit" ] [ text "Save" ]
        button [ attr.``type`` "reset" ] [ text "Reset" ]
        Button.create (
          text "Fill and submit",
          (fun () ->
            name.Value.Value <- "Same turn"
            password.Value.Value <- "password123"
            confirmation.Value.Value <- "password123"
            age.Value.Value <- 25
            country.Value.Value <- Some "Canada"
            interests.Value.Value <- Set.singleton "Music"
            person.Submit save)
        )
        Button.create (
          text "Invalidate and submit",
          (fun () ->
            password.Value.Value <- "changed123"
            person.Submit save)
        )
        p [ attr.id "submitted" ] [ Doc.TextView(submitted.View |> View.Map string) ]
        p [ attr.id "result" ] [ Doc.TextView result.View ]
        p [ attr.id "valid" ] [
          Doc.TextView(person.IsValid |> View.Map(fun value -> if value then "true" else "false"))
        ]
        p [ attr.id "error-count" ] [
          Doc.TextView(
            person.Result
            |> View.Map(function
              | Ok _ -> "0"
              | Error errors -> string errors.Length)
          )
        ]
        p [ attr.id "dirty" ] [
          Doc.TextView(name.IsDirty |> View.Map(fun value -> if value then "true" else "false"))
        ]
        p [ attr.id "touched" ] [
          Doc.TextView(name.IsTouched |> View.Map(fun value -> if value then "true" else "false"))
        ]
      ]
    )

  let private carouselPage autoCycle hiddenControls =
    let index = Var.Create 0
    let arrows = Var.Create true
    let indicators = Var.Create true

    let slides = [
      CarouselItem.create (
        div [ Attr.Style "padding" "24px" ] [
          text "First slide"
          input [ attr.placeholder "Slide input" ] []
        ],
        label = "First"
      )
      CarouselItem.create (
        div [ Attr.Style "padding" "24px" ] [
          text "Second slide"
          Button.create (text "Slide action", (fun () -> ()))
        ],
        label = "Second"
      )
      CarouselItem.create (
        div [ Attr.Style "padding" "24px"; Attr.Style "height" "180px" ] [ text "Third slide" ],
        label = "Third"
      )
    ]

    let items = Var.Create slides
    let mounted = Var.Create true

    div [] [
      mounted.View
      |> Doc.BindView(fun show ->
        if show then
          Carousel.create (
            items.View,
            activeIndex = index,
            ?autoCycleInterval = (if autoCycle then Some 1000 else None),
            label = "Test highlights",
            attrs = [
              if hiddenControls then
                Carousel.Controls.hideArrows
                Carousel.Controls.hideIndicators
              else
                View.not arrows.View |> Attr.DynamicClassPred "weave-carousel--hide-arrows"

                View.not indicators.View
                |> Attr.DynamicClassPred "weave-carousel--hide-indicators"
            ]
          )
        else
          Doc.Empty)
      span [ Attr.Create "id" "carousel-index" ] [ textView (index.View |> View.Map string<int>) ]
      Button.create (text "Select last", (fun () -> index.Set 2))
      Button.create (text "Invalid selection", (fun () -> index.Set 99))
      Button.create (text "Negative selection", (fun () -> index.Set -5))
      Button.create (text "Single slide", (fun () -> items.Set [ slides.Head ]))
      Button.create (text "Empty slides", (fun () -> items.Set []))
      Button.create (text "Restore slides", (fun () -> items.Set slides))
      Button.create (text "Hide arrows", (fun () -> arrows.Set false))
      Button.create (text "Hide indicators", (fun () -> indicators.Set false))
      Button.create (text "Remove carousel", (fun () -> mounted.Set false))
      Button.create (text "Restore carousel", (fun () -> mounted.Set true))
    ]

  let private carouselPairPage () =
    let firstIndex = Var.Create 0
    let secondIndex = Var.Create 0
    let mounted = Var.Create true

    let slides () =
      View.Const [ CarouselItem.create (text "Alpha"); CarouselItem.create (text "Beta") ]

    div [] [
      mounted.View
      |> Doc.BindView(fun show ->
        if show then
          Carousel.create (
            slides (),
            activeIndex = firstIndex,
            autoCycleInterval = 1000,
            label = "First carousel"
          )
        else
          Doc.Empty)
      Carousel.create (
        slides (),
        activeIndex = secondIndex,
        autoCycleInterval = 1000,
        label = "Second carousel"
      )
      span [ attr.id "first-index" ] [ textView (firstIndex.View |> View.Map string<int>) ]
      Button.create (text "Remove first carousel", fun () -> mounted.Set false)
    ]

  let private animationPage () =
    let isVisible = Var.Create false
    let isActive = Var.Create false

    div [] [
      div [ Attr.Create "data-testid" "fade-in-static"; AnimationEntrance.fadeIn ] [ text "Fade in" ]
      div [ Attr.Create "data-testid" "bounce-static"; AnimationEmphasis.bounce ] [ text "Bounce" ]
      div [
        Attr.Create "data-testid" "pulse-hover"
        AnimationEmphasis.pulse
        AnimationOn.hover
      ] [ text "Pulse on hover" ]
      div [ Attr.Create "data-testid" "suppress-container"; AnimationKind.suppress ] [
        div [ Attr.Create "data-testid" "suppress-child"; AnimationEntrance.fadeIn ] [
          text "Suppressed child"
        ]
      ]
      Button.create (
        text "Toggle Show",
        (fun () -> isVisible.Value <- not isVisible.Value),
        attrs = [ Attr.Create "data-testid" "show-toggle-btn" ]
      )
      Animate.showWith
        AnimationPair.fadeInOut
        isVisible.View
        (fun () -> div [ Attr.Create "data-testid" "show-content" ] [ text "Animated show content" ])
        [ Attr.Create "data-testid" "show-wrapper" ]
        None
      Button.create (
        text "Toggle Class",
        (fun () -> isActive.Value <- not isActive.Value),
        attrs = [ Attr.Create "data-testid" "toggle-btn" ]
      )
      div [
        Attr.Create "data-testid" "toggle-target"
        Animate.toggleClass AnimationPair.fadeInOut isActive.View
      ] [ text "Toggle target" ]
      div [] [
        div [
          Attr.Create "data-testid" "stagger-1"
          AnimationEntrance.fadeIn
          AnimationDelay.stagger 1
        ] [ text "Stagger 1" ]
        div [
          Attr.Create "data-testid" "stagger-2"
          AnimationEntrance.fadeIn
          AnimationDelay.stagger 2
        ] [ text "Stagger 2" ]
        div [
          Attr.Create "data-testid" "stagger-3"
          AnimationEntrance.fadeIn
          AnimationDelay.stagger 3
        ] [ text "Stagger 3" ]
      ]
    ]

  let private checkboxPage () =
    div [] [
      Checkbox.create (Var.Create false, View.Const "Default Checkbox")
      Checkbox.create (Var.Create true, View.Const "Checked Checkbox")
      Checkbox.create (Var.Create false, View.Const "Disabled Checkbox", enabled = View.Const false)
      Checkbox.create (
        Var.Create true,
        View.Const "Disabled checked",
        enabled = View.Const false,
        attrs = [ Attr.Create "data-testid" "disabled-checked-checkbox" ]
      )
      Checkbox.create (Var.Create false, View.Const "Small", attrs = [ Checkbox.Size.small ])
      Checkbox.create (Var.Create false, View.Const "Large", attrs = [ Checkbox.Size.large ])
      for id, labelText, labelAttrs in
        [
          "long-label-checkbox", "Keep every word of this checkbox label available in a narrow form.", []
          "unbroken-label-checkbox", "AnUnbrokenTaskIdentifierThatMustRemainFullyReadableInANarrowForm", []
          "nowrap-label-checkbox",
          "Keep this deliberately single-line label on one line.",
          [ Typography.noWrap ]
        ] do
        div [ Attr.Style "width" "220px" ] [
          Checkbox.create (
            Var.Create false,
            displayText = View.Const labelText,
            attrs = [ Attr.Create "data-testid" id; yield! labelAttrs ]
          )
        ]
      for name, color in
        [
          "primary", Checkbox.Color.primary
          "secondary", Checkbox.Color.secondary
          "tertiary", Checkbox.Color.tertiary
          "error", Checkbox.Color.error
          "warning", Checkbox.Color.warning
          "success", Checkbox.Color.success
          "info", Checkbox.Color.info
        ] do
        Checkbox.create (
          Var.Create true,
          View.Const name,
          attrs = [ color; Attr.Create "data-testid" ("checkbox-color-" + name) ]
        )
    ]

  let private radioPage () =
    let selected = Var.Create "A"

    div [] [
      Radio.create (
        selected,
        "A",
        displayText = View.Const "Option A",
        inputAttrs = [ Attr.Create "name" "test-options" ]
      )
      Radio.create (
        selected,
        "B",
        displayText = View.Const "Option B",
        inputAttrs = [ Attr.Create "name" "test-options" ]
      )
      Radio.create (
        selected,
        "C",
        displayText = View.Const "Disabled Option",
        enabled = View.Const false,
        inputAttrs = [ Attr.Create "name" "test-options" ]
      )
      Radio.create (
        Var.Create "selected",
        "selected",
        displayText = View.Const "Disabled selected",
        enabled = View.Const false,
        inputAttrs = [ Attr.Create "name" "disabled-selected" ],
        attrs = [ Attr.Create "data-testid" "disabled-selected-radio" ]
      )
      for name, color in
        [
          "primary", Radio.Color.primary
          "secondary", Radio.Color.secondary
          "tertiary", Radio.Color.tertiary
          "error", Radio.Color.error
          "warning", Radio.Color.warning
          "success", Radio.Color.success
          "info", Radio.Color.info
        ] do
        Radio.create (
          Var.Create "selected",
          "selected",
          displayText = View.Const name,
          inputAttrs = [ Attr.Create "name" ("radio-" + name) ],
          attrs = [ color; Attr.Create "data-testid" ("radio-color-" + name) ]
        )

    ]

  let private radioFormGroupsPage () =
    let first = Var.Create "A"
    let second = Var.Create "A"

    div [] [
      form [ Attr.Create "data-testid" "first-radio-form" ] [
        Radio.create (
          first,
          "A",
          displayText = View.Const "First A",
          inputAttrs = [ Attr.Create "name" "choice" ]
        )
        Radio.create (
          first,
          "B",
          displayText = View.Const "First B",
          inputAttrs = [ Attr.Create "name" "choice" ]
        )
      ]
      form [ Attr.Create "data-testid" "second-radio-form" ] [
        Radio.create (
          second,
          "A",
          displayText = View.Const "Second A",
          inputAttrs = [ Attr.Create "name" "choice" ]
        )
        Radio.create (
          second,
          "B",
          displayText = View.Const "Second B",
          inputAttrs = [ Attr.Create "name" "choice" ]
        )
      ]
    ]

  let private switchPage () =
    div [] [
      Switch.create (Var.Create false, content = text "Off Switch")
      Switch.create (Var.Create true, content = text "On Switch")
      Switch.create (Var.Create false, content = text "Disabled Switch", enabled = View.Const false)
      Switch.create (
        Var.Create true,
        content = text "Disabled On",
        enabled = View.Const false,
        attrs = [ Attr.Create "data-testid" "disabled-on-switch" ]
      )
      for name, color in
        [
          "primary", Switch.Color.primary
          "secondary", Switch.Color.secondary
          "tertiary", Switch.Color.tertiary
          "error", Switch.Color.error
          "warning", Switch.Color.warning
          "success", Switch.Color.success
          "info", Switch.Color.info
        ] do
        Switch.create (
          Var.Create true,
          content = text name,
          attrs = [ color; Attr.Create "data-testid" ("switch-color-" + name) ]
        )

    ]

  let private fieldPage () =
    div [] [
      TextField.singleLine (Var.Create "", labelText = View.Const "Standard Field")
      TextField.singleLine (
        Var.Create "some value",
        labelText = View.Const "Outlined Field",
        attrs = [ TextField.Variant.outlined ]
      )
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "Disabled Field",
        enabled = View.Const false
      )
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "With Help",
        showHelpText = View.Const true,
        helpText = text "Help text content"
      )
      TextField.singleLine (
        Var.Create "not-an-email",
        labelText = View.Const "Email",
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.create (
            text "Invalid email address",
            attrs = [ Attr.Create "id" "email-error"; FieldHelpText.Color.error ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "email-error"
        ],
        attrs = [ TextField.Color.error ]
      )
    ]

  let private numericFieldPage () =
    div [] [
      div [ Attr.Create "data-testid" "numeric-int" ] [
        NumericField.create (Var.Create 5, labelText = View.Const "Integer spin")
      ]
      div [ Attr.Create "data-testid" "numeric-float" ] [
        NumericField.create (Var.Create 5.0, labelText = View.Const "Float spin")
      ]
      NumericField.create (
        Var.Create 5,
        labelText = View.Const "Standard Numeric",
        showSpinButtons = View.Const true
      )
      NumericField.create (
        Var.Create 10,
        labelText = View.Const "Outlined Numeric",
        showSpinButtons = View.Const true,
        attrs = [ NumericField.Variant.outlined ]
      )
      NumericField.create (
        Var.Create 150,
        labelText = View.Const "Max 100",
        max = 100,
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.create (
            text "Value exceeds maximum",
            attrs = [ Attr.Create "id" "numeric-error"; FieldHelpText.Color.error ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "numeric-error"
        ],
        attrs = [ NumericField.Color.error ]
      )
    ]

  let private textFieldPage () =
    div [] [
      TextField.singleLine (Var.Create "", labelText = View.Const "Standard Text Field")
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "Outlined Text Field",
        attrs = [ TextField.Variant.outlined ]
      )
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "Disabled Text Field",
        enabled = View.Const false
      )
      TextField.singleLine (
        Var.Create "not-an-email",
        labelText = View.Const "Email",
        showHelpText = View.Const true,
        helpText =
          FieldHelpText.create (
            text "Invalid email address",
            attrs = [ Attr.Create "id" "text-email-error"; FieldHelpText.Color.error ]
          ),
        inputAttrs = [
          Attr.ariaInvalid (View.Const true)
          Attr.Create "aria-describedby" "text-email-error"
        ],
        attrs = [ TextField.Color.error ]
      )
      TextField.singleLine (
        Var.Create "",
        labelText = View.Const "Persistent label",
        placeholder = View.Const "Type here",
        startAdornment = text "@",
        attrs = [ TextField.Variant.outlined; TextField.Label.above ]
      )
      TextField.singleLine (Var.Create "", labelText = View.Const "Max Length", maxLength = 50)
      TextField.multiLine (Var.Create "", labelText = View.Const "Multiline Text Field")
    ]

  let private selectPage () =
    let dynamicDisabled = Var.Create false
    let selectEnabled = Var.Create true
    let selectReadOnly = Var.Create false

    let keyboardItems =
      [
        SelectItem.create (text "Blocked", "Blocked", "Blocked", disabled = View.Const true)
        SelectItem.create (text "Alpha", "Alpha", "Alpha")
        SelectItem.create (text "Dynamic", "Dynamic", "Dynamic", disabled = dynamicDisabled.View)
        SelectItem.create (text "Omega", "Omega", "Omega")
      ]
      |> Var.Create

    let items =
      [ "Apple"; "Banana"; "Cherry" ]
      |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
      |> View.Const

    let multiItems =
      [ "Red"; "Green"; "Blue" ]
      |> List.map (fun c -> SelectItem.create (text c, c, c))
      |> View.Const

    div [] [
      Select.create (
        items,
        Var.Create<string option> None,
        labelText = View.Const "Single Select",
        placeholder = View.Const "Choose a fruit"
      )
      Select.create (
        items,
        Var.Create<string option>(Some "Apple"),
        labelText = View.Const "With Value",
        clearable = View.Const true
      )
      Select.create (
        items,
        Var.Create<string option> None,
        labelText = View.Const "Disabled Select",
        enabled = View.Const false
      )
      Select.create (
        items,
        Var.Create<string option> None,
        labelText = View.Const "Searchable",
        searchable = true
      )
      MultiSelect.create (
        multiItems,
        Var.Create(set [ "Red"; "Blue" ]),
        labelText = View.Const "Multi Select"
      )
      div [ Attr.Create "data-testid" "keyboard-single" ] [
        Select.create (
          keyboardItems.View,
          Var.Create<string option> None,
          searchable = true,
          clearable = View.Const true,
          labelText = View.Const "Keyboard single"
        )
      ]
      div [ Attr.Create "data-testid" "keyboard-multi" ] [
        MultiSelect.create (
          keyboardItems.View,
          Var.Create(Set.empty<string>),
          searchable = true,
          clearable = View.Const true,
          labelText = View.Const "Keyboard multi"
        )
      ]
      div [ Attr.Create "data-testid" "restricted-single" ] [
        Select.create (
          keyboardItems.View,
          Var.Create<string option> None,
          searchable = true,
          clearable = View.Const true,
          enabled = selectEnabled.View,
          readOnly = selectReadOnly.View,
          labelText = View.Const "Restricted single"
        )
      ]
      div [ Attr.Create "data-testid" "restricted-multi" ] [
        MultiSelect.create (
          keyboardItems.View,
          Var.Create(Set.empty<string>),
          searchable = true,
          clearable = View.Const true,
          enabled = selectEnabled.View,
          readOnly = selectReadOnly.View,
          labelText = View.Const "Restricted multi"
        )
      ]
      div [ Attr.Create "data-testid" "select-all-multi" ] [
        MultiSelect.create (
          keyboardItems.View,
          Var.Create(Set.empty<string>),
          searchable = true,
          showSelectAll = true,
          labelText = View.Const "Select all options"
        )
      ]
      div [ Attr.Create "data-testid" "select-all-no-search" ] [
        MultiSelect.create (
          keyboardItems.View,
          Var.Create(Set.empty<string>),
          showSelectAll = true,
          labelText = View.Const "Select all without search"
        )
      ]
      div [ Attr.Create "data-testid" "select-palette" ] [
        for name, color in
          [
            "primary", Select.Color.primary
            "secondary", Select.Color.secondary
            "tertiary", Select.Color.tertiary
            "error", Select.Color.error
            "warning", Select.Color.warning
            "success", Select.Color.success
            "info", Select.Color.info
          ] do
          Select.create (
            items,
            Var.Create<string option>(Some "Apple"),
            labelText = View.Const(name + " select"),
            attrs = [ color; Attr.Create "data-testid" ("select-color-" + name) ]
          )
      ]
      Button.create (
        text "Toggle enabled",
        (fun () -> Var.Set selectEnabled (not selectEnabled.Value)),
        attrs = [ Attr.Create "data-testid" "toggle-select-enabled" ]
      )
      Button.create (
        text "Toggle read only",
        (fun () -> Var.Set selectReadOnly (not selectReadOnly.Value)),
        attrs = [ Attr.Create "data-testid" "toggle-select-readonly" ]
      )
      Button.create (
        text "Disable dynamic option",
        (fun () -> Var.Set dynamicDisabled true),
        attrs = [ Attr.Create "data-testid" "disable-option" ]
      )
      Button.create (
        text "Reorder options",
        (fun () -> Var.Set keyboardItems (List.rev keyboardItems.Value)),
        attrs = [ Attr.Create "data-testid" "reorder-options" ]
      )
      Button.create (
        text "Disable all options",
        (fun () ->
          keyboardItems.Value
          |> List.map (fun item -> { item with Disabled = View.Const true })
          |> Var.Set keyboardItems),
        attrs = [ Attr.Create "data-testid" "disable-all-options" ]
      )
    ]

  let private buttonPage () =
    let activations = Var.Create 0

    div [] [
      Button.create (text "Filled Button", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.create (text "Outlined Button", (fun () -> ()), attrs = [ Button.Variant.outlined ])
      Button.create (
        text "Disabled Button",
        (fun () -> ()),
        enabled = View.Const false,
        attrs = [ Button.Variant.filled ]
      )
      IconButton.primary (
        Icon.create (Icon.UiActions UiActions.Home),
        (fun () -> ()),
        attrs = [ Attr.Create "aria-label" "home"; Button.Variant.filled ]
      )
      div [ Attr.Create "data-testid" "activation-count" ] [
        Doc.TextView(activations.View |> View.Map string)
      ]
      Button.create (
        text "Count activation",
        (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [ Attr.Create "data-testid" "activation-button" ]
      )
      IconButton.create (
        text "Icon",
        (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [
          Attr.Create "data-testid" "activation-icon"
          Attr.Create "aria-label" "Count icon"
        ]
      )
      Button.create (
        text "Disabled count",
        (fun () -> Var.Set activations (activations.Value + 1)),
        enabled = View.Const false,
        attrs = [ Attr.Create "data-testid" "activation-disabled" ]
      )
      IconButton.create (
        text "Disabled icon",
        (fun () -> Var.Set activations (activations.Value + 1)),
        enabled = View.Const false,
        attrs = [
          Attr.Create "data-testid" "activation-disabled-icon"
          Attr.Create "aria-label" "Disabled count icon"
        ]
      )
      Button.primary (text "Primary", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.secondary (text "Secondary", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.tertiary (text "Tertiary", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.success (text "Success", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.warning (text "Warning", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.info (text "Info", (fun () -> ()), attrs = [ Button.Variant.filled ])
      Button.error (text "Error", (fun () -> ()), attrs = [ Button.Variant.filled ])
      div [
        SurfaceColor.BackgroundColor.surface
        Attr.Create "data-testid" "button-palette"
      ] [
        for name, color in
          [
            "primary", Button.Color.primary
            "secondary", Button.Color.secondary
            "tertiary", Button.Color.tertiary
            "error", Button.Color.error
            "warning", Button.Color.warning
            "success", Button.Color.success
            "info", Button.Color.info
          ] do
          for variantName, variant in
            [
              "filled", Button.Variant.filled
              "outlined", Button.Variant.outlined
              "text", Button.Variant.text
            ] do
            Button.create (
              text (name + " " + variantName),
              (fun () -> ()),
              attrs = [
                color
                variant
                Attr.Create "data-testid" ("button-contrast-" + name + "-" + variantName)
              ]
            )
      ]
    ]

  let private dialogPage () =
    let isOpen = Var.Create true

    isOpen.View
    |> Doc.BindView(fun visible ->
      if visible then
        Dialog.create (
          DialogTitle.create (text "Dialog Title"),
          Doc.Concat [
            DialogContent.create (div [ Typography.body1 ] [ text "Dialog content goes here." ])
            DialogActions.create (
              Button.create (text "Action", (fun () -> ()), attrs = [ Button.Variant.filled ])
            )
          ],
          dialogInteraction = View.Const(Dialog.Interaction.Optional(fun () -> isOpen.Value <- false))
        )
      else
        Doc.Empty)

  let private dialogWithDrawerPage () =
    DrawerContainer.create (
      mainContent = Dialog.create (DialogTitle.create (text "Dialog"), DialogContent.create (text "Content")),
      leftDrawer =
        Drawer.create (
          div [] [ text "Navigation" ],
          View.Const true,
          variant = Drawer.Variant.Responsive,
          isFixed = false,
          attrs = [ Attr.Create "data-testid" "dialog-test-drawer" ]
        ),
      attrs = [ Attr.Style "height" "100vh" ]
    )

  let private dialogForcePage () =
    Dialog.create (
      DialogTitle.create (text "Force Dialog Title"),
      DialogContent.create (
        div [] [
          div [ Typography.body1 ] [ text "You must complete this action." ]
          div [] [
            Button.create (text "Confirm", (fun () -> ()), attrs = [ Button.Variant.filled ])
          ]
        ]
      ),
      dialogInteraction = View.Const Dialog.Interaction.Force
    )

  let private dialogTriggeredPage () =
    let isOpen = Var.Create false

    div [] [
      Button.create (
        text "Open Dialog",
        (fun () -> isOpen.Value <- true),
        attrs = [ Attr.Create "id" "open-dialog-btn"; Button.Variant.filled ]
      )
      isOpen.View
      |> Doc.BindView(fun visible ->
        if visible then
          Dialog.create (
            DialogTitle.create (text "Triggered Dialog"),
            DialogContent.create (
              div [] [
                div [ Typography.body1 ] [ text "Dialog opened by button." ]
                div [] [
                  Button.create (text "Action", (fun () -> ()), attrs = [ Button.Variant.filled ])
                ]
              ]
            ),
            dialogInteraction = View.Const(Dialog.Interaction.Optional(fun () -> isOpen.Value <- false))
          )
        else
          Doc.Empty)
    ]

  let private tabsPage () =
    let tabs =
      [
        TabItem.createCustom (text "Tab 1", div [] [ text "Panel 1 content" ])
        TabItem.createCustom (text "Tab 2", div [] [ text "Panel 2 content" ])
        TabItem.createCustom (
          text "Disabled Tab",
          div [] [ text "Disabled panel" ],
          disabled = View.Const true
        )
      ]
      |> View.Const

    Tabs.create (tabs)

  let private dropdownPage () =
    let selectedAction = Var.Create "none"

    div [] [
      div [ Attr.Create "data-testid" "dropdown-action" ] [ Doc.TextView selectedAction.View ]
      DropdownMenu.create (
        text "Open Menu",
        View.Const [
          DropdownMenuItem.create (text "Item 1", (fun () -> Var.Set selectedAction "item 1"))
          DropdownMenuItem.create (text "Item 2", (fun () -> ()))
          DropdownMenuItem.create (text "Disabled Item", (fun () -> ()), disabled = View.Const true)
        ]
      )
      Button.create (text "After", (fun () -> ()), attrs = [ Attr.Create "data-testid" "focus-target" ])
    ]

  let private expansionPanelPage () =
    let expanded1 = Var.Create true
    let expanded2 = Var.Create false
    let expanded3 = Var.Create false
    let disabledExpanded = Var.Create false

    ExpansionPanelContainer.create (
      [
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Expanded Panel",
            expanded1,
            icon = ExpansionPanelHeader.defaultIcon expanded1
          ),
          ExpansionPanelContent.create (div [ Typography.body1 ] [ text "Expanded content" ]),
          expanded = expanded1
        )
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Collapsed Panel",
            expanded2,
            icon = ExpansionPanelHeader.defaultIcon expanded2
          ),
          ExpansionPanelContent.create (
            div [ Typography.body1 ] [
              text "Collapsed content"
              Button.create (
                text "Panel action",
                ignore,
                attrs = [ Attr.Create "data-testid" "collapsed-panel-action" ]
              )
            ]
          ),
          expanded = expanded2
        )
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Focus Color Panel",
            expanded3,
            icon = ExpansionPanelHeader.defaultIcon expanded3,
            attrs = [ ExpansionPanel.FocusColor.error ]
          ),
          ExpansionPanelContent.create (div [ Typography.body1 ] [ text "Focus color content" ]),
          expanded = expanded3
        )
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Disabled Panel",
            disabledExpanded,
            enabled = View.Const false,
            attrs = [ Attr.Create "data-testid" "disabled-expansion-header" ]
          ),
          ExpansionPanelContent.create (text "Disabled panel content"),
          expanded = disabledExpanded
        )
      ]
    )

  let private dynamicExpansionPanelPage () =
    let expanded = Var.Create false
    let panelEnabled = Var.Create true
    let headerEnabled = Var.Create true

    div [] [
      ExpansionPanelContainer.create [
        ExpansionPanel.create (
          ExpansionPanelHeader.create (
            text "Dynamic header",
            expanded,
            enabled = headerEnabled.View,
            attrs = [ Attr.Create "data-testid" "dynamic-expansion-header" ]
          ),
          ExpansionPanelContent.create (text "Panel content"),
          expanded = expanded,
          enabled = panelEnabled.View
        )
      ]
      Button.create (
        text "Toggle panel",
        (fun () -> panelEnabled.Value <- not panelEnabled.Value),
        attrs = [ Attr.Create "data-testid" "toggle-panel-enabled" ]
      )
      Button.create (
        text "Toggle header",
        (fun () -> headerEnabled.Value <- not headerEnabled.Value),
        attrs = [ Attr.Create "data-testid" "toggle-header-enabled" ]
      )
    ]

  let private alertPage () =
    div [] [
      Alert.create (text "Success alert message", attrs = [ Alert.Color.success ])
      Alert.create (text "Error alert message", attrs = [ Alert.Color.error ])
      Alert.create (text "Warning alert message", attrs = [ Alert.Color.warning ])
      Alert.create (text "Info alert message", attrs = [ Alert.Color.info ])
      for name, color in
        [
          "primary", Alert.Color.primary
          "secondary", Alert.Color.secondary
          "tertiary", Alert.Color.tertiary
          "error", Alert.Color.error
          "warning", Alert.Color.warning
          "success", Alert.Color.success
          "info", Alert.Color.info
        ] do
        Alert.create (
          text (name + " standard"),
          onClose = (fun () -> ()),
          attrs = [
            color
            Alert.Variant.standard
            Attr.Create "data-testid" ("alert-" + name + "-standard")
          ]
        )

        Alert.create (
          text (name + " outlined"),
          onClose = (fun () -> ()),
          attrs = [
            color
            Alert.Variant.outlined
            Attr.Create "data-testid" ("alert-" + name + "-outlined")
          ]
        )

        Alert.create (
          text (name + " filled"),
          onClose = (fun () -> ()),
          attrs = [
            color
            Alert.Variant.filled
            Attr.Create "data-testid" ("alert-" + name + "-filled")
          ]
        )
    ]

  let private cardPage () =
    let opened = Var.Create "none"

    div [] [
      Card.create (
        Doc.Concat [
          CardHeader.create (
            h2 [] [ text "Bordered card" ],
            subtitle = text "Card subtitle",
            leading = Icon.create (Icon.UiActions UiActions.Home),
            actions =
              IconButton.create (
                Icon.create (Icon.UiActions UiActions.Home),
                (fun () -> ()),
                attrs = [ Attr.Create "aria-label" "home" ]
              )
          )
          CardContent.create (div [ Typography.body1 ] [ text "Card body content" ])
          CardActions.create (Button.create (text "Action", (fun () -> ()), attrs = [ Button.Variant.text ]))
        ],
        attrs = [ Attr.Create "data-testid" "bordered-card" ]
      )
      Card.create (
        Doc.Concat [
          CardHeader.create (text "Elevated card")
          CardContent.create (div [ Typography.body1 ] [ text "Elevated content" ])
        ],
        attrs = [ Card.Variant.elevated ]
      )
      Card.create (
        CardContent.create (div [ Typography.body1 ] [ text "Filled content" ]),
        attrs = [ Card.Variant.filled; Attr.Create "data-testid" "filled-card" ]
      )
      Card.create (
        Doc.Concat [
          CardActionArea.create (
            Doc.Concat [
              CardMedia.create (div [ Attr.Style "height" "80px" ] [])
              CardHeader.create (h2 [] [ text "Clickable card" ])
              CardContent.create (div [ Typography.body1 ] [ text "Opens the card" ])
            ],
            onClick = (fun () -> Var.Set opened "clickable")
          )
          CardActions.create (
            Button.create (
              text "Secondary",
              (fun () -> Var.Set opened "secondary"),
              attrs = [ Button.Variant.text ]
            )
          )
        ],
        attrs = [ Attr.Create "data-testid" "action-card" ]
      )
      Card.create (
        CardActionArea.create (CardContent.create (text "Linked card"), href = "#card"),
        attrs = [ Attr.Create "data-testid" "link-card" ]
      )
      div [ Attr.Create "data-testid" "opened-card" ] [ textView opened.View ]
    ]

  let private tablePage () =
    let clicked = Var.Create "none"
    let selected = Var.Create(None: string option)
    let checkedRows = Var.Create(Set.ofList [ "Alpha" ])

    let externalSort =
      Var.Create(
        Some {
          Column = "count"
          Direction = Table.SortDirection.Descending
        }
        : Table.SortState option
      )

    let rows = [ "Beta", 2; "Alpha", 3; "Gamma", 1 ]

    let columns () = [
      TableColumn.create (
        "name",
        text "Name",
        (fun (name: string, _: int) -> text name),
        sort = Table.ColumnSort.by fst
      )
      TableColumn.create ("count", text "Count", (fun (_: string, count: int) -> text (string<int> count)))
    ]

    let externalColumns = [
      TableColumn.create ("name", text "Name", (fun (name: string, _: int) -> text name))
      TableColumn.create (
        "count",
        text "Count",
        (fun (_: string, count: int) -> text (string<int> count)),
        sort = Table.ColumnSort.External
      )
    ]

    let externallySorted =
      externalSort.View
      |> View.Map(fun state ->
        match state with
        | Some { Direction = Table.SortDirection.Ascending } -> List.sortBy snd rows
        | Some { Direction = Table.SortDirection.Descending } -> List.sortByDescending snd rows
        | None -> rows)

    let describeSort (state: Table.SortState option) =
      match state with
      | Some {
               Column = column
               Direction = Table.SortDirection.Ascending
             } -> column + " ascending"
      | Some {
               Column = column
               Direction = Table.SortDirection.Descending
             } -> column + " descending"
      | None -> "unsorted"

    div [] [
      Table.create (
        columns (),
        View.Const rows,
        fst,
        onRowClick = (fun (name, _) -> Var.Set clicked name),
        selectedKey = selected,
        caption = text "Clickable rows",
        attrs = [
          Attr.Create "data-testid" "clickable-table"
          Table.striped
          Table.StripeColor.custom "rgb(255, 0, 0)"
          Table.HoverColor.custom "rgb(0, 0, 255)"
        ]
      )
      div [ Attr.Create "data-testid" "clicked-row" ] [ textView clicked.View ]
      Table.create (
        columns (),
        View.Const [],
        fst,
        emptyContent = text "No rows",
        caption = text "Empty rows",
        attrs = [
          Attr.Create "data-testid" "empty-table"
          Table.Density.compact
          Table.hiddenCaption
        ]
      )
      Table.create (
        columns (),
        View.Const rows,
        fst,
        caption = text "Sticky rows",
        attrs = [
          Attr.Create "data-testid" "sticky-table"
          Table.stickyHeader
          Table.Density.spacious
          Table.hiddenCaption
          Attr.Style "max-height" "120px"
        ]
      )
      Table.create (
        columns (),
        View.Const rows,
        fst,
        selectedKeys = checkedRows,
        rowHeader = "name",
        caption = text "Selectable rows",
        attrs = [ Attr.Create "data-testid" "select-table"; Table.hiddenCaption ]
      )
      Table.create (
        columns (),
        View.Const rows,
        fst,
        selectedKey = Var.Create(None: string option),
        selectedKeys = Var.Create(Set.empty: Set<string>),
        caption = text "Current and checked rows",
        attrs = [ Attr.Create "data-testid" "current-and-checked-table"; Table.hiddenCaption ]
      )
      div [ Attr.Create "data-testid" "checked-rows" ] [
        textView (checkedRows.View |> View.Map(Set.toList >> String.concat ","))
      ]
      Table.create (
        externalColumns,
        externallySorted,
        fst,
        sort = externalSort,
        caption = text "Externally sorted rows",
        attrs = [ Attr.Create "data-testid" "external-table"; Table.hiddenCaption ]
      )
      div [ Attr.Create "data-testid" "external-sort" ] [
        textView (externalSort.View |> View.Map describeSort)
      ]
    ]

  let private defaultTablePage () =
    let rows = [ "Alpha"; "Beta" ]
    let selected = Var.Create(Some "Alpha")
    let columns = [ TableColumn.create ("name", text "Name", (fun (row: string) -> text row)) ]

    Table.create (
      columns,
      View.Const rows,
      id,
      selectedKey = selected,
      caption = text "Default rows",
      attrs = [ Table.hoverable ]
    )

  let private themePage useSystemPreference =
    let initialMode =
      if useSystemPreference then
        Theming.detectSystemPreference ()
      else
        Theming.Light

    Theming.setMode initialMode

    let recordMode mode =
      let value = if mode = Theming.Dark then "dark" else "light"
      JS.Document.Body.SetAttribute("data-observed-mode", value)

    div [] [
      Button.create (
        text "Primary",
        ignore,
        attrs = [
          Attr.Create "data-testid" "theme-swatch"
          Button.Variant.filled
          Button.Color.primary
        ]
      )
      for name, fill, color in
        [
          "primary", BrandColor.Fill.primary, Button.Color.primary
          "secondary", BrandColor.Fill.secondary, Button.Color.secondary
          "tertiary", BrandColor.Fill.tertiary, Button.Color.tertiary
          "error", BrandColor.Fill.error, Button.Color.error
          "warning", BrandColor.Fill.warning, Button.Color.warning
          "success", BrandColor.Fill.success, Button.Color.success
          "info", BrandColor.Fill.info, Button.Color.info
        ] do
        div [ fill; Attr.Create "data-testid" ("theme-fill-" + name) ] [ text name ]

        Button.create (
          text name,
          ignore,
          attrs = [
            color
            Button.Variant.filled
            Attr.Create "data-testid" ("theme-button-" + name)
          ]
        )
      div [ Attr.Class "weave-card"; Attr.Create "data-testid" "theme-card" ] [ text "Card" ]
      div [ Typography.body1; Attr.Create "data-testid" "theme-body" ] [ text "Body" ]
      Button.create (
        text "Toggle",
        (fun () -> Theming.toggleMode () |> recordMode),
        attrs = [ Attr.Create "data-testid" "theme-toggle" ]
      )
      Button.create (
        text "Light",
        (fun () ->
          Theming.setMode Theming.Light
          Theming.getMode () |> recordMode),
        attrs = [ Attr.Create "data-testid" "theme-light" ]
      )
      Button.create (
        text "Dark",
        (fun () ->
          Theming.setMode Theming.Dark
          Theming.getMode () |> recordMode),
        attrs = [ Attr.Create "data-testid" "theme-dark" ]
      )
    ]

  let private plainDialogPage () =
    Dialog.create (text "Plain dialog title", text "Dialog content")

  let private appbarPage () =
    div [] [
      AppBar.create (div [] [ text "App Bar Title" ], attrs = [ Attr.Create "aria-label" "Static app bar" ])
    ]

  let private listenerRemovalPage () =
    let mounted = Var.Create true
    let active = Var.Create true
    let calls = Var.Create 0
    let target = ref (JS.Document.CreateElement "div")

    div [] [
      Button.create (
        text "Toggle listener",
        (fun () -> Var.Set mounted (not mounted.Value)),
        attrs = [ Attr.Create "data-testid" "remove-listener" ]
      )
      Button.create (text "Outside", ignore, attrs = [ Attr.Create "data-testid" "listener-outside" ])
      div [ Attr.Create "data-testid" "listener-calls" ] [ textView (calls.View |> View.Map string) ]
      mounted.View
      |> Doc.BindView(fun visible ->
        if visible then
          div [
            Attr.Create "data-testid" "listener-owner"
            on.afterRender (fun el -> target.Value <- el)
          ] [
            DocumentEventListener.onMouseDown
              [ target ]
              (fun () -> Var.Set calls (calls.Value + 1))
              active.View
            text "Owner"
          ]
        else
          Doc.Empty)
    ]

  let private drawerPage variant initiallyOpen =
    let isOpen = Var.Create initiallyOpen

    div [] [
      Button.create (
        text "Toggle Drawer",
        (fun () -> Var.Set isOpen (not isOpen.Value)),
        attrs = [ Attr.Create "data-testid" "drawer-toggle"; Button.Variant.filled ]
      )

      DrawerContainer.create (
        mainContent =
          div [] [
            Button.create (
              text "Main Focus Target",
              (fun () -> ()),
              attrs = [ Attr.Create "data-testid" "main-focus-target" ]
            )
          ],
        leftDrawer =
          Drawer.create (
            div [] [
              Button.create (
                text "Drawer Button",
                (fun () -> ()),
                attrs = [ Attr.Create "data-testid" "drawer-button" ]
              )
            ],
            isOpen.View,
            variant = variant,
            overlayClose = (fun () -> Var.Set isOpen false),
            attrs = [ Attr.Create "aria-label" "Navigation drawer" ]
          )
      )
    ]

  let private linkPage () =
    let activations = Var.Create 0

    div [] [
      div [ Attr.Create "data-testid" "link-count" ] [ Doc.TextView(activations.View |> View.Map string) ]
      Link.create (
        text "Count link",
        href = "#link",
        onClick = (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [ Attr.Create "data-testid" "count-link" ]
      )
      IconLink.create (
        text "Icon",
        href = "#link",
        onClick = (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [
          Attr.Create "data-testid" "count-icon-link"
          Attr.Create "aria-label" "Count icon link"
        ]
      )
      Link.create (
        text "Disabled count link",
        href = "#other",
        enabled = View.Const false,
        onClick = (fun () -> Var.Set activations (activations.Value + 1)),
        attrs = [ Attr.Create "data-testid" "disabled-count-link" ]
      )
      Link.create (text "Default Link", href = "#")
      Link.create (text "Disabled Link", href = "#", enabled = View.Const false)
      Link.create (text "Always Underline", href = "#", attrs = [ Link.Underline.always ])
      Link.create (text "No Underline", href = "#", attrs = [ Link.Underline.none ])
      div [ SurfaceColor.BackgroundColor.paper; Padding.All.small ] [
        for name, colorAttrs in
          [
            "default", []
            "primary", [ Link.Color.primary ]
            "secondary", [ Link.Color.secondary ]
            "tertiary", [ Link.Color.tertiary ]
            "success", [ Link.Color.success ]
            "error", [ Link.Color.error ]
            "warning", [ Link.Color.warning ]
            "info", [ Link.Color.info ]
          ] do
          Link.create (
            text ("Readable " + name + " link"),
            href = "#button",
            attrs = [ Attr.Create "data-testid" ("paper-link-" + name); yield! colorAttrs ]
          )
      ]
    ]

  let private listPage () =
    let selected = Var.Create<string option> None
    let nestedSelected = Var.Create(Set.empty<string>)
    let nestedExpanded = Var.Create false

    div [] [
      selected.View
      |> Doc.BindView(fun s ->
        div [ Attr.Create "data-testid" "list-selection" ] [
          text (
            match s with
            | Some v -> v
            | None -> "none"
          )
        ])

      WeaveList.create (
        [
          ListItem.create (text "Item 1", value = "1")
          ListItem.create (text "Item 2", value = "2")
          ListItem.create (text "Item 3", value = "3")
          ListItem.create (text "Disabled Item", value = "4", disabled = View.Const true)
        ],
        selectedValue = selected,
        attrs = [ Attr.Create "aria-label" "Test items" ]
      )
      div [ Attr.Create "data-testid" "nested-selected" ] [
        Doc.TextView(
          nestedSelected.View
          |> View.Map(fun values -> values |> Set.toList |> String.concat ",")
        )
      ]
      div [ Attr.Create "data-testid" "selected-palette" ] [
        for name, color in
          [
            "primary", WeaveList.Color.primary
            "secondary", WeaveList.Color.secondary
            "tertiary", WeaveList.Color.tertiary
            "error", WeaveList.Color.error
            "warning", WeaveList.Color.warning
            "success", WeaveList.Color.success
            "info", WeaveList.Color.info
          ] do
          WeaveList.create (
            [
              ListItem.create (
                text (name + " item"),
                value = name,
                secondaryContent = text "Secondary detail",
                attrs = [ color ]
              )
            ],
            selectedValue = Var.Create(Some name)
          )
      ]
      div [ Attr.Create "data-testid" "nested-list" ] [
        WeaveList.create (
          [
            ListItem.create (
              text "Group",
              expanded = nestedExpanded,
              nestedChildren = [
                ListItem.create (text "A", value = "a")
                ListItem.create (text "B", value = "b")
              ]
            )
          ],
          selectedValues = nestedSelected,
          selectionMode = WeaveList.SelectionMode.MultiSelection
        )
      ]
    ]

  let private chipPage () =
    let clicked = Var.Create "none"
    let chipActions = Var.Create 0
    let chipCloses = Var.Create 0

    div [] [
      clicked.View
      |> Doc.BindView(fun v -> div [ Attr.Create "data-testid" "chip-clicked" ] [ text v ])

      Chip.create (
        text "Clickable Chip",
        onClick = (fun () -> Var.Set clicked "clicked"),
        attrs = [ Chip.Variant.filled; Attr.Create "data-testid" "clickable-chip" ]
      )
      Chip.create (text "Default Chip", attrs = [ Chip.Variant.filled ])
      for name, color in
        [
          "primary", Chip.Color.primary
          "secondary", Chip.Color.secondary
          "tertiary", Chip.Color.tertiary
          "error", Chip.Color.error
          "warning", Chip.Color.warning
          "success", Chip.Color.success
          "info", Chip.Color.info
        ] do
        for variantName, variant in
          [
            "filled", Chip.Variant.filled
            "outlined", Chip.Variant.outlined
            "text", Chip.Variant.text
          ] do
          Chip.create (
            text (name + " " + variantName),
            onClick = ignore,
            attrs = [
              color
              variant
              Attr.Create "data-testid" ("chip-color-" + name + "-" + variantName)
            ]
          )

      Chip.create (
        text "Disabled Chip",
        onClick = (fun () -> ()),
        enabled = View.Const false,
        attrs = [ Chip.Variant.filled; Attr.Create "data-testid" "disabled-chip" ]
      )
      Chip.create (text "Deletable Chip", onClose = (fun () -> ()), attrs = [ Chip.Variant.outlined ])
      div [ Attr.Create "data-testid" "chip-actions" ] [ Doc.TextView(chipActions.View |> View.Map string) ]
      div [ Attr.Create "data-testid" "chip-closes" ] [ Doc.TextView(chipCloses.View |> View.Map string) ]
      Chip.create (
        text "Both",
        onClick = (fun () -> Var.Set chipActions (chipActions.Value + 1)),
        onClose = (fun () -> Var.Set chipCloses (chipCloses.Value + 1)),
        attrs = [ Attr.Create "data-testid" "chip-both" ]
      )
      Chip.create (
        text "Disabled close",
        onClose = (fun () -> Var.Set chipCloses (chipCloses.Value + 1)),
        enabled = View.Const false,
        attrs = [ Attr.Create "data-testid" "chip-disabled-close" ]
      )
      Chip.create (
        text "Chip link",
        href = "#chip",
        onClick = (fun () -> Var.Set chipActions (chipActions.Value + 1)),
        attrs = [ Attr.Create "data-testid" "chip-link" ]
      )
      Chip.create (
        text "Closable link",
        href = "#other",
        onClick = (fun () -> Var.Set chipActions (chipActions.Value + 1)),
        onClose = (fun () -> Var.Set chipCloses (chipCloses.Value + 1)),
        attrs = [ Attr.Create "data-testid" "closable-chip-link" ]
      )
    ]

  let private chipsetPage () =
    let selected = Var.Create<string option> None

    div [] [
      selected.View
      |> Doc.BindView(fun s ->
        div [ Attr.Create "data-testid" "chipset-selection" ] [
          text (
            match s with
            | Some v -> v
            | None -> "none"
          )
        ])

      ChipSet.create (
        [
          ChipItem.create (text "Chip A", "a", attrs = [ Chip.Variant.outlined ])
          ChipItem.create (text "Chip B", "b", attrs = [ Chip.Variant.outlined ])
          ChipItem.create (text "Chip C", "c", attrs = [ Chip.Variant.outlined ])
          ChipItem.create (
            text "Disabled Chip",
            "d",
            disabled = View.Const true,
            attrs = [ Chip.Variant.outlined ]
          )
        ],
        selectedValue = selected,
        selectionMode = ChipSet.SelectionMode.Toggle
      )
      div [ Attr.Create "data-testid" "closable-chipset" ] [
        ChipSet.create (
          [
            ChipItem.create (text "One", "one", closable = true)
            ChipItem.create (text "Disabled", "disabled", closable = true, disabled = View.Const true)
            ChipItem.create (text "Three", "three", closable = true)
          ],
          selectedValue = Var.Create None,
          onClose = (fun _ -> ())
        )
      ]
    ]

  let private buttonmenuPage () =
    let action = Var.Create "none"

    let menuItem label =
      Button.create (
        text label,
        (fun () -> action.Set label),
        attrs = [ Button.Variant.text; Button.Width.full; BorderRadius.All.none ]
      )

    div [] [
      ButtonMenu.create (text "Menu", [ menuItem "Action 1"; menuItem "Action 2" ])
      ButtonMenu.create (
        text "Horizontal Menu",
        [ menuItem "Left 1"; menuItem "Left 2" ],
        direction = ButtonMenu.Direction.Right,
        attrs = [ Attr.Create "data-testid" "horizontal-menu" ]
      )
      ButtonMenu.create (
        text "Edge cases",
        [
          Button.create (text "Unavailable", (fun () -> action.Set "disabled"), enabled = View.Const false)
          div [] [
            Link.create (
              text "Wrapped link",
              href = "#menu-link-target",
              onClick = (fun () -> action.Set "link")
            )
          ]
        ],
        attrs = [ Attr.Create "data-testid" "edge-menu" ]
      )
      div [ Attr.Create "data-testid" "menu-action" ] [ textView action.View ]
      Button.create (
        text "After",
        (fun () -> ()),
        attrs = [
          Attr.Create "data-testid" "focus-target"
          Attr.Create "id" "menu-link-target"
        ]
      )
    ]

  let private buttongroupPage () =
    div [] [
      ButtonGroup.create (
        [
          Button.create (text "Left", (fun () -> ()), attrs = [ Button.Variant.outlined ])
          Button.create (text "Center", (fun () -> ()), attrs = [ Button.Variant.outlined ])
          Button.create (text "Right", (fun () -> ()), attrs = [ Button.Variant.outlined ])
        ]
      )
      ButtonGroup.create (
        [
          Button.create (text "Reply", ignore)
          ButtonMenu.create (
            text "Menu",
            [
              Button.create (
                text "Reply All",
                ignore,
                attrs = [
                  Button.Variant.filled
                  Button.Width.full
                  Attr.Create "data-testid" "grouped-split-action"
                ]
              )
            ],
            direction = ButtonMenu.Direction.Bottom,
            attrs = [ Attr.Create "data-testid" "grouped-split-menu" ]
          )
        ],
        attrs = [
          ButtonGroup.Variant.outlined
          ButtonGroup.Color.primary
          Attr.Create "data-testid" "grouped-split-button"
        ]
      )
    ]

  let private filledButtonGroupPalettePage () =
    div [] [
      Button.create (text "Before palette", onClick = ignore)
      for name, color in
        [
          "primary", ButtonGroup.Color.primary
          "secondary", ButtonGroup.Color.secondary
          "tertiary", ButtonGroup.Color.tertiary
          "error", ButtonGroup.Color.error
          "warning", ButtonGroup.Color.warning
          "success", ButtonGroup.Color.success
          "info", ButtonGroup.Color.info
        ] do
        ButtonGroup.create (
          [
            Button.create (text "First", onClick = ignore)
            Button.create (text "Second", onClick = ignore)
          ],
          attrs = [
            ButtonGroup.Variant.filled
            color
            Attr.Create "data-testid" ("group-color-" + name + "-filled")
          ]
        )
    ]

  let private tooltipPage () =
    div [ Attr.Create "style" "padding: 16px" ] [
      Tooltip.create (
        Button.create (text "Hover me", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Tooltip text",
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-default" ]
      )
      Tooltip.create (
        Button.create (text "Click me", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Click tooltip",
        activationEvents = [ Tooltip.Activation.Click ],
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-click" ]
      )
      Tooltip.create (
        Button.create (text "Bottom", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Bottom tooltip",
        direction = Tooltip.Direction.Bottom,
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-bottom" ]
      )
      Tooltip.create (
        Button.create (text "Left", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Left tooltip",
        direction = Tooltip.Direction.Left,
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-left" ]
      )
      Tooltip.create (
        Button.create (text "Right", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Right tooltip",
        direction = Tooltip.Direction.Right,
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-right" ]
      )
      Tooltip.create (
        Button.create (text "No arrow", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "No arrow tooltip",
        showArrow = false,
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-no-arrow" ]
      )
      Tooltip.create (
        Button.create (text "Primary", (fun () -> ()), attrs = [ Button.Variant.filled ]),
        text "Primary tooltip",
        tooltipAttrs = [ Tooltip.Color.primary ],
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-primary" ]
      )
    ]

  let private tooltipViewportPage () =
    let mounted = Var.Create true
    let detail = Var.Create(String.replicate 30 "LongContent")

    let edge direction name placement =
      Tooltip.create (
        Button.create (text name, ignore),
        text ("Viewport " + name + " tooltip"),
        direction = direction,
        wrapperAttrs = [
          Attr.Create "data-testid" ("tooltip-edge-" + name.ToLower())
          Attr.Style "position" "absolute"
          yield! placement
        ]
      )

    div [ Attr.Style "position" "relative"; Attr.Style "min-height" "1000px" ] [
      edge Tooltip.Direction.Right "Right" [ Attr.Style "left" "200px"; Attr.Style "top" "120px" ]
      edge Tooltip.Direction.Left "Left" [ Attr.Style "left" "8px"; Attr.Style "top" "180px" ]
      edge Tooltip.Direction.Top "Top" [ Attr.Style "left" "120px"; Attr.Style "top" "8px" ]
      Tooltip.create (
        Button.create (text "Bottom", ignore),
        text "Viewport Bottom tooltip",
        direction = Tooltip.Direction.Bottom,
        wrapperAttrs = [
          Attr.Create "data-testid" "tooltip-edge-bottom"
          Attr.Style "position" "fixed"
          Attr.Style "left" "120px"
          Attr.Style "bottom" "8px"
        ]
      )
      mounted.View
      |> Doc.BindView(fun visible ->
        if visible then
          Tooltip.create (
            Button.create (text "Long content", ignore),
            Doc.TextView detail.View,
            direction = Tooltip.Direction.Bottom,
            wrapperAttrs = [
              Attr.Create "data-testid" "tooltip-long"
              Attr.Style "position" "absolute"
              Attr.Style "left" "8px"
              Attr.Style "top" "260px"
            ]
          )
        else
          Doc.Empty)
      div [ Attr.Style "padding-top" "420px" ] [
        Button.create (
          text "Extend tooltip content",
          (fun () -> detail.Value <- String.replicate 300 "LongContent"),
          attrs = [ Attr.Create "data-testid" "tooltip-extend" ]
        )
        Button.create (
          text "Toggle long tooltip",
          (fun () -> mounted.Value <- not mounted.Value),
          attrs = [ Attr.Create "data-testid" "tooltip-toggle" ]
        )
      ]
    ]

  let private tooltipCustomIdPage () =
    Tooltip.create (
      Button.create (text "Custom ID trigger", ignore),
      text "Custom ID description",
      tooltipAttrs = [ attr.id "custom-tooltip" ],
      wrapperAttrs = [ Attr.Create "data-testid" "tooltip-custom-id" ]
    )

  let private tooltipNestedPage () =
    Tooltip.create (
      Tooltip.create (
        Button.create (text "Nested trigger", ignore),
        text "Inner description",
        tooltipAttrs = [ attr.id "inner-tooltip" ],
        wrapperAttrs = [ Attr.Create "data-testid" "tooltip-inner" ]
      ),
      text "Outer description",
      tooltipAttrs = [ attr.id "outer-tooltip" ],
      wrapperAttrs = [ Attr.Create "data-testid" "tooltip-outer" ]
    )

  let private tooltipInDialogPage () =
    let isOpen = Var.Create true

    isOpen.View
    |> Doc.BindView(fun visible ->
      if visible then
        Dialog.create (
          DialogTitle.create (text "Tooltip host dialog"),
          DialogContent.create (
            Tooltip.create (
              Button.create (text "Tooltip trigger", ignore),
              text "Helpful tooltip text",
              wrapperAttrs = [ Attr.Create "data-testid" "tooltip-in-dialog" ]
            )
          ),
          dialogInteraction = View.Const(Dialog.Interaction.Optional(fun () -> isOpen.Value <- false))
        )
      else
        Doc.Empty)

  let private sliderPage () =
    div [] [
      Slider.create (Var.Create 50, labelText = View.Const "Default Slider")
      Slider.create (
        Var.Create 0,
        min = 0,
        max = 10,
        step = 1,
        showTickMarks = true,
        tickMarkLabels = [ "0"; "5"; "10" ],
        labelText = View.Const "With Ticks"
      )
      Slider.create (Var.Create 30, labelText = View.Const "Disabled Slider", enabled = View.Const false)
      Slider.primary (Var.Create 75, labelText = View.Const "Primary Slider")
      for name, color in
        [
          "primary", Slider.Color.primary
          "secondary", Slider.Color.secondary
          "tertiary", Slider.Color.tertiary
          "error", Slider.Color.error
          "warning", Slider.Color.warning
          "success", Slider.Color.success
          "info", Slider.Color.info
        ] do
        Slider.create (
          Var.Create 50,
          labelText = View.Const name,
          attrs = [ color; Attr.Create "data-testid" ("slider-color-" + name) ]
        )

    ]

  let private dynamicSliderPage () =
    let enabled = Var.Create true

    div [] [
      Slider.create (
        Var.Create 50,
        min = 0,
        max = 100,
        labelText = View.Const "Adjust",
        enabled = enabled.View
      )
      Button.create (
        text "Toggle slider",
        (fun () -> enabled.Value <- not enabled.Value),
        attrs = [ Attr.Create "data-testid" "toggle-slider-enabled" ]
      )
    ]

  let private resizeListenerPage () =
    let mounted = Var.Create true
    let size = Var.Create<ResizeListener.ElementSize option> None

    div [] [
      Button.create (
        text "Toggle tracked element",
        (fun () -> Var.Set mounted (not mounted.Value)),
        attrs = [ Attr.Create "data-testid" "resize-toggle" ]
      )
      size.View
      |> Doc.BindView(fun current ->
        div [ Attr.Create "data-testid" "tracked-width" ] [
          match current with
          | Some measured -> Doc.TextView(measured.WidthView |> View.Map(sprintf "%.0f"))
          | None -> text "not mounted"
        ])
      mounted.View
      |> Doc.BindView(fun visible ->
        if visible then
          div [
            Attr.Create "data-testid" "resize-target"
            Attr.Style "width" "120px"
            Attr.Style "height" "20px"
            ResizeListener.track size
          ] []
        else
          Doc.Empty)
    ]

  let private highlighterPage () =
    let source = Var.Create "Weave weave WEAVE; C++ [a]. <b>weave</b>"
    let search = Var.Create "weave"
    let sensitive = Var.Create false
    let terms = Var.Create "we\nweave\n\nC++\n[a].\nweave"

    let multiple =
      terms.View |> View.Map(fun value -> value.Split('\n') |> Array.toList)

    div [ Typography.body1 ] [
      TextField.multiLine (
        source,
        labelText = View.Const "Source text",
        inputAttrs = [ Attr.Create "data-testid" "highlight-source" ]
      )
      TextField.multiLine (
        search,
        labelText = View.Const "Search text",
        inputAttrs = [ Attr.Create "data-testid" "highlight-search" ]
      )
      TextField.multiLine (
        terms,
        labelText = View.Const "Search terms, one per line",
        inputAttrs = [ Attr.Create "data-testid" "highlight-terms" ]
      )
      Switch.create (sensitive, content = text "Case sensitive")
      Highlighter.create (
        source.View,
        search.View,
        caseSensitive = sensitive.View,
        attrs = [ Attr.Create "data-testid" "single-highlight" ]
      )
      Highlighter.createMany (
        source.View,
        multiple,
        caseSensitive = sensitive.View,
        attrs = [ Attr.Create "data-testid" "multi-highlight" ]
      )
      Button.create (
        text "Change terms",
        (fun () -> terms.Value <- "<b>\nweave"),
        attrs = [ Attr.Create "data-testid" "change-terms" ]
      )
      Highlighter.create (
        View.Const null,
        View.Const null,
        attrs = [ Attr.Create "data-testid" "null-highlight" ]
      )
      for id, terms in
        [
          "empty-terms", []
          "null-terms", [ null; ""; null ]
          "mixed-terms", [ null; ""; "weave"; "weave" ]
        ] do
        Highlighter.createMany (source.View, View.Const terms, attrs = [ Attr.Create "data-testid" id ])
      for label, color in
        [
          "primary", Highlighter.Color.primary
          "secondary", Highlighter.Color.secondary
          "tertiary", Highlighter.Color.tertiary
          "warning", Highlighter.Color.warning
          "success", Highlighter.Color.success
          "error", Highlighter.Color.error
          "info", Highlighter.Color.info
        ] do
        Highlighter.create (
          View.Const label,
          View.Const label,
          attrs = [ color; Attr.Create "data-testid" ("highlight-" + label) ]
        )
    ]

  let private scrollToTopPage (offset: int) (initial: bool) =
    let mounted = Var.Create true

    div [] [
      Button.create (
        text "Toggle page button",
        (fun () -> mounted.Set(not mounted.Value)),
        attrs = [ Attr.Create "data-testid" "scroll-toggle" ]
      )
      div [ Attr.Style "position" "relative" ] [
        div [
          attr.id "scroll-target"
          attr.tabindex "0"
          Attr.Create "role" "region"
          Attr.Create "aria-label" "Scrollable panel"
          Attr.Style "height" "200px"
          Attr.Style "overflow" "auto"
          if initial then
            on.afterRender (fun el -> el?scrollTop <- 450)
        ] [
          div [ Attr.Style "height" "1400px"; Attr.Style "width" "1600px" ] [ text "Panel content" ]
        ]
        ScrollToTop.create (
          selector = "#scroll-target",
          topOffset = 80,
          attrs = [
            Attr.Create "data-testid" "scroll-panel"
            ScrollToTop.Behavior.instant
            Attr.Create "aria-label" "Back to panel top"
            Attr.Style "position" "absolute"
            Button.Color.secondary
          ]
        )
      ]
      mounted.View
      |> Doc.BindView(fun show ->
        if show then
          ScrollToTop.create (
            topOffset = offset,
            attrs = [ Attr.Create "data-testid" "scroll-page"; Button.Color.primary ]
          )
        else
          Doc.Empty)
      ScrollToTop.create (
        selector = "#missing-target",
        attrs = [ Attr.Create "data-testid" "scroll-missing" ]
      )
      div [ Attr.Style "height" "1800px" ] [ text "Page content" ]
    ]

  let private progressPage () =
    let value = Var.Create 35.0
    let buffer = Var.Create 85.0
    let busy = Var.Create false
    let testId id = Attr.Create "data-testid" id

    div [ Attr.Style "padding" "16px" ] [
      Progress.Linear.create (
        value = value.View,
        bufferValue = buffer.View,
        indeterminate = busy.View,
        min = 10.0,
        max = 110.0,
        attrs = [
          Attr.Create "aria-label" "Download"
          testId "linear"
          Progress.Linear.Size.large
          Progress.Linear.Variant.rounded
        ]
      )
      Progress.Circular.create (
        value = value.View,
        indeterminate = busy.View,
        min = 10.0,
        max = 110.0,
        content = text "Done",
        attrs = [
          Attr.Create "aria-label" "Download circle"
          testId "circular"
          Progress.Circular.Size.large
        ]
      )
      Progress.Linear.create (attrs = [ Attr.Create "aria-label" "Loading"; testId "unknown-linear" ])
      Progress.Circular.create (
        attrs = [ Attr.Create "aria-label" "Loading circle"; testId "unknown-circular" ]
      )
      Progress.Linear.create (
        value = View.Const 50.0,
        min = 5.0,
        max = 5.0,
        attrs = [ testId "invalid-range" ]
      )
      div [ Attr.Create "dir" "rtl" ] [
        Progress.Linear.create (value = View.Const 25.0, attrs = [ testId "rtl-linear" ])
      ]
      for size in
        [
          Progress.Circular.Size.small
          Progress.Circular.Size.medium
          Progress.Circular.Size.large
        ] do
        Progress.Circular.create (value = View.Const 50.0, attrs = [ size ])
      Button.create (text "Advance", (fun () -> value.Value <- 60.0), attrs = [ testId "advance" ])
      Button.create (text "Upper", (fun () -> value.Value <- 200.0), attrs = [ testId "upper" ])
      Button.create (text "Lower", (fun () -> value.Value <- -10.0), attrs = [ testId "lower" ])
      Button.create (text "NaN", (fun () -> value.Value <- nan), attrs = [ testId "nan" ])
      Button.create (text "Infinity", (fun () -> value.Value <- infinity), attrs = [ testId "infinity" ])
      Button.create (text "Buffer", (fun () -> buffer.Value <- 20.0), attrs = [ testId "buffer" ])
      Button.create (
        text "Toggle mode",
        (fun () -> busy.Value <- not busy.Value),
        attrs = [ testId "toggle-mode" ]
      )
    ]

  let private paginationPage () =
    let count = Var.Create 20
    let page = Var.Create 1
    let enabled = Var.Create true
    let command name action = Button.create (text name, action)

    div [] [
      Pagination.create (
        count.View,
        selectedPage = page,
        enabled = enabled.View,
        attrs = [ Pagination.Controls.showFirstLast; Attr.Create "id" "pager" ]
      )
      div [ Attr.Create "id" "page-value" ] [ textView (page.View |> View.Map string) ]
      command "Select ten" (fun () -> page.Set 10)
      command "Select below range" (fun () -> page.Set -5)
      command "Select above range" (fun () -> page.Set 100)
      command "Shrink count" (fun () -> count.Set 3)
      command "Empty count" (fun () -> count.Set 0)
      command "Negative count" (fun () -> count.Set -10)
      command "Restore count" (fun () -> count.Set 20)
      command "Toggle enabled" (fun () -> enabled.Set(not enabled.Value))
    ]

  let private paginationRangesPage () =
    div [] [
      for name, count, page, boundary, siblings in
        [
          "single", 1, 1, 1, 1
          "small", 3, 2, 1, 1
          "seven", 7, 4, 1, 1
          "start", 20, 1, 1, 1
          "middle", 20, 10, 1, 1
          "end", 20, 20, 1, 1
          "single-gap", 8, 4, 1, 1
          "custom", 20, 10, 2, 2
          "zero-window", 20, 10, 0, 0
          "negative-window", 20, 10, -1, -2
          "overlap", 3, 2, 10, 10
          "huge", System.Int32.MaxValue, System.Int32.MaxValue, 1, 1
        ] do
        Pagination.create (
          View.Const count,
          selectedPage = Var.Create page,
          boundaryCount = boundary,
          siblingCount = siblings,
          attrs = [
            Pagination.Controls.hidePreviousNext
            Attr.Create "aria-label" name
            Attr.Create "id" ("range-" + name)
          ]
        )
      for variantName, variant in
        [
          "text", Pagination.Variant.text
          "filled", Pagination.Variant.filled
          "outlined", Pagination.Variant.outlined
        ] do
        for colorName, color in
          [
            "primary", Pagination.Color.primary
            "secondary", Pagination.Color.secondary
            "tertiary", Pagination.Color.tertiary
            "error", Pagination.Color.error
            "warning", Pagination.Color.warning
            "success", Pagination.Color.success
            "info", Pagination.Color.info
          ] do
          Pagination.create (
            View.Const 5,
            selectedPage = Var.Create 2,
            attrs = [ variant; color; Attr.Create "aria-label" (variantName + " " + colorName) ]
          )
    ]

  let private paginationControlsPage () =
    div [] [
      for name, controls in
        [
          "default", []
          "boundaries", [ Pagination.Controls.showFirstLast ]
          "numbers-only", [ Pagination.Controls.hidePreviousNext ]
          "boundaries-only", [ Pagination.Controls.showFirstLast; Pagination.Controls.hidePreviousNext ]
        ] do
        Button.create (text "Before pager", (fun () -> ()), attrs = [ Attr.Create "id" ("before-" + name) ])

        Pagination.create (
          View.Const 5,
          selectedPage = Var.Create 3,
          attrs = [
            yield! controls
            Attr.Create "id" ("controls-" + name)
            if name <> "default" then
              Attr.Create "aria-label" (name + " pages")
          ]
        )

        Button.create (text "After pager", (fun () -> ()), attrs = [ Attr.Create "id" ("after-" + name) ])
    ]

  let private snackbarPage () =
    let notifications = SnackbarController.create (duration = 1000, maxVisible = 1)
    let other = SnackbarController.create (maxVisible = 1)
    let mounted = Var.Create true
    let last = Var.Create ""
    let result = Var.Create 0
    let position = Var.Create false

    let send message =
      Var.Set last (notifications.show (message, dismissal = Snackbar.Dismissal.Manual))

    let control label callback =
      Button.create (text label, onClick = callback)

    div [ Attr.Style "padding" "120px 16px" ] [
      control "Persistent" (fun () -> send "Persistent message")
      control "Duplicate" (fun () -> send "Persistent message")
      control "Timed" (fun () -> Var.Set last (notifications.show ("Timed message")))
      control "Timed override" (fun () ->
        notifications.show ("Short message", dismissal = Snackbar.Dismissal.After 250)
        |> ignore)
      control "Queue timed" (fun () -> Var.Set last (notifications.show ("Queued message")))
      control "Action" (fun () ->
        Var.Set
          last
          (notifications.show (
            "Item deleted",
            action = SnackbarAction.create ("Undo", fun () -> Var.Set result (result.Value + 1))
          )))
      control "Timed action" (fun () ->
        notifications.show (
          "Timed action",
          action = SnackbarAction.create ("Retry", ignore),
          dismissal = Snackbar.Dismissal.After 1000
        )
        |> ignore)
      control "Custom" (fun () ->
        notifications.add (
          strong [] [ text "Custom content" ],
          key = "custom",
          dismissal = Snackbar.Dismissal.Manual,
          attrs = [ Snackbar.Variant.filled; Snackbar.Color.success ]
        )
        |> ignore)
      control "Remove last" (fun () -> notifications.remove last.Value)
      control "Clear" notifications.clear
      control "Toggle host" (fun () -> Var.Set mounted (not mounted.Value))
      control "Toggle position" (fun () -> Var.Set position (not position.Value))
      control "Other" (fun () ->
        other.show ("Other message", dismissal = Snackbar.Dismissal.Manual) |> ignore)
      div [ Attr.Create "data-testid" "last-id" ] [ Doc.TextView last.View ]
      div [ Attr.Create "data-testid" "action-result" ] [ Doc.TextView(result.View |> View.Map string) ]
      mounted.View
      |> Doc.BindView(fun show ->
        if show then
          Snackbar.create (
            notifications,
            attrs = [
              Attr.Create "data-testid" "primary-host"
              Attr.DynamicClassPred "weave-snackbar-host--top-center" position.View
            ]
          )
        else
          Doc.Empty)
      Snackbar.create (other, attrs = [ Attr.Create "data-testid" "other-host"; Snackbar.Position.topLeft ])
    ]

  let private snackbarPalettePage () =
    let notifications = SnackbarController.create (maxVisible = 24)

    for color in
      [
        Snackbar.Color.primary
        Snackbar.Color.secondary
        Snackbar.Color.tertiary
        Snackbar.Color.success
        Snackbar.Color.error
        Snackbar.Color.warning
        Snackbar.Color.info
      ] do
      for variant in
        [
          Snackbar.Variant.standard
          Snackbar.Variant.outlined
          Snackbar.Variant.filled
        ] do
        notifications.add (
          text "Changes saved",
          dismissal = Snackbar.Dismissal.Manual,
          action = SnackbarAction.create ("Undo", ignore),
          attrs = [ color; variant ]
        )
        |> ignore

    Snackbar.create (notifications)

  let private renderPage (hash: string) =
    match hash with
    | "scroll-to-top" -> scrollToTopPage 300 false
    | "scroll-to-top-zero" -> scrollToTopPage 0 false
    | "scroll-to-top-negative" -> scrollToTopPage -20 false
    | "scroll-to-top-initial" -> scrollToTopPage 300 true
    | "scroll-to-top-accessible" -> scrollToTopPage 0 true
    | "pagination" -> paginationPage ()
    | "pagination-controls" -> paginationControlsPage ()
    | "pagination-ranges" -> paginationRangesPage ()
    | "checkbox" -> checkboxPage ()
    | "radio-groups" -> radioFormGroupsPage ()
    | "radio" -> radioPage ()
    | "switch" -> switchPage ()
    | "field" -> fieldPage ()
    | "numericfield" -> numericFieldPage ()
    | "textfield" -> textFieldPage ()
    | "form" -> formPage ()
    | "form-sizing" -> formSizingPage ()
    | "form-grid-validation" -> formGridValidationPage ()
    | "form-arithmetic" -> formArithmeticPage ()
    | "select" -> selectPage ()
    | "resize-listener" -> resizeListenerPage ()
    | "animation" -> animationPage ()
    | "button" -> buttonPage ()
    | "dialog-empty" ->
      Dialog.create (
        DialogTitle.create (text "No actions"),
        DialogContent.create (
          div [] [ text "Message"; button [ Attr.Style "display" "none" ] [ text "Hidden" ] ]
        )
      )
    | "dialog-plain" -> plainDialogPage ()
    | "dialog" -> dialogPage ()
    | "dialog-with-drawer" -> dialogWithDrawerPage ()
    | "dialog-force" -> dialogForcePage ()
    | "dialog-triggered" -> dialogTriggeredPage ()
    | "tabs" -> tabsPage ()
    | "carousel" -> carouselPage false false
    | "carousel-autoplay" -> carouselPage true false
    | "carousel-custom-controls" -> carouselPage false true
    | "carousel-autoplay-custom-controls" -> carouselPage true true
    | "carousel-pair" -> carouselPairPage ()
    | "dropdown-menu" -> dropdownPage ()
    | "expansion-panel" -> expansionPanelPage ()
    | "expansion-panel-dynamic" -> dynamicExpansionPanelPage ()
    | "alert" -> alertPage ()
    | "snackbar" -> snackbarPage ()
    | "snackbar-example" -> Weave.Docs.Examples.SnackbarExamples.render ()
    | "snackbar-palette" -> snackbarPalettePage ()
    | "card" -> cardPage ()
    | "table" -> tablePage ()
    | "table-default" -> defaultTablePage ()
    | "theme" -> themePage false
    | "theme-system" -> themePage true
    | "appbar" -> appbarPage ()
    | "listener-removal" -> listenerRemovalPage ()
    | "drawer-closed" -> drawerPage Drawer.Variant.Temporary false
    | "drawer-responsive" -> drawerPage Drawer.Variant.Responsive true
    | "drawer" -> drawerPage Drawer.Variant.Temporary true
    | "highlighter" -> highlighterPage ()
    | "link" -> linkPage ()
    | "list" -> listPage ()
    | "chip" -> chipPage ()
    | "chipset" -> chipsetPage ()
    | "buttonmenu" -> buttonmenuPage ()
    | "buttongroup" -> buttongroupPage ()
    | "buttongroup-palette" -> filledButtonGroupPalettePage ()
    | "tooltip" -> tooltipPage ()
    | "tooltip-custom-id" -> tooltipCustomIdPage ()
    | "tooltip-nested" -> tooltipNestedPage ()
    | "tooltip-viewport" -> tooltipViewportPage ()
    | "tooltip-in-dialog" -> tooltipInDialogPage ()
    | "progress" -> progressPage ()
    | "slider" -> sliderPage ()
    | "slider-dynamic" -> dynamicSliderPage ()
    | "color-picker" -> ColorPickerPage.render ()
    | _ -> div [] [ text (sprintf "Unknown page: %s" hash) ]

  [<Inline "window.location.hash.replace('#', '')">]
  let private getHash () = X<string>

  [<SPAEntryPoint>]
  let Main () =
    let hash = getHash ()
    let page = renderPage hash
    // WebSharper runs the parent afterRender callback after synchronizing its descendants.
    let wrapper =
      div [ on.afterRender (fun el -> el.SetAttribute("data-e2e-ready", "true")) ] [ page ]

    wrapper |> Doc.RunById "main"
