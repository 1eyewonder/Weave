---
last_commit_released: 30ae69ac5004cc4e5c0f6b6f49e606d5b52afe0a
---

# Changelog

## v0.3.0

### 🏗️ Breaking changes

- **BREAKING: Heading sizes and letter spacing changed.** Review heading wrapping in existing layouts. Override `--typography-hN-size` and `--typography-hN-letterspacing` to retain your theme's previous values (`N` is the heading level). [d5de59d](https://github.com/1eyewonder/Weave/commit/d5de59dde5f39ef6247620c43bb9be404ee92de9)
- **BREAKING: Headings h1–h5 now default to bold.** To restore previous weights, set `--typography-h1-weight` and `--typography-h2-weight` to `var(--weave-weight-light)`, and the h3–h5 weight variables to `var(--weave-weight-regular)`. [d5de59d](https://github.com/1eyewonder/Weave/commit/d5de59dde5f39ef6247620c43bb9be404ee92de9)
- **BREAKING: TextField counters moved inside `.weave-field`.** Update custom sibling selectors to target `.weave-field .weave-field__counter`. [9720386](https://github.com/1eyewonder/Weave/commit/9720386c947564a73b2efa3b3073bcf234f7cf15)

### 🚀 Features

- Added `DatePicker` for selecting a calendar date. [07a303b](https://github.com/1eyewonder/Weave/commit/07a303bd6666e004566d669acd0c0b14825a38c9)
- Added `DateRangePicker` for selecting an inclusive date range. [07a303b](https://github.com/1eyewonder/Weave/commit/07a303bd6666e004566d669acd0c0b14825a38c9)
- Added `TimePicker` with editable time fields, 12/24-hour clocks, and configurable minute steps. [07a303b](https://github.com/1eyewonder/Weave/commit/07a303bd6666e004566d669acd0c0b14825a38c9)
- Added `Range<'T>` and `Interval<'T>` for validated bounds, membership checks, and endpoint mapping. [07a303b](https://github.com/1eyewonder/Weave/commit/07a303bd6666e004566d669acd0c0b14825a38c9)
- Added `DateAvailability` and `TimeAvailability` for composing booking windows, exclusions, and filters. [07a303b](https://github.com/1eyewonder/Weave/commit/07a303bd6666e004566d669acd0c0b14825a38c9)
- Added `Globalization` for shared calendar culture, with per-picker week-start and date-format overrides. [07a303b](https://github.com/1eyewonder/Weave/commit/07a303bd6666e004566d669acd0c0b14825a38c9)

### 🐞 Bug Fixes

- **Select and MultiSelect:** Disabled controls leave the tab order, and disabled/read-only states are exposed to assistive technology. [abae806](https://github.com/1eyewonder/Weave/commit/abae806ffcb2a6ed91771cbb82e8747f5a68fc34)
- **Select and MultiSelect:** Fixed long-label sizing and menus overflowing the viewport after scrolling or resizing. [84b8b69](https://github.com/1eyewonder/Weave/commit/84b8b69bbf23c2ec84cb376b1c84e73b6c9990c3)
- **Fields:** Aligned control heights and caret colors across field variants. [60daf82](https://github.com/1eyewonder/Weave/commit/60daf82fe6a7266ca5596603dc83a81e9839d743)
- **Dialogs and nested popups:** Fixed focus containment and Escape closing the containing overlay before its popup. [1fd58ef](https://github.com/1eyewonder/Weave/commit/1fd58ef602fcf66665224bd99b91cd7caf0cb8b8), [0fc37ac](https://github.com/1eyewonder/Weave/commit/0fc37ac8ccbc87075d550a5b9c18d1a69df71c9b)
- **Drawers:** Fixed custom widths and focus trapping when responsive layouts change backdrop visibility. [bbd9a91](https://github.com/1eyewonder/Weave/commit/bbd9a91ed2973e99fbef022b05dbeacadecb01e7), [9d19cdd](https://github.com/1eyewonder/Weave/commit/9d19cdd62a275820a7a04138469214da8684c46c)
- **Tooltips:** Fixed viewport clipping, keyboard access to long content, and accessible trigger descriptions. [4af2c6c](https://github.com/1eyewonder/Weave/commit/4af2c6c654e14addc8e63ca7c8b24300518dbbdd)
- **Tabs:** Fixed selected-tab indicator placement and visibility after resizing, and improved filled-tab contrast. [7605899](https://github.com/1eyewonder/Weave/commit/7605899cb7e6f00c136c990dd80a24f9cc2f7502), [f0f0d64](https://github.com/1eyewonder/Weave/commit/f0f0d6401d84702f77897159e479de435d7b2a26)
- **Checkboxes:** Long labels wrap without shrinking the checkbox, with correct spacing in right-to-left layouts. [666fa1d](https://github.com/1eyewonder/Weave/commit/666fa1de095922f1245aa98606381542b8d932c2), [e5e02eb](https://github.com/1eyewonder/Weave/commit/e5e02ebf9ff75fe9ef704d3d577c864df37727e4)
- **Chips:** Disabled actions and remove controls now expose their disabled state and leave the tab order. [92e210c](https://github.com/1eyewonder/Weave/commit/92e210c496d857b69aef95ce061584dade4098f4)
- **Switches:** Corrected track and thumb proportions across sizes. [38d9479](https://github.com/1eyewonder/Weave/commit/38d947974b4498ef102ffc4f7ae275cbd71c9a91)
- **Cards:** Long headers wrap instead of being truncated. [5dda519](https://github.com/1eyewonder/Weave/commit/5dda5199174f16356908184bc3be56471b05e918)
- **Carousels:** Navigation controls fit narrow containers. [4e6064f](https://github.com/1eyewonder/Weave/commit/4e6064fd25ae266fdb586d7e74a0f60cdd25ddd2)
- **Pagination:** Wrapped control rows stay centered. [581a5cb](https://github.com/1eyewonder/Weave/commit/581a5cb1cb89f63fefa5600c265797db423f4ee9)
- **Links:** Fixed hover and focus contrast. [c410f74](https://github.com/1eyewonder/Weave/commit/c410f74a02e9db90f4d1bc3d1eddd895cd15af82)
- **App bars:** Fixed button contrast on themed backgrounds. [eeb2255](https://github.com/1eyewonder/Weave/commit/eeb225569e57e1f92d3b596dda064f12cac3323d)
- **ScrollListener:** Fixed section navigation highlighting the wrong visible heading. [72b47b9](https://github.com/1eyewonder/Weave/commit/72b47b91794d1143e171c064e247d9c1a21e2adf)

### Documentation

- Refreshed the documentation home, navigation, search, and interactive component examples. [5af21d3](https://github.com/1eyewonder/Weave/commit/5af21d3aa82322acb1114009f736f9be7b25fb89)
- Added a [Date & Time guide](https://1eyewonder.github.io/Weave/#date-time-guide) covering ranges, intervals, availability, and picker integration. [07a303b](https://github.com/1eyewonder/Weave/commit/07a303bd6666e004566d669acd0c0b14825a38c9)

### Starter templates

- Updated all four starter templates to use `Weave.WebSharper` `0.3.0`. [923cc3d](https://github.com/1eyewonder/Weave/commit/923cc3d527807a73c1a1e7e2808ea26a3ea97575)

**Compatibility:** Tested with WebSharper and WebSharper.FSharp `10.1.9.686`, and WebSharper.UI `10.1.7.686`. Starter templates target .NET 10.

[Full changes since v0.2.0](https://github.com/1eyewonder/Weave/compare/v0.2.0...v0.3.0)

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
