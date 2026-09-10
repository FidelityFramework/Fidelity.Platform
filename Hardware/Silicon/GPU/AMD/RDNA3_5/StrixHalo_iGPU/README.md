# Strix Halo RDNA 3.5 inventory scaffold

[Platform.clef](Platform.clef) preserves the existing identification record.
It has no core, memory inventory, queue or endpoint map and is not an accepted
GPU execution target. The empty reset collection is explicit.

The [Linux ROCm bindings](../../../../../../Environments/Linux/x86_64/ROCm/README.md)
have their own CPU compilation manifests. They are no longer included in this
silicon package. Shared Strix Halo CPU/GPU/NPU memory and physical instance
identity still need one authoritative SoC topology; these catalogue names do
not imply separate ownership of shared RAM.
