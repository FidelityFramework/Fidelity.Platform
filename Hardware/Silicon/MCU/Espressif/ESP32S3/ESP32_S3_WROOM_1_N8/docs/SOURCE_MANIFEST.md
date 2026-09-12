# ESP32-S3-WROOM-1-N8 source manifest

Every fact in this package is transcribed from one of the sources below.
Retrieved 2026-09-11.

| File | Document | Version | Use |
| --- | --- | --- | --- |
| `esp32-s3_datasheet_en.pdf` | ESP32-S3 Series Datasheet | v2.2 (2026-03-09) | Address mapping structure (Fig 4-1), strapping pins (Table 3-1), power-up glitches (Table 2-2), IO MUX function columns, pin power domains |
| `esp32-s3_technical_reference_manual_en.pdf` | ESP32-S3 Technical Reference Manual | v1.8 (2026-03-03) | SRAM bank boundaries (Table 4.3-2), peripheral base addresses (Table 4.3-3), SRAM block aliasing (Table 15.3-1), interrupt matrix |
| `esp32-s3-wroom-1_wroom-1u_datasheet_en.pdf` | ESP32-S3-WROOM-1/1U Datasheet | current at retrieval | Module pad list, in-module flash configuration, the -N8 variant's absence of PSRAM |
| `svd/esp32s3.svd` | `espressif/svd` @ `main`, `svd/esp32s3.svd` | 2.78 MB, 54 peripherals, 2053 registers | Machine-readable register offsets and widths; input to `tools/svd_to_clef.py` |
| `core-isa-esp32s3.h` | `espressif/esp-idf` @ `master`, `components/xtensa/esp32s3/include/xtensa/config/core-isa.h` | current at retrieval | Xtensa LX7 core configuration: vector addresses, register windows, interrupt levels, FPU presence |

## Provenance notes

**The SVD and the TRM were cross-checked where they appeared to disagree.** The
SVD lists `INTERRUPT_CORE0` and `INTERRUPT_CORE1` at the same base address
(`0x600C_2000`), which looks like an error. It is not: TRM Table 4.3-3 declares a
single 4 KB Interrupt Matrix block at `0x600C_2000..0x600C_2FFF` and marks
`0x600C_3000` reserved. One block carries both cores' map registers.

**The SVD and the TRM do genuinely disagree about `EXTMEM`.** The SVD places a
95-register `EXTMEM` peripheral at `0x600C_4000`; TRM Table 4.3-3 marks
`0x600C_4000..0x600C_BFFF` reserved. `EXTMEM` is the external-memory cache and
MMU configuration block, and it is real but publicly undocumented in the address
table. This package does not declare it: an all-SRAM image never configures the
flash cache. A workload that runs from the flash windows will need it, and will
be relying on the SVD rather than the TRM for its address.

**Register-level access permissions are weak evidence.** The SVD leaves `access`
unset on 2048 of its 2053 registers. `tools/svd_to_clef.py` therefore emits
`Access = "rw"` as a transcription default wherever the SVD is silent. That is a
statement about the source document, not a checked hardware permission, and a
register's real permissions should be read from the TRM before a write is relied
on.

**Vector offsets come from the core configuration, not the TRM.** The ESP32-S3
TRM documents the interrupt matrix and VECBASE protection but not the Xtensa
vector layout, which belongs to the LX7 core configuration. `core-isa-esp32s3.h`
is the authority for the offsets in `Description.clef`.

## Offset verification, 2026-09-11

Eight generated offsets were checked by hand against the TRM's register-summary
tables. **All eight offsets agree**, which is the evidence that the SVD
projection is transcribing addresses correctly:

| Register | Generated | TRM summary | TRM access |
| --- | --- | --- | --- |
| `SYSTEM_PERIP_CLK_EN0_REG` | `0x18` | `0x0018` | R/W |
| `GPIO_OUT_W1TS_REG` | `0x8` | `0x0008` | **WO** |
| `LEDC_CONF_REG` | `0xD0` | `0x00D0` | R/W |
| `SYSTIMER_UNIT0_VALUE_LO_REG` | `0x44` | `0x0044` | **RO** |
| `SPI_MS_DLEN_REG` | `0x1C` | `0x001C` | R/W |
| `SYSTIMER_CONF_REG` | `0x0` | `0x0000` | R/W |
| `RTC_CNTL_RTC_WDTWPROTECT_REG` | `0xB0` | `0x00B0` | R/W |
| `TIMG_WDTWPROTECT_REG` | `0x64` | `0x0064` | R/W |

Note that the TRM spells the RTC watchdog register `RTC_CNTL_RTC_WDTWPROTECT_REG`,
doubling the `RTC` prefix, where the SVD spells it `RTC_CNTL.WDTWPROTECT`. The
address agrees; only the name differs.

The same check **demonstrates the access-permission weakness** rather than
leaving it as a caveat. The SVD is silent on all eight, so the generator emitted
`Access = "rw"` for every one — but the TRM records `GPIO_OUT_W1TS_REG` as
write-only and `SYSTIMER_UNIT0_VALUE_LO_REG` as read-only. Two of eight declared
permissions are therefore wider than the hardware's.

That is harmless as inventory, because inventory only permits a transaction and
a workload's `DeviceGrant` may narrow it. It is *not* harmless if a grant is
written against these declarations and trusted: a grant that inherits `"rw"` on
`GPIO_OUT_W1TS_REG` claims a read that returns undefined data, and one on
`SYSTIMER_UNIT0_VALUE_LO_REG` claims a write the hardware ignores.

Before the first `DeviceAccessPlan` is written, the registers it grants should
have their access read out of the TRM and corrected here — either by hand in a
reviewed override table consulted by the generator, or by extracting the access
column from the TRM's register summaries. Until then, treat every `Access` value
in `Registers.clef` as "the SVD did not say".
