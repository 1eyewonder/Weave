# Project workspace starter

A sample project tracker with shared list and board views. Search issue titles, filter by status or assignee, open issue details, change status, and add issues. Search matches are highlighted in issue titles. The add form uses typed validation, project completion uses Weave's progress component, and a snackbar confirms new issues. All data is fictional and held in memory: refreshing the page resets your changes. No account or backend is required.

## Run locally

Requires .NET SDK 10.0.403 or a newer .NET 10 feature band. From the generated project:

```sh
dotnet run --urls http://localhost:5000
```

Open http://localhost:5000. Edit `Domain.fs` for the sample issues, `State.fs` for shared state and filtering, and `Views.fs` for Weave components and page sections. `Client.fs` starts the app; `wwwroot/workspace.css` controls its appearance.

The header's Dark mode / Light mode button uses Weave's theme API. The preview starts in light mode; the choice is not persisted across page reloads. Primary, secondary, tertiary, and dark-mode colors can be customized in the template stylesheet.


Weave component colors live in the stylesheet's `body[data-theme]` rules; set font roles on `:root`. Template-specific layout and surface styles are in the same file. See the Weave docs’ Theming page for the complete token reference.
