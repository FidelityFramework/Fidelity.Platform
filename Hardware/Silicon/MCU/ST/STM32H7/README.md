# STM32H7 initial package

[Family.clef](Family.clef) declares family identity and an explicitly H747-scoped
DBGMCU address. [STM32H747XIH6](STM32H747XIH6) supplies the initial memory/vector
and GPIO register subset. Other H7 subfamilies are not inferred from this part.

The staged `docs/stm32h747ag.pdf` is DS12930 Rev3 for STM32H747xI/G.
[Initial GPIO provenance](STM32H747XIH6/docs/INITIAL_GPIO_SOURCES.md) records scope
and validation; the family SVD remains an interim source.

