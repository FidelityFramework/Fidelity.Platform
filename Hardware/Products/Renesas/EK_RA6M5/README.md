# Renesas EK-RA6M5 product

This package owns the EK-RA6M5 v1 MP connectivity and references the identified
[R7FA6M5BH3CFC silicon/package](../../../Silicon/MCU/Renesas/RA6M5/R7FA6M5BH3CFC).
It contains all 1,149 terminal facts, 355 connector contacts and 38 trace-link
defaults. [Description.clef](Description.clef) names the LED and switch wiring;
button roles, PWM duty, clock selection and register grants belong to applications.

The source is D017572_04 schematic issue 3.0 and the supplied MP design package.
The physical PCB artwork revision and strap/component population have not been
independently confirmed. Issue 3.0 is a document revision; no physical revision
folder is inferred from it. See the [source manifest](docs/SOURCE_MANIFEST.md).

The package supplies inventory through an explicit silicon dependency. It does
not export a selected PlatformDescription or an active DeviceAccessPlan.
[EK_RA6M5_HelloBlinky](../../../../Profiles/EK_RA6M5_HelloBlinky) composes the first
accepted image with the freestanding execution environment.

Vendor reference assets remain under [docs](docs/README.md), with their original
bytes and provenance. Their C/FSP examples are reference material and are not
compiled or invoked by the Fidelity build.
