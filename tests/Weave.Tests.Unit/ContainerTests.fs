module Weave.Tests.Unit.ContainerTests

open Expecto
open Weave
open Weave.CssHelpers

[<Tests>]
let containerTests =
  testList "Container" [

    testList "MaxWidth.toClass" [
      testTheory "each max width maps to the correct class" [
        Container.MaxWidth.ExtraSmall, "weave-container--maxwidth-xs"
        Container.MaxWidth.Small, "weave-container--maxwidth-sm"
        Container.MaxWidth.Medium, "weave-container--maxwidth-md"
        Container.MaxWidth.Large, "weave-container--maxwidth-lg"
        Container.MaxWidth.ExtraLarge, "weave-container--maxwidth-xl"
        Container.MaxWidth.ExtraExtraLarge, "weave-container--maxwidth-xxl"
      ]
      <| fun (maxWidth, expected) -> Expect.equal (Container.MaxWidth.toClass maxWidth) expected ""

      testCase "all max widths produce distinct classes"
      <| fun () ->
        let classes =
          [
            Container.MaxWidth.ExtraSmall
            Container.MaxWidth.Small
            Container.MaxWidth.Medium
            Container.MaxWidth.Large
            Container.MaxWidth.ExtraLarge
            Container.MaxWidth.ExtraExtraLarge
          ]
          |> List.map Container.MaxWidth.toClass

        Expect.equal (List.distinct classes).Length classes.Length "each max width maps to a unique class"
    ]
  ]
