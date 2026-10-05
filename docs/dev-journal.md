# Development Journal

## 14 September 2026

### What worked

- Installed Unity 6.6 with Android Build Support.
- Switched the project to Android and configured IL2CPP with ARM64.
- Built and installed the project successfully on an Android emulator using ADB.

### What did not work / issues

- I do not currently own an Android phone, so I am using an Android emulator for testing.
- The first Android build took a long time because IL2CPP compilation was required.
- ADB initially was not recognised in PowerShell, so I used Unity's own ADB path.

### Project shortlist

Project option: 1 – Arena-Survivor Roguelite

Core verb: Move and survive

First feature to cut if the project falls behind schedule: Final boss encounter

---

## 21 September 2026 - Week 3 Lab A

### Part B - CPU profiling

- Worst-frame main thread: 62.03 ms
- PlayerLoop: 61.64 ms
- Largest marker: `PostLateUpdate.FinishFrame` - 19.70 ms
- `PostLateUpdate.ProfilerEndFrame` - 6.18 ms
- `FixedUpdate.PhysicsFixedUpdate` - 4.52 ms
- GC Allocated in Frame: 150 B
- GC.Collect: No

### Part C - Rendering and memory

- SetPass Calls: 2
- Batches / Draw Calls: 0
- Triangles: 3
- Vertices: 7
- Total Reserved Memory: 347.6 MB
- Textures: 57.2 MB
- Meshes: 5.4 KB
- Audio: 1.1 MB
- Largest Unity object category: RenderTexture - 52.6 MB
- Largest object in snapshot: ProfilerScreenshotF - 19.8 MB

### Part D - RenderScaleProbe

- Full render scale frame time: 17.92 ms
- Half render scale (0.5) frame time: 16.72 ms
- Difference: approximately 6.7%
- Verdict: CPU-bound
- Reason: halving the render scale produced only a small improvement, so GPU rendering cost is not the main bottleneck.
- First CPU-side candidate to investigate: `PostLateUpdate.FinishFrame` (~19.70 ms in the earlier worst-frame capture).

### Part E - Frame Debugger

- Frame Debugger capture completed on Android emulator.
- 8 total frame events were observed.
- UI overlay pass observed under `Canvas.RenderOverlays`.

---

## 23 September 2026 - Week 3 Lab B

### Part A - Frame target

- `Application.targetFrameRate = 60`
- `QualitySettings.vSyncCount = 0`
- Optimized Frame Pacing enabled.
- Android emulator used for testing.

### Part B - Frame time baseline

- Menu: 17.33 ms average / 33.33 ms p99
- Steady gameplay: 17.08 ms average / 33.33 ms p99
- Worst-case stress test: 17.72 ms average / 33.33 ms p99
- Target frame time: 16.67 ms for 60 fps.
- Measurements were taken from the Release build on the Android emulator.
- A temporary stress-test scene with moving objects was used for the worst-case measurement because the main gameplay loop is not yet complete.

### Part C - GC allocations

- GC Allocated in Frame: 116 B
- Allocating markers:
  - `RenderPipelineManager.DoRenderLoop_Internal()` - 48 B
  - `NativeInputSystem.NotifyBeforeUpdate()` under `FixedUpdate.NewInputFixedUpdate` - 34 B
  - `FrameEvents.NewInputBeforeRenderUpdate` - 17 B
  - `NativeInputSystem.NotifyBeforeUpdate()` under `PreUpdate.NewInputUpdate` - 17 B
- GC.Collect: none observed during the steady-state capture.

### Part D - Peak memory and rendering

- Peak Unity memory observed: approximately 0.59 GB.
- TOTAL PSS: 364.2 MB.
- Worst-case SetPass Calls: 3.
- Worst-case Batches / Draw Calls: 0.
- Worst-case Triangles: 507.
- Worst-case Vertices: approximately 1.0K.
- TOTAL RSS observed: approximately 467.7 MB.
- TOTAL SWAP PSS observed: approximately 6.7 MB.

### Part E - Cold start and APK size

- Cold start #1: 1327 ms
- Cold start #2: 844 ms
- Cold start #3: 823 ms
- Median cold start: 844 ms
- APK size: 31.93 MB

### Part F - Baseline documentation

- Added `docs/CA2/baseline/baseline-sheet.md`.
- Added `docs/CA2/baseline/w03-sampler-log.png`.
- Week 3 Lab B measurements were recorded as the first performance baseline row.

---

## CA1 retrospective — Week 2 work (14–16 September 2026)

### Release pipeline, gameplay scope and accessibility

- Created a release keystore and configured Android release signing. Recorded the key alias and validity separately from the passwords; credentials must not be committed to the repository.
- Prepared the MDA one-pager and locked the arena-survivor scope on 16 September: one portrait arena, player movement, automatic combat, three weapons, three passive upgrades and a ten-minute run as the vertical-slice target. Additional planets and bosses are stretch goals.
- Added pause/resume and accessibility work to the gameplay scene. The accessibility design includes optional haptics, Small/Normal/Large text-size choices and a Reduce Motion preference.

### Reflection

Separating development and release configurations reduces the risk of submitting a debugging build. Locking the vertical-slice scope makes it easier to prioritize the core gameplay before adding art, extra environments or bosses. A signed APK and reproducible installation process are as important to this assessment as features visible inside the game.

---

## 3 October 2026 — Week 4 / CA1 preparation

### Release build and signing

- Verified the Release Android profile uses IL2CPP and ARM64 with Development Build and Build App Bundle disabled.
- Kept the package name `com.ardabecanim.mobilegame`, advanced the prototype from v0.2.0 (versionCode 2) to v0.3.0 (intended versionCode 3), and used a release keystore with a 50-year validity period.
- Encountered a Gradle `:launcher:packageRelease` failure because the signing key could not be decrypted. As the earlier key password could not be recovered, created a new release keystore and completed the build.
- The existing emulator installation was signed with a different key, so an in-place update failed. Uninstalled the previous application and installed the new APK with ADB. This can erase local app data and shows why securely retaining the release keystore is important.

### Main menu and application flow

- Created a separate `MainMenu` scene with a plain black background, the game title, and working Start Game and Quit buttons. Kept the first menu deliberately simple so functional Android delivery took priority over promotional artwork.
- Used `MainMenuManager.cs` to load `Sample Scene` from Start Game. Added `MainMenu` as build scene 0 and kept `Sample Scene` in the build list.
- Confirmed that the startup splash screen displays and that the Start Game and Quit buttons work on the Android 16 emulator.

### Device testing and known issue

- Installed and tested v0.3.0 on the Android 16 emulator: splash screen, main menu and gameplay worked.
- Installed v0.3.0 on the Android 14 x86-64 Pixel emulator: the menu opens, but starting gameplay causes a native `SIGILL` crash during Unity's asynchronous scene loading. The cause is not yet established. Version 0.2.0 previously worked on this emulator.
- Kept the successful Android 16 test as CA1 evidence and recorded the Android 14 regression in `docs/CA1/device-matrix.md` instead of presenting both configurations as successful.
- Preserved the ADB installation proof and saved a screenshot of the current game running in the emulator.

### Accessibility verification

- Confirmed the Haptics toggle is connected to the `Haptics` script, which stores the setting with `PlayerPrefs`. Physical vibration cannot be verified on the emulator.
- Corrected TextScale coverage so the affected text responds to the Small, Normal and Large selections.
- Added `ReduceMotion.cs` to the existing toggle and confirmed the preference persists between Play Mode sessions. The preference does not yet suppress motion effects; it will be integrated when such effects exist.

### Publication readiness and design changes

- Prepared the store-asset checklist and store-description drafts. The planned icon, feature graphic and promotional screenshots have not yet been created or uploaded.
- Updated the privacy statement to disclose local storage of accessibility preferences using `PlayerPrefs`. Gameplay progress is not currently saved; the build's packages, permissions and network behavior still need a final review before any store publication.
- Simplified planned weapons to Pistol, Laser Sword and Rocket Launcher, and passive upgrades to Damage Up, Speed Up and Shield. This changes the presentation and intended implementation of the features without expanding the locked three-weapon/three-upgrade scope.
- Updated the MDA with the earlier Week 3 profiling baseline. Those measurements do not represent a new profiling run on v0.3.0; the supporting files remain in `docs/CA2/`.

### Reflections and next actions

The packaging failure showed that entering passwords in Unity is not proof that the correct signing key is available: signing must be tested by completing a release build. Changing the key also demonstrated why APK updates require a consistent signing identity. Separating the menu from gameplay made the startup flow easier to maintain, but testing exposed a regression on one emulator. For CA1, documenting the failure accurately is preferable to claiming compatibility that has not been achieved. The next development priorities are investigating that regression, implementing the playable arena-survivor loop, profiling the updated build and making the Reduce Motion preference affect any future motion effects.

---

## 3 October 2026 — Final UI polish and CA1 submission preparation

### UI changes and testing

- Reorganized the pause and settings interfaces into separate panels, with Settings and Back navigation so only the intended panel is shown at a time.
- Enlarged titles, labels, buttons and settings controls for a portrait mobile layout. Updated the main menu to match the Pause and Settings menu styling while retaining its simple black background.
- Applied the existing TextScale component to the main-menu text and checked that saved text-size preferences work across scenes.
- Unity crashed during one layout-editing session; some unsaved scene layout changes were lost. Rebuilt the visual layout without changing the working scripts and saved the scenes before rebuilding.
- Rebuilt the v0.3.0 release APK with the updated interface, reinstalled it on the Android 16 emulator and verified the splash screen, Main Menu, Start, Quit, Pause, Settings and Back navigation.
- Captured updated evidence showing the running game and emulator model and prepared the APK and documentation for CA1 submission.

### Reflection

Keeping panel-navigation logic separate from the visual layout allowed the interfaces to be redesigned without rewriting the settings and pause scripts. The Unity crash reinforced the importance of saving scenes frequently and committing working checkpoints. Verifying the APK after rebuilding was necessary because an earlier APK filename alone would not prove that the updated interface was included.

### Week 5 Lab A - Coroutine Search

Searched project-authored scripts for:

- IEnumerator
- StartCoroutine
- WaitForSeconds

No project-authored coroutines were found.

The only match was:

`Assets/Scripts/Enemy.cs`

which already uses:

`Awaitable.WaitForSecondsAsync(0.1f)`

Coroutine matches found elsewhere were inside TextMesh Pro example/demo scripts and were not modified.

### Week 5 Lab A - Vertical Slice Skeleton

- Created Boot, Menu, Game and Result scene flow.
- Added GameManager state machine with Playing, Paused, Won and Lost states.
- Integrated GameManager with the existing LifecycleGuard pause system.
- Added temporary hit feedback: enemy flashes white when hit.
- Searched project-authored scripts for coroutines.
- No project-authored IEnumerator / StartCoroutine usage was found.
- Enemy.cs already uses Awaitable.WaitForSecondsAsync.
- Added WaveTimer using Awaitable with a linked CancellationToken.
- Added EnemyPool with prewarm, spawn and release.
- Enemies are reused from the pool rather than destroyed.
- Pool hierarchy count remains stable during reuse.
- Typical spawning frame: ~17.09 ms.
- No Instantiate allocation observed on the selected steady-state spawning frame.
- Small allocations remained elsewhere in the frame (17 B / 48 B / 17 B).
- Saved profiler capture as:
  `docs/CA2/baseline/w05-spawner.data`
