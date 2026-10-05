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