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
      BrandColor.Primary
      BrandColor.Secondary
      BrandColor.Tertiary
      BrandColor.Error
      BrandColor.Warning
      BrandColor.Success
      BrandColor.Info
    ]

    let content =
      div [] [
        Subtitle2.Div("Enabled", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Grid.Create(
          colors
          |> List.map (fun color ->
            let label = sprintf "%A" color

            GridItem.Create(
              Chip.Create(
                text label,
                attrs = [ cls [ Chip.Variant.toClass Chip.Variant.Filled; Chip.Color.toClass color ] ]
              )
            )),
          spacing = Grid.GutterSpacing.create 2
        )
        Subtitle2.Div("Disabled", attrs = [ Margin.toClasses Margin.Vertical.extraSmall |> cls ])
        Grid.Create(
          colors
          |> List.map (fun color ->
            let label = sprintf "%A" color

            GridItem.Create(
              Chip.Create(
                text label,
                enabled = View.Const false,
                attrs = [ cls [ Chip.Variant.toClass Chip.Variant.Filled; Chip.Color.toClass color ] ]
              )
            )),
          spacing = Grid.GutterSpacing.create 2
        )
      ]

    let code =
      """open Weave

let colors = [
    BrandColor.Primary; BrandColor.Secondary; BrandColor.Tertiary
    BrandColor.Error; BrandColor.Warning; BrandColor.Success; BrandColor.Info
]

colors
|> List.map (fun color ->
    Chip.Create(
        text (sprintf "%A" color),
        attrs = [
            cls [
                Chip.Variant.toClass Chip.Variant.Filled // see here
                Chip.Color.toClass color
            ]
        ]
    )
)"""

    Helpers.codeSampleSection "Filled Chips" description content code

  let private textChipsExample () =
    let description =
      Helpers.bodyText "Text chips have a transparent background and suit low-emphasis contexts."

    let colors = [
      BrandColor.Primary
      BrandColor.Secondary
      BrandColor.Tertiary
      BrandColor.Error
      BrandColor.Warning
      BrandColor.Success
      BrandColor.Info
    ]

    let content =
      div [] [
        Subtitle2.Div("Enabled", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Grid.Create(
          colors
          |> List.map (fun color ->
            let label = sprintf "%A" color

            GridItem.Create(
              Chip.Create(
                text label,
                attrs = [ cls [ Chip.Variant.toClass Chip.Variant.Text; Chip.Color.toClass color ] ]
              )
            )),
          spacing = Grid.GutterSpacing.create 2
        )
        Subtitle2.Div("Disabled", attrs = [ Margin.toClasses Margin.Vertical.extraSmall |> cls ])
        Grid.Create(
          colors
          |> List.map (fun color ->
            let label = sprintf "%A" color

            GridItem.Create(
              Chip.Create(
                text label,
                enabled = View.Const false,
                attrs = [ cls [ Chip.Variant.toClass Chip.Variant.Text; Chip.Color.toClass color ] ]
              )
            )),
          spacing = Grid.GutterSpacing.create 2
        )
      ]

    let code =
      """open Weave

Chip.Create(
    text "Primary",
    attrs = [
        cls [
            Chip.Variant.toClass Chip.Variant.Text // see here
            Chip.Color.toClass BrandColor.Primary
        ]
    ]
)"""

    Helpers.codeSampleSection "Text Chips" description content code

  let private outlinedChipsExample () =
    let description =
      Helpers.bodyText "Outlined chips use a border and transparent background for a lightweight look."

    let colors = [
      BrandColor.Primary
      BrandColor.Secondary
      BrandColor.Tertiary
      BrandColor.Error
      BrandColor.Warning
      BrandColor.Success
      BrandColor.Info
    ]

    let content =
      div [] [
        Subtitle2.Div("Enabled", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Grid.Create(
          colors
          |> List.map (fun color ->
            let label = sprintf "%A" color

            GridItem.Create(
              Chip.Create(
                text label,
                attrs = [ cls [ Chip.Variant.toClass Chip.Variant.Outlined; Chip.Color.toClass color ] ]
              )
            )),
          spacing = Grid.GutterSpacing.create 2
        )
        Subtitle2.Div("Disabled", attrs = [ Margin.toClasses Margin.Vertical.extraSmall |> cls ])
        Grid.Create(
          colors
          |> List.map (fun color ->
            let label = sprintf "%A" color

            GridItem.Create(
              Chip.Create(
                text label,
                enabled = View.Const false,
                attrs = [ cls [ Chip.Variant.toClass Chip.Variant.Outlined; Chip.Color.toClass color ] ]
              )
            )),
          spacing = Grid.GutterSpacing.create 2
        )
      ]

    let code =
      """open Weave

Chip.Create(
    text "Primary",
    attrs = [
        cls [
            Chip.Variant.toClass Chip.Variant.Outlined // see here
            Chip.Color.toClass BrandColor.Primary
        ]
    ]
)"""

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
                 Chip.Create(
                   text item,
                   onClose = (fun () -> items.Value |> List.filter (fun i -> i <> item) |> Var.Set items),
                   attrs = [
                     cls [
                       Chip.Variant.toClass Chip.Variant.Outlined
                       Chip.Color.toClass BrandColor.Primary
                     ]
                     Margin.toClasses Margin.Right.extraSmall |> cls
                     Margin.toClasses Margin.Bottom.extraSmall |> cls
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
        Chip.Create(
            text item,
            onClose = (fun () ->
                items.Value
                |> List.filter (fun i -> i <> item)
                |> Var.Set items), // see here
            attrs = [
                cls [
                    Chip.Variant.toClass Chip.Variant.Outlined
                    Chip.Color.toClass BrandColor.Primary
                ]
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
                 Chip.Create(
                   text item,
                   onClose = (fun () -> items.Value |> List.filter (fun i -> i <> item) |> Var.Set items),
                   closeIcon = Icon.Create(Icon.UiActions UiActions.Close),
                   attrs = [
                     cls [
                       Chip.Variant.toClass Chip.Variant.Filled
                       Chip.Color.toClass BrandColor.Secondary
                     ]
                     Margin.toClasses Margin.Right.extraSmall |> cls
                     Margin.toClasses Margin.Bottom.extraSmall |> cls
                   ]
                 ))))
      ]

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

Chip.Create(
    text "Draft",
    onClose = (fun () -> (* remove chip *)),
    closeIcon = Icon.Create(Icon.UiActions UiActions.Close), // see here
    attrs = [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Secondary
        ]
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
        Body2.Div(
          count.View |> View.MapCached(sprintf "Clicked %d times"),
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Chip.Create(
          text "Click me",
          onClick = (fun () -> Var.Set count (count.Value + 1)),
          attrs = [
            cls [
              Chip.Variant.toClass Chip.Variant.Filled
              Chip.Color.toClass BrandColor.Primary
            ]
          ]
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let count = Var.Create 0

Chip.Create(
    text "Click me",
    onClick = (fun () -> Var.Set count (count.Value + 1)), // see here
    attrs = [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Primary
        ]
    ]
)"""

    Helpers.codeSampleSection "Clickable" description content code

  let private iconExample () =
    let description =
      Helpers.bodyText "Pass content to display a leading element inside the chip, such as an icon or avatar."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Chip.Create(
              text "Saved",
              content = Icon.Create(Icon.UiActions UiActions.CheckCircle),
              attrs = [
                cls [
                  Chip.Variant.toClass Chip.Variant.Filled
                  Chip.Color.toClass BrandColor.Success
                ]
              ]
            )
          )
          GridItem.Create(
            Chip.Create(
              text "Warning",
              content = Icon.Create(Icon.Action Action.Warning),
              attrs = [
                cls [
                  Chip.Variant.toClass Chip.Variant.Outlined
                  Chip.Color.toClass BrandColor.Warning
                ]
              ]
            )
          )
          GridItem.Create(
            Chip.Create(
              text "Error",
              content = Icon.Create(Icon.Action Action.Error),
              attrs = [
                cls [
                  Chip.Variant.toClass Chip.Variant.Filled
                  Chip.Color.toClass BrandColor.Error
                ]
              ]
            )
          )
          GridItem.Create(
            Chip.Create(
              text "Info",
              content = Icon.Create(Icon.Action Action.Info),
              attrs = [
                cls [
                  Chip.Variant.toClass Chip.Variant.Outlined
                  Chip.Color.toClass BrandColor.Info
                ]
              ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

Chip.Create(
    text "Saved",
    content = Icon.Create(Icon.UiActions UiActions.CheckCircle), // see here
    attrs = [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Success
        ]
    ]
)"""

    Helpers.codeSampleSection "Content" description content code

  let private densityExample () =
    let description =
      Helpers.bodyText
        "Density controls chip height and padding. Pass the density class in attrs to set it per-instance, or apply it to a parent container to affect all children."

    let content =
      let densityAttrs density = [
        Chip.Density.toClass density
        Chip.Variant.toClass Chip.Variant.Filled
        Chip.Color.toClass BrandColor.Primary
      ]

      let col density =
        let label = sprintf "%A" density

        GridItem.Create(
          div [] [
            Subtitle2.Div(label, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            div [ cls [ Flex.toClass None Flex.Flex; AlignItems.toClass AlignItems.Center ] ] [
              Chip.Create(
                text "Basic",
                attrs = [ cls (densityAttrs density); Margin.toClasses Margin.Right.extraSmall |> cls ]
              )
              Chip.Create(
                text "With Icon",
                content = Icon.Create(Icon.UiActions UiActions.CheckCircle),
                attrs = [ cls (densityAttrs density); Margin.toClasses Margin.Right.extraSmall |> cls ]
              )
              Chip.Create(text "Closable", onClose = (fun () -> ()), attrs = [ cls (densityAttrs density) ])
            ]
          ],
          xs = Grid.Width.create 12,
          sm = Grid.Width.create 4
        )

      Grid.Create(
        [ col Density.Compact; col Density.Standard; col Density.Spacious ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave

Chip.Create(
    text "Compact",
    attrs = [
        cls [
            Chip.Density.toClass Density.Compact // see here
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Primary
        ]
    ]
)"""

    Helpers.codeSampleSection "Density" description content code

  let private linkChipExample () =
    let description =
      Helpers.bodyText
        "Pass an href to render the chip as an anchor element. The chip navigates to the given URL on click."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            Chip.Create(
              text "WebSharper",
              href = "https://websharper.com",
              content = Icon.Create(Icon.UiActions UiActions.OpenInNew),
              attrs = [
                cls [
                  Chip.Variant.toClass Chip.Variant.Outlined
                  Chip.Color.toClass BrandColor.Primary
                ]
              ]
            )
          )
          GridItem.Create(
            Chip.Create(
              text "GitHub",
              href = "https://github.com/1eyewonder/Weave",
              content = Icon.Create(Icon.UiActions UiActions.OpenInNew),
              attrs = [
                cls [ Chip.Variant.toClass Chip.Variant.Filled; Chip.Color.toClass BrandColor.Info ]
              ]
            )
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.Icons.MaterialSymbols

Chip.Create(
    text "WebSharper",
    href = "https://websharper.com", // see here
    content = Icon.Create(Icon.UiActions UiActions.OpenInNew),
    attrs = [
        cls [
            Chip.Variant.toClass Chip.Variant.Outlined
            Chip.Color.toClass BrandColor.Primary
        ]
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

        Chip.Create(
          text "Toggle me",
          onClick = (fun () -> Var.Set isSelected (not isSelected.Value)),
          selected = isSelected.View,
          attrs = [
            cls [
              Chip.Variant.toClass Chip.Variant.Filled
              Chip.Color.toClass BrandColor.Primary
            ]
          ]
        )
      ]

    let code =
      """open Weave
open WebSharper.UI

let isSelected = Var.Create false

Chip.Create(
    text "Toggle me",
    onClick = (fun () -> Var.Set isSelected (not isSelected.Value)),
    selected = isSelected.View, // see here
    attrs = [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Primary
        ]
    ]
)"""

    Helpers.codeSampleSection "Selected" description content code

  let private borderRadiusExample () =
    let description =
      Helpers.bodyText
        "By default, chips use a pill shape (fully rounded). Override the border radius via attrs using the BorderRadius utility to achieve different shapes."

    let content =
      Grid.Create(
        [
          GridItem.Create(
            div [] [
              Subtitle2.Div("Pill (default)", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              Chip.Create(
                text "Default",
                attrs = [
                  cls [
                    Chip.Variant.toClass Chip.Variant.Filled
                    Chip.Color.toClass BrandColor.Primary
                  ]
                ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Subtitle2.Div("Large", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              Chip.Create(
                text "Large",
                attrs = [
                  cls [
                    Chip.Variant.toClass Chip.Variant.Filled
                    Chip.Color.toClass BrandColor.Secondary
                    BorderRadius.toClass BorderRadius.All.large
                  ]
                ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Subtitle2.Div("Medium", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              Chip.Create(
                text "Medium",
                attrs = [
                  cls [
                    Chip.Variant.toClass Chip.Variant.Filled
                    Chip.Color.toClass BrandColor.Tertiary
                    BorderRadius.toClass BorderRadius.All.medium
                  ]
                ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Subtitle2.Div("Small", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              Chip.Create(
                text "Small",
                attrs = [
                  cls [
                    Chip.Variant.toClass Chip.Variant.Filled
                    Chip.Color.toClass BrandColor.Success
                    BorderRadius.toClass BorderRadius.All.small
                  ]
                ]
              )
            ]
          )
          GridItem.Create(
            div [] [
              Subtitle2.Div("None", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
              Chip.Create(
                text "None",
                attrs = [
                  cls [
                    Chip.Variant.toClass Chip.Variant.Filled
                    Chip.Color.toClass BrandColor.Error
                    BorderRadius.toClass BorderRadius.All.none
                  ]
                ]
              )
            ]
          )
        ],
        spacing = Grid.GutterSpacing.create 2
      )

    let code =
      """open Weave
open Weave.CssHelpers

// Override the default pill shape with BorderRadius utility classes
Chip.Create(
    text "Medium",
    attrs = [
        cls [
            Chip.Variant.toClass Chip.Variant.Filled
            Chip.Color.toClass BrandColor.Tertiary
            BorderRadius.toClass BorderRadius.All.medium // see here
        ]
    ]
)

// Other options: BorderRadius.All.large, .small, .none"""

    Helpers.codeSampleSection "Border Radius" description content code

  let render () =
    Container.Create(
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
      maxWidth = Container.MaxWidth.Large
    )
