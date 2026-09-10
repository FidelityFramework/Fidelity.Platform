# Virtual machines

A machine package describes the interface presented to a guest: boot/discovery
requirements, memory domains, timers, interrupts and exposed device instances.
The CPU execution environment and workload selection are separate dependencies.

`Synthetic/RestrictedGuest64` supplies test inventory for the
[restricted guest profile](../../Profiles/RestrictedGuest64/README.md). It has no
hypervisor implementation, boot image, page tables or virtio driver. A production
machine package needs versioned provider documentation and executable acceptance.
