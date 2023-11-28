using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    public Slider healthSlider;

    Animator animator;

    public float sightDistance = 25f;
    public float requiredInterest = 5f;
    public float speed, memory, maxhealth, iFramesOnHit;

    float distance;
    float interest;
    float moveVelocityFloat;
    float health;
    float iFrames = 0f;
    RaycastHit hit;
    Vector3 moveVelocity;
    Vector3 lastPosition;
    Vector3 lastSeenPlayerPosition;

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

        healthSlider.value = health;
    }

    void Idle()
    {
        agent.speed = speed;
    }

    void Searching()
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

    void Fighting()
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
            agent.SetDestination(lastSeenPlayerPosition);
        }
        else // start attack
        {
            agent.isStopped = true;
            animator.SetTrigger("attack1");
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
            Destroy(gameObject);
        }
    }

    Quaternion RotateTowardsPlayer()
    {
        Vector3 direction = lastSeenPlayerPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        return Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
    }
}
