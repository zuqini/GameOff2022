using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        animator.SetTrigger("BellPress");
    }
}
