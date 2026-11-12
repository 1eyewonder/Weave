namespace Weave.Tests.E2E.Site

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave

[<JavaScript>]
module SelectSizingPage =

  let longLabel =
    "A longer option whose full text should remain available in the menu"

  let longWord = "AnExceptionallyLongUnbrokenOptionThatMustWrapAtTheViewportBoundary"

  let render () =
    let items =
      [ "Short"; longLabel; longWord ]
      |> List.map (fun label -> SelectItem.create (text label, label, label))
      |> View.Const

    let select name attributes positioning =
      div [
        Attr.Create "data-testid" name
        Attr.Style "inline-size" "150px"
        Attr.Style "margin-block-end" "16px"
        yield! positioning
      ] [
        Select.create (
          items,
          Var.Create(Some longLabel),
          labelText = View.Const "Size",
          attrs = [ Select.Width.full; Select.Variant.outlined; yield! attributes ]
        )
      ]

    div [
      Attr.Create "data-testid" "select-sizing-page"
      Attr.Style "block-size" "100vh"
      Attr.Style "box-sizing" "border-box"
      Attr.Style "padding" "16px"
      Attr.Style "overflow" "auto"
    ] [
      div [ Attr.Style "max-width" "320px" ] [
        for name, label, widthAttrs in
          [
            "stable", "Stable (default)", []
            "dynamic", "Dynamic width", [ Select.Width.dynamic ]
            "full", "A long descriptive choice label that remains accessible", [ Select.Width.full ]
          ] do
          let measuredItems =
            (if name = "dynamic" then
               [ "A"; "A considerably longer option" ]
             else
               [ "A"; "BB" ])
            |> List.map (fun value -> SelectItem.create (text value, value, value))
            |> View.Const

          div [ Attr.Create "data-testid" ("measured-" + name) ] [
            Select.create (
              measuredItems,
              Var.Create<string option> None,
              labelText = View.Const label,
              attrs = [ Select.Variant.outlined; yield! widthAttrs ]
            )
          ]
      ]
      select "bound" [] []
      select "unbound" [ Select.PopoverWidth.unbound ] []

      let richContent =
        span [ Attr.Create "data-testid" "rich-value" ] [
          span [] [ text "Rich" ]
          span [ Attr.Style "margin-inline-start" "4px" ] [ text "value" ]
        ]

      let richItems =
        View.Const [
          SelectItem.create (text "Rich option", "Rich option", "rich", selectedContent = richContent)
        ]

      div [ Attr.Create "data-testid" "rich"; Attr.Style "inline-size" "150px" ] [
        Select.create (
          richItems,
          Var.Create(Some "rich"),
          labelText = View.Const "Custom value",
          attrs = [ Select.Width.full; Select.Variant.outlined ]
        )
      ]

      select "edge" [ Select.PopoverWidth.unbound ] [
        Attr.Style "position" "fixed"
        Attr.Style "right" "8px"
        Attr.Style "bottom" "8px"
      ]

      select "viewport-moving" [ Select.PopoverWidth.unbound ] [
        Attr.Style "position" "fixed"
        Attr.Style "left" "45%"
        Attr.Style "top" "80px"
      ]

      select "origin-right" [
        Select.PopoverWidth.unbound
        Select.AnchorOrigin.bottomRight
        Select.TransformOrigin.topRight
      ] [
        Attr.Style "position" "fixed"
        Attr.Style "left" "45%"
        Attr.Style "top" "240px"
      ]

      select "origin-above" [
        Select.PopoverWidth.unbound
        Select.AnchorOrigin.topLeft
        Select.TransformOrigin.bottomLeft
      ] [
        Attr.Style "position" "fixed"
        Attr.Style "left" "45%"
        Attr.Style "top" "480px"
      ]

      div [ Attr.Style "block-size" "500px" ] []
      select "scrolling" [ Select.PopoverWidth.unbound ] []
      div [ Attr.Style "block-size" "600px" ] []
    ]
