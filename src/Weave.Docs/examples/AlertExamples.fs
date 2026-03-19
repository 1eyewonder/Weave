namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module AlertExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText "Use Alert.create with a content doc to display a simple inline message."

    let content =
      div [
        SurfaceColor.toBackgroundColor SurfaceColor.Background
        Padding.All.small
        BorderRadius.All.small
      ] [ Alert.create (text "This is an alert.", attrs = [ Alert.Variant.standard ]) ]

    let code =
      """open Weave


Alert.create(
    text "This is an alert.",
    attrs = [
        Alert.Variant.standard
    ]
)"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private variantExamples () =
    let description =
      Helpers.bodyText "Alerts come in three variants: Standard, Outlined, and Filled."

    let content =
      Grid.create (
        [
          let alert label variant =
            GridItem.create (
              Alert.create (text label, attrs = [ variant; Alert.Color.info ]),
              xs = Grid.Width.create 12,
              md = Grid.Width.create 4
            )

          alert "Standard" Alert.Variant.standard
          alert "Outlined" Alert.Variant.outlined
          alert "Filled" Alert.Variant.filled
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave


Alert.create(
    text "Standard",
    attrs = [
        Alert.Variant.standard
        Alert.Color.info
    ]
)

Alert.create(
    text "Outlined",
    attrs = [
        Alert.Variant.outlined
        Alert.Color.info
    ]
)

Alert.create(
    text "Filled",
    attrs = [
        Alert.Variant.filled
        Alert.Color.info
    ]
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private colorExamples () =
    let description =
      Helpers.bodyText
        "Alerts support all brand colors. The default color uses the surface palette with no tint."

    let colors = [
      "Default", Attr.Empty
      "Primary", Alert.Color.primary
      "Secondary", Alert.Color.secondary
      "Tertiary", Alert.Color.tertiary
      "Error", Alert.Color.error
      "Warning", Alert.Color.warning
      "Success", Alert.Color.success
      "Info", Alert.Color.info
    ]

    let content =
      div [
        SurfaceColor.toBackgroundColor SurfaceColor.Background
        Padding.All.small
        BorderRadius.All.small
      ] [
        Grid.create (
          colors
          |> List.map (fun (label, colorAttr) ->
            GridItem.create (
              Alert.create (text label, attrs = [ Alert.Variant.standard; colorAttr ]),
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              md = Grid.Width.create 3
            )),
          spacing = Grid.GutterSpacing.create 2
        )
      ]

    let code =
      """open Weave


let colors = [
    "Default",   Attr.Empty
    "Primary",   Alert.Color.primary   // see here
    "Secondary", Alert.Color.secondary
    "Tertiary",  Alert.Color.tertiary
    "Error",     Alert.Color.error
    "Warning",   Alert.Color.warning
    "Success",   Alert.Color.success
    "Info",      Alert.Color.info
]

colors
|> List.map (fun (label, colorAttr) ->
    Alert.create(
        text label,
        attrs = [ Alert.Variant.standard; colorAttr ]
    )
)"""

    Helpers.codeSampleSection "Colors" description content code

  let private iconExamples () =
    let description =
      Helpers.bodyText "Pass an icon to place a leading icon inside the alert."

    let content =
      Grid.create (
        [
          GridItem.create (
            Alert.create (
              text "Your changes have been saved.",
              icon = Icon.create (Icon.UiActions UiActions.CheckCircle),
              attrs = [ Alert.Variant.standard; Alert.Color.success ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.create (
            Alert.create (
              text "This action cannot be undone.",
              icon = Icon.create (Icon.Action Action.Warning),
              attrs = [ Alert.Variant.standard; Alert.Color.warning ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.create (
            Alert.create (
              text "Something went wrong. Please try again.",
              icon = Icon.create (Icon.Action Action.Error),
              attrs = [ Alert.Variant.standard; Alert.Color.error ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.create (
            Alert.create (
              text "Your session will expire in 5 minutes.",
              icon = Icon.create (Icon.Action Action.Info),
              attrs = [ Alert.Variant.standard; Alert.Color.info ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols


Alert.create(
    text "Your changes have been saved.",
    icon = Icon.create(Icon.UiActions UiActions.CheckCircle),
    attrs = [
        Alert.Variant.standard
        Alert.Color.success
    ]
)"""

    Helpers.codeSampleSection "Icons" description content code

  let private closeExamples () =
    let description =
      Helpers.bodyText
        "Pass an onClose callback to render a close button. The callback is responsible for hiding or removing the alert."

    let visible = Var.Create true

    let content =
      div [] [
        visible.View
        |> Doc.BindView(fun show ->
          if show then
            Alert.create (
              text "Click the close button to dismiss this alert.",
              onClose = (fun () -> Var.Set visible false),
              attrs = [ Alert.Variant.outlined; Alert.Color.info ]
            )
          else
            Button.info (
              text "Reset",
              onClick = (fun () -> Var.Set visible true),
              attrs = [ Button.Variant.outlined ]
            ))
      ]

    let code =
      """open Weave

open WebSharper.UI

let visible = Var.Create true

visible.View
|> Doc.BindView (fun show ->
    if show then
        Alert.create(
            text "Click the close button to dismiss this alert.",
            onClose = (fun () -> Var.Set visible false),
            attrs = [
                Alert.Variant.outlined
                Alert.Color.info
            ]
        )
    else
        Doc.Empty
)"""

    Helpers.codeSampleSection "Close Button" description content code

  let private densityExamples () =
    let description =
      Helpers.bodyText
        "Density controls alert padding. Pass the density class in attrs to set it per-instance."

    let content =
      let col (label: string) densityAttr =
        div [ densityAttr ] [
          Subtitle2.div (label, attrs = [ Margin.Bottom.extraSmall ])
          Alert.create (
            text "This is an alert message.",
            attrs = [ Alert.Variant.standard; Alert.Color.info ]
          )
        ]

      Grid.create (
        [
          GridItem.create (col "Compact" Density.compact, xs = Grid.Width.create 12, sm = Grid.Width.create 4)
          GridItem.create (
            col "Standard" Density.standard,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
          GridItem.create (
            col "Spacious" Density.spacious,
            xs = Grid.Width.create 12,
            sm = Grid.Width.create 4
          )
        ],
        spacing = Grid.GutterSpacing.create 2,
        attrs = [ AlignItems.start ]
      )

    let code =
      """open Weave


Alert.create(
    text "Compact alert.",
    attrs = [
        Density.compact // see here
        Alert.Variant.standard
        Alert.Color.info
    ]
)

Alert.create(
    text "Standard alert.",
    attrs = [
        Density.standard // see here
        Alert.Variant.standard
        Alert.Color.info
    ]
)

Alert.create(
    text "Spacious alert.",
    attrs = [
        Density.spacious // see here
        Alert.Variant.standard
        Alert.Color.info
    ]
)
"""

    Helpers.codeSampleSection "Density" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Alert"
        Helpers.bodyText "Displays an important message inline within the page content."
        Helpers.divider ()
        basicExample ()
        Helpers.divider ()
        variantExamples ()
        Helpers.divider ()
        colorExamples ()
        Helpers.divider ()
        iconExamples ()
        Helpers.divider ()
        closeExamples ()
        Helpers.divider ()
        densityExamples ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
