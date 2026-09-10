# Arty A7-100T Platform Binding

This folder contains the canonical FPGA platform binding for the Digilent Arty A7-100T board.

## Design Intent

The intent of this package is:

1. Keep board endpoint identity authoritative and traceable to vendor documentation.
2. Provide a stable raw binding surface for compiler/platform integration.
3. Enable an application-facing Prelude for ergonomic access (HelloArty and later projects).

## Source-of-Truth Documents

Local documentation used to structure this binding:

- `docs/Arty-A7-100-Master.xdc` (primary pin/package constraints)
- `docs/Arty_Master.xdc` (alternate/reference constraints)
- `docs/Arty A7 Reference Manual - Digilent Reference.html` + `_files/` (board behavior and usage semantics)
- `docs/arty_a7_sch.pdf` (schematic cross-check)
- `docs/ds181_Artix_7_Data_Sheet.pdf` (silicon electrical/performance reference)

Practical precedence used here:

1. XDC constraints for pin mappings
2. Board reference manual for semantics and behavior
3. Schematic for connectivity validation
4. Silicon datasheet for electrical/performance context

## Current Binding Scope

[`ArtyA7_100T.Bindings.clef`](ArtyA7_100T.Bindings.clef) currently models:

- 100 MHz system clock (`E3`)
- Green LEDs (`led[0..3]`)
- RGB LED channels (`rgb[0..3].r/g/b`)
- Buttons (`btn[0..3]`)
- Switches (`sw[0..3]`)
- USB-UART (`uart_tx`, `uart_rx`)
- ChipKit digital GPIO (`ck_io*`, `ck_ioa`, `ck_rst`)
- Dedicated reset semantics (`reset_n` as non-user GPIO)

## Deferred Scope (Intentional)

The following XDC sections are intentionally deferred until needed:

- Pmod headers (JA/JB/JC/JD)
- ChipKit analog variants (XADC + digital modes)
- ChipKit SPI and I2C
- Ethernet PHY
- QSPI flash
- Power measurement channels

This keeps focus on HelloArty baseline behavior without over-expanding first-pass bindings.

## HelloArty Lens

For HelloArty ("blinky" with button/switch control), this package already has the required primitives:

- clock
- LED/RGB outputs
- button/switch inputs
- optional UART diagnostics

## Layering Plan: Bindings + Prelude

This package is the raw authoritative layer.

Current layering:

1. Raw binding layer
   - exact endpoint definitions, pin/package mappings, polarity/behavior notes
2. Application Prelude layer
   - ergonomic APIs for color control, blink cadence, switch/button interaction

Current files:

- [ArtyA7_100T.Bindings.clef](ArtyA7_100T.Bindings.clef) (Contracts endpoints and device part)
- [ArtyA7_100T.Prelude.clef](ArtyA7_100T.Prelude.clef) (application-facing conveniences)
- [ArtyA7_100T.Description.clef](ArtyA7_100T.Description.clef) (additive BAREWire memory/buffer/transport description)

The [manifest](Fidelity.Platform.fidproj) includes all three. Application pin
attributes are resolved by CCS into `Codata.Pins`; Composer serializes XDC from
that result. The BAREWire description has only three surface endpoints and does
not yet replace the full pin map. [Migration status](../../../../docs/BAREWire_Rebase_Plan.md)
records the remaining gates, including the unresolved `Prelude.Package`
reference to `Platform.xdcConstraints`. This README is a declaration inventory,
not fresh acceptance of every Prelude export or a new synthesis run.

## Notes

- Keep mappings source-backed; do not infer endpoints.
- Preserve stable naming once published.
- Update this README when new endpoint families are promoted from deferred to mapped.
