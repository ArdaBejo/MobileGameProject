using System;
using System.Diagnostics;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public bool IsLoading { get; private set; }
    [SerializeField] Slider bar;
    [SerializeField] GameObject loadingScreen;
    TMP_Text progressText;
    int lastPercent = -1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        progressText = GreyboxUI.Label(loadingScreen.transform, "LoadingLabel", "Loading...", Vector2.one * 0.5f,
            new Vector2(0, 55), new Vector2(600, 100), 32);
        loadingScreen.SetActive(false);
    }

    public static void RequestLoad(string scene)
    {
        if (Instance == null)
        {
            Debug.LogError("SceneLoader is missing. Start Play Mode from the Boot scene.");
            return;
        }
        _ = Instance.Load(scene, Application.exitCancellationToken);
    }

    public async Awaitable Load(string scene, CancellationToken ct)
    {
        // The lock is taken before the first await, so double clicks cannot queue loads.
        if (IsLoading) return;
        IsLoading = true;
        AsyncOperation operation = null;
        var stopwatch = Stopwatch.StartNew();
        try
        {
            ct.ThrowIfCancellationRequested();
            if (!Application.CanStreamedLevelBeLoaded(scene))
                throw new ArgumentException($"Scene '{scene}' is not enabled in the build scene list.");

            lastPercent = -1;
            SetProgress(0);
            loadingScreen.SetActive(true);
            // Draw the overlay before starting the scene operation.
            await Awaitable.NextFrameAsync(ct);
            operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            if (operation == null) throw new InvalidOperationException($"Could not load scene '{scene}'.");
            operation.allowSceneActivation = false;

            // isDone remains false at 0.9 while activation is blocked.
            while (operation.progress < 0.9f)
            {
                SetProgress(operation.progress / 0.9f);
                await Awaitable.NextFrameAsync(ct);
            }

            SetProgress(1);
            ct.ThrowIfCancellationRequested();
            operation.allowSceneActivation = true;
            await operation;
            // Let the destination scene initialize before revealing it.
            await Awaitable.NextFrameAsync(ct);
            stopwatch.Stop();
            Debug.Log($"[perf] scene {scene} loaded in {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (OperationCanceledException)
        {
            // Only application exit cancels normal UI loads; backgrounding does not.
        }
        catch (Exception exception)
        {
            Debug.LogException(exception, this);
        }
        finally
        {
            // Unity scene operations cannot be cancelled. Never leave activation locked.
            if (operation != null) operation.allowSceneActivation = true;
            if (loadingScreen != null) loadingScreen.SetActive(false);
            IsLoading = false;
        }
    }

    void SetProgress(float value)
    {
        bar.value = Mathf.Clamp01(value);
        int percent = Mathf.RoundToInt(bar.value * 100);
        if (percent == lastPercent) return;
        lastPercent = percent;
        progressText.text = $"Loading... {percent}%";
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
