namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

open WebSharper.JavaScript

[<JavaScript>]
module IconsExamples =

  let private referenceGoogleFonts () =
    let description =
      Helpers.bodyText
        "To use Material Symbols icons in your project, include the appropriate Google Fonts link in your HTML head section. Choose the link that matches the icon style you want to use."

    let content =
      div [] [
        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Outlined Icons" ]
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "<link href=\"https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined\" rel=\"stylesheet\" />"
        ]

        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Rounded Icons" ]
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "<link href=\"https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded\" rel=\"stylesheet\" />"
        ]

        div [ Typography.h6; Margin.Bottom.extraSmall ] [ text "Sharp Icons" ]
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
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

    Helpers.codeSampleSection "Reference Google Fonts" description content code

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
          attrs = [ Radio.Color.primary; Margin.Vertical.extraSmall ]
        ))

    let styleOptions = [ Style.Outlined; Style.Rounded; Style.Sharp ]

    let styleRadios =
      styleOptions
      |> List.map (fun value ->
        Radio.create (
          style,
          value,
          displayText = View.Const(sprintf "%A" value),
          attrs = [ Radio.Color.primary; Margin.Vertical.extraSmall ]
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
          attrs = [ Radio.Color.primary; Margin.Vertical.extraSmall ]
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
          attrs = [ Radio.Color.primary; Margin.Vertical.extraSmall ]
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
          attrs = [ Radio.Color.primary; Margin.Vertical.extraSmall ]
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
          attrs = [ Radio.Color.primary; Margin.Vertical.extraSmall ]
        ))

    let createItem icon =
      GridItem.create (
        div [
          Flex.Flex.allSizes
          FlexDirection.Column.allSizes
          JustifyContent.center
          AlignItems.center
          Margin.Bottom.extraSmall
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

          div [ Typography.body2; Attr.Style "text-align" "center" ] [ text iconText ]
        ],
        xs = Grid.Width.create 6,
        sm = Grid.Width.create 4,
        md = Grid.Width.create 2,
        xl = Grid.Width.create 1
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

    let content =
      div [] [
        div [ Typography.h6 ] [ text "Select Fill" ]

        div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; JustifyContent.flexStart ] [ yield! filledRadios ]

        div [ Typography.h6 ] [ text "Select Style" ]

        div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; JustifyContent.flexStart ] [ yield! styleRadios ]

        div [ Typography.h6 ] [ text "Select Optical Size" ]

        div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; JustifyContent.flexStart ] [
          yield! opticalSizeRadios
        ]

        div [ Typography.h6 ] [ text "Select Weight" ]

        div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; JustifyContent.flexStart ] [ yield! weightRadios ]

        div [ Typography.h6 ] [ text "Select Grade" ]

        div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; JustifyContent.flexStart ] [ yield! gradeRadios ]

        div [ Typography.h6 ] [ text "Select Color" ]

        div [ Flex.Flex.allSizes; FlexWrap.Wrap.allSizes; JustifyContent.flexStart ] [ yield! colorRadios ]

        examples
      ]

    let code =
      """open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

open WebSharper.UI

Icon.create(
    Icon.Action Action.Alarm,
    style = View.Const Style.Rounded,
    weight = View.Const MaterialSymbols.IconWeight.``400``,
    opticalSize = View.Const MaterialSymbols.OpticalSize.``24``,
    grade = View.Const MaterialSymbols.IconGrade.``0``,
    attrs = [
        Typography.Color.primary
    ]
)

Icon.create(
    Icon.UiActions UiActions.Favorite,
    style = View.Const Style.OutlinedFilled,
    weight = View.Const MaterialSymbols.IconWeight.``700``,
    attrs = [
        Typography.Color.error
    ]
)"""

    Helpers.codeSampleSection "Icons" Doc.Empty content code

  // Grid.create(
  //   [
  //     div [ Typography.h6 ] [ text "Select Fill" ]
  //     GridItem.create(
  //       Grid.create(filledRadios, justify = JustifyContent.FlexStart),
  //       xs = Grid.Width.create 12,
  //       attrs = [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ]
  //     )

  //     div [ Typography.h6 ] [ text "Select Style" ]
  //     GridItem.create(
  //       Grid.create(styleRadios, justify = JustifyContent.FlexStart),
  //       xs = Grid.Width.create 12,
  //       attrs = [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ]
  //     )

  //     div [ Typography.h6 ] [ text "Select Optical Size" ]
  //     GridItem.create(
  //       Grid.create(opticalSizeRadios, justify = JustifyContent.FlexStart),
  //       xs = Grid.Width.create 12,
  //       attrs = [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ]
  //     )

  //     div [ Typography.h6 ] [ text "Select Weight" ]
  //     GridItem.create(
  //       Grid.create(weightRadios, justify = JustifyContent.FlexStart),
  //       xs = Grid.Width.create 12,
  //       attrs = [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ]
  //     )

  //     div [ Typography.h6 ] [ text "Select Grade" ]
  //     GridItem.create(
  //       Grid.create(gradeRadios, justify = JustifyContent.FlexStart),
  //       xs = Grid.Width.create 12,
  //       attrs = [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ]
  //     )

  //     div [ Typography.h6 ] [ text "Select Color" ]
  //     GridItem.create(
  //       Grid.create(colorRadios, justify = JustifyContent.FlexStart),
  //       xs = Grid.Width.create 12,
  //       attrs = [ cls [ yield! Margin.toClasses Margin.Bottom.extraSmall ] ]
  //     )

  //     GridItem.create(examples, xs = Grid.Width.create 12)
  //   ],
  //   justify = JustifyContent.FlexStart
  // )
  //|> Helpers.section "Icons" Doc.Empty

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Icons"
        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Weave has built in helpers for Material Symbols icons. The icons are structured within a DU which is organized into child DUs which align with the categories defined by Google."
        ]

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "Note, that the Icon DU does not guarantee static type safety when selecting icons since the end user is reponsible for including the correct Google Fonts link in their HTML head section. Make sure to include the appropriate link for the style of icons you wish to use."
        ]

        Helpers.divider ()
        referenceGoogleFonts ()
        Helpers.divider ()
        icons ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
