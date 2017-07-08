const markdown = require('markdown-it')();
const katex = require('markdown-it-katex');

export class MarkdownValueConverter {
    toView(value) {
        markdown.use(katex);
        return markdown.render(value);
    }
}