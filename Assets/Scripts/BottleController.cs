using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float startAngle;
    private float targetAngle;
    private float rotationTimeElapsed = Mathf.Infinity;
    private bool isRotating = false;

    public PourZoneController pourZone;
    public Draggable draggable;
    public Transform liquid;
    public Transform lid;
    public int pourAngle = 60;
    public float rotationLerpDuration = 1;
    public LiquidType liquidType;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        liquid.transform.position = transform.position;
        pourZone.transform.position = transform.position;
    }

    void FixedUpdate()
    {
        var shouldPour = draggable.IsDragging && pourZone.TargetCup != null && !pourZone.TargetCup.IsFullLiquid(liquidType);
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
            pourZone.TargetCup.PourLiquid(Time.deltaTime, liquidType);
        }

        if (rotationTimeElapsed < rotationLerpDuration)
        {
            var currentAngle = Mathf.Lerp(startAngle, targetAngle, Mathfx.Hermite(0, 1, rotationTimeElapsed / rotationLerpDuration));
            rb.SetRotation(currentAngle);
            lid.localRotation = Quaternion.AngleAxis(-currentAngle, Vector3.forward);
            rotationTimeElapsed += Time.deltaTime;
        } else {
            rb.SetRotation(targetAngle);
            lid.localRotation = Quaternion.AngleAxis(-targetAngle, Vector3.forward);
        }
    }

    private void setTargetRotation(float target)
    {
        targetAngle = target;
        startAngle = rb.rotation;
        rotationTimeElapsed = 0;
    }
}
