using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This whole thing needs some cleanup
public class KettleController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform waterTop;
    private Transform waterBot;

    private float startAngle;
    private float targetAngle;
    private float rotationTimeElapsed = Mathf.Infinity;
    private bool isRotating = false;
    private float waterLevel;
    private bool isOnKettleBase = true;
    private float waterTemperature = 0;

    private Vector3 startPos;
    private float latchTimeElapsed = Mathf.Infinity;

    public bool IsOnKettleBase { get => isOnKettleBase; }
    public float WaterTemperature { get => waterTemperature; }

    public Draggable draggable;
    public ParticleSystem ps;
    public Transform baseLatchedPosition;
    public Transform water;
    public Transform waterMarkers;
    public KettleLeverController kettleLever;
    public PourZoneController pourZone;
    public int pourAngle = 60;
    public float rotationLerpDuration = 1;
    public float waterCapacityInSec = 2;
    public float waterFillRateInSec = 1;
    public float latchLerpDuration = 1;
    public float temperatureDecrRate = 2;
    public float temperatureIncrRate = 10;
    public float maxTemperature = 100;
    public float unlatchTemperature = 90;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // maybe refactor this
        waterTop = waterMarkers.Find("WaterTop");
        waterBot = waterMarkers.Find("WaterBot");
        waterLevel = waterCapacityInSec;
    }

    void Update()
    {
        waterMarkers.transform.position = transform.position;
        water.transform.position = new Vector3(
                transform.position.x,
                waterTop.position.y - (1 - waterLevel / waterCapacityInSec) * (waterTop.position.y - waterBot.position.y),
                transform.position.z);
        pourZone.transform.position = transform.position;
    }

    void FixedUpdate()
    {
        waterTemperature = Mathf.Max(0, waterTemperature - temperatureDecrRate * Time.deltaTime);

        var shouldPour = draggable.IsDragging && pourZone.TargetCup != null && !pourZone.TargetCup.IsFullWater();
        if (shouldPour && !isRotating)
        {
            isRotating = true;
            setTargetRotation(pourAngle);
        }
        else if (!shouldPour && isRotating)
        {
            isRotating = false;
            setTargetRotation(0);
        }

        var isPouring = isRotating && targetAngle == rb.rotation;
        if (isPouring)
        {
            Debug.Log("Pouring");
            var waterToPour = Mathf.Min(Time.deltaTime, waterLevel);
            // if isPouring is true, TargetCup cannot be null
            var waterPoured = pourZone.TargetCup.PourWater(waterToPour);
            waterLevel = Mathf.Max(0f, waterLevel - waterPoured); // waterlevel should always be >= 0
        }

        if (!draggable.IsDragging)
        {
            draggable.IsEnabled = IsUnlatched();
            if (!isOnKettleBase && latchTimeElapsed >= latchLerpDuration)
            {
                startPos = rb.position;
                latchTimeElapsed = 0;
            }
        }

        if (IsHot() && ps.isStopped)
        {
            ps.Play();
        }
        else if (!IsHot() && ps.isPlaying)
        {
            ps.Stop();
        }

        IterateLerp();
    }

    private void IterateLerp()
    {
        if (rotationTimeElapsed < rotationLerpDuration)
        {
            rb.SetRotation(Mathf.Lerp(startAngle, targetAngle, Mathfx.Hermite(0, 1, rotationTimeElapsed / rotationLerpDuration)));
            // Debug.Log("rprogress: " + rotationTimeElapsed / rotationLerpDuration);
            rotationTimeElapsed += Time.deltaTime;
        } else {
            rb.SetRotation(targetAngle);
        }

        if (latchTimeElapsed < latchLerpDuration)
        {
            var targetPos = Vector3.Lerp(startPos, baseLatchedPosition.position, Mathfx.Hermite(0, 1, latchTimeElapsed / latchLerpDuration));
            // Debug.Log("lprogress: " + latchTimeElapsed / latchLerpDuration);
            rb.MovePosition(targetPos);
            latchTimeElapsed += Time.deltaTime;
        } else if (!draggable.IsEnabled || !draggable.IsDragging) {
            rb.MovePosition(baseLatchedPosition.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "KettleBase")
        {
            return;
        }

        isOnKettleBase = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "KettleBase")
        {
            return;
        }

        isOnKettleBase = false;
    }

    public void FillWater(float dt)
    {
        waterLevel = Mathf.Min(waterCapacityInSec, waterLevel + waterFillRateInSec * dt);
    }

    public void RaiseTemperature(float dt)
    {
        waterTemperature = Mathf.Min(maxTemperature, waterTemperature + temperatureIncrRate * dt);
    }

    public bool IsMaxTemperature()
    {
        return waterTemperature > maxTemperature - 1;
    }

    public bool IsUnlatched()
    {
        return !kettleLever.IsLeverDown() && IsHot();
    }

    public bool IsHot()
    {
        return waterTemperature > unlatchTemperature;
    }

    private void setTargetRotation(float target)
    {
        targetAngle = target;
        startAngle = rb.rotation;
        rotationTimeElapsed = 0;
    }
}
