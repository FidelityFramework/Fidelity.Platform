# Admission reference tests

Run `dotnet run --project tests/Admission/Admission.Tests.fsproj` from the
Fidelity.Platform root. This is an F# executable test runner. MSBuild copies
unchanged authored `.clef` files to ignored `.fs` filenames in `obj/` for the
managed compiler; no separate implementation or shell/Python test script exists.
The runner also asks CCS to check the original reference `.fidproj` closures.

Tests use synthetic facts, digests and evidence. They establish report
consistency behavior and source validity, not proof authentication, kernel
admission, native code generation or hardware operation. See the
[implementation boundary](../../docs/ADMISSION_AND_SIDECARS.md).
