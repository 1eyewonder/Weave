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

  [<RequireQualifiedAccess; Struct>]
  type Flex =
    | Flex
    | Inline
    | InlineBlock

  /// <summary>
  /// Controls if a container is a flex box
  /// </summary>
  module Flex =

    let toClass breakpoint flex =
      match breakpoint, flex with
      | None, Flex.Flex -> Css.``d-flex``
      | None, Flex.Inline -> Css.``d-inline-flex``
      | None, Flex.InlineBlock -> Css.``d-inline-block``
      | Some Breakpoint.ExtraSmall, Flex.Flex -> Css.``d-xs-flex``
      | Some Breakpoint.ExtraSmall, Flex.Inline -> Css.``d-xs-inline-flex``
      | Some Breakpoint.ExtraSmall, Flex.InlineBlock -> Css.``d-xs-inline-block``
      | Some Breakpoint.Small, Flex.Flex -> Css.``d-sm-flex``
      | Some Breakpoint.Small, Flex.Inline -> Css.``d-sm-inline-flex``
      | Some Breakpoint.Small, Flex.InlineBlock -> Css.``d-sm-inline-block``
      | Some Breakpoint.Medium, Flex.Flex -> Css.``d-md-flex``
      | Some Breakpoint.Medium, Flex.Inline -> Css.``d-md-inline-flex``
      | Some Breakpoint.Medium, Flex.InlineBlock -> Css.``d-md-inline-block``
      | Some Breakpoint.Large, Flex.Flex -> Css.``d-lg-flex``
      | Some Breakpoint.Large, Flex.Inline -> Css.``d-lg-inline-flex``
      | Some Breakpoint.Large, Flex.InlineBlock -> Css.``d-lg-inline-block``
      | Some Breakpoint.ExtraLarge, Flex.Flex -> Css.``d-xl-flex``
      | Some Breakpoint.ExtraLarge, Flex.Inline -> Css.``d-xl-inline-flex``
      | Some Breakpoint.ExtraLarge, Flex.InlineBlock -> Css.``d-xl-inline-block``
      | Some Breakpoint.ExtraExtraLarge, Flex.Flex -> Css.``d-xxl-flex``
      | Some Breakpoint.ExtraExtraLarge, Flex.Inline -> Css.``d-xxl-inline-flex``
      | Some Breakpoint.ExtraExtraLarge, Flex.InlineBlock -> Css.``d-xxl-inline-block``

    module Flex =

      let allSizes = toClass None Flex.Flex
      let xs = toClass (Some Breakpoint.ExtraSmall) Flex.Flex
      let sm = toClass (Some Breakpoint.Small) Flex.Flex
      let md = toClass (Some Breakpoint.Medium) Flex.Flex
      let lg = toClass (Some Breakpoint.Large) Flex.Flex
      let xl = toClass (Some Breakpoint.ExtraLarge) Flex.Flex
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) Flex.Flex

    module Inline =

      let allSizes = toClass None Flex.Inline
      let xs = toClass (Some Breakpoint.ExtraSmall) Flex.Inline
      let sm = toClass (Some Breakpoint.Small) Flex.Inline
      let md = toClass (Some Breakpoint.Medium) Flex.Inline
      let lg = toClass (Some Breakpoint.Large) Flex.Inline
      let xl = toClass (Some Breakpoint.ExtraLarge) Flex.Inline
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) Flex.Inline

    module InlineBlock =

      let allSizes = toClass None Flex.InlineBlock
      let xs = toClass (Some Breakpoint.ExtraSmall) Flex.InlineBlock
      let sm = toClass (Some Breakpoint.Small) Flex.InlineBlock
      let md = toClass (Some Breakpoint.Medium) Flex.InlineBlock
      let lg = toClass (Some Breakpoint.Large) Flex.InlineBlock
      let xl = toClass (Some Breakpoint.ExtraLarge) Flex.InlineBlock
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) Flex.InlineBlock

  [<RequireQualifiedAccess; Struct>]
  type FlexWrap =
    | NoWrap
    | Wrap
    | WrapReverse

  /// <summary>
  /// Controls the wrap of flex items. Place on a flex container.
  /// </summary>
  module FlexWrap =

    let toClass breakpoint wrap =
      match breakpoint, wrap with
      | None, FlexWrap.NoWrap -> Css.``flex-nowrap``
      | None, FlexWrap.Wrap -> Css.``flex-wrap``
      | None, FlexWrap.WrapReverse -> Css.``flex-wrap-reverse``
      | Some Breakpoint.ExtraSmall, FlexWrap.NoWrap -> Css.``flex-xs-nowrap``
      | Some Breakpoint.ExtraSmall, FlexWrap.Wrap -> Css.``flex-xs-wrap``
      | Some Breakpoint.ExtraSmall, FlexWrap.WrapReverse -> Css.``flex-xs-wrap-reverse``
      | Some Breakpoint.Small, FlexWrap.NoWrap -> Css.``flex-sm-nowrap``
      | Some Breakpoint.Small, FlexWrap.Wrap -> Css.``flex-sm-wrap``
      | Some Breakpoint.Small, FlexWrap.WrapReverse -> Css.``flex-sm-wrap-reverse``
      | Some Breakpoint.Medium, FlexWrap.NoWrap -> Css.``flex-md-nowrap``
      | Some Breakpoint.Medium, FlexWrap.Wrap -> Css.``flex-md-wrap``
      | Some Breakpoint.Medium, FlexWrap.WrapReverse -> Css.``flex-md-wrap-reverse``
      | Some Breakpoint.Large, FlexWrap.NoWrap -> Css.``flex-lg-nowrap``
      | Some Breakpoint.Large, FlexWrap.Wrap -> Css.``flex-lg-wrap``
      | Some Breakpoint.Large, FlexWrap.WrapReverse -> Css.``flex-lg-wrap-reverse``
      | Some Breakpoint.ExtraLarge, FlexWrap.NoWrap -> Css.``flex-xl-nowrap``
      | Some Breakpoint.ExtraLarge, FlexWrap.Wrap -> Css.``flex-xl-wrap``
      | Some Breakpoint.ExtraLarge, FlexWrap.WrapReverse -> Css.``flex-xl-wrap-reverse``
      | Some Breakpoint.ExtraExtraLarge, FlexWrap.NoWrap -> Css.``flex-xxl-nowrap``
      | Some Breakpoint.ExtraExtraLarge, FlexWrap.Wrap -> Css.``flex-xxl-wrap``
      | Some Breakpoint.ExtraExtraLarge, FlexWrap.WrapReverse -> Css.``flex-xxl-wrap-reverse``

    module NoWrap =

      let allSizes = toClass None FlexWrap.NoWrap
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexWrap.NoWrap
      let sm = toClass (Some Breakpoint.Small) FlexWrap.NoWrap
      let md = toClass (Some Breakpoint.Medium) FlexWrap.NoWrap
      let lg = toClass (Some Breakpoint.Large) FlexWrap.NoWrap
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexWrap.NoWrap
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexWrap.NoWrap

    module Wrap =

      let allSizes = toClass None FlexWrap.Wrap
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexWrap.Wrap
      let sm = toClass (Some Breakpoint.Small) FlexWrap.Wrap
      let md = toClass (Some Breakpoint.Medium) FlexWrap.Wrap
      let lg = toClass (Some Breakpoint.Large) FlexWrap.Wrap
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexWrap.Wrap
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexWrap.Wrap

    module WrapReverse =

      let allSizes = toClass None FlexWrap.WrapReverse
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexWrap.WrapReverse
      let sm = toClass (Some Breakpoint.Small) FlexWrap.WrapReverse
      let md = toClass (Some Breakpoint.Medium) FlexWrap.WrapReverse
      let lg = toClass (Some Breakpoint.Large) FlexWrap.WrapReverse
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexWrap.WrapReverse
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexWrap.WrapReverse

  [<RequireQualifiedAccess; Struct>]
  type FlexDirection =
    | Row
    | RowReverse
    | Column
    | ColumnReverse

  /// <summary>
  /// Controls the direction of flex items. Place on a flex container.
  /// </summary>
  module FlexDirection =

    let toClass breakpoint direction =
      match breakpoint, direction with
      | None, FlexDirection.Row -> Css.``flex-row``
      | None, FlexDirection.RowReverse -> Css.``flex-row-reverse``
      | None, FlexDirection.Column -> Css.``flex-column``
      | None, FlexDirection.ColumnReverse -> Css.``flex-column-reverse``
      | Some Breakpoint.ExtraSmall, FlexDirection.Row -> Css.``flex-xs-row``
      | Some Breakpoint.ExtraSmall, FlexDirection.RowReverse -> Css.``flex-xs-row-reverse``
      | Some Breakpoint.ExtraSmall, FlexDirection.Column -> Css.``flex-xs-column``
      | Some Breakpoint.ExtraSmall, FlexDirection.ColumnReverse -> Css.``flex-xs-column-reverse``
      | Some Breakpoint.Small, FlexDirection.Row -> Css.``flex-sm-row``
      | Some Breakpoint.Small, FlexDirection.RowReverse -> Css.``flex-sm-row-reverse``
      | Some Breakpoint.Small, FlexDirection.Column -> Css.``flex-sm-column``
      | Some Breakpoint.Small, FlexDirection.ColumnReverse -> Css.``flex-sm-column-reverse``
      | Some Breakpoint.Medium, FlexDirection.Row -> Css.``flex-md-row``
      | Some Breakpoint.Medium, FlexDirection.RowReverse -> Css.``flex-md-row-reverse``
      | Some Breakpoint.Medium, FlexDirection.Column -> Css.``flex-md-column``
      | Some Breakpoint.Medium, FlexDirection.ColumnReverse -> Css.``flex-md-column-reverse``
      | Some Breakpoint.Large, FlexDirection.Row -> Css.``flex-lg-row``
      | Some Breakpoint.Large, FlexDirection.RowReverse -> Css.``flex-lg-row-reverse``
      | Some Breakpoint.Large, FlexDirection.Column -> Css.``flex-lg-column``
      | Some Breakpoint.Large, FlexDirection.ColumnReverse -> Css.``flex-lg-column-reverse``
      | Some Breakpoint.ExtraLarge, FlexDirection.Row -> Css.``flex-xl-row``
      | Some Breakpoint.ExtraLarge, FlexDirection.RowReverse -> Css.``flex-xl-row-reverse``
      | Some Breakpoint.ExtraLarge, FlexDirection.Column -> Css.``flex-xl-column``
      | Some Breakpoint.ExtraLarge, FlexDirection.ColumnReverse -> Css.``flex-xl-column-reverse``
      | Some Breakpoint.ExtraExtraLarge, FlexDirection.Row -> Css.``flex-xxl-row``
      | Some Breakpoint.ExtraExtraLarge, FlexDirection.RowReverse -> Css.``flex-xxl-row-reverse``
      | Some Breakpoint.ExtraExtraLarge, FlexDirection.Column -> Css.``flex-xxl-column``
      | Some Breakpoint.ExtraExtraLarge, FlexDirection.ColumnReverse -> Css.``flex-xxl-column-reverse``

    module Row =

      let allSizes = toClass None FlexDirection.Row
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexDirection.Row
      let sm = toClass (Some Breakpoint.Small) FlexDirection.Row
      let md = toClass (Some Breakpoint.Medium) FlexDirection.Row
      let lg = toClass (Some Breakpoint.Large) FlexDirection.Row
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexDirection.Row
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexDirection.Row

    module RowReverse =

      let allSizes = toClass None FlexDirection.RowReverse
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexDirection.RowReverse
      let sm = toClass (Some Breakpoint.Small) FlexDirection.RowReverse
      let md = toClass (Some Breakpoint.Medium) FlexDirection.RowReverse
      let lg = toClass (Some Breakpoint.Large) FlexDirection.RowReverse
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexDirection.RowReverse
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexDirection.RowReverse

    module Column =

      let allSizes = toClass None FlexDirection.Column
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexDirection.Column
      let sm = toClass (Some Breakpoint.Small) FlexDirection.Column
      let md = toClass (Some Breakpoint.Medium) FlexDirection.Column
      let lg = toClass (Some Breakpoint.Large) FlexDirection.Column
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexDirection.Column
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexDirection.Column

    module ColumnReverse =

      let allSizes = toClass None FlexDirection.ColumnReverse
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexDirection.ColumnReverse
      let sm = toClass (Some Breakpoint.Small) FlexDirection.ColumnReverse
      let md = toClass (Some Breakpoint.Medium) FlexDirection.ColumnReverse
      let lg = toClass (Some Breakpoint.Large) FlexDirection.ColumnReverse
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexDirection.ColumnReverse
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexDirection.ColumnReverse

  [<RequireQualifiedAccess; Struct>]
  type FlexItem =
    | Flex
    | Auto
    | Initial
    | Grow
    | NoGrow
    | Shrink
    | NoShrink
    | None

  /// <summary>
  /// Utilities for controlling how flex items both grow and shrink. Place on items within a flex container.
  /// </summary>
  module FlexItem =

    let toClass breakpoint flex =
      match breakpoint, flex with
      | None, FlexItem.Flex -> Css.``flex-1``
      | None, FlexItem.Auto -> Css.``flex-auto``
      | None, FlexItem.Initial -> Css.``flex-initial``
      | None, FlexItem.Grow -> Css.``flex-grow-1``
      | None, FlexItem.NoGrow -> Css.``flex-grow-0``
      | None, FlexItem.Shrink -> Css.``flex-shrink-1``
      | None, FlexItem.NoShrink -> Css.``flex-shrink-0``
      | None, FlexItem.None -> Css.``flex-none``
      | Some Breakpoint.ExtraSmall, FlexItem.Flex -> Css.``flex-xs-1``
      | Some Breakpoint.ExtraSmall, FlexItem.Auto -> Css.``flex-xs-auto``
      | Some Breakpoint.ExtraSmall, FlexItem.Initial -> Css.``flex-xs-initial``
      | Some Breakpoint.ExtraSmall, FlexItem.Grow -> Css.``flex-xs-grow-1``
      | Some Breakpoint.ExtraSmall, FlexItem.NoGrow -> Css.``flex-xs-grow-0``
      | Some Breakpoint.ExtraSmall, FlexItem.Shrink -> Css.``flex-xs-shrink-1``
      | Some Breakpoint.ExtraSmall, FlexItem.NoShrink -> Css.``flex-xs-shrink-0``
      | Some Breakpoint.ExtraSmall, FlexItem.None -> Css.``flex-xs-none``
      | Some Breakpoint.Small, FlexItem.Flex -> Css.``flex-sm-1``
      | Some Breakpoint.Small, FlexItem.Auto -> Css.``flex-sm-auto``
      | Some Breakpoint.Small, FlexItem.Initial -> Css.``flex-sm-initial``
      | Some Breakpoint.Small, FlexItem.Grow -> Css.``flex-sm-grow-1``
      | Some Breakpoint.Small, FlexItem.NoGrow -> Css.``flex-sm-grow-0``
      | Some Breakpoint.Small, FlexItem.Shrink -> Css.``flex-sm-shrink-1``
      | Some Breakpoint.Small, FlexItem.NoShrink -> Css.``flex-sm-shrink-0``
      | Some Breakpoint.Small, FlexItem.None -> Css.``flex-sm-none``
      | Some Breakpoint.Medium, FlexItem.Flex -> Css.``flex-md-1``
      | Some Breakpoint.Medium, FlexItem.Auto -> Css.``flex-md-auto``
      | Some Breakpoint.Medium, FlexItem.Initial -> Css.``flex-md-initial``
      | Some Breakpoint.Medium, FlexItem.Grow -> Css.``flex-md-grow-1``
      | Some Breakpoint.Medium, FlexItem.NoGrow -> Css.``flex-md-grow-0``
      | Some Breakpoint.Medium, FlexItem.Shrink -> Css.``flex-md-shrink-1``
      | Some Breakpoint.Medium, FlexItem.NoShrink -> Css.``flex-md-shrink-0``
      | Some Breakpoint.Medium, FlexItem.None -> Css.``flex-md-none``
      | Some Breakpoint.Large, FlexItem.Flex -> Css.``flex-lg-1``
      | Some Breakpoint.Large, FlexItem.Auto -> Css.``flex-lg-auto``
      | Some Breakpoint.Large, FlexItem.Initial -> Css.``flex-lg-initial``
      | Some Breakpoint.Large, FlexItem.Grow -> Css.``flex-lg-grow-1``
      | Some Breakpoint.Large, FlexItem.NoGrow -> Css.``flex-lg-grow-0``
      | Some Breakpoint.Large, FlexItem.Shrink -> Css.``flex-lg-shrink-1``
      | Some Breakpoint.Large, FlexItem.NoShrink -> Css.``flex-lg-shrink-0``
      | Some Breakpoint.Large, FlexItem.None -> Css.``flex-lg-none``
      | Some Breakpoint.ExtraLarge, FlexItem.Flex -> Css.``flex-xl-1``
      | Some Breakpoint.ExtraLarge, FlexItem.Auto -> Css.``flex-xl-auto``
      | Some Breakpoint.ExtraLarge, FlexItem.Initial -> Css.``flex-xl-initial``
      | Some Breakpoint.ExtraLarge, FlexItem.Grow -> Css.``flex-xl-grow-1``
      | Some Breakpoint.ExtraLarge, FlexItem.NoGrow -> Css.``flex-xl-grow-0``
      | Some Breakpoint.ExtraLarge, FlexItem.Shrink -> Css.``flex-xl-shrink-1``
      | Some Breakpoint.ExtraLarge, FlexItem.NoShrink -> Css.``flex-xl-shrink-0``
      | Some Breakpoint.ExtraLarge, FlexItem.None -> Css.``flex-xl-none``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Flex -> Css.``flex-xxl-1``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Auto -> Css.``flex-xxl-auto``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Initial -> Css.``flex-xxl-initial``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Grow -> Css.``flex-xxl-grow-1``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.NoGrow -> Css.``flex-xxl-grow-0``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.Shrink -> Css.``flex-xxl-shrink-1``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.NoShrink -> Css.``flex-xxl-shrink-0``
      | Some Breakpoint.ExtraExtraLarge, FlexItem.None -> Css.``flex-xxl-none``

    module Flex =

      let allSizes = toClass None FlexItem.Flex
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Flex
      let sm = toClass (Some Breakpoint.Small) FlexItem.Flex
      let md = toClass (Some Breakpoint.Medium) FlexItem.Flex
      let lg = toClass (Some Breakpoint.Large) FlexItem.Flex
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Flex
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Flex

    module Auto =

      let allSizes = toClass None FlexItem.Auto
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Auto
      let sm = toClass (Some Breakpoint.Small) FlexItem.Auto
      let md = toClass (Some Breakpoint.Medium) FlexItem.Auto
      let lg = toClass (Some Breakpoint.Large) FlexItem.Auto
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Auto
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Auto

    module Initial =

      let allSizes = toClass None FlexItem.Initial
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Initial
      let sm = toClass (Some Breakpoint.Small) FlexItem.Initial
      let md = toClass (Some Breakpoint.Medium) FlexItem.Initial
      let lg = toClass (Some Breakpoint.Large) FlexItem.Initial
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Initial
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Initial

    module Grow =

      let allSizes = toClass None FlexItem.Grow
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Grow
      let sm = toClass (Some Breakpoint.Small) FlexItem.Grow
      let md = toClass (Some Breakpoint.Medium) FlexItem.Grow
      let lg = toClass (Some Breakpoint.Large) FlexItem.Grow
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Grow
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Grow

    module NoGrow =

      let allSizes = toClass None FlexItem.NoGrow
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.NoGrow
      let sm = toClass (Some Breakpoint.Small) FlexItem.NoGrow
      let md = toClass (Some Breakpoint.Medium) FlexItem.NoGrow
      let lg = toClass (Some Breakpoint.Large) FlexItem.NoGrow
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.NoGrow
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.NoGrow

    module Shrink =

      let allSizes = toClass None FlexItem.Shrink
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.Shrink
      let sm = toClass (Some Breakpoint.Small) FlexItem.Shrink
      let md = toClass (Some Breakpoint.Medium) FlexItem.Shrink
      let lg = toClass (Some Breakpoint.Large) FlexItem.Shrink
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.Shrink
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.Shrink

    module NoShrink =

      let allSizes = toClass None FlexItem.NoShrink
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.NoShrink
      let sm = toClass (Some Breakpoint.Small) FlexItem.NoShrink
      let md = toClass (Some Breakpoint.Medium) FlexItem.NoShrink
      let lg = toClass (Some Breakpoint.Large) FlexItem.NoShrink
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.NoShrink
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.NoShrink

    module None =

      let allSizes = toClass None FlexItem.None
      let xs = toClass (Some Breakpoint.ExtraSmall) FlexItem.None
      let sm = toClass (Some Breakpoint.Small) FlexItem.None
      let md = toClass (Some Breakpoint.Medium) FlexItem.None
      let lg = toClass (Some Breakpoint.Large) FlexItem.None
      let xl = toClass (Some Breakpoint.ExtraLarge) FlexItem.None
      let xxl = toClass (Some Breakpoint.ExtraExtraLarge) FlexItem.None

  [<RequireQualifiedAccess; Struct>]
  type JustifyContent =
    | FlexStart
    | Center
    | FlexEnd
    | SpaceBetween
    | SpaceAround
    | SpaceEvenly

  /// <summary>
  /// Controls how flex and grid items are positioned along a container's main axis. Place on a flex or grid container.
  /// </summary>
  module JustifyContent =

    let toClass justify =
      match justify with
      | JustifyContent.FlexStart -> Css.``justify-start``
      | JustifyContent.Center -> Css.``justify-center``
      | JustifyContent.FlexEnd -> Css.``justify-end``
      | JustifyContent.SpaceBetween -> Css.``justify-space-between``
      | JustifyContent.SpaceAround -> Css.``justify-space-around``
      | JustifyContent.SpaceEvenly -> Css.``justify-space-evenly``

  [<RequireQualifiedAccess; Struct>]
  type AlignContent =
    | Center
    | Start
    | End
    | SpaceBetween
    | SpaceAround
    | Stretch

  /// <summary>
  /// Controls how rows are positioned in a flex or grid container. Place on a flex or grid container.
  /// </summary>
  module AlignContent =

    let toClass align =
      match align with
      | AlignContent.Center -> Css.``align-content-center``
      | AlignContent.Start -> Css.``align-content-start``
      | AlignContent.End -> Css.``align-content-end``
      | AlignContent.SpaceBetween -> Css.``align-content-space-between``
      | AlignContent.SpaceAround -> Css.``align-content-space-around``
      | AlignContent.Stretch -> Css.``align-content-stretch``

  [<RequireQualifiedAccess; Struct>]
  type AlignItems =
    | Baseline
    | Center
    | Start
    | End
    | Stretch

  /// <summary>
  /// Controls how flex and grid items are positioned along a container's cross axis. Place on a flex or grid container.
  /// </summary>
  module AlignItems =

    let toClass align =
      match align with
      | AlignItems.Baseline -> Css.``align-baseline``
      | AlignItems.Center -> Css.``align-center``
      | AlignItems.Start -> Css.``align-start``
      | AlignItems.End -> Css.``align-end``
      | AlignItems.Stretch -> Css.``align-stretch``

  [<RequireQualifiedAccess; Struct>]
  type AlignSelf =
    | Auto
    | Center
    | Start
    | End
    | Stretch

  /// <summary>
  /// Controls how an individual flex or grid item is positioned along its container's cross axis. Place on a flex or grid item.
  /// </summary>
  module AlignSelf =

    let toClass align =
      match align with
      | AlignSelf.Auto -> Css.``align-self-auto``
      | AlignSelf.Center -> Css.``align-self-center``
      | AlignSelf.Start -> Css.``align-self-start``
      | AlignSelf.End -> Css.``align-self-end``
      | AlignSelf.Stretch -> Css.``align-self-stretch``

  [<RequireQualifiedAccess; Struct>]
  type Visibility =
    | AlwaysVisible
    | AlwaysHidden
    | HideOnlyOn of Breakpoint
    | VisibleOnlyOn of Breakpoint

  [<RequireQualifiedAccess; Struct>]
  type Display =
    | Block of Visibility
    | Flex of Visibility
    | Inline of Visibility
    | InlineBlock of Visibility
    | InlineFlex of Visibility

  module Display =

    let toClasses display =
      match display with
      | Display.Block visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-block`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-block`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-block``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-block``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-block``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-block``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-block``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-none``; Css.``d-xxl-block`` ]
        | Visibility.AlwaysVisible -> [ Css.``d-block`` ]
      | Display.Flex visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-flex``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-flex``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-flex``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-flex``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-flex``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-none``; Css.``d-xxl-flex`` ]
        | Visibility.AlwaysVisible -> [ Css.``d-flex`` ]
      | Display.Inline visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-inline`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-inline``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-inline``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-inline``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-inline``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-inline``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-none``; Css.``d-xxl-inline`` ]
        | Visibility.AlwaysVisible -> [ Css.``d-inline`` ]
      | Display.InlineBlock visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-inline-block`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-inline-block``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-inline-block``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-inline-block``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-inline-block``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-inline-block``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [
            Css.``d-none``
            Css.``d-xxl-inline-block``
          ]
        | Visibility.AlwaysVisible -> [ Css.``d-inline-block`` ]
      | Display.InlineFlex visibility ->
        match visibility with
        | Visibility.AlwaysHidden -> [ Css.``d-none`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-none``; Css.``d-sm-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Small -> [ Css.``d-sm-none``; Css.``d-md-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Medium -> [ Css.``d-md-none``; Css.``d-lg-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.Large -> [ Css.``d-lg-none``; Css.``d-xl-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraLarge -> [ Css.``d-xl-none``; Css.``d-xxl-inline-flex`` ]
        | Visibility.HideOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-xxl-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraSmall -> [ Css.``d-inline-flex``; Css.``d-sm-none`` ]
        | Visibility.VisibleOnlyOn Breakpoint.Small -> [
            Css.``d-none``
            Css.``d-sm-inline-flex``
            Css.``d-md-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Medium -> [
            Css.``d-none``
            Css.``d-md-inline-flex``
            Css.``d-lg-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.Large -> [
            Css.``d-none``
            Css.``d-lg-inline-flex``
            Css.``d-xl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraLarge -> [
            Css.``d-none``
            Css.``d-xl-inline-flex``
            Css.``d-xxl-none``
          ]
        | Visibility.VisibleOnlyOn Breakpoint.ExtraExtraLarge -> [ Css.``d-none``; Css.``d-xxl-inline-flex`` ]
        | Visibility.AlwaysVisible -> [ Css.``d-inline-flex`` ]

  [<RequireQualifiedAccess; Struct>]
  type Overflow =
    | Auto
    | Hidden
    | Visible
    | Scroll
    | XAuto
    | XHidden
    | YAuto
    | YHidden

  module Overflow =

    let toClass overflow =
      match overflow with
      | Overflow.Auto -> Css.``overflow-auto``
      | Overflow.Hidden -> Css.``overflow-hidden``
      | Overflow.Visible -> Css.``overflow-visible``
      | Overflow.Scroll -> Css.``overflow-scroll``
      | Overflow.XAuto -> Css.``overflow-x-auto``
      | Overflow.XHidden -> Css.``overflow-x-hidden``
      | Overflow.YAuto -> Css.``overflow-y-auto``
      | Overflow.YHidden -> Css.``overflow-y-hidden``

  [<RequireQualifiedAccess; Struct>]
  type ObjectFit =
    | Fill
    | Contain
    | Cover
    | None
    | ScaleDown

  /// <summary>
  /// Specifies how a replaced element's content should be resized
  /// </summary>
  module ObjectFit =

    let toClass objectFit =
      match objectFit with
      | ObjectFit.Fill -> Css.``object-fill``
      | ObjectFit.Contain -> Css.``object-contain``
      | ObjectFit.Cover -> Css.``object-cover``
      | ObjectFit.None -> Css.``object-none``
      | ObjectFit.ScaleDown -> Css.``object-scale-down``

  [<RequireQualifiedAccess; Struct>]
  type ObjectPosition =
    | Bottom
    | Center
    | Left
    | LeftBottom
    | LeftTop
    | Right
    | RightBottom
    | RightTop

  /// <summary>
  /// Specifies how a replaced element's content should be positioned within its container
  /// </summary>
  module ObjectPosition =

    let toClass objectPosition =
      match objectPosition with
      | ObjectPosition.Bottom -> Css.``object-bottom``
      | ObjectPosition.Center -> Css.``object-center``
      | ObjectPosition.Left -> Css.``object-left``
      | ObjectPosition.LeftBottom -> Css.``object-left-bottom``
      | ObjectPosition.LeftTop -> Css.``object-left-top``
      | ObjectPosition.Right -> Css.``object-right``
      | ObjectPosition.RightBottom -> Css.``object-right-bottom``
      | ObjectPosition.RightTop -> Css.``object-right-top``

  [<RequireQualifiedAccess; Struct>]
  type Position =
    | Static
    | Relative
    | Absolute
    | Fixed
    | Sticky

  /// <summary>
  /// Utilities for controlling how an element is positioned in the DOM
  /// </summary>
  module Position =

    let toClass position =
      match position with
      | Position.Static -> Css.``static``
      | Position.Relative -> Css.``relative``
      | Position.Absolute -> Css.``absolute``
      | Position.Fixed -> Css.``fixed``
      | Position.Sticky -> Css.``sticky``
