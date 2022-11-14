using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform brewTarget;
    public Transform serveTarget;

    private float horizontal = 0;
    private CinemachineVirtualCamera vcam;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void FixedUpdate()
    {
        if (horizontal > 0)
        {
            vcam.Follow = brewTarget;
        } else if (horizontal < 0)
        {
            vcam.Follow = serveTarget;
        }
    }
}
