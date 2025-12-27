using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    //Camera value
    private Camera mainCamera;
    private Vector3 initialPos_Camera;
    private Quaternion initialRot_Camera;
    //child 有时候不会跟着旋转
    private Vector3 initialLocalPos_Child; 
    private Quaternion initialLocalRot_Child; 
    void Start()
    {
        //initialize mainCamera
        mainCamera = FindAnyObjectByType<Camera>();
        initialPos_Camera=mainCamera.transform.position;
        initialRot_Camera=mainCamera.transform.rotation;

        //initialize child （默认一个）
        Transform child = mainCamera.transform.GetChild(0);
        initialLocalPos_Child = child.localPosition;
        initialLocalRot_Child = child.localRotation;
    }

    public void Reset_Camera()
    {
        mainCamera.transform.position = initialPos_Camera;
        mainCamera.transform.rotation = initialRot_Camera;

        Transform child = mainCamera.transform.GetChild(0);
        child.localPosition = initialLocalPos_Child;
        child.localRotation = initialLocalRot_Child;
        Debug.Log("Position has been resetted");
    }
}
