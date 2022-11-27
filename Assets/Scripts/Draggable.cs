using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private float startXPos;
    private float startYPos;
    private bool isDragging = false;
    private bool isEnabled = true;
    private bool shouldDiscard = false;
    private int originalSortingLayerID;

    public Rigidbody2D rb;
    public int despawnTimeInSecAfterDisdard = 3;
    public bool IsDragging { get => isDragging; set => isDragging = value; }
    public bool IsEnabled { get => isEnabled; set => isEnabled = value; }

    void Start()
    {
    }

    void Update()
    {
        if (!isEnabled)
        {
            return;
        }

        if (IsDragging)
        {
            DragObject();
        }
    }

    void FixedUpdate()
    {
        if (!isEnabled)
        {
            isDragging = false;
            return;
        }

        if (IsDragging)
        {
            Debug.Log("dragging");
            rb.MovePosition(targetPosition);
        }

        transform.position = rb.position;
    }

    public void OnMouseDown()
    {
        if (!isEnabled) {
            return;
        }
        Debug.Log("lol");
        isDragging = true;
        Vector3 mousePos = Utils.GetWorldPositionOnPlane(Input.mousePosition, transform.position.z);

        startXPos = mousePos.x - transform.position.x;
        startYPos = mousePos.y - transform.position.y;
        targetPosition = GetTargetPosition(mousePos);
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
            var colChildren = transform.parent.gameObject.GetComponentsInChildren<Collider2D>();
            foreach (var col in colChildren)
            {
                col.enabled = false;
            }
            var rendChildren = transform.parent.gameObject.GetComponentsInChildren<Renderer>();
            foreach (var rend in rendChildren)
            {
                // assert that every child has the same sorting layer
                originalSortingLayerID = rend.sortingLayerID;
                rend.sortingLayerID = SortingLayer.NameToID("BehindTable");
            }

            StartCoroutine(Discard());
            return;
        }
    }

    private IEnumerator Discard() {
        yield return new WaitForSeconds(despawnTimeInSecAfterDisdard);

        transform.parent.gameObject.SetActive(false);
        var colChildren = transform.parent.gameObject.GetComponentsInChildren<Collider2D>();
        foreach (var col in colChildren)
        {
            col.enabled = true;
        }
        var rendChildren = transform.parent.gameObject.GetComponentsInChildren<Renderer>();
        foreach (var rend in rendChildren)
        {
            rend.sortingLayerID = originalSortingLayerID;
        }
    }

    private void DragObject()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos = Utils.GetWorldPositionOnPlane(mousePos, transform.position.z);
        targetPosition = GetTargetPosition(mousePos);
        currentVelocity = targetPosition - transform.position;
    }

    private Vector3 GetTargetPosition(Vector3 mousePos)
    {
        return new Vector3(mousePos.x - startXPos, mousePos.y - startYPos, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Discard")
        {
            return;
        }
        other.transform.localScale = new Vector3(1.25f, 1.25f, 1);
        shouldDiscard = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Discard")
        {
            return;
        }
        shouldDiscard = false;
        other.transform.localScale = new Vector3(1, 1, 1);
    }
}
