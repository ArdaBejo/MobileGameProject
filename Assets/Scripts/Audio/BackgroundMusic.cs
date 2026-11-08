using UnityEngine;

// Lives on Boot/App, so a scene transition does not restart the soundtrack.
public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }
    const string EnabledKey = "music_enabled";
    const string VolumeKey = "music_volume";
    [SerializeField, Range(0, 1)] float volume = 0.18f;
    AudioSource source;
    bool backgrounded;
    bool focused = true;

    public bool MusicEnabled { get; private set; }
    public float MusicVolume { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.spatialBlend = 0;
        MusicEnabled = PlayerPrefs.GetInt(EnabledKey, 1) != 0;
        MusicVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(VolumeKey, volume));
        source.volume = MusicVolume;
        source.mute = !MusicEnabled;
        source.clip = Resources.Load<AudioClip>("Sound/Void Runer");
        if (source.clip == null)
        {
            Debug.LogWarning("OST not found at Resources/Sound/Void Runer.");
            return;
        }
        source.Play();
        RefreshPlayback();
    }

    public void SetMusicEnabled(bool enabled)
    {
        MusicEnabled = enabled;
        source.mute = !enabled;
        PlayerPrefs.SetInt(EnabledKey, enabled ? 1 : 0);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        source.volume = MusicVolume;
        PlayerPrefs.SetFloat(VolumeKey, MusicVolume);
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
