# No Stars Remain — Data-Use and Privacy Statement

**Version:** 0.3.0  
**Module:** Mobile Game Development  
**Status:** CA1 Prototype

## 1. Overview

No Stars Remain is a single-player Android game developed in Unity 6.6. The current prototype is intended for academic assessment and is not available on the Google Play Store.

## 2. Data Collection and Usage

The current prototype does not intentionally collect, process, transmit or share personal user information.

The game does not require account registration, email addresses, location information or access to personal files.

No advertising, third-party analytics SDK or user-tracking service has been implemented. Local diagnostic telemetry records startup, gameplay outcomes and successful upgrades for coursework balancing tests.

## 3. Local Data Storage

The current prototype uses Unity PlayerPrefs to store three accessibility preferences locally on the device: Haptics (on/off), Text Size (Small, Normal or Large), and Reduce Motion (on/off). These preferences persist between application sessions and are not intentionally transmitted to a server. The Reduce Motion preference is saved, but motion-reduction effects have not yet been implemented.

The prototype also uses Unity PlayerPrefs to store limited gameplay progress locally on the device:

- `wallet.coins` — stores the player's current soft-currency balance.
- `wallet.level` — stores the player's current upgrade level.

These values persist between application sessions and are not transmitted to a server.

Local diagnostic telemetry is also written to Unity logs/logcat and `telemetry.log` in `Application.persistentDataPath`. Startup records include app version, device model, operating system, elapsed runtime and a random session ID generated for each application session. This ID is not a permanent user or device identifier. The game does not upload these records. The developer may manually export the file for coursework evidence. The log currently appends across launches without automatic rotation; it can be removed by clearing app data or uninstalling the app.

The event names are `session_start`, `level_start`, `level_complete`, `level_fail` and `upgrade_purchased`. Gameplay parameters include lane and attempt numbers, run duration, a fixed failure-cause ID, coin balances/credits, kill score, upgrade ID/level and the amount spent. No player-entered free text is logged. The event definitions are documented in `docs/CA2/Economy_Telemetry_Map.md`.

## 4. Network Communication

The game does not implement multiplayer functionality, online accounts, cloud storage or external API connections.

The intended gameplay is entirely offline.

## 5. Third-Party Services

Unity is used as the game development engine. No additional advertising or analytics services have been intentionally integrated.

The final build's Unity packages, permissions and network behaviour will be reviewed before publication.

## 6. Google Play Data Safety

If the game is published on Google Play, its actual data collection, processing and sharing practices will be reviewed and declared accurately in the Google Play Data safety form.

A publicly accessible privacy policy will also be provided when required.

Any future features involving analytics, advertising, online services or personal information will require this statement and the corresponding Data safety declarations to be updated.

## 7. Publication Status

No Stars Remain has not been submitted to any application store. This statement documents the current prototype's intended runtime behaviour and demonstrates awareness of future publication requirements.
