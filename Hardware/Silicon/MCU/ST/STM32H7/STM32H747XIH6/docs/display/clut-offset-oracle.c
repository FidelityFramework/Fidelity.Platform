/* SPDX-License-Identifier: Apache-2.0
 * Host-only generated CLUT offset evidence, 2026-09-13.
 * Struct excerpt: Copyright (c) 2019 STMicroelectronics. All rights reserved.
 * STMicroelectronics/cmsis-device-h7, commit
 * 81db1ec63cdc191fae1565b772da3ea5aa29a683, Include/stm32h747xx.h.
 * Modified by extracting LTDC_Layer_TypeDef and adding a compiled offsetof
 * comparison against the captured DisplayRegisters.clef descriptor literal.
 * The original CLUTWR member comment is retained verbatim; it names layer 2's
 * global offset. The member layout and layer 1 base establish the real offset.
 * See LICENSE.CMSIS.md and CLUT_SUPPLEMENT.md. No device is accessed.
 * This file is not linked into firmware and is not a C binding.
 */
#include <stdint.h>
#include <stddef.h>
#include <stdio.h>
#define __IO volatile
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

/* Captured from the pinned header's LTDC_Layer1_BASE expression. */
enum { LTDC_LAYER1_OFFSET = 0x84, CLEF_LAYER1_CLUTWR_OFFSET = 196 };
_Static_assert(sizeof(uint32_t) == 4, "requires 32-bit register words");
_Static_assert(offsetof(LTDC_Layer_TypeDef, CLUTWR) == 0x40, "CLUTWR member layout");
_Static_assert(LTDC_LAYER1_OFFSET + offsetof(LTDC_Layer_TypeDef, CLUTWR) ==
               CLEF_LAYER1_CLUTWR_OFFSET, "CLUTWR descriptor offset");
int main(void)
{
    const size_t compiled = LTDC_LAYER1_OFFSET + offsetof(LTDC_Layer_TypeDef, CLUTWR);
    printf("layer1Clutwr %zu %u\n", compiled, (unsigned)CLEF_LAYER1_CLUTWR_OFFSET);
    return compiled != CLEF_LAYER1_CLUTWR_OFFSET;
}
