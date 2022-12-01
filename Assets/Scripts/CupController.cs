using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour
{
    private float water = 0;
    private float milk = 0;
    private float oatMilk = 0;
    private float honey = 0;
    private Animator anim;

    public ParticleSystem ps;
    public TeaBagZoneController teabagZone;
    public float waterCapacityInSec = 0.5f;
    public float nonWaterLiquidCapacityInSec = 0.25f;
    public Draggable draggable;

    public Animator Anim { get => anim; }

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
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

    public void ResetCup()
    {
        water = 0;
        milk = 0;
        oatMilk = 0;
        honey = 0;
    }

    // returns how much water is poured
    public float PourWater(float pouredWater)
    {
        water = Mathf.Min(waterCapacityInSec, water + pouredWater);
        return Mathf.Min(pouredWater, waterCapacityInSec - water);
    }

    public void PourLiquid(float liquid, LiquidType type)
    {
        switch (type)
        {
            case LiquidType.Milk:
                milk = Mathf.Min(nonWaterLiquidCapacityInSec, milk + liquid);
                break;
            case LiquidType.OatMilk:
                oatMilk = Mathf.Min(nonWaterLiquidCapacityInSec, oatMilk + liquid);
                break;
            case LiquidType.Honey:
                honey = Mathf.Min(nonWaterLiquidCapacityInSec, honey + liquid);
                break;
            default:
                // not possible
                Debug.Assert(false);
                break;
        }
    }

    public bool IsFullWater()
    {
        return water >= waterCapacityInSec - 0.005;
    }

    public bool IsFullLiquid(LiquidType type)
    {
        switch (type)
        {
            case LiquidType.Milk:
                return milk >= nonWaterLiquidCapacityInSec - 0.005;
            case LiquidType.OatMilk:
                return oatMilk >= nonWaterLiquidCapacityInSec - 0.005;
            case LiquidType.Honey:
                return honey >= nonWaterLiquidCapacityInSec - 0.005;
            default:
                // not possible
                Debug.Assert(false);
                return false;
        }
    }
}
