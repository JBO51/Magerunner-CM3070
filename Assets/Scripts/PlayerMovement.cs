using UnityEngine;
using UnityEngine.InputSystem; //Modern input hardware
//This script is a rigidbody based movement mechanism, to support realistic physics simulation
//Non-kinematic system, unlike Neon White

//Forces rigidbody component to gameobject to avoid crashing
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //Basic movement adjustments like movement speed and jump force
    [Header("Movement Adjustments")]
    public float movementSpeed = 8f;
    public float jumpForce = 6f;

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

    void Start()
    {
        //Get rigidibody so unity doesnt have to look every frame
        rb = GetComponent<Rigidbody>();
    }

    //check for space button presses in Update because Update runs every single frame
    void Update()
    {
        //1. GROUND VALIDATION
        //Create invisible sphere on feet, and if it intersects with any collider on
        //the groundMask player, it returns true
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //2. JUMP DETECTION
        //Poll for button presses, checking all conditions are met (grounded, pressed space)
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            //Apply jump force to the y axis without changing x and z movement
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    //process constant force applications, velocity changes, or physics calculations
    //in FixedUpdate because it runs multiple times on fixed timer (e.g. 50 per second)
    void FixedUpdate()
    {

        //3. MOVEMENT INPUT
        //Continuous holding of keys is safe to poll in FixedUpdate over Update
        //variables for x and z movement
        float x = 0f;
        float z = 0f;

        //Basic if statements to see basic input
        if (Keyboard.current.wKey.isPressed) z = 1f;
        if (Keyboard.current.sKey.isPressed) z = -1f;
        if (Keyboard.current.dKey.isPressed) x = 1f;
        if (Keyboard.current.aKey.isPressed) x = -1f;

        //4. CALCULATE DIRECTION 
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
        Vector3 moveDirection = (right * x + forward * z).normalized;
        Vector3 targetVelocity = moveDirection * movementSpeed;

        //5. MOMENTUM ACCELEARATION
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

        //step current velocity toward the target velocity over time for smoother movement
        Vector3 smoothedHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            speedChangeRate * Time.fixedDeltaTime
        );

        //Overwrite X and Z velocity, Leave y velocity alone for gravity
        rb.linearVelocity = new Vector3(smoothedHorizontalVelocity.x, rb.linearVelocity.y, smoothedHorizontalVelocity.z);
    }
}