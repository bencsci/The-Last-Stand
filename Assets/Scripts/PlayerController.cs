using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // Public variable to control movement speed
    public float speed;

    // Vector representing movement direction
    private Vector2 move;

    // Vector representing mouse look direction
    private Vector2 mouseLook;

    // Target rotation for smooth rotation
    private Vector3 rotationTarget;

    // Reference to the Animator component
    private Animator animator;

    // Reference to the crosshair UI element
    public static RectTransform crosshair;

    // Smoothness factor for rotation
    private float smoothness = 25.0f;

    // Animation parameters for horizontal movement
    private int moveXAnimation;

    // Animation parameters for vertical movement
    private int moveZAnimation;

    // Direction of movement along the X-axis
    private float moveDirectionX;

    // Direction of movement along the Z-axis
    private float moveDirectionZ;

    // Flag indicating if the player is active
    public static bool active = true;

    // Flag indicating if the player is colliding with something
    private bool colliding = false;

    // Reference to the Rigidbody component
    private Rigidbody rb;

    // Flag indicating if the player is dead
    public static bool isDead;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void onMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    private void Awake()
    {;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); 
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        moveXAnimation = Animator.StringToHash("MoveX");
        moveZAnimation = Animator.StringToHash("MoveZ");
    }
    // Update is called once per frame
    void Update()
    {
        // Move player if movement is allowed
        if (active)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                rotationTarget = ray.GetPoint(rayDistance);
            }


            if (!colliding)
            {
                movePlayerWithAim();
            }

            // Mouse crosshair with mouse + little position shift to match the correst place the player is shooting at
            Vector2 targetPosition;

            float xOffset = Screen.width * 0.01f;
            float yOffset = Screen.height * 0.02f;

            // Check if mouse is to the bottom of the screen
            if (mouseLook.y < Screen.height * 0.375f) 
            {
                targetPosition = new Vector2(mouseLook.x - xOffset, mouseLook.y + yOffset * 2); 

            } // Check if mouse is to the right of the screen
            else if (mouseLook.x > Screen.width * 0.5f && mouseLook.y < Screen.height * 0.45f) 
            {
                targetPosition = new Vector2(mouseLook.x + xOffset, mouseLook.y); 
            }
            else // Default case (mouse is to the left or the top)
            {
                targetPosition = new Vector2(mouseLook.x + xOffset, mouseLook.y + yOffset * 2); 
            }


            crosshair.transform.position = Vector2.Lerp(crosshair.transform.position, targetPosition, Time.deltaTime * smoothness);
        }
        else
        {
            // Play no animation if not moving
            animator.SetFloat(moveXAnimation, 0);
            animator.SetFloat(moveZAnimation, 0);
        }

    }

    public void movePlayerWithAim()
    {
        var lookPos = rotationTarget - transform.position;
        lookPos.y = 0f;
        var rotation = Quaternion.LookRotation(lookPos);

        Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

        // Adjust rotation
        if (aimDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothness);
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);

        // Player Movement
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        float smoothSpeed = 15f;

        // Calculate the dot product of the forward direction and movement direction
        float forwardDot = Vector3.Dot(transform.forward, movement.normalized);
        float rightDot = Vector3.Dot(transform.right, movement.normalized);

        // Determine the animation parameters based on dot products
        moveDirectionX = Mathf.Clamp(rightDot, -1f, 1f);
        moveDirectionZ = Mathf.Clamp(forwardDot, -1f, 1f);

        animator.SetFloat(moveXAnimation, Mathf.Lerp(animator.GetFloat(moveXAnimation), moveDirectionX, Time.deltaTime * smoothSpeed));
        animator.SetFloat(moveZAnimation, Mathf.Lerp(animator.GetFloat(moveZAnimation), moveDirectionZ, Time.deltaTime * smoothSpeed));

    }

    private void FixedUpdate()
    {
        if (colliding && active)
        {
            movePlayerWithAim();
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Ground"))
        {
            colliding = true;
        }
        
    }


    private void OnCollisionExit(Collision collision)
    {
        if (!collision.transform.CompareTag("Ground"))
        {
            colliding = false;
        }
        
    }
    private void LateUpdate()
    {
        if (active && move.magnitude == 0f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }


    }
}