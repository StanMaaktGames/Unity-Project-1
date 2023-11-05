using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;

    Animator animator;

    public float sightDistance = 25f;
    public float requiredInterest = 5f;
    public float speed, memory, maxhealth, iFramesOnHit;

    float distance;
    public float interest;
    float moveVelocityFloat;
    float health;
    float iFrames = 0f;
    RaycastHit hit;
    Vector3 moveVelocity;
    Vector3 lastPosition;
    Vector3 lastSeenPlayerPosition;
    Vector3 targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (interest >= requiredInterest * 1.5f) 
        {
            Fighting();
        }
        else if (interest > 0)
        {
            Searching();
        }
        else 
        {
            Idle();
        }
        
        if (distance <= sightDistance)
        {
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, 50))
            {
                if (hit.transform == player.transform)
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
        }
        interest -= Time.deltaTime * memory;

        iFrames -= Time.deltaTime;
        animator.SetInteger("movingState", AnimateMovingState());
        lastPosition = transform.position;
    }

    void Idle()
    {
        agent.speed = speed;
    }

    void Searching()
    {
        agent.speed = speed;
        if (interest > requiredInterest/3*2) 
        {
            agent.SetDestination(lastSeenPlayerPosition);
        }
        else if (interest > requiredInterest/3)
        {
            transform.LookAt(player.transform);
        }
    }

    void Fighting()
    {
        agent.speed = speed;
        if (distance > 2)
        {
            agent.SetDestination(lastSeenPlayerPosition);
        }
        else 
        {
            agent.SetDestination(transform.position);
        }
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

    public void EnemyHit(float damage)
    {
        if (iFrames <= 0)
        {
            health -= damage;
            iFrames = iFramesOnHit;
        }
        if (health <= 0)
        {
            Debug.Log("death");
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("PlayerWeapon"))
        {
            iFrames = 0f;
        }
    }
}
