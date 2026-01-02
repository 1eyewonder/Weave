namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols
open Weave.CssHelpers
open WebSharper.JavaScript

[<JavaScript>]
module IconsExamples =

  let referenceGoogleFonts () =
    let description =
      Helpers.bodyText
        "To use Material Symbols icons in your project, you need to include the appropriate Google Fonts link in your HTML head section. Depending on the style of icons you want to use, you can choose from the following options:"

    div [] [
      H6.Create("Outlined Icons", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      Body1.Create(
        "<link href=\"https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined\" rel=\"stylesheet\" />",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )

      H6.Create("Rounded Icons", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      Body1.Create(
        "<link href=\"https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded\" rel=\"stylesheet\" />",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )

      H6.Create("Sharp Icons", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
      Body1.Create(
        "<link href=\"https://fonts.googleapis.com/css2?family=Material+Symbols+Sharp\" rel=\"stylesheet\" />",
        attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
      )

      H6.Create("More Styles", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])

      let body =
        div [] [
          text "For more styles and options, visit "
          a [
            attr.href "https://developers.google.com/fonts/docs/material_symbols"
            Attr.Style "text-decoration" "underline"
            Typography.Color.toClass BrandColor.Primary |> cl
          ] [ text "Google's developer's guide" ]
        ]

      Body1.Create(body, attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
    ]
    |> Helpers.section "Reference Google Fonts" description

  let icons () =
    let filled = Var.Create false
    let style = Var.Create<MaterialSymbols.Style> Style.Outlined
    let opticalSize = Var.Create MaterialSymbols.OpticalSize.``24``
    let weight = Var.Create MaterialSymbols.IconWeight.``400``
    let grade = Var.Create MaterialSymbols.IconGrade.``0``

    let filledOptions = [ false, "Unfilled"; true, "Filled" ]

    let filledRadios =
      filledOptions
      |> List.map (fun (value, label) ->
        GridItem.Create(
          Radio.Create(
            filled,
            value,
            displayText = View.Const label,
            attrs = [ Radio.Color.toClass BrandColor.Primary |> cl ]
          ),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 3
        ))

    let styleOptions = [ Style.Outlined; Style.Rounded; Style.Sharp ]

    let styleRadios =
      styleOptions
      |> List.map (fun value ->
        GridItem.Create(
          Radio.Create(
            style,
            value,
            displayText = View.Const(sprintf "%A" value),
            attrs = [ Radio.Color.toClass BrandColor.Primary |> cl ]
          ),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 3
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
        GridItem.Create(
          Radio.Create(
            opticalSize,
            size,
            displayText = View.Const(sprintf "%A" size),
            attrs = [ Radio.Color.toClass BrandColor.Primary |> cl ]
          ),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 3
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
        GridItem.Create(
          Radio.Create(
            weight,
            w,
            displayText = View.Const(sprintf "%A" w),
            attrs = [ Radio.Color.toClass BrandColor.Primary |> cl ]
          ),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 1
        ))

    let gradeOptions = [
      MaterialSymbols.IconGrade.``-25``
      MaterialSymbols.IconGrade.``0``
      MaterialSymbols.IconGrade.``200``
    ]

    let gradeRadios =
      gradeOptions
      |> List.map (fun g ->
        GridItem.Create(
          Radio.Create(
            grade,
            g,
            displayText = View.Const(sprintf "%A" g),
            attrs = [ Radio.Color.toClass BrandColor.Primary |> cl ]
          ),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 3
        ))

    let createItem icon =
      GridItem.Create(
        div [
          cls [
            Flex.Flex.allSizes
            FlexDirection.Column.allSizes
            JustifyContent.toClass JustifyContent.Center
            AlignItems.toClass AlignItems.Center
            yield! Margin.toClasses Margin.Bottom.extraSmall
          ]
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
            Icon.Create(
              icon,
              style = View.Const style,
              weight = weight.View,
              grade = grade.View,
              opticalSize = opticalSize.View,
              attrs = [ Attr.Style "font-size" "3em" ]
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

          Body2.Create(iconText, attrs = [ Attr.Style "text-align" "center" ])
        ],
        xs = Grid.Width.create 6,
        sm = Grid.Width.create 4,
        md = Grid.Width.create 2,
        xl = Grid.Width.create 1
      )

    let createHeader title expanded =
      ExpansionPanelHeader.CreateWithDefaultIcons(
        text title,
        expanded,
        attrs = [ cls [ ExpansionPanel.Color.toColor BrandColor.Primary ] ]
      )

    let actionsExpanded = Var.Create true

    let expansionPanel =
      ExpansionPanelContainer.Create(
        [
          ExpansionPanel.Create(
            header = createHeader "Sample Icons" actionsExpanded,
            content =
              Grid.Create(
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
              ),
            expanded = actionsExpanded
          )
        ]
      )

    Grid.Create(
      [
        GridItem.Create(H6.Create("Select Fill"), xs = Grid.Width.create 12, md = Grid.Width.create 12)
        GridItem.Create(Grid.Create(filledRadios), xs = Grid.Width.create 12, md = Grid.Width.create 9)
        FlexBreak.Create()
        GridItem.Create(H6.Create("Select Style"), xs = Grid.Width.create 12, md = Grid.Width.create 12)
        GridItem.Create(Grid.Create(styleRadios), xs = Grid.Width.create 12, md = Grid.Width.create 9)
        FlexBreak.Create()
        GridItem.Create(
          H6.Create("Select Optical Size"),
          xs = Grid.Width.create 12,
          md = Grid.Width.create 12
        )
        GridItem.Create(Grid.Create(opticalSizeRadios), xs = Grid.Width.create 12, md = Grid.Width.create 9)
        FlexBreak.Create()
        GridItem.Create(H6.Create("Select Weight"), xs = Grid.Width.create 12, md = Grid.Width.create 12)
        GridItem.Create(Grid.Create(weightRadios), xs = Grid.Width.create 12, md = Grid.Width.create 9)
        FlexBreak.Create()
        GridItem.Create(H6.Create("Select Grade"), xs = Grid.Width.create 12, md = Grid.Width.create 12)
        GridItem.Create(Grid.Create(gradeRadios), xs = Grid.Width.create 12, md = Grid.Width.create 9)
        FlexBreak.Create()
        GridItem.Create(expansionPanel, xs = Grid.Width.create 12)
      ]
    )
    |> Helpers.section "Icons" Doc.Empty

  let render () =
    Container.Create(
      div [] [
        H1.Create("Icons", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
        Body1.Create(
          "Weave has built in helpers for Material Symbols icons. The icons are structured within a DU which is organized into child DUs which align with the categories defined by Google.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Body1.Create(
          "Note, that the Icon DU does not guarantee static type safety when selecting icons since the end user is reponsible for including the correct Google Fonts link in their HTML head section. Make sure to include the appropriate link for the style of icons you wish to use.",
          attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ]
        )

        Helpers.divider ()
        referenceGoogleFonts ()
        Helpers.divider ()
        icons ()
      ]
    )
