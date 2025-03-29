using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables & References

    private CharacterController controller;
    public float speed = 5f; 
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    Vector3 velocity;
    
    bool isGrounded;
    bool isMoving;
    
    private Vector3 lastPosition = new Vector3(0, 0, 0);
    
    #endregion

    #region Unity Functions

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset the vertical velocity when grounded
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z; // Calculate the movement direction
        
        // Move the character 
        controller.Move(move * speed * Time.deltaTime); 

        // Check if the player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 
        }
        
        // falling down
        velocity.y += gravity * Time.deltaTime; 
        
        controller.Move(velocity * Time.deltaTime); // Apply the vertical velocity to the character controller

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
            
        }
        
        lastPosition = gameObject.transform.position;
    }

    #endregion
    
}
