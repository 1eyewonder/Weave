namespace Weave.CssHelpers

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open Weave.CssHelpers.Core

[<AutoOpen; JavaScript>]
module Layout =

  module BrowserUtils =

    let windowWidth: Var<int> = Var.Create JS.Window.InnerWidth

    do JS.Window.AddEventListener("resize", fun (e: Dom.Event) -> windowWidth.Value <- JS.Window.InnerWidth)

  module Breakpoint =

    let browser =
      BrowserUtils.windowWidth.View
      |> View.MapCached(fun width ->
        if width < 600 then Breakpoint.ExtraSmall
        elif width < 960 then Breakpoint.Small
        elif width < 1280 then Breakpoint.Medium
        elif width < 1920 then Breakpoint.Large
        elif width < 2560 then Breakpoint.ExtraLarge
        else Breakpoint.ExtraExtraLarge)

    let browserAsText =
      browser
      |> View.MapCached(fun bp ->
        match bp with
        | Breakpoint.ExtraSmall -> "XS"
        | Breakpoint.Small -> "SM"
        | Breakpoint.Medium -> "MD"
        | Breakpoint.Large -> "LG"
        | Breakpoint.ExtraLarge -> "XL"
        | Breakpoint.ExtraExtraLarge -> "XXL")

  [<RequireQualifiedAccess; Struct>]
  type Orientation =
    | Portrait
    | Landscape

  module Orientation =

    let browser =
      BrowserUtils.windowWidth.View
      |> View.MapCached(fun width ->
        if JS.Window.InnerHeight >= width then
          Orientation.Portrait
        else
          Orientation.Landscape)

  /// <summary>
  /// Controls if a container is a flex box
  /// </summary>
  module Flex =

    module Flex =

      let allSizes = cl Css.``weave-d-flex``
      let xs = cl Css.``weave-d-xs-flex``
      let sm = cl Css.``weave-d-sm-flex``
      let md = cl Css.``weave-d-md-flex``
      let lg = cl Css.``weave-d-lg-flex``
      let xl = cl Css.``weave-d-xl-flex``
      let xxl = cl Css.``weave-d-xxl-flex``

    module Inline =

      let allSizes = cl Css.``weave-d-inline-flex``
      let xs = cl Css.``weave-d-xs-inline-flex``
      let sm = cl Css.``weave-d-sm-inline-flex``
      let md = cl Css.``weave-d-md-inline-flex``
      let lg = cl Css.``weave-d-lg-inline-flex``
      let xl = cl Css.``weave-d-xl-inline-flex``
      let xxl = cl Css.``weave-d-xxl-inline-flex``

    module InlineBlock =

      let allSizes = cl Css.``weave-d-inline-block``
      let xs = cl Css.``weave-d-xs-inline-block``
      let sm = cl Css.``weave-d-sm-inline-block``
      let md = cl Css.``weave-d-md-inline-block``
      let lg = cl Css.``weave-d-lg-inline-block``
      let xl = cl Css.``weave-d-xl-inline-block``
      let xxl = cl Css.``weave-d-xxl-inline-block``

  /// <summary>
  /// Controls the wrap of flex items. Place on a flex container.
  /// </summary>
  module FlexWrap =

    module NoWrap =

      let allSizes = cl Css.``weave-flex-nowrap``
      let xs = cl Css.``weave-flex-xs-nowrap``
      let sm = cl Css.``weave-flex-sm-nowrap``
      let md = cl Css.``weave-flex-md-nowrap``
      let lg = cl Css.``weave-flex-lg-nowrap``
      let xl = cl Css.``weave-flex-xl-nowrap``
      let xxl = cl Css.``weave-flex-xxl-nowrap``

    module Wrap =

      let allSizes = cl Css.``weave-flex-wrap``
      let xs = cl Css.``weave-flex-xs-wrap``
      let sm = cl Css.``weave-flex-sm-wrap``
      let md = cl Css.``weave-flex-md-wrap``
      let lg = cl Css.``weave-flex-lg-wrap``
      let xl = cl Css.``weave-flex-xl-wrap``
      let xxl = cl Css.``weave-flex-xxl-wrap``

    module WrapReverse =

      let allSizes = cl Css.``weave-flex-wrap-reverse``
      let xs = cl Css.``weave-flex-xs-wrap-reverse``
      let sm = cl Css.``weave-flex-sm-wrap-reverse``
      let md = cl Css.``weave-flex-md-wrap-reverse``
      let lg = cl Css.``weave-flex-lg-wrap-reverse``
      let xl = cl Css.``weave-flex-xl-wrap-reverse``
      let xxl = cl Css.``weave-flex-xxl-wrap-reverse``

  /// <summary>
  /// Controls the direction of flex items. Place on a flex container.
  /// </summary>
  module FlexDirection =

    module Row =

      let allSizes = cl Css.``weave-flex-row``
      let xs = cl Css.``weave-flex-xs-row``
      let sm = cl Css.``weave-flex-sm-row``
      let md = cl Css.``weave-flex-md-row``
      let lg = cl Css.``weave-flex-lg-row``
      let xl = cl Css.``weave-flex-xl-row``
      let xxl = cl Css.``weave-flex-xxl-row``

    module RowReverse =

      let allSizes = cl Css.``weave-flex-row-reverse``
      let xs = cl Css.``weave-flex-xs-row-reverse``
      let sm = cl Css.``weave-flex-sm-row-reverse``
      let md = cl Css.``weave-flex-md-row-reverse``
      let lg = cl Css.``weave-flex-lg-row-reverse``
      let xl = cl Css.``weave-flex-xl-row-reverse``
      let xxl = cl Css.``weave-flex-xxl-row-reverse``

    module Column =

      let allSizes = cl Css.``weave-flex-column``
      let xs = cl Css.``weave-flex-xs-column``
      let sm = cl Css.``weave-flex-sm-column``
      let md = cl Css.``weave-flex-md-column``
      let lg = cl Css.``weave-flex-lg-column``
      let xl = cl Css.``weave-flex-xl-column``
      let xxl = cl Css.``weave-flex-xxl-column``

    module ColumnReverse =

      let allSizes = cl Css.``weave-flex-column-reverse``
      let xs = cl Css.``weave-flex-xs-column-reverse``
      let sm = cl Css.``weave-flex-sm-column-reverse``
      let md = cl Css.``weave-flex-md-column-reverse``
      let lg = cl Css.``weave-flex-lg-column-reverse``
      let xl = cl Css.``weave-flex-xl-column-reverse``
      let xxl = cl Css.``weave-flex-xxl-column-reverse``

  /// <summary>
  /// Utilities for controlling how flex items both grow and shrink. Place on items within a flex container.
  /// </summary>
  module FlexItem =

    module Flex =

      let allSizes = cl Css.``weave-flex-1``
      let xs = cl Css.``weave-flex-xs-1``
      let sm = cl Css.``weave-flex-sm-1``
      let md = cl Css.``weave-flex-md-1``
      let lg = cl Css.``weave-flex-lg-1``
      let xl = cl Css.``weave-flex-xl-1``
      let xxl = cl Css.``weave-flex-xxl-1``

    module Auto =

      let allSizes = cl Css.``weave-flex-auto``
      let xs = cl Css.``weave-flex-xs-auto``
      let sm = cl Css.``weave-flex-sm-auto``
      let md = cl Css.``weave-flex-md-auto``
      let lg = cl Css.``weave-flex-lg-auto``
      let xl = cl Css.``weave-flex-xl-auto``
      let xxl = cl Css.``weave-flex-xxl-auto``

    module Initial =

      let allSizes = cl Css.``weave-flex-initial``
      let xs = cl Css.``weave-flex-xs-initial``
      let sm = cl Css.``weave-flex-sm-initial``
      let md = cl Css.``weave-flex-md-initial``
      let lg = cl Css.``weave-flex-lg-initial``
      let xl = cl Css.``weave-flex-xl-initial``
      let xxl = cl Css.``weave-flex-xxl-initial``

    module Grow =

      let allSizes = cl Css.``weave-flex-grow-1``
      let xs = cl Css.``weave-flex-xs-grow-1``
      let sm = cl Css.``weave-flex-sm-grow-1``
      let md = cl Css.``weave-flex-md-grow-1``
      let lg = cl Css.``weave-flex-lg-grow-1``
      let xl = cl Css.``weave-flex-xl-grow-1``
      let xxl = cl Css.``weave-flex-xxl-grow-1``

    module NoGrow =

      let allSizes = cl Css.``weave-flex-grow-0``
      let xs = cl Css.``weave-flex-xs-grow-0``
      let sm = cl Css.``weave-flex-sm-grow-0``
      let md = cl Css.``weave-flex-md-grow-0``
      let lg = cl Css.``weave-flex-lg-grow-0``
      let xl = cl Css.``weave-flex-xl-grow-0``
      let xxl = cl Css.``weave-flex-xxl-grow-0``

    module Shrink =

      let allSizes = cl Css.``weave-flex-shrink-1``
      let xs = cl Css.``weave-flex-xs-shrink-1``
      let sm = cl Css.``weave-flex-sm-shrink-1``
      let md = cl Css.``weave-flex-md-shrink-1``
      let lg = cl Css.``weave-flex-lg-shrink-1``
      let xl = cl Css.``weave-flex-xl-shrink-1``
      let xxl = cl Css.``weave-flex-xxl-shrink-1``

    module NoShrink =

      let allSizes = cl Css.``weave-flex-shrink-0``
      let xs = cl Css.``weave-flex-xs-shrink-0``
      let sm = cl Css.``weave-flex-sm-shrink-0``
      let md = cl Css.``weave-flex-md-shrink-0``
      let lg = cl Css.``weave-flex-lg-shrink-0``
      let xl = cl Css.``weave-flex-xl-shrink-0``
      let xxl = cl Css.``weave-flex-xxl-shrink-0``

    module None =

      let allSizes = cl Css.``weave-flex-none``
      let xs = cl Css.``weave-flex-xs-none``
      let sm = cl Css.``weave-flex-sm-none``
      let md = cl Css.``weave-flex-md-none``
      let lg = cl Css.``weave-flex-lg-none``
      let xl = cl Css.``weave-flex-xl-none``
      let xxl = cl Css.``weave-flex-xxl-none``

    /// <summary>
    /// Container-level grow helpers that apply flex-grow to specific children.
    /// Place on a flex container to control which children grow.
    /// </summary>
    module GrowChildren =

      module Start =

        let allSizes = cl Css.``weave-flex-grow-start``
        let xs = cl Css.``weave-flex-xs-grow-start``
        let sm = cl Css.``weave-flex-sm-grow-start``
        let md = cl Css.``weave-flex-md-grow-start``
        let lg = cl Css.``weave-flex-lg-grow-start``
        let xl = cl Css.``weave-flex-xl-grow-start``
        let xxl = cl Css.``weave-flex-xxl-grow-start``

      module End =

        let allSizes = cl Css.``weave-flex-grow-end``
        let xs = cl Css.``weave-flex-xs-grow-end``
        let sm = cl Css.``weave-flex-sm-grow-end``
        let md = cl Css.``weave-flex-md-grow-end``
        let lg = cl Css.``weave-flex-lg-grow-end``
        let xl = cl Css.``weave-flex-xl-grow-end``
        let xxl = cl Css.``weave-flex-xxl-grow-end``

      module StartAndEnd =

        let allSizes = cl Css.``weave-flex-grow-start-and-end``
        let xs = cl Css.``weave-flex-xs-grow-start-and-end``
        let sm = cl Css.``weave-flex-sm-grow-start-and-end``
        let md = cl Css.``weave-flex-md-grow-start-and-end``
        let lg = cl Css.``weave-flex-lg-grow-start-and-end``
        let xl = cl Css.``weave-flex-xl-grow-start-and-end``
        let xxl = cl Css.``weave-flex-xxl-grow-start-and-end``

      module Middle =

        let allSizes = cl Css.``weave-flex-grow-middle``
        let xs = cl Css.``weave-flex-xs-grow-middle``
        let sm = cl Css.``weave-flex-sm-grow-middle``
        let md = cl Css.``weave-flex-md-grow-middle``
        let lg = cl Css.``weave-flex-lg-grow-middle``
        let xl = cl Css.``weave-flex-xl-grow-middle``
        let xxl = cl Css.``weave-flex-xxl-grow-middle``

      module All =

        let allSizes = cl Css.``weave-flex-grow-all``
        let xs = cl Css.``weave-flex-xs-grow-all``
        let sm = cl Css.``weave-flex-sm-grow-all``
        let md = cl Css.``weave-flex-md-grow-all``
        let lg = cl Css.``weave-flex-lg-grow-all``
        let xl = cl Css.``weave-flex-xl-grow-all``
        let xxl = cl Css.``weave-flex-xxl-grow-all``

  /// <summary>
  /// Controls how flex and grid items are positioned along a container's main axis. Place on a flex or grid container.
  /// </summary>
  module JustifyContent =

    let flexStart = cl Css.``weave-justify-start``
    let center = cl Css.``weave-justify-center``
    let flexEnd = cl Css.``weave-justify-end``
    let spaceBetween = cl Css.``weave-justify-space-between``
    let spaceAround = cl Css.``weave-justify-space-around``
    let spaceEvenly = cl Css.``weave-justify-space-evenly``

    module ExtraSmall =

      let flexStart = cl Css.``weave-justify-xs-start``
      let center = cl Css.``weave-justify-xs-center``
      let flexEnd = cl Css.``weave-justify-xs-end``
      let spaceBetween = cl Css.``weave-justify-xs-space-between``
      let spaceAround = cl Css.``weave-justify-xs-space-around``
      let spaceEvenly = cl Css.``weave-justify-xs-space-evenly``

    module Small =

      let flexStart = cl Css.``weave-justify-sm-start``
      let center = cl Css.``weave-justify-sm-center``
      let flexEnd = cl Css.``weave-justify-sm-end``
      let spaceBetween = cl Css.``weave-justify-sm-space-between``
      let spaceAround = cl Css.``weave-justify-sm-space-around``
      let spaceEvenly = cl Css.``weave-justify-sm-space-evenly``

    module Medium =

      let flexStart = cl Css.``weave-justify-md-start``
      let center = cl Css.``weave-justify-md-center``
      let flexEnd = cl Css.``weave-justify-md-end``
      let spaceBetween = cl Css.``weave-justify-md-space-between``
      let spaceAround = cl Css.``weave-justify-md-space-around``
      let spaceEvenly = cl Css.``weave-justify-md-space-evenly``

    module Large =

      let flexStart = cl Css.``weave-justify-lg-start``
      let center = cl Css.``weave-justify-lg-center``
      let flexEnd = cl Css.``weave-justify-lg-end``
      let spaceBetween = cl Css.``weave-justify-lg-space-between``
      let spaceAround = cl Css.``weave-justify-lg-space-around``
      let spaceEvenly = cl Css.``weave-justify-lg-space-evenly``

    module ExtraLarge =

      let flexStart = cl Css.``weave-justify-xl-start``
      let center = cl Css.``weave-justify-xl-center``
      let flexEnd = cl Css.``weave-justify-xl-end``
      let spaceBetween = cl Css.``weave-justify-xl-space-between``
      let spaceAround = cl Css.``weave-justify-xl-space-around``
      let spaceEvenly = cl Css.``weave-justify-xl-space-evenly``

    module ExtraExtraLarge =

      let flexStart = cl Css.``weave-justify-xxl-start``
      let center = cl Css.``weave-justify-xxl-center``
      let flexEnd = cl Css.``weave-justify-xxl-end``
      let spaceBetween = cl Css.``weave-justify-xxl-space-between``
      let spaceAround = cl Css.``weave-justify-xxl-space-around``
      let spaceEvenly = cl Css.``weave-justify-xxl-space-evenly``

  /// <summary>
  /// Controls how rows are positioned in a flex or grid container. Place on a flex or grid container.
  /// </summary>
  module AlignContent =

    let center = cl Css.``weave-align-content-center``
    let start = cl Css.``weave-align-content-start``
    let end' = cl Css.``weave-align-content-end``
    let spaceBetween = cl Css.``weave-align-content-space-between``
    let spaceAround = cl Css.``weave-align-content-space-around``
    let stretch = cl Css.``weave-align-content-stretch``

    module ExtraSmall =

      let center = cl Css.``weave-align-content-xs-center``
      let start = cl Css.``weave-align-content-xs-start``
      let end' = cl Css.``weave-align-content-xs-end``
      let spaceBetween = cl Css.``weave-align-content-xs-space-between``
      let spaceAround = cl Css.``weave-align-content-xs-space-around``
      let stretch = cl Css.``weave-align-content-xs-stretch``

    module Small =

      let center = cl Css.``weave-align-content-sm-center``
      let start = cl Css.``weave-align-content-sm-start``
      let end' = cl Css.``weave-align-content-sm-end``
      let spaceBetween = cl Css.``weave-align-content-sm-space-between``
      let spaceAround = cl Css.``weave-align-content-sm-space-around``
      let stretch = cl Css.``weave-align-content-sm-stretch``

    module Medium =

      let center = cl Css.``weave-align-content-md-center``
      let start = cl Css.``weave-align-content-md-start``
      let end' = cl Css.``weave-align-content-md-end``
      let spaceBetween = cl Css.``weave-align-content-md-space-between``
      let spaceAround = cl Css.``weave-align-content-md-space-around``
      let stretch = cl Css.``weave-align-content-md-stretch``

    module Large =

      let center = cl Css.``weave-align-content-lg-center``
      let start = cl Css.``weave-align-content-lg-start``
      let end' = cl Css.``weave-align-content-lg-end``
      let spaceBetween = cl Css.``weave-align-content-lg-space-between``
      let spaceAround = cl Css.``weave-align-content-lg-space-around``
      let stretch = cl Css.``weave-align-content-lg-stretch``

    module ExtraLarge =

      let center = cl Css.``weave-align-content-xl-center``
      let start = cl Css.``weave-align-content-xl-start``
      let end' = cl Css.``weave-align-content-xl-end``
      let spaceBetween = cl Css.``weave-align-content-xl-space-between``
      let spaceAround = cl Css.``weave-align-content-xl-space-around``
      let stretch = cl Css.``weave-align-content-xl-stretch``

    module ExtraExtraLarge =

      let center = cl Css.``weave-align-content-xxl-center``
      let start = cl Css.``weave-align-content-xxl-start``
      let end' = cl Css.``weave-align-content-xxl-end``
      let spaceBetween = cl Css.``weave-align-content-xxl-space-between``
      let spaceAround = cl Css.``weave-align-content-xxl-space-around``
      let stretch = cl Css.``weave-align-content-xxl-stretch``

  /// <summary>
  /// Controls how flex and grid items are positioned along a container's cross axis. Place on a flex or grid container.
  /// </summary>
  module AlignItems =

    let baseline = cl Css.``weave-align-baseline``
    let center = cl Css.``weave-align-center``
    let start = cl Css.``weave-align-start``
    let end' = cl Css.``weave-align-end``
    let stretch = cl Css.``weave-align-stretch``

    module ExtraSmall =

      let baseline = cl Css.``weave-align-xs-baseline``
      let center = cl Css.``weave-align-xs-center``
      let start = cl Css.``weave-align-xs-start``
      let end' = cl Css.``weave-align-xs-end``
      let stretch = cl Css.``weave-align-xs-stretch``

    module Small =

      let baseline = cl Css.``weave-align-sm-baseline``
      let center = cl Css.``weave-align-sm-center``
      let start = cl Css.``weave-align-sm-start``
      let end' = cl Css.``weave-align-sm-end``
      let stretch = cl Css.``weave-align-sm-stretch``

    module Medium =

      let baseline = cl Css.``weave-align-md-baseline``
      let center = cl Css.``weave-align-md-center``
      let start = cl Css.``weave-align-md-start``
      let end' = cl Css.``weave-align-md-end``
      let stretch = cl Css.``weave-align-md-stretch``

    module Large =

      let baseline = cl Css.``weave-align-lg-baseline``
      let center = cl Css.``weave-align-lg-center``
      let start = cl Css.``weave-align-lg-start``
      let end' = cl Css.``weave-align-lg-end``
      let stretch = cl Css.``weave-align-lg-stretch``

    module ExtraLarge =

      let baseline = cl Css.``weave-align-xl-baseline``
      let center = cl Css.``weave-align-xl-center``
      let start = cl Css.``weave-align-xl-start``
      let end' = cl Css.``weave-align-xl-end``
      let stretch = cl Css.``weave-align-xl-stretch``

    module ExtraExtraLarge =

      let baseline = cl Css.``weave-align-xxl-baseline``
      let center = cl Css.``weave-align-xxl-center``
      let start = cl Css.``weave-align-xxl-start``
      let end' = cl Css.``weave-align-xxl-end``
      let stretch = cl Css.``weave-align-xxl-stretch``

  /// <summary>
  /// Controls how an individual flex or grid item is positioned along its container's cross axis. Place on a flex or grid item.
  /// </summary>
  module AlignSelf =

    let auto = cl Css.``weave-align-self-auto``
    let center = cl Css.``weave-align-self-center``
    let start = cl Css.``weave-align-self-start``
    let end' = cl Css.``weave-align-self-end``
    let stretch = cl Css.``weave-align-self-stretch``

    module ExtraSmall =

      let auto = cl Css.``weave-align-self-xs-auto``
      let center = cl Css.``weave-align-self-xs-center``
      let start = cl Css.``weave-align-self-xs-start``
      let end' = cl Css.``weave-align-self-xs-end``
      let stretch = cl Css.``weave-align-self-xs-stretch``

    module Small =

      let auto = cl Css.``weave-align-self-sm-auto``
      let center = cl Css.``weave-align-self-sm-center``
      let start = cl Css.``weave-align-self-sm-start``
      let end' = cl Css.``weave-align-self-sm-end``
      let stretch = cl Css.``weave-align-self-sm-stretch``

    module Medium =

      let auto = cl Css.``weave-align-self-md-auto``
      let center = cl Css.``weave-align-self-md-center``
      let start = cl Css.``weave-align-self-md-start``
      let end' = cl Css.``weave-align-self-md-end``
      let stretch = cl Css.``weave-align-self-md-stretch``

    module Large =

      let auto = cl Css.``weave-align-self-lg-auto``
      let center = cl Css.``weave-align-self-lg-center``
      let start = cl Css.``weave-align-self-lg-start``
      let end' = cl Css.``weave-align-self-lg-end``
      let stretch = cl Css.``weave-align-self-lg-stretch``

    module ExtraLarge =

      let auto = cl Css.``weave-align-self-xl-auto``
      let center = cl Css.``weave-align-self-xl-center``
      let start = cl Css.``weave-align-self-xl-start``
      let end' = cl Css.``weave-align-self-xl-end``
      let stretch = cl Css.``weave-align-self-xl-stretch``

    module ExtraExtraLarge =

      let auto = cl Css.``weave-align-self-xxl-auto``
      let center = cl Css.``weave-align-self-xxl-center``
      let start = cl Css.``weave-align-self-xxl-start``
      let end' = cl Css.``weave-align-self-xxl-end``
      let stretch = cl Css.``weave-align-self-xxl-stretch``

  /// <summary>
  /// Controls element visibility across breakpoints for various display types.
  /// Use nested modules to select display type, then <c>HideOn</c> or <c>ShowOnly</c> for responsive behaviour.
  /// </summary>
  module Display =

    module Block =

      let always = cls [ Css.``weave-d-block`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-block`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-block`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-block`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-block`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-block`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-block``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [ Css.``weave-d-none``; Css.``weave-d-sm-block``; Css.``weave-d-md-none`` ]

        let md =
          cls [ Css.``weave-d-none``; Css.``weave-d-md-block``; Css.``weave-d-lg-none`` ]

        let lg =
          cls [ Css.``weave-d-none``; Css.``weave-d-lg-block``; Css.``weave-d-xl-none`` ]

        let xl =
          cls [ Css.``weave-d-none``; Css.``weave-d-xl-block``; Css.``weave-d-xxl-none`` ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-block`` ]

    module Flex =

      let always = cls [ Css.``weave-d-flex`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-flex`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-flex`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-flex`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-flex`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-flex`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-flex``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [ Css.``weave-d-none``; Css.``weave-d-sm-flex``; Css.``weave-d-md-none`` ]

        let md =
          cls [ Css.``weave-d-none``; Css.``weave-d-md-flex``; Css.``weave-d-lg-none`` ]

        let lg =
          cls [ Css.``weave-d-none``; Css.``weave-d-lg-flex``; Css.``weave-d-xl-none`` ]

        let xl =
          cls [ Css.``weave-d-none``; Css.``weave-d-xl-flex``; Css.``weave-d-xxl-none`` ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-flex`` ]

    module Inline =

      let always = cls [ Css.``weave-d-inline`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-inline`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-inline`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-inline`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-inline`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-inline`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-inline``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [ Css.``weave-d-none``; Css.``weave-d-sm-inline``; Css.``weave-d-md-none`` ]

        let md =
          cls [ Css.``weave-d-none``; Css.``weave-d-md-inline``; Css.``weave-d-lg-none`` ]

        let lg =
          cls [ Css.``weave-d-none``; Css.``weave-d-lg-inline``; Css.``weave-d-xl-none`` ]

        let xl =
          cls [ Css.``weave-d-none``; Css.``weave-d-xl-inline``; Css.``weave-d-xxl-none`` ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-inline`` ]

    module InlineBlock =

      let always = cls [ Css.``weave-d-inline-block`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-inline-block`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-inline-block`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-inline-block`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-inline-block`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-inline-block`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-inline-block``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-sm-inline-block``
            Css.``weave-d-md-none``
          ]

        let md =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-md-inline-block``
            Css.``weave-d-lg-none``
          ]

        let lg =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-lg-inline-block``
            Css.``weave-d-xl-none``
          ]

        let xl =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-xl-inline-block``
            Css.``weave-d-xxl-none``
          ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-inline-block`` ]

    module InlineFlex =

      let always = cls [ Css.``weave-d-inline-flex`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-inline-flex`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-inline-flex`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-inline-flex`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-inline-flex`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-inline-flex`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-inline-flex``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-sm-inline-flex``
            Css.``weave-d-md-none``
          ]

        let md =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-md-inline-flex``
            Css.``weave-d-lg-none``
          ]

        let lg =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-lg-inline-flex``
            Css.``weave-d-xl-none``
          ]

        let xl =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-xl-inline-flex``
            Css.``weave-d-xxl-none``
          ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-inline-flex`` ]

    module Table =

      let always = cls [ Css.``weave-d-table`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-table`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-table`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-table`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-table`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-table`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-table``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [ Css.``weave-d-none``; Css.``weave-d-sm-table``; Css.``weave-d-md-none`` ]

        let md =
          cls [ Css.``weave-d-none``; Css.``weave-d-md-table``; Css.``weave-d-lg-none`` ]

        let lg =
          cls [ Css.``weave-d-none``; Css.``weave-d-lg-table``; Css.``weave-d-xl-none`` ]

        let xl =
          cls [ Css.``weave-d-none``; Css.``weave-d-xl-table``; Css.``weave-d-xxl-none`` ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-table`` ]

    module TableRow =

      let always = cls [ Css.``weave-d-table-row`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-table-row`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-table-row`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-table-row`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-table-row`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-table-row`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-table-row``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [ Css.``weave-d-none``; Css.``weave-d-sm-table-row``; Css.``weave-d-md-none`` ]

        let md =
          cls [ Css.``weave-d-none``; Css.``weave-d-md-table-row``; Css.``weave-d-lg-none`` ]

        let lg =
          cls [ Css.``weave-d-none``; Css.``weave-d-lg-table-row``; Css.``weave-d-xl-none`` ]

        let xl =
          cls [ Css.``weave-d-none``; Css.``weave-d-xl-table-row``; Css.``weave-d-xxl-none`` ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-table-row`` ]

    module TableCell =

      let always = cls [ Css.``weave-d-table-cell`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-table-cell`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-table-cell`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-table-cell`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-table-cell`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-table-cell`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-table-cell``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [ Css.``weave-d-none``; Css.``weave-d-sm-table-cell``; Css.``weave-d-md-none`` ]

        let md =
          cls [ Css.``weave-d-none``; Css.``weave-d-md-table-cell``; Css.``weave-d-lg-none`` ]

        let lg =
          cls [ Css.``weave-d-none``; Css.``weave-d-lg-table-cell``; Css.``weave-d-xl-none`` ]

        let xl =
          cls [
            Css.``weave-d-none``
            Css.``weave-d-xl-table-cell``
            Css.``weave-d-xxl-none``
          ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-table-cell`` ]

    module Contents =

      let always = cls [ Css.``weave-d-contents`` ]
      let hidden = cls [ Css.``weave-d-none`` ]

      module HideOn =

        let xs = cls [ Css.``weave-d-none``; Css.``weave-d-sm-contents`` ]
        let sm = cls [ Css.``weave-d-sm-none``; Css.``weave-d-md-contents`` ]
        let md = cls [ Css.``weave-d-md-none``; Css.``weave-d-lg-contents`` ]
        let lg = cls [ Css.``weave-d-lg-none``; Css.``weave-d-xl-contents`` ]
        let xl = cls [ Css.``weave-d-xl-none``; Css.``weave-d-xxl-contents`` ]
        let xxl = cls [ Css.``weave-d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``weave-d-contents``; Css.``weave-d-sm-none`` ]

        let sm =
          cls [ Css.``weave-d-none``; Css.``weave-d-sm-contents``; Css.``weave-d-md-none`` ]

        let md =
          cls [ Css.``weave-d-none``; Css.``weave-d-md-contents``; Css.``weave-d-lg-none`` ]

        let lg =
          cls [ Css.``weave-d-none``; Css.``weave-d-lg-contents``; Css.``weave-d-xl-none`` ]

        let xl =
          cls [ Css.``weave-d-none``; Css.``weave-d-xl-contents``; Css.``weave-d-xxl-none`` ]

        let xxl = cls [ Css.``weave-d-none``; Css.``weave-d-xxl-contents`` ]

  module Overflow =

    let auto = cl Css.``weave-overflow-auto``
    let hidden = cl Css.``weave-overflow-hidden``
    let visible = cl Css.``weave-overflow-visible``
    let scroll = cl Css.``weave-overflow-scroll``
    let xAuto = cl Css.``weave-overflow-x-auto``
    let xHidden = cl Css.``weave-overflow-x-hidden``
    let xVisible = cl Css.``weave-overflow-x-visible``
    let xScroll = cl Css.``weave-overflow-x-scroll``
    let yAuto = cl Css.``weave-overflow-y-auto``
    let yHidden = cl Css.``weave-overflow-y-hidden``
    let yVisible = cl Css.``weave-overflow-y-visible``
    let yScroll = cl Css.``weave-overflow-y-scroll``

  /// <summary>
  /// Controls the gap (gutters) between rows and columns in flex and grid containers.
  /// Values are n * 4 pixels (e.g., g1 = 4px, g2 = 8px, g10 = 40px, g20 = 80px).
  /// </summary>
  module Gap =

    module All =

      let g0 = cl Css.``weave-gap-0``
      let g1 = cl Css.``weave-gap-1``
      let g2 = cl Css.``weave-gap-2``
      let g3 = cl Css.``weave-gap-3``
      let g4 = cl Css.``weave-gap-4``
      let g5 = cl Css.``weave-gap-5``
      let g6 = cl Css.``weave-gap-6``
      let g7 = cl Css.``weave-gap-7``
      let g8 = cl Css.``weave-gap-8``
      let g9 = cl Css.``weave-gap-9``
      let g10 = cl Css.``weave-gap-10``
      let g11 = cl Css.``weave-gap-11``
      let g12 = cl Css.``weave-gap-12``
      let g13 = cl Css.``weave-gap-13``
      let g14 = cl Css.``weave-gap-14``
      let g15 = cl Css.``weave-gap-15``
      let g16 = cl Css.``weave-gap-16``
      let g17 = cl Css.``weave-gap-17``
      let g18 = cl Css.``weave-gap-18``
      let g19 = cl Css.``weave-gap-19``
      let g20 = cl Css.``weave-gap-20``

      module Small =

        let g0 = cl Css.``weave-gap-sm-0``
        let g1 = cl Css.``weave-gap-sm-1``
        let g2 = cl Css.``weave-gap-sm-2``
        let g3 = cl Css.``weave-gap-sm-3``
        let g4 = cl Css.``weave-gap-sm-4``
        let g5 = cl Css.``weave-gap-sm-5``
        let g6 = cl Css.``weave-gap-sm-6``
        let g7 = cl Css.``weave-gap-sm-7``
        let g8 = cl Css.``weave-gap-sm-8``
        let g9 = cl Css.``weave-gap-sm-9``
        let g10 = cl Css.``weave-gap-sm-10``
        let g11 = cl Css.``weave-gap-sm-11``
        let g12 = cl Css.``weave-gap-sm-12``
        let g13 = cl Css.``weave-gap-sm-13``
        let g14 = cl Css.``weave-gap-sm-14``
        let g15 = cl Css.``weave-gap-sm-15``
        let g16 = cl Css.``weave-gap-sm-16``
        let g17 = cl Css.``weave-gap-sm-17``
        let g18 = cl Css.``weave-gap-sm-18``
        let g19 = cl Css.``weave-gap-sm-19``
        let g20 = cl Css.``weave-gap-sm-20``

      module Medium =

        let g0 = cl Css.``weave-gap-md-0``
        let g1 = cl Css.``weave-gap-md-1``
        let g2 = cl Css.``weave-gap-md-2``
        let g3 = cl Css.``weave-gap-md-3``
        let g4 = cl Css.``weave-gap-md-4``
        let g5 = cl Css.``weave-gap-md-5``
        let g6 = cl Css.``weave-gap-md-6``
        let g7 = cl Css.``weave-gap-md-7``
        let g8 = cl Css.``weave-gap-md-8``
        let g9 = cl Css.``weave-gap-md-9``
        let g10 = cl Css.``weave-gap-md-10``
        let g11 = cl Css.``weave-gap-md-11``
        let g12 = cl Css.``weave-gap-md-12``
        let g13 = cl Css.``weave-gap-md-13``
        let g14 = cl Css.``weave-gap-md-14``
        let g15 = cl Css.``weave-gap-md-15``
        let g16 = cl Css.``weave-gap-md-16``
        let g17 = cl Css.``weave-gap-md-17``
        let g18 = cl Css.``weave-gap-md-18``
        let g19 = cl Css.``weave-gap-md-19``
        let g20 = cl Css.``weave-gap-md-20``

    module X =

      let g0 = cl Css.``weave-gap-x-0``
      let g1 = cl Css.``weave-gap-x-1``
      let g2 = cl Css.``weave-gap-x-2``
      let g3 = cl Css.``weave-gap-x-3``
      let g4 = cl Css.``weave-gap-x-4``
      let g5 = cl Css.``weave-gap-x-5``
      let g6 = cl Css.``weave-gap-x-6``
      let g7 = cl Css.``weave-gap-x-7``
      let g8 = cl Css.``weave-gap-x-8``
      let g9 = cl Css.``weave-gap-x-9``
      let g10 = cl Css.``weave-gap-x-10``
      let g11 = cl Css.``weave-gap-x-11``
      let g12 = cl Css.``weave-gap-x-12``
      let g13 = cl Css.``weave-gap-x-13``
      let g14 = cl Css.``weave-gap-x-14``
      let g15 = cl Css.``weave-gap-x-15``
      let g16 = cl Css.``weave-gap-x-16``
      let g17 = cl Css.``weave-gap-x-17``
      let g18 = cl Css.``weave-gap-x-18``
      let g19 = cl Css.``weave-gap-x-19``
      let g20 = cl Css.``weave-gap-x-20``

      module Small =

        let g0 = cl Css.``weave-gap-x-sm-0``
        let g1 = cl Css.``weave-gap-x-sm-1``
        let g2 = cl Css.``weave-gap-x-sm-2``
        let g3 = cl Css.``weave-gap-x-sm-3``
        let g4 = cl Css.``weave-gap-x-sm-4``
        let g5 = cl Css.``weave-gap-x-sm-5``
        let g6 = cl Css.``weave-gap-x-sm-6``
        let g7 = cl Css.``weave-gap-x-sm-7``
        let g8 = cl Css.``weave-gap-x-sm-8``
        let g9 = cl Css.``weave-gap-x-sm-9``
        let g10 = cl Css.``weave-gap-x-sm-10``
        let g11 = cl Css.``weave-gap-x-sm-11``
        let g12 = cl Css.``weave-gap-x-sm-12``
        let g13 = cl Css.``weave-gap-x-sm-13``
        let g14 = cl Css.``weave-gap-x-sm-14``
        let g15 = cl Css.``weave-gap-x-sm-15``
        let g16 = cl Css.``weave-gap-x-sm-16``
        let g17 = cl Css.``weave-gap-x-sm-17``
        let g18 = cl Css.``weave-gap-x-sm-18``
        let g19 = cl Css.``weave-gap-x-sm-19``
        let g20 = cl Css.``weave-gap-x-sm-20``

      module Medium =

        let g0 = cl Css.``weave-gap-x-md-0``
        let g1 = cl Css.``weave-gap-x-md-1``
        let g2 = cl Css.``weave-gap-x-md-2``
        let g3 = cl Css.``weave-gap-x-md-3``
        let g4 = cl Css.``weave-gap-x-md-4``
        let g5 = cl Css.``weave-gap-x-md-5``
        let g6 = cl Css.``weave-gap-x-md-6``
        let g7 = cl Css.``weave-gap-x-md-7``
        let g8 = cl Css.``weave-gap-x-md-8``
        let g9 = cl Css.``weave-gap-x-md-9``
        let g10 = cl Css.``weave-gap-x-md-10``
        let g11 = cl Css.``weave-gap-x-md-11``
        let g12 = cl Css.``weave-gap-x-md-12``
        let g13 = cl Css.``weave-gap-x-md-13``
        let g14 = cl Css.``weave-gap-x-md-14``
        let g15 = cl Css.``weave-gap-x-md-15``
        let g16 = cl Css.``weave-gap-x-md-16``
        let g17 = cl Css.``weave-gap-x-md-17``
        let g18 = cl Css.``weave-gap-x-md-18``
        let g19 = cl Css.``weave-gap-x-md-19``
        let g20 = cl Css.``weave-gap-x-md-20``

    module Y =

      let g0 = cl Css.``weave-gap-y-0``
      let g1 = cl Css.``weave-gap-y-1``
      let g2 = cl Css.``weave-gap-y-2``
      let g3 = cl Css.``weave-gap-y-3``
      let g4 = cl Css.``weave-gap-y-4``
      let g5 = cl Css.``weave-gap-y-5``
      let g6 = cl Css.``weave-gap-y-6``
      let g7 = cl Css.``weave-gap-y-7``
      let g8 = cl Css.``weave-gap-y-8``
      let g9 = cl Css.``weave-gap-y-9``
      let g10 = cl Css.``weave-gap-y-10``
      let g11 = cl Css.``weave-gap-y-11``
      let g12 = cl Css.``weave-gap-y-12``
      let g13 = cl Css.``weave-gap-y-13``
      let g14 = cl Css.``weave-gap-y-14``
      let g15 = cl Css.``weave-gap-y-15``
      let g16 = cl Css.``weave-gap-y-16``
      let g17 = cl Css.``weave-gap-y-17``
      let g18 = cl Css.``weave-gap-y-18``
      let g19 = cl Css.``weave-gap-y-19``
      let g20 = cl Css.``weave-gap-y-20``

      module Small =

        let g0 = cl Css.``weave-gap-y-sm-0``
        let g1 = cl Css.``weave-gap-y-sm-1``
        let g2 = cl Css.``weave-gap-y-sm-2``
        let g3 = cl Css.``weave-gap-y-sm-3``
        let g4 = cl Css.``weave-gap-y-sm-4``
        let g5 = cl Css.``weave-gap-y-sm-5``
        let g6 = cl Css.``weave-gap-y-sm-6``
        let g7 = cl Css.``weave-gap-y-sm-7``
        let g8 = cl Css.``weave-gap-y-sm-8``
        let g9 = cl Css.``weave-gap-y-sm-9``
        let g10 = cl Css.``weave-gap-y-sm-10``
        let g11 = cl Css.``weave-gap-y-sm-11``
        let g12 = cl Css.``weave-gap-y-sm-12``
        let g13 = cl Css.``weave-gap-y-sm-13``
        let g14 = cl Css.``weave-gap-y-sm-14``
        let g15 = cl Css.``weave-gap-y-sm-15``
        let g16 = cl Css.``weave-gap-y-sm-16``
        let g17 = cl Css.``weave-gap-y-sm-17``
        let g18 = cl Css.``weave-gap-y-sm-18``
        let g19 = cl Css.``weave-gap-y-sm-19``
        let g20 = cl Css.``weave-gap-y-sm-20``

      module Medium =

        let g0 = cl Css.``weave-gap-y-md-0``
        let g1 = cl Css.``weave-gap-y-md-1``
        let g2 = cl Css.``weave-gap-y-md-2``
        let g3 = cl Css.``weave-gap-y-md-3``
        let g4 = cl Css.``weave-gap-y-md-4``
        let g5 = cl Css.``weave-gap-y-md-5``
        let g6 = cl Css.``weave-gap-y-md-6``
        let g7 = cl Css.``weave-gap-y-md-7``
        let g8 = cl Css.``weave-gap-y-md-8``
        let g9 = cl Css.``weave-gap-y-md-9``
        let g10 = cl Css.``weave-gap-y-md-10``
        let g11 = cl Css.``weave-gap-y-md-11``
        let g12 = cl Css.``weave-gap-y-md-12``
        let g13 = cl Css.``weave-gap-y-md-13``
        let g14 = cl Css.``weave-gap-y-md-14``
        let g15 = cl Css.``weave-gap-y-md-15``
        let g16 = cl Css.``weave-gap-y-md-16``
        let g17 = cl Css.``weave-gap-y-md-17``
        let g18 = cl Css.``weave-gap-y-md-18``
        let g19 = cl Css.``weave-gap-y-md-19``
        let g20 = cl Css.``weave-gap-y-md-20``

  /// <summary>
  /// Controls the order of flex and grid items. Place on a flex or grid item.
  /// </summary>
  module Order =

    let first = cl Css.``weave-order-first``
    let last = cl Css.``weave-order-last``
    let o0 = cl Css.``weave-order-0``
    let o1 = cl Css.``weave-order-1``
    let o2 = cl Css.``weave-order-2``
    let o3 = cl Css.``weave-order-3``
    let o4 = cl Css.``weave-order-4``
    let o5 = cl Css.``weave-order-5``
    let o6 = cl Css.``weave-order-6``
    let o7 = cl Css.``weave-order-7``
    let o8 = cl Css.``weave-order-8``
    let o9 = cl Css.``weave-order-9``
    let o10 = cl Css.``weave-order-10``
    let o11 = cl Css.``weave-order-11``
    let o12 = cl Css.``weave-order-12``
