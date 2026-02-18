namespace Fidelity.Platform.CPU.Linux.X86_64.StrixHalo

open Fidelity.Platform.Contracts

module Platform =
    let descriptor: PlatformDescriptor = {
        Id = "cpu-linux-x86_64-strix-halo"
        DisplayName = "Strix Halo CPU (Linux x86-64)"
        Substrate = SubstrateKind.CPU
        Vendor = "AMD"
        Family = "x86_64"
        Device = "Strix Halo CPU Complex"
        Package = "Host"
        SpeedGrade = "n/a"
        Clocks = []
        Groups = []
        Uarts = []
        DedicatedPins = []
        Notes = [
            "Scaffolding package for substrate-first Fidelity.Platform organization."
            "Use Linux_x86_64 package for current executable platform quotations."
        ]
    }
