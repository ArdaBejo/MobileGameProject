using UnityEngine;

// Lives on Boot/App, so a scene transition does not restart the soundtrack.
public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }
    [SerializeField, Range(0, 1)] float volume = 0.18f;
    AudioSource source;
    bool backgrounded;
    bool focused = true;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.spatialBlend = 0;
        source.volume = volume;
        source.clip = Resources.Load<AudioClip>("Sound/Void Runer");
        if (source.clip == null)
        {
            Debug.LogWarning("OST not found at Resources/Sound/Void Runer.");
            return;
        }
        source.Play();
        RefreshPlayback();
    }

    void OnEnable() => LifecycleGuard.PausedChanged += OnGamePause;
    void OnDisable() => LifecycleGuard.PausedChanged -= OnGamePause;
    void OnGamePause(bool paused) => RefreshPlayback();
    void OnApplicationPause(bool paused) { backgrounded = paused; RefreshPlayback(); }
    void OnApplicationFocus(bool hasFocus) { focused = hasFocus; RefreshPlayback(); }

    void RefreshPlayback()
    {
        if (source == null) return;
        if (backgrounded || !focused || LifecycleGuard.IsPaused) source.Pause();
        else source.UnPause();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
