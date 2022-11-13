using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private List<Dialogue> dialogue;
    private int dialogueIndex = 0;
    private int sentenceIndex = 0;

    // Start is called before the first frame update
    void Start() {
    }

    public void StartDialogue(List<Dialogue> dialogue) {
        this.dialogue = dialogue;
        dialogueIndex = 0;
        sentenceIndex = 0;

        // bring up dialogue box
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue() {
        if (dialogueIndex >= dialogue.Count) {
            EndDialogue();
        }
        var speaker = dialogue[dialogueIndex];
        if (sentenceIndex >= speaker.sentences.Length) {
            sentenceIndex = 0;
            dialogueIndex++;
            DisplayNextDialogue();
            return;
        }

        var sentence = speaker.sentences[sentenceIndex++];
        nameText.SetText(speaker.name);
        dialogueText.SetText(sentence);
    }

    public void EndDialogue() {
        dialogueIndex = 0;
        sentenceIndex = 0;
        // bring down dialogue box
    }
}
