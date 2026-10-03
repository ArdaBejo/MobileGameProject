# MDA One-Pager - No Stars Remain

**Module:** Mobile Game Development (A12581) · **Student:** Arda Becanım, 20120739 · **Project option:** 1 – Arena-Survivor Roguelite · **Due:** Wed 16 Sep 2026 (Week 2 Lab B)

## One-line pitch

A lone space survivor fights through increasingly hostile alien swarms on a dying planet, building an arsenal of automatic weapons and upgrades to survive a ten-minute run.

## Aesthetics (what the player feels)

- Isolation and tension in a hostile sci-fi environment.
- Growing power and mastery as weapons and upgrades become stronger.
- Relief and satisfaction from surviving increasingly dangerous enemy waves.

## Core mechanics (3 to 5 verbs or systems)

1. Move and position using a one-thumb virtual stick.
2. Automatic weapon attacks against nearby enemies.
3. Collect experience and level up during the run.
4. Choose weapons and passive upgrades at level-up.
5. Survive increasingly dense and powerful enemy waves.

## Dynamics (what emerges when the mechanics meet the player)

- Players continuously reposition to avoid being surrounded while automatic weapons attack.
- Different weapon combinations encourage different movement and positioning strategies.
- Increasing enemy density forces the player to balance survival with collecting experience.
- Upgrade choices gradually create a more powerful build over the course of the run.

## Progression & content

- **Session length:** 10 minutes per run.
- **Content in the vertical slice (by Week 6):** 1 arena, 3 weapons, 3 upgrades, a 10-minute run, pause/resume.
- **Weapons (revised for CA1):** Pistol (automatic ranged shots), Laser Sword (automatic close-range attacks), Rocket Launcher (periodic explosive attacks).
- **Passive upgrades (revised for CA1):** Damage Up (increased weapon damage), Speed Up (increased movement speed), Shield (reduced incoming damage).
- **Content by CA3:** 1 polished arena, 1 boss encounter, improved enemy variety, weapon feedback and balancing.
- **Stretch goals:** additional planets/arenas and additional bosses.

## Platform features (Android)

- **Touch model:** one-thumb virtual stick; combat is automatic.
- **Safe areas and orientation:** portrait orientation with UI kept inside the safe area.
- **Haptics:** optional short vibration when the player is hit, controlled by a Settings toggle.
- **Text size:** Small / Normal / Large settings.
- **Reduce motion:** optional setting for screen shake and other motion effects.
- **Lifecycle:** pause/resume and focus loss handled from Week 2.
- **Store / testing tracks:** awareness only, no uploads.

## Performance budget (your device)

- **Device:** Google sdk_gphone64_x86_64 emulator, Android 16 / API 36; graphics API for the Week 3 baseline: OpenGLES3.
- **Target frame time:** 16.7 ms at 60 fps.
- **99th percentile frame time:** under 25 ms.
- **Memory ceiling:** under 600 MB.
- **Cold start:** under 3 s to interactive.
- **APK size:** under 100 MB.
- **High FPS toggle:** no.

### Week 3 profiling evidence (earlier build, not v0.3.0)

- **Baseline recorded 26 Sep 2026:** average steady-gameplay frame time 17.08 ms; p99 33.33 ms (above the 25 ms target); worst-case average 17.72 ms.
- **Memory:** Android TOTAL PSS 364.2 MB; Unity peak total reserved approximately 0.59 GB (close to the 600 MB target; different measurement from PSS).
- **APK size:** 31.93 MB (below the 100 MB target). Median cold-start measurement: 844 ms; time to first interactive screen was not separately recorded.
- **Worst-frame investigation:** a 62.03 ms main-thread frame, with 61.64 ms under PlayerLoop and 19.70 ms under PostLateUpdate.FinishFrame. Reducing render scale from 1.0 to 0.5 improved the observed frame time from 17.92 ms to 16.72 ms (~6.7%), suggesting CPU-side work should be investigated first, although the test does not conclusively isolate the bottleneck.
- **Risk and next step:** frame-time spikes exceed the p99 budget. Re-profile the playable build and investigate frame completion, physics and avoidable allocations before adding more enemies or visual effects.
- **Evidence:** `docs/CA2/baseline-sheet.md`, `docs/CA2/bottleneck-01.md`, `docs/CA2/w03-profile.data`, `docs/CA2/w03-bad-frame.png`, `docs/CA2/w03-gpu-pass.png`, and the Week 3 sampler log.

## Monetisation (if any) & ethics notes

- No monetisation will be implemented for the coursework build.
- If published, the preferred model would be a one-time purchase or optional cosmetic content.
- No loot boxes, pay-to-win upgrades, forced advertisements or timers designed to pressure spending.

## Risks & cuts list (in the order they get cut)

1. Additional planets / arenas.
2. Additional bosses beyond the first boss.
3. Extra weapon types beyond the required three.
4. Extra enemy types and decorative combat effects.
5. Screen shake and non-essential visual effects.
6. Additional upgrade types beyond the required three.

The core movement mechanic, automatic combat, pause/resume path and Android device build will not be cut.

## Scope lock

- **Locked on:** Wed 16 Sep 2026
- **Changes after lock** require a note in the development journal explaining what changed and why.
- **3 Oct 2026 CA1 revision:** simplified the three weapon names and three passive upgrades to make the implementation and player-facing explanation clearer; kept the number of weapons/upgrades, core mechanics and ten-minute run unchanged.

## Week 6 vertical-slice target

1 arena, 3 weapons, 3 upgrades, a 10-minute run, pause/resume

Reference: Hunicke, LeBlanc and Zubek (2004), _MDA: A Formal Approach to Game Design and Game Research_.
