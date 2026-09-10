# Execution environments

Environment packages own ABI, startup, runtime services and native bindings.
They reference shared architecture facts. A profile selects a complete execution
target and any product-specific resources and workload budgets.

Implemented selections include Linux x86_64 bindings and the Cortex-M33
freestanding facts used by HelloBlinky. Freestanding x86_64 currently supplies
execution facts for a synthetic compiler-validation profile, without guest boot
or runtime mapping support. Windows, macOS, Android and iOS remain reserved.

Packaging and deployment are driven by Composer. An OCI image or application
bundle does not by itself establish a hardware or runtime contract.
