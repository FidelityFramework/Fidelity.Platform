# STM32H747XIH6 selected GPIO and display slices

[Description.clef](Description.clef) declares identified-part memory envelopes
and vector storage. [Registers.clef](Registers.clef) supplies 20 selected GPIOI,
GPIOK, RCC and read-only diagnostic transactions through Fidelity MMIO contracts.
A profile supplies mappings and grants over these original spaces.

[Source authority, limitations and validation](docs/INITIAL_GPIO_SOURCES.md)
distinguish source closure from generated-image or hardware acceptance.

[DisplayRegisters.clef](DisplayRegisters.clef) supplies a separate 80-register
RCC/GPIOG/GPIOJ/DSI/LTDC slice. The
[display profile](../../../../../../Profiles/STM32H747I_DISCO_HelloDISCO_Display/README.md)
selects that source explicitly alongside the ordinary part dependency; the
base part manifest does not expose it implicitly. The native banner is visible
on the connected NT35510 panel and returns after CN2 reconnect without a
debugger. [Pinned display sources and the preserved offset oracle](docs/display/SOURCES.md)
record its limits and evidence. Neither slice is a full STM32H7 register or
driver catalog; audio, touch, SDRAM and general DMA/cache ownership remain open.
