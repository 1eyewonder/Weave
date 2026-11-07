# Weave template package

## Planned NuGet experience

Once the template package and its Weave dependency have been published, users will be able to install a template and create an application without a repository checkout:

```bash
dotnet new install Weave.Templates
```

Choose one starter; each command creates an independent application with only that starter:

```bash
dotnet new weave-app -n MyApp
dotnet new weave-projects -n MyProjects
dotnet new weave-repository -n MyRepository
dotnet new weave-dashboard -n MyDashboard
```

For example, run the project workspace with:

```bash
cd MyProjects
dotnet run --urls http://localhost:5000
```

Deploy the generated project with your preferred hosting setup. Deployment configuration is not included; workspace sample data is fictional and resets on refresh. The published packages and this installation flow are not available yet.

## Development preview

This package generates editable F# WebSharper apps. It currently requires a local Weave checkout: the component library and its static assets have not yet been published as a standalone package. Do not publish this preview to NuGet as a production-ready template.

From the repository root, restore the worktree dependencies, pack, and install locally:

```powershell
dotnet tool restore
dotnet paket restore
yarn install --immutable
dotnet pack templates/Weave.Templates/Weave.Templates.fsproj -o ./artifacts/templates
dotnet new install ./artifacts/templates/Weave.Templates.0.1.0-preview.1.nupkg
```

Generate a starter outside the checkout:

```powershell
dotnet new weave-dashboard -n MyDashboard -o ../MyDashboard
dotnet new weave-repository -n MyRepository -o ../MyRepository
dotnet new weave-projects -n MyProjects -o ../MyProjects
dotnet new weave-app -n MyApp -o ../MyApp
```

Follow the generated README to supply `WeaveProjectPath` and run the app. The workspace's sample data is in-memory and resets on refresh. To remove the local installation, run `dotnet new uninstall Weave.Templates`.

Before a public release, replace the project reference and stylesheet copy with a versioned Weave package and verify its WebSharper and static asset behavior from a clean generated app. See [TEMPLATE-CONCEPTS.md](TEMPLATE-CONCEPTS.md) for the other proposed starters.


