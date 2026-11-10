namespace Weave.Tests.E2E

open Xunit
open Microsoft.Playwright
open Deque.AxeCore.Playwright

type FormInteractionTests(fixture: TestFixture) =
  inherit E2ETestBase(fixture)

  [<Theory>]
  [<InlineData("standard")>]
  [<InlineData("filled")>]
  [<InlineData("outlined")>]
  member this.``grid validation keeps controls anchored without reserving helper space``(variant: string) =
    task {
      do! this.NavigateTo("form-grid-validation")
      let form = this.Page.Locator($"form[data-variant='{variant}']")
      let name = form.GetByLabel("Name", LocatorGetByLabelOptions(Exact = true))
      let quantity = form.GetByLabel("Quantity", LocatorGetByLabelOptions(Exact = true))

      let nameField = form.Locator(".weave-field:not(.weave-field--numeric)")
      let quantityField = form.Locator(".weave-field--numeric")

      let! nameBefore = name.BoundingBoxAsync()
      let! quantityBefore = quantity.BoundingBoxAsync()
      let! nameFieldBefore = nameField.BoundingBoxAsync()
      let! quantityFieldBefore = quantityField.BoundingBoxAsync()
      do! this.Expect(form.Locator(".weave-field__help-text:visible")).ToHaveCountAsync(0)
      do! quantity.FillAsync("3")
      do! quantity.PressAsync("Tab")
      do! this.Expect(quantity).ToHaveAttributeAsync("aria-invalid", "true")

      do!
        this
          .Expect(form.GetByText("Order in pairs.", LocatorGetByTextOptions(Exact = true)))
          .ToBeVisibleAsync()

      let! nameAfter = name.BoundingBoxAsync()
      let! quantityAfter = quantity.BoundingBoxAsync()
      let! nameFieldAfter = nameField.BoundingBoxAsync()
      let! quantityFieldAfter = quantityField.BoundingBoxAsync()

      for before, after in [ nameBefore, nameAfter; quantityBefore, quantityAfter ] do
        Assert.True(
          abs (before.Y - after.Y) <= 1.0f,
          $"{variant}: input top moved from {before.Y}px to {after.Y}px"
        )

        Assert.True(
          abs (before.Height - after.Height) <= 1.0f,
          $"{variant}: input height changed during validation"
        )

      Assert.True(
        abs (nameFieldBefore.Height - nameFieldAfter.Height) <= 1.0f,
        "The valid field should not acquire helper space"
      )

      Assert.True(
        quantityFieldAfter.Height > quantityFieldBefore.Height,
        "The error message should occupy normal flow only while visible"
      )

      do! quantity.FillAsync("2")
      do! quantity.PressAsync("Tab")
      do! this.Expect(quantity).ToHaveAttributeAsync("aria-invalid", "false")
      let! repaired = quantityField.BoundingBoxAsync()

      Assert.True(
        abs (repaired.Height - quantityFieldBefore.Height) <= 1.0f,
        "Repairing the field should remove helper space"
      )
    }

  [<Fact>]
  member this.``float comparisons reject NaN in JavaScript``() = task {
    do! this.NavigateTo("form-arithmetic")
    do! this.Expect(this.Page.Locator("#nan-bounds")).ToHaveTextAsync("true")
  }

  [<Fact>]
  member this.``arithmetic totals revalidate on either field and preserve submitted quantity``() = task {
    do! this.NavigateTo("form-arithmetic")
    let price = this.Page.GetByLabel("Price", PageGetByLabelOptions(Exact = true))
    let quantity = this.Page.GetByLabel("Quantity", PageGetByLabelOptions(Exact = true))

    let save =
      this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Save", Exact = true))

    do! save.ClickAsync()
    do! this.Expect(this.Page.Locator("#saved")).ToHaveTextAsync("25 x 4")
    do! quantity.FillAsync("5")
    do! this.Expect(quantity).ToHaveAttributeAsync("aria-invalid", "true")

    do!
      this.Expect(this.Page.GetByText("Over budget.", PageGetByTextOptions(Exact = true))).ToBeVisibleAsync()

    do! save.ClickAsync()
    do! this.Expect(this.Page.Locator("#saved")).ToHaveTextAsync("25 x 4")
    do! price.FillAsync("20")
    do! this.Expect(quantity).ToHaveAttributeAsync("aria-invalid", "false")
    do! save.ClickAsync()
    do! this.Expect(this.Page.Locator("#saved")).ToHaveTextAsync("20 x 5")
    do! quantity.FillAsync("11")

    do!
      this
        .Expect(
          this.Page.GetByText("Choose between 1 and 10. Over budget.", PageGetByTextOptions(Exact = true))
        )
        .ToBeVisibleAsync()

    do! price.FillAsync("0")
    do! save.ClickAsync()
    do! this.Expect(price).ToHaveAttributeAsync("aria-invalid", "true")
    do! this.Expect(this.Page.Locator("#saved")).ToHaveTextAsync("20 x 5")
  }

  [<Theory>]
  [<InlineData("standard")>]
  [<InlineData("filled")>]
  [<InlineData("outlined")>]
  member this.``text and numeric fields align in an inline form``(variant: string) = task {
    do! this.NavigateTo("form-sizing")
    let form = this.Page.Locator($"form[data-variant='{variant}']")

    let textControl =
      form.Locator(".weave-field:not(.weave-field--numeric) .weave-field__control")

    let numericControl = form.Locator(".weave-field--numeric .weave-field__control")

    let assertAligned () = task {
      let! textBox = textControl.BoundingBoxAsync()
      let! numericBox = numericControl.BoundingBoxAsync()

      Assert.True(
        abs (textBox.Height - numericBox.Height) <= 1.0f,
        $"{variant}: text height {textBox.Height}px should match numeric height {numericBox.Height}px"
      )

      Assert.True(
        abs ((textBox.Y + textBox.Height) - (numericBox.Y + numericBox.Height)) <= 1.0f,
        $"{variant}: text and numeric controls should share their bottom edge"
      )
    }

    do! assertAligned ()
    do! form.GetByLabel("Name", LocatorGetByLabelOptions(Exact = true)).FillAsync("Ada")
    do! assertAligned ()
    do! form.GetByLabel("Age", LocatorGetByLabelOptions(Exact = true)).FocusAsync()
    do! assertAligned ()
  }

  member private this.FillValidForm() = task {
    do! this.Page.GetByLabel("Name", PageGetByLabelOptions(Exact = true)).FillAsync("Ada")
    do! this.Page.GetByLabel("Password", PageGetByLabelOptions(Exact = true)).FillAsync("password123")
    do! this.Page.GetByLabel("Confirm password").FillAsync("password123")
    do! this.Page.GetByLabel("Age", PageGetByLabelOptions(Exact = true)).FillAsync("21")

    do!
      this.Page
        .GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Country", Exact = true))
        .ClickAsync()

    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = "Canada", Exact = true)).ClickAsync()

    do!
      this.Page
        .GetByRole(AriaRole.Combobox, PageGetByRoleOptions(Name = "Interests", Exact = true))
        .ClickAsync()

    do! this.Page.GetByRole(AriaRole.Option, PageGetByRoleOptions(Name = "Music", Exact = true)).ClickAsync()
    do! this.Page.Keyboard.PressAsync("Escape")
  }

  [<Fact>]
  member this.``submit reveals every error and blocks the callback``() = task {
    do! this.NavigateTo("form")
    let invalid = this.Page.Locator("[aria-invalid='true']")
    do! this.Expect(invalid).ToHaveCountAsync(0)
    do! this.Expect(this.Page.Locator("#valid")).ToHaveTextAsync("false")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Save", Exact = true)).ClickAsync()
    do! this.Expect(invalid).ToHaveCountAsync(6)
    do! this.Expect(this.Page.Locator("#error-count")).ToHaveTextAsync("7")
    do! this.Expect(this.Page.Locator("#submitted")).ToHaveTextAsync("0")

    do!
      this
        .Expect(this.Page.GetByText("Name required. Name too short.", PageGetByTextOptions(Exact = true)))
        .ToBeVisibleAsync()

    let name = this.Page.GetByLabel("Name", PageGetByLabelOptions(Exact = true))
    let! errorId = name.GetAttributeAsync("aria-describedby")
    do! this.Expect(this.Page.Locator("#" + errorId)).ToHaveTextAsync("Name required. Name too short.")

    do!
      this
        .Expect(this.Page.Locator(".weave-field").Filter(LocatorFilterOptions(Has = name)))
        .ToHaveClassAsync(System.Text.RegularExpressions.Regex("weave-field--error"))
  }

  [<Fact>]
  member this.``blur reveals errors and editing repairs them without replacing input``() = task {
    do! this.NavigateTo("form")
    let name = this.Page.GetByLabel("Name", PageGetByLabelOptions(Exact = true))
    do! name.FocusAsync()
    do! name.PressAsync("Tab")
    do! this.Expect(name).ToHaveAttributeAsync("aria-invalid", "true")
    do! this.Expect(this.Page.Locator("[aria-invalid='true']")).ToHaveCountAsync(1)
    do! name.FillAsync("Ada")
    do! this.Expect(name).ToHaveAttributeAsync("aria-invalid", "false")
    do! this.Expect(name).ToBeFocusedAsync()
    do! this.Expect(this.Page.Locator("#dirty")).ToHaveTextAsync("true")

    do!
      this
        .Expect(this.Page.GetByText("Your full name", PageGetByTextOptions(Exact = true)))
        .ToBeVisibleAsync()
  }

  [<Fact>]
  member this.``valid submit uses latest typed values and reset restores initial untouched state``() = task {
    do! this.NavigateTo("form")
    do! this.FillValidForm()
    do! this.Expect(this.Page.Locator("#valid")).ToHaveTextAsync("true")
    let name = this.Page.GetByLabel("Name", PageGetByLabelOptions(Exact = true))
    do! name.FillAsync("Grace")
    do! name.PressAsync("Enter")
    do! this.Expect(this.Page.Locator("#submitted")).ToHaveTextAsync("1")
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync("Grace|21|Canada|Music")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Reset", Exact = true)).ClickAsync()
    do! this.Expect(name).ToHaveValueAsync("")
    do! this.Expect(this.Page.GetByLabel("Age", PageGetByLabelOptions(Exact = true))).ToHaveValueAsync("17")
    do! this.Expect(this.Page.Locator("#valid")).ToHaveTextAsync("false")
    do! this.Expect(this.Page.Locator("#dirty")).ToHaveTextAsync("false")
    do! this.Expect(this.Page.Locator("#touched")).ToHaveTextAsync("false")
    do! this.Expect(this.Page.Locator("[aria-invalid='true']")).ToHaveCountAsync(0)
    do! this.Expect(this.Page.Locator("#submitted")).ToHaveTextAsync("1")
  }

  [<Fact>]
  member this.``changing password invalidates a touched confirmation``() = task {
    do! this.NavigateTo("form")
    do! this.FillValidForm()
    let confirmation = this.Page.GetByLabel("Confirm password")
    do! confirmation.FocusAsync()
    do! confirmation.PressAsync("Tab")
    do! this.Expect(confirmation).ToHaveAttributeAsync("aria-invalid", "false")
    do! this.Page.GetByLabel("Password", PageGetByLabelOptions(Exact = true)).FillAsync("different123")
    do! this.Expect(confirmation).ToHaveAttributeAsync("aria-invalid", "true")

    do!
      this
        .Expect(this.Page.GetByText("Passwords must match.", PageGetByTextOptions(Exact = true)))
        .ToBeVisibleAsync()

    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Save", Exact = true)).ClickAsync()
    do! this.Expect(this.Page.Locator("#submitted")).ToHaveTextAsync("0")
    do! confirmation.FillAsync("different123")
    do! this.Expect(confirmation).ToHaveAttributeAsync("aria-invalid", "false")
    do! this.Expect(this.Page.Locator("#valid")).ToHaveTextAsync("true")
  }

  [<Fact>]
  member this.``programmatic submit samples same-turn updates in nested models``() = task {
    do! this.NavigateTo("form")

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Fill and submit", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Page.Locator("#submitted")).ToHaveTextAsync("1")
    do! this.Expect(this.Page.Locator("#result")).ToHaveTextAsync("Same turn|25|Canada|Music")

    do!
      this.Page
        .GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Invalidate and submit", Exact = true))
        .ClickAsync()

    do! this.Expect(this.Page.Locator("#valid")).ToHaveTextAsync("false")
    do! this.Expect(this.Page.Locator("#submitted")).ToHaveTextAsync("1")
    do! this.Expect(this.Page.GetByLabel("Confirm password")).ToHaveAttributeAsync("aria-invalid", "true")
  }

  [<Fact>]
  member this.``validated form is accessible before and after errors``() = task {
    do! this.RunAxeScan("form")
    do! this.Page.GetByRole(AriaRole.Button, PageGetByRoleOptions(Name = "Save", Exact = true)).ClickAsync()
    // A second scan observes the actual page state after validation, without navigating.
    let! result = this.Page.RunAxe()

    Assert.Empty(
      result.Violations
      |> Array.filter (fun v ->
        v.Id <> "region"
        && v.Id <> "landmark-one-main"
        && v.Id <> "page-has-heading-one"
        && v.Id <> "color-contrast")
    )
  }
