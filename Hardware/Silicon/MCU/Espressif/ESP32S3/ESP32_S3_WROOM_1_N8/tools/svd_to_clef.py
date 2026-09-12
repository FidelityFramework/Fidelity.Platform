#!/usr/bin/env python3
"""Project Espressif's ESP32-S3 SVD onto Fidelity.Platform device declarations.

The SVD is the register-inventory premise; this tool transcribes it, it does not
infer capability. Only the peripherals and registers named in SELECTION below are
emitted: inventory names the transactions a driver-facing view permits, and the
workload still owns its mappings and grants (see docs/MMIO_CONTRACTS.md).

Every ESP32-S3 peripheral register is 32 bits wide and word-addressed, so every
emitted DeviceRegister carries TransactionBits = 32. The SVD leaves register-level
`access` unset for 2048 of its 2053 registers, so Access is taken from the SVD when
present and otherwise declared "rw" -- recorded here as a transcription default,
not as a checked hardware permission.

Usage:  svd_to_clef.py <esp32s3.svd> [--out Registers.clef]
"""

import argparse
import sys
import xml.etree.ElementTree as ET

# Peripheral -> region name, and the registers this bring-up view permits.
# "NAME[i]" selects one element of an SVD <dim> array (NAME%s with dimIncrement).
SELECTION = [
    ("RTC_CNTL", "rtcCntl", [
        # The RTC and super watchdogs run out of reset; an image that does not
        # disarm them is reset before it reaches the application.
        "WDTCONFIG0", "WDTWPROTECT", "SWD_CONF", "SWD_WPROTECT", "CLK_CONF",
    ]),
    ("TIMG0", "timg0", ["WDTCONFIG0", "WDTWPROTECT"]),
    ("TIMG1", "timg1", ["WDTCONFIG0", "WDTWPROTECT"]),
    ("SYSTEM", "system", [
        # Peripheral clock gating and reset. SPI2, LEDC, RMT and SYSTIMER are
        # gated at reset; a write here precedes any other peripheral transaction.
        "PERIP_CLK_EN0", "PERIP_CLK_EN1", "PERIP_RST_EN0", "PERIP_RST_EN1",
        "SYSCLK_CONF", "CPU_PER_CONF",
    ]),
    ("IO_MUX", "ioMux", [
        # Per-pad function select, drive strength and pull configuration.
        # GPIO[n] is the pad at index n; the badge's pads are listed in the
        # product package, not here.
        "PIN_CTRL",
        "GPIO[4]", "GPIO[5]", "GPIO[6]", "GPIO[7]", "GPIO[9]", "GPIO[10]",
        "GPIO[11]", "GPIO[12]", "GPIO[1]", "GPIO[2]", "GPIO[43]", "GPIO[44]",
    ]),
    ("GPIO", "gpio", [
        # Set/clear-on-write registers keep a read-modify-write out of the
        # driver: OUT_W1TS/W1TC are the display control lines' only writes.
        "OUT", "OUT_W1TS", "OUT_W1TC", "OUT1", "OUT1_W1TS", "OUT1_W1TC",
        "ENABLE", "ENABLE_W1TS", "ENABLE_W1TC",
        "ENABLE1", "ENABLE1_W1TS", "ENABLE1_W1TC",
        "IN", "IN1", "STRAP",
        # GPIO matrix output routing for the pads the badge drives from a
        # peripheral rather than from the GPIO output register.
        "FUNC[4]_OUT_SEL_CFG", "FUNC[5]_OUT_SEL_CFG",
    ]),
    ("SPI2", "spi2", [
        # The display bus. GPIO10/11/12 are FSPI's own IO_MUX pins on this
        # package, so SPI2 reaches the panel without the GPIO matrix.
        "CMD", "CTRL", "CLOCK", "USER", "USER1", "USER2", "MS_DLEN", "MISC",
        "DMA_CONF", "SLAVE", "CLK_GATE",
        "W0", "W1", "W2", "W3", "W4", "W5", "W6", "W7",
        "W8", "W9", "W10", "W11", "W12", "W13", "W14", "W15",
    ]),
    ("LEDC", "ledc", [
        # Backlight PWM on GPIO5. Timer 0 drives channel 0.
        "CONF", "TIMER[0]_CONF", "CH[0]_CONF0", "CH[0]_CONF1",
    ]),
    ("RMT", "rmt", [
        # WS2812 bit timing for the five badge LEDs.
        "SYS_CONF", "REF_CNT_RST", "TX_SIM", "INT_ENA", "INT_RAW", "INT_CLR",
    ]),
    ("SYSTIMER", "systimer", [
        # The LVGL tick and every bring-up delay read UNIT0.
        "CONF", "UNIT0_OP", "UNIT0_LOAD_HI", "UNIT0_LOAD_LO", "UNIT0_LOAD",
        "UNIT0_VALUE_HI", "UNIT0_VALUE_LO",
        "TARGET0_HI", "TARGET0_LO", "TARGET0_CONF", "COMP0_LOAD",
        "INT_ENA", "INT_RAW", "INT_CLR", "INT_ST",
    ]),
    ("USB_DEVICE", "usbSerialJtag", [
        # USB Serial/JTAG is the console on this board: GPIO19/20 are wired to
        # the USB-Micro connector and no UART is broken out.
        "EP1", "EP1_CONF", "CONF0", "INT_RAW", "INT_ST", "INT_ENA", "INT_CLR",
        "JFIFO_ST",
    ]),
    ("INTERRUPT_CORE0", "interruptCore0", [
        # Xtensa has no NVIC: the interrupt matrix routes a peripheral source
        # onto one of the CPU's own interrupt numbers, which the vector table
        # then dispatches.
        "SPI2_DMA_INT_MAP", "LEDC_INT_MAP", "RMT_INTR_MAP",
        "SYSTIMER_TARGET0_INT_MAP", "USB_DEVICE_INT_MAP", "GPIO_INTERRUPT_PRO_MAP",
    ]),
]


def load(path):
    """peripheral name -> (baseAddress, {register name -> (offset, access)})."""
    root = ET.parse(path).getroot()
    raw = {}
    for p in root.iter("peripheral"):
        raw[p.findtext("name")] = p

    def registers(p, seen=()):
        """Resolve <register> entries, following derivedFrom for a shared block."""
        name = p.findtext("name")
        derived = p.get("derivedFrom")
        out = {}
        if derived and derived not in seen and derived in raw:
            out.update(registers(raw[derived], seen + (name,)))
        for reg in p.findall(".//register"):
            offset = int(reg.findtext("addressOffset"), 16)
            access = reg.findtext("access")
            size = int(reg.findtext("size") or "0x20", 16)
            if size != 32:
                raise SystemExit(f"{name}.{reg.findtext('name')}: unexpected {size}-bit register")
            dim = reg.findtext("dim")
            rname = reg.findtext("name")
            if dim:
                step = int(reg.findtext("dimIncrement"), 16)
                stem = rname.replace("%s", "")
                for i in range(int(dim)):
                    out[f"{stem}[{i}]"] = (offset + i * step, access)
                    # LEDC spells CH%s_CONF0; index the stem, not the tail.
                    if "%s" in rname:
                        out[rname.replace("%s", f"[{i}]")] = (offset + i * step, access)
            else:
                out[rname] = (offset, access)
        return out

    return {n: (int(p.findtext("baseAddress"), 16), registers(p)) for n, p in raw.items()}


def clef_name(peripheral, register):
    """A stable, statically known Clef binding name: gpioOutW1ts, ioMuxGpio11."""
    tail = register.replace("[", "").replace("]", "")
    parts = [w for w in tail.replace("%s", "").split("_") if w]
    camel = "".join(p.capitalize() if i else p.lower() for i, p in enumerate(parts))
    return peripheral + camel[0].upper() + camel[1:]


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("svd")
    ap.add_argument("--out", default="-")
    args = ap.parse_args()

    svd = load(args.svd)
    lines = [
        "module Fidelity.Platform.Hardware.Silicon.MCU.Espressif.ESP32S3.ESP32_S3_WROOM_1_N8.Registers",
        "",
        "open Fidelity.Platform.Contracts",
        "",
        "// GENERATED by tools/svd_to_clef.py from docs/svd/esp32s3.svd. Do not edit by hand;",
        "// change the SELECTION list in the generator and regenerate.",
        "//",
        "// Every ESP32-S3 peripheral register is 32 bits and word-addressed, so every",
        "// transaction below is 32-bit. Inventory permits a transaction; it does not grant",
        "// access -- the workload's DeviceAccessPlan does that. The SVD leaves register-level",
        "// access unset for nearly every register, so \"rw\" here is a transcription default",
        "// and not a checked hardware permission.",
        "",
        f'let source = "espressif/svd@main svd/esp32s3.svd; ESP32-S3 TRM v1.8; ESP32-S3 Datasheet v2.2"',
        "",
    ]

    missing, count = [], 0
    for peripheral, region, selected in SELECTION:
        if peripheral not in svd:
            missing.append(peripheral)
            continue
        base, regs = svd[peripheral]
        lines.append(f"// --- {peripheral} @ 0x{base:08X} " + "-" * (44 - len(peripheral)))
        lines.append(
            f'let {region}Region: DeviceRegion = {{ Space = Description.{region}; '
            f'AddressSpace = "cpu-physical"; Source = source }}'
        )
        for sel in selected:
            if sel not in regs:
                missing.append(f"{peripheral}.{sel}")
                continue
            offset, access = regs[sel]
            lines.append(
                f"let {clef_name(region, sel)}: DeviceRegister = {{ "
                f'Name = "{clef_name(region, sel)}"; Region = {region}Region; '
                f"Offset = 0x{offset:X}; TransactionBits = 32; "
                f'Access = "{ {"read-only": "r", "write-only": "w"}.get(access, "rw") }"; '
                f'ByteOrder = "little"; Ordering = "volatile" }}'
            )
            count += 1
        lines.append("")

    text = "\n".join(lines).rstrip() + "\n"
    if args.out == "-":
        sys.stdout.write(text)
    else:
        with open(args.out, "w") as f:
            f.write(text)
    print(f"emitted {count} registers over {len(SELECTION)} peripherals -> {args.out}", file=sys.stderr)
    if missing:
        print("NOT FOUND IN SVD: " + ", ".join(missing), file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
