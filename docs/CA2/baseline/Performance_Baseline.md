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
