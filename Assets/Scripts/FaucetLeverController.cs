using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaucetLeverController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("FaucetLeverDown"))
        {
            // fill water
        }
    }

    void OnMouseDown()
    {
        animator.SetBool("IsPressed", true);
    }

    void OnMouseUp()
    {
        animator.SetBool("IsPressed", false);
    }
}
