import assert from 'node:assert/strict';
import { readFileSync, readdirSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import test from 'node:test';
import { JSDOM } from 'jsdom';
import hljs from '../../src/Weave.Docs/highlighting/engine.js';

const wwwroot = new URL('../../src/Weave.Docs/wwwroot/', import.meta.url);
const bundle = readFileSync(new URL('vendor/highlightjs/weave-highlight.min.js', wwwroot), 'utf8');
const highlight = source => hljs.highlight(source, { language: 'fsharp' }).value;

test('F# bindings, records, numbers and member names have useful token scopes', () => {
  const html = highlight('type Person = { Name: string; Age: int }\nlet age = 18\nand! name = getName\nButton.primary(text "Save")');
  assert.match(html, /hljs-type">string/);
  assert.match(html, /hljs-type">int/);
  assert.match(html, /hljs-number">18/);
  assert.match(html, /hljs-keyword">and!/);
  assert.match(html, /hljs-title function_">primary/);
});

test('tuple, array, generic and union annotations retain their type scopes', () => {
  for (const [source, types] of [
    ['let f (x: (int * string) list) = x', ['int', 'string', 'list']],
    ['type Value = Value of (int * string)', ['int', 'string']],
    ['let values: int[] list = []', ['int', 'list']],
    ['let values: Map<string, int list> = Map.empty', ['string', 'int', 'list']],
    ['type Person = { Name: string; Age: int }', ['string', 'int']],
  ]) {
    const dom = new JSDOM(highlight(source));
    const actual = [...dom.window.document.querySelectorAll('.hljs-type')].map(span => span.textContent);
    assert.deepEqual(actual, types, source);
    dom.window.close();
  }
});

test('ranges and trailing-dot floats are separate from operators', () => {
  const dom = new JSDOM(highlight('let values = [18..21]\nlet one = 1.\nlet x = 0xFFuy\nlet y = 1_000M'));
  assert.deepEqual([...dom.window.document.querySelectorAll('.hljs-number')].map(span => span.textContent),
    ['18', '21', '1.', '0xFFuy', '1_000M']);
  assert.ok([...dom.window.document.querySelectorAll('.hljs-operator')].some(span => span.textContent === '..'));
  dom.window.close();
});

test('all existing triple-quoted docs strings preserve their source text', () => {
  const examples = new URL('../../src/Weave.Docs/examples/', import.meta.url);
  const dom = new JSDOM();
  const container = dom.window.document.createElement('div');
  let checked = 0;
  for (const file of readdirSync(examples).filter(name => name.endsWith('.fs'))) {
    for (const [, source] of readFileSync(new URL(file, examples), 'utf8').matchAll(/"""([\s\S]*?)"""/g)) {
      container.innerHTML = highlight(source);
      assert.equal(container.textContent, source.replace(/\r\n?/g, '\n'), file);
      checked++;
    }
  }
  assert.ok(checked > 0, 'The docs source sweep must exercise actual strings');
  dom.window.close();
});

function fixture(language = 'fsharp') {
  // The bundle is trusted local build output; the source strings are always text.
  const dom = new JSDOM('<pre><code></code></pre>', { runScripts: 'outside-only' });
  dom.window.eval(bundle);
  const element = dom.window.document.querySelector('code');
  element.className = 'language-' + language;
  return { dom, element, render: () => dom.window.highlightCodeElement(element) };
}

test('rendering twice and replacing spans with identical text both preserve highlighting', () => {
  const { dom, element, render } = fixture();
  const source = 'let age = 18';
  element.textContent = source;
  render();
  const markup = element.innerHTML;
  render();
  assert.equal(element.innerHTML, markup);
  element.textContent = source;
  render();
  assert.equal(element.innerHTML, markup);
  assert.equal(element.querySelector('.hljs-keyword').textContent, 'let');
  dom.window.close();
});

test('an existing code element can change source and language', () => {
  const { dom, element, render } = fixture();
  element.textContent = 'let age = 18';
  render();
  element.textContent = 'let age = 21';
  render();
  assert.equal(element.querySelector('.hljs-number').textContent, '21');
  element.className = 'language-plaintext';
  render();
  assert.equal(element.textContent, 'let age = 21');
  assert.equal(element.querySelectorAll('span').length, 0);
  dom.window.close();
});

test('HTML in code stays text and cannot become active elements', () => {
  const source = 'let html = "<img src=x onerror=alert(1)><script>alert(1)</script>"';
  const { dom, element, render } = fixture();
  element.textContent = source;
  render();
  assert.equal(element.textContent, source);
  assert.equal(element.querySelectorAll('img, script').length, 0);
  assert.ok(element.querySelector('.hljs-string'));
  element.textContent = source + '\nlet other = "<b>example</b>"';
  render();
  assert.equal(element.querySelectorAll('img, script, b').length, 0);
  dom.window.close();
});

test('the setup page languages are included in the same bundle', () => {
  for (const [language, source, selector] of [
    ['bash', 'echo "hello"', '.hljs-string'],
    ['html', '<link rel="stylesheet" href="Scripts/Weave/styles.css">', '.hljs-tag'],
  ]) {
    const { dom, element, render } = fixture(language);
    element.textContent = source;
    render();
    assert.equal(element.textContent, source);
    assert.ok(element.querySelector(selector));
    dom.window.close();
  }
});

test('the shipped license matches the pinned upstream package', () => {
  const upstream = readFileSync(fileURLToPath(new URL('../../node_modules/highlight.js/LICENSE', import.meta.url)), 'utf8');
  const shipped = readFileSync(new URL('vendor/highlightjs/LICENSE', wwwroot), 'utf8');
  assert.equal(shipped.replace(/\r\n/g, '\n'), upstream.replace(/\r\n/g, '\n'));
});
