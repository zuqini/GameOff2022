using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourZoneController : MonoBehaviour
{
    private CupController targetCup;

    public CupController TargetCup { get => targetCup; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Cup")
        {
            return;
        }

        targetCup = other.GetComponent<CupController>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Cup")
        {
            return;
        }

        targetCup = null;
    }
}
