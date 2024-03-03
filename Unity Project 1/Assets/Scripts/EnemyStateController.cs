using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    EnemyMovement movement;
    RaycastHit hit;
    Animator animator;

    public GameObject player;

    float interest, distance;

    public Vector3 lastSeenPlayerPosition;
    public float requiredInterest = 5f;
    public float sightDistance = 25f;
    public float memory;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (interest >= requiredInterest * 1.5f) 
        {
            GetComponent<EnemyAttacking>().Fighting();
        }
        else if (interest > requiredInterest * 0.33f)
        {
            GetComponent<EnemyMovement>().Searching(lastSeenPlayerPosition);
        }
        else 
        {
            GetComponent<EnemyMovement>().Idle();
        }  

        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= sightDistance)
        {
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, sightDistance))
            {
                interest += Time.deltaTime * memory;
                interest += (sightDistance / (distance + 0.0001f) - 1) * Time.deltaTime;
                lastSeenPlayerPosition = player.transform.position;
                if (interest > requiredInterest)
                {
                    interest = requiredInterest * 2;
                }
            }
        }
        if (interest > 0)
        {
            interest -= Time.deltaTime * memory;
        }
    }
}
