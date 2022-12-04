using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<List<Dialogue>> dialogues;

    void Start() {
    }

    public void TriggerDialogue(int i) {
        // GameController.SharedInstance.dialogueManager.StartDialogue(dialogues[i]);
    }
}
