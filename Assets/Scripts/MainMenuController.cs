using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public LevelLoader levelLoader;
    public void HandleStart()
    {
        Debug.Log("start");
        levelLoader.LoadNextLevel();
    }

    public void HandleQuit()
    {
        Debug.Log("start");
        Application.Quit();
    }
}
