import hljs from 'highlight.js/lib/core';
import bash from 'highlight.js/lib/languages/bash';
import xml from 'highlight.js/lib/languages/xml';
import plaintext from 'highlight.js/lib/languages/plaintext';
import fsharp from './fsharp.js';

// Only languages used by the docs are included in the browser bundle.
hljs.registerLanguage('fsharp', fsharp);
hljs.registerLanguage('bash', bash);
hljs.registerLanguage('xml', xml);
hljs.registerLanguage('plaintext', plaintext);

export default hljs;
