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

  let private abilityModifier score = int (floor (float (score - 10) / 2.0))

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
      div [ Padding.All.extraSmall ] [
        Grid.create (
          [
            GridItem.create (
              TextField.singleLine (
                charName,
                labelText = View.Const "Character Name",
                placeholder = View.Const "Enter your hero's name",
                attrs = [ TextField.Variant.outlined; TextField.Color.primary; TextField.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve ]
            )

            GridItem.create (
              Select.create (
                raceItems,
                race,
                labelText = View.Const "Race",
                placeholder = View.Const "Choose a race",
                attrs = [ Select.Variant.outlined; Select.Color.primary; Select.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )

            GridItem.create (
              Select.create (
                classItems,
                charClass,
                labelText = View.Const "Class",
                placeholder = View.Const "Choose a class",
                attrs = [ Select.Variant.outlined; Select.Color.primary; Select.Width.full ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )

            GridItem.create (
              NumericField.create (
                level,
                labelText = View.Const "Level",
                min = 1,
                max = 20,
                attrs = [
                  NumericField.Variant.outlined
                  NumericField.Color.primary
                  NumericField.Width.full
                ]
              ),
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.four ]
            )
          ]
        )

        // Proficiency bonus display
        div [ Margin.Top.small; Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
          div [ Typography.subtitle2 ] [ text "Proficiency Bonus:" ]

          div [ Typography.h6; BrandColor.TextColor.primary ] [
            textView (level.View |> View.MapCached(proficiencyBonus >> formatModifier))
          ]
        ]
      ]

    let abilityCard (ability: Ability) (score: Var<int>) =
      div [
        SurfaceColor.BackgroundColor.surface
        Padding.All.extraSmall
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
          attrs = [
            NumericField.Variant.outlined
            NumericField.Color.primary
            NumericField.Width.full
            Attr.Style "max-width" "100px"
            Typography.Align.center
          ]
        )

        div [ Typography.h5; BrandColor.TextColor.primary ] [
          textView (score.View |> View.MapCached(abilityModifier >> formatModifier))
        ]

        div [ Typography.caption; Typography.Color.textSecondary ] [ text "modifier" ]
      ]

    let abilitiesTab =
      div [ Padding.All.extraSmall ] [
        Grid.create (
          [
            GridItem.create (
              abilityCard Strength str,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (
              abilityCard Dexterity dex,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (
              abilityCard Constitution con,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (
              abilityCard Intelligence int',
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (
              abilityCard Wisdom wis,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )
            GridItem.create (
              abilityCard Charisma cha,
              attrs = [ GridItem.Span.twelve; GridItem.Span.Small.six; GridItem.Span.Medium.four ]
            )
          ],
          attrs = [ AlignItems.stretch ]
        )
      ]

    let hitPointsTab =
      div [ Padding.All.extraSmall ] [
        Grid.create (
          [
            GridItem.create (
              div [
                SurfaceColor.BackgroundColor.surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Max Hit Points" ]

                NumericField.create (
                  maxHp,
                  min = 1,
                  max = 999,
                  attrs = [
                    NumericField.Variant.outlined
                    NumericField.Color.primary
                    Attr.Style "width" "100%"
                  ]
                )
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )

            GridItem.create (
              div [
                SurfaceColor.BackgroundColor.surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e1
              ] [
                div [ Typography.subtitle2; Margin.Bottom.extraSmall ] [ text "Current Hit Points" ]

                div [ Margin.Bottom.extraSmall ] [
                  let colorClasses =
                    Map.ofList [
                      "healthy", "weave-slider--success"
                      "wounded", "weave-slider--warning"
                      "critical", "weave-slider--error"
                    ]

                  let health =
                    (currentHp.View, maxHp.View)
                    ||> View.map2Cached (fun current maximum ->
                      let percent = float current / float maximum * 100.0

                      if percent > 50.0 then "healthy"
                      elif percent > 25.0 then "wounded"
                      else "critical")

                  let label =
                    (currentHp.View, maxHp.View)
                    ||> View.Map2(fun current maximum -> sprintf "HP: %d / %d" current maximum)

                  // Changing HP updates attrs without replacing the focused slider.
                  maxHp.View
                  |> Doc.BindView(fun maximum ->
                    Slider.create (
                      currentHp,
                      min = 0,
                      max = maximum,
                      labelText = label,
                      attrs = [ colorClasses |> Attr.classSelection health ]
                    ))
                ]
              ],
              attrs = [ GridItem.Span.twelve; GridItem.Span.Medium.six ]
            )
          ],
          attrs = [ AlignItems.stretch ]
        )

        // Quick HP buttons
        div [
          Flex.Flex.allSizes
          FlexWrap.Wrap.allSizes
          JustifyContent.center
          Gap.All.g2
          Margin.Top.small
        ] [
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
      div [
        Padding.All.extraSmall
        Attr.Style "width" "100%"
        Attr.Style "max-width" "40rem"
        Attr.Style "margin-inline" "auto"
      ] [
        div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2; Margin.Bottom.small ] [
          div [ Typography.subtitle2 ] [ text "Proficiency Bonus:" ]

          div [ Typography.body1; BrandColor.TextColor.primary ] [
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
            div [ FlexItem.Flex.allSizes ] [
              Checkbox.create (
                skill.Proficient,
                View.Const skill.Name,
                attrs = [ Checkbox.Color.primary; Density.compact ]
              )
            ]

            div [ Typography.caption; Typography.Color.textSecondary ] [
              text (abilityToString skill.Ability)
            ]

            div [
              Typography.subtitle2
              BrandColor.TextColor.primary
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
      div [ Padding.All.extraSmall ] [
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
                  Button.Width.full
                  Attr.Style "min-height" "80px"
                ]
              ),
              attrs = [ GridItem.Span.six; GridItem.Span.Small.four; GridItem.Span.Medium.two ]
            ))
        )

        // Roll result display
        div [ Flex.Flex.allSizes; JustifyContent.center; Margin.Top.small ] [
          lastRoll.View
          |> Doc.BindView(fun roll ->
            match roll with
            | Some result ->
              div [
                SurfaceColor.BackgroundColor.surface
                Padding.All.small
                BorderRadius.All.small
                Elevation.e2
                Flex.Flex.allSizes
                FlexDirection.Column.allSizes
                AlignItems.center
                Attr.Style "min-width" "150px"
              ] [
                div [ Typography.overline; Typography.Color.textSecondary ] [ text "Result" ]
                div [ Typography.h3; BrandColor.TextColor.primary ] [ text result ]
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

  let private domainSource =
    """[<Struct>]
type Ability =
  | Strength
  | Dexterity
  | Constitution
  | Intelligence
  | Wisdom
  | Charisma

type SkillDef = {
  Name: string
  Ability: Ability
  Proficient: Var<bool>
}

// Three skills keep this learning example small; the full demo has 18.
let skill name ability = {
  Name = name
  Ability = ability
  Proficient = Var.Create false
}

let skills = [
  skill "Acrobatics" Dexterity
  skill "Athletics" Strength
  skill "Arcana" Intelligence
]"""

  let private abilitySource =
    """let str = Var.Create 10
let dex = Var.Create 10
let con = Var.Create 10
let int' = Var.Create 10
let wis = Var.Create 10
let cha = Var.Create 10

let abilityToString (a: Ability) =
  match a with
  | Strength -> "STR"
  | Dexterity -> "DEX"
  | Constitution -> "CON"
  | Intelligence -> "INT"
  | Wisdom -> "WIS"
  | Charisma -> "CHA"

// Pure functions — the heart of the character math
let abilityModifier score = int (floor (float (score - 10) / 2.0))
let proficiencyBonus (level: int) = (level - 1) / 4 + 2

let skillBonus abilityScore level isProficient =
  abilityModifier abilityScore
  + (if isProficient then proficiencyBonus level else 0)

let formatModifier (m: int) =
  if m >= 0 then sprintf "+%d" m else sprintf "%d" m

// Reactive ability card — reusable for all 6 abilities
let abilityCard (ability: Ability) (score: Var<int>) =
  div [
    Elevation.e1
    Padding.All.extraSmall
    Flex.Flex.allSizes
    FlexDirection.Column.allSizes
    AlignItems.center
    Gap.All.g2
  ] [
    div [ Typography.overline ] [ text (abilityToString ability) ]
    NumericField.create (
      score,
      min = 1,
      max = 30,
      attrs = [
        NumericField.Variant.outlined
        NumericField.Width.full
        Attr.Style "max-width" "100px"
      ]
    )
    div [ Typography.h5; BrandColor.TextColor.primary ] [
      textView (
        score.View |> View.MapCached(abilityModifier >> formatModifier)
      )
    ]
  ]

// 6 ability cards in a grid
let abilityCards =
  Grid.create (
    [
      Strength, str
      Dexterity, dex
      Constitution, con
      Intelligence, int'
      Wisdom, wis
      Charisma, cha
    ]
    |> List.map (fun (ability, score) ->
      GridItem.create (
        abilityCard ability score,
        attrs = [
          GridItem.Span.twelve
          GridItem.Span.Small.six
          GridItem.Span.Medium.four
        ]
      ))
  )"""

  let private hpSource =
    """let maxHp = Var.Create 10
let currentHp = Var.Create 10

let colorClasses =
  Map.ofList [
    "healthy", "weave-slider--success"
    "wounded", "weave-slider--warning"
    "critical", "weave-slider--error"
  ]

let health =
  (currentHp.View, maxHp.View)
  ||> View.map2Cached (fun current maximum ->
    let percent = float current / float maximum * 100.0

    if percent > 50.0 then "healthy"
    elif percent > 25.0 then "wounded"
    else "critical")

let hpLabel =
  (currentHp.View, maxHp.View)
  ||> View.Map2(fun current maximum -> sprintf "HP: %d / %d" current maximum)

let hpSlider =
  maxHp.View
  |> Doc.BindView(fun maximum ->
    Slider.create (
      currentHp,
      min = 0,
      max = maximum,
      labelText = hpLabel,
      attrs = [ colorClasses |> Attr.classSelection health ]
    ))

let hpControls =
  div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
    hpSlider
    Button.create (
      text "Heal +5",
      onClick =
        (fun () -> Var.Set currentHp (min maxHp.Value (currentHp.Value + 5))),
      attrs = [ Button.Color.success; Button.Variant.outlined ]
    )
    Button.create (
      text "Damage -5",
      onClick = (fun () -> Var.Set currentHp (max 0 (currentHp.Value - 5))),
      attrs = [ Button.Color.error; Button.Variant.outlined ]
    )
  ]"""

  let private skillSource =
    """let level = Var.Create 1

let abilityVar (a: Ability) =
  match a with
  | Strength -> str
  | Dexterity -> dex
  | Constitution -> con
  | Intelligence -> int'
  | Wisdom -> wis
  | Charisma -> cha

let skillList =
  div [
    Attr.Style "width" "100%"
    Attr.Style "max-width" "40rem"
    Attr.Style "margin-inline" "auto"
  ] [
    NumericField.create (
      level,
      min = 1,
      max = 20,
      labelText = View.Const "Level"
    )
    for skill in skills do
      let abilityScore = abilityVar skill.Ability

      div [ Flex.Flex.allSizes; AlignItems.center; Gap.All.g2 ] [
        Checkbox.create (
          skill.Proficient,
          View.Const skill.Name,
          attrs = [ Checkbox.Color.primary ]
        )
        text (abilityToString skill.Ability)
        textView (
          (abilityScore.View, level.View, skill.Proficient.View)
          |||> View.Map3(fun score level proficient ->
            skillBonus score level proficient |> formatModifier)
        )
      ]
  ]"""

  let private diceSource =
    """open Weave.Icons.MaterialSymbols

let lastRoll = Var.Create<string option> None
let random = System.Random()

let rollDie (sides: int) =
  let result = random.Next(1, sides + 1)
  Var.Set lastRoll (Some(sprintf "d%d: %d" sides result))

let diceButtons =
  Grid.create (
    [ 4; 6; 8; 10; 12; 20 ]
    |> List.map (fun sides ->
      GridItem.create (
        Button.create (
          div [ FlexDirection.Column.allSizes; AlignItems.center ] [
            Icon.create (Icon.Travel Travel.Casino)
            text (sprintf "d%d" sides)
          ],
          onClick = (fun () -> rollDie sides),
          attrs = [
            Button.Color.primary
            Button.Variant.outlined
            Button.Width.full
            Attr.Style "min-height" "80px"
          ]
        ),
        attrs = [
          GridItem.Span.six
          GridItem.Span.Small.four
          GridItem.Span.Medium.two
        ]
      ))
  )

let rollResult =
  lastRoll.View
  |> Doc.BindView(fun roll ->
    match roll with
    | Some result ->
      div [ Typography.h3; BrandColor.TextColor.primary ] [ text result ]
    | None -> div [ Typography.body2 ] [ text "Click a die to roll!" ])

div [ Flex.Flex.allSizes; FlexDirection.Column.allSizes; Gap.All.g3 ] [
  abilityCards
  hpControls
  skillList
  diceButtons
  rollResult
]"""

  // These are the exact displayed blocks, in their learning order.
  let private sourceParts = [ domainSource; abilitySource; hpSource; skillSource; diceSource ]

  let private combinedSource = String.concat "\n\n" sourceParts

  let private sourceSections () =
    div [] [

      Helpers.codeSection
        "fsharp"
        "Domain Model"
        (Helpers.bodyText
          "Start here. Define the six abilities and three illustrative skills for the smaller example.")
        domainSource

      Helpers.codeSection
        "fsharp"
        "Ability Scores & Modifiers"
        (Helpers.bodyText
          "Continues Domain Model. Define ability state, round modifiers down, and create a reusable ability card.")
        abilitySource

      Helpers.codeSection
        "fsharp"
        "Hit Points"
        (Helpers.bodyText
          "Continues the previous sections. Define hit-point state and clamped healing and damage actions. The slider also labels the numeric value, so color is a secondary cue.")
        hpSource

      Helpers.codeSection
        "fsharp"
        "Skills & Proficiency"
        (Helpers.bodyText
          "Continues Domain Model and Ability Scores & Modifiers. Label each proficiency checkbox with its skill and combine ability, level, and proficiency Views.")
        skillSource

      Helpers.codeSection
        "fsharp"
        "Dice Roller"
        (Helpers.bodyText
          "Continues all previous sections. Add dice actions and a result display, then return the composed character example. The full app adds identity controls, tabs, and the remaining skills.")
        diceSource

      Helpers.codeSection
        "fsharp"
        "Combined example"
        (Helpers.bodyText
          "Copy all the sections in order. Use the application setup in Getting Started and paste this source inside its JavaScript module; its final expression returns the composed Doc. The smaller example focuses on the mechanisms above, while the full demo adds presentation and sample data.")
        combinedSource
    ]

  let render () =
    Container.create (
      div [] [
        Helpers.pageTitle "Character Sheet"

        div [ Typography.body1; Margin.Bottom.extraSmall ] [
          text
            "A D&D-inspired character sheet with ability scores, hit points, skills, and dice rolls. Follow the source sections to combine a typed domain model, pure calculations, and reactive components."
        ]

        Helpers.divider ()
        fullAppSection ()
        Helpers.divider ()
        Helpers.bodyText
          "Build the smaller character example from top to bottom. Each source section continues the preceding sections; Combined example copies the same sequence in one block."
        sourceSections ()
      ],
      attrs = [ Container.MaxWidth.large ]
    )
