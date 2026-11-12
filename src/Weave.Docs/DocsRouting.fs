namespace Weave.Docs.Examples

open WebSharper
open WebSharper.JavaScript

[<JavaScript>]
module DocsRouting =

  [<Struct>]
  type Page =
    | Home
    | GettingStartedExamples
    | AppBarExamples
    | SpacerExamples
    | ScrollToTopExamples
    | ButtonExamples
    | ButtonGroupExamples
    | ButtonMenuExamples
    | TypographyExamples
    | TooltipExamples
    | GridExamples
    | CheckboxExamples
    | CardExamples
    | CarouselExamples
    | PaginationExamples
    | ChipExamples
    | ChipSetExamples
    | RadioButtonExamples
    | SwitchExamples
    | ProgressExamples
    | SliderExamples
    | ColorPickerExamples
    | ContainerExamples
    | FieldExamples
    | FormExamples
    | NumericFieldExamples
    | DropdownMenuExamples
    | SelectExamples
    | ExpansionPanelExamples
    | DialogExamples
    | DrawerExamples
    | IconsExamples
    | TableExamples
    | TabsExamples
    | TextFieldExamples
    | ListExamples
    | AlertExamples
    | SnackbarExamples
    | LinkExamples
    | HighlighterExamples
    | DividerExamples
    | SpacingExamples
    | OpacityExamples
    | TransitionExamples
    | AnimationExamples
    | BorderExamples
    | DisplayExamples
    | ElevationExamples
    | FlexboxExamples
    | ThemingExamples
    | ShowcaseTaskTracker
    | ShowcasePomodoroTimer
    | ShowcaseExpenseTracker
    | ShowcaseCharacterSheet
    | ReactiveHelpersExamples
    | EventHandlersExamples

  let pageToString page =
    match page with
    | Home -> "Home"
    | GettingStartedExamples -> "Getting Started"
    | AppBarExamples -> "App Bar"
    | SpacerExamples -> "Spacer"
    | ScrollToTopExamples -> "Scroll to Top"
    | ButtonExamples -> "Button"
    | ButtonGroupExamples -> "Button Group"
    | ButtonMenuExamples -> "Button Menu"
    | TypographyExamples -> "Typography"
    | TooltipExamples -> "Tooltip"
    | GridExamples -> "Grid"
    | CheckboxExamples -> "Checkbox"
    | CardExamples -> "Card"
    | CarouselExamples -> "Carousel"
    | PaginationExamples -> "Pagination"
    | ChipExamples -> "Chip"
    | ChipSetExamples -> "Chip Set"
    | RadioButtonExamples -> "Radio Button"
    | SwitchExamples -> "Switch"
    | ProgressExamples -> "Progress"
    | SliderExamples -> "Slider"
    | ColorPickerExamples -> "Color Picker"
    | ContainerExamples -> "Container"
    | FormExamples -> "Forms & Validation"
    | FieldExamples -> "Field"
    | NumericFieldExamples -> "Numeric Field"
    | DropdownMenuExamples -> "Dropdown Menu"
    | SelectExamples -> "Select"
    | ExpansionPanelExamples -> "Expansion Panel"
    | DialogExamples -> "Dialog"
    | DrawerExamples -> "Drawer"
    | IconsExamples -> "Icons"
    | TableExamples -> "Table"
    | TabsExamples -> "Tabs"
    | TextFieldExamples -> "Text Field"
    | ListExamples -> "List"
    | AlertExamples -> "Alert"
    | HighlighterExamples -> "Highlighter"
    | SnackbarExamples -> "Snackbar"
    | LinkExamples -> "Link"
    | DividerExamples -> "Divider"
    | SpacingExamples -> "Spacing"
    | OpacityExamples -> "Opacity"
    | TransitionExamples -> "Transitions"
    | AnimationExamples -> "Animations"
    | BorderExamples -> "Borders"
    | DisplayExamples -> "Display"
    | ElevationExamples -> "Elevation"
    | FlexboxExamples -> "Flexbox"
    | ThemingExamples -> "Theming"
    | ShowcaseTaskTracker -> "Task Tracker"
    | ShowcasePomodoroTimer -> "Pomodoro Timer"
    | ShowcaseExpenseTracker -> "Expense Tracker"
    | ShowcaseCharacterSheet -> "Character Sheet"
    | ReactiveHelpersExamples -> "Reactive Helpers"
    | EventHandlersExamples -> "Event Handlers"

  /// Reader vocabulary supplements public component names in navigation search.
  let pageSearchAliases page =
    match page with
    | GettingStartedExamples -> "install setup quickstart starter template first app"
    | TextFieldExamples -> "input textbox textarea text entry password multiline"
    | NumericFieldExamples -> "input number decimal integer spinner"
    | FieldExamples -> "input label helper validation error adornment"
    | FormExamples -> "input form validation submit required error"
    | DialogExamples -> "modal popup confirmation overlay"
    | DrawerExamples -> "sidebar navigation panel offcanvas overlay"
    | DropdownMenuExamples -> "menu popup actions context"
    | SelectExamples -> "dropdown options picker combobox"
    | SnackbarExamples -> "toast notification message feedback"
    | AlertExamples -> "notification message feedback status warning error"
    | ExpansionPanelExamples -> "accordion disclosure collapse expand"
    | ProgressExamples -> "loading spinner indicator percentage"
    | PaginationExamples -> "pages paging page navigation"
    | CheckboxExamples -> "check checked boolean selection toggle"
    | RadioButtonExamples -> "radio options choice selection"
    | SwitchExamples -> "toggle boolean settings on off"
    | ChipExamples
    | ChipSetExamples -> "tag badge token filter selection"
    | TypographyExamples -> "text fonts headings weight type"
    | ThemingExamples -> "theme dark light colors palette density style"
    | GridExamples
    | FlexboxExamples -> "layout columns rows responsive alignment"
    | IconsExamples -> "icon symbols material"
    | ReactiveHelpersExamples -> "state var view binding reactive map combine"
    | EventHandlersExamples -> "event click keyboard input handler"
    | _ -> ""

  let stringToPage s =
    match s with
    | "Home" -> Some Home
    | "Getting Started" -> Some GettingStartedExamples
    | "App Bar" -> Some AppBarExamples
    | "Spacer" -> Some SpacerExamples
    | "Scroll to Top" -> Some ScrollToTopExamples
    | "Button" -> Some ButtonExamples
    | "Button Group" -> Some ButtonGroupExamples
    | "Button Menu" -> Some ButtonMenuExamples
    | "Typography" -> Some TypographyExamples
    | "Tooltip" -> Some TooltipExamples
    | "Grid" -> Some GridExamples
    | "Checkbox" -> Some CheckboxExamples
    | "Card" -> Some CardExamples
    | "Carousel" -> Some CarouselExamples
    | "Pagination" -> Some PaginationExamples
    | "Chip" -> Some ChipExamples
    | "Chip Set" -> Some ChipSetExamples
    | "Radio Button" -> Some RadioButtonExamples
    | "Switch" -> Some SwitchExamples
    | "Progress" -> Some ProgressExamples
    | "Slider" -> Some SliderExamples
    | "Color Picker" -> Some ColorPickerExamples
    | "Container" -> Some ContainerExamples
    | "Forms & Validation" -> Some FormExamples
    | "Field" -> Some FieldExamples
    | "Numeric Field" -> Some NumericFieldExamples
    | "Dropdown Menu" -> Some DropdownMenuExamples
    | "Select" -> Some SelectExamples
    | "Expansion Panel" -> Some ExpansionPanelExamples
    | "Dialog" -> Some DialogExamples
    | "Drawer" -> Some DrawerExamples
    | "Icons" -> Some IconsExamples
    | "Table" -> Some TableExamples
    | "Tabs" -> Some TabsExamples
    | "Text Field" -> Some TextFieldExamples
    | "List" -> Some ListExamples
    | "Alert" -> Some AlertExamples
    | "Highlighter" -> Some HighlighterExamples
    | "Snackbar" -> Some SnackbarExamples
    | "Link" -> Some LinkExamples
    | "Divider" -> Some DividerExamples
    | "Spacing" -> Some SpacingExamples
    | "Opacity" -> Some OpacityExamples
    | "Transitions" -> Some TransitionExamples
    | "Animations" -> Some AnimationExamples
    | "Borders" -> Some BorderExamples
    | "Display" -> Some DisplayExamples
    | "Elevation" -> Some ElevationExamples
    | "Flexbox" -> Some FlexboxExamples
    | "Theming" -> Some ThemingExamples
    | "Task Tracker" -> Some ShowcaseTaskTracker
    | "Pomodoro Timer" -> Some ShowcasePomodoroTimer
    | "Expense Tracker" -> Some ShowcaseExpenseTracker
    | "Character Sheet" -> Some ShowcaseCharacterSheet
    | "Reactive Helpers" -> Some ReactiveHelpersExamples
    | "Event Handlers" -> Some EventHandlersExamples
    | _ -> None

  let pageToHash page =
    match page with
    | Home -> "#home"
    | GettingStartedExamples -> "#getting-started"
    | AppBarExamples -> "#app-bar"
    | SpacerExamples -> "#spacer"
    | ScrollToTopExamples -> "#scroll-to-top"
    | ButtonExamples -> "#button"
    | ButtonGroupExamples -> "#button-group"
    | ButtonMenuExamples -> "#button-menu"
    | TypographyExamples -> "#typography"
    | TooltipExamples -> "#tooltip"
    | GridExamples -> "#grid"
    | CheckboxExamples -> "#checkbox"
    | CardExamples -> "#card"
    | CarouselExamples -> "#carousel"
    | PaginationExamples -> "#pagination"
    | ChipExamples -> "#chip"
    | ChipSetExamples -> "#chip-set"
    | RadioButtonExamples -> "#radio-button"
    | SwitchExamples -> "#switch"
    | ProgressExamples -> "#progress"
    | SliderExamples -> "#slider"
    | ColorPickerExamples -> "#color-picker"
    | ContainerExamples -> "#container"
    | FormExamples -> "#forms"
    | FieldExamples -> "#field"
    | NumericFieldExamples -> "#numeric-field"
    | DropdownMenuExamples -> "#dropdown-menu"
    | SelectExamples -> "#select"
    | ExpansionPanelExamples -> "#expansion-panel"
    | DialogExamples -> "#dialog"
    | DrawerExamples -> "#drawer"
    | IconsExamples -> "#icons"
    | TableExamples -> "#table"
    | TabsExamples -> "#tabs"
    | TextFieldExamples -> "#text-field"
    | ListExamples -> "#list"
    | AlertExamples -> "#alert"
    | HighlighterExamples -> "#highlighter"
    | SnackbarExamples -> "#snackbar"
    | LinkExamples -> "#link"
    | DividerExamples -> "#divider"
    | SpacingExamples -> "#spacing"
    | OpacityExamples -> "#opacity"
    | TransitionExamples -> "#transitions"
    | AnimationExamples -> "#animations"
    | BorderExamples -> "#borders"
    | DisplayExamples -> "#display"
    | ElevationExamples -> "#elevation"
    | FlexboxExamples -> "#flexbox"
    | ThemingExamples -> "#theming"
    | ShowcaseTaskTracker -> "#showcase-task-tracker"
    | ShowcasePomodoroTimer -> "#showcase-pomodoro-timer"
    | ShowcaseExpenseTracker -> "#showcase-expense-tracker"
    | ShowcaseCharacterSheet -> "#showcase-character-sheet"
    | ReactiveHelpersExamples -> "#reactive-helpers"
    | EventHandlersExamples -> "#event-handlers"

  let hashToPage hash =
    match hash with
    | s when String.length s = 0 -> Some Home
    | "#home" -> Some Home
    | "#getting-started" -> Some GettingStartedExamples
    | "#app-bar" -> Some AppBarExamples
    | "#spacer" -> Some SpacerExamples
    | "#scroll-to-top" -> Some ScrollToTopExamples
    | "#button" -> Some ButtonExamples
    | "#button-group" -> Some ButtonGroupExamples
    | "#button-menu" -> Some ButtonMenuExamples
    | "#typography" -> Some TypographyExamples
    | "#tooltip" -> Some TooltipExamples
    | "#grid" -> Some GridExamples
    | "#checkbox" -> Some CheckboxExamples
    | "#card" -> Some CardExamples
    | "#carousel" -> Some CarouselExamples
    | "#pagination" -> Some PaginationExamples
    | "#chip" -> Some ChipExamples
    | "#chip-set" -> Some ChipSetExamples
    | "#radio-button" -> Some RadioButtonExamples
    | "#switch" -> Some SwitchExamples
    | "#progress" -> Some ProgressExamples
    | "#slider" -> Some SliderExamples
    | "#color-picker" -> Some ColorPickerExamples
    | "#container" -> Some ContainerExamples
    | "#forms" -> Some FormExamples
    | "#field" -> Some FieldExamples
    | "#numeric-field" -> Some NumericFieldExamples
    | "#dropdown-menu" -> Some DropdownMenuExamples
    | "#select" -> Some SelectExamples
    | "#expansion-panel" -> Some ExpansionPanelExamples
    | "#dialog" -> Some DialogExamples
    | "#drawer" -> Some DrawerExamples
    | "#icons" -> Some IconsExamples
    | "#table" -> Some TableExamples
    | "#tabs" -> Some TabsExamples
    | "#text-field" -> Some TextFieldExamples
    | "#list" -> Some ListExamples
    | "#alert" -> Some AlertExamples
    | "#highlighter" -> Some HighlighterExamples
    | "#snackbar" -> Some SnackbarExamples
    | "#link" -> Some LinkExamples
    | "#divider" -> Some DividerExamples
    | "#spacing" -> Some SpacingExamples
    | "#opacity" -> Some OpacityExamples
    | "#transitions" -> Some TransitionExamples
    | "#animations" -> Some AnimationExamples
    | "#borders" -> Some BorderExamples
    | "#display" -> Some DisplayExamples
    | "#elevation" -> Some ElevationExamples
    | "#flexbox" -> Some FlexboxExamples
    | "#theming" -> Some ThemingExamples
    | "#showcase-task-tracker" -> Some ShowcaseTaskTracker
    | "#showcase-pomodoro-timer" -> Some ShowcasePomodoroTimer
    | "#showcase-expense-tracker" -> Some ShowcaseExpenseTracker
    | "#showcase-character-sheet" -> Some ShowcaseCharacterSheet
    | "#reactive-helpers" -> Some ReactiveHelpersExamples
    | "#event-handlers" -> Some EventHandlersExamples
    | _ -> None

  [<Inline "window.location.hash">]
  let getLocationHash () = X<string>

  [<Inline "window.location.hash = $hash">]
  let setLocationHash (hash: string) = X<unit>

  [<Inline "window.addEventListener('hashchange', function() { $callback(window.location.hash); })">]
  let onHashChange (callback: string -> unit) = X<unit>

  /// Update the URL without adding a browser history entry (won't fire hashchange)
  [<Inline "history.replaceState(null, '', $hash)">]
  let replaceStateHash (hash: string) = X<unit>

  /// Navigate to the root URL, removing the hash entirely (creates a history entry)
  [<Inline "history.pushState(null, '', location.pathname + location.search)">]
  let clearHash () = X<unit>

  /// Split a hash string on the first "/" — returns a 1- or 2-element array
  [<Inline "(function(s) { var i = s.indexOf('/'); if (i < 0) return [s]; var section = s.substring(i + 1); try { section = decodeURIComponent(section); } catch (_) {} return [s.substring(0, i), section]; })($s)">]
  let splitHashParts (s: string) = X<string[]>

  /// Remove a leading "#" character
  [<Inline "$s.substring(1)">]
  let stripHash (s: string) = X<string>
