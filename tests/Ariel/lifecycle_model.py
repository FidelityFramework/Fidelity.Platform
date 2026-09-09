"""Bounded transition checks for Ariel's claim/complete/release protocol.

This model checks interleavings, not native ABI or POSIX behavior. The current
typed carrier realization has separate fresh executable gates under native/.
"""

from collections import deque
from dataclasses import dataclass, replace
import unittest


@dataclass(frozen=True)
class Region:
    next_index: int
    participants: tuple  # (phase, lo, hi), phase: ready/executing/completed/released
    visits: tuple

    @property
    def retired(self):
        return all(phase == "released" for phase, _, _ in self.participants)

    @property
    def output_complete(self):
        return all(n == 1 for n in self.visits)


def transitions(state, count, chunk):
    for index, (phase, lo, hi) in enumerate(state.participants):
        participants = list(state.participants)
        if phase == "ready":
            if state.next_index < count:
                end = state.next_index + min(chunk, count - state.next_index)
                participants[index] = ("executing", state.next_index, end)
                yield replace(state, next_index=end, participants=tuple(participants))
            else:
                participants[index] = ("completed", 0, 0)
                yield replace(state, participants=tuple(participants))
        elif phase == "executing":
            visits = list(state.visits)
            for element in range(lo, hi):
                visits[element] += 1
            participants[index] = ("ready", 0, 0)
            yield replace(state, visits=tuple(visits), participants=tuple(participants))
        elif phase == "completed":
            participants[index] = ("released", 0, 0)
            yield replace(state, participants=tuple(participants))


class LifecycleModel(unittest.TestCase):
    def test_generation_reuse_after_every_release(self):
        # A participant may see the next publication immediately after its
        # acknowledgment, before returning to its condition wait. Two tokens
        # suffice only because admission waits for every participant release.
        for carriers in range(1, 4):
            initial = (0, 0, (0,) * carriers, (False,) * carriers, ((),) * carriers)
            pending = deque([initial])
            seen_states = {initial}
            terminals = 0
            while pending:
                epoch, token, observed, executing, histories = pending.popleft()
                self.assertTrue(all(history == tuple(range(1, len(history) + 1))
                                    for history in histories))
                following = []
                all_released = not any(executing) and all(value == token for value in observed)
                if epoch < 6 and all_released:
                    following.append((epoch + 1, 2 if token == 1 else 1,
                                      observed, executing, histories))
                for index in range(carriers):
                    running = list(executing)
                    if not executing[index] and observed[index] != token:
                        acquired = list(observed)
                        acquired[index] = token
                        recorded = list(histories)
                        recorded[index] += (epoch,)
                        running[index] = True
                        following.append((epoch, token, tuple(acquired),
                                          tuple(running), tuple(recorded)))
                    elif executing[index]:
                        running[index] = False
                        following.append((epoch, token, observed, tuple(running), histories))
                if not following:
                    terminals += 1
                    self.assertEqual(epoch, 6)
                    self.assertTrue(all_released)
                    self.assertTrue(all(history == (1, 2, 3, 4, 5, 6) for history in histories))
                for state in following:
                    if state not in seen_states:
                        seen_states.add(state)
                        pending.append(state)
            self.assertEqual(terminals, 1)

    def test_all_bounded_completion_orders(self):
        states_checked = 0
        saw_completed_but_not_retired = False
        for count in range(5):
            for carriers in range(1, 4):
                for chunk in range(1, 4):
                    initial = Region(0, (("ready", 0, 0),) * carriers, (0,) * count)
                    pending = deque([initial])
                    seen = {initial}
                    terminal = 0
                    while pending:
                        state = pending.popleft()
                        states_checked += 1
                        self.assertLessEqual(state.next_index, count)
                        self.assertTrue(all(v in (0, 1) for v in state.visits))
                        if state.output_complete and not state.retired:
                            saw_completed_but_not_retired = True
                            # The next region is rejected while any capture is live.
                            self.assertFalse(all(p[0] == "released" for p in state.participants))
                        following = list(transitions(state, count, chunk))
                        if not following:
                            terminal += 1
                            self.assertTrue(state.retired)
                            self.assertTrue(state.output_complete)
                        for next_state in following:
                            if next_state not in seen:
                                seen.add(next_state)
                                pending.append(next_state)
                    self.assertGreater(terminal, 0)
        self.assertTrue(saw_completed_but_not_retired)
        self.assertGreater(states_checked, 1000)
        print(f"Checked {states_checked} bounded lifecycle states")


if __name__ == "__main__":
    unittest.main()
