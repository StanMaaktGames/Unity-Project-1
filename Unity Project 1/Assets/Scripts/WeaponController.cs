using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Collider weaponCollider;

    public int opponentLayer;

    public float damage = 10f;

    void Start()
    {
        weaponCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == opponentLayer)
        {
            if (opponentLayer == 9)
            {
                other.GetComponent<PlayerController>().PlayerHit(damage);
            }
            else if (opponentLayer == 8)
            {
                other.GetComponent<EnemyHealth>().EnemyHit(damage);
            }
        }
        // if (other.CompareTag("Enemy"))
        // {
        //     other.GetComponent<EnemyController>().EnemyHit(damage);
        // }
    }

    void AttackStart()
    {
        weaponCollider.enabled = true;
    }

    void AttackEnd()
    {
        weaponCollider.enabled = false;
    }
}
