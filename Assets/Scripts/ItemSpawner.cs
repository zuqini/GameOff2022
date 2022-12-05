using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private Draggable currentDraggable;

    public GameObject itemToSpawn;

    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(itemToSpawn.tag);
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
            currentDraggable.IsEnabled = true;
            currentDraggable.IsDragging = true;

            Utils.SetColliderEnabledRecursive(item, true);
            Utils.SetRendererRecursive(item, itemToSpawn.GetComponentInChildren<Renderer>().sortingLayerID);

            // special logic to handle resetting cup
            item.SetActive(true);
            if (item.tag == "TeaCup")
            {
                var cupController = item.transform.Find("Cup").GetComponent<CupController>();
                cupController.ResetCup();
                currentDraggable.PlayDragSound();
            }
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
