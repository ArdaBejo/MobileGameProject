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
- **Weapons:** Pulse Blaster, Plasma Orbitals, Homing Micro-Missiles.
- **Passive upgrades:** increased fire rate, increased damage, increased movement speed.
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

- **Device:** Google sdk_gphone64_x86_64 emulator, Android 16 / API 36, Vulkan.
- **Target frame time:** 16.7 ms at 60 fps.
- **99th percentile frame time:** under 25 ms.
- **Memory ceiling:** under 600 MB.
- **Cold start:** under 3 s to interactive.
- **APK size:** under 100 MB.
- **High FPS toggle:** no.

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

## Week 6 vertical-slice target

1 arena, 3 weapons, 3 upgrades, a 10-minute run, pause/resume

Reference: Hunicke, LeBlanc and Zubek (2004), _MDA: A Formal Approach to Game Design and Game Research_.
