module Release

open System
open System.IO
open System.IO.Compression
open System.Reflection
open System.Security.Cryptography
open System.Text.RegularExpressions
open System.Xml.Linq

let private root = Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, ".."))

let private changelog () =
  File.ReadAllText(Path.Combine(root, "CHANGELOG.md"))

let private tag () =
  let value = Environment.GetEnvironmentVariable "RELEASE_TAG"
  let pattern = @"\Av(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-rc\.[1-9]\d*)?\z"

  if String.IsNullOrWhiteSpace value || not (Regex.IsMatch(value, pattern)) then
    failwithf "Invalid release tag: %s" value

  value

let checkBaseline _ =
  if not (Regex.IsMatch(changelog (), @"(?m)^last_commit_released: [0-9a-fA-F]{7,40}[ \t]*\r?$")) then
    failwith "Set last_commit_released in CHANGELOG.md after the first release"

let writeNotes _ =
  let version = tag ()
  let text = changelog ()

  let pattern =
    @"(?m)^##[ \t]+\[?(v?\d+\.\d+\.\d+(?:-rc\.\d+)?)\]?(?:[ \t]+-[ \t]+\d{4}-\d{2}-\d{2})?[ \t]*\r?$"

  let headings = Regex.Matches(text, pattern) |> Seq.cast<Match> |> Seq.toArray

  let matches =
    headings
    |> Array.mapi (fun i heading ->
      let value = heading.Groups[1].Value.TrimStart('v')

      let endIndex =
        if i + 1 < headings.Length then
          headings[i + 1].Index
        else
          text.Length

      value, text.Substring(heading.Index + heading.Length, endIndex - heading.Index - heading.Length).Trim())
    |> Array.filter (fun (value, _) -> value = version.Substring(1))

  match matches with
  | [| _, notes |] when not (String.IsNullOrWhiteSpace notes) ->
    File.WriteAllText(Path.Combine(root, "release-body.md"), notes + "\n")
  | _ -> failwithf "Expected exactly one nonempty changelog section for %s" version

let verifyPackage _ =
  let version = (tag ()).Substring(1)

  let path =
    Path.Combine(root, "artifacts", "package", $"Weave.WebSharper.%s{version}.nupkg")

  use archive = ZipFile.OpenRead path

  let entry name =
    match archive.GetEntry name with
    | null -> failwithf "Package is missing %s" name
    | value -> value

  let readBytes name =
    use stream = (entry name).Open()
    use output = new MemoryStream()
    stream.CopyTo output
    output.ToArray()

  entry "PACKAGE.md" |> ignore
  let assemblyBytes = readBytes "lib/netstandard2.0/Weave.dll"
  let css = readBytes "contentFiles/any/any/styles.css"
  use nuspec = (entry "Weave.WebSharper.nuspec").Open()
  let document = XDocument.Load nuspec
  let ns = document.Root.Name.Namespace
  let metadata = document.Root.Element(ns + "metadata")

  if
    metadata.Element(ns + "id").Value <> "Weave.WebSharper"
    || metadata.Element(ns + "version").Value <> version
  then
    failwith "Package ID or version does not match the release tag"

  let assembly = Assembly.Load assemblyBytes
  use embedded = assembly.GetManifestResourceStream "styles.css"

  if isNull embedded then
    failwith "The Weave assembly does not embed styles.css"

  use contents = new MemoryStream()
  embedded.CopyTo contents

  if SHA256.HashData(contents.ToArray()) <> SHA256.HashData(css) then
    failwith "Embedded and packaged stylesheets differ"
