namespace Weave.CssHelpers

open WebSharper
open Weave.CssHelpers.Core

[<AutoOpen; JavaScript>]
module Animation =

  /// Entrance animations — elements appearing in the UI.
  [<RequireQualifiedAccess; Struct>]
  type AnimationEntrance =
    | FadeIn
    | ScaleIn
    | ScaleYIn
    | SlideUpIn
    | SlideDownIn
    | SlideLeftIn
    | SlideRightIn

  module AnimationEntrance =

    let toClass entrance =
      match entrance with
      | AnimationEntrance.FadeIn -> Css.``weave-animation--fade-in``
      | AnimationEntrance.ScaleIn -> Css.``weave-animation--scale-in``
      | AnimationEntrance.ScaleYIn -> Css.``weave-animation--scale-y-in``
      | AnimationEntrance.SlideUpIn -> Css.``weave-animation--slide-up-in``
      | AnimationEntrance.SlideDownIn -> Css.``weave-animation--slide-down-in``
      | AnimationEntrance.SlideLeftIn -> Css.``weave-animation--slide-left-in``
      | AnimationEntrance.SlideRightIn -> Css.``weave-animation--slide-right-in``

  /// Exit animations — elements leaving the UI.
  [<RequireQualifiedAccess; Struct>]
  type AnimationExit =
    | FadeOut
    | ScaleOut
    | ScaleYOut
    | SlideUpOut
    | SlideDownOut
    | SlideLeftOut
    | SlideRightOut

  module AnimationExit =

    let toClass exit =
      match exit with
      | AnimationExit.FadeOut -> Css.``weave-animation--fade-out``
      | AnimationExit.ScaleOut -> Css.``weave-animation--scale-out``
      | AnimationExit.ScaleYOut -> Css.``weave-animation--scale-y-out``
      | AnimationExit.SlideUpOut -> Css.``weave-animation--slide-up-out``
      | AnimationExit.SlideDownOut -> Css.``weave-animation--slide-down-out``
      | AnimationExit.SlideLeftOut -> Css.``weave-animation--slide-left-out``
      | AnimationExit.SlideRightOut -> Css.``weave-animation--slide-right-out``

  /// Emphasis animations — drawing attention to elements.
  [<RequireQualifiedAccess; Struct>]
  type AnimationEmphasis =
    | Pulse
    | Shake
    | Bounce

  module AnimationEmphasis =

    let toClass emphasis =
      match emphasis with
      | AnimationEmphasis.Pulse -> Css.``weave-animation--pulse``
      | AnimationEmphasis.Shake -> Css.``weave-animation--shake``
      | AnimationEmphasis.Bounce -> Css.``weave-animation--bounce``

  /// Top-level union for applying any single animation via attrs.
  [<RequireQualifiedAccess>]
  type AnimationKind =
    | Entrance of AnimationEntrance
    | Exit of AnimationExit
    | Emphasis of AnimationEmphasis
    /// Applies weave-animation--none, which disables all animations AND transitions
    /// on the element and its entire subtree (via * selector with !important).
    /// Use this to hard-suppress motion in a component subtree — not as a "no animation" sentinel.
    /// To conditionally apply no animation, simply omit the kind class instead.
    | Suppress

  module AnimationKind =

    let toClass kind =
      match kind with
      | AnimationKind.Entrance entrance -> AnimationEntrance.toClass entrance
      | AnimationKind.Exit exit -> AnimationExit.toClass exit
      | AnimationKind.Emphasis emphasis -> AnimationEmphasis.toClass emphasis
      | AnimationKind.Suppress -> Css.``weave-animation--none``

  /// Overrides the animation duration set by the kind class.
  [<RequireQualifiedAccess; Struct>]
  type AnimationDuration =
    | Shortest
    | Shorter
    | Short
    | Standard
    | Medium
    | Long
    | Longer
    | Longest

  module AnimationDuration =

    let toClass duration =
      match duration with
      | AnimationDuration.Shortest -> Css.``weave-animation-duration--shortest``
      | AnimationDuration.Shorter -> Css.``weave-animation-duration--shorter``
      | AnimationDuration.Short -> Css.``weave-animation-duration--short``
      | AnimationDuration.Standard -> Css.``weave-animation-duration--standard``
      | AnimationDuration.Medium -> Css.``weave-animation-duration--medium``
      | AnimationDuration.Long -> Css.``weave-animation-duration--long``
      | AnimationDuration.Longer -> Css.``weave-animation-duration--longer``
      | AnimationDuration.Longest -> Css.``weave-animation-duration--longest``

  /// Overrides the animation timing function set by the kind class.
  [<RequireQualifiedAccess; Struct>]
  type AnimationEasing =
    | Standard
    | Decelerate
    | Accelerate
    | Bounce

  module AnimationEasing =

    let toClass easing =
      match easing with
      | AnimationEasing.Standard -> Css.``weave-animation-easing--standard``
      | AnimationEasing.Decelerate -> Css.``weave-animation-easing--decelerate``
      | AnimationEasing.Accelerate -> Css.``weave-animation-easing--accelerate``
      | AnimationEasing.Bounce -> Css.``weave-animation-easing--bounce``

  /// Controls animation iteration count (CSS-only, seamless looping).
  /// For periodic replay with pauses between plays, use Animate.replayEvery instead.
  [<RequireQualifiedAccess; Struct>]
  type AnimationIteration = | Infinite

  module AnimationIteration =

    let toClass iteration =
      match iteration with
      | AnimationIteration.Infinite -> Css.``weave-animation-iteration--infinite``

  /// Stagger delay helpers for sequenced list animations.
  /// The SCSS generates .weave-animation-delay--1 through --10.
  module AnimationDelay =

    /// Returns the CSS class for a stagger delay step.
    /// Steps are clamped to the valid range 1-10.
    let stagger (step: int) =
      sprintf "weave-animation-delay--%d" (max 1 (min 10 step))

  /// Defers animation playback to a pseudo-state trigger (hover, focus, or both).
  /// Compose with any animation kind class — the trigger overrides
  /// animation-name to none at rest and restores it on the pseudo-state.
  ///
  /// Best suited for emphasis animations (Pulse, Shake, Bounce) whose keyframes
  /// start and end at the same visual state. Entrance/exit animations will flash
  /// to their "from" keyframe on trigger, which is usually not what you want.
  ///
  /// These are CSS-only triggers. For JS-based triggers (click, timer), see
  /// Animate.replayOnClick and Animate.replayEvery in Utilities.fs.
  /// Do not combine AnimationOn with Animate replay helpers on the same element —
  /// the CSS trigger's animation-name: none will override the JS replay.
  [<RequireQualifiedAccess; Struct>]
  type AnimationOn =
    | Hover
    | Focus
    | HoverFocus

  module AnimationOn =

    let toClass trigger =
      match trigger with
      | AnimationOn.Hover -> Css.``weave-animation-on--hover``
      | AnimationOn.Focus -> Css.``weave-animation-on--focus``
      | AnimationOn.HoverFocus -> Css.``weave-animation-on--hover-focus``

  type AnimationPair = {
    Enter: AnimationEntrance
    Exit: AnimationExit
  }

  module AnimationPair =

    let create enter exit = {
      Enter = enter
      Exit = exit
    }

    let fadeInOut = create AnimationEntrance.FadeIn AnimationExit.FadeOut

    let scaleInOut = create AnimationEntrance.ScaleIn AnimationExit.ScaleOut

    let scaleYInOut = create AnimationEntrance.ScaleYIn AnimationExit.ScaleYOut

    let slideUpInOut = create AnimationEntrance.SlideUpIn AnimationExit.SlideDownOut

    let slideDownInOut = create AnimationEntrance.SlideDownIn AnimationExit.SlideUpOut

    let slideLeftInOut =
      create AnimationEntrance.SlideLeftIn AnimationExit.SlideRightOut

    let slideRightInOut =
      create AnimationEntrance.SlideRightIn AnimationExit.SlideLeftOut
