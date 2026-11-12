# Docs code highlighting

The docs site highlights code blocks and API type signatures in the browser. This dependency is not shipped in the Weave NuGet package. Copy uses the original source text.

## Build and test

From the repository root, run:

```sh
yarn build:docs:highlighting
yarn test:docs:highlighting
```

The docs build also regenerates the browser bundle. The test suite checks grammar, text preservation, repeated rendering, and HTML escaping with jsdom; it does not need a browser server.

## Source and ownership

- [engine.js](engine.js) registers F#, its API type-signature mode, Bash, HTML/XML, and plain text. [fsharp.js](fsharp.js) extends the pinned highlight.js F# grammar; [regex.js](regex.js) is unchanged from upstream.
- [index.js](index.js) highlights from element text after WebSharper renders it. [ExampleHelpers.fs](../examples/shared/ExampleHelpers.fs) calls that hook for code and type signatures.
- [code-highlighting.css](../wwwroot/code-highlighting.css) supplies the light and dark token colors. The [highlight.js license](../wwwroot/vendor/highlightjs/LICENSE) covers the bundled upstream code and fork.

The grammar handles F# numeric suffixes, ranges, record annotations, and member names. API types use the same token rules as type annotations in code blocks. Highlighting is lexical: it cannot reliably distinguish every module, type, or member from context.

When updating the pinned [highlight.js source](https://github.com/highlightjs/highlight.js/tree/11.11.1), compare the local grammar and regex helper with upstream, retain its license, run the tests and docs build, and inspect code blocks in both themes.
