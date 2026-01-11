namespace Weave

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers

// Note: There are a lot of snake case matches here because I was too lazy to write a converter from Pascal case
// which didn't use any System.* functions which don't work during WebSharper compilation.

[<JavaScript>]
module Icons =

  [<Struct>]
  type MaterialGeneration = | Symbols

  /// https://fonts.google.com/icons?icon.set=Material+Symbols
  module MaterialSymbols =

    [<RequireQualifiedAccess; Struct>]
    type Style =
      | Outlined
      | OutlinedFilled
      | Rounded
      | RoundedFilled
      | Sharp
      | SharpFilled

    [<RequireQualifiedAccess; Struct>]
    type IconWeight =
      | ``100``
      | ``200``
      | ``300``
      | ``400``
      | ``500``
      | ``600``
      | ``700``

    [<RequireQualifiedAccess; Struct>]
    type IconGrade =
      | ``-25``
      | ``0``
      | ``200``

    [<RequireQualifiedAccess; Struct>]
    type OpticalSize =
      | ``20``
      | ``24``
      | ``40``
      | ``48``

    [<RequireQualifiedAccess; Struct>]
    type Action =
      | ``3dRotation``
      | Accessibility
      | AccessibilityNew
      | Accessible
      | AccessibleForward
      | AccountBox
      | AccountChild
      | AccountChildInvert
      | AccountCircle
      | AccountCircleOff
      | Ad
      | AdGroup
      | AdGroupOff
      | AdOff
      | AddAd
      | AddAlert
      | AdsClick
      | Alarm
      | AlarmAdd
      | AlarmOff
      | AlarmOn
      | AlarmPause
      | AlarmSmartWake
      | AllInclusive
      | AllOut
      | Anchor
      | Api
      | Approval
      | ApprovalDelegation
      | ApprovalDelegationOff
      | ArrowSelectorTool
      | AutoDelete
      | AwardStar
      | BackgroundReplace
      | Backup
      | BackupTable
      | BatchPrediction
      | BookRibbon
      | Bookmark
      | BookmarkAdd
      | BookmarkAdded
      | BookmarkBag
      | BookmarkCheck
      | BookmarkFlag
      | BookmarkHeart
      | BookmarkManager
      | BookmarkRemove
      | BookmarkStacks
      | BookmarkStar
      | Bookmarks
      | Browse
      | BugReport
      | Build
      | BuildCircle
      | CalendarCheck
      | CalendarClock
      | CalendarLock
      | CalendarMonth
      | CalendarToday
      | Category
      | Celebration
      | ChangeHistory
      | ChromeReaderMode
      | CircleNotifications
      | Circles
      | CirclesExt
      | Code
      | CodeBlocks
      | CodeOff
      | CollectionsBookmark
      | Commit
      | ComponentExchange
      | ContactsProduct
      | Dangerous
      | DataLossPrevention
      | DateRange
      | DeleteHistory
      | DeveloperGuide
      | DomainVerification
      | DomainVerificationOff
      | DraftOrders
      | DynamicFeed
      | EditCalendar
      | EditNotifications
      | EditSquare
      | Error
      | Event
      | EventAvailable
      | EventBusy
      | EventNote
      | EventRepeat
      | EventUpcoming
      | Extension
      | FeatureSearch
      | Feedback
      | FindReplace
      | Fingerprint
      | FingerprintOff
      | Flutter
      | FlutterDash
      | FreeCancellation
      | Gesture
      | GestureSelect
      | HandGesture
      | HandGestureOff
      | Help
      | HelpCenter
      | HelpClinic
      | History
      | History2
      | HistoryOff
      | HistoryToggleOff
      | HomeAppLogo
      | HotelClass
      | Hourglass
      | HourglassCheck
      | HourglassDisabled
      | HourglassEmpty
      | HourglassPause
      | HowToReg
      | Http
      | IndeterminateQuestionBox
      | Info
      | InfoI
      | Input
      | Interests
      | Keep
      | KeepOff
      | KeepPublic
      | Label
      | LabelImportant
      | LabelOff
      | Language
      | License
      | Lightbulb
      | LightbulbCircle
      | Lists
      | Lock
      | LockClock
      | LockOpen
      | LockOpenCircle
      | LockOpenRight
      | LockPerson
      | LockReset
      | LogoDev
      | ManageAccounts
      | ManageHistory
      | Manufacturing
      | MeasuringTape
      | ModelTraining
      | More
      | MoreTime
      | NewLabel
      | NoAccounts
      | NotificationAdd
      | NotificationImportant
      | OfflineBolt
      | OfflinePin
      | OfflinePinOff
      | OnDeviceTraining
      | OnlinePrediction
      | OpenInBrowser
      | Outbound
      | Pageview
      | PanTool
      | PanToolAlt
      | PanZoom
      | Pending
      | PermContactCalendar
      | PersonAddDisabled
      | PersonEdit
      | PinEnd
      | PinInvoke
      | Pinboard
      | PinboardUnread
      | PinchZoomIn
      | PinchZoomOut
      | Polymer
      | PowerSettingsCircle
      | PowerSettingsNew
      | Preview
      | PreviewOff
      | Priority
      | PriorityHigh
      | Problem
      | PublishedWithChanges
      | QuestionMark
      | RateReview
      | RecordVoiceOver
      | ReleaseAlert
      | Reminder
      | RoundedCorner
      | Rsvp
      | Rule
      | RunningWithErrors
      | SaveAs
      | Schedule
      | ScrollableHeader
      | Sdk
      | SearchActivity
      | SearchHandsFree
      | SelectWindow
      | SelectWindow2
      | SelectWindowOff
      | SettingsAccountBox
      | SettingsOverscan
      | SettingsPower
      | Shadow
      | ShadowAdd
      | ShadowMinus
      | Shift
      | ShiftLock
      | ShiftLockOff
      | Snooze
      | SquareFoot
      | Stars
      | Sticker
      | StickerAdd
      | SupervisedUserCircle
      | SupervisedUserCircleOff
      | SupervisorAccount
      | Support
      | Swipe
      | Target
      | TaskAlt
      | TimeAuto
      | Timer10Alt1
      | Timer3Alt1
      | TimerPause
      | TimerPlay
      | Today
      | TouchApp
      | TouchDouble
      | TouchLong
      | TouchTriple
      | TrackpadInput
      | TrackpadInput2
      | TrackpadInput3
      | Translate
      | TranslateIndic
      | Unlicense
      | Unpublished
      | Update
      | UpdateDisabled
      | Upgrade
      | UploadFile
      | UserAttributes
      | Verified
      | VerifiedOff
      | Visibility
      | VisibilityLock
      | VisibilityOff
      | VoiceOverOff
      | WandShine
      | WandStars
      | Warning
      | WarningOff
      | WatchScreentime
      | WaterLock
      | Web
      | WebAsset
      | WebAssetOff
      | WebTraffic
      | Webhook
      | WifiProtectedSetup
      | Wysiwyg
      | YoutubeActivity

    module Action =

      let toSnakeCase (icon: Action) =
        match icon with
        | Action.``3dRotation`` -> "3d_rotation"
        | Action.Accessibility -> "accessibility"
        | Action.AccessibilityNew -> "accessibility_new"
        | Action.Accessible -> "accessible"
        | Action.AccessibleForward -> "accessible_forward"
        | Action.AccountBox -> "account_box"
        | Action.AccountChild -> "account_child"
        | Action.AccountChildInvert -> "account_child_invert"
        | Action.AccountCircle -> "account_circle"
        | Action.AccountCircleOff -> "account_circle_off"
        | Action.Ad -> "ad"
        | Action.AdGroup -> "ad_group"
        | Action.AdGroupOff -> "ad_group_off"
        | Action.AdOff -> "ad_off"
        | Action.AddAd -> "add_ad"
        | Action.AddAlert -> "add_alert"
        | Action.AdsClick -> "ads_click"
        | Action.Alarm -> "alarm"
        | Action.AlarmAdd -> "alarm_add"
        | Action.AlarmOff -> "alarm_off"
        | Action.AlarmOn -> "alarm_on"
        | Action.AlarmPause -> "alarm_pause"
        | Action.AlarmSmartWake -> "alarm_smart_wake"
        | Action.AllInclusive -> "all_inclusive"
        | Action.AllOut -> "all_out"
        | Action.Anchor -> "anchor"
        | Action.Api -> "api"
        | Action.Approval -> "approval"
        | Action.ApprovalDelegation -> "approval_delegation"
        | Action.ApprovalDelegationOff -> "approval_delegation_off"
        | Action.ArrowSelectorTool -> "arrow_selector_tool"
        | Action.AutoDelete -> "auto_delete"
        | Action.AwardStar -> "award_star"
        | Action.BackgroundReplace -> "background_replace"
        | Action.Backup -> "backup"
        | Action.BackupTable -> "backup_table"
        | Action.BatchPrediction -> "batch_prediction"
        | Action.BookRibbon -> "book_ribbon"
        | Action.Bookmark -> "bookmark"
        | Action.BookmarkAdd -> "bookmark_add"
        | Action.BookmarkAdded -> "bookmark_added"
        | Action.BookmarkBag -> "bookmark_bag"
        | Action.BookmarkCheck -> "bookmark_check"
        | Action.BookmarkFlag -> "bookmark_flag"
        | Action.BookmarkHeart -> "bookmark_heart"
        | Action.BookmarkManager -> "bookmark_manager"
        | Action.BookmarkRemove -> "bookmark_remove"
        | Action.BookmarkStacks -> "bookmark_stacks"
        | Action.BookmarkStar -> "bookmark_star"
        | Action.Bookmarks -> "bookmarks"
        | Action.Browse -> "browse"
        | Action.BugReport -> "bug_report"
        | Action.Build -> "build"
        | Action.BuildCircle -> "build_circle"
        | Action.CalendarCheck -> "calendar_check"
        | Action.CalendarClock -> "calendar_clock"
        | Action.CalendarLock -> "calendar_lock"
        | Action.CalendarMonth -> "calendar_month"
        | Action.CalendarToday -> "calendar_today"
        | Action.Category -> "category"
        | Action.Celebration -> "celebration"
        | Action.ChangeHistory -> "change_history"
        | Action.ChromeReaderMode -> "chrome_reader_mode"
        | Action.CircleNotifications -> "circle_notifications"
        | Action.Circles -> "circles"
        | Action.CirclesExt -> "circles_ext"
        | Action.Code -> "code"
        | Action.CodeBlocks -> "code_blocks"
        | Action.CodeOff -> "code_off"
        | Action.CollectionsBookmark -> "collections_bookmark"
        | Action.Commit -> "commit"
        | Action.ComponentExchange -> "component_exchange"
        | Action.ContactsProduct -> "contacts_product"
        | Action.Dangerous -> "dangerous"
        | Action.DataLossPrevention -> "data_loss_prevention"
        | Action.DateRange -> "date_range"
        | Action.DeleteHistory -> "delete_history"
        | Action.DeveloperGuide -> "developer_guide"
        | Action.DomainVerification -> "domain_verification"
        | Action.DomainVerificationOff -> "domain_verification_off"
        | Action.DraftOrders -> "draft_orders"
        | Action.DynamicFeed -> "dynamic_feed"
        | Action.EditCalendar -> "edit_calendar"
        | Action.EditNotifications -> "edit_notifications"
        | Action.EditSquare -> "edit_square"
        | Action.Error -> "error"
        | Action.Event -> "event"
        | Action.EventAvailable -> "event_available"
        | Action.EventBusy -> "event_busy"
        | Action.EventNote -> "event_note"
        | Action.EventRepeat -> "event_repeat"
        | Action.EventUpcoming -> "event_upcoming"
        | Action.Extension -> "extension"
        | Action.FeatureSearch -> "feature_search"
        | Action.Feedback -> "feedback"
        | Action.FindReplace -> "find_replace"
        | Action.Fingerprint -> "fingerprint"
        | Action.FingerprintOff -> "fingerprint_off"
        | Action.Flutter -> "flutter"
        | Action.FlutterDash -> "flutter_dash"
        | Action.FreeCancellation -> "free_cancellation"
        | Action.Gesture -> "gesture"
        | Action.GestureSelect -> "gesture_select"
        | Action.HandGesture -> "hand_gesture"
        | Action.HandGestureOff -> "hand_gesture_off"
        | Action.Help -> "help"
        | Action.HelpCenter -> "help_center"
        | Action.HelpClinic -> "help_clinic"
        | Action.History -> "history"
        | Action.History2 -> "history2"
        | Action.HistoryOff -> "history_off"
        | Action.HistoryToggleOff -> "history_toggle_off"
        | Action.HomeAppLogo -> "home_app_logo"
        | Action.HotelClass -> "hotel_class"
        | Action.Hourglass -> "hourglass"
        | Action.HourglassCheck -> "hourglass_check"
        | Action.HourglassDisabled -> "hourglass_disabled"
        | Action.HourglassEmpty -> "hourglass_empty"
        | Action.HourglassPause -> "hourglass_pause"
        | Action.HowToReg -> "how_to_reg"
        | Action.Http -> "http"
        | Action.IndeterminateQuestionBox -> "indeterminate_question_box"
        | Action.Info -> "info"
        | Action.InfoI -> "info_i"
        | Action.Input -> "input"
        | Action.Interests -> "interests"
        | Action.Keep -> "keep"
        | Action.KeepOff -> "keep_off"
        | Action.KeepPublic -> "keep_public"
        | Action.Label -> "label"
        | Action.LabelImportant -> "label_important"
        | Action.LabelOff -> "label_off"
        | Action.Language -> "language"
        | Action.License -> "license"
        | Action.Lightbulb -> "lightbulb"
        | Action.LightbulbCircle -> "lightbulb_circle"
        | Action.Lists -> "lists"
        | Action.Lock -> "lock"
        | Action.LockClock -> "lock_clock"
        | Action.LockOpen -> "lock_open"
        | Action.LockOpenCircle -> "lock_open_circle"
        | Action.LockOpenRight -> "lock_open_right"
        | Action.LockPerson -> "lock_person"
        | Action.LockReset -> "lock_reset"
        | Action.LogoDev -> "logo_dev"
        | Action.ManageAccounts -> "manage_accounts"
        | Action.ManageHistory -> "manage_history"
        | Action.Manufacturing -> "manufacturing"
        | Action.MeasuringTape -> "measuring_tape"
        | Action.ModelTraining -> "model_training"
        | Action.More -> "more"
        | Action.MoreTime -> "more_time"
        | Action.NewLabel -> "new_label"
        | Action.NoAccounts -> "no_accounts"
        | Action.NotificationAdd -> "notification_add"
        | Action.NotificationImportant -> "notification_important"
        | Action.OfflineBolt -> "offline_bolt"
        | Action.OfflinePin -> "offline_pin"
        | Action.OfflinePinOff -> "offline_pin_off"
        | Action.OnDeviceTraining -> "on_device_training"
        | Action.OnlinePrediction -> "online_prediction"
        | Action.OpenInBrowser -> "open_in_browser"
        | Action.Outbound -> "outbound"
        | Action.Pageview -> "pageview"
        | Action.PanTool -> "pan_tool"
        | Action.PanToolAlt -> "pan_tool_alt"
        | Action.PanZoom -> "pan_zoom"
        | Action.Pending -> "pending"
        | Action.PermContactCalendar -> "perm_contact_calendar"
        | Action.PersonAddDisabled -> "person_add_disabled"
        | Action.PersonEdit -> "person_edit"
        | Action.PinEnd -> "pin_end"
        | Action.PinInvoke -> "pin_invoke"
        | Action.Pinboard -> "pinboard"
        | Action.PinboardUnread -> "pinboard_unread"
        | Action.PinchZoomIn -> "pinch_zoom_in"
        | Action.PinchZoomOut -> "pinch_zoom_out"
        | Action.Polymer -> "polymer"
        | Action.PowerSettingsCircle -> "power_settings_circle"
        | Action.PowerSettingsNew -> "power_settings_new"
        | Action.Preview -> "preview"
        | Action.PreviewOff -> "preview_off"
        | Action.Priority -> "priority"
        | Action.PriorityHigh -> "priority_high"
        | Action.Problem -> "problem"
        | Action.PublishedWithChanges -> "published_with_changes"
        | Action.QuestionMark -> "question_mark"
        | Action.RateReview -> "rate_review"
        | Action.RecordVoiceOver -> "record_voice_over"
        | Action.ReleaseAlert -> "release_alert"
        | Action.Reminder -> "reminder"
        | Action.RoundedCorner -> "rounded_corner"
        | Action.Rsvp -> "rsvp"
        | Action.Rule -> "rule"
        | Action.RunningWithErrors -> "running_with_errors"
        | Action.SaveAs -> "save_as"
        | Action.Schedule -> "schedule"
        | Action.ScrollableHeader -> "scrollable_header"
        | Action.Sdk -> "sdk"
        | Action.SearchActivity -> "search_activity"
        | Action.SearchHandsFree -> "search_hands_free"
        | Action.SelectWindow -> "select_window"
        | Action.SelectWindow2 -> "select_window2"
        | Action.SelectWindowOff -> "select_window_off"
        | Action.SettingsAccountBox -> "settings_account_box"
        | Action.SettingsOverscan -> "settings_overscan"
        | Action.SettingsPower -> "settings_power"
        | Action.Shadow -> "shadow"
        | Action.ShadowAdd -> "shadow_add"
        | Action.ShadowMinus -> "shadow_minus"
        | Action.Shift -> "shift"
        | Action.ShiftLock -> "shift_lock"
        | Action.ShiftLockOff -> "shift_lock_off"
        | Action.Snooze -> "snooze"
        | Action.SquareFoot -> "square_foot"
        | Action.Stars -> "stars"
        | Action.Sticker -> "sticker"
        | Action.StickerAdd -> "sticker_add"
        | Action.SupervisedUserCircle -> "supervised_user_circle"
        | Action.SupervisedUserCircleOff -> "supervised_user_circle_off"
        | Action.SupervisorAccount -> "supervisor_account"
        | Action.Support -> "support"
        | Action.Swipe -> "swipe"
        | Action.Target -> "target"
        | Action.TaskAlt -> "task_alt"
        | Action.TimeAuto -> "time_auto"
        | Action.Timer10Alt1 -> "timer_10_alt_1"
        | Action.Timer3Alt1 -> "timer_3_alt_1"
        | Action.TimerPause -> "timer_pause"
        | Action.TimerPlay -> "timer_play"
        | Action.Today -> "today"
        | Action.TouchApp -> "touch_app"
        | Action.TouchDouble -> "touch_double"
        | Action.TouchLong -> "touch_long"
        | Action.TouchTriple -> "touch_triple"
        | Action.TrackpadInput -> "trackpad_input"
        | Action.TrackpadInput2 -> "trackpad_input_2"
        | Action.TrackpadInput3 -> "trackpad_input_3"
        | Action.Translate -> "translate"
        | Action.TranslateIndic -> "translate_indic"
        | Action.Unlicense -> "unlicense"
        | Action.Unpublished -> "unpublished"
        | Action.Update -> "update"
        | Action.UpdateDisabled -> "update_disabled"
        | Action.Upgrade -> "upgrade"
        | Action.UploadFile -> "upload_file"
        | Action.UserAttributes -> "user_attributes"
        | Action.Verified -> "verified"
        | Action.VerifiedOff -> "verified_off"
        | Action.Visibility -> "visibility"
        | Action.VisibilityLock -> "visibility_lock"
        | Action.VisibilityOff -> "visibility_off"
        | Action.VoiceOverOff -> "voice_over_off"
        | Action.WandShine -> "wand_shine"
        | Action.WandStars -> "wand_stars"
        | Action.Warning -> "warning"
        | Action.WarningOff -> "warning_off"
        | Action.WatchScreentime -> "watch_screentime"
        | Action.WaterLock -> "water_lock"
        | Action.Web -> "web"
        | Action.WebAsset -> "web_asset"
        | Action.WebAssetOff -> "web_asset_off"
        | Action.WebTraffic -> "web_traffic"
        | Action.Webhook -> "webhook"
        | Action.WifiProtectedSetup -> "wifi_protected_setup"
        | Action.Wysiwyg -> "wysiwyg"
        | Action.YoutubeActivity -> "youtube_activity"

    [<RequireQualifiedAccess; Struct>]
    type Activities =
      | Air
      | Architecture
      | ArrowCoolDown
      | ArrowWarmUp
      | AvgPace
      | AvgTime
      | Azm
      | Backpack
      | Badminton
      | BathOutdoor
      | BathPrivate
      | BathPublicLarge
      | Bia
      | Biotech
      | BooksMoviesAndMusic
      | Cadence
      | Cake
      | CakeAdd
      | Campaign
      | Camping
      | CheckInOut
      | Cleaning
      | ConfirmationNumber
      | Construction
      | Distance
      | DownhillSkiing
      | EcgHeart
      | Eda
      | Elevation
      | Engineering
      | Exercise
      | Experiment
      | FamilyLink
      | FeaturedSeasonalAndGifts
      | Fertile
      | Floor
      | GlassCup
      | HealthMetrics
      | Hiking
      | HowToVote
      | HrResting
      | IceSkating
      | Ifl
      | InteractiveSpace
      | Kayaking
      | Kitesurfing
      | Laps
      | MenstrualHealth
      | Mindfulness
      | MonitorWeightGain
      | MonitorWeightLoss
      | Newsstand
      | NoBackpack
      | NordicWalking
      | Onsen
      | Pace
      | Padel
      | Paragliding
      | PersonCelebrate
      | PersonPlay
      | PersonalInjury
      | Phishing
      | PhysicalTherapy
      | Piano
      | PianoOff
      | Pickleball
      | Podiatry
      | ReadinessScore
      | RealEstateAgent
      | Relax
      | RewardedAds
      | RollerSkating
      | Rowing
      | Sauna
      | School
      | Science
      | ScienceOff
      | Scoreboard
      | ScubaDiving
      | SelfImprovement
      | ServiceToolbox
      | Skateboarding
      | Sledding
      | SleepScore
      | Snowboarding
      | Snowshoeing
      | Spo2
      | Sports
      | SportsAndOutdoors
      | SportsBaseball
      | SportsBasketball
      | SportsCricket
      | SportsEsports
      | SportsFootball
      | SportsGolf
      | SportsGymnastics
      | SportsHandball
      | SportsHockey
      | SportsKabaddi
      | SportsMartialArts
      | SportsMma
      | SportsMotorsports
      | SportsRugby
      | SportsScore
      | SportsSoccer
      | SportsTennis
      | SportsVolleyball
      | Sprint
      | Steps
      | Storm
      | StressManagement
      | Surfing
      | SwitchAccount
      | Swords
      | Theaters
      | Toys
      | ToysAndGames
      | ToysFan
      | Trophy
      | Vo2Max
      | VolunteerActivism
      | Water
      | WaterFull
      | WaterLoss
      | WaterMedium
      | Waves

    module Activities =

      let toSnakeCase (icon: Activities) =
        match icon with
        | Activities.Air -> "air"
        | Activities.Architecture -> "architecture"
        | Activities.ArrowCoolDown -> "arrow_cool_down"
        | Activities.ArrowWarmUp -> "arrow_warm_up"
        | Activities.AvgPace -> "avg_pace"
        | Activities.AvgTime -> "avg_time"
        | Activities.Azm -> "azm"
        | Activities.Backpack -> "backpack"
        | Activities.Badminton -> "badminton"
        | Activities.BathOutdoor -> "bath_outdoor"
        | Activities.BathPrivate -> "bath_private"
        | Activities.BathPublicLarge -> "bath_public_large"
        | Activities.Bia -> "bia"
        | Activities.Biotech -> "biotech"
        | Activities.BooksMoviesAndMusic -> "books_movies_and_music"
        | Activities.Cadence -> "cadence"
        | Activities.Cake -> "cake"
        | Activities.CakeAdd -> "cake_add"
        | Activities.Campaign -> "campaign"
        | Activities.Camping -> "camping"
        | Activities.CheckInOut -> "check_in_out"
        | Activities.Cleaning -> "cleaning"
        | Activities.ConfirmationNumber -> "confirmation_number"
        | Activities.Construction -> "construction"
        | Activities.Distance -> "distance"
        | Activities.DownhillSkiing -> "downhill_skiing"
        | Activities.EcgHeart -> "ecg_heart"
        | Activities.Eda -> "eda"
        | Activities.Elevation -> "elevation"
        | Activities.Engineering -> "engineering"
        | Activities.Exercise -> "exercise"
        | Activities.Experiment -> "experiment"
        | Activities.FamilyLink -> "family_link"
        | Activities.FeaturedSeasonalAndGifts -> "featured_seasonal_and_gifts"
        | Activities.Fertile -> "fertile"
        | Activities.Floor -> "floor"
        | Activities.GlassCup -> "glass_cup"
        | Activities.HealthMetrics -> "health_metrics"
        | Activities.Hiking -> "hiking"
        | Activities.HowToVote -> "how_to_vote"
        | Activities.HrResting -> "hr_resting"
        | Activities.IceSkating -> "ice_skating"
        | Activities.Ifl -> "ifl"
        | Activities.InteractiveSpace -> "interactive_space"
        | Activities.Kayaking -> "kayaking"
        | Activities.Kitesurfing -> "kitesurfing"
        | Activities.Laps -> "laps"
        | Activities.MenstrualHealth -> "menstrual_health"
        | Activities.Mindfulness -> "mindfulness"
        | Activities.MonitorWeightGain -> "monitor_weight_gain"
        | Activities.MonitorWeightLoss -> "monitor_weight_loss"
        | Activities.Newsstand -> "newsstand"
        | Activities.NoBackpack -> "no_backpack"
        | Activities.NordicWalking -> "nordic_walking"
        | Activities.Onsen -> "onsen"
        | Activities.Pace -> "pace"
        | Activities.Padel -> "padel"
        | Activities.Paragliding -> "paragliding"
        | Activities.PersonCelebrate -> "person_celebrate"
        | Activities.PersonPlay -> "person_play"
        | Activities.PersonalInjury -> "personal_injury"
        | Activities.Phishing -> "phishing"
        | Activities.PhysicalTherapy -> "physical_therapy"
        | Activities.Piano -> "piano"
        | Activities.PianoOff -> "piano_off"
        | Activities.Pickleball -> "pickleball"
        | Activities.Podiatry -> "podiatry"
        | Activities.ReadinessScore -> "readiness_score"
        | Activities.RealEstateAgent -> "real_estate_agent"
        | Activities.Relax -> "relax"
        | Activities.RewardedAds -> "rewarded_ads"
        | Activities.RollerSkating -> "roller_skating"
        | Activities.Rowing -> "rowing"
        | Activities.Sauna -> "sauna"
        | Activities.School -> "school"
        | Activities.Science -> "science"
        | Activities.ScienceOff -> "science_off"
        | Activities.Scoreboard -> "scoreboard"
        | Activities.ScubaDiving -> "scuba_diving"
        | Activities.SelfImprovement -> "self_improvement"
        | Activities.ServiceToolbox -> "service_toolbox"
        | Activities.Skateboarding -> "skateboarding"
        | Activities.Sledding -> "sledding"
        | Activities.SleepScore -> "sleep_score"
        | Activities.Snowboarding -> "snowboarding"
        | Activities.Snowshoeing -> "snowshoeing"
        | Activities.Spo2 -> "spo2"
        | Activities.Sports -> "sports"
        | Activities.SportsAndOutdoors -> "sports_and_outdoors"
        | Activities.SportsBaseball -> "sports_baseball"
        | Activities.SportsBasketball -> "sports_basketball"
        | Activities.SportsCricket -> "sports_cricket"
        | Activities.SportsEsports -> "sports_esports"
        | Activities.SportsFootball -> "sports_football"
        | Activities.SportsGolf -> "sports_golf"
        | Activities.SportsGymnastics -> "sports_gymnastics"
        | Activities.SportsHandball -> "sports_handball"
        | Activities.SportsHockey -> "sports_hockey"
        | Activities.SportsKabaddi -> "sports_kabaddi"
        | Activities.SportsMartialArts -> "sports_martial_arts"
        | Activities.SportsMma -> "sports_mma"
        | Activities.SportsMotorsports -> "sports_motorsports"
        | Activities.SportsRugby -> "sports_rugby"
        | Activities.SportsScore -> "sports_score"
        | Activities.SportsSoccer -> "sports_soccer"
        | Activities.SportsTennis -> "sports_tennis"
        | Activities.SportsVolleyball -> "sports_volleyball"
        | Activities.Sprint -> "sprint"
        | Activities.Steps -> "steps"
        | Activities.Storm -> "storm"
        | Activities.StressManagement -> "stress_management"
        | Activities.Surfing -> "surfing"
        | Activities.SwitchAccount -> "switch_account"
        | Activities.Swords -> "swords"
        | Activities.Theaters -> "theaters"
        | Activities.Toys -> "toys"
        | Activities.ToysAndGames -> "toys_and_games"
        | Activities.ToysFan -> "toys_fan"
        | Activities.Trophy -> "trophy"
        | Activities.Vo2Max -> "vo2_max"
        | Activities.VolunteerActivism -> "volunteer_activism"
        | Activities.Water -> "water"
        | Activities.WaterFull -> "water_full"
        | Activities.WaterLoss -> "water_loss"
        | Activities.WaterMedium -> "water_medium"
        | Activities.Waves -> "waves"

    [<RequireQualifiedAccess; Struct>]
    type Android =
      | ``1xMobiledata``
      | ``1xMobiledataBadge``
      | ``3gMobiledata``
      | ``3gMobiledataBadge``
      | ``4gMobiledata``
      | ``4gMobiledataBadge``
      | ``4gPlusMobiledata``
      | ``5g``
      | ``5gMobiledataBadge``
      | Adb
      | AirplanemodeInactive
      | Android
      | AndroidCell4Bar
      | AndroidCell4BarAlert
      | AndroidCell4BarOff
      | AndroidCell4BarPlus
      | AndroidCell5Bar
      | AndroidCell5BarAlert
      | AndroidCell5BarOff
      | AndroidCell5BarPlus
      | AndroidCellDual4Bar
      | AndroidCellDual4BarAlert
      | AndroidCellDual4BarPlus
      | AndroidCellDual5Bar
      | AndroidCellDual5BarAlert
      | AndroidCellDual5BarPlus
      | AndroidWifi3Bar
      | AndroidWifi3BarAlert
      | AndroidWifi3BarLock
      | AndroidWifi3BarOff
      | AndroidWifi3BarPlus
      | AndroidWifi3BarQuestion
      | AndroidWifi4Bar
      | AndroidWifi4BarAlert
      | AndroidWifi4BarLock
      | AndroidWifi4BarOff
      | AndroidWifi4BarPlus
      | AndroidWifi4BarQuestion
      | ApkDocument
      | ApkInstall
      | BacklightHigh
      | BacklightHighOff
      | BacklightLow
      | BadgeCriticalBattery
      | Battery0Bar
      | Battery1Bar
      | Battery2Bar
      | Battery3Bar
      | Battery4Bar
      | Battery5Bar
      | Battery6Bar
      | BatteryAlert
      | BatteryAndroid0
      | BatteryAndroid1
      | BatteryAndroid2
      | BatteryAndroid3
      | BatteryAndroid4
      | BatteryAndroid5
      | BatteryAndroid6
      | BatteryAndroidAlert
      | BatteryAndroidBolt
      | BatteryAndroidFrame1
      | BatteryAndroidFrame2
      | BatteryAndroidFrame3
      | BatteryAndroidFrame4
      | BatteryAndroidFrame5
      | BatteryAndroidFrame6
      | BatteryAndroidFrameAlert
      | BatteryAndroidFrameBolt
      | BatteryAndroidFrameFull
      | BatteryAndroidFramePlus
      | BatteryAndroidFrameQuestion
      | BatteryAndroidFrameShare
      | BatteryAndroidFrameShield
      | BatteryAndroidFull
      | BatteryAndroidPlus
      | BatteryAndroidQuestion
      | BatteryAndroidShare
      | BatteryAndroidShield
      | BatteryChange
      | BatteryCharging20
      | BatteryCharging30
      | BatteryCharging50
      | BatteryCharging60
      | BatteryCharging80
      | BatteryCharging90
      | BatteryChargingFull
      | BatteryError
      | BatteryFull
      | BatteryFullAlt
      | BatteryLow
      | BatteryPlus
      | BatteryShare
      | BatteryStatusGood
      | BatteryUnknown
      | BatteryVeryLow
      | BigtopUpdates
      | Bluetooth
      | BluetoothConnected
      | BluetoothDisabled
      | BluetoothDrive
      | BluetoothSearching
      | BrightnessAlert
      | BrightnessAuto
      | BrightnessEmpty
      | BrightnessMedium
      | Cable
      | Cameraswitch
      | Charger
      | ContextualToken
      | ContextualTokenAdd
      | DarkMode
      | DataSaverOn
      | DataUsage
      | DevicesFold
      | DevicesFold2
      | DisplayExternalInput
      | DoNotDisturbOnTotalSilence
      | DockToBottom
      | DockToLeft
      | DockToRight
      | DualScreen
      | Dvr
      | EMobiledata
      | EMobiledataBadge
      | EvMobiledataBadge
      | FlashlightOff
      | FlashlightOn
      | GMobiledata
      | GMobiledataBadge
      | GppBad
      | GppMaybe
      | GraphicEq
      | Grid3x3
      | Grid3x3Off
      | Grid4x4
      | GridGoldenratio
      | HMobiledata
      | HMobiledataBadge
      | HPlusMobiledata
      | HPlusMobiledataBadge
      | Ios
      | KeyboardCapslockBadge
      | KeyboardExternalInput
      | KeyboardFull
      | KeyboardKeys
      | KeyboardOff
      | KeyboardOnscreen
      | KeyboardPreviousLanguage
      | LightMode
      | LteMobiledata
      | LteMobiledataBadge
      | LtePlusMobiledata
      | LtePlusMobiledataBadge
      | MagnifyDocked
      | MagnifyFullscreen
      | MediaBluetoothOff
      | MediaBluetoothOn
      | MobileSensorHi
      | MobileSensorLo
      | MobileWrench
      | MobiledataOff
      | ModeStandby
      | Nearby
      | NearbyError
      | NearbyOff
      | NetworkCell
      | NetworkCheck
      | NetworkLocked
      | NetworkPing
      | NetworkWifi
      | NetworkWifi1Bar
      | NetworkWifi1BarLocked
      | NetworkWifi2Bar
      | NetworkWifi2BarLocked
      | NetworkWifi3Bar
      | NetworkWifi3BarLocked
      | NetworkWifiLocked
      | Nfc
      | NfcOff
      | Nightlight
      | NoiseAware
      | NoiseControlOff
      | NoiseControlOn
      | OverviewKey
      | Password
      | Password2
      | Password2Off
      | Pattern
      | PermDataSetting
      | PermScanWifi
      | Pin
      | PortableWifiOff
      | QuickPhrases
      | RMobiledata
      | Radar
      | RssFeed
      | ScreenRecord
      | ScreenRotationAlt
      | ScreenRotationUp
      | ScreenshotFrame
      | ScreenshotFrame2
      | ScreenshotKeyboard
      | ScreenshotRegion
      | SettingsSystemDaydream
      | SignalCellular0Bar
      | SignalCellular1Bar
      | SignalCellular2Bar
      | SignalCellular3Bar
      | SignalCellular4Bar
      | SignalCellularAlt
      | SignalCellularAlt1Bar
      | SignalCellularAlt2Bar
      | SignalCellularConnectedNoInternet0Bar
      | SignalCellularConnectedNoInternet4Bar
      | SignalCellularNodata
      | SignalCellularNull
      | SignalCellularOff
      | SignalCellularPause
      | SignalDisconnected
      | SignalWifi0Bar
      | SignalWifi4Bar
      | SignalWifiBad
      | SignalWifiOff
      | SignalWifiStatusbarNotConnected
      | SignalWifiStatusbarNull
      | SimCardDownload
      | Splitscreen
      | SplitscreenAdd
      | SplitscreenBottom
      | SplitscreenLeft
      | SplitscreenRight
      | SplitscreenTop
      | SplitscreenVerticalAdd
      | Storage
      | Stylus
      | StylusNote
      | Thermostat
      | Timer10Select
      | Timer3Select
      | Timer5
      | Timer5Shutter
      | Usb
      | UsbOff
      | Wallpaper
      | WallpaperSlideshow
      | Widgets
      | Wifi
      | Wifi1Bar
      | Wifi2Bar
      | WifiCallingBar1
      | WifiCallingBar2
      | WifiCallingBar3
      | WifiFind
      | WifiHome
      | WifiLock
      | WifiNotification
      | WifiOff
      | WifiTethering
      | WifiTetheringError
      | WifiTetheringOff

    module Android =

      let toSnakeCase (icon: Android) =
        match icon with
        | Android.``1xMobiledata`` -> "1x_mobiledata"
        | Android.``1xMobiledataBadge`` -> "1x_mobiledata_badge"
        | Android.``3gMobiledata`` -> "3g_mobiledata"
        | Android.``3gMobiledataBadge`` -> "3g_mobiledata_badge"
        | Android.``4gMobiledata`` -> "4g_mobiledata"
        | Android.``4gMobiledataBadge`` -> "4g_mobiledata_badge"
        | Android.``4gPlusMobiledata`` -> "4g_plus_mobiledata"
        | Android.``5g`` -> "5g"
        | Android.``5gMobiledataBadge`` -> "5g_mobiledata_badge"
        | Android.Adb -> "adb"
        | Android.AirplanemodeInactive -> "airplanemode_inactive"
        | Android.Android -> "android"
        | Android.AndroidCell4Bar -> "android_cell_4_bar"
        | Android.AndroidCell4BarAlert -> "android_cell_4_bar_alert"
        | Android.AndroidCell4BarOff -> "android_cell_4_bar_off"
        | Android.AndroidCell4BarPlus -> "android_cell_4_bar_plus"
        | Android.AndroidCell5Bar -> "android_cell_5_bar"
        | Android.AndroidCell5BarAlert -> "android_cell_5_bar_alert"
        | Android.AndroidCell5BarOff -> "android_cell_5_bar_off"
        | Android.AndroidCell5BarPlus -> "android_cell_5_bar_plus"
        | Android.AndroidCellDual4Bar -> "android_cell_dual_4_bar"
        | Android.AndroidCellDual4BarAlert -> "android_cell_dual_4_bar_alert"
        | Android.AndroidCellDual4BarPlus -> "android_cell_dual_4_bar_plus"
        | Android.AndroidCellDual5Bar -> "android_cell_dual_5_bar"
        | Android.AndroidCellDual5BarAlert -> "android_cell_dual_5_bar_alert"
        | Android.AndroidCellDual5BarPlus -> "android_cell_dual_5_bar_plus"
        | Android.AndroidWifi3Bar -> "android_wifi_3_bar"
        | Android.AndroidWifi3BarAlert -> "android_wifi_3_bar_alert"
        | Android.AndroidWifi3BarLock -> "android_wifi_3_bar_lock"
        | Android.AndroidWifi3BarOff -> "android_wifi_3_bar_off"
        | Android.AndroidWifi3BarPlus -> "android_wifi_3_bar_plus"
        | Android.AndroidWifi3BarQuestion -> "android_wifi_3_bar_question"
        | Android.AndroidWifi4Bar -> "android_wifi_4_bar"
        | Android.AndroidWifi4BarAlert -> "android_wifi_4_bar_alert"
        | Android.AndroidWifi4BarLock -> "android_wifi_4_bar_lock"
        | Android.AndroidWifi4BarOff -> "android_wifi_4_bar_off"
        | Android.AndroidWifi4BarPlus -> "android_wifi_4_bar_plus"
        | Android.AndroidWifi4BarQuestion -> "android_wifi_4_bar_question"
        | Android.ApkDocument -> "apk_document"
        | Android.ApkInstall -> "apk_install"
        | Android.BacklightHigh -> "backlight_high"
        | Android.BacklightHighOff -> "backlight_high_off"
        | Android.BacklightLow -> "backlight_low"
        | Android.BadgeCriticalBattery -> "badge_critical_battery"
        | Android.Battery0Bar -> "battery_0_bar"
        | Android.Battery1Bar -> "battery_1_bar"
        | Android.Battery2Bar -> "battery_2_bar"
        | Android.Battery3Bar -> "battery_3_bar"
        | Android.Battery4Bar -> "battery_4_bar"
        | Android.Battery5Bar -> "battery_5_bar"
        | Android.Battery6Bar -> "battery_6_bar"
        | Android.BatteryAlert -> "battery_alert"
        | Android.BatteryAndroid0 -> "battery_android_0"
        | Android.BatteryAndroid1 -> "battery_android_1"
        | Android.BatteryAndroid2 -> "battery_android_2"
        | Android.BatteryAndroid3 -> "battery_android_3"
        | Android.BatteryAndroid4 -> "battery_android_4"
        | Android.BatteryAndroid5 -> "battery_android_5"
        | Android.BatteryAndroid6 -> "battery_android_6"
        | Android.BatteryAndroidAlert -> "battery_android_alert"
        | Android.BatteryAndroidBolt -> "battery_android_bolt"
        | Android.BatteryAndroidFrame1 -> "battery_android_frame_1"
        | Android.BatteryAndroidFrame2 -> "battery_android_frame_2"
        | Android.BatteryAndroidFrame3 -> "battery_android_frame_3"
        | Android.BatteryAndroidFrame4 -> "battery_android_frame_4"
        | Android.BatteryAndroidFrame5 -> "battery_android_frame_5"
        | Android.BatteryAndroidFrame6 -> "battery_android_frame_6"
        | Android.BatteryAndroidFrameAlert -> "battery_android_frame_alert"
        | Android.BatteryAndroidFrameBolt -> "battery_android_frame_bolt"
        | Android.BatteryAndroidFrameFull -> "battery_android_frame_full"
        | Android.BatteryAndroidFramePlus -> "battery_android_frame_plus"
        | Android.BatteryAndroidFrameQuestion -> "battery_android_frame_question"
        | Android.BatteryAndroidFrameShare -> "battery_android_frame_share"
        | Android.BatteryAndroidFrameShield -> "battery_android_frame_shield"
        | Android.BatteryAndroidFull -> "battery_android_full"
        | Android.BatteryAndroidPlus -> "battery_android_plus"
        | Android.BatteryAndroidQuestion -> "battery_android_question"
        | Android.BatteryAndroidShare -> "battery_android_share"
        | Android.BatteryAndroidShield -> "battery_android_shield"
        | Android.BatteryChange -> "battery_change"
        | Android.BatteryCharging20 -> "battery_charging_20"
        | Android.BatteryCharging30 -> "battery_charging_30"
        | Android.BatteryCharging50 -> "battery_charging_50"
        | Android.BatteryCharging60 -> "battery_charging_60"
        | Android.BatteryCharging80 -> "battery_charging_80"
        | Android.BatteryCharging90 -> "battery_charging_90"
        | Android.BatteryChargingFull -> "battery_charging_full"
        | Android.BatteryError -> "battery_error"
        | Android.BatteryFull -> "battery_full"
        | Android.BatteryFullAlt -> "battery_full_alt"
        | Android.BatteryLow -> "battery_low"
        | Android.BatteryPlus -> "battery_plus"
        | Android.BatteryShare -> "battery_share"
        | Android.BatteryStatusGood -> "battery_status_good"
        | Android.BatteryUnknown -> "battery_unknown"
        | Android.BatteryVeryLow -> "battery_very_low"
        | Android.BigtopUpdates -> "bigtop_updates"
        | Android.Bluetooth -> "bluetooth"
        | Android.BluetoothConnected -> "bluetooth_connected"
        | Android.BluetoothDisabled -> "bluetooth_disabled"
        | Android.BluetoothDrive -> "bluetooth_drive"
        | Android.BluetoothSearching -> "bluetooth_searching"
        | Android.BrightnessAlert -> "brightness_alert"
        | Android.BrightnessAuto -> "brightness_auto"
        | Android.BrightnessEmpty -> "brightness_empty"
        | Android.BrightnessMedium -> "brightness_medium"
        | Android.Cable -> "cable"
        | Android.Cameraswitch -> "cameraswitch"
        | Android.Charger -> "charger"
        | Android.ContextualToken -> "contextual_token"
        | Android.ContextualTokenAdd -> "contextual_token_add"
        | Android.DarkMode -> "dark_mode"
        | Android.DataSaverOn -> "data_saver_on"
        | Android.DataUsage -> "data_usage"
        | Android.DevicesFold -> "devices_fold"
        | Android.DevicesFold2 -> "devices_fold_2"
        | Android.DisplayExternalInput -> "display_external_input"
        | Android.DoNotDisturbOnTotalSilence -> "do_not_disturb_on_total_silence"
        | Android.DockToBottom -> "dock_to_bottom"
        | Android.DockToLeft -> "dock_to_left"
        | Android.DockToRight -> "dock_to_right"
        | Android.DualScreen -> "dual_screen"
        | Android.Dvr -> "dvr"
        | Android.EMobiledata -> "e_mobiledata"
        | Android.EMobiledataBadge -> "e_mobiledata_badge"
        | Android.EvMobiledataBadge -> "ev_mobiledata_badge"
        | Android.FlashlightOff -> "flashlight_off"
        | Android.FlashlightOn -> "flashlight_on"
        | Android.GMobiledata -> "g_mobiledata"
        | Android.GMobiledataBadge -> "g_mobiledata_badge"
        | Android.GppBad -> "gpp_bad"
        | Android.GppMaybe -> "gpp_maybe"
        | Android.GraphicEq -> "graphic_eq"
        | Android.Grid3x3 -> "grid_3x3"
        | Android.Grid3x3Off -> "grid_3x3_off"
        | Android.Grid4x4 -> "grid_4x4"
        | Android.GridGoldenratio -> "grid_goldenratio"
        | Android.HMobiledata -> "h_mobiledata"
        | Android.HMobiledataBadge -> "h_mobiledata_badge"
        | Android.HPlusMobiledata -> "h_plus_mobiledata"
        | Android.HPlusMobiledataBadge -> "h_plus_mobiledata_badge"
        | Android.Ios -> "ios"
        | Android.KeyboardCapslockBadge -> "keyboard_capslock_badge"
        | Android.KeyboardExternalInput -> "keyboard_external_input"
        | Android.KeyboardFull -> "keyboard_full"
        | Android.KeyboardKeys -> "keyboard_keys"
        | Android.KeyboardOff -> "keyboard_off"
        | Android.KeyboardOnscreen -> "keyboard_onscreen"
        | Android.KeyboardPreviousLanguage -> "keyboard_previous_language"
        | Android.LightMode -> "light_mode"
        | Android.LteMobiledata -> "lte_mobiledata"
        | Android.LteMobiledataBadge -> "lte_mobiledata_badge"
        | Android.LtePlusMobiledata -> "lte_plus_mobiledata"
        | Android.LtePlusMobiledataBadge -> "lte_plus_mobiledata_badge"
        | Android.MagnifyDocked -> "magnify_docked"
        | Android.MagnifyFullscreen -> "magnify_fullscreen"
        | Android.MediaBluetoothOff -> "media_bluetooth_off"
        | Android.MediaBluetoothOn -> "media_bluetooth_on"
        | Android.MobileSensorHi -> "mobile_sensor_hi"
        | Android.MobileSensorLo -> "mobile_sensor_lo"
        | Android.MobileWrench -> "mobile_wrench"
        | Android.MobiledataOff -> "mobiledata_off"
        | Android.ModeStandby -> "mode_standby"
        | Android.Nearby -> "nearby"
        | Android.NearbyError -> "nearby_error"
        | Android.NearbyOff -> "nearby_off"
        | Android.NetworkCell -> "network_cell"
        | Android.NetworkCheck -> "network_check"
        | Android.NetworkLocked -> "network_locked"
        | Android.NetworkPing -> "network_ping"
        | Android.NetworkWifi -> "network_wifi"
        | Android.NetworkWifi1Bar -> "network_wifi_1_bar"
        | Android.NetworkWifi1BarLocked -> "network_wifi_1_bar_locked"
        | Android.NetworkWifi2Bar -> "network_wifi_2_bar"
        | Android.NetworkWifi2BarLocked -> "network_wifi_2_bar_locked"
        | Android.NetworkWifi3Bar -> "network_wifi_3_bar"
        | Android.NetworkWifi3BarLocked -> "network_wifi_3_bar_locked"
        | Android.NetworkWifiLocked -> "network_wifi_locked"
        | Android.Nfc -> "nfc"
        | Android.NfcOff -> "nfc_off"
        | Android.Nightlight -> "nightlight"
        | Android.NoiseAware -> "noise_aware"
        | Android.NoiseControlOff -> "noise_control_off"
        | Android.NoiseControlOn -> "noise_control_on"
        | Android.OverviewKey -> "overview_key"
        | Android.Password -> "password"
        | Android.Password2 -> "password_2"
        | Android.Password2Off -> "password_2_off"
        | Android.Pattern -> "pattern"
        | Android.PermDataSetting -> "perm_data_setting"
        | Android.PermScanWifi -> "perm_scan_wifi"
        | Android.Pin -> "pin"
        | Android.PortableWifiOff -> "portable_wifi_off"
        | Android.QuickPhrases -> "quick_phrases"
        | Android.RMobiledata -> "r_mobiledata"
        | Android.Radar -> "radar"
        | Android.RssFeed -> "rss_feed"
        | Android.ScreenRecord -> "screen_record"
        | Android.ScreenRotationAlt -> "screen_rotation_alt"
        | Android.ScreenRotationUp -> "screen_rotation_up"
        | Android.ScreenshotFrame -> "screenshot_frame"
        | Android.ScreenshotFrame2 -> "screenshot_frame_2"
        | Android.ScreenshotKeyboard -> "screenshot_keyboard"
        | Android.ScreenshotRegion -> "screenshot_region"
        | Android.SettingsSystemDaydream -> "settings_system_daydream"
        | Android.SignalCellular0Bar -> "signal_cellular_0_bar"
        | Android.SignalCellular1Bar -> "signal_cellular_1_bar"
        | Android.SignalCellular2Bar -> "signal_cellular_2_bar"
        | Android.SignalCellular3Bar -> "signal_cellular_3_bar"
        | Android.SignalCellular4Bar -> "signal_cellular_4_bar"
        | Android.SignalCellularAlt -> "signal_cellular_alt"
        | Android.SignalCellularAlt1Bar -> "signal_cellular_alt_1_bar"
        | Android.SignalCellularAlt2Bar -> "signal_cellular_alt_2_bar"
        | Android.SignalCellularConnectedNoInternet0Bar -> "signal_cellular_connected_no_internet_0_bar"
        | Android.SignalCellularConnectedNoInternet4Bar -> "signal_cellular_connected_no_internet_4_bar"
        | Android.SignalCellularNodata -> "signal_cellular_nodata"
        | Android.SignalCellularNull -> "signal_cellular_null"
        | Android.SignalCellularOff -> "signal_cellular_off"
        | Android.SignalCellularPause -> "signal_cellular_pause"
        | Android.SignalDisconnected -> "signal_disconnected"
        | Android.SignalWifi0Bar -> "signal_wifi_0_bar"
        | Android.SignalWifi4Bar -> "signal_wifi_4_bar"
        | Android.SignalWifiBad -> "signal_wifi_bad"
        | Android.SignalWifiOff -> "signal_wifi_off"
        | Android.SignalWifiStatusbarNotConnected -> "signal_wifi_statusbar_not_connected"
        | Android.SignalWifiStatusbarNull -> "signal_wifi_statusbar_null"
        | Android.SimCardDownload -> "sim_card_download"
        | Android.Splitscreen -> "splitscreen"
        | Android.SplitscreenAdd -> "splitscreen_add"
        | Android.SplitscreenBottom -> "splitscreen_bottom"
        | Android.SplitscreenLeft -> "splitscreen_left"
        | Android.SplitscreenRight -> "splitscreen_right"
        | Android.SplitscreenTop -> "splitscreen_top"
        | Android.SplitscreenVerticalAdd -> "splitscreen_vertical_add"
        | Android.Storage -> "storage"
        | Android.Stylus -> "stylus"
        | Android.StylusNote -> "stylus_note"
        | Android.Thermostat -> "thermostat"
        | Android.Timer10Select -> "timer_10_select"
        | Android.Timer3Select -> "timer_3_select"
        | Android.Timer5 -> "timer_5"
        | Android.Timer5Shutter -> "timer_5_shutter"
        | Android.Usb -> "usb"
        | Android.UsbOff -> "usb_off"
        | Android.Wallpaper -> "wallpaper"
        | Android.WallpaperSlideshow -> "wallpaper_slideshow"
        | Android.Widgets -> "widgets"
        | Android.Wifi -> "wifi"
        | Android.Wifi1Bar -> "wifi_1_bar"
        | Android.Wifi2Bar -> "wifi_2_bar"
        | Android.WifiCallingBar1 -> "wifi_calling_bar_1"
        | Android.WifiCallingBar2 -> "wifi_calling_bar_2"
        | Android.WifiCallingBar3 -> "wifi_calling_bar_3"
        | Android.WifiFind -> "wifi_find"
        | Android.WifiHome -> "wifi_home"
        | Android.WifiLock -> "wifi_lock"
        | Android.WifiNotification -> "wifi_notification"
        | Android.WifiOff -> "wifi_off"
        | Android.WifiTethering -> "wifi_tethering"
        | Android.WifiTetheringError -> "wifi_tethering_error"
        | Android.WifiTetheringOff -> "wifi_tethering_off"

    [<RequireQualifiedAccess; Struct>]
    type AudioVideo =
      | ``10k``
      | ``1k``
      | ``1kPlus``
      | ``2d``
      | ``2k``
      | ``2kPlus``
      | ``30fps``
      | ``3d``
      | ``3k``
      | ``3kPlus``
      | ``4k``
      | ``4kPlus``
      | ``5k``
      | ``5kPlus``
      | ``60fps``
      | ``6k``
      | ``6kPlus``
      | ``7k``
      | ``7kPlus``
      | ``8k``
      | ``8kPlus``
      | ``9k``
      | ``9kPlus``
      | AdaptiveAudioMic
      | AdaptiveAudioMicOff
      | AddToQueue
      | Airplay
      | Album
      | AnimatedImages
      | ArOnYou
      | ArStickers
      | ArtTrack
      | Artist
      | AudioDescription
      | AudioFile
      | Autopause
      | Autoplay
      | Autostop
      | AvTimer
      | Av1
      | Avc
      | BrandAwareness
      | BrandingWatermark
      | BroadcastOnHome
      | BroadcastOnPersonal
      | CallToAction
      | CinematicBlur
      | ClosedCaption
      | ClosedCaptionAdd
      | ClosedCaptionDisabled
      | ControlCamera
      | DigitalOutOfHome
      | DiscoverTune
      | EarSound
      | EditAudio
      | Equalizer
      | Explicit
      | EyeTracking
      | FastForward
      | FastRewind
      | FeaturedPlayList
      | FeaturedVideo
      | FiberDvr
      | FiberManualRecord
      | FiberNew
      | FiberPin
      | FiberSmartRecord
      | Forward10
      | Forward30
      | Forward5
      | ForwardCircle
      | ForwardMedia
      | FramePerson
      | FramePersonMic
      | FramePersonOff
      | FullHd
      | Genres
      | HangoutVideo
      | HangoutVideoOff
      | Hd
      | Hearing
      | HearingAid
      | HearingAidDisabled
      | HearingAidDisabledLeft
      | HearingAidLeft
      | HearingDisabled
      | HighQuality
      | InstantMix
      | InterpreterMode
      | LibraryAddCheck
      | LibraryBooks
      | LibraryMusic
      | Lyrics
      | MediaLink
      | Mic
      | MicAlert
      | MicDouble
      | MicGear
      | MicOff
      | MissedVideoCall
      | Movie
      | MovieEdit
      | MovieInfo
      | MovieOff
      | MovieSpeaker
      | MusicCast
      | MusicHistory
      | MusicNote
      | MusicNote2
      | MusicNoteAdd
      | MusicOff
      | MusicVideo
      | NoSound
      | NotStarted
      | Pause
      | PauseCircle
      | PlayArrow
      | PlayCircle
      | PlayDisabled
      | PlayLesson
      | PlayPause
      | PlaylistAdd
      | PlaylistAddCheck
      | PlaylistAddCheckCircle
      | PlaylistAddCircle
      | PlaylistPlay
      | PlaylistRemove
      | Podcasts
      | Privacy
      | QueueMusic
      | QueuePlayNext
      | Radio
      | RecentActors
      | RemoveFromQueue
      | Repeat
      | RepeatOn
      | RepeatOne
      | RepeatOneOn
      | ReplaceAudio
      | ReplaceImage
      | ReplaceVideo
      | Replay
      | Replay10
      | Replay30
      | Replay5
      | Resume
      | Sd
      | SelectToSpeak
      | SettingsVoice
      | Shuffle
      | ShuffleOn
      | SkipNext
      | SkipPrevious
      | SlowMotionVideo
      | SoundDetectionDogBarking
      | SoundDetectionGlassBreak
      | SoundDetectionLoudSound
      | SoundSampler
      | SpatialAudio
      | SpatialAudioOff
      | SpatialSpeaker
      | SpatialTracking
      | SpeechToText
      | Speed
      | Speed025
      | Speed02x
      | Speed05
      | Speed05x
      | Speed075
      | Speed07x
      | Speed12
      | Speed125
      | Speed12x
      | Speed15
      | Speed15x
      | Speed175
      | Speed17x
      | Speed2x
      | SplitScene
      | SplitSceneDown
      | SplitSceneLeft
      | SplitSceneRight
      | SplitSceneUp
      | Stop
      | StopCircle
      | Stream
      | Subscriptions
      | Subtitles
      | SubtitlesGear
      | SurroundSound
      | TextToSpeech
      | VideoCall
      | VideoCameraBack
      | VideoCameraBackAdd
      | VideoCameraFront
      | VideoCameraFrontOff
      | VideoLabel
      | VideoLibrary
      | VideoSearch
      | VideoSettings
      | VideoStable
      | VideoTemplate
      | Videocam
      | VideocamAlert
      | VideocamOff
      | ViewInAr
      | ViewInArOff
      | VoiceSelection
      | VoiceSelectionOff
      | VolumeDown
      | VolumeMute
      | VolumeOff
      | VolumeUp

    module AudioVideo =

      let toSnakeCase (icon: AudioVideo) =
        match icon with
        | AudioVideo.``10k`` -> "10k"
        | AudioVideo.``1k`` -> "1k"
        | AudioVideo.``1kPlus`` -> "1k_plus"
        | AudioVideo.``2d`` -> "2d"
        | AudioVideo.``2k`` -> "2k"
        | AudioVideo.``2kPlus`` -> "2k_plus"
        | AudioVideo.``30fps`` -> "30fps"
        | AudioVideo.``3d`` -> "3d"
        | AudioVideo.``3k`` -> "3k"
        | AudioVideo.``3kPlus`` -> "3k_plus"
        | AudioVideo.``4k`` -> "4k"
        | AudioVideo.``4kPlus`` -> "4k_plus"
        | AudioVideo.``5k`` -> "5k"
        | AudioVideo.``5kPlus`` -> "5k_plus"
        | AudioVideo.``60fps`` -> "60fps"
        | AudioVideo.``6k`` -> "6k"
        | AudioVideo.``6kPlus`` -> "6k_plus"
        | AudioVideo.``7k`` -> "7k"
        | AudioVideo.``7kPlus`` -> "7k_plus"
        | AudioVideo.``8k`` -> "8k"
        | AudioVideo.``8kPlus`` -> "8k_plus"
        | AudioVideo.``9k`` -> "9k"
        | AudioVideo.``9kPlus`` -> "9k_plus"
        | AudioVideo.AdaptiveAudioMic -> "adaptive_audio_mic"
        | AudioVideo.AdaptiveAudioMicOff -> "adaptive_audio_mic_off"
        | AudioVideo.AddToQueue -> "add_to_queue"
        | AudioVideo.Airplay -> "airplay"
        | AudioVideo.Album -> "album"
        | AudioVideo.AnimatedImages -> "animated_images"
        | AudioVideo.ArOnYou -> "ar_on_you"
        | AudioVideo.ArStickers -> "ar_stickers"
        | AudioVideo.ArtTrack -> "art_track"
        | AudioVideo.Artist -> "artist"
        | AudioVideo.AudioDescription -> "audio_description"
        | AudioVideo.AudioFile -> "audio_file"
        | AudioVideo.Autopause -> "autopause"
        | AudioVideo.Autoplay -> "autoplay"
        | AudioVideo.Autostop -> "autostop"
        | AudioVideo.AvTimer -> "av_timer"
        | AudioVideo.Av1 -> "av1"
        | AudioVideo.Avc -> "avc"
        | AudioVideo.BrandAwareness -> "brand_awareness"
        | AudioVideo.BrandingWatermark -> "branding_watermark"
        | AudioVideo.BroadcastOnHome -> "broadcast_on_home"
        | AudioVideo.BroadcastOnPersonal -> "broadcast_on_personal"
        | AudioVideo.CallToAction -> "call_to_action"
        | AudioVideo.CinematicBlur -> "cinematic_blur"
        | AudioVideo.ClosedCaption -> "closed_caption"
        | AudioVideo.ClosedCaptionAdd -> "closed_caption_add"
        | AudioVideo.ClosedCaptionDisabled -> "closed_caption_disabled"
        | AudioVideo.ControlCamera -> "control_camera"
        | AudioVideo.DigitalOutOfHome -> "digital_out_of_home"
        | AudioVideo.DiscoverTune -> "discover_tune"
        | AudioVideo.EarSound -> "ear_sound"
        | AudioVideo.EditAudio -> "edit_audio"
        | AudioVideo.Equalizer -> "equalizer"
        | AudioVideo.Explicit -> "explicit"
        | AudioVideo.EyeTracking -> "eye_tracking"
        | AudioVideo.FastForward -> "fast_forward"
        | AudioVideo.FastRewind -> "fast_rewind"
        | AudioVideo.FeaturedPlayList -> "featured_play_list"
        | AudioVideo.FeaturedVideo -> "featured_video"
        | AudioVideo.FiberDvr -> "fiber_dvr"
        | AudioVideo.FiberManualRecord -> "fiber_manual_record"
        | AudioVideo.FiberNew -> "fiber_new"
        | AudioVideo.FiberPin -> "fiber_pin"
        | AudioVideo.FiberSmartRecord -> "fiber_smart_record"
        | AudioVideo.Forward10 -> "forward_10"
        | AudioVideo.Forward30 -> "forward_30"
        | AudioVideo.Forward5 -> "forward_5"
        | AudioVideo.ForwardCircle -> "forward_circle"
        | AudioVideo.ForwardMedia -> "forward_media"
        | AudioVideo.FramePerson -> "frame_person"
        | AudioVideo.FramePersonMic -> "frame_person_mic"
        | AudioVideo.FramePersonOff -> "frame_person_off"
        | AudioVideo.FullHd -> "full_hd"
        | AudioVideo.Genres -> "genres"
        | AudioVideo.HangoutVideo -> "hangout_video"
        | AudioVideo.HangoutVideoOff -> "hangout_video_off"
        | AudioVideo.Hd -> "hd"
        | AudioVideo.Hearing -> "hearing"
        | AudioVideo.HearingAid -> "hearing_aid"
        | AudioVideo.HearingAidDisabled -> "hearing_aid_disabled"
        | AudioVideo.HearingAidDisabledLeft -> "hearing_aid_disabled_left"
        | AudioVideo.HearingAidLeft -> "hearing_aid_left"
        | AudioVideo.HearingDisabled -> "hearing_disabled"
        | AudioVideo.HighQuality -> "high_quality"
        | AudioVideo.InstantMix -> "instant_mix"
        | AudioVideo.InterpreterMode -> "interpreter_mode"
        | AudioVideo.LibraryAddCheck -> "library_add_check"
        | AudioVideo.LibraryBooks -> "library_books"
        | AudioVideo.LibraryMusic -> "library_music"
        | AudioVideo.Lyrics -> "lyrics"
        | AudioVideo.MediaLink -> "media_link"
        | AudioVideo.Mic -> "mic"
        | AudioVideo.MicAlert -> "mic_alert"
        | AudioVideo.MicDouble -> "mic_double"
        | AudioVideo.MicGear -> "mic_gear"
        | AudioVideo.MicOff -> "mic_off"
        | AudioVideo.MissedVideoCall -> "missed_video_call"
        | AudioVideo.Movie -> "movie"
        | AudioVideo.MovieEdit -> "movie_edit"
        | AudioVideo.MovieInfo -> "movie_info"
        | AudioVideo.MovieOff -> "movie_off"
        | AudioVideo.MovieSpeaker -> "movie_speaker"
        | AudioVideo.MusicCast -> "music_cast"
        | AudioVideo.MusicHistory -> "music_history"
        | AudioVideo.MusicNote -> "music_note"
        | AudioVideo.MusicNote2 -> "music_note_2"
        | AudioVideo.MusicNoteAdd -> "music_note_add"
        | AudioVideo.MusicOff -> "music_off"
        | AudioVideo.MusicVideo -> "music_video"
        | AudioVideo.NoSound -> "no_sound"
        | AudioVideo.NotStarted -> "not_started"
        | AudioVideo.Pause -> "pause"
        | AudioVideo.PauseCircle -> "pause_circle"
        | AudioVideo.PlayArrow -> "play_arrow"
        | AudioVideo.PlayCircle -> "play_circle"
        | AudioVideo.PlayDisabled -> "play_disabled"
        | AudioVideo.PlayLesson -> "play_lesson"
        | AudioVideo.PlayPause -> "play_pause"
        | AudioVideo.PlaylistAdd -> "playlist_add"
        | AudioVideo.PlaylistAddCheck -> "playlist_add_check"
        | AudioVideo.PlaylistAddCheckCircle -> "playlist_add_check_circle"
        | AudioVideo.PlaylistAddCircle -> "playlist_add_circle"
        | AudioVideo.PlaylistPlay -> "playlist_play"
        | AudioVideo.PlaylistRemove -> "playlist_remove"
        | AudioVideo.Podcasts -> "podcasts"
        | AudioVideo.Privacy -> "privacy"
        | AudioVideo.QueueMusic -> "queue_music"
        | AudioVideo.QueuePlayNext -> "queue_play_next"
        | AudioVideo.Radio -> "radio"
        | AudioVideo.RecentActors -> "recent_actors"
        | AudioVideo.RemoveFromQueue -> "remove_from_queue"
        | AudioVideo.Repeat -> "repeat"
        | AudioVideo.RepeatOn -> "repeat_on"
        | AudioVideo.RepeatOne -> "repeat_one"
        | AudioVideo.RepeatOneOn -> "repeat_one_on"
        | AudioVideo.ReplaceAudio -> "replace_audio"
        | AudioVideo.ReplaceImage -> "replace_image"
        | AudioVideo.ReplaceVideo -> "replace_video"
        | AudioVideo.Replay -> "replay"
        | AudioVideo.Replay10 -> "replay_10"
        | AudioVideo.Replay30 -> "replay_30"
        | AudioVideo.Replay5 -> "replay_5"
        | AudioVideo.Resume -> "resume"
        | AudioVideo.Sd -> "sd"
        | AudioVideo.SelectToSpeak -> "select_to_speak"
        | AudioVideo.SettingsVoice -> "settings_voice"
        | AudioVideo.Shuffle -> "shuffle"
        | AudioVideo.ShuffleOn -> "shuffle_on"
        | AudioVideo.SkipNext -> "skip_next"
        | AudioVideo.SkipPrevious -> "skip_previous"
        | AudioVideo.SlowMotionVideo -> "slow_motion_video"
        | AudioVideo.SoundDetectionDogBarking -> "sound_detection_dog_barking"
        | AudioVideo.SoundDetectionGlassBreak -> "sound_detection_glass_break"
        | AudioVideo.SoundDetectionLoudSound -> "sound_detection_loud_sound"
        | AudioVideo.SoundSampler -> "sound_sampler"
        | AudioVideo.SpatialAudio -> "spatial_audio"
        | AudioVideo.SpatialAudioOff -> "spatial_audio_off"
        | AudioVideo.SpatialSpeaker -> "spatial_speaker"
        | AudioVideo.SpatialTracking -> "spatial_tracking"
        | AudioVideo.SpeechToText -> "speech_to_text"
        | AudioVideo.Speed -> "speed"
        | AudioVideo.Speed025 -> "speed_025"
        | AudioVideo.Speed02x -> "speed_02x"
        | AudioVideo.Speed05 -> "speed_05"
        | AudioVideo.Speed05x -> "speed_05x"
        | AudioVideo.Speed075 -> "speed_075"
        | AudioVideo.Speed07x -> "speed_07x"
        | AudioVideo.Speed12 -> "speed_12"
        | AudioVideo.Speed125 -> "speed_125"
        | AudioVideo.Speed12x -> "speed_12x"
        | AudioVideo.Speed15 -> "speed_15"
        | AudioVideo.Speed15x -> "speed_15x"
        | AudioVideo.Speed175 -> "speed_175"
        | AudioVideo.Speed17x -> "speed_17x"
        | AudioVideo.Speed2x -> "speed_2x"
        | AudioVideo.SplitScene -> "split_scene"
        | AudioVideo.SplitSceneDown -> "split_scene_down"
        | AudioVideo.SplitSceneLeft -> "split_scene_left"
        | AudioVideo.SplitSceneRight -> "split_scene_right"
        | AudioVideo.SplitSceneUp -> "split_scene_up"
        | AudioVideo.Stop -> "stop"
        | AudioVideo.StopCircle -> "stop_circle"
        | AudioVideo.Stream -> "stream"
        | AudioVideo.Subscriptions -> "subscriptions"
        | AudioVideo.Subtitles -> "subtitles"
        | AudioVideo.SubtitlesGear -> "subtitles_gear"
        | AudioVideo.SurroundSound -> "surround_sound"
        | AudioVideo.TextToSpeech -> "text_to_speech"
        | AudioVideo.VideoCall -> "video_call"
        | AudioVideo.VideoCameraBack -> "video_camera_back"
        | AudioVideo.VideoCameraBackAdd -> "video_camera_back_add"
        | AudioVideo.VideoCameraFront -> "video_camera_front"
        | AudioVideo.VideoCameraFrontOff -> "video_camera_front_off"
        | AudioVideo.VideoLabel -> "video_label"
        | AudioVideo.VideoLibrary -> "video_library"
        | AudioVideo.VideoSearch -> "video_search"
        | AudioVideo.VideoSettings -> "video_settings"
        | AudioVideo.VideoStable -> "video_stable"
        | AudioVideo.VideoTemplate -> "video_template"
        | AudioVideo.Videocam -> "videocam"
        | AudioVideo.VideocamAlert -> "videocam_alert"
        | AudioVideo.VideocamOff -> "videocam_off"
        | AudioVideo.ViewInAr -> "view_in_ar"
        | AudioVideo.ViewInArOff -> "view_in_ar_off"
        | AudioVideo.VoiceSelection -> "voice_selection"
        | AudioVideo.VoiceSelectionOff -> "voice_selection_off"
        | AudioVideo.VolumeDown -> "volume_down"
        | AudioVideo.VolumeMute -> "volume_mute"
        | AudioVideo.VolumeOff -> "volume_off"
        | AudioVideo.VolumeUp -> "volume_up"

    [<RequireQualifiedAccess; Struct>]
    type Business =
      | AccountBalance
      | AccountBalanceWallet
      | AccountTree
      | AddBusiness
      | AddCard
      | AddChart
      | AddShoppingCart
      | Analytics
      | AreaChart
      | Atm
      | Atr
      | AttachMoney
      | BarChart
      | BarChart4Bars
      | BarChartOff
      | Barcode
      | BarcodeReader
      | BarcodeScanner
      | BidLandscape
      | BidLandscapeDisabled
      | Box
      | BoxAdd
      | BoxEdit
      | BriefcaseMeal
      | BubbleChart
      | Calculate
      | CandlestickChart
      | CardMembership
      | CardTravel
      | CardsStar
      | Cases
      | ChartData
      | Checkbook
      | Contactless
      | ContactlessOff
      | ConversionPath
      | ConversionPathOff
      | ConveyorBelt
      | Copyright
      | CorporateFare
      | CreditCard
      | CreditCardClock
      | CreditCardGear
      | CreditCardHeart
      | CreditCardOff
      | CreditScore
      | CurrencyBitcoin
      | CurrencyExchange
      | CurrencyFranc
      | CurrencyLira
      | CurrencyPound
      | CurrencyRuble
      | CurrencyRupee
      | CurrencyRupeeCircle
      | CurrencyYen
      | CurrencyYuan
      | DataExploration
      | DataTable
      | Database
      | DatabaseOff
      | DatabaseSearch
      | DatabaseUpload
      | DeliveryTruckBolt
      | DeliveryTruckSpeed
      | Domain
      | DomainAdd
      | DomainDisabled
      | DonutLarge
      | DonutSmall
      | Energy
      | Enterprise
      | EnterpriseOff
      | Euro
      | EuroSymbol
      | FamilyHistory
      | Finance
      | FinanceMode
      | Flowchart
      | Flowsheet
      | Forklift
      | FrontLoader
      | FullStackedBarChart
      | Graph1
      | Graph2
      | Graph3
      | Graph4
      | Graph5
      | Graph6
      | Graph7
      | Graph8
      | GroupedBarChart
      | InactiveOrder
      | InsertChart
      | Leaderboard
      | LegendToggle
      | Loyalty
      | Mediation
      | MeetingRoom
      | Mintmark
      | Mitre
      | Money
      | MoneyBag
      | MoneyOff
      | MoneyRange
      | Monitoring
      | MultilineChart
      | NetworkNode
      | NextWeek
      | NoMeetingRoom
      | OrderApprove
      | OrderPlay
      | Orders
      | Paid
      | Pallet
      | PaymentArrowDown
      | PaymentCard
      | Payments
      | PercentDiscount
      | PieChart
      | PlannerReview
      | Podium
      | PrecisionManufacturing
      | PriceChange
      | PriceCheck
      | ProductionQuantityLimits
      | QrCode
      | QrCode2
      | QrCode2Add
      | QrCodeScanner
      | QueryStats
      | QuickReorder
      | Receipt
      | ReceiptLong
      | ReceiptLongOff
      | Redeem
      | RemoveShoppingCart
      | RoomPreferences
      | Savings
      | ScatterPlot
      | Schema
      | SearchInsights
      | Sell
      | SendMoney
      | Shop
      | ShopTwo
      | ShoppingBag
      | ShoppingBagSpeed
      | ShoppingBasket
      | ShoppingCart
      | ShoppingCartOff
      | Shoppingmode
      | ShowChart
      | SourceEnvironment
      | SsidChart
      | StackedBarChart
      | StackedLineChart
      | Store
      | Storefront
      | StrikethroughS
      | Tenancy
      | Timeline
      | Toll
      | TrackChanges
      | TrendingDown
      | TrendingFlat
      | TrendingUp
      | Trolley
      | Troubleshoot
      | UniversalCurrency
      | UniversalCurrencyAlt
      | UpiPay
      | Wallet
      | WaterfallChart
      | Work
      | WorkAlert
      | WorkHistory
      | WorkUpdate

    module Business =

      let toSnakeCase (icon: Business) =
        match icon with
        | Business.AccountBalance -> "account_balance"
        | Business.AccountBalanceWallet -> "account_balance_wallet"
        | Business.AccountTree -> "account_tree"
        | Business.AddBusiness -> "add_business"
        | Business.AddCard -> "add_card"
        | Business.AddChart -> "add_chart"
        | Business.AddShoppingCart -> "add_shopping_cart"
        | Business.Analytics -> "analytics"
        | Business.AreaChart -> "area_chart"
        | Business.Atm -> "atm"
        | Business.Atr -> "atr"
        | Business.AttachMoney -> "attach_money"
        | Business.BarChart -> "bar_chart"
        | Business.BarChart4Bars -> "bar_chart_4_bars"
        | Business.BarChartOff -> "bar_chart_off"
        | Business.Barcode -> "barcode"
        | Business.BarcodeReader -> "barcode_reader"
        | Business.BarcodeScanner -> "barcode_scanner"
        | Business.BidLandscape -> "bid_landscape"
        | Business.BidLandscapeDisabled -> "bid_landscape_disabled"
        | Business.Box -> "box"
        | Business.BoxAdd -> "box_add"
        | Business.BoxEdit -> "box_edit"
        | Business.BriefcaseMeal -> "briefcase_meal"
        | Business.BubbleChart -> "bubble_chart"
        | Business.Calculate -> "calculate"
        | Business.CandlestickChart -> "candlestick_chart"
        | Business.CardMembership -> "card_membership"
        | Business.CardTravel -> "card_travel"
        | Business.CardsStar -> "cards_star"
        | Business.Cases -> "cases"
        | Business.ChartData -> "chart_data"
        | Business.Checkbook -> "checkbook"
        | Business.Contactless -> "contactless"
        | Business.ContactlessOff -> "contactless_off"
        | Business.ConversionPath -> "conversion_path"
        | Business.ConversionPathOff -> "conversion_path_off"
        | Business.ConveyorBelt -> "conveyor_belt"
        | Business.Copyright -> "copyright"
        | Business.CorporateFare -> "corporate_fare"
        | Business.CreditCard -> "credit_card"
        | Business.CreditCardClock -> "credit_card_clock"
        | Business.CreditCardGear -> "credit_card_gear"
        | Business.CreditCardHeart -> "credit_card_heart"
        | Business.CreditCardOff -> "credit_card_off"
        | Business.CreditScore -> "credit_score"
        | Business.CurrencyBitcoin -> "currency_bitcoin"
        | Business.CurrencyExchange -> "currency_exchange"
        | Business.CurrencyFranc -> "currency_franc"
        | Business.CurrencyLira -> "currency_lira"
        | Business.CurrencyPound -> "currency_pound"
        | Business.CurrencyRuble -> "currency_ruble"
        | Business.CurrencyRupee -> "currency_rupee"
        | Business.CurrencyRupeeCircle -> "currency_rupee_circle"
        | Business.CurrencyYen -> "currency_yen"
        | Business.CurrencyYuan -> "currency_yuan"
        | Business.DataExploration -> "data_exploration"
        | Business.DataTable -> "data_table"
        | Business.Database -> "database"
        | Business.DatabaseOff -> "database_off"
        | Business.DatabaseSearch -> "database_search"
        | Business.DatabaseUpload -> "database_upload"
        | Business.DeliveryTruckBolt -> "delivery_truck_bolt"
        | Business.DeliveryTruckSpeed -> "delivery_truck_speed"
        | Business.Domain -> "domain"
        | Business.DomainAdd -> "domain_add"
        | Business.DomainDisabled -> "domain_disabled"
        | Business.DonutLarge -> "donut_large"
        | Business.DonutSmall -> "donut_small"
        | Business.Energy -> "energy"
        | Business.Enterprise -> "enterprise"
        | Business.EnterpriseOff -> "enterprise_off"
        | Business.Euro -> "euro"
        | Business.EuroSymbol -> "euro_symbol"
        | Business.FamilyHistory -> "family_history"
        | Business.Finance -> "finance"
        | Business.FinanceMode -> "finance_mode"
        | Business.Flowchart -> "flowchart"
        | Business.Flowsheet -> "flowsheet"
        | Business.Forklift -> "forklift"
        | Business.FrontLoader -> "front_loader"
        | Business.FullStackedBarChart -> "full_stacked_bar_chart"
        | Business.Graph1 -> "graph_1"
        | Business.Graph2 -> "graph_2"
        | Business.Graph3 -> "graph_3"
        | Business.Graph4 -> "graph_4"
        | Business.Graph5 -> "graph_5"
        | Business.Graph6 -> "graph_6"
        | Business.Graph7 -> "graph_7"
        | Business.Graph8 -> "graph_8"
        | Business.GroupedBarChart -> "grouped_bar_chart"
        | Business.InactiveOrder -> "inactive_order"
        | Business.InsertChart -> "insert_chart"
        | Business.Leaderboard -> "leaderboard"
        | Business.LegendToggle -> "legend_toggle"
        | Business.Loyalty -> "loyalty"
        | Business.Mediation -> "mediation"
        | Business.MeetingRoom -> "meeting_room"
        | Business.Mintmark -> "mintmark"
        | Business.Mitre -> "mitre"
        | Business.Money -> "money"
        | Business.MoneyBag -> "money_bag"
        | Business.MoneyOff -> "money_off"
        | Business.MoneyRange -> "money_range"
        | Business.Monitoring -> "monitoring"
        | Business.MultilineChart -> "multiline_chart"
        | Business.NetworkNode -> "network_node"
        | Business.NextWeek -> "next_week"
        | Business.NoMeetingRoom -> "no_meeting_room"
        | Business.OrderApprove -> "order_approve"
        | Business.OrderPlay -> "order_play"
        | Business.Orders -> "orders"
        | Business.Paid -> "paid"
        | Business.Pallet -> "pallet"
        | Business.PaymentArrowDown -> "payment_arrow_down"
        | Business.PaymentCard -> "payment_card"
        | Business.Payments -> "payments"
        | Business.PercentDiscount -> "percent_discount"
        | Business.PieChart -> "pie_chart"
        | Business.PlannerReview -> "planner_review"
        | Business.Podium -> "podium"
        | Business.PrecisionManufacturing -> "precision_manufacturing"
        | Business.PriceChange -> "price_change"
        | Business.PriceCheck -> "price_check"
        | Business.ProductionQuantityLimits -> "production_quantity_limits"
        | Business.QrCode -> "qr_code"
        | Business.QrCode2 -> "qr_code_2"
        | Business.QrCode2Add -> "qr_code_2_add"
        | Business.QrCodeScanner -> "qr_code_scanner"
        | Business.QueryStats -> "query_stats"
        | Business.QuickReorder -> "quick_reorder"
        | Business.Receipt -> "receipt"
        | Business.ReceiptLong -> "receipt_long"
        | Business.ReceiptLongOff -> "receipt_long_off"
        | Business.Redeem -> "redeem"
        | Business.RemoveShoppingCart -> "remove_shopping_cart"
        | Business.RoomPreferences -> "room_preferences"
        | Business.Savings -> "savings"
        | Business.ScatterPlot -> "scatter_plot"
        | Business.Schema -> "schema"
        | Business.SearchInsights -> "search_insights"
        | Business.Sell -> "sell"
        | Business.SendMoney -> "send_money"
        | Business.Shop -> "shop"
        | Business.ShopTwo -> "shop_two"
        | Business.ShoppingBag -> "shopping_bag"
        | Business.ShoppingBagSpeed -> "shopping_bag_speed"
        | Business.ShoppingBasket -> "shopping_basket"
        | Business.ShoppingCart -> "shopping_cart"
        | Business.ShoppingCartOff -> "shopping_cart_off"
        | Business.Shoppingmode -> "shoppingmode"
        | Business.ShowChart -> "show_chart"
        | Business.SourceEnvironment -> "source_environment"
        | Business.SsidChart -> "ssid_chart"
        | Business.StackedBarChart -> "stacked_bar_chart"
        | Business.StackedLineChart -> "stacked_line_chart"
        | Business.Store -> "store"
        | Business.Storefront -> "storefront"
        | Business.StrikethroughS -> "strikethrough_s"
        | Business.Tenancy -> "tenancy"
        | Business.Timeline -> "timeline"
        | Business.Toll -> "toll"
        | Business.TrackChanges -> "track_changes"
        | Business.TrendingDown -> "trending_down"
        | Business.TrendingFlat -> "trending_flat"
        | Business.TrendingUp -> "trending_up"
        | Business.Trolley -> "trolley"
        | Business.Troubleshoot -> "troubleshoot"
        | Business.UniversalCurrency -> "universal_currency"
        | Business.UniversalCurrencyAlt -> "universal_currency_alt"
        | Business.UpiPay -> "upi_pay"
        | Business.Wallet -> "wallet"
        | Business.WaterfallChart -> "waterfall_chart"
        | Business.Work -> "work"
        | Business.WorkAlert -> "work_alert"
        | Business.WorkHistory -> "work_history"
        | Business.WorkUpdate -> "work_update"

    [<RequireQualifiedAccess; Struct>]
    type Communicate =
      | ``3p``
      | AddCall
      | AddComment
      | AllInbox
      | AlternateEmail
      | AttachEmail
      | Attribution
      | AutoReadPause
      | AutoReadPlay
      | BusinessMessages
      | CalendarAddOn
      | CalendarAppsScript
      | Call
      | CallEnd
      | CallLog
      | CallMade
      | CallMerge
      | CallMissed
      | CallMissedOutgoing
      | CallQuality
      | CallReceived
      | CallSplit
      | CancelPresentation
      | CancelScheduleSend
      | CellTower
      | CellWifi
      | Chat
      | ChatAddOn
      | ChatAppsScript
      | ChatBubble
      | ChatDashed
      | ChatError
      | ChatInfo
      | ChatPasteGo
      | ChatPasteGo2
      | CoPresent
      | Comment
      | CommentBank
      | CommentsDisabled
      | ContactEmergency
      | ContactMail
      | ContactPhone
      | ContactSupport
      | Contacts
      | DialerSip
      | Dialpad
      | Drafts
      | Duo
      | E911Avatar
      | ForYou
      | Forum
      | ForwardToInbox
      | GTranslate
      | GroupSearch
      | HourglassBottom
      | HourglassTop
      | Hub
      | ImportContacts
      | Inbox
      | InboxCustomize
      | InboxText
      | InboxTextAsterisk
      | InboxTextPerson
      | InboxTextShare
      | Inventory2
      | Lan
      | Link
      | LinkOff
      | LiveHelp
      | Mail
      | MailAsterisk
      | MailLock
      | MailOff
      | MailShield
      | MarkAsUnread
      | MarkChatRead
      | MarkChatUnread
      | MarkEmailRead
      | MarkEmailUnread
      | MarkUnreadChatAlt
      | MarkunreadMailbox
      | Mms
      | MobileCancel
      | MobileSound
      | MobileSoundOff
      | ModeComment
      | MoveToInbox
      | Nat
      | NetworkIntelNode
      | NetworkIntelligence
      | NetworkIntelligenceHistory
      | NetworkIntelligenceUpdate
      | NetworkManage
      | NextPlan
      | NotificationAudio
      | NotificationAudioOff
      | NotificationMultiple
      | NotificationSettings
      | NotificationSound
      | Notifications
      | NotificationsActive
      | NotificationsOff
      | NotificationsPaused
      | NotificationsUnread
      | Ods
      | Odt
      | Outbox
      | OutboxAlt
      | OutgoingMail
      | PausePresentation
      | PermPhoneMsg
      | PersonSearch
      | PhoneBluetoothSpeaker
      | PhoneCallback
      | PhoneDisabled
      | PhoneEnabled
      | PhoneForwarded
      | PhoneInTalk
      | PhoneLocked
      | PhoneMissed
      | PhonePaused
      | PictureInPicture
      | PictureInPictureAlt
      | PictureInPictureCenter
      | PictureInPictureLarge
      | PictureInPictureMedium
      | PictureInPictureMobile
      | PictureInPictureOff
      | PictureInPictureSmall
      | PlayForWork
      | PresentToAll
      | Quickreply
      | Reviews
      | RingVolume
      | Rtt
      | SatelliteAlt
      | ScheduleSend
      | Score
      | Send
      | SendAndArchive
      | SettingsBluetooth
      | SettingsPhone
      | SignalCellularAdd
      | Sip
      | Sms
      | SpeakerNotes
      | SpeakerNotesOff
      | SpeakerPhone
      | Spoke
      | StackedEmail
      | StackedInbox
      | SwapCalls
      | ThreadUnread
      | ThreatIntelligence
      | Tooltip
      | Tooltip2
      | Topic
      | Unarchive
      | Unsubscribe
      | Upcoming
      | VideoChat
      | VoiceChat
      | VoiceChatOff
      | Voicemail
      | Voicemail2
      | WifiAdd
      | WifiCalling
      | WifiChannel
      | WifiProxy

    module Communicate =

      let toSnakeCase (icon: Communicate) =
        match icon with
        | Communicate.``3p`` -> "3p"
        | Communicate.AddCall -> "add_call"
        | Communicate.AddComment -> "add_comment"
        | Communicate.AllInbox -> "all_inbox"
        | Communicate.AlternateEmail -> "alternate_email"
        | Communicate.AttachEmail -> "attach_email"
        | Communicate.Attribution -> "attribution"
        | Communicate.AutoReadPause -> "auto_read_pause"
        | Communicate.AutoReadPlay -> "auto_read_play"
        | Communicate.BusinessMessages -> "business_messages"
        | Communicate.CalendarAddOn -> "calendar_add_on"
        | Communicate.CalendarAppsScript -> "calendar_apps_script"
        | Communicate.Call -> "call"
        | Communicate.CallEnd -> "call_end"
        | Communicate.CallLog -> "call_log"
        | Communicate.CallMade -> "call_made"
        | Communicate.CallMerge -> "call_merge"
        | Communicate.CallMissed -> "call_missed"
        | Communicate.CallMissedOutgoing -> "call_missed_outgoing"
        | Communicate.CallQuality -> "call_quality"
        | Communicate.CallReceived -> "call_received"
        | Communicate.CallSplit -> "call_split"
        | Communicate.CancelPresentation -> "cancel_presentation"
        | Communicate.CancelScheduleSend -> "cancel_schedule_send"
        | Communicate.CellTower -> "cell_tower"
        | Communicate.CellWifi -> "cell_wifi"
        | Communicate.Chat -> "chat"
        | Communicate.ChatAddOn -> "chat_add_on"
        | Communicate.ChatAppsScript -> "chat_apps_script"
        | Communicate.ChatBubble -> "chat_bubble"
        | Communicate.ChatDashed -> "chat_dashed"
        | Communicate.ChatError -> "chat_error"
        | Communicate.ChatInfo -> "chat_info"
        | Communicate.ChatPasteGo -> "chat_paste_go"
        | Communicate.ChatPasteGo2 -> "chat_paste_go_2"
        | Communicate.CoPresent -> "co_present"
        | Communicate.Comment -> "comment"
        | Communicate.CommentBank -> "comment_bank"
        | Communicate.CommentsDisabled -> "comments_disabled"
        | Communicate.ContactEmergency -> "contact_emergency"
        | Communicate.ContactMail -> "contact_mail"
        | Communicate.ContactPhone -> "contact_phone"
        | Communicate.ContactSupport -> "contact_support"
        | Communicate.Contacts -> "contacts"
        | Communicate.DialerSip -> "dialer_sip"
        | Communicate.Dialpad -> "dialpad"
        | Communicate.Drafts -> "drafts"
        | Communicate.Duo -> "duo"
        | Communicate.E911Avatar -> "e911_avatar"
        | Communicate.ForYou -> "for_you"
        | Communicate.Forum -> "forum"
        | Communicate.ForwardToInbox -> "forward_to_inbox"
        | Communicate.GTranslate -> "g_translate"
        | Communicate.GroupSearch -> "group_search"
        | Communicate.HourglassBottom -> "hourglass_bottom"
        | Communicate.HourglassTop -> "hourglass_top"
        | Communicate.Hub -> "hub"
        | Communicate.ImportContacts -> "import_contacts"
        | Communicate.Inbox -> "inbox"
        | Communicate.InboxCustomize -> "inbox_customize"
        | Communicate.InboxText -> "inbox_text"
        | Communicate.InboxTextAsterisk -> "inbox_text_asterisk"
        | Communicate.InboxTextPerson -> "inbox_text_person"
        | Communicate.InboxTextShare -> "inbox_text_share"
        | Communicate.Inventory2 -> "inventory_2"
        | Communicate.Lan -> "lan"
        | Communicate.Link -> "link"
        | Communicate.LinkOff -> "link_off"
        | Communicate.LiveHelp -> "live_help"
        | Communicate.Mail -> "mail"
        | Communicate.MailAsterisk -> "mail_asterisk"
        | Communicate.MailLock -> "mail_lock"
        | Communicate.MailOff -> "mail_off"
        | Communicate.MailShield -> "mail_shield"
        | Communicate.MarkAsUnread -> "mark_as_unread"
        | Communicate.MarkChatRead -> "mark_chat_read"
        | Communicate.MarkChatUnread -> "mark_chat_unread"
        | Communicate.MarkEmailRead -> "mark_email_read"
        | Communicate.MarkEmailUnread -> "mark_email_unread"
        | Communicate.MarkUnreadChatAlt -> "mark_unread_chat_alt"
        | Communicate.MarkunreadMailbox -> "markunread_mailbox"
        | Communicate.Mms -> "mms"
        | Communicate.MobileCancel -> "mobile_cancel"
        | Communicate.MobileSound -> "mobile_sound"
        | Communicate.MobileSoundOff -> "mobile_sound_off"
        | Communicate.ModeComment -> "mode_comment"
        | Communicate.MoveToInbox -> "move_to_inbox"
        | Communicate.Nat -> "nat"
        | Communicate.NetworkIntelNode -> "network_intel_node"
        | Communicate.NetworkIntelligence -> "network_intelligence"
        | Communicate.NetworkIntelligenceHistory -> "network_intelligence_history"
        | Communicate.NetworkIntelligenceUpdate -> "network_intelligence_update"
        | Communicate.NetworkManage -> "network_manage"
        | Communicate.NextPlan -> "next_plan"
        | Communicate.NotificationAudio -> "notification_audio"
        | Communicate.NotificationAudioOff -> "notification_audio_off"
        | Communicate.NotificationMultiple -> "notification_multiple"
        | Communicate.NotificationSettings -> "notification_settings"
        | Communicate.NotificationSound -> "notification_sound"
        | Communicate.Notifications -> "notifications"
        | Communicate.NotificationsActive -> "notifications_active"
        | Communicate.NotificationsOff -> "notifications_off"
        | Communicate.NotificationsPaused -> "notifications_paused"
        | Communicate.NotificationsUnread -> "notifications_unread"
        | Communicate.Ods -> "ods"
        | Communicate.Odt -> "odt"
        | Communicate.Outbox -> "outbox"
        | Communicate.OutboxAlt -> "outbox_alt"
        | Communicate.OutgoingMail -> "outgoing_mail"
        | Communicate.PausePresentation -> "pause_presentation"
        | Communicate.PermPhoneMsg -> "perm_phone_msg"
        | Communicate.PersonSearch -> "person_search"
        | Communicate.PhoneBluetoothSpeaker -> "phone_bluetooth_speaker"
        | Communicate.PhoneCallback -> "phone_callback"
        | Communicate.PhoneDisabled -> "phone_disabled"
        | Communicate.PhoneEnabled -> "phone_enabled"
        | Communicate.PhoneForwarded -> "phone_forwarded"
        | Communicate.PhoneInTalk -> "phone_in_talk"
        | Communicate.PhoneLocked -> "phone_locked"
        | Communicate.PhoneMissed -> "phone_missed"
        | Communicate.PhonePaused -> "phone_paused"
        | Communicate.PictureInPicture -> "picture_in_picture"
        | Communicate.PictureInPictureAlt -> "picture_in_picture_alt"
        | Communicate.PictureInPictureCenter -> "picture_in_picture_center"
        | Communicate.PictureInPictureLarge -> "picture_in_picture_large"
        | Communicate.PictureInPictureMedium -> "picture_in_picture_medium"
        | Communicate.PictureInPictureMobile -> "picture_in_picture_mobile"
        | Communicate.PictureInPictureOff -> "picture_in_picture_off"
        | Communicate.PictureInPictureSmall -> "picture_in_picture_small"
        | Communicate.PlayForWork -> "play_for_work"
        | Communicate.PresentToAll -> "present_to_all"
        | Communicate.Quickreply -> "quickreply"
        | Communicate.Reviews -> "reviews"
        | Communicate.RingVolume -> "ring_volume"
        | Communicate.Rtt -> "rtt"
        | Communicate.SatelliteAlt -> "satellite_alt"
        | Communicate.ScheduleSend -> "schedule_send"
        | Communicate.Score -> "score"
        | Communicate.Send -> "send"
        | Communicate.SendAndArchive -> "send_and_archive"
        | Communicate.SettingsBluetooth -> "settings_bluetooth"
        | Communicate.SettingsPhone -> "settings_phone"
        | Communicate.SignalCellularAdd -> "signal_cellular_add"
        | Communicate.Sip -> "sip"
        | Communicate.Sms -> "sms"
        | Communicate.SpeakerNotes -> "speaker_notes"
        | Communicate.SpeakerNotesOff -> "speaker_notes_off"
        | Communicate.SpeakerPhone -> "speaker_phone"
        | Communicate.Spoke -> "spoke"
        | Communicate.StackedEmail -> "stacked_email"
        | Communicate.StackedInbox -> "stacked_inbox"
        | Communicate.SwapCalls -> "swap_calls"
        | Communicate.ThreadUnread -> "thread_unread"
        | Communicate.ThreatIntelligence -> "threat_intelligence"
        | Communicate.Tooltip -> "tooltip"
        | Communicate.Tooltip2 -> "tooltip_2"
        | Communicate.Topic -> "topic"
        | Communicate.Unarchive -> "unarchive"
        | Communicate.Unsubscribe -> "unsubscribe"
        | Communicate.Upcoming -> "upcoming"
        | Communicate.VideoChat -> "video_chat"
        | Communicate.VoiceChat -> "voice_chat"
        | Communicate.VoiceChatOff -> "voice_chat_off"
        | Communicate.Voicemail -> "voicemail"
        | Communicate.Voicemail2 -> "voicemail_2"
        | Communicate.WifiAdd -> "wifi_add"
        | Communicate.WifiCalling -> "wifi_calling"
        | Communicate.WifiChannel -> "wifi_channel"
        | Communicate.WifiProxy -> "wifi_proxy"

    [<RequireQualifiedAccess; Struct>]
    type Hardware =
      | AddDiamond
      | AdfScanner
      | AodTablet
      | AodWatch
      | ArrowsLeftRightCircle
      | ArrowsUpDownCircle
      | AssistantDevice
      | AudioVideoReceiver
      | BCircle
      | Balance
      | BrowserUpdated
      | CameraVideo
      | Cast
      | CastConnected
      | CastForEducation
      | CastPause
      | CastWarning
      | ChromecastDevice
      | CircleCircle
      | Computer
      | ComputerArrowUp
      | ComputerCancel
      | ComputerSound
      | ConnectedTv
      | Deskphone
      | DesktopAccessDisabled
      | DesktopCloud
      | DesktopCloudStack
      | DesktopMac
      | DesktopWindows
      | DeveloperBoard
      | DeveloperBoardOff
      | DeveloperModeTv
      | DeviceBand
      | DeviceHub
      | DeviceThermostat
      | Devices
      | DevicesOff
      | DevicesOther
      | DevicesWearables
      | DiscFull
      | DisplaySettings
      | Dns
      | EarbudCase
      | EarbudLeft
      | EarbudRight
      | Earbuds
      | Earbuds2
      | EarbudsBattery
      | Ecg
      | EmojiLanguage
      | Fax
      | FitnessTracker
      | FitnessTrackers
      | GameBumperLeft
      | GameBumperRight
      | GameButtonL
      | GameButtonL1
      | GameButtonL2
      | GameButtonR
      | GameButtonR1
      | GameButtonR2
      | GameButtonZl
      | GameButtonZr
      | GameStickL3
      | GameStickLeft
      | GameStickR3
      | GameStickRight
      | GameTriggerLeft
      | GameTriggerRight
      | Gamepad
      | GamepadCircleDown
      | GamepadCircleLeft
      | GamepadCircleRight
      | GamepadCircleUp
      | GamepadDown
      | GamepadLeft
      | GamepadRight
      | GamepadUp
      | GeneralDevice
      | GoogleHomeDevices
      | HandheldController
      | HardDisk
      | HardDrive
      | HardDrive2
      | HeadMountedDevice
      | Headphones
      | HeadphonesBattery
      | HeadsetMic
      | HeadsetOff
      | HomeMax
      | HomeMini
      | Host
      | ImportantDevices
      | JamboardKiosk
      | Joystick
      | Keyboard
      | KeyboardAlt
      | KeyboardArrowDown
      | KeyboardArrowLeft
      | KeyboardArrowRight
      | KeyboardArrowUp
      | KeyboardBackspace
      | KeyboardCapslock
      | KeyboardHide
      | KeyboardLock
      | KeyboardLockOff
      | KeyboardReturn
      | KeyboardTab
      | KeyboardTabRtl
      | LaptopCar
      | LaptopChromebook
      | LaptopMac
      | LaptopWindows
      | Lda
      | LiftToTalk
      | LightningStand
      | LiveTv
      | MediaOutput
      | MediaOutputOff
      | Memory
      | MemoryAlt
      | Merge
      | Mimo
      | MimoDisconnect
      | MissingController
      | Mobile
      | Mobile2
      | Mobile3
      | MobileAlert
      | MobileArrowDown
      | MobileArrowRight
      | MobileArrowUpRight
      | MobileBlock
      | MobileCamera
      | MobileCast
      | MobileCharge
      | MobileChat
      | MobileCheck
      | MobileCode
      | MobileDots
      | MobileGear
      | MobileHand
      | MobileHandLeft
      | MobileHandLeftOff
      | MobileHandOff
      | MobileInfo
      | MobileLandscape
      | MobileLayout
      | MobileLockLandscape
      | MobileLockPortrait
      | MobileLoupe
      | MobileMenu
      | MobileOff
      | MobileQuestion
      | MobileRotate
      | MobileRotateLock
      | MobileScreensaver
      | MobileShare
      | MobileShareStack
      | MobileSound2
      | MobileSpeaker
      | MobileText
      | MobileText2
      | MobileTicket
      | MobileUnlock
      | MobileVibrate
      | Monitor
      | MonitorWeight
      | Mouse
      | MouseLock
      | MouseLockOff
      | NightSightMax
      | NoSim
      | OpenJam
      | P2p
      | Pacemaker
      | PlugConnect
      | PointOfSale
      | Power
      | PowerInput
      | PowerOff
      | Print
      | PrintAdd
      | PrintConnect
      | PrintDisabled
      | PrintError
      | PrintLock
      | PunchClock
      | RampLeft
      | RampRight
      | RearCamera
      | RectangleAdd
      | RememberMe
      | ResetTv
      | ResetWrench
      | Robot
      | Robot2
      | RoundaboutLeft
      | RoundaboutRight
      | Route
      | Router
      | RouterOff
      | Save
      | SaveClock
      | Scale
      | Scanner
      | ScreenSearchDesktop
      | ScreenShare
      | ScreenshotMonitor
      | ScreenshotTablet
      | SdCard
      | SdCardAlert
      | SecurityKey
      | ServerPerson
      | SettingsEthernet
      | SettingsInputAntenna
      | SettingsInputComponent
      | SettingsInputHdmi
      | SettingsInputSvideo
      | SettingsRemote
      | SettopComponent
      | SimCard
      | SmartCardReader
      | SmartCardReaderOff
      | SmartDisplay
      | SmartToy
      | Speaker
      | SpeakerGroup
      | SquareCircle
      | StopScreenShare
      | Straight
      | Tablet
      | TabletAndroid
      | TabletCamera
      | TabletMac
      | TouchpadMouse
      | TouchpadMouseOff
      | TriangleCircle
      | Tty
      | Tv
      | TvDisplays
      | TvGuide
      | TvNext
      | TvOff
      | TvOptionsEditChannels
      | TvOptionsInputSettings
      | TvRemote
      | TvSignin
      | Ventilator
      | VideogameAsset
      | VideogameAssetOff
      | Watch
      | WatchArrow
      | WatchButtonPress
      | WatchCheck
      | WatchLock
      | WatchOff
      | WatchVibration
      | WatchWake
      | XCircle
      | YCircle

    module Hardware =

      let toSnakeCase (icon: Hardware) =
        match icon with
        | Hardware.AddDiamond -> "add_diamond"
        | Hardware.AdfScanner -> "adf_scanner"
        | Hardware.AodTablet -> "aod_tablet"
        | Hardware.AodWatch -> "aod_watch"
        | Hardware.ArrowsLeftRightCircle -> "arrows_left_right_circle"
        | Hardware.ArrowsUpDownCircle -> "arrows_up_down_circle"
        | Hardware.AssistantDevice -> "assistant_device"
        | Hardware.AudioVideoReceiver -> "audio_video_receiver"
        | Hardware.BCircle -> "b_circle"
        | Hardware.Balance -> "balance"
        | Hardware.BrowserUpdated -> "browser_updated"
        | Hardware.CameraVideo -> "camera_video"
        | Hardware.Cast -> "cast"
        | Hardware.CastConnected -> "cast_connected"
        | Hardware.CastForEducation -> "cast_for_education"
        | Hardware.CastPause -> "cast_pause"
        | Hardware.CastWarning -> "cast_warning"
        | Hardware.ChromecastDevice -> "chromecast_device"
        | Hardware.CircleCircle -> "circle_circle"
        | Hardware.Computer -> "computer"
        | Hardware.ComputerArrowUp -> "computer_arrow_up"
        | Hardware.ComputerCancel -> "computer_cancel"
        | Hardware.ComputerSound -> "computer_sound"
        | Hardware.ConnectedTv -> "connected_tv"
        | Hardware.Deskphone -> "deskphone"
        | Hardware.DesktopAccessDisabled -> "desktop_access_disabled"
        | Hardware.DesktopCloud -> "desktop_cloud"
        | Hardware.DesktopCloudStack -> "desktop_cloud_stack"
        | Hardware.DesktopMac -> "desktop_mac"
        | Hardware.DesktopWindows -> "desktop_windows"
        | Hardware.DeveloperBoard -> "developer_board"
        | Hardware.DeveloperBoardOff -> "developer_board_off"
        | Hardware.DeveloperModeTv -> "developer_mode_tv"
        | Hardware.DeviceBand -> "device_band"
        | Hardware.DeviceHub -> "device_hub"
        | Hardware.DeviceThermostat -> "device_thermostat"
        | Hardware.Devices -> "devices"
        | Hardware.DevicesOff -> "devices_off"
        | Hardware.DevicesOther -> "devices_other"
        | Hardware.DevicesWearables -> "devices_wearables"
        | Hardware.DiscFull -> "disc_full"
        | Hardware.DisplaySettings -> "display_settings"
        | Hardware.Dns -> "dns"
        | Hardware.EarbudCase -> "earbud_case"
        | Hardware.EarbudLeft -> "earbud_left"
        | Hardware.EarbudRight -> "earbud_right"
        | Hardware.Earbuds -> "earbuds"
        | Hardware.Earbuds2 -> "earbuds_2"
        | Hardware.EarbudsBattery -> "earbuds_battery"
        | Hardware.Ecg -> "ecg"
        | Hardware.EmojiLanguage -> "emoji_language"
        | Hardware.Fax -> "fax"
        | Hardware.FitnessTracker -> "fitness_tracker"
        | Hardware.FitnessTrackers -> "fitness_trackers"
        | Hardware.GameBumperLeft -> "game_bumper_left"
        | Hardware.GameBumperRight -> "game_bumper_right"
        | Hardware.GameButtonL -> "game_button_l"
        | Hardware.GameButtonL1 -> "game_button_l1"
        | Hardware.GameButtonL2 -> "game_button_l2"
        | Hardware.GameButtonR -> "game_button_r"
        | Hardware.GameButtonR1 -> "game_button_r1"
        | Hardware.GameButtonR2 -> "game_button_r2"
        | Hardware.GameButtonZl -> "game_button_zl"
        | Hardware.GameButtonZr -> "game_button_zr"
        | Hardware.GameStickL3 -> "game_stick_l3"
        | Hardware.GameStickLeft -> "game_stick_left"
        | Hardware.GameStickR3 -> "game_stick_r3"
        | Hardware.GameStickRight -> "game_stick_right"
        | Hardware.GameTriggerLeft -> "game_trigger_left"
        | Hardware.GameTriggerRight -> "game_trigger_right"
        | Hardware.Gamepad -> "gamepad"
        | Hardware.GamepadCircleDown -> "gamepad_circle_down"
        | Hardware.GamepadCircleLeft -> "gamepad_circle_left"
        | Hardware.GamepadCircleRight -> "gamepad_circle_right"
        | Hardware.GamepadCircleUp -> "gamepad_circle_up"
        | Hardware.GamepadDown -> "gamepad_down"
        | Hardware.GamepadLeft -> "gamepad_left"
        | Hardware.GamepadRight -> "gamepad_right"
        | Hardware.GamepadUp -> "gamepad_up"
        | Hardware.GeneralDevice -> "general_device"
        | Hardware.GoogleHomeDevices -> "google_home_devices"
        | Hardware.HandheldController -> "handheld_controller"
        | Hardware.HardDisk -> "hard_disk"
        | Hardware.HardDrive -> "hard_drive"
        | Hardware.HardDrive2 -> "hard_drive_2"
        | Hardware.HeadMountedDevice -> "head_mounted_device"
        | Hardware.Headphones -> "headphones"
        | Hardware.HeadphonesBattery -> "headphones_battery"
        | Hardware.HeadsetMic -> "headset_mic"
        | Hardware.HeadsetOff -> "headset_off"
        | Hardware.HomeMax -> "home_max"
        | Hardware.HomeMini -> "home_mini"
        | Hardware.Host -> "host"
        | Hardware.ImportantDevices -> "important_devices"
        | Hardware.JamboardKiosk -> "jamboard_kiosk"
        | Hardware.Joystick -> "joystick"
        | Hardware.Keyboard -> "keyboard"
        | Hardware.KeyboardAlt -> "keyboard_alt"
        | Hardware.KeyboardArrowDown -> "keyboard_arrow_down"
        | Hardware.KeyboardArrowLeft -> "keyboard_arrow_left"
        | Hardware.KeyboardArrowRight -> "keyboard_arrow_right"
        | Hardware.KeyboardArrowUp -> "keyboard_arrow_up"
        | Hardware.KeyboardBackspace -> "keyboard_backspace"
        | Hardware.KeyboardCapslock -> "keyboard_capslock"
        | Hardware.KeyboardHide -> "keyboard_hide"
        | Hardware.KeyboardLock -> "keyboard_lock"
        | Hardware.KeyboardLockOff -> "keyboard_lock_off"
        | Hardware.KeyboardReturn -> "keyboard_return"
        | Hardware.KeyboardTab -> "keyboard_tab"
        | Hardware.KeyboardTabRtl -> "keyboard_tab_rtl"
        | Hardware.LaptopCar -> "laptop_car"
        | Hardware.LaptopChromebook -> "laptop_chromebook"
        | Hardware.LaptopMac -> "laptop_mac"
        | Hardware.LaptopWindows -> "laptop_windows"
        | Hardware.Lda -> "lda"
        | Hardware.LiftToTalk -> "lift_to_talk"
        | Hardware.LightningStand -> "lightning_stand"
        | Hardware.LiveTv -> "live_tv"
        | Hardware.MediaOutput -> "media_output"
        | Hardware.MediaOutputOff -> "media_output_off"
        | Hardware.Memory -> "memory"
        | Hardware.MemoryAlt -> "memory_alt"
        | Hardware.Merge -> "merge"
        | Hardware.Mimo -> "mimo"
        | Hardware.MimoDisconnect -> "mimo_disconnect"
        | Hardware.MissingController -> "missing_controller"
        | Hardware.Mobile -> "mobile"
        | Hardware.Mobile2 -> "mobile_2"
        | Hardware.Mobile3 -> "mobile_3"
        | Hardware.MobileAlert -> "mobile_alert"
        | Hardware.MobileArrowDown -> "mobile_arrow_down"
        | Hardware.MobileArrowRight -> "mobile_arrow_right"
        | Hardware.MobileArrowUpRight -> "mobile_arrow_up_right"
        | Hardware.MobileBlock -> "mobile_block"
        | Hardware.MobileCamera -> "mobile_camera"
        | Hardware.MobileCast -> "mobile_cast"
        | Hardware.MobileCharge -> "mobile_charge"
        | Hardware.MobileChat -> "mobile_chat"
        | Hardware.MobileCheck -> "mobile_check"
        | Hardware.MobileCode -> "mobile_code"
        | Hardware.MobileDots -> "mobile_dots"
        | Hardware.MobileGear -> "mobile_gear"
        | Hardware.MobileHand -> "mobile_hand"
        | Hardware.MobileHandLeft -> "mobile_hand_left"
        | Hardware.MobileHandLeftOff -> "mobile_hand_left_off"
        | Hardware.MobileHandOff -> "mobile_hand_off"
        | Hardware.MobileInfo -> "mobile_info"
        | Hardware.MobileLandscape -> "mobile_landscape"
        | Hardware.MobileLayout -> "mobile_layout"
        | Hardware.MobileLockLandscape -> "mobile_lock_landscape"
        | Hardware.MobileLockPortrait -> "mobile_lock_portrait"
        | Hardware.MobileLoupe -> "mobile_loupe"
        | Hardware.MobileMenu -> "mobile_menu"
        | Hardware.MobileOff -> "mobile_off"
        | Hardware.MobileQuestion -> "mobile_question"
        | Hardware.MobileRotate -> "mobile_rotate"
        | Hardware.MobileRotateLock -> "mobile_rotate_lock"
        | Hardware.MobileScreensaver -> "mobile_screensaver"
        | Hardware.MobileShare -> "mobile_share"
        | Hardware.MobileShareStack -> "mobile_share_stack"
        | Hardware.MobileSound2 -> "mobile_sound_2"
        | Hardware.MobileSpeaker -> "mobile_speaker"
        | Hardware.MobileText -> "mobile_text"
        | Hardware.MobileText2 -> "mobile_text_2"
        | Hardware.MobileTicket -> "mobile_ticket"
        | Hardware.MobileUnlock -> "mobile_unlock"
        | Hardware.MobileVibrate -> "mobile_vibrate"
        | Hardware.Monitor -> "monitor"
        | Hardware.MonitorWeight -> "monitor_weight"
        | Hardware.Mouse -> "mouse"
        | Hardware.MouseLock -> "mouse_lock"
        | Hardware.MouseLockOff -> "mouse_lock_off"
        | Hardware.NightSightMax -> "night_sight_max"
        | Hardware.NoSim -> "no_sim"
        | Hardware.OpenJam -> "open_jam"
        | Hardware.P2p -> "p2p"
        | Hardware.Pacemaker -> "pacemaker"
        | Hardware.PlugConnect -> "plug_connect"
        | Hardware.PointOfSale -> "point_of_sale"
        | Hardware.Power -> "power"
        | Hardware.PowerInput -> "power_input"
        | Hardware.PowerOff -> "power_off"
        | Hardware.Print -> "print"
        | Hardware.PrintAdd -> "print_add"
        | Hardware.PrintConnect -> "print_connect"
        | Hardware.PrintDisabled -> "print_disabled"
        | Hardware.PrintError -> "print_error"
        | Hardware.PrintLock -> "print_lock"
        | Hardware.PunchClock -> "punch_clock"
        | Hardware.RampLeft -> "ramp_left"
        | Hardware.RampRight -> "ramp_right"
        | Hardware.RearCamera -> "rear_camera"
        | Hardware.RectangleAdd -> "rectangle_add"
        | Hardware.RememberMe -> "remember_me"
        | Hardware.ResetTv -> "reset_tv"
        | Hardware.ResetWrench -> "reset_wrench"
        | Hardware.Robot -> "robot"
        | Hardware.Robot2 -> "robot_2"
        | Hardware.RoundaboutLeft -> "roundabout_left"
        | Hardware.RoundaboutRight -> "roundabout_right"
        | Hardware.Route -> "route"
        | Hardware.Router -> "router"
        | Hardware.RouterOff -> "router_off"
        | Hardware.Save -> "save"
        | Hardware.SaveClock -> "save_clock"
        | Hardware.Scale -> "scale"
        | Hardware.Scanner -> "scanner"
        | Hardware.ScreenSearchDesktop -> "screen_search_desktop"
        | Hardware.ScreenShare -> "screen_share"
        | Hardware.ScreenshotMonitor -> "screenshot_monitor"
        | Hardware.ScreenshotTablet -> "screenshot_tablet"
        | Hardware.SdCard -> "sd_card"
        | Hardware.SdCardAlert -> "sd_card_alert"
        | Hardware.SecurityKey -> "security_key"
        | Hardware.ServerPerson -> "server_person"
        | Hardware.SettingsEthernet -> "settings_ethernet"
        | Hardware.SettingsInputAntenna -> "settings_input_antenna"
        | Hardware.SettingsInputComponent -> "settings_input_component"
        | Hardware.SettingsInputHdmi -> "settings_input_hdmi"
        | Hardware.SettingsInputSvideo -> "settings_input_svideo"
        | Hardware.SettingsRemote -> "settings_remote"
        | Hardware.SettopComponent -> "settop_component"
        | Hardware.SimCard -> "sim_card"
        | Hardware.SmartCardReader -> "smart_card_reader"
        | Hardware.SmartCardReaderOff -> "smart_card_reader_off"
        | Hardware.SmartDisplay -> "smart_display"
        | Hardware.SmartToy -> "smart_toy"
        | Hardware.Speaker -> "speaker"
        | Hardware.SpeakerGroup -> "speaker_group"
        | Hardware.SquareCircle -> "square_circle"
        | Hardware.StopScreenShare -> "stop_screen_share"
        | Hardware.Straight -> "straight"
        | Hardware.Tablet -> "tablet"
        | Hardware.TabletAndroid -> "tablet_android"
        | Hardware.TabletCamera -> "tablet_camera"
        | Hardware.TabletMac -> "tablet_mac"
        | Hardware.TouchpadMouse -> "touchpad_mouse"
        | Hardware.TouchpadMouseOff -> "touchpad_mouse_off"
        | Hardware.TriangleCircle -> "triangle_circle"
        | Hardware.Tty -> "tty"
        | Hardware.Tv -> "tv"
        | Hardware.TvDisplays -> "tv_displays"
        | Hardware.TvGuide -> "tv_guide"
        | Hardware.TvNext -> "tv_next"
        | Hardware.TvOff -> "tv_off"
        | Hardware.TvOptionsEditChannels -> "tv_options_edit_channels"
        | Hardware.TvOptionsInputSettings -> "tv_options_input_settings"
        | Hardware.TvRemote -> "tv_remote"
        | Hardware.TvSignin -> "tv_signin"
        | Hardware.Ventilator -> "ventilator"
        | Hardware.VideogameAsset -> "videogame_asset"
        | Hardware.VideogameAssetOff -> "videogame_asset_off"
        | Hardware.Watch -> "watch"
        | Hardware.WatchArrow -> "watch_arrow"
        | Hardware.WatchButtonPress -> "watch_button_press"
        | Hardware.WatchCheck -> "watch_check"
        | Hardware.WatchLock -> "watch_lock"
        | Hardware.WatchOff -> "watch_off"
        | Hardware.WatchVibration -> "watch_vibration"
        | Hardware.WatchWake -> "watch_wake"
        | Hardware.XCircle -> "x_circle"
        | Hardware.YCircle -> "y_circle"

    [<RequireQualifiedAccess; Struct>]
    type Home =
      | ActivityZone
      | Airwave
      | Aq
      | AqIndoor
      | ArmingCountdown
      | ArrowsMoreDown
      | ArrowsMoreUp
      | AssistantOnHub
      | BatteryHoriz000
      | BatteryHoriz050
      | BatteryHoriz075
      | BatteryProfile
      | Chromecast2
      | CleaningBucket
      | ClimateMiniSplit
      | CoolToDry
      | DetectionAndZone
      | DetectionAndZoneOff
      | Detector
      | DetectorAlarm
      | DetectorBattery
      | DetectorCo
      | DetectorOffline
      | DetectorStatus
      | DoorOpen
      | DoorSensor
      | DoorbellChime
      | EarlyOn
      | FamiliarFaceAndZone
      | FarsightDigital
      | FloorLamp
      | GoogleTvRemote
      | GoogleWifi
      | Heat
      | HeatPumpBalance
      | HomeMaxDots
      | HomeSpeaker
      | HomeStorage
      | HouseWithShield
      | HumidityIndoor
      | Laundry
      | LightGroup
      | MfgNestYaleLock
      | ModeDual
      | MotionSensorActive
      | MotionSensorAlert
      | MotionSensorIdle
      | MotionSensorUrgent
      | NestAudio
      | NestCamFloodlight
      | NestCamIndoor
      | NestCamIq
      | NestCamIqOutdoor
      | NestCamMagnetMount
      | NestCamOutdoor
      | NestCamStand
      | NestCamWallMount
      | NestCamWiredStand
      | NestClockFarsightAnalog
      | NestClockFarsightDigital
      | NestConnect
      | NestDetect
      | NestDisplay
      | NestDisplayMax
      | NestDoorbellVisitor
      | NestEcoLeaf
      | NestFarsightCool
      | NestFarsightDual
      | NestFarsightEco
      | NestFarsightHeat
      | NestFarsightSeasonal
      | NestFarsightWeather
      | NestFoundSavings
      | NestHeatLinkE
      | NestHeatLinkGen3
      | NestHelloDoorbell
      | NestMini
      | NestMultiRoom
      | NestProtect
      | NestRemoteComfortSensor
      | NestSecureAlarm
      | NestSunblock
      | NestTag
      | NestThermostat
      | NestThermostatEEu
      | NestThermostatGen3
      | NestThermostatSensor
      | NestThermostatSensorEu
      | NestThermostatZirconiumEu
      | NestTrueRadiant
      | NestWakeOnApproach
      | NestWakeOnPress
      | NestWifiPoint
      | NestWifiPro
      | NestWifiPro2
      | NestWifiRouter
      | OnHubDevice
      | Productivity
      | SelfCare
      | SensorsKrx
      | SensorsKrxOff
      | SettingsAlert
      | ShieldWithHeart
      | ShieldWithHouse
      | StadiaController
      | TableLamp
      | TamperDetectionOn
      | TempPreferencesEco
      | ToolsFlatHead
      | ToolsInstallationKit
      | ToolsLadder
      | ToolsLevel
      | ToolsPhillips
      | ToolsPliersWireStripper
      | ToolsPowerDrill
      | WallLamp
      | WaterPump
      | WeatherSnowy
      | WindowClosed
      | WindowOpen
      | WindowSensor
      | ZonePersonAlert
      | ZonePersonIdle
      | ZonePersonUrgent

    module Home =

      let toSnakeCase (icon: Home) =
        match icon with
        | Home.ActivityZone -> "activity_zone"
        | Home.Airwave -> "airwave"
        | Home.Aq -> "aq"
        | Home.AqIndoor -> "aq_indoor"
        | Home.ArmingCountdown -> "arming_countdown"
        | Home.ArrowsMoreDown -> "arrows_more_down"
        | Home.ArrowsMoreUp -> "arrows_more_up"
        | Home.AssistantOnHub -> "assistant_on_hub"
        | Home.BatteryHoriz000 -> "battery_horiz_000"
        | Home.BatteryHoriz050 -> "battery_horiz_050"
        | Home.BatteryHoriz075 -> "battery_horiz_075"
        | Home.BatteryProfile -> "battery_profile"
        | Home.Chromecast2 -> "chromecast_2"
        | Home.CleaningBucket -> "cleaning_bucket"
        | Home.ClimateMiniSplit -> "climate_mini_split"
        | Home.CoolToDry -> "cool_to_dry"
        | Home.DetectionAndZone -> "detection_and_zone"
        | Home.DetectionAndZoneOff -> "detection_and_zone_off"
        | Home.Detector -> "detector"
        | Home.DetectorAlarm -> "detector_alarm"
        | Home.DetectorBattery -> "detector_battery"
        | Home.DetectorCo -> "detector_co"
        | Home.DetectorOffline -> "detector_offline"
        | Home.DetectorStatus -> "detector_status"
        | Home.DoorOpen -> "door_open"
        | Home.DoorSensor -> "door_sensor"
        | Home.DoorbellChime -> "doorbell_chime"
        | Home.EarlyOn -> "early_on"
        | Home.FamiliarFaceAndZone -> "familiar_face_and_zone"
        | Home.FarsightDigital -> "farsight_digital"
        | Home.FloorLamp -> "floor_lamp"
        | Home.GoogleTvRemote -> "google_tv_remote"
        | Home.GoogleWifi -> "google_wifi"
        | Home.Heat -> "heat"
        | Home.HeatPumpBalance -> "heat_pump_balance"
        | Home.HomeMaxDots -> "home_max_dots"
        | Home.HomeSpeaker -> "home_speaker"
        | Home.HomeStorage -> "home_storage"
        | Home.HouseWithShield -> "house_with_shield"
        | Home.HumidityIndoor -> "humidity_indoor"
        | Home.Laundry -> "laundry"
        | Home.LightGroup -> "light_group"
        | Home.MfgNestYaleLock -> "mfg_nest_yale_lock"
        | Home.ModeDual -> "mode_dual"
        | Home.MotionSensorActive -> "motion_sensor_active"
        | Home.MotionSensorAlert -> "motion_sensor_alert"
        | Home.MotionSensorIdle -> "motion_sensor_idle"
        | Home.MotionSensorUrgent -> "motion_sensor_urgent"
        | Home.NestAudio -> "nest_audio"
        | Home.NestCamFloodlight -> "nest_cam_floodlight"
        | Home.NestCamIndoor -> "nest_cam_indoor"
        | Home.NestCamIq -> "nest_cam_iq"
        | Home.NestCamIqOutdoor -> "nest_cam_iq_outdoor"
        | Home.NestCamMagnetMount -> "nest_cam_magnet_mount"
        | Home.NestCamOutdoor -> "nest_cam_outdoor"
        | Home.NestCamStand -> "nest_cam_stand"
        | Home.NestCamWallMount -> "nest_cam_wall_mount"
        | Home.NestCamWiredStand -> "nest_cam_wired_stand"
        | Home.NestClockFarsightAnalog -> "nest_clock_farsight_analog"
        | Home.NestClockFarsightDigital -> "nest_clock_farsight_digital"
        | Home.NestConnect -> "nest_connect"
        | Home.NestDetect -> "nest_detect"
        | Home.NestDisplay -> "nest_display"
        | Home.NestDisplayMax -> "nest_display_max"
        | Home.NestDoorbellVisitor -> "nest_doorbell_visitor"
        | Home.NestEcoLeaf -> "nest_eco_leaf"
        | Home.NestFarsightCool -> "nest_farsight_cool"
        | Home.NestFarsightDual -> "nest_farsight_dual"
        | Home.NestFarsightEco -> "nest_farsight_eco"
        | Home.NestFarsightHeat -> "nest_farsight_heat"
        | Home.NestFarsightSeasonal -> "nest_farsight_seasonal"
        | Home.NestFarsightWeather -> "nest_farsight_weather"
        | Home.NestFoundSavings -> "nest_found_savings"
        | Home.NestHeatLinkE -> "nest_heat_link_e"
        | Home.NestHeatLinkGen3 -> "nest_heat_link_gen_3"
        | Home.NestHelloDoorbell -> "nest_hello_doorbell"
        | Home.NestMini -> "nest_mini"
        | Home.NestMultiRoom -> "nest_multi_room"
        | Home.NestProtect -> "nest_protect"
        | Home.NestRemoteComfortSensor -> "nest_remote_comfort_sensor"
        | Home.NestSecureAlarm -> "nest_secure_alarm"
        | Home.NestSunblock -> "nest_sunblock"
        | Home.NestTag -> "nest_tag"
        | Home.NestThermostat -> "nest_thermostat"
        | Home.NestThermostatEEu -> "nest_thermostat_e_eu"
        | Home.NestThermostatGen3 -> "nest_thermostat_gen_3"
        | Home.NestThermostatSensor -> "nest_thermostat_sensor"
        | Home.NestThermostatSensorEu -> "nest_thermostat_sensor_eu"
        | Home.NestThermostatZirconiumEu -> "nest_thermostat_zirconium_eu"
        | Home.NestTrueRadiant -> "nest_true_radiant"
        | Home.NestWakeOnApproach -> "nest_wake_on_approach"
        | Home.NestWakeOnPress -> "nest_wake_on_press"
        | Home.NestWifiPoint -> "nest_wifi_point"
        | Home.NestWifiPro -> "nest_wifi_pro"
        | Home.NestWifiPro2 -> "nest_wifi_pro_2"
        | Home.NestWifiRouter -> "nest_wifi_router"
        | Home.OnHubDevice -> "on_hub_device"
        | Home.Productivity -> "productivity"
        | Home.SelfCare -> "self_care"
        | Home.SensorsKrx -> "sensors_krx"
        | Home.SensorsKrxOff -> "sensors_krx_off"
        | Home.SettingsAlert -> "settings_alert"
        | Home.ShieldWithHeart -> "shield_with_heart"
        | Home.ShieldWithHouse -> "shield_with_house"
        | Home.StadiaController -> "stadia_controller"
        | Home.TableLamp -> "table_lamp"
        | Home.TamperDetectionOn -> "tamper_detection_on"
        | Home.TempPreferencesEco -> "temp_preferences_eco"
        | Home.ToolsFlatHead -> "tools_flat_head"
        | Home.ToolsInstallationKit -> "tools_installation_kit"
        | Home.ToolsLadder -> "tools_ladder"
        | Home.ToolsLevel -> "tools_level"
        | Home.ToolsPhillips -> "tools_phillips"
        | Home.ToolsPliersWireStripper -> "tools_pliers_wire_stripper"
        | Home.ToolsPowerDrill -> "tools_power_drill"
        | Home.WallLamp -> "wall_lamp"
        | Home.WaterPump -> "water_pump"
        | Home.WeatherSnowy -> "weather_snowy"
        | Home.WindowClosed -> "window_closed"
        | Home.WindowOpen -> "window_open"
        | Home.WindowSensor -> "window_sensor"
        | Home.ZonePersonAlert -> "zone_person_alert"
        | Home.ZonePersonIdle -> "zone_person_idle"
        | Home.ZonePersonUrgent -> "zone_person_urgent"

    [<RequireQualifiedAccess; Struct>]
    type Household =
      | AcUnit
      | AirFreshener
      | AirPurifier
      | AirPurifierGen
      | Apparel
      | BackHand
      | Balcony
      | BathSoak
      | Bathroom
      | Bathtub
      | Bed
      | BedroomBaby
      | BedroomChild
      | BedroomParent
      | Blanket
      | Blender
      | Blinds
      | BlindsClosed
      | CameraIndoor
      | CameraOutdoor
      | Chair
      | ChairAlt
      | ChairCounter
      | ChairFireplace
      | ChairUmbrella
      | Checkroom
      | ChildCare
      | Coffee
      | CoffeeMaker
      | ControllerGen
      | Cooking
      | Countertops
      | Crib
      | Curtains
      | CurtainsClosed
      | Deck
      | Desk
      | DetectorSmoke
      | DineHeart
      | DineLamp
      | Dining
      | Dishwasher
      | DishwasherGen
      | DoorBack
      | DoorFront
      | DoorSliding
      | Doorbell
      | Doorbell3p
      | Dresser
      | Dry
      | ElectricBolt
      | ElectricMeter
      | EmergencyHeat
      | EmergencyHeat2
      | EmergencyHome
      | EmergencyRecording
      | EmergencyShare
      | EmergencyShareOff
      | EnergyProgramSaving
      | EnergyProgramTimeUsed
      | EnergySavingsLeaf
      | EventSeat
      | FamilyHome
      | Faucet
      | Fence
      | FireExtinguisher
      | Fireplace
      | Flatware
      | ForkSpoon
      | Foundation
      | Fragrance
      | Garage
      | GarageDoor
      | GarageHome
      | GasMeter
      | Gate
      | Grass
      | Grocery
      | Hallway
      | Hardware
      | HealthAndBeauty
      | HeatPump
      | HighChair
      | Highlight
      | HomeAndGarden
      | HomeImprovementAndTools
      | HomeIotDevice
      | HotTub
      | House
      | HouseSiding
      | HouseholdSupplies
      | HumidityHigh
      | HumidityLow
      | HumidityMid
      | Hvac
      | InHomeMode
      | Iron
      | Kettle
      | KingBed
      | Kitchen
      | Light
      | LightOff
      | Lightbulb2
      | Living
      | Matter
      | Microwave
      | MicrowaveGen
      | ModeCool
      | ModeCoolOff
      | ModeFan
      | ModeFanOff
      | ModeHeat
      | ModeHeatCool
      | ModeHeatOff
      | ModeNight
      | ModeOffOn
      | Mop
      | Multicooker
      | OutdoorGrill
      | Outlet
      | Oven
      | OvenGen
      | Propane
      | PropaneTank
      | RangeHood
      | RemoteGen
      | RollerShades
      | RollerShadesClosed
      | Roofing
      | Scene
      | SensorDoor
      | SensorOccupied
      | SensorWindow
      | Sensors
      | SensorsOff
      | Shelves
      | ShieldMoon
      | Shower
      | SingleBed
      | Skillet
      | SkilletCooktop
      | SmartOutlet
      | Soap
      | Sprinkler
      | Stockpot
      | Stroller
      | Styler
      | Switch
      | TableBar
      | TableLarge
      | TableRestaurant
      | TamperDetectionOff
      | Thermometer
      | ThermometerAdd
      | ThermometerAlert
      | ThermometerGain
      | ThermometerLoss
      | ThermometerMinus
      | ThermostatAuto
      | ThermostatCarbon
      | TvGen
      | TvWithAssistant
      | Umbrella
      | Vacuum
      | Valve
      | VerticalShades
      | VerticalShadesClosed
      | WallArt
      | Wash
      | WaterDamage
      | WaterHeater
      | Weekend
      | Window
      | Yard

    module Household =

      let toSnakeCase (icon: Household) =
        match icon with
        | Household.AcUnit -> "ac_unit"
        | Household.AirFreshener -> "air_freshener"
        | Household.AirPurifier -> "air_purifier"
        | Household.AirPurifierGen -> "air_purifier_gen"
        | Household.Apparel -> "apparel"
        | Household.BackHand -> "back_hand"
        | Household.Balcony -> "balcony"
        | Household.BathSoak -> "bath_soak"
        | Household.Bathroom -> "bathroom"
        | Household.Bathtub -> "bathtub"
        | Household.Bed -> "bed"
        | Household.BedroomBaby -> "bedroom_baby"
        | Household.BedroomChild -> "bedroom_child"
        | Household.BedroomParent -> "bedroom_parent"
        | Household.Blanket -> "blanket"
        | Household.Blender -> "blender"
        | Household.Blinds -> "blinds"
        | Household.BlindsClosed -> "blinds_closed"
        | Household.CameraIndoor -> "camera_indoor"
        | Household.CameraOutdoor -> "camera_outdoor"
        | Household.Chair -> "chair"
        | Household.ChairAlt -> "chair_alt"
        | Household.ChairCounter -> "chair_counter"
        | Household.ChairFireplace -> "chair_fireplace"
        | Household.ChairUmbrella -> "chair_umbrella"
        | Household.Checkroom -> "checkroom"
        | Household.ChildCare -> "child_care"
        | Household.Coffee -> "coffee"
        | Household.CoffeeMaker -> "coffee_maker"
        | Household.ControllerGen -> "controller_gen"
        | Household.Cooking -> "cooking"
        | Household.Countertops -> "countertops"
        | Household.Crib -> "crib"
        | Household.Curtains -> "curtains"
        | Household.CurtainsClosed -> "curtains_closed"
        | Household.Deck -> "deck"
        | Household.Desk -> "desk"
        | Household.DetectorSmoke -> "detector_smoke"
        | Household.DineHeart -> "dine_heart"
        | Household.DineLamp -> "dine_lamp"
        | Household.Dining -> "dining"
        | Household.Dishwasher -> "dishwasher"
        | Household.DishwasherGen -> "dishwasher_gen"
        | Household.DoorBack -> "door_back"
        | Household.DoorFront -> "door_front"
        | Household.DoorSliding -> "door_sliding"
        | Household.Doorbell -> "doorbell"
        | Household.Doorbell3p -> "doorbell_3p"
        | Household.Dresser -> "dresser"
        | Household.Dry -> "dry"
        | Household.ElectricBolt -> "electric_bolt"
        | Household.ElectricMeter -> "electric_meter"
        | Household.EmergencyHeat -> "emergency_heat"
        | Household.EmergencyHeat2 -> "emergency_heat_2"
        | Household.EmergencyHome -> "emergency_home"
        | Household.EmergencyRecording -> "emergency_recording"
        | Household.EmergencyShare -> "emergency_share"
        | Household.EmergencyShareOff -> "emergency_share_off"
        | Household.EnergyProgramSaving -> "energy_program_saving"
        | Household.EnergyProgramTimeUsed -> "energy_program_time_used"
        | Household.EnergySavingsLeaf -> "energy_savings_leaf"
        | Household.EventSeat -> "event_seat"
        | Household.FamilyHome -> "family_home"
        | Household.Faucet -> "faucet"
        | Household.Fence -> "fence"
        | Household.FireExtinguisher -> "fire_extinguisher"
        | Household.Fireplace -> "fireplace"
        | Household.Flatware -> "flatware"
        | Household.ForkSpoon -> "fork_spoon"
        | Household.Foundation -> "foundation"
        | Household.Fragrance -> "fragrance"
        | Household.Garage -> "garage"
        | Household.GarageDoor -> "garage_door"
        | Household.GarageHome -> "garage_home"
        | Household.GasMeter -> "gas_meter"
        | Household.Gate -> "gate"
        | Household.Grass -> "grass"
        | Household.Grocery -> "grocery"
        | Household.Hallway -> "hallway"
        | Household.Hardware -> "hardware"
        | Household.HealthAndBeauty -> "health_and_beauty"
        | Household.HeatPump -> "heat_pump"
        | Household.HighChair -> "high_chair"
        | Household.Highlight -> "highlight"
        | Household.HomeAndGarden -> "home_and_garden"
        | Household.HomeImprovementAndTools -> "home_improvement_and_tools"
        | Household.HomeIotDevice -> "home_iot_device"
        | Household.HotTub -> "hot_tub"
        | Household.House -> "house"
        | Household.HouseSiding -> "house_siding"
        | Household.HouseholdSupplies -> "household_supplies"
        | Household.HumidityHigh -> "humidity_high"
        | Household.HumidityLow -> "humidity_low"
        | Household.HumidityMid -> "humidity_mid"
        | Household.Hvac -> "hvac"
        | Household.InHomeMode -> "in_home_mode"
        | Household.Iron -> "iron"
        | Household.Kettle -> "kettle"
        | Household.KingBed -> "king_bed"
        | Household.Kitchen -> "kitchen"
        | Household.Light -> "light"
        | Household.LightOff -> "light_off"
        | Household.Lightbulb2 -> "lightbulb_2"
        | Household.Living -> "living"
        | Household.Matter -> "matter"
        | Household.Microwave -> "microwave"
        | Household.MicrowaveGen -> "microwave_gen"
        | Household.ModeCool -> "mode_cool"
        | Household.ModeCoolOff -> "mode_cool_off"
        | Household.ModeFan -> "mode_fan"
        | Household.ModeFanOff -> "mode_fan_off"
        | Household.ModeHeat -> "mode_heat"
        | Household.ModeHeatCool -> "mode_heat_cool"
        | Household.ModeHeatOff -> "mode_heat_off"
        | Household.ModeNight -> "mode_night"
        | Household.ModeOffOn -> "mode_off_on"
        | Household.Mop -> "mop"
        | Household.Multicooker -> "multicooker"
        | Household.OutdoorGrill -> "outdoor_grill"
        | Household.Outlet -> "outlet"
        | Household.Oven -> "oven"
        | Household.OvenGen -> "oven_gen"
        | Household.Propane -> "propane"
        | Household.PropaneTank -> "propane_tank"
        | Household.RangeHood -> "range_hood"
        | Household.RemoteGen -> "remote_gen"
        | Household.RollerShades -> "roller_shades"
        | Household.RollerShadesClosed -> "roller_shades_closed"
        | Household.Roofing -> "roofing"
        | Household.Scene -> "scene"
        | Household.SensorDoor -> "sensor_door"
        | Household.SensorOccupied -> "sensor_occupied"
        | Household.SensorWindow -> "sensor_window"
        | Household.Sensors -> "sensors"
        | Household.SensorsOff -> "sensors_off"
        | Household.Shelves -> "shelves"
        | Household.ShieldMoon -> "shield_moon"
        | Household.Shower -> "shower"
        | Household.SingleBed -> "single_bed"
        | Household.Skillet -> "skillet"
        | Household.SkilletCooktop -> "skillet_cooktop"
        | Household.SmartOutlet -> "smart_outlet"
        | Household.Soap -> "soap"
        | Household.Sprinkler -> "sprinkler"
        | Household.Stockpot -> "stockpot"
        | Household.Stroller -> "stroller"
        | Household.Styler -> "styler"
        | Household.Switch -> "switch"
        | Household.TableBar -> "table_bar"
        | Household.TableLarge -> "table_large"
        | Household.TableRestaurant -> "table_restaurant"
        | Household.TamperDetectionOff -> "tamper_detection_off"
        | Household.Thermometer -> "thermometer"
        | Household.ThermometerAdd -> "thermometer_add"
        | Household.ThermometerAlert -> "thermometer_alert"
        | Household.ThermometerGain -> "thermometer_gain"
        | Household.ThermometerLoss -> "thermometer_loss"
        | Household.ThermometerMinus -> "thermometer_minus"
        | Household.ThermostatAuto -> "thermostat_auto"
        | Household.ThermostatCarbon -> "thermostat_carbon"
        | Household.TvGen -> "tv_gen"
        | Household.TvWithAssistant -> "tv_with_assistant"
        | Household.Umbrella -> "umbrella"
        | Household.Vacuum -> "vacuum"
        | Household.Valve -> "valve"
        | Household.VerticalShades -> "vertical_shades"
        | Household.VerticalShadesClosed -> "vertical_shades_closed"
        | Household.WallArt -> "wall_art"
        | Household.Wash -> "wash"
        | Household.WaterDamage -> "water_damage"
        | Household.WaterHeater -> "water_heater"
        | Household.Weekend -> "weekend"
        | Household.Window -> "window"
        | Household.Yard -> "yard"

    [<RequireQualifiedAccess; Struct>]
    type Images =
      | ``10mp``
      | ``11mp``
      | ``12mp``
      | ``13mp``
      | ``14mp``
      | ``15mp``
      | ``16mp``
      | ``17mp``
      | ``18mp``
      | ``19mp``
      | ``20mp``
      | ``21mp``
      | ``22mp``
      | ``23mp``
      | ``24fpsSelect``
      | ``24mp``
      | ``2mp``
      | ``30fpsSelect``
      | ``3mp``
      | ``4mp``
      | ``50mp``
      | ``5mp``
      | ``60fpsSelect``
      | ``6mp``
      | ``7mp``
      | ``8mp``
      | ``9mp``
      | AddAPhoto
      | AddPhotoAlternate
      | Adjust
      | Animation
      | AspectRatio
      | AutoAwesomeMosaic
      | AutoAwesomeMotion
      | AutoStories
      | AutoStoriesOff
      | AutofpsSelect
      | BackgroundDotLarge
      | BackgroundDotSmall
      | BackgroundGridSmall
      | BlurCircular
      | BlurLinear
      | BlurMedium
      | BlurOff
      | BlurOn
      | BlurShort
      | Brightness1
      | Brightness2
      | Brightness3
      | Brightness4
      | Brightness5
      | Brightness6
      | Brightness7
      | BrokenImage
      | Brush
      | BurstMode
      | Camera
      | CameraRoll
      | CenterFocusStrong
      | CenterFocusWeak
      | Circle
      | Colorize
      | Compare
      | Contrast
      | ContrastCircle
      | ContrastRtlOff
      | ContrastSquare
      | ControlPointDuplicate
      | Crop
      | Crop169
      | Crop32
      | Crop54
      | Crop75
      | Crop916
      | CropFree
      | CropLandscape
      | CropPortrait
      | CropRotate
      | CropSquare
      | Deblur
      | Dehaze
      | Details
      | DirtyLens
      | DropperEye
      | Edit
      | EvShadow
      | EvShadowAdd
      | EvShadowMinus
      | Exposure
      | ExposureNeg1
      | ExposureNeg2
      | ExposurePlus1
      | ExposurePlus2
      | ExposureZero
      | FaceRetouchingOff
      | FilePng
      | Filter
      | Filter1
      | Filter2
      | Filter3
      | Filter4
      | Filter5
      | Filter6
      | Filter7
      | Filter8
      | Filter9
      | Filter9Plus
      | FilterBAndW
      | FilterCenterFocus
      | FilterDrama
      | FilterFrames
      | FilterNone
      | FilterRetrolux
      | FilterTiltShift
      | FilterVintage
      | Flaky
      | Flare
      | FlashAuto
      | FlashOff
      | FlashOn
      | Flip
      | FlipCameraAndroid
      | FlipCameraIos
      | Fluorescent
      | GalleryThumbnail
      | Gif
      | Gif2
      | GifBox
      | Gradient
      | Grain
      | GridOff
      | GridOn
      | HdrAuto
      | HdrAutoSelect
      | HdrEnhancedSelect
      | HdrOff
      | HdrOffSelect
      | HdrOn
      | HdrOnSelect
      | HdrPlus
      | HdrPlusOff
      | HdrStrong
      | HdrWeak
      | Healing
      | Hevc
      | HideImage
      | HighDensity
      | HighRes
      | Image
      | ImageArrowUp
      | ImageAspectRatio
      | ImageInset
      | ImageSearch
      | Imagesmode
      | IncompleteCircle
      | InvertColors
      | InvertColorsOff
      | Landscape
      | Landscape2
      | Landscape2Edit
      | Landscape2Off
      | LeakAdd
      | LeakRemove
      | LensBlur
      | LinkedCamera
      | Looks
      | Looks3
      | Looks4
      | Looks5
      | Looks6
      | LooksOne
      | LooksTwo
      | Loupe
      | LowDensity
      | MacroAuto
      | MacroOff
      | MaskedTransitions
      | MaskedTransitionsAdd
      | MicExternalOff
      | MicExternalOn
      | MobileCameraFront
      | MobileCameraRear
      | MonochromePhotos
      | MotionBlur
      | MotionMode
      | MotionPhotosAuto
      | MotionPhotosOn
      | MotionPhotosPaused
      | MotionPlay
      | Mp
      | Nature
      | NaturePeople
      | NightSightAuto
      | NightSightAutoOff
      | NoFlash
      | NoPhotography
      | Opacity
      | Palette
      | Panorama
      | PanoramaHorizontal
      | PanoramaPhotosphere
      | PanoramaVertical
      | PanoramaWideAngle
      | PartyMode
      | PermCameraMic
      | Photo
      | PhotoAlbum
      | PhotoAutoMerge
      | PhotoCamera
      | PhotoCameraBack
      | PhotoCameraFront
      | PhotoFrame
      | PhotoLibrary
      | PhotoPrints
      | PhotoSizeSelectLarge
      | PhotoSizeSelectSmall
      | PictureAsPdf
      | PlannerBannerAdPt
      | RawOff
      | RawOn
      | ResetBrightness
      | ResetExposure
      | ResetFocus
      | ResetIso
      | ResetSettings
      | ResetShadow
      | ResetShutterSpeed
      | ResetWhiteBalance
      | Rotate90DegreesCcw
      | Rotate90DegreesCw
      | RotateLeft
      | RotateRight
      | SettingsBRoll
      | SettingsBrightness
      | SettingsCinematicBlur
      | SettingsMotionMode
      | SettingsNightSight
      | SettingsPanorama
      | SettingsPhotoCamera
      | SettingsSlowMotion
      | SettingsTimelapse
      | SettingsVideoCamera
      | ShutterSpeed
      | ShutterSpeedAdd
      | ShutterSpeedMinus
      | Slideshow
      | Straighten
      | Style
      | SwitchCamera
      | SwitchVideo
      | Texture
      | TextureAdd
      | TextureMinus
      | Timelapse
      | Timer
      | Timer1
      | Timer10
      | Timer2
      | Timer3
      | TimerOff
      | Tonality
      | Tonality2
      | TrailLength
      | TrailLengthMedium
      | TrailLengthShort
      | Transform
      | TransitionChop
      | TransitionDissolve
      | TransitionFade
      | TransitionPush
      | TransitionSlide
      | Tune
      | Unknown2
      | ViewComfy
      | ViewCompact
      | ViewRealSize
      | Vignette
      | Vignette2
      | Vr180Create2d
      | Vr180Create2dOff
      | Vrpano
      | WbAuto
      | WbIncandescent
      | WbIridescent
      | WbShade
      | WbSunny
      | WbTwilight
      | WebStories

    module Images =

      let toSnakeCase (icon: Images) =
        match icon with
        | Images.``10mp`` -> "10mp"
        | Images.``11mp`` -> "11mp"
        | Images.``12mp`` -> "12mp"
        | Images.``13mp`` -> "13mp"
        | Images.``14mp`` -> "14mp"
        | Images.``15mp`` -> "15mp"
        | Images.``16mp`` -> "16mp"
        | Images.``17mp`` -> "17mp"
        | Images.``18mp`` -> "18mp"
        | Images.``19mp`` -> "19mp"
        | Images.``20mp`` -> "20mp"
        | Images.``21mp`` -> "21mp"
        | Images.``22mp`` -> "22mp"
        | Images.``23mp`` -> "23mp"
        | Images.``24fpsSelect`` -> "24fps_select"
        | Images.``24mp`` -> "24mp"
        | Images.``2mp`` -> "2mp"
        | Images.``30fpsSelect`` -> "30fps_select"
        | Images.``3mp`` -> "3mp"
        | Images.``4mp`` -> "4mp"
        | Images.``50mp`` -> "50mp"
        | Images.``5mp`` -> "5mp"
        | Images.``60fpsSelect`` -> "60fps_select"
        | Images.``6mp`` -> "6mp"
        | Images.``7mp`` -> "7mp"
        | Images.``8mp`` -> "8mp"
        | Images.``9mp`` -> "9mp"
        | Images.AddAPhoto -> "add_a_photo"
        | Images.AddPhotoAlternate -> "add_photo_alternate"
        | Images.Adjust -> "adjust"
        | Images.Animation -> "animation"
        | Images.AspectRatio -> "aspect_ratio"
        | Images.AutoAwesomeMosaic -> "auto_awesome_mosaic"
        | Images.AutoAwesomeMotion -> "auto_awesome_motion"
        | Images.AutoStories -> "auto_stories"
        | Images.AutoStoriesOff -> "auto_stories_off"
        | Images.AutofpsSelect -> "autofps_select"
        | Images.BackgroundDotLarge -> "background_dot_large"
        | Images.BackgroundDotSmall -> "background_dot_small"
        | Images.BackgroundGridSmall -> "background_grid_small"
        | Images.BlurCircular -> "blur_circular"
        | Images.BlurLinear -> "blur_linear"
        | Images.BlurMedium -> "blur_medium"
        | Images.BlurOff -> "blur_off"
        | Images.BlurOn -> "blur_on"
        | Images.BlurShort -> "blur_short"
        | Images.Brightness1 -> "brightness_1"
        | Images.Brightness2 -> "brightness_2"
        | Images.Brightness3 -> "brightness_3"
        | Images.Brightness4 -> "brightness_4"
        | Images.Brightness5 -> "brightness_5"
        | Images.Brightness6 -> "brightness_6"
        | Images.Brightness7 -> "brightness_7"
        | Images.BrokenImage -> "broken_image"
        | Images.Brush -> "brush"
        | Images.BurstMode -> "burst_mode"
        | Images.Camera -> "camera"
        | Images.CameraRoll -> "camera_roll"
        | Images.CenterFocusStrong -> "center_focus_strong"
        | Images.CenterFocusWeak -> "center_focus_weak"
        | Images.Circle -> "circle"
        | Images.Colorize -> "colorize"
        | Images.Compare -> "compare"
        | Images.Contrast -> "contrast"
        | Images.ContrastCircle -> "contrast_circle"
        | Images.ContrastRtlOff -> "contrast_rtl_off"
        | Images.ContrastSquare -> "contrast_square"
        | Images.ControlPointDuplicate -> "control_point_duplicate"
        | Images.Crop -> "crop"
        | Images.Crop169 -> "crop_16_9"
        | Images.Crop32 -> "crop_3_2"
        | Images.Crop54 -> "crop_5_4"
        | Images.Crop75 -> "crop_7_5"
        | Images.Crop916 -> "crop_9_16"
        | Images.CropFree -> "crop_free"
        | Images.CropLandscape -> "crop_landscape"
        | Images.CropPortrait -> "crop_portrait"
        | Images.CropRotate -> "crop_rotate"
        | Images.CropSquare -> "crop_square"
        | Images.Deblur -> "deblur"
        | Images.Dehaze -> "dehaze"
        | Images.Details -> "details"
        | Images.DirtyLens -> "dirty_lens"
        | Images.DropperEye -> "dropper_eye"
        | Images.Edit -> "edit"
        | Images.EvShadow -> "ev_shadow"
        | Images.EvShadowAdd -> "ev_shadow_add"
        | Images.EvShadowMinus -> "ev_shadow_minus"
        | Images.Exposure -> "exposure"
        | Images.ExposureNeg1 -> "exposure_neg_1"
        | Images.ExposureNeg2 -> "exposure_neg_2"
        | Images.ExposurePlus1 -> "exposure_plus_1"
        | Images.ExposurePlus2 -> "exposure_plus_2"
        | Images.ExposureZero -> "exposure_zero"
        | Images.FaceRetouchingOff -> "face_retouching_off"
        | Images.FilePng -> "file_png"
        | Images.Filter -> "filter"
        | Images.Filter1 -> "filter_1"
        | Images.Filter2 -> "filter_2"
        | Images.Filter3 -> "filter_3"
        | Images.Filter4 -> "filter_4"
        | Images.Filter5 -> "filter_5"
        | Images.Filter6 -> "filter_6"
        | Images.Filter7 -> "filter_7"
        | Images.Filter8 -> "filter_8"
        | Images.Filter9 -> "filter_9"
        | Images.Filter9Plus -> "filter_9_plus"
        | Images.FilterBAndW -> "filter_b_and_w"
        | Images.FilterCenterFocus -> "filter_center_focus"
        | Images.FilterDrama -> "filter_drama"
        | Images.FilterFrames -> "filter_frames"
        | Images.FilterNone -> "filter_none"
        | Images.FilterRetrolux -> "filter_retrolux"
        | Images.FilterTiltShift -> "filter_tilt_shift"
        | Images.FilterVintage -> "filter_vintage"
        | Images.Flaky -> "flaky"
        | Images.Flare -> "flare"
        | Images.FlashAuto -> "flash_auto"
        | Images.FlashOff -> "flash_off"
        | Images.FlashOn -> "flash_on"
        | Images.Flip -> "flip"
        | Images.FlipCameraAndroid -> "flip_camera_android"
        | Images.FlipCameraIos -> "flip_camera_ios"
        | Images.Fluorescent -> "fluorescent"
        | Images.GalleryThumbnail -> "gallery_thumbnail"
        | Images.Gif -> "gif"
        | Images.Gif2 -> "gif_2"
        | Images.GifBox -> "gif_box"
        | Images.Gradient -> "gradient"
        | Images.Grain -> "grain"
        | Images.GridOff -> "grid_off"
        | Images.GridOn -> "grid_on"
        | Images.HdrAuto -> "hdr_auto"
        | Images.HdrAutoSelect -> "hdr_auto_select"
        | Images.HdrEnhancedSelect -> "hdr_enhanced_select"
        | Images.HdrOff -> "hdr_off"
        | Images.HdrOffSelect -> "hdr_off_select"
        | Images.HdrOn -> "hdr_on"
        | Images.HdrOnSelect -> "hdr_on_select"
        | Images.HdrPlus -> "hdr_plus"
        | Images.HdrPlusOff -> "hdr_plus_off"
        | Images.HdrStrong -> "hdr_strong"
        | Images.HdrWeak -> "hdr_weak"
        | Images.Healing -> "healing"
        | Images.Hevc -> "hevc"
        | Images.HideImage -> "hide_image"
        | Images.HighDensity -> "high_density"
        | Images.HighRes -> "high_res"
        | Images.Image -> "image"
        | Images.ImageArrowUp -> "image_arrow_up"
        | Images.ImageAspectRatio -> "image_aspect_ratio"
        | Images.ImageInset -> "image_inset"
        | Images.ImageSearch -> "image_search"
        | Images.Imagesmode -> "imagesmode"
        | Images.IncompleteCircle -> "incomplete_circle"
        | Images.InvertColors -> "invert_colors"
        | Images.InvertColorsOff -> "invert_colors_off"
        | Images.Landscape -> "landscape"
        | Images.Landscape2 -> "landscape_2"
        | Images.Landscape2Edit -> "landscape_2_edit"
        | Images.Landscape2Off -> "landscape_2_off"
        | Images.LeakAdd -> "leak_add"
        | Images.LeakRemove -> "leak_remove"
        | Images.LensBlur -> "lens_blur"
        | Images.LinkedCamera -> "linked_camera"
        | Images.Looks -> "looks"
        | Images.Looks3 -> "looks_3"
        | Images.Looks4 -> "looks_4"
        | Images.Looks5 -> "looks_5"
        | Images.Looks6 -> "looks_6"
        | Images.LooksOne -> "looks_one"
        | Images.LooksTwo -> "looks_two"
        | Images.Loupe -> "loupe"
        | Images.LowDensity -> "low_density"
        | Images.MacroAuto -> "macro_auto"
        | Images.MacroOff -> "macro_off"
        | Images.MaskedTransitions -> "masked_transitions"
        | Images.MaskedTransitionsAdd -> "masked_transitions_add"
        | Images.MicExternalOff -> "mic_external_off"
        | Images.MicExternalOn -> "mic_external_on"
        | Images.MobileCameraFront -> "mobile_camera_front"
        | Images.MobileCameraRear -> "mobile_camera_rear"
        | Images.MonochromePhotos -> "monochrome_photos"
        | Images.MotionBlur -> "motion_blur"
        | Images.MotionMode -> "motion_mode"
        | Images.MotionPhotosAuto -> "motion_photos_auto"
        | Images.MotionPhotosOn -> "motion_photos_on"
        | Images.MotionPhotosPaused -> "motion_photos_paused"
        | Images.MotionPlay -> "motion_play"
        | Images.Mp -> "mp"
        | Images.Nature -> "nature"
        | Images.NaturePeople -> "nature_people"
        | Images.NightSightAuto -> "night_sight_auto"
        | Images.NightSightAutoOff -> "night_sight_auto_off"
        | Images.NoFlash -> "no_flash"
        | Images.NoPhotography -> "no_photography"
        | Images.Opacity -> "opacity"
        | Images.Palette -> "palette"
        | Images.Panorama -> "panorama"
        | Images.PanoramaHorizontal -> "panorama_horizontal"
        | Images.PanoramaPhotosphere -> "panorama_photosphere"
        | Images.PanoramaVertical -> "panorama_vertical"
        | Images.PanoramaWideAngle -> "panorama_wide_angle"
        | Images.PartyMode -> "party_mode"
        | Images.PermCameraMic -> "perm_camera_mic"
        | Images.Photo -> "photo"
        | Images.PhotoAlbum -> "photo_album"
        | Images.PhotoAutoMerge -> "photo_auto_merge"
        | Images.PhotoCamera -> "photo_camera"
        | Images.PhotoCameraBack -> "photo_camera_back"
        | Images.PhotoCameraFront -> "photo_camera_front"
        | Images.PhotoFrame -> "photo_frame"
        | Images.PhotoLibrary -> "photo_library"
        | Images.PhotoPrints -> "photo_prints"
        | Images.PhotoSizeSelectLarge -> "photo_size_select_large"
        | Images.PhotoSizeSelectSmall -> "photo_size_select_small"
        | Images.PictureAsPdf -> "picture_as_pdf"
        | Images.PlannerBannerAdPt -> "planner_banner_ad_pt"
        | Images.RawOff -> "raw_off"
        | Images.RawOn -> "raw_on"
        | Images.ResetBrightness -> "reset_brightness"
        | Images.ResetExposure -> "reset_exposure"
        | Images.ResetFocus -> "reset_focus"
        | Images.ResetIso -> "reset_iso"
        | Images.ResetSettings -> "reset_settings"
        | Images.ResetShadow -> "reset_shadow"
        | Images.ResetShutterSpeed -> "reset_shutter_speed"
        | Images.ResetWhiteBalance -> "reset_white_balance"
        | Images.Rotate90DegreesCcw -> "rotate_90_degrees_ccw"
        | Images.Rotate90DegreesCw -> "rotate_90_degrees_cw"
        | Images.RotateLeft -> "rotate_left"
        | Images.RotateRight -> "rotate_right"
        | Images.SettingsBRoll -> "settings_b_roll"
        | Images.SettingsBrightness -> "settings_brightness"
        | Images.SettingsCinematicBlur -> "settings_cinematic_blur"
        | Images.SettingsMotionMode -> "settings_motion_mode"
        | Images.SettingsNightSight -> "settings_night_sight"
        | Images.SettingsPanorama -> "settings_panorama"
        | Images.SettingsPhotoCamera -> "settings_photo_camera"
        | Images.SettingsSlowMotion -> "settings_slow_motion"
        | Images.SettingsTimelapse -> "settings_timelapse"
        | Images.SettingsVideoCamera -> "settings_video_camera"
        | Images.ShutterSpeed -> "shutter_speed"
        | Images.ShutterSpeedAdd -> "shutter_speed_add"
        | Images.ShutterSpeedMinus -> "shutter_speed_minus"
        | Images.Slideshow -> "slideshow"
        | Images.Straighten -> "straighten"
        | Images.Style -> "style"
        | Images.SwitchCamera -> "switch_camera"
        | Images.SwitchVideo -> "switch_video"
        | Images.Texture -> "texture"
        | Images.TextureAdd -> "texture_add"
        | Images.TextureMinus -> "texture_minus"
        | Images.Timelapse -> "timelapse"
        | Images.Timer -> "timer"
        | Images.Timer1 -> "timer_1"
        | Images.Timer10 -> "timer_10"
        | Images.Timer2 -> "timer_2"
        | Images.Timer3 -> "timer_3"
        | Images.TimerOff -> "timer_off"
        | Images.Tonality -> "tonality"
        | Images.Tonality2 -> "tonality_2"
        | Images.TrailLength -> "trail_length"
        | Images.TrailLengthMedium -> "trail_length_medium"
        | Images.TrailLengthShort -> "trail_length_short"
        | Images.Transform -> "transform"
        | Images.TransitionChop -> "transition_chop"
        | Images.TransitionDissolve -> "transition_dissolve"
        | Images.TransitionFade -> "transition_fade"
        | Images.TransitionPush -> "transition_push"
        | Images.TransitionSlide -> "transition_slide"
        | Images.Tune -> "tune"
        | Images.Unknown2 -> "unknown_2"
        | Images.ViewComfy -> "view_comfy"
        | Images.ViewCompact -> "view_compact"
        | Images.ViewRealSize -> "view_real_size"
        | Images.Vignette -> "vignette"
        | Images.Vignette2 -> "vignette_2"
        | Images.Vr180Create2d -> "vr180_create_2d"
        | Images.Vr180Create2dOff -> "vr180_create_2d_off"
        | Images.Vrpano -> "vrpano"
        | Images.WbAuto -> "wb_auto"
        | Images.WbIncandescent -> "wb_incandescent"
        | Images.WbIridescent -> "wb_iridescent"
        | Images.WbShade -> "wb_shade"
        | Images.WbSunny -> "wb_sunny"
        | Images.WbTwilight -> "wb_twilight"
        | Images.WebStories -> "web_stories"

    [<RequireQualifiedAccess; Struct>]
    type Maps =
      | ``360``
      | AddHome
      | AddHomeWork
      | AddLocation
      | AddLocationAlt
      | AddRoad
      | AddTriangle
      | AirlineStops
      | AltRoute
      | AssistWalker
      | AwardMeal
      | BabyChangingStation
      | Beenhere
      | BusinessCenter
      | CalendarMeal2
      | Castle
      | Church
      | CleaningServices
      | CompassCalibration
      | ConnectingAirports
      | CrisisAlert
      | Directions
      | DirectionsAlt
      | DirectionsAltOff
      | DirectionsOff
      | DryCleaning
      | East
      | EditAttributes
      | EditLocation
      | EditLocationAlt
      | EditRoad
      | ElectricalServices
      | Emergency
      | EvStation
      | Explore
      | ExploreNearby
      | ExploreOff
      | Factory
      | Fastfood
      | FileMap
      | FileMapStack
      | FireHydrant
      | FireTruck
      | Flag
      | Flag2
      | FlagCheck
      | FlagCircle
      | FlightClass
      | FmdBad
      | Fort
      | Globe
      | GlobeAsia
      | GlobeLocationPin
      | GlobeUk
      | HanamiDango
      | Handyman
      | HomePin
      | HomeRepairService
      | HomeWork
      | KanjiAlcohol
      | KebabDining
      | Layers
      | LayersClear
      | LocalActivity
      | LocalAtm
      | LocalCarWash
      | LocalConvenienceStore
      | LocalDrink
      | LocalFireDepartment
      | LocalFlorist
      | LocalGasStation
      | LocalHospital
      | LocalLaundryService
      | LocalLibrary
      | LocalMall
      | LocalParking
      | LocalPharmacy
      | LocalPizza
      | LocalPolice
      | LocalPostOffice
      | LocalSee
      | LocationAway
      | LocationDisabled
      | LocationHome
      | LocationOff
      | LocationOn
      | LocationSearching
      | Map
      | MapPinHeart
      | MapPinReview
      | MapSearch
      | MapsUgc
      | MealDinner
      | MealLunch
      | MedicalServices
      | MinorCrash
      | ModeOfTravel
      | Mosque
      | Move
      | MoveLocation
      | MovedLocation
      | Moving
      | MovingMinistry
      | MultipleAirports
      | MultipleStop
      | MyLocation
      | Navigation
      | NearMe
      | NearMeDisabled
      | NoMeals
      | North
      | NorthEast
      | NorthWest
      | NotListedLocation
      | Package
      | Package2
      | ParentChildDining
      | Park
      | Pergola
      | PersonPin
      | PersonPinCircle
      | PestControl
      | PestControlRodent
      | PetSupplies
      | PinDrop
      | Plumbing
      | RemoveRoad
      | RestArea
      | Restaurant
      | RunCircle
      | SafetyCheck
      | SafetyCheckOff
      | Satellite
      | SetMeal
      | ShareEta
      | ShareLocation
      | ShavedIce
      | Signpost
      | Soba
      | SoloDining
      | Sos
      | SoupKitchen
      | South
      | SouthEast
      | SouthWest
      | Stadium
      | Streetview
      | Synagogue
      | TakeoutDining
      | TakeoutDining2
      | TatamiSeat
      | TempleBuddhist
      | TempleHindu
      | TheaterComedy
      | ThingsToDo
      | Tour
      | Traffic
      | TransferWithinAStation
      | TransitEnterexit
      | TripOrigin
      | Udon
      | UniversalLocal
      | Warehouse
      | West
      | WhereToVote
      | WineBar
      | WrongLocation
      | Yakitori
      | ZoomInMap
      | ZoomOutMap

    module Maps =

      let toSnakeCase (icon: Maps) =
        match icon with
        | Maps.``360`` -> "360"
        | Maps.AddHome -> "add_home"
        | Maps.AddHomeWork -> "add_home_work"
        | Maps.AddLocation -> "add_location"
        | Maps.AddLocationAlt -> "add_location_alt"
        | Maps.AddRoad -> "add_road"
        | Maps.AddTriangle -> "add_triangle"
        | Maps.AirlineStops -> "airline_stops"
        | Maps.AltRoute -> "alt_route"
        | Maps.AssistWalker -> "assist_walker"
        | Maps.AwardMeal -> "award_meal"
        | Maps.BabyChangingStation -> "baby_changing_station"
        | Maps.Beenhere -> "beenhere"
        | Maps.BusinessCenter -> "business_center"
        | Maps.CalendarMeal2 -> "calendar_meal_2"
        | Maps.Castle -> "castle"
        | Maps.Church -> "church"
        | Maps.CleaningServices -> "cleaning_services"
        | Maps.CompassCalibration -> "compass_calibration"
        | Maps.ConnectingAirports -> "connecting_airports"
        | Maps.CrisisAlert -> "crisis_alert"
        | Maps.Directions -> "directions"
        | Maps.DirectionsAlt -> "directions_alt"
        | Maps.DirectionsAltOff -> "directions_alt_off"
        | Maps.DirectionsOff -> "directions_off"
        | Maps.DryCleaning -> "dry_cleaning"
        | Maps.East -> "east"
        | Maps.EditAttributes -> "edit_attributes"
        | Maps.EditLocation -> "edit_location"
        | Maps.EditLocationAlt -> "edit_location_alt"
        | Maps.EditRoad -> "edit_road"
        | Maps.ElectricalServices -> "electrical_services"
        | Maps.Emergency -> "emergency"
        | Maps.EvStation -> "ev_station"
        | Maps.Explore -> "explore"
        | Maps.ExploreNearby -> "explore_nearby"
        | Maps.ExploreOff -> "explore_off"
        | Maps.Factory -> "factory"
        | Maps.Fastfood -> "fastfood"
        | Maps.FileMap -> "file_map"
        | Maps.FileMapStack -> "file_map_stack"
        | Maps.FireHydrant -> "fire_hydrant"
        | Maps.FireTruck -> "fire_truck"
        | Maps.Flag -> "flag"
        | Maps.Flag2 -> "flag_2"
        | Maps.FlagCheck -> "flag_check"
        | Maps.FlagCircle -> "flag_circle"
        | Maps.FlightClass -> "flight_class"
        | Maps.FmdBad -> "fmd_bad"
        | Maps.Fort -> "fort"
        | Maps.Globe -> "globe"
        | Maps.GlobeAsia -> "globe_asia"
        | Maps.GlobeLocationPin -> "globe_location_pin"
        | Maps.GlobeUk -> "globe_uk"
        | Maps.HanamiDango -> "hanami_dango"
        | Maps.Handyman -> "handyman"
        | Maps.HomePin -> "home_pin"
        | Maps.HomeRepairService -> "home_repair_service"
        | Maps.HomeWork -> "home_work"
        | Maps.KanjiAlcohol -> "kanji_alcohol"
        | Maps.KebabDining -> "kebab_dining"
        | Maps.Layers -> "layers"
        | Maps.LayersClear -> "layers_clear"
        | Maps.LocalActivity -> "local_activity"
        | Maps.LocalAtm -> "local_atm"
        | Maps.LocalCarWash -> "local_car_wash"
        | Maps.LocalConvenienceStore -> "local_convenience_store"
        | Maps.LocalDrink -> "local_drink"
        | Maps.LocalFireDepartment -> "local_fire_department"
        | Maps.LocalFlorist -> "local_florist"
        | Maps.LocalGasStation -> "local_gas_station"
        | Maps.LocalHospital -> "local_hospital"
        | Maps.LocalLaundryService -> "local_laundry_service"
        | Maps.LocalLibrary -> "local_library"
        | Maps.LocalMall -> "local_mall"
        | Maps.LocalParking -> "local_parking"
        | Maps.LocalPharmacy -> "local_pharmacy"
        | Maps.LocalPizza -> "local_pizza"
        | Maps.LocalPolice -> "local_police"
        | Maps.LocalPostOffice -> "local_post_office"
        | Maps.LocalSee -> "local_see"
        | Maps.LocationAway -> "location_away"
        | Maps.LocationDisabled -> "location_disabled"
        | Maps.LocationHome -> "location_home"
        | Maps.LocationOff -> "location_off"
        | Maps.LocationOn -> "location_on"
        | Maps.LocationSearching -> "location_searching"
        | Maps.Map -> "map"
        | Maps.MapPinHeart -> "map_pin_heart"
        | Maps.MapPinReview -> "map_pin_review"
        | Maps.MapSearch -> "map_search"
        | Maps.MapsUgc -> "maps_ugc"
        | Maps.MealDinner -> "meal_dinner"
        | Maps.MealLunch -> "meal_lunch"
        | Maps.MedicalServices -> "medical_services"
        | Maps.MinorCrash -> "minor_crash"
        | Maps.ModeOfTravel -> "mode_of_travel"
        | Maps.Mosque -> "mosque"
        | Maps.Move -> "move"
        | Maps.MoveLocation -> "move_location"
        | Maps.MovedLocation -> "moved_location"
        | Maps.Moving -> "moving"
        | Maps.MovingMinistry -> "moving_ministry"
        | Maps.MultipleAirports -> "multiple_airports"
        | Maps.MultipleStop -> "multiple_stop"
        | Maps.MyLocation -> "my_location"
        | Maps.Navigation -> "navigation"
        | Maps.NearMe -> "near_me"
        | Maps.NearMeDisabled -> "near_me_disabled"
        | Maps.NoMeals -> "no_meals"
        | Maps.North -> "north"
        | Maps.NorthEast -> "north_east"
        | Maps.NorthWest -> "north_west"
        | Maps.NotListedLocation -> "not_listed_location"
        | Maps.Package -> "package"
        | Maps.Package2 -> "package_2"
        | Maps.ParentChildDining -> "parent_child_dining"
        | Maps.Park -> "park"
        | Maps.Pergola -> "pergola"
        | Maps.PersonPin -> "person_pin"
        | Maps.PersonPinCircle -> "person_pin_circle"
        | Maps.PestControl -> "pest_control"
        | Maps.PestControlRodent -> "pest_control_rodent"
        | Maps.PetSupplies -> "pet_supplies"
        | Maps.PinDrop -> "pin_drop"
        | Maps.Plumbing -> "plumbing"
        | Maps.RemoveRoad -> "remove_road"
        | Maps.RestArea -> "rest_area"
        | Maps.Restaurant -> "restaurant"
        | Maps.RunCircle -> "run_circle"
        | Maps.SafetyCheck -> "safety_check"
        | Maps.SafetyCheckOff -> "safety_check_off"
        | Maps.Satellite -> "satellite"
        | Maps.SetMeal -> "set_meal"
        | Maps.ShareEta -> "share_eta"
        | Maps.ShareLocation -> "share_location"
        | Maps.ShavedIce -> "shaved_ice"
        | Maps.Signpost -> "signpost"
        | Maps.Soba -> "soba"
        | Maps.SoloDining -> "solo_dining"
        | Maps.Sos -> "sos"
        | Maps.SoupKitchen -> "soup_kitchen"
        | Maps.South -> "south"
        | Maps.SouthEast -> "south_east"
        | Maps.SouthWest -> "south_west"
        | Maps.Stadium -> "stadium"
        | Maps.Streetview -> "streetview"
        | Maps.Synagogue -> "synagogue"
        | Maps.TakeoutDining -> "takeout_dining"
        | Maps.TakeoutDining2 -> "takeout_dining_2"
        | Maps.TatamiSeat -> "tatami_seat"
        | Maps.TempleBuddhist -> "temple_buddhist"
        | Maps.TempleHindu -> "temple_hindu"
        | Maps.TheaterComedy -> "theater_comedy"
        | Maps.ThingsToDo -> "things_to_do"
        | Maps.Tour -> "tour"
        | Maps.Traffic -> "traffic"
        | Maps.TransferWithinAStation -> "transfer_within_a_station"
        | Maps.TransitEnterexit -> "transit_enterexit"
        | Maps.TripOrigin -> "trip_origin"
        | Maps.Udon -> "udon"
        | Maps.UniversalLocal -> "universal_local"
        | Maps.Warehouse -> "warehouse"
        | Maps.West -> "west"
        | Maps.WhereToVote -> "where_to_vote"
        | Maps.WineBar -> "wine_bar"
        | Maps.WrongLocation -> "wrong_location"
        | Maps.Yakitori -> "yakitori"
        | Maps.ZoomInMap -> "zoom_in_map"
        | Maps.ZoomOutMap -> "zoom_out_map"

    [<RequireQualifiedAccess; Struct>]
    type Privacy =
      | Account
      | Ad
      | Approval
      | Calendar
      | Category
      | ChromeReaderMode
      | Code
      | Contacts
      | Dangerous
      | DataLossPrevention
      | DomainVerification
      | DynamicFeed
      | Event
      | Extension
      | Feedback
      | FindReplace
      | Fingerprint
      | Help
      | History
      | Info
      | Label
      | Language
      | Lightbulb
      | Lock
      | ManageAccounts
      | ModelTraining
      | Notification
      | OnlinePrediction
      | Password
      | PermContactCalendar
      | Person
      | PrivacyTip
      | PublishedWithChanges
      | QuestionMark
      | RecordVoiceOver
      | Security
      | Settings
      | Shield
      | Support
      | SupervisorAccount
      | Verified
      | Visibility
      | Warning

    module Privacy =

      let toSnakeCase (icon: Privacy) =
        match icon with
        | Privacy.Account -> "account"
        | Privacy.Ad -> "ad"
        | Privacy.Approval -> "approval"
        | Privacy.Calendar -> "calendar"
        | Privacy.Category -> "category"
        | Privacy.ChromeReaderMode -> "chrome_reader_mode"
        | Privacy.Code -> "code"
        | Privacy.Contacts -> "contacts"
        | Privacy.Dangerous -> "dangerous"
        | Privacy.DataLossPrevention -> "data_loss_prevention"
        | Privacy.DomainVerification -> "domain_verification"
        | Privacy.DynamicFeed -> "dynamic_feed"
        | Privacy.Event -> "event"
        | Privacy.Extension -> "extension"
        | Privacy.Feedback -> "feedback"
        | Privacy.FindReplace -> "find_replace"
        | Privacy.Fingerprint -> "fingerprint"
        | Privacy.Help -> "help"
        | Privacy.History -> "history"
        | Privacy.Info -> "info"
        | Privacy.Label -> "label"
        | Privacy.Language -> "language"
        | Privacy.Lightbulb -> "lightbulb"
        | Privacy.Lock -> "lock"
        | Privacy.ManageAccounts -> "manage_accounts"
        | Privacy.ModelTraining -> "model_training"
        | Privacy.Notification -> "notification"
        | Privacy.OnlinePrediction -> "online_prediction"
        | Privacy.Password -> "password"
        | Privacy.PermContactCalendar -> "perm_contact_calendar"
        | Privacy.Person -> "person"
        | Privacy.PrivacyTip -> "privacy_tip"
        | Privacy.PublishedWithChanges -> "published_with_changes"
        | Privacy.QuestionMark -> "question_mark"
        | Privacy.RecordVoiceOver -> "record_voice_over"
        | Privacy.Security -> "security"
        | Privacy.Settings -> "settings"
        | Privacy.Shield -> "shield"
        | Privacy.Support -> "support"
        | Privacy.SupervisorAccount -> "supervisor_account"
        | Privacy.Verified -> "verified"
        | Privacy.Visibility -> "visibility"
        | Privacy.Warning -> "warning"

    [<RequireQualifiedAccess; Struct>]
    type Social =
      | ``18UpRating``
      | ``6FtApart``
      | Acupuncture
      | AddReaction
      | AdminMeds
      | Agender
      | Allergies
      | Allergy
      | Altitude
      | Antigravity
      | Barefoot
      | Bedtime
      | BedtimeOff
      | Blind
      | BloodPressure
      | Bloodtype
      | BodyFat
      | BodySystem
      | Bomb
      | Boy
      | Breastfeeding
      | Brick
      | BringYourOwnIp
      | CalendarMeal
      | Candle
      | Cannabis
      | CardioLoad
      | Cardiology
      | Cheer
      | ChefHat
      | Chess
      | ChessBishop
      | ChessBishop2
      | ChessKing
      | ChessKing2
      | ChessKnight
      | ChessPawn
      | ChessPawn2
      | ChessQueen
      | ChessRook
      | ChildHat
      | CleanHands
      | ClearDay
      | ClinicalNotes
      | Co2
      | Cognition
      | Cognition2
      | ComedyMask
      | ComicBubble
      | Communication
      | Communities
      | Compost
      | Conditions
      | Congenital
      | ConnectWithoutContact
      | Conversation
      | Cookie
      | CookieOff
      | Coronavirus
      | Crossword
      | Crowdsource
      | Crown
      | CrueltyFree
      | Cyclone
      | Deceased
      | Demography
      | Dentistry
      | Dermatology
      | Destruction
      | DewPoint
      | Diamond
      | DiamondShine
      | DigitalWellbeing
      | DineIn
      | Diversity1
      | Diversity2
      | Diversity3
      | Diversity4
      | DominoMask
      | Drone
      | Drone2
      | Earthquake
      | Eco
      | EditorChoice
      | Egg
      | EggAlt
      | Elderly
      | ElderlyWoman
      | EmojiFoodBeverage
      | EmojiNature
      | EmojiObjects
      | EmojiPeople
      | EmojiSymbols
      | EmojiTransportation
      | Emoticon
      | Endocrinology
      | Ent
      | Explosion
      | Eyebrow
      | Eyeglasses
      | Eyeglasses2
      | Eyeglasses2Sound
      | Face
      | Face2
      | Face3
      | Face4
      | Face5
      | Face6
      | FaceDown
      | FaceLeft
      | FaceNod
      | FaceRight
      | FaceShake
      | FaceUp
      | Falling
      | FamilyGroup
      | FamilyStar
      | Female
      | Femur
      | FemurAlt
      | Flood
      | Fluid
      | FluidBalance
      | FluidMed
      | Foggy
      | FoldedHands
      | FollowTheSigns
      | FootBones
      | Footprint
      | Forest
      | FrontHand
      | GardenCart
      | Gastroenterology
      | Gavel
      | Genetics
      | Girl
      | GlobeBook
      | Glucose
      | Group
      | GroupAdd
      | GroupOff
      | GroupRemove
      | GroupWork
      | Groups
      | Groups2
      | Groups3
      | Guardian
      | Gynecology
      | HandBones
      | HandMeal
      | HandPackage
      | Handshake
      | HealthAndSafety
      | HealthCross
      | HeartBroken
      | HeartSmile
      | Helicopter
      | Hematology
      | Hive
      | HomeHealth
      | Humerus
      | HumerusAlt
      | HumidityPercentage
      | IdentityPlatform
      | Immunology
      | Infrared
      | Inpatient
      | KidStar
      | LabPanel
      | LabResearch
      | Labs
      | Landslide
      | Lips
      | Male
      | Man
      | Man2
      | Man3
      | Man4
      | Manga
      | Masks
      | Massage
      | MedicalInformation
      | MedicalMask
      | Medication
      | MedicationLiquid
      | MenuBook2
      | Metabolism
      | Microbiology
      | MilitaryTech
      | Mist
      | MixtureMed
      | MonitorHeart
      | Mood
      | MoodBad
      | MoonStars
      | MountainFlag
      | MovingBeds
      | Mystery
      | Nephrology
      | Neurology
      | NoAdultContent
      | NotAccessible
      | NotAccessibleForward
      | Nutrition
      | OilBarrel
      | Oncology
      | Ophthalmology
      | OralDisease
      | Orbit
      | Orthopedics
      | OutdoorGarden
      | Outpatient
      | OutpatientMed
      | Owl
      | OxygenSaturation
      | PartlyCloudyDay
      | PartlyCloudyNight
      | PartnerExchange
      | PartnerHeart
      | Pediatrics
      | Person
      | Person2
      | Person3
      | Person4
      | PersonAdd
      | PersonAlert
      | PersonApron
      | PersonCancel
      | PersonCheck
      | PersonHeart
      | PersonOff
      | PersonRaisedHand
      | PersonRemove
      | PersonText
      | Pets
      | Pill
      | PillOff
      | Planet
      | Playground
      | Playground2
      | PlayingCards
      | PokerChip
      | PottedPlant
      | PrayerTimes
      | Pregnancy
      | PregnantWoman
      | Prescriptions
      | Procedure
      | Psychiatry
      | Psychology
      | PsychologyAlt
      | Public
      | PublicOff
      | Pulmonology
      | PulseAlert
      | Quiz
      | Radiology
      | Rainy
      | RainyHeavy
      | RainyLight
      | RainySnow
      | Raven
      | RecentPatient
      | Recommend
      | Recycling
      | ReduceCapacity
      | RespiratoryRate
      | Rheumatology
      | RibCage
      | Rocket
      | RocketLaunch
      | Routine
      | SafetyDivider
      | Salinity
      | Sanitizer
      | SentimentCalm
      | SentimentContent
      | SentimentDissatisfied
      | SentimentExcited
      | SentimentExtremelyDissatisfied
      | SentimentFrustrated
      | SentimentNeutral
      | SentimentSad
      | SentimentSatisfied
      | SentimentStressed
      | SentimentVeryDissatisfied
      | SentimentVerySatisfied
      | SentimentWorried
      | SettingsSeating
      | SevereCold
      | Share
      | ShareOff
      | ShieldWatch
      | ShortStay
      | Sick
      | SignLanguage
      | Simulation
      | Siren
      | SirenCheck
      | SirenOpen
      | SirenQuestion
      | Skeleton
      | Skull
      | SkullList
      | Snowing
      | SnowingHeavy
      | SocialDistance
      | SocialLeaderboard
      | SolarPower
      | SouthAmerica
      | SpecificGravity
      | SquareDot
      | StarShine
      | Stars2
      | Stethoscope
      | StethoscopeArrow
      | StethoscopeCheck
      | Strategy
      | Sunny
      | SunnySnowing
      | SupportAgent
      | Surgical
      | SwordRose
      | Symptoms
      | Syringe
      | TableSign
      | Tactic
      | Taunt
      | ThumbDown
      | ThumbUp
      | ThumbsUpDouble
      | ThumbsUpDown
      | Thunderstorm
      | Tibia
      | TibiaAlt
      | Tornado
      | TotalDissolvedSolids
      | Transgender
      | TravelExplore
      | Tsunami
      | UlnaRadius
      | UlnaRadiusAlt
      | Undereye
      | Urology
      | Vaccines
      | VapeFree
      | VapingRooms
      | VitalSigns
      | Volcano
      | Ward
      | WaterBottle
      | WaterBottleLarge
      | WaterDo
      | WaterDrop
      | WaterEc
      | WaterLux
      | WaterOrp
      | WaterPh
      | WaterVoc
      | WavingHand
      | Wc
      | WeatherHail
      | WeatherMix
      | Weight
      | Whatshot
      | WindPower
      | Woman
      | Woman2
      | WorkspacePremium
      | Workspaces
      | WoundsInjuries
      | Wrist

    module Social =

      let toSnakeCase (icon: Social) =
        match icon with
        | Social.``18UpRating`` -> "18_up_rating"
        | Social.``6FtApart`` -> "6_ft_apart"
        | Social.Acupuncture -> "acupuncture"
        | Social.AddReaction -> "add_reaction"
        | Social.AdminMeds -> "admin_meds"
        | Social.Agender -> "agender"
        | Social.Allergies -> "allergies"
        | Social.Allergy -> "allergy"
        | Social.Altitude -> "altitude"
        | Social.Antigravity -> "antigravity"
        | Social.Barefoot -> "barefoot"
        | Social.Bedtime -> "bedtime"
        | Social.BedtimeOff -> "bedtime_off"
        | Social.Blind -> "blind"
        | Social.BloodPressure -> "blood_pressure"
        | Social.Bloodtype -> "bloodtype"
        | Social.BodyFat -> "body_fat"
        | Social.BodySystem -> "body_system"
        | Social.Bomb -> "bomb"
        | Social.Boy -> "boy"
        | Social.Breastfeeding -> "breastfeeding"
        | Social.Brick -> "brick"
        | Social.BringYourOwnIp -> "bring_your_own_ip"
        | Social.CalendarMeal -> "calendar_meal"
        | Social.Candle -> "candle"
        | Social.Cannabis -> "cannabis"
        | Social.CardioLoad -> "cardio_load"
        | Social.Cardiology -> "cardiology"
        | Social.Cheer -> "cheer"
        | Social.ChefHat -> "chef_hat"
        | Social.Chess -> "chess"
        | Social.ChessBishop -> "chess_bishop"
        | Social.ChessBishop2 -> "chess_bishop_2"
        | Social.ChessKing -> "chess_king"
        | Social.ChessKing2 -> "chess_king_2"
        | Social.ChessKnight -> "chess_knight"
        | Social.ChessPawn -> "chess_pawn"
        | Social.ChessPawn2 -> "chess_pawn_2"
        | Social.ChessQueen -> "chess_queen"
        | Social.ChessRook -> "chess_rook"
        | Social.ChildHat -> "child_hat"
        | Social.CleanHands -> "clean_hands"
        | Social.ClearDay -> "clear_day"
        | Social.ClinicalNotes -> "clinical_notes"
        | Social.Co2 -> "co2"
        | Social.Cognition -> "cognition"
        | Social.Cognition2 -> "cognition_2"
        | Social.ComedyMask -> "comedy_mask"
        | Social.ComicBubble -> "comic_bubble"
        | Social.Communication -> "communication"
        | Social.Communities -> "communities"
        | Social.Compost -> "compost"
        | Social.Conditions -> "conditions"
        | Social.Congenital -> "congenital"
        | Social.ConnectWithoutContact -> "connect_without_contact"
        | Social.Conversation -> "conversation"
        | Social.Cookie -> "cookie"
        | Social.CookieOff -> "cookie_off"
        | Social.Coronavirus -> "coronavirus"
        | Social.Crossword -> "crossword"
        | Social.Crowdsource -> "crowdsource"
        | Social.Crown -> "crown"
        | Social.CrueltyFree -> "cruelty_free"
        | Social.Cyclone -> "cyclone"
        | Social.Deceased -> "deceased"
        | Social.Demography -> "demography"
        | Social.Dentistry -> "dentistry"
        | Social.Dermatology -> "dermatology"
        | Social.Destruction -> "destruction"
        | Social.DewPoint -> "dew_point"
        | Social.Diamond -> "diamond"
        | Social.DiamondShine -> "diamond_shine"
        | Social.DigitalWellbeing -> "digital_wellbeing"
        | Social.DineIn -> "dine_in"
        | Social.Diversity1 -> "diversity_1"
        | Social.Diversity2 -> "diversity_2"
        | Social.Diversity3 -> "diversity_3"
        | Social.Diversity4 -> "diversity_4"
        | Social.DominoMask -> "domino_mask"
        | Social.Drone -> "drone"
        | Social.Drone2 -> "drone_2"
        | Social.Earthquake -> "earthquake"
        | Social.Eco -> "eco"
        | Social.EditorChoice -> "editor_choice"
        | Social.Egg -> "egg"
        | Social.EggAlt -> "egg_alt"
        | Social.Elderly -> "elderly"
        | Social.ElderlyWoman -> "elderly_woman"
        | Social.EmojiFoodBeverage -> "emoji_food_beverage"
        | Social.EmojiNature -> "emoji_nature"
        | Social.EmojiObjects -> "emoji_objects"
        | Social.EmojiPeople -> "emoji_people"
        | Social.EmojiSymbols -> "emoji_symbols"
        | Social.EmojiTransportation -> "emoji_transportation"
        | Social.Emoticon -> "emoticon"
        | Social.Endocrinology -> "endocrinology"
        | Social.Ent -> "ent"
        | Social.Explosion -> "explosion"
        | Social.Eyebrow -> "eyebrow"
        | Social.Eyeglasses -> "eyeglasses"
        | Social.Eyeglasses2 -> "eyeglasses_2"
        | Social.Eyeglasses2Sound -> "eyeglasses_2_sound"
        | Social.Face -> "face"
        | Social.Face2 -> "face_2"
        | Social.Face3 -> "face_3"
        | Social.Face4 -> "face_4"
        | Social.Face5 -> "face_5"
        | Social.Face6 -> "face_6"
        | Social.FaceDown -> "face_down"
        | Social.FaceLeft -> "face_left"
        | Social.FaceNod -> "face_nod"
        | Social.FaceRight -> "face_right"
        | Social.FaceShake -> "face_shake"
        | Social.FaceUp -> "face_up"
        | Social.Falling -> "falling"
        | Social.FamilyGroup -> "family_group"
        | Social.FamilyStar -> "family_star"
        | Social.Female -> "female"
        | Social.Femur -> "femur"
        | Social.FemurAlt -> "femur_alt"
        | Social.Flood -> "flood"
        | Social.Fluid -> "fluid"
        | Social.FluidBalance -> "fluid_balance"
        | Social.FluidMed -> "fluid_med"
        | Social.Foggy -> "foggy"
        | Social.FoldedHands -> "folded_hands"
        | Social.FollowTheSigns -> "follow_the_signs"
        | Social.FootBones -> "foot_bones"
        | Social.Footprint -> "footprint"
        | Social.Forest -> "forest"
        | Social.FrontHand -> "front_hand"
        | Social.GardenCart -> "garden_cart"
        | Social.Gastroenterology -> "gastroenterology"
        | Social.Gavel -> "gavel"
        | Social.Genetics -> "genetics"
        | Social.Girl -> "girl"
        | Social.GlobeBook -> "globe_book"
        | Social.Glucose -> "glucose"
        | Social.Group -> "group"
        | Social.GroupAdd -> "group_add"
        | Social.GroupOff -> "group_off"
        | Social.GroupRemove -> "group_remove"
        | Social.GroupWork -> "group_work"
        | Social.Groups -> "groups"
        | Social.Groups2 -> "groups_2"
        | Social.Groups3 -> "groups_3"
        | Social.Guardian -> "guardian"
        | Social.Gynecology -> "gynecology"
        | Social.HandBones -> "hand_bones"
        | Social.HandMeal -> "hand_meal"
        | Social.HandPackage -> "hand_package"
        | Social.Handshake -> "handshake"
        | Social.HealthAndSafety -> "health_and_safety"
        | Social.HealthCross -> "health_cross"
        | Social.HeartBroken -> "heart_broken"
        | Social.HeartSmile -> "heart_smile"
        | Social.Helicopter -> "helicopter"
        | Social.Hematology -> "hematology"
        | Social.Hive -> "hive"
        | Social.HomeHealth -> "home_health"
        | Social.Humerus -> "humerus"
        | Social.HumerusAlt -> "humerus_alt"
        | Social.HumidityPercentage -> "humidity_percentage"
        | Social.IdentityPlatform -> "identity_platform"
        | Social.Immunology -> "immunology"
        | Social.Infrared -> "infrared"
        | Social.Inpatient -> "inpatient"
        | Social.KidStar -> "kid_star"
        | Social.LabPanel -> "lab_panel"
        | Social.LabResearch -> "lab_research"
        | Social.Labs -> "labs"
        | Social.Landslide -> "landslide"
        | Social.Lips -> "lips"
        | Social.Male -> "male"
        | Social.Man -> "man"
        | Social.Man2 -> "man_2"
        | Social.Man3 -> "man_3"
        | Social.Man4 -> "man_4"
        | Social.Manga -> "manga"
        | Social.Masks -> "masks"
        | Social.Massage -> "massage"
        | Social.MedicalInformation -> "medical_information"
        | Social.MedicalMask -> "medical_mask"
        | Social.Medication -> "medication"
        | Social.MedicationLiquid -> "medication_liquid"
        | Social.MenuBook2 -> "menu_book_2"
        | Social.Metabolism -> "metabolism"
        | Social.Microbiology -> "microbiology"
        | Social.MilitaryTech -> "military_tech"
        | Social.Mist -> "mist"
        | Social.MixtureMed -> "mixture_med"
        | Social.MonitorHeart -> "monitor_heart"
        | Social.Mood -> "mood"
        | Social.MoodBad -> "mood_bad"
        | Social.MoonStars -> "moon_stars"
        | Social.MountainFlag -> "mountain_flag"
        | Social.MovingBeds -> "moving_beds"
        | Social.Mystery -> "mystery"
        | Social.Nephrology -> "nephrology"
        | Social.Neurology -> "neurology"
        | Social.NoAdultContent -> "no_adult_content"
        | Social.NotAccessible -> "not_accessible"
        | Social.NotAccessibleForward -> "not_accessible_forward"
        | Social.Nutrition -> "nutrition"
        | Social.OilBarrel -> "oil_barrel"
        | Social.Oncology -> "oncology"
        | Social.Ophthalmology -> "ophthalmology"
        | Social.OralDisease -> "oral_disease"
        | Social.Orbit -> "orbit"
        | Social.Orthopedics -> "orthopedics"
        | Social.OutdoorGarden -> "outdoor_garden"
        | Social.Outpatient -> "outpatient"
        | Social.OutpatientMed -> "outpatient_med"
        | Social.Owl -> "owl"
        | Social.OxygenSaturation -> "oxygen_saturation"
        | Social.PartlyCloudyDay -> "partly_cloudy_day"
        | Social.PartlyCloudyNight -> "partly_cloudy_night"
        | Social.PartnerExchange -> "partner_exchange"
        | Social.PartnerHeart -> "partner_heart"
        | Social.Pediatrics -> "pediatrics"
        | Social.Person -> "person"
        | Social.Person2 -> "person_2"
        | Social.Person3 -> "person_3"
        | Social.Person4 -> "person_4"
        | Social.PersonAdd -> "person_add"
        | Social.PersonAlert -> "person_alert"
        | Social.PersonApron -> "person_apron"
        | Social.PersonCancel -> "person_cancel"
        | Social.PersonCheck -> "person_check"
        | Social.PersonHeart -> "person_heart"
        | Social.PersonOff -> "person_off"
        | Social.PersonRaisedHand -> "person_raised_hand"
        | Social.PersonRemove -> "person_remove"
        | Social.PersonText -> "person_text"
        | Social.Pets -> "pets"
        | Social.Pill -> "pill"
        | Social.PillOff -> "pill_off"
        | Social.Planet -> "planet"
        | Social.Playground -> "playground"
        | Social.Playground2 -> "playground_2"
        | Social.PlayingCards -> "playing_cards"
        | Social.PokerChip -> "poker_chip"
        | Social.PottedPlant -> "potted_plant"
        | Social.PrayerTimes -> "prayer_times"
        | Social.Pregnancy -> "pregnancy"
        | Social.PregnantWoman -> "pregnant_woman"
        | Social.Prescriptions -> "prescriptions"
        | Social.Procedure -> "procedure"
        | Social.Psychiatry -> "psychiatry"
        | Social.Psychology -> "psychology"
        | Social.PsychologyAlt -> "psychology_alt"
        | Social.Public -> "public"
        | Social.PublicOff -> "public_off"
        | Social.Pulmonology -> "pulmonology"
        | Social.PulseAlert -> "pulse_alert"
        | Social.Quiz -> "quiz"
        | Social.Radiology -> "radiology"
        | Social.Rainy -> "rainy"
        | Social.RainyHeavy -> "rainy_heavy"
        | Social.RainyLight -> "rainy_light"
        | Social.RainySnow -> "rainy_snow"
        | Social.Raven -> "raven"
        | Social.RecentPatient -> "recent_patient"
        | Social.Recommend -> "recommend"
        | Social.Recycling -> "recycling"
        | Social.ReduceCapacity -> "reduce_capacity"
        | Social.RespiratoryRate -> "respiratory_rate"
        | Social.Rheumatology -> "rheumatology"
        | Social.RibCage -> "rib_cage"
        | Social.Rocket -> "rocket"
        | Social.RocketLaunch -> "rocket_launch"
        | Social.Routine -> "routine"
        | Social.SafetyDivider -> "safety_divider"
        | Social.Salinity -> "salinity"
        | Social.Sanitizer -> "sanitizer"
        | Social.SentimentCalm -> "sentiment_calm"
        | Social.SentimentContent -> "sentiment_content"
        | Social.SentimentDissatisfied -> "sentiment_dissatisfied"
        | Social.SentimentExcited -> "sentiment_excited"
        | Social.SentimentExtremelyDissatisfied -> "sentiment_extremely_dissatisfied"
        | Social.SentimentFrustrated -> "sentiment_frustrated"
        | Social.SentimentNeutral -> "sentiment_neutral"
        | Social.SentimentSad -> "sentiment_sad"
        | Social.SentimentSatisfied -> "sentiment_satisfied"
        | Social.SentimentStressed -> "sentiment_stressed"
        | Social.SentimentVeryDissatisfied -> "sentiment_very_dissatisfied"
        | Social.SentimentVerySatisfied -> "sentiment_very_satisfied"
        | Social.SentimentWorried -> "sentiment_worried"
        | Social.SettingsSeating -> "settings_seating"
        | Social.SevereCold -> "severe_cold"
        | Social.Share -> "share"
        | Social.ShareOff -> "share_off"
        | Social.ShieldWatch -> "shield_watch"
        | Social.ShortStay -> "short_stay"
        | Social.Sick -> "sick"
        | Social.SignLanguage -> "sign_language"
        | Social.Simulation -> "simulation"
        | Social.Siren -> "siren"
        | Social.SirenCheck -> "siren_check"
        | Social.SirenOpen -> "siren_open"
        | Social.SirenQuestion -> "siren_question"
        | Social.Skeleton -> "skeleton"
        | Social.Skull -> "skull"
        | Social.SkullList -> "skull_list"
        | Social.Snowing -> "snowing"
        | Social.SnowingHeavy -> "snowing_heavy"
        | Social.SocialDistance -> "social_distance"
        | Social.SocialLeaderboard -> "social_leaderboard"
        | Social.SolarPower -> "solar_power"
        | Social.SouthAmerica -> "south_america"
        | Social.SpecificGravity -> "specific_gravity"
        | Social.SquareDot -> "square_dot"
        | Social.StarShine -> "star_shine"
        | Social.Stars2 -> "stars_2"
        | Social.Stethoscope -> "stethoscope"
        | Social.StethoscopeArrow -> "stethoscope_arrow"
        | Social.StethoscopeCheck -> "stethoscope_check"
        | Social.Strategy -> "strategy"
        | Social.Sunny -> "sunny"
        | Social.SunnySnowing -> "sunny_snowing"
        | Social.SupportAgent -> "support_agent"
        | Social.Surgical -> "surgical"
        | Social.SwordRose -> "sword_rose"
        | Social.Symptoms -> "symptoms"
        | Social.Syringe -> "syringe"
        | Social.TableSign -> "table_sign"
        | Social.Tactic -> "tactic"
        | Social.Taunt -> "taunt"
        | Social.ThumbDown -> "thumb_down"
        | Social.ThumbUp -> "thumb_up"
        | Social.ThumbsUpDouble -> "thumbs_up_double"
        | Social.ThumbsUpDown -> "thumbs_up_down"
        | Social.Thunderstorm -> "thunderstorm"
        | Social.Tibia -> "tibia"
        | Social.TibiaAlt -> "tibia_alt"
        | Social.Tornado -> "tornado"
        | Social.TotalDissolvedSolids -> "total_dissolved_solids"
        | Social.Transgender -> "transgender"
        | Social.TravelExplore -> "travel_explore"
        | Social.Tsunami -> "tsunami"
        | Social.UlnaRadius -> "ulna_radius"
        | Social.UlnaRadiusAlt -> "ulna_radius_alt"
        | Social.Undereye -> "undereye"
        | Social.Urology -> "urology"
        | Social.Vaccines -> "vaccines"
        | Social.VapeFree -> "vape_free"
        | Social.VapingRooms -> "vaping_rooms"
        | Social.VitalSigns -> "vital_signs"
        | Social.Volcano -> "volcano"
        | Social.Ward -> "ward"
        | Social.WaterBottle -> "water_bottle"
        | Social.WaterBottleLarge -> "water_bottle_large"
        | Social.WaterDo -> "water_do"
        | Social.WaterDrop -> "water_drop"
        | Social.WaterEc -> "water_ec"
        | Social.WaterLux -> "water_lux"
        | Social.WaterOrp -> "water_orp"
        | Social.WaterPh -> "water_ph"
        | Social.WaterVoc -> "water_voc"
        | Social.WavingHand -> "waving_hand"
        | Social.Wc -> "wc"
        | Social.WeatherHail -> "weather_hail"
        | Social.WeatherMix -> "weather_mix"
        | Social.Weight -> "weight"
        | Social.Whatshot -> "whatshot"
        | Social.WindPower -> "wind_power"
        | Social.Woman -> "woman"
        | Social.Woman2 -> "woman_2"
        | Social.WorkspacePremium -> "workspace_premium"
        | Social.Workspaces -> "workspaces"
        | Social.WoundsInjuries -> "wounds_injuries"
        | Social.Wrist -> "wrist"

    [<RequireQualifiedAccess; Struct>]
    type Text =
      | AddColumnLeft
      | AddColumnRight
      | AddLink
      | AddNotes
      | AddRowAbove
      | AddRowBelow
      | AddToDrive
      | AlignCenter
      | AlignEnd
      | AlignFlexCenter
      | AlignFlexEnd
      | AlignFlexStart
      | AlignHorizontalCenter
      | AlignHorizontalLeft
      | AlignHorizontalRight
      | AlignItemsStretch
      | AlignJustifyCenter
      | AlignJustifyFlexEnd
      | AlignJustifyFlexStart
      | AlignJustifySpaceAround
      | AlignJustifySpaceBetween
      | AlignJustifySpaceEven
      | AlignJustifyStretch
      | AlignSelfStretch
      | AlignSpaceAround
      | AlignSpaceBetween
      | AlignSpaceEven
      | AlignStart
      | AlignStretch
      | AlignVerticalBottom
      | AlignVerticalCenter
      | AlignVerticalTop
      | AmpStories
      | Archive
      | Article
      | ArticlePerson
      | ArticleShortcut
      | Assignment
      | AssignmentAdd
      | AssignmentGlobe
      | AssignmentInd
      | AssignmentLate
      | AssignmentReturn
      | AssignmentReturned
      | AssignmentTurnedIn
      | Asterisk
      | AttachFile
      | AttachFileAdd
      | AttachFileOff
      | Attachment
      | Automation
      | Ballot
      | Book
      | Book2
      | Book3
      | Book4
      | Book5
      | Book6
      | BorderAll
      | BorderBottom
      | BorderClear
      | BorderColor
      | BorderHorizontal
      | BorderInner
      | BorderLeft
      | BorderOuter
      | BorderRight
      | BorderStyle
      | BorderTop
      | BorderVertical
      | BrandFamily
      | BreakingNews
      | BreakingNewsAlt1
      | BusinessChip
      | CalendarViewDay
      | CalendarViewMonth
      | CalendarViewWeek
      | CardsStack
      | CellMerge
      | Checklist
      | ChecklistRtl
      | Clarify
      | Cloud
      | CloudAlert
      | CloudCircle
      | CloudDone
      | CloudDownload
      | CloudLock
      | CloudOff
      | CloudSync
      | CloudUpload
      | Colors
      | CombineColumns
      | ContactPage
      | ContentCopy
      | ContentCut
      | ContentPaste
      | ContentPasteGo
      | ContentPasteOff
      | ContentPasteSearch
      | Contract
      | ContractDelete
      | ContractEdit
      | ConvertToText
      | CopyAll
      | Counter0
      | Counter1
      | Counter2
      | Counter3
      | Counter4
      | Counter5
      | Counter6
      | Counter7
      | Counter8
      | Counter9
      | Csv
      | CustomTypography
      | Dashboard
      | Dashboard2
      | Dashboard2Edit
      | Dashboard2Gear
      | DashboardCustomize
      | DataArray
      | DataObject
      | DecimalDecrease
      | DecimalIncrease
      | Description
      | Deselect
      | DesignServices
      | Diagnosis
      | DiagonalLine
      | Dictionary
      | Difference
      | Docs
      | DocsAddOn
      | DocsAppsScript
      | DocumentScanner
      | DocumentSearch
      | Draft
      | DragHandle
      | Draw
      | DrawAbstract
      | DrawCollage
      | DriveExport
      | DriveFileMove
      | DriveFolderUpload
      | EditDocument
      | EditNote
      | EditOff
      | Equal
      | EraserSize1
      | EraserSize2
      | EraserSize3
      | EraserSize4
      | EraserSize5
      | ExportNotes
      | FactCheck
      | FileCopy
      | FileCopyOff
      | FilePresent
      | FileSave
      | FileSaveOff
      | Files
      | FinanceChip
      | FindInPage
      | FitPage
      | FitPageHeight
      | FitPageWidth
      | FitWidth
      | FlexDirection
      | FlexNoWrap
      | FlexWrap
      | FlipToBack
      | FlipToFront
      | Folder
      | FolderCheck
      | FolderCheck2
      | FolderCode
      | FolderCopy
      | FolderData
      | FolderDelete
      | FolderEye
      | FolderInfo
      | FolderLimited
      | FolderManaged
      | FolderMatch
      | FolderOff
      | FolderOpen
      | FolderShared
      | FolderSpecial
      | FolderSupervised
      | FolderZip
      | FontDownload
      | FontDownloadOff
      | FormatAlignCenter
      | FormatAlignJustify
      | FormatAlignLeft
      | FormatAlignRight
      | FormatBold
      | FormatClear
      | FormatColorFill
      | FormatColorReset
      | FormatColorText
      | FormatH1
      | FormatH2
      | FormatH3
      | FormatH4
      | FormatH5
      | FormatH6
      | FormatImageBack
      | FormatImageBreakLeft
      | FormatImageBreakRight
      | FormatImageFront
      | FormatImageInlineLeft
      | FormatImageInlineRight
      | FormatImageLeft
      | FormatImageRight
      | FormatIndentDecrease
      | FormatIndentIncrease
      | FormatInkHighlighter
      | FormatItalic
      | FormatLetterSpacing
      | FormatLetterSpacing2
      | FormatLetterSpacingStandard
      | FormatLetterSpacingWide
      | FormatLetterSpacingWider
      | FormatLineSpacing
      | FormatListBulleted
      | FormatListBulletedAdd
      | FormatListNumbered
      | FormatListNumberedRtl
      | FormatOverline
      | FormatPaint
      | FormatParagraph
      | FormatQuote
      | FormatQuoteOff
      | FormatShapes
      | FormatSize
      | FormatStrikethrough
      | FormatTextClip
      | FormatTextOverflow
      | FormatTextWrap
      | FormatTextdirectionLToR
      | FormatTextdirectionRToL
      | FormatTextdirectionVertical
      | FormatUnderlined
      | FormatUnderlinedSquiggle
      | FormsAddOn
      | FormsAppsScript
      | FrameInspect
      | FrameReload
      | FrameSource
      | FullCoverage
      | Function
      | Functions
      | Glyphs
      | Grading
      | GridGuides
      | GridView
      | HeapSnapshotLarge
      | HeapSnapshotMultiple
      | HeapSnapshotThumbnail
      | Height
      | Hexagon
      | HighlighterSize1
      | HighlighterSize2
      | HighlighterSize3
      | HighlighterSize4
      | HighlighterSize5
      | HistoryEdu
      | HorizontalDistribute
      | HorizontalRule
      | HorizontalSplit
      | ImagesearchRoller
      | InkEraser
      | InkEraserOff
      | InkHighlighter
      | InkHighlighterMove
      | InkMarker
      | InkPen
      | InkSelection
      | InsertPageBreak
      | InsertText
      | IntegrationInstructions
      | Inventory
      | Join
      | JoinInner
      | JoinLeft
      | JoinRight
      | LabProfile
      | LanguageChineseArray
      | LanguageChineseCangjie
      | LanguageChineseDayi
      | LanguageChinesePinyin
      | LanguageChineseQuick
      | LanguageChineseWubi
      | LanguageFrench
      | LanguageGbEnglish
      | LanguageInternational
      | LanguageJapaneseKana
      | LanguageKoreanLatin
      | LanguagePinyin
      | LanguageSpanish
      | LanguageUs
      | LanguageUsColemak
      | LanguageUsDvorak
      | LassoSelect
      | LetterSwitch
      | LineAxis
      | LineCurve
      | LineEnd
      | LineEndArrow
      | LineEndArrowNotch
      | LineEndCircle
      | LineEndDiamond
      | LineEndSquare
      | LineStart
      | LineStartArrow
      | LineStartArrowNotch
      | LineStartCircle
      | LineStartDiamond
      | LineStartSquare
      | LineStyle
      | LineWeight
      | LinearScale
      | List
      | ListAlt
      | ListAltAdd
      | ListAltCheck
      | LocationChip
      | LowPriority
      | Lowercase
      | Margin
      | Markdown
      | MarkdownCopy
      | MarkdownPaste
      | MatchCase
      | MatchCaseOff
      | MatchWord
      | MenuBook
      | MergeType
      | News
      | Newsmode
      | Newspaper
      | NoteAdd
      | NoteAlt
      | NoteStack
      | NoteStackAdd
      | Notes
      | Numbers
      | OtherAdmission
      | Overview
      | Padding
      | PageFooter
      | PageHeader
      | Pageless
      | Pages
      | PenSize1
      | PenSize2
      | PenSize3
      | PenSize4
      | PenSize5
      | PendingActions
      | Pentagon
      | Percent
      | PermMedia
      | PersonBook
      | PivotTableChart
      | Plagiarism
      | Polyline
      | Post
      | PostAdd
      | ProcessChart
      | ReadMore
      | Rectangle
      | RegularExpression
      | RemoveSelection
      | Reorder
      | RequestPage
      | RequestQuote
      | ResetImage
      | RestorePage
      | Rubric
      | RuleFolder
      | Scan
      | ScanDelete
      | Script
      | Segment
      | Select
      | Serif
      | ShapeLine
      | Shapes
      | SheetsRtl
      | ShortText
      | Signature
      | SlabSerif
      | SlideLibrary
      | SmbShare
      | SnippetFolder
      | SourceNotes
      | SpaceBar
      | SpaceDashboard
      | SpecialCharacter
      | Spellcheck
      | Square
      | StackHexagon
      | StickyNote
      | StickyNote2
      | StockMedia
      | StrokeFull
      | StrokePartial
      | StylusBrush
      | StylusFountainPen
      | StylusHighlighter
      | StylusLaserPointer
      | StylusPen
      | StylusPencil
      | Subject
      | Subscript
      | SubtitlesOff
      | Summarize
      | Superscript
      | Table
      | TableChart
      | TableChartView
      | TableConvert
      | TableEdit
      | TableEye
      | TableRows
      | TableRowsNarrow
      | TableView
      | Tag
      | Task
      | TeamDashboard
      | TextAd
      | TextCompare
      | TextDecrease
      | TextFields
      | TextFieldsAlt
      | TextFormat
      | TextIncrease
      | TextRotateUp
      | TextRotateVertical
      | TextRotationAngledown
      | TextRotationAngleup
      | TextRotationDown
      | TextRotationNone
      | TextSelectEnd
      | TextSelectJumpToBeginning
      | TextSelectJumpToEnd
      | TextSelectMoveBackCharacter
      | TextSelectMoveBackWord
      | TextSelectMoveDown
      | TextSelectMoveForwardCharacter
      | TextSelectMoveForwardWord
      | TextSelectMoveUp
      | TextSelectStart
      | TextSnippet
      | TextUp
      | ThumbnailBar
      | Title
      | Titlecase
      | Toc
      | TopPanelClose
      | TopPanelOpen
      | Tsv
      | TwoPager
      | TwoPagerStore
      | TypeSpecimen
      | Ungroup
      | Unknown7
      | UnknownDocument
      | Uppercase
      | VariableAdd
      | VariableInsert
      | VariableRemove
      | Variables
      | VerticalAlignBottom
      | VerticalAlignCenter
      | VerticalAlignTop
      | VerticalDistribute
      | VerticalSplit
      | VideoFile
      | ViewAgenda
      | ViewArray
      | ViewCarousel
      | ViewColumn
      | ViewColumn2
      | ViewDay
      | ViewHeadline
      | ViewList
      | ViewModule
      | ViewObjectTrack
      | ViewQuilt
      | ViewSidebar
      | ViewStream
      | ViewWeek
      | VotingChip
      | WrapText

    module Text =

      let toSnakeCase (icon: Text) =
        match icon with
        | Text.AddColumnLeft -> "add_column_left"
        | Text.AddColumnRight -> "add_column_right"
        | Text.AddLink -> "add_link"
        | Text.AddNotes -> "add_notes"
        | Text.AddRowAbove -> "add_row_above"
        | Text.AddRowBelow -> "add_row_below"
        | Text.AddToDrive -> "add_to_drive"
        | Text.AlignCenter -> "align_center"
        | Text.AlignEnd -> "align_end"
        | Text.AlignFlexCenter -> "align_flex_center"
        | Text.AlignFlexEnd -> "align_flex_end"
        | Text.AlignFlexStart -> "align_flex_start"
        | Text.AlignHorizontalCenter -> "align_horizontal_center"
        | Text.AlignHorizontalLeft -> "align_horizontal_left"
        | Text.AlignHorizontalRight -> "align_horizontal_right"
        | Text.AlignItemsStretch -> "align_items_stretch"
        | Text.AlignJustifyCenter -> "align_justify_center"
        | Text.AlignJustifyFlexEnd -> "align_justify_flex_end"
        | Text.AlignJustifyFlexStart -> "align_justify_flex_start"
        | Text.AlignJustifySpaceAround -> "align_justify_space_around"
        | Text.AlignJustifySpaceBetween -> "align_justify_space_between"
        | Text.AlignJustifySpaceEven -> "align_justify_space_even"
        | Text.AlignJustifyStretch -> "align_justify_stretch"
        | Text.AlignSelfStretch -> "align_self_stretch"
        | Text.AlignSpaceAround -> "align_space_around"
        | Text.AlignSpaceBetween -> "align_space_between"
        | Text.AlignSpaceEven -> "align_space_even"
        | Text.AlignStart -> "align_start"
        | Text.AlignStretch -> "align_stretch"
        | Text.AlignVerticalBottom -> "align_vertical_bottom"
        | Text.AlignVerticalCenter -> "align_vertical_center"
        | Text.AlignVerticalTop -> "align_vertical_top"
        | Text.AmpStories -> "amp_stories"
        | Text.Archive -> "archive"
        | Text.Article -> "article"
        | Text.ArticlePerson -> "article_person"
        | Text.ArticleShortcut -> "article_shortcut"
        | Text.Assignment -> "assignment"
        | Text.AssignmentAdd -> "assignment_add"
        | Text.AssignmentGlobe -> "assignment_globe"
        | Text.AssignmentInd -> "assignment_ind"
        | Text.AssignmentLate -> "assignment_late"
        | Text.AssignmentReturn -> "assignment_return"
        | Text.AssignmentReturned -> "assignment_returned"
        | Text.AssignmentTurnedIn -> "assignment_turned_in"
        | Text.Asterisk -> "asterisk"
        | Text.AttachFile -> "attach_file"
        | Text.AttachFileAdd -> "attach_file_add"
        | Text.AttachFileOff -> "attach_file_off"
        | Text.Attachment -> "attachment"
        | Text.Automation -> "automation"
        | Text.Ballot -> "ballot"
        | Text.Book -> "book"
        | Text.Book2 -> "book_2"
        | Text.Book3 -> "book_3"
        | Text.Book4 -> "book_4"
        | Text.Book5 -> "book_5"
        | Text.Book6 -> "book_6"
        | Text.BorderAll -> "border_all"
        | Text.BorderBottom -> "border_bottom"
        | Text.BorderClear -> "border_clear"
        | Text.BorderColor -> "border_color"
        | Text.BorderHorizontal -> "border_horizontal"
        | Text.BorderInner -> "border_inner"
        | Text.BorderLeft -> "border_left"
        | Text.BorderOuter -> "border_outer"
        | Text.BorderRight -> "border_right"
        | Text.BorderStyle -> "border_style"
        | Text.BorderTop -> "border_top"
        | Text.BorderVertical -> "border_vertical"
        | Text.BrandFamily -> "brand_family"
        | Text.BreakingNews -> "breaking_news"
        | Text.BreakingNewsAlt1 -> "breaking_news_alt_1"
        | Text.BusinessChip -> "business_chip"
        | Text.CalendarViewDay -> "calendar_view_day"
        | Text.CalendarViewMonth -> "calendar_view_month"
        | Text.CalendarViewWeek -> "calendar_view_week"
        | Text.CardsStack -> "cards_stack"
        | Text.CellMerge -> "cell_merge"
        | Text.Checklist -> "checklist"
        | Text.ChecklistRtl -> "checklist_rtl"
        | Text.Clarify -> "clarify"
        | Text.Cloud -> "cloud"
        | Text.CloudAlert -> "cloud_alert"
        | Text.CloudCircle -> "cloud_circle"
        | Text.CloudDone -> "cloud_done"
        | Text.CloudDownload -> "cloud_download"
        | Text.CloudLock -> "cloud_lock"
        | Text.CloudOff -> "cloud_off"
        | Text.CloudSync -> "cloud_sync"
        | Text.CloudUpload -> "cloud_upload"
        | Text.Colors -> "colors"
        | Text.CombineColumns -> "combine_columns"
        | Text.ContactPage -> "contact_page"
        | Text.ContentCopy -> "content_copy"
        | Text.ContentCut -> "content_cut"
        | Text.ContentPaste -> "content_paste"
        | Text.ContentPasteGo -> "content_paste_go"
        | Text.ContentPasteOff -> "content_paste_off"
        | Text.ContentPasteSearch -> "content_paste_search"
        | Text.Contract -> "contract"
        | Text.ContractDelete -> "contract_delete"
        | Text.ContractEdit -> "contract_edit"
        | Text.ConvertToText -> "convert_to_text"
        | Text.CopyAll -> "copy_all"
        | Text.Counter0 -> "counter_0"
        | Text.Counter1 -> "counter_1"
        | Text.Counter2 -> "counter_2"
        | Text.Counter3 -> "counter_3"
        | Text.Counter4 -> "counter_4"
        | Text.Counter5 -> "counter_5"
        | Text.Counter6 -> "counter_6"
        | Text.Counter7 -> "counter_7"
        | Text.Counter8 -> "counter_8"
        | Text.Counter9 -> "counter_9"
        | Text.Csv -> "csv"
        | Text.CustomTypography -> "custom_typography"
        | Text.Dashboard -> "dashboard"
        | Text.Dashboard2 -> "dashboard_2"
        | Text.Dashboard2Edit -> "dashboard_2_edit"
        | Text.Dashboard2Gear -> "dashboard_2_gear"
        | Text.DashboardCustomize -> "dashboard_customize"
        | Text.DataArray -> "data_array"
        | Text.DataObject -> "data_object"
        | Text.DecimalDecrease -> "decimal_decrease"
        | Text.DecimalIncrease -> "decimal_increase"
        | Text.Description -> "description"
        | Text.Deselect -> "deselect"
        | Text.DesignServices -> "design_services"
        | Text.Diagnosis -> "diagnosis"
        | Text.DiagonalLine -> "diagonal_line"
        | Text.Dictionary -> "dictionary"
        | Text.Difference -> "difference"
        | Text.Docs -> "docs"
        | Text.DocsAddOn -> "docs_add_on"
        | Text.DocsAppsScript -> "docs_apps_script"
        | Text.DocumentScanner -> "document_scanner"
        | Text.DocumentSearch -> "document_search"
        | Text.Draft -> "draft"
        | Text.DragHandle -> "drag_handle"
        | Text.Draw -> "draw"
        | Text.DrawAbstract -> "draw_abstract"
        | Text.DrawCollage -> "draw_collage"
        | Text.DriveExport -> "drive_export"
        | Text.DriveFileMove -> "drive_file_move"
        | Text.DriveFolderUpload -> "drive_folder_upload"
        | Text.EditDocument -> "edit_document"
        | Text.EditNote -> "edit_note"
        | Text.EditOff -> "edit_off"
        | Text.Equal -> "equal"
        | Text.EraserSize1 -> "eraser_size_1"
        | Text.EraserSize2 -> "eraser_size_2"
        | Text.EraserSize3 -> "eraser_size_3"
        | Text.EraserSize4 -> "eraser_size_4"
        | Text.EraserSize5 -> "eraser_size_5"
        | Text.ExportNotes -> "export_notes"
        | Text.FactCheck -> "fact_check"
        | Text.FileCopy -> "file_copy"
        | Text.FileCopyOff -> "file_copy_off"
        | Text.FilePresent -> "file_present"
        | Text.FileSave -> "file_save"
        | Text.FileSaveOff -> "file_save_off"
        | Text.Files -> "files"
        | Text.FinanceChip -> "finance_chip"
        | Text.FindInPage -> "find_in_page"
        | Text.FitPage -> "fit_page"
        | Text.FitPageHeight -> "fit_page_height"
        | Text.FitPageWidth -> "fit_page_width"
        | Text.FitWidth -> "fit_width"
        | Text.FlexDirection -> "flex_direction"
        | Text.FlexNoWrap -> "flex_no_wrap"
        | Text.FlexWrap -> "flex_wrap"
        | Text.FlipToBack -> "flip_to_back"
        | Text.FlipToFront -> "flip_to_front"
        | Text.Folder -> "folder"
        | Text.FolderCheck -> "folder_check"
        | Text.FolderCheck2 -> "folder_check_2"
        | Text.FolderCode -> "folder_code"
        | Text.FolderCopy -> "folder_copy"
        | Text.FolderData -> "folder_data"
        | Text.FolderDelete -> "folder_delete"
        | Text.FolderEye -> "folder_eye"
        | Text.FolderInfo -> "folder_info"
        | Text.FolderLimited -> "folder_limited"
        | Text.FolderManaged -> "folder_managed"
        | Text.FolderMatch -> "folder_match"
        | Text.FolderOff -> "folder_off"
        | Text.FolderOpen -> "folder_open"
        | Text.FolderShared -> "folder_shared"
        | Text.FolderSpecial -> "folder_special"
        | Text.FolderSupervised -> "folder_supervised"
        | Text.FolderZip -> "folder_zip"
        | Text.FontDownload -> "font_download"
        | Text.FontDownloadOff -> "font_download_off"
        | Text.FormatAlignCenter -> "format_align_center"
        | Text.FormatAlignJustify -> "format_align_justify"
        | Text.FormatAlignLeft -> "format_align_left"
        | Text.FormatAlignRight -> "format_align_right"
        | Text.FormatBold -> "format_bold"
        | Text.FormatClear -> "format_clear"
        | Text.FormatColorFill -> "format_color_fill"
        | Text.FormatColorReset -> "format_color_reset"
        | Text.FormatColorText -> "format_color_text"
        | Text.FormatH1 -> "format_h1"
        | Text.FormatH2 -> "format_h2"
        | Text.FormatH3 -> "format_h3"
        | Text.FormatH4 -> "format_h4"
        | Text.FormatH5 -> "format_h5"
        | Text.FormatH6 -> "format_h6"
        | Text.FormatImageBack -> "format_image_back"
        | Text.FormatImageBreakLeft -> "format_image_break_left"
        | Text.FormatImageBreakRight -> "format_image_break_right"
        | Text.FormatImageFront -> "format_image_front"
        | Text.FormatImageInlineLeft -> "format_image_inline_left"
        | Text.FormatImageInlineRight -> "format_image_inline_right"
        | Text.FormatImageLeft -> "format_image_left"
        | Text.FormatImageRight -> "format_image_right"
        | Text.FormatIndentDecrease -> "format_indent_decrease"
        | Text.FormatIndentIncrease -> "format_indent_increase"
        | Text.FormatInkHighlighter -> "format_ink_highlighter"
        | Text.FormatItalic -> "format_italic"
        | Text.FormatLetterSpacing -> "format_letter_spacing"
        | Text.FormatLetterSpacing2 -> "format_letter_spacing_2"
        | Text.FormatLetterSpacingStandard -> "format_letter_spacing_standard"
        | Text.FormatLetterSpacingWide -> "format_letter_spacing_wide"
        | Text.FormatLetterSpacingWider -> "format_letter_spacing_wider"
        | Text.FormatLineSpacing -> "format_line_spacing"
        | Text.FormatListBulleted -> "format_list_bulleted"
        | Text.FormatListBulletedAdd -> "format_list_bulleted_add"
        | Text.FormatListNumbered -> "format_list_numbered"
        | Text.FormatListNumberedRtl -> "format_list_numbered_rtl"
        | Text.FormatOverline -> "format_overline"
        | Text.FormatPaint -> "format_paint"
        | Text.FormatParagraph -> "format_paragraph"
        | Text.FormatQuote -> "format_quote"
        | Text.FormatQuoteOff -> "format_quote_off"
        | Text.FormatShapes -> "format_shapes"
        | Text.FormatSize -> "format_size"
        | Text.FormatStrikethrough -> "format_strikethrough"
        | Text.FormatTextClip -> "format_text_clip"
        | Text.FormatTextOverflow -> "format_text_overflow"
        | Text.FormatTextWrap -> "format_text_wrap"
        | Text.FormatTextdirectionLToR -> "format_textdirection_l_to_r"
        | Text.FormatTextdirectionRToL -> "format_textdirection_r_to_l"
        | Text.FormatTextdirectionVertical -> "format_textdirection_vertical"
        | Text.FormatUnderlined -> "format_underlined"
        | Text.FormatUnderlinedSquiggle -> "format_underlined_squiggle"
        | Text.FormsAddOn -> "forms_add_on"
        | Text.FormsAppsScript -> "forms_apps_script"
        | Text.FrameInspect -> "frame_inspect"
        | Text.FrameReload -> "frame_reload"
        | Text.FrameSource -> "frame_source"
        | Text.FullCoverage -> "full_coverage"
        | Text.Function -> "function"
        | Text.Functions -> "functions"
        | Text.Glyphs -> "glyphs"
        | Text.Grading -> "grading"
        | Text.GridGuides -> "grid_guides"
        | Text.GridView -> "grid_view"
        | Text.HeapSnapshotLarge -> "heap_snapshot_large"
        | Text.HeapSnapshotMultiple -> "heap_snapshot_multiple"
        | Text.HeapSnapshotThumbnail -> "heap_snapshot_thumbnail"
        | Text.Height -> "height"
        | Text.Hexagon -> "hexagon"
        | Text.HighlighterSize1 -> "highlighter_size_1"
        | Text.HighlighterSize2 -> "highlighter_size_2"
        | Text.HighlighterSize3 -> "highlighter_size_3"
        | Text.HighlighterSize4 -> "highlighter_size_4"
        | Text.HighlighterSize5 -> "highlighter_size_5"
        | Text.HistoryEdu -> "history_edu"
        | Text.HorizontalDistribute -> "horizontal_distribute"
        | Text.HorizontalRule -> "horizontal_rule"
        | Text.HorizontalSplit -> "horizontal_split"
        | Text.ImagesearchRoller -> "imagesearch_roller"
        | Text.InkEraser -> "ink_eraser"
        | Text.InkEraserOff -> "ink_eraser_off"
        | Text.InkHighlighter -> "ink_highlighter"
        | Text.InkHighlighterMove -> "ink_highlighter_move"
        | Text.InkMarker -> "ink_marker"
        | Text.InkPen -> "ink_pen"
        | Text.InkSelection -> "ink_selection"
        | Text.InsertPageBreak -> "insert_page_break"
        | Text.InsertText -> "insert_text"
        | Text.IntegrationInstructions -> "integration_instructions"
        | Text.Inventory -> "inventory"
        | Text.Join -> "join"
        | Text.JoinInner -> "join_inner"
        | Text.JoinLeft -> "join_left"
        | Text.JoinRight -> "join_right"
        | Text.LabProfile -> "lab_profile"
        | Text.LanguageChineseArray -> "language_chinese_array"
        | Text.LanguageChineseCangjie -> "language_chinese_cangjie"
        | Text.LanguageChineseDayi -> "language_chinese_dayi"
        | Text.LanguageChinesePinyin -> "language_chinese_pinyin"
        | Text.LanguageChineseQuick -> "language_chinese_quick"
        | Text.LanguageChineseWubi -> "language_chinese_wubi"
        | Text.LanguageFrench -> "language_french"
        | Text.LanguageGbEnglish -> "language_gb_english"
        | Text.LanguageInternational -> "language_international"
        | Text.LanguageJapaneseKana -> "language_japanese_kana"
        | Text.LanguageKoreanLatin -> "language_korean_latin"
        | Text.LanguagePinyin -> "language_pinyin"
        | Text.LanguageSpanish -> "language_spanish"
        | Text.LanguageUs -> "language_us"
        | Text.LanguageUsColemak -> "language_us_colemak"
        | Text.LanguageUsDvorak -> "language_us_dvorak"
        | Text.LassoSelect -> "lasso_select"
        | Text.LetterSwitch -> "letter_switch"
        | Text.LineAxis -> "line_axis"
        | Text.LineCurve -> "line_curve"
        | Text.LineEnd -> "line_end"
        | Text.LineEndArrow -> "line_end_arrow"
        | Text.LineEndArrowNotch -> "line_end_arrow_notch"
        | Text.LineEndCircle -> "line_end_circle"
        | Text.LineEndDiamond -> "line_end_diamond"
        | Text.LineEndSquare -> "line_end_square"
        | Text.LineStart -> "line_start"
        | Text.LineStartArrow -> "line_start_arrow"
        | Text.LineStartArrowNotch -> "line_start_arrow_notch"
        | Text.LineStartCircle -> "line_start_circle"
        | Text.LineStartDiamond -> "line_start_diamond"
        | Text.LineStartSquare -> "line_start_square"
        | Text.LineStyle -> "line_style"
        | Text.LineWeight -> "line_weight"
        | Text.LinearScale -> "linear_scale"
        | Text.List -> "list"
        | Text.ListAlt -> "list_alt"
        | Text.ListAltAdd -> "list_alt_add"
        | Text.ListAltCheck -> "list_alt_check"
        | Text.LocationChip -> "location_chip"
        | Text.LowPriority -> "low_priority"
        | Text.Lowercase -> "lowercase"
        | Text.Margin -> "margin"
        | Text.Markdown -> "markdown"
        | Text.MarkdownCopy -> "markdown_copy"
        | Text.MarkdownPaste -> "markdown_paste"
        | Text.MatchCase -> "match_case"
        | Text.MatchCaseOff -> "match_case_off"
        | Text.MatchWord -> "match_word"
        | Text.MenuBook -> "menu_book"
        | Text.MergeType -> "merge_type"
        | Text.News -> "news"
        | Text.Newsmode -> "newsmode"
        | Text.Newspaper -> "newspaper"
        | Text.NoteAdd -> "note_add"
        | Text.NoteAlt -> "note_alt"
        | Text.NoteStack -> "note_stack"
        | Text.NoteStackAdd -> "note_stack_add"
        | Text.Notes -> "notes"
        | Text.Numbers -> "numbers"
        | Text.OtherAdmission -> "other_admission"
        | Text.Overview -> "overview"
        | Text.Padding -> "padding"
        | Text.PageFooter -> "page_footer"
        | Text.PageHeader -> "page_header"
        | Text.Pageless -> "pageless"
        | Text.Pages -> "pages"
        | Text.PenSize1 -> "pen_size_1"
        | Text.PenSize2 -> "pen_size_2"
        | Text.PenSize3 -> "pen_size_3"
        | Text.PenSize4 -> "pen_size_4"
        | Text.PenSize5 -> "pen_size_5"
        | Text.PendingActions -> "pending_actions"
        | Text.Pentagon -> "pentagon"
        | Text.Percent -> "percent"
        | Text.PermMedia -> "perm_media"
        | Text.PersonBook -> "person_book"
        | Text.PivotTableChart -> "pivot_table_chart"
        | Text.Plagiarism -> "plagiarism"
        | Text.Polyline -> "polyline"
        | Text.Post -> "post"
        | Text.PostAdd -> "post_add"
        | Text.ProcessChart -> "process_chart"
        | Text.ReadMore -> "read_more"
        | Text.Rectangle -> "rectangle"
        | Text.RegularExpression -> "regular_expression"
        | Text.RemoveSelection -> "remove_selection"
        | Text.Reorder -> "reorder"
        | Text.RequestPage -> "request_page"
        | Text.RequestQuote -> "request_quote"
        | Text.ResetImage -> "reset_image"
        | Text.RestorePage -> "restore_page"
        | Text.Rubric -> "rubric"
        | Text.RuleFolder -> "rule_folder"
        | Text.Scan -> "scan"
        | Text.ScanDelete -> "scan_delete"
        | Text.Script -> "script"
        | Text.Segment -> "segment"
        | Text.Select -> "select"
        | Text.Serif -> "serif"
        | Text.ShapeLine -> "shape_line"
        | Text.Shapes -> "shapes"
        | Text.SheetsRtl -> "sheets_rtl"
        | Text.ShortText -> "short_text"
        | Text.Signature -> "signature"
        | Text.SlabSerif -> "slab_serif"
        | Text.SlideLibrary -> "slide_library"
        | Text.SmbShare -> "smb_share"
        | Text.SnippetFolder -> "snippet_folder"
        | Text.SourceNotes -> "source_notes"
        | Text.SpaceBar -> "space_bar"
        | Text.SpaceDashboard -> "space_dashboard"
        | Text.SpecialCharacter -> "special_character"
        | Text.Spellcheck -> "spellcheck"
        | Text.Square -> "square"
        | Text.StackHexagon -> "stack_hexagon"
        | Text.StickyNote -> "sticky_note"
        | Text.StickyNote2 -> "sticky_note_2"
        | Text.StockMedia -> "stock_media"
        | Text.StrokeFull -> "stroke_full"
        | Text.StrokePartial -> "stroke_partial"
        | Text.StylusBrush -> "stylus_brush"
        | Text.StylusFountainPen -> "stylus_fountain_pen"
        | Text.StylusHighlighter -> "stylus_highlighter"
        | Text.StylusLaserPointer -> "stylus_laser_pointer"
        | Text.StylusPen -> "stylus_pen"
        | Text.StylusPencil -> "stylus_pencil"
        | Text.Subject -> "subject"
        | Text.Subscript -> "subscript"
        | Text.SubtitlesOff -> "subtitles_off"
        | Text.Summarize -> "summarize"
        | Text.Superscript -> "superscript"
        | Text.Table -> "table"
        | Text.TableChart -> "table_chart"
        | Text.TableChartView -> "table_chart_view"
        | Text.TableConvert -> "table_convert"
        | Text.TableEdit -> "table_edit"
        | Text.TableEye -> "table_eye"
        | Text.TableRows -> "table_rows"
        | Text.TableRowsNarrow -> "table_rows_narrow"
        | Text.TableView -> "table_view"
        | Text.Tag -> "tag"
        | Text.Task -> "task"
        | Text.TeamDashboard -> "team_dashboard"
        | Text.TextAd -> "text_ad"
        | Text.TextCompare -> "text_compare"
        | Text.TextDecrease -> "text_decrease"
        | Text.TextFields -> "text_fields"
        | Text.TextFieldsAlt -> "text_fields_alt"
        | Text.TextFormat -> "text_format"
        | Text.TextIncrease -> "text_increase"
        | Text.TextRotateUp -> "text_rotate_up"
        | Text.TextRotateVertical -> "text_rotate_vertical"
        | Text.TextRotationAngledown -> "text_rotation_angledown"
        | Text.TextRotationAngleup -> "text_rotation_angleup"
        | Text.TextRotationDown -> "text_rotation_down"
        | Text.TextRotationNone -> "text_rotation_none"
        | Text.TextSelectEnd -> "text_select_end"
        | Text.TextSelectJumpToBeginning -> "text_select_jump_to_beginning"
        | Text.TextSelectJumpToEnd -> "text_select_jump_to_end"
        | Text.TextSelectMoveBackCharacter -> "text_select_move_back_character"
        | Text.TextSelectMoveBackWord -> "text_select_move_back_word"
        | Text.TextSelectMoveDown -> "text_select_move_down"
        | Text.TextSelectMoveForwardCharacter -> "text_select_move_forward_character"
        | Text.TextSelectMoveForwardWord -> "text_select_move_forward_word"
        | Text.TextSelectMoveUp -> "text_select_move_up"
        | Text.TextSelectStart -> "text_select_start"
        | Text.TextSnippet -> "text_snippet"
        | Text.TextUp -> "text_up"
        | Text.ThumbnailBar -> "thumbnail_bar"
        | Text.Title -> "title"
        | Text.Titlecase -> "titlecase"
        | Text.Toc -> "toc"
        | Text.TopPanelClose -> "top_panel_close"
        | Text.TopPanelOpen -> "top_panel_open"
        | Text.Tsv -> "tsv"
        | Text.TwoPager -> "two_pager"
        | Text.TwoPagerStore -> "two_pager_store"
        | Text.TypeSpecimen -> "type_specimen"
        | Text.Ungroup -> "ungroup"
        | Text.Unknown7 -> "unknown_7"
        | Text.UnknownDocument -> "unknown_document"
        | Text.Uppercase -> "uppercase"
        | Text.VariableAdd -> "variable_add"
        | Text.VariableInsert -> "variable_insert"
        | Text.VariableRemove -> "variable_remove"
        | Text.Variables -> "variables"
        | Text.VerticalAlignBottom -> "vertical_align_bottom"
        | Text.VerticalAlignCenter -> "vertical_align_center"
        | Text.VerticalAlignTop -> "vertical_align_top"
        | Text.VerticalDistribute -> "vertical_distribute"
        | Text.VerticalSplit -> "vertical_split"
        | Text.VideoFile -> "video_file"
        | Text.ViewAgenda -> "view_agenda"
        | Text.ViewArray -> "view_array"
        | Text.ViewCarousel -> "view_carousel"
        | Text.ViewColumn -> "view_column"
        | Text.ViewColumn2 -> "view_column_2"
        | Text.ViewDay -> "view_day"
        | Text.ViewHeadline -> "view_headline"
        | Text.ViewList -> "view_list"
        | Text.ViewModule -> "view_module"
        | Text.ViewObjectTrack -> "view_object_track"
        | Text.ViewQuilt -> "view_quilt"
        | Text.ViewSidebar -> "view_sidebar"
        | Text.ViewStream -> "view_stream"
        | Text.ViewWeek -> "view_week"
        | Text.VotingChip -> "voting_chip"
        | Text.WrapText -> "wrap_text"

    [<RequireQualifiedAccess; Struct>]
    type Transit =
      | Agriculture
      | Airlines
      | AirportShuttle
      | Ambulance
      | AutoTowing
      | AutoTransmission
      | BikeDock
      | BikeLane
      | BikeScooter
      | BoatBus
      | BoatRailway
      | BusAlert
      | BusRailway
      | CableCar
      | CarCrash
      | CarDefrostLeft
      | CarDefrostLowLeft
      | CarDefrostLowRight
      | CarDefrostMidLeft
      | CarDefrostMidLowLeft
      | CarDefrostMidLowRight
      | CarDefrostMidRight
      | CarDefrostRight
      | CarFanLowLeft
      | CarFanLowMidLeft
      | CarFanLowRight
      | CarFanMidLeft
      | CarFanMidLowRight
      | CarFanMidRight
      | CarFanRecirculate
      | CarGear
      | CarLock
      | CarMirrorHeat
      | CarTag
      | Commute
      | DepartureBoard
      | DirectionsBike
      | DirectionsBoat
      | DirectionsBus
      | DirectionsCar
      | DirectionsRailway
      | DirectionsRailway2
      | DirectionsRun
      | DirectionsSubway
      | DirectionsWalk
      | ElectricBike
      | ElectricCar
      | ElectricMoped
      | ElectricRickshaw
      | ElectricScooter
      | FanFocus
      | FanIndirect
      | Flight
      | FlightLand
      | FlightTakeoff
      | Flyover
      | ForkLeft
      | ForkRight
      | Funicular
      | GarageCheck
      | GarageMoney
      | GondolaLift
      | Hail
      | Hov
      | HvacMaxDefrost
      | LocalShipping
      | LocalTaxi
      | Metro
      | Monorail
      | Moped
      | MopedPackage
      | Motorcycle
      | NoCrash
      | NoTransfer
      | ParkingMeter
      | ParkingSign
      | ParkingValet
      | PedalBike
      | PlaneContrails
      | RailwayAlert
      | RailwayAlert2
      | Road
      | RvHookup
      | Sailing
      | Scooter
      | SeatCoolLeft
      | SeatCoolRight
      | SeatHeatLeft
      | SeatHeatRight
      | SeatVentLeft
      | SeatVentRight
      | Snowmobile
      | SpeedCamera
      | SteeringWheelHeat
      | Subway
      | SubwayWalk
      | SwapDrivingApps
      | SwapDrivingAppsWheel
      | TaxiAlert
      | TireRepair
      | TrafficJam
      | Train
      | Tram
      | TransitTicket
      | Transportation
      | TrolleyCableCar
      | TurnLeft
      | TurnRight
      | TurnSharpLeft
      | TurnSharpRight
      | TurnSlightLeft
      | TurnSlightRight
      | TwoWheeler
      | UTurnLeft
      | UTurnRight
      | UnpavedRoad
      | WindshieldDefrostAuto
      | WindshieldDefrostFront
      | WindshieldDefrostRear
      | WindshieldHeatFront

    module Transit =

      let toSnakeCase (icon: Transit) =
        match icon with
        | Transit.Agriculture -> "agriculture"
        | Transit.Airlines -> "airlines"
        | Transit.AirportShuttle -> "airport_shuttle"
        | Transit.Ambulance -> "ambulance"
        | Transit.AutoTowing -> "auto_towing"
        | Transit.AutoTransmission -> "auto_transmission"
        | Transit.BikeDock -> "bike_dock"
        | Transit.BikeLane -> "bike_lane"
        | Transit.BikeScooter -> "bike_scooter"
        | Transit.BoatBus -> "boat_bus"
        | Transit.BoatRailway -> "boat_railway"
        | Transit.BusAlert -> "bus_alert"
        | Transit.BusRailway -> "bus_railway"
        | Transit.CableCar -> "cable_car"
        | Transit.CarCrash -> "car_crash"
        | Transit.CarDefrostLeft -> "car_defrost_left"
        | Transit.CarDefrostLowLeft -> "car_defrost_low_left"
        | Transit.CarDefrostLowRight -> "car_defrost_low_right"
        | Transit.CarDefrostMidLeft -> "car_defrost_mid_left"
        | Transit.CarDefrostMidLowLeft -> "car_defrost_mid_low_left"
        | Transit.CarDefrostMidLowRight -> "car_defrost_mid_low_right"
        | Transit.CarDefrostMidRight -> "car_defrost_mid_right"
        | Transit.CarDefrostRight -> "car_defrost_right"
        | Transit.CarFanLowLeft -> "car_fan_low_left"
        | Transit.CarFanLowMidLeft -> "car_fan_low_mid_left"
        | Transit.CarFanLowRight -> "car_fan_low_right"
        | Transit.CarFanMidLeft -> "car_fan_mid_left"
        | Transit.CarFanMidLowRight -> "car_fan_mid_low_right"
        | Transit.CarFanMidRight -> "car_fan_mid_right"
        | Transit.CarFanRecirculate -> "car_fan_recirculate"
        | Transit.CarGear -> "car_gear"
        | Transit.CarLock -> "car_lock"
        | Transit.CarMirrorHeat -> "car_mirror_heat"
        | Transit.CarTag -> "car_tag"
        | Transit.Commute -> "commute"
        | Transit.DepartureBoard -> "departure_board"
        | Transit.DirectionsBike -> "directions_bike"
        | Transit.DirectionsBoat -> "directions_boat"
        | Transit.DirectionsBus -> "directions_bus"
        | Transit.DirectionsCar -> "directions_car"
        | Transit.DirectionsRailway -> "directions_railway"
        | Transit.DirectionsRailway2 -> "directions_railway_2"
        | Transit.DirectionsRun -> "directions_run"
        | Transit.DirectionsSubway -> "directions_subway"
        | Transit.DirectionsWalk -> "directions_walk"
        | Transit.ElectricBike -> "electric_bike"
        | Transit.ElectricCar -> "electric_car"
        | Transit.ElectricMoped -> "electric_moped"
        | Transit.ElectricRickshaw -> "electric_rickshaw"
        | Transit.ElectricScooter -> "electric_scooter"
        | Transit.FanFocus -> "fan_focus"
        | Transit.FanIndirect -> "fan_indirect"
        | Transit.Flight -> "flight"
        | Transit.FlightLand -> "flight_land"
        | Transit.FlightTakeoff -> "flight_takeoff"
        | Transit.Flyover -> "flyover"
        | Transit.ForkLeft -> "fork_left"
        | Transit.ForkRight -> "fork_right"
        | Transit.Funicular -> "funicular"
        | Transit.GarageCheck -> "garage_check"
        | Transit.GarageMoney -> "garage_money"
        | Transit.GondolaLift -> "gondola_lift"
        | Transit.Hail -> "hail"
        | Transit.Hov -> "hov"
        | Transit.HvacMaxDefrost -> "hvac_max_defrost"
        | Transit.LocalShipping -> "local_shipping"
        | Transit.LocalTaxi -> "local_taxi"
        | Transit.Metro -> "metro"
        | Transit.Monorail -> "monorail"
        | Transit.Moped -> "moped"
        | Transit.MopedPackage -> "moped_package"
        | Transit.Motorcycle -> "motorcycle"
        | Transit.NoCrash -> "no_crash"
        | Transit.NoTransfer -> "no_transfer"
        | Transit.ParkingMeter -> "parking_meter"
        | Transit.ParkingSign -> "parking_sign"
        | Transit.ParkingValet -> "parking_valet"
        | Transit.PedalBike -> "pedal_bike"
        | Transit.PlaneContrails -> "plane_contrails"
        | Transit.RailwayAlert -> "railway_alert"
        | Transit.RailwayAlert2 -> "railway_alert_2"
        | Transit.Road -> "road"
        | Transit.RvHookup -> "rv_hookup"
        | Transit.Sailing -> "sailing"
        | Transit.Scooter -> "scooter"
        | Transit.SeatCoolLeft -> "seat_cool_left"
        | Transit.SeatCoolRight -> "seat_cool_right"
        | Transit.SeatHeatLeft -> "seat_heat_left"
        | Transit.SeatHeatRight -> "seat_heat_right"
        | Transit.SeatVentLeft -> "seat_vent_left"
        | Transit.SeatVentRight -> "seat_vent_right"
        | Transit.Snowmobile -> "snowmobile"
        | Transit.SpeedCamera -> "speed_camera"
        | Transit.SteeringWheelHeat -> "steering_wheel_heat"
        | Transit.Subway -> "subway"
        | Transit.SubwayWalk -> "subway_walk"
        | Transit.SwapDrivingApps -> "swap_driving_apps"
        | Transit.SwapDrivingAppsWheel -> "swap_driving_apps_wheel"
        | Transit.TaxiAlert -> "taxi_alert"
        | Transit.TireRepair -> "tire_repair"
        | Transit.TrafficJam -> "traffic_jam"
        | Transit.Train -> "train"
        | Transit.Tram -> "tram"
        | Transit.TransitTicket -> "transit_ticket"
        | Transit.Transportation -> "transportation"
        | Transit.TrolleyCableCar -> "trolley_cable_car"
        | Transit.TurnLeft -> "turn_left"
        | Transit.TurnRight -> "turn_right"
        | Transit.TurnSharpLeft -> "turn_sharp_left"
        | Transit.TurnSharpRight -> "turn_sharp_right"
        | Transit.TurnSlightLeft -> "turn_slight_left"
        | Transit.TurnSlightRight -> "turn_slight_right"
        | Transit.TwoWheeler -> "two_wheeler"
        | Transit.UTurnLeft -> "u_turn_left"
        | Transit.UTurnRight -> "u_turn_right"
        | Transit.UnpavedRoad -> "unpaved_road"
        | Transit.WindshieldDefrostAuto -> "windshield_defrost_auto"
        | Transit.WindshieldDefrostFront -> "windshield_defrost_front"
        | Transit.WindshieldDefrostRear -> "windshield_defrost_rear"
        | Transit.WindshieldHeatFront -> "windshield_heat_front"

    [<RequireQualifiedAccess; Struct>]
    type Travel =
      | AirlineSeatFlat
      | AirlineSeatFlatAngled
      | AirlineSeatIndividualSuite
      | AirlineSeatLegroomExtra
      | AirlineSeatLegroomNormal
      | AirlineSeatLegroomReduced
      | AirlineSeatReclineExtra
      | AirlineSeatReclineNormal
      | AirplaneTicket
      | Apartment
      | Attractions
      | BakeryDining
      | BathBedrock
      | BeachAccess
      | BeerMeal
      | Bento
      | BreakfastDining
      | BrunchDining
      | Bungalow
      | Cabin
      | CarRental
      | CarRepair
      | Carpenter
      | CarryOnBag
      | CarryOnBagChecked
      | CarryOnBagInactive
      | CarryOnBagQuestion
      | Casino
      | Chalet
      | CheckedBag
      | CheckedBagQuestion
      | ChildFriendly
      | Concierge
      | Cottage
      | DinnerDining
      | DoNotStep
      | DoNotTouch
      | Elevator
      | Escalator
      | EscalatorWarning
      | FamilyRestroom
      | Festival
      | FitnessCenter
      | FlightsAndHotels
      | FoodBank
      | Gite
      | GolfCourse
      | HolidayVillage
      | Hotel
      | Houseboat
      | Icecream
      | JapaneseCurry
      | JapaneseFlag
      | Liquor
      | LocalBar
      | LocalCafe
      | LocalDining
      | LocationCity
      | Luggage
      | LunchDining
      | MountainSteam
      | Museum
      | NightShelter
      | Nightlife
      | NoDrinks
      | NoFood
      | NoLuggage
      | NoStroller
      | Okonomiyaki
      | OtherHouses
      | Passport
      | PersonalBag
      | PersonalBagOff
      | PersonalBagQuestion
      | PersonalPlaces
      | Pool
      | RamenDining
      | RiceBowl
      | RoomService
      | SmokeFree
      | SmokingRooms
      | Spa
      | SportsBar
      | Stairs
      | Stairs2
      | Tapas
      | Travel
      | TravelLuggageAndBags
      | Trip
      | Villa
      | Washoku
      | WheelchairPickup
      | Yoshoku
      | YourTrips

    module Travel =

      let toSnakeCase (icon: Travel) =
        match icon with
        | Travel.AirlineSeatFlat -> "airline_seat_flat"
        | Travel.AirlineSeatFlatAngled -> "airline_seat_flat_angled"
        | Travel.AirlineSeatIndividualSuite -> "airline_seat_individual_suite"
        | Travel.AirlineSeatLegroomExtra -> "airline_seat_legroom_extra"
        | Travel.AirlineSeatLegroomNormal -> "airline_seat_legroom_normal"
        | Travel.AirlineSeatLegroomReduced -> "airline_seat_legroom_reduced"
        | Travel.AirlineSeatReclineExtra -> "airline_seat_recline_extra"
        | Travel.AirlineSeatReclineNormal -> "airline_seat_recline_normal"
        | Travel.AirplaneTicket -> "airplane_ticket"
        | Travel.Apartment -> "apartment"
        | Travel.Attractions -> "attractions"
        | Travel.BakeryDining -> "bakery_dining"
        | Travel.BathBedrock -> "bath_bedrock"
        | Travel.BeachAccess -> "beach_access"
        | Travel.BeerMeal -> "beer_meal"
        | Travel.Bento -> "bento"
        | Travel.BreakfastDining -> "breakfast_dining"
        | Travel.BrunchDining -> "brunch_dining"
        | Travel.Bungalow -> "bungalow"
        | Travel.Cabin -> "cabin"
        | Travel.CarRental -> "car_rental"
        | Travel.CarRepair -> "car_repair"
        | Travel.Carpenter -> "carpenter"
        | Travel.CarryOnBag -> "carry_on_bag"
        | Travel.CarryOnBagChecked -> "carry_on_bag_checked"
        | Travel.CarryOnBagInactive -> "carry_on_bag_inactive"
        | Travel.CarryOnBagQuestion -> "carry_on_bag_question"
        | Travel.Casino -> "casino"
        | Travel.Chalet -> "chalet"
        | Travel.CheckedBag -> "checked_bag"
        | Travel.CheckedBagQuestion -> "checked_bag_question"
        | Travel.ChildFriendly -> "child_friendly"
        | Travel.Concierge -> "concierge"
        | Travel.Cottage -> "cottage"
        | Travel.DinnerDining -> "dinner_dining"
        | Travel.DoNotStep -> "do_not_step"
        | Travel.DoNotTouch -> "do_not_touch"
        | Travel.Elevator -> "elevator"
        | Travel.Escalator -> "escalator"
        | Travel.EscalatorWarning -> "escalator_warning"
        | Travel.FamilyRestroom -> "family_restroom"
        | Travel.Festival -> "festival"
        | Travel.FitnessCenter -> "fitness_center"
        | Travel.FlightsAndHotels -> "flights_and_hotels"
        | Travel.FoodBank -> "food_bank"
        | Travel.Gite -> "gite"
        | Travel.GolfCourse -> "golf_course"
        | Travel.HolidayVillage -> "holiday_village"
        | Travel.Hotel -> "hotel"
        | Travel.Houseboat -> "houseboat"
        | Travel.Icecream -> "icecream"
        | Travel.JapaneseCurry -> "japanese_curry"
        | Travel.JapaneseFlag -> "japanese_flag"
        | Travel.Liquor -> "liquor"
        | Travel.LocalBar -> "local_bar"
        | Travel.LocalCafe -> "local_cafe"
        | Travel.LocalDining -> "local_dining"
        | Travel.LocationCity -> "location_city"
        | Travel.Luggage -> "luggage"
        | Travel.LunchDining -> "lunch_dining"
        | Travel.MountainSteam -> "mountain_steam"
        | Travel.Museum -> "museum"
        | Travel.NightShelter -> "night_shelter"
        | Travel.Nightlife -> "nightlife"
        | Travel.NoDrinks -> "no_drinks"
        | Travel.NoFood -> "no_food"
        | Travel.NoLuggage -> "no_luggage"
        | Travel.NoStroller -> "no_stroller"
        | Travel.Okonomiyaki -> "okonomiyaki"
        | Travel.OtherHouses -> "other_houses"
        | Travel.Passport -> "passport"
        | Travel.PersonalBag -> "personal_bag"
        | Travel.PersonalBagOff -> "personal_bag_off"
        | Travel.PersonalBagQuestion -> "personal_bag_question"
        | Travel.PersonalPlaces -> "personal_places"
        | Travel.Pool -> "pool"
        | Travel.RamenDining -> "ramen_dining"
        | Travel.RiceBowl -> "rice_bowl"
        | Travel.RoomService -> "room_service"
        | Travel.SmokeFree -> "smoke_free"
        | Travel.SmokingRooms -> "smoking_rooms"
        | Travel.Spa -> "spa"
        | Travel.SportsBar -> "sports_bar"
        | Travel.Stairs -> "stairs"
        | Travel.Stairs2 -> "stairs_2"
        | Travel.Tapas -> "tapas"
        | Travel.Travel -> "travel"
        | Travel.TravelLuggageAndBags -> "travel_luggage_and_bags"
        | Travel.Trip -> "trip"
        | Travel.Villa -> "villa"
        | Travel.Washoku -> "washoku"
        | Travel.WheelchairPickup -> "wheelchair_pickup"
        | Travel.Yoshoku -> "yoshoku"
        | Travel.YourTrips -> "your_trips"

    [<RequireQualifiedAccess; Struct>]
    type UiActions =
      | ``123``
      | Abc
      | AccessibleMenu
      | ActionKey
      | Acute
      | Add
      | Add2
      | AddBox
      | AddCircle
      | AddTask
      | AllMatch
      | Amend
      | AppBadging
      | AppRegistration
      | Apps
      | AppsOutage
      | ArrowAndEdge
      | ArrowBack
      | ArrowBack2
      | ArrowBackIos
      | ArrowBackIosNew
      | ArrowCircleDown
      | ArrowCircleLeft
      | ArrowCircleRight
      | ArrowCircleUp
      | ArrowDownward
      | ArrowDownwardAlt
      | ArrowDropDown
      | ArrowDropDownCircle
      | ArrowDropUp
      | ArrowForward
      | ArrowForwardIos
      | ArrowInsert
      | ArrowLeft
      | ArrowLeftAlt
      | ArrowMenuClose
      | ArrowMenuOpen
      | ArrowOrEdge
      | ArrowOutward
      | ArrowRange
      | ArrowRight
      | ArrowRightAlt
      | ArrowShapeUp
      | ArrowShapeUpStack
      | ArrowShapeUpStack2
      | ArrowSplit
      | ArrowTopLeft
      | ArrowTopRight
      | ArrowUploadProgress
      | ArrowUploadReady
      | ArrowUpward
      | ArrowUpwardAlt
      | ArrowsInput
      | ArrowsOutput
      | ArrowsOutward
      | AssistantDirection
      | AssistantNavigation
      | Autorenew
      | BackToTab
      | Backspace
      | Block
      | Bolt
      | Borg
      | BottomAppBar
      | BottomDrawer
      | BottomNavigation
      | BottomPanelClose
      | BottomPanelOpen
      | BottomRightClick
      | BottomSheets
      | BrowseActivity
      | BrowseGallery
      | Bubble
      | Bubbles
      | BucketCheck
      | ButtonsAlt
      | Cached
      | Cancel
      | CaptivePortal
      | Capture
      | Cards
      | CategorySearch
      | ChangeCircle
      | Check
      | CheckBox
      | CheckBoxOutlineBlank
      | CheckCircle
      | CheckCircleUnread
      | CheckIndeterminateSmall
      | CheckSmall
      | ChevronBackward
      | ChevronForward
      | ChevronLeft
      | ChevronLineUp
      | ChevronRight
      | ChipExtraction
      | Chips
      | Chronic
      | ClearAll
      | ClockArrowDown
      | ClockArrowUp
      | ClockLoader10
      | ClockLoader20
      | ClockLoader40
      | ClockLoader60
      | ClockLoader80
      | ClockLoader90
      | Close
      | CloseFullscreen
      | CloseSmall
      | CollapseAll
      | CollapseContent
      | CompareArrows
      | Compress
      | CreateNewFolder
      | Css
      | Cycle
      | DataAlert
      | DataCheck
      | DataInfoAlert
      | DataThresholding
      | Dataset
      | DatasetLinked
      | Delete
      | DeleteForever
      | DeleteSweep
      | DensityLarge
      | DensityMedium
      | DensitySmall
      | DeployedCode
      | DeployedCodeAccount
      | DeployedCodeAlert
      | DeployedCodeHistory
      | DeployedCodeUpdate
      | DesktopLandscape
      | DesktopLandscapeAdd
      | DesktopPortrait
      | Dialogs
      | DirectorySync
      | DisabledByDefault
      | DoNotDisturbOff
      | DoNotDisturbOn
      | DoneAll
      | DoneOutline
      | DoubleArrow
      | Download
      | Download2
      | DownloadDone
      | DownloadForOffline
      | Downloading
      | DragClick
      | DragIndicator
      | DragPan
      | Dropdown
      | DynamicForm
      | EditArrowDown
      | EditArrowUp
      | Eject
      | EmptyDashboard
      | Enable
      | ErrorMed
      | EventList
      | ExitToApp
      | Expand
      | ExpandAll
      | ExpandCircleDown
      | ExpandCircleRight
      | ExpandCircleUp
      | ExpandContent
      | ExpansionPanels
      | ExtensionOff
      | Favorite
      | FileDownloadOff
      | FileExport
      | FileJson
      | FileOpen
      | FileUploadOff
      | FilterAlt
      | FilterAltOff
      | FilterArrowRight
      | FilterList
      | FilterListOff
      | FirstPage
      | FitScreen
      | FloatLandscape2
      | FloatPortrait2
      | Forward
      | FrameBug
      | FrameExclamation
      | Fullscreen
      | FullscreenExit
      | FullscreenPortrait
      | GoToLine
      | HeartCheck
      | HeartMinus
      | HeartPlus
      | Hide
      | HideSource
      | HighlightKeyboardFocus
      | HighlightMouseCursor
      | HighlightTextCursor
      | Hls
      | HlsOff
      | Home
      | HourglassArrowDown
      | HourglassArrowUp
      | Html
      | Iframe
      | IframeOff
      | IndeterminateCheckBox
      | InputCircle
      | InstallDesktop
      | IosShare
      | Javascript
      | JumpToElement
      | Key
      | KeyOff
      | KeyVertical
      | KeyboardCommandKey
      | KeyboardControlKey
      | KeyboardDoubleArrowDown
      | KeyboardDoubleArrowLeft
      | KeyboardDoubleArrowRight
      | KeyboardDoubleArrowUp
      | KeyboardOptionKey
      | LastPage
      | LeftClick
      | LeftPanelClose
      | LeftPanelOpen
      | LibraryAdd
      | LinkedServices
      | Login
      | Logout
      | MagnificationLarge
      | MagnificationSmall
      | ManageSearch
      | Maximize
      | Menu
      | MenuOpen
      | Minimize
      | Modeling
      | MoreDown
      | MoreHoriz
      | MoreUp
      | MoreVert
      | MoveDown
      | MoveGroup
      | MoveItem
      | MoveSelectionDown
      | MoveSelectionLeft
      | MoveSelectionRight
      | MoveSelectionUp
      | MoveUp
      | MultimodalHandEye
      | NewWindow
      | OpenInFull
      | OpenInNew
      | OpenInNewDown
      | OpenInNewOff
      | OpenRun
      | OpenWith
      | Output
      | OutputCircle
      | PageControl
      | PageInfo
      | PageMenuIos
      | PartnerReports
      | PatientList
      | Php
      | Pinch
      | Pip
      | PipExit
      | PlaceItem
      | PointScan
      | PositionBottomLeft
      | PositionBottomRight
      | PositionTopRight
      | Preliminary
      | ProgressActivity
      | PromptSuggestion
      | Publish
      | QuestionExchange
      | QuickReference
      | QuickReferenceAll
      | RadioButtonChecked
      | RadioButtonPartial
      | RadioButtonUnchecked
      | Rebase
      | RebaseEdit
      | Recenter
      | Redo
      | Refresh
      | Remove
      | RemoveDone
      | ReopenWindow
      | Repartition
      | Reply
      | ReplyAll
      | Resize
      | ResponsiveLayout
      | RestartAlt
      | RestoreFromTrash
      | RightClick
      | RightPanelClose
      | RightPanelOpen
      | Ripples
      | RotateAuto
      | RuleSettings
      | SavedSearch
      | Search
      | SearchCheck
      | SearchCheck2
      | SearchGear
      | SearchOff
      | SelectAll
      | SelectCheckBox
      | SendTimeExtension
      | Settings
      | SettingsAccessibility
      | SettingsApplications
      | SettingsBackupRestore
      | SettingsHeart
      | ShareReviews
      | ShareWindows
      | ShelfAutoHide
      | ShelfPosition
      | ShoppingCartCheckout
      | SideNavigation
      | Sliders
      | Sort
      | SortByAlpha
      | SplitscreenLandscape
      | SplitscreenPortrait
      | Stack
      | StackGroup
      | StackOff
      | StackStar
      | Stacks
      | Star
      | StarHalf
      | StarRate
      | StarRateHalf
      | Start
      | Stat0
      | Stat1
      | Stat2
      | Stat3
      | StatMinus1
      | StatMinus2
      | StatMinus3
      | Step
      | StepInto
      | StepOut
      | StepOver
      | Steppers
      | SubdirectoryArrowLeft
      | SubdirectoryArrowRight
      | Subheader
      | SwapHoriz
      | SwapHorizontalCircle
      | SwapVert
      | SwapVerticalCircle
      | Sweep
      | SwipeDown
      | SwipeDownAlt
      | SwipeLeft
      | SwipeLeftAlt
      | SwipeRight
      | SwipeRightAlt
      | SwipeUp
      | SwipeUpAlt
      | SwipeVertical
      | SwitchAccess
      | SwitchAccess2
      | SwitchAccess3
      | SwitchAccessShortcut
      | SwitchAccessShortcutAdd
      | SwitchLeft
      | SwitchRight
      | Switches
      | Sync
      | SyncAlt
      | SyncArrowDown
      | SyncArrowUp
      | SyncDesktop
      | SyncDisabled
      | SyncProblem
      | SyncSavedLocally
      | SyncSavedLocallyOff
      | SystemUpdateAlt
      | Tab
      | TabClose
      | TabCloseInactive
      | TabCloseRight
      | TabDuplicate
      | TabGroup
      | TabInactive
      | TabMove
      | TabNewRight
      | TabRecent
      | TabSearch
      | TabUnselected
      | Tabs
      | Terminal
      | ThermostatArrowDown
      | ThermostatArrowUp
      | TileLarge
      | TileMedium
      | TileSmall
      | TimerArrowDown
      | TimerArrowUp
      | Toast
      | ToggleOff
      | ToggleOn
      | Token
      | Toolbar
      | Undo
      | UnfoldLess
      | UnfoldLessDouble
      | UnfoldMore
      | UnfoldMoreDouble
      | Unknown5
      | UnknownMed
      | Upload
      | Upload2
      | ViewApps
      | ViewComfyAlt
      | ViewCompactAlt
      | ViewCozy
      | ViewKanban
      | ViewTimeline
      | WidgetMedium
      | WidgetMenu
      | WidgetSmall
      | WidgetWidth
      | WidthFull
      | WidthNormal
      | WidthWide
      | YoutubeSearchedFor
      | ZoomIn
      | ZoomOut

    module UiActions =

      let toSnakeCase (icon: UiActions) =
        match icon with
        | UiActions.``123`` -> "123"
        | UiActions.Abc -> "abc"
        | UiActions.AccessibleMenu -> "accessible_menu"
        | UiActions.ActionKey -> "action_key"
        | UiActions.Acute -> "acute"
        | UiActions.Add -> "add"
        | UiActions.Add2 -> "add_2"
        | UiActions.AddBox -> "add_box"
        | UiActions.AddCircle -> "add_circle"
        | UiActions.AddTask -> "add_task"
        | UiActions.AllMatch -> "all_match"
        | UiActions.Amend -> "amend"
        | UiActions.AppBadging -> "app_badging"
        | UiActions.AppRegistration -> "app_registration"
        | UiActions.Apps -> "apps"
        | UiActions.AppsOutage -> "apps_outage"
        | UiActions.ArrowAndEdge -> "arrow_and_edge"
        | UiActions.ArrowBack -> "arrow_back"
        | UiActions.ArrowBack2 -> "arrow_back_2"
        | UiActions.ArrowBackIos -> "arrow_back_ios"
        | UiActions.ArrowBackIosNew -> "arrow_back_ios_new"
        | UiActions.ArrowCircleDown -> "arrow_circle_down"
        | UiActions.ArrowCircleLeft -> "arrow_circle_left"
        | UiActions.ArrowCircleRight -> "arrow_circle_right"
        | UiActions.ArrowCircleUp -> "arrow_circle_up"
        | UiActions.ArrowDownward -> "arrow_downward"
        | UiActions.ArrowDownwardAlt -> "arrow_downward_alt"
        | UiActions.ArrowDropDown -> "arrow_drop_down"
        | UiActions.ArrowDropDownCircle -> "arrow_drop_down_circle"
        | UiActions.ArrowDropUp -> "arrow_drop_up"
        | UiActions.ArrowForward -> "arrow_forward"
        | UiActions.ArrowForwardIos -> "arrow_forward_ios"
        | UiActions.ArrowInsert -> "arrow_insert"
        | UiActions.ArrowLeft -> "arrow_left"
        | UiActions.ArrowLeftAlt -> "arrow_left_alt"
        | UiActions.ArrowMenuClose -> "arrow_menu_close"
        | UiActions.ArrowMenuOpen -> "arrow_menu_open"
        | UiActions.ArrowOrEdge -> "arrow_or_edge"
        | UiActions.ArrowOutward -> "arrow_outward"
        | UiActions.ArrowRange -> "arrow_range"
        | UiActions.ArrowRight -> "arrow_right"
        | UiActions.ArrowRightAlt -> "arrow_right_alt"
        | UiActions.ArrowShapeUp -> "arrow_shape_up"
        | UiActions.ArrowShapeUpStack -> "arrow_shape_up_stack"
        | UiActions.ArrowShapeUpStack2 -> "arrow_shape_up_stack_2"
        | UiActions.ArrowSplit -> "arrow_split"
        | UiActions.ArrowTopLeft -> "arrow_top_left"
        | UiActions.ArrowTopRight -> "arrow_top_right"
        | UiActions.ArrowUploadProgress -> "arrow_upload_progress"
        | UiActions.ArrowUploadReady -> "arrow_upload_ready"
        | UiActions.ArrowUpward -> "arrow_upward"
        | UiActions.ArrowUpwardAlt -> "arrow_upward_alt"
        | UiActions.ArrowsInput -> "arrows_input"
        | UiActions.ArrowsOutput -> "arrows_output"
        | UiActions.ArrowsOutward -> "arrows_outward"
        | UiActions.AssistantDirection -> "assistant_direction"
        | UiActions.AssistantNavigation -> "assistant_navigation"
        | UiActions.Autorenew -> "autorenew"
        | UiActions.BackToTab -> "back_to_tab"
        | UiActions.Backspace -> "backspace"
        | UiActions.Block -> "block"
        | UiActions.Bolt -> "bolt"
        | UiActions.Borg -> "borg"
        | UiActions.BottomAppBar -> "bottom_app_bar"
        | UiActions.BottomDrawer -> "bottom_drawer"
        | UiActions.BottomNavigation -> "bottom_navigation"
        | UiActions.BottomPanelClose -> "bottom_panel_close"
        | UiActions.BottomPanelOpen -> "bottom_panel_open"
        | UiActions.BottomRightClick -> "bottom_right_click"
        | UiActions.BottomSheets -> "bottom_sheets"
        | UiActions.BrowseActivity -> "browse_activity"
        | UiActions.BrowseGallery -> "browse_gallery"
        | UiActions.Bubble -> "bubble"
        | UiActions.Bubbles -> "bubbles"
        | UiActions.BucketCheck -> "bucket_check"
        | UiActions.ButtonsAlt -> "buttons_alt"
        | UiActions.Cached -> "cached"
        | UiActions.Cancel -> "cancel"
        | UiActions.CaptivePortal -> "captive_portal"
        | UiActions.Capture -> "capture"
        | UiActions.Cards -> "cards"
        | UiActions.CategorySearch -> "category_search"
        | UiActions.ChangeCircle -> "change_circle"
        | UiActions.Check -> "check"
        | UiActions.CheckBox -> "check_box"
        | UiActions.CheckBoxOutlineBlank -> "check_box_outline_blank"
        | UiActions.CheckCircle -> "check_circle"
        | UiActions.CheckCircleUnread -> "check_circle_unread"
        | UiActions.CheckIndeterminateSmall -> "check_indeterminate_small"
        | UiActions.CheckSmall -> "check_small"
        | UiActions.ChevronBackward -> "chevron_backward"
        | UiActions.ChevronForward -> "chevron_forward"
        | UiActions.ChevronLeft -> "chevron_left"
        | UiActions.ChevronLineUp -> "chevron_line_up"
        | UiActions.ChevronRight -> "chevron_right"
        | UiActions.ChipExtraction -> "chip_extraction"
        | UiActions.Chips -> "chips"
        | UiActions.Chronic -> "chronic"
        | UiActions.ClearAll -> "clear_all"
        | UiActions.ClockArrowDown -> "clock_arrow_down"
        | UiActions.ClockArrowUp -> "clock_arrow_up"
        | UiActions.ClockLoader10 -> "clock_loader_10"
        | UiActions.ClockLoader20 -> "clock_loader_20"
        | UiActions.ClockLoader40 -> "clock_loader_40"
        | UiActions.ClockLoader60 -> "clock_loader_60"
        | UiActions.ClockLoader80 -> "clock_loader_80"
        | UiActions.ClockLoader90 -> "clock_loader_90"
        | UiActions.Close -> "close"
        | UiActions.CloseFullscreen -> "close_fullscreen"
        | UiActions.CloseSmall -> "close_small"
        | UiActions.CollapseAll -> "collapse_all"
        | UiActions.CollapseContent -> "collapse_content"
        | UiActions.CompareArrows -> "compare_arrows"
        | UiActions.Compress -> "compress"
        | UiActions.CreateNewFolder -> "create_new_folder"
        | UiActions.Css -> "css"
        | UiActions.Cycle -> "cycle"
        | UiActions.DataAlert -> "data_alert"
        | UiActions.DataCheck -> "data_check"
        | UiActions.DataInfoAlert -> "data_info_alert"
        | UiActions.DataThresholding -> "data_thresholding"
        | UiActions.Dataset -> "dataset"
        | UiActions.DatasetLinked -> "dataset_linked"
        | UiActions.Delete -> "delete"
        | UiActions.DeleteForever -> "delete_forever"
        | UiActions.DeleteSweep -> "delete_sweep"
        | UiActions.DensityLarge -> "density_large"
        | UiActions.DensityMedium -> "density_medium"
        | UiActions.DensitySmall -> "density_small"
        | UiActions.DeployedCode -> "deployed_code"
        | UiActions.DeployedCodeAccount -> "deployed_code_account"
        | UiActions.DeployedCodeAlert -> "deployed_code_alert"
        | UiActions.DeployedCodeHistory -> "deployed_code_history"
        | UiActions.DeployedCodeUpdate -> "deployed_code_update"
        | UiActions.DesktopLandscape -> "desktop_landscape"
        | UiActions.DesktopLandscapeAdd -> "desktop_landscape_add"
        | UiActions.DesktopPortrait -> "desktop_portrait"
        | UiActions.Dialogs -> "dialogs"
        | UiActions.DirectorySync -> "directory_sync"
        | UiActions.DisabledByDefault -> "disabled_by_default"
        | UiActions.DoNotDisturbOff -> "do_not_disturb_off"
        | UiActions.DoNotDisturbOn -> "do_not_disturb_on"
        | UiActions.DoneAll -> "done_all"
        | UiActions.DoneOutline -> "done_outline"
        | UiActions.DoubleArrow -> "double_arrow"
        | UiActions.Download -> "download"
        | UiActions.Download2 -> "download_2"
        | UiActions.DownloadDone -> "download_done"
        | UiActions.DownloadForOffline -> "download_for_offline"
        | UiActions.Downloading -> "downloading"
        | UiActions.DragClick -> "drag_click"
        | UiActions.DragIndicator -> "drag_indicator"
        | UiActions.DragPan -> "drag_pan"
        | UiActions.Dropdown -> "dropdown"
        | UiActions.DynamicForm -> "dynamic_form"
        | UiActions.EditArrowDown -> "edit_arrow_down"
        | UiActions.EditArrowUp -> "edit_arrow_up"
        | UiActions.Eject -> "eject"
        | UiActions.EmptyDashboard -> "empty_dashboard"
        | UiActions.Enable -> "enable"
        | UiActions.ErrorMed -> "error_med"
        | UiActions.EventList -> "event_list"
        | UiActions.ExitToApp -> "exit_to_app"
        | UiActions.Expand -> "expand"
        | UiActions.ExpandAll -> "expand_all"
        | UiActions.ExpandCircleDown -> "expand_circle_down"
        | UiActions.ExpandCircleRight -> "expand_circle_right"
        | UiActions.ExpandCircleUp -> "expand_circle_up"
        | UiActions.ExpandContent -> "expand_content"
        | UiActions.ExpansionPanels -> "expansion_panels"
        | UiActions.ExtensionOff -> "extension_off"
        | UiActions.Favorite -> "favorite"
        | UiActions.FileDownloadOff -> "file_download_off"
        | UiActions.FileExport -> "file_export"
        | UiActions.FileJson -> "file_json"
        | UiActions.FileOpen -> "file_open"
        | UiActions.FileUploadOff -> "file_upload_off"
        | UiActions.FilterAlt -> "filter_alt"
        | UiActions.FilterAltOff -> "filter_alt_off"
        | UiActions.FilterArrowRight -> "filter_arrow_right"
        | UiActions.FilterList -> "filter_list"
        | UiActions.FilterListOff -> "filter_list_off"
        | UiActions.FirstPage -> "first_page"
        | UiActions.FitScreen -> "fit_screen"
        | UiActions.FloatLandscape2 -> "float_landscape_2"
        | UiActions.FloatPortrait2 -> "float_portrait_2"
        | UiActions.Forward -> "forward"
        | UiActions.FrameBug -> "frame_bug"
        | UiActions.FrameExclamation -> "frame_exclamation"
        | UiActions.Fullscreen -> "fullscreen"
        | UiActions.FullscreenExit -> "fullscreen_exit"
        | UiActions.FullscreenPortrait -> "fullscreen_portrait"
        | UiActions.GoToLine -> "go_to_line"
        | UiActions.HeartCheck -> "heart_check"
        | UiActions.HeartMinus -> "heart_minus"
        | UiActions.HeartPlus -> "heart_plus"
        | UiActions.Hide -> "hide"
        | UiActions.HideSource -> "hide_source"
        | UiActions.HighlightKeyboardFocus -> "highlight_keyboard_focus"
        | UiActions.HighlightMouseCursor -> "highlight_mouse_cursor"
        | UiActions.HighlightTextCursor -> "highlight_text_cursor"
        | UiActions.Hls -> "hls"
        | UiActions.HlsOff -> "hls_off"
        | UiActions.Home -> "home"
        | UiActions.HourglassArrowDown -> "hourglass_arrow_down"
        | UiActions.HourglassArrowUp -> "hourglass_arrow_up"
        | UiActions.Html -> "html"
        | UiActions.Iframe -> "iframe"
        | UiActions.IframeOff -> "iframe_off"
        | UiActions.IndeterminateCheckBox -> "indeterminate_check_box"
        | UiActions.InputCircle -> "input_circle"
        | UiActions.InstallDesktop -> "install_desktop"
        | UiActions.IosShare -> "ios_share"
        | UiActions.Javascript -> "javascript"
        | UiActions.JumpToElement -> "jump_to_element"
        | UiActions.Key -> "key"
        | UiActions.KeyOff -> "key_off"
        | UiActions.KeyVertical -> "key_vertical"
        | UiActions.KeyboardCommandKey -> "keyboard_command_key"
        | UiActions.KeyboardControlKey -> "keyboard_control_key"
        | UiActions.KeyboardDoubleArrowDown -> "keyboard_double_arrow_down"
        | UiActions.KeyboardDoubleArrowLeft -> "keyboard_double_arrow_left"
        | UiActions.KeyboardDoubleArrowRight -> "keyboard_double_arrow_right"
        | UiActions.KeyboardDoubleArrowUp -> "keyboard_double_arrow_up"
        | UiActions.KeyboardOptionKey -> "keyboard_option_key"
        | UiActions.LastPage -> "last_page"
        | UiActions.LeftClick -> "left_click"
        | UiActions.LeftPanelClose -> "left_panel_close"
        | UiActions.LeftPanelOpen -> "left_panel_open"
        | UiActions.LibraryAdd -> "library_add"
        | UiActions.LinkedServices -> "linked_services"
        | UiActions.Login -> "login"
        | UiActions.Logout -> "logout"
        | UiActions.MagnificationLarge -> "magnification_large"
        | UiActions.MagnificationSmall -> "magnification_small"
        | UiActions.ManageSearch -> "manage_search"
        | UiActions.Maximize -> "maximize"
        | UiActions.Menu -> "menu"
        | UiActions.MenuOpen -> "menu_open"
        | UiActions.Minimize -> "minimize"
        | UiActions.Modeling -> "modeling"
        | UiActions.MoreDown -> "more_down"
        | UiActions.MoreHoriz -> "more_horiz"
        | UiActions.MoreUp -> "more_up"
        | UiActions.MoreVert -> "more_vert"
        | UiActions.MoveDown -> "move_down"
        | UiActions.MoveGroup -> "move_group"
        | UiActions.MoveItem -> "move_item"
        | UiActions.MoveSelectionDown -> "move_selection_down"
        | UiActions.MoveSelectionLeft -> "move_selection_left"
        | UiActions.MoveSelectionRight -> "move_selection_right"
        | UiActions.MoveSelectionUp -> "move_selection_up"
        | UiActions.MoveUp -> "move_up"
        | UiActions.MultimodalHandEye -> "multimodal_hand_eye"
        | UiActions.NewWindow -> "new_window"
        | UiActions.OpenInFull -> "open_in_full"
        | UiActions.OpenInNew -> "open_in_new"
        | UiActions.OpenInNewDown -> "open_in_new_down"
        | UiActions.OpenInNewOff -> "open_in_new_off"
        | UiActions.OpenRun -> "open_run"
        | UiActions.OpenWith -> "open_with"
        | UiActions.Output -> "output"
        | UiActions.OutputCircle -> "output_circle"
        | UiActions.PageControl -> "page_control"
        | UiActions.PageInfo -> "page_info"
        | UiActions.PageMenuIos -> "page_menu_ios"
        | UiActions.PartnerReports -> "partner_reports"
        | UiActions.PatientList -> "patient_list"
        | UiActions.Php -> "php"
        | UiActions.Pinch -> "pinch"
        | UiActions.Pip -> "pip"
        | UiActions.PipExit -> "pip_exit"
        | UiActions.PlaceItem -> "place_item"
        | UiActions.PointScan -> "point_scan"
        | UiActions.PositionBottomLeft -> "position_bottom_left"
        | UiActions.PositionBottomRight -> "position_bottom_right"
        | UiActions.PositionTopRight -> "position_top_right"
        | UiActions.Preliminary -> "preliminary"
        | UiActions.ProgressActivity -> "progress_activity"
        | UiActions.PromptSuggestion -> "prompt_suggestion"
        | UiActions.Publish -> "publish"
        | UiActions.QuestionExchange -> "question_exchange"
        | UiActions.QuickReference -> "quick_reference"
        | UiActions.QuickReferenceAll -> "quick_reference_all"
        | UiActions.RadioButtonChecked -> "radio_button_checked"
        | UiActions.RadioButtonPartial -> "radio_button_partial"
        | UiActions.RadioButtonUnchecked -> "radio_button_unchecked"
        | UiActions.Rebase -> "rebase"
        | UiActions.RebaseEdit -> "rebase_edit"
        | UiActions.Recenter -> "recenter"
        | UiActions.Redo -> "redo"
        | UiActions.Refresh -> "refresh"
        | UiActions.Remove -> "remove"
        | UiActions.RemoveDone -> "remove_done"
        | UiActions.ReopenWindow -> "reopen_window"
        | UiActions.Repartition -> "repartition"
        | UiActions.Reply -> "reply"
        | UiActions.ReplyAll -> "reply_all"
        | UiActions.Resize -> "resize"
        | UiActions.ResponsiveLayout -> "responsive_layout"
        | UiActions.RestartAlt -> "restart_alt"
        | UiActions.RestoreFromTrash -> "restore_from_trash"
        | UiActions.RightClick -> "right_click"
        | UiActions.RightPanelClose -> "right_panel_close"
        | UiActions.RightPanelOpen -> "right_panel_open"
        | UiActions.Ripples -> "ripples"
        | UiActions.RotateAuto -> "rotate_auto"
        | UiActions.RuleSettings -> "rule_settings"
        | UiActions.SavedSearch -> "saved_search"
        | UiActions.Search -> "search"
        | UiActions.SearchCheck -> "search_check"
        | UiActions.SearchCheck2 -> "search_check_2"
        | UiActions.SearchGear -> "search_gear"
        | UiActions.SearchOff -> "search_off"
        | UiActions.SelectAll -> "select_all"
        | UiActions.SelectCheckBox -> "select_check_box"
        | UiActions.SendTimeExtension -> "send_time_extension"
        | UiActions.Settings -> "settings"
        | UiActions.SettingsAccessibility -> "settings_accessibility"
        | UiActions.SettingsApplications -> "settings_applications"
        | UiActions.SettingsBackupRestore -> "settings_backup_restore"
        | UiActions.SettingsHeart -> "settings_heart"
        | UiActions.ShareReviews -> "share_reviews"
        | UiActions.ShareWindows -> "share_windows"
        | UiActions.ShelfAutoHide -> "shelf_auto_hide"
        | UiActions.ShelfPosition -> "shelf_position"
        | UiActions.ShoppingCartCheckout -> "shopping_cart_checkout"
        | UiActions.SideNavigation -> "side_navigation"
        | UiActions.Sliders -> "sliders"
        | UiActions.Sort -> "sort"
        | UiActions.SortByAlpha -> "sort_by_alpha"
        | UiActions.SplitscreenLandscape -> "splitscreen_landscape"
        | UiActions.SplitscreenPortrait -> "splitscreen_portrait"
        | UiActions.Stack -> "stack"
        | UiActions.StackGroup -> "stack_group"
        | UiActions.StackOff -> "stack_off"
        | UiActions.StackStar -> "stack_star"
        | UiActions.Stacks -> "stacks"
        | UiActions.Star -> "star"
        | UiActions.StarHalf -> "star_half"
        | UiActions.StarRate -> "star_rate"
        | UiActions.StarRateHalf -> "star_rate_half"
        | UiActions.Start -> "start"
        | UiActions.Stat0 -> "stat_0"
        | UiActions.Stat1 -> "stat_1"
        | UiActions.Stat2 -> "stat_2"
        | UiActions.Stat3 -> "stat_3"
        | UiActions.StatMinus1 -> "stat_minus_1"
        | UiActions.StatMinus2 -> "stat_minus_2"
        | UiActions.StatMinus3 -> "stat_minus_3"
        | UiActions.Step -> "step"
        | UiActions.StepInto -> "step_into"
        | UiActions.StepOut -> "step_out"
        | UiActions.StepOver -> "step_over"
        | UiActions.Steppers -> "steppers"
        | UiActions.SubdirectoryArrowLeft -> "subdirectory_arrow_left"
        | UiActions.SubdirectoryArrowRight -> "subdirectory_arrow_right"
        | UiActions.Subheader -> "subheader"
        | UiActions.SwapHoriz -> "swap_horiz"
        | UiActions.SwapHorizontalCircle -> "swap_horizontal_circle"
        | UiActions.SwapVert -> "swap_vert"
        | UiActions.SwapVerticalCircle -> "swap_vertical_circle"
        | UiActions.Sweep -> "sweep"
        | UiActions.SwipeDown -> "swipe_down"
        | UiActions.SwipeDownAlt -> "swipe_down_alt"
        | UiActions.SwipeLeft -> "swipe_left"
        | UiActions.SwipeLeftAlt -> "swipe_left_alt"
        | UiActions.SwipeRight -> "swipe_right"
        | UiActions.SwipeRightAlt -> "swipe_right_alt"
        | UiActions.SwipeUp -> "swipe_up"
        | UiActions.SwipeUpAlt -> "swipe_up_alt"
        | UiActions.SwipeVertical -> "swipe_vertical"
        | UiActions.SwitchAccess -> "switch_access"
        | UiActions.SwitchAccess2 -> "switch_access_2"
        | UiActions.SwitchAccess3 -> "switch_access_3"
        | UiActions.SwitchAccessShortcut -> "switch_access_shortcut"
        | UiActions.SwitchAccessShortcutAdd -> "switch_access_shortcut_add"
        | UiActions.SwitchLeft -> "switch_left"
        | UiActions.SwitchRight -> "switch_right"
        | UiActions.Switches -> "switches"
        | UiActions.Sync -> "sync"
        | UiActions.SyncAlt -> "sync_alt"
        | UiActions.SyncArrowDown -> "sync_arrow_down"
        | UiActions.SyncArrowUp -> "sync_arrow_up"
        | UiActions.SyncDesktop -> "sync_desktop"
        | UiActions.SyncDisabled -> "sync_disabled"
        | UiActions.SyncProblem -> "sync_problem"
        | UiActions.SyncSavedLocally -> "sync_saved_locally"
        | UiActions.SyncSavedLocallyOff -> "sync_saved_locally_off"
        | UiActions.SystemUpdateAlt -> "system_update_alt"
        | UiActions.Tab -> "tab"
        | UiActions.TabClose -> "tab_close"
        | UiActions.TabCloseInactive -> "tab_close_inactive"
        | UiActions.TabCloseRight -> "tab_close_right"
        | UiActions.TabDuplicate -> "tab_duplicate"
        | UiActions.TabGroup -> "tab_group"
        | UiActions.TabInactive -> "tab_inactive"
        | UiActions.TabMove -> "tab_move"
        | UiActions.TabNewRight -> "tab_new_right"
        | UiActions.TabRecent -> "tab_recent"
        | UiActions.TabSearch -> "tab_search"
        | UiActions.TabUnselected -> "tab_unselected"
        | UiActions.Tabs -> "tabs"
        | UiActions.Terminal -> "terminal"
        | UiActions.ThermostatArrowDown -> "thermostat_arrow_down"
        | UiActions.ThermostatArrowUp -> "thermostat_arrow_up"
        | UiActions.TileLarge -> "tile_large"
        | UiActions.TileMedium -> "tile_medium"
        | UiActions.TileSmall -> "tile_small"
        | UiActions.TimerArrowDown -> "timer_arrow_down"
        | UiActions.TimerArrowUp -> "timer_arrow_up"
        | UiActions.Toast -> "toast"
        | UiActions.ToggleOff -> "toggle_off"
        | UiActions.ToggleOn -> "toggle_on"
        | UiActions.Token -> "token"
        | UiActions.Toolbar -> "toolbar"
        | UiActions.Undo -> "undo"
        | UiActions.UnfoldLess -> "unfold_less"
        | UiActions.UnfoldLessDouble -> "unfold_less_double"
        | UiActions.UnfoldMore -> "unfold_more"
        | UiActions.UnfoldMoreDouble -> "unfold_more_double"
        | UiActions.Unknown5 -> "unknown_5"
        | UiActions.UnknownMed -> "unknown_med"
        | UiActions.Upload -> "upload"
        | UiActions.Upload2 -> "upload_2"
        | UiActions.ViewApps -> "view_apps"
        | UiActions.ViewComfyAlt -> "view_comfy_alt"
        | UiActions.ViewCompactAlt -> "view_compact_alt"
        | UiActions.ViewCozy -> "view_cozy"
        | UiActions.ViewKanban -> "view_kanban"
        | UiActions.ViewTimeline -> "view_timeline"
        | UiActions.WidgetMedium -> "widget_medium"
        | UiActions.WidgetMenu -> "widget_menu"
        | UiActions.WidgetSmall -> "widget_small"
        | UiActions.WidgetWidth -> "widget_width"
        | UiActions.WidthFull -> "width_full"
        | UiActions.WidthNormal -> "width_normal"
        | UiActions.WidthWide -> "width_wide"
        | UiActions.YoutubeSearchedFor -> "youtube_searched_for"
        | UiActions.ZoomIn -> "zoom_in"
        | UiActions.ZoomOut -> "zoom_out"

    [<RequireQualifiedAccess; Struct>]
    type Icon =
      | Action of action: Action
      | Activities of activities: Activities
      | Android of android: Android
      | AudioVideo of audioVideo: AudioVideo
      | Business of business: Business
      | Communicate of communicate: Communicate
      | Hardware of hardware: Hardware
      | Home of home: Home
      | Household of household: Household
      | Images of images: Images
      | Maps of maps: Maps
      | Privacy of privacy: Privacy
      | Social of social: Social
      | Text of text: Text
      | Transit of transit: Transit
      | Travel of travel: Travel
      | UiActions of uiActions: UiActions

    module Icon =

      let toSnakeCase (icon: Icon) =
        match icon with
        | Icon.Action action -> Action.toSnakeCase action
        | Icon.Activities activities -> Activities.toSnakeCase activities
        | Icon.Android android -> Android.toSnakeCase android
        | Icon.AudioVideo audioVideo -> AudioVideo.toSnakeCase audioVideo
        | Icon.Business business -> Business.toSnakeCase business
        | Icon.Communicate communicate -> Communicate.toSnakeCase communicate
        | Icon.Hardware hardware -> Hardware.toSnakeCase hardware
        | Icon.Home home -> Home.toSnakeCase home
        | Icon.Household household -> Household.toSnakeCase household
        | Icon.Images images -> Images.toSnakeCase images
        | Icon.Maps maps -> Maps.toSnakeCase maps
        | Icon.Privacy privacy -> Privacy.toSnakeCase privacy
        | Icon.Social social -> Social.toSnakeCase social
        | Icon.Text text -> Text.toSnakeCase text
        | Icon.Transit transit -> Transit.toSnakeCase transit
        | Icon.Travel travel -> Travel.toSnakeCase travel
        | Icon.UiActions uiActions -> UiActions.toSnakeCase uiActions

open Icons

[<JavaScript; AutoOpen>]
module private IconHelpers =

  open Icons.MaterialSymbols

  let generationToString generation =
    match generation with
    | Symbols -> "material-symbols"

  let styleToString style =
    match style with
    | Style.OutlinedFilled
    | Style.Outlined -> "outlined"
    | Style.RoundedFilled
    | Style.Rounded -> "rounded"
    | Style.SharpFilled
    | Style.Sharp -> "sharp"

  let fillToString style =
    match style with
    | Style.OutlinedFilled
    | Style.RoundedFilled
    | Style.SharpFilled -> Some "'FILL' 1"
    | Style.Outlined
    | Style.Rounded
    | Style.Sharp -> None

  let weightToString weight =
    match weight with
    | IconWeight.``100`` -> "'wght' 100"
    | IconWeight.``200`` -> "'wght' 200"
    | IconWeight.``300`` -> "'wght' 300"
    | IconWeight.``400`` -> "'wght' 400"
    | IconWeight.``500`` -> "'wght' 500"
    | IconWeight.``600`` -> "'wght' 600"
    | IconWeight.``700`` -> "'wght' 700"

  let gradeToString grade =
    match grade with
    | IconGrade.``-25`` -> "'GRAD' -25"
    | IconGrade.``0`` -> "'GRAD' 0"
    | IconGrade.``200`` -> "'GRAD' 200"

  let opticalSizeToString size =
    match size with
    | OpticalSize.``20`` -> "'opsz' 20"
    | OpticalSize.``24`` -> "'opsz' 24"
    | OpticalSize.``40`` -> "'opsz' 40"
    | OpticalSize.``48`` -> "'opsz' 48"

[<JavaScript>]
type Icon =

  static member Create
    (
      icon: MaterialSymbols.Icon,
      ?style: View<MaterialSymbols.Style>,
      ?weight: View<MaterialSymbols.IconWeight>,
      ?grade: View<MaterialSymbols.IconGrade>,
      ?opticalSize: View<MaterialSymbols.OpticalSize>,
      ?attrs: Attr list
    ) =
    let attrs = defaultArg attrs []

    let parametersFirst =
      (ViewOption.sequence style, ViewOption.sequence weight) ||> View.zipCached

    let parametersSecond =
      (ViewOption.sequence grade, ViewOption.sequence opticalSize) ||> View.zipCached

    let parameters = (parametersFirst, parametersSecond) ||> View.zipCached

    let iconStyle =
      parameters
      |> View.MapCached(fun ((style, weight), (grade, opticalSize)) ->
        [
          match style |> Option.bind fillToString with
          | Some style -> style
          | None -> ()
          match weight with
          | Some weight -> weightToString weight
          | None -> ()
          match grade with
          | Some grade -> gradeToString grade
          | None -> ()
          match opticalSize with
          | Some size -> opticalSizeToString size
          | None -> ()
        ]
        |> String.concat ", ")
      |> Attr.DynamicStyle "font-variation-settings"

    span [
      [
        Some MaterialSymbols.Style.Outlined
        Some MaterialSymbols.Style.OutlinedFilled
        Some MaterialSymbols.Style.Rounded
        Some MaterialSymbols.Style.RoundedFilled
        Some MaterialSymbols.Style.Sharp
        Some MaterialSymbols.Style.SharpFilled
        None
      ]
      |> List.map (fun style ->
        match style with
        | Some s -> style, (generationToString Symbols, styleToString s) ||> sprintf "%s-%s"
        | None -> style, generationToString Symbols)
      |> Map.ofList
      |> Attr.classSelection (ViewOption.sequence style)

      iconStyle
      yield! attrs
    ] [ MaterialSymbols.Icon.toSnakeCase icon |> text ]
