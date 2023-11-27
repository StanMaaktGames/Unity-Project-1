using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public GameObject followObject;

    void Update()
    {
        transform.LookAt(followObject.transform.position);
    }
}
