module Weave.Tests.Unit.DrawerTests

open Expecto
open Weave

[<Tests>]
let drawerTests =
  testList "Drawer" [
    testList "Variant.toClass" [
      testTheory "each variant maps to the correct class" [
        Drawer.Variant.Temporary, "weave-drawer--temporary"
        Drawer.Variant.Persistent, "weave-drawer--persistent"
        Drawer.Variant.Responsive, "weave-drawer--responsive"
        Drawer.Variant.Mini, "weave-drawer--mini"
      ]
      <| fun (variant, expected) -> Expect.equal (Drawer.Variant.toClass variant) expected ""
    ]

    testList "Variant.toOverlayClass" [
      testTheory "each variant maps to the correct overlay class" [
        Drawer.Variant.Temporary, Some "weave-drawer__overlay--temporary"
        Drawer.Variant.Responsive, Some "weave-drawer__overlay--responsive"
        Drawer.Variant.Mini, Some "weave-drawer__overlay--mini"
        Drawer.Variant.Persistent, None
      ]
      <| fun (variant, expected) -> Expect.equal (Drawer.Variant.toOverlayClass variant) expected ""
    ]

    testList "Position.toClass" [
      testTheory "each position maps to the correct class" [
        Drawer.Position.Left, "weave-drawer--pos-left"
        Drawer.Position.Right, "weave-drawer--pos-right"
      ]
      <| fun (position, expected) -> Expect.equal (Drawer.Position.toClass position) expected ""
    ]

    testList "ClipMode.toDrawerClass" [
      testTheory "each clip mode maps to the correct class" [
        Drawer.ClipMode.AppBar, Some "weave-drawer--clip-appbar"
        Drawer.ClipMode.FullHeight, Some "weave-drawer--clip-fullheight"
      ]
      <| fun (clipMode, expected) -> Expect.equal (Drawer.ClipMode.toDrawerClass clipMode) expected ""
    ]

    testList "DrawerBreakpoint.toDrawerClass" [
      testTheory "At breakpoints map to the correct class" [
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraSmall, Some "weave-drawer--xs"
        Drawer.DrawerBreakpoint.At Breakpoint.Small, Some "weave-drawer--sm"
        Drawer.DrawerBreakpoint.At Breakpoint.Medium, Some "weave-drawer--md"
        Drawer.DrawerBreakpoint.At Breakpoint.Large, Some "weave-drawer--lg"
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraLarge, Some "weave-drawer--xl"
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraExtraLarge, Some "weave-drawer--xxl"
        Drawer.DrawerBreakpoint.None, None
        Drawer.DrawerBreakpoint.Always, None
      ]
      <| fun (bp, expected) -> Expect.equal (Drawer.DrawerBreakpoint.toDrawerClass bp) expected ""
    ]

    testList "DrawerBreakpoint.toOverlayClass" [
      testTheory "At breakpoints map to the correct overlay class" [
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraSmall, Some "weave-drawer__overlay--xs"
        Drawer.DrawerBreakpoint.At Breakpoint.Small, Some "weave-drawer__overlay--sm"
        Drawer.DrawerBreakpoint.At Breakpoint.Medium, Some "weave-drawer__overlay--md"
        Drawer.DrawerBreakpoint.At Breakpoint.Large, Some "weave-drawer__overlay--lg"
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraLarge, Some "weave-drawer__overlay--xl"
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraExtraLarge, Some "weave-drawer__overlay--xxl"
        Drawer.DrawerBreakpoint.None, None
        Drawer.DrawerBreakpoint.Always, None
      ]
      <| fun (bp, expected) -> Expect.equal (Drawer.DrawerBreakpoint.toOverlayClass bp) expected ""
    ]

    testList "DrawerBreakpoint.toContainerClass" [
      testTheory "each breakpoint maps to the correct container class" [
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraSmall, "weave-drawer-container--bp-xs"
        Drawer.DrawerBreakpoint.At Breakpoint.Small, "weave-drawer-container--bp-sm"
        Drawer.DrawerBreakpoint.At Breakpoint.Medium, "weave-drawer-container--bp-md"
        Drawer.DrawerBreakpoint.At Breakpoint.Large, "weave-drawer-container--bp-lg"
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraLarge, "weave-drawer-container--bp-xl"
        Drawer.DrawerBreakpoint.At Breakpoint.ExtraExtraLarge, "weave-drawer-container--bp-xxl"
        Drawer.DrawerBreakpoint.None, "weave-drawer-container--bp-none"
        Drawer.DrawerBreakpoint.Always, "weave-drawer-container--bp-always"
      ]
      <| fun (bp, expected) -> Expect.equal (Drawer.DrawerBreakpoint.toContainerClass bp) expected ""
    ]

    testList "Density.toClass" [
      testTheory "each density maps to the correct header class" [
        Density.Compact, "weave-drawer__header--compact"
        Density.Standard, "weave-drawer__header--standard"
        Density.Spacious, "weave-drawer__header--spacious"
      ]
      <| fun (density, expected) -> Expect.equal (Drawer.Density.toClass density) expected ""
    ]
  ]
