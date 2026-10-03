# Device Matrix

| Device           | Android             | Serial (last 4) | Install result         | Notes                                                                                                                                    |
| ---------------- | ------------------- | --------------- | ---------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| Pixel 8 emulator | Android 16 / API 36 | 5554            | Success, versionCode 2 | No major safe-area or aspect-ratio issues observed.                                                                                      |
| Pixel emulator   | Android 14 / API 34 | 5554            | Success, versionCode 2 | No visible safe-area or aspect-ratio issues. Pause/settings UI displayed correctly. Performance felt smoother than the primary emulator. |

## Version 0.3.0 — Additional tests (3 October 2026)

The original v0.2.0 results above are retained unchanged. The following results are for the newer release.

| Device                   | Android             | Build  | Install result | Notes                                                                                           |
| ------------------------ | ------------------- | ------ | -------------- | ----------------------------------------------------------------------------------------------- |
| Pixel 8 emulator         | Android 16 / API 36 | v0.3.0 | Success        | Splash screen, main menu, Start Game, gameplay and Quit worked.                                 |
| Pixel emulator (Pixel 1) | Android 14 / API 34 | v0.3.0 | Success        | Splash screen and main menu opened, but Start Game caused a crash while loading `Sample Scene`. |

### Known issue in v0.3.0

The Android 14 emulator reported a `SIGILL` (fatal signal 4) during Unity scene loading (`Loading.AsyncRe` / `Loading.Preload`). The emulator reports `x86_64` architecture. The root cause has not been confirmed. The issue remains unresolved and is not present in the reported Android 16 test.

**CA1 evidence:** Use the successful Android 16 v0.3.0 run. Keep the Android 14 failure recorded as a known issue for later investigation.
