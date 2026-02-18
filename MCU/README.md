# MCU Substrate

This subtree contains microcontroller platform packages.

## Layout

- `MCU/ST/<Family>/<Board>/`
- `MCU/Renesas/<Family>/<Board>/`

Each board leaf should include:

- `Fidelity.Platform.fidproj`
- `Platform.fs`
- `docs/` source pack with authoritative vendor artifacts
- `docs/SOURCE_MANIFEST.md` mapping each exposed endpoint category to source files

## Docs Layout Policy

Use a flat `docs/` layout by default.

- Do not require `docs/board`, `docs/vendor`, or `docs/pinouts` subfolders.
- Subfolders are optional and only for convenience when a specific board bundle benefits from them.

## Rule

Do not guess pin mappings. Leave endpoint lists empty until board documentation is present in `docs/`.
