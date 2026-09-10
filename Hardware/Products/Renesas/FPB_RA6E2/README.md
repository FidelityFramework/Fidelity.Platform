# Renesas FPB-RA6E2 references

Reference-only product source pack. No Fidelity platform manifest, silicon map,
driver implementation or hardware acceptance is supplied here.

The staged [user manual](docs/REN_r20ut5161eg0100_fpb-ra6e2_user_manual_MAT_20230621.pdf)
identifies **FPB-RA6E2 v1**, the RA6E2 Fast Prototyping Board, document
R20UT5161EG0100, revision 1.00, June 2023. This document identity does not establish
the revision or population of any physically observed board.

The pack also contains the [FSP flyer](docs/fsp-flyer-r11pm0012eu0100.pdf) and a
[vendor quickstart](docs/quickstart_fpb_ra6e2_ep) whose readme names FSP 4.4.0,
e² studio 2023-04 and Arm GCC 10.3-2021.10. Those are the example's recorded
requirements, not a Composer toolchain selection. The quickstart and its C source
remain reference assets; Fidelity does not build or deploy them.

The source pack was relocated from the former MCU catalogue with every file's
bytes preserved. Add executable support only through documented silicon,
product, execution and profile declarations with their corresponding checks.
