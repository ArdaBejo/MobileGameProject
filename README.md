# MobileGameProject

**Module:** Mobile Game Development (A12581)  
**Project title:** No Stars Remain  
**Project option:** 1 – Arena-Survivor Roguelite

## Project overview

No Stars Remain is a portrait-oriented arena-survivor roguelite for Android.

The player moves using a one-thumb virtual stick while weapons attack automatically. The current planned core weapons are:

- Pulse Blaster
- Plasma Orbitals
- Homing Micro-Missiles

Initial passive upgrades:

- Increased fire rate
- Increased damage
- Increased movement speed

The Week 6 target is:

- 1 arena
- 3 weapons
- 3 upgrades
- 10-minute run
- pause/resume

## Android test device

Primary test configuration:

- Device: Google sdk_gphone64_x86_64 emulator
- Android version: Android 16 / API 36
- Resolution: 1080x2400 @ 420 dpi
- Graphics API: OpenGLES3

Second compatibility test configuration:

- Device profile: Pixel emulator
- Android version: Android 14 / API 34
- Resolution: 1080x1920
- Install result: Success, versionCode 2
- Notes: no visible safe-area or aspect-ratio issues; pause/settings UI displayed correctly

A physical Android device is not currently available. A second emulator with a different device profile was used for the alternate-device test with lecturer approval.

## Build profiles

### Android Dev

Used for:

- Unity Profiler
- Frame Debugger
- GC allocation checks
- development testing

Settings:

- Development Build: ON
- Autoconnect Profiler: ON
- Deep Profiling: OFF

### Android Release

Used for:

- release signing
- frame-time baseline measurements
- cold-start testing
- TOTAL PSS measurement
- APK-size measurement

Current release version:

- Version name: `0.2.0`
- Bundle version code: `2`

## Signing

- Keystore location: stored outside the repository
- Keystore file: `NoStarsRemain.keystore`
- Alias: `nostarsremain`
- Valid from: 28 September 2026
- Valid until: 15 September 2076
- SHA-256 fingerprint: add the exact SHA-256 value from the `keytool -list -v` output
- Keystore backups: two separate locations
- Passwords are not stored in the repository

The repository ignores release keystores using:

```gitignore
*.keystore
*.jks
```

## Build

1. Clone the repository.
2. Open the project in Unity 6.6.
3. Switch the project to Android in **File > Build Profiles**.
4. In Player Settings confirm:
   - Package Name: `com.ardabecanim.mobilegame`
   - Version: `0.2.0`
   - Bundle Version Code: `2`
   - Minimum API Level: Android 8.0 / API 26
   - Target API Level: `Automatic (highest installed)`
   - Scripting Backend: `IL2CPP`
   - Target Architecture: `ARM64`
5. Select the Android Release build profile.
6. Make sure Development Build is off.
7. Make sure Build App Bundle is off.
8. Build the APK to:

```text
releases/MyGame-0.2.0-arm64.apk
```

9. Install the APK with ADB:

```powershell
adb install -r releases/MyGame-0.2.0-arm64.apk
```

If `adb` is not available on PATH, use the ADB executable from the Android SDK or Unity installation.

10. Verify the installed version:

```powershell
adb shell dumpsys package com.ardabecanim.mobilegame | Select-String "versionCode|versionName"
```

Expected output includes:

```text
versionCode=2
versionName=0.2.0
```

## Device targets

- Minimum API Level: Android 8.0 / API 26
- Target API Level: Automatic (highest installed)

Tested configurations:

- Pixel 8 emulator — Android 16 / API 36 — 1080x2400
- Pixel emulator — Android 14 / API 34 — 1080x1920

The release APK installed successfully on both configurations with `versionCode=2`. No major safe-area or aspect-ratio issues were observed.

## Accessibility and UI

Current implemented features include:

- pause/resume handling
- haptics toggle
- text-size settings
- reduce-motion setting
- Android lifecycle handling
- SafeArea-based UI
- pause/settings menu

The pause/settings UI is still being refined for better mobile sizing and readability.

## Performance baseline

Week 3 profiling and baseline measurements have been completed.

Current baseline highlights:

- Full render scale: 17.92 ms
- Half render scale: 16.72 ms
- Render-scale improvement: approximately 6.7%
- Current verdict: CPU-bound
- Menu: 17.33 ms avg / 33.33 ms p99
- Steady gameplay: 17.08 ms avg / 33.33 ms p99
- Worst-case stress test: 17.72 ms avg / 33.33 ms p99
- Steady-state GC allocation: 116 B
- Cold-start median: 844 ms
- APK size: 31.93 MB

Performance evidence is stored under:

```text
docs/CA2/baseline/
```

## Performance tools

Current profiling tools/scripts include:

- `MobileBootstrap.cs`
- `FrameTimeSampler.cs`
- `RenderScaleProbe.cs`

Week 3 evidence includes:

- Profiler capture
- bad-frame screenshot
- Frame Debugger screenshot
- bottleneck note
- performance baseline sheet

## AI assistance

AI assistance was used for development support, troubleshooting, documentation drafting, and interpreting profiling results. All generated code and instructions were reviewed and tested before being added to the project.
