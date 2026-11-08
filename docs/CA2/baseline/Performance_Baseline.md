# Performance Baseline

## Week 3 Baseline

- Menu: 17.33 ms avg / 33.33 ms p99
- Steady gameplay: 17.08 ms avg / 33.33 ms p99
- Worst-case stress: 17.72 ms avg / 33.33 ms p99
- Steady-state GC allocation: 116 B
- Cold-start median: 844 ms
- APK size: 31.93 MB
- Baseline verdict: CPU-bound

## Week 5 - Pooled Enemy Spawning

- Test device: Android emulator
- Build profile: Android Dev
- Development Build: ON
- Autoconnect Profiler: ON
- Typical spawning frame time: ~17.09 ms
- GC Alloc from pooled spawning path: no allocation observed from Instantiate on the selected steady-state frame
- Small allocations observed elsewhere: 17 B / 48 B / 17 B
- Note: these small allocations were not identified as coming from EnemyPool / Enemy reuse
- Pool behaviour: enemies were reused and the pool hierarchy did not continuously grow
- Profiler capture: `w05-spawner.data`
## Week 5 Lab B — Greybox lane and async loading (12 October 2026)

### Configuration and evidence

- Device: `emulator-5554`, `sdk_gphone64_x86_64`, Android 16. These are emulator results, not physical-device benchmarks.
- Installed package: `com.ardabecanim.mobilegame`, version `0.4.0`, versionCode `4`.
- Build verified in Unity logcat: Release, IL2CPP, `arm64-v8a`, stripping enabled. The emulator identifies as x86_64; cross-architecture execution and host load may affect results, but no cause has been isolated.
- APK: `Builds/MyGame-0.4.0.apk`; installed base.apk SHA-256 matches the local artifact.
- SHA-256: `abe98149f86a16d0c15db7500e10475c0075156f847840ad257e87a6131dfa77`.
- Launch component resolved using ADB: `com.ardabecanim.mobilegame/com.unity3d.player.UnityPlayerGameActivity`.
- Each trial used `am force-stop` before `am start -W`; all three outputs report `LaunchState: COLD`.
- Raw evidence: `w05-load-times.txt`; values and artifact identity: `w05-load-times.json`; repeatable collector: `measure-loads.ps1`.

| Trial | Android TotalTime (ms) | Unity interactive marker (s) | Boot -> Menu loader (ms) |
| --- | ---: | ---: | ---: |
| 1 | 3209 | 9.44 | 5416 |
| 2 | 3469 | 8.05 | 3958 |
| 3 | 2733 | 8.01 | 4023 |
| Median | **3209** | **8.05** | **4023** |

### Budgets and interpretation

| Metric | Observed | Project budget | Assessment |
| --- | --- | --- | --- |
| Android first-frame cold start | 3209 ms median | < 3000 ms (diagnostic target adopted for this baseline) | Above target |
| Unity time to interactive marker | 8.05 s median | < 3 s (existing MDA interactive target) | Above target |
| Boot -> Menu scene load | 4023 ms median | < 1000 ms (initial transition budget adopted here) | Above target |
| Menu -> Game scene load | 1513 ms (one transition, PID 22403) | < 1000 ms | Above target |
| APK size | 35,674,041 bytes = 35.67 MB / 34.02 MiB | < 100 MB | Within target |

`TotalTime` ends at Android's first drawn frame, before the Unity menu is necessarily usable. `MenuReady` logs `Time.realtimeSinceStartup` at the first Menu Update after the loading overlay stops blocking input; this is the lab's Unity-runtime marker, not an independently timed icon-tap-to-interactive wall-clock measurement. SceneLoader Stopwatch durations include drawing the overlay, scene loading/activation and the destination initialization frame. No artificial loading delay was enabled.

The earlier 844 ms Week 3 figure came from a different build/test configuration and is retained as historical evidence, not a like-for-like regression claim. The current release misses the interactive budget. Next investigation: profile startup and Boot -> Menu activation on a comparable configuration, separating initialization work from emulator/host effects before attributing a cause.

## Week 7 Lab B: Thermals

Planned test: Balanced (60 FPS target, render scale 1.0), same arena route and workload before and after. Use a Development Build with Profiler attachment. Existing Week 7 Lab A emulator captures are a separate experiment and are not reused as soak evidence.

| Field | Value |
| --- | --- |
| Build ID and mode | 0.4.0 (4), Development IL2CPP arm64, source d1ffeb0 + 164fdff documentation, package com.ardabecanim.mobilegame.soak; Balanced 60 FPS target / 1.0 scale |
| Start temperature (C) and timestamp | 25.0 °C virtual, 19:51:46.966 Dublin (about 22 s before the timer; app already running) |
| End temperature (C), timestamp and 10-minute delta | 25.0 °C virtual; read requested at 20:02:09.393; delta 0.0 °C |
| AP temperature level at minutes 0 / 5 / 10 | 0.90 / 0.90 / no overlay during scene transition; subsequent arena overlay 0.90 |
| First ThrottlingImminent | Not observed during recorded emulator run |
| Throttling observed | No warning reported by virtual AP provider |
| Maximum AP temperature level | 0.90 normalized, virtual |
| Before frame time: mean / p99 (ms) | 3076.94 / 5394.11, 18 frames |
| After frame time: mean / p99 (ms) | 5257.00 / 14207.69, 10 frames |
| Conditions | Pixel 8 Android 16 emulator; automated center-position auto-fire with Retry and scene transitions; host also ran Fortnite during part of the session |

For an emulator run, report guest battery and AP values as virtual readings. A sustained emulator frame-time result is useful; it does not establish a physical phone thermal delta or throttling budget. The game's normal arena run lasts two minutes, so document any restarts rather than calling repeated runs uninterrupted gameplay. Keep captures and logcat in the [thermal soak evidence folder](thermal-soak/).

Android smoke check (3 November): Unity successfully built and ADB installed the separate development APK. SHA-256: `2b3f19e4471f71203a817b2d5ac1596b8db361b403648c89305aa09cb51e4b8c`. The [smoke screenshot](thermal-soak/overlay-smoke.png) shows both readouts in the arena, including AP NoWarning, level 0.90 and trend 0.00. The instantaneous 23 FPS / 44.0 ms reading is a startup smoke sample, not a sustained result or before capture. The timed run below is separate from this smoke check.

The resumed run lasted 600.08 s by the host timeline (19:52:09.311–20:02:09.393 Dublin, 3 November 2026). The earlier interrupted run is excluded. Repeated arena attempts included Result and loading intervals, so this is not an uninterrupted fixed-route gameplay trial. Minute 5 image finished writing about 12 s after its trigger; the minute 10 image shows the actual loading transition. The separate post-soak screenshot (file written 20:02:57) shows NoWarning, level 0.90 and trend 0.00 in the next arena. The original pre-launch battery reading was also 25.0 °C at 19:29:26.283; the comparison uses the more recent near-start reading.

Raw Android Player Profiler captures and frame-time CSVs are in the [evidence folder](thermal-soak/). Each recording was enabled for approximately 60 seconds; the exported frame durations cover 55.38 s before and 52.57 s after. Mean is arithmetic; p99 is nearest rank and equals the maximum in these small samples. These are real recorded frames, but the emulator was very slow with the Development build and attached Profiler, and host workload was uncontrolled. The difference does not establish a thermal regression or a usable sustained FPS budget. Repeat under stable host load with a repeatable gameplay route before using these figures for tuning. Moodle exit ticket not submitted.

## Week 8 Lab B: Release startup and memory

8 November 2026, Pixel 8 Android 16 emulator. Signed ARM64 IL2CPP build 0.8.0 (5), ID 49ea902f.

| Metric | Measured |
| --- | --- |
| Android cold launch TotalTime | 1048 ms for the marked run |
| Home relaunch TotalTime | 193 ms, classified HOT by Android with the original process retained |
| Menu interactive | 13.30 s Unity process uptime |
| Menu to arena scene load | 1564 ms |
| Early arena TOTAL PSS | 420086 KiB (about 410 MiB), sampled about 13 seconds after arena load |
| PSS after five minutes of play | Pending |
| 60-second frame-time stability | Pending |

The early memory sample is a smoke reading. The full run and evidence are tracked in the [Week 8 Lab B test plan](../../CA3/qa/test-plan.md).
