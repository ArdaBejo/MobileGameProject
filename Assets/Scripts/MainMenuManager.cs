
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Start Game clicked");
        SceneLoader.RequestLoad("Game");
    }
}
