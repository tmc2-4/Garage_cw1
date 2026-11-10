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

    private CharacterController controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey("w")) moveZ += 1f;
        if (Input.GetKey("s")) moveZ -= 1f;
        if (Input.GetKey("a")) moveX -= 1f;
        if (Input.GetKey("d")) moveX += 1f;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (controller != null)
        {
            controller.Move(move * speed * Time.deltaTime);

            Vector3 gravity = new Vector3(0, -9.81f, 0);
            controller.Move(gravity * Time.deltaTime);
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
    