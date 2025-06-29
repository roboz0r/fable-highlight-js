// ts2fable 0.9.0
module rec HighlightJS

open System
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Browser.Types

/// Represents a POJO with keys of type 'K and values of type 'V.
type Record<'K, 'V> = interface end

type Record<'K, 'V> with

    static member inline Create(props: seq<(float * 'V)>) : Record<float, 'V> =
        createObj
            [
                for (k, v) in props do
                    yield (string k) ==> v
            ]
        |> unbox

    static member inline Create(props: seq<(string * 'V)>) : Record<string, 'V> =
        createObj
            [
                for (k, v) in props do
                    yield k ==> v
            ]
        |> unbox

type Array<'T> = System.Collections.Generic.IList<'T>
type Error = System.Exception
type RegExp = System.Text.RegularExpressions.Regex

module HighlightJS_private =
    type CompiledMode = HighlightJS.CompiledMode
    type Mode = HighlightJS.Mode
    type Language = HighlightJS.Language

    [<StringEnum>]
    [<RequireQualifiedAccess>]
    type MatchType =
        | Begin
        | End
        | Illegal

    [<AllowNullLiteral>]
    type EnhancedMatch = interface end

    [<AllowNullLiteral>]
    type AnnotatedError = interface end

    type KeywordData = string * float

    type KeywordDict = Record<string, KeywordData>

[<ImportDefault("highlight.js")>]
let hljs: HLJSApi = jsNative

[<AutoOpen>]
module HighlightJS =
    type KeywordDict = HighlightJS_private.KeywordDict

    [<AllowNullLiteral>]
    type HLJSApi =
        inherit PublicApi
        inherit ModesAPI

    [<AllowNullLiteral>]
    type VuePlugin =
        abstract install: (obj option -> unit) with get, set

    [<AllowNullLiteral>]
    type RegexEitherOptions =
        abstract capture: bool option with get, set

    [<AllowNullLiteral>]
    type PublicApi =
        abstract highlight: code: string * options: HighlightOptions -> HighlightResult

        [<Obsolete("use `higlight(code, {lang: ..., ignoreIllegals: ...})`")>]
        abstract highlight: languageName: string * code: string * ?ignoreIllegals: bool -> HighlightResult

        abstract highlightAuto: code: string * ?languageSubset: ResizeArray<string> -> AutoHighlightResult
        abstract highlightBlock: element: HTMLElement -> unit
        abstract highlightElement: element: HTMLElement -> unit
        abstract configure: options: HLJSOptions -> unit
        abstract initHighlighting: unit -> unit
        abstract initHighlightingOnLoad: unit -> unit
        abstract highlightAll: unit -> unit
        abstract registerLanguage: languageName: string * language: LanguageFn -> unit
        abstract unregisterLanguage: languageName: string -> unit
        abstract listLanguages: unit -> ResizeArray<string>
        abstract registerAliases: aliasList: U2<string, ResizeArray<string>> * p1: {| languageName: string |} -> unit
        abstract getLanguage: languageName: string -> Language option
        abstract autoDetection: languageName: string -> bool
        abstract ``inherit``: original: 'T * [<ParamArray>] args: Record<string, obj option>[] -> 'T
        abstract addPlugin: plugin: HLJSPlugin -> unit
        abstract removePlugin: plugin: HLJSPlugin -> unit
        abstract debugMode: unit -> unit
        abstract safeMode: unit -> unit
        abstract versionString: string with get, set
        abstract vuePlugin: unit -> VuePlugin
        abstract regex: PublicApiRegex with get, set
        abstract newInstance: unit -> HLJSApi

    [<AllowNullLiteral>]
    type ModesAPI =
        abstract SHEBANG: ((obj) option -> Mode) with get, set
        abstract BACKSLASH_ESCAPE: Mode with get, set
        abstract QUOTE_STRING_MODE: Mode with get, set
        abstract APOS_STRING_MODE: Mode with get, set
        abstract PHRASAL_WORDS_MODE: Mode with get, set
        abstract COMMENT: (U2<string, RegExp> -> U2<string, RegExp> -> (U2<Mode, ModesAPICOMMENT>) option -> Mode) with get, set
        abstract C_LINE_COMMENT_MODE: Mode with get, set
        abstract C_BLOCK_COMMENT_MODE: Mode with get, set
        abstract HASH_COMMENT_MODE: Mode with get, set
        abstract NUMBER_MODE: Mode with get, set
        abstract C_NUMBER_MODE: Mode with get, set
        abstract BINARY_NUMBER_MODE: Mode with get, set
        abstract REGEXP_MODE: Mode with get, set
        abstract TITLE_MODE: Mode with get, set
        abstract UNDERSCORE_TITLE_MODE: Mode with get, set
        abstract METHOD_GUARD: Mode with get, set
        abstract END_SAME_AS_BEGIN: (Mode -> Mode) with get, set
        abstract IDENT_RE: string with get, set
        abstract UNDERSCORE_IDENT_RE: string with get, set
        abstract MATCH_NOTHING_RE: string with get, set
        abstract NUMBER_RE: string with get, set
        abstract C_NUMBER_RE: string with get, set
        abstract BINARY_NUMBER_RE: string with get, set
        abstract RE_STARTERS_RE: string with get, set

    type LanguageFn = delegate of hljs: HLJSApi -> Language

    type CompilerExt = delegate of mode: Mode * parent: U2<Mode, Language> option -> unit

    [<AllowNullLiteral>]
    type HighlightResult2ndBest =
        abstract code: string option with get, set
        abstract relevance: float with get, set
        abstract value: string with get, set
        abstract language: string option with get, set
        abstract illegal: bool with get, set
        abstract errorRaised: Error option with get, set
        abstract _illegalBy: illegalData option with get, set
        abstract _emitter: Emitter with get, set
        abstract _top: U2<Language, CompiledMode> option with get, set

    [<AllowNullLiteral>]
    type HighlightResult =
        abstract code: string option with get, set
        abstract relevance: float with get, set
        abstract value: string with get, set
        abstract language: string option with get, set
        abstract illegal: bool with get, set
        abstract errorRaised: Error option with get, set
        abstract secondBest: HighlightResult2ndBest option with get, set
        abstract _illegalBy: illegalData option with get, set
        abstract _emitter: Emitter with get, set
        abstract _top: U2<Language, CompiledMode> option with get, set

    [<AllowNullLiteral>]
    type AutoHighlightResult =
        inherit HighlightResult

    [<AllowNullLiteral>]
    type illegalData =
        abstract message: string with get, set
        abstract context: string with get, set
        abstract index: float with get, set
        abstract resultSoFar: string with get, set
        abstract mode: CompiledMode with get, set

    [<AllowNullLiteral>]
    type BeforeHighlightContext =
        abstract code: string with get, set
        abstract language: string with get, set
        abstract result: HighlightResult option with get, set

    /// keyof HLJSPlugin
    [<StringEnum; RequireQualifiedAccess>]
    type PluginEvent =
        | [<CompiledName("after:highlight")>] AfterHighlight
        | [<CompiledName("before:highlight")>] BeforeHighlight
        | [<CompiledName("after:highlightElement")>] AfterHighlightElement
        | [<CompiledName("before:highlightElement")>] BeforeHighlightElement
        | [<CompiledName("after:highlightBlock")>] AfterHighlightBlock
        | [<CompiledName("before:highlightBlock")>] BeforeHighlightBlock

    [<AllowNullLiteral>]
    type HLJSPlugin =
        abstract ``after:highlight``: (HighlightResult -> unit) option with get, set
        abstract ``before:highlight``: (BeforeHighlightContext -> unit) option with get, set

        abstract ``after:highlightElement``:
            ({|
                el: Element
                result: HighlightResult
                text: string
            |}
                -> unit) option with get, set

        abstract ``before:highlightElement``: ({| el: Element; language: string |} -> unit) option with get, set

        abstract ``after:highlightBlock``:
            ({|
                block: Element
                result: HighlightResult
                text: string
            |}
                -> unit) option with get, set

        abstract ``before:highlightBlock``: ({| block: Element; language: string |} -> unit) option with get, set

    [<AllowNullLiteral>]
    type EmitterConstructor = interface end

    [<AllowNullLiteral>]
    type EmitterConstructorStatic =
        [<EmitConstructor>]
        abstract Create: opts: obj option -> EmitterConstructor

    [<AllowNullLiteral>]
    [<Global>]
    type HighlightOptions [<ParamObject; Emit("$0")>] (language: string, ?ignoreIllegals: bool) =
        member val language: string = jsNative with get, set
        member val ignoreIllegals: bool option = jsNative with get, set

    [<AllowNullLiteral>]
    [<Global>]
    type HLJSOptions
        [<ParamObject; Emit("$0")>]
        (
            ?languages: ResizeArray<string>,
            ?noHighlightRe: RegExp,
            ?languageDetectRe: RegExp,
            ?classPrefix: string,
            ?cssSelector: string,
            ?ignoreUnescapedHTML: bool,
            ?throwUnescapedHTML: bool
        ) =
        member val noHighlightRe: RegExp = jsNative with get, set
        member val languageDetectRe: RegExp = jsNative with get, set
        member val classPrefix: string = jsNative with get, set
        member val cssSelector: string = jsNative with get, set
        member val languages: ResizeArray<string> option = jsNative with get, set
        member val __emitter: EmitterConstructor = jsNative with get, set
        member val ignoreUnescapedHTML: bool option = jsNative with get, set
        member val throwUnescapedHTML: bool option = jsNative with get, set

    [<AllowNullLiteral>]
    type CallbackResponse =
        abstract data: Record<string, obj option> with get, set
        abstract ignoreMatch: (unit -> unit) with get, set
        abstract isMatchIgnored: bool with get, set

    [<AllowNullLiteral>]
    type RegExpMatchArray =
        abstract groups: RegExpMatchArrayGroups option with get, set

    [<AllowNullLiteral>]
    type RegExpMatchArrayGroups =
        [<EmitIndexer>]
        abstract Item: key: string -> string with get, set

    type ModeCallback = delegate of ``match``: RegExpMatchArray * response: CallbackResponse -> unit

    [<AllowNullLiteral>]
    type Language =
        inherit LanguageDetail

    [<AllowNullLiteral>]
    type Mode =
        inherit ModeCallbacks
        inherit ModeDetails

    [<AllowNullLiteral>]
    type LanguageDetail =
        abstract name: string option with get, set
        abstract unicodeRegex: bool option with get, set
        abstract rawDefinition: (unit -> Language) option with get, set
        abstract aliases: ResizeArray<string> option with get, set
        abstract disableAutodetect: bool option with get, set
        abstract contains: ResizeArray<Mode> with get, set
        abstract case_insensitive: bool option with get, set

        abstract keywords:
            U3<string, ResizeArray<string>, Record<string, U3<string, ResizeArray<string>, RegExp>>> option with get, set

        abstract isCompiled: bool option with get, set
        abstract exports: obj option with get, set
        abstract classNameAliases: Record<string, string> option with get, set
        abstract compilerExtensions: ResizeArray<CompilerExt> option with get, set
        abstract supersetOf: string option with get, set

    [<AllowNullLiteral>]
    type Emitter =
        abstract startScope: name: string -> unit
        abstract endScope: unit -> unit
        abstract addText: text: string -> unit
        abstract toHTML: unit -> string
        abstract finalize: unit -> unit
        abstract __addSublanguage: emitter: Emitter * subLanguageName: string -> unit

    [<AllowNullLiteral>]
    type HighlightedHTMLElementAdditions =
        abstract result: obj option with get, set
        abstract secondBest: obj option with get, set
        abstract parentNode: HTMLElement with get, set

    [<AllowNullLiteral>]
    type HighlightedHTMLElement =
        inherit HTMLElement
        inherit HighlightedHTMLElementAdditions

    [<AllowNullLiteral>]
    type ModeCallbacks =
        abstract ``on:end``: Function option with get, set
        abstract ``on:begin``: ModeCallback option with get, set

    [<AllowNullLiteral>]
    type CompiledLanguage =
        inherit LanguageDetail
        inherit CompiledMode
        abstract isCompiled: bool with get, set
        abstract contains: ResizeArray<CompiledMode> with get, set
        abstract keywords: Record<string, obj option> with get, set

    [<AllowNullLiteral>]
    type CompiledScope = interface end

    [<AllowNullLiteral>]
    type CompiledMode = interface end

    [<AllowNullLiteral>]
    type ModeDetails =
        abstract ``begin``: U3<RegExp, string, ResizeArray<U2<RegExp, string>>> option with get, set
        abstract ``match``: U3<RegExp, string, ResizeArray<U2<RegExp, string>>> option with get, set
        abstract ``end``: U3<RegExp, string, ResizeArray<U2<RegExp, string>>> option with get, set

        [<Obsolete("Deprecated in favor of `scope`")>]
        abstract className: string option with get, set

        abstract scope: U2<string, Record<float, string>> option with get, set
        abstract beginScope: U2<string, Record<float, string>> option with get, set
        abstract endScope: U2<string, Record<float, string>> option with get, set
        abstract contains: ResizeArray<U2<Mode, string>> option with get, set
        abstract endsParent: bool option with get, set
        abstract endsWithParent: bool option with get, set
        abstract endSameAsBegin: bool option with get, set
        abstract skip: bool option with get, set
        abstract excludeBegin: bool option with get, set
        abstract excludeEnd: bool option with get, set
        abstract returnBegin: bool option with get, set
        abstract returnEnd: bool option with get, set
        abstract __beforeBegin: Function option with get, set
        abstract parent: Mode option with get, set
        abstract starts: Mode option with get, set
        abstract lexemes: U2<string, RegExp> option with get, set
        abstract keywords: U3<string, ResizeArray<string>, Record<string, U2<string, ResizeArray<string>>>> option with get, set
        abstract beginKeywords: string option with get, set
        abstract relevance: float option with get, set
        abstract illegal: U3<string, RegExp, Array<U2<string, RegExp>>> option with get, set
        abstract variants: ResizeArray<Mode> option with get, set
        abstract cachedVariants: ResizeArray<Mode> option with get, set
        abstract subLanguage: U2<string, ResizeArray<string>> option with get, set
        abstract isCompiled: bool option with get, set
        abstract label: string option with get, set


    [<AllowNullLiteral>]
    type PublicApiRegex =
        abstract concat: [<ParamArray>] args: U2<RegExp, string>[] -> string
        abstract lookahead: re: U2<RegExp, string> -> string
        abstract either: [<ParamArray>] args: U2<ResizeArray<U2<RegExp, string>>, obj * RegexEitherOptions> -> string
        abstract optional: re: U2<RegExp, string> -> string
        abstract anyNumberOfTimes: re: U2<RegExp, string> -> string

    [<AllowNullLiteral>]
    type ModesAPICOMMENT = interface end

module Highlight_js_lib_languages_ =
    type LanguageFn = HighlightJS.LanguageFn

    [<AllowNullLiteral>]
    type IExports =
        abstract defineLanguage: LanguageFn

module Languages =
    open HighlightJS

    [<ImportDefault("highlight.js/lib/languages/1c")>]
    let _1c: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/abnf")>]
    let abnf: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/accesslog")>]
    let accesslog: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/actionscript")>]
    let actionscript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/ada")>]
    let ada: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/angelscript")>]
    let angelscript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/apache")>]
    let apache: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/applescript")>]
    let applescript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/arcade")>]
    let arcade: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/arduino")>]
    let arduino: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/armasm")>]
    let armasm: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/asciidoc")>]
    let asciidoc: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/aspectj")>]
    let aspectj: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/autohotkey")>]
    let autohotkey: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/autoit")>]
    let autoit: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/avrasm")>]
    let avrasm: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/awk")>]
    let awk: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/axapta")>]
    let axapta: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/bash")>]
    let bash: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/basic")>]
    let basic: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/bnf")>]
    let bnf: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/brainfuck")>]
    let brainfuck: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/c")>]
    let c: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/cal")>]
    let cal: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/capnproto")>]
    let capnproto: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/ceylon")>]
    let ceylon: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/clean")>]
    let clean: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/clojure-repl")>]
    let clojure_repl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/clojure")>]
    let clojure: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/cmake")>]
    let cmake: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/coffeescript")>]
    let coffeescript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/coq")>]
    let coq: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/cos")>]
    let cos: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/cpp")>]
    let cpp: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/crmsh")>]
    let crmsh: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/crystal")>]
    let crystal: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/csharp")>]
    let csharp: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/csp")>]
    let csp: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/css")>]
    let css: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/d")>]
    let d: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/dart")>]
    let dart: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/delphi")>]
    let delphi: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/diff")>]
    let diff: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/django")>]
    let django: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/dns")>]
    let dns: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/dockerfile")>]
    let dockerfile: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/dos")>]
    let dos: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/dsconfig")>]
    let dsconfig: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/dts")>]
    let dts: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/dust")>]
    let dust: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/ebnf")>]
    let ebnf: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/elixir")>]
    let elixir: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/elm")>]
    let elm: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/erb")>]
    let erb: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/erlang-repl")>]
    let erlang_repl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/erlang")>]
    let erlang: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/excel")>]
    let excel: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/fix")>]
    let fix: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/flix")>]
    let flix: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/fortran")>]
    let fortran: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/fsharp")>]
    let fsharp: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/gams")>]
    let gams: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/gauss")>]
    let gauss: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/gcode")>]
    let gcode: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/gherkin")>]
    let gherkin: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/glsl")>]
    let glsl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/gml")>]
    let gml: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/go")>]
    let go: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/golo")>]
    let golo: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/gradle")>]
    let gradle: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/graphql")>]
    let graphql: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/groovy")>]
    let groovy: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/haml")>]
    let haml: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/handlebars")>]
    let handlebars: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/haskell")>]
    let haskell: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/haxe")>]
    let haxe: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/hsp")>]
    let hsp: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/http")>]
    let http: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/hy")>]
    let hy: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/inform7")>]
    let inform7: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/ini")>]
    let ini: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/irpf90")>]
    let irpf90: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/isbl")>]
    let isbl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/java")>]
    let java: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/javascript")>]
    let javascript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/jboss-cli")>]
    let jboss_cli: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/json")>]
    let json: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/julia-repl")>]
    let julia_repl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/julia")>]
    let julia: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/kotlin")>]
    let kotlin: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/lasso")>]
    let lasso: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/latex")>]
    let latex: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/ldif")>]
    let ldif: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/leaf")>]
    let leaf: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/less")>]
    let less: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/lisp")>]
    let lisp: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/livecodeserver")>]
    let livecodeserver: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/livescript")>]
    let livescript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/llvm")>]
    let llvm: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/lsl")>]
    let _lsl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/lua")>]
    let lua: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/makefile")>]
    let makefile: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/markdown")>]
    let markdown: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/mathematica")>]
    let mathematica: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/matlab")>]
    let matlab: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/maxima")>]
    let maxima: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/mel")>]
    let mel: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/mercury")>]
    let mercury: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/mipsasm")>]
    let mipsasm: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/mizar")>]
    let mizar: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/mojolicious")>]
    let mojolicious: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/monkey")>]
    let monkey: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/moonscript")>]
    let moonscript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/n1ql")>]
    let n1ql: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/nestedtext")>]
    let nestedtext: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/nginx")>]
    let nginx: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/nim")>]
    let nim: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/nix")>]
    let nix: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/node-repl")>]
    let node_repl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/nsis")>]
    let nsis: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/objectivec")>]
    let objectivec: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/ocaml")>]
    let ocaml: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/openscad")>]
    let openscad: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/oxygene")>]
    let oxygene: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/parser3")>]
    let parser3: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/perl")>]
    let perl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/pf")>]
    let pf: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/pgsql")>]
    let pgsql: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/php-template")>]
    let php_template: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/php")>]
    let php: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/plaintext")>]
    let plaintext: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/pony")>]
    let pony: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/powershell")>]
    let powershell: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/processing")>]
    let processing: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/profile")>]
    let profile: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/prolog")>]
    let prolog: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/properties")>]
    let properties: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/protobuf")>]
    let protobuf: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/puppet")>]
    let puppet: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/purebasic")>]
    let purebasic: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/python-repl")>]
    let python_repl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/python")>]
    let python: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/q")>]
    let q: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/qml")>]
    let qml: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/r")>]
    let r: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/reasonml")>]
    let reasonml: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/rib")>]
    let rib: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/roboconf")>]
    let roboconf: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/routeros")>]
    let routeros: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/rsl")>]
    let rsl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/ruby")>]
    let ruby: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/ruleslanguage")>]
    let ruleslanguage: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/rust")>]
    let rust: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/sas")>]
    let sas: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/scala")>]
    let scala: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/scheme")>]
    let scheme: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/scilab")>]
    let scilab: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/scss")>]
    let scss: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/shell")>]
    let shell: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/smali")>]
    let smali: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/smalltalk")>]
    let smalltalk: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/sml")>]
    let sml: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/sqf")>]
    let sqf: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/sql")>]
    let sql: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/stan")>]
    let stan: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/stata")>]
    let stata: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/step21")>]
    let step21: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/stylus")>]
    let stylus: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/subunit")>]
    let subunit: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/swift")>]
    let swift: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/taggerscript")>]
    let taggerscript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/tap")>]
    let tap: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/tcl")>]
    let tcl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/thrift")>]
    let thrift: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/tp")>]
    let tp: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/twig")>]
    let twig: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/typescript")>]
    let typescript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/vala")>]
    let vala: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/vbnet")>]
    let vbnet: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/vbscript-html")>]
    let vbscript_html: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/vbscript")>]
    let vbscript: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/verilog")>]
    let verilog: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/vhdl")>]
    let vhdl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/vim")>]
    let vim: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/wasm")>]
    let wasm: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/wren")>]
    let wren: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/x86asm")>]
    let x86asm: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/xl")>]
    let xl: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/xml")>]
    let xml: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/xquery")>]
    let xquery: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/yaml")>]
    let yaml: LanguageFn = jsNative

    [<ImportDefault("highlight.js/lib/languages/zephir")>]
    let zephir: LanguageFn = jsNative


/// Usage `do importSideEffects Styles.Default`
[<RequireQualifiedAccess>]
module Styles =
    [<Literal>]
    let _1c_light = "highlight.js/styles/1c-light.css"

    [<Literal>]
    let a11y_dark = "highlight.js/styles/a11y-dark.css"

    [<Literal>]
    let a11y_light = "highlight.js/styles/a11y-light.css"

    [<Literal>]
    let agate = "highlight.js/styles/agate.css"

    [<Literal>]
    let an_old_hope = "highlight.js/styles/an-old-hope.css"

    [<Literal>]
    let androidstudio = "highlight.js/styles/androidstudio.css"

    [<Literal>]
    let arduino_light = "highlight.js/styles/arduino-light.css"

    [<Literal>]
    let arta = "highlight.js/styles/arta.css"

    [<Literal>]
    let ascetic = "highlight.js/styles/ascetic.css"

    [<Literal>]
    let atom_one_dark_reasonable = "highlight.js/styles/atom-one-dark-reasonable.css"

    [<Literal>]
    let atom_one_dark = "highlight.js/styles/atom-one-dark.css"

    [<Literal>]
    let atom_one_light = "highlight.js/styles/atom-one-light.css"

    [<Literal>]
    let brown_paper = "highlight.js/styles/brown-paper.css"

    [<Literal>]
    let codepen_embed = "highlight.js/styles/codepen-embed.css"

    [<Literal>]
    let color_brewer = "highlight.js/styles/color-brewer.css"

    [<Literal>]
    let cybertopia_cherry = "highlight.js/styles/cybertopia-cherry.css"

    [<Literal>]
    let cybertopia_dimmer = "highlight.js/styles/cybertopia-dimmer.css"

    [<Literal>]
    let cybertopia_icecap = "highlight.js/styles/cybertopia-icecap.css"

    [<Literal>]
    let cybertopia_saturated = "highlight.js/styles/cybertopia-saturated.css"

    [<Literal>]
    let dark = "highlight.js/styles/dark.css"

    [<Literal>]
    let _default = "highlight.js/styles/default.css"

    [<Literal>]
    let devibeans = "highlight.js/styles/devibeans.css"

    [<Literal>]
    let docco = "highlight.js/styles/docco.css"

    [<Literal>]
    let far = "highlight.js/styles/far.css"

    [<Literal>]
    let felipec = "highlight.js/styles/felipec.css"

    [<Literal>]
    let foundation = "highlight.js/styles/foundation.css"

    [<Literal>]
    let github_dark_dimmed = "highlight.js/styles/github-dark-dimmed.css"

    [<Literal>]
    let github_dark = "highlight.js/styles/github-dark.css"

    [<Literal>]
    let github = "highlight.js/styles/github.css"

    [<Literal>]
    let gml = "highlight.js/styles/gml.css"

    [<Literal>]
    let googlecode = "highlight.js/styles/googlecode.css"

    [<Literal>]
    let gradient_dark = "highlight.js/styles/gradient-dark.css"

    [<Literal>]
    let gradient_light = "highlight.js/styles/gradient-light.css"

    [<Literal>]
    let grayscale = "highlight.js/styles/grayscale.css"

    [<Literal>]
    let hybrid = "highlight.js/styles/hybrid.css"

    [<Literal>]
    let idea = "highlight.js/styles/idea.css"

    [<Literal>]
    let intellij_light = "highlight.js/styles/intellij-light.css"

    [<Literal>]
    let ir_black = "highlight.js/styles/ir-black.css"

    [<Literal>]
    let isbl_editor_dark = "highlight.js/styles/isbl-editor-dark.css"

    [<Literal>]
    let isbl_editor_light = "highlight.js/styles/isbl-editor-light.css"

    [<Literal>]
    let kimbie_dark = "highlight.js/styles/kimbie-dark.css"

    [<Literal>]
    let kimbie_light = "highlight.js/styles/kimbie-light.css"

    [<Literal>]
    let lightfair = "highlight.js/styles/lightfair.css"

    [<Literal>]
    let lioshi = "highlight.js/styles/lioshi.css"

    [<Literal>]
    let magula = "highlight.js/styles/magula.css"

    [<Literal>]
    let mono_blue = "highlight.js/styles/mono-blue.css"

    [<Literal>]
    let monokai_sublime = "highlight.js/styles/monokai-sublime.css"

    [<Literal>]
    let monokai = "highlight.js/styles/monokai.css"

    [<Literal>]
    let night_owl = "highlight.js/styles/night-owl.css"

    [<Literal>]
    let nnfx_dark = "highlight.js/styles/nnfx-dark.css"

    [<Literal>]
    let nnfx_light = "highlight.js/styles/nnfx-light.css"

    [<Literal>]
    let nord = "highlight.js/styles/nord.css"

    [<Literal>]
    let obsidian = "highlight.js/styles/obsidian.css"

    [<Literal>]
    let panda_syntax_dark = "highlight.js/styles/panda-syntax-dark.css"

    [<Literal>]
    let panda_syntax_light = "highlight.js/styles/panda-syntax-light.css"

    [<Literal>]
    let paraiso_dark = "highlight.js/styles/paraiso-dark.css"

    [<Literal>]
    let paraiso_light = "highlight.js/styles/paraiso-light.css"

    [<Literal>]
    let pojoaque = "highlight.js/styles/pojoaque.css"

    [<Literal>]
    let purebasic = "highlight.js/styles/purebasic.css"

    [<Literal>]
    let qtcreator_dark = "highlight.js/styles/qtcreator-dark.css"

    [<Literal>]
    let qtcreator_light = "highlight.js/styles/qtcreator-light.css"

    [<Literal>]
    let rainbow = "highlight.js/styles/rainbow.css"

    [<Literal>]
    let rose_pine_dawn = "highlight.js/styles/rose-pine-dawn.css"

    [<Literal>]
    let rose_pine_moon = "highlight.js/styles/rose-pine-moon.css"

    [<Literal>]
    let rose_pine = "highlight.js/styles/rose-pine.css"

    [<Literal>]
    let routeros = "highlight.js/styles/routeros.css"

    [<Literal>]
    let school_book = "highlight.js/styles/school-book.css"

    [<Literal>]
    let shades_of_purple = "highlight.js/styles/shades-of-purple.css"

    [<Literal>]
    let srcery = "highlight.js/styles/srcery.css"

    [<Literal>]
    let stackoverflow_dark = "highlight.js/styles/stackoverflow-dark.css"

    [<Literal>]
    let stackoverflow_light = "highlight.js/styles/stackoverflow-light.css"

    [<Literal>]
    let sunburst = "highlight.js/styles/sunburst.css"

    [<Literal>]
    let tokyo_night_dark = "highlight.js/styles/tokyo-night-dark.css"

    [<Literal>]
    let tokyo_night_light = "highlight.js/styles/tokyo-night-light.css"

    [<Literal>]
    let tomorrow_night_blue = "highlight.js/styles/tomorrow-night-blue.css"

    [<Literal>]
    let tomorrow_night_bright = "highlight.js/styles/tomorrow-night-bright.css"

    [<Literal>]
    let vs = "highlight.js/styles/vs.css"

    [<Literal>]
    let vs2015 = "highlight.js/styles/vs2015.css"

    [<Literal>]
    let xcode = "highlight.js/styles/xcode.css"

    [<Literal>]
    let xt256 = "highlight.js/styles/xt256.css"
