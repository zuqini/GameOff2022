using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTextBubbleController : MonoBehaviour
{
    private Animator anim;
    private bool isSpawned = false;

    public bool IsSpawned { get => isSpawned; }
    public CustomerController customer;
    public float despawnTimeInSecs = 0.3f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Spawn()
    {
        if (!isSpawned)
        {
            anim.SetTrigger("SpawnTriggerTextBubble");
            isSpawned = true;
        }
    }

    void OnMouseDown()
    {
        if (isSpawned)
        {
            isSpawned = false;
            anim.SetTrigger("DespawnTriggerTextBubble");
            StartCoroutine(StartDialogue());
        }
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(despawnTimeInSecs);
        GameController.SharedInstance.dialogueManager.StartDialogue(new List<Dialogue> { customer.GenerateOrderDialogueByText() });
    }

    void OnMouseUp()
    {
    }
}
