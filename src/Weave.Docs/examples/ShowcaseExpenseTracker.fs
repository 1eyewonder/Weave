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
              Field.create (
                newDesc,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Description",
                placeholder = View.Const "What did you spend on?",
                attrs = [ Field.Color.primary; Field.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
            )

            GridItem.create (
              NumericField.create (
                newAmount,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Amount",
                min = 0,
                max = 99999,
                attrs = [ Field.Color.primary; Field.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.three ]
            )

            GridItem.create (
              Select.create (
                categoryItems,
                newCategory,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Category",
                attrs = [ Select.Color.primary; Select.Width.full ]
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
                attrs = [ Button.Color.primary; Button.Variant.filled; Attr.Style "min-height" "40px" ]
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
                SurfaceColor.toBackgroundColor SurfaceColor.Surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.center
              ] [
                div [ Typography.overline; Typography.Color.textSecondary ] [ text "Total" ]

                div [ Typography.h4; BrandColor.toColor BrandColor.Primary ] [
                  textView (totalAmount |> View.MapCached(sprintf "$%.2f"))
                ]
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
            )

            GridItem.create (
              div [
                SurfaceColor.toBackgroundColor SurfaceColor.Surface
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
                SurfaceColor.toBackgroundColor SurfaceColor.Surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.center
              ] [
                div [ Typography.overline; Typography.Color.textSecondary ] [ text "Average" ]

                div [ Typography.h4; BrandColor.toColor BrandColor.Tertiary ] [
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
                  AlignItems.center
                  Gap.All.g2
                  Padding.Vertical.extraSmall
                  Padding.Horizontal.extraSmall
                  SurfaceColor.toBackgroundColor SurfaceColor.Surface
                  BorderRadius.All.small
                  Margin.Bottom.extraSmall
                  Elevation.e1
                ] [
                  Chip.create (
                    text (categoryToString expense.Category),
                    attrs = [ categoryToColor expense.Category; Chip.Variant.filled ]
                  )

                  div [ FlexItem.Flex.allSizes; Typography.body1 ] [ text expense.Description ]

                  div [ Typography.subtitle1 ] [ text (sprintf "$%.2f" expense.Amount) ]

                  IconButton.create (
                    Icon.create (Icon.UiActions UiActions.Delete, attrs = [ Attr.Style "font-size" "18px" ]),
                    onClick = (fun () -> removeExpense expense.Id),
                    attrs = [ Density.compact ]
                  )
                ]
            ])
      ]

    Helpers.section "The Full App" description content

  let private domainModelSection () =
    let description =
      Helpers.bodyText
        "Categories are a discriminated union — each variant maps to a display name and Chip color. Expenses are records with an ID for stable identity in the reactive list."

    let content = Doc.Empty

    let code =
      """[<Struct>]
type Category = Food | Transport | Entertainment | Shopping | Bills | Other

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
    | Other -> Chip.Color.warning"""

    Helpers.codeSampleSection "Domain Model" description content code

  let private addingExpensesSection () =
    let description =
      Helpers.bodyText
        "The form combines Field (description), NumericField (amount), and Select (category). When Add is clicked, we parse the category string back to the DU, create an Expense record, and append it to the reactive list."

    let content = Doc.Empty

    let code =
      """let expenses: Var<Expense list> = Var.Create []
let newDesc = Var.Create ""
let newAmount = Var.Create 0
let newCategory = Var.Create<string option> (Some "Food")

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
        Var.Set newDesc ""
        Var.Set newAmount 0

// Form layout
Grid.create([
    GridItem.create(
        Field.create(newDesc,
            variant = Field.Variant.Outlined,
            labelText = View.Const "Description",
            attrs = [ Field.Color.primary; Field.Width.full ]),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ])
    GridItem.create(
        NumericField.create(newAmount,
            variant = Field.Variant.Outlined,
            labelText = View.Const "Amount",
            min = 0, max = 99999,
            attrs = [ Field.Color.primary; Field.Width.full ]),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.three ])
    GridItem.create(
        Select.create(categoryItems, newCategory,
            variant = Select.Variant.Outlined,
            labelText = View.Const "Category",
            attrs = [ Select.Color.primary; Select.Width.full ]),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.three ])
])"""

    Helpers.codeSampleSection "Adding Expenses" description content code

  let private filteringSection () =
    let description =
      Helpers.bodyText
        "Category chips toggle on/off for filtering. The chip color comes from the same categoryToColor function used elsewhere. The filtered list is a computed View — everything downstream updates automatically."

    let content = Doc.Empty

    let code =
      """let activeCategories =
    Var.Create (allCategories |> List.map categoryToString |> Set.ofList)

// Computed filtered list
let filteredExpenses =
    (expenses.View, activeCategories.View)
    ||> View.Map2(fun list cats ->
        list |> List.filter (fun e ->
            Set.contains (categoryToString e.Category) cats))

// Toggle chips
for cat in allCategories do
    let catName = categoryToString cat
    activeCategories.View
    |> Doc.BindView(fun active ->
        let isActive = Set.contains catName active
        Chip.create(
            text catName,
            onClick = Some (fun () ->
                Var.Update activeCategories (fun s ->
                    if Set.contains catName s
                    then Set.remove catName s
                    else Set.add catName s)),
            attrs = [
                categoryToColor cat
                if isActive then Chip.Variant.filled
                else Chip.Variant.outlined
            ]))"""

    Helpers.codeSampleSection "Category Filtering" description content code

  let private summarySection () =
    let description =
      Helpers.bodyText
        "Summary stats are computed reactively with View.Map — total, count, and average. No imperative accumulators needed. When the filtered list changes, all stats update automatically."

    let content = Doc.Empty

    let code =
      """// All stats are derived Views — no manual bookkeeping
let totalAmount =
    filteredExpenses
    |> View.Map (List.sumBy (fun e -> e.Amount))

let expenseCount =
    filteredExpenses |> View.Map List.length

let avgAmount =
    (totalAmount, expenseCount)
    ||> View.Map2(fun total count ->
        if count > 0 then total / float count else 0.0)

// Reactive summary cards
Grid.create([
    GridItem.create(
        div [ Padding.All.small; Elevation.e1 ] [
            div [ Typography.overline ] [ text "Total" ]
            div [ Typography.h4; BrandColor.toColor BrandColor.Primary ] [
                textView (totalAmount |> View.MapCached(sprintf "$%.2f"))
            ]
        ],
        attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ])
    GridItem.create(
        div [ Padding.All.small; Elevation.e1 ] [
            div [ Typography.overline ] [ text "Expenses" ]
            div [ Typography.h4 ] [
                textView (expenseCount |> View.MapCached(sprintf "%d"))
            ]
        ],
        attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ])
    GridItem.create(
        div [ Padding.All.small; Elevation.e1 ] [
            div [ Typography.overline ] [ text "Average" ]
            div [ Typography.h4; BrandColor.toColor BrandColor.Tertiary ] [
                textView (avgAmount |> View.MapCached(sprintf "$%.2f"))
            ]
        ],
        attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ])
])"""

    Helpers.codeSampleSection "Summary Dashboard" description content code

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
        domainModelSection ()
        Helpers.divider ()
        addingExpensesSection ()
        Helpers.divider ()
        filteringSection ()
        Helpers.divider ()
        summarySection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
