"""Compare the native carrier choice with inherited and restricted CPU masks."""
import os
from pathlib import Path
import re
import subprocess
import sys

repository = Path(__file__).resolve().parents[3]
declaration = (repository / "Environments/Linux/x86_64/Ariel/Capabilities.clef").read_text()
budget = int(re.search(r"let carrier_budget: int = (\d+)", declaration).group(1))
allowed = sorted(os.sched_getaffinity(0))
sizes = sorted({1, min(3, len(allowed)), len(allowed)})
for size in sizes:
    selected = set(allowed[:size])
    result = subprocess.run([sys.argv[1]], timeout=10,
                            preexec_fn=lambda: os.sched_setaffinity(0, selected))
    expected = min(size, budget)
    if result.returncode != expected:
        raise SystemExit(f"Affinity count: expected {expected}, got {result.returncode}")
print("Allowed affinity: native counts match inherited and restricted masks")
