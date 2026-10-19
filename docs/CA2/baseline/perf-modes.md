# Week 7 Lab A — performance modes (Android Emulator)

**Test environment: Android Emulator (emulator-5554), not a physical phone.** All measurements, AP events and screenshots below belong to this emulator run.

**Accepted test platform:** the project owner reports that the lecturer permits emulator testing. Physical-phone verification is therefore not a remaining requirement for this lab submission. This permission does not make emulator thermal readings evidence of physical-device heating.

Test date: 19 October 2026. Device: Android Emulator, Google sdk_gphone64_x86_64, Android 16 / API 36, OpenGLES3, 1080 x 2400. ADB display diagnostics report a single supported refresh rate of approximately 60 Hz. These are emulator results, not physical-phone thermal or battery measurements.

| Mode | targetFrameRate | Render scale | Frame time avg (ms) | 99th (ms)* | Bottleneck | Virtual display Hz |
| --- | --- | --- | --- | --- | --- | --- |
| Balanced (default) | 60 | 1.0 | 16.74 | 16.67 | TargetFrameRate, occasional GPU | 60 |
| High FPS | 120 | 0.8 | 16.83 | 33.33 | TargetFrameRate after startup GPU/CPU transitions | 60 |

## Measurement method and limits

The existing FrameTimeSampler logs average and p99 of Time.unscaledDeltaTime over each 600-frame window. Balanced uses PID 25592 windows ending 22:17:03.002, 22:17:13.092 and 22:17:23.079 (16.75, 16.81, 16.67 ms). High FPS uses PID 25744 windows ending 22:18:37.489, 22:18:47.566 and 22:18:57.696 (16.78, 16.81, 16.89 ms). Each set contains 1,800 sampled frames, approximately 30 seconds; startup and mode-transition windows are excluded. Means are calculated from the three equally sized windows' rounded logged means.

*The 99th column is the highest logged window p99, not a pooled 1,800-frame percentile: the raw individual frames were not exported. FrameTimeSampler keeps sampling during pause, so the log alone cannot independently prove every sample was active gameplay; the owner reported playing in both modes. These Unity frame-delta samples are not GPU timings or an external frame-pacing trace.

The emulator remains near 60 FPS in both modes. Logs verify the requested target changes from 60 to 120 and the active URP asset render scale changes from 1.0 to 0.8. This test does not demonstrate 120 FPS on a high-refresh physical display. Build signing and Development Build status were not independently verified for this installed APK.

## Evidence

- [Balanced Settings screenshot](perf-modes/balanced.png): High FPS OFF (Balanced).
- [Balanced gameplay overlay](perf-modes/balanced-overlay.png): 58 FPS, 17.3 ms, target 60 and render scale 1.0, captured on the Android Emulator during Wave 3.
- [High FPS Settings screenshot](perf-modes/high-fps.png): High FPS ON.
- [High FPS gameplay overlay](perf-modes/high-fps-overlay.png): 60 FPS, 16.8 ms, target 120 and render scale 0.8, captured on the Android Emulator after the overlay was added.
- [Logcat excerpt](logcat-ap-events.txt): both mode applications, AP active/thermal/bottleneck messages and frame-time samples.

The original Settings screenshots show the toggle states. Subsequent gameplay screenshots supply visible FPS/frame-time and render-scale evidence for both modes. These are separate instantaneous readouts captured at different stages of gameplay (High FPS in Wave 1, Balanced in Wave 3), not a controlled performance comparison. The table above still reports the earlier logcat sampling windows, not new measurements derived from the overlay screenshots. Physical-phone testing is outside this accepted emulator submission.

Persistence is independently supported by a fresh process: PID 25744 starts at 22:18:15.209 with perf_mode=HighFps high=True targetFrameRate=120 renderScale=0.8. Both processes report AP active and NoWarning; emulator thermal values are not evidence of real device heat. AP logging remains enabled for Lab B, with scalers disabled.

Moodle exit ticket: the project owner confirmed submission. This is an owner-reported completion; the assistant did not inspect or submit the forum post. The prepared evidence includes both mode screenshots and this actual event line: `10-19 22:18:19.700 I/Unity (25744): [AP] bottleneck=TargetFrameRate`.
