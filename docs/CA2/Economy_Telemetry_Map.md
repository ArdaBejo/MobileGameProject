# Economy telemetry map

No Stars Remain — Week 6 Lab B, 17 October 2026

| Event | Parameters (type) | Fires when | Balancing question |
| --- | --- | --- | --- |
| `session_start` | `app_version`, `device_model`, `android` (strings) | Once in Boot per application/Editor session | Which build and device produced the records? |
| `level_start` | `level_id`, `attempt`, `coins` (ints) | Lane first becomes playable after loading; resumes do not repeat it | How many attempts are made, and with what entry wealth? |
| `level_complete` | `level_id` (int), `time_s` (float), `coins_earned`, `score` (ints) | Once on entering Won | How long is a successful run; how many coins and kills does it produce? |
| `level_fail` | `level_id` (int), `time_s` (float), `cause` (id), `coins_earned` (int) | Once on entering Lost; current cause is `hp` | When do players die, and how much currency have they earned? |
| `upgrade_purchased` | `upgrade_id` (id), `level`, `cost`, `coins_left` (ints) | Successful Wallet.TryUpgrade spend; current upgrade is `damage` | When can the first upgrade be bought, and how does spending affect wealth? |

Each record has elapsed runtime and an eight-character random session ID. Attempts increment per lane within that session. `time_s` uses scaled gameplay time since level_start, excluding pauses; `score` is kills. `coins_earned` counts Wallet.Earn credits during the run, not the net balance after purchases; a fake coin pack would also count if used during the run. Purchase cost is captured before increasing the upgrade level. Metadata values are percent-encoded as single tokens; numeric formatting is locale-independent.

Data stays in Unity logs/logcat and `telemetry.log` under `Application.persistentDataPath`; the game does not upload it or use an analytics SDK. Manual export is used for coursework evidence, and local storage/retention are disclosed in `docs/CA1/privacy-statement.md`.
