using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MusicSettings : MonoBehaviour
{
    bool changed;

    public static void AddTo(Transform settings)
    {
        var center = new Vector2(0.5f, 0.5f);
        var row = GreyboxUI.Rect("MusicSettings", settings, center, center, new Vector2(0, -352), new Vector2(650, 80));
        var controls = row.gameObject.AddComponent<MusicSettings>();
        var music = BackgroundMusic.Instance;
        var toggle = GreyboxUI.Button(row, "Music", center, new Vector2(-200, 0), () => { }, 28);
        var toggleLabel = toggle.GetComponentInChildren<TMP_Text>();
        ((RectTransform)toggle.transform).sizeDelta = new Vector2(245, 68);
        toggleLabel.rectTransform.sizeDelta = new Vector2(235, 60);
        var volumeLabel = GreyboxUI.Label(row, "VolumeLabel", "", center,
            new Vector2(135, 24), new Vector2(360, 42), 27);

        var track = GreyboxUI.Rect("MusicVolume", row, center, center, new Vector2(135, -21), new Vector2(370, 42));
        track.gameObject.AddComponent<Image>().color = new Color(0.13f, 0.17f, 0.19f);
        var travel = GreyboxUI.Rect("HandleArea", track, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-46, 0));
        var handle = GreyboxUI.Rect("Handle", travel, new Vector2(0.5f, 0f), new Vector2(0.5f, 1f),
            Vector2.zero, new Vector2(44, 0));
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
        toggle.onClick.AddListener(() =>
        {
            music.SetMusicEnabled(!music.MusicEnabled);
            controls.changed = true;
            Refresh();
        });
        slider.onValueChanged.AddListener(value =>
        {
            music.SetMusicVolume(value);
            controls.changed = true;
            Refresh();
        });
        Refresh();
    }

    void OnDisable()
    {
        if (!changed) return;
        PlayerPrefs.Save();
        changed = false;
    }
}
