using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Drag KyleRobot here in the Inspector

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. Get Mouse Input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. Handle Vertical Rotation (Looking Up and Down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents the "backflip"

        // Apply vertical rotation to the CAMERA only
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 3. Handle Horizontal Rotation (Looking Left and Right)
        // Apply horizontal rotation to the WHOLE PLAYER BODY
        playerBody.Rotate(Vector3.up * mouseX);
    }
}