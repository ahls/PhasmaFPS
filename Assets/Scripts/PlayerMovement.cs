using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float gravity = -9.81f;
    public float rotationSpeed = 100.0f;
    public Transform camera;
    public float jumpHeight = 4f;

    public Transform groundCheck;
    public float groundDistance = 0.8f;
    public LayerMask groundMask;
    public CharacterController controller;
    
    float xRotation = 0f;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Character looking
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(0, mouseX, 0);
        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //Character movement
        float forwardMovement = Input.GetAxis("Vertical");
        //float strafeMovement = Input.GetAxis("Horizontal");

        Vector3 move = /*transform.right * strafeMovement + */ transform.forward * forwardMovement;

        controller.Move(move * speed * Time.deltaTime);

        //Gravity Implementation
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }
        
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * gravity * -2);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
