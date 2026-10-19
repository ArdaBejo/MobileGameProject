using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Lab evidence overlay. Measures real elapsed frame intervals, excluding pause/loading.
public sealed class PerformanceOverlay : MonoBehaviour
{
    RectTransform safeArea;
    TMP_Text timing;
    TMP_Text modeLabel;
    PerfMode sampledMode;
    double previousTime;
    double windowSeconds;
    int frames;
    bool sampling;

    void Start()
    {
        safeArea = GreyboxUI.Canvas("PerformanceOverlay", 100);
        safeArea.parent.SetParent(transform, false);
        // This canvas must never intercept movement or Settings touches.
        Destroy(safeArea.parent.GetComponent<GraphicRaycaster>());
        var panel = GreyboxUI.Rect("Readout", safeArea, new Vector2(1, 1), new Vector2(1, 1),
            new Vector2(-174, -187), new Vector2(324, 102));
        var background = panel.gameObject.AddComponent<Image>();
        background.color = new Color(0.02f, 0.03f, 0.04f, 0.9f);
        background.raycastTarget = false;
        timing = GreyboxUI.Label(panel, "Timing", "Measuring...", Vector2.one * 0.5f,
            new Vector2(0, 17), new Vector2(312, 42), 24);
        modeLabel = GreyboxUI.Label(panel, "Mode", "", Vector2.one * 0.5f,
            new Vector2(0, -25), new Vector2(312, 38), 18);
        sampledMode = PerformanceMode.Instance.Current;
        RefreshMode();
    }

    void Update()
    {
        if (safeArea == null || PerformanceMode.Instance == null) return;
        bool inGame = SceneManager.GetActiveScene().name == "Game";
        safeArea.gameObject.SetActive(inGame);
        if (!inGame) { ResetWindow(); return; }

        if (sampledMode != PerformanceMode.Instance.Current)
        {
            sampledMode = PerformanceMode.Instance.Current;
            ResetWindow();
            timing.text = LifecycleGuard.IsPaused ? "Resume to measure" : "Measuring...";
            RefreshMode();
        }

        if (LifecycleGuard.IsPaused || (SceneLoader.Instance != null && SceneLoader.Instance.IsLoading))
        {
            ResetWindow();
            return; // Keep the last completed gameplay sample visible in Settings.
        }

        double now = Time.realtimeSinceStartupAsDouble;
        if (!sampling)
        {
            previousTime = now;
            sampling = true;
            return;
        }
        windowSeconds += now - previousTime;
        previousTime = now;
        frames++;
        if (windowSeconds < 0.5) return;
        timing.SetText("FPS: {0:0} | {1:1} ms", (float)(frames / windowSeconds),
            (float)(1000 * windowSeconds / frames));
        RefreshMode();
        windowSeconds = 0;
        frames = 0;
    }

    void RefreshMode()
    {
        var asset = UniversalRenderPipeline.asset;
        string name = sampledMode == PerfMode.HighFps ? "High FPS" : "Balanced";
        string scale = asset != null
            ? asset.renderScale.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) : "n/a";
        modeLabel.text = $"{name} | target {Application.targetFrameRate}\nRender scale: {scale}";
    }

    void ResetWindow()
    {
        sampling = false;
        frames = 0;
        windowSeconds = 0;
    }

    void OnApplicationPause(bool paused) => ResetWindow();
    void OnApplicationFocus(bool focused) => ResetWindow();
}
