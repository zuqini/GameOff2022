using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaBagZoneController : MonoBehaviour
{
    private List<TeaBagController> teabags;
    public Transform teaBagConstrainer;

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
            var holderPosition = new Vector2(Mathf.Max(Mathf.Min(holder.position.x, right), left), Mathf.Max(holder.position.y, y));
            holder.MovePosition(holderPosition);
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "TeaBag")
        {
            return;
        }
        teabags.Add(other.gameObject.GetComponent<TeaBagController>());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "TeaBag")
        {
            return;
        }
        teabags.Remove(other.gameObject.GetComponent<TeaBagController>());
    }
}
