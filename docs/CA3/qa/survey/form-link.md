# No Stars Remain closed test, build 0.8.0

Status: Google Forms published, responder access set to anyone with the link, email collection off, and sign-in restriction off. Verified the responder screen and all question types. The live responder link is stored locally in the ignored Assets/Resources/PeerFeedbackUrl.txt and must be shared only with the closed test. The editor URL is never distributed.

The form must accept anonymous responses without collecting names or emails. Validate the respondent page while signed out and on an Android test device. Use a marked smoke response, remove it before peer collection, and record both checks.

| Order | Field | Response |
| --- | --- | --- |
| 1 | Device model and Android version | Matrix choices + Other text |
| 2 | Build ID shown on About | Short text; copy exact ID, or unavailable if app cannot open |
| 3 | Install | Worked first time / Needed help / Failed |
| 4 | If installation failed, what did the phone say? | Optional text |
| 5 | Minutes played | Nonnegative number; 0 for no play |
| 6 | Crash, freeze or not responding seen? | None / Freeze / Crash / Not responding / Not tested |
| 7 | When did it happen, and what were you doing? | Optional text |
| 8 | Feel | 1–5 grid: smoothness, heat, battery drain, control responsiveness |
| 9 | Loop | 1–5 grid: understood goal, wanted one more go, difficulty felt fair |
| 10 | One thing to fix | Text |
| 11 | One thing to keep | Text |
| 12 | Consent | Required checkbox: anonymous university project use |
