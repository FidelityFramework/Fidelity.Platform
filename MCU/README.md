# MCU Substrate

This subtree contains microcontroller platform packages.

## Layout

- `MCU/ST/<Family>/<Board>/`
- `MCU/Renesas/<Family>/<Board>/`

Each board leaf should include:

- `Fidelity.Platform.fidproj`
- Typed declaration sources selected explicitly by the manifest (`.clef` or `.fs`)
- `docs/` source pack with authoritative vendor artifacts
- `docs/SOURCE_MANIFEST.md` mapping each exposed endpoint category to source files

## Docs Layout Policy

Use a flat `docs/` layout by default.

- Do not require `docs/board`, `docs/vendor`, or `docs/pinouts` subfolders.
- Subfolders are optional and only for convenience when a specific board bundle benefits from them.

## Rule

Do not guess pin mappings. Add a mapping only with a cited source and variant.
Sources may be staged here or pinned by path/hash in a sibling reference pack;
their local availability must be explicit. Distinguish wiring coverage from
implemented register/driver support and physical acceptance.

The [EK-RA6M5](Renesas/RA6M5/EK_RA6M5/docs/README.md) has accepted bare startup,
GPIO/interrupt/SysTick behavior and a complete board wiring inventory. The
[Meadow F7](ST/STM32F7/MeadowF7/docs/README.md) remains a declaration scaffold.
