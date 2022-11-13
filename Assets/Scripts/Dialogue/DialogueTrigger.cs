using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<List<Dialogue>> dialogues;

    void Start() {
        dialogues = new List<List<Dialogue>> {
            new List<Dialogue> {
                new Dialogue {
                    name = "Mario",
                    sentences = new string[] {
                        "itsa me",
                        "mario",
                    },
                },
                new Dialogue {
                    name = "Toad",
                    sentences = new string[] {
                        "hi mario",
                        "unfortunately the princess is in another castle",
                        "lol",
                    },
                }
            },
        };
    }

    public void TriggerDialogue(int i) {
        GameController.SharedInstance.dialogueManager.StartDialogue(dialogues[i]);
    }
}
