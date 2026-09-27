# Performance Baseline Sheet

| Field | Row 1 |
|-------|-------|
| Date, commit, versionName / versionCode, Release or Dev, Unity version | 26 Sep 2026, commit pending, 0.1.1 / 2, Release + Dev measurements, Unity 6.6 |
| Device model, Android version, SoC / GPU, graphics API, refresh rate | Google sdk_gphone64_x86_64 emulator, Android 16 / API 36, x86_64 emulator, OpenGLES3, refresh rate not recorded |
| Target fps | 60 |
| Menu: avg ms / p99 ms | 17.33 ms / 33.33 ms |
| Steady gameplay: avg ms / p99 ms | 17.08 ms / 33.33 ms |
| Worst case: avg ms / p99 ms | 17.72 ms / 33.33 ms |
| GC allocated per frame (bytes), allocating markers | 116 B; `RenderPipelineManager.DoRenderLoop_Internal()` 48 B; `NativeInputSystem.NotifyBeforeUpdate()` 34 B; `FrameEvents.NewInputBeforeRenderUpdate` 17 B; `NativeInputSystem.NotifyBeforeUpdate()` 17 B |
| Peak Total Reserved (MB) / TOTAL PSS (MB) | ~0.59 GB Unity memory / 364.2 MB TOTAL PSS |
| Worst case: SetPass / batches / triangles | 3 / 0 / 507 |
| Cold start ms (median of 3), first interactive s | 844 ms median; first interactive not separately measured |
| APK size (MB) | 31.93 MB |
| Thermal delta, throttling (Week 7) | |
| Load time (Week 5) | |