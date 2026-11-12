using UnityEngine;

public class PauseSettingsNavigation : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    BuildStamp about;

    void Awake()
    {
        if (pausePanel != null)
        {
            var restart = GreyboxUI.Button(pausePanel.transform, "Restart", new Vector2(0.5f, 0.5f),
                new Vector2(0, -320), RestartRun, 40);
            ((RectTransform)restart.transform).sizeDelta = new Vector2(500, 100);
        }

        if (settingsPanel != null && settingsPanel.GetComponent<PerformanceSettingsUI>() == null)
            settingsPanel.AddComponent<PerformanceSettingsUI>();
        if (settingsPanel != null)
        {
            about = BuildStamp.Create(transform);
            MusicSettings.AddTo(settingsPanel.transform);
            GreyboxUI.Button(settingsPanel.transform, "About", new Vector2(0.5f, 0.5f),
                new Vector2(0, -540), about.Open, 24);
        }
    }

    static void RestartRun()
    {
        SceneLoader.RequestLoad("Game", () =>
        {
            Wallet.Instance?.Restore(0, 0);
        });
    }

    public bool TryCloseSettingsOverlay() => about?.TryClose() == true;

    public void ShowPauseState(bool paused)
    {
        if (!paused)
        {
            TryCloseSettingsOverlay();
            settingsPanel.SetActive(false);
            pausePanel.SetActive(false);
            return;
        }

        // Focus-loss notifications must not put Pause over an open Settings panel.
        pausePanel.SetActive(!settingsPanel.activeSelf);
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToPause()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
