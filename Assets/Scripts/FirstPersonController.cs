using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxSpeed = 4.0f;
    public float acc = 2.0f;
    public float dec = 0.4f;
    public float mouseSensitivity = 150.0f;

    [Header("Camera")]
    public Transform playerCamera;
    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKey("w"))
        {
      
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
  
            transform.position -= transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }


        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0.0f, 0.0f);
        }

        transform.Rotate(0, mouseX, 0);
    }
}
    