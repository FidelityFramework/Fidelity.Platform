# ES0445 source status

Checked 2026-09-13. ST's [official ES0445 PDF](https://www.st.com/resource/en/errata_sheet/es0445-stm32h745xig-stm32h755xi-stm32h747xig-stm32h757xi-device-errata-stmicroelectronics.pdf)
was readable through the web document tool: **Rev 6, September 2025, 57 pages**.
Its first page includes STM32H747XI and identifies silicon revisions U/V with
`REV_ID=0x2003`.

The targeted review accepted section **2.13.1, page 26** as the source of the
LTDC startup workaround: start the PLL3R pixel clock before enabling the LTDC
register interface. The corresponding HelloDISCO correction is documented in
the [CLUT supplement](../STM32H747XIH6/docs/display/CLUT_SUPPLEMENT.md).
This is a review of one relevant erratum, not complete errata acceptance.

The PDF itself has **not been archived locally**. Direct HTTPS retrieval,
then official alternative URL/domain forms, timed out. No PDF SHA256 is
claimed, and no converted text or screenshot is presented as the original PDF.
Retain this exact remote revision and section identity until the original
PDF can be downloaded and hashed. The separate supplement manifest records
the remote observation without pretending it is a local source checkpoint.
