namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module PaginationExamples =

  let private muted value =
    span [ Typography.body2; Attr.Class "docs-pagination__muted" ] [ text value ]

  let private pageStatus total (page: Var<int>) =
    div [
      Typography.caption
      Attr.Class "docs-pagination__status"
      Attr.Create "aria-live" "polite"
    ] [
      textView (page.View |> View.Map(fun current -> sprintf "Page %d of %d" current total))
    ]

  let private basicExample () =
    let page = Var.Create 1

    let content =
      div [ Attr.Class "docs-pagination__stage" ] [
        span [ Typography.overline; Attr.Class "docs-pagination__eyebrow" ] [ text "Explore the collection" ]
        h3 [ Typography.h6 ] [ text "A page at a time" ]
        muted "Choose a number or use the arrows to move through ten pages."
        Pagination.create (
          View.Const 10,
          selectedPage = page,
          attrs = [ Attr.Create "aria-label" "Collection pages" ]
        )
        pageStatus 10 page
      ]

    Helpers.codeSampleSection
      "Basic pagination"
      (Helpers.bodyText
        "Share a selectedPage Var with your content. Page numbers start at one, and the arrows stop at either end.")
      content
      """let page = Var.Create 1
Pagination.create (
    View.Const 10,
    selectedPage = page,
    attrs = [ Attr.Create "aria-label" "Collection pages" ]
)"""

  let private stylesExample () =
    let variant title detail colorName attrs enabled =
      Card.create (
        CardContent.create (
          div [ Attr.Class "docs-pagination__variant-layout" ] [
            div [ Attr.Class "docs-pagination__variant-copy" ] [
              span [ Typography.overline; Attr.Class "docs-pagination__eyebrow" ] [ text colorName ]
              h3 [ Typography.subtitle1 ] [ text title ]
              muted detail
            ]
            Pagination.create (
              View.Const 3,
              selectedPage = Var.Create 2,
              enabled = View.Const enabled,
              attrs = Attr.Create "aria-label" (title + " example pages") :: attrs
            )
          ]
        ),
        attrs = [ Card.Variant.filled; Attr.Class "docs-pagination__variant" ]
      )

    let content =
      div [ Attr.Class "docs-pagination__variant-grid" ] [
        variant "Text" "A quiet highlight for the current page." "Primary" [] true
        variant
          "Filled"
          "A solid accent for stronger emphasis."
          "Secondary"
          [ Pagination.Variant.filled; Pagination.Color.secondary ]
          true
        variant
          "Outlined"
          "Defined edges around every page."
          "Tertiary"
          [ Pagination.Variant.outlined; Pagination.Color.tertiary ]
          true
        variant "Disabled" "The same layout, with navigation unavailable." "Inactive" [] false
      ]

    Helpers.codeSampleSection
      "Variants and colors"
      (Helpers.bodyText
        "Choose the emphasis that fits your interface. Variants and palette colors go in attrs; enabled controls whether the pager is interactive.")
      content
      """// Text is the default; primary is the default color.
Pagination.create (View.Const 3, selectedPage = Var.Create 2)

Pagination.create (
    View.Const 3,
    selectedPage = Var.Create 2,
    attrs = [ Pagination.Variant.filled; Pagination.Color.secondary ]
)
Pagination.create (
    View.Const 3,
    selectedPage = Var.Create 2,
    attrs = [ Pagination.Variant.outlined; Pagination.Color.tertiary ]
)
Pagination.create (View.Const 3, selectedPage = Var.Create 2, enabled = View.Const false)"""

  let private rangesExample () =
    let range title detail settings boundary siblings controlAttrs =
      let page = Var.Create 50

      div [ Attr.Class "docs-pagination__range" ] [
        div [ Attr.Class "docs-pagination__range-heading" ] [
          div [] [ h3 [ Typography.subtitle1 ] [ text title ]; muted detail ]
          span [ Typography.caption; Attr.Class "docs-pagination__settings" ] [ text settings ]
        ]
        Pagination.create (
          View.Const 100,
          selectedPage = page,
          boundaryCount = boundary,
          siblingCount = siblings,
          attrs = Attr.Create "aria-label" (title + " pages") :: controlAttrs
        )
        pageStatus 100 page
      ]

    let content =
      div [ Attr.Class "docs-pagination__ranges" ] [
        range
          "Jump across a long collection"
          "First and last controls put the ends within reach."
          "1 boundary / 1 sibling"
          1
          1
          [ Pagination.Controls.showFirstLast ]
        range
          "Keep more context in view"
          "Show more neighboring pages with number-only navigation."
          "2 boundaries / 2 siblings"
          2
          2
          [ Pagination.Controls.hidePreviousNext ]
      ]

    Helpers.codeSampleSection
      "Long ranges"
      (Helpers.bodyText
        "Ellipses keep large collections compact. Compare two windows into the same 100-page collection; both start at page 50. Single-page gaps stay clickable.")
      content
      """Pagination.create (
    View.Const 100,
    selectedPage = Var.Create 50,
    attrs = [
        Pagination.Controls.showFirstLast
        Attr.Create "aria-label" "Jump across a long collection pages"
    ]
)
Pagination.create (
    View.Const 100,
    selectedPage = Var.Create 50,
    boundaryCount = 2,
    siblingCount = 2,
    attrs = [
        Pagination.Controls.hidePreviousNext
        Attr.Create "aria-label" "Keep more context in view pages"
    ]
)"""

  let private dataExample () =
    let records = [
      "Button", "Actions and interactions"
      "Card", "Content and surfaces"
      "Carousel", "Content and surfaces"
      "Checkbox", "Forms and input"
      "Chip", "Selection and feedback"
      "Dialog", "Overlays and navigation"
      "Divider", "Layout and spacing"
      "Drawer", "Overlays and navigation"
      "Dropdown Menu", "Selection and feedback"
      "Expansion Panel", "Content and surfaces"
      "Field", "Forms and input"
      "Grid", "Layout and spacing"
      "Link", "Actions and interactions"
      "List", "Content and surfaces"
      "Numeric Field", "Forms and input"
      "Pagination", "Overlays and navigation"
      "Radio Button", "Forms and input"
      "Select", "Forms and input"
      "Slider", "Forms and input"
      "Switch", "Forms and input"
      "Table", "Content and surfaces"
      "Tabs", "Overlays and navigation"
      "Text Field", "Forms and input"
    ]

    let pageSize = 5
    let count = (records.Length + pageSize - 1) / pageSize
    let page = Var.Create 1

    let content =
      Card.create (
        Doc.Concat [
          CardHeader.create (
            h3 [] [ text "Component directory" ],
            subtitle = text "Browse the building blocks of your next interface.",
            actions =
              Chip.create (text "23 components", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
          )
          CardContent.create (
            page.View
            |> Doc.BindView(fun selected ->
              let start = (max 1 (min count selected) - 1) * pageSize

              WeaveList.create (
                records
                |> List.skip start
                |> List.truncate pageSize
                |> List.mapi (fun index (name, category) ->
                  ListItem.create (
                    div [ Attr.Class "docs-pagination__record" ] [
                      span [ Typography.caption; Attr.Class "docs-pagination__record-number" ] [
                        text (sprintf "%02d" (start + index + 1))
                      ]
                      span [] [ text name ]
                      span [ Typography.caption; Attr.Class "docs-pagination__muted" ] [ text category ]
                    ]
                  )),
                readOnly = View.Const true,
                bordered = View.Const false
              )),
            attrs = [ Attr.Class "docs-pagination__records" ]
          )
          div [ Attr.Class "docs-pagination__data-footer" ] [
            div [
              Typography.caption
              Attr.Class "docs-pagination__muted"
              Attr.Create "aria-live" "polite"
            ] [
              textView (
                page.View
                |> View.Map(fun selected ->
                  sprintf
                    "Showing %d–%d of %d"
                    ((selected - 1) * pageSize + 1)
                    (min records.Length (selected * pageSize))
                    records.Length)
              )
            ]
            Pagination.create (
              View.Const count,
              selectedPage = page,
              attrs = [ Attr.Create "aria-label" "Component directory pages" ]
            )
          ]
        ],
        attrs = [ Attr.Class "docs-pagination__directory" ]
      )

    Helpers.codeSampleSection
      "Paging data"
      (Helpers.bodyText
        "A pager chooses the page; your application loads or slices the records. Here, one Var drives both the directory and its navigation. Use aria-label to give each pager a distinct accessible name.")
      content
      """let pageSize = 5
let page = Var.Create 1
let count = View.Const ((records.Length + pageSize - 1) / pageSize)

page.View |> Doc.BindView (fun selected ->
    records
    |> List.skip ((selected - 1) * pageSize)
    |> List.truncate pageSize
    |> List.map (fun (name, category) ->
        ListItem.create (text name, secondaryContent = text category))
    |> fun items -> WeaveList.create (items, readOnly = View.Const true))

Pagination.create (
    count,
    selectedPage = page,
    attrs = [ Attr.Create "aria-label" "Component directory pages" ]
)"""

  let private apiReference () =
    Helpers.apiSection
      (Helpers.bodyText
        "Pagination.create returns a Doc. Controls use native button keyboard behavior: Tab to focus, Enter or Space to activate.")
      [
        Helpers.apiTable "Pagination.create" [
          Helpers.apiParam "count" "View<int>" "" "Reactive total page count; negative counts behave as zero"
          Helpers.apiParam
            "?selectedPage"
            "Var<int>"
            "Var.Create 1"
            "One-based, two-way selection; display clamps to the valid range without changing this Var"
          Helpers.apiParam
            "?boundaryCount"
            "int"
            "1"
            "Visible pages at each end; negative values behave as zero"
          Helpers.apiParam
            "?siblingCount"
            "int"
            "1"
            "Visible neighbors on each side; negative values behave as zero"
          Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Enable all controls"
          Helpers.apiParam "?attrs" "Attr list" "[]" "Attributes and styling on the navigation element"
        ]
        Helpers.returnTypeNote
          "Navigation arrows use Weave's outlined Material Symbols icons. Load the Material Symbols Outlined font as shown on the Icons page."
        Helpers.styleModuleTable "Pagination.Controls" [
          "showFirstLast", "Show first and last page buttons (hidden by default)"
          "hidePreviousNext", "Hide previous and next page buttons (visible by default)"
        ]
        Helpers.returnTypeNote
          "Hidden controls remain mounted but are excluded from keyboard navigation and the accessibility tree. Set Attr.Create \"aria-label\" \"Records pages\" in attrs to override the default navigation label, Pagination."
        Helpers.styleModuleTable "Pagination.Variant" [
          "text", "Default: subtle background on the current page"
          "filled", "Solid palette background on the current page"
          "outlined", "Outlined buttons with an accented current page"
        ]
        Helpers.styleModuleTable "Pagination.Color" [
          "primary", "Primary (default)"
          "secondary", "Secondary"
          "tertiary", "Tertiary"
          "error", "Error"
          "warning", "Warning"
          "success", "Success"
          "info", "Info"
        ]
        Helpers.returnTypeNote
          "When count is zero, no page buttons render and navigation controls disable. If the count shrinks, the display clamps selection; the caller's Var changes only when a different page is activated. Reset the Var in your application when a filter should start again at page one."
      ]

  let render () =
    Container.create (
      div [ Attr.Class "docs-pagination" ] [
        Helpers.pageTitle "Pagination"
        Helpers.bodyText
          "Navigate a collection one page at a time with reactive selection and compact page ranges."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        stylesExample ()
        Helpers.divider ()
        rangesExample ()
        Helpers.divider ()
        dataExample ()
        Helpers.divider ()
        apiReference ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
