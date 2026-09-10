# HelloArty profile

The [manifest](Fidelity.Platform.fidproj) selects the BAREWire description in
[Description.clef](Description.clef), with the existing platform ID and public
declaration namespace preserved. It depends on the
[Digilent product](../../Hardware/Products/Digilent/ArtyA7_100T/README.md), whose
Contracts descriptor and pin/Prelude APIs remain operative for Composer.

This profile owns the `ArtyReport` selection, 32-byte transmit-buffer ceiling
and UART transport. It explicitly chooses the product API's existing 115200
baud default by reference. Product wiring and lifecycle, together with the
referenced Xilinx BRAM/register declarations, form one selected description.

This is a single FPGA execution selection. The separate Strix Halo lab catalogue
does not turn CPU/GPU/NPU dependencies into a heterogeneous execution context.

## Migration acceptance — 2026-09-10

Direct Composer compilation of HelloArty passed through CIRCT to fresh
SystemVerilog and XDC. Composer verified 25 HDL ports against 25 constraints;
an independent F# check matched every selected package pin and electrical
standard to the staged Digilent master XDC. The application currently returns
`UartReport = ValueNone`, so this verifies clock, switches, buttons and LEDs;
it does not establish a working UART transmitter.

The generated XDC constrains the source-backed 100 MHz oscillator at 10 ns.
HelloArty's existing 25 MHz compilation setting still controls the structural
timing heuristic. Those are separate existing settings, not evidence of a
generated clock divider or of timing closure at 100 MHz. No Vivado implementation,
new bitstream, programming or physical-board acceptance was performed here.
