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

## 12 October 2026 — Week 5 Lab B, Part A implementation

Lane definition: one fixed, bounded 2D arena with three 40-second survivor waves. The player moves with a virtual joystick (WASD/arrows for Editor testing) and an automatic pistol. Survive 120 seconds to win; losing all five HP ends the run. No boss, XP or upgrades are included in this lab step.

- Added runtime greybox setup to Game: flat floor/boundaries, blue player, red enemies, yellow shots, fixed portrait camera and safe-area gameplay HUD.
- Reused EnemyPool and the Awaitable WaveTimer. Enemies spawn near the arena edges, chase the player and deal contact damage with one second of player invulnerability.
- Added three increasing wave stages, kill count, countdown, shot/hit audio stub, enemy hit flash, player hit flash and optional haptics.
- Prewarmed 60 enemies and 32 reusable pistol shots. Projectile collision checks sweep the travelled segment.
- Added Result outcome, survival time, kills, Retry and Main Menu. Scene transitions still use the existing loading path until Part B.
- The arena, player and UI are generated during Play Mode by GreyboxLane; parameters and scene references are visible in the Inspector.

Validation: initial C# build passed with no errors or warnings. Play Mode/device testing and difficulty tuning are pending. No new performance measurements or neighbour-playtest observations have been recorded. Parts B–E remain pending, and no commit or tag was created by the assistant.

### Part A playtest follow-up

The project owner reported that the gameplay worked in Play Mode, including the implemented flow, but the playable arena felt small. Enlarged the bounded arena from 7.2 x 9.6 to 8.2 x 11.6 world units and kept the camera fixed, with aspect-ratio fitting to preserve the boundaries. HP and current combat values remain unchanged.

Future UI preference: place the timer at the top and display health bars above characters. This is recorded for a later UI pass, not implemented in this step. External neighbour playtest and device verification are still pending.

### Part A touch-control revision

After the Android build ran successfully, the project owner reported that joystick gestures sometimes opened Android's bottom system navigation. Replaced the joystick with direct player dragging at the owner's request: touch near the blue player to grab it, follow the finger without an offset, release to stop. Movement is clamped at the arena boundary. A second finger cannot take over an active drag; UI touches cannot start movement, and pause/focus changes clear the drag. Mouse dragging and WASD/arrows remain available for Editor testing. Direct dragging follows finger position rather than the keyboard movement-speed limit; difficulty may need rebalancing after a device test.

The scope change preserves automatic combat, three waves and the run duration. Android verification of the revised controls is pending.

Drag balance correction: the project owner found direct position following too fast. Drag now sets a target and moves toward it at the same 3 world units/second limit as keyboard movement. No finger offset is applied; releasing still stops immediately. Device retest pending.

Part A layout follow-up: enlarged the fixed arena to 9.2 x 12.8 world units and adjusted camera fitting. Replaced the bottom Pause text button with a 72 x 72 pause icon button at the top-left inside the safe area. Play Mode/device layout check pending.

## 12 October 2026 — Week 5 Lab B, Part B implementation

- Added a singleton SceneLoader component to Boot/App with DontDestroyOnLoad. Its serialized child LoadingCanvas uses Screen Space Overlay, high sort order, a dark input-blocking Image and a non-interactable Slider with no handle.
- Routed Bootstrap -> Menu, Start -> Game, Win/Lose -> Result, Retry -> Game and Main Menu -> Menu through the same Awaitable Load method.
- Normalized real AsyncOperation.progress by 0.9. The loop waits for progress < 0.9, releases allowSceneActivation and then awaits activation. No fake loading delay was added.
- Guarded duplicate requests, blocked gameplay/Escape pause while loading and added cleanup for errors/cancellation so activation cannot remain locked. Only the application exit token is passed; focus/background changes do not cancel it.
- Added Stopwatch scene-load logging for later measurements. Logged duration includes the overlay presentation and destination initialization frames.
- Existing EnemyPool prewarm remains in Awake and shot/UI setup in Start while the overlay is visible. Optional prewarm before activation is not implemented.

Validation: C# build passed with zero errors/warnings; Boot scene document/component references and Slider script GUID were checked. Runtime overlay visibility, destination transitions and singleton persistence still require Play Mode/device testing. No new load-time figures were claimed. The intentional hang and Android Home/Back resilience exercises remain for Part C. No commit or tag was created by the assistant.

## Week 5 Lab B — Part C activation-trap experiment

The project owner ran the temporary Editor-only hang experiment and confirmed that Loading stayed at 100% without activating Game. The experiment used allowSceneActivation=false and while (!operation.isDone), demonstrating that progress stops at 0.9 while isDone remains false. The temporary experiment code was then removed; the loader retains only while (operation.progress < 0.9f) followed by activation. Normal-flow retest and Android mid-load Home/Back tests remain pending.

### Part C verification follow-up

The project owner confirmed that normal loading worked after removal of the hang experiment and that both Android resilience tests were performed successfully: Home mid-load, wait five seconds and return; Android Back during loading. These are owner-reported results, not assistant-observed tests.

## Week 5 Lab B — Part D preparation

Connected device inspected via Unity's bundled ADB: emulator-5554, sdk_gphone64_x86_64, Android 16. Installed package com.ardabecanim.mobilegame reports versionName 0.4.0 / versionCode 4. Launch component was resolved as com.ardabecanim.mobilegame/com.unity3d.player.UnityPlayerGameActivity.

Added MenuReady to log [perf] interactive once, on the first Menu Update after SceneLoader stops blocking input. MainMenuManager attaches it at runtime. SceneLoader already logs Stopwatch duration. C# build passed with zero errors/warnings. A fresh Android build/install is required before collecting all measurements from the same instrumented APK. No new cold-start or interactive figures are recorded yet.

### Part D measured results

Collected three force-stopped cold launches on Android 16 emulator-5554 using the rebuilt v0.4.0 Release IL2CPP ARM64 APK. Installed APK SHA-256 matched Builds/MyGame-0.4.0.apk. Android TotalTime: 3209 / 3469 / 2733 ms, median 3209 ms. Unity MenuReady interactive markers: 9.44 / 8.05 / 8.01 s, median 8.05 s. Boot -> Menu Stopwatch durations: 5416 / 3958 / 4023 ms, median 4023 ms. APK size: 35,674,041 bytes (35.67 MB / 34.02 MiB). Raw launch/perf evidence and a reusable measurement script are saved in docs/CA2/baseline.

The 8.05 s interactive marker exceeds the existing 3 s MDA target. No cause is claimed from these timings alone; startup/activation profiling on a comparable configuration is the next performance task. The old Week 3 timing is a different-build baseline. Menu -> Game measurement is pending a Start Game input in the third measured process. External neighbour playtest remains pending.

Part D completion: captured Menu -> Game load of 1513 ms in PID 22403 (one sample, not a median). Updated the Baseline Sheet and raw/JSON evidence. Part D measurements are complete; startup and transition targets are exceeded, while APK size is within budget.

## Week 5 Lab B — Part E playtest feedback

The project owner relayed feedback from other players: the game was understandable, but currently felt quite simple. Wave 1 felt too easy while Wave 3 felt too difficult. The project owner confirmed that the external playtest lasted two minutes.

Two observations:
1. The current greybox content felt basic. This is feedback about content depth, not evidence that controls or objectives were confusing.
2. The difficulty ramp was uneven: little pressure in Wave 1, excessive pressure in Wave 3.

No specific comprehension confusions were reported; do not invent two to satisfy the worksheet wording. The fixed arena, automatic pistol and survival loop were reported as understandable.

First planned change: smooth the difficulty ramp by slightly increasing Wave 1 pressure and reducing Wave 3 pressure, then retest before choosing final values. Keep the three 40-second waves and 120-second run. No boss, extra weapons or progression systems are added in this lab step. Gameplay values have not yet changed in response to this feedback.

Lab E result: the external playtest lasted two minutes; no comprehension confusions were reported. The two actual observations above are recorded instead of invented confusions. The student will commit and create the worksheet tag v0.3.1-lane; the project version remains 0.4.0. No commit or tag has been created by the assistant.
