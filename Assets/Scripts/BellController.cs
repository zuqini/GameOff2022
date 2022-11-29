using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D cup;
    private CupController cupController;
    private Vector3 startingPos;
    private float timeElapsed = Mathf.Infinity;

    public CupZoneController serveZone;
    public Transform serveTarget;
    public Collider2D leftCollider;
    public CameraController cameraController;
    public float serveDuration = 2;
    public float cameraDelayDuration = .25f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (cup == null)
        {
            return;
        }

        if (timeElapsed < serveDuration)
        {
            var targetPos = Vector3.Lerp(startingPos, serveTarget.position, Mathfx.Hermite(0, 1, timeElapsed / serveDuration));
            cup.transform.position = targetPos;
            timeElapsed += Time.deltaTime;
        } else {
            cup.transform.position = serveTarget.position;
            leftCollider.enabled = true;
        }
    }

    void OnMouseDown()
    {
        animator.SetTrigger("BellPress");

        if (serveZone.TargetCup == null)
        {
            return;
        }
        cup = serveZone.TargetCup.GetComponent<Rigidbody2D>();
        startingPos = cup.transform.position;
        timeElapsed = 0;
        leftCollider.enabled = false;
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        yield return new WaitForSeconds(cameraDelayDuration);
        cameraController.FollowServe();
    }
}
