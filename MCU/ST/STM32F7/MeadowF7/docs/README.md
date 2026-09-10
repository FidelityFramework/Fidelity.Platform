# Meadow F7 Source Pack

This source pack contains STM32F750 and ESP32 datasheets plus a saved Meadow
F7v2 vendor web page. [SOURCE_MANIFEST.md](SOURCE_MANIFEST.md) identifies the
actual files and hashes. Their presence does not establish that the board
variant, complete pin routing or boot contract has been reconciled.

The [platform source](../Platform.clef) remains a scaffold with empty endpoint
collections and `Core = None`; it also omits the required Contracts `Resets`
field. No current bare-metal application acceptance is recorded for this leaf.

## Inputs still needed for implementation

- Board revision/package confirmation and a complete hardware guide
- MCU register reference manual (the staged datasheet is not a substitute)
- Board schematic (PDF)
- Pinout/alternate-function tables (PDF/CSV/XLSX)
- Vendor app notes used to define timing/electrical constraints (optional)

## Folder Notes

- Keep source artifacts directly under `docs/` by default.
- Optional subfolders are allowed, but not required.
- Update `SOURCE_MANIFEST.md` when adding, replacing, or re-versioning artifacts.
