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

- Device: Google sdk_gphone64_x86_64 emulator
- Android version: Android 16 / API 36
- Resolution: 1080x2400 @ 420 dpi
- Graphics API: OpenGLES3
- Physical Android device: not currently available

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

- Frame-time baseline measurements
- cold-start testing
- TOTAL PSS measurement
- APK-size measurement

Current release version:

- Version name: 0.1.1
- Bundle version code: 2

## Build and install

If the emulator appears in Unity:

1. Open the project in Unity 6.6.
2. Open **File > Build Profiles**.
3. Select the required Android profile.
4. Use **Build And Run**.

If Unity does not list the emulator, build the APK normally and install it manually using ADB:

```powershell
& "D:\UNITY\6000.6.0f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe" install -r "PATH_TO_APK"
