import { build } from 'esbuild';
import { fileURLToPath } from 'node:url';

await build({
  entryPoints: [fileURLToPath(new URL('./highlighting/index.js', import.meta.url))],
  outfile: fileURLToPath(new URL('./wwwroot/vendor/highlightjs/weave-highlight.min.js', import.meta.url)),
  bundle: true,
  minify: true,
  format: 'iife',
  legalComments: 'inline',
  banner: {
    js: '/*! highlight.js 11.11.1 and Weave F# grammar; BSD-3-Clause. See vendor/highlightjs/LICENSE. */'
  }
});
