namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module SourceString =
  let varBinding (name: string) (value: string) =
    let escaped =
      value
      |> Seq.map (fun char ->
        match char with
        | '\\' -> "\\\\"
        | '"' -> "\\\""
        | '\n' -> "\\n"
        | '\r' -> "\\r"
        | '\t' -> "\\t"
        | _ ->
          let code = int char

          if
            code < 32
            || code = 127
            || code = 0x2028
            || code = 0x2029
            || (code >= 0xD800 && code <= 0xDFFF)
          then
            sprintf "\\u%04X" code
          else
            string char)
      |> Seq.fold
        (fun (chunks: string list) (part: string) ->
          match chunks with
          | head :: rest when head.Length + part.Length <= 48 -> (head + part) :: rest
          | _ -> part :: chunks)
        []
      |> List.rev

    match escaped with
    | [] -> [ "let " + name + " = Var.Create \"\"" ]
    | [ value ] -> [ "let " + name + " = Var.Create \"" + value + "\"" ]
    | values -> [
        "let " + name + " ="
        "  Var.Create ("
        for index, value in values |> List.indexed do
          "    \"" + value + "\"" + (if index < values.Length - 1 then " +" else "")
        "  )"
      ]
