# Release playbook

## ShipIt baseline

The stable `v0.1.0` release is published, and `CHANGELOG.md` records its tagged commit as `last_commit_released`. After each library release, update that value to the released tag's commit before preparing the next release. In GitHub **Settings → Actions → General**, allow Actions to create pull requests.

## Subsequent releases

1. On GitHub, open **Actions → Prepare release → Run workflow**, select `main`, and click **Run workflow**. `prepare-release.yml` runs ShipIt, which opens or updates a release PR.
2. Review the changelog and WebSharper compatibility notes. Merge the PR into `main`.
3. Tag its merge commit with the version in the changelog, for example:

   ```sh
   git checkout main
   git pull --ff-only
   git tag -a v0.1.1 -m "v0.1.1"
   git push origin v0.1.1
   ```

4. Watch **Actions → Release** and confirm the package and GitHub release were published.

For a stable release, remove `pre_release: rc` from the changelog front matter before running ShipIt. Tags must point to a commit reachable from `main`.

## Release templates

1. Set the library version in `templates/WeaveVersion.props`, then run `dotnet fsi templates/SyncWeaveVersion.fsx` to update the starters. Set the template package version in `templates/Weave.Templates/Weave.Templates.fsproj` and merge the changes to `main`.
2. Publish the referenced `Weave.WebSharper` version and wait until it is available on NuGet. Confirm the NuGet trusted-publishing policy for `release-templates.yml`, environment `nuget`, and package `Weave.WebSharper.Templates` is configured.
3. Tag the template release commit with the template package version:

   ```sh
   git checkout main
   git pull --ff-only
   git tag -a templates-v0.1.1 -m "templates-v0.1.1"
   git push origin templates-v0.1.1
   ```

4. Watch **Actions → Release templates**. It packs the templates, restores and builds each generated app, checks the stylesheets, and publishes the package and GitHub release.

The library and template workflows run independently. If releasing both, wait for the library to appear on NuGet before pushing the template tag.

## After publication: update and push documentation

After the relevant packages are available from NuGet and their GitHub releases have succeeded:

1. Check the versioned install commands and package links in `README.md`, `templates/README.md`, and `src/Weave.Docs/examples/GettingStartedExamples.fs`. Update the library and template references to their latest published versions, including prerelease flags when appropriate. Search the remaining documentation and examples for stale package versions and release-status language.
2. For a library release, update `last_commit_released` in `CHANGELOG.md` to the commit referenced by the published library tag. Do not advance the baseline before publication.
3. Verify the commands and links against the published package versions, then commit and push the documentation and baseline changes to `main` through the repository's normal review process.
4. Wait for CI and the documentation deployment to succeed. Check the deployed getting-started page and repository README to confirm users see the updated instructions.

If the library and template packages are released separately, update only the references for the package that has actually been published, and repeat this checklist for the other package.

## Workflows

- **Prepare release** (`prepare-release.yml`): run manually to have ShipIt propose a changelog and version in a pull request. It does not publish or create a release tag.
- **Release** (`release.yml`): runs when you push a `v` tag; it tests, packages, publishes to NuGet, and creates the GitHub release. It can also be run manually to retry an existing tag.
- **Release templates** (`release-templates.yml`): runs when you push a `templates-v` tag; it validates and publishes the template package. It can also retry an existing tag manually.

Publishing uses NuGet trusted publishing via `NuGet/login@v1` and the `nuget` GitHub Actions environment. Configure a separate NuGet policy for each workflow (`release.yml` for `Weave.WebSharper` and `release-templates.yml` for `Weave.WebSharper.Templates`); the workflows do not use a stored `NUGET_TOKEN` secret.

## Commit prefixes

| Prefix                                                                        | Use for                   | Stable version effect   |
| ----------------------------------------------------------------------------- | ------------------------- | ----------------------- |
| `feat:`                                                                       | A new feature             | Minor                   |
| `fix:`                                                                        | A bug fix                 | Patch                   |
| `perf:`                                                                       | A performance improvement | Minor                   |
| `feat!:` or `fix!:`                                                           | A breaking change         | Major                   |
| `docs:`, `test:`, `refactor:`, `style:`, `build:`, `ci:`, `chore:`, `revert:` | Other changes             | No release on their own |

Examples: `feat(tabs): add keyboard navigation` and `fix: correct dialog focus`. ShipIt uses `feat`, `fix`, `perf`, and breaking changes for release notes. During a prerelease cycle, ShipIt increments its RC number instead of applying the stable-version bumps above.

## Working between releases

- Use these prefixes for commits and squash merges.
- In each clone, run `dotnet tool restore` and `git config core.hooksPath hooks` to enable the commit-message hook. `build.sh` and `build.cmd` also configure the hook path.
- Preview ShipIt with `dotnet shipit --dry-run --skip-merge-commit`. Fix invalid commit messages rather than silently skipping them.

## If a release fails

- Before NuGet publishes: rerun **Actions → Release** using the existing tag.
- For templates, use **Actions → Release templates** with the existing template tag instead.
- After NuGet publishes but GitHub release creation fails: create the GitHub release manually with the matching notes and verified package. Check NuGet before retrying an ambiguous push; published versions cannot be replaced.

## Package checks

The tested baseline is WebSharper and WebSharper.FSharp `10.1.9.686` and WebSharper.UI `10.1.7.686`. Add a reproducible lock and CI coverage before claiming another combination.

A normal Release pack generates and embeds CSS and includes a content-file copy. The release checks that they match. Do not use `dotnet pack --no-build` for releases; applications must link the extracted stylesheet.
