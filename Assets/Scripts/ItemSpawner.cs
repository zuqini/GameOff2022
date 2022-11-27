using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private Draggable currentDraggable;

    public string itemToSpawn;

    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(itemToSpawn);
        if (item != null)
        {
            var mousePos = Utils.GetWorldPositionOnPlane(Input.mousePosition, 0);
            var rbs = item.gameObject.GetComponentsInChildren<Rigidbody2D>();
            foreach (var rb in rbs)
            {
                // this skips physics calculations
                rb.transform.position = mousePos;
            }

            currentDraggable = item.transform.Find("Draggable").GetComponent<Draggable>();
            currentDraggable.transform.position = mousePos;
            currentDraggable.IsDragging = true;
            item.SetActive(true);
        }
    }

    void OnMouseUp()
    {
        if (currentDraggable != null)
        {
            currentDraggable.OnMouseUp();
            currentDraggable = null;
        }
    }
}
