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
        if (changed)
        {
            // Flush actual wallet state on every pause source, not only Android Home.
            if (value && Wallet.Instance != null) WalletSave.Save(Wallet.Instance);
            if (value) PlayerPrefs.Save();
            if (!value && Wallet.Instance != null)
                WalletSave.Load(Wallet.Instance);
            Debug.Log($"[Lifecycle] paused={value} wallet={Wallet.Instance?.Coins} rank={Wallet.Instance?.Level}");
            PausedChanged?.Invoke(value);
        }
    }
}
