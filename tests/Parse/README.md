# Native decimal parsing checks

Run `dotnet fsi tests/Parse/Runner.fsx -- /path/to/private/Composer` from the
Platform repository. The driver uses the sibling Composer regression runner
and its prebuilt .NET ProcessHost. It never builds Composer or evaluates Clef
parsing through F#/.NET numeric conversions.

The four native programs compile the actual `Parse.clef` through CompilerSurface:
ten demanded value checks cover all decimal digits, zero/nine, leading zeroes,
signs and fractions; three invalid-input controls require exit 1, empty stdout
and the exact source-located terminal match diagnostic. These distinguish the
ASCII decimal alphabet from adjacent characters and a non-ASCII Unicode digit.
Compiler inputs/hashes, artifacts, raw streams and results remain under the
printed unique temporary directory.

These bounded inputs do not prove arbitrary-length accumulator capacity, a full
numeric-text grammar, or decimal rounding guarantees. Original full-profile
`06_AddNumbersInteractive` remains a separate unchanged manifest gate with paced
stdin and its existing output oracle.
