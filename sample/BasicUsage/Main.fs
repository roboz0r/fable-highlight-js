open HighlightJS

let html = hljs.highlightAuto("<h1>Hello World!</h1>").value
let html2 = hljs.highlight("<h1>Hello World!</h1>", HighlightOptions("xml")).value
printfn "%s" html
printfn "%s" html2

hljs.registerLanguage ("javascript", Languages.javascript)

let js =
    hljs.highlight("console.log('Hello World!');", HighlightOptions("javascript")).value

printfn "%s" js
