# Fidelity.Platform documentation

Start with [platform declarations and compiler integration](CANONICAL_PLATFORM_SPEC.md).
It describes the current declaration readers, numeric narrowing and backend
boundaries. The historical filename is retained for source references.

| Document | Purpose | Status |
| --- | --- | --- |
| [Repository taxonomy](../PLATFORM_STRUCTURE.md) | Silicon, products, virtual machines, environments, protocols and profiles; current paths | Implemented package structure; reserved support is marked |
| [Platform composition](PLATFORM_COMPOSITION.md) | Source ownership, explicit export selection, execution checks and remaining limits | Implemented single-target composition |
| [Documentation audit](DOCUMENTATION_AUDIT.md) | Source anchors, original findings and subsequent disposition | Dated baseline with MMIO/taxonomy follow-up |
| [Platform declarations](CANONICAL_PLATFORM_SPEC.md) | Current contracts and their actual consumers | Implementation reference |
| [Platform facts and derived state](D4b_PLATFORM_CARRIER_LOCATION.md) | Declaration/context/graph responsibilities and deferred decisions | Current baseline plus explicitly open design questions |
| [BAREWire integration](BAREWire_Rebase_Plan.md) | What migrated and the remaining pin/schema work | Migration status and acceptance criteria |
| [MMIO contracts and predicates](MMIO_CONTRACTS.md) | Region inventory, mappings, workload grants, CCS evidence and executable limits | Implemented static MMIO slice |
| [Admission and accelerator handoffs](ADMISSION_AND_SIDECARS.md) | BPF host distinctions, verifier/resource budgets, Strix Halo/Arty and Metal UMA boundaries | Executable reference checks and source packages; compiler/gate integration pending |
| [ESP32-S3 bring-up](ESP32S3_BRINGUP.md) | Xtensa image, vector and hardware bring-up history | §10 records the compiled HelloESP display/LED/button/interrupt workload running; radio remains outside acceptance |
| [STM32H7 synthesizer design](STM32H7_SYNTH_DESIGN.md) | Source/package audit, native DSP and touch UI, BAREWire memory/ownership extensions, evidence boundaries and acceptance ladder | Interactive HelloDISCO and independent cold start accepted; audio, touch and the synth remain future work |
| [STM32H7 driver roadmap](STM32H7_DRIVER_ROADMAP.md) | Board routes and a documentation scaffold for resuming after Clef DSP/cryptography readiness | Bounded HelloDISCO accepted; future APIs and descriptor expansion deferred |
| [UI and display model across targets](DISPLAY_MODEL.md) | Cold functional components, reactive areas and rendering boundaries across MCU, SBC, desktop and WREN hosts | Grounded design exploration; native display first, optional binding paths retained |
| [Sweet Potato native UI port](SWEET_POTATO_UI_PORT.md) | KeyStation boot, Meson HDMI, USB touch, restricted Mali rendering and the hosted Linux reference | Documentation scaffold and source inventory; no board or GPU acceptance |
| [Radio model](RADIO_MODEL.md) | Wi-Fi/Bluetooth/LoRa ownership, reusable BLE commands/events, backend trust boundaries and language-readiness questions | Documentation scaffold; no radio driver or radio hardware acceptance |
| [Cryptography providers](CRYPTOGRAPHY_PROVIDERS.md) | Fidelity.Cryptography ownership, hardware capabilities, custody and provider admission | Documentation scaffold; no crypto provider implementation or acceptance |

Hardware sources and board acceptance belong with the corresponding product;
see the [EK-RA6M5 source pack](../Hardware/Products/Renesas/EK_RA6M5/docs/README.md),
the [CCC 2026 badge hardware reference](../Hardware/Products/CircuitBoardMedics/CCC2026Badge/docs/BADGE_HARDWARE.md)
and [Arty binding](../Hardware/Products/Digilent/ArtyA7_100T/README.md).

The maintained Composer F# runners for this structure are
[PlatformComposition](../../Composer/tests/PlatformComposition) and
[PlatformCatalog](../../Composer/tests/PlatformCatalog). The
[composition record](PLATFORM_COMPOSITION.md#validation-boundary) records completed
regressions separately from hardware acceptance.

## Keeping the documents current

The [operation-specific capability plan](D4b_PLATFORM_CARRIER_LOCATION.md#planned-operation-specific-capabilities--2026-09-20)
tracks Composer M-01, numeric selection, scheduler requirements and target-aware
witnessing. Its acceptance remains planned; the linked implementation tables
retain their existing evidence boundaries.

- Separate implemented behavior, dated test evidence, trusted hardware premises
  and proposed work. A declared fact or successful example is not proof of all
  exported APIs or all platform behavior.
- Link to the owning source and name the consuming function. Avoid reproducing
  whole schemas or relying on unstable line numbers and branch names.
- When a migration lands, replace its old instructions with the resulting state
  and remaining gates. Git retains the original rationale; duplicate active
  specifications should not survive a migration.
- Keep authored guides in Markdown. Vendor HTML/PDF snapshots remain source
  artifacts with provenance, not framework design documents.
- New checks should use the maintained compiler/F# infrastructure. Existing
  Python harnesses outside the MCU path are inventoried in the audit; their
  presence does not make them the template for new work.

Cross-repository relative links assume sibling checkouts under `repos/`. They are
workspace references and will not necessarily resolve in Forgejo's repository
viewer. The audit pins the inspected revisions; no vendor manual or toolchain
is fetched automatically by reading these documents.
