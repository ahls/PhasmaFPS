using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public Transform camera;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float forwardMovement = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        //float strafeMovement = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Translate(0, 0, forwardMovement);
        transform.Rotate(0, mouseX, 0);
        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
