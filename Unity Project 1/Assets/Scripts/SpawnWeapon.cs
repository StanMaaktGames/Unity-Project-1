using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeapon : MonoBehaviour
{
    public GameObject weaponPrefab;

    GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon = Instantiate(weaponPrefab, transform.position, transform.rotation, transform);
    }
}