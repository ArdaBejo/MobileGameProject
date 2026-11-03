using System;
using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

// The adjacent performance readout supplies frame time; this label reports live AP status.
public sealed class ThermalOverlay : MonoBehaviour
{
    TMP_Text label;
    float nextRefresh;
    string lastReadout;

    public void Bind(TMP_Text target) => label = target;

    void OnEnable() => nextRefresh = 0f;

    void Update()
    {
        if (label == null || Time.unscaledTime < nextRefresh) return;
        nextRefresh = Time.unscaledTime + 0.5f;

        var ap = Holder.Instance;
        string readout = "Thermal\n(AP inactive)";
        if (ap != null && ap.Active)
        {
            var metrics = ap.ThermalStatus.ThermalMetrics;
            readout = FormattableString.Invariant(
                $"Thermal: {metrics.WarningLevel}\nLevel {metrics.TemperatureLevel:0.00} | Trend {metrics.TemperatureTrend:+0.00;-0.00;0.00}");
        }
        if (readout == lastReadout) return;
        lastReadout = readout;
        label.text = readout;
    }
}
