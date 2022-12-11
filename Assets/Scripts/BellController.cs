using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BellController : MonoBehaviour
{
    private Animator animator;
    private AudioSource bellSound;
    private Rigidbody2D cup;
    private CupController cupController;
    private Vector3 startingPos;
    private float timeElapsed = Mathf.Infinity;
    private bool isServing = false;

    public CupZoneController serveZone;
    public Transform serveTarget;
    public CameraController cameraController;
    public Collider2D leftBoundary;
    public CustomerController customer;
    public float cupJumpAnimationLength = 2.20f;
    public float serveDuration = 2;
    public float cameraDelayDuration = .25f;

    void Start()
    {
        animator = GetComponent<Animator>();
        bellSound = GetComponent<AudioSource>();
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
            StartCoroutine(CustomerReviewSequence());
        }
    }

    private IEnumerator CustomerReviewSequence() {
        yield return new WaitForSeconds(cupJumpAnimationLength);
        cupController.teabagZone.SetContentsInactive();

        // hack waitforseconds to reset animation
        yield return new WaitForSeconds(0.2f);
        cupController.Anim.enabled = false;
        cup.transform.parent.gameObject.SetActive(false);

        // compare
        // var cupOrder = cupController.GetOrder();
        // var customerOrder = customer.Order;

        // Debug.Log("cupOrder");
        // PrintOrder(cupOrder);
        // Debug.Log("customerOrder");
        // PrintOrder(customerOrder);
        // Debug.Log(cupOrder.Equals(customerOrder));

        //timing with drumroll
        yield return new WaitForSeconds(0.8f);
        GameController.SharedInstance.dialogueManager.ShouldEnd = true;
        GameController.SharedInstance.dialogueManager.StartDialogue(new List<Dialogue> { customer.GenerateOrderComparisonText(cupController.GetOrder()) });
        GameController.SharedInstance.AdvanceLevel();
    }

    private void PrintOrder(Order order)
    {
        foreach (var field in typeof(Order).GetFields(BindingFlags.Instance |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Public))
        {
             Debug.Log(string.Format("{0} = {1}", field.Name, field.GetValue(order)));
        }
    }

    void OnMouseDown()
    {
        animator.SetTrigger("BellPress");
        bellSound.Play();
        if (!customer.HasOrdered || serveZone.TargetCup == null)
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
        customer.PlayDrumRoll();
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        yield return new WaitForSeconds(cameraDelayDuration);
        cameraController.FollowServe();
        GameController.SharedInstance.dialogueManager.EndDialogue();
    }
}
