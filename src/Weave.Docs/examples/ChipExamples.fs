namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ChipExamples =

  let private filledChipsExample () =
    let description =
      Helpers.bodyText "Filled chips use a solid background and work well for emphasis."

    let colors = [
      "Primary", Chip.Color.primary
      "Secondary", Chip.Color.secondary
      "Tertiary", Chip.Color.tertiary
      "Error", Chip.Color.error
      "Warning", Chip.Color.warning
      "Success", Chip.Color.success
      "Info", Chip.Color.info
    ]

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Enabled" ]
        Grid.create (
          colors
          |> List.map (fun (label, colorAttr) ->
            GridItem.create (Chip.create (text label, attrs = [ Chip.Variant.filled; colorAttr ]))),
          attrs = [ Grid.Spacing.small ]
        )
        div [ Typography.subtitle2; Margin.Vertical.extraSmall ] [ text "Disabled" ]
        Grid.create (
          colors
          |> List.map (fun (label, colorAttr) ->
            GridItem.create (
              Chip.create (
                text label,
                enabled = View.Const false,
                attrs = [ Chip.Variant.filled; colorAttr ]
              )
            )),
          attrs = [ Grid.Spacing.small ]
        )
      ]

    let code =
      """open Weave

Chip.create(text "Primary", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
Chip.create(text "Secondary", attrs = [ Chip.Variant.filled; Chip.Color.secondary ])
Chip.create(text "Tertiary", attrs = [ Chip.Variant.filled; Chip.Color.tertiary ])
Chip.create(text "Error", attrs = [ Chip.Variant.filled; Chip.Color.error ])
Chip.create(text "Warning", attrs = [ Chip.Variant.filled; Chip.Color.warning ])
Chip.create(text "Success", attrs = [ Chip.Variant.filled; Chip.Color.success ])
Chip.create(text "Info", attrs = [ Chip.Variant.filled; Chip.Color.info ])"""

    Helpers.codeSampleSection "Filled Chips" description content code

  let private textChipsExample () =
    let description =
      Helpers.bodyText "Text chips have a transparent background and suit low-emphasis contexts."

    let colors = [
      "Primary", Chip.Color.primary
      "Secondary", Chip.Color.secondary
      "Tertiary", Chip.Color.tertiary
      "Error", Chip.Color.error
      "Warning", Chip.Color.warning
      "Success", Chip.Color.success
      "Info", Chip.Color.info
    ]

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Enabled" ]
        Grid.create (
          colors
          |> List.map (fun (label, colorAttr) ->
            GridItem.create (Chip.create (text label, attrs = [ Chip.Variant.text; colorAttr ]))),
          attrs = [ Grid.Spacing.small ]
        )
        div [ Typography.subtitle2; Margin.Vertical.extraSmall ] [ text "Disabled" ]
        Grid.create (
          colors
          |> List.map (fun (label, colorAttr) ->
            GridItem.create (
              Chip.create (text label, enabled = View.Const false, attrs = [ Chip.Variant.text; colorAttr ])
            )),
          attrs = [ Grid.Spacing.small ]
        )
      ]

    let code =
      """open Weave

Chip.create(text "Primary", attrs = [ Chip.Variant.text; Chip.Color.primary ])
Chip.create(text "Secondary", attrs = [ Chip.Variant.text; Chip.Color.secondary ])
Chip.create(text "Tertiary", attrs = [ Chip.Variant.text; Chip.Color.tertiary ])
Chip.create(text "Error", attrs = [ Chip.Variant.text; Chip.Color.error ])
Chip.create(text "Warning", attrs = [ Chip.Variant.text; Chip.Color.warning ])
Chip.create(text "Success", attrs = [ Chip.Variant.text; Chip.Color.success ])
Chip.create(text "Info", attrs = [ Chip.Variant.text; Chip.Color.info ])"""

    Helpers.codeSampleSection "Text Chips" description content code

  let private outlinedChipsExample () =
    let description =
      Helpers.bodyText "Outlined chips use a border and transparent background for a lightweight look."

    let colors = [
      "Primary", Chip.Color.primary
      "Secondary", Chip.Color.secondary
      "Tertiary", Chip.Color.tertiary
      "Error", Chip.Color.error
      "Warning", Chip.Color.warning
      "Success", Chip.Color.success
      "Info", Chip.Color.info
    ]

    let content =
      div [] [
        div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Enabled" ]
        Grid.create (
          colors
          |> List.map (fun (label, colorAttr) ->
            GridItem.create (Chip.create (text label, attrs = [ Chip.Variant.outlined; colorAttr ]))),
          attrs = [ Grid.Spacing.small ]
        )
        div [ Typography.subtitle2; Margin.Vertical.extraSmall ] [ text "Disabled" ]
        Grid.create (
          colors
          |> List.map (fun (label, colorAttr) ->
            GridItem.create (
              Chip.create (
                text label,
                enabled = View.Const false,
                attrs = [ Chip.Variant.outlined; colorAttr ]
              )
            )),
          attrs = [ Grid.Spacing.small ]
        )
      ]

    let code =
      """open Weave

Chip.create(text "Primary", attrs = [ Chip.Variant.outlined; Chip.Color.primary ])
Chip.create(text "Secondary", attrs = [ Chip.Variant.outlined; Chip.Color.secondary ])
Chip.create(text "Tertiary", attrs = [ Chip.Variant.outlined; Chip.Color.tertiary ])
Chip.create(text "Error", attrs = [ Chip.Variant.outlined; Chip.Color.error ])
Chip.create(text "Warning", attrs = [ Chip.Variant.outlined; Chip.Color.warning ])
Chip.create(text "Success", attrs = [ Chip.Variant.outlined; Chip.Color.success ])
Chip.create(text "Info", attrs = [ Chip.Variant.outlined; Chip.Color.info ])"""

    Helpers.codeSampleSection "Outlined Chips" description content code

  let private closableExample () =
    let description =
      Helpers.bodyText
        "Pass an onClose callback to render a close button. The callback handles removing the chip from the UI."

    let items = Var.Create [ "React"; "Angular"; "Vue"; "Svelte"; "WebSharper" ]

    let content =
      div [] [
        items.View
        |> Doc.BindView(fun currentItems ->
          if List.isEmpty currentItems then
            Button.primary (
              text "Reset",
              onClick = (fun () -> Var.Set items [ "React"; "Angular"; "Vue"; "Svelte"; "WebSharper" ]),
              attrs = [ Button.Variant.outlined ]
            )
          else
            div
              []
              (currentItems
               |> List.map (fun item ->
                 Chip.create (
                   text item,
                   onClose = (fun () -> items.Value |> List.filter (fun i -> i <> item) |> Var.Set items),
                   attrs = [
                     Chip.Variant.outlined
                     Chip.Color.primary
                     Margin.Right.extraSmall
                     Margin.Bottom.extraSmall
                   ]
                 ))))
      ]

    let code =
      """open Weave
open WebSharper.UI

let items = Var.Create ["React"; "Angular"; "Vue"; "Svelte"; "WebSharper"]

items.View
|> Doc.BindView (fun currentItems ->
    currentItems
    |> List.map (fun item ->
        Chip.create(
            text item,
            onClose = (fun () -> // see here
                items.Value
                |> List.filter (fun i -> i <> item)
                |> Var.Set items),
            attrs = [
                Chip.Variant.outlined
                Chip.Color.primary
            ]
        )
    )
)"""

    Helpers.codeSampleSection "Closable" description content code

  let private customCloseIconExample () =
    let description =
      Helpers.bodyText
        "Pass a closeIcon to replace the default close symbol with a custom icon. The icon inherits the close button's click behavior automatically."

    let items = Var.Create [ "Draft"; "Published"; "Archived" ]

    let content =
      div [] [
        items.View
        |> Doc.BindView(fun currentItems ->
          if List.isEmpty currentItems then
            Button.primary (
              text "Reset",
              onClick = (fun () -> Var.Set items [ "Draft"; "Published"; "Archived" ]),
              attrs = [ Button.Variant.outlined ]
            )
          else
            div
              []
              (currentItems
               |> List.map (fun item ->
                 Chip.create (
                   text item,
                   onClose = (fun () -> items.Value |> List.filter (fun i -> i <> item) |> Var.Set items),
                   closeIcon = Icon.create (Icon.UiActions UiActions.Close),
                   attrs = [
                     Chip.Variant.filled
                     Chip.Color.secondary
                     Margin.Right.extraSmall
                     Margin.Bottom.extraSmall
                   ]
                 ))))
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

Chip.create(
    text "Draft",
    onClose = (fun () -> printfn "Chip closed"),
    closeIcon = Icon.create(Icon.UiActions UiActions.Close), // see here
    attrs = [
        Chip.Variant.filled
        Chip.Color.secondary
    ]
)"""

    Helpers.codeSampleSection "Custom Close Icon" description content code

  let private clickableExample () =
    let description =
      Helpers.bodyText
        "Pass an onClick handler to make a chip interactive. The cursor changes to a pointer on hover."

    let count = Var.Create 0

    let content =
      div [] [
        div [ Typography.body2; Margin.Bottom.extraSmall ] [
          textView (count.View |> View.MapCached(sprintf "Clicked %d times"))
        ]

        Chip.create (
          text "Click me",
          onClick = (fun () -> Var.Set count (count.Value + 1)),
          attrs = [ Chip.Variant.filled; Chip.Color.primary ]
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let count = Var.Create 0

Chip.create(
    text "Click me",
    onClick = (fun () -> Var.Set count (count.Value + 1)), // see here
    attrs = [
        Chip.Variant.filled
        Chip.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Clickable" description content code

  let private iconExample () =
    let description =
      Helpers.bodyText "Pass content to display a leading element inside the chip, such as an icon or avatar."

    let content =
      Grid.create (
        [
          GridItem.create (
            Chip.create (
              text "Saved",
              content = Icon.create (Icon.UiActions UiActions.CheckCircle),
              attrs = [ Chip.Variant.filled; Chip.Color.success ]
            )
          )
          GridItem.create (
            Chip.create (
              text "Warning",
              content = Icon.create (Icon.Action Action.Warning),
              attrs = [ Chip.Variant.outlined; Chip.Color.warning ]
            )
          )
          GridItem.create (
            Chip.create (
              text "Error",
              content = Icon.create (Icon.Action Action.Error),
              attrs = [ Chip.Variant.filled; Chip.Color.error ]
            )
          )
          GridItem.create (
            Chip.create (
              text "Info",
              content = Icon.create (Icon.Action Action.Info),
              attrs = [ Chip.Variant.outlined; Chip.Color.info ]
            )
          )
        ]
      )

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

Chip.create(
    text "Saved",
    content = Icon.create(Icon.UiActions UiActions.CheckCircle), // see here
    attrs = [
        Chip.Variant.filled
        Chip.Color.success
    ]
)"""

    Helpers.codeSampleSection "Content" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls chip height and padding. Pass the density class in attrs to set it per-instance, or apply it to a parent container to affect all children."

    let content =
      let col (label: string) chipDensityAttr =
        let attrs = [ chipDensityAttr; Chip.Variant.filled; Chip.Color.primary ]

        GridItem.create (
          div [] [
            div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text label ]
            div [ Flex.Flex.allSizes; AlignItems.center ] [
              Chip.create (text "Basic", attrs = attrs @ [ Margin.Right.extraSmall ])
              Chip.create (
                text "With Icon",
                content = Icon.create (Icon.UiActions UiActions.CheckCircle),
                attrs = attrs @ [ Margin.Right.extraSmall ]
              )
              Chip.create (text "Closable", onClose = (fun () -> ()), attrs = attrs)
            ]
          ],
          attrs = [ GridItem.Span.twelve; GridItem.Span.Small.four ]
        )

      Grid.create (
        [
          col "Compact" Chip.Density.compact
          col "Standard" Chip.Density.standard
          col "Spacious" Chip.Density.spacious
        ]
      )

    let code =
      """open Weave

Chip.create(
    text "Compact",
    attrs = [
        Chip.Density.compact // see here
        Chip.Variant.filled
        Chip.Color.primary
    ]
)

Chip.create(
    text "Standard",
    attrs = [
        Chip.Density.standard
        Chip.Variant.filled
        Chip.Color.primary
    ]
)

Chip.create(
    text "Spacious",
    attrs = [
        Chip.Density.spacious
        Chip.Variant.filled
        Chip.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Density" description content code

  let private linkChipExample () =
    let description =
      Helpers.bodyText
        "Pass an href to render the chip as an anchor element. The chip navigates to the given URL on click."

    let content =
      Grid.create (
        [
          GridItem.create (
            Chip.create (
              text "WebSharper",
              href = "https://websharper.com",
              content = Icon.create (Icon.UiActions UiActions.OpenInNew),
              attrs = [ Chip.Variant.outlined; Chip.Color.primary ]
            )
          )
          GridItem.create (
            Chip.create (
              text "GitHub",
              href = "https://github.com/1eyewonder/Weave",
              content = Icon.create (Icon.UiActions UiActions.OpenInNew),
              attrs = [ Chip.Variant.filled; Chip.Color.info ]
            )
          )
        ]
      )

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

Chip.create(
    text "WebSharper",
    href = "https://websharper.com", // see here
    content = Icon.create(Icon.UiActions UiActions.OpenInNew),
    attrs = [
        Chip.Variant.outlined
        Chip.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Link Chip" description content code

  let private selectedExample () =
    let description =
      Helpers.bodyText
        "Pass a reactive View<bool> to the selected parameter to control the chip's selected state. Click the button to toggle."

    let isSelected = Var.Create false

    let content =
      div [] [
        isSelected.View |> View.MapCached(sprintf "Selected: %b") |> View.printfn

        Chip.create (
          text "Toggle me",
          onClick = (fun () -> Var.Set isSelected (not isSelected.Value)),
          selected = isSelected.View,
          attrs = [ Chip.Variant.filled; Chip.Color.primary ]
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let isSelected = Var.Create false

Chip.create(
    text "Toggle me",
    onClick = (fun () -> Var.Set isSelected (not isSelected.Value)),
    selected = isSelected.View, // see here
    attrs = [
        Chip.Variant.filled
        Chip.Color.primary
    ]
)"""

    Helpers.codeSampleSection "Selected" description content code

  let private borderRadiusExample () =
    let description =
      Helpers.bodyText
        "By default, chips use a pill shape (fully rounded). Override the border radius via attrs using the BorderRadius utility to achieve different shapes."

    let content =
      Grid.create (
        [
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Pill (default)" ]
              Chip.create (text "Default", attrs = [ Chip.Variant.filled; Chip.Color.primary ])
            ]
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Large" ]
              Chip.create (
                text "Large",
                attrs = [ Chip.Variant.filled; Chip.Color.secondary; BorderRadius.All.large ]
              )
            ]
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Medium" ]
              Chip.create (
                text "Medium",
                attrs = [ Chip.Variant.filled; Chip.Color.tertiary; BorderRadius.All.medium ]
              )
            ]
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Small" ]
              Chip.create (
                text "Small",
                attrs = [ Chip.Variant.filled; Chip.Color.success; BorderRadius.All.small ]
              )
            ]
          )
          GridItem.create (
            div [] [
              div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "None" ]
              Chip.create (
                text "None",
                attrs = [ Chip.Variant.filled; Chip.Color.error; BorderRadius.All.none ]
              )
            ]
          )
        ]
      )

    let code =
      """open Weave

// Default is pill shape (fully rounded)
Chip.create(text "Default", attrs = [ Chip.Variant.filled; Chip.Color.primary ])

// Override with BorderRadius utility classes
Chip.create(text "Large", attrs = [ Chip.Variant.filled; Chip.Color.secondary; BorderRadius.All.large ])
Chip.create(text "Medium", attrs = [ Chip.Variant.filled; Chip.Color.tertiary; BorderRadius.All.medium ])
Chip.create(text "Small", attrs = [ Chip.Variant.filled; Chip.Color.success; BorderRadius.All.small ])
Chip.create(text "None", attrs = [ Chip.Variant.filled; Chip.Color.error; BorderRadius.All.none ])"""

    Helpers.codeSampleSection "Border Radius" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Chip"
        Helpers.bodyText "Chips represent compact elements such as tags, filters, or choices."
        Helpers.divider ()
        filledChipsExample ()
        Helpers.divider ()
        textChipsExample ()
        Helpers.divider ()
        outlinedChipsExample ()
        Helpers.divider ()
        selectedExample ()
        Helpers.divider ()
        closableExample ()
        Helpers.divider ()
        customCloseIconExample ()
        Helpers.divider ()
        clickableExample ()
        Helpers.divider ()
        iconExample ()
        Helpers.divider ()
        linkChipExample ()
        Helpers.divider ()
        densityExample ()
        Helpers.divider ()
        borderRadiusExample ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
