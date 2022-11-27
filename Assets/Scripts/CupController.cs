using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour
{
    private float temperature;
    private float water = 0;

    public float waterCapacityInSec = 0.5f;

    void Start()
    {
    }

    void Update()
    {
    }

    public void PourWater(float pouredWater)
    {
        water = Mathf.Min(waterCapacityInSec, water + pouredWater);
    }
}
