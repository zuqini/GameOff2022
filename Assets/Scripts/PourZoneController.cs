using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourZoneController : MonoBehaviour
{
    private bool shouldPour = false;

    public bool ShouldPour { get => shouldPour; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Cup")
        {
            return;
        }
        shouldPour = true;
        Debug.Log("lol");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Cup")
        {
            return;
        }
        shouldPour = false;
        Debug.Log("non");
    }
}
