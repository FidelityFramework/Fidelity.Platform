# Language readiness before expanding DISCO

This is a short handoff to the Clef language/compiler work, not a declaration
that the listed facilities are implemented. Specifications, design proposals,
executable compiler support and target evidence must be recorded separately.
The review behind this outline did not establish implementation coverage beyond
the retained HelloDISCO checks.

## Shared needs of DSP, cryptography and drivers

| Concern | What the language/compiler work needs to settle | Existing reference |
| --- | --- | --- |
| Integer semantics | Intermediate capacity, deliberate modulo arithmetic, signed remainder, valid shifts and nonzero divisors; a small final range does not make overflowing intermediates valid | [Width inference](../../../../clef-lang-spec/spec/width-inference.md) |
| DSP numerics | Range, accuracy and reproducibility as distinct obligations; fixed-point scale/rescaling, floating rounding points, exceptional values and permitted reduction order | [Numeric selection](../../../../clef-lang-spec/spec/numeric-selection.md), [rounding](../../../../clef-lang-spec/spec/rounding.md) |
| Bounded representation | Static/region/stack storage for buffers, records and closures; exact boundary layouts and widths; rejection of allocations or escapes unsupported by a heapless target | [Memory regions](../../../../clef-lang-spec/spec/memory-regions.md), [closure representation](../../../../clef-lang-spec/spec/closure-representation.md) |
| Typed composition | Required records, unions, pattern matching and abstractions must preserve their meaning through lowering; driver authors should not need to proliferate demo-specific workarounds | [HelloDISCO compiler findings](../../../../MCU/ST/STM32H747I-DISCO/HelloDISCO/docs/COMPILER_GAPS.md) |
| Cryptographic computation | Exact integer/bit semantics, deterministic layouts and bounded storage, assessed separately from the security of an algorithm or protocol | [Cryptography and bits](../../../../clef-lang-spec/spec/intrinsics-cryptography-bits.md) |
| Security properties | Explicit constant-time, secret-independence and secret-disposal contracts with evidence that optimization and target lowering preserve the selected property | [Credential authority draft](../../../../clef-lang-spec/spec/credential-authority.md) |
| Target capability and evidence | Operation-specific native/emulated/unavailable support for the actual part/configuration; retained positive and rejection cases, linked artifacts and explicit proof premises | [Numeric capabilities](../../../../clef-lang-spec/spec/numeric-selection.md), [conformance](../../../../clef-lang-spec/spec/conformance.md) |

The current numeric-selection chapter explicitly leaves construction selection
and parts of its platform schema unfinished. The rounding chapter distinguishes
MMIO access predicates from arithmetic capability resolution. Memory-region
documentation describes explicit arena operations as implemented while lifetime
parameter enforcement is planned. These distinctions matter before selecting
the final descriptor and ownership representations.

The cryptography intrinsic chapter primarily addresses WebSocket SHA-1/Base64
and bit/byte operations. It is not evidence of general native cryptography
readiness. The credential authority chapter is a scoped draft; neither it nor
a zeroization example establishes a complete constant-time or secure-erasure
contract. Record the intended property and required checking mechanism before
turning those examples into hardware or library commitments.

## Pickup condition

For each capability a future driver or DSP/crypto kernel actually needs, retain:

1. The agreed source semantics and any remaining specification decisions.
2. Small positive and negative cases through the real Clef/CCS/Composer path.
3. The selected representation, target lowering and linked-artifact evidence.
4. The proposition checked, its premises and the limits of that evidence.

Known-answer tests, reference arithmetic and host/target agreement are useful
checks; they do not by themselves establish every numeric or security property.
General device-transfer ownership, cache visibility and hardware protocol
acceptance remain explicit obligations when DISCO work resumes.

Before growing the board scaffold into code, review its provisional choices
against the completed language surface. Preserve the accepted HelloDISCO
artifacts as regression evidence rather than requiring future APIs to imitate
their current assembly boundaries or scalar lookup workarounds. Full UI
computation-expression or signal-runtime design belongs to its own language
work, not to a prerequisite for this documentation scaffold.
