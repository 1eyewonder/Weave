# Maintain the documentation

Build component pages around a working preview, copyable F# source, and an accurate API reference.

## Run the site

Complete the repository [development setup](../../README.md#development-setup), then run this from the repository root:

```sh
yarn dev:docs
```

Open the URL printed by Vite. Editing F# may reset the live demo state.

## Write a component page

- Start with a short purpose statement and the smallest useful interaction. Give later sections distinct jobs: appearance, behavior, composition, or API lookup.
- Let appearance controls combine common options. When adding a configurator, remove sections it fully covers, such as separate color or density examples. Keep comparisons and focused interactions that teach something different, and preserve existing section links.
- Keep the preview and its source in sync. Generate both from the same configuration; register generated source with the [snippet checker](../../tests/Weave.Docs.Snippets/README.md).
- Check API names, types, defaults, and options against the [component source](../Weave/components) and SCSS. Use the shared helpers in [ExampleHelpers.fs](examples/shared/ExampleHelpers.fs): `codeSampleSection` for a live F# example and `codeSection` for source without a preview.
- For a new page, update [routing](DocsRouting.fs), the [home catalog](DocsHome.fs), the [example router](ExamplesRouter.fs), and [project compile order](Weave.Docs.fsproj).

## Keep source useful to copy

Put shared setup in [Getting Started](examples/GettingStartedExamples.fs). Show the component call, its essential state, and its real children. Omit preview-only grids, spacing, bounds, and outer containers. Keep layout code when the layout itself is being taught, as in Grid, forms, drawer composition, and complete showcases.

A comparison can show independent calls; it need not copy the preview's layout. Match the visible labels and behavior, and explain when a short teaching example differs from the larger live demo. For cumulative examples, see the [Task Tracker showcase](examples/showcases/ShowcaseTaskTracker.fs) and the [checker guide](../../tests/Weave.Docs.Snippets/README.md).

Keep displayed source near 78 columns, with two-space indentation and closing parentheses on their own lines in multiline calls. Fantomas formats the surrounding F# file, not source inside strings. The repository `.editorconfig` still governs implementation code.

Use public component attributes for the live example before adding docs CSS. If a copied example exposes a clipping, contrast, or keyboard problem, investigate the component and its shared styles and add a focused regression test.

## Validate the page

For a focused source check, build the docs project and filter by example filename:

```sh
dotnet build src/Weave.Docs/Weave.Docs.fsproj
dotnet fsi tests/Weave.Docs.Snippets/check.fsx ButtonExamples
```

Run the complete documentation gate before review:

| Platform | Command |
| --- | --- |
| Windows PowerShell | `.\build.cmd BuildDocs` |
| macOS / Linux | `./build.sh BuildDocs` |

`BuildDocs` builds CSS and the site in Release, then checks displayed F# source. Run `yarn test:docs:highlighting` when changing code or type highlighting. See the [checker guide](../../tests/Weave.Docs.Snippets/README.md) for its coverage and limits.

Review the rendered page in both themes, on desktop and a narrow mobile viewport. Open and copy the code, exercise the controls with keyboard and pointer, and check long content and deep links. For component changes, run the relevant rendering and browser tests described in the repository [testing guide](../../README.md#testing).
