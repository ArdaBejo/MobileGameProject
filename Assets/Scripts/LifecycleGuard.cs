using UnityEngine;

public class LifecycleGuard : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    public static event System.Action<bool> PausedChanged;

    void Awake() => SetPaused(false);

    void OnDestroy() => SetPaused(false);

    void OnApplicationPause(bool paused)
    {
        if (!paused) return;
        PlayerPrefs.Save();
        SetPaused(true);
    }

    void OnApplicationFocus(bool f)
    {
        if (!f) SetPaused(true);
    }

    public void SetPaused(bool value)
    {
        SetGlobalPaused(value);
    }

    public static void SetGlobalPaused(bool value)
    {
        bool changed = IsPaused != value;
        IsPaused = value;
        Time.timeScale = value ? 0f : 1f;
        AudioListener.pause = value;
        if (changed) PausedChanged?.Invoke(value);
    }
}
