using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private float startXPos;
    private float startYPos;
    private bool isDragging = false;
    private bool isEnabled = true;

    public bool IsDragging { get => isDragging; }
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
            rb.MovePosition(targetPosition);
        }

        transform.position = rb.position;
    }

    void OnMouseDown()
    {
        if (!isEnabled) {
            return;
        }
        isDragging = true;
        Vector3 mousePos = Input.mousePosition;

        mousePos = Utils.GetWorldPositionOnPlane(mousePos, transform.position.z);

        startXPos = mousePos.x - transform.position.x;
        startYPos = mousePos.y - transform.position.y;
        targetPosition = GetTargetPosition(mousePos);
    }

    void OnMouseUp()
    {
        isDragging = false;
        if (!isEnabled) {
            return;
        }
        rb.velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z) * 20;
        currentVelocity = Vector3.zero;
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
}
