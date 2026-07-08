//Script for the guillotine movement
//Works by cycling state of Pause, SLice and Retract
//Attached to Level Design -> Platform3 in Level 2
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GuillotineMovement : MonoBehaviour
{
    [Header("Settings")]
    public float sliceSpeed = 30f;    
    public float retractSpeed = 5f;  
    public float pauseDuration = 1.5f;
    public float moveRange = 4f;

    private Rigidbody rb;
    private Vector3 startPosition;
    //State 0 = Pausing, 1 = Slicing, 2 = Retracting
    private int state = 0; 
    private float stateTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        //top and bottom of the movement
        float topY = startPosition.y;
        float bottomY = startPosition.y - moveRange;

        if (rb.isKinematic == false)
        {
            switch (state)
            {
                //Pausing
                case 0:
                    //Set velocity to 0
                    rb.linearVelocity = Vector3.zero;
                    stateTimer += Time.fixedDeltaTime;
                    //Once the duration of the pause is over, move state
                    if (stateTimer >= pauseDuration)
                    {
                        state = 1;
                        stateTimer = 0f;
                    }
                    break;
                //Slicing
                case 1:
                    //Set downwards velocity
                    rb.linearVelocity = new Vector3(0f, -sliceSpeed, 0f);
                    //Once it caps out at the bottom, move to retraction
                    if (transform.position.y <= bottomY)
                    {
                        state = 2;
                    }
                    break;
                //Retracting
                case 2:
                    //Set velocity to go up in retract speed
                    rb.linearVelocity = new Vector3(0f, retractSpeed, 0f);
                    //If it reaches the top, stop, store the pos then begin the process again
                    if (transform.position.y >= topY)
                    {
                        transform.position = new Vector3(transform.position.x, topY, transform.position.z);
                        state = 0;
                    }
                    break;
            }
        }
    }
}