using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject sword;

    public Collider weaponCollider;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(sword, transform.position, Quaternion.identity, transform);
        weaponCollider = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weaponCollider.enabled = true;
        }
        else
        {
            weaponCollider.enabled = false;
        }
    }
}