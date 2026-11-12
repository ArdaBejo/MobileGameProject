# Known issues

## Play Asset Delivery class lookup error in sideloaded release

- Build and device — v0.8.0 (code 5), ID 49ea902f. Pixel 8 Android Emulator, Android 16 / API 36.
- Repro — install the signed APK, force-stop the app, run `adb shell am start -W -n com.ardabecanim.mobilegame/com.unity3d.player.UnityPlayerGameActivity`, and watch the Unity tag in Logcat. Reproduced on online and airplane-mode launches.
- Expected — the sideloaded build reaches the title screen without a missing-class error.
- Observed — Unity logged `java.lang.ClassNotFoundException: Didn't find class "com.google.android.play.core.assetpacks.AssetPackManager"`. The same release subsequently reached the menu and arena. Player impact beyond the logged error is unconfirmed.
- Evidence — [Week 8 Lab B Logcat excerpt](docs/CA3/qa/logcat-w08-testplan.txt), lines at 16:02:48.145 and 16:05:41.240 in the emulator clock.

## Battery Saver leaves High FPS enabled in release 49ea902f

- Build and device — v0.8.0 (code 5), ID 49ea902f. Pixel 8 Android Emulator, Android 16 / API 36.
- Repro — leave High FPS enabled, enable Android Battery Saver, confirm that Android reports it on, then enter the arena.
- Expected — the FPS cap and overlay reflect Battery Saver.
- Observed — the overlay still reported High FPS, target 120, render scale 0.8. Gameplay continued. A physical-device check is pending.
- Evidence — [Android Battery Saver state](docs/CA3/qa/battery-state.txt) and [arena overlay](docs/CA3/qa/screens/battery-end.png).

- Fix and verification — the game now reads Android PowerManager Battery Saver state. QA build 0cdfdb9f switched 120 → 60 → 120 when Saver was toggled. See the [retest Logcat](docs/CA3/qa/qa-battery-logcat.txt).

## CA2 final Android build

The Android Rendering Profiler did not expose a usable draw-call or batch count on the Pixel 8 emulator. The Frame Debugger timed out. Windows Editor draw-call samples and emulator performance measurements are documented in the [CA2 baseline](docs/CA2/Performance_Baseline.md).
