namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ShowcaseExpenseTracker =

  [<JavaScript; Struct>]
  type Category =
    | Food
    | Transport
    | Entertainment
    | Shopping
    | Bills
    | Other

  [<JavaScript>]
  type Expense = {
    Id: int
    Description: string
    Amount: float
    Category: Category
  }

  let private categoryToString (c: Category) =
    match c with
    | Food -> "Food"
    | Transport -> "Transport"
    | Entertainment -> "Entertainment"
    | Shopping -> "Shopping"
    | Bills -> "Bills"
    | Other -> "Other"

  let private stringToCategory (s: string) =
    match s with
    | "Food" -> Some Food
    | "Transport" -> Some Transport
    | "Entertainment" -> Some Entertainment
    | "Shopping" -> Some Shopping
    | "Bills" -> Some Bills
    | "Other" -> Some Other
    | _ -> None

  let private categoryToColor (c: Category) =
    match c with
    | Food -> Chip.Color.success
    | Transport -> Chip.Color.info
    | Entertainment -> Chip.Color.tertiary
    | Shopping -> Chip.Color.primary
    | Bills -> Chip.Color.error
    | Other -> Chip.Color.warning

  let private allCategories = [ Food; Transport; Entertainment; Shopping; Bills; Other ]

  let private fullAppSection () =
    let expenses: Var<Expense list> = Var.Create []
    let nextId = Var.Create 0
    let newDesc = Var.Create ""
    let newAmount = Var.Create 0
    let newCategory = Var.Create<string option>(Some "Food")

    let activeCategories =
      Var.Create(allCategories |> List.map categoryToString |> Set.ofList)

    let addExpense () =
      let desc = newDesc.Value.Trim()
      let amt = float newAmount.Value

      if desc.Length > 0 && amt > 0.0 then
        let cat =
          newCategory.Value |> Option.bind stringToCategory |> Option.defaultValue Other

        let expense = {
          Id = nextId.Value
          Description = desc
          Amount = amt
          Category = cat
        }

        Var.Update expenses (fun list -> list @ [ expense ])
        Var.Set nextId (nextId.Value + 1)
        Var.Set newDesc ""
        Var.Set newAmount 0

    let removeExpense (id: int) =
      Var.Update expenses (List.filter (fun e -> e.Id <> id))

    let categoryItems =
      allCategories
      |> List.map (fun c ->
        let label = categoryToString c
        SelectItem.create (text label, label, label))
      |> View.Const

    let filteredExpenses =
      (expenses.View, activeCategories.View)
      ||> View.Map2(fun list cats ->
        list |> List.filter (fun e -> Set.contains (categoryToString e.Category) cats))

    let totalAmount = filteredExpenses |> View.Map(List.sumBy (fun e -> e.Amount))

    let expenseCount = filteredExpenses |> View.Map List.length

    let avgAmount =
      (totalAmount, expenseCount)
      ||> View.Map2(fun total count -> if count > 0 then total / float count else 0.0)

    let description =
      Helpers.bodyText
        "The complete Expense Tracker. Add expenses with categories, filter by type, and see live summary statistics — all reactively computed."

    let content =
      div [] [
        // Add expense form
        Grid.create (
          [
            GridItem.create (
              TextField.singleLine (
                newDesc,
                labelText = View.Const "Description",
                placeholder = View.Const "What did you spend on?",
                attrs = [ TextField.Variant.outlined; TextField.Color.primary; TextField.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
            )

            GridItem.create (
              NumericField.create (
                newAmount,
                labelText = View.Const "Amount",
                min = 0,
                max = 99999,
                attrs = [
                  NumericField.Variant.outlined
                  NumericField.Color.primary
                  NumericField.Width.full
                ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
            )

            GridItem.create (
              Select.create (
                categoryItems,
                newCategory,
                labelText = View.Const "Category",
                attrs = [ Select.Variant.outlined; Select.Color.primary; Select.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
            )

            GridItem.create (
              Button.create (
                div [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
                  Icon.create (Icon.UiActions UiActions.Add)
                  text "Add"
                ],
                onClick = addExpense,
                attrs = [
                  Button.Color.primary
                  Button.Variant.filled
                  Attr.Style "margin-top" "var(--weave-field-label-gutter)"
                ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.two; Flex.Flex.allSizes ]
            )
          ],
          attrs = [ AlignItems.stretch ]
        )

        Divider.create (attrs = [ Margin.Vertical.small ])

        // Summary dashboard
        Grid.create (
          [
            GridItem.create (
              div [
                Attr.Style "inline-size" "100%"
                Attr.Style "box-sizing" "border-box"
                SurfaceColor.BackgroundColor.surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.center
              ] [
                div [ Typography.overline; Typography.Color.textSecondary ] [ text "Total" ]

                div [ Typography.h4; BrandColor.TextColor.primary ] [
                  textView (totalAmount |> View.MapCached(sprintf "$%.2f"))
                ]
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
            )

            GridItem.create (
              div [
                Attr.Style "inline-size" "100%"
                Attr.Style "box-sizing" "border-box"
                SurfaceColor.BackgroundColor.surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.center
              ] [
                div [ Typography.overline; Typography.Color.textSecondary ] [ text "Expenses" ]
                div [ Typography.h4 ] [ textView (expenseCount |> View.MapCached(sprintf "%d")) ]
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
            )

            GridItem.create (
              div [
                Attr.Style "inline-size" "100%"
                Attr.Style "box-sizing" "border-box"
                SurfaceColor.BackgroundColor.surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.center
              ] [
                div [ Typography.overline; Typography.Color.textSecondary ] [ text "Average" ]

                div [ Typography.h4; BrandColor.TextColor.tertiary ] [
                  textView (avgAmount |> View.MapCached(sprintf "$%.2f"))
                ]
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
            )
          ]
        )

        Divider.create (attrs = [ Margin.Vertical.small ])

        // Category filter chips
        div [ Margin.Bottom.extraSmall ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Filter by category" ]

          div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
            for cat in allCategories do
              let catName = categoryToString cat

              activeCategories.View
              |> Doc.BindView(fun active ->
                let isActive = Set.contains catName active

                Chip.create (
                  text catName,
                  onClick =
                    (fun () ->
                      Var.Update activeCategories (fun s ->
                        if Set.contains catName s then
                          Set.remove catName s
                        else
                          Set.add catName s)),
                  attrs = [
                    categoryToColor cat
                    if isActive then
                      Chip.Variant.filled
                    else
                      Chip.Variant.outlined
                    Cursor.pointer
                  ]
                ))
          ]
        ]

        // Expense list
        filteredExpenses
        |> Doc.BindView(fun list ->
          if List.isEmpty list then
            Alert.create (
              div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
                Icon.create (Icon.Business Business.AccountBalance)
                text "No expenses yet — add one above!"
              ],
              attrs = [ Alert.Color.info; Alert.Variant.outlined; Margin.Top.extraSmall ]
            )
          else
            div [ Margin.Top.extraSmall ] [
              for expense in list do
                div [
                  Flex.Flex.allSizes
                  FlexWrap.Wrap.allSizes
                  AlignItems.center
                  Gap.All.g2
                  Padding.Vertical.extraSmall
                  Padding.Horizontal.extraSmall
                  SurfaceColor.BackgroundColor.surface
                  BorderRadius.All.small
                  Margin.Bottom.extraSmall
                  Elevation.e1
                ] [
                  div [
                    FlexItem.Flex.allSizes
                    Typography.body1
                    Attr.Style "min-inline-size" "0"
                    Attr.Style "flex-basis" "16rem"
                    Attr.Style "overflow-wrap" "anywhere"
                  ] [ text expense.Description ]

                  div [
                    Flex.Flex.allSizes
                    FlexWrap.Wrap.allSizes
                    AlignItems.center
                    JustifyContent.flexEnd
                    Gap.All.g2
                    FlexItem.NoShrink.allSizes
                    Attr.Style "max-inline-size" "100%"
                    Attr.Style "margin-inline-start" "auto"
                  ] [
                    Chip.create (
                      text (categoryToString expense.Category),
                      attrs = [ categoryToColor expense.Category; Chip.Variant.filled ]
                    )

                    div [
                      Flex.Flex.allSizes
                      AlignItems.center
                      Gap.All.g2
                      FlexItem.NoShrink.allSizes
                    ] [
                      div [ Typography.subtitle1 ] [ text (sprintf "$%.2f" expense.Amount) ]

                      IconButton.create (
                        Icon.create (
                          Icon.UiActions UiActions.Delete,
                          attrs = [ Attr.Style "font-size" "18px" ]
                        ),
                        onClick = (fun () -> removeExpense expense.Id),
                        attrs = [
                          Density.compact
                          Attr.Create "aria-label" ("Delete " + expense.Description)
                        ]
                      )
                    ]
                  ]
                ]
            ])
      ]

    Helpers.section "The Full App" description content

  let private domainSource =
    """[<Struct>]
type Category =
  | Food
  | Transport
  | Entertainment
  | Shopping
  | Bills
  | Other

type Expense = {
  Id: int
  Description: string
  Amount: float
  Category: Category
}

// Pattern matching maps categories to UI concerns
let categoryToColor (c: Category) =
  match c with
  | Food -> Chip.Color.success
  | Transport -> Chip.Color.info
  | Entertainment -> Chip.Color.tertiary
  | Shopping -> Chip.Color.primary
  | Bills -> Chip.Color.error
  | Other -> Chip.Color.warning

let allCategories = [ Food; Transport; Entertainment; Shopping; Bills; Other ]

let categoryToString (c: Category) =
  match c with
  | Food -> "Food"
  | Transport -> "Transport"
  | Entertainment -> "Entertainment"
  | Shopping -> "Shopping"
  | Bills -> "Bills"
  | Other -> "Other"

let stringToCategory (s: string) =
  allCategories |> List.tryFind (fun c -> categoryToString c = s)"""

  let private addingSource =
    """let expenses: Var<Expense list> = Var.Create []
let nextId = Var.Create 0
let newDesc = Var.Create ""
let newAmount = Var.Create 0
let newCategory = Var.Create<string option>(Some "Food")

let addExpense () =
  let desc = newDesc.Value.Trim()
  let amt = float newAmount.Value

  if desc.Length > 0 && amt > 0.0 then
    let cat =
      newCategory.Value
      |> Option.bind stringToCategory
      |> Option.defaultValue Other

    let expense = {
      Id = nextId.Value
      Description = desc
      Amount = amt
      Category = cat
    }

    Var.Update expenses (fun list -> list @ [ expense ])
    Var.Set nextId (nextId.Value + 1)
    Var.Set newDesc ""
    Var.Set newAmount 0

let categoryItems =
  allCategories
  |> List.map (fun c ->
    let label = categoryToString c
    SelectItem.create (text label, label, label))
  |> View.Const

let expenseForm =
  div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
    Grid.create (
      [
        GridItem.create (
          TextField.singleLine (
            newDesc,
            labelText = View.Const "Description",
            attrs = [
              TextField.Variant.outlined
              TextField.Color.primary
              TextField.Width.full
            ]
          ),
          attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
        )
        GridItem.create (
          NumericField.create (
            newAmount,
            labelText = View.Const "Amount",
            min = 0,
            max = 99999,
            attrs = [
              NumericField.Variant.outlined
              NumericField.Color.primary
              NumericField.Width.full
            ]
          ),
          attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.three ]
        )
        GridItem.create (
          Select.create (
            categoryItems,
            newCategory,
            labelText = View.Const "Category",
            attrs = [
              Select.Variant.outlined
              Select.Color.primary
              Select.Width.full
            ]
          ),
          attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.three ]
        )
      ]
    )
    Button.primary (
      text "Add expense",
      onClick = addExpense,
      attrs = [ Button.Variant.filled ]
    )
  ]"""

  let private filterSource =
    """open Weave.Icons.MaterialSymbols

let activeCategories =
  Var.Create(allCategories |> List.map categoryToString |> Set.ofList)

// Computed filtered list
let filteredExpenses =
  (expenses.View, activeCategories.View)
  ||> View.Map2(fun list cats ->
    list
    |> List.filter (fun e -> Set.contains (categoryToString e.Category) cats))

let toggleCategory name =
  Var.Update activeCategories (fun active ->
    if Set.contains name active then
      Set.remove name active
    else
      Set.add name active)

let categoryFilters =
  div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; Gap.All.g2 ] [
    for category in allCategories do
      let name = categoryToString category

      activeCategories.View
      |> Doc.BindView(fun active ->
        Chip.create (
          text name,
          onClick = (fun () -> toggleCategory name),
          attrs = [
            categoryToColor category
            if Set.contains name active then
              Chip.Variant.filled
            else
              Chip.Variant.outlined
          ]
        ))
  ]

let removeExpense (id: int) =
  Var.Update expenses (List.filter (fun e -> e.Id <> id))

let expenseList =
  filteredExpenses
  |> Doc.BindView(fun expenses ->
    if List.isEmpty expenses then
      Alert.create (text "No expenses in the selected categories.")
    else
      div [] [
        for expense in expenses do
          div [
            Flex.Flex.allSizes
            FlexWrap.Wrap.allSizes
            AlignItems.center
            Gap.All.g2
          ] [
            div [
              FlexItem.Flex.allSizes
              Attr.Style "min-inline-size" "0"
              Attr.Style "flex-basis" "16rem"
              Attr.Style "overflow-wrap" "anywhere"
            ] [ text expense.Description ]

            div [
              Flex.Flex.allSizes
              FlexWrap.Wrap.allSizes
              AlignItems.center
              JustifyContent.flexEnd
              Gap.All.g2
              FlexItem.NoShrink.allSizes
              Attr.Style "max-inline-size" "100%"
              Attr.Style "margin-inline-start" "auto"
            ] [
              Chip.create (
                text (categoryToString expense.Category),
                attrs = [
                  categoryToColor expense.Category
                  Chip.Variant.filled
                ]
              )
              div [
                Flex.Flex.allSizes
                AlignItems.center
                Gap.All.g2
                FlexItem.NoShrink.allSizes
              ] [
                text (sprintf "$%.2f" expense.Amount)
                IconButton.create (
                  Icon.create (Icon.UiActions UiActions.Delete),
                  onClick = (fun () -> removeExpense expense.Id),
                  attrs = [
                    Density.compact
                    Attr.Create "aria-label" ("Delete " + expense.Description)
                  ]
                )
              ]
            ]
          ]
      ])"""

  let private summarySource =
    """// Derive summary values from the filtered list.
let totalAmount = filteredExpenses |> View.Map(List.sumBy (fun e -> e.Amount))

let expenseCount = filteredExpenses |> View.Map List.length

let avgAmount =
  (totalAmount, expenseCount)
  ||> View.Map2(fun total count ->
    if count > 0 then total / float count else 0.0)

// Reactive summary cards
let summary =
  Grid.create (
    [
      GridItem.create (
        div [
          Padding.All.small
          Elevation.e1
          Attr.Style "inline-size" "100%"
          Attr.Style "box-sizing" "border-box"
        ] [
          div [ Typography.overline ] [ text "Total" ]
          div [ Typography.h4; BrandColor.TextColor.primary ] [
            textView (totalAmount |> View.MapCached(sprintf "$%.2f"))
          ]
        ],
        attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
      )
      GridItem.create (
        div [
          Padding.All.small
          Elevation.e1
          Attr.Style "inline-size" "100%"
          Attr.Style "box-sizing" "border-box"
        ] [
          div [ Typography.overline ] [ text "Expenses" ]
          div [ Typography.h4 ] [
            textView (expenseCount |> View.MapCached(sprintf "%d"))
          ]
        ],
        attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
      )
      GridItem.create (
        div [
          Padding.All.small
          Elevation.e1
          Attr.Style "inline-size" "100%"
          Attr.Style "box-sizing" "border-box"
        ] [
          div [ Typography.overline ] [ text "Average" ]
          div [ Typography.h4; BrandColor.TextColor.tertiary ] [
            textView (avgAmount |> View.MapCached(sprintf "$%.2f"))
          ]
        ],
        attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
      )
    ]
  )

div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
  expenseForm
  categoryFilters
  summary
  expenseList
]"""

  // These are the exact displayed blocks, in their learning order.
  let private sourceParts = [ domainSource; addingSource; filterSource; summarySource ]

  let private combinedSource = String.concat "\n\n" sourceParts

  let private sourceSections () =
    div [] [

      Helpers.codeSection
        "fsharp"
        "Domain Model"
        (Helpers.bodyText "Start here. Define categories, expense records, and their display mappings.")
        domainSource

      Helpers.codeSection
        "fsharp"
        "Adding Expenses"
        (Helpers.bodyText
          "Continues Domain Model. Define input state and expenseForm, including the action that appends a valid expense. This small example accepts whole-dollar amounts.")
        addingSource

      Helpers.codeSection
        "fsharp"
        "Category Filtering"
        (Helpers.bodyText
          "Continues Domain Model and Adding Expenses. Toggle categories and bind the visible expense list to the filtered View.")
        filterSource

      Helpers.codeSection
        "fsharp"
        "Summary Dashboard"
        (Helpers.bodyText
          "Continues all previous sections. Derive totals from the filtered list and compose the form, filters, summary, and expenses.")
        summarySource

      Helpers.codeSection
        "fsharp"
        "Combined example"
        (Helpers.bodyText
          "Copy all the sections in order. Use the application setup in Getting Started and paste this source inside its JavaScript module; its final expression returns the composed Doc. The smaller example focuses on the mechanisms above, while the full demo adds presentation and sample data.")
        combinedSource
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Expense Tracker"

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Track spending with categories, filtering, and live summaries. This showcase demonstrates reactive computations with View.Map, category-based filtering with Chips, and data-driven UI composition with Weave's form components."
        ]

        Helpers.divider ()
        fullAppSection ()
        Helpers.divider ()
        Helpers.bodyText
          "Build the smaller example from top to bottom. Each source section continues the preceding sections; Combined example copies the same sequence in one block."
        sourceSections ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
