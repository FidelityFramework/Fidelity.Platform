# Radio capabilities and the application model

Design handoff, 2026-09-13. Radio joins the hardware/driver outline after the
accepted HelloDISCO checkpoint. This work records hardware availability,
source owners, reusable application semantics and future acceptance questions.
It adds no driver, foreign archive, asynchronous runtime or executable descriptor.
Clef language/compiler readiness for DSP and cryptography remains the next
implementation focus.

## Ownership in Fidelity.Platform

| Concern | Owner |
| --- | --- |
| Radio chip/block identity, supported PHYs and device interfaces | [Hardware/Silicon/Radio](../Hardware/Silicon/Radio/README.md) for future discrete parts; the existing MCU package for an integrated radio |
| Selected module/antenna, power, pins, reset and shared buses | `Hardware/Products/<manufacturer>/<product>/` |
| Reusable wireless wire layouts and protocol state transitions | [Protocols/Radio](../Protocols/Radio/README.md) |
| Native device access or vendor/host binding and its execution requirements | Device backend with the selected silicon/environment dependencies; host/foreign bindings under `Environments/` |
| Shared memory views, access grants, transactions and their obligations | `Contracts/` using BAREWire vocabulary; extend only for a concrete consumer |
| Selected radio roles, buffers, deadlines and interference budgets | The workload's `Profiles/` selection |
| Device discovery UI, synth controls and service payload meaning | Application model and messages, above the backend |

Radio is a peer capability to display, audio and storage. It crosses the same
ownership layers instead of placing an entire networking stack inside a board
descriptor. A radio's availability is distinct from a working driver, and a
working driver is distinct from a checked memory/timing/security claim.

The [hardware inventory](../Hardware/Silicon/Radio/README.md) establishes that
the HelloESP badge contains Wi-Fi and Bluetooth LE, while the DISCO needs an
external radio. Neither has LoRa. The accepted Hello workloads exercise none
of these radio functions.

[Meadow F7 Feather](../Hardware/Products/WildernessLabs/MeadowF7/README.md)
supplies a third arrangement: an STM32F7 application processor with an ESP32
radio coprocessor already on the product. The user's approximately five boards
are intended consumers later in 2026; exact revisions remain unconfirmed.
That future composition must name both devices, the interprocessor transport,
firmware boundary and power/reset ownership. Shared Bluetooth/Wi-Fi semantics
can survive this change of topology; the ESP32-S3 implementation and memory
layout cannot simply be assigned to Meadow's different ESP32 part.

Wilderness Labs documents [separate charging circuitry](https://developer.wildernesslabs.co/Hardware/Reference/Meadow_Hardware/Meadow_F7/)
on the Feather. Charging is a board power capability; sleep/wake involves
processor, radio and rail state. A future lifecycle contract should coordinate
those owners rather than put battery charging inside the Radio API.

## Revisiting BluetoothModule

The user's supplied F# `BluetoothModule` is a useful BLE central/GATT client
sketch: discover devices, connect, discover services and characteristics,
read/write their values, and subscribe to notifications. It also accesses
characteristic descriptors. Its `BluetoothModel` and `dispatch` already expose
a useful state boundary. The dependency,
[Plugin.BLE](https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le), adapts
hosted Xamarin/MAUI Bluetooth services; it is not the missing freestanding
ESP32-S3 or STM32 controller driver.

Preserve those operations as application commands and result/event messages.
The eventual backend can vary while a deterministic model consumes the same
meaning. For the usual phone-as-central design, the instrument would supply
the complementary BLE peripheral/GATT server role; the supplied client code
does not implement that role. A reversed arrangement is also possible.

| Current sample shape | Direction to preserve or refine |
| --- | --- |
| Discovery awaits scanning and dispatches an updated captured model | Dispatch discoveries/completion as messages to the current model; a late response must not overwrite newer state |
| `IDevice`, `IService` and `ICharacteristic` objects | Opaque identities scoped to a connection generation; bound discovery, service and characteristic tables |
| Empty arrays, `None` or `false` on exceptions | Distinguish successful empty values, absence, malformed input, cancellation, timeout and transport failure |
| String UUID parsing during service lookup | Parse/validate UUIDs at the boundary, then use typed values |
| Local event handler added before `StartUpdatesAsync` | Own subscription lifetime, remove handlers on failed start/stop/disconnect, and reject late notifications from an old connection |
| Notification callback reads a byte array and logs it | Validate a bounded payload, establish ownership, then enqueue an application message |
| Implicit async completion and write semantics | Explicit deadlines, cancellation and queue limits; distinguish local acceptance, protocol acknowledgement and application acknowledgement |

These are design requirements, not a port or a claim that Clef's general async
or signal surface is complete. The [display model](DISPLAY_MODEL.md) supplies
the common direction: authoritative state transitions, explicit effects and
selective consumer updates. A bounded notification stream can feed the model;
signals can project selected values into the UI when their lowering is ready.
Latest-value coalescing can suit a color or parameter display. Note on/off
events need a different overflow policy so an omitted note-off cannot leave
the instrument sounding indefinitely.

## Backends and the trust boundary

Espressif distributes [Wi-Fi RF/stack libraries](https://github.com/espressif/esp32-wifi-lib)
and [ESP32-C3/S3 Bluetooth libraries below HCI](https://github.com/espressif/esp32c3-bt-lib)
as precompiled objects under Apache-2.0. A native Clef service API, or even a
native Bluetooth host, would still leave those components outside source-level
analysis when using that vendor path. Static linking changes packaging; it
does not isolate the linked code from unikernel memory or establish its timing.

Keep three backend options open:

| Option | What must be resolved before implementation |
| --- | --- |
| Integrated ESP32-S3 radio through vendor libraries | Exact library/ABI provenance, ROM and runtime dependencies, tasks/interrupts, memory, calibration, callbacks and Wi-Fi/BLE coexistence; linkability with the bare image must be demonstrated |
| External radio/controller over a bounded transport | Selected module and firmware, HCI or device command format, framing/flow control, reset, timeout/recovery and actual host memory access |
| Hosted Bluetooth/network service | Environment-specific binding, permissions/lifecycle and a translation into the same application messages |

ESP-IDF documents a [controller HCI UART route](https://docs.espressif.com/projects/esp-idf/en/stable/esp32s3/api-reference/bluetooth/controller_vhci.html)
as well as virtual HCI. This is a useful architectural seam, not a ready Clef
backend. Wi-Fi does not automatically share that Bluetooth command interface.
An external controller can put vendor execution in another address space,
provided the actual transport cannot write arbitrary host memory. Host framing,
buffer bounds and protocol validation still need checking; radio behavior and
firmware remain explicit premises. Moving work to a second MCU core alone
does not establish memory isolation.

For a synthesizer, remote parameter/preset control is a candidate consumer.
Radio events should enter the bounded control path and should not block PCM
refill. ESP32-S3's LE-only controller does not supply Classic Bluetooth audio;
any audio-over-radio requirement needs its own supported profile, bandwidth
and latency analysis. No wireless audio requirement is selected by this scaffold.

## BAREWire and language-readiness questions

The first selected transport must account for packet length, byte order,
alignment, framing, ownership and completion. DMA access also needs actual
master reachability and cache/publication rules. A callback's borrowed storage
cannot outlive its lease: either retain an explicitly owned slot or copy into a
bounded queue. Zero-copy is a result to establish for a particular path, not an
assumption attached to every notification. Decode standard protocol bytes as
specified; use a versioned application schema only for payloads we control.

Bound work per iteration, pending requests, connections, subscriptions, retries
and queued events. Define queue-full and disconnect behavior. The compiler
work includes integer/bit semantics, bounded storage, ownership/effects and
faithful lowering; a general computation-expression syntax is not a prerequisite
for recording the state machine. BAREWire/MMIO declarations alone prove neither
a complete radio stack nor authenticated communication.

Pairing/bond keys, Wi-Fi credentials and any LoRaWAN durable state need an
explicit entropy, key-custody and storage design. Reuse the direction of
[Modular Blob Storage](../../clef-lang-spec/spec/modular-blob-storage.md) when
those facilities are ready; the HelloDISCO persistence experiment remains
design-only. Durable protocol state and user preferences have different update
and recovery requirements. Do not restore a saved preference as a live connection
or introduce an unsealed credential journal as an interim MBS implementation.

## Resumption point

After the relevant [language-readiness checks](../Profiles/STM32H747I_DISCO_Synth_Reference/scaffold/LANGUAGE_READINESS.md),
choose one role and one consumer. A bounded BLE discovery exercise followed by
one characteristic read/notification is a candidate for a client; advertising
one control characteristic is a different candidate for the instrument server.
Select the backend before estimating either effort.

Accept the first implementation through deterministic message traces, malformed
and oversized input handling, cancellation/disconnect/reconnect behavior,
buffer-lifetime checks, image/resource accounting and an observed peer exchange.
Any bound library must be recorded as part of the artifact and its trust basis.
Add Wi-Fi coexistence or audio-load interference checks only when those workloads
are actually selected. Radio remains optional in the
[DISCO scaffold](../Profiles/STM32H747I_DISCO_Synth_Reference/scaffold/README.md);
it does not extend HelloDISCO's completed hardware milestone.
