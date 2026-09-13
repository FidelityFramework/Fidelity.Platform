# Initial connection and power check

**Latest status, 2026-09-12:** after selecting the documented CN2 host-power
configuration, the user reported the green 5 V LED. A fresh `st-info --probe`
successfully identified **chip ID `0x450`**, family **STM32H74x_H75x**, and
**2,097,152 bytes of flash**. The initial target-access failure is resolved.
No BOOT0 recovery or option-byte change was needed to establish connectivity.
Earlier failed observations below are retained as diagnostic history.

**Application-flash follow-up:** a successful hot-plug read of the entire
`0x08000000..0x081FFFFF` range returned **2 MiB entirely `0xFF`**. Both banks
are erased; no preloaded internal-flash application is present. The
[capture and inspection record](../../../../../../MCU/ST/STM32H747I-DISCO/recovery/2026-09-12-initial-rl_2uh7m/README.md)
preserves the bytes, command and digest. No erase or programming was performed
during that initial capture. The installed flash utility's later live RAM reads
were inconsistent; the application acceptance below uses halted OpenOCD reads.
The user observed rapid red/green COM activity during the read, then steady
red after completion; this is compatible with communication followed by idle.

**Native display follow-up:** HelloDISCO's native display experiment has now
been programmed. The user confirmed color bars and then the orange Clef glyph
after applying the BSP's NT35510 timing values. The corrected 17370-byte image
was programmed, verified and reset using OpenOCD; exact flash readback and
stage-20 status, controller `0x80`, framebuffer `0x24000040`, and zero DSI error
flags are retained in the
[reset acceptance record](../../../../../../MCU/ST/STM32H747I-DISCO/recovery/2026-09-12-static-display-bsp-timings-w7nhibgb/reset-acceptance.json).
Raw IDCODE was `0x20036450`; boot-option values were captured without changes.
The M4 still boots the erased second bank and faults; this experiment claims M7
display operation only. The debugger can halt that faulted core. A deliberate M4
park image or boot policy remains a later requirement, not an accepted dual-core
runtime. No power-supply or option-byte modification was made for display bring-up.

**Banner acceptance:** the subsequent 475936-byte banner firmware passed ELF
segment flash comparison and exact 460800-byte framebuffer comparison. The user
confirmed the banner was visible and returned after disconnecting/reconnecting
CN2, with OpenOCD shut down. The
[accepted banner record](../../../../../../MCU/ST/STM32H747I-DISCO/recovery/2026-09-12-banner-display-eerblu_w/README.md)
contains the final image, hashes, readbacks and limits. This supersedes the
historical erased-flash and blank-display observations below.

Observed 2026-09-12 in this workspace. The user connected two cables and
reported a green blink, a dark display and a steady red LED on the underside.
Connector identities, JP6 position, LED identity and actual board revision
have not yet been confirmed. The user subsequently read the red LED's marking
as **CURRENT OVER**. Both USB cables were to be disconnected immediately;
disconnection has not yet been confirmed. Inspect the unpowered board's actual
connector labels and JP6 position before proposing reconnection. This is an
overcurrent indication, not a confirmed diagnosis of the electrical cause.

Host inspection found STLINK-V3 USB `0483:374e`, a virtual serial port and the
`DIS_H747XI` mass-storage volume. `st-info --probe` found one V3J10 probe but
reported `Failed to enter SWD mode`, chip ID `0x000`, and unknown target with
zero flash/SRAM sizes. Those zero values are failed detection, not MCU facts.
`DETAILS.TXT` reports V3J10M3, built 2021-11-19. `FAIL.TXT` reports that the
interface firmware failed to reset/halt the target. That file alone may reflect
an earlier attempt; the current probe failure independently establishes that
the target was inaccessible during this check.

No flash erase/program, option-byte change, power configuration write or
recovery procedure was performed. The probe attempt is debugger communication,
not proof of a completely passive electrical observation.

### Follow-up: CN2 only

The user subsequently reported one cable from the computer to **CN2**, with
the **COM LED steadily red**. USB enumeration succeeded again, but a fresh
`st-info --probe` still failed to enter SWD and returned chip ID `0x000`.
The present state of **LD8**, **CURRENT OVER**, and **JP6** remains unknown.
The COM indication must be distinguished from the earlier reported overcurrent
indicator before drawing conclusions about target power.
ST [TN1235 Rev 7 §6](https://www.st.com/resource/en/technical_note/tn1235-overview-of-stlink-derivatives-stmicroelectronics.pdf)
defines steady red COM as the idle state after USB enumeration. This is normal
probe status and does not establish that the target is powered or accessible.

The next user observation was **CURRENT OVER off**, **LD8 off**, and JP6 on
the **lowest pair of pins**. Physical orientation and the selected silkscreen
label are still unknown. This is consistent with ST-LINK powered while the
main board's +5 V supply is absent. Local UM2411 Rev 7 Table 3, p18 specifies
**STlk** for CN2 power from an enumerated host PC. The next step is to unplug
CN2, identify and select the pair printed **STlk**, reconnect CN2, and observe
LD8 and CURRENT OVER. Do not infer the printed selection from “lowest.”

After that instruction, the user reported the green LED and the probe succeeded
with the identity above. Its `sram: 131072` field must not be promoted to a total
H747 RAM inventory. `FAIL.TXT` still contains the earlier reset/halt failure;
the current successful target identification supersedes it for connectivity
diagnosis. Exact silicon revision, physical assembly, boot configuration and
display behavior remain unrecorded. This check establishes debug access only.

## Identify power before changing boot state

Local UM2411 Rev 7 §§6.2–6.2.2, pp.15–16, distinguishes these inputs:

| Connector | Purpose | JP6 source label |
| --- | --- | --- |
| CN2 | STLINK USB Micro-B; host/debug connection | STlk for enumerated host power; CHgr for the documented non-enumerated supply mode |
| CN14 | External 5 V USB Micro-B power input | U5V |
| CN1 | USB OTG HS Micro-AB | HS when used as the board power input |
| CN13 | STDC14 debugging header | Not a USB power connector |

Two cables do not combine their power ratings. JP6 selects the board supply.
For the documented CN14 supply plus CN2 debug arrangement, remove power before
moving the selector, select **U5V**, connect the suitable **5 V source to CN14**,
confirm steady **green LD8**, then connect **CN2 to the host**. Removing power
before moving jumpers is the handling recommendation here; the external-first
connection order is the manual's instruction. Confirm silkscreen identity
before applying this sequence to the physical board.

LD8 indicates the board's +5 V rail; it does not measure Vcore or establish
successful MCU boot. LD10 on the underside is the STLINK communication LED.
LD9 indicates STLINK power overcurrent; LD5 indicates USB HS overcurrent. An
identified overcurrent indication calls for disconnecting supplies and checking
the cause, not a boot-mode change.

After establishing the selected supply, retry the probe and record the target
identity. If access still fails, distinguish reset wiring, debug configuration,
firmware/boot state, protection and SMPS/LDO compatibility using the actual
board revision and preserved settings. A dark LCD or chip ID `0x000` is not
sufficient evidence to modify BOOT0/R192, erase flash or change option bytes.

The [live UM2411 URL](https://www.st.com/resource/en/user_manual/um2411-discovery-kit-with-stm32h747xi-mcu-stmicroelectronics.pdf)
served Rev 8 (July 2026) when checked; the local Rev 7 remains the pinned source
for the page references above. A displayed image requires running firmware and
the correct panel initialization as well as power. Confirm the product's
factory-software status before assuming there should already be a demo. Local
Rev 7 Table 23 explicitly records no preloaded demonstration for product IDs
DK32H747I$AT2 and DK32H747I$AT3; do not generalize that to an unidentified unit.
