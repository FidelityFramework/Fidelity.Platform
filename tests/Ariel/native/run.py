"""Compile and execute fresh typed Clef native gates; retain artifacts for review."""
import argparse
from pathlib import Path
import resource
import subprocess
import shutil
import sys
import tempfile
import time

here = Path(__file__).resolve().parent
repository = here.parents[2]
stages = ("Allocation", "CreateJoin", "EmptyOutput", "Startup", "Affinity", "Lifecycle")
parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument("--composer", type=Path, default=repository.parent / "Composer" /
                    "src/bin/Debug/net10.0/Composer.dll")
parser.add_argument("--stages", nargs="+", choices=stages, default=stages)
args = parser.parse_args()
if not args.composer.is_file():
    parser.error(f"Composer assembly does not exist: {args.composer}")
artifacts = Path(tempfile.mkdtemp(prefix="ariel-native-"))
print(f"Native artifacts: {artifacts}", flush=True)

def disable_core_dumps():
    resource.setrlimit(resource.RLIMIT_CORE, (0, 0))

for stage in args.stages:
    directory = artifacts / stage
    directory.mkdir()
    binary = directory / "probe"
    log = directory / "compile.log"
    began = time.time_ns()
    with log.open("w") as output:
        compiled = subprocess.run(
            ["dotnet", str(args.composer.resolve()), "compile", str(here / f"{stage}.fidproj"),
             "--output", str(binary), "--keep-intermediates", "--verbose"],
            cwd=repository, stdout=output, stderr=subprocess.STDOUT, timeout=240)
    # Composer currently places phase artifacts beside the source project even
    # when the executable output is elsewhere. Preserve only files this build
    # actually wrote; earlier stages must not masquerade as current evidence.
    intermediate_root = here / "targets/intermediates"
    if intermediate_root.is_dir():
        for source in intermediate_root.rglob("*"):
            if source.is_file() and source.stat().st_mtime_ns >= began:
                destination = directory / "intermediates" / source.relative_to(intermediate_root)
                destination.parent.mkdir(parents=True, exist_ok=True)
                shutil.copy2(source, destination)
    if compiled.returncode != 0 or not binary.is_file():
        sys.stderr.write("\n".join(log.read_text().splitlines()[-60:]) + "\n")
        raise SystemExit(f"{stage}: compile failed; see {log}")
    if stage == "EmptyOutput":
        command = [sys.executable, str(here / "expect_empty_rejection.py"), str(binary)]
    elif stage == "Affinity":
        command = [sys.executable, str(here / "expect_affinity.py"), str(binary)]
    else:
        command = [str(binary)]
    try:
        result = subprocess.run(command, cwd=repository, timeout=30,
                                preexec_fn=disable_core_dumps)
    except subprocess.TimeoutExpired:
        raise SystemExit(f"{stage}: native gate timed out; artifacts {directory}") from None
    if result.returncode != 0:
        raise SystemExit(f"{stage}: native gate failed with status {result.returncode}; artifacts {directory}")
    print(f"{stage}: PASS", flush=True)
