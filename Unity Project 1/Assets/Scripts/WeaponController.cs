using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int opponentLayer;

    public float damage = 10f;

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
                Debug.Log(other);
                other.GetComponentInParent<EnemyHealth>().EnemyHit(damage);
            }
        }
    }
}
