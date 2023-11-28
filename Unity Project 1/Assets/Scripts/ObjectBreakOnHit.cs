using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBreakOnHit : MonoBehaviour
{
    public float health = 30f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            health -= 10f;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
