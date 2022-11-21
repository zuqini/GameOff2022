using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleController : MonoBehaviour
{
    public Transform cup;
    public float cupProximity = 8;
    public int pourAngle = 60;
    public float lerpDuration = 1000;

    private Rigidbody2D rb;
    private ItemController itemController;
    private float startAngle;
    private float targetAngle;
    private float timeElapsed = Mathf.Infinity;
    private bool isRotated = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        itemController = GetComponent<ItemController>();
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
    }

    void FixedUpdate()
    {
        var shouldRotate = Mathf.Abs(cup.position.x - transform.position.x) <= cupProximity && itemController.IsDragging;
        if (shouldRotate && !isRotated)
        {
            isRotated = true;
            setTargetRotation(pourAngle);
        }
        else if (!shouldRotate && isRotated)
        {
            isRotated = false;
            setTargetRotation(0);
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
