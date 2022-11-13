using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public void LoadNextLevel()
    {
        var nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene > SceneManager.sceneCount)
        {
            nextScene = 0;
        }
        StartCoroutine(LoadLevel(nextScene));
    }

    public void LoadNextScene(string scene)
    {
        StartCoroutine(LoadScene(scene));
    }

    IEnumerator LoadScene(string scene)
    {
        transition.SetTrigger("StartAnimation");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(scene);
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("StartAnimation");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
