# Apple Silicon / Metal / FPGA reference

Synthetic CPU/GPU shared-buffer and host/FPGA Ethernet handoffs using the same
contract as [Strix Halo / Arty](../StrixHalo_ArtyLab). No Mac model, allocation,
FPGA board, wire layout or deadline has been selected. macOS classic BPF is the
packet interface; Metal owns GPU resource access. Neither extends UMA to the FPGA.

See [the contract and validation boundary](../../docs/ADMISSION_AND_SIDECARS.md).
