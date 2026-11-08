using UnityEngine;
using UnityEngine.UI;

public sealed class MusicSettings : MonoBehaviour
{
    public static MusicSettings Create(Transform owner)
    {
        var center = new Vector2(0.5f, 0.5f);
        var safe = GreyboxUI.Canvas("MusicSettingsCanvas", 120);
        var root = safe.parent.gameObject;
        root.transform.SetParent(owner, false);
        var panel = root.AddComponent<MusicSettings>();
        var backdrop = GreyboxUI.Rect("MusicBackdrop", safe, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        backdrop.gameObject.AddComponent<Image>().color = new Color(0.035f, 0.055f, 0.085f, 0.99f);
        var music = BackgroundMusic.Instance;
        var toggle = GreyboxUI.Button(backdrop, "Music", center, new Vector2(0, 150), () => { });
        var toggleLabel = toggle.GetComponentInChildren<TMPro.TMP_Text>();
        var volumeLabel = GreyboxUI.Label(backdrop, "VolumeLabel", "", center,
            new Vector2(0, 45), new Vector2(460, 60), 28);

        var track = GreyboxUI.Rect("MusicVolume", backdrop, center, center, new Vector2(0, -30), new Vector2(440, 64));
        track.gameObject.AddComponent<Image>().color = new Color(0.13f, 0.17f, 0.19f);
        var travel = GreyboxUI.Rect("HandleArea", track, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-46, 0));
        var handle = GreyboxUI.Rect("Handle", travel, center, center, Vector2.zero, new Vector2(44, 56));
        var handleImage = handle.gameObject.AddComponent<Image>();
        handleImage.color = GreyboxUI.Accent;
        var slider = track.gameObject.AddComponent<Slider>();
        slider.handleRect = handle;
        slider.targetGraphic = handleImage;
        slider.SetValueWithoutNotify(music != null ? music.MusicVolume : 0);

        void Refresh()
        {
            toggleLabel.text = music != null && music.MusicEnabled ? "Music: On" : "Music: Off";
            volumeLabel.text = $"Music volume: {Mathf.RoundToInt(slider.value * 100)}%";
        }

        toggle.interactable = slider.interactable = music != null;
        toggle.onClick.AddListener(() => { music.SetMusicEnabled(!music.MusicEnabled); Refresh(); });
        slider.onValueChanged.AddListener(value => { music.SetMusicVolume(value); Refresh(); });
        GreyboxUI.Button(backdrop, "Back", center, new Vector2(0, -155), () => panel.TryClose(), 24);
        Refresh();
        root.SetActive(false);
        return panel;
    }

    public void Open() => gameObject.SetActive(true);

    public bool TryClose()
    {
        if (!gameObject.activeSelf) return false;
        gameObject.SetActive(false);
        PlayerPrefs.Save();
        return true;
    }
}
