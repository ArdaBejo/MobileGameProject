# CA2 release-candidate checklist

- [ ] Install and launch the release-signed Unity 6.6 Android APK (IL2CPP, ARM64) without ADB and capture an on-device screenshot identifying the device. Signed RC 620b79a5 installed, launched and reached Game via ADB on the Pixel 8 emulator. [RC evidence](baseline/week9-art-pass/rc1-lifecycle.txt)
- [x] Verify ten pause and resume cycles without a soft lock on signed RC 620b79a5 on the Pixel 8 Android emulator. [Lifecycle log](baseline/week9-art-pass/rc1-lifecycle.txt)
- [ ] Complete the Baseline Sheet for the release candidate, including device and battery details, CPU/GPU frame time, GC allocations, peak memory, draw calls and batches, resolution and render scale, ten-minute thermals, cold start, APK size and before/after bottleneck evidence.
- [x] Document telemetry events and their expected call sites. [Event map](Economy_Telemetry_Map.md)
- [ ] Review the README build steps, controls, device targets and Known Issues against the release candidate.
- [ ] Finish the 1,000–1,500 word reflective journal with Weeks 5–9 bullets, a structured reflection and references. [Review draft](Reflective_Journal.md)
- [ ] Record 3–5 minutes of gameplay including boot, menu, one run, pause/resume and relaunch.
- [x] Create and push the v0.2-rc1 tag after emulator smoke verification. The tag points to the source for build 620b79a5.


Week 9 Lab A exit ticket draft: Pixel 8 Android emulator, Auto selected Low. Unity Memory Profiler measured 4.80 MB Texture2D and 0.17 MB AudioClip with Music Off. Moodle posting and showcase-list confirmation are pending.

CA2 is due Sunday, 15 November 2026 at 23:59 Dublin time. Submit one ZIP containing the signed APK under `APK/`, the gameplay video under `Video/`, the baseline, event map and reflection under `Docs/`, and top-level `README.md`, `KnownIssues.md` and `LINKS.txt`. The links file identifies the repository and final CA2 tag. Bring a five-minute demo for the showcase.
