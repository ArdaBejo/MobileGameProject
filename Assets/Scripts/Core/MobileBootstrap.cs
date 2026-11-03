using UnityEngine;

public class MobileBootstrap : MonoBehaviour
{
    void Awake()
    {
        // Allow direct Game-scene testing without persisting its scene-local Bootstrap.
        if (PerformanceMode.Instance == null)
            PerformanceMode.EnsureOn(new GameObject("Performance"));
        QualitySettings.vSyncCount = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Debug.Log($"[Boot] {SystemInfo.deviceModel} | " +
                  $"{SystemInfo.operatingSystem} | " +
                  $"{SystemInfo.graphicsDeviceType} | " +
                  $"{Screen.width}x{Screen.height} @ {Screen.dpi} dpi");
    }
}
