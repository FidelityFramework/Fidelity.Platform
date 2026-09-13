# Hardware

`Silicon` owns reusable architecture, part, package and compute-block facts.
`Products` owns physical assemblies, wiring and documented revision applicability.
`VirtualMachines` owns machine interfaces presented to guests.

[Silicon/Radio](Silicon/Radio/README.md) inventories current radio availability
and reserves future discrete parts, including LoRa devices. Integrated radios
remain with their MCU owner; [protocols](../Protocols/Radio/README.md) and
environment bindings are separate from physical hardware identity.

A product references its constituent silicon. A selected profile combines those
resources with an execution environment and workload requirements. Repeated
physical instances must retain distinct resource identities; shared part
definitions do not grant access to every instance.

See the [repository structure](../PLATFORM_STRUCTURE.md) for current packages.
