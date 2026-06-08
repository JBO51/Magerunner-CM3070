using UnityEngine;
using UnityEngine.InputSystem;
//This script is a rigidbody based movement mechanism, to support realistic physics simulation
//Non-kinematic system, unlike Neon White

public class PlayerMovement : MonoBehaviour
{
    //Basic movement adjustments like movement speed and jump force
    [Header("Movement Adjustments")]
    public float movementSpeed = 8f;
    public float jumpForce = 6f;
    public float fallMultiplier = 2.5f;

    //Acceleration and deceleration so movement feels normal
    [Tooltip("Player acceleration")]
    public float acceleration = 40f;
    [Tooltip("Player deceleration")]
    public float deceleration = 50f;

    //Ground verification needed for interacting with the floor
    [Header("Ground Verification")]
    //groundCheck will be an empty gameobject on the players feet to act as center point
    public Transform groundCheck;
    //Radius of the invisible sphere we place around the groundCheck point
    public float groundDistance = 0.4f;
    //LayerMask to interact only with Ground Layer objects
    public LayerMask groundMask;

    //Attach camera to player since its not a child
    [Header("Camera Reference")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isJumping = false;
    private Vector2 moveInput;

    void Start()
    {
        //Hook to rigidbody of player
        rb = GetComponent<Rigidbody>();
    }

    //check for space button presses in Update because Update runs every single frame
    //cant do physics calculations here, do it in fixedUpdate
    void Update()
    {
        //1. GROUND VALIDATION
        //Create invisible sphere on feet, and if it intersects with any collider on
        //the groundMask player, it returns true
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //2. JUMP DETECTION
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            //Apply jump force to the y axis without changing x and z movement
            isJumping = true;
        }

        //3. WASD MOVEMENT
        //variables for x and z movement
        float x = 0f;
        float z = 0f;

        //if statements to see wasd input
        if (Keyboard.current.wKey.isPressed) z = 1f;
        if (Keyboard.current.sKey.isPressed) z = -1f;
        if (Keyboard.current.dKey.isPressed) x = 1f;
        if (Keyboard.current.aKey.isPressed) x = -1f;
        //Store input into Vector2 for fixedUpdate
        moveInput = new Vector2(x, z);
    }

    //process constant force applications, velocity changes, or physics calculations
    //in FixedUpdate because it runs multiple times on fixed timer (e.g. 50 per second)
    void FixedUpdate()
    {
        //Apply Jump Force if is jumping is true from update()
        
        if (isJumping) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isJumping = false;
        }

        //Add more falling velocity
        //If player's Y velocity is less than 0, they passed the peak of their jump and are falling.
        if (rb.linearVelocity.y < 0)
        {
            //Apply downward force to fall to the ground faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        //CALCULATE DIRECTION 
        //Grab camera directional vector and flatten y axis to 0
        //Looking up or down wont slow movement
        //Also normalise so diagonal movement isnt faster
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //Calculate vector direction and the velocity from values
        Vector3 moveDirection = (right * moveInput.x + forward * moveInput.y).normalized;
        Vector3 targetVelocity = moveDirection * movementSpeed;

        //MOMENTUM ACCELEARATION
        //Separate horizontal velocity so gravity isnt changed
        Vector3 currentHorizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //check if actively accelerating or letting go of the keys to stop
        float speedChangeRate;
        if (moveDirection.magnitude > 0f)
        {
            speedChangeRate = acceleration;
        }
        else
        {
            speedChangeRate = deceleration;
        }

        //step current velocity toward the target velocity over time to make movement smoother 
        Vector3 smoothedHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            speedChangeRate * Time.fixedDeltaTime
        );

        //Overwrite X and Z velocity, Leave y velocity alone for gravity
        rb.linearVelocity = new Vector3(smoothedHorizontalVelocity.x, rb.linearVelocity.y, smoothedHorizontalVelocity.z);
    }
}