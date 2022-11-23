using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Draggable draggable;
    private Transform waterTop;
    private Transform waterBot;

    private float startAngle;
    private float targetAngle;
    private float timeElapsed = Mathf.Infinity;
    private bool isRotating = false;
    private float waterLevel;
    private bool isLatchedToBase = false;
    private float waterTemperature = 0;

    public bool IsLatchedToBase { get => isLatchedToBase; }
    public float WaterTemperature { get => waterTemperature; }

    public Transform baseLatchedPosition;
    public Transform cup;
    public Transform water;
    public Transform waterMarkers;
    public float cupProximity = 8;
    public int pourAngle = 60;
    public float lerpDuration = 1000;
    public float waterDrainTimeInSec = 5;
    public float waterFillRate = 1;
    public float latchSpeed = 10;
    public float temperatureDecrRate = 2;
    public float temperatureIncrRate = 10;
    public float maxTemperature = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        draggable = GetComponent<Draggable>();
        // maybe refactor this
        waterTop = waterMarkers.Find("WaterTop");
        waterBot = waterMarkers.Find("WaterBot");
        waterLevel = waterDrainTimeInSec;
    }

    void Update()
    {
        if (timeElapsed < lerpDuration)
        {
            rb.SetRotation(Mathf.Lerp(startAngle, targetAngle, EaseIn(timeElapsed / lerpDuration)));
            timeElapsed += Time.deltaTime;
        } else {
            rb.SetRotation(targetAngle);
        }
        waterMarkers.transform.position = transform.position;
        water.transform.position = new Vector3(
                transform.position.x,
                waterTop.position.y - (1 - waterLevel / waterDrainTimeInSec) * (waterTop.position.y - waterBot.position.y),
                transform.position.z);
    }

    void FixedUpdate()
    {
        var shouldPour = draggable.IsDragging &&
            transform.position.x - cup.position.x <= cupProximity;
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
            waterLevel = Mathf.Max(0f, waterLevel - Time.deltaTime);
        }

        if (!draggable.IsDragging && isLatchedToBase)
        {
            // Move our position a step closer to the target.
            var step =  latchSpeed * Time.deltaTime; // calculate distance to move
            rb.MovePosition(Vector2.MoveTowards(transform.position, baseLatchedPosition.position, step));
        }

        waterTemperature = Mathf.Max(0, waterTemperature - temperatureDecrRate * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "KettleBase")
        {
            return;
        }

        isLatchedToBase = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "KettleBase")
        {
            return;
        }

        isLatchedToBase = false;
    }

    public void FillWater(float dt)
    {
        waterLevel = Mathf.Min(waterDrainTimeInSec, waterLevel + waterFillRate * dt);
    }

    public void RaiseTemperature(float dt)
    {
        waterTemperature = Mathf.Min(maxTemperature, waterTemperature + temperatureIncrRate * dt);
    }

    public bool isMaxTemperature()
    {
        return waterTemperature > maxTemperature - 1;
    }

    private void setTargetRotation(float target)
    {
        targetAngle = target;
        startAngle = rb.rotation;
        timeElapsed = 0;
    }

    private float EaseIn(float t)
    {
        return t * t * t;
    }
}
