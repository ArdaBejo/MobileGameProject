
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    BuildStamp about;
    void Awake()
    {
        if (GetComponent<MenuReady>() == null) gameObject.AddComponent<MenuReady>();
        about = BuildStamp.Create(transform);
        var ui = GreyboxUI.Canvas("MenuAboutEntry", 5);
        ui.parent.SetParent(transform, false);
        GreyboxUI.Button(ui, "About", new Vector2(0.5f, 0.5f), new Vector2(0, -165), about.Open, 24);
    }

    void Update()
    {
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true) about.TryClose();
    }

    public void StartGame()
    {
        Debug.Log("Start Game clicked");
        SceneLoader.RequestLoad("Game");
    }
}
