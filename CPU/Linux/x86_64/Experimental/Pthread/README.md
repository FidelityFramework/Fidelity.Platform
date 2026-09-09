# Experimental pthread ABI surface

This directory preserves a legacy raw-address ABI prototype. It is not current
Clef-compatible source: ffi-boundary §1 requires opaque CHandle values and forbids
nativeint-as-pointer and NativePtr operations. The production pthread pilot and
bindings live two directories above and use the default typed surface.

The experimental Ariel prototype selects this manifest explicitly. Its intended
replacement requires CHandle plus bounded-array/capture projection in the compiler.
Passing Python ABI probes or the runtime-neutral lifecycle model does not supply
that missing native integration.

The generated layout and symbol facts target x86-64 glibc 2.34+; pthread exports
are bound through libc. Regeneration uses this directory's pilot with the explicit
experimental_native_pointer_surface option. See Farscape/docs/Native_Carrier_Bindings.md.
