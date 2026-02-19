namespace Fidelity.Platform.MCU.Renesas.RA6M5.EK_RA6M5

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
        Id = "mcu-renesas-ra6m5-ek-ra6m5"
        DisplayName = "Renesas EK-RA6M5"
        Substrate = SubstrateKind.MCU
        Vendor = "Renesas"
        Family = "RA6M5"
        Device = "EK-RA6M5"
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
