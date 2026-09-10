# Strix Halo XDNA2 inventory scaffold

[Platform.clef](Platform.clef) preserves the existing identification record.
It has no core, memory inventory, tile topology or channel map and is not an
accepted NPU execution target. The empty reset collection is explicit.

The [Linux XRT bindings](../../../../../../Environments/Linux/x86_64/XRT/README.md)
have their own CPU compilation manifests. Shared Strix Halo CPU/GPU/NPU memory
and physical instance identity still need one authoritative SoC topology;
catalogue entries do not imply independent ownership of shared RAM.
