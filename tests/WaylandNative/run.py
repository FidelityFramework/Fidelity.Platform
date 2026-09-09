"""Build and run fresh native display-boundary probes, retaining their artifacts."""
import argparse
from pathlib import Path
import resource
import shutil
import subprocess
import sys
import tempfile
import time

here = Path(__file__).resolve().parent
repository = here.parents[1]
parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument("--composer", type=Path, default=repository.parent / "Composer/src/bin/Debug/net10.0/Composer.dll")
parser.add_argument("--stages", nargs="+", choices=["InterfaceTables", "ResvgIdentity"], default=["InterfaceTables", "ResvgIdentity"])
args = parser.parse_args()
artifacts = Path(tempfile.mkdtemp(prefix="display-native-"))
print(f"Native artifacts: {artifacts}", flush=True)

def no_core_dump():
    resource.setrlimit(resource.RLIMIT_CORE, (0, 0))

for stage in args.stages:
    directory = artifacts / stage
    directory.mkdir()
    binary = directory / "probe"
    log = directory / "compile.log"
    began = time.time_ns()
    with log.open("w") as output:
        result = subprocess.run(["dotnet", str(args.composer.resolve()), "compile", str(here / f"{stage}.fidproj"), "--output", str(binary), "--keep-intermediates", "--verbose"], cwd=repository, stdout=output, stderr=subprocess.STDOUT, timeout=600)
    for source in (here / "targets/intermediates").rglob("*"):
        if source.is_file() and source.stat().st_mtime_ns >= began:
            destination = directory / "intermediates" / source.relative_to(here / "targets/intermediates")
            destination.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(source, destination)
    if result.returncode != 0 or not binary.is_file():
        sys.stderr.write("\n".join(log.read_text().splitlines()[-60:]) + "\n")
        raise SystemExit(f"{stage}: compile failed; see {log}")
    result = subprocess.run([str(binary)], cwd=repository, timeout=30, preexec_fn=no_core_dump)
    if result.returncode != 0:
        raise SystemExit(f"{stage}: native gate failed {result.returncode}; artifacts {directory}")
    print(f"{stage}: PASS", flush=True)
