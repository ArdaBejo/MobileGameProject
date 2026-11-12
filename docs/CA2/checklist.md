# CA2 release-candidate checklist

- [x] Install and launch signed ARM64 IL2CPP build b3927eac on the Pixel 8 Android emulator. The project owner confirmed emulator acceptance for CA2. The submission folder contains in-game and About screenshots.
- [x] Verify ten pause and resume cycles without a soft lock on signed RC 620b79a5 on the Pixel 8 Android emulator. [Lifecycle log](baseline/week9-art-pass/rc1-lifecycle.txt)
- [ ] Complete the Baseline Sheet for the release candidate, including device and battery details, CPU/GPU frame time, GC allocations, peak memory, draw calls and batches, resolution and render scale, ten-minute thermals, cold start, APK size and before/after bottleneck evidence.
- [x] Document telemetry events and their expected call sites. [Event map](Economy_Telemetry_Map.md)
- [x] Review README build steps, controls, device targets and Known Issues for CA2.
- [ ] Finish the 1,000–1,500 word reflective journal with Weeks 5–9 bullets, a structured reflection and references. [Journal prepared for owner review](Reflective_Journal.md)
- [ ] Record 3–5 minutes of gameplay including boot, menu, one run, pause/resume and relaunch. [Recording plan](gameplay-capture.md)
- [x] Create and push the v0.2-rc1 tag after emulator smoke verification. The tag points to the source for build 620b79a5.
- [x] Create and push the final v0.2-CA2 tag at the source commit for signed build b3927eac.


Week 9 Lab A exit ticket draft: Pixel 8 Android emulator, Auto selected Low. Unity Memory Profiler measured 4.80 MB Texture2D and 0.17 MB AudioClip with Music Off. Week 9 scope excludes Moodle posting and the showcase.

CA2 is due Sunday, 15 November 2026 at 23:59 Dublin time. Submit one ZIP containing the signed APK under `APK/`, the gameplay video under `Video/`, the baseline, event map and reflection under `Docs/`, and top-level `README.md`, `KnownIssues.md` and `LINKS.txt`. The links file identifies the repository and final CA2 tag.
