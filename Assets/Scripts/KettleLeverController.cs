using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleLeverController : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private bool boilingTemp = false;

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
            } else {
                animator.SetBool("IsPressed", false);
                audioSource.volume = 1;
                audioSource.PlayOneShot(clickSound);
                if (kettle.IsMaxTemperature())
                {
                    boilingTemp = true;
                    audioSource.volume = 0.3f;
                    audioSource.PlayOneShot(boilSound);
                }
            }
        }
        if (boilingTemp && !kettle.IsHot() && audioSource.isPlaying)
        {
            boilingTemp = false;
            audioSource.Stop();
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
