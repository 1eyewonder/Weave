import hljs from './engine.js';

// WebSharper calls this after rendering a code element. Rebuild from its text
// each time: a reactive update can replace the spans without changing the source,
// or reuse the element for a different language.
window.highlightCodeElement = function (element) {
  if (!element) return;
  const source = element.textContent;
  element.textContent = source;
  delete element.dataset.highlighted;
  hljs.highlightElement(element);
};
