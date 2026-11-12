# Tooltip close transition mutation validation

## Scope and snapshot

- Property: when a visible tooltip closes, it stays rendered while opacity fades, then `display: none` removes it from layout.
- PR baseline revision: `326ac51cdbf96f26cef3cabd8c6c196e29e2d43d` (`codex/tooltip-placement`).
- Implementation snapshot: local branch `codex/tooltip-fade` in `C:\Users\brons\source\repos\Weave\t`.
- Production language / test runner: F# / WebSharper; E2E tests use xUnit 3.1.5 and Playwright.
- Runtime: Windows x64, .NET SDK 10.0.403, Node.js 22.19.0.
- Final source fingerprints (SHA-256):
  - `src/Weave/components/Tooltip.fs`: `7C4228346AA51C0B9704C1DE24B57C10ED882004F03D3607F7A0835800861531`
  - `tests/Weave.Tests.E2E/interaction/TooltipInteractionTests.fs`: `1063967C4E0CAFEE4AE5067B34BABF7472659E966008650D04A156B084B7E746`

## Baseline

The E2E test assembly, fixture site, and Weave library built successfully with 0 warnings and 0 errors. Sass was compiled directly from the installed package because Corepack could not download Yarn in this network-restricted environment.

Command:

```text
dotnet test tests/Weave.Tests.E2E/Weave.Tests.E2E.fsproj --no-build --no-restore --filter FullyQualifiedName~TooltipInteractionTests --logger "console;verbosity=minimal"
```

Result on the final implementation: **40 passed, 0 failed, 0 skipped** in Chromium. The new fade regression also passed individually in Chromium, Firefox, and WebKit (1/1 each).

## Manual mutant

The source is F#, so a manual source mutant was used in a separate worktree snapshot. The mutant added this immediate hide to `TooltipPlacement.setVisible`:

```fsharp
else
  setDisplay element "none"
```

This models the reviewed defect: setting `display: none` as soon as the visible class is removed. Mutant source SHA-256: `4C244A35CF0B2A478CBAECD8BD51D38475CF6E931853C9A091AD2B26674D9F60`.

Command:

```text
dotnet test m/tests/Weave.Tests.E2E/Weave.Tests.E2E.fsproj --no-build --no-restore --filter "DisplayName~tooltip fades out before it is removed from layout" --logger "console;verbosity=normal"
```

**Verdict: Detected.** The mutated library, site, and test assembly built successfully. The test ran in Chromium and failed at its 3-second `WaitForFunctionAsync` condition because the tooltip never remained `display: block` with opacity below 1. The production baseline passed the same behavior check.

## Tooling note

`yarn build:css` could not start because Corepack could not resolve `repo.yarnpkg.com`. The installed Sass package compiled the stylesheet successfully, and the resulting CSS was used for the E2E site build. No Stryker tool was installed or used; this is an F# source mutation with an existing xUnit/VSTest runner.
