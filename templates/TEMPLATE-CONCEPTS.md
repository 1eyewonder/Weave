# Application template concepts

These are proposals for editable F# / WebSharper starter applications. They use fictional names and sample data, and take inspiration from common workflows rather than copying product branding or assets. Each starter should work without an account, API key, or backend. Changes to sample data are in memory unless otherwise stated.

## Project workspace (`weave-projects`)

A project tracker inspired by Linear's compact issue and project views.

- Layout: workspace sidebar, project heading and summary, issues toolbar, grouped issue list, and an optional board with status columns. On small screens, collapse navigation and allow board columns to scroll horizontally.
- Initial interactions: toggle list/board, filter by status or assignee, search issue titles, open and close issue details, add an issue, and change an issue's status. List and board share the same in-memory data.
- Demonstrates: Drawer, AppBar, Button, TextField, Select, Chip, Dialog, and responsive layout. Use explicit status controls for moving issues; accessible drag-and-drop is a separate enhancement.
- Reference: https://linear.app/docs/display-options

## Repository workspace (`weave-repository`)

A repository-style workspace inspired by GitHub's code and issue pages, useful for documentation portals or internal project hubs as well as code hosting UIs.

- Layout: global header, repository identity and metadata, horizontal navigation, file listing with branch selector, README panel, and an issues page with filters. Stack the metadata area beneath content on narrow screens.
- Initial interactions: navigate Code and Issues, select a sample branch, open folders and return via breadcrumbs, search/filter open and closed issues, and open issue details. Keep navigation and sample data consistent across views.
- Demonstrates: Tabs or links, DropdownMenu, Table/List, Chip, Button, and responsive layout. Do not imply the demo can access Git repositories or GitHub without an integration.
- Reference: https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/filtering-and-searching-issues-and-pull-requests

## Business dashboard (`weave-dashboard`)

An operations dashboard inspired by Stripe's information hierarchy and payment workflows, adaptable to other business data.

- Layout: application sidebar, account/header area, overview metrics, activity section, and a searchable transaction table. Collapse navigation on mobile and keep the data table usable at narrow widths.
- Initial interactions: switch a sample date range, filter transactions by status, search customer or reference, sort a column, and open a transaction detail panel. Metrics and table respond to the selected sample data and range.
- Demonstrates: Drawer, AppBar, Card, Select, TextField, Table, Chip, and Dialog. Use a simple accessible trend summary until a chart solution is selected. All amounts and transactions are fictional; no payments are processed.
- Reference: https://docs.stripe.com/dashboard/search

## Shared implementation approach

1. First resolve a consumable Weave package and its CSS/WebSharper assets. Until then, generated projects follow the existing local-checkout preview convention.
2. Provide each as a distinct `dotnet new` template, with independently editable source and clearly separated sample data. Factor authoring/build tooling only where it does not leave generated projects dependent on the template package.
3. Reuse Weave components and theme tokens; document any missing primitives rather than presenting incomplete controls as functional. Use original names, text, colors, and icons.
4. Include keyboard-operable controls, labels, visible focus, empty/filter-no-results states, responsive layouts, and light/dark theme coverage.
5. Verify each packaged template from a clean generated project: restore, build, run, and browser-test its named interactions and narrow-screen layout. Show previews and source in the docs.

Suggested order: project workspace first (shared state across list and board), repository workspace second (navigation and content), dashboard third (data views). Validate and adjust this order against component gaps discovered during implementation.
