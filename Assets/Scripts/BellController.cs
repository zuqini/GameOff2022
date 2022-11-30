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
    private bool isServing = false;

    public CupZoneController serveZone;
    public Transform serveTarget;
    public CameraController cameraController;
    public Collider2D leftBoundary;
    public float cupJumpAnimationLength = 2.20f;
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
        if (cup == null || !isServing)
        {
            return;
        }

        if (timeElapsed < serveDuration)
        {
            var targetPos = Vector3.Lerp(startingPos, serveTarget.position, Mathfx.Hermite(0, 1, timeElapsed / serveDuration));
            cup.MovePosition(targetPos);
            timeElapsed += Time.deltaTime;
        } else {
            isServing = false;
            leftBoundary.enabled = true;
            cup.transform.position = serveTarget.position;
            // Debug.Log(cup.transform.localPosition);
            // x is harcdoded by above position
            // @TODO think of a better way to do this
            cupController.teabagZone.SetStirCollider(false);
            cupController.Anim.enabled = true;
            cupController.Anim.SetTrigger("CupJumpAndBounce");
            StartCoroutine(SetInactiveAfterAnimation());
        }
    }

    private IEnumerator SetInactiveAfterAnimation() {
        yield return new WaitForSeconds(cupJumpAnimationLength);
        cupController.teabagZone.SetContentsInactive();
        // hack
        yield return new WaitForSeconds(1);
        cupController.Anim.enabled = false;
        cup.transform.parent.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        animator.SetTrigger("BellPress");

        if (serveZone.TargetCup == null)
        {
            return;
        }
        isServing = true;
        leftBoundary.enabled = false;
        cupController = serveZone.TargetCup;
        cupController.draggable.IsEnabled = false;
        cup = serveZone.TargetCup.GetComponent<Rigidbody2D>();
        startingPos = cup.transform.position;
        timeElapsed = 0;
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        yield return new WaitForSeconds(cameraDelayDuration);
        cameraController.FollowServe();
    }
}
