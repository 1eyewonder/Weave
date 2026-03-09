namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols
open Weave.CssHelpers

[<JavaScript>]
module AlertExamples =

  let private basicExample () =
    let description =
      Helpers.bodyText "Use Alert.Create with a content doc to display a simple inline message."

    let content =
      div [
        SurfaceColor.toBackgroundColor SurfaceColor.Background
        cls [ yield! Padding.toClasses Padding.All.small; BorderRadius.toClass BorderRadius.All.small ]
      ] [
        Alert.Create(
          text "This is an alert.",
          attrs = [
            Alert.Variant.toClass Alert.Variant.Standard |> cl
          ]
        )
      ]

    let code =
      """open Weave
open Weave.CssHelpers

Alert.Create(
    text "This is an alert.",
    attrs = [
        Alert.Variant.toClass Alert.Variant.Standard |> cl
    ]
)"""

    Helpers.codeSampleSection "Basic Usage" description content code

  let private variantExamples () =
    let description =
      Helpers.bodyText "Alerts come in three variants: Standard, Outlined, and Filled."

    let content =
      Grid.Create(
        [
          let alert variant =
            let label = sprintf "%A" variant

            GridItem.Create(
              Alert.Create(
                text label,
                attrs = [
                  Alert.Variant.toClass variant |> cl
                  Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Info)
                  |> Option.map cl
                  |> Option.defaultValue Attr.Empty
                ]
              ),
              xs = Grid.Width.create 12,
              md = Grid.Width.create 4
            )

          alert Alert.Variant.Standard
          alert Alert.Variant.Outlined
          alert Alert.Variant.Filled
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

Alert.Create(
    text "Standard",
    attrs = [
        Alert.Variant.toClass Alert.Variant.Standard |> cl
        Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Info)
        |> Option.map cl
        |> Option.defaultValue Attr.Empty
    ]
)

Alert.Create(
    text "Outlined",
    attrs = [
        Alert.Variant.toClass Alert.Variant.Outlined |> cl
        Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Info)
        |> Option.map cl
        |> Option.defaultValue Attr.Empty
    ]
)

Alert.Create(
    text "Filled",
    attrs = [
        Alert.Variant.toClass Alert.Variant.Filled |> cl
        Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Info)
        |> Option.map cl
        |> Option.defaultValue Attr.Empty
    ]
)"""

    Helpers.codeSampleSection "Variants" description content code

  let private colorExamples () =
    let description =
      Helpers.bodyText
        "Alerts support all brand colors. The default color uses the surface palette with no tint."

    let colors = [
      Alert.AlertColor.Default
      Alert.AlertColor.BrandColor BrandColor.Primary
      Alert.AlertColor.BrandColor BrandColor.Secondary
      Alert.AlertColor.BrandColor BrandColor.Tertiary
      Alert.AlertColor.BrandColor BrandColor.Error
      Alert.AlertColor.BrandColor BrandColor.Warning
      Alert.AlertColor.BrandColor BrandColor.Success
      Alert.AlertColor.BrandColor BrandColor.Info
    ]

    let content =
      div [
        SurfaceColor.toBackgroundColor SurfaceColor.Background
        cls [ yield! Padding.toClasses Padding.All.small; BorderRadius.toClass BorderRadius.All.small ]
      ] [
        Grid.Create(
          colors
          |> List.map (fun color ->
            let label = sprintf "%A" color

            GridItem.Create(
              Alert.Create(
                text label,
                attrs = [
                  Alert.Variant.toClass Alert.Variant.Standard |> cl
                  Alert.AlertColor.toClass color |> Option.map cl |> Option.defaultValue Attr.Empty
                ]
              ),
              xs = Grid.Width.create 12,
              sm = Grid.Width.create 6,
              md = Grid.Width.create 3
            )),
          spacing = Grid.GutterSpacing.create 2
        )
      ]

    let code =
      """open Weave
open Weave.CssHelpers

let colors = [
    Alert.AlertColor.Default
    Alert.AlertColor.BrandColor BrandColor.Primary
    Alert.AlertColor.BrandColor BrandColor.Secondary
    Alert.AlertColor.BrandColor BrandColor.Tertiary
    Alert.AlertColor.BrandColor BrandColor.Error
    Alert.AlertColor.BrandColor BrandColor.Warning
    Alert.AlertColor.BrandColor BrandColor.Success
    Alert.AlertColor.BrandColor BrandColor.Info
]

colors
|> List.map (fun color ->
    Alert.Create(
        text (sprintf "%A" color),
        attrs = [
            Alert.Variant.toClass Alert.Variant.Standard |> cl
            Alert.AlertColor.toClass color |> Option.map cl |> Option.defaultValue Attr.Empty
        ]
    )
)"""

    Helpers.codeSampleSection "Colors" description content code

  let private iconExamples () =
    let description =
      Helpers.bodyText "Pass an icon to place a leading icon inside the alert."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Alert.Create(
              text "Your changes have been saved.",
              icon = Icon.Create(Icon.UiActions UiActions.CheckCircle),
              attrs = [
                Alert.Variant.toClass Alert.Variant.Standard |> cl
                Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Success)
                |> Option.map cl
                |> Option.defaultValue Attr.Empty
              ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.Create(
            Alert.Create(
              text "This action cannot be undone.",
              icon = Icon.Create(Icon.Action Action.Warning),
              attrs = [
                Alert.Variant.toClass Alert.Variant.Standard |> cl
                Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Warning)
                |> Option.map cl
                |> Option.defaultValue Attr.Empty
              ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.Create(
            Alert.Create(
              text "Something went wrong. Please try again.",
              icon = Icon.Create(Icon.Action Action.Error),
              attrs = [
                Alert.Variant.toClass Alert.Variant.Standard |> cl
                Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Error)
                |> Option.map cl
                |> Option.defaultValue Attr.Empty
              ]
            ),
            xs = Grid.Width.create 12,
            md = Grid.Width.create 6
          )
          GridItem.Create(
            Alert.Create(
              text "Your session will expire in 5 minutes.",
              icon = Icon.Create(Icon.Action Action.Info),
              attrs = [
                Alert.Variant.toClass Alert.Variant.Standard |> cl
                Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Info)
                |> Option.map cl
                |> Option.defaultValue Attr.Empty
              ]
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
open Weave.CssHelpers

Alert.Create(
    text "Your changes have been saved.",
    icon = Icon.Create(Icon.UiActions UiActions.CheckCircle),
    attrs = [
        Alert.Variant.toClass Alert.Variant.Standard |> cl
        Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Success)
        |> Option.map cl
        |> Option.defaultValue Attr.Empty
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
            Alert.Create(
              text "Click the close button to dismiss this alert.",
              onClose = (fun () -> Var.Set visible false),
              attrs = [
                Alert.Variant.toClass Alert.Variant.Outlined |> cl
                Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Info)
                |> Option.map cl
                |> Option.defaultValue Attr.Empty
              ]
            )
          else
            Button.Create(
              text "Reset",
              onClick = (fun () -> Var.Set visible true),
              attrs = [
                Button.Variant.Outlined |> Button.Variant.toClass |> cl
                Button.Color.toClass BrandColor.Info |> cl
              ]
            ))
      ]

    let code =
      """open Weave
open Weave.CssHelpers
open WebSharper.UI

let visible = Var.Create true

visible.View
|> Doc.BindView (fun show ->
    if show then
        Alert.Create(
            text "Click the close button to dismiss this alert.",
            onClose = (fun () -> Var.Set visible false),
            attrs = [
                Alert.Variant.toClass Alert.Variant.Outlined |> cl
                Alert.AlertColor.toClass (Alert.AlertColor.BrandColor BrandColor.Info)
                |> Option.map cl
                |> Option.defaultValue Attr.Empty
            ]
        )
    else
        Doc.Empty
)"""

    Helpers.codeSampleSection "Close Button" description content code

  let render () =
    Container.Create(
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
      ],
      maxWidth = Container.MaxWidth.Large
    )
