echo Restoring dotnet tools...
dotnet tool restore

dotnet run --project ./build/Weave.Build.fsproj -- -t %*