module Weave.Tests.Rendering.ContainmentAssertions

open Xunit
open Microsoft.Playwright

[<Literal>]
let tolerance = 1.0f

/// Child fits entirely within parent on both axes (1px tolerance).
let assertContainedWithin
  (parentName: string)
  (childName: string)
  (parentBox: LocatorBoundingBoxResult)
  (childBox: LocatorBoundingBoxResult)
  =
  Assert.True(
    childBox.X >= parentBox.X - tolerance
    && childBox.X + childBox.Width <= parentBox.X + parentBox.Width + tolerance,
    $"{childName} (x:{childBox.X}, w:{childBox.Width}) should be horizontally within {parentName} (x:{parentBox.X}, w:{parentBox.Width})"
  )

  Assert.True(
    childBox.Y >= parentBox.Y - tolerance
    && childBox.Y + childBox.Height <= parentBox.Y + parentBox.Height + tolerance,
    $"{childName} (y:{childBox.Y}, h:{childBox.Height}) should be vertically within {parentName} (y:{parentBox.Y}, h:{parentBox.Height})"
  )

/// Child's top and bottom edges are flush with parent's (1px tolerance).
let assertFillsHeight
  (parentName: string)
  (childName: string)
  (parentBox: LocatorBoundingBoxResult)
  (childBox: LocatorBoundingBoxResult)
  =
  Assert.True(
    abs (childBox.Y - parentBox.Y) <= tolerance,
    $"{childName} top ({childBox.Y}) should align with {parentName} top ({parentBox.Y})"
  )

  Assert.True(
    abs ((childBox.Y + childBox.Height) - (parentBox.Y + parentBox.Height))
    <= tolerance,
    $"{childName} bottom ({childBox.Y + childBox.Height}) should align with {parentName} bottom ({parentBox.Y + parentBox.Height})"
  )

/// Child's left and right edges are flush with parent's (1px tolerance).
let assertFillsWidth
  (parentName: string)
  (childName: string)
  (parentBox: LocatorBoundingBoxResult)
  (childBox: LocatorBoundingBoxResult)
  =
  Assert.True(
    abs (childBox.X - parentBox.X) <= tolerance,
    $"{childName} left ({childBox.X}) should align with {parentName} left ({parentBox.X})"
  )

  Assert.True(
    abs ((childBox.X + childBox.Width) - (parentBox.X + parentBox.Width))
    <= tolerance,
    $"{childName} right ({childBox.X + childBox.Width}) should align with {parentName} right ({parentBox.X + parentBox.Width})"
  )

/// All elements in the list should have the same rendered height (1px tolerance).
let assertHeightsMatch (familyName: string) (entries: (string * LocatorBoundingBoxResult) list) =
  match entries with
  | []
  | [ _ ] -> ()
  | (refName, refBox) :: rest ->
    for (name, box) in rest do
      Assert.True(
        abs (box.Height - refBox.Height) <= tolerance,
        $"[{familyName}] {name} height ({box.Height}px) should match {refName} height ({refBox.Height}px)"
      )

/// All elements should share the same computed CSS property value.
let assertComputedValuesMatch (familyName: string) (propertyName: string) (entries: (string * string) list) =
  match entries with
  | []
  | [ _ ] -> ()
  | (refName, refValue) :: rest ->
    for (name, actual) in rest do
      Assert.True(
        (actual = refValue),
        $"[{familyName}] {name} {propertyName} ('{actual}') should match {refName} ('{refValue}')"
      )
