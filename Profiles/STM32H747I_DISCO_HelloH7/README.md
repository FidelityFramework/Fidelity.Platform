# STM32H747I_DISCO_HelloH7 — historical selection

This earlier profile is retained as a draft source artifact. The active
bring-up application and checked profile are now
[HelloDISCO](../STM32H747I_DISCO_HelloDISCO/README.md).

The minimum H7 family/part/product/environment closure now exists and the
Composer/CCS M7 build path has been implemented. The old README's missing-file
and M33-only backend blockers no longer describe this workspace. HelloDISCO's
first GPIO image builds; this legacy selection has no corresponding HelloH7
application or hardware acceptance.

Do not use the earlier proposal as authority for BCM4 option-byte changes,
RAM retention, or universal DMA exclusion from TCM. DMA1/2 cannot reach TCM;
MDMA can. Startup initialization determines whether diagnostic words survive
software reset. Boot addresses and software policy assign images to cores;
flash bank 2 is not intrinsically M4-owned. No option-byte change has been made.

The [current design](../../docs/STM32H7_SYNTH_DESIGN.md),
[source audit](../../Hardware/Products/ST/STM32H747I_DISCO/docs/SOURCE_AUDIT.md)
and [connection record](../../Hardware/Products/ST/STM32H747I_DISCO/docs/CONNECTION_CHECK.md)
track the accepted source, physical observations and remaining work. Initial
probing identified chip ID `0x450`, and the first full internal-flash read found
both banks erased. Subsequent HelloDISCO display images have been programmed:
the landscape banner is visible and returns after CN2 reconnect without a
debugger. That acceptance belongs to the
[display selection](../STM32H747I_DISCO_HelloDISCO_Display/README.md), not this
historical profile.
