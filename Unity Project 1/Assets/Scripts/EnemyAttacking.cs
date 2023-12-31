using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    public GameObject WeaponController;

    public float speed;
    
    float distance;

    Animator animator;
    EnemyStateController EnemyStateController;
    AnimationClip attackAnimClip;
    AnimationEvent evt1;
    AnimationEvent evt2;
    AnimationEvent evt3;
    AnimationEvent evt4;
    AnimationEvent evt5;
    AnimationEvent evt6;
    Collider weaponCollider;

    void Start()
    {
        weaponCollider = WeaponController.GetComponentInChildren<Collider>();
        // weaponCollider.enabled = false;
        animator = GetComponentInChildren<Animator>();
        EnemyStateController = GetComponent<EnemyStateController>();
        attackAnimClip = animator.runtimeAnimatorController.animationClips[2];

        evt1 = new AnimationEvent();
        evt1.time = 1.05f;
        evt1.functionName = "AttackStart";
        evt2 = new AnimationEvent();
        evt2.time = 1.23f;
        evt2.functionName = "AttackEnd";
        evt3 = new AnimationEvent();
        evt3.time = 2.34f;
        evt3.functionName = "AttackStart";
        evt4 = new AnimationEvent();
        evt4.time = 2.6f;
        evt4.functionName = "AttackEnd";
        evt5 = new AnimationEvent();
        evt5.time = 5.0f;
        evt5.functionName = "AttackStart";
        evt6 = new AnimationEvent();
        evt6.time = 5.33f;
        evt6.functionName = "AttackEnd";
    }

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

    void AttackStart()
    {
        weaponCollider.enabled = true;
    }

    void AttackEnd()
    {
        weaponCollider.enabled = false;
    }
}
