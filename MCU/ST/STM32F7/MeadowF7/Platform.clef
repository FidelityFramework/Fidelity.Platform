namespace Fidelity.Platform.MCU.ST.STM32F7.MeadowF7

open Fidelity.Platform.Contracts

module Platform =
    // Endpoint families to populate once authoritative docs are staged in docs/.
    let plannedEndpointFamilies: string list = [
        "GPIO"
        "UART"
        "I2C"
        "SPI"
        "PWM"
        "ADC"
        "DAC"
        "USB"
        "Timers"
        "Interrupts"
    ]

    let descriptor: PlatformDescriptor = {
        Id = "mcu-st-stm32f7-meadowf7"
        DisplayName = "Wilderness Labs Meadow F7 (STM32F7)"
        Substrate = SubstrateKind.MCU
        Vendor = "STMicroelectronics / Wilderness Labs"
        Family = "STM32F7"
        Device = "Meadow F7"
        Package = "Board"
        SpeedGrade = "n/a"
        Clocks = []
        Groups = []
        Uarts = []
        DedicatedPins = []
        Notes = [
            "Scaffold only: endpoint maps intentionally empty until board source pack is present."
            "Populate from authoritative PDFs/manuals/schematics in docs/ and record citations in SOURCE_MANIFEST.md."
            "Keep platform binding names stable once introduced; avoid compatibility aliases."
        ]
    }
