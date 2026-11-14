---
last_commit_released: 30ae69ac5004cc4e5c0f6b6f49e606d5b52afe0a
---

# Changelog

## v0.3.0

### Breaking changes and upgrade guidance

This is a pre-1.0 minor release. The changes below can affect existing layouts or custom CSS; the major version remains `0`.

- **BREAKING (visual): Heading sizes and spacing changed.** `Typography.h1` through `h6` now use a consistent 1.25-ratio responsive scale and revised letter spacing. Review heading wrapping and page layouts. To retain your previous styling, override the affected `--typography-hN-size` and `--typography-hN-letterspacing` variables in your theme, replacing `N` with the heading level.
- **BREAKING (visual): Heading weights changed.** `h1` and `h2` now default to bold instead of light; `h3`, `h4`, and `h5` default to bold instead of regular. To retain the old weights, set `--typography-h1-weight` and `--typography-h2-weight` to `var(--weave-weight-light)`, and the `h3` through `h5` weight variables to `var(--weave-weight-regular)`.
- **BREAKING (custom CSS): TextField counters moved inside their field.** A character counter is now a child of `.weave-field`, rather than its sibling. Update custom sibling selectors or layout rules that assumed the counter sat outside the field; use `.weave-field .weave-field__counter` when targeting it.

### New date and time components

- Added `DatePicker` for selecting one calendar date.
- Added `DateRangePicker` for selecting an ordered, inclusive date range, including a single-day range.
- Added `TimePicker` with a default 12-hour clock and an explicit 24-hour option.
- Added editable hour and minute fields alongside the time dial, with numeric input, validation messages, and configurable minute steps.
- Added shared picker variants, theme colors, clear controls, and editable, read-only, and disabled modes.
- Pickers stay open after selection so users can review the value; Done, Escape, or an outside click closes the popup.
- Added application-wide calendar culture through `Globalization`; month names, weekday labels, and default week starts follow that culture. Date pickers also accept an explicit first-day override and custom display formatting.
- Added generic `Range<'T>` values with checked construction, inclusive/exclusive bounds, and read-only `Lower` and `Upper` members.
- Added `Interval<'T>` to represent bounded, unbounded, or single-value intervals.
- Added range and interval membership checks, closed/singleton constructors, and `mapBounds` helpers for projecting endpoint values.
- Added `DateAvailability` and `TimeAvailability` to combine allowed windows, exclusions, and filters. Rules can change reactively without recreating a picker.
- Added picker integration with `FormField`. Clock, minute-step, parsing, and availability errors participate in form validation; unavailable typed or externally assigned times stay entered and block submission until corrected.

### Component and interaction fixes

- **Select and MultiSelect:** Disabled comboboxes now leave the tab order and expose reactive disabled and read-only states to assistive technology.
- **Select and MultiSelect:** Long labels and selected values fit within their controls instead of clipping action icons or overflowing the layout.
- **Select and MultiSelect:** Unbound menus stay inside the viewport and reposition when the page scrolls or the window resizes.
- **Fields:** Standard, filled, and outlined controls now use consistent heights.
- **Fields:** Caret colors follow the field's theme color.
- **Fields:** Above-field labels reserve their spacing once, including when a Select wraps an inner field.
- **Dialogs:** Tab navigation includes editable content, summaries, and radio groups while skipping hidden, inert, and disabled controls.
- **Nested overlays:** Escape closes the active selector or tooltip before its containing dialog or drawer.
- **Drawers:** Custom widths now agree across the panel, surrounding layout, and hover-expanded state.
- **Drawers:** Keyboard focus is trapped only while the backdrop is visible, including when responsive breakpoints or custom styles change its visibility.
- **Drawers:** Closing or removing an overlay restores focus to its trigger when focus is still inside the drawer.
- **Tooltips:** Bubbles reposition near viewport edges and after scrolling or resizing.
- **Tooltips:** Long content remains scrollable and reachable with the keyboard.
- **Tooltips:** Accessible descriptions connect to native triggers, including custom tooltip IDs and replacement triggers.
- **Tabs:** The initial indicator starts at the selected tab instead of sliding from the first tab.
- **Tabs:** The selected tab remains visible after its container resizes.
- **Tabs:** Filled tabs and themed header backgrounds use contrasting text and clearer active-pill boundaries. Custom header backgrounds can set `--tabs-header-foreground` for an appropriate text color.
- **Checkboxes:** Long labels wrap without shrinking the checkbox control.
- **Checkboxes:** Label spacing follows right-to-left layouts.
- **Chips:** Disabled links, action buttons, and remove controls expose their disabled state and update their tab stops reactively.
- **Switches:** The track and thumb scale together for each size.
- **Cards:** Long header text wraps instead of being truncated.
- **Carousels:** Navigation controls remain usable in narrow containers.
- **Pagination:** Wrapped control rows remain centered.
- **Links:** Hover and focus colors remain readable across supported color styles.
- **App bars:** Filled, outlined, and text buttons retain readable colors on themed surfaces.
- **ScrollListener:** Before a heading crosses the activation threshold, the nearest visible heading is selected instead of a later heading farther down the page.

### Documentation

- Rebuilt the documentation home, navigation, search, and responsive component examples.
- Added a Date & Time guide explaining ranges, intervals, availability rules, and picker integration, with API references and copyable examples.
- Added interactive appearance configurators and improved copyable F# examples, syntax highlighting, and example layout on desktop and mobile.

### Starter templates

- Updated `Weave.WebSharper.Templates` to `0.3.0`.
- Updated `weave-app`, `weave-dashboard`, `weave-projects`, and `weave-repository` to reference `Weave.WebSharper` `0.3.0`.

### Compatibility

- The tested baseline remains WebSharper and WebSharper.FSharp `10.1.9.686`, with WebSharper.UI `10.1.7.686`.
- Starter templates continue to target .NET 10 and require the .NET 10.0.403 SDK or a newer .NET 10 feature band.

[Full library changes since v0.2.0](https://github.com/1eyewonder/Weave/compare/v0.2.0...v0.3.0)

## v0.2.0

- Add a typed form validation DSL and reactive components for carousels, Unicode-aware text highlighting, pagination, progress indicators, scroll-to-top navigation, queued snackbars, and color picking.
- Add recent components to starter templates and improve documentation examples and F# syntax highlighting.
- Improve keyboard interaction, resize-aware tabs, numeric inputs, button-menu hover states, and feedback actions on colored surfaces.
- Lower the minimum FSharp.Core dependency.
- Built and tested against WebSharper 10.1.9.686 and WebSharper.UI 10.1.7.686.

## v0.1.0

- Refresh component styling, theme colors, documentation navigation, and getting-started guidance.
- Improve keyboard focus, accessible colors and interaction states across components in light and dark themes.
- Refine dialogs, tables, form controls, code examples, and responsive documentation layouts.
- Expand browser interaction, accessibility, and rendering coverage for the refreshed components.
- Built and tested against WebSharper 10.1.9.686 and WebSharper.UI 10.1.7.686.

## v0.1.0-rc.1

- First release candidate of the Weave F# component library for WebSharper.
- Includes UI components, layout and styling utilities, theming, and the compiled stylesheet.
- Built and tested against WebSharper 10.1.9.686 and WebSharper.UI 10.1.7.686. APIs may change before the stable release.
