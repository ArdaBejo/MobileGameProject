
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    BuildStamp about;
    MusicSettings music;
    void Awake()
    {
        if (GetComponent<MenuReady>() == null) gameObject.AddComponent<MenuReady>();
        about = BuildStamp.Create(transform);
        music = MusicSettings.Create(transform);
        var ui = GreyboxUI.Canvas("MenuAboutEntry", 5);
        ui.parent.SetParent(transform, false);
        GreyboxUI.Button(ui, "About", new Vector2(0.5f, 0.5f), new Vector2(0, -165), about.Open, 24);
        GreyboxUI.Button(ui, "Music", new Vector2(0.5f, 0.5f), new Vector2(0, -250), music.Open, 24);
    }

    void Update()
    {
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true && !music.TryClose()) about.TryClose();
    }

    public void StartGame()
    {
        Debug.Log("Start Game clicked");
        SceneLoader.RequestLoad("Game");
    }
}
