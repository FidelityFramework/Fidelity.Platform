# Instruction machines

`eBPF/` and `cBPF/` own instruction-width descriptions independently of host
hardware. OS environment packages own admission gates, hooks, context and
loading requirements. A machine package grants no host capability and selects
no native ABI. These packages do not add a Composer compilation target.

See [admission contracts](../docs/ADMISSION_AND_SIDECARS.md).
