using UnityEngine;

[DefaultExecutionOrder(-100)]
public sealed class DeviceTier : MonoBehaviour
{
    public const string OverrideKey = "tier_override";
    public const int Low = 0;
    public const int High = 2;

    public static int Current => QualitySettings.GetQualityLevel();
    public static string CurrentName => QualitySettings.names[Current];

    void Awake()
    {
        int tier = PlayerPrefs.GetInt(OverrideKey, -1);
        if (tier < Low || tier > High) tier = Detect();
        QualitySettings.SetQualityLevel(tier, true);
        Shader.globalMaximumLOD = tier == Low ? 300 : 600;
        Application.targetFrameRate = tier == Low ? 30 : 60;
        Debug.Log($"tier={CurrentName}");
    }

    static int Detect()
    {
        int memory = SystemInfo.systemMemorySize;
        int cores = SystemInfo.processorCount;
        if (memory >= 8192 && cores >= 8) return High;
        return memory >= 4096 && cores >= 6 ? 1 : Low;
    }

    public static void SetTierOverride(int option)
    {
        if (option == 0) PlayerPrefs.DeleteKey(OverrideKey);
        else PlayerPrefs.SetInt(OverrideKey, option - 1);
        PlayerPrefs.Save();
    }
}
