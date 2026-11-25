# Week 11 Lab A evidence index

25 November 2026. Current signed release: 0.9.1 / 7, efb9acb7; [identity](release.json). Gameplay balance and pause cancellation source checkpoint: 2aa8675. The earlier evidence commit is e3fd69a.

- Part A: [Perfetto report](../../perf/perfetto-2026-11-23.md), [clean trace](../../perf/pace-clean.pftrace), [actual UI screenshot](../../perf/perfetto-clean-frame-track.jpg). Preferred emulator submission cadence: average 21.08 ms / p99 38.71 ms. Native app SurfaceFrame and frequency data are unavailable; the SQL-derived cadence method is explicit.
- Part B: [Rendering fallback / heavy-frame analysis](../../perf/agi-2026-11-23.md). Active Android Development frame 106: 31.79 ms; FinishFrameRendering 25.02 ms, UI overlay 2.60 ms. These nested CPU timings narrow the investigation. Exact draw calls, GPU passes, overdraw and GPU timing remain unverified; AGI does not support Android emulators.
- Part C: [Awaitable audit](../awaitable-audit.md), actual ResumeGuard integration in WaveTimer, pause-cancelled hit feedback and fake purchase. [Balance assertions](balance-checks.txt) pass all three waves.
- Part D: [Six-case table](../resume-tests.md), release screenshots/log and instrumented Home/shade/lock checks. Five emulator paths have evidence. Simulated telephony did not reproduce the required real-call interruption; that row remains unverified. Audio checks are engine state, not speaker recordings.
- Part E: [Journal](../../../dev-journal.md) updated. [Prepared exit ticket](../../perf/exit-ticket.md), not posted. Moodle posting is outside the agreed automated scope.

The lab is not represented as fully verified on physical hardware. Original screenshot-driven and later clean traces are retained separately; their different runs and uncontrolled host load prevent a controlled performance comparison. The successful emulator uses the host NVIDIA GPU backend; a failed headless SwiftShader startup is recorded separately and contributes no performance numbers.
