namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs

[<JavaScript>]
module PaginationExamples =

  let private basicExample () =
    let content =
      let page = Var.Create 1

      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g2
      ] [
        Pagination.create (
          View.Const 10,
          selectedPage = page,
          attrs = [ Attr.Create "aria-label" "Collection pages" ]
        )
        p [ Attr.Create "role" "status" ] [ textView (page.View |> View.Map(sprintf "Page %d of 10")) ]
      ]

    Helpers.codeSampleSection
      "Basic pagination"
      (Helpers.bodyText
        "Page numbers start at one. Share selectedPage with the content that changes; arrows stop at either end.")
      content
      """let page = Var.Create 1

Pagination.create (
  View.Const 10,
  selectedPage = page,
  attrs = [ Attr.Create "aria-label" "Collection pages" ]
)"""

  let appearance () =
    let settings = Var.Create PaginationAppearance.defaults
    let page = Var.Create 2

    let choice label options get set =
      let selected =
        Var.Lens settings (get >> Some) (fun current value ->
          match value with
          | Some value -> set current value
          | None -> current)

      let items =
        options
        |> List.map (fun (value: string) ->
          let name = value.Substring(0, 1).ToUpper() + value.Substring(1)
          SelectItem.create (text name, name, value))
        |> View.Const

      Select.create (
        items,
        selected,
        labelText = View.Const label,
        attrs = [
          Select.Variant.outlined
          Select.Label.above
          Select.Width.full
          Select.PopoverWidth.unbound
          Select.Color.primary
        ]
      )

    let controls =
      div [ Attr.Class "docs-appearance__controls" ] [
        choice "Variant" PaginationAppearance.variants (fun c -> c.Variant) (fun c value -> {
          c with
              Variant = value
        })
        choice "Color" PaginationAppearance.colors (fun c -> c.Color) (fun c value -> {
          c with
              Color = value
        })
      ]

    let firstLast =
      Var.Lens settings (fun c -> c.FirstLast) (fun c value -> { c with FirstLast = value })

    let disabled =
      Var.Lens settings (fun c -> c.Disabled) (fun c value -> { c with Disabled = value })

    let switches =
      div [ Attr.Class "docs-appearance__actions" ] [
        Switch.create (firstLast, text "First and last")
        Switch.create (disabled, text "Disabled")
      ]

    let attrs =
      Map.ofList [
        "Pagination.Variant.filled", Pagination.Variant.filled
        "Pagination.Variant.outlined", Pagination.Variant.outlined
        "Pagination.Color.secondary", Pagination.Color.secondary
        "Pagination.Color.tertiary", Pagination.Color.tertiary
        "Pagination.Color.error", Pagination.Color.error
        "Pagination.Color.warning", Pagination.Color.warning
        "Pagination.Color.success", Pagination.Color.success
        "Pagination.Color.info", Pagination.Color.info
        "Pagination.Controls.showFirstLast", Pagination.Controls.showFirstLast
      ]

    let preview =
      settings.View
      |> Doc.BindView(fun current ->
        Pagination.create (
          View.Const 3,
          selectedPage = page,
          enabled = View.Const(not current.Disabled),
          attrs = [
            Attr.Create "aria-label" "Collection pages"
            yield!
              PaginationAppearance.attributes current
              |> List.map (fun attr -> Map.find attr attrs)
          ]
        ))

    let reset () =
      Var.Set settings PaginationAppearance.defaults
      Var.Set page 2

    Helpers.appearanceSection
      [ "variants-and-colors" ]
      (Helpers.bodyText "Combine a variant, color, and controls. Select a page to update the example code.")
      controls
      (Some switches)
      preview
      reset
      ((settings.View, page.View) ||> View.Map2 PaginationAppearance.source)

  let private rangesExample () =
    let content =
      div [
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g4
      ] [
        Pagination.create (
          View.Const 100,
          selectedPage = Var.Create 50,
          attrs = [
            Pagination.Controls.showFirstLast
            Attr.Create "aria-label" "First and last controls example"
          ]
        )
        Pagination.create (
          View.Const 100,
          selectedPage = Var.Create 50,
          boundaryCount = 2,
          siblingCount = 2,
          attrs = [
            Pagination.Controls.hidePreviousNext
            Attr.Create "aria-label" "Number only example"
          ]
        )
      ]

    Helpers.codeSampleSection
      "Long ranges"
      (Helpers.bodyText
        "Both pagers begin at page 50 of 100. The first adds first and last controls; the second shows two boundary pages and two neighboring pages.")
      content
      """Pagination.create (
  View.Const 100,
  selectedPage = Var.Create 50,
  attrs = [
    Pagination.Controls.showFirstLast
    Attr.Create "aria-label" "First and last controls example"
  ]
)
Pagination.create (
  View.Const 100,
  selectedPage = Var.Create 50,
  boundaryCount = 2,
  siblingCount = 2,
  attrs = [
    Pagination.Controls.hidePreviousNext
    Attr.Create "aria-label" "Number only example"
  ]
)"""

  let private dataExample () =
    let content =
      let records = [ "Button"; "Card"; "Checkbox"; "Dialog"; "Grid"; "Text Field" ]
      let pageSize = 3
      let count = (records.Length + pageSize - 1) / pageSize
      let page = Var.Create 1

      let visibleRecords =
        page.View
        |> Doc.BindView(fun selected ->
          records
          |> List.skip ((max 1 (min count selected) - 1) * pageSize)
          |> List.truncate pageSize
          |> List.map (fun name -> ListItem.create (text name))
          |> fun items -> WeaveList.create (items, readOnly = View.Const true))

      div [] [
        visibleRecords
        Pagination.create (
          View.Const count,
          selectedPage = page,
          attrs = [ Attr.Create "aria-label" "Component directory pages" ]
        )
      ]

    Helpers.codeSampleSection
      "Paging data"
      (Helpers.bodyText
        "A pager chooses the page; your application loads or slices the records. This six-item list uses three records per page.")
      content
      """let records = [ "Button"; "Card"; "Checkbox"; "Dialog"; "Grid"; "Text Field" ]
let pageSize = 3
let count = (records.Length + pageSize - 1) / pageSize
let page = Var.Create 1

let visibleRecords =
  page.View
  |> Doc.BindView(fun selected ->
    records
    |> List.skip ((max 1 (min count selected) - 1) * pageSize)
    |> List.truncate pageSize
    |> List.map (fun name -> ListItem.create (text name))
    |> fun items -> WeaveList.create (items, readOnly = View.Const true))

Doc.Concat [
  visibleRecords
  Pagination.create (
    View.Const count,
    selectedPage = page,
    attrs = [ Attr.Create "aria-label" "Component directory pages" ]
  )
]"""

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
        appearance ()
        Helpers.divider ()
        rangesExample ()
        Helpers.divider ()
        dataExample ()
        Helpers.divider ()
        apiReference ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
