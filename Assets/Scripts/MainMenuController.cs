using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public LevelLoader levelLoader;
    public void HandleStart()
    {
        levelLoader.LoadNextLevel();
    }

    public void HandleQuit()
    {
        Application.Quit();
    }
}
