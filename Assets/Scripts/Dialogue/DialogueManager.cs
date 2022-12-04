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
    private List<string> displayedSentences;
    private Animator anim;
    private int dialogueIndex = 0;
    private int sentenceIndex = 0;
    private int characterIndex = 0;
    private float elapsedTime = Mathf.Infinity;
    private bool finishedDialogue = true;
    private bool shouldEnd = false;

    public bool ShouldEnd { get => shouldEnd; set => shouldEnd = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = canvas.GetComponent<Animator>();
        displayedSentences = new List<string>();
    }

    void FixedUpdate()
    {
        // if (finishedSentence || string.IsNullOrEmpty(currentSentence))
        // {
        //     // load next sentence
        //     return;
        // }
        // elapsedTime += Time.deltaTime;

        // int currentIndex = (int)(elapsedTime / charDisplaySpeed); // I think this is right?
        // currentIndex = Mathf.Min(currentIndex, currentSentence.Length);
        // dialogueText.SetText(currentSentence.Substring(0, currentIndex));
        // if (currentIndex == currentSentence.Length) {
        //     finishedSentence = true;
        // }
    }

    public void StartDialogue(List<Dialogue> dialogue)
    {
        this.dialogue = dialogue;
        dialogueIndex = 0;
        sentenceIndex = 0;
        characterIndex = 0;
        displayedSentences.Clear();
        anim.SetTrigger("SpawnDialogue");

        // bring up dialogue box
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (dialogue == null)
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
            displayedSentences.Clear();
            sentenceIndex = 0;
            characterIndex = 0;
            dialogueIndex++;
            DisplayNextDialogue();
            return;
        }

        nameText.SetText(speaker.name);
        elapsedTime = 0;
        StartCoroutine(DisplayNextCharacter());
    }

    public void EndDialogue()
    {
        if (shouldEnd) {
            GameController.SharedInstance.CurrentCustomer.Exit();
        }
        dialogue = null;
        displayedSentences.Clear();
        dialogueIndex = 0;
        sentenceIndex = 0;
        shouldEnd = false;
        // bring down dialogue box
        anim.SetTrigger("DespawnDialogue");
    }

    private IEnumerator DisplayNextCharacter()
    {
        var currentDialogue = dialogue[dialogueIndex];
        var currentSentence = currentDialogue.sentences[sentenceIndex];
        while (true)
        {
            yield return new WaitForSeconds(charDisplaySpeed);
            characterIndex++;
            if (characterIndex > currentSentence.Length) {
                displayedSentences.Add(currentSentence);
                sentenceIndex++;
                characterIndex = 0;
                if (sentenceIndex >= currentDialogue.sentences.Length)
                {
                    displayedSentences.Clear();
                    sentenceIndex = 0;
                    characterIndex = 0;
                    dialogueIndex++;
                    DisplayNextDialogue();
                    break;
                }
                currentSentence = currentDialogue.sentences[sentenceIndex];
            }
            dialogueText.SetText(string.Format("{0}{1}",string.Join("", displayedSentences), currentDialogue.sentences[sentenceIndex].Substring(0, characterIndex)));
        }
    }
}
