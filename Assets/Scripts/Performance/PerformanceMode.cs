using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum PerfMode { Balanced, HighFps }

[DefaultExecutionOrder(-50)]
public sealed class PerformanceMode : MonoBehaviour
{
    public const string PreferenceKey = "perf_mode";
    public static PerformanceMode Instance { get; private set; }
    public PerfMode Current { get; private set; }
    public bool BatterySaver { get; private set; }
    public static bool HighFpsAvailable => DeviceTier.Current == DeviceTier.High;
    readonly Dictionary<UniversalRenderPipelineAsset, float> originalScales = new();
#if UNITY_ANDROID && !UNITY_EDITOR
    AndroidJavaObject powerManager;
    float nextPowerCheck;
#endif

    public static void EnsureOn(GameObject owner)
    {
        if (Instance != null || owner.GetComponent<PerformanceMode>() != null) return;
        if (owner.GetComponent<DeviceTier>() == null) owner.AddComponent<DeviceTier>();
        owner.AddComponent<PerformanceMode>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
#if UNITY_ANDROID && !UNITY_EDITOR
        using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        using var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        powerManager = activity?.Call<AndroidJavaObject>("getSystemService", "power");
        BatterySaver = powerManager?.Call<bool>("isPowerSaveMode") == true;
#endif
        Apply(PlayerPrefs.GetInt(PreferenceKey, 0) == 1 ? PerfMode.HighFps : PerfMode.Balanced, false);
        if (GetComponent<PerformanceOverlay>() == null) gameObject.AddComponent<PerformanceOverlay>();
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    void Update()
    {
        if (powerManager == null || Time.unscaledTime < nextPowerCheck) return;
        nextPowerCheck = Time.unscaledTime + 2f;
        bool enabled = powerManager.Call<bool>("isPowerSaveMode");
        if (BatterySaver == enabled) return;
        BatterySaver = enabled;
        Apply(Current, false);
    }
#endif

    public void SetHighFps(bool on) => Apply(on ? PerfMode.HighFps : PerfMode.Balanced);

    public void Apply(PerfMode mode) => Apply(mode, true);

    void Apply(PerfMode mode, bool save)
    {
        Current = mode;
        bool high = mode == PerfMode.HighFps && HighFpsAvailable;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = DeviceTier.Current == DeviceTier.Low ? 30 : high && !BatterySaver ? 120 : 60;
        var asset = UniversalRenderPipeline.asset;
        if (asset != null)
        {
            if (!originalScales.TryGetValue(asset, out float baseScale))
                originalScales.Add(asset, baseScale = asset.renderScale);
            asset.renderScale = high || BatterySaver ? Mathf.Min(baseScale, 0.8f) : baseScale;
        }
        if (save)
        {
            PlayerPrefs.SetInt(PreferenceKey, mode == PerfMode.HighFps ? 1 : 0);
            PlayerPrefs.Save();
        }
        Debug.Log($"perf_mode={mode} high={high} batterySaver={BatterySaver} targetFrameRate={Application.targetFrameRate} renderScale={(asset != null ? asset.renderScale.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) : "unavailable")}");
    }

    void OnDestroy()
    {
        if (Instance != this) return;
#if UNITY_ANDROID && !UNITY_EDITOR
        powerManager?.Dispose();
#endif
        // URP assets are shared with the Editor: leave their authored values intact.
        if (Application.isEditor)
            foreach (var entry in originalScales)
                if (entry.Key != null) entry.Key.renderScale = entry.Value;
        Instance = null;
    }
}
