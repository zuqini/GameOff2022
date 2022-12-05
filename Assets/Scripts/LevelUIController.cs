using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    private Animator currentLevelUI;

    public Transform startPosition;
    public GameObject levelUIPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GenerateCurrentLevel()
    {
        GameObject levelUI = ObjectPooler.SharedInstance.GetPooledObject(levelUIPrefab.tag);
        if (levelUI != null)
        {
            levelUI.transform.position = new Vector3(startPosition.position.x + GameController.SharedInstance.Level,
                    startPosition.position.y,
                    startPosition.position.z);
            levelUI.SetActive(true);
            currentLevelUI = levelUI.GetComponent<Animator>();
        }
    }

    public void SetCurrentLevelWin()
    {
        currentLevelUI.SetTrigger("Win");
    }

    public void SetCurrentLevelLose()
    {
        currentLevelUI.SetTrigger("Lose");
    }
}
