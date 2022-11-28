using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController SharedInstance;
    public LevelLoader levelLoader;
    public DialogueManager dialogueManager;
    public DialogueTrigger dialogueTrigger;
    public float despawnTimeInSecAfterDisdard = 1.5f;

    void Start()
    {
        // dialogueTrigger.TriggerDialogue(0);
    }

    void Awake()
    {
        if (SharedInstance != null && SharedInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        SharedInstance = this;
    }

    void Update()
    {
        // if (Input.GetButtonDown("Fire1"))
        // {
        //     Vector3 mousePos = Input.mousePosition;
        //     {
        //         var position = Utils.GetWorldPositionOnPlane(mousePos, 0);
        //         Debug.Log(position.x + ", " + position.y);
        //     }
        // }
    }
}
