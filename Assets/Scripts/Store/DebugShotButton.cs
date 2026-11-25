using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class DebugShotButton : MonoBehaviour
{
    StoreShot shot;
    int number;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Register()
    {
        SceneManager.sceneLoaded -= AddToGame;
        if (Debug.isDebugBuild) SceneManager.sceneLoaded += AddToGame;
    }

    static void AddToGame(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Game") return;
        var safeArea = GreyboxUI.Canvas("StoreCaptureCanvas", 110);
        safeArea.gameObject.AddComponent<DebugShotButton>();
    }

    void Awake()
    {
        shot = gameObject.AddComponent<StoreShot>();
        GreyboxUI.Button(transform, "Capture", new Vector2(1, 1),
            new Vector2(-90, -280), Capture, 20);
    }

    async void Capture()
    {
        if (GreyboxLane.Instance == null || !GreyboxLane.Instance.Playing) return;
        try { await shot.CaptureAsync($"shot{number++:00}"); }
        catch (System.Exception error) { Debug.LogException(error, this); }
    }
}
