#!/usr/bin/env bash
set -euo pipefail
ariel_test_dir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
ariel_project_root="$(cd -- "$ariel_test_dir/../../../../../.." && pwd)"
ariel_composer_dll="${ARIEL_COMPOSER_DLL:-$ariel_project_root/../Composer/src/bin/Debug/net10.0/Composer.dll}"
ariel_output="$ariel_test_dir/targets/ariel-lifecycle"
python "$ariel_project_root/tests/Ariel/lifecycle_model.py"
dotnet "$ariel_composer_dll" compile "$ariel_test_dir/Ariel.Experimental.fidproj" \
    --output "$ariel_output" --keep-intermediates
timeout 30 "$ariel_output"
