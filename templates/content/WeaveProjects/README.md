# Project workspace starter

A sample project tracker with shared list and board views. Search issue titles, filter by status or assignee, open issue details, change status, and add issues. All data is fictional and held in memory: refreshing the page resets your changes. No account or backend is required.

## Run locally

Prerequisites: .NET 10 SDK, Node.js and Yarn, plus a Weave checkout with its dependencies installed. Weave is not yet published as a NuGet package, so this preview requires a local project reference.

From the Weave checkout, restore its tools and dependencies:

```powershell
dotnet tool restore
dotnet paket restore
yarn install --immutable
```

Then, from this generated project's directory, supply the absolute path to the checkout's `Weave.fsproj`:

```powershell
dotnet run --project ./WeaveStarter.fsproj -p:WeaveProjectPath="C:/path/to/Weave/src/Weave/Weave.fsproj" --urls http://localhost:5000
```

Open http://localhost:5000. Edit `Domain.fs` for the sample issues, `State.fs` for shared state and filtering, and `Views.fs` for Weave components and page sections. `Client.fs` starts the app; `wwwroot/workspace.css` controls its appearance. The build compiles Weave's SCSS in the checkout and copies the stylesheet into this app.

The header's Dark mode / Light mode button uses Weave's theme API. The preview starts in light mode; the choice is not persisted across page reloads. Primary, secondary, tertiary, and dark-mode colors can be customized in the template stylesheet.


Weave component colors live in the stylesheet's `body[data-theme]` rules; set font roles on `:root`. Template-specific layout and surface styles are in the same file. See the Weave docs’ Theming page for the complete token reference.
