using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        Telemetry.StartSession();
        DontDestroyOnLoad(gameObject);
        if (GetComponent<AdaptiveBootstrap>() == null) gameObject.AddComponent<AdaptiveBootstrap>();
        if (GetComponent<BackgroundMusic>() == null) gameObject.AddComponent<BackgroundMusic>();
    }

    async Awaitable Start() => await SceneLoader.Instance.Load("Menu", Application.exitCancellationToken);
}
