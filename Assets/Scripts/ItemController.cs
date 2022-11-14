using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private bool isDragging = false;

    private float startXPos;
    private float startYPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDragging)
        {
            DragObject();
        }
    }

    void FixedUpdate()
    {
        if (!isDragging)
        {
            return;
        }

        rb.MovePosition(targetPosition);
    }

    void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos = Utils.GetWorldPositionOnPlane(mousePos, transform.position.z);

        startXPos = mousePos.x - transform.position.x;
        startYPos = mousePos.y - transform.position.y;
        targetPosition = GetTargetPosition(mousePos);

        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.velocity = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z) * 20;
        currentVelocity = Vector3.zero;
    }

    public void DragObject()
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
