# EK-RA6M5 Source Pack

[SOURCE_MANIFEST.md](SOURCE_MANIFEST.md) inventories the staged and referenced Renesas
artifacts with revisions, hashes, scope, and the locally supplied MP design package. The initial register
review uses MCU hardware manual 1.50 and board manual 1.01; locally staged MCU 1.40
PDFs are retained as prior revisions.

[HELLOBLINKY_HARDWARE.md](HELLOBLINKY_HARDWARE.md) pins the reset/MMIO/vector subset
needed by the first application. The complete platform package remains broader than
that subset. Each later endpoint family should extend the source manifest and carry
its own implementation and board acceptance evidence.
[IO_MAP.md](IO_MAP.md) describes all package/board connections, interface sharing,
manual corrections and credential bring-up implications. [CONNECTOR_MAP.md](CONNECTOR_MAP.md)
lists all 355 connector contacts, including all 160 native-header contacts.
The typed declarations are checked against the pinned vendor netlist by the .NET
project under `../tests/`; no vendor assets are required to build HelloBlinky.
