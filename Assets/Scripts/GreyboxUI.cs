using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Shared flat UI for the lane and its result. Existing Pause/Settings remain above it.
public static class GreyboxUI
{
    public static RectTransform Canvas(string name, int order)
    {
        var obj = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = obj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = order;
        var scaler = obj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(720, 1280);
        scaler.matchWidthOrHeight = 0.5f;
        var safe = Rect("SafeArea", obj.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        safe.gameObject.AddComponent<GreyboxSafeArea>();
        return safe;
    }

    public static RectTransform Rect(string name, Transform parent, Vector2 min, Vector2 max, Vector2 position, Vector2 size)
    {
        var rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
        return rect;
    }

    public static TMP_Text Label(Transform parent, string name, string value, Vector2 anchor, Vector2 position, Vector2 size, float fontSize)
    {
        var rect = Rect(name, parent, anchor, anchor, position, size);
        var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.raycastTarget = false;
        rect.gameObject.AddComponent<TextScale>();
        return text;
    }

    public static Button Button(Transform parent, string label, Vector2 anchor, Vector2 position, UnityAction action)
    {
        var rect = Rect(label + "Button", parent, anchor, anchor, position, new Vector2(300, 72));
        var image = rect.gameObject.AddComponent<Image>();
        image.color = new Color(0.13f, 0.24f, 0.32f);
        var button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(action);
        Label(rect, "Label", label, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(290, 68), 28);
        return button;
    }

    public static Button PauseButton(Transform parent, UnityAction action)
    {
        var anchor = new Vector2(0, 1);
        var rect = Rect("PauseButton", parent, anchor, anchor, new Vector2(62, -62), new Vector2(72, 72));
        var background = rect.gameObject.AddComponent<Image>();
        background.color = new Color(0.13f, 0.24f, 0.32f);
        var button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = background;
        button.onClick.AddListener(action);
        foreach (float x in new[] { -10f, 10f })
        {
            var bar = Rect("PauseBar", rect, Vector2.one * 0.5f, Vector2.one * 0.5f, new Vector2(x, 0), new Vector2(9, 30));
            var image = bar.gameObject.AddComponent<Image>();
            image.color = Color.white;
            image.raycastTarget = false;
        }
        return button;
    }
}
