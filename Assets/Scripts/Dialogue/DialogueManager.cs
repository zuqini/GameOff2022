using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public CanvasGroup dialogueBox;
    public Canvas canvas;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public float charDisplaySpeed = 1f/10f; // 10 char per second

    private List<Dialogue> dialogue;
    private Animator anim;
    private int dialogueIndex = 0;
    private int sentenceIndex = 0;
    private string currentSentence;
    private float elapsedTime = Mathf.Infinity;
    private bool finishedSentence = true;
    private bool shouldEnd = false;

    public bool ShouldEnd { get => shouldEnd; set => shouldEnd = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = canvas.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (finishedSentence || string.IsNullOrEmpty(currentSentence))
        {
            return;
        }
        elapsedTime += Time.deltaTime;

        int currentIndex = (int)(elapsedTime / charDisplaySpeed); // I think this is right?
        currentIndex = Mathf.Min(currentIndex, currentSentence.Length);
        dialogueText.SetText(currentSentence.Substring(0, currentIndex));
        if (currentIndex == currentSentence.Length) {
            finishedSentence = true;
        }

    }

    public void StartDialogue(List<Dialogue> dialogue)
    {
        this.dialogue = dialogue;
        dialogueIndex = 0;
        sentenceIndex = 0;
        anim.SetTrigger("SpawnDialogue");

        // bring up dialogue box
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (dialogue == null || !finishedSentence)
        {
            return;
        }
        if (dialogueIndex >= dialogue.Count)
        {
            if (shouldEnd) EndDialogue();
            return;
        }

        var speaker = dialogue[dialogueIndex];
        if (sentenceIndex >= speaker.sentences.Length)
        {
            sentenceIndex = 0;
            dialogueIndex++;
            DisplayNextDialogue();
            return;
        }

        var sentence = speaker.sentences[sentenceIndex++];
        nameText.SetText(speaker.name);
        // dialogueText.SetText(sentence);
        currentSentence = sentence;
        elapsedTime = 0;
        finishedSentence = false;
    }

    public void EndDialogue()
    {
        if (shouldEnd) {
            GameController.SharedInstance.CurrentCustomer.Exit();
        }
        dialogue = null;
        dialogueIndex = 0;
        sentenceIndex = 0;
        shouldEnd = false;
        finishedSentence = true;
        // bring down dialogue box
        anim.SetTrigger("DespawnDialogue");
    }
}
