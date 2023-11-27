using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Animator animator;
    
    public UnityEngine.AI.NavMeshAgent agent;

    public float speed, requiredInterest;

    float interest, moveVelocityFloat;
    Vector3 lastSeenPlayerPosition, moveVelocity, lastPosition;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        animator.SetInteger("movingState", AnimateMovingState());
        lastPosition = transform.position;
    }

    public void Idle()
    {
        agent.speed = speed;
    }

    public void Searching()
    {
        agent.isStopped = false;
        agent.speed = speed;
        if (interest > requiredInterest/3*2) 
        {
            agent.SetDestination(lastSeenPlayerPosition);
        }
        else if (interest > requiredInterest/3)
        {
            transform.rotation = RotateTowardsPlayer();
        }
    }

    Quaternion RotateTowardsPlayer()
    {
        Vector3 direction = lastSeenPlayerPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        return Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
    }

    int AnimateMovingState() //determine whether the idle, walking or running animation should be playing
    {
        moveVelocity = ((transform.position - lastPosition)) / Time.deltaTime;
        moveVelocityFloat = new Vector2(Mathf.Abs(moveVelocity.x), Mathf.Abs(moveVelocity.z)).magnitude;
        if (moveVelocityFloat > 0)
        {
            return 1;
        }
        return 0;
    }
}
