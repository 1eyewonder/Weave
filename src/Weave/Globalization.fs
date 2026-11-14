namespace Weave

open System
open System.Globalization
open WebSharper
open WebSharper.UI

/// Shared application culture for calendar labels and default week order.
/// Use a BCP 47 culture name such as en-US, en-GB, or fr-FR.
[<JavaScript; RequireQualifiedAccess>]
module Globalization =
  [<Inline "Intl.DateTimeFormat().resolvedOptions().locale">]
  let private detectCulture () =
    let name = CultureInfo.CurrentCulture.Name
    if String.IsNullOrEmpty name then "en-US" else name

  [<Inline "new Intl.Locale($name).toString()">]
  let private canonicalize (name: string) = CultureInfo.GetCultureInfo(name).Name

  let private currentCulture = Var.Create(detectCulture ())

  /// Read-only view; changes update mounted calendars as well as future calendars.
  let culture = currentCulture.View

  let setCulture name =
    name |> canonicalize |> Var.Set currentCulture

  // CLDR 49 first-day fallback for engines exposing neither getWeekInfo nor weekInfo.
  // https://github.com/unicode-org/cldr-json/blob/main/cldr-json/cldr-core/supplemental/weekData.json
  [<Inline "(function(name) { var locale = new Intl.Locale(name); var info = typeof locale.getWeekInfo === 'function' ? locale.getWeekInfo() : locale.weekInfo; if (info) return info.firstDay % 7; var region = locale.maximize().region; if (region === 'MV') return 5; if (' AF BH DJ DZ EG IQ IR JO KW LY OM QA SD SY '.includes(' ' + region + ' ')) return 6; if (' AG AS BD BR BS BT BW BZ CA CO DM DO ET GT GU HK HN ID IL IN IS JM JP KE KH KR LA MH MM MO MT MX MZ NI NP PA PE PH PK PR PT PY SA SG SV TH TT TW UM US VE VI WS YE ZA ZW '.includes(' ' + region + ' ')) return 0; return 1; })($name)">]
  let internal firstDayOfWeek (name: string) =
    CultureInfo.GetCultureInfo(name).DateTimeFormat.FirstDayOfWeek

  [<Inline "new Intl.DateTimeFormat($name, { calendar: 'gregory', month: 'long', timeZone: 'UTC' }).format(new Date(Date.UTC(2026, $month - 1, 15)))">]
  let internal monthName (name: string) month =
    CultureInfo.GetCultureInfo(name).DateTimeFormat.GetMonthName(month)

  [<Inline "new Intl.DateTimeFormat($name, { weekday: 'short', timeZone: 'UTC' }).format(new Date(Date.UTC(2023, 0, 1 + $day)))">]
  let internal weekdayName (name: string) (day: DayOfWeek) =
    CultureInfo.GetCultureInfo(name).DateTimeFormat.GetAbbreviatedDayName(day)

  [<Inline "(function(name, year, month) { var date = new Date(Date.UTC(2000, month - 1, 15)); date.setUTCFullYear(year); return new Intl.DateTimeFormat(name, { calendar: 'gregory', year: 'numeric', month: 'long', timeZone: 'UTC' }).format(date); })($name, $year, $month)">]
  let internal monthTitle (name: string) year month =
    DateTime(year, month, 15).ToString("Y", CultureInfo.GetCultureInfo(name))
