namespace Weave

open WebSharper

[<JavaScript; RequireQualifiedAccess>]
type LowerBounds<'T when 'T: comparison> =
  | Inclusive of 'T
  | Exclusive of 'T
  | Unbounded

[<JavaScript; RequireQualifiedAccess>]
type UpperBounds<'T when 'T: comparison> =
  | Inclusive of 'T
  | Exclusive of 'T
  | Unbounded

/// An opaque range of comparable values. Construct through the Range module and read its bounds through Lower and Upper.
[<JavaScript; RequireQualifiedAccess>]
type Range<'T when 'T: comparison> =
  private
  | Range of lower: LowerBounds<'T> * upper: UpperBounds<'T>

  member this.Lower =
    let (Range.Range(lower, _)) = this
    lower

  member this.Upper =
    let (Range.Range(_, upper)) = this
    upper

/// Ordered-value membership, including explicit unbounded and single-value intervals.
/// Constructors return None for reversed finite bounds or equal endpoints with either end excluded.
[<JavaScript; RequireQualifiedAccess>]
type Interval<'T when 'T: comparison> =
  | Bounded of range: Range<'T>
  | Unbounded
  | Degenerate of 'T

[<JavaScript; RequireQualifiedAccess>]
module Range =

  /// Return None for reversed finite endpoints or equal endpoints unless both are inclusive.
  let create lower upper =
    let valid =
      match lower, upper with
      | LowerBounds.Inclusive first, UpperBounds.Inclusive last -> compare first last <= 0
      | (LowerBounds.Inclusive first | LowerBounds.Exclusive first),
        (UpperBounds.Inclusive last | UpperBounds.Exclusive last) -> compare first last < 0
      | LowerBounds.Unbounded, _
      | _, UpperBounds.Unbounded -> true

    if valid then Some(Range.Range(lower, upper)) else None

  let closed lower upper =
    create (LowerBounds.Inclusive lower) (UpperBounds.Inclusive upper)

  let singleton value =
    Range.Range(LowerBounds.Inclusive value, UpperBounds.Inclusive value)

  /// Project endpoint values, preserving bound markers and endpoint positions.
  /// Return None if projection reverses finite endpoints or makes equal endpoints contradictory; never sort or compute a set image.
  let mapBounds projection (range: Range<'T>) =
    let lower =
      match range.Lower with
      | LowerBounds.Inclusive value -> LowerBounds.Inclusive(projection value)
      | LowerBounds.Exclusive value -> LowerBounds.Exclusive(projection value)
      | LowerBounds.Unbounded -> LowerBounds.Unbounded

    let upper =
      match range.Upper with
      | UpperBounds.Inclusive value -> UpperBounds.Inclusive(projection value)
      | UpperBounds.Exclusive value -> UpperBounds.Exclusive(projection value)
      | UpperBounds.Unbounded -> UpperBounds.Unbounded

    create lower upper

  /// Extract ordered, finite, inclusive endpoints without changing their values.
  let tryGetClosed (range: Range<'T>) =
    match range.Lower, range.Upper with
    | LowerBounds.Inclusive lower, UpperBounds.Inclusive upper -> Some(lower, upper)
    | _ -> None

  let inRange (range: Range<'T>) value =
    let lowerOk =
      match range.Lower with
      | LowerBounds.Inclusive lower -> value >= lower
      | LowerBounds.Exclusive lower -> value > lower
      | LowerBounds.Unbounded -> true

    let upperOk =
      match range.Upper with
      | UpperBounds.Inclusive upper -> value <= upper
      | UpperBounds.Exclusive upper -> value < upper
      | UpperBounds.Unbounded -> true

    lowerOk && upperOk

  /// Preserve bound semantics, using the explicit unbounded and single-value cases when applicable.
  let toInterval (range: Range<'T>) =
    match range.Lower, range.Upper with
    | LowerBounds.Unbounded, UpperBounds.Unbounded -> Interval.Unbounded
    | LowerBounds.Inclusive lower, UpperBounds.Inclusive upper when lower = upper -> Interval.Degenerate lower
    | _ -> Interval.Bounded range

[<JavaScript; RequireQualifiedAccess>]
module Interval =

  let create lower upper =
    Range.create lower upper |> Option.map Interval.Bounded

  let closed lower upper =
    Range.closed lower upper |> Option.map Interval.Bounded

  let singleton value = Interval.Degenerate value

  /// Project endpoint values while preserving the interval case and bound markers.
  /// Accepted Bounded intervals stay Bounded; collapsed finite endpoints require both ends inclusive.
  let mapBounds projection interval =
    match interval with
    | Interval.Bounded range -> range |> Range.mapBounds projection |> Option.map Interval.Bounded
    | Interval.Unbounded -> Some Interval.Unbounded
    | Interval.Degenerate value -> Some(Interval.Degenerate(projection value))

  let tryGetBounds interval =
    match interval with
    | Interval.Bounded range -> Some range
    | Interval.Unbounded -> None
    | Interval.Degenerate value -> Some(Range.singleton value)

  let inInterval interval value =
    match interval with
    | Interval.Bounded range -> value |> Range.inRange range
    | Interval.Unbounded -> true
    | Interval.Degenerate single -> single = value

  let (|WithinInterval|OutsideInterval|) interval value =
    if inInterval interval value then
      WithinInterval
    else
      OutsideInterval
