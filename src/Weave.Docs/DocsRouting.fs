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
    | ButtonExamples
    | ButtonGroupExamples
    | ButtonMenuExamples
    | TypographyExamples
    | TooltipExamples
    | GridExamples
    | CheckboxExamples
    | ChipExamples
    | ChipSetExamples
    | RadioButtonExamples
    | SwitchExamples
    | SliderExamples
    | ContainerExamples
    | FieldExamples
    | NumericFieldExamples
    | DropdownExamples
    | SelectExamples
    | ExpansionPanelExamples
    | DialogExamples
    | DrawerExamples
    | IconsExamples
    | TabsExamples
    | ListExamples
    | AlertExamples
    | LinkExamples
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

  let pageToString page =
    match page with
    | Home -> "Home"
    | GettingStartedExamples -> "Getting Started"
    | AppBarExamples -> "App Bar"
    | SpacerExamples -> "Spacer"
    | ButtonExamples -> "Button"
    | ButtonGroupExamples -> "Button Group"
    | ButtonMenuExamples -> "Button Menu"
    | TypographyExamples -> "Typography"
    | TooltipExamples -> "Tooltip"
    | GridExamples -> "Grid"
    | CheckboxExamples -> "Checkbox"
    | ChipExamples -> "Chip"
    | ChipSetExamples -> "Chip Set"
    | RadioButtonExamples -> "Radio Button"
    | SwitchExamples -> "Switch"
    | SliderExamples -> "Slider"
    | ContainerExamples -> "Container"
    | FieldExamples -> "Field"
    | NumericFieldExamples -> "Numeric Field"
    | DropdownExamples -> "Dropdown"
    | SelectExamples -> "Select"
    | ExpansionPanelExamples -> "Expansion Panel"
    | DialogExamples -> "Dialog"
    | DrawerExamples -> "Drawer"
    | IconsExamples -> "Icons"
    | TabsExamples -> "Tabs"
    | ListExamples -> "List"
    | AlertExamples -> "Alert"
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

  let stringToPage s =
    match s with
    | "Home" -> Some Home
    | "Getting Started" -> Some GettingStartedExamples
    | "App Bar" -> Some AppBarExamples
    | "Spacer" -> Some SpacerExamples
    | "Button" -> Some ButtonExamples
    | "Button Group" -> Some ButtonGroupExamples
    | "Button Menu" -> Some ButtonMenuExamples
    | "Typography" -> Some TypographyExamples
    | "Tooltip" -> Some TooltipExamples
    | "Grid" -> Some GridExamples
    | "Checkbox" -> Some CheckboxExamples
    | "Chip" -> Some ChipExamples
    | "Chip Set" -> Some ChipSetExamples
    | "Radio Button" -> Some RadioButtonExamples
    | "Switch" -> Some SwitchExamples
    | "Slider" -> Some SliderExamples
    | "Container" -> Some ContainerExamples
    | "Field" -> Some FieldExamples
    | "Numeric Field" -> Some NumericFieldExamples
    | "Dropdown" -> Some DropdownExamples
    | "Select" -> Some SelectExamples
    | "Expansion Panel" -> Some ExpansionPanelExamples
    | "Dialog" -> Some DialogExamples
    | "Drawer" -> Some DrawerExamples
    | "Icons" -> Some IconsExamples
    | "Tabs" -> Some TabsExamples
    | "List" -> Some ListExamples
    | "Alert" -> Some AlertExamples
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
    | _ -> None

  let pageToHash page =
    match page with
    | Home -> ""
    | GettingStartedExamples -> "#getting-started"
    | AppBarExamples -> "#app-bar"
    | SpacerExamples -> "#spacer"
    | ButtonExamples -> "#button"
    | ButtonGroupExamples -> "#button-group"
    | ButtonMenuExamples -> "#button-menu"
    | TypographyExamples -> "#typography"
    | TooltipExamples -> "#tooltip"
    | GridExamples -> "#grid"
    | CheckboxExamples -> "#checkbox"
    | ChipExamples -> "#chip"
    | ChipSetExamples -> "#chip-set"
    | RadioButtonExamples -> "#radio-button"
    | SwitchExamples -> "#switch"
    | SliderExamples -> "#slider"
    | ContainerExamples -> "#container"
    | FieldExamples -> "#field"
    | NumericFieldExamples -> "#numeric-field"
    | DropdownExamples -> "#dropdown"
    | SelectExamples -> "#select"
    | ExpansionPanelExamples -> "#expansion-panel"
    | DialogExamples -> "#dialog"
    | DrawerExamples -> "#drawer"
    | IconsExamples -> "#icons"
    | TabsExamples -> "#tabs"
    | ListExamples -> "#list"
    | AlertExamples -> "#alert"
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

  let hashToPage hash =
    match hash with
    | s when String.length s = 0 -> Some Home
    | "#home" -> Some Home
    | "#getting-started" -> Some GettingStartedExamples
    | "#app-bar" -> Some AppBarExamples
    | "#spacer" -> Some SpacerExamples
    | "#button" -> Some ButtonExamples
    | "#button-group" -> Some ButtonGroupExamples
    | "#button-menu" -> Some ButtonMenuExamples
    | "#typography" -> Some TypographyExamples
    | "#tooltip" -> Some TooltipExamples
    | "#grid" -> Some GridExamples
    | "#checkbox" -> Some CheckboxExamples
    | "#chip" -> Some ChipExamples
    | "#chip-set" -> Some ChipSetExamples
    | "#radio-button" -> Some RadioButtonExamples
    | "#switch" -> Some SwitchExamples
    | "#slider" -> Some SliderExamples
    | "#container" -> Some ContainerExamples
    | "#field" -> Some FieldExamples
    | "#numeric-field" -> Some NumericFieldExamples
    | "#dropdown" -> Some DropdownExamples
    | "#select" -> Some SelectExamples
    | "#expansion-panel" -> Some ExpansionPanelExamples
    | "#dialog" -> Some DialogExamples
    | "#drawer" -> Some DrawerExamples
    | "#icons" -> Some IconsExamples
    | "#tabs" -> Some TabsExamples
    | "#list" -> Some ListExamples
    | "#alert" -> Some AlertExamples
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
  [<Inline "$s.split('/')">]
  let splitHashParts (s: string) = X<string[]>

  /// Remove a leading "#" character
  [<Inline "$s.substring(1)">]
  let stripHash (s: string) = X<string>
