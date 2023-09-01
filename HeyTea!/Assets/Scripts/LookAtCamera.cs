using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
File Name : LookAtCamera.cs
Function  : Define how the cutting progress bar is displayed
Author    : Yong Wu
Data      : 01.09.2023

*/
public class LookAtCamera : MonoBehaviour
{

    private enum Mode {
        LookAt,
        LokkAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;

    private void LateUpdate() {
        //It can be set to switch as the camera changes, or it can be set to keep facing straight ahead.
        switch (mode) {
            case Mode.LookAt: transform.LookAt(Camera.main.transform);break;
            case Mode.LokkAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
        
    }

}
