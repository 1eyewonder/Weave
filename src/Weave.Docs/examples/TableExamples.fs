namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module TableExamples =

  let private comparisonGrid columns items =
    let cells =
      items
      |> List.map (fun item ->
        GridItem.create (
          item,
          attrs = [
            GridItem.Span.twelve
            GridItem.Span.Small.six
            (if columns = 4 then GridItem.Span.Medium.three
             elif columns = 3 then GridItem.Span.Medium.four
             else GridItem.Span.Medium.six)
          ]
        ))

    Grid.create (cells, attrs = [ Grid.Spacing.large; AlignItems.start ])

  type private Fruit = {
    Name: string
    Color: string
    Stock: int
  }

  let private fruits = [
    {
      Name = "Apple"
      Color = "Red"
      Stock = 42
    }
    {
      Name = "Banana"
      Color = "Yellow"
      Stock = 17
    }
    {
      Name = "Cherry"
      Color = "Red"
      Stock = 120
    }
    {
      Name = "Grape"
      Color = "Purple"
      Stock = 63
    }
  ]

  let private fruitName (f: Fruit) = f.Name

  let private columns () = [
    TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
    TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
    TableColumn.create (
      "stock",
      text "Stock",
      (fun (f: Fruit) -> text (string<int> f.Stock)),
      attrs = [ Attr.Style "text-align" "end" ]
    )
  ]

  let private basicExample () =
    let description =
      Helpers.bodyText
        "Use Table.create with a list of TableColumn definitions, a reactive list of rows, and a rowKey that gives each row a stable identity. Unchanged rows are not re-rendered when the rows update. Each column has a key that the sort state and rowHeader refer to. Pass caption to name the table for screen readers, and add Table.hiddenCaption to keep it off screen."

    let content =
      let fruits = [
        {
          Name = "Apple"
          Color = "Red"
          Stock = 42
        }
        {
          Name = "Banana"
          Color = "Yellow"
          Stock = 17
        }
        {
          Name = "Cherry"
          Color = "Red"
          Stock = 120
        }
        {
          Name = "Grape"
          Color = "Purple"
          Stock = 63
        }
      ]

      let columns = [
        TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
        TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
        TableColumn.create (
          "stock",
          text "Stock",
          (fun (f: Fruit) -> text (string<int> f.Stock)),
          attrs = [ Attr.Style "text-align" "end" ]
        )
      ]

      Table.create (
        columns,
        View.Const fruits,
        (fun f -> f.Name),
        rowHeader = "name",
        caption = text "Fruit stock"
      )

    let code =
      """type Fruit = {
  Name: string
  Color: string
  Stock: int
}

let fruits = [
  {
    Name = "Apple"
    Color = "Red"
    Stock = 42
  }
  {
    Name = "Banana"
    Color = "Yellow"
    Stock = 17
  }
  {
    Name = "Cherry"
    Color = "Red"
    Stock = 120
  }
  {
    Name = "Grape"
    Color = "Purple"
    Stock = 63
  }
]

let columns = [
  TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
  TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
  TableColumn.create (
    "stock",
    text "Stock",
    (fun (f: Fruit) -> text (string<int> f.Stock)),
    attrs = [ Attr.Style "text-align" "end" ]
  )
]

Table.create (
  columns,
  View.Const fruits,
  (fun f -> f.Name),
  rowHeader = "name",
  caption = text "Fruit stock"
)
"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private selectionExample () =
    let description =
      Helpers.bodyText
        "Pass selectedKey to track the selected row, and onRowClick to react to activation. Clicking anywhere on a row activates it. The first cell holds a button, so keyboard and screen reader users reach and activate the row the same way they would any other button. The selected row is marked with aria-current."

    let content =
      let fruits = [
        {
          Name = "Apple"
          Color = "Red"
          Stock = 42
        }
        {
          Name = "Banana"
          Color = "Yellow"
          Stock = 17
        }
        {
          Name = "Cherry"
          Color = "Red"
          Stock = 120
        }
        {
          Name = "Grape"
          Color = "Purple"
          Stock = 63
        }
      ]

      let columns = [
        TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
        TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
        TableColumn.create (
          "stock",
          text "Stock",
          (fun (f: Fruit) -> text (string<int> f.Stock)),
          attrs = [ Attr.Style "text-align" "end" ]
        )
      ]

      let selected = Var.Create(Some "Banana")

      Table.create (columns, View.Const fruits, (fun f -> f.Name), selectedKey = selected)

    let code =
      """type Fruit = {
  Name: string
  Color: string
  Stock: int
}

let fruits = [
  {
    Name = "Apple"
    Color = "Red"
    Stock = 42
  }
  {
    Name = "Banana"
    Color = "Yellow"
    Stock = 17
  }
  {
    Name = "Cherry"
    Color = "Red"
    Stock = 120
  }
  {
    Name = "Grape"
    Color = "Purple"
    Stock = 63
  }
]

let columns = [
  TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
  TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
  TableColumn.create (
    "stock",
    text "Stock",
    (fun (f: Fruit) -> text (string<int> f.Stock)),
    attrs = [ Attr.Style "text-align" "end" ]
  )
]

let selected = Var.Create(Some "Banana")

Table.create (
  columns,
  View.Const fruits,
  (fun f -> f.Name),
  selectedKey = selected
)
"""

    Helpers.codeSampleSection "Row Selection" description content code

  let private multiSelectExample () =
    let description =
      Helpers.bodyText
        "Pass selectedKeys to add a checkbox column for selecting any number of rows. The header checkbox selects or clears every row shown and is mixed while only some are selected. With rowHeader set, each checkbox is named by its row header."

    let content =
      let fruits = [
        {
          Name = "Apple"
          Color = "Red"
          Stock = 42
        }
        {
          Name = "Banana"
          Color = "Yellow"
          Stock = 17
        }
        {
          Name = "Cherry"
          Color = "Red"
          Stock = 120
        }
        {
          Name = "Grape"
          Color = "Purple"
          Stock = 63
        }
      ]

      let columns = [
        TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
        TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
        TableColumn.create (
          "stock",
          text "Stock",
          (fun (f: Fruit) -> text (string<int> f.Stock)),
          attrs = [ Attr.Style "text-align" "end" ]
        )
      ]

      let selected = Var.Create(Set.ofList [ "Cherry" ])

      Table.create (
        columns,
        View.Const fruits,
        (fun f -> f.Name),
        selectedKeys = selected,
        rowHeader = "name",
        caption = text "Select fruit",
        attrs = [ Table.hiddenCaption ]
      )

    let code =
      """type Fruit = {
  Name: string
  Color: string
  Stock: int
}

let fruits = [
  {
    Name = "Apple"
    Color = "Red"
    Stock = 42
  }
  {
    Name = "Banana"
    Color = "Yellow"
    Stock = 17
  }
  {
    Name = "Cherry"
    Color = "Red"
    Stock = 120
  }
  {
    Name = "Grape"
    Color = "Purple"
    Stock = 63
  }
]

let columns = [
  TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
  TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
  TableColumn.create (
    "stock",
    text "Stock",
    (fun (f: Fruit) -> text (string<int> f.Stock)),
    attrs = [ Attr.Style "text-align" "end" ]
  )
]

let selected = Var.Create(Set.ofList [ "Cherry" ])

Table.create (
  columns,
  View.Const fruits,
  (fun f -> f.Name),
  selectedKeys = selected,
  rowHeader = "name",
  caption = text "Select fruit",
  attrs = [ Table.hiddenCaption ]
)
"""

    Helpers.codeSampleSection "Multiple Selection" description content code

  let private sortingExample () =
    let description =
      Helpers.bodyText
        "Pass sort to a column to make its header a sort button. Table.ColumnSort.by sorts by a projected value, and Table.ColumnSort.Compare takes a comparison. Activating the header cycles ascending, descending, then the original order, and the header cell reports the state through aria-sort. Pass a sort Var to the table to start sorted or to read the current sort."

    let content =
      let fruits = [
        {
          Name = "Apple"
          Color = "Red"
          Stock = 42
        }
        {
          Name = "Banana"
          Color = "Yellow"
          Stock = 17
        }
        {
          Name = "Cherry"
          Color = "Red"
          Stock = 120
        }
        {
          Name = "Grape"
          Color = "Purple"
          Stock = 63
        }
      ]

      let columns = [
        TableColumn.create (
          "name",
          text "Name",
          (fun f -> text f.Name),
          sort = Table.ColumnSort.by (fun f -> f.Name)
        )
        TableColumn.create ("color", text "Color", (fun f -> text f.Color))
        TableColumn.create (
          "stock",
          text "Stock",
          (fun f -> text (string<int> f.Stock)),
          sort = Table.ColumnSort.by (fun f -> f.Stock),
          attrs = [ Attr.Style "text-align" "end" ]
        )
      ]

      let sort =
        Var.Create(
          Some {
            Column = "stock"
            Direction = Table.SortDirection.Descending
          }
          : Table.SortState option
        )

      Table.create (columns, View.Const fruits, (fun f -> f.Name), sort = sort)

    let code =
      """type Fruit = {
  Name: string
  Color: string
  Stock: int
}

let fruits = [
  {
    Name = "Apple"
    Color = "Red"
    Stock = 42
  }
  {
    Name = "Banana"
    Color = "Yellow"
    Stock = 17
  }
  {
    Name = "Cherry"
    Color = "Red"
    Stock = 120
  }
  {
    Name = "Grape"
    Color = "Purple"
    Stock = 63
  }
]

let columns = [
  TableColumn.create (
    "name",
    text "Name",
    (fun f -> text f.Name),
    sort = Table.ColumnSort.by (fun f -> f.Name)
  )
  TableColumn.create ("color", text "Color", (fun f -> text f.Color))
  TableColumn.create (
    "stock",
    text "Stock",
    (fun f -> text (string<int> f.Stock)),
    sort = Table.ColumnSort.by (fun f -> f.Stock),
    attrs = [ Attr.Style "text-align" "end" ]
  )
]

let sort =
  Var.Create(
    Some {
      Column = "stock"
      Direction = Table.SortDirection.Descending
    }
    : Table.SortState option
  )

Table.create (columns, View.Const fruits, (fun f -> f.Name), sort = sort)
"""

    Helpers.codeSampleSection "Sorting" description content code

  let private externalSortExample () =
    let description =
      Helpers.bodyText
        "Use Table.ColumnSort.External when the rows arrive already ordered, such as from a server. The header still cycles the sort state, and the rows View reads that state to fetch or order the data."

    let content =
      let fruits = [
        {
          Name = "Apple"
          Color = "Red"
          Stock = 42
        }
        {
          Name = "Banana"
          Color = "Yellow"
          Stock = 17
        }
        {
          Name = "Cherry"
          Color = "Red"
          Stock = 120
        }
        {
          Name = "Grape"
          Color = "Purple"
          Stock = 63
        }
      ]

      let fetchFruitsSortedBy (state: Table.SortState option) = async {
        return
          match state with
          | Some { Direction = Table.SortDirection.Ascending } -> List.sortBy (fun f -> f.Stock) fruits
          | Some { Direction = Table.SortDirection.Descending } ->
            List.sortByDescending (fun f -> f.Stock) fruits
          | None -> fruits
      }

      let sort = Var.Create None

      let rows = sort.View |> View.MapAsync fetchFruitsSortedBy

      let columns = [
        TableColumn.create ("name", text "Name", (fun f -> text f.Name))
        TableColumn.create (
          "stock",
          text "Stock",
          (fun f -> text (string<int> f.Stock)),
          sort = Table.ColumnSort.External,
          attrs = [ Attr.Style "text-align" "end" ]
        )
      ]

      Table.create (columns, rows, (fun f -> f.Name), sort = sort)

    let code =
      """type Fruit = {
  Name: string
  Color: string
  Stock: int
}

let fruits = [
  {
    Name = "Apple"
    Color = "Red"
    Stock = 42
  }
  {
    Name = "Banana"
    Color = "Yellow"
    Stock = 17
  }
  {
    Name = "Cherry"
    Color = "Red"
    Stock = 120
  }
  {
    Name = "Grape"
    Color = "Purple"
    Stock = 63
  }
]

let fetchFruitsSortedBy (state: Table.SortState option) =
  async {
    return
      match state with
      | Some {
               Direction = Table.SortDirection.Ascending
             } -> List.sortBy (fun f -> f.Stock) fruits
      | Some {
               Direction = Table.SortDirection.Descending
             } -> List.sortByDescending (fun f -> f.Stock) fruits
      | None -> fruits
  }

let sort = Var.Create None

let rows = sort.View |> View.MapAsync fetchFruitsSortedBy

let columns = [
  TableColumn.create ("name", text "Name", (fun f -> text f.Name))
  TableColumn.create (
    "stock",
    text "Stock",
    (fun f -> text (string<int> f.Stock)),
    sort = Table.ColumnSort.External,
    attrs = [ Attr.Style "text-align" "end" ]
  )
]

Table.create (columns, rows, (fun f -> f.Name), sort = sort)
"""

    Helpers.codeSampleSection "External Sorting" description content code

  let private filterExample () =
    let description =
      Helpers.bodyText
        "Rows are a View, so filtering is a View.Map over the source. Pass emptyContent to show a message when no rows match."

    let content =
      let fruits = [
        {
          Name = "Apple"
          Color = "Red"
          Stock = 42
        }
        {
          Name = "Banana"
          Color = "Yellow"
          Stock = 17
        }
        {
          Name = "Cherry"
          Color = "Red"
          Stock = 120
        }
        {
          Name = "Grape"
          Color = "Purple"
          Stock = 63
        }
      ]

      let columns = [
        TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
        TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
        TableColumn.create (
          "stock",
          text "Stock",
          (fun (f: Fruit) -> text (string<int> f.Stock)),
          attrs = [ Attr.Style "text-align" "end" ]
        )
      ]

      let query = Var.Create ""

      let filtered =
        query.View
        |> View.Map(fun q -> fruits |> List.filter (fun f -> f.Name.ToLower().Contains(q.Trim().ToLower())))

      div [] [
        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          AlignItems.end'
          Gap.All.g3
          Margin.Bottom.small
        ] [
          TextField.singleLine (
            query,
            labelText = View.Const "Filter by name",
            placeholder = View.Const "Search by fruit",
            attrs = [ TextField.Variant.outlined; TextField.Label.above ]
          )
          Button.create (
            text "Clear filter",
            onClick = (fun () -> Var.Set query ""),
            attrs = [ Button.Variant.outlined ]
          )
        ]
        Table.create (
          columns,
          filtered,
          (fun f -> f.Name),
          rowHeader = "name",
          caption = text "Fruit stock",
          emptyContent = text "No fruit matches the filter",
          attrs = [ Table.hoverable ]
        )
      ]

    let code =
      """type Fruit = {
  Name: string
  Color: string
  Stock: int
}

let fruits = [
  {
    Name = "Apple"
    Color = "Red"
    Stock = 42
  }
  {
    Name = "Banana"
    Color = "Yellow"
    Stock = 17
  }
  {
    Name = "Cherry"
    Color = "Red"
    Stock = 120
  }
  {
    Name = "Grape"
    Color = "Purple"
    Stock = 63
  }
]

let columns = [
  TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
  TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
  TableColumn.create (
    "stock",
    text "Stock",
    (fun (f: Fruit) -> text (string<int> f.Stock)),
    attrs = [ Attr.Style "text-align" "end" ]
  )
]

let query = Var.Create ""

let filtered =
  query.View
  |> View.Map(fun q ->
    fruits
    |> List.filter (fun f -> f.Name.ToLower().Contains(q.Trim().ToLower())))

Doc.Concat [
  TextField.singleLine (
    query,
    labelText = View.Const "Filter by name",
    placeholder = View.Const "Search by fruit",
    attrs = [ TextField.Variant.outlined; TextField.Label.above ]
  )
  Button.create (
    text "Clear filter",
    onClick = (fun () -> Var.Set query ""),
    attrs = [ Button.Variant.outlined ]
  )
  Table.create (
    columns,
    filtered,
    (fun f -> f.Name),
    rowHeader = "name",
    caption = text "Fruit stock",
    emptyContent = text "No fruit matches the filter",
    attrs = [ Table.hoverable ]
  )
]
"""

    Helpers.codeSampleSection "Filtering and Empty State" description content code

  let private stylingExample () =
    let description =
      Helpers.bodyText
        "Table.striped tints alternate rows; Table.hoverable highlights the row under the pointer. Choose a palette role or a custom CSS color, and check custom colors against row text in both themes. These tables use the default density."

    let content =
      let fruits = [ "Apple", 42; "Banana", 17; "Cherry", 120 ]

      let createTable attrs =
        let columns = [
          TableColumn.create ("fruit", text "Fruit", (fun (name, _) -> text name))
          TableColumn.create (
            "stock",
            text "Stock",
            (fun (_, stock) -> text (string<int> stock)),
            attrs = [ Attr.Style "text-align" "end" ]
          )
        ]

        Table.create (columns, View.Const fruits, fst, attrs = attrs)

      let preview label attrs =
        div [ Attr.Style "inline-size" "100%"; Attr.Style "min-inline-size" "0" ] [
          p [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          createTable attrs
        ]

      comparisonGrid 3 [
        preview "Striped" [ Table.striped; Table.StripeColor.primary ]
        preview "Hoverable" [ Table.hoverable; Table.HoverColor.success ]
        preview "Custom stripe" [
          Table.striped
          Table.StripeColor.custom "rgba(var(--palette-primary-rgb), var(--weave-alpha-selected))"
        ]
      ]

    let code =
      """let fruits = [ "Apple", 42; "Banana", 17; "Cherry", 120 ]

let createTable attrs =
  let columns = [
    TableColumn.create ("fruit", text "Fruit", (fun (name, _) -> text name))
    TableColumn.create (
      "stock",
      text "Stock",
      (fun (_, stock) -> text (string<int> stock)),
      attrs = [ Attr.Style "text-align" "end" ]
    )
  ]

  Table.create (columns, View.Const fruits, fst, attrs = attrs)

createTable [ Table.striped; Table.StripeColor.primary ]

createTable [ Table.hoverable; Table.HoverColor.success ]

let stripe =
  "rgba(var(--palette-primary-rgb), var(--weave-alpha-selected))"

createTable [ Table.striped; Table.StripeColor.custom stripe ]"""

    Helpers.codeSampleSection "Striping and Hover" description content code

  let private stickyHeaderExample () =
    let description =
      Helpers.bodyText
        "Table.stickyHeader keeps the header row visible while the body scrolls. Give the table a max height so it scrolls inside its own frame."

    let content =
      let fruits = [
        {
          Name = "Apple"
          Color = "Red"
          Stock = 42
        }
        {
          Name = "Banana"
          Color = "Yellow"
          Stock = 17
        }
        {
          Name = "Cherry"
          Color = "Red"
          Stock = 120
        }
        {
          Name = "Grape"
          Color = "Purple"
          Stock = 63
        }
      ]

      let columns = [
        TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
        TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
        TableColumn.create (
          "stock",
          text "Stock",
          (fun (f: Fruit) -> text (string<int> f.Stock)),
          attrs = [ Attr.Style "text-align" "end" ]
        )
      ]

      Table.create (
        columns,
        View.Const fruits,
        (fun f -> f.Name),
        attrs = [ Table.stickyHeader; Attr.Style "max-height" "160px" ]
      )

    let code =
      """type Fruit = {
  Name: string
  Color: string
  Stock: int
}

let fruits = [
  {
    Name = "Apple"
    Color = "Red"
    Stock = 42
  }
  {
    Name = "Banana"
    Color = "Yellow"
    Stock = 17
  }
  {
    Name = "Cherry"
    Color = "Red"
    Stock = 120
  }
  {
    Name = "Grape"
    Color = "Purple"
    Stock = 63
  }
]

let columns = [
  TableColumn.create ("name", text "Name", (fun (f: Fruit) -> text f.Name))
  TableColumn.create ("color", text "Color", (fun (f: Fruit) -> text f.Color))
  TableColumn.create (
    "stock",
    text "Stock",
    (fun (f: Fruit) -> text (string<int> f.Stock)),
    attrs = [ Attr.Style "text-align" "end" ]
  )
]

Table.create (
  columns,
  View.Const fruits,
  (fun f -> f.Name),
  attrs = [ Table.stickyHeader; Attr.Style "max-height" "160px" ]
)
"""

    Helpers.codeSampleSection "Sticky Header" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Table.") [
      Helpers.apiTable "Table.create" [
        Helpers.apiParam
          "columns"
          "TableColumnDef<'T> list"
          ""
          "Column definitions created with TableColumn.create"
        Helpers.apiParam "rows" "View<'T list>" "" "Reactive list of row values"
        Helpers.apiParam
          "rowKey"
          "'T -> string"
          ""
          "Stable row identity used to avoid re-rendering unchanged rows and to track selection"
        Helpers.apiParam
          "?onRowClick"
          "'T -> unit"
          ""
          "Called when a row is clicked or its first-cell button is activated"
        Helpers.apiParam
          "?selectedKey"
          "Var<string option>"
          ""
          "Key of the selected row. Activating a row selects it, and the selected row gets aria-current"
        Helpers.apiParam
          "?selectedKeys"
          "Var<Set<string>>"
          ""
          "Keys of the checked rows. Adds a checkbox column with a header checkbox for every row shown"
        Helpers.apiParam
          "?sort"
          "Var<Table.SortState option>"
          "unsorted"
          "The sort state, for starting sorted or reacting to sorting"
        Helpers.apiParam
          "?rowHeader"
          "string"
          ""
          "Key of the column whose cells label their rows, rendered as row header cells"
        Helpers.apiParam
          "?caption"
          "Doc"
          ""
          "The table's caption and accessible name. Hide it visually with Table.hiddenCaption"
        Helpers.apiParam "?emptyContent" "Doc" "" "Content shown in a full-width row when there are no rows"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Additional attributes (density, striped, hoverable, sticky header, colors, etc.)"
      ]

      Helpers.apiTable "TableColumn.create" [
        Helpers.apiParam
          "key"
          "string"
          ""
          "Identifies the column in the sort state and in rowHeader. Unique within the table"
        Helpers.apiParam "header" "Doc" "" "Content of the column header cell"
        Helpers.apiParam "cell" "'T -> Doc" "" "Renders the body cell for a row value"
        Helpers.apiParam
          "?sort"
          "Table.ColumnSort<'T>"
          ""
          "Makes the header a sort button. Compare or by orders the rows in the table, and External leaves the order to the rows View"
        Helpers.apiParam
          "?attrs"
          "Attr list"
          "[]"
          "Attributes applied to both the header and body cells of the column"
      ]

      Helpers.styleModuleTable "Table.Density" [
        ("compact", "Reduced cell padding for dense data")
        ("standard", "Default cell padding")
        ("spacious", "Extra cell padding")
      ]

      Helpers.styleModuleTable "Table" [
        ("striped", "Tints every other row")
        ("hoverable", "Highlights the row under the pointer")
        ("stickyHeader", "Keeps the header row visible while the body scrolls")
        ("hiddenCaption", "Hides the caption visually while keeping it as the accessible name")
      ]

      Helpers.styleModuleTable "Table.StripeColor" [
        ("primary, secondary, tertiary, error, warning, success, info",
         "Tints alternate rows with a palette color")
        ("custom", "Tints alternate rows with any CSS color")
      ]

      Helpers.styleModuleTable "Table.HoverColor" [
        ("primary, secondary, tertiary, error, warning, success, info",
         "Highlights the row under the pointer with a palette color")
        ("custom", "Highlights the row under the pointer with any CSS color")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Table"
        Helpers.bodyText "Displays rows of data in columns using a semantic table."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        DataAppearance.table ()
        Helpers.divider ()
        selectionExample ()
        Helpers.divider ()
        multiSelectExample ()
        Helpers.divider ()
        sortingExample ()
        Helpers.divider ()
        externalSortExample ()
        Helpers.divider ()
        filterExample ()
        Helpers.divider ()
        stylingExample ()
        Helpers.divider ()
        stickyHeaderExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
