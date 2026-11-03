
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    void Awake()
    {
        if (GetComponent<MenuReady>() == null) gameObject.AddComponent<MenuReady>();
    }

    public void StartGame()
    {
        Debug.Log("Start Game clicked");
        SceneLoader.RequestLoad("Game");
    }
}
