using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private float timeElapsed = Mathf.Infinity;
    private float colorTimeElapsed = Mathf.Infinity;
    private bool positionInitialized = false;

    public float lerpDuration = 1f;
    public float colorLerpDuration = 1f;
    public Transform startPosition;
    public Transform targetPosition;

    public void Init()
    {
        sprite.color = Color.black;
        timeElapsed = 0;
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Init();
    }

    void FixedUpdate()
    {
        if (timeElapsed < lerpDuration)
        {
            var dt = Mathfx.Hermite(0, 1, timeElapsed / lerpDuration);
            var targetPos = Vector3.Lerp(startPosition.position, targetPosition.position, dt);
            transform.position = targetPos;

            timeElapsed += Time.deltaTime;
        } else if (!positionInitialized) {
            transform.position = targetPosition.position;
            colorTimeElapsed = 0;
            positionInitialized = true;
        }

        if (colorTimeElapsed < colorLerpDuration)
        {
            var dt = Mathfx.Hermite(0, 1, colorTimeElapsed / colorLerpDuration);
            sprite.color = Color.Lerp(Color.black, Color.white, dt);

            colorTimeElapsed += Time.deltaTime;
        } else if (positionInitialized) {
            sprite.color = Color.white;
        }
    }
}
