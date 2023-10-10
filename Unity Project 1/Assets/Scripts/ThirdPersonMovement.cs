using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Slider staminaSlider;
    public GameObject staminaSliderRect;
    public Collider weaponCollider;

    RectTransform staminaSliderRectTransform;
    Animator animator;

    public float speed = 6f;
    public float runSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float maxStamina = 100f;
    public float staminaRechargeCooldown = 0.5f;
    public float staminaRecoverySpeed = 10f;

    public float turnSmoothTime = 0.1f;
    float turnsmoothVelocity;
    public float groundDistance = 0.4f;

    Vector3 velocity;
    public bool isGrounded;
    float stamina = 0f;
    float staminaRecharge = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponentInChildren<Animator>();
        weaponCollider = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        staminaSlider.value = stamina;
        staminaRecharge -= Time.deltaTime;
        if (stamina < maxStamina && staminaRecharge < 0)
        {
            stamina += Time.deltaTime * staminaRecoverySpeed;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("falling", !isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f )
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnsmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey("left shift") && stamina > 0)
            {
                controller.Move(moveDir.normalized * runSpeed * Time.deltaTime);
                animator.SetFloat("speed", runSpeed);
                stamina -= Time.deltaTime * 5;
                staminaRecharge = staminaRechargeCooldown;
            }
            else
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
                animator.SetFloat("speed", speed);
            }
        } 
        else
        {
            animator.SetFloat("speed", 0f);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnMouseDown()
    {
        animator.SetTrigger("attack");
    }
}
