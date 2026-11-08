open System
open System.IO
open System.Text.RegularExpressions
open System.Xml.Linq

let root = __SOURCE_DIRECTORY__

let version =
  XDocument.Load(Path.Combine(root, "WeaveVersion.props")).Descendants(XName.Get "WeaveLibraryVersion")
  |> Seq.exactlyOne
  |> _.Value.Trim()

if not (Regex.IsMatch(version, @"\A\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?\z")) then
  failwithf "Invalid WeaveLibraryVersion: %s" version

let check = fsi.CommandLineArgs |> Array.contains "--check"

let pattern =
  Regex("(?<=<PackageReference Include=\"Weave.WebSharper\" Version=\")[^\"]+(?=\")")

for name in [ "WeaveApp"; "WeaveDashboard"; "WeaveProjects"; "WeaveRepository" ] do
  let path = Path.Combine(root, "content", name, "WeaveStarter.fsproj")
  let text = File.ReadAllText path
  let matches = pattern.Matches text

  if matches.Count <> 1 then
    failwithf "Expected one Weave.WebSharper reference in %s" path

  if matches[0].Value <> version then
    if check then
      failwithf
        "%s uses %s; expected %s. Run dotnet fsi templates/SyncWeaveVersion.fsx"
        path
        matches[0].Value
        version
    else
      File.WriteAllText(path, pattern.Replace(text, version))
      printfn "Updated %s to %s" path version
