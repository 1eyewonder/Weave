#load "../../src/Weave.Docs/examples/shared/SourceString.fs"
#load "../../src/Weave.Docs/examples/text-field/TextFieldAppearance.fs"

open Weave.Docs.Examples.TextFieldAppearance

let mutable checks = 0

let equal label expected actual =
  checks <- checks + 1

  if expected <> actual then
    failwithf "%s: expected %A, got %A" label expected actual

let minimal =
  "let name = Var.Create \"\"\n\nTextField.singleLine (name, labelText = View.Const \"Name\")"

equal "default source is minimal" minimal (source defaults "")
equal "defaults need no explicit attributes" [] (settings defaults)
equal "all supported appearances are covered" 448 configurations.Length

equal
  "each appearance produces distinct source"
  454
  (sourceCases () |> List.map snd |> Set.ofList |> Set.count)

let withPlaceholder = { defaults with Placeholder = true }

equal
  "placeholder is shown when selected"
  true
  ((source withPlaceholder "").Contains "placeholder = View.Const \"Ada Lovelace\"")

equal "default omits placeholder" false ((source defaults "").Contains "placeholder =")

let escapedValue = source defaults "Ada \"Q\" \\ folder"
equal "typed quotes and slashes are escaped" true (escapedValue.Contains "Ada \\\"Q\\\" \\\\ folder")
let longValue = source defaults (String.replicate 100 "a")
equal "long values are wrapped" true (longValue.Split '\n' |> Array.forall (fun line -> line.Length < 80))

let outlinedAbove = {
  defaults with
      Variant = Variant.Outlined
      LabelPosition = LabelPosition.Above
}

equal "outlined supports an above label" outlinedAbove (normalise outlinedAbove)

let unsupportedAbove = { outlinedAbove with Variant = Variant.Filled }

equal
  "changing variant returns to floating labels"
  LabelPosition.Floating
  (normalise unsupportedAbove).LabelPosition

equal
  "normalising a label preserves other appearance settings"
  { unsupportedAbove with LabelPosition = LabelPosition.Floating }
  (normalise unsupportedAbove)

equal
  "source normalises unsupported combinations"
  (source { defaults with Variant = Variant.Filled } "")
  (source unsupportedAbove "")

let complete = {
  outlinedAbove with
      Color = Color.Secondary
      Width = Width.Full
}

equal
  "source and preview share the same ordered settings"
  [
    Setting.Outlined
    Setting.Accent Color.Secondary
    Setting.FullWidth
    Setting.LabelAbove
  ]
  (settings complete)

for title, code in sourceCases () do
  let lines = code.Split '\n'
  equal (title + " readable width") true (lines |> Array.forall (fun line -> line.Length < 80))
  equal (title + " self contained state") true (code.Contains "let name =")
  equal (title + " excludes preview framing") false (code.Contains "div [" || code.Contains "Select.create")

printfn "%d Text Field appearance checks passed." checks
