using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrouch;
    private bool sprinting;
    private bool crouching;
    public float playerSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float crouchTimer;
    public float normalSpeed=10f;
    public float sprintSpeed = 22f;
    public float speed;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    public void IncreaseSprintSpeed(float power){
                 sprintSpeed+=power;
    }
    public void IncreaseNormalSpeed(float power){
        speed += power;
    }
    // Update is called once per frame
    void Update()
    {
        
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }
  
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
            speed = sprintSpeed;
        else
            speed = normalSpeed;
    }

    public void ProcessMovement(Vector2 input)
{
    Vector3 moveDirection = Vector3.zero;
    moveDirection.x = input.x;
    moveDirection.z = input.y;  // so with s and w move forward and backward
    controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime); // Use speed here, not playerSpeed
    playerVelocity.y += gravity * Time.deltaTime;
    if (isGrounded && playerVelocity.y < 0)
        playerVelocity.y = -2f;

    controller.Move(playerVelocity * Time.deltaTime);
}

    public void Jump()
    {
        if (isGrounded)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

}
