import { cpSync, readdirSync, existsSync } from 'fs'
import { build } from 'esbuild'

cpSync('src/Weave.Docs/build/', 'src/Weave.Docs/wwwroot/Scripts/', { recursive: true });

const prebundles = readdirSync('src/Weave.Docs/build/');

prebundles.forEach(file => {
  if (file.endsWith('.js')) {
    var options =
    {
      entryPoints: ['src/Weave.Docs/build/' + file],
      bundle: true,
      minify: true,
      format: 'iife',
      outfile: 'src/Weave.Docs/wwwroot/Scripts/' + file,
      globalName: 'wsbundle'
    };

    console.log("Bundling:", file);
    build(options);
  }
});

if (existsSync('src/Weave.Docs/build/workers/')) {
  const workers = readdirSync('src/Weave.Docs/build/workers/');

  workers.forEach(file => {
    if (file.endsWith('.js')) {
      var options =
      {
        entryPoints: ['src/Weave.Docs/build/workers/' + file],
        bundle: true,
        minify: true,
        format: 'iife',
        outfile: 'src/Weave.Docs/wwwroot/Scripts/workers/' + file,
      };

      console.log("Bundling worker:", file);
      build(options);
    }
  });
}