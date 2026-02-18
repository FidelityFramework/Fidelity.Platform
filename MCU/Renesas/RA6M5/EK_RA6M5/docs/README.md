# EK-RA6M5 Source Pack

Store authoritative source artifacts for this board in this folder.

## Expected Inputs

- Board user manual / hardware guide (PDF)
- MCU hardware manual and datasheet (PDF)
- Board schematic (PDF)
- Pinout/port-function tables (PDF/CSV/XLSX)
- Vendor app notes used to define timing/electrical constraints (optional)

## Folder Notes

- Place immutable vendor files under `vendor/`.
- Place board-level reference files under `board/`.
- Place extracted pinout tables under `pinouts/`.
- Update `SOURCE_MANIFEST.md` when adding, replacing, or re-versioning artifacts.
