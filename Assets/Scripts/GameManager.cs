using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] int levelId = 1;
    static readonly Dictionary<int, int> attempts = new();
    bool laneReady;
    bool runStarted;
    bool runEnded;
    float startedAt;
    int coinsEarned;
    Wallet runWallet;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetAttempts() => attempts.Clear();

    async void Start()
    {
        try
        {
            // Start is called during scene activation, before the loading overlay closes.
            while (SceneLoader.Instance != null && SceneLoader.Instance.IsLoading)
                await Awaitable.NextFrameAsync(destroyCancellationToken);
            laneReady = true;
            SetState(LifecycleGuard.IsPaused ? State.Paused : State.Playing);
        }
        catch (OperationCanceledException) { }
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
        if (CurrentState == State.Won || CurrentState == State.Lost) return;
        SetState(State.Won);
        LifecycleGuard.SetGlobalPaused(false);
        SceneLoader.RequestLoad("Result");
    }

    public void LoseGame()
    {
        if (CurrentState == State.Won || CurrentState == State.Lost) return;
        SetState(State.Lost);
        LifecycleGuard.SetGlobalPaused(false);
        SceneLoader.RequestLoad("Result");
    }

    void SetState(State newState)
    {
        bool changed = CurrentState != newState;
        CurrentState = newState;

        if (laneReady && newState == State.Playing && !runStarted) BeginRun();
        if ((newState == State.Won || newState == State.Lost) && runStarted && !runEnded) EndRun(newState);
        if (!changed) return;

        Debug.Log($"Game state changed to: {newState}");

        StateChanged?.Invoke(newState);
    }

    void BeginRun()
    {
        runStarted = true;
        startedAt = Time.time;
        attempts.TryGetValue(levelId, out int attempt);
        attempts[levelId] = ++attempt;
        runWallet = Wallet.Instance;
        if (runWallet != null) runWallet.Earned += OnCoinsEarned;
        Telemetry.Log("level_start", ("level_id", levelId), ("attempt", attempt),
            ("coins", runWallet != null ? runWallet.Coins : 0));
    }

    void OnCoinsEarned(int amount)
    {
        if (!runEnded) coinsEarned += amount;
    }

    void EndRun(State outcome)
    {
        runEnded = true;
        float seconds = Mathf.Max(0, Time.time - startedAt);
        if (outcome == State.Won)
            Telemetry.Log("level_complete", ("level_id", levelId), ("time_s", seconds),
                ("coins_earned", coinsEarned), ("score", GreyboxLane.Instance != null ? GreyboxLane.Instance.Kills : 0));
        else
            Telemetry.Log("level_fail", ("level_id", levelId), ("time_s", seconds),
                ("cause", "hp"), ("coins_earned", coinsEarned));
    }

    void OnEnable()
    {
        LifecycleGuard.PausedChanged += OnPauseChanged;
    }

    void OnDisable()
    {
        LifecycleGuard.PausedChanged -= OnPauseChanged;
        if (runWallet != null) runWallet.Earned -= OnCoinsEarned;
    }

    void OnPauseChanged(bool paused)
    {
        if (CurrentState == State.Won || CurrentState == State.Lost) return;
        SetState(paused ? State.Paused : State.Playing);
    }
}
