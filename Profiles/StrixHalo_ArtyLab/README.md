# Strix Halo and Arty lab catalogue

[Profile.clef](Profile.clef) retains the existing list of intended target IDs.
The [manifest](Fidelity.Platform.fidproj) references GPU/NPU silicon scaffolds
and the Digilent product at their new paths.

This package is a catalogue of intended components. It selects no authoritative
execution description, and it does not compose CPU, GPU, NPU and FPGA memory
domains or launch several artifacts. The CPU ID is still a label without a
matching Strix Halo CPU product declaration. Select a concrete single-target
profile for an actual compilation.
