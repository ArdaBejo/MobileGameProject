using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        Playing,
        Paused,
        Won,
        Lost
    }

    public State CurrentState { get; private set; } = State.Playing;

    public event Action<State> StateChanged;

    void Start()
    {
        SetState(LifecycleGuard.IsPaused ? State.Paused : State.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState == State.Playing) LifecycleGuard.SetGlobalPaused(true);
    }

    public void ResumeGame()
    {
        if (CurrentState == State.Paused) LifecycleGuard.SetGlobalPaused(false);
    }

    public void WinGame()
    {
        SetState(State.Won);
        LifecycleGuard.SetGlobalPaused(false);
        SceneManager.LoadScene("Result");
    }

    public void LoseGame()
    {
        SetState(State.Lost);
        LifecycleGuard.SetGlobalPaused(false);
        SceneManager.LoadScene("Result");
    }

    void SetState(State newState)
    {
        CurrentState = newState;

        Debug.Log($"Game state changed to: {newState}");

        StateChanged?.Invoke(newState);
    }

    void OnEnable()
    {
        LifecycleGuard.PausedChanged += OnPauseChanged;
    }

    void OnDisable()
    {
        LifecycleGuard.PausedChanged -= OnPauseChanged;
    }

    void OnPauseChanged(bool paused)
    {
        if (CurrentState == State.Won || CurrentState == State.Lost) return;
        SetState(paused ? State.Paused : State.Playing);
    }
}
