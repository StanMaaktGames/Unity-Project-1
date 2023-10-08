using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBreakOnHit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (other.CompareTag("PlayerWeapon"))
        {
            Destroy(gameObject);
        }
    }
}
