using System.Threading;
using UnityEngine;

// One owner per loop. Every pause source goes through LifecycleGuard, including Back
// and focus loss. Cancelling the old source precedes starting its replacement.
public abstract class ResumeGuard : MonoBehaviour
{
    CancellationTokenSource lifetime;
    bool paused;
    protected virtual void OnEnable()
    {
        paused = LifecycleGuard.IsPaused;
        LifecycleGuard.PausedChanged += OnPausedChanged;
        if (!paused) StartLoop();
    }
    void OnPausedChanged(bool value)
    {
        if (paused == value) return;
        paused = value;
        StopLoop();
        if (!paused) StartLoop();
    }
    void StartLoop()
    {
        lifetime = CancellationTokenSource.CreateLinkedTokenSource(
            destroyCancellationToken, Application.exitCancellationToken);
        RestartTimer(lifetime.Token);
    }
    void StopLoop()
    {
        var previous = lifetime;
        lifetime = null;
        previous?.Cancel();
        previous?.Dispose();
    }
    protected abstract void RestartTimer(CancellationToken token);
    protected virtual void OnDisable()
    {
        LifecycleGuard.PausedChanged -= OnPausedChanged;
        StopLoop();
    }
}
