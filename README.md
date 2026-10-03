# MobileGameProject

**Module:** Mobile Game Development (A12581)  
**Project title:** No Stars Remain  
**Project option:** 1 – Arena-Survivor Roguelite  
**Current CA1 release:** 0.3.0

## Project overview

No Stars Remain is a portrait-oriented arena-survivor roguelite for Android. The player moves using a one-thumb virtual stick while weapons attack automatically. The current planned weapons are:

- Pistol
- Laser Sword
- Rocket Launcher

Planned passive upgrades:

- Damage Up
- Speed Up
- Shield

The Week 6 vertical-slice target is one arena, three weapons, three upgrades, a ten-minute run, and pause/resume functionality. These are development targets, not a claim that the full gameplay loop is complete in CA1.

The CA1 build includes a splash screen, a separate `MainMenu` scene with working Start Game and Quit buttons, and the existing gameplay prototype in `Sample Scene`. Settings and pause/resume controls are available in the gameplay scene.

## Android test devices

**Primary CA1 test configuration (v0.3.0):**

- Device model: `sdk_gphone64_x86_64` emulator (Pixel 8 profile)
- Android: Android 16 / API 36
- Resolution: 1080 × 2400 at 420 dpi
- Graphics API: OpenGLES3 (recorded Week 3 baseline)
- Result: APK installed and the splash screen, main menu, Start Game, gameplay scene and Quit functionality were tested successfully.

**Earlier compatibility test (v0.2.0):**

- Device profile: Pixel emulator
- Android: Android 14 / API 34
- Resolution: 1080 × 1920
- Result: v0.2.0 installed and the pause/settings interface worked; no visible safe-area or aspect-ratio issues were observed.

A physical Android device was not available. Emulator testing was permitted by the lecturer. Full test history is recorded in `docs/CA1/device-matrix.md`. The earlier Android 14 results do not establish compatibility of v0.3.0.

## Build profiles

### Android (development)

Used for Unity Profiler, Frame Debugger, GC allocation checks and development testing.

- Development Build: ON
- Autoconnect Profiler: ON
- Deep Profiling: OFF

### Release Android

Used to generate the release-signed APK for CA1.

- Development Build: OFF
- Build App Bundle (Google Play): OFF (output is an APK)
- Scripting Backend: IL2CPP
- Target Architecture: ARM64

## Signing

The release APK is signed with a custom release keystore. The original keystore was replaced after its key password could not be recovered.

- Keystore: kept outside the repository
- Key alias: `nostarsremain`
- Keystore path, validity dates and SHA-256 fingerprint: record the **new release keystore's** verified values in private signing notes; do not reuse the old keystore's details without checking.
- Back up the new keystore and its passwords securely. The same signing key is required for future updates signed outside Play App Signing.
- Never commit keystores or passwords.

The repository should ignore signing files:

```gitignore
*.keystore
*.jks
```

## Build instructions

1. Clone the repository and open the project in Unity 6.6 (`6000.6.0f1`) with Android Build Support.
2. In **File → Build Profiles**, select and activate **Release Android**.
3. Check **Edit → Project Settings → Player → Android → Other Settings**:
   - Package name: `com.ardabecanim.mobilegame`
   - Version: `0.3.0`
   - Bundle Version Code: `3`
   - Minimum API Level: Android 8.0 / API 26
   - Target API Level: Automatic (highest installed)
   - Scripting Backend: IL2CPP
   - Target Architecture: ARM64
4. Confirm **Development Build** and **Build App Bundle** are both disabled.
5. In **Publishing Settings**, select the new release keystore and key alias; provide the passwords locally. Do not add secrets to the repository.
6. In the Build Profiles Scene List, enable the scenes in this order:
   - `0` — `MainMenu`
   - `1` — `Sample Scene`
7. Click **Build** and save the release APK as:

```text
releases/MyGame-0.3.0-arm64.apk
```

8. With an authorized Android device or emulator connected, install the APK using ADB (adjust paths as needed):

```powershell
& "D:\UNITY\6000.6.0f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe" devices
& "D:\UNITY\6000.6.0f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe" install -r "C:\Users\probe\OneDrive\Desktop\MobileGameProject\releases\MyGame-0.3.0-arm64.apk"
```

9. Verify the installed version:

```powershell
& "D:\UNITY\6000.6.0f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe" shell dumpsys package com.ardabecanim.mobilegame | Select-String "versionCode|versionName"
```

Expected for the v0.3.0 build: `versionCode=3` and `versionName=0.3.0`. Record the actual terminal output in `docs/CA1/install-proof.txt`.

**Update note:** An APK signed using the new keystore cannot replace an installation signed with the old key. If Android reports `INSTALL_FAILED_UPDATE_INCOMPATIBLE`, uninstall the older installation before installing the new APK; uninstalling removes the app's local data.

## Accessibility and UI

The gameplay prototype includes pause/resume, lifecycle handling, a SafeArea-aware interface and the following accessibility controls:

- Haptics toggle, saved locally with `PlayerPrefs`; physical vibration has not been verified on an emulator.
- Small, Normal and Large text-size settings.
- Reduce Motion toggle, saved locally with `PlayerPrefs`. Motion-effect suppression is planned for when those effects are implemented.

The main menu uses a simple black background and functional Start Game and Quit buttons. Further visual polish is planned after CA1. Detailed accessibility notes are in `docs/CA1/accessibility-pass.md`.

## Performance baseline

Week 3 measurements were collected on an earlier build; these figures are **not new v0.3.0 benchmarks**.

- Full render scale: 17.92 ms
- Half render scale: 16.72 ms (~6.7% improvement)
- Menu: 17.33 ms average / 33.33 ms p99
- Steady gameplay: 17.08 ms average / 33.33 ms p99
- Worst-case stress test: 17.72 ms average / 33.33 ms p99
- Steady-state GC allocation: 116 B per frame
- TOTAL PSS: 364.2 MB
- Median measured cold start: 844 ms
- Earlier APK size: 31.93 MB

The render-scale comparison suggests prioritizing CPU-side investigation, especially `PostLateUpdate.FinishFrame`. These findings are preliminary and should be rechecked as gameplay develops.

Profiling records and screenshots remain in `docs/CA2/`, including the baseline, profiler capture, frame screenshots and bottleneck analysis.

## CA1 documentation

- `docs/CA1/install-proof.txt` — ADB installation evidence
- `docs/CA1/device-screenshot.png` — screenshot of the running release
- `docs/CA1/device-matrix.md` — emulator test history
- `docs/CA1/accessibility-pass.md` — accessibility checks and limitations
- `docs/CA1/store-assets-checklist.md` — planned store graphics and screenshots
- `docs/CA1/descriptions.md` — draft short and long store descriptions
- `docs/CA1/privacy-statement.md` — current data-use and privacy statement
- `docs/CA1/mda-onepager.md` — MDA, scope lock, risks and performance budget
- `docs/dev-journal.md` — development decisions, testing and reflections

No app-store upload is required for CA1.

## AI assistance

AI assistance was used for development support, troubleshooting, documentation drafting and interpreting profiling results. All submitted code and descriptions remain the student's responsibility and should be reviewed and explained by the student.
