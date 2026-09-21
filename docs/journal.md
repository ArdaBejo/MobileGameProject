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

### Week 3 Lab A - RenderScaleProbe

- Full render scale frame time: 17.92 ms
- Half render scale (0.5) frame time: 16.72 ms
- Difference: approximately 6.7%
- Verdict: CPU-bound
- Reason: halving the render scale produced only a small improvement, so GPU rendering cost is not the main bottleneck.
- First CPU-side candidate to investigate: PostLateUpdate.FinishFrame (~19.70 ms in the earlier worst-frame capture).

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
- Improvement: approximately 6.7%
- Verdict: CPU-bound
- Reason: halving the render scale produced only a small change in frame time.

### Part E - Frame Debugger

- Frame Debugger capture completed on Android emulator.
- 8 total frame events were observed.
- UI overlay pass observed under `Canvas.RenderOverlays`.
