using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCursorController : MonoBehaviour
{
    public CameraController cameraController;

    void OnMouseDown()
    {
        cameraController.FollowBrew();
    }
}
