# Microcontroller silicon

Part families and exact packages live here. They own register inventories,
package pins, silicon memory and peripheral capabilities. Shared core facts live
under [CPU](../CPU/); product wiring and vendor board packs live under
[Products](../../Products/).

The [RA6M5 part](Renesas/RA6M5/R7FA6M5BH3CFC/README.md) supports the accepted
EK-RA6M5 HelloBlinky composition. The [STM32F7 family](ST/STM32F7/README.md)
remains a scaffold. Wiring coverage does not imply driver or startup support.

Production declarations use `.clef`, with explicit source lists in `.fidproj`.
Every pin mapping must identify its authoritative source and applicable variant.
Product documentation may be stored locally or pinned by path and hash in a
reference pack; its availability must be explicit.
