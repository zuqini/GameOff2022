using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCursorController : MonoBehaviour
{
    public CameraController cameraController;
    public bool goToRight = true;

    void OnMouseDown()
    {
        if (goToRight)
            cameraController.FollowBrew();
        else
            cameraController.FollowServe();
    }
}
