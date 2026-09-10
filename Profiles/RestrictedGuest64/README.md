# Restricted 64-bit guest design probe

This is a synthetic compiler-validation profile. It declares two available
device windows in guest physical space, with 64-bit pointers and 32-bit register
transactions. The Composer device-access tests grant only selected transport
registers through a distinct CPU virtual mapping above 4 GiB. They reject access
to the ungranted management register and reject an unresolved runtime mapping.

The addresses are fixture data. This package does not boot a VM or implement
virtio. A real guest must establish its mapping and memory attributes, interrupt
routes, DMA visibility and ordering before a transport driver can use it.
