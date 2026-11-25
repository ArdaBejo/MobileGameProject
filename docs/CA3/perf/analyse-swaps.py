"""Recompute pacing-summary.json from trace_processor CSV. Python standard library only."""
import argparse, csv, json, math
from pathlib import Path
root = Path(__file__).resolve().parent
parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument("--csv", default="egl-swaps.csv", help="CSV path relative to this script")
parser.add_argument("--output", default="pacing-summary.json", help="JSON path relative to this script")
args = parser.parse_args()
rows = list(csv.DictReader((root / args.csv).open(encoding="utf-8-sig")))
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
(root / args.output).write_text(json.dumps(summary, indent=2), encoding="utf-8")
print(json.dumps(summary, indent=2))
