namespace Weave.Docs

#if !INTERACTIVE
open WebSharper

[<JavaScript>]
#endif
module ScrollTopAppearance =

  type Configuration = {
    Offset: int
    Color: string
    Instant: bool
  }

  let offsets = [ 100, "100 px"; 150, "150 px"; 300, "300 px" ]

  let colors = [
    "primary", "Primary"
    "secondary", "Secondary"
    "tertiary", "Tertiary"
    "error", "Error"
    "warning", "Warning"
    "success", "Success"
    "info", "Info"
  ]

  let defaults = {
    Offset = 150
    Color = "secondary"
    Instant = true
  }

  let render current =
    [
      "let id = WeaveId.create \"scroll-demo\""
      ""
      "div [ Attr.Style \"position\" \"relative\" ] ["
      "  div ["
      "    attr.id id"
      "    attr.tabindex \"0\""
      "    Attr.Create \"role\" \"region\""
      "    Attr.Create \"aria-label\" \"Scrollable chapters\""
      "    Attr.Style \"height\" \"260px\""
      "    Overflow.yAuto"
      "    Padding.All.small"
      "    SurfaceColor.BackgroundColor.paper"
      "  ] ["
      "    for chapter in 1..12 do"
      "      p [ Typography.body1; Margin.Bottom.small ] ["
      "        text (sprintf \"Chapter %d. Keep scrolling.\" chapter)"
      "      ]"
      "  ]"
      "  ScrollToTop.create ("
      "    selector = \"#\" + id,"
      if current.Offset <> 300 then
        sprintf "    topOffset = %d," current.Offset
      "    attrs = ["
      "      Button.Color." + current.Color
      "      Attr.Style \"position\" \"absolute\""
      "      Attr.Create \"aria-label\" \"Back to panel top\""
      if current.Instant then
        "      ScrollToTop.Behavior.instant"
      "    ]"
      "  )"
      "]"
    ]
    |> String.concat "\n"

  let cases () = [
    for offset, _ in offsets do
      for color, _ in colors do
        for instant in [ false; true ] do
          let current = {
            Offset = offset
            Color = color
            Instant = instant
          }

          sprintf "%d %s %b" offset color instant, render current
  ]
