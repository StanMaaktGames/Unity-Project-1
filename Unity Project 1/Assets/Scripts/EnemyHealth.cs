using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Slider healthSlider;

    public float maxhealth, iFramesOnHit;

    float health;
    float iFrames = 0f;

    void Start()
    {
        health = maxhealth;
    }

    void Update()
    {
        healthSlider.value = health;
        iFrames -= Time.deltaTime;
    }

    public void EnemyHit(float damage)
    {
        if (iFrames <= 0)
        {
            health -= damage;
            iFrames = iFramesOnHit;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
