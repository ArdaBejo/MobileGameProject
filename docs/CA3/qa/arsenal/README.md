# Arsenal verification

25 November 2026, Pixel 8 Android Emulator, Android 16 / API 36. Development QA build 1759cd51, version 0.9.0 / code 6.

The local QA script credits synthetic kills to exercise the selection thresholds, invokes the real UI button callbacks and tests the real EnemyPool damage methods. This is a regression check, not a normal run or a balancing sample. The QA script is excluded from the signed build.

- Fresh run starts with Pistol 1, Sword 0, Rocket 0.
- Five kill credits open the selector and set timeScale to zero.
- ADB Back during the selector leaves the choice open and timeScale zero.
- Selecting Laser Sword resumes play and advances the run level.
- The next threshold permits Rocket Launcher unlock.
- Damage Up, Speed Up and Shield modify the expected stats. Shield consumes one charge.
- Pistol level increases through its choice.
- Area damage safely removes two pooled enemies while iterating.
- Automatic attacks damage live targets.
- Pause → Restart reloads the arena with the starting arsenal and clears the previous wallet upgrade state.

[Android log](android-checks.txt) · [Development choice screenshot](choice-development.png)

Signed release installation and a normal-play choice check are recorded separately.

## Signed 0.9.0 release

Build dd2cd44e, source 2e24ecd, 36,652,066 bytes. Verification confirmed ARM64 IL2CPP, Development Build off and signing-certificate continuity. The installed base.apk hash matched the local release. The APK contains no local regression harness.

A normal, unmodified arena run earned five kills and opened level 2. Back left the selection available, and selecting Laser Sword resumed gameplay. The existing Play Asset Delivery class-lookup warning also appeared on startup and is tracked in KnownIssues.md.

[Release identity](release.json) · [Normal-play Logcat](release-log.txt) · [Signed choice screen](choice-release.png)
