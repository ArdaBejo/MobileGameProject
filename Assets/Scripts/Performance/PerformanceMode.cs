using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum PerfMode { Balanced, HighFps }

public sealed class PerformanceMode : MonoBehaviour
{
    public const string PreferenceKey = "perf_mode";
    public static PerformanceMode Instance { get; private set; }
    public PerfMode Current { get; private set; }
    readonly Dictionary<UniversalRenderPipelineAsset, float> originalScales = new();

    public static void EnsureOn(GameObject owner)
    {
        if (Instance == null && owner.GetComponent<PerformanceMode>() == null)
            owner.AddComponent<PerformanceMode>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Apply(PlayerPrefs.GetInt(PreferenceKey, 0) == 1 ? PerfMode.HighFps : PerfMode.Balanced, false);
    }

    public void SetHighFps(bool on) => Apply(on ? PerfMode.HighFps : PerfMode.Balanced);

    public void Apply(PerfMode mode) => Apply(mode, true);

    void Apply(PerfMode mode, bool save)
    {
        Current = mode;
        bool high = mode == PerfMode.HighFps;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = high ? 120 : 60;
        var asset = UniversalRenderPipeline.asset;
        if (asset != null)
        {
            if (!originalScales.ContainsKey(asset)) originalScales.Add(asset, asset.renderScale);
            asset.renderScale = high ? 0.8f : 1f;
        }
        if (save)
        {
            PlayerPrefs.SetInt(PreferenceKey, high ? 1 : 0);
            PlayerPrefs.Save();
        }
        Debug.Log($"perf_mode={mode} high={high} targetFrameRate={Application.targetFrameRate} renderScale={(asset != null ? asset.renderScale.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) : "unavailable")}");
    }

    void OnDestroy()
    {
        if (Instance != this) return;
        // URP assets are shared with the Editor: leave their authored values intact.
        if (Application.isEditor)
            foreach (var entry in originalScales)
                if (entry.Key != null) entry.Key.renderScale = entry.Value;
        Instance = null;
    }
}
