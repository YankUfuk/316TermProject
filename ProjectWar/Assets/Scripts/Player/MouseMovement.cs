using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    
    private float xRotation = 0f;
    private float yRotation = 0f;
    
    [SerializeField] private float topClamp = -90f;
    [SerializeField] private float bottomClamp = 90f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotation around the x axis 
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp); // Clamp the vertical rotation to prevent flipping

        // rotation around the y axis
        yRotation += mouseX;

        // Apply the rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        
    }
}
