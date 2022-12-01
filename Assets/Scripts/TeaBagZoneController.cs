using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaBagZoneController : MonoBehaviour
{
    private List<TeaBagController> teabags;
    private List<Rigidbody2D> stirs;
    private AudioSource dropSound;

    private int stirCount = 0;
    private int sugarCount = 0;
    private int blackTeaCount = 0;
    private int herbalTeaCount = 0;
    private int lightTeaCount = 0;

    public Transform teaBagConstrainer;

    public Rigidbody2D cup;
    public float stirSpeed = 200f;
    public float stirAngle = -60f;

    public int StirCount { get => stirCount; }
    public int BlackTeaCount { get => blackTeaCount; }
    public int HerbalTeaCount { get => herbalTeaCount; }
    public int LightTeaCount { get => lightTeaCount; }
    public int SugarCount { get => sugarCount; }

    void Start()
    {
        teabags = new List<TeaBagController>();
        stirs = new List<Rigidbody2D>();
        dropSound = GetComponent<AudioSource>();
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
            stir.SetRotation(Mathf.Max(-60, Mathf.Min(60, Utils.ClampAngle(stir.rotation))));

            // var rotation = Utils.ClampAngle(stir.rotation);
            // if (rotation < stirAngle) {
            //     rotation = Mathf.Min(rotation + stirSpeed * Time.deltaTime, stirAngle);
            // } else if (rotation > stirAngle) {
            //     rotation = Mathf.Max(rotation - stirSpeed * Time.deltaTime, stirAngle);
            // }
            // stir.SetRotation(rotation);
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "BlackTeaBag" || 
                other.gameObject.tag == "HerbalTeaBag" ||
                other.gameObject.tag == "LightTeaBag")
        {
            var teabag = other.gameObject.GetComponent<TeaBagController>();
            var renderer = other.gameObject.GetComponent<Renderer>();
            renderer.enabled = false;
            teabag.draggable.IsEnabled = false;
            teabags.Add(teabag);
            other.enabled = false;

            if (other.gameObject.tag == "BlackTeaBag") blackTeaCount++;
            if (other.gameObject.tag == "HerbalTeaBag") herbalTeaCount++;
            if (other.gameObject.tag == "LightTeaBag") lightTeaCount++;
            dropSound.Play();
        }

        if (other.gameObject.tag == "Sugar")
        {
            other.transform.parent.gameObject.SetActive(false);
            sugarCount++;
            dropSound.Play();
        }

        if (other.gameObject.tag == "Stir")
        {
            var stir = other.gameObject.GetComponent<Rigidbody2D>();
            var draggable = other.transform.parent.Find("Draggable").GetComponent<Draggable>();
            stirs.Add(stir);
            // other.enabled = false;
            draggable.IsEnabled = false;
            stirCount++;
            dropSound.Play();
        }
    }

    public void AddForceToContents(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
    {
        teabags.ForEach(teabag => {
            teabag.Body.AddForce(force, mode);
        });
        stirs.ForEach(stir => {
            stir.AddForce(force, mode);
        });
    }

    public void SetContentVelocity(Vector2 velocity)
    {
        teabags.ForEach(teabag => {
            teabag.Body.velocity = velocity;
        });
        stirs.ForEach(stir => {
            stir.velocity = velocity;
        });
    }

    public void SetStirCollider(bool enabled)
    {
        stirs.ForEach(stir => {
            var colliders = stir.GetComponents<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
        });
    }

    public void DiscardContents(bool disableColliders = true)
    {
        teabags.ForEach(teabag => {
            var teabagParent = teabag.transform.parent.gameObject;
            if (disableColliders)
            {
                Utils.SetColliderEnabledRecursive(teabagParent, false);
            }
            Utils.SetRendererRecursive(teabagParent, SortingLayer.NameToID("BehindTable"));
            StartCoroutine(SetInactive(teabagParent));
        });
        stirs.ForEach(stir => {
            var stirParent = stir.transform.parent.gameObject;
            if (disableColliders)
            {
                Utils.SetColliderEnabledRecursive(stirParent, false);
            }
            Utils.SetRendererRecursive(stirParent, SortingLayer.NameToID("BehindTable"));
            StartCoroutine(SetInactive(stirParent));
        });
        teabags.Clear();
        stirs.Clear();
    }

    public void SetContentsInactive()
    {
        teabags.ForEach(teabag => {
            var teabagParent = teabag.transform.parent.gameObject;
            teabagParent.SetActive(false);
        });
        stirs.ForEach(stir => {

            var stirParent = stir.transform.parent.gameObject;
            stirParent.SetActive(false);
        });
        teabags.Clear();
        stirs.Clear();
    }

    private IEnumerator SetInactive(GameObject item) {
        yield return new WaitForSeconds(GameController.SharedInstance.despawnTimeInSecAfterDisdard);
        item.SetActive(false);
    }
}
