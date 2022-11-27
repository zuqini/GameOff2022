using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour
{
    private float temperature;
    private float water = 0;

    public ParticleSystem ps;
    public float waterCapacityInSec = 0.5f;

    void Start()
    {
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (IsFullWater() && ps.isStopped)
        {
            ps.Play();
        }
        else if (!IsFullWater() && ps.isPlaying)
        {
            ps.Stop();
        }
    }

    // returns how much water is poured
    public float PourWater(float pouredWater)
    {
        water = Mathf.Min(waterCapacityInSec, water + pouredWater);
        return Mathf.Min(pouredWater, waterCapacityInSec - water);
    }

    public bool IsFullWater()
    {
        return water >= waterCapacityInSec - 0.05;
    }
}
