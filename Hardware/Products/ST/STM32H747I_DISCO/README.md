# STM32H747I-DISCO source pack and HelloDISCO

Status, 2026-09-12: the family, part, product and environment source closure is
present and checked for the initial GPIO workload. The
[HelloDISCO profile](../../../../Profiles/STM32H747I_DISCO_HelloDISCO/README.md)
builds a verified Cortex-M7 LED/joystick image. The separate
[static-display profile](../../../../Profiles/STM32H747I_DISCO_HelloDISCO_Display/README.md)
now runs native LTDC/DSI code: the user confirmed the landscape Clef banner on
the NT35510 panel and its return after reconnecting CN2 without a debugger.
Audio, touch, general DMA ownership and external-memory
protocols remain outside this accepted display path.

The [source audit](docs/SOURCE_AUDIT.md) identifies downloaded documents by
contents, hashes, revisions and board applicability. The
[synthesizer design](../../../../docs/STM32H7_SYNTH_DESIGN.md) defines the full
audio/display/touch direction and the staged path to a native Clef unikernel.
The [connection check](docs/CONNECTION_CHECK.md) records successful power and
probe follow-up, chip ID `0x450`, and the saved read of erased internal flash.

The staged [UM2411 manual](docs/um2411-discovery-kit-with-stm32h747xi-mcu-stmicroelectronics.pdf)
is Rev 7, September 2025. Its introduction identifies STM32H747I-DISCO and the
STM32H747I-DISC1 variant without the LCD module. The
[DB3608 data brief](docs/stm32h747i-disco.pdf) is Rev 3, August 2025. These document
revisions are not observed physical-board revisions.

The [STMod+ interface reference](docs/tn1238-stmod-interface-specification-stmicroelectronics.pdf)
and [source-pack license](docs/open_platform_license_agreement.pdf) remain beside
the product documents. All staged PDF bytes are preserved. Silicon-part facts,
variant applicability, memory layout, pin routing and execution requirements
must be reconciled per workload. The
[initial register source record](../../../Silicon/MCU/ST/STM32H7/STM32H747XIH6/docs/INITIAL_GPIO_SOURCES.md)
states the limited GPIO acceptance and missing reference authorities.
