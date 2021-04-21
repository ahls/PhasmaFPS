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
    public Transform wallCheck;
    public float wallCheckHeight = 1.5f;
    public float groundDistance = 0.8f;
    public float wallDistance = 1f;
    public LayerMask groundMask;
    public CharacterController controller;
    public Vector2 accumulatedRecoil;
    [SerializeField] private Animator _animator;

    float xRotation = 0f;
    Vector3 velocity;
    bool isGrounded;
    bool nearWall;
    private float wallClimbingModifier;
    private bool canWallJump = true;
    private bool canDoubleJump = true;


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
        moveRecoil();

        //Character movement
        float forwardMovement = Input.GetAxis("Vertical");
        float strafeMovement = Input.GetAxis("Horizontal");
        if(forwardMovement > 0 && Input.GetKey(KeyCode.LeftShift))
        {//Sprint check
            forwardMovement *= 1.5f;
        }

        if(forwardMovement != 0 && strafeMovement != 0)
        {
            forwardMovement *= .707f;
            strafeMovement *= .707f;
        }

        Vector3 move = transform.right * strafeMovement + transform.forward * forwardMovement;
        
        
        controller.Move(move * speed * Time.deltaTime);


        //animationPart
        _animator.SetFloat("forwardMovement", forwardMovement);
        _animator.SetBool("onGround", isGrounded);

        //Gravity Implementation
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        nearWall = Physics.CheckCapsule(wallCheck.position - Vector3.up * wallCheckHeight, wallCheck.position + Vector3.up * wallCheckHeight, wallDistance, groundMask);
        
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
            canWallJump = true;
            canDoubleJump = true;
        }
        
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            jump(jumpHeight);
        }
        else if (canWallJump && velocity.y > 0 && forwardMovement > 0 && !isGrounded && nearWall && Input.GetButtonDown("Jump"))
        {
            jump(jumpHeight);
            canWallJump = false;
        }
        else if (canDoubleJump && !isGrounded && Input.GetButtonDown("Jump"))
        {
            jump(jumpHeight);
            canDoubleJump = false;
        }
        else if(forwardMovement > 0 && !isGrounded && nearWall)
        {
            wallClimbingModifier = 0.5f;           //wall climbing section
        }
        else
        {
            wallClimbingModifier = 1;
        }
        velocity.y += wallClimbingModifier * gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void AddRecoil(float minX,float minY,float maxX,float maxY)
    {
        accumulatedRecoil.x += Random.Range(minX, maxX);
        accumulatedRecoil.y += Random.Range(minY, maxY);

    }
    private void moveRecoil()
    {
        float verticalAmount = accumulatedRecoil.y / 2;
        float horizontalAmount = accumulatedRecoil.x / 2;
        accumulatedRecoil.y -= verticalAmount;
        accumulatedRecoil.x -= horizontalAmount;
        xRotation -= verticalAmount;
        transform.Rotate(0, horizontalAmount, 0);

    }
    public void jump(float height)
    {
        velocity.y = Mathf.Sqrt(height * gravity * -2);
        _animator.SetTrigger("jump");
    }
}
