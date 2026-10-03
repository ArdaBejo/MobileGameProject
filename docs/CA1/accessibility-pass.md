---
type: project
status: in-progress
lab: W02-B
unity: 6000.6.0f1
scene: Assets/Scenes/SampleScene.unity
updated: 2026-10-03
tags: [accessibility, haptics, ui, w02, no-stars-remain]
---

# Accessibility

## Goal

Show the three player settings required for the mobile game from the first build: a haptics toggle, a text-size choice and a reduce-motion option, all stored in PlayerPrefs. Alongside this, record the ten-check accessibility pass for the current version of _No Stars Remain_.

## Lab it supports

Week 2 Lab B, Parts B (haptics), C (text size) and D (accessibility pass): _Lifecycle, back, accessibility and scope lock_. Part A covers lifecycle and pause behaviour.

## What the student learns

- One haptic pulse should be used for a meaningful event, behind a setting, and never from `Update`.
- `Handheld.Vibrate` has no duration or intensity control and cannot be physically verified in the emulator.
- Text size is a multiplier on each label's own base size, applied by a component on every readable text with Auto Size off.
- Small, Normal and Large text-size options use factors of `0.85`, `1.0` and `1.25`.
- The reduce-motion setting should exist before motion effects are added, so screen shake or similar effects can respect it later.
- Every important game state should remain understandable in monochrome, silence and with haptics disabled.
- Touch targets should be at least 48 dp including padding.
- Colour should never be the only way to communicate information.
- The game should remain usable one-handed where possible.
- Accessibility checks should be repeated once the playable arena-survivor loop is implemented.

## Files

| Path                                  | Purpose                                                                                   |
| ------------------------------------- | ----------------------------------------------------------------------------------------- |
| `Assets/Scripts/Haptics.cs`           | Static haptics setting stored in PlayerPrefs. `Pulse()` sends one vibration when enabled. |
| `Assets/Scripts/HapticsSettings.cs`   | Connects the Haptics toggle in the Settings panel to `Haptics.Enabled`.                   |
| `Assets/Scripts/TextScale.cs`         | Added to readable TextMeshPro text. Applies Small, Normal and Large scale factors.        |
| `Assets/Scripts/TextScaleSettings.cs` | Connects the Small, Normal and Large buttons to the text scale values.                    |
| `Assets/Scripts/LifecycleGuard.cs`    | Handles pause behaviour when the application loses focus or is backgrounded.              |
| `Assets/Scripts/PauseMenu.cs`         | Controls the pause panel and Resume action.                                               |
| `Assets/Scripts/ReduceMotion.cs`      | Connects the Reduce Motion toggle and stores its state in PlayerPrefs.                    |
| `docs/CA1/accessibility-pass.md`      | Accessibility findings and fixes for the current build.                                   |

## How to test

1. **Editor:** Play the scene. Press Escape to open the Pause menu. Open Settings and toggle Haptics on and off. Switch between Small, Normal and Large text sizes and confirm the labels resize correctly.
2. **Emulator:** Build and Run the project. Confirm the Pause and Settings interfaces remain readable and fit inside the current SafeArea.
3. **Physical Android device:** when available, confirm that one short vibration occurs for the selected meaningful event with Haptics enabled and none occurs with it disabled.
4. **Text size:** select Large and confirm that the Pause and Settings labels remain visible without clipping.
5. **Monochromacy:** when tested on a physical Android device, use Developer options > Simulate colour space > Monochromacy and verify that no game state depends on colour alone.
6. **One-handed use:** confirm that Pause and Settings controls can be reached comfortably with one thumb.
7. **Silent use:** set volume to zero and verify that important feedback is still communicated visually.
8. Repeat the complete accessibility pass once the main _No Stars Remain_ gameplay loop is playable.

## CA1 update — 3 October 2026 (v0.3.0 project)

The original Week 2 notes and test instructions above are retained. The following results reflect the additional checks performed during CA1 preparation:

| Setting       | Current status                | Evidence and limitations                                                                                                                                                                                     |
| ------------- | ----------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Haptics       | Implemented                   | `Haptics.cs` reads/writes the `haptics` PlayerPrefs key; Haptics is connected to a toggle. Actual vibration has not been verified on a physical device.                                                      |
| Text Size     | Working in Unity              | Small, Normal and Large controls now resize the intended text after correcting text-component setup. A complete Android device accessibility pass remains outstanding.                                       |
| Reduce Motion | Toggle and preference working | `ReduceMotion.cs` stores the `reduceMotion` PlayerPrefs key. The selection was verified to persist after stopping and restarting Unity Play Mode. No motion effects are currently controlled by the setting. |

**Scope of verification:** These are editor/project checks, not a claim that every setting was retested in the signed Android APK. Any settings that depend on device hardware or future gameplay effects remain unverified.

**Data-use note:** Accessibility preferences are saved locally with PlayerPrefs. The CA1 privacy statement must mention this local settings storage; the project has not implemented account-based saves or gameplay progress saves.

## Known limits

- `Handheld.Vibrate` cannot be physically tested in the Android emulator.
- The main arena-survivor gameplay loop is not yet implemented, so enemy readability, weapon feedback and combat accessibility cannot yet be fully assessed.
- The 60-second observation test with another player will be completed once the playable loop exists.
- Reduce-motion behaviour cannot be fully tested until screen shake or other motion effects are introduced.
- Final text contrast values will be checked once the visual style and colour palette are locked.

## Cuts list

1. Screen shake will be removed if it creates accessibility or readability problems.
2. Rich haptic effects will not be implemented; a single optional vibration pulse is sufficient.
3. Decorative UI animations can be removed if they affect clarity or performance.
4. Non-essential visual effects can be reduced if they make enemies or projectiles difficult to distinguish.
5. Extra accessibility presentation effects can be deferred while preserving the core Haptics, Text Size and Reduce Motion settings.

## Decisions

- Haptics are optional and must never communicate information that is unavailable visually.
- Text size is controlled by the player using Small, Normal and Large settings.
- Combat input in _No Stars Remain_ is based primarily on movement and positioning, with weapons attacking automatically.
- Accessibility will be tested again when the first playable arena-survivor build is available.

## References

- Lab sheet: `Week 02/Lab B/W02-LabB-LabSheet.md`
- Unity docs: `Handheld.Vibrate`
- Unity docs: `PlayerPrefs`
- Android touch target size guidance
- Android accessibility principles
