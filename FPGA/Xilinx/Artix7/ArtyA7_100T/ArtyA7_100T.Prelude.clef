namespace Fidelity.Platform.FPGA.Xilinx.Artix7.ArtyA7_100T.Prelude

open Fidelity.Platform.Contracts
open Fidelity.Platform.FPGA.Xilinx.Artix7.ArtyA7_100T.Bindings

module Endpoints =
    let clock: ClockEndpoint = Pins.sysClk

    let leds: PinEndpoint list = [
        Pins.led0
        Pins.led1
        Pins.led2
        Pins.led3
    ]

    let rgbLeds: PinEndpoint list list = [
        [ Pins.ledR0; Pins.ledG0; Pins.ledB0 ]
        [ Pins.ledR1; Pins.ledG1; Pins.ledB1 ]
        [ Pins.ledR2; Pins.ledG2; Pins.ledB2 ]
        [ Pins.ledR3; Pins.ledG3; Pins.ledB3 ]
    ]

    let buttons: PinEndpoint list = [ Pins.btn0; Pins.btn1; Pins.btn2; Pins.btn3 ]
    let switches: PinEndpoint list = [ Pins.sw0; Pins.sw1; Pins.sw2; Pins.sw3 ]
    let chipKitDigitalGpio: PinEndpoint list = Pins.chipKitDigitalGpioPins
    let usbUart: UartEndpoint = Uart.usbUart

module Package =
    let descriptor: PlatformDescriptor = Platform.descriptor
    let xdcConstraints: string list = Platform.xdcConstraints

    let pinMap: Map<string, PinEndpoint> = PlatformDescriptor.pinMap descriptor
    let packagePinMap: Map<string, PinEndpoint> = PlatformDescriptor.packagePinMap descriptor

    let tryLogicalPin (logicalName: string) : PinEndpoint option =
        Map.tryFind logicalName pinMap

    let tryPackagePin (packagePin: string) : PinEndpoint option =
        Map.tryFind packagePin packagePinMap
