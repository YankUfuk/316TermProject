using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    
    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleMovementInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }
    
    private void HandleMovementInput()
    {
        // Handles Camera Speed
        if (Input.GetKey(KeyCode.L))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }
        
        //Handles Camera Movement
        if (Input.GetKey(KeyCode.U))
        {
            newPosition += (transform.forward * movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.J))
        {
            newPosition += (transform.forward * -movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.K))
        {
            newPosition += (transform.right * movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.H))
        {
            newPosition += (transform.right * -movementSpeed * Time.deltaTime);
        }
        
        // Handles Camera Rotation
        if (Input.GetKey(KeyCode.Y))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount * Time.deltaTime);
        }
        
        // Handles Camera Zoom
        if (Input.GetKey(KeyCode.O))
        {
            newZoom += zoomAmount * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.P))
        {
            newZoom -= zoomAmount * Time.deltaTime;
        }
        
        transform.position = Vector3.Lerp(transform.position, newPosition, movementTime * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTime * Time.deltaTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, movementTime * Time.deltaTime);
    }
}
