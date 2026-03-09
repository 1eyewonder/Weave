namespace Weave.Docs.Examples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave.CssHelpers
open Weave
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ExamplesRouter =

  [<Struct>]
  type Page =
    | Home
    | AppBarExamples
    | SpacerExamples
    | ButtonExamples
    | ButtonGroupExamples
    | ButtonMenuExamples
    | TypographyExamples
    | TooltipExamples
    | GridExamples
    | CheckboxExamples
    | RadioButtonExamples
    | SwitchExamples
    | ContainerExamples
    | FieldExamples
    | NumericFieldExamples
    | DropdownExamples
    | ExpansionPanelExamples
    | DialogExamples
    | DrawerExamples
    | IconsExamples
    | TabsExamples
    | ListExamples
    | AlertExamples
    | LinkExamples
    | DividerExamples

  let private pageToString page =
    match page with
    | Home -> "Home"
    | AppBarExamples -> "App Bar"
    | SpacerExamples -> "Spacer"
    | ButtonExamples -> "Button"
    | ButtonGroupExamples -> "Button Group"
    | ButtonMenuExamples -> "Button Menu"
    | TypographyExamples -> "Typography"
    | TooltipExamples -> "Tooltip"
    | GridExamples -> "Grid"
    | CheckboxExamples -> "Checkbox"
    | RadioButtonExamples -> "Radio Button"
    | SwitchExamples -> "Switch"
    | ContainerExamples -> "Container"
    | FieldExamples -> "Field"
    | NumericFieldExamples -> "Numeric Field"
    | DropdownExamples -> "Dropdown"
    | ExpansionPanelExamples -> "Expansion Panel"
    | DialogExamples -> "Dialog"
    | DrawerExamples -> "Drawer"
    | IconsExamples -> "Icons"
    | TabsExamples -> "Tabs"
    | ListExamples -> "List"
    | AlertExamples -> "Alert"
    | LinkExamples -> "Link"
    | DividerExamples -> "Divider"

  let private stringToPage s =
    match s with
    | "Home" -> Some Home
    | "App Bar" -> Some AppBarExamples
    | "Spacer" -> Some SpacerExamples
    | "Button" -> Some ButtonExamples
    | "Button Group" -> Some ButtonGroupExamples
    | "Button Menu" -> Some ButtonMenuExamples
    | "Typography" -> Some TypographyExamples
    | "Tooltip" -> Some TooltipExamples
    | "Grid" -> Some GridExamples
    | "Checkbox" -> Some CheckboxExamples
    | "Radio Button" -> Some RadioButtonExamples
    | "Switch" -> Some SwitchExamples
    | "Container" -> Some ContainerExamples
    | "Field" -> Some FieldExamples
    | "Numeric Field" -> Some NumericFieldExamples
    | "Dropdown" -> Some DropdownExamples
    | "Expansion Panel" -> Some ExpansionPanelExamples
    | "Dialog" -> Some DialogExamples
    | "Drawer" -> Some DrawerExamples
    | "Icons" -> Some IconsExamples
    | "Tabs" -> Some TabsExamples
    | "List" -> Some ListExamples
    | "Alert" -> Some AlertExamples
    | "Link" -> Some LinkExamples
    | "Divider" -> Some DividerExamples
    | _ -> None

  let private pageToHash page =
    match page with
    | Home -> ""
    | AppBarExamples -> "#app-bar"
    | SpacerExamples -> "#spacer"
    | ButtonExamples -> "#button"
    | ButtonGroupExamples -> "#button-group"
    | ButtonMenuExamples -> "#button-menu"
    | TypographyExamples -> "#typography"
    | TooltipExamples -> "#tooltip"
    | GridExamples -> "#grid"
    | CheckboxExamples -> "#checkbox"
    | RadioButtonExamples -> "#radio-button"
    | SwitchExamples -> "#switch"
    | ContainerExamples -> "#container"
    | FieldExamples -> "#field"
    | NumericFieldExamples -> "#numeric-field"
    | DropdownExamples -> "#dropdown"
    | ExpansionPanelExamples -> "#expansion-panel"
    | DialogExamples -> "#dialog"
    | DrawerExamples -> "#drawer"
    | IconsExamples -> "#icons"
    | TabsExamples -> "#tabs"
    | ListExamples -> "#list"
    | AlertExamples -> "#alert"
    | LinkExamples -> "#link"
    | DividerExamples -> "#divider"

  let private hashToPage hash =
    match hash with
    | s when String.length s = 0 -> Some Home
    | "#home" -> Some Home
    | "#app-bar" -> Some AppBarExamples
    | "#spacer" -> Some SpacerExamples
    | "#button" -> Some ButtonExamples
    | "#button-group" -> Some ButtonGroupExamples
    | "#button-menu" -> Some ButtonMenuExamples
    | "#typography" -> Some TypographyExamples
    | "#tooltip" -> Some TooltipExamples
    | "#grid" -> Some GridExamples
    | "#checkbox" -> Some CheckboxExamples
    | "#radio-button" -> Some RadioButtonExamples
    | "#switch" -> Some SwitchExamples
    | "#container" -> Some ContainerExamples
    | "#field" -> Some FieldExamples
    | "#numeric-field" -> Some NumericFieldExamples
    | "#dropdown" -> Some DropdownExamples
    | "#expansion-panel" -> Some ExpansionPanelExamples
    | "#dialog" -> Some DialogExamples
    | "#drawer" -> Some DrawerExamples
    | "#icons" -> Some IconsExamples
    | "#tabs" -> Some TabsExamples
    | "#list" -> Some ListExamples
    | "#alert" -> Some AlertExamples
    | "#link" -> Some LinkExamples
    | "#divider" -> Some DividerExamples
    | _ -> None

  [<Inline "window.location.hash">]
  let private getLocationHash () = X<string>

  [<Inline "window.location.hash = $hash">]
  let private setLocationHash (hash: string) = X<unit>

  [<Inline "window.addEventListener('hashchange', function() { $callback(window.location.hash); })">]
  let private onHashChange (callback: string -> unit) = X<unit>

  /// Update the URL without adding a browser history entry (won't fire hashchange)
  [<Inline "history.replaceState(null, '', $hash)">]
  let private replaceStateHash (hash: string) = X<unit>

  /// Navigate to the root URL, removing the hash entirely (creates a history entry)
  [<Inline "history.pushState(null, '', location.pathname + location.search)">]
  let private clearHash () = X<unit>

  /// Split a hash string on the first "/" — returns a 1- or 2-element array
  [<Inline "$s.split('/')">]
  let private splitHashParts (s: string) = X<string[]>

  /// Remove a leading "#" character
  [<Inline "$s.substring(1)">]
  let private stripHash (s: string) = X<string>

  let private previewFor page : Doc =
    let cp children = div [ cl "cp" ] children

    match page with
    | AppBarExamples ->
      cp [
        div [
          cl "cp-bar"
          Attr.Style "top" "0"
          Attr.Style "left" "0"
          Attr.Style "right" "0"
          Attr.Style "height" "15px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "30px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "6px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "42px"
          Attr.Style "left" "10%"
          Attr.Style "right" "25%"
          Attr.Style "height" "5px"
        ] []
      ]
    | ContainerExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "10px"
          Attr.Style "left" "16%"
          Attr.Style "right" "16%"
          Attr.Style "bottom" "10px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "26px"
          Attr.Style "left" "22%"
          Attr.Style "right" "22%"
          Attr.Style "height" "6px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "40px"
          Attr.Style "left" "22%"
          Attr.Style "right" "34%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "53px"
          Attr.Style "left" "22%"
          Attr.Style "right" "28%"
          Attr.Style "height" "5px"
        ] []
      ]
    | DividerExamples ->
      cp [
        div [
          cl "cp-line"
          Attr.Style "top" "18px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "6px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "28px"
          Attr.Style "left" "8%"
          Attr.Style "right" "22%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "40px"
          Attr.Style "left" "0"
          Attr.Style "right" "0"
          Attr.Style "height" "2px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "49px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "6px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "59px"
          Attr.Style "left" "8%"
          Attr.Style "right" "22%"
          Attr.Style "height" "5px"
        ] []
      ]
    | DrawerExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "0"
          Attr.Style "left" "0"
          Attr.Style "width" "30%"
          Attr.Style "bottom" "0"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "16px"
          Attr.Style "left" "4%"
          Attr.Style "width" "20%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "28px"
          Attr.Style "left" "4%"
          Attr.Style "width" "20%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "40px"
          Attr.Style "left" "4%"
          Attr.Style "width" "16%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "52px"
          Attr.Style "left" "4%"
          Attr.Style "width" "18%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "26px"
          Attr.Style "left" "36%"
          Attr.Style "right" "8%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "40px"
          Attr.Style "left" "36%"
          Attr.Style "right" "20%"
          Attr.Style "height" "5px"
        ] []
      ]
    | GridExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "10px"
          Attr.Style "left" "8%"
          Attr.Style "width" "38%"
          Attr.Style "height" "28px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "10px"
          Attr.Style "left" "54%"
          Attr.Style "width" "38%"
          Attr.Style "height" "28px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "48px"
          Attr.Style "left" "8%"
          Attr.Style "width" "38%"
          Attr.Style "height" "26px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "48px"
          Attr.Style "left" "54%"
          Attr.Style "width" "38%"
          Attr.Style "height" "26px"
        ] []
      ]
    | SpacerExamples ->
      cp [
        div [
          cl "cp-pill"
          Attr.Style "top" "30px"
          Attr.Style "left" "6%"
          Attr.Style "width" "26%"
          Attr.Style "height" "24px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "30px"
          Attr.Style "right" "6%"
          Attr.Style "width" "26%"
          Attr.Style "height" "24px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "40px"
          Attr.Style "left" "34%"
          Attr.Style "right" "34%"
          Attr.Style "height" "2px"
          Attr.Style "background" "var(--palette-text-primary)"
          Attr.Style "opacity" "0.25"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "34px"
          Attr.Style "left" "33.5%"
          Attr.Style "width" "2px"
          Attr.Style "height" "14px"
          Attr.Style "background" "var(--palette-text-primary)"
          Attr.Style "opacity" "0.25"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "34px"
          Attr.Style "right" "33.5%"
          Attr.Style "width" "2px"
          Attr.Style "height" "14px"
          Attr.Style "background" "var(--palette-text-primary)"
          Attr.Style "opacity" "0.25"
        ] []
      ]
    | LinkExamples ->
      cp [
        span [
          Attr.Style "font-family" "'Material Symbols Outlined'"
          Attr.Style "font-size" "52px"
          Attr.Style "color" "var(--palette-primary)"
          Attr.Style "top" "50%"
          Attr.Style "left" "50%"
          Attr.Style "transform" "translate(-50%,-50%)"
          Attr.Style "line-height" "1"
          Attr.Style "user-select" "none"
        ] [ text "link" ]
      ]
    | TabsExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "0"
          Attr.Style "left" "0"
          Attr.Style "right" "0"
          Attr.Style "height" "24px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "5px"
          Attr.Style "left" "6%"
          Attr.Style "width" "26%"
          Attr.Style "height" "14px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "8px"
          Attr.Style "left" "36%"
          Attr.Style "width" "20%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "8px"
          Attr.Style "left" "60%"
          Attr.Style "width" "20%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "24px"
          Attr.Style "left" "6%"
          Attr.Style "width" "26%"
          Attr.Style "height" "3px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "40px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "54px"
          Attr.Style "left" "8%"
          Attr.Style "right" "22%"
          Attr.Style "height" "5px"
        ] []
      ]
    | ButtonExamples ->
      cp [
        div [
          cl "cp-pill"
          Attr.Style "top" "28px"
          Attr.Style "left" "22%"
          Attr.Style "right" "22%"
          Attr.Style "height" "28px"
        ] []
      ]
    | ButtonGroupExamples ->
      cp [
        div [
          cl "cp-pill"
          Attr.Style "top" "30px"
          Attr.Style "left" "5%"
          Attr.Style "width" "27%"
          Attr.Style "height" "24px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "30px"
          Attr.Style "left" "37%"
          Attr.Style "width" "27%"
          Attr.Style "height" "24px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "30px"
          Attr.Style "right" "5%"
          Attr.Style "width" "27%"
          Attr.Style "height" "24px"
        ] []
      ]
    | ButtonMenuExamples ->
      cp [
        div [
          cl "cp-pill"
          Attr.Style "top" "30px"
          Attr.Style "left" "10%"
          Attr.Style "width" "52%"
          Attr.Style "height" "26px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "30px"
          Attr.Style "right" "10%"
          Attr.Style "width" "24%"
          Attr.Style "height" "26px"
          Attr.Style "border-radius" "10px"
        ] []
      ]
    | CheckboxExamples ->
      cp [
        div [
          cl "cp-bar"
          Attr.Style "top" "12px"
          Attr.Style "left" "10%"
          Attr.Style "width" "14px"
          Attr.Style "height" "14px"
          Attr.Style "border-radius" "2px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "14px"
          Attr.Style "left" "28%"
          Attr.Style "right" "15%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "37px"
          Attr.Style "left" "10%"
          Attr.Style "width" "14px"
          Attr.Style "height" "14px"
          Attr.Style "border-radius" "2px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "39px"
          Attr.Style "left" "28%"
          Attr.Style "right" "28%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-box"
          Attr.Style "top" "62px"
          Attr.Style "left" "10%"
          Attr.Style "width" "14px"
          Attr.Style "height" "14px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "64px"
          Attr.Style "left" "28%"
          Attr.Style "right" "38%"
          Attr.Style "height" "7px"
        ] []
      ]
    | DropdownExamples ->
      cp [
        div [
          cl "cp-box"
          Attr.Style "top" "4px"
          Attr.Style "left" "12%"
          Attr.Style "right" "12%"
          Attr.Style "height" "22px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "11px"
          Attr.Style "left" "18%"
          Attr.Style "right" "35%"
          Attr.Style "height" "6px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "12px"
          Attr.Style "right" "18%"
          Attr.Style "width" "7px"
          Attr.Style "height" "5px"
          Attr.Style "border-radius" "1px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "28px"
          Attr.Style "left" "12%"
          Attr.Style "right" "12%"
          Attr.Style "height" "54px"
          Attr.Style "border-radius" "4px"
          Attr.Style "box-shadow" "0 4px 12px rgba(0,0,0,0.4)"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "34px"
          Attr.Style "left" "18%"
          Attr.Style "right" "35%"
          Attr.Style "height" "7px"
          Attr.Style "border-radius" "2px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "50px"
          Attr.Style "left" "18%"
          Attr.Style "right" "45%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "63px"
          Attr.Style "left" "18%"
          Attr.Style "right" "38%"
          Attr.Style "height" "5px"
        ] []
      ]
    | FieldExamples ->
      cp [
        div [
          cl "cp-line"
          Attr.Style "top" "20px"
          Attr.Style "left" "12%"
          Attr.Style "right" "50%"
          Attr.Style "height" "5px"
          Attr.Style "opacity" "0.3"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "35px"
          Attr.Style "left" "12%"
          Attr.Style "right" "12%"
          Attr.Style "height" "7px"
          Attr.Style "opacity" "0.35"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "46px"
          Attr.Style "left" "12%"
          Attr.Style "right" "12%"
          Attr.Style "height" "2px"
        ] []
      ]
    | NumericFieldExamples ->
      cp [
        div [
          cl "cp-box"
          Attr.Style "top" "19px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "50px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "26px"
          Attr.Style "left" "16%"
          Attr.Style "right" "28%"
          Attr.Style "height" "5px"
          Attr.Style "opacity" "0.3"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "39px"
          Attr.Style "left" "16%"
          Attr.Style "right" "44%"
          Attr.Style "height" "7px"
          Attr.Style "opacity" "0.35"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "19px"
          Attr.Style "right" "10%"
          Attr.Style "width" "1px"
          Attr.Style "height" "50px"
          Attr.Style "opacity" "0.3"
        ] []
        span [
          Attr.Style "font-family" "'Material Symbols Outlined'"
          Attr.Style "font-size" "18px"
          Attr.Style "color" "var(--palette-primary)"
          Attr.Style "top" "22px"
          Attr.Style "right" "11%"
          Attr.Style "line-height" "1"
          Attr.Style "user-select" "none"
        ] [ text "expand_less" ]
        span [
          Attr.Style "font-family" "'Material Symbols Outlined'"
          Attr.Style "font-size" "18px"
          Attr.Style "color" "var(--palette-primary)"
          Attr.Style "top" "45px"
          Attr.Style "right" "11%"
          Attr.Style "line-height" "1"
          Attr.Style "user-select" "none"
        ] [ text "expand_more" ]
      ]
    | RadioButtonExamples ->
      cp [
        div [
          cl "cp-ring"
          Attr.Style "top" "10px"
          Attr.Style "left" "10%"
          Attr.Style "width" "12px"
          Attr.Style "height" "12px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "12px"
          Attr.Style "left" "28%"
          Attr.Style "right" "30%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-dot"
          Attr.Style "top" "32px"
          Attr.Style "left" "10%"
          Attr.Style "width" "12px"
          Attr.Style "height" "12px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "34px"
          Attr.Style "left" "28%"
          Attr.Style "right" "42%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-ring"
          Attr.Style "top" "54px"
          Attr.Style "left" "10%"
          Attr.Style "width" "12px"
          Attr.Style "height" "12px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "56px"
          Attr.Style "left" "28%"
          Attr.Style "right" "36%"
          Attr.Style "height" "7px"
        ] []
      ]
    | SwitchExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "18px"
          Attr.Style "left" "calc(50% - 22px)"
          Attr.Style "width" "44px"
          Attr.Style "height" "22px"
          Attr.Style "border-radius" "11px"
        ] []
        div [
          cl "cp-dot"
          Attr.Style "top" "22px"
          Attr.Style "left" "calc(50% - 19px)"
          Attr.Style "width" "14px"
          Attr.Style "height" "14px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "52px"
          Attr.Style "left" "calc(50% - 22px)"
          Attr.Style "width" "44px"
          Attr.Style "height" "22px"
        ] []
        div [
          cl "cp-dot"
          Attr.Style "top" "56px"
          Attr.Style "left" "calc(50% + 5px)"
          Attr.Style "width" "14px"
          Attr.Style "height" "14px"
          Attr.Style "background" "rgba(255,255,255,0.9)"
          Attr.Style "border-radius" "50%"
        ] []
      ]
    | IconsExamples ->
      cp [
        span [
          Attr.Style "font-family" "'Material Symbols Outlined'"
          Attr.Style "font-size" "44px"
          Attr.Style "color" "var(--palette-primary)"
          Attr.Style "top" "calc(50% - 22px)"
          Attr.Style "left" "calc(25% - 22px)"
          Attr.Style "line-height" "1"
          Attr.Style "user-select" "none"
        ] [ text "star" ]
        span [
          Attr.Style "font-family" "'Material Symbols Outlined'"
          Attr.Style "font-size" "44px"
          Attr.Style "color" "var(--palette-primary)"
          Attr.Style "top" "calc(50% - 22px)"
          Attr.Style "left" "calc(75% - 22px)"
          Attr.Style "line-height" "1"
          Attr.Style "user-select" "none"
        ] [ text "settings" ]
      ]
    | ListExamples ->
      cp [
        div [
          cl "cp-dot"
          Attr.Style "top" "12px"
          Attr.Style "left" "6%"
          Attr.Style "width" "8px"
          Attr.Style "height" "8px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "10px"
          Attr.Style "left" "20%"
          Attr.Style "right" "8%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "21px"
          Attr.Style "left" "20%"
          Attr.Style "right" "25%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-dot"
          Attr.Style "top" "38px"
          Attr.Style "left" "6%"
          Attr.Style "width" "8px"
          Attr.Style "height" "8px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "36px"
          Attr.Style "left" "20%"
          Attr.Style "right" "8%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "47px"
          Attr.Style "left" "20%"
          Attr.Style "right" "30%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-dot"
          Attr.Style "top" "64px"
          Attr.Style "left" "6%"
          Attr.Style "width" "8px"
          Attr.Style "height" "8px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "62px"
          Attr.Style "left" "20%"
          Attr.Style "right" "14%"
          Attr.Style "height" "7px"
        ] []
      ]
    | TooltipExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "30px"
          Attr.Style "left" "calc(50% - 7px)"
          Attr.Style "width" "14px"
          Attr.Style "height" "14px"
          Attr.Style "transform" "rotate(45deg)"
          Attr.Style "border-radius" "2px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "8px"
          Attr.Style "left" "18%"
          Attr.Style "right" "18%"
          Attr.Style "height" "28px"
          Attr.Style "border-radius" "4px"
          Attr.Style "box-shadow" "0 2px 8px rgba(0,0,0,0.3)"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "17px"
          Attr.Style "left" "26%"
          Attr.Style "right" "26%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-ring"
          Attr.Style "top" "50px"
          Attr.Style "left" "calc(50% - 14px)"
          Attr.Style "width" "28px"
          Attr.Style "height" "28px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "57px"
          Attr.Style "left" "calc(50% - 1px)"
          Attr.Style "width" "2px"
          Attr.Style "height" "14px"
          Attr.Style "border-radius" "1px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "63px"
          Attr.Style "left" "calc(50% - 7px)"
          Attr.Style "width" "14px"
          Attr.Style "height" "2px"
          Attr.Style "border-radius" "1px"
        ] []
      ]
    | TypographyExamples ->
      cp [
        div [
          cl "cp-line"
          Attr.Style "top" "8px"
          Attr.Style "left" "8%"
          Attr.Style "right" "15%"
          Attr.Style "height" "11px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "26px"
          Attr.Style "left" "8%"
          Attr.Style "right" "30%"
          Attr.Style "height" "8px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "42px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "6px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "54px"
          Attr.Style "left" "8%"
          Attr.Style "right" "40%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "65px"
          Attr.Style "left" "8%"
          Attr.Style "right" "55%"
          Attr.Style "height" "3px"
        ] []
      ]
    | DialogExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "8px"
          Attr.Style "left" "12%"
          Attr.Style "right" "12%"
          Attr.Style "bottom" "8px"
          Attr.Style "box-shadow" "0 4px 20px rgba(0,0,0,0.5)"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "14px"
          Attr.Style "left" "18%"
          Attr.Style "right" "38%"
          Attr.Style "height" "7px"
          Attr.Style "border-radius" "2px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "28px"
          Attr.Style "left" "18%"
          Attr.Style "right" "18%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "38px"
          Attr.Style "left" "18%"
          Attr.Style "right" "28%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "57px"
          Attr.Style "right" "18%"
          Attr.Style "width" "22%"
          Attr.Style "height" "16px"
        ] []
        div [
          cl "cp-box"
          Attr.Style "top" "57px"
          Attr.Style "right" "44%"
          Attr.Style "width" "22%"
          Attr.Style "height" "16px"
        ] []
      ]
    | ExpansionPanelExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "6px"
          Attr.Style "left" "4%"
          Attr.Style "right" "4%"
          Attr.Style "height" "22px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "13px"
          Attr.Style "left" "10%"
          Attr.Style "right" "32%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "14px"
          Attr.Style "right" "10%"
          Attr.Style "width" "10px"
          Attr.Style "height" "6px"
          Attr.Style "border-radius" "2px"
        ] []
        div [
          cl "cp-fdim"
          Attr.Style "top" "32px"
          Attr.Style "left" "4%"
          Attr.Style "right" "4%"
          Attr.Style "height" "22px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "38px"
          Attr.Style "left" "10%"
          Attr.Style "right" "25%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "58px"
          Attr.Style "left" "4%"
          Attr.Style "right" "4%"
          Attr.Style "height" "22px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "65px"
          Attr.Style "left" "10%"
          Attr.Style "right" "32%"
          Attr.Style "height" "7px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "65px"
          Attr.Style "right" "10%"
          Attr.Style "width" "10px"
          Attr.Style "height" "6px"
          Attr.Style "border-radius" "2px"
        ] []
      ]
    | AlertExamples ->
      cp [
        div [
          Attr.Style "background" "rgba(var(--palette-warning-rgb), 0.15)"
          Attr.Style "border-radius" "6px"
          Attr.Style "top" "26px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "36px"
          Attr.Style "position" "absolute"
          Attr.Style "box-sizing" "border-box"
        ] []
        span [
          Attr.Style "font-family" "'Material Symbols Outlined'"
          Attr.Style "font-size" "18px"
          Attr.Style "line-height" "1"
          Attr.Style "color" "var(--palette-warning)"
          Attr.Style "top" "35px"
          Attr.Style "left" "14%"
          Attr.Style "user-select" "none"
        ] [ text "warning" ]
        div [
          cl "cp-line"
          Attr.Style "top" "41px"
          Attr.Style "left" "32%"
          Attr.Style "right" "14%"
          Attr.Style "height" "7px"
        ] []
      ]
    | _ -> Doc.Empty

  let private renderPage (navigate: Page -> unit) page =
    match page with
    | Home ->
      Container.Create(
        div [] [
          div [
            cls [
              Flex.Flex.allSizes
              FlexDirection.Column.allSizes
              AlignItems.toClass AlignItems.Center
            ]
            Margin.toClasses Margin.Bottom.large |> cls
            Margin.toClasses Margin.Top.medium |> cls
          ] [
            img [
              attr.src "assets/weave-logo.png"
              attr.alt "Weave Logo"
              Attr.Style "height" "120px"
              Attr.Style "object-fit" "contain"
              Margin.toClasses Margin.Bottom.small |> cls
            ] []
            H2.Div("Weave", attrs = [ Margin.toClasses Margin.Bottom.extraSmall |> cls ])
            Body1.Div(
              "Threading Logic. Fabricating UI.",
              attrs = [
                Attr.Style "font-style" "italic"
                Attr.Style "opacity" "0.7"
                Margin.toClasses Margin.Bottom.medium |> cls
              ]
            )
          ]

          let categorySection (title: string) (items: (string * Page) list) =
            div [ Margin.toClasses Margin.Bottom.medium |> cls ] [
              H5.Div(title, attrs = [ Margin.toClasses Margin.Bottom.small |> cls ])
              Grid.Create(
                items
                |> List.map (fun (label, page) ->
                  GridItem.Create(
                    div [
                      cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ]
                      SurfaceColor.toBackgroundColor SurfaceColor.Surface
                      BorderRadius.toClass BorderRadius.All.medium |> cl
                      Attr.Style "cursor" "pointer"
                      Attr.Style "height" "100%"
                      Attr.Style "overflow" "hidden"
                      Attr.Style "box-sizing" "border-box"
                      Attr.Class "docs-component-card"
                      on.click (fun _ _ -> navigate page)
                    ] [
                      previewFor page
                      div [
                        cls [
                          Flex.Flex.allSizes
                          JustifyContent.toClass JustifyContent.Center
                          AlignItems.toClass AlignItems.Center
                          yield! Padding.toClasses Padding.Horizontal.extraSmall
                          yield! Padding.toClasses Padding.Vertical.extraSmall
                        ]
                        Attr.Style "flex-grow" "1"
                      ] [
                        Body2.Div(label, attrs = [ Typography.Align.toClass Typography.Align.Center |> cl ])
                      ]
                    ],
                    xs = Grid.Width.create 6,
                    sm = Grid.Width.create 4,
                    md = Grid.Width.create 3,
                    attrs = [ Attr.Style "display" "flex"; Attr.Style "flex-direction" "column" ]
                  )),
                attrs = [ Attr.Style "align-items" "stretch" ]
              )
            ]

          categorySection "Layout" [
            "App Bar", AppBarExamples
            "Container", ContainerExamples
            "Divider", DividerExamples
            "Drawer", DrawerExamples
            "Grid", GridExamples
            "Spacer", SpacerExamples
          ]

          categorySection "Navigation" [ "Link", LinkExamples; "Tabs", TabsExamples ]

          categorySection "Inputs" [
            "Button", ButtonExamples
            "Button Group", ButtonGroupExamples
            "Button Menu", ButtonMenuExamples
            "Checkbox", CheckboxExamples
            "Dropdown", DropdownExamples
            "Field", FieldExamples
            "Numeric Field", NumericFieldExamples
            "Radio Button", RadioButtonExamples
            "Switch", SwitchExamples
          ]

          categorySection "Data Display" [
            "Icons", IconsExamples
            "List", ListExamples
            "Tooltip", TooltipExamples
            "Typography", TypographyExamples
          ]

          categorySection "Feedback" [
            "Alert", AlertExamples
            "Dialog", DialogExamples
            "Expansion Panel", ExpansionPanelExamples
          ]
        ],
        maxWidth = Container.MaxWidth.Large
      )
    | AppBarExamples -> AppBarExamples.render ()
    | SpacerExamples -> SpacerExamples.render ()
    | ButtonExamples -> ButtonExamples.render ()
    | ButtonGroupExamples -> ButtonGroupExamples.render ()
    | ButtonMenuExamples -> ButtonMenuExamples.render ()
    | TypographyExamples -> TypographyExamples.render ()
    | TooltipExamples -> TooltipExamples.render ()
    | GridExamples -> GridExamples.render ()
    | CheckboxExamples -> CheckboxExamples.render ()
    | RadioButtonExamples -> RadioButtonExamples.render ()
    | SwitchExamples -> SwitchExamples.render ()
    | ContainerExamples -> ContainerExamples.render ()
    | FieldExamples -> FieldExamples.render ()
    | NumericFieldExamples -> NumericFieldExamples.render ()
    | DropdownExamples -> DropdownExamples.render ()
    | ExpansionPanelExamples -> ExpansionPanelExamples.render ()
    | DialogExamples -> DialogExamples.render ()
    | DrawerExamples -> DrawerExamples.render ()
    | IconsExamples -> IconsExamples.render ()
    | TabsExamples -> TabsExamples.render ()
    | ListExamples -> ListExamples.render ()
    | AlertExamples -> AlertExamples.render ()
    | LinkExamples -> LinkExamples.render ()
    | DividerExamples -> DividerExamples.render ()

  let private githubSvg =
    Doc.Verbatim
      """<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor"><path d="M12 0c-6.626 0-12 5.373-12 12 0 5.302 3.438 9.8 8.207 11.387.599.111.793-.261.793-.577v-2.234c-3.338.726-4.033-1.416-4.033-1.416-.546-1.387-1.333-1.756-1.333-1.756-1.089-.745.083-.729.083-.729 1.205.084 1.839 1.237 1.839 1.237 1.07 1.834 2.807 1.304 3.492.997.107-.775.418-1.305.762-1.604-2.665-.305-5.467-1.334-5.467-5.931 0-1.311.469-2.381 1.236-3.221-.124-.303-.535-1.524.117-3.176 0 0 1.008-.322 3.301 1.23.957-.266 1.983-.399 3.003-.404 1.02.005 2.047.138 3.006.404 2.291-1.552 3.297-1.23 3.297-1.23.653 1.653.242 2.874.118 3.176.77.84 1.235 1.911 1.235 3.221 0 4.609-2.807 5.624-5.479 5.921.43.372.823 1.102.823 2.222v3.293c0 .319.192.694.801.576 4.765-1.589 8.199-6.086 8.199-11.386 0-6.627-5.373-12-12-12z"/></svg>"""

  let render () =
    let initialHash = getLocationHash ()
    let initialParts = splitHashParts initialHash
    let initialPageHash = initialParts.[0]

    let initialSection =
      if initialParts.Length > 1 then
        Some initialParts.[1]
      else
        None

    let initialPage = initialPageHash |> hashToPage |> Option.defaultValue Home

    // Stored reference to the .weave-main-content element, set via on.afterRender
    let mainEl = Var.Create<Dom.Element option> None

    let scrollMainToTop () =
      mainEl.Value |> Option.iter (fun el -> el?scrollTop <- 0)

    let scrollToSectionAfterDelay (id: string) (ms: int) =
      JS.Window?setTimeout(
        (fun () ->
          let target = JS.Document.GetElementById id

          if not (isNull target) then
            mainEl.Value
            |> Option.iter (fun m ->
              m?scrollTop <-
                As<float>(m?scrollTop) + target.GetBoundingClientRect().Top
                - m.GetBoundingClientRect().Top
                - 16.0)),
        ms
      )
      |> ignore

    // Retries every 50ms (up to ~2s) until the target element exists in the DOM,
    // then scrolls to it. Needed when the page content renders asynchronously.
    let scrollToSectionWhenReady (id: string) =
      let maxAttempts = 40
      let interval = 50
      let mutable attempt = 0

      let rec tryScroll () =
        attempt <- attempt + 1
        let target = JS.Document.GetElementById id

        if not (isNull target) then
          mainEl.Value
          |> Option.iter (fun m ->
            m?scrollTop <-
              As<float>(m?scrollTop) + target.GetBoundingClientRect().Top
              - m.GetBoundingClientRect().Top
              - 16.0)
        elif attempt < maxAttempts then
          JS.Window?setTimeout(tryScroll, interval) |> ignore

      tryScroll ()

    let selectedNav = Var.Create<string option>(Some(pageToString initialPage))
    let drawerOpen = Var.Create false

    let navigateTo (page: Page) =
      setLocationHash (pageToHash page)
      Var.Set selectedNav (Some(pageToString page))

    let currentPageView =
      selectedNav.View
      |> View.Map(fun sel -> sel |> Option.bind stringToPage |> Option.defaultValue Home)

    let navEffects =
      selectedNav.View
      |> Doc.sink (fun _ ->
        scrollMainToTop ()

        if BrowserUtils.windowWidth.Value < 960 then
          Var.Set drawerOpen false)

    onHashChange (fun hash ->
      let parts = splitHashParts hash
      let pagePart = parts.[0]
      let sectionPart = if parts.Length > 1 then Some parts.[1] else None

      match hashToPage pagePart with
      | Some page ->
        let currentName = selectedNav.Value |> Option.defaultValue ""
        let isPageChange = pageToString page <> currentName

        if isPageChange then
          Var.Set selectedNav (Some(pageToString page))

        // If the page changed, the new content renders async — poll until element exists.
        // If we're already on the page, the element is present so a short delay is fine.
        sectionPart
        |> Option.iter (fun id ->
          if isPageChange then
            scrollToSectionWhenReady id
          else
            scrollToSectionAfterDelay id 50)
      | None ->
        // Bare section slug from an in-page anchor link (e.g. href="#variants")
        // Rewrite the history entry to the combined "#page/section" format
        let sectionSlug = stripHash hash

        let pageHash =
          selectedNav.Value
          |> Option.bind stringToPage
          |> Option.map pageToHash
          |> Option.defaultValue "#home"

        replaceStateHash (pageHash + "/" + sectionSlug)
        scrollToSectionAfterDelay sectionSlug 0)

    let layoutExpanded = Var.Create true
    let navSectionExpanded = Var.Create true
    let inputsExpanded = Var.Create true
    let dataExpanded = Var.Create true
    let feedbackExpanded = Var.Create true

    let navLeafItem (label: string) =
      div [
        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          yield! Padding.toClasses Padding.Vertical.extraSmall
          yield! Padding.toClasses Padding.Horizontal.medium
        ]
        Attr.Style "cursor" "pointer"
        Attr.Style "border-radius" "6px"
        Attr.Style "margin" "1px 8px"
        Attr.Class "weave-nav-leaf"
        Attr.DynamicClassPred "weave-nav-item--active" (selectedNav.View |> View.Map(fun s -> s = Some label))
        on.click (fun _ _ -> stringToPage label |> Option.iter navigateTo)
      ] [ Body2.Div(label) ]

    let navGroup categoryIcon (label: string) (isExpanded: Var<bool>) items =
      div [] [
        div [
          cls [
            Flex.Flex.allSizes
            AlignItems.toClass AlignItems.Center
            yield! Padding.toClasses Padding.Vertical.extraSmall
            yield! Padding.toClasses Padding.Horizontal.small
          ]
          Attr.Style "cursor" "pointer"
          Attr.Style "gap" "8px"
          Attr.Class "weave-nav-group-header"
          on.click (fun _ _ -> Var.Update isExpanded not)
        ] [
          Icon.Create(categoryIcon, attrs = [ Attr.Style "font-size" "18px" ])
          Overline.Div(label, attrs = [ Attr.Style "flex" "1"; Attr.Style "opacity" "0.7" ])
          isExpanded.View
          |> Doc.BindView(fun exp ->
            Icon.Create(
              (if exp then
                 Icon.Hardware Hardware.KeyboardArrowDown
               else
                 Icon.Hardware Hardware.KeyboardArrowRight),
              attrs = [ Attr.Style "font-size" "16px"; Attr.Style "opacity" "0.6" ]
            ))
        ]
        isExpanded.View
        |> Doc.BindView(fun exp -> if exp then div [] items else Doc.Empty)
      ]

    let navList =
      div [ cls [ yield! Padding.toClasses Padding.Vertical.extraSmall ] ] [
        div [
          cls [
            Flex.Flex.allSizes
            AlignItems.toClass AlignItems.Center
            yield! Padding.toClasses Padding.Vertical.extraSmall
            yield! Padding.toClasses Padding.Horizontal.small
          ]
          Attr.Style "cursor" "pointer"
          Attr.Style "border-radius" "6px"
          Attr.Style "margin" "1px 8px"
          Attr.Style "gap" "8px"
          Attr.Class "weave-nav-leaf"
          Attr.DynamicClassPred
            "weave-nav-item--active"
            (selectedNav.View |> View.Map(fun s -> s = Some "Home"))
          on.click (fun _ _ -> navigateTo Home)
        ] [
          Icon.Create(Icon.UiActions UiActions.Home, attrs = [ Attr.Style "font-size" "18px" ])
          Body2.Div("Home")
        ]

        Divider.Create(attrs = [ Margin.toClasses Margin.Vertical.extraSmall |> cls ])

        navGroup (Icon.Android Android.Widgets) "Layout" layoutExpanded [
          navLeafItem "App Bar"
          navLeafItem "Container"
          navLeafItem "Divider"
          navLeafItem "Drawer"
          navLeafItem "Grid"
          navLeafItem "Spacer"
        ]

        navGroup (Icon.Maps Maps.Explore) "Navigation" navSectionExpanded [
          navLeafItem "Link"
          navLeafItem "Tabs"
        ]

        navGroup (Icon.UiActions UiActions.InputCircle) "Inputs" inputsExpanded [
          navLeafItem "Button"
          navLeafItem "Button Group"
          navLeafItem "Button Menu"
          navLeafItem "Checkbox"
          navLeafItem "Dropdown"
          navLeafItem "Field"
          navLeafItem "Numeric Field"
          navLeafItem "Radio Button"
          navLeafItem "Switch"
        ]

        navGroup (Icon.Business Business.Analytics) "Data Display" dataExpanded [
          navLeafItem "Icons"
          navLeafItem "List"
          navLeafItem "Tooltip"
          navLeafItem "Typography"
        ]

        navGroup (Icon.Action Action.Feedback) "Feedback" feedbackExpanded [
          navLeafItem "Alert"
          navLeafItem "Dialog"
          navLeafItem "Expansion Panel"
        ]
      ]

    let appBarContent =
      div [
        cls [
          Flex.Flex.allSizes
          AlignItems.toClass AlignItems.Center
          yield! Padding.toClasses Padding.Horizontal.small
          yield! Padding.toClasses Padding.Vertical.extraSmall
        ]
      ] [
        Button.CreateIcon(
          Icon.Create(Icon.UiActions UiActions.Menu),
          onClick = (fun () -> Var.Set drawerOpen (not drawerOpen.Value)),
          attrs = [ Margin.toClasses Margin.Right.extraSmall |> cls ]
        )

        div [
          cls [ Flex.Flex.allSizes; AlignItems.toClass AlignItems.Center ]
          Attr.Style "gap" "8px"
          Attr.Style "cursor" "pointer"
          on.click (fun _ _ -> navigateTo Home)
        ] [ H6.Div("Weave") ]

        Spacer.Create()

        a [
          attr.href "https://github.com/1eyewonder/Weave"
          attr.target "_blank"
          Attr.Style "color" "inherit"
          Attr.Style "display" "flex"
          Attr.Style "align-items" "center"
          cls [ yield! Padding.toClasses Padding.Horizontal.extraSmall ]
        ] [ githubSvg ]

        Button.CreateIcon(
          Theme.current.View
          |> Doc.BindView(fun mode ->
            match mode with
            | Theming.Light -> Icon.Create(Icon.Android Android.DarkMode)
            | Theming.Dark -> Icon.Create(Icon.Android Android.LightMode)),
          onClick =
            (fun () ->
              let newMode = Theming.toggleMode ()
              Var.Set Theme.current newMode)
        )
      ]

    div [
      Attr.Style "height" "100vh"
      Attr.Style "overflow" "hidden"
      cls [ Flex.Flex.allSizes; FlexDirection.Column.allSizes ]
      SurfaceColor.toBackgroundColor SurfaceColor.Background
    ] [
      navEffects

      AppBar.Create(
        appBarContent,
        position = AppBar.Position.Static,
        attrs = [ BrandColor.toBackgroundColor BrandColor.Primary ]
      )

      DrawerContainer.Create(
        mainContent =
          div [
            cl "weave-main-content"
            Attr.Style "overflow-y" "auto"
            Attr.Style "scroll-behavior" "smooth"
            Attr.Style "height" "100%"
            on.afterRender (fun el ->
              Var.Set mainEl (Some el)
              // If the initial URL contained a section, poll until the page content renders
              initialSection |> Option.iter scrollToSectionWhenReady)
            ScrollListener.trackSections ".section-header[id]" 80.0 (fun sectionId ->
              let pageHash =
                selectedNav.Value
                |> Option.bind stringToPage
                |> Option.map pageToHash
                |> Option.defaultValue "#home"

              if sectionId <> "" then
                replaceStateHash (pageHash + "/" + sectionId)
              else
                replaceStateHash pageHash)
          ] [
            div [ cls [ yield! Padding.toClasses Padding.All.small ] ] [
              currentPageView |> Doc.BindView(renderPage navigateTo)
            ]
          ],
        leftDrawer =
          Drawer.Create(
            navList,
            drawerOpen.View,
            variant = Drawer.Variant.Temporary,
            position = Drawer.Position.Left,
            breakpoint = Drawer.DrawerBreakpoint.At Breakpoint.Medium,
            overlayClose = (fun () -> Var.Set drawerOpen false),
            isFixed = false,
            attrs = [ Attr.Style "overflow-y" "auto" ]
          ),
        attrs = [ Attr.Style "flex" "1"; Attr.Style "min-height" "0" ]
      )
    ]
