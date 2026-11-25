WITH swaps AS (
 SELECT s.ts,LEAD(s.ts) OVER (ORDER BY s.ts) AS finish
 FROM slice s JOIN thread_track tt ON s.track_id=tt.id
 JOIN thread th USING(utid) JOIN process p USING(upid)
 WHERE p.name='com.ardabecanim.mobilegame' AND s.name='eglSwapBuffers' AND s.dur>=0
), captures AS (
 SELECT p.pid,min(s.ts) first_run,max(s.ts+s.dur) last_run
 FROM sched s JOIN thread t USING(utid) JOIN process p USING(upid)
 WHERE p.name='screencap' GROUP BY p.pid
), annotated AS (
 SELECT ts,finish,(finish-ts)/1e6 ms,
 EXISTS(SELECT 1 FROM captures c WHERE ts<c.last_run AND finish>c.first_run) overlaps_screenshot
 FROM swaps WHERE finish IS NOT NULL
)
SELECT overlaps_screenshot,count(*) intervals,round(avg(ms),3) avg_ms,
 sum(ms>33.33) over33ms,max(ms) max_ms
FROM annotated GROUP BY overlaps_screenshot;
