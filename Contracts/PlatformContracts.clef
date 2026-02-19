namespace Fidelity.Platform.Contracts

[<RequireQualifiedAccess>]
type SubstrateKind =
    | CPU
    | MCU
    | GPU
    | NPU
    | FPGA
    | CGRA

[<RequireQualifiedAccess>]
type PinDirection =
    | Input
    | Output
    | InOut

[<RequireQualifiedAccess>]
type ElectricalStandard =
    | LVCMOS33
    | LVCMOS18
    | LVDS
    | Unknown of string

[<CLIMutable>]
type PinEndpoint = {
    LogicalName: string
    PackagePin: string
    Direction: PinDirection
    Standard: ElectricalStandard
    Description: string option
}

[<CLIMutable>]
type ClockEndpoint = {
    Name: string
    FrequencyHz: int64
    PackagePin: string
    Standard: ElectricalStandard
    Description: string option
}

[<CLIMutable>]
type EndpointGroup = {
    Name: string
    Width: int
    Pins: PinEndpoint list
    Description: string option
}

[<CLIMutable>]
type UartEndpoint = {
    Name: string
    Tx: PinEndpoint
    Rx: PinEndpoint
    DefaultBaud: int
    SupportedBaudRates: int list
    Description: string option
}

[<CLIMutable>]
type PlatformDescriptor = {
    Id: string
    DisplayName: string
    Substrate: SubstrateKind
    Vendor: string
    Family: string
    Device: string
    Package: string
    SpeedGrade: string
    Clocks: ClockEndpoint list
    Groups: EndpointGroup list
    Uarts: UartEndpoint list
    DedicatedPins: PinEndpoint list
    Notes: string list
}

module PlatformDescriptor =
    let allPins (descriptor: PlatformDescriptor) : PinEndpoint list =
        let groupPins = descriptor.Groups |> List.collect (fun g -> g.Pins)
        let uartPins =
            descriptor.Uarts
            |> List.collect (fun u -> [ u.Tx; u.Rx ])
        groupPins @ uartPins @ descriptor.DedicatedPins

    let pinMap (descriptor: PlatformDescriptor) : Map<string, PinEndpoint> =
        allPins descriptor
        |> List.map (fun p -> p.LogicalName, p)
        |> Map.ofList

    let packagePinMap (descriptor: PlatformDescriptor) : Map<string, PinEndpoint> =
        allPins descriptor
        |> List.map (fun p -> p.PackagePin, p)
        |> Map.ofList
