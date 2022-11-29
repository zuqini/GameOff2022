using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaBagZoneController : MonoBehaviour
{
    private List<TeaBagController> teabags;
    private List<Rigidbody2D> stirs;
    public Transform teaBagConstrainer;
    public Rigidbody2D cup;

    public float stirSpeed = 200f;
    public float stirAngle = -60f;

    void Start()
    {
        teabags = new List<TeaBagController>();
        stirs = new List<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var left = teaBagConstrainer.Find("left").position.x;
        var right = teaBagConstrainer.Find("right").position.x;
        var y = teaBagConstrainer.Find("y").position.y;
        teabags.ForEach(teabag => {
            var holder = teabag.holder;
            var holderPosition = new Vector2(Mathf.Max(Mathf.Min(holder.position.x, right), left), Mathf.Max(Mathf.Min(holder.position.y, y+1), y));

            holder.transform.position = holderPosition;
            teabag.Body.MovePosition(transform.position);
        });

        stirs.ForEach(stir => {
            stir.MovePosition(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
            // stir.SetRotation(Mathf.Max(-60, Mathf.Min(60, Utils.ClampAngle(stir.rotation))));
            var rotation = Utils.ClampAngle(stir.rotation);
            if (rotation < stirAngle) {
                rotation = Mathf.Min(rotation + stirSpeed * Time.deltaTime, stirAngle);
            } else if (rotation > stirAngle) {
                rotation = Mathf.Max(rotation - stirSpeed * Time.deltaTime, stirAngle);
            }
            stir.SetRotation(rotation);
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "TeaBag")
        {
            var teabag = other.gameObject.GetComponent<TeaBagController>();
            var renderer = other.gameObject.GetComponent<Renderer>();
            renderer.enabled = false;
            teabag.draggable.IsEnabled = false;
            teabags.Add(teabag);
            other.enabled = false;
        }

        if (other.gameObject.tag == "Sugar")
        {
            other.transform.parent.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Stir")
        {
            var stir = other.gameObject.GetComponent<Rigidbody2D>();
            var draggable = other.transform.parent.Find("Draggable").GetComponent<Draggable>();
            stirs.Add(stir);
            other.enabled = false;
            draggable.IsEnabled = false;
        }
    }

    public void DiscardContents()
    {
        teabags.ForEach(teabag => {
            var teabagParent = teabag.transform.parent.gameObject;
            Utils.SetColliderEnabledRecursive(teabagParent, false);
            Utils.SetRendererRecursive(teabagParent, SortingLayer.NameToID("BehindTable"));
            StartCoroutine(SetInactive(teabagParent));
        });
        stirs.ForEach(stir => {
            var stirParent = stir.transform.parent.gameObject;
            Utils.SetColliderEnabledRecursive(stirParent, false);
            Utils.SetRendererRecursive(stirParent, SortingLayer.NameToID("BehindTable"));
            StartCoroutine(SetInactive(stirParent));
        });
        teabags.Clear();
        stirs.Clear();
    }

    private IEnumerator SetInactive(GameObject item) {
        yield return new WaitForSeconds(GameController.SharedInstance.despawnTimeInSecAfterDisdard);
        item.SetActive(false);
    }
}
