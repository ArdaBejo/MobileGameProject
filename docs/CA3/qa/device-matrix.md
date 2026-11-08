# Week 8 device matrix

| Phone / test target | SoC | RAM | Android | Panel Hz | Tier | Owner | Builds tested |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Android Emulator, Pixel 8 profile | Virtual x86-64 host CPU | Unverified | 16 / API 36 | 60 virtual | N/A (emulator) | Self | 0.8.0 installed. Startup showed a Unity Graphics API error. |
| Lenovo (model pending) | Pending | Pending | 16 / API 36 | Pending | Pending | Anonymous P1 | 0.8.0 installed and launched (owner report) |
| Lenovo (model pending) | Pending | Pending | Pending | Pending | Pending | Anonymous P2 | 0.8.0 tested (owner report) |

The emulator does not count as a peer phone. Populate P1/P2 only from real devices and record unsupported devices or install failures as results. Use an anonymous code rather than names or contact details.

## Peer sessions

| Peer code | Build ID | Install outcome | Minutes played | First 30 seconds: action or hesitation | Form submitted |
| --- | --- | --- | --- | --- | --- |
| P1 | 49ea902f (reported) | Installed first try and launched | 2–3 | Pending | Submitted with consent |
| P2 | 49ea902f (reported) | Installed first try and tested | 2–3 | Pending | Submitted with consent |

P1 liked the game and found it too easy, as relayed by the project owner.

P2 asked for a Restart option in the Pause menu, as relayed by the project owner.

The project owner confirmed that both peers agreed to anonymous use of their feedback for the university project.

A missing `session_end` is not a verified crash: Android can kill the process without calling Unity's quit callback. Report crashes, ANRs and soft locks from observed outcomes.
