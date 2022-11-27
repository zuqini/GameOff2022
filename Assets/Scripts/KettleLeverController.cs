using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleLeverController : MonoBehaviour
{
    private Animator animator;

    public KettleController kettle;

    public bool IsLeverDown()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("KettleLeverDown");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (IsLeverDown())
        {
            if (kettle.IsOnKettleBase && !kettle.IsMaxTemperature())
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
