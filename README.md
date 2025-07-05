# Fable.HighlightJS

Provides bindings for the [highlight.js](https://github.com/highlightjs/highlight.js) library.

## Usage

```fs
open HighlightJS

let xml = "<h1>Hello World!</h1>"
let hljs = hljs.newInstance()
hljs.registerLanguage("xml", Languages.xml)
let highlighted = hljs.highlight(xml, HighlightOptions("xml")).value
```

## Testing

Run tests

```pwsh
dotnet fable .\test\Tests.fsproj -o .\test\build\ --run node .\test\build\Program.js
```
