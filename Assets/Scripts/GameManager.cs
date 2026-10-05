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
        SetState(State.Playing);
    }

    public void PauseGame()
    {
        SetState(State.Paused);
    }

    public void ResumeGame()
    {
        SetState(State.Playing);
    }

    public void WinGame()
    {
        SetState(State.Won);
        SceneManager.LoadScene("Result");
    }

    public void LoseGame()
    {
        SetState(State.Lost);
        SceneManager.LoadScene("Result");
    }

    void SetState(State newState)
    {
        CurrentState = newState;
        Time.timeScale = newState == State.Paused ? 0f : 1f;

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
    SetState(paused ? State.Paused : State.Playing);
    }
}