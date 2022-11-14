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

    void Start()
    {
        dialogueTrigger.TriggerDialogue(0);
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
}
