namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Docs
open Weave.Icons
open Weave.Icons.MaterialSymbols

open WebSharper.JavaScript

[<JavaScript>]
module IconsExamples =

  let private basicIcons () =
    let content =
      div [
        Flex.Flex.allSizes
        FlexWrap.Wrap.allSizes
        JustifyContent.spaceEvenly
        AlignItems.center
        Gap.All.g4
      ] [
        span [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
          Icon.create (
            Icon.Action Action.Alarm,
            style = View.Const Style.Rounded,
            attrs = [ Typography.Color.primary; Attr.Create "aria-hidden" "true" ]
          )
          text "Reminder"
        ]
        span [ Flex.Inline.allSizes; AlignItems.center; Gap.All.g2 ] [
          Icon.create (
            Icon.UiActions UiActions.Favorite,
            style = View.Const Style.OutlinedFilled,
            attrs = [ Typography.Color.error; Attr.Create "aria-hidden" "true" ]
          )
          text "Favorite"
        ]
      ]

    Helpers.codeSampleSection
      "Basic Icons"
      (Helpers.bodyText
        "Choose an icon and its style. These icons accompany visible text and are decorative. Give an icon-only control an accessible name on the control.")
      content
      """open Weave.Icons
open Weave.Icons.MaterialSymbols

span [] [
  Icon.create (
    Icon.Action Action.Alarm,
    style = View.Const Style.Rounded,
    attrs = [ Typography.Color.primary; Attr.Create "aria-hidden" "true" ]
  )
  text "Reminder"
]

span [] [
  Icon.create (
    Icon.UiActions UiActions.Favorite,
    style = View.Const Style.OutlinedFilled,
    attrs = [ Typography.Color.error; Attr.Create "aria-hidden" "true" ]
  )
  text "Favorite"
]"""

  let private referenceGoogleFonts () =
    let description =
      Helpers.bodyText
        "To use Material Symbols icons in your project, include the appropriate Google Fonts link in your HTML head section. Choose the link that matches the icon style you want to use."

    let content =
      div [] [
        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Outlined Icons" ]
        div [
          Typography.body2
          Typography.Family.mono
          Margin.Bottom.extraSmall
          Attr.Style "overflow-wrap" "anywhere"
        ] [
          text
            "<link href=\"https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined\" rel=\"stylesheet\" />"
        ]

        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Rounded Icons" ]
        div [
          Typography.body2
          Typography.Family.mono
          Margin.Bottom.extraSmall
          Attr.Style "overflow-wrap" "anywhere"
        ] [
          text
            "<link href=\"https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded\" rel=\"stylesheet\" />"
        ]

        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Sharp Icons" ]
        div [
          Typography.body2
          Typography.Family.mono
          Margin.Bottom.extraSmall
          Attr.Style "overflow-wrap" "anywhere"
        ] [
          text
            "<link href=\"https://fonts.googleapis.com/css2?family=Material+Symbols+Sharp\" rel=\"stylesheet\" />"
        ]

        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "More Styles" ]

        let body =
          div [] [
            text "For more styles and options, visit "
            a [
              attr.href "https://developers.google.com/fonts/docs/material_symbols"
              Attr.Style "text-decoration" "underline"
              Typography.Color.primary
            ] [ text "Google's developer's guide" ]
          ]

        div [ Typography.body1; Margin.Bottom.extraSmall ] [ body ]
      ]

    let code =
      """<!-- Outlined -->
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined"
      rel="stylesheet" />

<!-- Rounded -->
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded"
      rel="stylesheet" />

<!-- Sharp -->
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Sharp"
      rel="stylesheet" />"""

    Helpers.codeSampleSectionFor "html" "Reference Google Fonts" description content code

  let private icons () =
    let filled = Var.Create false
    let style = Var.Create<MaterialSymbols.Style> Style.Outlined
    let opticalSize = Var.Create MaterialSymbols.OpticalSize.``24``
    let weight = Var.Create MaterialSymbols.IconWeight.``400``
    let grade = Var.Create MaterialSymbols.IconGrade.``0``
    let color = Var.Create<BrandColor option> None

    let filledOptions = [ false, "Unfilled"; true, "Filled" ]

    let filledRadios =
      filledOptions
      |> List.map (fun (value, label) ->
        Radio.create (
          filled,
          value,
          displayText = View.Const label,
          inputAttrs = [ attr.name "icon-playground-fill" ],
          attrs = [ Radio.Color.primary; Margin.All.none ]
        ))

    let styleOptions = [ Style.Outlined; Style.Rounded; Style.Sharp ]

    let styleRadios =
      styleOptions
      |> List.map (fun value ->
        Radio.create (
          style,
          value,
          displayText = View.Const(sprintf "%A" value),
          inputAttrs = [ attr.name "icon-playground-style" ],

          attrs = [ Radio.Color.primary; Margin.All.none ]
        ))

    let opticalSizeOptions = [
      MaterialSymbols.OpticalSize.``20``
      MaterialSymbols.OpticalSize.``24``
      MaterialSymbols.OpticalSize.``40``
      MaterialSymbols.OpticalSize.``48``
    ]

    let opticalSizeRadios =
      opticalSizeOptions
      |> List.map (fun size ->
        Radio.create (
          opticalSize,
          size,
          displayText = View.Const(sprintf "%A" size),
          inputAttrs = [ attr.name "icon-playground-optical-size" ],

          attrs = [ Radio.Color.primary; Margin.All.none ]
        ))

    let weightOptions = [
      MaterialSymbols.IconWeight.``100``
      MaterialSymbols.IconWeight.``200``
      MaterialSymbols.IconWeight.``300``
      MaterialSymbols.IconWeight.``400``
      MaterialSymbols.IconWeight.``500``
      MaterialSymbols.IconWeight.``600``
      MaterialSymbols.IconWeight.``700``
    ]

    let weightRadios =
      weightOptions
      |> List.map (fun w ->
        Radio.create (
          weight,
          w,
          displayText = View.Const(sprintf "%A" w),
          inputAttrs = [ attr.name "icon-playground-weight" ],

          attrs = [ Radio.Color.primary; Margin.All.none ]
        ))

    let gradeOptions = [
      MaterialSymbols.IconGrade.``-25``
      MaterialSymbols.IconGrade.``0``
      MaterialSymbols.IconGrade.``200``
    ]

    let gradeRadios =
      gradeOptions
      |> List.map (fun g ->
        Radio.create (
          grade,
          g,
          displayText = View.Const(sprintf "%A" g),
          inputAttrs = [ attr.name "icon-playground-grade" ],

          attrs = [ Radio.Color.primary; Margin.All.none ]
        ))

    let colorOptions = [
      None, "Default"
      Some BrandColor.Primary, "Primary"
      Some BrandColor.Secondary, "Secondary"
      Some BrandColor.Tertiary, "Tertiary"
      Some BrandColor.Info, "Info"
      Some BrandColor.Success, "Success"
      Some BrandColor.Warning, "Warning"
      Some BrandColor.Error, "Error"
    ]

    let colorRadios =
      colorOptions
      |> List.map (fun (value, label) ->
        Radio.create (
          color,
          value,
          displayText = View.Const label,
          inputAttrs = [ attr.name "icon-playground-color" ],
          attrs = [ Radio.Color.primary; Margin.All.none ]
        ))

    let createItem icon =
      GridItem.create (
        div [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          JustifyContent.center
          AlignItems.center
          Margin.Bottom.extraSmall
          Attr.Style "min-width" "0"
        ] [
          let style =
            (style.View, filled.View)
            ||> View.map2Cached (fun s f ->
              match s, f with
              | Style.Outlined, true -> Style.OutlinedFilled
              | Style.Outlined, false -> Style.Outlined
              | Style.Rounded, true -> Style.RoundedFilled
              | Style.Rounded, false -> Style.Rounded
              | Style.Sharp, true -> Style.SharpFilled
              | Style.Sharp, false -> Style.Sharp
              | _ -> s)

          style
          |> Doc.BindView(fun style ->
            Icon.create (
              icon,
              style = View.Const style,
              weight = weight.View,
              grade = grade.View,
              opticalSize = opticalSize.View,
              attrs = [
                Attr.Style "font-size" "3em"
                Attr.Create "aria-hidden" "true"

                Map.ofList [
                  None, ""
                  Some BrandColor.Primary, "weave-typography--primary"
                  Some BrandColor.Secondary, "weave-typography--secondary"
                  Some BrandColor.Tertiary, "weave-typography--tertiary"
                  Some BrandColor.Info, "weave-typography--info"
                  Some BrandColor.Success, "weave-typography--success"
                  Some BrandColor.Warning, "weave-typography--warning"
                  Some BrandColor.Error, "weave-typography--error"
                ]
                |> Attr.classSelection color.View
              ]
            ))

          let iconText =
            match icon with
            | Icon.Action a -> sprintf "%A" a
            | Icon.Activities a -> sprintf "%A" a
            | Icon.Android a -> sprintf "%A" a
            | Icon.AudioVideo a -> sprintf "%A" a
            | Icon.Business b -> sprintf "%A" b
            | Icon.Communicate c -> sprintf "%A" c
            | Icon.Hardware h -> sprintf "%A" h
            | Icon.Home h -> sprintf "%A" h
            | Icon.Household h -> sprintf "%A" h
            | Icon.Images i -> sprintf "%A" i
            | Icon.Maps m -> sprintf "%A" m
            | Icon.Privacy p -> sprintf "%A" p
            | Icon.Social s -> sprintf "%A" s
            | Icon.Text t -> sprintf "%A" t
            | Icon.Transit tr -> sprintf "%A" tr
            | Icon.Travel tv -> sprintf "%A" tv
            | Icon.UiActions ua -> sprintf "%A" ua

          div [
            Typography.body2
            Attr.Style "text-align" "center"
            Attr.Style "max-width" "100%"
            Attr.Style "overflow-wrap" "anywhere"
          ] [ text iconText ]
        ],
        attrs = [
          GridItem.Span.six
          GridItem.Span.Small.four
          GridItem.Span.Medium.two
          GridItem.Span.ExtraLarge.one
        ]
      )

    let examples =
      div [] [
        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Sample Icons" ]

        Grid.create (
          [
            yield!
              [
                Action.``3dRotation``
                Action.Accessibility
                Action.AccessibilityNew
                Action.Accessible
                Action.AccessibleForward
                Action.AccountBox
                Action.AccountChild
                Action.AccountChildInvert
                Action.AccountCircle
                Action.AccountCircleOff
                Action.Ad
                Action.AdGroup
                Action.AdGroupOff
                Action.AdOff
                Action.AddAd
                Action.AddAlert
                Action.AdsClick
                Action.Alarm
                Action.AlarmAdd
                Action.AlarmOff
                Action.AlarmOn
                Action.AlarmPause
                Action.AlarmSmartWake
                Action.AllInclusive
                Action.AllOut
                Action.Anchor
                Action.Api
                Action.Approval
                Action.ApprovalDelegation
                Action.ApprovalDelegationOff
                Action.ArrowSelectorTool
                Action.AutoDelete
                Action.AwardStar
                Action.BackgroundReplace
                Action.Backup
                Action.BackupTable
                Action.BatchPrediction
                Action.BookRibbon
                Action.Bookmark
                Action.BookmarkAdd
                Action.BookmarkAdded
                Action.BookmarkBag
                Action.BookmarkCheck
                Action.BookmarkFlag
                Action.BookmarkHeart
                Action.BookmarkManager
                Action.BookmarkRemove
                Action.BookmarkStacks
                Action.BookmarkStar
                Action.Bookmarks
                Action.Browse
                Action.BugReport
                Action.Build
                Action.BuildCircle
                Action.CalendarCheck
                Action.CalendarClock
                Action.CalendarLock
                Action.CalendarMonth
                Action.CalendarToday
                Action.Category
                Action.Celebration
                Action.ChangeHistory
                Action.ChromeReaderMode
                Action.CircleNotifications
                Action.Circles
                Action.CirclesExt
                Action.Code
                Action.CodeBlocks
                Action.CodeOff
                Action.CollectionsBookmark
                Action.Commit
                Action.ComponentExchange
                Action.ContactsProduct
                Action.Dangerous
                Action.DataLossPrevention
                Action.DateRange
                Action.DeleteHistory
                Action.DeveloperGuide
                Action.DomainVerification
                Action.DomainVerificationOff
                Action.DraftOrders
                Action.DynamicFeed
                Action.EditCalendar
                Action.EditNotifications
                Action.EditSquare
                Action.Error
                Action.Event
                Action.EventAvailable
                Action.EventBusy
                Action.EventNote
                Action.EventRepeat
                Action.EventUpcoming
                Action.Extension
                Action.FeatureSearch
                Action.Feedback
                Action.FindReplace
                Action.Fingerprint
                Action.FingerprintOff
                Action.Flutter
                Action.FlutterDash
                Action.FreeCancellation
                Action.Gesture
                Action.GestureSelect
                Action.HandGesture
                Action.HandGestureOff
                Action.Help
                Action.HelpCenter
                Action.HelpClinic
                Action.History
                Action.History2
                Action.HistoryOff
                Action.HistoryToggleOff
                Action.HomeAppLogo
                Action.HotelClass
                Action.Hourglass
                Action.HourglassCheck
                Action.HourglassDisabled
                Action.HourglassEmpty
                Action.HourglassPause
                Action.HowToReg
                Action.Http
                Action.IndeterminateQuestionBox
                Action.Info
                Action.InfoI
                Action.Input
                Action.Interests
              ]
              |> List.map (Icon.Action >> createItem)
          ]
        )
      ]

    let group label radios =
      fieldset [
        Attr.Style "border" "0"
        Attr.Style "padding" "0"
        Attr.Style "margin" "0"
        Attr.Style "min-inline-size" "0"
      ] [
        legend [ Typography.h6 ] [ text label ]
        div
          [
            Flex.Flex.allSizes
            FlexWrap.Wrap.allSizes
            JustifyContent.flexStart
            Gap.All.g2
          ]
          radios
      ]

    let content =
      div [] [
        group "Select Fill" filledRadios
        group "Select Style" styleRadios
        group "Select Optical Size" opticalSizeRadios
        group "Select Weight" weightRadios
        group "Select Grade" gradeRadios
        group "Select Color" colorRadios
      ]

    let styleAndFill = (style.View, filled.View) ||> View.Map2(fun s f -> s, f)
    let weightAndGrade = (weight.View, grade.View) ||> View.Map2(fun w g -> w, g)

    let sizeAndColor =
      (opticalSize.View, color.View) ||> View.Map2(fun size c -> size, c)

    let first = (styleAndFill, weightAndGrade) ||> View.Map2(fun a b -> a, b)

    let source =
      (first, sizeAndColor)
      ||> View.Map2(fun ((style, filled), (weight, grade)) (size, color) ->
        let colorName =
          match color with
          | None -> "Default"
          | Some value -> sprintf "%A" value

        IconPlaygroundSource.source {
          Style = sprintf "%A" style
          Filled = filled
          Weight = sprintf "%A" weight
          Grade = sprintf "%A" grade
          OpticalSize = sprintf "%A" size
          Color = colorName
        })

    let expanded = Var.Create false

    let header =
      ExpansionPanelHeader.create (
        content =
          div [ Typography.subtitle2 ] [
            textView (
              expanded.View
              |> View.Map(fun value -> if value then "Hide Code" else "Show Code")
            )
          ],
        expanded = expanded,
        icon =
          (expanded.View
           |> Doc.BindView(fun value ->
             Icon.create (
               Icon.UiActions(if value then UiActions.CollapseAll else UiActions.ExpandAll),
               attrs = [ Attr.Create "aria-hidden" "true" ]
             ))),
        attrs = [ ExpansionPanel.Color.primary ]
      )

    let code =
      source
      |> Doc.BindView(fun current ->
        div [ Attr.Class "docs-copy-code-wrapper" ] [
          span [ Attr.Class "docs-code-language" ] [ text "F#" ]
          Helpers.copyButton current
          pre [ attr.tabindex "0"; Attr.Create "aria-label" "Selected icon F# source code" ] [
            code [
              Attr.Class "language-fsharp"
              on.afterRender Helpers.highlightCodeElement
              Typography.Family.mono
            ] [ text current ]
          ]
        ])

    let panel =
      div [
        Attr.Class "docs-example"
        SurfaceColor.BackgroundColor.surface
        BorderRadius.All.small
      ] [
        div [ Attr.Class "docs-example__preview" ] [ content ]
        ExpansionPanelContainer.create (
          [
            ExpansionPanel.create (
              header = header,
              content = ExpansionPanelContent.create code,
              expanded = expanded
            )
          ],
          attrs = [ Attr.Class "docs-example__code" ]
        )
      ]

    Helpers.sectionPlain
      "Icon Playground"
      (Helpers.bodyText
        "Explore style, weight, size, grade, and color. The source shows the Alarm icon with the current settings; the grid applies them to all samples.")
      (div [] [ panel; examples ])

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Icons"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text "Render Material Symbols with typed F# icon names organized by category."
        ]

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Typed icon names do not load their fonts. Include the matching Google Fonts stylesheet in the HTML head so the chosen style can render."
        ]

        Helpers.divider ()
        referenceGoogleFonts ()
        Helpers.divider ()
        basicIcons ()
        Helpers.divider ()
        icons ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
