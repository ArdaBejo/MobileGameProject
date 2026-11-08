# Week 8 device matrix

| Phone / test target | SoC | RAM | Android | Panel Hz | Tier | Owner | Builds tested |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Android Emulator, Pixel 8 profile | Virtual x86-64 host CPU | Unverified | 16 / API 36 | 60 virtual | N/A (emulator) | Self | 0.8.0 installed. Startup showed a Unity Graphics API error. |
| Peer 1 (model pending) | Pending | Pending | 16 / API 36 | Pending | Pending | Anonymous P1 | 0.8.0 installed and launched (owner report) |
| Peer 2 | Pending | Pending | Pending | Pending | Pending | Anonymous P2 | Pending |

The emulator does not count as a peer phone. Populate P1/P2 only from real devices and record unsupported devices or install failures as results. Use an anonymous code rather than names or contact details.

## Peer sessions

| Peer code | Build ID | Install outcome | Minutes played | First 30 seconds: action or hesitation | Form submitted |
| --- | --- | --- | --- | --- | --- |
| P1 | 49ea902f (reported) | Installed and launched (owner report) | Pending | Pending | Verbal feedback recorded. Form submission pending. |
| P2 | Pending | Pending | Pending | Pending | Pending |

P1 liked the game, as relayed by the project owner.

Record the two reciprocal peer builds tested separately, with build ID and observed result. A missing `session_end` is not a verified crash: Android can kill the process without calling Unity's quit callback. Report crashes, ANRs and soft locks from observed outcomes.
