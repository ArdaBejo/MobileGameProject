using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        PerformanceMode.EnsureOn(gameObject);
        Telemetry.StartSession();
        DontDestroyOnLoad(gameObject);
        if (GetComponent<AdaptiveBootstrap>() == null) gameObject.AddComponent<AdaptiveBootstrap>();
        if (GetComponent<BackgroundMusic>() == null) gameObject.AddComponent<BackgroundMusic>();
    }

    async Awaitable Start() => await SceneLoader.Instance.Load("Menu", Application.exitCancellationToken);
}
