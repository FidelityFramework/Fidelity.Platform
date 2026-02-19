namespace Fidelity.Platform.FPGA.Xilinx.Artix7.ArtyA7_100T.Bindings

open Fidelity.Platform.Contracts

module Pins =
    let sysClk: ClockEndpoint = {
        Name = "sys_clk"
        FrequencyHz = 100_000_000L
        PackagePin = "E3"
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "100 MHz on-board oscillator"
    }

    let led0: PinEndpoint = {
        LogicalName = "led[0]"
        PackagePin = "H5"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Green LED0, active-high"
    }

    let led1: PinEndpoint = {
        LogicalName = "led[1]"
        PackagePin = "J5"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Green LED1, active-high"
    }

    let led2: PinEndpoint = {
        LogicalName = "led[2]"
        PackagePin = "T9"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Green LED2, active-high"
    }

    let led3: PinEndpoint = {
        LogicalName = "led[3]"
        PackagePin = "T10"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Green LED3, active-high"
    }

    let ledR0: PinEndpoint = {
        LogicalName = "rgb[0].r"
        PackagePin = "G6"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED0 red channel"
    }

    let ledG0: PinEndpoint = {
        LogicalName = "rgb[0].g"
        PackagePin = "F6"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED0 green channel"
    }

    let ledB0: PinEndpoint = {
        LogicalName = "rgb[0].b"
        PackagePin = "E1"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED0 blue channel"
    }

    let ledR1: PinEndpoint = {
        LogicalName = "rgb[1].r"
        PackagePin = "G3"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED1 red channel"
    }

    let ledG1: PinEndpoint = {
        LogicalName = "rgb[1].g"
        PackagePin = "J4"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED1 green channel"
    }

    let ledB1: PinEndpoint = {
        LogicalName = "rgb[1].b"
        PackagePin = "G4"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED1 blue channel"
    }

    let ledR2: PinEndpoint = {
        LogicalName = "rgb[2].r"
        PackagePin = "J3"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED2 red channel"
    }

    let ledG2: PinEndpoint = {
        LogicalName = "rgb[2].g"
        PackagePin = "J2"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED2 green channel"
    }

    let ledB2: PinEndpoint = {
        LogicalName = "rgb[2].b"
        PackagePin = "H4"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED2 blue channel"
    }

    let ledR3: PinEndpoint = {
        LogicalName = "rgb[3].r"
        PackagePin = "K1"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED3 red channel"
    }

    let ledG3: PinEndpoint = {
        LogicalName = "rgb[3].g"
        PackagePin = "H6"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED3 green channel"
    }

    let ledB3: PinEndpoint = {
        LogicalName = "rgb[3].b"
        PackagePin = "K2"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "RGB LED3 blue channel"
    }

    let btn0: PinEndpoint = {
        LogicalName = "btn[0]"
        PackagePin = "D9"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Push button 0 (momentary, active-high)"
    }

    let btn1: PinEndpoint = {
        LogicalName = "btn[1]"
        PackagePin = "C9"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Push button 1 (momentary, active-high)"
    }

    let btn2: PinEndpoint = {
        LogicalName = "btn[2]"
        PackagePin = "B9"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Push button 2 (momentary, active-high)"
    }

    let btn3: PinEndpoint = {
        LogicalName = "btn[3]"
        PackagePin = "B8"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Push button 3 (momentary, active-high)"
    }

    let sw0: PinEndpoint = {
        LogicalName = "sw[0]"
        PackagePin = "A8"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Slide switch 0 (latching)"
    }

    let sw1: PinEndpoint = {
        LogicalName = "sw[1]"
        PackagePin = "C11"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Slide switch 1 (latching)"
    }

    let sw2: PinEndpoint = {
        LogicalName = "sw[2]"
        PackagePin = "C10"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Slide switch 2 (latching)"
    }

    let sw3: PinEndpoint = {
        LogicalName = "sw[3]"
        PackagePin = "A10"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "Slide switch 3 (latching)"
    }

    // NOTE: Migration docs map D10/A9 as TX/RX respectively in generated artifacts.
    // Keep this canonical package aligned with that mapping.
    let uartTx: PinEndpoint = {
        LogicalName = "uart_tx"
        PackagePin = "D10"
        Direction = PinDirection.Output
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "USB-UART TX from FPGA"
    }

    let uartRx: PinEndpoint = {
        LogicalName = "uart_rx"
        PackagePin = "A9"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.LVCMOS33
        Description = Some "USB-UART RX to FPGA"
    }

    // Dedicated reset button is wired to dedicated FPGA reset infrastructure in docs.
    let resetButton: PinEndpoint = {
        LogicalName = "reset_n"
        PackagePin = "DEDICATED"
        Direction = PinDirection.Input
        Standard = ElectricalStandard.Unknown "Dedicated reset network"
        Description = Some "Active-low board reset button (not exposed as user I/O pin in this package)"
    }

    // ChipKit/Arduino digital GPIO bank from Digilent Arty-A7-100 master XDC.
    let chipKitDigitalGpioPins: PinEndpoint list =
        [
            "ck_io0", "V15"
            "ck_io1", "U16"
            "ck_io2", "P14"
            "ck_io3", "T11"
            "ck_io4", "R12"
            "ck_io5", "T14"
            "ck_io6", "T15"
            "ck_io7", "T16"
            "ck_io8", "N15"
            "ck_io9", "M16"
            "ck_io10", "V17"
            "ck_io11", "U18"
            "ck_io12", "R17"
            "ck_io13", "P17"
            "ck_io26", "U11"
            "ck_io27", "V16"
            "ck_io28", "M13"
            "ck_io29", "R10"
            "ck_io30", "R11"
            "ck_io31", "R13"
            "ck_io32", "R15"
            "ck_io33", "P15"
            "ck_io34", "R16"
            "ck_io35", "N16"
            "ck_io36", "N14"
            "ck_io37", "U17"
            "ck_io38", "T18"
            "ck_io39", "R18"
            "ck_io40", "P18"
            "ck_io41", "N17"
            "ck_ioa", "M17"
            "ck_rst", "C2"
        ]
        |> List.map (fun (name, pin) -> {
            LogicalName = name
            PackagePin = pin
            Direction = PinDirection.InOut
            Standard = ElectricalStandard.LVCMOS33
            Description = Some "ChipKit/Arduino header GPIO endpoint"
        })

module Groups =
    open Pins

    let greenLeds: EndpointGroup = {
        Name = "led"
        Width = 4
        Pins = [ led0; led1; led2; led3 ]
        Description = Some "Discrete green user LEDs"
    }

    let rgb0: EndpointGroup = {
        Name = "rgb[0]"
        Width = 3
        Pins = [ ledR0; ledG0; ledB0 ]
        Description = Some "RGB LED 0 channels"
    }

    let rgb1: EndpointGroup = {
        Name = "rgb[1]"
        Width = 3
        Pins = [ ledR1; ledG1; ledB1 ]
        Description = Some "RGB LED 1 channels"
    }

    let rgb2: EndpointGroup = {
        Name = "rgb[2]"
        Width = 3
        Pins = [ ledR2; ledG2; ledB2 ]
        Description = Some "RGB LED 2 channels"
    }

    let rgb3: EndpointGroup = {
        Name = "rgb[3]"
        Width = 3
        Pins = [ ledR3; ledG3; ledB3 ]
        Description = Some "RGB LED 3 channels"
    }

    let buttons: EndpointGroup = {
        Name = "btn"
        Width = 4
        Pins = [ btn0; btn1; btn2; btn3 ]
        Description = Some "Momentary push buttons"
    }

    let switches: EndpointGroup = {
        Name = "sw"
        Width = 4
        Pins = [ sw0; sw1; sw2; sw3 ]
        Description = Some "Latching slide switches"
    }

    let chipKitDigitalGpio: EndpointGroup = {
        Name = "ck_io"
        Width = List.length chipKitDigitalGpioPins
        Pins = chipKitDigitalGpioPins
        Description = Some "ChipKit/Arduino digital GPIO endpoints exposed by the board headers"
    }

module Uart =
    open Pins

    let usbUart: UartEndpoint = {
        Name = "usb_uart"
        Tx = uartTx
        Rx = uartRx
        DefaultBaud = 115200
        SupportedBaudRates = [ 9600; 19200; 38400; 57600; 115200 ]
        Description = Some "On-board USB-UART bridge channel"
    }

module Platform =
    open Pins
    open Groups

    let descriptor: PlatformDescriptor = {
        Id = "fpga-xilinx-artix7-arty-a7-100t"
        DisplayName = "Digilent Arty A7-100T (Rev D/E)"
        Substrate = SubstrateKind.FPGA
        Vendor = "Xilinx"
        Family = "Artix-7"
        Device = "XC7A100T"
        Package = "CSG324"
        SpeedGrade = "-1"
        Clocks = [ sysClk ]
        Groups = [ greenLeds; rgb0; rgb1; rgb2; rgb3; buttons; switches; chipKitDigitalGpio ]
        Uarts = [ Uart.usbUart ]
        DedicatedPins = [ resetButton ]
        Notes = [
            "All user I/O in this package is LVCMOS33."
            "Buttons are modeled as active-high momentary inputs."
            "Switches are modeled as active-high latching inputs."
            "LED outputs are active-high."
            "UART mapping follows current HelloBlinky migration artifacts: uart_tx=D10, uart_rx=A9."
            "Reset button is represented semantically; pin is marked dedicated because it is not modeled as user GPIO."
            "ChipKit digital GPIO endpoints are sourced from Digilent Arty-A7-100 Master XDC (ck_io*, ck_ioa, ck_rst)."
        ]
    }

    let xdcConstraints: string list =
        [
            "set_property -dict { PACKAGE_PIN E3  IOSTANDARD LVCMOS33 } [get_ports { sys_clk }]"
            "create_clock -add -name sys_clk_pin -period 10.00 -waveform {0 5} [get_ports { sys_clk }]"
            "set_property -dict { PACKAGE_PIN H5  IOSTANDARD LVCMOS33 } [get_ports { led[0] }]"
            "set_property -dict { PACKAGE_PIN J5  IOSTANDARD LVCMOS33 } [get_ports { led[1] }]"
            "set_property -dict { PACKAGE_PIN T9  IOSTANDARD LVCMOS33 } [get_ports { led[2] }]"
            "set_property -dict { PACKAGE_PIN T10 IOSTANDARD LVCMOS33 } [get_ports { led[3] }]"
            "set_property -dict { PACKAGE_PIN G6  IOSTANDARD LVCMOS33 } [get_ports { led0_r }]"
            "set_property -dict { PACKAGE_PIN F6  IOSTANDARD LVCMOS33 } [get_ports { led0_g }]"
            "set_property -dict { PACKAGE_PIN E1  IOSTANDARD LVCMOS33 } [get_ports { led0_b }]"
            "set_property -dict { PACKAGE_PIN G3  IOSTANDARD LVCMOS33 } [get_ports { led1_r }]"
            "set_property -dict { PACKAGE_PIN J4  IOSTANDARD LVCMOS33 } [get_ports { led1_g }]"
            "set_property -dict { PACKAGE_PIN G4  IOSTANDARD LVCMOS33 } [get_ports { led1_b }]"
            "set_property -dict { PACKAGE_PIN J3  IOSTANDARD LVCMOS33 } [get_ports { led2_r }]"
            "set_property -dict { PACKAGE_PIN J2  IOSTANDARD LVCMOS33 } [get_ports { led2_g }]"
            "set_property -dict { PACKAGE_PIN H4  IOSTANDARD LVCMOS33 } [get_ports { led2_b }]"
            "set_property -dict { PACKAGE_PIN K1  IOSTANDARD LVCMOS33 } [get_ports { led3_r }]"
            "set_property -dict { PACKAGE_PIN H6  IOSTANDARD LVCMOS33 } [get_ports { led3_g }]"
            "set_property -dict { PACKAGE_PIN K2  IOSTANDARD LVCMOS33 } [get_ports { led3_b }]"
            "set_property -dict { PACKAGE_PIN D9  IOSTANDARD LVCMOS33 } [get_ports { btn[0] }]"
            "set_property -dict { PACKAGE_PIN C9  IOSTANDARD LVCMOS33 } [get_ports { btn[1] }]"
            "set_property -dict { PACKAGE_PIN B9  IOSTANDARD LVCMOS33 } [get_ports { btn[2] }]"
            "set_property -dict { PACKAGE_PIN B8  IOSTANDARD LVCMOS33 } [get_ports { btn[3] }]"
            "set_property -dict { PACKAGE_PIN A8  IOSTANDARD LVCMOS33 } [get_ports { sw[0] }]"
            "set_property -dict { PACKAGE_PIN C11 IOSTANDARD LVCMOS33 } [get_ports { sw[1] }]"
            "set_property -dict { PACKAGE_PIN C10 IOSTANDARD LVCMOS33 } [get_ports { sw[2] }]"
            "set_property -dict { PACKAGE_PIN A10 IOSTANDARD LVCMOS33 } [get_ports { sw[3] }]"
            "set_property -dict { PACKAGE_PIN D10 IOSTANDARD LVCMOS33 } [get_ports { uart_tx }]"
            "set_property -dict { PACKAGE_PIN A9  IOSTANDARD LVCMOS33 } [get_ports { uart_rx }]"
        ]
        @ (
            Pins.chipKitDigitalGpioPins
            |> List.map (fun pin ->
                sprintf "set_property -dict { PACKAGE_PIN %-3s IOSTANDARD LVCMOS33 } [get_ports { %s }]" pin.PackagePin pin.LogicalName
            )
        )
