# Business dashboard starter

A fictional dashboard with a time-period selector, derived overview metrics and activity breakdown, and a searchable, filterable, sortable transaction table. Select a row to see sample details. The period affects metrics and table; search and status affect the table only. All amounts, customers and transactions are fictional and relative to the current day. No payments are processed and no backend is connected.

## Run locally

Requires .NET 10 SDK, Node.js, Yarn and a Weave checkout with its dependencies installed. Weave has not yet been published as a NuGet package. From the checkout, run `dotnet tool restore`, `dotnet paket restore` and `yarn install --immutable`.

From the generated project, point to the checkout:

```powershell
dotnet run --project ./WeaveStarter.fsproj -p:WeaveProjectPath="C:/path/to/Weave/src/Weave/Weave.fsproj" --urls http://localhost:5000
```

Open http://localhost:5000. Edit `Domain.fs` to replace the sample transactions, `State.fs` for filtering and selection, and `Views.fs` for the metrics, Weave table, and details panel. `Client.fs` starts the app. Change colors and layout in `wwwroot/dashboard.css`. The mode button uses Weave's theming API; the choice starts as light and is not persisted across reloads.


Weave component colors live in the stylesheet's `body[data-theme]` rules; set font roles on `:root`. Template-specific layout and surface styles are in the same file. See the Weave docs’ Theming page for the complete token reference.
