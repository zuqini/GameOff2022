using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleLeverController : MonoBehaviour
{
    private Animator animator;

    public KettleController kettle;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("KettleLeverDown"))
        {
            if (kettle.IsLatchedToBase && !kettle.isMaxTemperature())
            {
                kettle.RaiseTemperature(Time.deltaTime);
            } else {
                animator.SetBool("IsPressed", false);
            }
        }
    }

    void OnMouseDown()
    {
        animator.SetBool("IsPressed", true);
    }
}
