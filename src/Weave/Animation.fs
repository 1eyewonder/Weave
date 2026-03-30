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

    let fadeIn = cl Css.``weave-animation--fade-in``
    let scaleIn = cl Css.``weave-animation--scale-in``
    let scaleYIn = cl Css.``weave-animation--scale-y-in``
    let slideUpIn = cl Css.``weave-animation--slide-up-in``
    let slideDownIn = cl Css.``weave-animation--slide-down-in``
    let slideLeftIn = cl Css.``weave-animation--slide-left-in``
    let slideRightIn = cl Css.``weave-animation--slide-right-in``

    let internal toClass entrance =
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

    let fadeOut = cl Css.``weave-animation--fade-out``
    let scaleOut = cl Css.``weave-animation--scale-out``
    let scaleYOut = cl Css.``weave-animation--scale-y-out``
    let slideUpOut = cl Css.``weave-animation--slide-up-out``
    let slideDownOut = cl Css.``weave-animation--slide-down-out``
    let slideLeftOut = cl Css.``weave-animation--slide-left-out``
    let slideRightOut = cl Css.``weave-animation--slide-right-out``

    let internal toClass exit =
      match exit with
      | AnimationExit.FadeOut -> Css.``weave-animation--fade-out``
      | AnimationExit.ScaleOut -> Css.``weave-animation--scale-out``
      | AnimationExit.ScaleYOut -> Css.``weave-animation--scale-y-out``
      | AnimationExit.SlideUpOut -> Css.``weave-animation--slide-up-out``
      | AnimationExit.SlideDownOut -> Css.``weave-animation--slide-down-out``
      | AnimationExit.SlideLeftOut -> Css.``weave-animation--slide-left-out``
      | AnimationExit.SlideRightOut -> Css.``weave-animation--slide-right-out``

  /// Emphasis animations — drawing attention to elements.
  module AnimationEmphasis =

    let pulse = cl Css.``weave-animation--pulse``
    let shake = cl Css.``weave-animation--shake``
    let bounce = cl Css.``weave-animation--bounce``

  /// Applies weave-animation--none, disabling all animations AND transitions
  /// on the element and its entire subtree (via * selector with !important).
  /// Use this to hard-suppress motion in a component subtree — not as a "no animation" sentinel.
  /// To conditionally apply no animation, simply omit the animation class instead.
  module AnimationKind =

    let suppress = cl Css.``weave-animation--none``

  /// Overrides the animation duration set by the kind class.
  module AnimationDuration =

    let shortest = cl Css.``weave-animation-duration--shortest``
    let shorter = cl Css.``weave-animation-duration--shorter``
    let short = cl Css.``weave-animation-duration--short``
    let standard = cl Css.``weave-animation-duration--standard``
    let medium = cl Css.``weave-animation-duration--medium``
    let long = cl Css.``weave-animation-duration--long``
    let longer = cl Css.``weave-animation-duration--longer``
    let longest = cl Css.``weave-animation-duration--longest``

  /// Overrides the travel distance of slide, shake, and bounce animations.
  /// Each level scales the entire set of --weave-animation-distance-* tokens
  /// proportionally, from subtle (extraSmall) to dramatic (extraLarge).
  module AnimationDistance =

    let extraSmall = cl Css.``weave-animation-distance--extra-small``
    let small = cl Css.``weave-animation-distance--small``
    let medium = cl Css.``weave-animation-distance--medium``
    let large = cl Css.``weave-animation-distance--large``
    let extraLarge = cl Css.``weave-animation-distance--extra-large``

  /// Overrides the scale intensity of scale-in/out, scale-y-in/out, and pulse
  /// animations. Values closer to identity (1.0) produce subtler motion;
  /// values further from identity produce more dramatic motion.
  module AnimationScale =

    let extraSmall = cl Css.``weave-animation-scale--extra-small``
    let small = cl Css.``weave-animation-scale--small``
    let medium = cl Css.``weave-animation-scale--medium``
    let large = cl Css.``weave-animation-scale--large``
    let extraLarge = cl Css.``weave-animation-scale--extra-large``

  /// Overrides the animation timing function set by the kind class.
  module AnimationEasing =

    let standard = cl Css.``weave-animation-easing--standard``
    let decelerate = cl Css.``weave-animation-easing--decelerate``
    let accelerate = cl Css.``weave-animation-easing--accelerate``
    let bounce = cl Css.``weave-animation-easing--bounce``

  /// Controls animation iteration count (CSS-only, seamless looping).
  /// For periodic replay with pauses between plays, use Animate.replayEvery instead.
  module AnimationIteration =

    let infinite = cl Css.``weave-animation-iteration--infinite``

  /// Stagger delay helpers for sequenced list animations.
  /// The SCSS generates .weave-animation-delay--1 through --10.
  module AnimationDelay =

    /// Returns the CSS class for a stagger delay step.
    /// Steps are clamped to the valid range 1-10.
    let stagger (step: int) =
      cl (sprintf "weave-animation-delay--%d" (max 1 (min 10 step)))

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
  /// Defers animation playback to a CSS pseudo-state trigger (hover, focus, or both).
  module AnimationOn =

    let hover = cl Css.``weave-animation-on--hover``
    let focus = cl Css.``weave-animation-on--focus``
    let hoverFocus = cl Css.``weave-animation-on--hover-focus``

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
