namespace WeaveStarter

open WebSharper
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open Weave
open Weave.Theming

[<JavaScript>]
module Domain =
  type Transaction = {
    Reference: string
    Customer: string
    Email: string
    Amount: int
    Status: string
    DaysAgo: int
    Method: string
  }

  // Fictional values. Amounts are stored as integer cents.
  let transactions = [
    {
      Reference = "PAY-1048"
      Customer = "Maya Chen"
      Email = "maya@example.test"
      Amount = 24800
      Status = "Succeeded"
      DaysAgo = 0
      Method = "Card •••• 4242"
    }
    {
      Reference = "PAY-1047"
      Customer = "Oliver Reed"
      Email = "oliver@example.test"
      Amount = 12900
      Status = "Succeeded"
      DaysAgo = 1
      Method = "Card •••• 1881"
    }
    {
      Reference = "PAY-1046"
      Customer = "Sofia Park"
      Email = "sofia@example.test"
      Amount = 8650
      Status = "Pending"
      DaysAgo = 2
      Method = "Bank transfer"
    }
    {
      Reference = "PAY-1045"
      Customer = "Jules Martin"
      Email = "jules@example.test"
      Amount = 42000
      Status = "Succeeded"
      DaysAgo = 4
      Method = "Card •••• 5555"
    }
    {
      Reference = "PAY-1044"
      Customer = "Nora Ellis"
      Email = "nora@example.test"
      Amount = 19900
      Status = "Failed"
      DaysAgo = 6
      Method = "Card •••• 1111"
    }
    {
      Reference = "PAY-1043"
      Customer = "Amir Patel"
      Email = "amir@example.test"
      Amount = 30500
      Status = "Succeeded"
      DaysAgo = 12
      Method = "Bank transfer"
    }
    {
      Reference = "PAY-1042"
      Customer = "Harper Lee"
      Email = "harper@example.test"
      Amount = 15700
      Status = "Pending"
      DaysAgo = 23
      Method = "Card •••• 4242"
    }
    {
      Reference = "PAY-1041"
      Customer = "Elena Ruiz"
      Email = "elena@example.test"
      Amount = 72000
      Status = "Succeeded"
      DaysAgo = 46
      Method = "Card •••• 1881"
    }
    {
      Reference = "PAY-1040"
      Customer = "Theo Brooks"
      Email = "theo@example.test"
      Amount = 5400
      Status = "Failed"
      DaysAgo = 76
      Method = "Bank transfer"
    }
  ]
