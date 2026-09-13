# STM32H747I-DISCO HelloDISCO selection

The initial executable selection for
[HelloDISCO](../../../MCU/ST/STM32H747I-DISCO/HelloDISCO/README.md): M7 code in
internal flash bank 1, data/stack in DTCM, four GPIOI LEDs, five GPIOK joystick
contacts, and SysTick. Original part/core memory declarations retain identity;
the application owns mappings and grants.

The intended demo is a landscape Clef logo with joystick-controlled logo color
and LED blinking. This first image selects the LED/joystick stage. The pure
scene is authored and checked in the application, while its LCD hardware path
still needs accepted display clocks, panel initialization and memory placement.
Touch is deferred. See the application's current build/test status and plan.

This profile selects no DMA, SDRAM, LCD, audio, MPU/cache change or M4 workload.
Those additions must extend the selected declarations and their consumers,
not reuse permissive raw-address code. Image validity does not establish boot
options, clock state, physical wiring, stack bounds or real-time deadlines.
