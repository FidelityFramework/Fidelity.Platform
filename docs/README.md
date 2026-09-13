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
| [ESP32-S3 bring-up](ESP32S3_BRINGUP.md) | Xtensa toolchain gap, ROM-loaded SRAM image format, LX7 vector obligations, Composer backend deltas and the LVGL/Farscape boundary | Planning only; no image built and no declaration CCS-checked |
| [STM32H7 synthesizer design](STM32H7_SYNTH_DESIGN.md) | Source/package audit, native DSP and touch UI, BAREWire memory/ownership extensions, evidence boundaries and acceptance ladder | Static banner accepted on hardware and after CN2 reconnect; GPIO/display integration and synth implementation pending |
| [STM32H7 driver roadmap](STM32H7_DRIVER_ROADMAP.md) | Board routes, proposed driver boundaries, shared ownership and the post-HelloDISCO audio cutline | Scope/contract outlines; audio, touch, storage and SDRAM drivers remain unimplemented |
| [UI and display model across targets](DISPLAY_MODEL.md) | Fabulous/Partas-inspired composition, state and rendering boundaries; comparisons of HelloESP, HelloDISCO, HelloWayland and WrenHello | Grounded design exploration; native display first, optional binding paths retained |

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
