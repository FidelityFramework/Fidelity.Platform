/// Platform helper functions for string/number conversion.
/// These are compiled as MLIR functions and linked into the final binary.
namespace Fidelity.Platform.Linux_x86_64

open Microsoft.FSharp.Quotations

/// Helper function descriptors - signatures and MLIR implementations
module Helpers =

    /// Describes a platform helper function
    type HelperFunction = {
        Name: string
        /// MLIR function signature
        Signature: string
        /// MLIR function body (complete implementation)
        Body: string
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // Parse helpers - convert strings to values
    // ═══════════════════════════════════════════════════════════════════════════

    /// Parse a string to int64
    /// Input: fat string (!llvm.struct<(ptr, i64)>)
    /// Output: int64
    let parseIntHelper: HelperFunction = {
        Name = "fidelity_parse_int"
        Signature = "(!llvm.struct<(ptr, i64)>) -> i64"
        Body = """
func.func @fidelity_parse_int(%str: !llvm.struct<(ptr, i64)>) -> i64 {
  // Extract pointer and length
  %ptr = llvm.extractvalue %str[0] : !llvm.struct<(ptr, i64)>
  %len = llvm.extractvalue %str[1] : !llvm.struct<(ptr, i64)>
  
  // Constants
  %c0 = arith.constant 0 : i64
  %c1 = arith.constant 1 : i64
  %c10 = arith.constant 10 : i64
  %c48 = arith.constant 48 : i64    // '0'
  %c45 = arith.constant 45 : i8     // '-'
  
  // Check if first char is '-'
  %first_char = llvm.load %ptr : !llvm.ptr -> i8
  %is_neg = arith.cmpi eq, %first_char, %c45 : i8
  
  // Starting position: 1 if negative, 0 if positive
  %start_pos = arith.select %is_neg, %c1, %c0 : i64
  
  // Parse digits with scf.while loop
  // State: (value: i64, pos: i64)
  %result:2 = scf.while (%val = %c0, %pos = %start_pos) : (i64, i64) -> (i64, i64) {
    %in_bounds = arith.cmpi slt, %pos, %len : i64
    scf.condition(%in_bounds) %val, %pos : i64, i64
  } do {
  ^bb0(%val_arg: i64, %pos_arg: i64):
    // Load character at position
    %char_ptr = llvm.getelementptr %ptr[%pos_arg] : (!llvm.ptr, i64) -> !llvm.ptr, i8
    %char = llvm.load %char_ptr : !llvm.ptr -> i8
    %char_i64 = arith.extui %char : i8 to i64
    
    // Convert char to digit: digit = char - '0'
    %digit = arith.subi %char_i64, %c48 : i64
    
    // Update value: val = val * 10 + digit
    %val_times_10 = arith.muli %val_arg, %c10 : i64
    %new_val = arith.addi %val_times_10, %digit : i64
    
    // Increment position
    %new_pos = arith.addi %pos_arg, %c1 : i64
    
    scf.yield %new_val, %new_pos : i64, i64
  }
  
  // Apply sign: if negative, negate result
  %negated = arith.subi %c0, %result#0 : i64
  %final = arith.select %is_neg, %negated, %result#0 : i64
  
  return %final : i64
}
"""
    }

    /// Parse a string to float64
    /// Input: fat string (!llvm.struct<(ptr, i64)>)
    /// Output: f64
    let parseFloatHelper: HelperFunction = {
        Name = "fidelity_parse_float"
        Signature = "(!llvm.struct<(ptr, i64)>) -> f64"
        Body = """
func.func @fidelity_parse_float(%str: !llvm.struct<(ptr, i64)>) -> f64 {
  // Extract pointer and length
  %ptr = llvm.extractvalue %str[0] : !llvm.struct<(ptr, i64)>
  %len = llvm.extractvalue %str[1] : !llvm.struct<(ptr, i64)>
  
  // Constants
  %c0_i64 = arith.constant 0 : i64
  %c1_i64 = arith.constant 1 : i64
  %c10_i64 = arith.constant 10 : i64
  %c48 = arith.constant 48 : i64       // '0'
  %c45 = arith.constant 45 : i8        // '-'
  %c46 = arith.constant 46 : i8        // '.'
  %c0_f64 = arith.constant 0.0 : f64
  %c10_f64 = arith.constant 10.0 : f64
  
  // Check if first char is '-'
  %first_char = llvm.load %ptr : !llvm.ptr -> i8
  %is_neg = arith.cmpi eq, %first_char, %c45 : i8
  %start_pos = arith.select %is_neg, %c1_i64, %c0_i64 : i64
  
  // Parse integer part (before decimal point)
  // State: (value: i64, pos: i64)
  %int_result:2 = scf.while (%val = %c0_i64, %pos = %start_pos) : (i64, i64) -> (i64, i64) {
    %in_bounds = arith.cmpi slt, %pos, %len : i64
    %char_ptr = llvm.getelementptr %ptr[%pos] : (!llvm.ptr, i64) -> !llvm.ptr, i8
    %char = llvm.load %char_ptr : !llvm.ptr -> i8
    %not_dot = arith.cmpi ne, %char, %c46 : i8
    %continue = arith.andi %in_bounds, %not_dot : i1
    scf.condition(%continue) %val, %pos : i64, i64
  } do {
  ^bb0(%val_arg: i64, %pos_arg: i64):
    %char_ptr = llvm.getelementptr %ptr[%pos_arg] : (!llvm.ptr, i64) -> !llvm.ptr, i8
    %char = llvm.load %char_ptr : !llvm.ptr -> i8
    %char_i64 = arith.extui %char : i8 to i64
    %digit = arith.subi %char_i64, %c48 : i64
    %val_times_10 = arith.muli %val_arg, %c10_i64 : i64
    %new_val = arith.addi %val_times_10, %digit : i64
    %new_pos = arith.addi %pos_arg, %c1_i64 : i64
    scf.yield %new_val, %new_pos : i64, i64
  }
  
  // Convert integer part to float
  %int_f64 = arith.sitofp %int_result#0 : i64 to f64
  
  // Check if we have decimal point
  %has_decimal = arith.cmpi slt, %int_result#1, %len : i64
  
  // Parse fractional part if present
  %frac_start = arith.addi %int_result#1, %c1_i64 : i64  // Skip the '.'
  
  // State: (frac_value: f64, divisor: f64, pos: i64)
  %frac_result:3 = scf.while (%frac = %c0_f64, %div = %c1_f64, %pos = %frac_start) : (f64, f64, i64) -> (f64, f64, i64) {
    %in_bounds = arith.cmpi slt, %pos, %len : i64
    %continue = arith.andi %has_decimal, %in_bounds : i1
    scf.condition(%continue) %frac, %div, %pos : f64, f64, i64
  } do {
  ^bb0(%frac_arg: f64, %div_arg: f64, %pos_arg: i64):
    %char_ptr = llvm.getelementptr %ptr[%pos_arg] : (!llvm.ptr, i64) -> !llvm.ptr, i8
    %char = llvm.load %char_ptr : !llvm.ptr -> i8
    %char_i64 = arith.extui %char : i8 to i64
    %digit_i64 = arith.subi %char_i64, %c48 : i64
    %digit_f64 = arith.sitofp %digit_i64 : i64 to f64
    
    // new_div = div * 10
    %new_div = arith.mulf %div_arg, %c10_f64 : f64
    // frac = frac + digit / new_div
    %scaled_digit = arith.divf %digit_f64, %new_div : f64
    %new_frac = arith.addf %frac_arg, %scaled_digit : f64
    
    %new_pos = arith.addi %pos_arg, %c1_i64 : i64
    scf.yield %new_frac, %new_div, %new_pos : f64, f64, i64
  }
  
  // Combine integer and fractional parts
  %combined = arith.addf %int_f64, %frac_result#0 : f64
  
  // Apply sign
  %negated = arith.negf %combined : f64
  %final = arith.select %is_neg, %negated, %combined : f64
  
  return %final : f64
}
"""
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // String helpers
    // ═══════════════════════════════════════════════════════════════════════════

    /// Check if string contains a character
    /// Input: fat string, i8 char
    /// Output: i1 (bool)
    let stringContainsCharHelper: HelperFunction = {
        Name = "fidelity_string_contains_char"
        Signature = "(!llvm.struct<(ptr, i64)>, i8) -> i1"
        Body = """
func.func @fidelity_string_contains_char(%str: !llvm.struct<(ptr, i64)>, %target: i8) -> i1 {
  %ptr = llvm.extractvalue %str[0] : !llvm.struct<(ptr, i64)>
  %len = llvm.extractvalue %str[1] : !llvm.struct<(ptr, i64)>
  
  %c0 = arith.constant 0 : i64
  %c1 = arith.constant 1 : i64
  %false = arith.constant false
  
  // Search loop: state = (found: i1, pos: i64)
  %result:2 = scf.while (%found = %false, %pos = %c0) : (i1, i64) -> (i1, i64) {
    %in_bounds = arith.cmpi slt, %pos, %len : i64
    %not_found = arith.cmpi eq, %found, %false : i1
    %continue = arith.andi %in_bounds, %not_found : i1
    scf.condition(%continue) %found, %pos : i1, i64
  } do {
  ^bb0(%found_arg: i1, %pos_arg: i64):
    %char_ptr = llvm.getelementptr %ptr[%pos_arg] : (!llvm.ptr, i64) -> !llvm.ptr, i8
    %char = llvm.load %char_ptr : !llvm.ptr -> i8
    %is_match = arith.cmpi eq, %char, %target : i8
    %new_pos = arith.addi %pos_arg, %c1 : i64
    scf.yield %is_match, %new_pos : i1, i64
  }
  
  return %result#0 : i1
}
"""
    }

    /// All helper functions to be emitted
    let allHelpers: HelperFunction list = [
        parseIntHelper
        parseFloatHelper
        stringContainsCharHelper
    ]

    /// Generate MLIR for all helpers (to be emitted at module start)
    let emitAllHelpers () : string =
        allHelpers
        |> List.map (fun h -> h.Body)
        |> String.concat "\n\n"
