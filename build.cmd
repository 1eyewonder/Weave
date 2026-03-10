git rev-parse --git-dir >NUL 2>&1 && (
  echo Configuring git hooks path...
  git config core.hooksPath hooks
)

echo Restoring dotnet tools...
dotnet tool restore

echo Restoring Paket dependencies...
dotnet paket restore

dotnet run --project ./build/Weave.Build.fsproj -- -t %*