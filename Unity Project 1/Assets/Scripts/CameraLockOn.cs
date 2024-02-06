using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraLockOn : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public CinemachineFreeLook playerCamera;
    public CinemachineVirtualCamera lockOnCamera;
    public float distance = 3f;
    public bool lockedOn = true;
    public Image cameraTargetImage;

    float lockCooldown;

    void Update()
    {

        transform.position = player.position - transform.forward * distance + new Vector3(0, 2, 0);
        if (lockedOn)
        {
            transform.LookAt(target);

            if (Input.GetMouseButtonDown(2) && lockCooldown <= 0)
            {
                lockCooldown = 0.3f;
                lockedOn = false;
                lockOnCamera.Priority = 5;
                target = null;
                cameraTargetImage.enabled = false;

                // var orbital = playerCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
                // orbital.m_XAxis.Value = 0f;
            }
        }

        lockCooldown -= Time.deltaTime;
    }

    public void LockCamera(Transform foundTarget)
    {
        if (!lockedOn && Input.GetMouseButtonDown(2) && lockCooldown <= 0)
        {
            lockCooldown = 0.3f;
            target = foundTarget;
            lockedOn = true;
            lockOnCamera.Priority = 15;
            cameraTargetImage.enabled = true;
        }
    }
}