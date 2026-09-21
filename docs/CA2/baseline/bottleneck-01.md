# Bottleneck 01

## What
The current development build is CPU-bound during the baseline profiling capture.

## Where
The main CPU cost appears under `PlayerLoop`, with `PostLateUpdate.FinishFrame` as the largest marker observed in the worst-frame capture.

## Numbers
- Worst-frame main thread: 62.03 ms
- PlayerLoop: 61.64 ms
- Largest marker: `PostLateUpdate.FinishFrame` — 19.70 ms
- `PostLateUpdate.ProfilerEndFrame` — 6.18 ms
- `FixedUpdate.PhysicsFixedUpdate` — 4.52 ms
- GC Allocated in Frame: 150 B
- GC.Collect: No
- SetPass Calls: 2
- Batches / Draw Calls: 0
- Triangles: 3
- Vertices: 7
- Total Reserved Memory: 347.6 MB
- Textures: 57.2 MB
- Meshes: 5.4 KB
- Audio: 1.1 MB
- Full render scale frame time: 17.92 ms
- Half render scale (0.5) frame time: 16.72 ms
- Render-scale improvement: approximately 6.7%
- Frame Debugger: 8 total events
- Observed GPU/UI pass: `Canvas.RenderOverlays`

## Verdict
CPU-bound. Reducing render scale from full scale to 0.5 only reduced frame time from 17.92 ms to 16.72 ms, which is a small change and far below the one-third reduction expected for a GPU-bound case.

## Fix to try
Investigate the CPU-side work under `PostLateUpdate.FinishFrame` and reduce unnecessary per-frame CPU work before changing GPU render settings.

## Evidence
- [Profiler capture](w03-profile.data)
- [Bad frame screenshot](w03-bad-frame.png)
- [Frame Debugger screenshot](w03-gpu-pass.png)