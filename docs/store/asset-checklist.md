# Store asset checklist

Specs checked against the Play *Add preview assets* help page on 25 November 2026. The project uses the Week 10 Lab A screenshot target of 9:16, with each side at least 1080 px. [Google Play requirements](https://support.google.com/googleplay/android-developer/answer/9866151)

| File | Required spec | Verified export | Result |
| --- | --- | --- | --- |
| icon-512.png | 512×512, 32-bit PNG, ≤1024 KB | 512×512, RGBA PNG, 2,232 bytes | Pass |
| feature-1024x500.png | 1024×500, 24-bit PNG or JPEG, no alpha | 1024×500, RGB PNG, 18,108 bytes | Pass |
| screenshots/01-auto-fire.png | 9:16, sides 1080–3840 px, RGB PNG, HUD hidden | 1080×1920, RGB PNG | Pass |
| screenshots/02-drag-right.png | 9:16, sides 1080–3840 px, RGB PNG, HUD hidden | 1080×1920, RGB PNG | Pass |
| screenshots/03-break-away.png | 9:16, sides 1080–3840 px, RGB PNG, HUD hidden | 1080×1920, RGB PNG | Pass |
| screenshots/04-enemy-pressure.png | 9:16, sides 1080–3840 px, RGB PNG, HUD hidden | 1080×1920, RGB PNG | Pass |
| Screenshot count | Lab 4–6, promotion at least three portrait 9:16 shots | Four portrait 9:16 shots | Pass |
| Short A / Short B | ≤80 characters each | 76 / 76 | Pass |
| Long description | ≤4,000 characters | 793 | Pass |

The icon is the astronaut portrait supplied by the project owner, resized with nearest-neighbour sampling and its transparency preserved. It has no baked rounded corners. The feature graphic uses the real astronaut, moon-bat and deck assets, with the title and characters inside the central area.

The assistant inspected the icon at 48×48 and the screenshot set visually. The owner supplied the preferred icon after reviewing earlier versions. Another person's two-second icon-recognition check remains a human critique activity.

## Android capture

Captured on 25 November 2026, Pixel 8 Android Emulator, Android 16 / API 36. Development build dada9b08, package com.ardabecanim.mobilegame.storeqa, Auto Low. Original display 1080×2400. StoreShot hides all enabled UI canvases and the player's health bar, waits until end of frame and exports the central 1080×1920 pixels as RGB PNG. This excludes the bottom-right watermark. The final PNGs are the files pulled from the Android player, without retouching.

| File | Gameplay moment | Feature communicated |
| --- | --- | --- |
| 01-auto-fire.png | Astronaut and projectile with an enemy approaching from the right | Automatic pistol fire in the bounded arena |
| 02-drag-right.png | Player moved towards the lower-right area with enemies entering above | Direct drag movement and space management |
| 03-break-away.png | Player moved up the left side while several enemies follow | Evading pursuit |
| 04-enemy-pressure.png | Player moved across the upper arena with enemies approaching below and above | Repositioning between enemies |

Shot 02 from the raw capture set was excluded because it repeated a nearby movement moment and caught an enemy during its dark flash. [Capture log](screenshots/capture-log.txt)

## Lab B handoff

Preferred critique screenshot is 03-break-away.png. Short variants A and B are in descriptions.md. The Moodle exit ticket has not been posted.
