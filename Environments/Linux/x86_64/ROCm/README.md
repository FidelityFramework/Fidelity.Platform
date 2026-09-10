# Linux ROCm integration

[Fidelity.ROCm.fidproj](Fidelity.ROCm.fidproj) and its
[bridge](Fidelity.ROCm.Bridge.fidproj) own the generated `amdhip64` CPU-hosted
bindings. These sources previously sat beside the Strix Halo GPU scaffold.
They are not specific to that silicon inventory, and the GPU manifest no
longer includes them as accelerator source files.

The [RDNA 3.5 inventory](../../../../Hardware/Silicon/GPU/AMD/RDNA3_5/StrixHalo_iGPU/README.md)
remains a scaffold. Moving these bindings does not establish a tested GPU
kernel backend, device allocation policy or shared-memory composition.
