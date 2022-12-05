using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// @TODO need to refactor the spaghetti logic
public class DialogueManager : MonoBehaviour
{
    public CanvasGroup dialogueBox;
    public Canvas canvas;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public float defaultCharDelayInSec = 0.02f;

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
            // if (shouldEnd) EndDialogue();
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
        characterIndex = 0;
        shouldEnd = false;
        // bring down dialogue box
        anim.SetTrigger("DespawnDialogue");
    }

    public void OnDialogueClick()
    {
        if (shouldEnd) EndDialogue();
    }

    private IEnumerator DisplayNextCharacter()
    {
        var currentDialogue = dialogue[dialogueIndex];
        var currentSentence = currentDialogue.sentences[sentenceIndex];
        while (true)
        {
            characterIndex++;
            // Debug.Log("Displaying char: "+currentSentence.sentence[characterIndex - 1]);
            while (characterIndex <= currentSentence.sentence.Length && char.IsWhiteSpace(currentSentence.sentence[characterIndex - 1]))
            {
                // Debug.Log("isWhiteSpaece");
                characterIndex++;
            }
            if (characterIndex > currentSentence.sentence.Length) {
                displayedSentences.Add(currentSentence.sentence);
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
            } else {
                yield return new WaitForSeconds(currentSentence.sentenceCharDelayInSec >= 0 ? currentSentence.sentenceCharDelayInSec : defaultCharDelayInSec);
            }
            dialogueText.SetText(string.Format("{0}{1}",string.Join("", displayedSentences), currentSentence.sentence.Substring(0, characterIndex)));
        }
    }
}
