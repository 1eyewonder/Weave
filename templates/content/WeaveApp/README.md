# WeaveStarter

This development-preview starter requires .NET 10, Node.js, Yarn and a local Weave checkout. Weave is not yet published as a NuGet package.

Install the checkout's dependencies using its documented development setup, then run:

```powershell
dotnet run -p:WeaveProjectPath="C:/path/to/Weave/src/Weave/Weave.fsproj"
```

Use an absolute path to `Weave.fsproj`. The build compiles Weave's SCSS using the checkout's Yarn setup and copies its stylesheet to this app. The generated app references the checkout and is not yet a standalone distribution. Edit `Client.fs` to start building your UI.

The mode toggle uses Weave’s F# mode helpers. Customize the page background and toggle in `wwwroot/app.css` and the UI in `Client.fs`.
