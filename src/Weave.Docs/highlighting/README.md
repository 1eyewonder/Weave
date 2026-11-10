# Docs code highlighting

This is a docs-only browser feature. No highlighter dependency is shipped in the
Weave NuGet package. ExampleHelpers.fs keeps calling window.highlightCodeElement
after WebSharper renders each code element; Copy continues using the original source.

## Ownership

- engine.js registers only F#, Bash, HTML/XML and plain text against the pinned
  highlight.js package in the root package.json and yarn.lock.
- fsharp.js is our fork of highlight.js 11.11.1 src/languages/fsharp.js.
- regex.js is unchanged from that release's src/lib/regex.js.
- index.js is the WebSharper hook; it always highlights from text, including when
  an existing element gets the same source again or changes language.
- ../wwwroot/code-highlighting.css owns the light/dark editor colors and follows
  the active Weave palette. Light mode separates keywords (berry), types (green),
  functions (blue), strings (amber) and numbers (violet). Dark mode keeps its
  own token mapping. Neutral identifiers and
  15px code (medium weight in light mode) improve readability.
- ../wwwroot/vendor/highlightjs/LICENSE covers highlight.js and the forked sources
  (BSD-3-Clause). Keep the upstream author credits and license when updating.

Upstream source: https://github.com/highlightjs/highlight.js/tree/11.11.1

## Build and checks

Run yarn build:docs:highlighting to rebuild the single browser asset,
wwwroot/vendor/highlightjs/weave-highlight.min.js. The docs project also runs this
before building, for Debug and Release. The generated bundle is ignored; the
source, pinned dependency and license are tracked. Install with yarn install
--immutable before building, as with the existing Sass build.

Run yarn test:docs:highlighting for grammar, text preservation and real DOM
re-render/escaping checks. RunTests and RunNonBrowserTests also run this command,
so CI uses its existing test job. The DOM tests use jsdom and do
not require an HTTP server, browser download, or the F# compiler.

## Local grammar changes and limits

The fork adds and!, F# numeric suffix/range handling, record annotation
boundaries, and identifier/member scopes. Semicolons and closing record braces
end an annotation; parentheses and brackets remain part of tuple/array types.
Capitalized identifiers and dotted lowercase members use visual heuristics.
This is lexical highlighting: it cannot reliably distinguish modules, types,
record fields and functions, or provide inferred-type tooltips.

When updating upstream, compare the grammar and regex helper against the pinned
release, reapply these changes, retain the license, and run the regression suite
and a docs build. Check the code blocks in both themes before publishing.

Compiler-aware snippets with FSharp.Formatting would require a separate build
tool and checked source files. They are not part of this feature.
