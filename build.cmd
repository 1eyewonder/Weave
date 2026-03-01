echo Restoring dotnet tools...
dotnet tool restore

echo Restoring Paket dependencies...
dotnet paket restore

dotnet run --project ./build/Weave.Build.fsproj -- -t %*