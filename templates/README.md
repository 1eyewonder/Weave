# Weave templates

After publication, install the release candidate:

```sh
dotnet new install Weave.WebSharper.Templates --prerelease
```

Create a starter with `dotnet new weave-app`, `weave-dashboard`, `weave-projects`, or `weave-repository`. The generated app references `Weave.WebSharper` and requires the .NET 10.0.403 SDK or a newer .NET 10 feature band. The library package must be available on the configured NuGet feed.

For local testing before publication, pack both projects and install the template `.nupkg` from `artifacts`. Restore generated apps using a feed containing the Weave library package.

## Releasing templates

Template releases use their own `templates-v` tags and versions. Set the library version in `templates/WeaveVersion.props`, then run `dotnet fsi templates/SyncWeaveVersion.fsx` to update all starters. Set the template package version in `templates/Weave.Templates/Weave.Templates.fsproj`, then merge to `main`. Packing checks that the starter versions match. Publish the library first and wait until that version is available on NuGet before pushing the template tag, for example `templates-v0.1.0-rc.1`. The **Release templates** workflow installs the package, restores and builds all four generated apps, checks their stylesheets, and publishes the template package and GitHub release. `NUGET_TOKEN` must permit publishing `Weave.WebSharper.Templates` as well as the library.

If a run fails before NuGet publication, retry **Release templates** manually with the existing tag. If NuGet succeeded but GitHub release creation failed, create the GitHub release manually; published package versions cannot be replaced.
