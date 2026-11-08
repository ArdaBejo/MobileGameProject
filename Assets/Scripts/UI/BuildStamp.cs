using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class BuildStamp : MonoBehaviour
{
    TMP_Text label;
    string feedbackUrl;
    static readonly Vector2 Center = new(0.5f, 0.5f);

    public static string Id
    {
        get
        {
            string guid = Application.buildGUID;
            return string.IsNullOrEmpty(guid) ? "editor" : guid.Substring(0, Math.Min(8, guid.Length));
        }
    }

    public static BuildStamp Create(Transform owner)
    {
        var safe = GreyboxUI.Canvas("AboutCanvas", 120);
        var root = safe.parent.gameObject;
        root.transform.SetParent(owner, false);
        var backdrop = GreyboxUI.Rect("AboutBackdrop", safe, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        backdrop.gameObject.AddComponent<Image>().color = new Color(0.035f, 0.055f, 0.085f, 0.99f);
        var panel = root.AddComponent<BuildStamp>();
        GreyboxUI.Label(backdrop, "AboutTitle", "About No Stars Remain", Center,
            new Vector2(0, 230), new Vector2(650, 72), 32);
        panel.label = GreyboxUI.Label(backdrop, "BuildLabel", "", Center,
            new Vector2(0, 40), new Vector2(640, 270), 23);
        panel.label.richText = false;
        GreyboxUI.Button(backdrop, "Copy build ID", Center, new Vector2(0, -137),
            () => GUIUtility.systemCopyBuffer = Id, 23);
        GreyboxUI.Button(backdrop, "Back", Center, new Vector2(0, -313), panel.Close, 24);
        string url = Resources.Load<TextAsset>("PeerFeedbackUrl")?.text.Trim();
        panel.feedbackUrl = Uri.TryCreate(url, UriKind.Absolute, out var parsed) && parsed.Scheme == Uri.UriSchemeHttps
            ? parsed.AbsoluteUri : "";
        if (panel.feedbackUrl.Length > 0)
            GreyboxUI.Button(backdrop, "Feedback", Center, new Vector2(0, -225),
                () => Application.OpenURL(panel.feedbackUrl), 24);
        root.SetActive(false);
        return panel;
    }

    public void Open()
    {
        if (SceneLoader.Instance != null && SceneLoader.Instance.IsLoading) return;
        label.text = FormattableString.Invariant(
            $"v{Application.version} ({Id})\n{SystemInfo.deviceModel}\n{SystemInfo.operatingSystem}\n{SystemInfo.systemMemorySize} MB RAM\n{Screen.width}×{Screen.height} @ {Screen.currentResolution.refreshRateRatio.value:0.#} Hz\n\nFeedback: {(feedbackUrl.Length > 0 ? feedbackUrl : "Not available for this build")}");
        gameObject.SetActive(true);
        Debug.Log($"build={Application.version}/{Id}");
    }

    public void Close() => gameObject.SetActive(false);

    public bool TryClose()
    {
        if (!gameObject.activeSelf) return false;
        Close();
        return true;
    }
}
