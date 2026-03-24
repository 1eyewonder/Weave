namespace Weave.Docs.Examples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

open Weave
open Weave.Tabs
open Weave.Icons
open Weave.Icons.MaterialSymbols

[<JavaScript>]
module ShowcaseCharacterSheet =

  [<JavaScript; Struct>]
  type Race =
    | Human
    | Elf
    | Dwarf
    | Halfling
    | Dragonborn
    | Tiefling

  [<JavaScript; Struct>]
  type CharacterClass =
    | Fighter
    | Wizard
    | Rogue
    | Cleric
    | Ranger
    | Bard

  [<JavaScript; Struct>]
  type Ability =
    | Strength
    | Dexterity
    | Constitution
    | Intelligence
    | Wisdom
    | Charisma

  [<JavaScript>]
  type SkillDef = {
    Name: string
    Ability: Ability
    Proficient: Var<bool>
  }

  let private abilityModifier (score: int) = (score - 10) / 2

  let private proficiencyBonus (level: int) = (level - 1) / 4 + 2

  let private skillBonus (abilityScore: int) (level: int) (isProficient: bool) =
    abilityModifier abilityScore
    + (if isProficient then proficiencyBonus level else 0)

  let private formatModifier (m: int) =
    if m >= 0 then sprintf "+%d" m else sprintf "%d" m

  let private abilityToString (a: Ability) =
    match a with
    | Strength -> "STR"
    | Dexterity -> "DEX"
    | Constitution -> "CON"
    | Intelligence -> "INT"
    | Wisdom -> "WIS"
    | Charisma -> "CHA"

  let private fullAppSection () =
    // Character identity
    let charName = Var.Create ""
    let race = Var.Create<string option> None
    let charClass = Var.Create<string option> None
    let level = Var.Create 1

    // Ability scores
    let str = Var.Create 10
    let dex = Var.Create 10
    let con = Var.Create 10
    let int' = Var.Create 10
    let wis = Var.Create 10
    let cha = Var.Create 10

    let abilityVar (a: Ability) =
      match a with
      | Strength -> str
      | Dexterity -> dex
      | Constitution -> con
      | Intelligence -> int'
      | Wisdom -> wis
      | Charisma -> cha

    // Hit points
    let maxHp = Var.Create 10
    let currentHp = Var.Create 10

    // Skills
    let skills = [
      {
        Name = "Acrobatics"
        Ability = Dexterity
        Proficient = Var.Create false
      }
      {
        Name = "Animal Handling"
        Ability = Wisdom
        Proficient = Var.Create false
      }
      {
        Name = "Arcana"
        Ability = Intelligence
        Proficient = Var.Create false
      }
      {
        Name = "Athletics"
        Ability = Strength
        Proficient = Var.Create false
      }
      {
        Name = "Deception"
        Ability = Charisma
        Proficient = Var.Create false
      }
      {
        Name = "History"
        Ability = Intelligence
        Proficient = Var.Create false
      }
      {
        Name = "Insight"
        Ability = Wisdom
        Proficient = Var.Create false
      }
      {
        Name = "Intimidation"
        Ability = Charisma
        Proficient = Var.Create false
      }
      {
        Name = "Investigation"
        Ability = Intelligence
        Proficient = Var.Create false
      }
      {
        Name = "Medicine"
        Ability = Wisdom
        Proficient = Var.Create false
      }
      {
        Name = "Nature"
        Ability = Intelligence
        Proficient = Var.Create false
      }
      {
        Name = "Perception"
        Ability = Wisdom
        Proficient = Var.Create false
      }
      {
        Name = "Performance"
        Ability = Charisma
        Proficient = Var.Create false
      }
      {
        Name = "Persuasion"
        Ability = Charisma
        Proficient = Var.Create false
      }
      {
        Name = "Religion"
        Ability = Intelligence
        Proficient = Var.Create false
      }
      {
        Name = "Sleight of Hand"
        Ability = Dexterity
        Proficient = Var.Create false
      }
      {
        Name = "Stealth"
        Ability = Dexterity
        Proficient = Var.Create false
      }
      {
        Name = "Survival"
        Ability = Wisdom
        Proficient = Var.Create false
      }
    ]

    // Dice roller
    let diceDialogOpen = Var.Create false
    let lastRoll = Var.Create<string option> None
    let random = System.Random()

    let rollDie (sides: int) =
      let result = random.Next(1, sides + 1)
      Var.Set lastRoll (Some(sprintf "d%d: %d" sides result))

    let raceItems =
      [ "Human"; "Elf"; "Dwarf"; "Halfling"; "Dragonborn"; "Tiefling" ]
      |> List.map (fun r -> SelectItem.create (text r, r, r))
      |> View.Const

    let classItems =
      [ "Fighter"; "Wizard"; "Rogue"; "Cleric"; "Ranger"; "Bard" ]
      |> List.map (fun c -> SelectItem.create (text c, c, c))
      |> View.Const

    let identityTab =
      div [ Padding.All.small ] [
        Grid.create (
          [
            GridItem.create (
              Field.create (
                charName,
                variant = Field.Variant.Outlined,
                labelText = View.Const "Character Name",
                placeholder = View.Const "Enter your hero's name",
                attrs = [ Field.Color.primary; Field.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve ]
            )

            GridItem.create (
              Select.create (
                raceItems,
                race,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Race",
                placeholder = View.Const "Choose a race",
                attrs = [ Select.Color.primary; Select.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )

            GridItem.create (
              Select.create (
                classItems,
                charClass,
                variant = Select.Variant.Outlined,
                labelText = View.Const "Class",
                placeholder = View.Const "Choose a class",
                attrs = [ Select.Color.primary; Select.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )

            GridItem.create (
              NumericField.create (
                level,
                labelText = View.Const "Level",
                min = 1,
                max = 20,
                attrs = [ Field.Color.primary; Field.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
            )
          ]
        )

        // Proficiency bonus display
        div [ Margin.Top.small; Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
          div [ Typography.subtitle2 ] [ text "Proficiency Bonus:" ]

          div [ Typography.h6; BrandColor.toColor BrandColor.Primary ] [
            textView (level.View |> View.MapCached(proficiencyBonus >> formatModifier))
          ]
        ]
      ]

    let abilityCard (ability: Ability) (score: Var<int>) =
      div [
        SurfaceColor.toBackgroundColor SurfaceColor.Surface
        Padding.All.small
        BorderRadius.All.small
        Elevation.e1
        Flex.Flex.allSizes
        FlexDirection.Column.allSizes
        AlignItems.center
        Gap.All.g2
      ] [
        div [ Typography.overline; Typography.Color.textSecondary ] [ text (abilityToString ability) ]

        NumericField.create (
          score,
          min = 1,
          max = 30,
          attrs = [ Field.Color.primary; Attr.Style "max-width" "100px"; Typography.Align.center ]
        )

        div [ Typography.h5; BrandColor.toColor BrandColor.Primary ] [
          textView (score.View |> View.MapCached(abilityModifier >> formatModifier))
        ]

        div [ Typography.caption; Typography.Color.textSecondary ] [ text "modifier" ]
      ]

    let abilitiesTab =
      div [ Padding.All.small ] [
        Grid.create (
          [
            GridItem.create (
              abilityCard Strength str,
              attrs = [ GridItem.Span.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (
              abilityCard Dexterity dex,
              attrs = [ GridItem.Span.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (
              abilityCard Constitution con,
              attrs = [ GridItem.Span.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (
              abilityCard Intelligence int',
              attrs = [ GridItem.Span.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (abilityCard Wisdom wis, attrs = [ GridItem.Span.six; GridItem.Span.Medium.four ])
            GridItem.create (
              abilityCard Charisma cha,
              attrs = [ GridItem.Span.six; GridItem.Span.Medium.four ]
            )
          ],
          attrs = [ AlignItems.stretch ]
        )
      ]

    let hitPointsTab =
      div [ Padding.All.small ] [
        Grid.create (
          [
            GridItem.create (
              div [
                SurfaceColor.toBackgroundColor SurfaceColor.Surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Max Hit Points" ]

                NumericField.create (
                  maxHp,
                  min = 1,
                  max = 999,
                  attrs = [ Field.Color.primary; Field.Width.full ]
                )
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )

            GridItem.create (
              div [
                SurfaceColor.toBackgroundColor SurfaceColor.Surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Current Hit Points" ]

                div [ Margin.Bottom.extraSmall ] [
                  (currentHp.View, maxHp.View)
                  ||> View.Map2(fun curr maxVal ->
                    Slider.create (
                      currentHp,
                      min = 0,
                      max = maxVal,
                      labelText =
                        ((currentHp.View, maxHp.View) ||> View.Map2(fun c m -> sprintf "HP: %d / %d" c m)),
                      attrs = [
                        let pct =
                          if maxVal > 0 then
                            float curr / float maxVal * 100.0
                          else
                            100.0

                        if pct > 50.0 then Slider.Color.success
                        elif pct > 25.0 then Slider.Color.warning
                        else Slider.Color.error
                      ]
                    ))
                  |> Doc.EmbedView
                ]
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )
          ],
          attrs = [ AlignItems.stretch ]
        )

        // Quick HP buttons
        div [ Flex.Flex.allSizes; JustifyContent.center; Gap.All.g2; Margin.Top.small ] [
          Button.create (
            text "Heal +5",
            onClick = (fun () -> Var.Set currentHp (min maxHp.Value (currentHp.Value + 5))),
            attrs = [ Button.Color.success; Button.Variant.outlined ]
          )

          Button.create (
            text "Damage -5",
            onClick = (fun () -> Var.Set currentHp (max 0 (currentHp.Value - 5))),
            attrs = [ Button.Color.error; Button.Variant.outlined ]
          )

          Button.create (
            text "Full Heal",
            onClick = (fun () -> Var.Set currentHp maxHp.Value),
            attrs = [ Button.Color.success; Button.Variant.filled ]
          )
        ]
      ]

    let skillsTab =
      div [ Padding.All.small ] [
        div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2; Margin.Bottom.small ] [
          div [ Typography.subtitle2 ] [ text "Proficiency Bonus:" ]

          div [ Typography.body1; BrandColor.toColor BrandColor.Primary ] [
            textView (level.View |> View.MapCached(proficiencyBonus >> formatModifier))
          ]
        ]

        for skill in skills do
          let abilityScore = abilityVar skill.Ability

          div [
            Flex.Flex.allSizes
            AlignItems.center
            Gap.All.g2
            Padding.Vertical.extraSmall
            Attr.Style "border-bottom" "1px solid var(--palette-border)"
          ] [
            Checkbox.create (
              skill.Proficient,
              View.Const "",
              attrs = [ Checkbox.Color.primary; Density.compact ]
            )

            div [ FlexItem.Flex.allSizes; Typography.body2 ] [ text skill.Name ]

            div [ Typography.caption; Typography.Color.textSecondary ] [
              text (abilityToString skill.Ability)
            ]

            div [
              Typography.subtitle2
              BrandColor.toColor BrandColor.Primary
              Attr.Style "min-width" "30px"
              Typography.Align.center
            ] [
              textView (
                (abilityScore.View, level.View, skill.Proficient.View)
                |||> View.Map3(fun score lvl prof -> skillBonus score lvl prof |> formatModifier)
              )
            ]
          ]
      ]

    let diceTab =
      div [ Padding.All.small ] [
        div [ Typography.subtitle2; Margin.Bottom.small ] [ text "Roll the dice!" ]

        Grid.create (
          [ 4; 6; 8; 10; 12; 20 ]
          |> List.map (fun sides ->
            GridItem.create (
              Button.create (
                div [
                  Flex.Flex.allSizes
                  FlexDirection.Column.allSizes
                  AlignItems.center
                  Gap.All.g1
                ] [
                  Icon.create (Icon.Travel Travel.Casino, attrs = [ Attr.Style "font-size" "24px" ])
                  text (sprintf "d%d" sides)
                ],
                onClick = (fun () -> rollDie sides),
                attrs = [
                  Button.Color.primary
                  Button.Variant.outlined
                  Attr.Style "min-width" "80px"
                  Attr.Style "min-height" "80px"
                ]
              ),
              attrs = [ GridItem.Span.four; GridItem.Span.Small.four; GridItem.Span.Medium.two ]
            ))
        )

        // Roll result display
        div [ Flex.Flex.allSizes; JustifyContent.center; Margin.Top.small ] [
          lastRoll.View
          |> Doc.BindView(fun roll ->
            match roll with
            | Some result ->
              div [
                SurfaceColor.toBackgroundColor SurfaceColor.Surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e2
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.center
                Attr.Style "min-width" "150px"
              ] [
                div [ Typography.overline; Typography.Color.textSecondary ] [ text "Result" ]
                div [ Typography.h3; BrandColor.toColor BrandColor.Primary ] [ text result ]
              ]
            | None ->
              div [ Typography.body2; Typography.Color.textSecondary ] [ text "Click a die to roll!" ])
        ]
      ]

    let description =
      Helpers.bodyText
        "The complete D&D Character Sheet. Switch between tabs to manage your character's identity, ability scores, hit points, skills, and roll dice."

    let content =
      div [] [
        Tabs.create (
          View.Const [
            TabItem.create (
              "Identity",
              identityTab,
              startIcon = Icon.create (Icon.Social Social.Person, attrs = [ Attr.Style "font-size" "18px" ])
            )
            TabItem.create (
              "Abilities",
              abilitiesTab,
              startIcon =
                Icon.create (Icon.Activities Activities.Exercise, attrs = [ Attr.Style "font-size" "18px" ])
            )
            TabItem.create (
              "Hit Points",
              hitPointsTab,
              startIcon =
                Icon.create (Icon.Activities Activities.EcgHeart, attrs = [ Attr.Style "font-size" "18px" ])
            )
            TabItem.create (
              "Skills",
              skillsTab,
              startIcon = Icon.create (Icon.Action Action.Stars, attrs = [ Attr.Style "font-size" "18px" ])
            )
            TabItem.create (
              "Dice",
              diceTab,
              startIcon = Icon.create (Icon.Travel Travel.Casino, attrs = [ Attr.Style "font-size" "18px" ])
            )
          ],
          attrs = [ Color.primary ]
        )
      ]

    Helpers.section "The Full App" description content

  let private domainModelSection () =
    let description =
      Helpers.bodyText
        "The character sheet is built on rich F# domain types. Discriminated unions for Race, Class, and Ability give us exhaustive pattern matching — the compiler ensures every ability has a score, every skill maps to an ability."

    let content = Doc.Empty

    let code =
      """[<Struct>] type Race = Human | Elf | Dwarf | Halfling | Dragonborn | Tiefling
[<Struct>] type CharacterClass = Fighter | Wizard | Rogue | Cleric | Ranger | Bard
[<Struct>] type Ability = Strength | Dexterity | Constitution | Intelligence | Wisdom | Charisma

type SkillDef = {
    Name: string
    Ability: Ability
    Proficient: Var<bool>   // Two-way binding to Checkbox
}

let skills = [
    { Name = "Acrobatics"; Ability = Dexterity; Proficient = Var.Create false }
    { Name = "Athletics"; Ability = Strength; Proficient = Var.Create false }
    { Name = "Arcana"; Ability = Intelligence; Proficient = Var.Create false }
    // ... 18 skills, each mapped to its governing ability
]"""

    Helpers.codeSampleSection "Domain Modeling" description content code

  let private abilityScoresSection () =
    let description =
      Helpers.bodyText
        "Ability modifiers are pure functions — no state, no side effects. The modifier is computed reactively via View.MapCached, so it updates instantly as you change the score. Each ability card is a reusable function that takes an Ability DU and a Var<int>."

    let content = Doc.Empty

    let code =
      """// Pure functions — the heart of the character math
let abilityModifier (score: int) = (score - 10) / 2
let proficiencyBonus (level: int) = (level - 1) / 4 + 2
let skillBonus abilityScore level isProficient =
    abilityModifier abilityScore
    + (if isProficient then proficiencyBonus level else 0)

let formatModifier (m: int) =
    if m >= 0 then sprintf "+%d" m else sprintf "%d" m

// Reactive ability card — reusable for all 6 abilities
let abilityCard (ability: Ability) (score: Var<int>) =
    div [ Elevation.e1; Padding.All.small ] [
        div [ Typography.overline ] [ text (abilityToString ability) ]
        NumericField.create(score, min = 1, max = 30)
        div [ Typography.h5; BrandColor.toColor BrandColor.Primary ] [
            textView (score.View
                |> View.MapCached(abilityModifier >> formatModifier))
        ]
    ]

// 6 ability cards in a grid
Grid.create([
    GridItem.create(abilityCard Strength str)
    GridItem.create(abilityCard Dexterity dex)
    GridItem.create(abilityCard Constitution con)
    GridItem.create(abilityCard Intelligence int')
    GridItem.create(abilityCard Wisdom wis)
    GridItem.create(abilityCard Charisma cha)
])"""

    Helpers.codeSampleSection "Ability Scores" description content code

  let private hitPointsSection () =
    let description =
      Helpers.bodyText
        "Hit points use a Slider whose color changes reactively based on HP percentage — green when healthy, yellow when hurt, red when critical. The slider's max updates when maxHP changes. Quick action buttons use simple Var.Set with clamping."

    let content = Doc.Empty

    let code =
      """let maxHp = Var.Create 10
let currentHp = Var.Create 10

// Color-coded HP slider — changes with percentage
(currentHp.View, maxHp.View)
||> View.Map2(fun curr maxVal ->
    Slider.create(
        currentHp,
        min = 0,
        max = maxVal,
        labelText = ((currentHp.View, maxHp.View)
            ||> View.Map2(fun c m -> sprintf "HP: %d / %d" c m)),
        attrs = [
            let pct = float curr / float maxVal * 100.0
            if pct > 50.0 then Slider.Color.success
            elif pct > 25.0 then Slider.Color.warning
            else Slider.Color.error
        ]))
|> Doc.EmbedView

// Quick action buttons with clamping
Button.create(
    text "Heal +5",
    onClick = (fun () ->
        Var.Set currentHp (min maxHp.Value (currentHp.Value + 5))),
    attrs = [ Button.Color.success; Button.Variant.outlined ])

Button.create(
    text "Damage -5",
    onClick = (fun () ->
        Var.Set currentHp (max 0 (currentHp.Value - 5))),
    attrs = [ Button.Color.error; Button.Variant.outlined ])"""

    Helpers.codeSampleSection "Hit Points" description content code

  let private skillsSection () =
    let description =
      Helpers.bodyText
        "Each skill has a Checkbox for proficiency and a computed bonus that combines the governing ability modifier with the proficiency bonus. View.Map3 merges the three reactive sources (ability score, level, proficiency) into a single derived value."

    let content = Doc.Empty

    let code =
      """// Each skill row: checkbox + name + ability tag + computed bonus
for skill in skills do
    let abilityScore = abilityVar skill.Ability

    div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
        // Proficiency checkbox — two-way bound
        Checkbox.create(
            skill.Proficient,
            View.Const "",
            attrs = [ Checkbox.Color.primary; Density.compact ])

        div [ FlexItem.Flex.allSizes ] [ text skill.Name ]

        // Ability abbreviation tag
        div [ Typography.caption ] [ text (abilityToString skill.Ability) ]

        // Computed skill bonus — reactive over 3 inputs
        div [ Typography.subtitle2; BrandColor.toColor BrandColor.Primary ] [
            textView (
                (abilityScore.View, level.View, skill.Proficient.View)
                |||> View.Map3(fun score lvl prof ->
                    skillBonus score lvl prof |> formatModifier))
        ]
    ]"""

    Helpers.codeSampleSection "Skills & Proficiencies" description content code

  let private diceRollerSection () =
    let description =
      Helpers.bodyText
        "A simple dice roller using F#'s System.Random — one button per die type (d4 through d20). The result is displayed reactively. No Dialog needed here — the dice panel lives right in the Tabs content."

    let content = Doc.Empty

    let code =
      """let lastRoll = Var.Create<string option> None
let random = System.Random()

let rollDie (sides: int) =
    let result = random.Next(1, sides + 1)
    Var.Set lastRoll (Some (sprintf "d%d: %d" sides result))

// Die buttons in a grid
Grid.create(
    [ 4; 6; 8; 10; 12; 20 ]
    |> List.map (fun sides ->
        GridItem.create(
            Button.create(
                div [ FlexDirection.Column.allSizes; AlignItems.center ] [
                    Icon.create(Icon.Travel Travel.Casino)
                    text (sprintf "d%d" sides)
                ],
                onClick = (fun () -> rollDie sides),
                attrs = [ Button.Color.primary; Button.Variant.outlined ]
            ))))

// Reactive result display
lastRoll.View
|> Doc.BindView(fun roll ->
    match roll with
    | Some result ->
        div [ Typography.h3; BrandColor.toColor BrandColor.Primary ] [
            text result
        ]
    | None ->
        div [ Typography.body2 ] [ text "Click a die to roll!" ])"""

    Helpers.codeSampleSection "Dice Roller" description content code

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Character Sheet"

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "A D&D-inspired character sheet with ability scores, hit points, skills, and a dice roller. This showcase is the ultimate demonstration of F# domain modeling with discriminated unions, pure computation functions, tabbed layouts, and rich component composition."
        ]

        Helpers.divider ()
        fullAppSection ()
        Helpers.divider ()
        domainModelSection ()
        Helpers.divider ()
        abilityScoresSection ()
        Helpers.divider ()
        hitPointsSection ()
        Helpers.divider ()
        skillsSection ()
        Helpers.divider ()
        diceRollerSection ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
