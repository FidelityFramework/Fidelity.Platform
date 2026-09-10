# Strix Halo and Arty: ThreeBody physical reference

[Profile.clef](Profile.clef) retains the existing list of intended target IDs.
The [manifest](Fidelity.Platform.fidproj) references GPU/NPU silicon scaffolds
and the Digilent product at their new paths.

[Handoffs.clef](Handoffs.clef) adds the intended Strix CPU/GPU shared allocation
and host-to-Arty Ethernet boundary, using the common BAREWire-backed handoff
contract. HIP and Linux eBPF facts remain in their environment packages. USB
programming/UART bring-up and the planned Ethernet workload link are distinct.
The independent [handoff manifest](Fidelity.Platform.Handoffs.fidproj) permits
checking this design without compiling generated native ROCm bindings.

This package is a catalogue of intended components. It selects no authoritative
execution description, and it does not compose CPU, GPU, NPU and FPGA memory
domains or launch several artifacts. The CPU ID is still a label without a
matching Strix Halo CPU product declaration. Select a concrete single-target
profile for an actual compilation.

Installed RAM is one physical pool with CPU/GPU-accessible views, not two memory
capacities to add. GPUVM/GTT limits, coherence, mapping and completion evidence
remain required facts. NPU sharing requires its own established contract.
The fixture's payload capacity is synthetic and its schema unresolved.
See [admission, UMA and timing boundaries](../../docs/ADMISSION_AND_SIDECARS.md).
