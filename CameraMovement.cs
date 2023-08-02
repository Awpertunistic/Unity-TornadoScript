using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 100.0f;

    private bool isRotating = false;
    private Vector3 lastMousePosition;

    private void Update()
    {
        // Handle camera movement
        HandleMovement();

        // Handle camera rotation when right mouse button is held down
        HandleRotation();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button pressed
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1)) // Right mouse button released
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - lastMousePosition;

            float rotationX = mouseDelta.y * rotationSpeed * Time.deltaTime;
            float rotationY = -mouseDelta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, rotationY, Space.World);
            transform.Rotate(Vector3.right, rotationX, Space.Self);

            lastMousePosition = currentMousePosition;
        }
    }
}