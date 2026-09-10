# Wilderness Labs Meadow F7 product scaffold

Wilderness Labs owns the product identity; the selected MCU family references
the [ST STM32F7 scaffold](../../../Silicon/MCU/ST/STM32F7/README.md).
[Platform.clef](Platform.clef) retains empty endpoint collections and no core
description. This package is an inventory scaffold, not a working freestanding
target or an accepted board bring-up.

Hardware revision applicability remains unconfirmed. The staged vendor page
describes F7v2, but that document identity does not establish the physical
board revision or populated MCU. No revision directory is invented. The
[source manifest](docs/SOURCE_MANIFEST.md) distinguishes available evidence
from the missing routing and startup information.
