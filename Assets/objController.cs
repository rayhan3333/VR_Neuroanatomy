using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float mouseSensitivity = 0.1f; // Sensitivity for mouse movement
    public float rotationSpeed = 100f; // Speed of rotation

    private void Update()
    {
        // Keyboard movement (WASD or arrow keys)
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow
        float moveUpward = 0f;

        // Optional: Use Q/E to move up and down
        if (Input.GetKey(KeyCode.Q)) moveUpward = -1; // Move down
        if (Input.GetKey(KeyCode.E)) moveUpward = 1; // Move up

        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveHorizontal, moveUpward, moveVertical) * moveSpeed * Time.deltaTime;

        // Move the object
        transform.Translate(movement, Space.World);

        // Mouse movement for translation
        if (Input.GetMouseButton(0)) // Left mouse button to move with the mouse
        {
            // Get the mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Adjust the position based on mouse movement
            Vector3 mouseMovement = new Vector3(mouseX, 0, mouseY) * mouseSensitivity;
            transform.Translate(mouseMovement, Space.World);
        }

        // Mouse movement for rotation
        if (Input.GetMouseButton(1)) // Right mouse button to rotate with the mouse
        {
            // Get the mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotate the object based on mouse movement
            transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, -mouseY * rotationSpeed * Time.deltaTime, Space.World); // Inverted Y for natural rotation
        }
    }
}
