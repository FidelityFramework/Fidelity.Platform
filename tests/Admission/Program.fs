module AdmissionReferenceTests

open System
open System.IO
open Fidelity.Platform.Contracts.Admission
open Fidelity.Platform.Profiles.BPF_Reference
open Fidelity.Platform.Profiles.AppleSilicon_Metal_FPGA_Reference
open Clef.Compiler.Project

let require condition message = if not condition then failwith message
let tests = ResizeArray<string * (unit -> unit)>()
let test name body = tests.Add(name, body)
let has standing subject findings =
    findings |> Array.exists (fun (item: Finding) -> item.Standing = standing && item.Subject = subject)

// Deliberately synthetic data: none of these labels is deployment evidence.
let limit: Budget = {
    Metric = "stack"; Unit = "bytes"; Scope = "combined-call-chain"
    Maximum = Some 512; Source = "synthetic fixture policy"
}
let profile: AdmissionProfile = {
    Profile.linux with
        Host = { Profile.linux.Host with Budgets = [| limit |] }
        Facts = Profile.linux.Host.RequiredFacts |> Array.map (fun name ->
            { Name = name; Value = Some "fixture"; Evidence = "fixture-only" })
        Digest = Some "fixture-profile-digest"
}
let resource: ResourceEvidence = {
    Metric = "stack"; Unit = "bytes"; Scope = "combined-call-chain"
    Value = 512; Standing = "checked-upper-bound"; Evidence = "fixture-analysis"
}
let report: AnalysisReport = {
    ProfileDigest = "fixture-profile-digest"; ArtifactDigest = "fixture-artifact-digest"
    Machine = "ebpf"; Features = [| "packet-read" |]; Resources = [| resource |]
    Obligations = profile.Host.Obligations |> Array.map (fun name ->
        { Name = name; Standing = "discharged"; Evidence = "fixture-discharge" })
}
let run selected result = Check.run selected "fixture-artifact-digest" result
let sidecar = Fidelity.Platform.Contracts.Sidecar.Check.run

do
    test "exact-budget-boundary" (fun () -> require (run profile report |> Array.isEmpty) "Consistent fixture rejected")
    test "budget-overflow" (fun () ->
        require (run profile { report with Resources = [| { resource with Value = 513 } |] } |> has "violation" "stack") "Over-budget stack accepted")
    test "missing-budget-evidence" (fun () ->
        require (run profile { report with Resources = [||] } |> has "pending" "stack") "Missing evidence became zero")
    test "scope-does-not-collapse" (fun () ->
        require (run profile { report with Resources = [| { resource with Scope = "one-frame" } |] } |> has "pending" "stack") "Per-frame bound discharged call-chain budget")
    test "instructions-are-not-time" (fun () ->
        let selected = { profile with Host = { profile.Host with Budgets = [| { limit with Metric = "execution-work"; Unit = "nanoseconds"; Scope = "one-invocation" } |] } }
        let measured = { resource with Metric = "execution-work"; Unit = "instructions"; Scope = "one-invocation" }
        require (run selected { report with Resources = [| measured |] } |> has "pending" "execution-work") "Instruction count discharged a deadline")
    for standing in [ "estimate"; "measurement" ] do
        test (standing + "-cannot-discharge") (fun () ->
            let findings = run profile { report with Resources = [| { resource with Standing = standing; Value = 1 } |] }
            require (has "pending" "stack" findings && has "advisory" "stack" findings) "Advisory became a hard bound")
    test "unknown-limit" (fun () ->
        let selected = { profile with Host = { profile.Host with Budgets = [| { limit with Maximum = None } |] } }
        require (run selected report |> has "pending" "stack") "Unknown budget accepted")
    test "negative-evidence" (fun () ->
        require (run profile { report with Resources = [| { resource with Value = -1 } |] } |> has "violation" "stack") "Negative evidence accepted")
    test "duplicate-resource-evidence" (fun () ->
        require (run profile { report with Resources = [| resource; resource |] } |> has "violation" "stack") "Conflicting results silently selected")
    test "stale-profile" (fun () ->
        require (run profile { report with ProfileDigest = "old-profile" } |> has "violation" profile.Id) "Configuration drift reused evidence")
    test "changed-final-artifact" (fun () ->
        require (run profile { report with ArtifactDigest = "before-lowering" } |> has "violation" "artifact") "Lowered artifact reused earlier evidence")
    test "missing-host-fact" (fun () ->
        require (run { profile with Facts = [||] } report |> Array.exists (fun f -> f.Standing = "pending")) "Unpinned host accepted")
    test "duplicate-host-fact" (fun () ->
        require (run { profile with Facts = Array.append profile.Facts [| profile.Facts.[0] |] } report |> has "violation" "host facts") "Duplicate host identity accepted")
    test "missing-obligation" (fun () ->
        require (run profile { report with Obligations = [||] } |> has "pending" profile.Host.Obligations.[0]) "Empty proof set accepted")
    test "refuted-obligation" (fun () ->
        let evidence = Array.copy report.Obligations
        evidence.[0] <- { evidence.[0] with Standing = "refuted" }
        require (run profile { report with Obligations = evidence } |> has "violation" evidence.[0].Name) "Refutation ignored")
    test "macos-is-not-ebpf" (fun () ->
        require (run Profile.macos report |> has "violation" "machine") "eBPF admitted into cBPF contract")
    test "macos-has-no-xdp-verdict" (fun () ->
        require (run Profile.macos { report with Machine = "cbpf"; Features = [| "xdp-verdict" |] } |> has "violation" "xdp-verdict") "XDP semantics inherited by capture filter")
    test "reference-profiles-stay-unresolved" (fun () ->
        for selected in [ Profile.linux; Profile.windows; Profile.macos ] do
            require (run selected report |> has "pending" selected.Id) "Reference catalogue claimed pinned evidence")
    test "uma-does-not-prove-aliasing" (fun () ->
        require (sidecar Handoffs.cpuToGpu |> Array.contains "Aliasing or DMA mapping evidence is unresolved.") "UMA invented a mapping")
    test "ethernet-does-not-share-backing" (fun () ->
        require (sidecar { Handoffs.hostToFpga with Mechanism = "shared-backing"; MappingEvidence = Some "fixture" }
            |> Array.contains "Shared backing requires a shared-memory transport.") "Ethernet became coherent RAM")
    test "wire-layout-mismatch" (fun () ->
        let changed = { Handoffs.hostToFpga with Destination = { Handoffs.payload with Schema = "different-layout" } }
        require (sidecar changed |> Array.contains "BAREWire payload schema identities must agree.") "Different schemas accepted")
    test "buffer-capacity-and-release" (fun () ->
        let changed = { Handoffs.hostToFpga with Destination = { Handoffs.payload with Capacity = 100L }; ConsumerRelease = "" }
        require (sidecar changed |> Array.length = 2) "Capacity or release requirement lost")
    test "dma-needs-mapping" (fun () ->
        require (sidecar { Handoffs.hostToFpga with Mechanism = "dma" } |> Array.contains "Aliasing or DMA mapping evidence is unresolved.") "DMA mapping assumed")
    test "strix-uma-needs-mapping-too" (fun () ->
        let strix = Fidelity.Platform.Profiles.StrixHalo_ArtyLab.Handoffs.cpuToGpu
        require (sidecar strix |> Array.contains "Aliasing or DMA mapping evidence is unresolved.") "Strix UMA invented a GPU mapping")
    test "strix-and-metal-share-contract-not-bindings" (fun () ->
        let strix = Fidelity.Platform.Profiles.StrixHalo_ArtyLab.Handoffs.cpuToGpu
        require (strix.Transport.Kind = Handoffs.cpuToGpu.Transport.Kind) "Shared handoff lost common transport"
        require (strix.ConsumerRelease <> Handoffs.cpuToGpu.ConsumerRelease) "Vendor completion obligations collapsed")
    test "ccs-checks-original-clef-packages" (fun () ->
        let root = Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, "../.."))
        for relative in [ "Profiles/BPF_Reference/Fidelity.Platform.fidproj"; "Profiles/AppleSilicon_Metal_FPGA_Reference/Fidelity.Platform.fidproj"; "Profiles/StrixHalo_ArtyLab/Fidelity.Platform.Handoffs.fidproj" ] do
            let path = Path.Combine(root, relative)
            match ProjectChecker.checkProject path with
            | Error message -> failwith message
            | Ok project ->
                require (not (ProjectChecker.hasErrors project)) (String.concat "\n" (ProjectChecker.getErrorMessages project)))

[<EntryPoint>]
let main _ =
    let mutable failures = 0
    for name, body in tests do
        try body (); printfn "PASS %s" name
        with error -> failures <- failures + 1; eprintfn "FAIL %s: %s" name error.Message
    printfn "%d/%d reference checks passed" (tests.Count - failures) tests.Count
    if failures = 0 then 0 else 1
