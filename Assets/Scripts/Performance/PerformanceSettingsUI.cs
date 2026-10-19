using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Created by the existing Settings navigation; never displayed on the gameplay HUD.
public sealed class PerformanceSettingsUI : MonoBehaviour
{
    Toggle toggle;
    TMP_Text label;

    void Awake()
    {
        var row = new GameObject("High FPS", typeof(RectTransform), typeof(Image), typeof(Toggle));
        row.layer = gameObject.layer;
        row.transform.SetParent(transform, false);
        var rect = (RectTransform)row.transform;
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, -270);
        rect.sizeDelta = new Vector2(650, 64);
        var background = row.GetComponent<Image>();
        background.color = new Color(0.08f, 0.12f, 0.18f, 1);
        toggle = row.GetComponent<Toggle>();
        toggle.targetGraphic = background;

        var textObject = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.layer = row.layer;
        textObject.transform.SetParent(row.transform, false);
        var textRect = (RectTransform)textObject.transform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        label = textObject.GetComponent<TMP_Text>();
        label.fontSize = 32;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;
        label.raycastTarget = false;
        textObject.AddComponent<TextScale>();
        toggle.onValueChanged.AddListener(OnChanged);
    }

    void OnEnable()
    {
        bool high = PerformanceMode.Instance != null
            ? PerformanceMode.Instance.Current == PerfMode.HighFps
            : PlayerPrefs.GetInt(PerformanceMode.PreferenceKey, 0) == 1;
        toggle.SetIsOnWithoutNotify(high);
        RefreshLabel(high);
    }

    void OnChanged(bool high)
    {
        if (PerformanceMode.Instance == null) return;
        PerformanceMode.Instance.SetHighFps(high);
        RefreshLabel(high);
    }

    void RefreshLabel(bool high) => label.text = high ? "High FPS: ON" : "High FPS: OFF (Balanced)";

    void OnDestroy()
    {
        if (toggle != null) toggle.onValueChanged.RemoveListener(OnChanged);
    }
}
