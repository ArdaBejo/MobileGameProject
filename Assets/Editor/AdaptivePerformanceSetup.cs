using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.AdaptivePerformance.Google.Android;

// Explicit setup tool, not an auto-run hook: later release logging choices are preserved.
public static class AdaptivePerformanceSetup
{
    const string Folder = "Assets/Settings/AdaptivePerformance";
    const string GeneralPath = Folder + "/AdaptivePerformanceGeneralSettings.asset";
    const string ProviderPath = Folder + "/AndroidProviderSettings.asset";
    const string LoaderPath = Folder + "/AndroidLoader.asset";

    [MenuItem("Tools/Performance/Configure Week 7 Android Provider")]
    public static void Configure()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            throw new InvalidOperationException("Configure Adaptive Performance outside Play Mode.");
        if (!AssetDatabase.IsValidFolder(Folder))
            AssetDatabase.CreateFolder("Assets/Settings", "AdaptivePerformance");

        var targets = GetOrCreate<AdaptivePerformanceGeneralSettingsPerBuildTarget>(GeneralPath);
        EditorBuildSettings.AddConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey, targets, true);
        // Unity 6.6 has a global feature switch in addition to per-platform loaders.
        var targetProperties = new SerializedObject(targets);
        var featureEnabled = targetProperties.FindProperty("m_EnableAdaptivePerformance");
        if (featureEnabled == null) throw new InvalidOperationException("Unity 6.6 AP feature switch not found.");
        featureEnabled.boolValue = true;
        targetProperties.ApplyModifiedPropertiesWithoutUndo();
        // Required by the Android provider for precise frame/performance-hint data.
        PlayerSettings.enableFrameTimingStats = true;
        var android = targets.SettingsForBuildTarget(BuildTargetGroup.Android);
        if (android == null)
        {
            android = ScriptableObject.CreateInstance<AdaptivePerformanceGeneralSettings>();
            android.name = "Android Settings";
            AssetDatabase.AddObjectToAsset(android, targets);
            targets.SetSettingsForBuildTarget(BuildTargetGroup.Android, android);
        }
        android.InitManagerOnStart = true;
        if (android.Manager == null)
        {
            var manager = ScriptableObject.CreateInstance<AdaptivePerformanceManagerSettings>();
            manager.name = "Android Providers";
            AssetDatabase.AddObjectToAsset(manager, targets);
            android.Manager = manager;
        }
        android.Manager.automaticLoading = true;
        android.Manager.automaticRunning = true;
        var loader = GetOrCreate<GoogleAndroidProviderLoader>(LoaderPath);
        // Add only this provider; retain any explicitly configured existing loaders.
        if (!android.Manager.loaders.Contains(loader)) android.Manager.loaders.Add(loader);

        var provider = GetOrCreate<GoogleAndroidProviderSettings>(ProviderPath);
        EditorBuildSettings.AddConfigObject(GoogleAndroidProviderConstants.k_SettingsKey, provider, true);
        provider.logging = true;
        provider.googleProviderLogging = true;
        provider.indexerSettings ??= new AdaptivePerformanceIndexerSettings();
        provider.indexerSettings.active = true;
        // Fresh indexer defaults retain the package's thermal/performance action delays.
        provider.scalerSettings ??= new AdaptivePerformanceScalerSettings();
        foreach (var scaler in Scalers(provider.scalerSettings)) scaler.enabled = false;

        EditorUtility.SetDirty(targets);
        EditorUtility.SetDirty(android);
        EditorUtility.SetDirty(android.Manager);
        EditorUtility.SetDirty(provider);
        AssetDatabase.SaveAssets();
        Validate();
    }

    [MenuItem("Tools/Performance/Validate Android Provider Settings")]
    public static void Validate()
    {
        if (!EditorBuildSettings.TryGetConfigObject(AdaptivePerformanceGeneralSettings.k_SettingsKey,
                out AdaptivePerformanceGeneralSettingsPerBuildTarget targets))
            throw new InvalidOperationException("Adaptive Performance target settings are not registered.");
        if (!new SerializedObject(targets).FindProperty("m_EnableAdaptivePerformance").boolValue)
            throw new InvalidOperationException("Unity's Adaptive Performance feature is disabled.");
        if (!PlayerSettings.enableFrameTimingStats)
            throw new InvalidOperationException("Android provider requires Frame Timing Stats.");
        var android = targets.SettingsForBuildTarget(BuildTargetGroup.Android);
        if (android == null || !android.InitManagerOnStart || android.Manager == null
            || !android.Manager.automaticLoading || !android.Manager.automaticRunning)
            throw new InvalidOperationException("Android startup initialization is not enabled.");
        bool hasAndroid = android.Manager.loaders.Exists(loader => loader is GoogleAndroidProviderLoader);
        if (!hasAndroid) throw new InvalidOperationException("Android provider loader is not assigned.");
        var provider = GoogleAndroidProviderSettings.GetSettings();
        if (provider == null || !provider.logging || !provider.googleProviderLogging
            || provider.indexerSettings == null || !provider.indexerSettings.active)
            throw new InvalidOperationException("Lab provider logging/indexer configuration is incomplete.");
        foreach (var scaler in Scalers(provider.scalerSettings))
            if (scaler.enabled) throw new InvalidOperationException("Lab A requires all scalers disabled.");
        Debug.Log("[AP Setup] PASS: Android provider assigned; initialize/startup + logging on; Indexer active; scalers off. Device runtime support still requires an Android test.");
    }

    static T GetOrCreate<T>(string path) where T : ScriptableObject
    {
        var asset = AssetDatabase.LoadAssetAtPath<T>(path);
        if (asset != null) return asset;
        asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path);
        return asset;
    }

    static System.Collections.Generic.IEnumerable<AdaptivePerformanceScalerSettingsBase> Scalers(AdaptivePerformanceScalerSettings settings)
    {
        foreach (var property in typeof(AdaptivePerformanceScalerSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            if (property.GetValue(settings) is AdaptivePerformanceScalerSettingsBase scaler) yield return scaler;
    }
}
