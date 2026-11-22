namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module SwipeAreaAppearance =

  let source threshold enabled =
    [
      "let result = Var.Create \"Swipe in any direction\""
      "let threshold = Var.Create " + string threshold
      "let enabled = Var.Create " + (if enabled then "true" else "false")
      ""
      "SwipeArea.create ("
      "  Card.create (CardContent.create ("
      "    div [ Padding.All.large; Attr.Style \"min-height\" \"180px\" ] ["
      "      p [ Typography.h6; Attr.Create \"aria-live\" \"polite\" ] ["
      "        textView result.View"
      "      ]"
      "    ]"
      "  ), attrs = [ Card.Variant.filled ]),"
      "  (fun swipe ->"
      "    result.Set ("
      "      sprintf \"%A · %.0f, %.0f px\""
      "        swipe.Direction swipe.DeltaX swipe.DeltaY"
      "    )),"
      "  threshold = threshold.View,"
      "  enabled = enabled.View,"
      "  attrs = [ SwipeArea.TouchAction.none ]"
      ")"
    ]
    |> String.concat "\n"

  let cases () = [
    for threshold in [ 10; 60; 150 ] do
      for enabled in [ false; true ] do
        sprintf "%d %b" threshold enabled, source threshold enabled
  ]
