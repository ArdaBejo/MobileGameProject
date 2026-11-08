# Week 8 Lab B resilience test plan

8 November 2026. Pixel 8 Android Emulator, Android 16 / API 36, x86-64 guest. Signed ARM64 IL2CPP release v0.8.0 (version code 5), build ID 49ea902f. Results apply to this emulator run. The later Music settings source changes were not part of this APK.

| # | Scenario | Trigger | Expected | Result | Evidence |
| --- | --- | --- | --- | --- | --- |
| 1 | Resume / background | Home for 30 s, return, repeat ten times, then force-stop and relaunch | Pauses with audio silent, resumes in place and preserves save | Partial, 1 of 10 cycles | [Paused on return](screens/after-home.png) and [arena resumed](screens/resumed.png). Audio and save after force-stop still unverified. |
| 2 | No network | Cold launch with airplane mode on; turn airplane mode off and back on during the arena | Menu and arena remain responsive and no unhandled exception stops play | Pass on emulator | [Offline arena](screens/offline-game.png), [mid-play](screens/offline-midplay.png), and offline markers in Logcat |
| 3 | Rotation | Disable auto-rotate and request landscape during play | Portrait lock holds and arena HUD retains its layout | Pass on emulator | [Rotation screenshot](screens/rotation.png), arena countdown and HUD visible |
| 4 | Battery Saver | Enable Battery Saver and play for two minutes | Gameplay remains responsive, FPS cap and overlay reflect saver mode | Pending | No two-minute capture yet |
| 5 | Low RAM | Request memory trim, background with other apps running, and return | No black screen, or saved progress restored after a kill | Partial | RUNNING_CRITICAL trim request and short Home/return restored the [paused arena](screens/trim-return.png). Camera/browser pressure and kill recovery still untested. |
| 6 | Cold and warm start | Force-stop and launch with timing, then Home for ten seconds and relaunch | Record launch times and verify the loading screen clears | Partial | Cold 1048 ms, Home relaunch 193 ms with the same PID, classified HOT by Android. Menu interactive at 13.30 s Unity uptime. Arena loaded in 1564 ms. |

[Logcat excerpt](logcat-w08-testplan.txt) contains the project events and relevant startup exception. The raw emulator log is kept locally. See [KnownIssues.md](../../../KnownIssues.md) for the startup exception.
