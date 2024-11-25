using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    //public float sensX;
    //public float sensY;
    public float rotationSpeed;
    public Transform orientation;

    [Header("Keybinds")]
    public KeyCode rotateLeft = KeyCode.Q;
    public KeyCode rotateRight = KeyCode.E;
    public KeyCode lookUp = KeyCode.R; // Key to look up
    public KeyCode lookDown = KeyCode.F; // Key to look down

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //// get mouse input
        //float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        //float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //yRotation += mouseX;
        //xRotation -= mouseY;

        // Get input for left and right rotation. (starting no rotation?)
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.Q)) // Rotate left
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.E)) // Rotate right
        {
            horizontalInput = 1f;
        }

        // Vertical input for looking up and down
        float verticalInput = 0f;

        if (Input.GetKey(lookUp)) // Look up
        {
            verticalInput = -1f;
        }
        else if (Input.GetKey(lookDown)) // Look down
        {
            verticalInput = 1f;
        }

        // Calculate horizontal rotation
        yRotation += horizontalInput * rotationSpeed * Time.deltaTime;
        // Calculate vertical rotation
        xRotation += verticalInput * rotationSpeed * Time.deltaTime;

        //restrict looking up or down more than 90 deg
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        //rotate cam + orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
