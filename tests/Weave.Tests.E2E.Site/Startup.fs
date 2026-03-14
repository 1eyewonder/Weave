open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open WebSharper.AspNetCore

[<EntryPoint>]
let main args =
  let builder = WebApplication.CreateBuilder(args)

  builder.Services.AddWebSharper().AddAuthentication("WebSharper").AddCookie("WebSharper", fun _ -> ())
  |> ignore

  let app = builder.Build()

  app.UseDefaultFiles().UseStaticFiles() |> ignore

  app.Run()

  0
