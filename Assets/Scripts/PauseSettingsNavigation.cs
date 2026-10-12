using UnityEngine;

public class PauseSettingsNavigation : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;

    public void ShowPauseState(bool paused)
    {
        if (!paused)
        {
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
