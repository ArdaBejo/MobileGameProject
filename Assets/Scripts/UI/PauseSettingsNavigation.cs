using UnityEngine;

public class PauseSettingsNavigation : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    BuildStamp about;

    void Awake()
    {
        if (settingsPanel != null && settingsPanel.GetComponent<PerformanceSettingsUI>() == null)
            settingsPanel.AddComponent<PerformanceSettingsUI>();
        if (settingsPanel != null)
        {
            about = BuildStamp.Create(transform);
            GreyboxUI.Button(settingsPanel.transform, "About", new Vector2(0.5f, 0.5f),
                new Vector2(0, -437), about.Open, 24);
        }
    }

    public bool TryCloseAbout() => about != null && about.TryClose();

    public void ShowPauseState(bool paused)
    {
        if (!paused)
        {
            TryCloseAbout();
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
