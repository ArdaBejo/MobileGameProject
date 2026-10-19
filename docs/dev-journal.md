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

## Week 6 Lab A - Economy Loop

- Soft currency: Coins
- Earn condition: enemy kill
- Initial coin reward: 10 coins
- Tuned coin reward: 7 coins
- Enemy health: increased from 3 to 4
- First upgrade purchased at approximately 30 seconds
- Coins remaining at the end of the 160-second run: 400
- Total upgrades purchased: 3

The upgrade increases player damage, so the effect is immediately noticeable because enemies require fewer hits after upgrading.

The economy was tuned by reducing the coin reward and increasing enemy health so upgrades remained useful without making enemies feel completely harmless.

## 17 October 2026 — Week 6 Lab B, Part A

Added main-thread, event-based local Telemetry with an eight-character session ID, invariant timestamps/numbers, and percent-encoded metadata values. Bootstrap logs session_start once with app_version, device_model and android. Records are appended to telemetry.log in Application.persistentDataPath and sent to Debug.Log for logcat. File write/access failures warn without interrupting gameplay. SubsystemRegistration resets static session state for Editor sessions with Domain Reload disabled. No SDK or upload was added. Updated the privacy statement to disclose the new local file.

Validation: project C# build passed with zero errors/warnings using an isolated intermediate output directory. A standalone harness compiled the actual logger source with Unity metadata/logging stubs and passed checks for session deduplication/reset, invariant formatting under Turkish locale, encoded metadata, append persistence, and non-fatal file errors. These checks do not replace Unity/Android runtime verification; Android logcat/file verification remains pending.

Async Pack 1 deadline supplied by the lab: Sunday 1 November 2026, 23:59. The Moodle brief and reading links have not yet been provided or reviewed here. Event wiring, event map and wallet persistence evidence remain subsequent lab checkpoints.

### Gameplay UI and arena follow-up

At the project owner's request, replaced the crowded gameplay/economy presentation with a restrained HUD: top-centred timer, wave/kills below, coins at top-right, pause icon at top-left and a compact lower upgrade strip. The upgrade now states Pistol damage, previews current -> next damage per hit and displays its coin cost. HP is shown as a small bar above the player. Text still uses the existing text-scale preference.

The floor fills the camera viewport. Playable bounds adapt to the display and safe area, with space reserved for the header/footer controls. Edge spawning uses the new arena centre/bounds. Drag speed, wallet prices/rewards and wave tuning are unchanged.

Validation: C# build passed with zero errors/warnings. The attempted separate Unity batch smoke-test process exited before running the test; no runtime or visual pass is claimed. Temporary smoke-test assets were removed. Play Mode, text-size layouts, touch controls and new arena pacing still need user verification.

Future progression preference: after a kill threshold, level up, pause gameplay and present upgrade choices in a Vampire Survivors-style run progression. This is recorded for a later design step; today's lab retains the coin-purchase upgrade flow.

### OST checkpoint

Connected the supplied Assets/Resources/Sound/Void Runer.mp3 soundtrack to a BackgroundMusic component on persistent Boot/App. It loops at a low default volume and is not restarted by scene transitions. Playback pauses on gameplay pause, application backgrounding or focus loss and resumes when appropriate. Imported the long track as streaming audio with background loading. Source C# compilation passed; actual audibility, loop transition and Android lifecycle behaviour are pending user testing.

## Week 6 Lab B — Parts B and C implementation

Wired level_start after the loading overlay closes and the lane first enters Playing; pause/resume does not start another attempt. Per-lane attempts reset each application/Editor session. Won/Lost each emit one outcome event, with scaled run time, gross Wallet.Earn credits during the run and kills as the completion score. Current failure cause is hp. Wallet.TryUpgrade captures the actual cost before incrementing Level and logs upgrade_purchased only on success, using upgrade_id=damage to match the implemented mechanic.

Added docs/CA2/Economy_Telemetry_Map.md with the five events, typed parameters, trigger points and balancing questions. Updated the local-only privacy disclosure for the full event set. Coins from a fake pack would count in run credits if that stub is used during an attempt; the map states this explicitly.

Validation: project C# build passed with zero errors/warnings. A standalone harness compiled the actual Telemetry, Wallet and GameManager source against Unity/lifecycle stubs. It passed checks for all five events, one start across repeated pause/resume, one terminal event, failed purchases not logging, pre-increment purchase cost, earned credits independent of spending, retry increment and session attempt reset. Unity lifecycle timing and Android file/logcat behaviour remain runtime verification work.

Next device checkpoint: build/install, observe session_start plus level_start, purchase an upgrade, complete and fail attempts, and pull telemetry.log. Then verify the same coin balance and upgrade level after Home/resume, force-stop and background am kill. No device pass or telemetry evidence has been fabricated. Do not create v0.4.0-economy until the required device proof is collected. Async Pack 1 Moodle review is still pending.

### Player prefab integration

Replaced the runtime blue square with the supplied Assets/Prefabs/Player.prefab, referenced explicitly from Game/GreyboxLane. Kept its sprite, Animator, colour and authored scale. Reused the prefab's existing HealthBar/HealthFill hierarchy and rebound their transient square graphics at runtime, with a world-size bar above the character. Hurt feedback restores the prefab colour instead of forcing blue. Movement boundaries and drag acquisition account for the displayed sprite size. Included the prefab and its referenced Sprite-0002.aseprite source in this checkpoint.

Validation: C# compilation passed with zero errors/warnings. Checked the prefab's root fileID and asset GUID against the scene reference. Play Mode visual/animation and touch verification remain pending.

Player animation follow-up: horizontal movement now sets sprite facing using flipX after Animator evaluation; vertical movement/stopping preserve the previous facing. Walk animation playback is reduced to 0.8x with an Inspector setting; movement speed remains 3 units/second. C# build passed with zero errors/warnings. Runtime visual check pending. Existing user edits to Player.prefab are not included in this code-only checkpoint.

### Runtime feedback and GitHub checkpoint

The project owner reported that the current presentation and player animation now look good. Included the owner's saved Player prefab scale adjustment from 5 to 4. This is general owner runtime feedback; specific Android telemetry event/file checks and wallet lifecycle proof remain pending. The completed checkpoints are being pushed to origin/main at the owner's request.

### Part D — owner-reported wallet lifecycle verification

The project owner confirmed that the wallet was preserved in all three Android lifecycle scenarios: Home/background and resume, force-stop/relaunch, and background am kill/relaunch. Record each as an owner-reported PASS. The assistant did not independently observe the before/after balances or upgrade levels, and no numerical values are invented. Telemetry file retrieval and five-event validation are the next evidence checks.

| Scenario | Result | Evidence type |
| --- | --- | --- |
| Home/background -> resume | PASS | Owner report |
| Force-stop -> relaunch | PASS | Owner report |
| Background am kill -> relaunch | PASS | Owner report |

### Week 6 Lab B — technical completion and evidence

Pulled telemetry.log from the Android 16 emulator's app persistent-data directory. All five event names are present: session_start, level_start, level_fail, level_complete and upgrade_purchased. Example verified purchase: damage level 1, cost 100, coins_left 830. A completed run reports time_s=120.02, coins_earned=950 and score=95. The log includes two session IDs and attempts progressing within a session. Raw device evidence is included in docs/CA2/telemetry/telemetry.log as part of the combined lab-completion commit, not as a separate evidence-only commit.

Wallet persistence passed Home/resume, force-stop/relaunch and background am kill/relaunch according to the project owner's testing (see the Part D table). These are owner-reported lifecycle results; the assistant independently verified event contents in the exported log. Existing logger/event logic checks and project compilation passed earlier.

Audio issue remains open: the owner reported a one-time ring/stutter followed by loss of all audio while gameplay continued near the arena's upper boundary. A repeat attempt did not reproduce it. Logs contained focus/audio-stream stop/start records but no definitive cause; no speculative audio code change was made.

The coding, event map, privacy update and telemetry/lifecycle verification for this lab are complete. Async Pack 1 is separate reading/reflection work due Sunday 1 November 2026 at 23:59. Its actual Moodle brief and linked readings have not yet been supplied or reviewed, so the lab's Moodle brief-review step remains outstanding. Do not mark the reflection or brief review as completed. This completion checkpoint will be tagged v0.4.0-economy; the application version remains 0.4.0.

## 19 October 2026 — Week 7 Lab A, Part A provider setup

Installed/resolved Adaptive Performance and Adaptive Performance Android through Unity Package Manager. The public registry still advertises 5.1.6, but this project's Unity 6000.6.0f1 resolves both packages as built-in 6.0.0: AP core has been integrated into the engine since Unity 6.3. Aligned the manifest pins with the actually resolved 6.0.0 packages and kept Unity's generated lockfile rather than manually inventing dependency records.

Created the provider configuration under Assets/Settings/AdaptivePerformance and registered it in EditorBuildSettings. Enabled Unity 6.6's global AP feature switch, Android initialization on startup, automatic loader start, provider/global logging, and the Indexer. Kept all scaler settings disabled; thermal/performance action delays remain the defaults (10 s / 4 s). Enabled PlayerSettings Frame Timing Stats as required by the installed Android provider. No runtime performance-mode or FPS changes are included in this checkpoint.

Added explicit Tools/Performance setup and validation menu commands. These do not run automatically or overwrite later release logging choices on every import. The assets and configuration references travel with Git; a clone does not depend on editor-local selection state.

Validation: two Unity batch runs resolved/compiled the actual installed packages and executed setup. The final run exited 0 and logged [AP Setup] PASS after verifying the global feature switch, Frame Timing Stats, Android loader/startup, logging, Indexer and disabled scalers. Batch shutdown emitted a provider StopSubsystems-before-initialization warning because the Android loader was configured but no Android runtime was started; no provider-active or thermal-device claim is made. Optional Desktop Simulator setup has not been added. Next: Part B runtime thermal/bottleneck event hooks and Android provider-activity validation.

### Week 7 Lab A — Part B event hooks

Added AdaptiveBootstrap on the persistent Boot/App object through Bootstrap.Awake, matching the existing music-component setup without modifying an open scene. Start reads Holder.Instance and subscribes to thermal and performance-bottleneck changes only when the provider is active. Startup, thermal and bottleneck messages use the lab's [AP] prefix; numeric thermal values use invariant formatting. OnDestroy removes the subscriptions. Unsupported runtimes report provider inactive instead of dereferencing a missing provider.

Validation: Unity 6000.6.0f1 batch compilation and AdaptivePerformanceSetup.Validate passed with exit code 0. This verifies the installed Unity 6.6 API and configuration, not device thermal support. Android startup/activity and event evidence remain pending; the Balanced/High FPS controller and settings toggle are the next checkpoint.

### Week 7 Lab A — Part C performance mode and settings toggle

Added persistent PerformanceMode on Boot/App. It restores perf_mode before Menu loads, applies Balanced (60 fps, render scale 1.0) or HighFps (120 fps, 0.8), saves explicit changes, disables vSync and logs each application. Removed the scene-local forced 60 fps assignment; direct Game-scene testing creates a separate persistent controller. Editor teardown restores the original URP asset scale. Set the authored URP render scale from 0.5 to the lab's Balanced default 1.0.

Settings navigation creates a High FPS uGUI Toggle row at runtime inside the existing panel (650 x 64 reference pixels, between text-size choices and Back). It displays ON/OFF, connects the dynamic bool to SetHighFps and uses SetIsOnWithoutNotify on opening. Existing scenes require no saved-file edits. Added the local perf_mode preference and diagnostic logs to the privacy statement.

Validation: Unity batch compilation and AdaptivePerformanceSetup.Validate passed, exit code 0. Static inspection confirms only PerformanceMode assigns targetFrameRate. Runtime UI layout, touch interaction, preference persistence and actual Android frame rate/render-scale evidence still require Play Mode/device checks. A 120 fps target is a request; device refresh rate and system policy can cap the result.

### Week 7 Lab A — emulator verification and evidence checkpoint

**Test environment: Android Emulator (emulator-5554), Google sdk_gphone64_x86_64, Android 16 / API 36, 1080 x 2400, 60 Hz virtual display.** All Week 7 measurements, AP events and mode screenshots in this checkpoint were captured on the emulator.

Saved owner-supplied Settings screenshots for High FPS ON/OFF and an ADB logcat excerpt from test PIDs 25592/25744. Both target/render-scale combinations are logged, AP reports active and thermal/bottleneck events, and a new process restores HighFps. Added perf-modes.md with approximately 30 seconds of samples per mode: Balanced 16.74 ms mean, highest window p99 16.67 ms; High FPS 16.83 ms mean, highest window p99 33.33 ms. Window choices, pause-sampling limitations and the distinction from a pooled p99 are documented.

The connected emulator has a 60 Hz display; this is not physical-phone thermal or 120 Hz verification. Settings screenshots prove toggle states but contain no performance overlay, so the lab's overlay-visible screenshot requirement remains open. Installed APK signing/Development Build status was not verified. Moodle exit ticket remains pending. Include Unity-generated Basic/Simulator settings assets and their registrations plus Android provider preloaded references so the current project configuration has no missing GUID references on a fresh checkout; no new provider selection was made in this checkpoint.

Current status: Parts A–C are implemented; Part D has emulator verification and Part E has the emulator comparison, log excerpt and Settings screenshots. The latest evidence supersedes earlier entries that described runtime checks as pending. Next steps are overlay-visible evidence, physical-phone verification when available, and the owner-submitted Moodle exit ticket. Week 7 Lab B's thermal soak is a separate next lab; emulator AP readings do not demonstrate physical thermal behaviour.

### Week 7 Lab A — on-screen performance readout

Added a non-interactive overlay to the persistent PerformanceMode controller, visible only in Game. It uses the existing safe-area canvas layout and shows a half-second real-time frame average (FPS and milliseconds), selected mode, requested target and active URP render scale. Pause/loading/background intervals are excluded from new samples; pausing preserves the last gameplay readout for screenshots. Changing mode clears the sample and requests Resume before measuring the new mode. Existing FrameTimeSampler logcat evidence is unchanged.

Validation: git diff --check passed and source/API references were inspected. A separate Unity batch validation could not run with the project already open in the owner's Editor; no new compilation or Android runtime success is claimed. Next: let the open Editor import/compile, build Android, play briefly after each mode switch and capture fresh overlay-visible screenshots.
