namespace Weave.Docs.Examples

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave
open Weave.Docs.Examples.DocsRouting

[<JavaScript>]
module ComponentPreviews =

  let forPage (page: Page) : Doc =
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
    | ChipExamples ->
      cp [
        div [
          cl "cp-pill"
          Attr.Style "top" "20px"
          Attr.Style "left" "8%"
          Attr.Style "width" "30%"
          Attr.Style "height" "20px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "20px"
          Attr.Style "left" "44%"
          Attr.Style "width" "24%"
          Attr.Style "height" "20px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "50px"
          Attr.Style "left" "8%"
          Attr.Style "width" "28%"
          Attr.Style "height" "20px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "50px"
          Attr.Style "left" "42%"
          Attr.Style "width" "32%"
          Attr.Style "height" "20px"
        ] []
      ]
    | ChipSetExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "10px"
          Attr.Style "left" "6%"
          Attr.Style "right" "6%"
          Attr.Style "bottom" "10px"
          Attr.Style "border-radius" "6px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "24px"
          Attr.Style "left" "12%"
          Attr.Style "width" "22%"
          Attr.Style "height" "18px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "24px"
          Attr.Style "left" "40%"
          Attr.Style "width" "22%"
          Attr.Style "height" "18px"
          Attr.Style "border-radius" "9px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "24px"
          Attr.Style "left" "68%"
          Attr.Style "width" "20%"
          Attr.Style "height" "18px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "52px"
          Attr.Style "left" "12%"
          Attr.Style "width" "26%"
          Attr.Style "height" "18px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "52px"
          Attr.Style "left" "44%"
          Attr.Style "width" "20%"
          Attr.Style "height" "18px"
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
    | SelectExamples ->
      cp [
        div [
          cl "cp-box"
          Attr.Style "top" "8px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "24px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "14px"
          Attr.Style "left" "16%"
          Attr.Style "right" "32%"
          Attr.Style "height" "5px"
          Attr.Style "opacity" "0.3"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "18px"
          Attr.Style "left" "16%"
          Attr.Style "right" "44%"
          Attr.Style "height" "6px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "14px"
          Attr.Style "right" "16%"
          Attr.Style "width" "6px"
          Attr.Style "height" "6px"
          Attr.Style "border-radius" "1px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "34px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "48px"
          Attr.Style "border-radius" "4px"
          Attr.Style "box-shadow" "0 4px 12px rgba(0,0,0,0.4)"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "42px"
          Attr.Style "left" "18%"
          Attr.Style "right" "44%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "54px"
          Attr.Style "left" "18%"
          Attr.Style "right" "36%"
          Attr.Style "height" "5px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "66px"
          Attr.Style "left" "18%"
          Attr.Style "right" "50%"
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
    | SpacingExamples ->
      cp [
        div [
          cl "cp-pill"
          Attr.Style "top" "10px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "16px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "36px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "20px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "66px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "24px"
        ] []
      ]
    | OpacityExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "10px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "14px"
          Attr.Style "opacity" "0.2"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "30px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "14px"
          Attr.Style "opacity" "0.5"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "50px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "14px"
          Attr.Style "opacity" "0.8"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "70px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "14px"
        ] []
      ]
    | TransitionExamples ->
      cp [
        div [
          cl "cp-pill"
          Attr.Style "top" "14px"
          Attr.Style "left" "10%"
          Attr.Style "right" "50%"
          Attr.Style "height" "12px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "34px"
          Attr.Style "left" "10%"
          Attr.Style "right" "30%"
          Attr.Style "height" "12px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "54px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "12px"
        ] []
      ]
    | BorderExamples ->
      cp [
        div [
          cl "cp-box"
          Attr.Style "top" "10px"
          Attr.Style "left" "10%"
          Attr.Style "right" "54%"
          Attr.Style "height" "30px"
        ] []
        div [
          cl "cp-box"
          Attr.Style "top" "10px"
          Attr.Style "left" "54%"
          Attr.Style "right" "10%"
          Attr.Style "height" "30px"
          Attr.Style "border-radius" "10px"
        ] []
        div [
          cl "cp-box"
          Attr.Style "top" "50px"
          Attr.Style "left" "10%"
          Attr.Style "right" "54%"
          Attr.Style "height" "30px"
          Attr.Style "border-style" "dashed"
        ] []
        div [
          cl "cp-box"
          Attr.Style "top" "50px"
          Attr.Style "left" "54%"
          Attr.Style "right" "10%"
          Attr.Style "height" "30px"
          Attr.Style "border-style" "dotted"
          Attr.Style "border-radius" "50%"
        ] []
      ]
    | DisplayExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "10px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "14px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "30px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "14px"
          Attr.Style "opacity" "0.5"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "50px"
          Attr.Style "left" "8%"
          Attr.Style "right" "54%"
          Attr.Style "height" "14px"
          Attr.Style "border-radius" "3px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "50px"
          Attr.Style "left" "54%"
          Attr.Style "right" "8%"
          Attr.Style "height" "14px"
          Attr.Style "border-radius" "3px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "70px"
          Attr.Style "left" "8%"
          Attr.Style "right" "40%"
          Attr.Style "height" "10px"
        ] []
      ]
    | ElevationExamples ->
      cp [
        div [
          cl "cp-fill"
          Attr.Style "top" "12px"
          Attr.Style "left" "10%"
          Attr.Style "right" "54%"
          Attr.Style "height" "30px"
          Attr.Style "border-radius" "4px"
          Attr.Style "box-shadow" "0 1px 3px rgba(0,0,0,0.2)"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "12px"
          Attr.Style "left" "54%"
          Attr.Style "right" "10%"
          Attr.Style "height" "30px"
          Attr.Style "border-radius" "4px"
          Attr.Style "box-shadow" "0 4px 12px rgba(0,0,0,0.3)"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "52px"
          Attr.Style "left" "10%"
          Attr.Style "right" "54%"
          Attr.Style "height" "30px"
          Attr.Style "border-radius" "4px"
          Attr.Style "box-shadow" "0 8px 24px rgba(0,0,0,0.4)"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "52px"
          Attr.Style "left" "54%"
          Attr.Style "right" "10%"
          Attr.Style "height" "30px"
          Attr.Style "border-radius" "4px"
          Attr.Style "box-shadow" "0 12px 36px rgba(0,0,0,0.5)"
        ] []
      ]
    | FlexboxExamples ->
      cp [
        div [
          cl "cp-bar"
          Attr.Style "top" "14px"
          Attr.Style "left" "8%"
          Attr.Style "width" "26%"
          Attr.Style "height" "20px"
          Attr.Style "border-radius" "3px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "14px"
          Attr.Style "left" "38%"
          Attr.Style "width" "26%"
          Attr.Style "height" "20px"
          Attr.Style "border-radius" "3px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "14px"
          Attr.Style "left" "68%"
          Attr.Style "width" "26%"
          Attr.Style "height" "20px"
          Attr.Style "border-radius" "3px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "46px"
          Attr.Style "left" "8%"
          Attr.Style "width" "42%"
          Attr.Style "height" "16px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "46px"
          Attr.Style "right" "8%"
          Attr.Style "width" "42%"
          Attr.Style "height" "16px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "68px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "16px"
        ] []
      ]
    | ThemingExamples ->
      cp [
        div [
          cl "cp-pill"
          Attr.Style "top" "12px"
          Attr.Style "left" "10%"
          Attr.Style "right" "50%"
          Attr.Style "height" "10px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "32px"
          Attr.Style "left" "10%"
          Attr.Style "right" "10%"
          Attr.Style "height" "50px"
          Attr.Style "border-radius" "6px"
        ] []
      ]
    // Showcase thumbnails
    | ShowcaseTaskTracker ->
      cp [
        // Checkbox + line rows (task list look)
        for top in [ 12; 32; 52; 72 ] do
          div [
            cl "cp-dot"
            Attr.Style "top" (sprintf "%dpx" top)
            Attr.Style "left" "10%"
            Attr.Style "width" "10px"
            Attr.Style "height" "10px"
          ] []

          div [
            cl "cp-pill"
            Attr.Style "top" (sprintf "%dpx" top)
            Attr.Style "left" "24%"
            Attr.Style "right" "10%"
            Attr.Style "height" "10px"
          ] []
      ]
    | ShowcasePomodoroTimer ->
      cp [
        // Circular timer shape
        div [
          cl "cp-dot"
          Attr.Style "top" "15px"
          Attr.Style "left" "50%"
          Attr.Style "transform" "translateX(-50%)"
          Attr.Style "width" "50px"
          Attr.Style "height" "50px"
          Attr.Style "border-radius" "50%"
        ] []

        // Control buttons
        div [
          cl "cp-pill"
          Attr.Style "top" "75px"
          Attr.Style "left" "20%"
          Attr.Style "width" "25%"
          Attr.Style "height" "10px"
        ] []

        div [
          cl "cp-pill"
          Attr.Style "top" "75px"
          Attr.Style "right" "20%"
          Attr.Style "width" "25%"
          Attr.Style "height" "10px"
        ] []
      ]
    | ShowcaseExpenseTracker ->
      cp [
        // Summary cards row
        for left in [ 8; 36; 64 ] do
          div [
            cl "cp-pill"
            Attr.Style "top" "10px"
            Attr.Style "left" (sprintf "%d%%" left)
            Attr.Style "width" "26%"
            Attr.Style "height" "28px"
            Attr.Style "border-radius" "4px"
          ] []

        // Expense rows
        for top in [ 48; 64; 80 ] do
          div [
            cl "cp-pill"
            Attr.Style "top" (sprintf "%dpx" top)
            Attr.Style "left" "8%"
            Attr.Style "right" "8%"
            Attr.Style "height" "10px"
          ] []
      ]
    | ShowcaseCharacterSheet ->
      cp [
        // Tab bar
        div [
          cl "cp-bar"
          Attr.Style "top" "0"
          Attr.Style "left" "0"
          Attr.Style "right" "0"
          Attr.Style "height" "12px"
        ] []

        // Ability score grid (2x3)
        for row in [ 0; 1 ] do
          for col in [ 0; 1; 2 ] do
            div [
              cl "cp-pill"
              Attr.Style "top" (sprintf "%dpx" (22 + row * 35))
              Attr.Style "left" (sprintf "%d%%" (8 + col * 31))
              Attr.Style "width" "26%"
              Attr.Style "height" "28px"
              Attr.Style "border-radius" "4px"
            ] []
      ]
    | SliderExamples ->
      cp [
        // Label text
        div [
          cl "cp-line"
          Attr.Style "top" "14px"
          Attr.Style "left" "8%"
          Attr.Style "right" "55%"
          Attr.Style "height" "5px"
        ] []
        // Track background
        div [
          cl "cp-line"
          Attr.Style "top" "30px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "4px"
        ] []
        // Fill
        div [
          cl "cp-bar"
          Attr.Style "top" "30px"
          Attr.Style "left" "8%"
          Attr.Style "width" "50%"
          Attr.Style "height" "4px"
          Attr.Style "border-radius" "2px"
        ] []
        // Thumb
        div [
          cl "cp-dot"
          Attr.Style "top" "26px"
          Attr.Style "left" "calc(8% + 50% - 6px)"
          Attr.Style "width" "12px"
          Attr.Style "height" "12px"
        ] []
        // Second slider — track
        div [
          cl "cp-line"
          Attr.Style "top" "58px"
          Attr.Style "left" "8%"
          Attr.Style "right" "8%"
          Attr.Style "height" "4px"
        ] []
        // Second slider — fill
        div [
          cl "cp-bar"
          Attr.Style "top" "58px"
          Attr.Style "left" "8%"
          Attr.Style "width" "30%"
          Attr.Style "height" "4px"
          Attr.Style "border-radius" "2px"
        ] []
        // Second slider — thumb
        div [
          cl "cp-dot"
          Attr.Style "top" "54px"
          Attr.Style "left" "calc(8% + 30% - 6px)"
          Attr.Style "width" "12px"
          Attr.Style "height" "12px"
        ] []
      ]
    | AnimationExamples ->
      cp [
        // Three staggered bars suggesting a cascading slide-in animation
        div [
          cl "cp-bar"
          Attr.Style "top" "14px"
          Attr.Style "left" "8%"
          Attr.Style "right" "20%"
          Attr.Style "height" "14px"
          Attr.Style "border-radius" "3px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "36px"
          Attr.Style "left" "18%"
          Attr.Style "right" "12%"
          Attr.Style "height" "14px"
          Attr.Style "border-radius" "3px"
          Attr.Style "opacity" "0.7"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "58px"
          Attr.Style "left" "28%"
          Attr.Style "right" "8%"
          Attr.Style "height" "14px"
          Attr.Style "border-radius" "3px"
          Attr.Style "opacity" "0.4"
        ] []
      ]
    | ReactiveHelpersExamples ->
      cp [
        // Two small boxes connected by an arrow
        div [
          cl "cp-box"
          Attr.Style "top" "20px"
          Attr.Style "left" "12px"
          Attr.Style "width" "24px"
          Attr.Style "height" "24px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "30px"
          Attr.Style "left" "40px"
          Attr.Style "width" "30px"
          Attr.Style "height" "4px"
          Attr.Style "border-radius" "2px"
        ] []
        div [
          cl "cp-box"
          Attr.Style "top" "20px"
          Attr.Style "left" "74px"
          Attr.Style "width" "24px"
          Attr.Style "height" "24px"
        ] []
        div [
          cl "cp-bar"
          Attr.Style "top" "30px"
          Attr.Style "left" "102px"
          Attr.Style "width" "30px"
          Attr.Style "height" "4px"
          Attr.Style "border-radius" "2px"
        ] []
        div [
          cl "cp-fill"
          Attr.Style "top" "16px"
          Attr.Style "left" "136px"
          Attr.Style "width" "32px"
          Attr.Style "height" "32px"
        ] []
        // Bottom flow
        div [
          cl "cp-line"
          Attr.Style "top" "58px"
          Attr.Style "left" "12px"
          Attr.Style "width" "80px"
          Attr.Style "height" "6px"
        ] []
        div [
          cl "cp-line"
          Attr.Style "top" "58px"
          Attr.Style "left" "100px"
          Attr.Style "width" "50px"
          Attr.Style "height" "6px"
        ] []
      ]
    | EventHandlersExamples ->
      cp [
        // Cursor-like triangle
        div [
          cl "cp-bar"
          Attr.Style "top" "22px"
          Attr.Style "left" "30px"
          Attr.Style "width" "20px"
          Attr.Style "height" "28px"
          Attr.Style "clip-path" "polygon(0 0, 100% 50%, 30% 100%)"
        ] []
        // Action lines radiating from cursor
        div [
          cl "cp-pill"
          Attr.Style "top" "18px"
          Attr.Style "left" "56px"
          Attr.Style "width" "40px"
          Attr.Style "height" "4px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "30px"
          Attr.Style "left" "56px"
          Attr.Style "width" "30px"
          Attr.Style "height" "4px"
        ] []
        div [
          cl "cp-pill"
          Attr.Style "top" "42px"
          Attr.Style "left" "56px"
          Attr.Style "width" "35px"
          Attr.Style "height" "4px"
        ] []
        // Keyboard key shapes
        div [
          cl "cp-box"
          Attr.Style "top" "58px"
          Attr.Style "left" "12px"
          Attr.Style "width" "22px"
          Attr.Style "height" "18px"
          Attr.Style "border-radius" "3px"
        ] []
        div [
          cl "cp-box"
          Attr.Style "top" "58px"
          Attr.Style "left" "38px"
          Attr.Style "width" "22px"
          Attr.Style "height" "18px"
          Attr.Style "border-radius" "3px"
        ] []
        div [
          cl "cp-box"
          Attr.Style "top" "58px"
          Attr.Style "left" "64px"
          Attr.Style "width" "50px"
          Attr.Style "height" "18px"
          Attr.Style "border-radius" "3px"
        ] []
      ]
    | _ -> Doc.Empty
