namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Operators

[<JavaScript>]
module ReactiveHelpersExamples =

  let private viewZipSection () =
    let description =
      Helpers.bodyText
        "View.zip combines two independent reactive values into a single View of a tuple. Use View.zipCached when the views update frequently and downstream mapping is expensive."

    let content =
      let sliderA = Var.Create 25
      let sliderB = Var.Create 75

      let zipped = View.zip sliderA.View sliderB.View

      div [] [
        Grid.create (
          [
            GridItem.create (
              Slider.create (sliderA, min = 0, max = 100, labelText = View.Const "Value A"),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )
            GridItem.create (
              Slider.create (sliderB, min = 0, max = 100, labelText = View.Const "Value B"),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )
          ]
        )
        div [ Margin.Top.small; Typography.body1 ] [
          text "Zipped: "
          textView (zipped |> View.Map(fun (a, b) -> sprintf "(%d, %d)" a b))
        ]
      ]

    let code =
      """let sliderA = Var.Create 25
let sliderB = Var.Create 75

let zipped = View.zip sliderA.View sliderB.View  // see here

textView (zipped |> View.Map(fun (a, b) -> sprintf "(%d, %d)" a b))"""

    Helpers.codeSampleSection "View.zip / View.zipCached" description content code

  let private viewMap2CachedSection () =
    let description =
      Helpers.bodyText
        "View.map2Cached transforms two reactive views into a computed value, caching the result to avoid redundant recalculations when neither input has changed."

    let content =
      let firstName = Var.Create "Ada"
      let lastName = Var.Create "Lovelace"

      let fullName =
        View.map2Cached (fun f l -> sprintf "%s %s" f l) firstName.View lastName.View

      div [] [
        Grid.create (
          [
            GridItem.create (
              TextField.singleLine (firstName, labelText = View.Const "First Name"),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )
            GridItem.create (
              TextField.singleLine (lastName, labelText = View.Const "Last Name"),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )
          ]
        )
        div [ Margin.Top.small; Typography.body1 ] [ text "Full name: "; textView fullName ]
      ]

    let code =
      """let firstName = Var.Create "Ada"
let lastName = Var.Create "Lovelace"

let fullName =
  View.map2Cached  // see here
    (fun f l -> sprintf "%s %s" f l)
    firstName.View
    lastName.View

div [ Typography.body1 ] [ textView fullName ]"""

    Helpers.codeSampleSection "View.map2Cached" description content code

  let private viewNotSection () =
    let description =
      Helpers.bodyText
        "View.not inverts a boolean View. A common pattern is using it to toggle UI elements based on a negated condition."

    let content =
      let isLocked = Var.Create false

      div [] [
        Switch.create (isLocked, content = text "Locked")
        div [ Margin.Top.small ] [
          Button.create (
            text "Submit",
            onClick = ignore,
            enabled = View.not isLocked.View // see here
          )
        ]
      ]

    let code =
      """let isLocked = Var.Create false

Switch.create (isLocked, content = text "Locked")

Button.create (
  text "Submit",
  onClick = ignore,
  enabled = View.not isLocked.View  // see here — enabled when NOT locked
)"""

    Helpers.codeSampleSection "View.not" description content code

  let private docSinkSection () =
    let description =
      Helpers.bodyText
        "Doc.sink runs a side-effect function each time a View updates and returns Doc.Empty. Doc.sinkCached only fires when the value actually changes, avoiding duplicate effects."

    let content =
      let counter = Var.Create 0
      let lastAction = Var.Create "No actions yet"

      div [] [
        Doc.sinkCached (fun n -> Var.Set lastAction (sprintf "Counter changed to %d" n)) counter.View
        div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g4 ] [
          Button.create (text "Increment", onClick = (fun () -> Var.Update counter (fun n -> n + 1)))
          Button.create (
            text "Reset",
            onClick = (fun () -> Var.Set counter 0),
            attrs = [ Button.Variant.outlined ]
          )
        ]
        div [ Margin.Top.small; Typography.body1 ] [
          text "Value: "
          textView (counter.View |> View.Map string)
        ]
        div [ Typography.body2; Opacity.sixty ] [ text "Last effect: "; textView lastAction.View ]
      ]

    let code =
      """let counter = Var.Create 0
let lastAction = Var.Create "No actions yet"

// Side-effect runs only when the value changes
Doc.sinkCached  // see here
  (fun n -> Var.Set lastAction (sprintf "Counter changed to %d" n))
  counter.View

Button.create (text "Increment", onClick = (fun () ->
  Var.Update counter (fun n -> n + 1)))"""

    Helpers.codeSampleSection "Doc.sink / Doc.sinkCached" description content code

  let private docBindViewOptionSection () =
    let description =
      Helpers.bodyText
        "Doc.bindViewOption renders content only when a View<'T option> is Some. When the view is None, nothing is rendered. Use Doc.bindViewOptionOrDefault to show fallback content for None."

    let content =
      let selected = Var.Create<string option> None

      let items =
        [ "Apple"; "Banana"; "Cherry" ]
        |> List.map (fun fruit -> SelectItem.create (text fruit, fruit, fruit))
        |> View.Const

      div [] [
        Select.create (
          items,
          selected,
          labelText = View.Const "Pick a fruit",
          clearable = View.Const true,
          attrs = [ Select.Width.fitContent ]
        )
        div [ Margin.Top.small ] [
          selected.View
          |> Doc.bindViewOptionOrDefault // see here
            (div [ Typography.body2; Opacity.sixty ] [ text "Nothing selected yet" ])
            (fun fruit -> div [ Typography.body1 ] [ text (sprintf "You picked: %s" fruit) ])
        ]
      ]

    let code =
      """let selected = Var.Create<string option> None

// Render only when Some
selected.View
|> Doc.bindViewOptionOrDefault  // see here
  (text "Nothing selected yet")  // fallback for None
  (fun fruit -> text (sprintf "You picked: %s" fruit))"""

    Helpers.codeSampleSection "Doc.bindViewOption" description content code

  let private attrEnabledSection () =
    let description =
      Helpers.bodyText
        "Attr.enabled dynamically sets the HTML disabled attribute based on a boolean View. When the view is false, the element gets disabled."

    let content =
      let isEnabled = Var.Create true

      div [] [
        Switch.create (isEnabled, content = text "Enable form")
        div [ Margin.Top.small; Flex.Flex.allSizes; Gap.All.g4 ] [
          TextField.singleLine (Var.Create "", labelText = View.Const "Name", enabled = isEnabled.View) // see here
          Button.create (text "Submit", onClick = ignore, enabled = isEnabled.View)
        ]
      ]

    let code =
      """let isEnabled = Var.Create true

Switch.create (isEnabled, content = text "Enable form")

TextField.singleLine (
  Var.Create "",
  labelText = View.Const "Name",
  enabled = isEnabled.View  // see here — disabled when switch is off
)

Button.create (text "Submit", onClick = ignore, enabled = isEnabled.View)"""

    Helpers.codeSampleSection "Attr.enabled" description content code

  let private attrClassSelectionSection () =
    let description =
      Helpers.bodyText
        "Attr.classSelection dynamically applies a CSS class based on the current value of a View and a map of values to class names. Only one class from the map is active at a time."

    let content =
      let size = Var.Create "medium"

      let sizeMap =
        Map.ofList [
          "small", "weave-rounded-sm"
          "medium", "weave-rounded"
          "large", "weave-rounded-lg"
        ]

      div [] [
        Grid.create (
          [
            for s in [ "small"; "medium"; "large" ] do
              GridItem.create (
                Radio.create (size, s, displayText = View.Const s),
                attrs = [ GridItem.Span.four ]
              )
          ]
        )
        div [
          Margin.Top.small
          BrandColor.BackgroundColor.primary
          Attr.Style "color" "var(--palette-primary-text)"
          Padding.All.medium
          Attr.classSelection size.View sizeMap // see here
        ] [ text "This box changes border-radius based on selection" ]
      ]

    let code =
      """let size = Var.Create "medium"

let sizeMap =
  Map.ofList [
    "small", Css.``weave-rounded-sm``
    "medium", Css.``weave-rounded``
    "large", Css.``weave-rounded-lg``
  ]

div [
  Attr.classSelection size.View sizeMap  // see here — swaps class dynamically
] [ text "This box changes border-radius based on selection" ]"""

    Helpers.codeSampleSection "Attr.classSelection" description content code

  let private operatorsSection () =
    let description =
      Helpers.bodyText
        "The (<||>) and (<&&>) operators combine two boolean Views with logical OR and AND. Use them to build compound conditions for visibility, enabled state, or dynamic classes."

    let content =
      let hasName = Var.Create false
      let hasEmail = Var.Create false

      let canSubmit = hasName.View <&&> hasEmail.View // see here
      let hasAny = hasName.View <||> hasEmail.View

      div [] [
        div [ Flex.Flex.allSizes; Gap.All.g4; Margin.Bottom.small ] [
          Checkbox.create (hasName, displayText = View.Const "Has Name")
          Checkbox.create (hasEmail, displayText = View.Const "Has Email")
        ]
        div [ Flex.Flex.allSizes; Gap.All.g4; AlignItems.center ] [
          Button.create (text "Submit (both required)", onClick = ignore, enabled = canSubmit)
          div [ Typography.body2 ] [
            textView (
              hasAny
              |> View.Map(fun v -> if v then "At least one checked" else "None checked")
            )
          ]
        ]
      ]

    let code =
      """open Weave.Operators

let hasName = Var.Create false
let hasEmail = Var.Create false

// Combine boolean views with operators
let canSubmit = hasName.View <&&> hasEmail.View  // see here — both must be true
let hasAny = hasName.View <||> hasEmail.View     // at least one true

Button.create (
  text "Submit",
  onClick = ignore,
  enabled = canSubmit
)"""

    Helpers.codeSampleSection "Operators (<||> and <&&>)" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Reactive Helpers"
        Helpers.bodyText
          "Weave extends WebSharper.UI with helper functions for composing reactive Views, binding optional values, and combining boolean conditions. These utilities reduce boilerplate when building dynamic interfaces."
        Helpers.divider ()
        viewZipSection ()
        Helpers.divider ()
        viewMap2CachedSection ()
        Helpers.divider ()
        viewNotSection ()
        Helpers.divider ()
        docSinkSection ()
        Helpers.divider ()
        docBindViewOptionSection ()
        Helpers.divider ()
        attrEnabledSection ()
        Helpers.divider ()
        attrClassSelectionSection ()
        Helpers.divider ()
        operatorsSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
