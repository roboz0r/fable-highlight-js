module Tests

open Fable.Pyxpecto
open HighlightJS

let tests =
    testList
        "Tests"
        [
            test "highlight xml" {
                let xml = "<h1>Hello World!</h1>"
                let hljs = hljsCommonLanguages
                let highlighted = hljs.highlight(xml, HighlightOptions("xml")).value

                let expected =
                    """<span class="hljs-tag">&lt;<span class="hljs-name">h1</span>&gt;</span>Hello World!<span class="hljs-tag">&lt;/<span class="hljs-name">h1</span>&gt;</span>"""

                "" |> Expect.equal highlighted expected
            }
            test "highlight fails with no languages" {
                let xml = "<h1>Hello World!</h1>"
                let hljs = hljs.newInstance ()

                Expect.throwsC
                    (fun () -> hljs.highlight (xml, HighlightOptions("xml")) |> ignore)
                    (fun ex -> "" |> Expect.isTrue (ex.Message.Contains("Unknown language: \"xml\"")))
            }

            test "highlight xml with registered language" {
                let xml = "<h1>Hello World!</h1>"
                let hljs = hljs.newInstance ()
                hljs.registerLanguage ("xml", Languages.xml)
                let highlighted = hljs.highlight(xml, HighlightOptions("xml")).value

                let expected =
                    """<span class="hljs-tag">&lt;<span class="hljs-name">h1</span>&gt;</span>Hello World!<span class="hljs-tag">&lt;/<span class="hljs-name">h1</span>&gt;</span>"""

                "" |> Expect.equal highlighted expected
            }
        ]
