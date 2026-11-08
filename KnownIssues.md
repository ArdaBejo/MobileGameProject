# Known issues

## Play Asset Delivery class lookup error in sideloaded release

- Build and device — v0.8.0 (code 5), ID 49ea902f. Pixel 8 Android Emulator, Android 16 / API 36.
- Repro — install the signed APK, force-stop the app, run `adb shell am start -W -n com.ardabecanim.mobilegame/com.unity3d.player.UnityPlayerGameActivity`, and watch the Unity tag in Logcat. Reproduced on online and airplane-mode launches.
- Expected — the sideloaded build reaches the title screen without a missing-class error.
- Observed — Unity logged `java.lang.ClassNotFoundException: Didn't find class "com.google.android.play.core.assetpacks.AssetPackManager"`. The same release subsequently reached the menu and arena. Player impact beyond the logged error is unconfirmed.
- Evidence — [Week 8 Lab B Logcat excerpt](docs/CA3/qa/logcat-w08-testplan.txt), lines at 16:02:48.145 and 16:05:41.240 in the emulator clock.
