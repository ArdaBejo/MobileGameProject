# Week 7 Lab B capture checklist

With Android active in Unity, use **Tools > Performance > Build Android Thermal Soak APK**. It writes `Builds/Week7-ThermalSoak-dev.apk` with Development and Autoconnect Profiler enabled. The debug-signed package uses a `.soak` suffix so it can be installed beside the release and does not use the release save.

1. Install the thermal-overlay Android Development Build and record its build ID. Select Balanced. Keep the same test device, mode, scene and route for both captures.
2. With the app closed, save a timestamped battery reading from `adb shell dumpsys battery` (temperature is in tenths of a degree Celsius).
3. Attach Unity Profiler to the Android player. Enable CPU Usage and Rendering and keep at least 7,200 frames in Profiler history for a full minute. Record 60 seconds, save `before.data`, and capture `minute-00.png`.
4. Record Unity logcat to `logcat-soak.txt`. Run for ten minutes and save `minute-05.png` and `minute-10.png`. Log any scene restarts or pauses. Keep the app in the foreground and note when the AP warning state changes.
5. Immediately save the end battery reading and timestamp. Repeat the same route and 60-second Profiler capture, save `after.data`, and use Profile Analyzer to report mean and p99. Record the first warning/throttling timestamp or explicitly state if none was observed and report the maximum recorded AP level.

The overlay is a live readout; its FPS and frame time are short averages, not capture percentiles. If testing on an emulator, label the run and thermal readings accordingly. Use the measured values to prepare the Moodle exit ticket; submission is a separate step.

On PowerShell 7.4+, `capture-soak.ps1` can collect the three screenshots, Unity logcat, end battery dump and timestamped timeline while you play. Pass the ADB executable path, device serial and a new output directory with `-Adb`, `-Serial` and `-OutputDirectory`. Start it with the player already in the arena after the before capture. It leaves existing logcat entries intact and does not control the game or record Profiler data.
