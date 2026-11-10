# Business dashboard starter

A fictional dashboard with a time-period selector, derived overview metrics and activity breakdown, and a searchable, filterable, sortable, paginated transaction table. Search matches are highlighted in customer names and references, and the success-rate metric uses Weave's progress component. Select a row to see sample details. The period affects metrics and table; search and status affect the table only. All amounts, customers and transactions are fictional and relative to the current day. No payments are processed and no backend is connected.

## Run locally

Requires .NET SDK 10.0.403 or a newer .NET 10 feature band. From the generated project:

```sh
dotnet run --urls http://localhost:5000
```

Open http://localhost:5000. Edit `Domain.fs` to replace the sample transactions, `State.fs` for filtering and selection, and `Views.fs` for the metrics, Weave table, and details panel. `Client.fs` starts the app. Change colors and layout in `wwwroot/dashboard.css`. The mode button uses Weave's theming API; the choice starts as light and is not persisted across reloads.


Weave component colors live in the stylesheet's `body[data-theme]` rules; set font roles on `:root`. Template-specific layout and surface styles are in the same file. See the Weave docs’ Theming page for the complete token reference.
