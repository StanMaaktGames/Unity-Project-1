using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    public float speed;
    
    float distance;

    Animator animator;
    EnemyStateController EnemyStateController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        EnemyStateController = GetComponent<EnemyStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
    }

    public void Fighting()
    {
        agent.speed = speed;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) // is attacking
        {
            agent.isStopped = true;
            transform.rotation = RotateTowardsPlayer();
        }
        else if (distance > 2)
        {
            agent.isStopped = false;
            agent.SetDestination(EnemyStateController.lastSeenPlayerPosition);
        }
        else // start attack
        {
            agent.isStopped = true;
            animator.SetTrigger("attack1");
            agent.SetDestination(transform.position);
        }
    }

    Quaternion RotateTowardsPlayer()
    {
        Vector3 direction = EnemyStateController.lastSeenPlayerPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        return Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
    }
}
