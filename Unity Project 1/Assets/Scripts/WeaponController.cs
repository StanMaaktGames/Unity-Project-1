using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Collider weaponCollider;

    GameObject weapon;

    public float damage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        weaponCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().EnemyHit(damage);
        }
    }
}
