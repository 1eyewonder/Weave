# Weave templates

Install the stable release with the .NET 10 SDK:

```sh
dotnet new install Weave.WebSharper.Templates@0.2.1
```

Create a starter with `dotnet new weave-app -n MyApp`, `dotnet new weave-dashboard -n MyDashboard`, `dotnet new weave-projects -n MyProjects`, or `dotnet new weave-repository -n MyRepository`. In the generated directory, run `dotnet run --urls http://localhost:5000` and open that URL. The generated app references `Weave.WebSharper` 0.2.0 and requires the .NET 10.0.403 SDK or a newer .NET 10 feature band. The library package must be available on the configured NuGet feed. To uninstall, run `dotnet new uninstall Weave.WebSharper.Templates`.

For local testing before publication, pack both projects and install the template `.nupkg` from `artifacts`. Restore generated apps using a feed containing the Weave library package.

## Releasing templates

Template releases use their own `templates-v` tags and versions. Set the library version in `templates/WeaveVersion.props`, then run `dotnet fsi templates/SyncWeaveVersion.fsx` to update all starters. Set the template package version in `templates/Weave.Templates/Weave.Templates.fsproj`, then merge to `main`. Packing checks that the starter versions match. Publish the library first and wait until that version is available on NuGet before pushing a new template tag matching the template package version, for example `templates-v0.1.1`. The **Release templates** workflow installs the package, restores and builds all four generated apps, checks their stylesheets, and publishes the template package and GitHub release. Configure a NuGet trusted publishing policy for `release-templates.yml`, the `nuget` environment, and `Weave.WebSharper.Templates`.

If a run fails before NuGet publication, retry **Release templates** manually with the existing tag. If NuGet succeeded but GitHub release creation failed, create the GitHub release manually; published package versions cannot be replaced.
