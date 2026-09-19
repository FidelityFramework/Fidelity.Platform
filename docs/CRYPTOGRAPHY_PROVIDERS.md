# Cryptography providers

[Fidelity.Cryptography](../../Fidelity.Cryptography/README.md) is the peer repository for first-party Clef algorithms and cryptographic operation contracts. Platform owns the hardware and environment capabilities used by its provider adapters.

This is a design boundary. No crypto provider export or hardware acceptance is introduced by this document.

## Ownership

Silicon declarations identify exact crypto engines or ISA features. Product declarations establish the fitted part and accessible resources. Environment declarations establish privilege and available host services. A workload profile selects the operations, custody policy and resource grants its application needs.

Cryptography adapters depend on the relevant Platform package and cryptographic contract. The portable algorithm core remains independent of a board. Platform declarations remain independent of application credential or certificate policy.

## Required capabilities

The [provider contract](../../Fidelity.Cryptography/docs/platform-providers.md) specifies the record each implementation needs. It includes algorithm/mode coverage, key forms and memory constraints. Native providers additionally identify clocks, reset, MMIO, DMA visibility and completion. Vendor bindings pin their actual ABI and runtime dependencies.

AES support alone does not establish AES-256-GCM support. Record block-cipher acceleration and authentication support separately. HUK custody, derivation, random generation and public-key acceleration also have separate capabilities.

Ariel supplies framework scheduling. The device driver retains responsibility for request completion and buffer release, including timeout, cancellation and reset. Reuse the [accelerator handoff contracts](ADMISSION_AND_SIDECARS.md) where their memory and ownership semantics apply.

## Target admission

Begin with the exact EK-RA6M5 SCE/key-custody path needed by the credential store. Compare the vendor binding and prospective native driver through the same Cryptography contract. Declare hardware premises and retain evidence for each implementation separately.

Other products require their own evidence. Sweet Potato graphics or video support does not establish crypto-engine access. A sibling MCU's AES block does not establish the fitted part's capabilities. A hosted service and a freestanding driver may expose different operations on the same silicon.

Application policy can prefer acceleration among admitted providers. A fallback must preserve algorithm, encoding and key custody. Provider unavailability must not silently export a protected key or choose a weaker suite.

MBS persistence requirements are owned by the [sealing contract](../../Fidelity.Cryptography/docs/mbs-sealing.md). WireGuard and FIDO profiles use the algorithms required by their protocols, independently of the AES-256 storage policy.
