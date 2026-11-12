# CA3 final-sprint targets

Set on 12 November 2026 for Week 9 Lab B.

## 1. Thermals

Target **p99 ≤ 33.33 ms** in the final minute of a ten-minute Balanced soak on a physical Android device, with the device model, tier, render scale and start/end temperatures recorded. This is a first sustained frame-pacing target, equivalent to a 30 FPS frame interval, while Balanced requests 60 FPS on Mid and High. Week 9's Auto Low emulator Development capture reached p99 166.67 ms in a different workload. Use the first comparable physical run as the before measurement. If Adaptive Performance reports ThrottlingImminent, evaluate a render-scale downshift with the same route and repeat the capture. [Baseline](../CA2/Performance_Baseline.md)

## 2. Resilience

Target **6/6 scenarios passing per device** on the same signed CA3 APK, using the primary Android device and a peer Android device. Retest background/resume, offline play, portrait lock, Battery Saver, memory pressure and cold/warm startup. Track confirmed crashes and unresolved session endings separately. A missing session_end can also come from a force-stop or OS kill. The Week 8 emulator run and the signed RC's ten Pause/Resume cycles are the starting evidence. [QA plan](qa/test-plan.md) · [RC lifecycle](../CA2/baseline/week9-art-pass/rc1-lifecycle.txt)

## 3. Store assets

Target **5 annotated portrait screenshots** showing the arena, enemy pressure, economy, run result and settings. Prepare the icon and feature graphic, draft store copy, and review the privacy statement against runtime data use. Use the Week 10 critique to revise the draft. [Async Pack 2 checklist](store/README.md)
