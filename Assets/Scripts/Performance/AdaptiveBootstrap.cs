using System.Globalization;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

// Lives on the persistent Boot/App object, so scene changes do not duplicate subscriptions.
public sealed class AdaptiveBootstrap : MonoBehaviour
{
    IAdaptivePerformance ap;
    bool subscribed;

    void Start()
    {
        ap = Holder.Instance;
        if (ap == null || !ap.Active)
        {
            Debug.Log("[AP] provider inactive on this device");
            return;
        }

        ap.ThermalStatus.ThermalEvent += OnThermal;
        ap.PerformanceStatus.PerformanceBottleneckChangeEvent += OnBottleneck;
        subscribed = true;
        var metrics = ap.ThermalStatus.ThermalMetrics;
        Debug.Log(System.FormattableString.Invariant(
            $"[AP] active warning={metrics.WarningLevel} level={metrics.TemperatureLevel:0.00}"));
    }

    void OnDestroy()
    {
        if (!subscribed || ap == null) return;
        ap.ThermalStatus.ThermalEvent -= OnThermal;
        ap.PerformanceStatus.PerformanceBottleneckChangeEvent -= OnBottleneck;
        subscribed = false;
    }

    void OnThermal(ThermalMetrics metrics)
    {
        Debug.Log(string.Format(CultureInfo.InvariantCulture,
            "[AP] thermal warning={0} level={1:0.00} trend={2:0.00}",
            metrics.WarningLevel, metrics.TemperatureLevel, metrics.TemperatureTrend));
    }

    void OnBottleneck(PerformanceBottleneckChangeEventArgs args)
    {
        Debug.Log($"[AP] bottleneck={args.PerformanceBottleneck}");
    }
}
