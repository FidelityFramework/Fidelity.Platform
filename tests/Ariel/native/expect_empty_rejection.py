"""Run the native undersized pthread output test without writing a core dump."""
import resource
import signal
import subprocess
import sys
import errno
import os
import pty


def no_core():
    resource.setrlimit(resource.RLIMIT_CORE, (0, 0))


# The standard MLIR assertion lowering uses puts before abort. A terminal makes
# that line visible even though abort intentionally does not flush stdio.
master, slave = pty.openpty()
try:
    result = subprocess.run([sys.argv[1]], stdout=slave, stderr=slave,
                            timeout=10, preexec_fn=no_core)
finally:
    os.close(slave)
output = bytearray()
try:
    while True:
        try:
            chunk = os.read(master, 4096)
        except OSError as error:
            if error.errno == errno.EIO:
                break
            raise
        if not chunk:
            break
        output.extend(chunk)
finally:
    os.close(master)
message = b"Foreign reference newthread requires at least one element"
if result.returncode != -signal.SIGABRT or message not in output:
    sys.stderr.write(f"Unexpected result: {result.returncode}\n")
    sys.stderr.buffer.write(output)
    raise SystemExit(1)
print("Empty pthread output: rejected before foreign call")
