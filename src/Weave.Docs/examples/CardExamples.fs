namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module CardExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText
        "Use Card.create to group related content on a bordered surface. Compose it with CardHeader, CardContent, and CardActions."

    let content =
      div [
        SurfaceColor.BackgroundColor.background
        Padding.All.small
        BorderRadius.All.small
      ] [
        Card.create (
          Doc.Concat [
            CardHeader.create (text "Weekly summary", subtitle = text "Updated today")
            CardContent.create (
              div [ Typography.body1 ] [
                text "Cards keep a visible edge so their bounds are clear without hover."
              ]
            )
            CardActions.create (
              Doc.Concat [
                Button.create (text "Dismiss", (fun () -> ()), attrs = [ Button.Variant.text ])
                Button.primary (text "Open", (fun () -> ()), attrs = [ Button.Variant.filled ])
              ]
            )
          ]
        )
      ]

    let code =
      """open Weave

Card.create(
    Doc.Concat [
        CardHeader.create(text "Weekly summary", subtitle = text "Updated today")
        CardContent.create(
            div [ Typography.body1 ] [ text "Cards keep a visible edge so their bounds are clear without hover." ]
        )
        CardActions.create(
            Doc.Concat [
                Button.create(text "Dismiss", (fun () -> ()), attrs = [ Button.Variant.text ])
                Button.primary(text "Open", (fun () -> ()), attrs = [ Button.Variant.filled ])
            ]
        )
    ]
)"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private variantExamples () =
    let description =
      Helpers.bodyText
        "A card has a visible border by default. Card.Variant.elevated swaps the border for a shadow, and Card.Variant.filled uses a tinted surface instead."

    let card label variant =
      GridItem.create (
        Card.create (
          Doc.Concat [
            CardHeader.create (text label)
            CardContent.create (div [ Typography.body2 ] [ text "Card content" ])
          ],
          attrs = [ variant ]
        ),
        attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
      )

    let content =
      div [
        SurfaceColor.BackgroundColor.background
        Padding.All.small
        BorderRadius.All.small
      ] [
        Grid.create (
          [
            card "Default" Attr.Empty
            card "Elevated" Card.Variant.elevated
            card "Filled" Card.Variant.filled
          ],
          attrs = [ Grid.Spacing.small ]
        )
      ]

    let code =
      """open Weave

Card.create(CardContent.create(text "Default"))

Card.create(
    CardContent.create(text "Elevated"),
    attrs = [ Card.Variant.elevated ]
)

Card.create(
    CardContent.create(text "Filled"),
    attrs = [ Card.Variant.filled ]
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private headerActionsExample () =
    let description =
      Helpers.bodyText
        "Pass leading to CardHeader to place an avatar or icon before the title, and actions to place controls such as icon buttons at the trailing edge. Pass a heading element such as h3 as the title so it appears in the page outline at the level that fits the surrounding content. The header keeps its own title style."

    let closed = Var.Create false

    let content =
      div [
        SurfaceColor.BackgroundColor.background
        Padding.All.small
        BorderRadius.All.small
      ] [
        closed.View
        |> Doc.BindView(fun isClosed ->
          if isClosed then
            Button.info (text "Reset", (fun () -> Var.Set closed false), attrs = [ Button.Variant.outlined ])
          else
            Card.create (
              Doc.Concat [
                CardHeader.create (
                  h3 [] [ text "Client profile" ],
                  subtitle = text "Overview",
                  leading = Icon.create (Icon.UiActions UiActions.Home),
                  actions =
                    IconButton.create (
                      Icon.create (Icon.UiActions UiActions.Close),
                      (fun () -> Var.Set closed true),
                      attrs = [ Attr.Create "aria-label" "close card" ]
                    )
                )
                CardContent.create (
                  div [ Typography.body2 ] [ text "Click the close icon to dismiss this card." ]
                )
              ]
            ))
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

let closed = Var.Create false

Card.create(
    Doc.Concat [
        CardHeader.create(
            h3 [] [ text "Client profile" ],
            subtitle = text "Overview",
            leading = Icon.create(Icon.UiActions UiActions.Home),
            actions =
                IconButton.create(
                    Icon.create(Icon.UiActions UiActions.Close),
                    (fun () -> Var.Set closed true),
                    attrs = [ Attr.Create "aria-label" "close card" ]
                )
        )
        CardContent.create(text "Click the close icon to dismiss this card.")
    ]
)"""

    Helpers.codeSampleSection "Header Content" description content code

  let private actionAreaExample () =
    let description =
      Helpers.bodyText
        "Wrap sections in CardActionArea to make them one target for the card's primary action. It renders a link when href is supplied and a button otherwise. Keep secondary controls such as CardActions outside the area so a click on them does not also trigger the primary action."

    let opened = Var.Create 0

    let content =
      div [
        SurfaceColor.BackgroundColor.background
        Padding.All.small
        BorderRadius.All.small
      ] [
        div [ Attr.Style "max-width" "320px" ] [
          Card.create (
            Doc.Concat [
              CardActionArea.create (
                Doc.Concat [
                  CardMedia.create (
                    div [
                      Attr.Style "height" "120px"
                      Attr.Style "background" "var(--palette-background-paper)"
                    ] [],
                    attrs = [ Attr.Create "aria-hidden" "true" ]
                  )
                  CardHeader.create (text "Weekly summary", subtitle = text "Updated today")
                  CardContent.create (
                    div [ Typography.body2 ] [
                      text "Opened "
                      textView (opened.View |> View.Map string<int>)
                      text " times"
                    ]
                  )
                ],
                onClick = (fun () -> Var.Update opened ((+) 1))
              )
              CardActions.create (
                Button.create (text "Share", (fun () -> ()), attrs = [ Button.Variant.text ])
              )
            ]
          )
        ]
      ]

    let code =
      """open Weave

let openSummary () = ()
let share () = ()

Card.create(
    Doc.Concat [
        CardActionArea.create(
            Doc.Concat [
                CardMedia.create(img [ attr.src "/summary.png"; attr.alt "" ] [], attrs = [ Attr.Style "height" "120px" ])
                CardHeader.create(text "Weekly summary", subtitle = text "Updated today")
                CardContent.create(text "Opens the summary")
            ],
            onClick = openSummary
        )
        CardActions.create(Button.create(text "Share", share, attrs = [ Button.Variant.text ]))
    ]
)

// As a link
CardActionArea.create(CardContent.create(text "Weekly summary"), href = "/summary")"""

    Helpers.codeSampleSection "Primary Action and Media" description content code

  let private apiReferenceSection () =
    Helpers.apiSection (Helpers.bodyText "Complete API reference for Card.") [
      Helpers.apiTable "Card.create" [
        Helpers.apiParam "content" "Doc" "" "The card content, typically composed of the card section types"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes (variant, spacing, etc.)"
      ]

      Helpers.apiTable "CardHeader.create" [
        Helpers.apiParam
          "title"
          "Doc"
          ""
          "The header title. Pass a heading element to include it in the page outline"
        Helpers.apiParam "?subtitle" "Doc" "" "Optional secondary text beneath the title"
        Helpers.apiParam "?leading" "Doc" "" "Optional avatar or icon placed before the title"
        Helpers.apiParam "?actions" "Doc" "" "Optional controls placed at the trailing edge"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes"
      ]

      Helpers.apiTable "CardMedia.create" [
        Helpers.apiParam "content" "Doc" "" "An image, video, or other media stretched edge to edge"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes, such as a height"
      ]

      Helpers.apiTable "CardActionArea.create" [
        Helpers.apiParam "content" "Doc" "" "The card sections the clickable area covers"
        Helpers.apiParam "?onClick" "unit -> unit" "" "Called when the area is activated"
        Helpers.apiParam "?href" "string" "" "Renders the area as a link to this address"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes"
      ]

      Helpers.apiTable "CardContent.create" [
        Helpers.apiParam "content" "Doc" "" "The main content region"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes"
      ]

      Helpers.apiTable "CardActions.create" [
        Helpers.apiParam "content" "Doc" "" "A footer row of actions aligned to the trailing edge"
        Helpers.apiParam "?attrs" "Attr list" "[]" "Additional attributes"
      ]

      Helpers.styleModuleTable "Card.Variant" [
        ("elevated", "Shadowed surface with no visible border")
        ("filled", "Tinted surface with no visible border")
      ]
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Card"
        Helpers.bodyText "Groups related content and actions on a single surface."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        headerActionsExample ()
        Helpers.divider ()
        actionAreaExample ()
        Helpers.divider ()
        apiReferenceSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
