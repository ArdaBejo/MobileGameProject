using UnityEngine;

public class PauseSettingsNavigation : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;

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
