# Repository workspace starter

A fictional repository workspace with sample branches, folders, files, and issues. Switch between Code and Issues, choose a branch, open folders and files, use the breadcrumb to navigate, or search and filter issues and open their details. All content is local sample data; this starter does not access a Git repository, GitHub, or a backend.

## Run locally

Requires .NET 10 SDK, Node.js, Yarn, and a Weave checkout with its dependencies installed. Weave has not yet been published as a NuGet package.

From the Weave checkout, run `dotnet tool restore`, `dotnet paket restore`, and `yarn install --immutable`. Then, from the generated project:

```powershell
dotnet run --project ./WeaveStarter.fsproj -p:WeaveProjectPath="C:/path/to/Weave/src/Weave/Weave.fsproj" --urls http://localhost:5000
```

Open http://localhost:5000. Edit `Domain.fs` to replace the fictional repository data, `State.fs` for navigation and filter state, and `Views.fs` for Weave components and page sections. `Client.fs` starts the app. Customize the palette in `wwwroot/repository.css`.

The header's Dark mode / Light mode button uses Weave's theme API. The preview starts in light mode; the choice is not persisted across page reloads. Primary, secondary, tertiary, and dark-mode colors can be customized in the template stylesheet.


Weave component colors live in the stylesheet's `body[data-theme]` rules; set font roles on `:root`. Template-specific layout and surface styles are in the same file. See the Weave docs’ Theming page for the complete token reference.
