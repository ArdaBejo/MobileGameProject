# Timed Awaitable and pause audit

25 November 2026, Unity 6000.6.

| Owner | Await | Pause / lifetime policy |
|---|---|---|
| WaveTimer | Scaled spawn interval | ResumeGuard cancels on every PausedChanged(true), destroys the old source, and starts one fresh interval on false. Repeated identical state changes do nothing. Linked to destruction and application exit. No accumulated offline spawns. |
| Enemy | 0.1-second hit flash | Pause cancels the cosmetic flash and restores normal colour immediately. It is not replayed on resume. Pool disable, destruction and exit also cancel. Damage and kill credit happen before the flash await. |
| FakePurchaseProvider / CoinShop | 0.5-second fake purchase | Caller provides linked destroy/exit token. Pause and disable cancel the pending request. Cancellation is caught and no coins are credited. Repeated taps cannot start overlapping requests. The shop is currently hidden. |
| GameManager | NextFrame while loading | Scene initialization, not a gameplay countdown. Lifetime token cancels on destruction; it can finish while paused, but chooses Paused state rather than starting active play. |
| SceneLoader / Bootstrap | NextFrame, async activation | Navigation deliberately continues while gameplay is paused, to prevent hanging at the loading overlay. Application exit token and existing loading guard apply. |
| StoreShot | EndOfFrame | Development screenshot capture deliberately works with a paused scene. Destroy token and finally restoration of canvases protect the capture. It is not a gameplay timer. |
| GreyboxLane / RunWeapons / RunProgression | Time-based Update cooldowns | Every Update path requires lane.Playing. Time.time and deltaTime remain frozen while paused. No independent asynchronous loop to restart. |

LifecycleGuard saves actual wallet state for Home, focus loss, Back, pause button and level-up. It reloads the saved wallet before notifying resume subscribers. Returning from Android does not automatically resume play; the player explicitly chooses Resume. Current run health, enemies and cooldowns remain in memory during ordinary backgrounding. Process death returns to Menu, retaining wallet and settings; it does not promise restoration of a partly played run.

No offline rewards exist, so no UTC delta or retroactive spawn catch-up is applied. No demo boss was added to the shipping game; the real wave spawner uses the guard directly.

API references:
- https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Awaitable.WaitForSecondsAsync.html
- https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Application-exitCancellationToken.html
- https://docs.unity3d.com/6000.6/Documentation/ScriptReference/MonoBehaviour-destroyCancellationToken.html
