module Weave.Tests.Unit.IconTests

open System
open System.IO
open Expecto
open Microsoft.FSharp.Reflection
open Weave.Icons.MaterialSymbols

[<Tests>]
let iconTests =
  testCase "every typed Material Symbol has a ligature in the supported font catalog"
  <| fun () ->
    let catalog =
      Path.Combine(AppContext.BaseDirectory, "MaterialSymbols.codepoints")
      |> File.ReadLines
      |> Seq.map (fun line -> line.Split(' ')[0])
      |> Set.ofSeq

    let missing = [
      for category in FSharpType.GetUnionCases typeof<Icon> do
        let field = category.GetFields() |> Array.exactlyOne

        for symbol in FSharpType.GetUnionCases field.PropertyType do
          let value = FSharpValue.MakeUnion(symbol, [||])
          let icon = FSharpValue.MakeUnion(category, [| value |]) :?> Icon
          let name = Icon.toSnakeCase icon

          if not (catalog.Contains name) then
            yield $"%s{category.Name}.%s{symbol.Name}: %s{name}"
    ]

    Expect.equal missing [] "Every icon mapping must exist in the Material Symbols font"
