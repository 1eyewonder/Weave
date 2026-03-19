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

      let allSizes = cl Css.``d-flex``
      let xs = cl Css.``d-xs-flex``
      let sm = cl Css.``d-sm-flex``
      let md = cl Css.``d-md-flex``
      let lg = cl Css.``d-lg-flex``
      let xl = cl Css.``d-xl-flex``
      let xxl = cl Css.``d-xxl-flex``

    module Inline =

      let allSizes = cl Css.``d-inline-flex``
      let xs = cl Css.``d-xs-inline-flex``
      let sm = cl Css.``d-sm-inline-flex``
      let md = cl Css.``d-md-inline-flex``
      let lg = cl Css.``d-lg-inline-flex``
      let xl = cl Css.``d-xl-inline-flex``
      let xxl = cl Css.``d-xxl-inline-flex``

    module InlineBlock =

      let allSizes = cl Css.``d-inline-block``
      let xs = cl Css.``d-xs-inline-block``
      let sm = cl Css.``d-sm-inline-block``
      let md = cl Css.``d-md-inline-block``
      let lg = cl Css.``d-lg-inline-block``
      let xl = cl Css.``d-xl-inline-block``
      let xxl = cl Css.``d-xxl-inline-block``

  /// <summary>
  /// Controls the wrap of flex items. Place on a flex container.
  /// </summary>
  module FlexWrap =

    module NoWrap =

      let allSizes = cl Css.``flex-nowrap``
      let xs = cl Css.``flex-xs-nowrap``
      let sm = cl Css.``flex-sm-nowrap``
      let md = cl Css.``flex-md-nowrap``
      let lg = cl Css.``flex-lg-nowrap``
      let xl = cl Css.``flex-xl-nowrap``
      let xxl = cl Css.``flex-xxl-nowrap``

    module Wrap =

      let allSizes = cl Css.``flex-wrap``
      let xs = cl Css.``flex-xs-wrap``
      let sm = cl Css.``flex-sm-wrap``
      let md = cl Css.``flex-md-wrap``
      let lg = cl Css.``flex-lg-wrap``
      let xl = cl Css.``flex-xl-wrap``
      let xxl = cl Css.``flex-xxl-wrap``

    module WrapReverse =

      let allSizes = cl Css.``flex-wrap-reverse``
      let xs = cl Css.``flex-xs-wrap-reverse``
      let sm = cl Css.``flex-sm-wrap-reverse``
      let md = cl Css.``flex-md-wrap-reverse``
      let lg = cl Css.``flex-lg-wrap-reverse``
      let xl = cl Css.``flex-xl-wrap-reverse``
      let xxl = cl Css.``flex-xxl-wrap-reverse``

  /// <summary>
  /// Controls the direction of flex items. Place on a flex container.
  /// </summary>
  module FlexDirection =

    module Row =

      let allSizes = cl Css.``flex-row``
      let xs = cl Css.``flex-xs-row``
      let sm = cl Css.``flex-sm-row``
      let md = cl Css.``flex-md-row``
      let lg = cl Css.``flex-lg-row``
      let xl = cl Css.``flex-xl-row``
      let xxl = cl Css.``flex-xxl-row``

    module RowReverse =

      let allSizes = cl Css.``flex-row-reverse``
      let xs = cl Css.``flex-xs-row-reverse``
      let sm = cl Css.``flex-sm-row-reverse``
      let md = cl Css.``flex-md-row-reverse``
      let lg = cl Css.``flex-lg-row-reverse``
      let xl = cl Css.``flex-xl-row-reverse``
      let xxl = cl Css.``flex-xxl-row-reverse``

    module Column =

      let allSizes = cl Css.``flex-column``
      let xs = cl Css.``flex-xs-column``
      let sm = cl Css.``flex-sm-column``
      let md = cl Css.``flex-md-column``
      let lg = cl Css.``flex-lg-column``
      let xl = cl Css.``flex-xl-column``
      let xxl = cl Css.``flex-xxl-column``

    module ColumnReverse =

      let allSizes = cl Css.``flex-column-reverse``
      let xs = cl Css.``flex-xs-column-reverse``
      let sm = cl Css.``flex-sm-column-reverse``
      let md = cl Css.``flex-md-column-reverse``
      let lg = cl Css.``flex-lg-column-reverse``
      let xl = cl Css.``flex-xl-column-reverse``
      let xxl = cl Css.``flex-xxl-column-reverse``

  /// <summary>
  /// Utilities for controlling how flex items both grow and shrink. Place on items within a flex container.
  /// </summary>
  module FlexItem =

    module Flex =

      let allSizes = cl Css.``flex-1``
      let xs = cl Css.``flex-xs-1``
      let sm = cl Css.``flex-sm-1``
      let md = cl Css.``flex-md-1``
      let lg = cl Css.``flex-lg-1``
      let xl = cl Css.``flex-xl-1``
      let xxl = cl Css.``flex-xxl-1``

    module Auto =

      let allSizes = cl Css.``flex-auto``
      let xs = cl Css.``flex-xs-auto``
      let sm = cl Css.``flex-sm-auto``
      let md = cl Css.``flex-md-auto``
      let lg = cl Css.``flex-lg-auto``
      let xl = cl Css.``flex-xl-auto``
      let xxl = cl Css.``flex-xxl-auto``

    module Initial =

      let allSizes = cl Css.``flex-initial``
      let xs = cl Css.``flex-xs-initial``
      let sm = cl Css.``flex-sm-initial``
      let md = cl Css.``flex-md-initial``
      let lg = cl Css.``flex-lg-initial``
      let xl = cl Css.``flex-xl-initial``
      let xxl = cl Css.``flex-xxl-initial``

    module Grow =

      let allSizes = cl Css.``flex-grow-1``
      let xs = cl Css.``flex-xs-grow-1``
      let sm = cl Css.``flex-sm-grow-1``
      let md = cl Css.``flex-md-grow-1``
      let lg = cl Css.``flex-lg-grow-1``
      let xl = cl Css.``flex-xl-grow-1``
      let xxl = cl Css.``flex-xxl-grow-1``

    module NoGrow =

      let allSizes = cl Css.``flex-grow-0``
      let xs = cl Css.``flex-xs-grow-0``
      let sm = cl Css.``flex-sm-grow-0``
      let md = cl Css.``flex-md-grow-0``
      let lg = cl Css.``flex-lg-grow-0``
      let xl = cl Css.``flex-xl-grow-0``
      let xxl = cl Css.``flex-xxl-grow-0``

    module Shrink =

      let allSizes = cl Css.``flex-shrink-1``
      let xs = cl Css.``flex-xs-shrink-1``
      let sm = cl Css.``flex-sm-shrink-1``
      let md = cl Css.``flex-md-shrink-1``
      let lg = cl Css.``flex-lg-shrink-1``
      let xl = cl Css.``flex-xl-shrink-1``
      let xxl = cl Css.``flex-xxl-shrink-1``

    module NoShrink =

      let allSizes = cl Css.``flex-shrink-0``
      let xs = cl Css.``flex-xs-shrink-0``
      let sm = cl Css.``flex-sm-shrink-0``
      let md = cl Css.``flex-md-shrink-0``
      let lg = cl Css.``flex-lg-shrink-0``
      let xl = cl Css.``flex-xl-shrink-0``
      let xxl = cl Css.``flex-xxl-shrink-0``

    module None =

      let allSizes = cl Css.``flex-none``
      let xs = cl Css.``flex-xs-none``
      let sm = cl Css.``flex-sm-none``
      let md = cl Css.``flex-md-none``
      let lg = cl Css.``flex-lg-none``
      let xl = cl Css.``flex-xl-none``
      let xxl = cl Css.``flex-xxl-none``

  /// <summary>
  /// Controls how flex and grid items are positioned along a container's main axis. Place on a flex or grid container.
  /// </summary>
  module JustifyContent =

    let flexStart = cl Css.``justify-start``
    let center = cl Css.``justify-center``
    let flexEnd = cl Css.``justify-end``
    let spaceBetween = cl Css.``justify-space-between``
    let spaceAround = cl Css.``justify-space-around``
    let spaceEvenly = cl Css.``justify-space-evenly``

  /// <summary>
  /// Controls how rows are positioned in a flex or grid container. Place on a flex or grid container.
  /// </summary>
  module AlignContent =

    let center = cl Css.``align-content-center``
    let start = cl Css.``align-content-start``
    let end' = cl Css.``align-content-end``
    let spaceBetween = cl Css.``align-content-space-between``
    let spaceAround = cl Css.``align-content-space-around``
    let stretch = cl Css.``align-content-stretch``

  /// <summary>
  /// Controls how flex and grid items are positioned along a container's cross axis. Place on a flex or grid container.
  /// </summary>
  module AlignItems =

    let baseline = cl Css.``align-baseline``
    let center = cl Css.``align-center``
    let start = cl Css.``align-start``
    let end' = cl Css.``align-end``
    let stretch = cl Css.``align-stretch``

  /// <summary>
  /// Controls how an individual flex or grid item is positioned along its container's cross axis. Place on a flex or grid item.
  /// </summary>
  module AlignSelf =

    let auto = cl Css.``align-self-auto``
    let center = cl Css.``align-self-center``
    let start = cl Css.``align-self-start``
    let end' = cl Css.``align-self-end``
    let stretch = cl Css.``align-self-stretch``

  /// <summary>
  /// Controls element visibility across breakpoints for various display types.
  /// Use nested modules to select display type, then <c>HideOn</c> or <c>ShowOnly</c> for responsive behaviour.
  /// </summary>
  module Display =

    module Block =

      let always = cls [ Css.``d-block`` ]
      let hidden = cls [ Css.``d-none`` ]

      module HideOn =

        let xs = cls [ Css.``d-none``; Css.``d-sm-block`` ]
        let sm = cls [ Css.``d-sm-none``; Css.``d-md-block`` ]
        let md = cls [ Css.``d-md-none``; Css.``d-lg-block`` ]
        let lg = cls [ Css.``d-lg-none``; Css.``d-xl-block`` ]
        let xl = cls [ Css.``d-xl-none``; Css.``d-xxl-block`` ]
        let xxl = cls [ Css.``d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``d-block``; Css.``d-sm-none`` ]
        let sm = cls [ Css.``d-none``; Css.``d-sm-block``; Css.``d-md-none`` ]
        let md = cls [ Css.``d-none``; Css.``d-md-block``; Css.``d-lg-none`` ]
        let lg = cls [ Css.``d-none``; Css.``d-lg-block``; Css.``d-xl-none`` ]
        let xl = cls [ Css.``d-none``; Css.``d-xl-block``; Css.``d-xxl-none`` ]
        let xxl = cls [ Css.``d-none``; Css.``d-xxl-block`` ]

    module Flex =

      let always = cls [ Css.``d-flex`` ]
      let hidden = cls [ Css.``d-none`` ]

      module HideOn =

        let xs = cls [ Css.``d-none``; Css.``d-sm-flex`` ]
        let sm = cls [ Css.``d-sm-none``; Css.``d-md-flex`` ]
        let md = cls [ Css.``d-md-none``; Css.``d-lg-flex`` ]
        let lg = cls [ Css.``d-lg-none``; Css.``d-xl-flex`` ]
        let xl = cls [ Css.``d-xl-none``; Css.``d-xxl-flex`` ]
        let xxl = cls [ Css.``d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``d-flex``; Css.``d-sm-none`` ]
        let sm = cls [ Css.``d-none``; Css.``d-sm-flex``; Css.``d-md-none`` ]
        let md = cls [ Css.``d-none``; Css.``d-md-flex``; Css.``d-lg-none`` ]
        let lg = cls [ Css.``d-none``; Css.``d-lg-flex``; Css.``d-xl-none`` ]
        let xl = cls [ Css.``d-none``; Css.``d-xl-flex``; Css.``d-xxl-none`` ]
        let xxl = cls [ Css.``d-none``; Css.``d-xxl-flex`` ]

    module Inline =

      let always = cls [ Css.``d-inline`` ]
      let hidden = cls [ Css.``d-none`` ]

      module HideOn =

        let xs = cls [ Css.``d-none``; Css.``d-sm-inline`` ]
        let sm = cls [ Css.``d-sm-none``; Css.``d-md-inline`` ]
        let md = cls [ Css.``d-md-none``; Css.``d-lg-inline`` ]
        let lg = cls [ Css.``d-lg-none``; Css.``d-xl-inline`` ]
        let xl = cls [ Css.``d-xl-none``; Css.``d-xxl-inline`` ]
        let xxl = cls [ Css.``d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``d-inline``; Css.``d-sm-none`` ]
        let sm = cls [ Css.``d-none``; Css.``d-sm-inline``; Css.``d-md-none`` ]
        let md = cls [ Css.``d-none``; Css.``d-md-inline``; Css.``d-lg-none`` ]
        let lg = cls [ Css.``d-none``; Css.``d-lg-inline``; Css.``d-xl-none`` ]
        let xl = cls [ Css.``d-none``; Css.``d-xl-inline``; Css.``d-xxl-none`` ]
        let xxl = cls [ Css.``d-none``; Css.``d-xxl-inline`` ]

    module InlineBlock =

      let always = cls [ Css.``d-inline-block`` ]
      let hidden = cls [ Css.``d-none`` ]

      module HideOn =

        let xs = cls [ Css.``d-none``; Css.``d-sm-inline-block`` ]
        let sm = cls [ Css.``d-sm-none``; Css.``d-md-inline-block`` ]
        let md = cls [ Css.``d-md-none``; Css.``d-lg-inline-block`` ]
        let lg = cls [ Css.``d-lg-none``; Css.``d-xl-inline-block`` ]
        let xl = cls [ Css.``d-xl-none``; Css.``d-xxl-inline-block`` ]
        let xxl = cls [ Css.``d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``d-inline-block``; Css.``d-sm-none`` ]
        let sm = cls [ Css.``d-none``; Css.``d-sm-inline-block``; Css.``d-md-none`` ]
        let md = cls [ Css.``d-none``; Css.``d-md-inline-block``; Css.``d-lg-none`` ]
        let lg = cls [ Css.``d-none``; Css.``d-lg-inline-block``; Css.``d-xl-none`` ]
        let xl = cls [ Css.``d-none``; Css.``d-xl-inline-block``; Css.``d-xxl-none`` ]
        let xxl = cls [ Css.``d-none``; Css.``d-xxl-inline-block`` ]

    module InlineFlex =

      let always = cls [ Css.``d-inline-flex`` ]
      let hidden = cls [ Css.``d-none`` ]

      module HideOn =

        let xs = cls [ Css.``d-none``; Css.``d-sm-inline-flex`` ]
        let sm = cls [ Css.``d-sm-none``; Css.``d-md-inline-flex`` ]
        let md = cls [ Css.``d-md-none``; Css.``d-lg-inline-flex`` ]
        let lg = cls [ Css.``d-lg-none``; Css.``d-xl-inline-flex`` ]
        let xl = cls [ Css.``d-xl-none``; Css.``d-xxl-inline-flex`` ]
        let xxl = cls [ Css.``d-xxl-none`` ]

      module ShowOnly =

        let xs = cls [ Css.``d-inline-flex``; Css.``d-sm-none`` ]
        let sm = cls [ Css.``d-none``; Css.``d-sm-inline-flex``; Css.``d-md-none`` ]
        let md = cls [ Css.``d-none``; Css.``d-md-inline-flex``; Css.``d-lg-none`` ]
        let lg = cls [ Css.``d-none``; Css.``d-lg-inline-flex``; Css.``d-xl-none`` ]
        let xl = cls [ Css.``d-none``; Css.``d-xl-inline-flex``; Css.``d-xxl-none`` ]
        let xxl = cls [ Css.``d-none``; Css.``d-xxl-inline-flex`` ]

  module Overflow =

    let auto = cl Css.``overflow-auto``
    let hidden = cl Css.``overflow-hidden``
    let visible = cl Css.``overflow-visible``
    let scroll = cl Css.``overflow-scroll``
    let xAuto = cl Css.``overflow-x-auto``
    let xHidden = cl Css.``overflow-x-hidden``
    let yAuto = cl Css.``overflow-y-auto``
    let yHidden = cl Css.``overflow-y-hidden``

  /// <summary>
  /// Specifies how a replaced element's content should be resized
  /// </summary>
  module ObjectFit =

    let fill = cl Css.``object-fill``
    let contain = cl Css.``object-contain``
    let cover = cl Css.``object-cover``
    let none = cl Css.``object-none``
    let scaleDown = cl Css.``object-scale-down``

  /// <summary>
  /// Specifies how a replaced element's content should be positioned within its container
  /// </summary>
  module ObjectPosition =

    let bottom = cl Css.``object-bottom``
    let center = cl Css.``object-center``
    let left = cl Css.``object-left``
    let leftBottom = cl Css.``object-left-bottom``
    let leftTop = cl Css.``object-left-top``
    let right = cl Css.``object-right``
    let rightBottom = cl Css.``object-right-bottom``
    let rightTop = cl Css.``object-right-top``

  /// <summary>
  /// Utilities for controlling how an element is positioned in the DOM
  /// </summary>
  module Position =

    let static' = cl Css.``static``
    let relative = cl Css.``relative``
    let absolute = cl Css.``absolute``
    let fixed' = cl Css.``fixed``
    let sticky = cl Css.``sticky``
