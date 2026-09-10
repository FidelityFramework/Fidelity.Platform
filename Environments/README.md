# Execution environments

Environment packages own ABI, startup, runtime services and native bindings.
They reference shared architecture facts. A profile selects a complete execution
target and any product-specific resources and workload budgets.

Implemented selections include Linux x86_64 bindings and the Cortex-M33
freestanding facts used by HelloBlinky. Freestanding x86_64 currently supplies
execution facts for a synthetic compiler-validation profile, without guest boot
or runtime mapping support. Linux and Windows now have eBPF reference contracts;
macOS has classic BPF and Metal reference contracts. These do not establish
native Windows/macOS targets. Android and iOS remain reserved.

Instruction-host contracts live directly under their OS when independent of a
native ABI. Native bindings remain architecture-specific. See
[admission and accelerator handoffs](../docs/ADMISSION_AND_SIDECARS.md).

Packaging and deployment are driven by Composer. An OCI image or application
bundle does not by itself establish a hardware or runtime contract.
