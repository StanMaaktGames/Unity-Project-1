using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public GameObject lockOnCamera;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Slider staminaSlider;
    public Slider healthSlider;
    public GameObject staminaSliderRect;
    public Transform weaponController;
    public GameObject deathScreen;
    public GameObject gameManager;
    
    Collider weaponCollider;
    Animator animator;
    CameraLockOn cameraLockOnScript;

    public float speed = 6f;
    public float runSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float maxStamina = 100f;
    public float maxHealth = 100f;
    public float staminaRechargeCooldown = 0.5f;
    public float staminaRecoverySpeed = 10f;
    public float turnSmoothTime = 0.1f;
    public float groundDistance = 0.4f;
    public float iFramesOnHit;

    float turnsmoothVelocity;
    float stamina;
    float health;
    float staminaRecharge = 0f;
    Vector3 hitNormal;
    Vector3 lastPosition;
    Vector3 moveVelocity;
    float moveVelocityFloat;
    float iFrames;
    float targetAngle;
    Vector3 velocity;
    bool isGrounded;
    bool alive = true;

    void Start()
    {
        stamina = maxStamina;
        health = maxHealth;
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        weaponCollider = weaponController.GetComponentInChildren<Collider>();
        cameraLockOnScript = lockOnCamera.GetComponent<CameraLockOn>();
    }

    void Update()
    {
        staminaSlider.value = stamina;
        healthSlider.value = health;
        staminaRecharge -= Time.deltaTime;
        if (stamina < maxStamina && staminaRecharge < 0)
        {
            stamina += Time.deltaTime * staminaRecoverySpeed;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("falling", !isGrounded);
        RaycastHit hit;
        if (isGrounded)
        {
            if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, 10, groundMask))
            {
                hitNormal = hit.normal;
                if (Mathf.Abs(hitNormal.x) + Mathf.Abs(hitNormal.z) > 1)
                {
                    isGrounded = false;
                }
            }
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (!alive)
        {
            horizontal = 0;
            vertical = 0;
        }
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (!cameraLockOnScript.lockedOn)
        {
            animator.SetInteger("direction", -1); // default
        }
        else if (direction.x < 0)
        {
            animator.SetInteger("direction", 3); // left
        }
        else if (direction.x > 0)
        {
            animator.SetInteger("direction", 1); // right
        }
        else if (direction.z < 0)
        {
            animator.SetInteger("direction", 2); // backwards
        }
        else if (direction.z > 0)
        {
            animator.SetInteger("direction", 0); // forward
        }
        else
        {
            animator.SetInteger("direction", -1); // no direction
        }
        if (direction.magnitude > 0.1f && alive) // move the player
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnsmoothVelocity, turnSmoothTime);
            if (!cameraLockOnScript.lockedOn)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack1")) // don't move while attacking
            {
                if (Input.GetKey("q") && stamina > 0) // sprint
                {
                    animator.SetInteger("movingState", 2);
                    stamina -= Time.deltaTime * 5;
                    staminaRecharge = staminaRechargeCooldown;
                }
                else if (Input.GetKey("q")) // can't sprint because out of stamina
                {
                    stamina -= Time.deltaTime * 5;
                    staminaRecharge = staminaRechargeCooldown;
                    animator.SetInteger("movingState", 1);
                } 
                else // move normally
                {
                    animator.SetInteger("movingState", 1);
                }
            }
        }
        else // not moving
        {
            animator.SetInteger("movingState", 0);
        }

        if (Input.GetButtonDown("Jump") && isGrounded && alive) // jump
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && !animator.GetBool("attack") && alive) // attack
        {
            if (animator.GetBool("endAttack"))
            {
                animator.ResetTrigger("endAttack");
            }
            else
            {
                animator.SetTrigger("endAttack");
            }
            animator.SetTrigger("attack");
            stamina -= 3;
            staminaRecharge = staminaRechargeCooldown;
        }

        if (alive && Input.GetKeyDown(KeyCode.LeftShift) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge Backwards") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge Left") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge Right")) // dodge
        {
            animator.SetTrigger("dodge");
            iFrames = 0.5f;
        }        

        iFrames -= Time.deltaTime;
        lastPosition = transform.position;
    }

    void AttackAnimEnd()
    {
        animator.ResetTrigger("attack");
    }

    void AttackStart()
    {
        weaponCollider.enabled = true;
    }
    void AttackEnd()
    {
        weaponCollider.enabled = false;
    }

    void OnControllerColliderHit (ControllerColliderHit hit) 
    {
        hitNormal = hit.normal;
    }

    public void PlayerHit(float damage)
    {
        if (iFrames <= 0)
        {
            animator.SetTrigger("hit");
            animator.ResetTrigger("attack");
            health -= damage;
            iFrames = iFramesOnHit;
        }
        if (health <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            deathScreen.SetActive(true);
            Debug.Log("death");
            alive = false;
        }
    }
}
