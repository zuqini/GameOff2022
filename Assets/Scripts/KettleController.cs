using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleController : MonoBehaviour
{
    public Transform cup;
    public Transform water;
    public Transform waterMarkers;
    public float cupProximity = 8;
    public int pourAngle = 60;
    public float lerpDuration = 1000;

    private Rigidbody2D rb;
    private Draggable draggable;
    private Transform waterTop;
    private Transform waterBot;

    private float startAngle;
    private float targetAngle;
    private float timeElapsed = Mathf.Infinity;
    private bool isPouring = false;
    private float waterLevel = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        draggable = GetComponent<Draggable>();
        // maybe refactor this
        waterTop = waterMarkers.Find("WaterTop");
        waterBot = waterMarkers.Find("WaterBot");
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
                waterTop.position.y - (100 - waterLevel) * (waterTop.position.y - waterBot.position.y),
                transform.position.z);
    }

    void FixedUpdate()
    {
        var shouldPour = Mathf.Abs(cup.position.x - transform.position.x) <= cupProximity && draggable.IsDragging;
        if (shouldPour && !isPouring)
        {
            isPouring = true;
            setTargetRotation(pourAngle);
        }
        else if (!shouldPour && isPouring)
        {
            isPouring = false;
            setTargetRotation(0);
        }

        if (isPouring)
        {
            waterLevel = Mathf.Max(0f, waterLevel - Time.deltaTime);
        }
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
