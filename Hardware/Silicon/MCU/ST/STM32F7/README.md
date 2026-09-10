# STM32F7 family scaffold

[Family.clef](Family.clef) records vendor and family identity only. It declares
no selected core, part/package, memory capacity or peripheral address.

The staged [STM32F750x8 datasheet](docs/stm32f750n8.pdf) is candidate source
material, DS12535 Rev 2 (August 2025). Its SHA-256 is
`9e604a901e41aa48926b14dd64f76bc0893b5dec5f647e9f6bbaa569fd0e7f39`.
It does not establish which MCU variant is populated on a particular Meadow.

The [Meadow F7 product scaffold](../../../../Products/WildernessLabs/MeadowF7/README.md)
references this family. Concrete part applicability, register references,
product revision and pin routing remain unverified.
