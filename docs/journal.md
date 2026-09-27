# Development Journal

## 14 September 2026

### What worked

- Installed Unity 6.6 with Android Build Support.
- Switched the project to Android and configured IL2CPP with ARM64.
- Built and installed the project successfully on an Android emulator using ADB.

### What did not work / issues

- I do not currently own an Android phone, so I am using an Android emulator for testing.
- The first Android build took a long time because IL2CPP compilation was required.
- ADB initially was not recognised in PowerShell, so I used Unity's own ADB path.

### Project shortlist

Project option: 1 – Arena-Survivor Roguelite

Core verb: Move and survive

First feature to cut if the project falls behind schedule: Final boss encounter

---

## 21 September 2026 - Week 3 Lab A

### Part B - CPU profiling

- Worst-frame main thread: 62.03 ms
- PlayerLoop: 61.64 ms
- Largest marker: `PostLateUpdate.FinishFrame` - 19.70 ms
- `PostLateUpdate.ProfilerEndFrame` - 6.18 ms
- `FixedUpdate.PhysicsFixedUpdate` - 4.52 ms
- GC Allocated in Frame: 150 B
- GC.Collect: No

### Part C - Rendering and memory

- SetPass Calls: 2
- Batches / Draw Calls: 0
- Triangles: 3
- Vertices: 7
- Total Reserved Memory: 347.6 MB
- Textures: 57.2 MB
- Meshes: 5.4 KB
- Audio: 1.1 MB
- Largest Unity object category: RenderTexture - 52.6 MB
- Largest object in snapshot: ProfilerScreenshotF - 19.8 MB

### Part D - RenderScaleProbe

- Full render scale frame time: 17.92 ms
- Half render scale (0.5) frame time: 16.72 ms
- Difference: approximately 6.7%
- Verdict: CPU-bound
- Reason: halving the render scale produced only a small improvement, so GPU rendering cost is not the main bottleneck.
- First CPU-side candidate to investigate: `PostLateUpdate.FinishFrame` (~19.70 ms in the earlier worst-frame capture).

### Part E - Frame Debugger

- Frame Debugger capture completed on Android emulator.
- 8 total frame events were observed.
- UI overlay pass observed under `Canvas.RenderOverlays`.

---

## 23 September 2026 - Week 3 Lab B

### Part A - Frame target

- `Application.targetFrameRate = 60`
- `QualitySettings.vSyncCount = 0`
- Optimized Frame Pacing enabled.
- Android emulator used for testing.

### Part B - Frame time baseline

- Menu: 17.33 ms average / 33.33 ms p99
- Steady gameplay: 17.08 ms average / 33.33 ms p99
- Worst-case stress test: 17.72 ms average / 33.33 ms p99
- Target frame time: 16.67 ms for 60 fps.
- Measurements were taken from the Release build on the Android emulator.
- A temporary stress-test scene with moving objects was used for the worst-case measurement because the main gameplay loop is not yet complete.

### Part C - GC allocations

- GC Allocated in Frame: 116 B
- Allocating markers:
  - `RenderPipelineManager.DoRenderLoop_Internal()` - 48 B
  - `NativeInputSystem.NotifyBeforeUpdate()` under `FixedUpdate.NewInputFixedUpdate` - 34 B
  - `FrameEvents.NewInputBeforeRenderUpdate` - 17 B
  - `NativeInputSystem.NotifyBeforeUpdate()` under `PreUpdate.NewInputUpdate` - 17 B
- GC.Collect: none observed during the steady-state capture.

### Part D - Peak memory and rendering

- Peak Unity memory observed: approximately 0.59 GB.
- TOTAL PSS: 364.2 MB.
- Worst-case SetPass Calls: 3.
- Worst-case Batches / Draw Calls: 0.
- Worst-case Triangles: 507.
- Worst-case Vertices: approximately 1.0K.
- TOTAL RSS observed: approximately 467.7 MB.
- TOTAL SWAP PSS observed: approximately 6.7 MB.

### Part E - Cold start and APK size

- Cold start #1: 1327 ms
- Cold start #2: 844 ms
- Cold start #3: 823 ms
- Median cold start: 844 ms
- APK size: 31.93 MB

### Part F - Baseline documentation

- Added `docs/CA2/baseline/baseline-sheet.md`.
- Added `docs/CA2/baseline/w03-sampler-log.png`.
- Week 3 Lab B measurements were recorded as the first performance baseline row.
