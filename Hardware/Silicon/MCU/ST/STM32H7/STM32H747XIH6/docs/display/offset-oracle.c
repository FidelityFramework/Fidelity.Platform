/* SPDX-License-Identifier: Apache-2.0
 * Host-only generated register-offset evidence, 2026-09-12.
 * Struct excerpts: Copyright (c) 2019 STMicroelectronics. All rights reserved.
 * Source: STMicroelectronics/cmsis-device-h7
 * commit 81db1ec63cdc191fae1565b772da3ea5aa29a683, Include/stm32h747xx.h.
 * Modified by extracting five typedefs and adding offsetof comparisons against
 * DisplayRegisters.clef. Qualifiers are defined locally; no device is accessed.
 * See LICENSE.CMSIS.md and SOURCES.md. This file is not a firmware C binding.
 */
#include <stdint.h>
#include <stddef.h>
#include <stdio.h>
#define __IO volatile
#define __I volatile const
#define __O volatile
typedef struct
{
 __IO uint32_t CR;             /*!< RCC clock control register,                                              Address offset: 0x00  */
 __IO uint32_t HSICFGR;        /*!< HSI Clock Calibration Register,                                          Address offset: 0x04  */
 __IO uint32_t CRRCR;          /*!< Clock Recovery RC  Register,                                             Address offset: 0x08  */
 __IO uint32_t CSICFGR;        /*!< CSI Clock Calibration Register,                                          Address offset: 0x0C  */
 __IO uint32_t CFGR;           /*!< RCC clock configuration register,                                        Address offset: 0x10  */
 uint32_t     RESERVED1;       /*!< Reserved,                                                                Address offset: 0x14  */
 __IO uint32_t D1CFGR;         /*!< RCC Domain 1 configuration register,                                     Address offset: 0x18  */
 __IO uint32_t D2CFGR;         /*!< RCC Domain 2 configuration register,                                     Address offset: 0x1C  */
 __IO uint32_t D3CFGR;         /*!< RCC Domain 3 configuration register,                                     Address offset: 0x20  */
 uint32_t     RESERVED2;       /*!< Reserved,                                                                Address offset: 0x24  */
 __IO uint32_t PLLCKSELR;      /*!< RCC PLLs Clock Source Selection Register,                                Address offset: 0x28  */
 __IO uint32_t PLLCFGR;        /*!< RCC PLLs  Configuration Register,                                        Address offset: 0x2C  */
 __IO uint32_t PLL1DIVR;       /*!< RCC PLL1 Dividers Configuration Register,                                Address offset: 0x30  */
 __IO uint32_t PLL1FRACR;      /*!< RCC PLL1 Fractional Divider Configuration Register,                      Address offset: 0x34  */
 __IO uint32_t PLL2DIVR;       /*!< RCC PLL2 Dividers Configuration Register,                                Address offset: 0x38  */
 __IO uint32_t PLL2FRACR;      /*!< RCC PLL2 Fractional Divider Configuration Register,                      Address offset: 0x3C  */
 __IO uint32_t PLL3DIVR;       /*!< RCC PLL3 Dividers Configuration Register,                                Address offset: 0x40  */
 __IO uint32_t PLL3FRACR;      /*!< RCC PLL3 Fractional Divider Configuration Register,                      Address offset: 0x44  */
 uint32_t      RESERVED3;      /*!< Reserved,                                                                Address offset: 0x48  */
 __IO uint32_t  D1CCIPR;       /*!< RCC Domain 1 Kernel Clock Configuration Register                         Address offset: 0x4C  */
 __IO uint32_t  D2CCIP1R;      /*!< RCC Domain 2 Kernel Clock Configuration Register                         Address offset: 0x50  */
 __IO uint32_t  D2CCIP2R;      /*!< RCC Domain 2 Kernel Clock Configuration Register                         Address offset: 0x54  */
 __IO uint32_t  D3CCIPR;       /*!< RCC Domain 3 Kernel Clock Configuration Register                         Address offset: 0x58  */
 uint32_t      RESERVED4;      /*!< Reserved,                                                                Address offset: 0x5C  */
 __IO uint32_t  CIER;          /*!< RCC Clock Source Interrupt Enable Register                               Address offset: 0x60  */
 __IO uint32_t  CIFR;          /*!< RCC Clock Source Interrupt Flag Register                                 Address offset: 0x64  */
 __IO uint32_t  CICR;          /*!< RCC Clock Source Interrupt Clear Register                                Address offset: 0x68  */
 uint32_t     RESERVED5;       /*!< Reserved,                                                                Address offset: 0x6C  */
 __IO uint32_t  BDCR;          /*!< RCC Vswitch Backup Domain Control Register,                              Address offset: 0x70  */
 __IO uint32_t  CSR;           /*!< RCC clock control & status register,                                     Address offset: 0x74  */
 uint32_t     RESERVED6;       /*!< Reserved,                                                                Address offset: 0x78  */
 __IO uint32_t AHB3RSTR;       /*!< RCC AHB3 peripheral reset register,                                      Address offset: 0x7C  */
 __IO uint32_t AHB1RSTR;       /*!< RCC AHB1 peripheral reset register,                                      Address offset: 0x80  */
 __IO uint32_t AHB2RSTR;       /*!< RCC AHB2 peripheral reset register,                                      Address offset: 0x84  */
 __IO uint32_t AHB4RSTR;       /*!< RCC AHB4 peripheral reset register,                                      Address offset: 0x88  */
 __IO uint32_t APB3RSTR;       /*!< RCC APB3 peripheral reset register,                                      Address offset: 0x8C  */
 __IO uint32_t APB1LRSTR;      /*!< RCC APB1 peripheral reset Low Word register,                             Address offset: 0x90  */
 __IO uint32_t APB1HRSTR;      /*!< RCC APB1 peripheral reset High Word register,                            Address offset: 0x94  */
 __IO uint32_t APB2RSTR;       /*!< RCC APB2 peripheral reset register,                                      Address offset: 0x98  */
 __IO uint32_t APB4RSTR;       /*!< RCC APB4 peripheral reset register,                                      Address offset: 0x9C  */
 __IO uint32_t GCR;            /*!< RCC RCC Global Control  Register,                                        Address offset: 0xA0  */
 uint32_t     RESERVED8;       /*!< Reserved,                                                                Address offset: 0xA4  */
 __IO uint32_t D3AMR;          /*!< RCC Domain 3 Autonomous Mode Register,                                   Address offset: 0xA8  */
 uint32_t     RESERVED11[9];    /*!< Reserved, 0xAC-0xCC                                                      Address offset: 0xAC  */
 __IO uint32_t RSR;            /*!< RCC Reset status register,                                               Address offset: 0xD0  */
 __IO uint32_t AHB3ENR;        /*!< RCC AHB3 peripheral clock  register,                                     Address offset: 0xD4  */
 __IO uint32_t AHB1ENR;        /*!< RCC AHB1 peripheral clock  register,                                     Address offset: 0xD8  */
 __IO uint32_t AHB2ENR;        /*!< RCC AHB2 peripheral clock  register,                                     Address offset: 0xDC  */
 __IO uint32_t AHB4ENR;        /*!< RCC AHB4 peripheral clock  register,                                     Address offset: 0xE0  */
 __IO uint32_t APB3ENR;        /*!< RCC APB3 peripheral clock  register,                                     Address offset: 0xE4  */
 __IO uint32_t APB1LENR;       /*!< RCC APB1 peripheral clock  Low Word register,                            Address offset: 0xE8  */
 __IO uint32_t APB1HENR;       /*!< RCC APB1 peripheral clock  High Word register,                           Address offset: 0xEC  */
 __IO uint32_t APB2ENR;        /*!< RCC APB2 peripheral clock  register,                                     Address offset: 0xF0  */
 __IO uint32_t APB4ENR;        /*!< RCC APB4 peripheral clock  register,                                     Address offset: 0xF4  */
 uint32_t      RESERVED12;      /*!< Reserved,                                                                Address offset: 0xF8  */
 __IO uint32_t AHB3LPENR;      /*!< RCC AHB3 peripheral sleep clock  register,                               Address offset: 0xFC  */
 __IO uint32_t AHB1LPENR;      /*!< RCC AHB1 peripheral sleep clock  register,                               Address offset: 0x100 */
 __IO uint32_t AHB2LPENR;      /*!< RCC AHB2 peripheral sleep clock  register,                               Address offset: 0x104 */
 __IO uint32_t AHB4LPENR;      /*!< RCC AHB4 peripheral sleep clock  register,                               Address offset: 0x108 */
 __IO uint32_t APB3LPENR;      /*!< RCC APB3 peripheral sleep clock  register,                               Address offset: 0x10C */
 __IO uint32_t APB1LLPENR;     /*!< RCC APB1 peripheral sleep clock  Low Word register,                      Address offset: 0x110 */
 __IO uint32_t APB1HLPENR;     /*!< RCC APB1 peripheral sleep clock  High Word register,                     Address offset: 0x114 */
 __IO uint32_t APB2LPENR;      /*!< RCC APB2 peripheral sleep clock  register,                               Address offset: 0x118 */
 __IO uint32_t APB4LPENR;      /*!< RCC APB4 peripheral sleep clock  register,                               Address offset: 0x11C */
 uint32_t     RESERVED13[4];   /*!< Reserved, 0x120-0x12C                                                    Address offset: 0x120 */

} RCC_TypeDef;
typedef struct
{
  __IO uint32_t MODER;    /*!< GPIO port mode register,               Address offset: 0x00      */
  __IO uint32_t OTYPER;   /*!< GPIO port output type register,        Address offset: 0x04      */
  __IO uint32_t OSPEEDR;  /*!< GPIO port output speed register,       Address offset: 0x08      */
  __IO uint32_t PUPDR;    /*!< GPIO port pull-up/pull-down register,  Address offset: 0x0C      */
  __IO uint32_t IDR;      /*!< GPIO port input data register,         Address offset: 0x10      */
  __IO uint32_t ODR;      /*!< GPIO port output data register,        Address offset: 0x14      */
  __IO uint32_t BSRR;     /*!< GPIO port bit set/reset,               Address offset: 0x18      */
  __IO uint32_t LCKR;     /*!< GPIO port configuration lock register, Address offset: 0x1C      */
  __IO uint32_t AFR[2];   /*!< GPIO alternate function registers,     Address offset: 0x20-0x24 */
} GPIO_TypeDef;
typedef struct
{
  __IO uint32_t VR;            /*!< DSI Host Version Register,                                 Address offset: 0x00      */
  __IO uint32_t CR;            /*!< DSI Host Control Register,                                 Address offset: 0x04      */
  __IO uint32_t CCR;           /*!< DSI HOST Clock Control Register,                           Address offset: 0x08      */
  __IO uint32_t LVCIDR;        /*!< DSI Host LTDC VCID Register,                               Address offset: 0x0C      */
  __IO uint32_t LCOLCR;        /*!< DSI Host LTDC Color Coding Register,                       Address offset: 0x10      */
  __IO uint32_t LPCR;          /*!< DSI Host LTDC Polarity Configuration Register,             Address offset: 0x14      */
  __IO uint32_t LPMCR;         /*!< DSI Host Low-Power Mode Configuration Register,            Address offset: 0x18      */
  uint32_t      RESERVED0[4];  /*!< Reserved, 0x1C - 0x2B                                                                */
  __IO uint32_t PCR;           /*!< DSI Host Protocol Configuration Register,                  Address offset: 0x2C      */
  __IO uint32_t GVCIDR;        /*!< DSI Host Generic VCID Register,                            Address offset: 0x30      */
  __IO uint32_t MCR;           /*!< DSI Host Mode Configuration Register,                      Address offset: 0x34      */
  __IO uint32_t VMCR;          /*!< DSI Host Video Mode Configuration Register,                Address offset: 0x38      */
  __IO uint32_t VPCR;          /*!< DSI Host Video Packet Configuration Register,              Address offset: 0x3C      */
  __IO uint32_t VCCR;          /*!< DSI Host Video Chunks Configuration Register,              Address offset: 0x40      */
  __IO uint32_t VNPCR;         /*!< DSI Host Video Null Packet Configuration Register,         Address offset: 0x44      */
  __IO uint32_t VHSACR;        /*!< DSI Host Video HSA Configuration Register,                 Address offset: 0x48      */
  __IO uint32_t VHBPCR;        /*!< DSI Host Video HBP Configuration Register,                 Address offset: 0x4C      */
  __IO uint32_t VLCR;          /*!< DSI Host Video Line Configuration Register,                Address offset: 0x50      */
  __IO uint32_t VVSACR;        /*!< DSI Host Video VSA Configuration Register,                 Address offset: 0x54      */
  __IO uint32_t VVBPCR;        /*!< DSI Host Video VBP Configuration Register,                 Address offset: 0x58      */
  __IO uint32_t VVFPCR;        /*!< DSI Host Video VFP Configuration Register,                 Address offset: 0x5C      */
  __IO uint32_t VVACR;         /*!< DSI Host Video VA Configuration Register,                  Address offset: 0x60      */
  __IO uint32_t LCCR;          /*!< DSI Host LTDC Command Configuration Register,              Address offset: 0x64      */
  __IO uint32_t CMCR;          /*!< DSI Host Command Mode Configuration Register,              Address offset: 0x68      */
  __IO uint32_t GHCR;          /*!< DSI Host Generic Header Configuration Register,            Address offset: 0x6C      */
  __IO uint32_t GPDR;          /*!< DSI Host Generic Payload Data Register,                    Address offset: 0x70      */
  __IO uint32_t GPSR;          /*!< DSI Host Generic Packet Status Register,                   Address offset: 0x74      */
  __IO uint32_t TCCR[6];       /*!< DSI Host Timeout Counter Configuration Register,           Address offset: 0x78-0x8F */
  __IO uint32_t TDCR;          /*!< DSI Host 3D Configuration Register,                        Address offset: 0x90      */
  __IO uint32_t CLCR;          /*!< DSI Host Clock Lane Configuration Register,                Address offset: 0x94      */
  __IO uint32_t CLTCR;         /*!< DSI Host Clock Lane Timer Configuration Register,          Address offset: 0x98      */
  __IO uint32_t DLTCR;         /*!< DSI Host Data Lane Timer Configuration Register,           Address offset: 0x9C      */
  __IO uint32_t PCTLR;         /*!< DSI Host PHY Control Register,                             Address offset: 0xA0      */
  __IO uint32_t PCONFR;        /*!< DSI Host PHY Configuration Register,                       Address offset: 0xA4      */
  __IO uint32_t PUCR;          /*!< DSI Host PHY ULPS Control Register,                        Address offset: 0xA8      */
  __IO uint32_t PTTCR;         /*!< DSI Host PHY TX Triggers Configuration Register,           Address offset: 0xAC      */
  __IO uint32_t PSR;           /*!< DSI Host PHY Status Register,                              Address offset: 0xB0      */
  uint32_t      RESERVED1[2];  /*!< Reserved, 0xB4 - 0xBB                                                                */
  __IO uint32_t ISR[2];        /*!< DSI Host Interrupt & Status Register,                      Address offset: 0xBC-0xC3 */
  __IO uint32_t IER[2];        /*!< DSI Host Interrupt Enable Register,                        Address offset: 0xC4-0xCB */
  uint32_t      RESERVED2[3];  /*!< Reserved, 0xD0 - 0xD7                                                                */
  __IO uint32_t FIR[2];        /*!< DSI Host Force Interrupt Register,                         Address offset: 0xD8-0xDF */
  uint32_t      RESERVED3[8];  /*!< Reserved, 0xE0 - 0xFF                                                                */
  __IO uint32_t VSCR;          /*!< DSI Host Video Shadow Control Register,                    Address offset: 0x100     */
  uint32_t      RESERVED4[2];  /*!< Reserved, 0x104 - 0x10B                                                              */
  __IO uint32_t LCVCIDR;       /*!< DSI Host LTDC Current VCID Register,                       Address offset: 0x10C     */
  __IO uint32_t LCCCR;         /*!< DSI Host LTDC Current Color Coding Register,               Address offset: 0x110     */
  uint32_t      RESERVED5;     /*!< Reserved, 0x114                                                                      */
  __IO uint32_t LPMCCR;        /*!< DSI Host Low-power Mode Current Configuration Register,    Address offset: 0x118     */
  uint32_t      RESERVED6[7];  /*!< Reserved, 0x11C - 0x137                                                              */
  __IO uint32_t VMCCR;         /*!< DSI Host Video Mode Current Configuration Register,        Address offset: 0x138     */
  __IO uint32_t VPCCR;         /*!< DSI Host Video Packet Current Configuration Register,      Address offset: 0x13C     */
  __IO uint32_t VCCCR;         /*!< DSI Host Video Chunks Current Configuration Register,     Address offset: 0x140     */
  __IO uint32_t VNPCCR;        /*!< DSI Host Video Null Packet Current Configuration Register, Address offset: 0x144     */
  __IO uint32_t VHSACCR;       /*!< DSI Host Video HSA Current Configuration Register,         Address offset: 0x148     */
  __IO uint32_t VHBPCCR;       /*!< DSI Host Video HBP Current Configuration Register,         Address offset: 0x14C     */
  __IO uint32_t VLCCR;         /*!< DSI Host Video Line Current Configuration Register,        Address offset: 0x150     */
  __IO uint32_t VVSACCR;       /*!< DSI Host Video VSA Current Configuration Register,         Address offset: 0x154     */
  __IO uint32_t VVBPCCR;       /*!< DSI Host Video VBP Current Configuration Register,         Address offset: 0x158     */
  __IO uint32_t VVFPCCR;       /*!< DSI Host Video VFP Current Configuration Register,         Address offset: 0x15C     */
  __IO uint32_t VVACCR;        /*!< DSI Host Video VA Current Configuration Register,          Address offset: 0x160     */
  uint32_t      RESERVED7[11]; /*!< Reserved, 0x164 - 0x18F                                                              */
  __IO uint32_t TDCCR;         /*!< DSI Host 3D Current Configuration Register,                Address offset: 0x190     */
  uint32_t      RESERVED8[155]; /*!< Reserved, 0x194 - 0x3FF                                                                */
  __IO uint32_t WCFGR;          /*!< DSI Wrapper Configuration Register,                        Address offset: 0x400       */
  __IO uint32_t WCR;            /*!< DSI Wrapper Control Register,                              Address offset: 0x404       */
  __IO uint32_t WIER;           /*!< DSI Wrapper Interrupt Enable Register,                     Address offset: 0x408       */
  __IO uint32_t WISR;           /*!< DSI Wrapper Interrupt and Status Register,                 Address offset: 0x40C       */
  __IO uint32_t WIFCR;          /*!< DSI Wrapper Interrupt Flag Clear Register,                 Address offset: 0x410       */
  uint32_t      RESERVED9;      /*!< Reserved, 0x414                                                                        */
  __IO uint32_t WPCR[5];        /*!< DSI Wrapper PHY Configuration Register,                    Address offset: 0x418-0x42B */
  uint32_t      RESERVED10;     /*!< Reserved, 0x42C                                                                        */
  __IO uint32_t WRPCR;          /*!< DSI Wrapper Regulator and PLL Control Register, Address offset: 0x430                  */
} DSI_TypeDef;
typedef struct
{
  uint32_t      RESERVED0[2];  /*!< Reserved, 0x00-0x04                                                       */
  __IO uint32_t SSCR;          /*!< LTDC Synchronization Size Configuration Register,    Address offset: 0x08 */
  __IO uint32_t BPCR;          /*!< LTDC Back Porch Configuration Register,              Address offset: 0x0C */
  __IO uint32_t AWCR;          /*!< LTDC Active Width Configuration Register,            Address offset: 0x10 */
  __IO uint32_t TWCR;          /*!< LTDC Total Width Configuration Register,             Address offset: 0x14 */
  __IO uint32_t GCR;           /*!< LTDC Global Control Register,                        Address offset: 0x18 */
  uint32_t      RESERVED1[2];  /*!< Reserved, 0x1C-0x20                                                       */
  __IO uint32_t SRCR;          /*!< LTDC Shadow Reload Configuration Register,           Address offset: 0x24 */
  uint32_t      RESERVED2[1];  /*!< Reserved, 0x28                                                            */
  __IO uint32_t BCCR;          /*!< LTDC Background Color Configuration Register,        Address offset: 0x2C */
  uint32_t      RESERVED3[1];  /*!< Reserved, 0x30                                                            */
  __IO uint32_t IER;           /*!< LTDC Interrupt Enable Register,                      Address offset: 0x34 */
  __IO uint32_t ISR;           /*!< LTDC Interrupt Status Register,                      Address offset: 0x38 */
  __IO uint32_t ICR;           /*!< LTDC Interrupt Clear Register,                       Address offset: 0x3C */
  __IO uint32_t LIPCR;         /*!< LTDC Line Interrupt Position Configuration Register, Address offset: 0x40 */
  __IO uint32_t CPSR;          /*!< LTDC Current Position Status Register,               Address offset: 0x44 */
  __IO uint32_t CDSR;         /*!< LTDC Current Display Status Register,                 Address offset: 0x48 */
} LTDC_TypeDef;
typedef struct
{
  __IO uint32_t CR;            /*!< LTDC Layerx Control Register                                  Address offset: 0x84 */
  __IO uint32_t WHPCR;         /*!< LTDC Layerx Window Horizontal Position Configuration Register Address offset: 0x88 */
  __IO uint32_t WVPCR;         /*!< LTDC Layerx Window Vertical Position Configuration Register   Address offset: 0x8C */
  __IO uint32_t CKCR;          /*!< LTDC Layerx Color Keying Configuration Register               Address offset: 0x90 */
  __IO uint32_t PFCR;          /*!< LTDC Layerx Pixel Format Configuration Register               Address offset: 0x94 */
  __IO uint32_t CACR;          /*!< LTDC Layerx Constant Alpha Configuration Register             Address offset: 0x98 */
  __IO uint32_t DCCR;          /*!< LTDC Layerx Default Color Configuration Register              Address offset: 0x9C */
  __IO uint32_t BFCR;          /*!< LTDC Layerx Blending Factors Configuration Register           Address offset: 0xA0 */
  uint32_t      RESERVED0[2];  /*!< Reserved */
  __IO uint32_t CFBAR;         /*!< LTDC Layerx Color Frame Buffer Address Register               Address offset: 0xAC */
  __IO uint32_t CFBLR;         /*!< LTDC Layerx Color Frame Buffer Length Register                Address offset: 0xB0 */
  __IO uint32_t CFBLNR;        /*!< LTDC Layerx ColorFrame Buffer Line Number Register            Address offset: 0xB4 */
  uint32_t      RESERVED1[3];  /*!< Reserved */
  __IO uint32_t CLUTWR;         /*!< LTDC Layerx CLUT Write Register                               Address offset: 0x144 */

} LTDC_Layer_TypeDef;
int main(void){
printf("rccCr %zu 0\n",offsetof(RCC_TypeDef,CR)+0);
printf("rccCfgr %zu 16\n",offsetof(RCC_TypeDef,CFGR)+0);
printf("rccD1cfgr %zu 24\n",offsetof(RCC_TypeDef,D1CFGR)+0);
printf("rccPllckselr %zu 40\n",offsetof(RCC_TypeDef,PLLCKSELR)+0);
printf("rccPllcfgr %zu 44\n",offsetof(RCC_TypeDef,PLLCFGR)+0);
printf("rccPll3divr %zu 64\n",offsetof(RCC_TypeDef,PLL3DIVR)+0);
printf("rccPll3fracr %zu 68\n",offsetof(RCC_TypeDef,PLL3FRACR)+0);
printf("rccD1ccipr %zu 76\n",offsetof(RCC_TypeDef,D1CCIPR)+0);
printf("rccApb3rstr %zu 140\n",offsetof(RCC_TypeDef,APB3RSTR)+0);
printf("rccAhb4enr %zu 224\n",offsetof(RCC_TypeDef,AHB4ENR)+0);
printf("rccApb3enr %zu 228\n",offsetof(RCC_TypeDef,APB3ENR)+0);
printf("resetModer %zu 0\n",offsetof(GPIO_TypeDef,MODER)+0);
printf("resetOtyper %zu 4\n",offsetof(GPIO_TypeDef,OTYPER)+0);
printf("resetOspeedr %zu 8\n",offsetof(GPIO_TypeDef,OSPEEDR)+0);
printf("resetPupdr %zu 12\n",offsetof(GPIO_TypeDef,PUPDR)+0);
printf("resetBsrr %zu 24\n",offsetof(GPIO_TypeDef,BSRR)+0);
printf("backlightModer %zu 0\n",offsetof(GPIO_TypeDef,MODER)+0);
printf("backlightOtyper %zu 4\n",offsetof(GPIO_TypeDef,OTYPER)+0);
printf("backlightOspeedr %zu 8\n",offsetof(GPIO_TypeDef,OSPEEDR)+0);
printf("backlightPupdr %zu 12\n",offsetof(GPIO_TypeDef,PUPDR)+0);
printf("backlightBsrr %zu 24\n",offsetof(GPIO_TypeDef,BSRR)+0);
printf("dsiCr %zu 4\n",offsetof(DSI_TypeDef,CR)+0);
printf("dsiCcr %zu 8\n",offsetof(DSI_TypeDef,CCR)+0);
printf("dsiLvcidr %zu 12\n",offsetof(DSI_TypeDef,LVCIDR)+0);
printf("dsiLcolcr %zu 16\n",offsetof(DSI_TypeDef,LCOLCR)+0);
printf("dsiLpcr %zu 20\n",offsetof(DSI_TypeDef,LPCR)+0);
printf("dsiLpmcr %zu 24\n",offsetof(DSI_TypeDef,LPMCR)+0);
printf("dsiPcr %zu 44\n",offsetof(DSI_TypeDef,PCR)+0);
printf("dsiGvcidr %zu 48\n",offsetof(DSI_TypeDef,GVCIDR)+0);
printf("dsiMcr %zu 52\n",offsetof(DSI_TypeDef,MCR)+0);
printf("dsiVmcr %zu 56\n",offsetof(DSI_TypeDef,VMCR)+0);
printf("dsiVpcr %zu 60\n",offsetof(DSI_TypeDef,VPCR)+0);
printf("dsiVccr %zu 64\n",offsetof(DSI_TypeDef,VCCR)+0);
printf("dsiVnpcr %zu 68\n",offsetof(DSI_TypeDef,VNPCR)+0);
printf("dsiVhsacr %zu 72\n",offsetof(DSI_TypeDef,VHSACR)+0);
printf("dsiVhbpcr %zu 76\n",offsetof(DSI_TypeDef,VHBPCR)+0);
printf("dsiVlcr %zu 80\n",offsetof(DSI_TypeDef,VLCR)+0);
printf("dsiVvsacr %zu 84\n",offsetof(DSI_TypeDef,VVSACR)+0);
printf("dsiVvbpcr %zu 88\n",offsetof(DSI_TypeDef,VVBPCR)+0);
printf("dsiVvfpcr %zu 92\n",offsetof(DSI_TypeDef,VVFPCR)+0);
printf("dsiVvacr %zu 96\n",offsetof(DSI_TypeDef,VVACR)+0);
printf("dsiCmcr %zu 104\n",offsetof(DSI_TypeDef,CMCR)+0);
printf("dsiGhcr %zu 108\n",offsetof(DSI_TypeDef,GHCR)+0);
printf("dsiGpdr %zu 112\n",offsetof(DSI_TypeDef,GPDR)+0);
printf("dsiGpsr %zu 116\n",offsetof(DSI_TypeDef,GPSR)+0);
printf("dsiClcr %zu 148\n",offsetof(DSI_TypeDef,CLCR)+0);
printf("dsiPctler %zu 160\n",offsetof(DSI_TypeDef,PCTLR)+0);
printf("dsiPconfr %zu 164\n",offsetof(DSI_TypeDef,PCONFR)+0);
printf("dsiPsr %zu 176\n",offsetof(DSI_TypeDef,PSR)+0);
printf("dsiIsr0 %zu 188\n",offsetof(DSI_TypeDef,ISR[0])+0);
printf("dsiIsr1 %zu 192\n",offsetof(DSI_TypeDef,ISR[1])+0);
printf("dsiIer0 %zu 196\n",offsetof(DSI_TypeDef,IER[0])+0);
printf("dsiIer1 %zu 200\n",offsetof(DSI_TypeDef,IER[1])+0);
printf("dsiWcfgr %zu 1024\n",offsetof(DSI_TypeDef,WCFGR)+0);
printf("dsiWcr %zu 1028\n",offsetof(DSI_TypeDef,WCR)+0);
printf("dsiWier %zu 1032\n",offsetof(DSI_TypeDef,WIER)+0);
printf("dsiWisr %zu 1036\n",offsetof(DSI_TypeDef,WISR)+0);
printf("dsiWpcr0 %zu 1048\n",offsetof(DSI_TypeDef,WPCR[0])+0);
printf("dsiWrpcr %zu 1072\n",offsetof(DSI_TypeDef,WRPCR)+0);
printf("ltdcSscr %zu 8\n",offsetof(LTDC_TypeDef,SSCR)+0);
printf("ltdcBpcr %zu 12\n",offsetof(LTDC_TypeDef,BPCR)+0);
printf("ltdcAwcr %zu 16\n",offsetof(LTDC_TypeDef,AWCR)+0);
printf("ltdcTwcr %zu 20\n",offsetof(LTDC_TypeDef,TWCR)+0);
printf("ltdcGcr %zu 24\n",offsetof(LTDC_TypeDef,GCR)+0);
printf("ltdcSrcr %zu 36\n",offsetof(LTDC_TypeDef,SRCR)+0);
printf("ltdcBccr %zu 44\n",offsetof(LTDC_TypeDef,BCCR)+0);
printf("ltdcIer %zu 52\n",offsetof(LTDC_TypeDef,IER)+0);
printf("ltdcIsr %zu 56\n",offsetof(LTDC_TypeDef,ISR)+0);
printf("ltdcIcr %zu 60\n",offsetof(LTDC_TypeDef,ICR)+0);
printf("layer1Cr %zu 132\n",offsetof(LTDC_Layer_TypeDef,CR)+132);
printf("layer1Whpcr %zu 136\n",offsetof(LTDC_Layer_TypeDef,WHPCR)+132);
printf("layer1Wvpcr %zu 140\n",offsetof(LTDC_Layer_TypeDef,WVPCR)+132);
printf("layer1Pfcr %zu 148\n",offsetof(LTDC_Layer_TypeDef,PFCR)+132);
printf("layer1Cacr %zu 152\n",offsetof(LTDC_Layer_TypeDef,CACR)+132);
printf("layer1Dccr %zu 156\n",offsetof(LTDC_Layer_TypeDef,DCCR)+132);
printf("layer1Bfcr %zu 160\n",offsetof(LTDC_Layer_TypeDef,BFCR)+132);
printf("layer1Cfbar %zu 172\n",offsetof(LTDC_Layer_TypeDef,CFBAR)+132);
printf("layer1Cfblr %zu 176\n",offsetof(LTDC_Layer_TypeDef,CFBLR)+132);
printf("layer1Cfblnr %zu 180\n",offsetof(LTDC_Layer_TypeDef,CFBLNR)+132);
printf("layer2Cr %zu 260\n",offsetof(LTDC_Layer_TypeDef,CR)+260);
}
