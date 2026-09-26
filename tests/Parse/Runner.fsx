// The driver uses .NET only for orchestration. Parse.clef executes as native Clef.
// Pass a previously built private compiler; this script never builds Composer.
#load "../../../Composer/tests/regression/RunnerCore.fsx"
open System
open System.IO
open System.Security.Cryptography
open System.Text.Json
open RunnerCore

let arguments = fsi.CommandLineArgs |> Array.skip 1 |> Array.filter ((<>) "--")
if arguments.Length <> 1 then
    failwith "Usage: dotnet fsi tests/Parse/Runner.fsx -- <private Composer executable>"
let compiler = Path.GetFullPath arguments[0]
if not (File.Exists compiler) then failwithf "Missing compiler: %s" compiler
let platform = Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, "../.."))
let work = Path.Combine(Path.GetTempPath(), "platform-parse-" + Guid.NewGuid().ToString("N"))
Directory.CreateDirectory work |> ignore
printfn "Native parser evidence: %s" work
let source = Path.Combine(platform, "Environments/Linux/x86_64/Parse.clef")
let copiedSource = Path.Combine(work, "Parse.clef")
File.Copy(source, copiedSource)
File.Copy(Path.Combine(__SOURCE_DIRECTORY__, "Values.clef"), Path.Combine(work, "Values.clef"))
let expected = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "ExpectedOutput.txt"))
File.WriteAllText(Path.Combine(work, "expected.stdout"), expected)
// Invalid inputs must demand the parsed value; no ignored ordinary argument.
let cases =
    [ "Values", None
      "BelowDigit", Some "Parse.int \"/\" = 0"
      "AboveDigitFraction", Some "Parse.float \"1.:2\" = 0.0"
      "NonAsciiDigit", Some "Parse.int \"٩\" = 0" ]
let surface = Path.Combine(platform, "Environments/Linux/x86_64/Fidelity.Platform.CompilerSurface.fidproj")
for name, invalid in cases do
    match invalid with
    | Some condition ->
        File.WriteAllText(Path.Combine(work, name + ".clef"),
            "module " + name + "\n[<EntryPoint>]\nlet main _ =\n    if " + condition +
            " then Console.writeln \"unexpected zero\" else Console.writeln \"unexpected nonzero\"\n    0\n")
    | None -> ()
    File.WriteAllText(Path.Combine(work, name + ".fidproj"),
        "[package]\nname = \"Parse" + name + "\"\nversion = \"0.1.0\"\n" +
        "[compilation]\ntarget = \"cpu\"\n[dependencies]\nplatform = { path = " +
        JsonSerializer.Serialize(surface) + " }\n[build]\nsources = [\"Parse.clef\", \"" + name +
        ".clef\"]\noutput = \"parse-test\"\noutput_kind = \"console\"\n")
let hashes =
    [ source; compiler; Path.Combine(Path.GetDirectoryName compiler, "Composer.dll")
      Path.Combine(Path.GetDirectoryName compiler, "Clef.Compiler.Service.dll")
      Path.Combine(__SOURCE_DIRECTORY__, "Values.clef"); Path.Combine(__SOURCE_DIRECTORY__, "ExpectedOutput.txt") ]
    |> List.map(fun path -> {| Path=path; Sha256=File.ReadAllBytes path |> SHA256.HashData |> Convert.ToHexString |})
File.WriteAllText(Path.Combine(work,"inputs.json"), JsonSerializer.Serialize(hashes, JsonSerializerOptions(WriteIndented=true)))
let matchLine = File.ReadAllLines source |> Array.findIndex(fun line -> line.Trim() = "match c with") |> (+) 1
let diagnostic = sprintf "Pattern match failed at %s:%d:10\n" copiedSource matchLine
let config = { SamplesRoot=work; CompilerPath=compiler; DefaultTimeoutSeconds=180; PruneIntermediates=true }
let observations = ResizeArray<_>()
for index, (name, invalid) in cases |> List.indexed do
    let evidence = Path.Combine(work, sprintf "%04d" (index+1))
    Directory.CreateDirectory evidence |> ignore
    let sample =
        { Name="."; ProjectFile=name + ".fidproj"; BinaryName="parse-test"; StdinFile=None
          ExpectedOutput=""; TimeoutSeconds=180; Skip=false; SkipReason=None }
    let _, compile, binary = (compileSamplePhaseAsync config (Some evidence) sample).GetAwaiter().GetResult()
    let mutable passed = false
    match compile, binary with
    | CompileSuccess _, Some executable ->
        let outcome, elapsed = runProcess executable [] work None 10000
        match outcome with
        | Completed(code, stdout, stderr) ->
            File.WriteAllText(Path.Combine(evidence,"run.stdout.log"), stdout)
            File.WriteAllText(Path.Combine(evidence,"run.stderr.log"), stderr)
            passed <-
                if invalid.IsSome then code = 1 && stdout = "" && stderr = diagnostic
                else code = 0 && stdout = expected && stderr = ""
            File.WriteAllText(Path.Combine(evidence,"run.status"), sprintf "exit=%d elapsed_ms=%d passed=%b\n" code elapsed passed)
        | _ -> File.WriteAllText(Path.Combine(evidence,"run.status"), sprintf "%A\n" outcome)
    | _ -> ()
    observations.Add {| Case=name; Passed=passed; Compile=sprintf "%A" compile |}
    printfn "%s %s" (if passed then "PASS" else "FAIL") name
File.WriteAllText(Path.Combine(work,"results.json"), JsonSerializer.Serialize(observations, JsonSerializerOptions(WriteIndented=true)))
if observations |> Seq.exists(fun result -> not result.Passed) then failwithf "Parser native check failed; see %s" work
printfn "All %d native parser checks passed" observations.Count
