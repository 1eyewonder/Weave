#load "../../src/Weave.Docs/examples/tabs/TabsAppearanceSource.fs"

open Weave.Docs

module Source = TabsAppearanceSource

let check condition message =
  if not condition then
    failwith message

let initial = Source.render Source.defaults 0

check
  (initial.Contains("attrs = [ Tabs.Color.primary ]"))
  "Primary is an explicit choice, not the native color default."

check (not (initial.Contains("Variant.text"))) "Default text styling should be omitted."
check (not (initial.Contains("Density.standard"))) "Default density should be omitted."
check (not (initial.Contains("position ="))) "Default top position should be omitted."
check (not (initial.Contains("Var.Create"))) "The default first tab needs no explicit state."

let selected = Source.render Source.defaults 2
check (selected.Contains("let selectedTab = Var.Create 2")) "The selected panel must be reproducible."
check (selected.Contains("activeIndex = selectedTab")) "Selected state must be bound to the component."

let filled = {
  Source.defaults with
      Variant = Source.Variant.Filled
      Color = Source.Color.Warning
      Density = Source.Density.Compact
      Position = Source.Position.Left
      Tray = Source.Tray.Matching
}

let configured = Source.render filled 1
check (configured.Contains("Tabs.HeaderBackground.warning")) "The tray must follow the chosen color."

check
  (configured.Contains("position = Tabs.Position.Left"))
  "Vertical placement must use the actual orientation parameter."

check (configured.Contains("Density.compact")) "Density must be represented in source."

let plain = Source.render { filled with Variant = Source.Variant.Text } 0
check (not (plain.Contains("HeaderBackground"))) "A text tab must not inherit a stale filled tray setting."

let iconAndCenter = {
  Source.defaults with
      Alignment = Source.Alignment.Center
      Icons = Source.Icons.End
}

let configuredIcons = Source.render iconAndCenter 0
check (configuredIcons.Contains("Tabs.Alignment.center")) "Centering must be represented in source."
check (configuredIcons.Contains("endIcon = Icon.create")) "End icons must be represented in source."
check (configuredIcons.Contains("open Weave.Icons")) "Icon imports must be included."

check
  (configuredIcons.Contains("Icon.create (Icon.UiActions UiActions.Home)"))
  "Icon values must use the actual union constructor syntax."

for position in [ Source.Position.Start; Source.Position.End ] do
  let source = Source.render { Source.defaults with Position = position } 0

  check
    (source.Contains("position = Tabs.Position." + Source.positionName position))
    "Logical positions must be represented in the generated source."

let cases = Source.cases ()
check (cases.Length = 1620) "Baseline combinations and the new option cross-product must be checked."

for title, source in cases do
  check
    (source.Split('\n') |> Array.forall (fun line -> line.Length < 80))
    (title + ": snippet lines exceed the width limit.")

  check
    (not (
      source.Contains("Helpers.")
      || source.Contains("GridItem.")
      || source.Contains("Select.create")
    ))
    (title + ": preview controls or layout leaked into copied code.")

for invalidIndex in [ -1; 3 ] do
  let rejected =
    try
      Source.render Source.defaults invalidIndex |> ignore
      false
    with :? System.ArgumentException ->
      true

  check rejected "Invalid selected panels must be rejected."

printfn "Validated %d Tabs appearance source cases and default/selection contracts." cases.Length
