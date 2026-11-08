# Peer test builds

| Version | Version code | Build ID (About) | APK SHA-256 | Closed-test download |
| --- | ---: | --- | --- | --- |
| 0.8.0 | 5 | 49ea902f | 13f23d02ca840188ef57b067e56db619f2e79d6a10bbb38a8e6ef04963c16e18 | Drive, anyone with link can view. URL and QR held locally for the closed test. |

Publish only the final release-signed ARM64 IL2CPP APK with Development Build off. In Unity, use **Tools > Distribution > Build Signed Peer APK** after setting the responder URL and entering the existing keystore and alias passwords in Android Publishing Settings. Unity outputs `Builds/NoStarsRemain-0.8.0-arm64.apk` and logs its GUID and SHA-256. Confirm the signer matches the previous release, verify version and architecture from the APK, then sideload and match the displayed and logged build ID. Upload that exact file to Drive; share it as Viewer with anyone who has the link, with search discovery off. Generate the QR from the verified responder-facing APK download link. Do not distribute the APK through a public forum.

Every new binary gets a new Unity build ID. Update the hash, QR and matrix before circulating a replacement.

The uploaded APK was downloaded without signing in. Its 37,669,058 bytes and SHA-256 match the local artifact. The 540 × 540 QR decodes to the exact Drive viewer URL. The release signer matches the previous release.
