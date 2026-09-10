# Linux XRT integration

[Fidelity.XRT.fidproj](Fidelity.XRT.fidproj) and its
[bridge](Fidelity.XRT.Bridge.fidproj) own the generated `xrt_coreutil` CPU-hosted
bindings. These sources previously sat beside the Strix Halo NPU scaffold.
Selecting a CPU library integration does not select an NPU execution target.

The [XDNA2 inventory](../../../../Hardware/Silicon/NPU/AMD/XDNA2/StrixHalo_NPU/README.md)
remains a scaffold. Queue, memory-domain, topology and runtime compatibility
requirements still need concrete declarations and consumers.
