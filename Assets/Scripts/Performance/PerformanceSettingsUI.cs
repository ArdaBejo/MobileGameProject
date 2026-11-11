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
        AddTierDropdown();
    }

    void AddTierDropdown()
    {
        var center = Vector2.one * 0.5f;
        var root = GreyboxUI.Rect("Device Tier", transform, center, center, new Vector2(0, 257), new Vector2(650, 54));
        var background = root.gameObject.AddComponent<Image>();
        background.color = new Color(0.08f, 0.12f, 0.18f);
        var dropdown = root.gameObject.AddComponent<TMP_Dropdown>();
        dropdown.targetGraphic = background;
        dropdown.captionText = GreyboxUI.Label(root, "Selected tier", "", center, Vector2.zero, new Vector2(630, 50), 25);

        var top = new Vector2(0.5f, 1f);
        var template = GreyboxUI.Rect("Tier choices", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
            Vector2.zero, new Vector2(650, 216));
        template.pivot = top;
        template.gameObject.AddComponent<Image>().color = background.color;
        var content = GreyboxUI.Rect("Content", template, top, top, Vector2.zero, new Vector2(650, 216));
        content.pivot = top;
        var item = GreyboxUI.Rect("Tier choice", content, top, top, new Vector2(0, -27), new Vector2(650, 54));
        var itemImage = item.gameObject.AddComponent<Image>();
        itemImage.color = new Color(0.12f, 0.18f, 0.24f);
        item.gameObject.AddComponent<Toggle>().targetGraphic = itemImage;
        dropdown.itemText = GreyboxUI.Label(item, "Tier label", "", center, Vector2.zero, new Vector2(630, 50), 26);
        dropdown.template = template;
        template.gameObject.SetActive(false);
        dropdown.AddOptions(new System.Collections.Generic.List<string> { "Auto", "Low", "Mid", "High" });
        dropdown.SetValueWithoutNotify(Mathf.Clamp(PlayerPrefs.GetInt(DeviceTier.OverrideKey, -1) + 1, 0, 3));
        void RefreshTierLabel() => dropdown.captionText.text =
            $"Tier: {dropdown.options[dropdown.value].text} • next launch ▾";
        dropdown.onValueChanged.AddListener(option =>
        {
            DeviceTier.SetTierOverride(option);
            RefreshTierLabel();
        });
        RefreshTierLabel();
    }

    void OnEnable()
    {
        bool high = PerformanceMode.Instance != null
            ? PerformanceMode.Instance.Current == PerfMode.HighFps
            : PlayerPrefs.GetInt(PerformanceMode.PreferenceKey, 0) == 1;
        toggle.SetIsOnWithoutNotify(high);
        toggle.interactable = PerformanceMode.HighFpsAvailable;
        RefreshLabel(high);
    }

    void OnChanged(bool high)
    {
        if (PerformanceMode.Instance == null) return;
        PerformanceMode.Instance.SetHighFps(high);
        RefreshLabel(high);
    }

    void RefreshLabel(bool high) => label.text = !PerformanceMode.HighFpsAvailable
        ? "High FPS requires High tier"
        : high ? "High FPS: ON" : "High FPS: OFF (Balanced)";

    void OnDestroy()
    {
        if (toggle != null) toggle.onValueChanged.RemoveListener(OnChanged);
    }
}
