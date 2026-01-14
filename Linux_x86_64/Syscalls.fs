/// Linux x86-64 syscall numbers.
/// These are the raw syscall numbers for the Linux kernel on x86-64.
namespace Fidelity.Platform.Linux_x86_64

open Microsoft.FSharp.Quotations

module Syscalls =

    /// File I/O syscalls
    [<Literal>]
    let SYS_read = 0

    [<Literal>]
    let SYS_write = 1

    [<Literal>]
    let SYS_open = 2

    [<Literal>]
    let SYS_close = 3

    [<Literal>]
    let SYS_lseek = 8

    [<Literal>]
    let SYS_pread64 = 17

    [<Literal>]
    let SYS_pwrite64 = 18

    /// Memory syscalls
    [<Literal>]
    let SYS_mmap = 9

    [<Literal>]
    let SYS_mprotect = 10

    [<Literal>]
    let SYS_munmap = 11

    [<Literal>]
    let SYS_brk = 12

    /// Process control
    [<Literal>]
    let SYS_exit = 60

    [<Literal>]
    let SYS_exit_group = 231

    [<Literal>]
    let SYS_getpid = 39

    [<Literal>]
    let SYS_fork = 57

    [<Literal>]
    let SYS_execve = 59

    [<Literal>]
    let SYS_wait4 = 61

    /// Time syscalls
    [<Literal>]
    let SYS_nanosleep = 35

    [<Literal>]
    let SYS_clock_gettime = 228

    [<Literal>]
    let SYS_clock_nanosleep = 230

    /// Socket syscalls
    [<Literal>]
    let SYS_socket = 41

    [<Literal>]
    let SYS_connect = 42

    [<Literal>]
    let SYS_accept = 43

    [<Literal>]
    let SYS_sendto = 44

    [<Literal>]
    let SYS_recvfrom = 45

    [<Literal>]
    let SYS_shutdown = 48

    [<Literal>]
    let SYS_bind = 49

    [<Literal>]
    let SYS_listen = 50

    [<Literal>]
    let SYS_setsockopt = 54

    [<Literal>]
    let SYS_getsockopt = 55

    /// Poll syscall (legacy - prefer epoll)
    [<Literal>]
    let SYS_poll = 7

    /// epoll syscalls (modern, scalable event notification)
    [<Literal>]
    let SYS_epoll_create1 = 291

    [<Literal>]
    let SYS_epoll_ctl = 233

    [<Literal>]
    let SYS_epoll_wait = 232

    /// epoll control operations
    [<Literal>]
    let EPOLL_CTL_ADD = 1

    [<Literal>]
    let EPOLL_CTL_DEL = 2

    [<Literal>]
    let EPOLL_CTL_MOD = 3

    /// epoll event flags
    [<Literal>]
    let EPOLLIN = 0x001u

    [<Literal>]
    let EPOLLOUT = 0x004u

    [<Literal>]
    let EPOLLERR = 0x008u

    [<Literal>]
    let EPOLLHUP = 0x010u

    [<Literal>]
    let EPOLLET = 0x80000000u  // Edge-triggered

    /// Socket constants
    [<Literal>]
    let AF_INET = 2

    [<Literal>]
    let SOCK_STREAM = 1

    [<Literal>]
    let SOL_SOCKET = 1

    [<Literal>]
    let SO_REUSEADDR = 2

    [<Literal>]
    let SHUT_RD = 0

    [<Literal>]
    let SHUT_WR = 1

    [<Literal>]
    let SHUT_RDWR = 2

    /// Poll event flags
    [<Literal>]
    let POLLIN = 0x0001s

    [<Literal>]
    let POLLOUT = 0x0004s

    [<Literal>]
    let POLLERR = 0x0008s

    [<Literal>]
    let POLLHUP = 0x0010s

    /// Standard file descriptors
    [<Literal>]
    let STDIN = 0

    [<Literal>]
    let STDOUT = 1

    [<Literal>]
    let STDERR = 2

    /// Syscall table as quotation for compile-time inspection
    let syscallTable: Expr<Map<string, int>> = <@
        Map.ofList [
            // File I/O
            "read", 0
            "write", 1
            "open", 2
            "close", 3
            "lseek", 8
            "pread64", 17
            "pwrite64", 18

            // Memory
            "mmap", 9
            "mprotect", 10
            "munmap", 11
            "brk", 12

            // Process
            "exit", 60
            "exit_group", 231
            "getpid", 39
            "fork", 57
            "execve", 59
            "wait4", 61

            // Time
            "nanosleep", 35
            "clock_gettime", 228
            "clock_nanosleep", 230

            // Sockets
            "socket", 41
            "connect", 42
            "accept", 43
            "sendto", 44
            "recvfrom", 45
            "shutdown", 48
            "bind", 49
            "listen", 50
            "setsockopt", 54
            "getsockopt", 55

            // Poll
            "poll", 7
        ]
    @>
