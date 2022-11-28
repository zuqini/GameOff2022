using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaBagZoneController : MonoBehaviour
{
    private List<TeaBagController> teabags;
    public Transform teaBagConstrainer;
    public Rigidbody2D cup;

    void Start()
    {
        teabags = new List<TeaBagController>();
    }

    void FixedUpdate()
    {
        var left = teaBagConstrainer.Find("left").position.x;
        var right = teaBagConstrainer.Find("right").position.x;
        var y = teaBagConstrainer.Find("y").position.y;
        teabags.ForEach(teabag => {
            var holder = teabag.holder;
            var holderPosition = new Vector2(Mathf.Max(Mathf.Min(holder.position.x, right), left), Mathf.Max(Mathf.Min(holder.position.y, y+1), y));

            // ignore physics by directly setting transform
            holder.transform.position = holderPosition;
            teabag.transform.position = transform.position;

            var draggable = teabag.draggable;
            draggable.IsEnabled = false;
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "TeaBag")
        {
            var teabag = other.gameObject.GetComponent<TeaBagController>();
            teabags.Add(teabag);
        }

        if (other.gameObject.tag == "Sugar")
        {
            other.transform.parent.gameObject.SetActive(false);
        }
    }

    public void DiscardAllTeaBags()
    {
        teabags.ForEach(teabag => {
            var teabagParent = teabag.transform.parent.gameObject;
            Utils.SetColliderEnabledRecursive(teabagParent, false);
            Utils.SetSortingLayerRecursive(teabagParent, SortingLayer.NameToID("BehindTable"));
            StartCoroutine(SetInactive(teabagParent));
        });
        teabags.Clear();
    }

    private IEnumerator SetInactive(GameObject item) {
        yield return new WaitForSeconds(GameController.SharedInstance.despawnTimeInSecAfterDisdard);
        item.SetActive(false);
    }
}
