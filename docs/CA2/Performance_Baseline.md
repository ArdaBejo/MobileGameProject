# CA2 performance baseline

Target device for the available QA captures: Pixel 8 Android Emulator, Android 16 / API 36, virtual x86-64 CPU with four cores, 2,531,832 KiB guest RAM and OpenGL ES 3. The emulator battery is virtual. The project owner confirmed that emulator evidence is accepted for CA2. Physical battery health and physical-device thermals are unmeasured.

| Metric | Result |
| --- | --- |
| Week 9 build, tier and render scale | Development 6c888a0f, Auto → Low, target 30 FPS, 0.75 URP scale at 1080 × 2400 |
| Frame time mean / p99, CPU/GPU time and GC allocations | 49.02 / 166.67 ms across 1,225 frames in 60.05 active seconds. CPU/GPU split and GC allocations are unmeasured for this build. |
| Draw calls and batches | Android Profiler retest saved 1,784 frames. Draw calls were unavailable and batches returned zero despite visible rendering. Editor Game View, Low tier, measured 23.08 draw calls/frame on average (21–25, 7,186 frames in 60.17 s). Android draw calls and a valid batch count remain unverified. |
| Memory | Week 9 snapshot: textures 4.80 MB / 64 MB, audio 0.17 MB / 16 MB, managed 7.37 MB used and 8.80 MB reserved / 64 MB. Week 8 PSS after about five active minutes: 409,367 KiB (399.8 MiB). Week 9 post-snapshot PSS: 473,158 KiB (462.1 MiB), spot sample. Peak unmeasured. |
| Week 7 thermal soak | Virtual battery 25.0 °C at start and end. No thermal warning from the virtual provider. |
| Startup | Week 8: 1,048 ms Android cold launch and 13.30 s to interactive. Week 9: 10.71 s to Unity interactive on Development. Signed RC: 1,283 ms Android cold launch and 4.60 s Unity interactive in the lifecycle test. Final signed build b3927eac: 2,300 ms Android cold launch and 3.86 s Unity interactive in one emulator run. |
| Current release APK size | The earlier signed v0.8.0 build was 37,669,058 bytes. Week 9 Development APK was 47,805,244 bytes. Signed RC 620b79a5 is 36,583,978 bytes. Final signed build b3927eac is 36,583,434 bytes. |
| Bottleneck and fix | The Week 8 release kept a 120 FPS target when Android Battery Saver was enabled. QA build 0cdfdb9f changed its target from 120 to 60 when enabled and restored 120 on exit. This verifies the requested target transition. Sustained device performance after the fix is unmeasured. |

The project memory ceiling is 600 MB. Week 9 allocates 64 MB to textures, 16 MB to audio and 520 MB to other memory, with a 64 MB managed-heap sub-budget. Unity snapshot categories and Android PSS are recorded separately. The QA profile had Music Off.

[Detailed measurements](baseline/Performance_Baseline.md) · [Week 7 thermal evidence](baseline/thermal-soak/) · [Week 8 QA and retest](../CA3/qa/test-plan.md)
