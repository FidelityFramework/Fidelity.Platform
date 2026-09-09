#!/usr/bin/env python3
"""Exercise generated Linux x86-64 pthread storage and entry ABI against glibc.

This observes the ABI of the generated production layout contracts with ctypes.
It does not establish Clef compiler acceptance; tests/Ariel/native contains the
separate compiled typed allocation, create/join and carrier lifecycle gates.
"""
import ctypes as c
import errno
import os
from pathlib import Path
import re


root = Path(__file__).resolve().parents[2]
types = (root / "CPU/Linux/x86_64/Bindings/Pthread/Types.clef").read_text()


def layout(name):
    module = re.search(rf"module {name} =\n(.*?)(?=\nmodule |\Z)", types, re.S)
    assert module, name
    size = int(re.search(r"let Size = (\d+)", module[1])[1])
    alignment = int(re.search(r"let Alignment = (\d+)", module[1])[1])
    return size, alignment


class Storage:
    def __init__(self, name):
        self.size, alignment = layout(name)
        assert alignment == c.alignment(c.c_uint64)
        assert self.size % alignment == 0
        self.words = (c.c_uint64 * (self.size // 8 + 2))()
        self.words[0] = self.words[-1] = 0xEFACED0123456789
        self.pointer = c.addressof(self.words) + 8
        assert self.pointer % alignment == 0

    def check_guards(self):
        assert self.words[0] == self.words[-1] == 0xEFACED0123456789


libc = c.CDLL("libc.so.6", use_errno=True)


def function(name, *args):
    f = getattr(libc, name)
    f.argtypes = args
    f.restype = c.c_int
    return f


pointer = c.c_void_p
mutex = Storage("pthread_mutex_t")
condition = Storage("pthread_cond_t")
assert function("pthread_mutex_init", pointer, pointer)(mutex.pointer, None) == 0
assert function("pthread_cond_init", pointer, pointer)(condition.pointer, None) == 0
assert function("pthread_mutex_lock", pointer)(mutex.pointer) == 0
c.set_errno(errno.EDOM)
status = function("pthread_mutex_trylock", pointer)(mutex.pointer)
assert status == errno.EBUSY, status
assert c.get_errno() == errno.EDOM, "pthread error must come from the return value"
assert function("pthread_mutex_unlock", pointer)(mutex.pointer) == 0
assert function("pthread_cond_broadcast", pointer)(condition.pointer) == 0

thread_size, thread_alignment = layout("pthread_t")
assert (thread_size, thread_alignment) == (c.sizeof(c.c_ulong), c.alignment(c.c_ulong))
thread = c.c_ulong()
environment = c.c_uint64(0x123456789ABCDEF0)
received = []
entry_type = c.CFUNCTYPE(pointer, pointer)


@entry_type
def entry(argument):
    received.append((argument, c.cast(argument, c.POINTER(c.c_uint64))[0]))
    return argument


create = function("pthread_create", c.POINTER(c.c_ulong), pointer, entry_type, pointer)
join = function("pthread_join", c.c_ulong, c.POINTER(pointer))
assert create(c.byref(thread), None, entry, c.addressof(environment)) == 0
returned = pointer()
assert join(thread, c.byref(returned)) == 0
assert received == [(c.addressof(environment), environment.value)]
assert returned.value == c.addressof(environment)

assert function("pthread_cond_destroy", pointer)(condition.pointer) == 0
assert function("pthread_mutex_destroy", pointer)(mutex.pointer) == 0
mutex.check_guards()
condition.check_guards()

affinity = Storage("cpu_set_t")
assert function("sched_getaffinity", c.c_int, c.c_size_t, pointer)(0, affinity.size, affinity.pointer) == 0
affinity.check_guards()
mask = c.string_at(affinity.pointer, affinity.size)
allowed = sum(byte.bit_count() for byte in mask)
assert allowed > 0
counted = function("__sched_cpucount", c.c_size_t, pointer)(affinity.size, affinity.pointer)
assert counted == allowed == len(os.sched_getaffinity(0)), (counted, allowed)
affinity.check_guards()
print(f"pthread ABI verified: mutex={mutex.size}, cond={condition.size}, thread={thread_size}; "
      f"explicit environment round trip, direct EBUSY return, affinity={allowed} CPUs")
