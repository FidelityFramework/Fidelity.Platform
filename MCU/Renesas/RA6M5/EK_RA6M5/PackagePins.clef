namespace Fidelity.Platform.MCU.Renesas.RA6M5.EK_RA6M5

/// Physical LQFP176 pin capabilities, transcribed from DS-1.50 Table 1.16.
/// These are alternatives, not simultaneous assignments or PSEL register values.
type PackagePin = {
    Number: int
    Port: string
    System: string
    ExternalBus: string
    Interrupt: string
    Serial: string
    Timer: string
    Analog: string
    Touch: string
}

module PackagePins =
    let source = "DS-1.50 Table 1.16, LQFP176 column"
    let pins: PackagePin array = [|
        { Number = 1; Port = "P400"; System = ""; ExternalBus = ""; Interrupt = "IRQ0"; Serial = "SCK4/SCK7/SCL0_A/AUDIO_CLK/ET0_WOL/ET0_WOL"; Timer = "GTIOC6A/AGTIO1"; Analog = "ADTRG1"; Touch = "" }
        { Number = 2; Port = "P401"; System = ""; ExternalBus = ""; Interrupt = "IRQ5-DS"; Serial = "CTS4_RTS4/TXD7/SDA0_A/CTX0/ET0_MDC/ET0_MDC"; Timer = "GTETRGA/GTIOC6B"; Analog = ""; Touch = "" }
        { Number = 3; Port = "P402"; System = "CACREF"; ExternalBus = ""; Interrupt = "IRQ4-DS"; Serial = "CTS4/RXD7/CRX0/AUDIO_CLK/ET0_MDIO/ET0_MDIO"; Timer = "AGTIO0/AGTIO1/AGTIO2/AGTIO3/RTCIC0"; Analog = ""; Touch = "" }
        { Number = 4; Port = "P403"; System = ""; ExternalBus = ""; Interrupt = "IRQ14-DS"; Serial = "CTS7_RTS7/SSIBCK0_A/ET0_LINKSTA/ET0_LINKSTA"; Timer = "GTIOC3A/AGTIO0/AGTIO1/AGTIO2/AGTIO3/RTCIC1"; Analog = ""; Touch = "" }
        { Number = 5; Port = "P404"; System = ""; ExternalBus = ""; Interrupt = "IRQ15-DS"; Serial = "CTS7/SSILRCK0_A/ET0_EXOUT/ET0_EXOUT"; Timer = "GTIOC3B/AGTIO0_G/AGTIO1/AGTIO2/AGTIO3/RTCIC2"; Analog = ""; Touch = "" }
        { Number = 6; Port = "P405"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "SSITXD0_A/ET0_TX_EN/RMII0_TXD_EN_B"; Timer = "GTIOC1A"; Analog = ""; Touch = "" }
        { Number = 7; Port = "P406"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "SSLA3_C/SSIRXD0_A/ET0_RX_ER/RMII0_TXD1_B"; Timer = "GTIOC1B/AGTO5"; Analog = ""; Touch = "" }
        { Number = 8; Port = "P700"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "MISOA_C/ET0_ETXD1/RMII0_TXD0_B"; Timer = "GTIOC5A/AGTO4"; Analog = ""; Touch = "" }
        { Number = 9; Port = "P701"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "MOSIA_C/ET0_ETXD0/REF50CK0_B"; Timer = "GTIOC5B/AGTO3"; Analog = ""; Touch = "" }
        { Number = 10; Port = "P702"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "RSPCKA_C/ET0_ERXD1/RMII0_RXD0_B"; Timer = "GTIOC6A/AGTO2"; Analog = ""; Touch = "" }
        { Number = 11; Port = "P703"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "SSLA0_C/ET0_ERXD0/RMII0_RXD1_B"; Timer = "GTIOC6B/AGTO1"; Analog = ""; Touch = "" }
        { Number = 12; Port = "P704"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "SSLA1_C/CTX0/ET0_RX_CLK/RMII0_RX_ER_B"; Timer = "AGTO0"; Analog = ""; Touch = "" }
        { Number = 13; Port = "P705"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "CTS3/SSLA2_C/CRX0/ET0_CRS/RMII0_CRS_DV_B"; Timer = "AGTIO0"; Analog = ""; Touch = "" }
        { Number = 14; Port = "P706"; System = ""; ExternalBus = ""; Interrupt = "IRQ7"; Serial = "USBHS_OVRCURB/RXD3_B"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 15; Port = "P707"; System = ""; ExternalBus = ""; Interrupt = "IRQ8"; Serial = "USBHS_OVRCURA/TXD3_B"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 16; Port = "PB00"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "USBHS_VBUSEN/SCK3_B"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 17; Port = "PB01"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "USBHS_VBUS/CTS_RTS3_B"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 18; Port = ""; System = "VBATT"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 19; Port = ""; System = "VCL0"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 20; Port = ""; System = "XCIN"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 21; Port = ""; System = "XCOUT"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 22; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 23; Port = "P213"; System = "XTAL"; ExternalBus = ""; Interrupt = "IRQ2"; Serial = "TXD1"; Timer = "GTETRGC/GTIOC0A/AGTEE2"; Analog = "ADTRG1"; Touch = "" }
        { Number = 24; Port = "P212"; System = "EXTAL"; ExternalBus = ""; Interrupt = "IRQ3"; Serial = "RXD1"; Timer = "GTETRGD/GTIOC0B/AGTEE1"; Analog = ""; Touch = "" }
        { Number = 25; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 26; Port = ""; System = "AVCC_USBHS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 27; Port = ""; System = "USBHS_RREF"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 28; Port = ""; System = "AVSS_USBHS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 29; Port = ""; System = "PVSS_USBHS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 30; Port = ""; System = "VSS2_USBHS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 31; Port = ""; System = "USBHS_DM"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 32; Port = ""; System = "USBHS_DP"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 33; Port = ""; System = "VSS1_USBHS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 34; Port = ""; System = "VCC_USBHS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 35; Port = "P708"; System = "CACREF"; ExternalBus = ""; Interrupt = "IRQ11"; Serial = "RXD1/SSLB3_B/AUDIO_CLK/ET0_ETXD3/CECIO"; Timer = ""; Analog = ""; Touch = "TS12" }
        { Number = 36; Port = "P415"; System = ""; ExternalBus = ""; Interrupt = "IRQ8"; Serial = "SCL2_B/SSLB2_B/USB_VBUSEN/SD0CD/ET0_TX_EN/RMII0_TXD_EN_A"; Timer = "GTIOC0A/AGTIO4"; Analog = ""; Touch = "TS11" }
        { Number = 37; Port = "P414"; System = ""; ExternalBus = ""; Interrupt = "IRQ9"; Serial = "SDA2_B/CTS0/SSLB1_B/SD0WP/ET0_RX_ER/RMII0_TXD1_A"; Timer = "GTIOC0B/AGTIO5"; Analog = ""; Touch = "TS10" }
        { Number = 38; Port = "P413"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "CTS0_RTS0/SSLB0_B/SD0CLK_A/ET0_ETXD1/RMII0_TXD0_A"; Timer = "GTOUUP/AGTEE3"; Analog = ""; Touch = "TS09" }
        { Number = 39; Port = "P412"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "SCK0/CTS3/RSPCKB_B/SD0CMD_A/ET0_ETXD0/REF50CK0_A"; Timer = "GTOULO/AGTEE1"; Analog = ""; Touch = "TS08" }
        { Number = 40; Port = "P411"; System = ""; ExternalBus = ""; Interrupt = "IRQ4"; Serial = "TXD0/CTS3_RTS3/MOSIB_B/SD0DAT0_A/ET0_ERXD1/RMII0_RXD0_A"; Timer = "GTOVUP/GTIOC9A/AGTOA1"; Analog = ""; Touch = "TS07" }
        { Number = 41; Port = "P410"; System = ""; ExternalBus = ""; Interrupt = "IRQ5"; Serial = "RXD0/SCL2_A/SCK3/MISOB_B/SD0DAT1_A/ET0_ERXD0/RMII0_RXD1_A"; Timer = "GTOVLO/GTIOC9B/AGTOB1"; Analog = ""; Touch = "TS06" }
        { Number = 42; Port = "P409"; System = ""; ExternalBus = ""; Interrupt = "IRQ6"; Serial = "TXD3/SDA2_A/USB_EXICEN/USBHS_EXICEN/ET0_RX_CLK/RMII0_RX_ER_A"; Timer = "GTOWUP/AGTOA2"; Analog = ""; Touch = "TS05" }
        { Number = 43; Port = "P408"; System = ""; ExternalBus = ""; Interrupt = "IRQ7"; Serial = "CTS4/RXD3/SCL0_B/USB_ID/USBHS_ID/ET0_CRS/RMII0_CRS_DV_A"; Timer = "GTOWLO/GTIOC6B/AGTOB2"; Analog = ""; Touch = "TS04" }
        { Number = 44; Port = "P407"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "CTS4_RTS4/SDA0_B/SSLA3_A/USB_VBUS/ET0_EXOUT/ET0_EXOUT"; Timer = "GTIOC6A/AGTIO0/RTCOUT"; Analog = "ADTRG0"; Touch = "TS03" }
        { Number = 45; Port = ""; System = "VSS_USB"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 46; Port = ""; System = "USB_DM"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 47; Port = ""; System = "USB_DP"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 48; Port = ""; System = "VCC_USB"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 49; Port = "P207"; System = ""; ExternalBus = "A17"; Interrupt = ""; Serial = "TXD4/SSLA2_A/QSSL"; Timer = ""; Analog = ""; Touch = "TSCAP" }
        { Number = 50; Port = "P206"; System = ""; ExternalBus = "WAIT"; Interrupt = "IRQ0-DS"; Serial = "RXD4/CTS9/SDA1_B/SSLA1_A/USB_VBUSEN/SD0DAT2_A/ET0_LINKSTA/ET0_LINKSTA/CECIO/SSIDATA0_C"; Timer = "GTIU"; Analog = ""; Touch = "TS02" }
        { Number = 51; Port = "P205"; System = "CLKOUT"; ExternalBus = "A16"; Interrupt = "IRQ1-DS"; Serial = "TXD4/CTS9_RTS9/SCL1_B/SSLA0_A/USB_OVRCURA-DS/SSILRCK0_C/SD0DAT3_A/ET0_WOL/ET0_WOL"; Timer = "GTIV/GTIOC4A/AGTO1"; Analog = ""; Touch = "TS01" }
        { Number = 52; Port = "P204"; System = "CACREF"; ExternalBus = "A18"; Interrupt = ""; Serial = "SCK4/SCK9/RSPCKA_A/USB_OVRCURB-DS/SSIBCK0_C/SD0DAT4_A/ET0_RX_DV"; Timer = "GTIW/GTIOC4B/AGTIO1"; Analog = ""; Touch = "TS00" }
        { Number = 53; Port = "P203"; System = ""; ExternalBus = "A19"; Interrupt = "IRQ2-DS"; Serial = "CTS2_RTS2/TXD9/MOSIA_A/CTX0/SD0DAT5_A/ET0_COL"; Timer = "GTIOC5A/AGTOA3"; Analog = ""; Touch = "TS18" }
        { Number = 54; Port = "P202"; System = ""; ExternalBus = "WR1/BC1"; Interrupt = "IRQ3-DS"; Serial = "SCK2/RXD9/MISOA_A/CRX0/SD0DAT6_A/ET0_ERXD2"; Timer = "GTIOC5B/AGTOB3"; Analog = ""; Touch = "TS19" }
        { Number = 55; Port = "P313"; System = ""; ExternalBus = "A20"; Interrupt = ""; Serial = "SD0DAT7_A/ET0_ERXD3"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 56; Port = "P314"; System = ""; ExternalBus = "A21"; Interrupt = ""; Serial = ""; Timer = ""; Analog = "ADTRG0"; Touch = "" }
        { Number = 57; Port = "P315"; System = ""; ExternalBus = "A22"; Interrupt = ""; Serial = "RXD4_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 58; Port = "P900"; System = ""; ExternalBus = "A23"; Interrupt = ""; Serial = "TXD4_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 59; Port = "P901"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "SCK4_C"; Timer = "AGTIO1_E"; Analog = ""; Touch = "" }
        { Number = 60; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 61; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 62; Port = "P214"; System = "TCLK"; ExternalBus = ""; Interrupt = ""; Serial = "QSPCLK/SD0CLK_B/ET0_MDC/ET0_MDC"; Timer = "GTIU/AGTO5"; Analog = ""; Touch = "" }
        { Number = 63; Port = "P211"; System = "TDATA0"; ExternalBus = "CS7"; Interrupt = ""; Serial = "QIO0/SD0CMD_B/ET0_MDIO/ET0_MDIO"; Timer = "GTIV/AGTOA5"; Analog = ""; Touch = "" }
        { Number = 64; Port = "P210"; System = "TDATA1"; ExternalBus = "CS6"; Interrupt = ""; Serial = "QIO1/SD0CD/ET0_WOL/ET0_WOL"; Timer = "GTIW/AGTOB5"; Analog = ""; Touch = "" }
        { Number = 65; Port = "P209"; System = "TDATA2"; ExternalBus = "CS5"; Interrupt = ""; Serial = "QIO2/SD0WP/ET0_EXOUT/ET0_EXOUT"; Timer = "GTOVUP/AGTEE5"; Analog = ""; Touch = "" }
        { Number = 66; Port = "P208"; System = "TDATA3"; ExternalBus = "CS4"; Interrupt = ""; Serial = "QIO3/SD0DAT0_B/ET0_LINKSTA/ET0_LINKSTA"; Timer = "GTOVLO"; Analog = ""; Touch = "" }
        { Number = 67; Port = ""; System = "RES"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 68; Port = "P201"; System = "MD"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 69; Port = "P200"; System = ""; ExternalBus = ""; Interrupt = "NMI"; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 70; Port = "P908"; System = ""; ExternalBus = ""; Interrupt = "IRQ11"; Serial = "USBHS_EXICEN"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 71; Port = "P907"; System = ""; ExternalBus = ""; Interrupt = "IRQ10"; Serial = "USBHS_ID"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 72; Port = "P906"; System = ""; ExternalBus = ""; Interrupt = "IRQ9"; Serial = "USB_EXICEN_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 73; Port = "P905"; System = ""; ExternalBus = ""; Interrupt = "IRQ8"; Serial = "USB_ID_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 74; Port = "P312"; System = ""; ExternalBus = "CS3"; Interrupt = ""; Serial = "CTS3_RTS3"; Timer = "AGTOA1"; Analog = ""; Touch = "" }
        { Number = 75; Port = "P311"; System = ""; ExternalBus = "CS2"; Interrupt = ""; Serial = "SCK3"; Timer = "AGTOB1"; Analog = ""; Touch = "" }
        { Number = 76; Port = "P310"; System = ""; ExternalBus = "A15"; Interrupt = ""; Serial = "TXD3/QIO3"; Timer = "AGTEE1"; Analog = ""; Touch = "" }
        { Number = 77; Port = "P309"; System = ""; ExternalBus = "A14"; Interrupt = ""; Serial = "RXD3/QIO2"; Timer = "AGTOA4"; Analog = ""; Touch = "" }
        { Number = 78; Port = "P308"; System = ""; ExternalBus = "A13"; Interrupt = ""; Serial = "CTS6/CTS3/QIO1"; Timer = "AGTOB4"; Analog = ""; Touch = "" }
        { Number = 79; Port = "P307"; System = ""; ExternalBus = "A12"; Interrupt = ""; Serial = "CTS6_RTS6/QIO0"; Timer = "GTOUUP_D/AGTEE4"; Analog = ""; Touch = "" }
        { Number = 80; Port = "P306"; System = ""; ExternalBus = "A11"; Interrupt = ""; Serial = "SCK6/QSSL"; Timer = "GTOULO_D/AGTOA2"; Analog = ""; Touch = "" }
        { Number = 81; Port = "P305"; System = ""; ExternalBus = "A10"; Interrupt = "IRQ8"; Serial = "TXD6/QSPCLK"; Timer = "GTOWUP/AGTOB2"; Analog = ""; Touch = "" }
        { Number = 82; Port = "P304"; System = ""; ExternalBus = "A9"; Interrupt = "IRQ9"; Serial = "RXD6"; Timer = "GTOWLO/GTIOC7A/AGTEE2"; Analog = ""; Touch = "" }
        { Number = 83; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 84; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 85; Port = "P303"; System = ""; ExternalBus = "A8"; Interrupt = ""; Serial = "CTS9"; Timer = "GTIOC7B"; Analog = ""; Touch = "" }
        { Number = 86; Port = "P302"; System = ""; ExternalBus = "A7"; Interrupt = "IRQ5"; Serial = "TXD2/SSLA3_B"; Timer = "GTOUUP/GTIOC4A"; Analog = ""; Touch = "" }
        { Number = 87; Port = "P301"; System = ""; ExternalBus = "A6"; Interrupt = "IRQ6"; Serial = "RXD2/CTS9_RTS9/SSLA2_B"; Timer = "GTOULO/GTIOC4B/AGTIO0"; Analog = ""; Touch = "" }
        { Number = 88; Port = "P300"; System = "TCK/SWCLK"; ExternalBus = ""; Interrupt = ""; Serial = "SSLA1_B"; Timer = "GTOUUP/GTIOC0A"; Analog = ""; Touch = "" }
        { Number = 89; Port = "P108"; System = "TMS/SWDIO"; ExternalBus = ""; Interrupt = ""; Serial = "CTS9_RTS9/SSLA0_B"; Timer = "GTOULO/GTIOC0B/AGTOA3"; Analog = ""; Touch = "" }
        { Number = 90; Port = "P109"; System = "TDO/SWO/CLKOUT"; ExternalBus = ""; Interrupt = ""; Serial = "TXD9/MOSIA_B/CTX1"; Timer = "GTOVUP/GTIOC1A/AGTOB3"; Analog = ""; Touch = "" }
        { Number = 91; Port = "P110"; System = "TDI"; ExternalBus = ""; Interrupt = "IRQ3"; Serial = "CTS2_RTS2/RXD9/MISOA_B/CRX1"; Timer = "GTOVLO/GTIOC1B/AGTEE3"; Analog = ""; Touch = "" }
        { Number = 92; Port = "P111"; System = ""; ExternalBus = "A5"; Interrupt = "IRQ4"; Serial = "SCK2/SCK9/RSPCKA_B"; Timer = "GTIOC3A/AGTOA5"; Analog = ""; Touch = "" }
        { Number = 93; Port = "P112"; System = ""; ExternalBus = "A4"; Interrupt = ""; Serial = "TXD2/SCK1/SSLA0_B/QSSL/OM_CS1/SSIBCK0_B"; Timer = "GTIOC3B/AGTOB5"; Analog = ""; Touch = "" }
        { Number = 94; Port = "P113"; System = ""; ExternalBus = "A3"; Interrupt = ""; Serial = "RXD2/SSILRCK0_B"; Timer = "GTIOC2A/AGTEE5"; Analog = ""; Touch = "" }
        { Number = 95; Port = "P114"; System = ""; ExternalBus = "A2"; Interrupt = ""; Serial = "CTS9/SSIRXD0_B"; Timer = "GTIOC2B/AGTIO5"; Analog = ""; Touch = "" }
        { Number = 96; Port = "P115"; System = ""; ExternalBus = "A1"; Interrupt = ""; Serial = "SSITXD0_B"; Timer = "GTIOC4A"; Analog = ""; Touch = "" }
        { Number = 97; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 98; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 99; Port = "P608"; System = ""; ExternalBus = "A0/BC0"; Interrupt = ""; Serial = ""; Timer = "GTIOC4B"; Analog = ""; Touch = "" }
        { Number = 100; Port = "P609"; System = ""; ExternalBus = "CS1"; Interrupt = ""; Serial = "CTX1/OM_ECS"; Timer = "GTIOC5A/AGTO5"; Analog = ""; Touch = "" }
        { Number = 101; Port = "P610"; System = ""; ExternalBus = "CS0"; Interrupt = ""; Serial = "CTS7/CRX1/OM_CS0"; Timer = "GTIOC5B/AGTO4"; Analog = ""; Touch = "" }
        { Number = 102; Port = "P611"; System = "CACREF/CLKOUT"; ExternalBus = ""; Interrupt = ""; Serial = "CTS7_RTS7"; Timer = "AGTO3"; Analog = ""; Touch = "" }
        { Number = 103; Port = "P612"; System = ""; ExternalBus = "D8"; Interrupt = ""; Serial = "SCK7"; Timer = "AGTO2"; Analog = ""; Touch = "" }
        { Number = 104; Port = "P613"; System = ""; ExternalBus = "D9"; Interrupt = ""; Serial = "TXD7"; Timer = "AGTO1"; Analog = ""; Touch = "" }
        { Number = 105; Port = "P614"; System = ""; ExternalBus = "D10"; Interrupt = ""; Serial = "RXD7"; Timer = "AGTO0"; Analog = ""; Touch = "" }
        { Number = 106; Port = "P615"; System = ""; ExternalBus = ""; Interrupt = "IRQ7"; Serial = "USB_VBUSEN_D"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 107; Port = "PA08"; System = ""; ExternalBus = ""; Interrupt = "IRQ6"; Serial = "USB_OVRCURA_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 108; Port = "PA09"; System = ""; ExternalBus = ""; Interrupt = "IRQ5"; Serial = "USB_OVRCURB_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 109; Port = "PA10"; System = ""; ExternalBus = ""; Interrupt = "IRQ4"; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 110; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 111; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 112; Port = ""; System = "VCL"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 113; Port = "PA01"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "SCK8_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 114; Port = "PA00"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "TXD8_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 115; Port = "P607"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "RXD8_C"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 116; Port = "P606"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "CTS8_RTS8_C"; Timer = "RTCOUT_B"; Analog = ""; Touch = "" }
        { Number = 117; Port = "P605"; System = ""; ExternalBus = "D11"; Interrupt = ""; Serial = "CTS8"; Timer = "GTIOC8A/AGTO4"; Analog = ""; Touch = "" }
        { Number = 118; Port = "P604"; System = ""; ExternalBus = "D12"; Interrupt = ""; Serial = "CTS9"; Timer = "GTIOC8B/AGTEE4"; Analog = ""; Touch = "" }
        { Number = 119; Port = "P603"; System = ""; ExternalBus = "D13"; Interrupt = ""; Serial = "CTS9_RTS9"; Timer = "GTIOC7A/AGTIO4"; Analog = ""; Touch = "" }
        { Number = 120; Port = "P602"; System = ""; ExternalBus = "EBCLK"; Interrupt = ""; Serial = "TXD9/OM_CS1"; Timer = "GTIOC7B/AGTO3"; Analog = ""; Touch = "" }
        { Number = 121; Port = "P601"; System = ""; ExternalBus = "WR/WR0"; Interrupt = ""; Serial = "RXD9/OM_SIO2"; Timer = "GTIOC6A/AGTEE3"; Analog = ""; Touch = "" }
        { Number = 122; Port = "P600"; System = "CACREF/CLKOUT"; ExternalBus = "RD"; Interrupt = ""; Serial = "SCK9/OM_SIO4"; Timer = "GTIOC6B/AGTIO3"; Analog = ""; Touch = "" }
        { Number = 123; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 124; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 125; Port = "P107"; System = ""; ExternalBus = "D7"; Interrupt = ""; Serial = "CTS8_RTS8/OM_SIO3"; Timer = "GTIOC8A/AGTOA0"; Analog = ""; Touch = "" }
        { Number = 126; Port = "P106"; System = ""; ExternalBus = "D6"; Interrupt = ""; Serial = "SCK8/SSLB3_A/OM_SIO0"; Timer = "GTIOC8B/AGTOB0"; Analog = ""; Touch = "" }
        { Number = 127; Port = "P105"; System = ""; ExternalBus = "D5"; Interrupt = "IRQ0"; Serial = "TXD8/SSLB2_A/OM_SIO5"; Timer = "GTETRGA/GTIOC1A/AGTO2"; Analog = ""; Touch = "" }
        { Number = 128; Port = "P104"; System = ""; ExternalBus = "D4"; Interrupt = "IRQ1"; Serial = "RXD8/SSLB1_A/QIO2/OM_DQS"; Timer = "GTETRGB/GTIOC1B/AGTEE2"; Analog = ""; Touch = "" }
        { Number = 129; Port = "P103"; System = ""; ExternalBus = "D3"; Interrupt = ""; Serial = "CTS0_RTS0/SSLB0_A/CTX0/QIO3/OM_SIO6"; Timer = "GTOWUP/GTIOC2A/AGTIO2"; Analog = ""; Touch = "" }
        { Number = 130; Port = "P102"; System = ""; ExternalBus = "D2"; Interrupt = ""; Serial = "SCK0/RSPCKB_A/CRX0/QIO0/OM_SIO1"; Timer = "GTOWLO/GTIOC2B/AGTO0"; Analog = "ADTRG0"; Touch = "" }
        { Number = 131; Port = "P101"; System = ""; ExternalBus = "D1"; Interrupt = "IRQ1"; Serial = "TXD0/CTS1_RTS1/MOSIB_A/QIO1/OM_SIO7"; Timer = "GTETRGB/GTIOC5A/AGTEE0"; Analog = ""; Touch = "" }
        { Number = 132; Port = "P100"; System = ""; ExternalBus = "D0"; Interrupt = "IRQ2"; Serial = "RXD0/SCK1/MISOB_A/QSPCLK/OM_SCLK"; Timer = "GTETRGA/GTIOC5B/AGTIO0"; Analog = ""; Touch = "" }
        { Number = 133; Port = "P800"; System = ""; ExternalBus = "D14"; Interrupt = ""; Serial = "CTS0"; Timer = "AGTOA4"; Analog = "AN125"; Touch = "" }
        { Number = 134; Port = "P801"; System = ""; ExternalBus = "D15"; Interrupt = ""; Serial = "CTS8"; Timer = "AGTOB4"; Analog = "AN126"; Touch = "" }
        { Number = 135; Port = "P802"; System = ""; ExternalBus = ""; Interrupt = "IRQ3"; Serial = ""; Timer = ""; Analog = "AN127"; Touch = "" }
        { Number = 136; Port = "P803"; System = ""; ExternalBus = ""; Interrupt = "IRQ2"; Serial = ""; Timer = ""; Analog = "AN128"; Touch = "" }
        { Number = 137; Port = "P804"; System = ""; ExternalBus = ""; Interrupt = "IRQ1"; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 138; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 139; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 140; Port = "P500"; System = "CACREF"; ExternalBus = ""; Interrupt = ""; Serial = "CTS5/USB_VBUSEN/QSPCLK"; Timer = "GTIU/AGTOA0"; Analog = "AN116"; Touch = "" }
        { Number = 141; Port = "P501"; System = ""; ExternalBus = ""; Interrupt = "IRQ11"; Serial = "TXD5/USB_OVRCURA/QSSL"; Timer = "GTIV/AGTOB0"; Analog = "AN117"; Touch = "" }
        { Number = 142; Port = "P502"; System = ""; ExternalBus = ""; Interrupt = "IRQ12"; Serial = "CTS6/RXD5/USB_OVRCURB/QIO0"; Timer = "GTIW/AGTOA2"; Analog = "AN118"; Touch = "" }
        { Number = 143; Port = "P503"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "CTS6_RTS6/SCK5/USB_EXICEN/QIO1"; Timer = "GTETRGC/AGTOB2"; Analog = "AN119"; Touch = "" }
        { Number = 144; Port = "P504"; System = ""; ExternalBus = "ALE"; Interrupt = ""; Serial = "SCK6/CTS5_RTS5/USB_ID/QIO2"; Timer = "GTETRGD/AGTOA3"; Analog = "AN120"; Touch = "" }
        { Number = 145; Port = "P505"; System = ""; ExternalBus = ""; Interrupt = "IRQ14"; Serial = "RXD6/QIO3"; Timer = "AGTOB3"; Analog = "AN121"; Touch = "" }
        { Number = 146; Port = "P506"; System = ""; ExternalBus = ""; Interrupt = "IRQ15"; Serial = "TXD6"; Timer = ""; Analog = "AN122"; Touch = "" }
        { Number = 147; Port = "P507"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "SCK6/SCK5"; Timer = ""; Analog = "AN123"; Touch = "" }
        { Number = 148; Port = "P508"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "CTS5_RTS5_B"; Timer = ""; Analog = "AN124"; Touch = "" }
        { Number = 149; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 150; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 151; Port = "P015"; System = ""; ExternalBus = ""; Interrupt = "IRQ13"; Serial = ""; Timer = ""; Analog = "AN013/DA1"; Touch = "" }
        { Number = 152; Port = "P014"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = "AN012/DA0"; Touch = "" }
        { Number = 153; Port = ""; System = "VREFL"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 154; Port = ""; System = "VREFH"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 155; Port = ""; System = "AVCC0"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 156; Port = ""; System = "AVSS0"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 157; Port = ""; System = "VREFL0"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 158; Port = ""; System = "VREFH0"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 159; Port = "P010"; System = ""; ExternalBus = ""; Interrupt = "IRQ14"; Serial = ""; Timer = ""; Analog = "AN010"; Touch = "" }
        { Number = 160; Port = "P009"; System = ""; ExternalBus = ""; Interrupt = "IRQ13-DS"; Serial = ""; Timer = ""; Analog = "AN009"; Touch = "" }
        { Number = 161; Port = "P008"; System = ""; ExternalBus = ""; Interrupt = "IRQ12-DS"; Serial = ""; Timer = ""; Analog = "AN008"; Touch = "" }
        { Number = 162; Port = "P007"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = "AN007"; Touch = "" }
        { Number = 163; Port = "P006"; System = ""; ExternalBus = ""; Interrupt = "IRQ11-DS"; Serial = ""; Timer = ""; Analog = "AN006"; Touch = "" }
        { Number = 164; Port = "P005"; System = ""; ExternalBus = ""; Interrupt = "IRQ10-DS"; Serial = ""; Timer = ""; Analog = "AN005"; Touch = "" }
        { Number = 165; Port = "P004"; System = ""; ExternalBus = ""; Interrupt = "IRQ9-DS"; Serial = ""; Timer = ""; Analog = "AN004"; Touch = "" }
        { Number = 166; Port = "P003"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = "AN003"; Touch = "" }
        { Number = 167; Port = "P002"; System = ""; ExternalBus = ""; Interrupt = "IRQ8-DS"; Serial = ""; Timer = ""; Analog = "AN002/AN102"; Touch = "" }
        { Number = 168; Port = "P001"; System = ""; ExternalBus = ""; Interrupt = "IRQ7-DS"; Serial = ""; Timer = ""; Analog = "AN001/AN101"; Touch = "" }
        { Number = 169; Port = "P000"; System = ""; ExternalBus = ""; Interrupt = "IRQ6-DS"; Serial = ""; Timer = ""; Analog = "AN000/AN100"; Touch = "" }
        { Number = 170; Port = ""; System = "VSS"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 171; Port = ""; System = "VCC"; ExternalBus = ""; Interrupt = ""; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 172; Port = "P806"; System = ""; ExternalBus = ""; Interrupt = "IRQ0"; Serial = ""; Timer = ""; Analog = ""; Touch = "" }
        { Number = 173; Port = "P805"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "TXD5_B"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 174; Port = "P513"; System = ""; ExternalBus = ""; Interrupt = ""; Serial = "RXD5_B"; Timer = ""; Analog = ""; Touch = "" }
        { Number = 175; Port = "P512"; System = ""; ExternalBus = ""; Interrupt = "IRQ14"; Serial = "TXD4/SCL1_A/CTX1"; Timer = "GTIOC0A"; Analog = ""; Touch = "" }
        { Number = 176; Port = "P511"; System = ""; ExternalBus = ""; Interrupt = "IRQ15"; Serial = "RXD4/SDA1_A/CRX1"; Timer = "GTIOC0B"; Analog = ""; Touch = "" }
    |]

