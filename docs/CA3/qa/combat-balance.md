# Combat balance — 25 November 2026

This is an arena-specific first tuning pass, inspired by the separation of Max Health, weapon base damage and percentage Might in Vampire Survivors. Its original numbers are not copied.

| Stat | Value |
|---|---|
| Player health | 100 |
| Enemy health on spawn, waves 1 / 2 / 3 | 30 / 45 / 60 |
| Contact damage, waves 1 / 2 / 3 | 12 / 16 / 20 |
| Pistol levels 1–5 | 14 / 18 / 22 / 26 / 30 |
| Sword levels 1–5, area sweep | 26 / 32 / 38 / 44 / 50 |
| Rocket levels 1–5, blast | 42 / 52 / 62 / 72 / 82 |
| Damage passive | +20% base damage per rank, maximum +60%, rounded |
| Paid pistol bonus | +2 base damage, maximum five purchases (+10) |
| Spawn intervals, waves 1 / 2 / 3 | 1.65 / 1.25 / 0.95 seconds |

Weapon values in the table exclude paid bonuses and Might. Might multiplies both the pistol base and its paid bonus. At zero upgrades the pistol needs 3 / 4 / 5 hits per enemy. Contact requires 9 / 7 / 5 hits to defeat a full-health player; the existing one-second immunity still applies. Shield still blocks a whole hit. No automatic heal was introduced.

Health scaling is fixed when each pooled enemy activates; advancing a wave does not heal an enemy already on screen. HUD previews and projectiles share CombatBalance. Legacy wallet ranks above five retain their saved value and coins, but damage is capped and further purchases disabled. Restart clears the wallet and all run upgrades using the existing restart behavior.

Opening enemies take fewer pistol hits than before. The final-wave spawn jump is smaller; stronger enemies still encourage sword and rocket choices. These are tuning hypotheses, not a claim that player difficulty feedback has been collected.

References: https://vampire.survivors.wiki/w/Max_Health and https://vampire.survivors.wiki/w/Might (community documentation, consulted for the stat structure).

Validation: full gameplay assembly compiles with the Unity 6000.6 toolchain. Android runtime checks follow in the Week 11 evidence.
