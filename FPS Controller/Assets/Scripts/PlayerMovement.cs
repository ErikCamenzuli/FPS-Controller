using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 15f;
    Vector3 velocity;
    public float gravity = -9.81f;

    public float velocityDrop;
    public Transform groundCheck;
    public float distanceToGround = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;
    public float jumpHeight;

    // Update is called once per frame
    void Update()
    {
        //creating a sphere and checking if it collides with anything on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, distanceToGround, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = velocityDrop;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;

        controller.Move(moveDirection * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * velocityDrop * gravity);
        }

        //y= 0.5(gravity * time^2)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
