using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    private float xRotation = 0f; // Current vertical rotation

    public float xSensitivity = 30f; // Horizontal sensitivity
    public float ySensitivity = 30f; // Vertical sensitivity

    private void Start()
    {
        // Ensure the camera is assigned
        if (cam == null)
        {
            Debug.LogError("Camera is not assigned in PlayerLook script.");
        }
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x; // Horizontal mouse input
        float mouseY = input.y; // Vertical mouse input

        // Calculate vertical rotation (camera up and down)
        xRotation -= (mouseY*Time.deltaTime)* ySensitivity ;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // Clamp vertical rotation
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate the player (left and right)
        transform.Rotate(Vector3.up * ( mouseX * Time.deltaTime) * xSensitivity );
    }
}
