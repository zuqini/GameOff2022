using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Collider2D col;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private float startXPos;
    private float startYPos;
    private bool isDragging = false;
    private bool isEnabled = true;
    private bool shouldDiscard = false;

    public Rigidbody2D rb;
    public AudioSource dragSound; //this needs to be passed in because it is played before item is active
    public bool shouldStabilize = false;
    public float stabilizationSpeed = 500f;
    public float targetStabilizationRotation = 0f;
    public bool shouldRotateWithItem = false;
    public bool canBeDiscarded = true;
    public bool IsDragging { get => isDragging; set => isDragging = value; }
    public bool IsEnabled { get => isEnabled; set => isEnabled = value; }

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isEnabled)
        {
            return;
        }

        if (IsDragging)
        {
            SetTargetPosition();
        }
    }

    void FixedUpdate()
    {
        if (!isEnabled)
        {
            col.enabled = false;
            isDragging = false;
            return;
        }
        col.enabled = true;

        if (IsDragging)
        {
            rb.MovePosition(targetPosition);

            if (shouldStabilize)
            {
                rb.angularVelocity = 0;
                var rotation = Utils.ClampAngle(rb.rotation);
                if (rotation > targetStabilizationRotation)
                {
                    rb.SetRotation(Mathf.Max(targetStabilizationRotation, rotation - stabilizationSpeed * Time.deltaTime));
                }
                else if (rotation < targetStabilizationRotation)
                {
                    rb.SetRotation(Mathf.Min(targetStabilizationRotation, rotation + stabilizationSpeed * Time.deltaTime));
                }
            }
        }

        transform.position = rb.position;
        if (shouldRotateWithItem)
        {
            transform.rotation = rb.transform.rotation;
        }
    }

    public void OnMouseDown()
    {
        if (!isEnabled) {
            return;
        }
        isDragging = true;
        Vector3 mousePos = Utils.GetWorldPositionOnPlane(Utils.ClipMousePosToScreen(Input.mousePosition), transform.position.z);

        startXPos = mousePos.x - transform.position.x;
        startYPos = mousePos.y - transform.position.y;
        targetPosition = GetTargetPosition(mousePos);

        PlayDragSound();
    }

    public void PlayDragSound()
    {
        // need to find a better way to do this, special logic for cup sound
        if (rb.gameObject.tag == "Cup")
        {
            dragSound.Play();
        }
    }

    public void OnMouseUp()
    {
        isDragging = false;
        if (!isEnabled) {
            return;
        }

        rb.velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z) * 20;
        currentVelocity = Vector3.zero;

        if (shouldDiscard) {
            isEnabled = false;
            Utils.SetColliderEnabledRecursive(transform.parent.gameObject, false);
            Utils.SetRendererRecursive(transform.parent.gameObject, SortingLayer.NameToID("BehindTable"));

            // need to find a better way to do this, special logic for discarding Cup shit
            if (rb.gameObject.tag == "Cup")
            {
                var cup = rb.GetComponent<CupController>();
                cup.teabagZone.DiscardContents();
            }

            StartCoroutine(SetInactive());
            return;
        }
    }

    private IEnumerator SetInactive() {
        yield return new WaitForSeconds(GameController.SharedInstance.despawnTimeInSecAfterDisdard);
        transform.parent.gameObject.SetActive(false);
    }

    private void SetTargetPosition()
    {
        Vector3 mousePos = Utils.GetWorldPositionOnPlane(Utils.ClipMousePosToScreen(Input.mousePosition), transform.position.z);
        targetPosition = GetTargetPosition(mousePos);
        currentVelocity = targetPosition - transform.position;
    }

    private Vector3 GetTargetPosition(Vector3 mousePos)
    {
        return new Vector3(mousePos.x - startXPos, mousePos.y - startYPos, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBeDiscarded || other.gameObject.tag != "Discard")
        {
            return;
        }
        other.transform.localScale = new Vector3(1.25f, 1.25f, 1);
        shouldDiscard = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!canBeDiscarded || other.gameObject.tag != "Discard")
        {
            return;
        }
        shouldDiscard = false;
        other.transform.localScale = new Vector3(1, 1, 1);
    }
}
