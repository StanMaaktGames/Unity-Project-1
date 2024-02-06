using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyMovement : MonoBehaviour
{
    Animator animator;
    
    public UnityEngine.AI.NavMeshAgent agent;

    public float speed, requiredInterest, walkRadius;
    public Transform[] path;

    float interest, moveVelocityFloat, idleWalkDelay;
    int pathPointIndex = 0;
    Vector3 lastSeenPlayerPosition, moveVelocity, lastPosition;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent.SetDestination(transform.position);
    }

    void Update()
    {
        animator.SetInteger("movingState", AnimateMovingState());
        lastPosition = transform.position;
    }

    public void Idle()
    {
        agent.isStopped = false;
        agent.speed = speed;

        if (agent.remainingDistance < 1.0f)
        {
            if (pathPointIndex > path.Length - 2)
            {
                pathPointIndex = 0;
            }
            else
            {
                pathPointIndex += 1;
            }
            agent.SetDestination(path[pathPointIndex].position);
        }

        // idleWalkDelay -= Time.deltaTime;
        // if(idleWalkDelay <= 0)
        // {
        //     idleWalkDelay = 5;
        //     agent.SetDestination(FindRandomPosition());
        // }
    }

    public void Searching(Vector3 lastSeenPlayerPosition)
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

    Vector3 FindRandomPosition() // find random accessible position within walkRadius
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Debug.Log(transform.position - hit.position);
        return(hit.position);
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
