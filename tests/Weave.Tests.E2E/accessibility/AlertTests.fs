namespace Weave.Tests.E2E

open Microsoft.Playwright.Xunit
open Xunit

[<Collection("E2E")>]
type AlertTests(server: TestServerFixture) =
  inherit E2ETestBase(server)

  [<Fact>]
  member this.``passes axe-core accessibility scan``() = this.RunAxeScan("alert")

  [<Fact>]
  member this.``alerts have role="alert" for screen reader announcement``() = task {
    do! this.NavigateTo("alert")
    let alerts = this.Page.Locator(".weave-alert")
    let! count = alerts.CountAsync()
    Assert.True(count > 0, "Expected at least one alert on the page")

    for i in 0 .. count - 1 do
      do! this.Expect(alerts.Nth(i)).ToHaveAttributeAsync("role", "alert")
  }
