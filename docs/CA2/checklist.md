# CA2 release-candidate checklist

- [ ] Install the current release-signed APK on a physical Android device without ADB.
- [x] Verify ten pause and resume cycles without a soft lock on the Week 8 emulator build. [QA evidence](../CA3/qa/test-plan.md)
- [ ] Complete the Baseline Sheet for the release candidate, including current frame time, GC allocations, memory, draw calls, thermal soak, cold start, APK size and before/after bottleneck evidence.
- [x] Document telemetry events and their expected call sites. [Event map](Economy_Telemetry_Map.md)
- [ ] Review the README build steps, controls, device targets and Known Issues against the release candidate.
- [ ] Finish the 1,000–1,500 word reflective journal.
- [ ] Record 3–5 minutes of gameplay including boot, menu, one run, pause/resume and relaunch.
- [ ] Create the v0.2-rc1 tag after verifying the release candidate.
