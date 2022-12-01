using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaucetLeverController : MonoBehaviour
{
    private Animator animator;
    private AudioSource pourSound;

    public KettleController kettle;

    void Start()
    {
        animator = GetComponent<Animator>();
        pourSound = GetComponent<AudioSource>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("FaucetLeverDown") &&
                kettle.IsOnKettleBase)
        {
            kettle.FillWater(Time.deltaTime);
            if (!kettle.isFull() && !pourSound.isPlaying)
            {
                pourSound.Play();
            } 
        }
        else if (pourSound.isPlaying)
        {
            pourSound.Stop();
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
