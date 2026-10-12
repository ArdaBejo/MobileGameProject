using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] LifecycleGuard guard;

    void OnEnable()
    {
        LifecycleGuard.PausedChanged += Show;
        Show(LifecycleGuard.IsPaused);
    }

    void OnDisable()
    {
        LifecycleGuard.PausedChanged -= Show;
    }

    void Show(bool paused)
    {
        var navigation = guard.GetComponent<PauseSettingsNavigation>();
        if (navigation != null)
            navigation.ShowPauseState(paused);
        else
            panel.SetActive(paused);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null || !kb.escapeKey.wasPressedThisFrame) return;

        guard.SetPaused(!LifecycleGuard.IsPaused);
    }

    public void OnResumePressed()
    {
        guard.SetPaused(false);
    }
}
