"""Recompute pacing-summary.json from trace_processor CSV. Python standard library only."""
import csv, json, math
from pathlib import Path
root = Path(__file__).resolve().parent
rows = list(csv.DictReader((root / "egl-swaps.csv").open(encoding="utf-8-sig")))
times = [int(row["ts"]) for row in rows]
intervals = [(b - a) / 1e6 for a, b in zip(times, times[1:])]
assert intervals and all(value > 0 for value in intervals)
ordered = sorted(intervals)
summary = {
    "method": "consecutive eglSwapBuffers entry timestamps on app render thread; submission cadence, not GPU execution or display presentation",
    "swapEvents": len(times), "intervals": len(intervals),
    "spanSeconds": (times[-1] - times[0]) / 1e9,
    "avgMs": sum(intervals) / len(intervals),
    "p99Ms": ordered[math.ceil(.99 * len(ordered)) - 1],
    "maxMs": max(intervals),
    "over16_7ms": sum(value > 16.7 for value in intervals),
    "over33_33ms": sum(value > 33.33 for value in intervals),
}
summary["over16_7Percent"] = 100 * summary["over16_7ms"] / len(intervals)
(root / "pacing-summary.json").write_text(json.dumps(summary, indent=2), encoding="utf-8")
print(json.dumps(summary, indent=2))
