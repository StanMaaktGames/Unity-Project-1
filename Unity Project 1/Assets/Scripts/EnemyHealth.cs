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
    Animator animator;

    void Start()
    {
        health = maxhealth;
        animator = GetComponentInChildren<Animator>();
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
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        Debug.Log("Death");
        animator.SetTrigger("death");
        yield return new WaitForSeconds(0.867f);
        Destroy(gameObject);
    }
}
