using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleLeverController : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip boilSound;
    public AudioClip clickSound;
    public KettleController kettle;

    public bool IsLeverDown()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("KettleLeverDown");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
                if (kettle.IsHot() && !audioSource.isPlaying)
                {
                    audioSource.volume = 0.3f;
                    audioSource.PlayOneShot(boilSound);
                }
            } else {
                animator.SetBool("IsPressed", false);
                audioSource.Stop();
                audioSource.volume = 1;
                audioSource.PlayOneShot(clickSound);
            }
        }
    }

    void OnMouseDown()
    {
        if (!IsLeverDown())
        {
            audioSource.volume = 1;
            audioSource.PlayOneShot(clickSound);
        }
        animator.SetBool("IsPressed", true);
    }
}
