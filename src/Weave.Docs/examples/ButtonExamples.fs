namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ButtonExamples =

  let private variantExamples () =
    let description =
      Helpers.bodyText "Buttons come in three variants: Filled, Outlined, and Text"

    let content =
      Grid.create (
        [
          GridItem.create (
            Button.primary (text "Filled", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
          )
          GridItem.create (
            Button.primary (text "Outlined", onClick = (fun () -> ()), attrs = [ Button.Variant.outlined ])
          )
          GridItem.create (
            Button.primary (text "Text", onClick = (fun () -> ()), attrs = [ Button.Variant.text ])
          )
        ]
      )

    let code =
      """open Weave

Button.primary(
    text "Filled",
    onClick = (fun () -> ()),
    attrs = [ Button.Variant.filled ]
)

Button.primary(
    text "Outlined",
    onClick = (fun () -> ()),
    attrs = [ Button.Variant.outlined ]
)

Button.primary(
    text "Text",
    onClick = (fun () -> ()),
    attrs = [ Button.Variant.text ]
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private colorExamples () =
    let colors = [
      "Primary", Button.Color.primary
      "Secondary", Button.Color.secondary
      "Tertiary", Button.Color.tertiary
      "Error", Button.Color.error
      "Warning", Button.Color.warning
      "Success", Button.Color.success
      "Info", Button.Color.info
    ]

    let description =
      Helpers.bodyText
        "Apply brand colors using shorthand constructors like Button.primary, Button.error, etc. These apply the color automatically — no need to pass Button.Color.x in attrs."

    let code =
      """open Weave

// Each color has a shorthand constructor: Button.primary, Button.secondary, etc.
Button.primary(text "Primary", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
Button.secondary(text "Secondary", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
Button.tertiary(text "Tertiary", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
Button.error(text "Error", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
Button.warning(text "Warning", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
Button.success(text "Success", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
Button.info(text "Info", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])"""

    let content =
      Grid.create (
        colors
        |> List.collect (fun (colorName, colorAttr) -> [
          GridItem.create (
            Button.create (
              text colorName,
              onClick = (fun () -> printfn "%s clicked" colorName),
              attrs = [ Button.Variant.filled; colorAttr; Button.Width.full ]
            )
          )
        ])
      )

    Helpers.codeSampleSection "Colors" description content code

  let private densityExamples () =
    let description =
      Helpers.bodyText
        "Density controls button height and padding. Pass the density class in attrs to set it per-instance. See the Density section on the Weave Styling page for container-level usage."

    let content =
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
          div [] [
            Button.primary (text "Filled", onClick = (fun () -> ()), attrs = [ Button.Variant.filled ])
          ]
          div [ Margin.Top.extraSmall ] [
            Button.primary (text "Outlined", onClick = (fun () -> ()), attrs = [ Button.Variant.outlined ])
          ]
          div [ Margin.Top.extraSmall ] [
            Button.primary (text "Text", onClick = (fun () -> ()), attrs = [ Button.Variant.text ])
          ]
        ]

      Grid.create (
        [
          GridItem.create (
            col "Compact" Density.compact,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Standard" Density.standard,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
          )
        ],
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave

Button.primary(
    text "Compact",
    onClick = (fun () -> ()),
    attrs = [
        Density.compact
        Button.Variant.filled
    ]
)

Button.primary(
    text "Standard",
    onClick = (fun () -> ()),
    attrs = [
        Density.standard
        Button.Variant.filled
    ]
)

Button.primary(
    text "Spacious",
    onClick = (fun () -> ()),
    attrs = [
        Density.spacious
        Button.Variant.filled
    ]
)"""

    Helpers.codeSampleSection "Density" description content code

  let private disabledExamples () =
    let description =
      Helpers.bodyText "Buttons can be disabled using the enabled parameter"

    let content =
      Grid.create (
        [
          GridItem.create (
            Button.primary (
              text "Enabled",
              onClick = (fun () -> printfn "Enabled clicked"),
              enabled = View.Const true,
              attrs = [ Button.Variant.filled ]
            )
          )
          GridItem.create (
            Button.create (
              text "Disabled",
              onClick = (fun () -> printfn "This won't fire"),
              enabled = View.Const false,
              attrs = [ Button.Variant.filled ]
            )
          )
        ]
      )

    let code =
      """open Weave
open WebSharper.UI

Button.primary(
    text "Enabled",
    onClick = (fun () -> printfn "Enabled clicked"),
    enabled = View.Const true,
    attrs = [ Button.Variant.filled ]
)

Button.create(
    text "Disabled",
    onClick = (fun () -> printfn "This won't fire"),
    enabled = View.Const false, // see here
    attrs = [ Button.Variant.filled ]
)"""

    Helpers.codeSampleSection "Disabled State" description content code

  let private fullWidthExample () =
    let description =
      Helpers.bodyText "Buttons can span the full width of their container"

    let content =
      Button.primary (
        text "Full Width Button",
        onClick = (fun () -> printfn "Full width clicked"),
        attrs = [ Button.Variant.filled; Button.Width.full ]
      )

    let code =
      """open Weave

Button.primary(
    text "Full Width Button",
    onClick = (fun () -> printfn "Full width clicked"),
    attrs = [
        Button.Variant.filled
        Button.Width.full // see here
    ]
)"""

    Helpers.codeSampleSection "Full Width" description content code

  let private borderRadiusExamples () =
    let description = Helpers.bodyText "Buttons can have different border radius styles"

    let content =
      Grid.create (
        [
          let btn label radiusAttr =
            GridItem.create (
              Button.primary (
                text label,
                onClick = (fun () -> ()),
                attrs = [ Button.Variant.filled; radiusAttr ]
              )
            )

          btn "None" BorderRadius.All.none
          btn "Small" BorderRadius.All.small
          btn "Medium" BorderRadius.All.medium
          btn "Large" BorderRadius.All.large
          btn "Pill" BorderRadius.pill
          btn "Circle" BorderRadius.circle
        ]
      )

    let code =
      """open Weave

Button.primary(text "None", onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.All.none ])
Button.primary(text "Small", onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.All.small ])
Button.primary(text "Medium", onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.All.medium ])
Button.primary(text "Large", onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.All.large ])
Button.primary(text "Pill", onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.pill ])
Button.primary(text "Circle", onClick = (fun () -> ()), attrs = [ Button.Variant.filled; BorderRadius.circle ])"""

    Helpers.codeSampleSection "Border Radius" description content code

  let private iconButtonExamples () =
    let description =
      Helpers.bodyText
        "Icon buttons render a single icon without a text label. Use IconButton.create to create them. They reuse the same variant, color, and size classes as regular buttons."

    let content =
      Grid.create (
        [
          GridItem.create (
            IconButton.error (
              Icon.create (Icon.UiActions UiActions.Delete),
              onClick = (fun () -> printfn "delete clicked"),
              attrs = [
                Attr.Create "aria-label" "delete"
                Button.Variant.filled
                BorderRadius.circle
              ]
            )
          )
          GridItem.create (
            IconButton.secondary (
              Icon.create (Icon.UiActions UiActions.Favorite),
              onClick = (fun () -> printfn "favorite clicked"),
              attrs = [
                Attr.Create "aria-label" "favorite"
                Button.Variant.filled
                BorderRadius.circle
              ]
            )
          )
          GridItem.create (
            IconButton.primary (
              Icon.create (Icon.UiActions UiActions.Home),
              onClick = (fun () -> printfn "home clicked"),
              attrs = [ Attr.Create "aria-label" "home"; Button.Variant.filled; BorderRadius.circle ]
            )
          )
          GridItem.create (
            IconButton.info (
              Icon.create (Icon.UiActions UiActions.Search),
              onClick = (fun () -> printfn "search clicked"),
              attrs = [
                Attr.Create "aria-label" "search"
                Button.Variant.filled
                BorderRadius.circle
              ]
            )
          )
        ]
      )

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

IconButton.error(
    Icon.create(Icon.UiActions UiActions.Delete),
    onClick = (fun () -> printfn "delete clicked"),
    attrs = [
        Attr.Create "aria-label" "delete"
        Button.Variant.filled
        BorderRadius.circle
    ]
)

IconButton.secondary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> printfn "favorite clicked"),
    attrs = [
        Attr.Create "aria-label" "favorite"
        Button.Variant.filled
        BorderRadius.circle
    ]
)

IconButton.primary(
    Icon.create(Icon.UiActions UiActions.Home),
    onClick = (fun () -> printfn "home clicked"),
    attrs = [
        Attr.Create "aria-label" "home"
        Button.Variant.filled
        BorderRadius.circle
    ]
)

IconButton.info(
    Icon.create(Icon.UiActions UiActions.Search),
    onClick = (fun () -> printfn "search clicked"),
    attrs = [
        Attr.Create "aria-label" "search"
        Button.Variant.filled
        BorderRadius.circle
    ]
)"""

    Helpers.codeSampleSection "Icon Buttons" description content code

  let private iconButtonDensityExamples () =
    let description =
      Helpers.bodyText
        "Icon buttons respond to density the same way as text buttons. Pass the density class in attrs to set it per-instance. See the Density section on the Weave Styling page for container-level usage."

    let content =
      let row (label: string) densityAttr =
        div [ densityAttr; Margin.Bottom.small ] [
          Grid.create (
            [
              GridItem.create (
                div [ Typography.subtitle2 ] [ text label ],
                attrs = [ GridItem.Span.twelve; GridItem.Span.Small.three ]
              )
              GridItem.create (
                IconButton.secondary (
                  Icon.create (Icon.UiActions UiActions.Favorite),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "favorite"; Button.Variant.filled ]
                ),
                attrs = [ GridItem.Span.four; GridItem.Span.Small.three ]
              )
              GridItem.create (
                IconButton.error (
                  Icon.create (Icon.UiActions UiActions.Delete),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "delete"; Button.Variant.outlined ]
                ),
                attrs = [ GridItem.Span.four; GridItem.Span.Small.three ]
              )
              GridItem.create (
                IconButton.primary (
                  Icon.create (Icon.UiActions UiActions.Search),
                  onClick = (fun () -> ()),
                  attrs = [ Attr.Create "aria-label" "search"; Button.Variant.text ]
                ),
                attrs = [ GridItem.Span.four; GridItem.Span.Small.three ]
              )
            ],
            attrs = [ JustifyContent.flexStart; AlignItems.center ]
          )
        ]

      div [] [
        row "Compact" Density.compact
        row "Standard" Density.standard
        row "Spacious" Density.spacious
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

IconButton.primary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        Density.compact // see here
        Button.Variant.filled
    ]
)

IconButton.primary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        Density.standard
        Button.Variant.filled
    ]
)

IconButton.primary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> ()),
    attrs = [
        Attr.Create "aria-label" "favorite"
        Density.spacious
        Button.Variant.filled
    ]
)"""

    Helpers.codeSampleSection "Icon Button Density" description content code

  let private iconButtonDisabledExamples () =
    let description =
      Helpers.bodyText "Icon buttons can be disabled the same way as regular buttons."

    let content =
      Grid.create (
        [
          GridItem.create (
            IconButton.secondary (
              Icon.create (Icon.UiActions UiActions.Favorite),
              onClick = (fun () -> printfn "favorite clicked"),
              enabled = View.Const true,
              attrs = [ Attr.Create "aria-label" "favorite"; Button.Variant.filled ]
            )
          )
          GridItem.create (
            IconButton.create (
              Icon.create (Icon.UiActions UiActions.Favorite),
              onClick = (fun () -> printfn "This won't fire"),
              enabled = View.Const false,
              attrs = [ Attr.Create "aria-label" "favorite"; Button.Variant.filled ]
            )
          )
        ]
      )

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols
open WebSharper.UI

IconButton.secondary(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> printfn "favorite clicked"),
    enabled = View.Const true,
    attrs = [
        Attr.Create "aria-label" "favorite"
        Button.Variant.filled
    ]
)

IconButton.create(
    Icon.create(Icon.UiActions UiActions.Favorite),
    onClick = (fun () -> printfn "This won't fire"),
    enabled = View.Const false, // see here
    attrs = [
        Attr.Create "aria-label" "favorite"
        Button.Variant.filled
    ]
)"""

    Helpers.codeSampleSection "Disabled Icon Buttons" description content code

  let private whenToUseSection () =
    let description =
      div [ Typography.body1 ] [
        text "Both "
        Helpers.inlineCode "Button"
        text " and "
        Helpers.inlineCode "IconButton"
        text " trigger actions — but they communicate intent differently. Use this to pick the right one."
      ]

    let content =
      div [] [
        Helpers.guidanceColumns
          (Helpers.guidanceCard "Use Button when\u2026" [
            Helpers.guidanceBullet
              "The action needs a text label"
              "labels like Save, Cancel, or Submit make the purpose immediately clear to all users."
            Helpers.guidanceBullet
              "Users must distinguish multiple actions"
              "side-by-side buttons need visible labels to differentiate — icons alone can be ambiguous."
            Helpers.guidanceBullet
              "The action is the primary call-to-action"
              "filled or outlined variants give text buttons the visual weight a CTA needs."
            Helpers.guidanceBullet
              "Accessibility requires a visible label"
              "screen readers and sighted users both benefit from explicit text."
          ])
          (Helpers.guidanceCard "Use IconButton when\u2026" [
            Helpers.guidanceBullet
              "The icon is universally understood"
              "close (X), menu (hamburger), and theme toggle have strong recognition."
            Helpers.guidanceBullet
              "Space is constrained"
              "toolbars, table rows, and mobile app bars leave little room for text."
            Helpers.guidanceBullet
              "The action is secondary or repeated"
              "edit and delete icons in a list row keep the layout clean."
            Helpers.guidanceBullet
              "Always pair with aria-label or Tooltip"
              "a bare icon without a text alternative fails WCAG 2.1 SC 1.1.1."
          ])

        Alert.create (
          text
            "IconButton always needs an aria-label or Tooltip so screen readers can announce the action. Use Tooltip.create to provide both a visual hint and accessible name.",
          attrs = [ Alert.Color.warning; Alert.Variant.outlined; Margin.Top.small ]
        )
      ]

    Helpers.sectionPlain "When to Use" description content

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Button and IconButton.") [
      Helpers.apiTable "Button.create" [
        Helpers.apiParam "innerContents" "Doc" "" "Content rendered inside the button label"
        Helpers.apiParam "onClick" "unit -> unit" "" "Callback invoked on click, tap, or keyboard activation"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Reactive enabled/disabled state"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (variant, color, width, etc.)"
      ]

      Helpers.apiTable "IconButton.create" [
        Helpers.apiParam "icon" "Doc" "" "Icon content rendered inside the button"
        Helpers.apiParam "onClick" "unit -> unit" "" "Callback invoked on click, tap, or keyboard activation"
        Helpers.apiParam "?enabled" "View<bool>" "View.Const true" "Reactive enabled/disabled state"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (variant, color, etc.)"
      ]

      Helpers.styleModuleTable "Button.Variant" [
        ("filled", "Solid background button (default)")
        ("outlined", "Bordered button with transparent background")
        ("text", "Text-only button with no background or border")
      ]

      Helpers.styleModuleTable "Button.Color" [
        ("primary", "Primary brand color")
        ("secondary", "Secondary brand color")
        ("tertiary", "Tertiary brand color")
        ("error", "Error/red color")
        ("warning", "Warning/orange color")
        ("success", "Success/green color")
        ("info", "Info/blue color")
      ]

      Helpers.styleModuleTable "Button.Width" [ ("full", "Button stretches to fill its container width") ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Button"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Buttons allow users to take actions and make choices with a single tap."
        ]

        Helpers.divider ()
        whenToUseSection ()
        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        colorExamples ()
        Helpers.divider ()
        densityExamples ()
        Helpers.divider ()
        disabledExamples ()
        Helpers.divider ()
        fullWidthExample ()
        Helpers.divider ()
        borderRadiusExamples ()
        Helpers.divider ()
        iconButtonExamples ()
        Helpers.divider ()
        iconButtonDensityExamples ()
        Helpers.divider ()
        iconButtonDisabledExamples ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
