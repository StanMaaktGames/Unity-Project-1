using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetectTarget : MonoBehaviour
{
    public LayerMask lockOnTargetMask;
    public GameObject lockOnCamera;

    bool targetFound = false;
    Transform target;

    void Update()
    {
        if (targetFound)
        {
            lockOnCamera.GetComponent<CameraLockOn>().LockCamera(target);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == 10)
        {
            target = other.transform;
            targetFound = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.layer == 10)
        {
            targetFound = false;
        }
    }
}
