# Carolina Code Conference 2026 badge — hardware reference

Designed by [Circuit Board Medics](https://circuitboardmedics.com) for the
Carolina Code Conference 2026. Silicon is an **ESP32-S3-WROOM-1-N8** module
(dual Xtensa LX7, 512 KB SRAM, 8 MB in-module quad SPI flash, **no PSRAM**).

This file is the wiring premise for
[`../Description.clef`](../Description.clef). It records what is physically
connected and what the silicon permits on those pins. It does not grant an
application access to anything — grants live in the workload's
`DeviceAccessPlan`.

## Evidence and its limits

The on-board signal assignments below are transcribed from the badge's own
repository (`README.md`, `AGENTS.md`) and **cross-checked against the shipped
CircuitPython sample code**, which is working evidence rather than
documentation: every pin in the "on-board peripherals" table appears in
`samples/*/code.py` driving the real hardware.

The broken-out pad labels are transcribed from the PCB silkscreen as
photographed in `img/badge_photo.png`, and the user has confirmed the control
inventory (five addressable LEDs, three control buttons distinct from RESET and
BOOT, one battery ON/OFF slide switch).

**There is no schematic or netlist for this board.** The EK-RA6M5 product
package in this repository can declare literal copper nets because Renesas
publishes a design package; nothing equivalent exists here. So this file
records *signal assignments*, not net membership, and
`Description.clef` declares no `BoardTerminal` array. Two consequences worth
stating plainly:

- Pull-up/pull-down resistor values, series resistors on the panel lines, the
  LED power path and the battery/USB switchover circuit are **not known** from
  the documents. The switchover behaviour below is quoted from the badge
  README, not derived from a circuit.
- A pad's electrical drive capability is the *chip's* specification, not this
  *board's*. Nothing is known about trace width or protection on the pads.

## On-board peripherals

### Display — HS180S10B, ST7735S controller, 160 × 128, 1.77 in

LCSC part `C5329585`. The `adafruit_st7735r` driver works against it because
the ST7735S is command-compatible with the ST7735R.

| Signal | GPIO | IO MUX function 0 on that pad | Notes |
| --- | --- | --- | --- |
| SPI clock (SCK) | GPIO12 | **FSPICLK** | native FSPI pin |
| MOSI | GPIO11 | **FSPID** | native FSPI pin; data into the panel |
| Chip select (CS) | GPIO10 | **FSPICS0** | native FSPI pin; active low |
| Data/command (DC) | GPIO6 | GPIO | high = data, low = command |
| Reset | GPIO7 | GPIO | active low |
| Backlight | GPIO5 | GPIO | high = on; drive from LEDC for brightness |
| Font chip select | GPIO9 | GPIO | active low; hold **high** to ignore the chip |
| Font chip data out (FS0) | GPIO44 | U0RXD | panel-module font ROM's MISO |

**The panel sits on SPI2's own IO MUX pins.** GPIO10/11/12 are `FSPICS0`,
`FSPID` and `FSPICLK` in the IO MUX, so SPI2 reaches the panel directly without
routing through the GPIO matrix. That is the fast path: a matrix route costs
setup and caps the usable clock, and a direct IO MUX route does not. Take it.

**The font chip is a trap worth knowing about.** The panel module carries a
separate font ROM sharing the SPI bus, selected by GPIO9 and returning data on
GPIO44. The shipped CircuitPython samples all drive GPIO9 high and never read
it. Do the same unless you specifically want the font ROM: leaving GPIO9
floating lets the font chip contend for the bus.

**GPIO44 is U0RXD.** Its function-0 assignment is UART0 receive.

### Addressable LEDs — five WS2812

| Item | Detail |
| --- | --- |
| Count | 5, silkscreened LED1..LED5 |
| Data | GPIO4, single chained data line |
| Protocol | WS2812 (NeoPixel), 24 bits per LED, GRB order |

WS2812 needs sub-microsecond pulse-width accuracy. Generate it with the **RMT**
peripheral, which emits level/duration symbol pairs from its own RAM, rather
than bit-banging GPIO4 — a bit-banged stream is corrupted by any interrupt
taken mid-frame.

### Buttons

Three user buttons, wired to ground and relying on the chip's internal weak
pull-ups: each pin reads **high when idle, low when pressed** (active low).
Enable the pull-up in the pad's IO MUX config word; there is no external
pull-up to assume.

| Silkscreen | GPIO | IO MUX function 0 | Notes |
| --- | --- | --- | --- |
| S1 | GPIO1 | GPIO | |
| S2 | GPIO2 | GPIO | |
| S3 | GPIO43 | **U0TXD** | weak pull-up + input enabled at reset |

Two more buttons are not general-purpose input:

| Silkscreen | Function | Connection |
| --- | --- | --- |
| SW3 "BOOT" | Boot-mode strapping | GPIO0 |
| SW2 "RESET" | Chip reset | `EN` / `CHIP_PU`, not a GPIO |

And one slide switch:

| Silkscreen | Function |
| --- | --- |
| SW1 "ON/OFF" | Switches the **battery** in and out. It has no GPIO and does not control USB power. |

> **A naming collision to watch.** The badge README calls the three user
> buttons "SW 1/2/3", but the silkscreen calls them **S1/S2/S3** and uses
> **SW1/SW2/SW3** for the power slide switch, RESET and BOOT. This file follows
> the silkscreen. When reading the README's switch table, "SW 1" means S1.
>
> Also note `AGENTS.md` in the badge repository describes the buttons as
> "pull-down configured — GPIO reads LOW when pressed" and then shows code
> setting `digitalio.Pull.UP`. The prose is wrong and the code is right: these
> are active-low buttons on internal pull-ups.

### USB

| Signal | GPIO |
| --- | --- |
| USB D+ | GPIO20 |
| USB D− | GPIO19 |

Wired to the USB-Micro connector (`USB1`) and driven by the **USB Serial/JTAG
controller**, not an external bridge — the board enumerates natively.

**This is the only console the board wires to a connector.** UART0 is not
available: GPIO43/44, its default `U0TXD`/`U0RXD` pins, carry S3 and the
font-chip data line instead. So an image that wants `printf` over the shipped
cable must bring up USB Serial/JTAG.

The fallback is a soldering iron rather than a dead end — UART1's own IO MUX
pins are broken out on pads IO17/IO18. For first bring-up, before USB is
standing up, that two-wire UART is considerably less work than a USB stack.

### Power

Supplied from either the USB-Micro connector or a **CR123A** cell. Per the badge
README, the control circuitry disconnects the battery automatically whenever USB
power is present, regardless of the slide switch, and there is **no on-board
charger** — so turn SW1 off or remove the cell when the badge is stored. The
board stays powered from USB with SW1 in either position.

## Broken-out GPIO pads

The badge has **no standard header**. Unused GPIOs terminate in 2.54 mm
through-hole solder pads, silkscreen-labelled, in three groups plus a power
pair. Fit 0.1 in headers if you want them removable.

Each pad's IO MUX configuration word is at `IO_MUX + 0x4 + 4 × n`, where `n` is
the GPIO number and `IO_MUX` is `0x6000_9000` — the `Offset` column gives that
address, so the pad maps straight onto a `DeviceRegister`.

### Left column, five pads (beside the display)

| Pad | GPIO | IO MUX F0 | Offset | Alternates | Analog |
| --- | --- | --- | --- | --- | --- |
| IO15 | 15 | GPIO | `0x60009040` | `U0RTS` (F2), `XTAL_32K_P` | `ADC2_CH4`, `RTC_GPIO15` |
| IO16 | 16 | GPIO | `0x60009044` | `U0CTS` (F2), `XTAL_32K_N` | `ADC2_CH5`, `RTC_GPIO16` |
| IO17 | 17 | GPIO | `0x60009048` | **`U1TXD`** (F2) | `ADC2_CH6`, `RTC_GPIO17` |
| IO18 | 18 | GPIO | `0x6000904C` | **`U1RXD`** (F2), `CLK_OUT3` | `ADC2_CH7`, `RTC_GPIO18` |
| IO8 | 8 | GPIO | `0x60009024` | `SUBSPICS1` (F2) | `ADC1_CH7`, `TOUCH8`, `RTC_GPIO8` |

**IO17 and IO18 are UART1's own IO MUX pins.** That is the serial console
escape hatch: a hardware UART on two adjacent pads, needing no GPIO matrix
route. Nothing is *wired* to a connector, but the peripheral is one header away.
IO15/IO16 additionally accept a 32.768 kHz crystal, and IO8 has capacitive
touch.

### Bottom row, eight pads (below the module)

| Pad | GPIO | IO MUX F0 | Offset | Notes |
| --- | --- | --- | --- | --- |
| IO3 | 3 | GPIO | `0x60009010` | ⚠ **strapping** — JTAG source select |
| IO46 | 46 | GPIO | `0x600090BC` | ⚠ **strapping** — boot mode + ROM print |
| IO13 | 13 | GPIO | `0x60009038` | `FSPIQ`/`FSPIIO7` (F2/F3); `ADC2_CH2`, `TOUCH13` |
| IO14 | 14 | GPIO | `0x6000903C` | `FSPIWP`/`FSPIDQS` (F2/F3); `ADC2_CH3`, `TOUCH14` |
| IO21 | 21 | GPIO | `0x60009058` | plain GPIO on every function; `RTC_GPIO21`, no ADC |
| IO47 | 47 | `SPICLK_P_DIFF` | `0x600090C0` | F1 is GPIO47 |
| IO48 | 48 | `SPICLK_N_DIFF` | `0x600090C4` | F1 is GPIO48 |
| IO45 | 45 | GPIO | `0x600090B8` | ⚠⚠ **strapping — VDD_SPI voltage. See hazards.** |

### Right column, eight pads (beside the buttons)

| Pad | GPIO | IO MUX F0 | Offset | Notes |
| --- | --- | --- | --- | --- |
| IO42 | 42 | **MTMS** | `0x600090AC` | JTAG |
| IO41 | 41 | **MTDI** | `0x600090A8` | JTAG |
| IO40 | 40 | **MTDO** | `0x600090A4` | JTAG |
| IO39 | 39 | **MTCK** | `0x600090A0` | JTAG |
| IO38 | 38 | GPIO | `0x6000909C` | `FSPIWP` on F2 |
| IO37 | 37 | GPIO | `0x60009098` | `FSPIQ` on F2; free only because this is an **N8** module |
| IO36 | 36 | GPIO | `0x60009094` | `FSPICLK` on F2; N8-only |
| IO35 | 35 | GPIO | `0x60009090` | `FSPID` on F2; N8-only |

### Power pads

| Pad | Detail |
| --- | --- |
| `GND` | Ground |
| `3V3 Out` | 3.3 V from the on-board regulator. Budget unknown — no schematic. |

## What the pad map gives you

**The whole JTAG port is broken out.** IO39/40/41/42 are `MTCK`, `MTDO`, `MTDI`
and `MTMS` — a complete JTAG TAP on adjacent pads. For bare-metal bring-up,
where there is no interpreter left to print from, that is the difference between
debugging and guessing.

**And you may not even need to solder to it.** With no eFuses burned — the
factory state — the default JTAG signal source is the **USB Serial/JTAG
controller**, not the pins (datasheet Table 3-5). So JTAG is already reachable
over the same USB-Micro cable that carries the console. The pads matter when you
want an external probe, or once `EFUSE_STRAP_JTAG_SEL` and IO3 have been used to
switch the source to the pins.

**A second SPI bus is available, with a caveat.** IO33–38 carry `FSPIHD`,
`FSPICS0`, `FSPID`, `FSPICLK`, `FSPIQ` and `FSPIWP` on function 2, and IO35/36/37
are broken out. But those are *the same FSPI signals* the display already uses on
GPIO10/11/12 — the IO MUX offers one FSPI on two pin groups, not two independent
buses. A second SPI peripheral on the pads must use **SPI3** or reach the pads
through the GPIO matrix.

**Eleven pads are unconditionally free:** IO8, IO13, IO14, IO15, IO16, IO17,
IO18, IO21, IO38, IO47, IO48. IO35/36/37 join them on this module because it is
an N8 part. Add JTAG's four if you are debugging over USB, and the strapping
three with care.

## Hazards

### ⚠⚠ IO45 sets the flash rail voltage

GPIO45 is the `VDD_SPI` voltage strapping pin. Its internal weak pull-down is
what selects 3.3 V for `VDD_SPI` at reset, and `VDD_SPI` feeds the module's
flash die.

**Pull IO45 high at reset and the module tries to run its 3.3 V flash from a
1.8 V rail. It will not boot.** The failure looks like a dead board, and because
`VDD_SPI` also powers the IO35–37 pad group, it is not obvious from the outside.

This pin is a latched strapping input only during reset; afterwards it is an
ordinary GPIO. But anything you attach to the pad — a pull-up, a driven output
from another device, even a high-impedance leakage path — is present *at the
next reset too*. Treat IO45 as output-only, drive it low or leave it floating
through reset, and never put a pull-up on it.

### ⚠ IO46 and IO3 are also strapping pins

- **IO46** (weak pull-down at reset) contributes to boot-mode selection with
  GPIO0 and controls ROM message printing. Holding it high at reset changes how
  the chip boots.
- **IO3** (floating at reset, no internal pull) selects the JTAG signal source
  when `EFUSE_STRAP_JTAG_SEL` is burned. Floating means *undriven*, so a long
  wire on this pad picks up noise; pull it deliberately if you use it.

GPIO0 is the BOOT button and is also strapping, but it is not on a pad.

### ⚠ Sixteen pins glitch low for ~60 µs at power-up

Datasheet Table 2-2 lists low-level glitches of typically 60 µs on GPIO1–14,
GPIO17, GPIO18 and others; GPIO18 and GPIO19 glitch both directions. That covers
**almost every on-board signal**: both S1/S2, the LED data line, the backlight,
and all four display control lines.

Two practical consequences:

- **The panel flashes white on cold boot** unless the backlight is driven low
  early. The shipped CircuitPython launcher works around exactly this by
  grabbing GPIO5 and driving it low as its very first action, before its slow
  imports. A bare-metal image should do the same, and can do it far sooner.
- **A glitch on GPIO4 can emit a spurious WS2812 symbol.** Expect the LEDs to
  need an explicit clear at startup rather than assuming they come up dark.

### ⚠ The flash is not a spare peripheral

`SPICS1`, `SPIHD`, `SPIWP`, `SPICS0`, `SPICLK`, `SPIQ` and `SPID` are bonded to
the flash die inside the module and are not brought out. They are not available
as GPIOs, and driving them would break code fetch. The `3V3 Out` pad is the only
power a pad offers.

## Facts that shape a bare-metal image

- **No PSRAM.** All working memory is the 512 KB of internal SRAM. A full
  160 × 128 framebuffer at 16 bpp is 40,960 bytes, so even double buffering
  costs 80 KB — comfortable. An image that fits in SRAM needs no flash cache or
  MMU configuration at all.
- **Every peripheral register is 32 bits and word-addressed.** There are no
  byte or halfword peripheral transactions on this part, so every MMIO binding
  is `Mmio.bind32`.
- **Peripherals are clock-gated at reset.** `SYSTEM.PERIP_CLK_EN0/1` must ungate
  SPI2, LEDC, RMT and SYSTIMER before their registers mean anything; a
  gated peripheral reads back zeros and silently discards writes.
- **Three watchdogs are armed before your code runs**: the RTC watchdog, the
  super watchdog and the timer-group MWDTs. An image that neither feeds nor
  disarms them is reset partway through bring-up, which presents as a boot loop
  with no output.
- **Xtensa has register windows and no NVIC.** Window overflow/underflow vectors
  are mandatory, and peripheral interrupts must be routed onto CPU interrupt
  numbers through the interrupt matrix. See
  [`../../../Silicon/MCU/Espressif/ESP32S3/ESP32_S3_WROOM_1_N8/Description.clef`](../../../Silicon/MCU/Espressif/ESP32S3/ESP32_S3_WROOM_1_N8/Description.clef)
  for the vector layout.

## Read from this board's silicon, 2026-09-11

Everything above is transcribed from documents and the silkscreen. The facts
below were read off the actual badge over the USB Serial/JTAG interface with
`esptool`, so they are measurements of one physical unit.

| Fact | Value |
| --- | --- |
| Chip | ESP32-S3, **QFN56**, revision **v0.2** |
| Features | Wi-Fi, BT 5 (LE), dual core + **LP core**, 240 MHz |
| Crystal | 40 MHz |
| Base MAC | `1c:db:d4:8c:9d:3c` |
| CircuitPython-reported MAC | `E8:5B:CB:3F:00:99` (a derived interface MAC, not the base) |
| Flash | **8 MB**, manufacturer `0x20`, device `0x4017` |
| Flash bus | **quad**, set in eFuse |
| Flash voltage | **3.3 V, set by a strapping pin** |

Two of these settle questions the documents leave open.

**The flash really is 8 MB on a quad bus**, so an image header for this board
carries `esp_image_flash_size_t = 3` and a quad SPI mode. The `-N8` in the
module's part number is confirmed by the part itself.

**"Flash voltage set by a strapping pin" is the IO45 hazard, reported by the
hardware.** The chip is telling us that the rail its flash die runs on was
chosen by a pin level latched at reset — the pin broken out on a pad, one row
away from IO46 and IO3. Nothing else on this board can brick it as quietly.

The chip also reports its USB mode as **USB Serial/JTAG** when reached through
the ROM's download path, confirming that the debug unit behind the USB-Micro
connector is the real thing rather than a CDC emulation.

## As shipped

The badge ships with **CircuitPython 10.2.1** flashed
(`espressif_esp32s3_devkitc_1_n8` build) and a sample launcher as `code.py`.
The device enumerates as USB `303a:7003`, presenting a CDC console and a
`CIRCUITPY` mass-storage volume.

Reflashing replaces that. **Read the 8 MB flash out to a file first** — the
CircuitPython image is recoverable from
[circuitpython.org](https://circuitpython.org/board/espressif_esp32s3_devkitc_1_n8/),
but a byte-exact dump also preserves the board's NVM, and it is the same
`recovery/<date>-stock` discipline the EK-RA6M5 bring-up already follows.
