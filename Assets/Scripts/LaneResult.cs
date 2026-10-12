using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class LaneResult : MonoBehaviour
{
    public static bool Won { get; private set; }
    public static float SurvivalTime { get; private set; }
    public static int Kills { get; private set; }

    public static void Record(bool won, float seconds, int kills)
    {
        Won = won;
        SurvivalTime = seconds;
        Kills = kills;
    }

    void Start()
    {
        LifecycleGuard.SetGlobalPaused(false);
        if (Camera.main != null) Camera.main.backgroundColor = new Color(0.025f, 0.035f, 0.05f);
        if (EventSystem.current == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        var ui = GreyboxUI.Canvas("ResultCanvas", 10);
        var center = new Vector2(0.5f, 0.5f);
        GreyboxUI.Label(ui, "Title", Won ? "YOU SURVIVED" : "RUN ENDED", center, new Vector2(0, 180), new Vector2(620, 100), 44);
        GreyboxUI.Label(ui, "Stats", $"Survived {SurvivalTime:F0}s / 120s\nKills: {Kills}", center, new Vector2(0, 60), new Vector2(600, 110), 30);
        GreyboxUI.Button(ui, "Retry", center, new Vector2(0, -90), () => SceneManager.LoadScene("Game"));
        GreyboxUI.Button(ui, "Main Menu", center, new Vector2(0, -190), () => SceneManager.LoadScene("Menu"));
    }
}
